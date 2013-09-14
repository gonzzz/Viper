using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Viper.Framework.Entities;
using Viper.Framework.Blocks;
using Viper.Framework.Enums;

namespace Viper.Framework.Engine
{
	/// <summary>
	/// Transaction Scheduler class. Selects the Active Transaction and moves transactions from the FEC 
	/// to the CEC and viceversa. 
	/// </summary>
	public class ViperScheduler
	{
		#region Public Const
		public const int DEFAULT_MAX_TARGET_TIME = Int32.MaxValue;
		#endregion

		#region Private Members
		private int m_iSystemTime;
		private int m_iTransactionCounter;
		private List<Transaction> m_ltCurrentEventsChain;
		private List<Transaction> m_ltFutureEventsChain;
		private List<Block> m_lModelBlocks;
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public int SystemTime
		{
			get 
			{
				return m_iSystemTime;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int TransactionCounter
		{
			get
			{
				return m_iTransactionCounter;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public List<Block> ModelBlocks
		{
			get
			{
				return m_lModelBlocks;
			}
			set
			{
				m_lModelBlocks = value;
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ViperScheduler()
		{
			m_iSystemTime = 0;
			m_iTransactionCounter = 0;
			m_ltCurrentEventsChain = new List<Transaction>();
			m_ltFutureEventsChain = new List<Transaction>();
			m_lModelBlocks = null;
		}
		#endregion

		#region Simulation Methods
		/// <summary>
		/// 
		/// </summary>
		private void GenerateNextTransactions()
		{
			// Get Simulation Model Blocks
			List<Block> theBlocks = ViperSystem.Instance().Blocks;

			// Find all Generate Blocks
			List<Block> generateBlocks = theBlocks.FindAll( b => b is GenerateBlock );

			// For each generate block, create next transactions
			foreach( Block block in generateBlocks )
			{
				GenerateBlock generateBlock = block as GenerateBlock;

				int iTransactionNumber = m_iTransactionCounter + 1;
				Transaction oTransaction = new Transaction( iTransactionNumber );
				if( generateBlock.Process( ref oTransaction ) == BlockProcessResult.TRANSACTION_PROCESSED )
				{
					// Effectively increment by 1
					m_iTransactionCounter++; 

					// Put the Transaction in the FEC
					m_ltFutureEventsChain.Add( oTransaction );
				}
			}
		}

		/// <summary>
		/// Set System Time to Next Transaction Time, move Transactions from FEC to CEC
		/// </summary>
		private void UpdateSystemTime()
		{
			// If there are no transactions in the FEC, we generate them from each Model's Generate Block
			if( m_ltFutureEventsChain.Count == 0 ) GenerateNextTransactions();

			// Get first next transaction in the FEC
			Transaction nextTransaction = m_ltFutureEventsChain.First();

			// Update System Time
			m_iSystemTime = nextTransaction.NextSystemTime;
			
			// Add transaction to the CEC
			m_ltCurrentEventsChain.Add( nextTransaction );
			
			// Remove it from the FEC
			m_ltFutureEventsChain.RemoveAt( 0 );

			// Get all transactions in the FEC with the same NextSystemTime
			List<Transaction> anotherTransactions =	m_ltFutureEventsChain.
													FindAll( t => t.NextSystemTime == m_iSystemTime );
			
			// If there are one or more transaction with the same system time
			if( anotherTransactions.Count > 0 )
			{
				// Add all transactions to the CEC
				m_ltCurrentEventsChain.AddRange( anotherTransactions );

				// Remove them all from the FEC
				m_ltFutureEventsChain.RemoveAll( t => t.NextSystemTime == m_iSystemTime );
			}
		}

		/// <summary>
		/// Set status OFF to all transactions in the CEC that have their ChangeFlag status ON
		/// </summary>
		private void ResetChangeFlags()
		{
			List<Transaction> transactionsWithChangeFlag =	m_ltCurrentEventsChain.
															FindAll( t => t.ChangeFlag );
			foreach( Transaction transaction in transactionsWithChangeFlag )
			{
				transaction.ChangeFlag = false;
			}
		}

		/// <summary>
		/// Scan for Transactions in the CEC, try to move them through the model until there
		/// are no more transactions to move.
		/// </summary>
		private void ScanTransactions()
		{
			// Reset CEC's transactions ChangeFlag status OFF
			ResetChangeFlags();

			// If there are at least one transaction in the CEC
			int iCurrentTransactionsInCEC = m_ltCurrentEventsChain.Count;
			if( iCurrentTransactionsInCEC > 0 )
			{
				// While there are transactions in the CEC
				int iCurrentTransactionIndex = 0;
				while( iCurrentTransactionIndex <= iCurrentTransactionsInCEC )
				{
					// Get transaction from the CEC
					Transaction currentTransaction = m_ltCurrentEventsChain[ iCurrentTransactionIndex ];
					if ( !currentTransaction.ScanStatus )
					{
						// If current Transaction has no ScanStatus flag ON, try to move it through the model
						MoveTransaction( currentTransaction );
					}

					// Will get next transaction in the following iteration
					iCurrentTransactionIndex++;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="currentTransaction"></param>
		private void MoveTransaction( Transaction currentTransaction )
		{
			// Try access next Model Block
			while( !currentTransaction.ChangeFlag )
			{
				
			}
		}

		/// <summary>
		/// Simulation Process
		/// </summary>
		/// <param name="iTargetTime"></param>
		public void Simulate( int iTargetTime = DEFAULT_MAX_TARGET_TIME )
		{
			// Reset System Time
			m_iSystemTime = 0;

			// While Current System Time is lower than Target System Time 
			while( m_iSystemTime <= iTargetTime )
			{
				// Update System Time, move transactions from FEC to CEC
				UpdateSystemTime();

				// Process transaction in the CEC, move them through the model
				ScanTransactions();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		public void InsertTransactionIntoFEC( Transaction oTransaction )
		{
			m_ltFutureEventsChain.Add( oTransaction );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		public void RemoveTransactionFromCEC( Transaction oTransaction )
		{
			m_ltCurrentEventsChain.Remove( oTransaction );
		}
		#endregion
	}
}
