using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Utils;
using Viper.Framework.Blocks;
using Viper.Framework.Entities;
using Viper.Framework.Enums;

namespace Viper.Framework.Engine
{
	public class ViperSystem
	{
		#region Singleton Implementation
		private static ViperSystem m_oSystem = null;

		/// <summary>
		/// Private Constructor
		/// </summary>
		private ViperSystem()
		{
			ErrorMessageLog = new List<string>();
			m_eLanguage = Languages.Spanish;
			m_lbSimulationBlocks = null;
			m_oModel = new Model();
			m_ltCurrentEventsChain = new List<Transaction>();
			m_ltFutureEventsChain = new List<Transaction>();
			m_iTerminationCount = Constants.DEFAULT_ZERO_VALUE;
			m_iSystemTime = Constants.DEFAULT_ZERO_VALUE;
			m_eState = SimulationState.Ready;
		}

		/// <summary>
		/// Get Singleton Instance
		/// </summary>
		/// <returns></returns>
		public static ViperSystem Instance()
		{
			if( m_oSystem == null )
			{
				m_oSystem = new ViperSystem();
			}

			return m_oSystem;
		}
		#endregion

		#region Private Attributes
		private const int DEFAULT_MAX_TARGET_TIME = Int32.MaxValue;
		private Languages m_eLanguage;
		private Model m_oModel;
		private SimulationState m_eState;
		private int m_iSystemTime;
		private int m_iTargetTime;
		private int m_iCurrentTransactionNumber;
		private int m_iTerminationCount;
		private int m_iTransactionCounter;
		private bool m_bChangeFlag;
		private List<Block> m_lbSimulationBlocks;
		private List<Transaction> m_ltCurrentEventsChain;
		private List<Transaction> m_ltFutureEventsChain;
		#endregion

		#region Public Properties
		/// <summary>
		/// 
		/// </summary>
		public Languages SystemLanguage
		{
			get
			{
				return m_eLanguage;
			}
			set
			{
				m_eLanguage = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Model Model
		{
			get
			{
				return m_oModel;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public List<Block> Blocks
		{
			get
			{
				return m_lbSimulationBlocks;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int TerminationCount
		{
			get
			{
				return m_iTerminationCount;
			}
			private set
			{
				m_iTerminationCount = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int SystemTime
		{
			get
			{
				return m_iSystemTime;
			}
			private set
			{
				m_iSystemTime = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int TargetTime
		{
			get
			{
				return m_iTargetTime;
			}
			private set
			{
				m_iTargetTime = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int CurrentTransactionNumber
		{
			get
			{
				return m_iCurrentTransactionNumber;
			}
			private set
			{
				m_iCurrentTransactionNumber = value;
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
			private set
			{
				m_iTransactionCounter = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SimulationState State
		{
			get
			{
				return m_eState;
			}
			private set
			{
				m_eState = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool ChangeFlag
		{
			get
			{
				return m_bChangeFlag;
			}
			set
			{
				m_bChangeFlag = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public List<String> ErrorMessageLog { get; set; }
		#endregion

		#region Public Methods
		public bool Simulate( String sPlainTextModel, int iTerminationCount )
		{
			// Previous: clean error message log (will be filled with error after parsing the model)
			ErrorMessageLog.Clear();

			// 1) Create Viper Model from Plain Text GPSS model	
			m_lbSimulationBlocks = BlockFactory.Instance().CreateModel( sPlainTextModel );
			ErrorMessageLog.AddRange( BlockFactory.Instance().ErrorMessageLog.ToArray() );

			// 2) Check that Viper Model has been created with NO errors
			if( ErrorMessageLog.Count > 0 ) return false;

			// 3) Add Entity Objects to Model
			CreateEntitiesForNonTransactionalBlocks();

			// 4) Set termination count (simulation will end when this number gets to zero or simulation time is up)
			m_iTerminationCount = iTerminationCount;

			// 5) Start Simulation
			Simulate();				

			return true;
		}
		#endregion

		#region Simulation Methods
		/// <summary>
		/// 
		/// </summary>
		public void Reset()
		{
			SystemTime = 0;
			TargetTime = DEFAULT_MAX_TARGET_TIME;
			ChangeFlag = false;
			State = SimulationState.Ready;
			CurrentTransactionNumber = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			SystemTime = 0;
			TargetTime = DEFAULT_MAX_TARGET_TIME;
			ChangeFlag = false;
			State = SimulationState.Ready;
			CurrentTransactionNumber = 0;
		}

		/// <summary>
		/// Creates and initializes Entities for STORAGE, VARIABLE, BVARIABLE, FVARIABLE, INITIAL, FUNCTION, TABLE, QTABLE and MATRIX blocks.
		/// Facilities are initialized the first time a SEIZE or PREEMPT block is entered by a transaction. Queues could be initialized by QTABLE or
		/// the first time a transaction enters a QUEUE block.
		/// </summary>
		private void CreateEntitiesForNonTransactionalBlocks()
		{
			var oNonTransactionalBlocks = ( from b in m_lbSimulationBlocks
										   where b.Executable == false
										   select b ).ToList();

			foreach( Block block in oNonTransactionalBlocks )
			{
				// At September/2013 i only have STORAGE
				if( block is StorageBlock )
				{
					StorageBlock storageBlock = block as StorageBlock;
					
					// Create new Storage in Model with Storage Block parameters
					Storage newStorage = new Storage( storageBlock.Label );
					newStorage.Capacity = Convert.ToInt32( storageBlock.OperandA );
					newStorage.Available = newStorage.Capacity;
					Model.AddStorage( newStorage );

					// Attach Storage Entity to Storage Block
					storageBlock.AttachStorage( Model.GetStorageByName( storageBlock.Label ) );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void GenerateFirstTransactions()
		{
			// Find all Generate Blocks
			List<Block> generateBlocks = this.m_lbSimulationBlocks.FindAll( b => b is GenerateBlock );

			// For each generate block, create next transactions
			foreach( Block block in generateBlocks )
			{
				GenerateBlock generateBlock = block as GenerateBlock;

				ScheduleNewTransactionFromGenerate( generateBlock );
			}
		}

		/// <summary>
		/// Set System Time to Next Transaction Time, move Transactions from FEC to CEC
		/// </summary>
		private void UpdateSystemTime()
		{
			// Order by NextTime ascending
			m_ltFutureEventsChain = m_ltFutureEventsChain.OrderBy( t => t.NextSystemTime ).ToList();

			// Get first next transaction in the FEC
			Transaction nextTransaction = m_ltFutureEventsChain.First();
			// Update Transaction
			nextTransaction.ScanStatus = false;
			nextTransaction.State = TransactionState.ACTIVE;

			// Update System Time
			m_iSystemTime = nextTransaction.NextSystemTime;

			// Add transaction to the CEC
			m_ltCurrentEventsChain.Add( nextTransaction );

			// Remove it from the FEC
			m_ltFutureEventsChain.RemoveAt( 0 );

			// Get all transactions in the FEC with the same NextSystemTime
			List<Transaction> anotherTransactions = m_ltFutureEventsChain.
													FindAll( t => t.NextSystemTime == m_iSystemTime );

			// If there are one or more transaction with the same system time
			if( anotherTransactions.Count > 0 )
			{
				// Update Transactions 
				anotherTransactions.ForEach(	delegate( Transaction t )
												{
													t.ScanStatus = false;
													t.State = TransactionState.ACTIVE;
												} );

				// Add all transactions to the CEC
				m_ltCurrentEventsChain.AddRange( anotherTransactions );

				// Remove them all from the FEC
				m_ltFutureEventsChain.RemoveAll( t => t.NextSystemTime == m_iSystemTime );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="currentTransaction"></param>
		private void MoveTransaction( Transaction currentTransaction )
		{
			// Process next Model Block and continue until transaction change its ScanStatus 
			BlockTransactional nextBlock = currentTransaction.NextBlock;
			while( !currentTransaction.ScanStatus ) {
				// Generate new transactions after current transaction leaves the generate block
				if( currentTransaction.CurrentBlock is GenerateBlock )
				{
					// transaction is leaving generate block, generate new transactions
					ScheduleNewTransactionFromGenerate( ( GenerateBlock )currentTransaction.CurrentBlock );
				}
				nextBlock.Process( ref currentTransaction );
			}
		}

		/// <summary>
		/// Simulation Process
		/// </summary>
		/// <param name="iTargetTime"></param>
		public void Simulate( int iTargetTime = DEFAULT_MAX_TARGET_TIME )
		{
			// Reset System Time
			Clear();

			// Start Simulation
			State = SimulationState.Running;
			TargetTime = iTargetTime;

			// Generate First Transactions (from each Generate Block)
			GenerateFirstTransactions();

			// While Current System Time is lower than Target System Time 
			while( !SimulationIsFinished() && SimulationIsRunning() )
			{
				while(	CurrentTransactionNumber > 0 && 
						( ( !ChangeFlag && State == SimulationState.Running ) || State == SimulationState.RunStepByStep ) && 
						!SimulationIsFinished() )
				{
					Transaction currentTransaction = GetTransactionFromCECByNumber( CurrentTransactionNumber );

					if( State == SimulationState.Running ) CurrentTransactionNumber = GetNextTransactionNumberFromCEC();

					if( !currentTransaction.ScanStatus )
					{
						MoveTransaction( currentTransaction );
					}
					else
					{
						if( State == SimulationState.RunStepByStep )
						{
							CurrentTransactionNumber = GetNextTransactionNumberFromCEC();
						}
					}
				}

				if( !ChangeFlag && !SimulationIsFinished() && CurrentTransactionNumber == 0 && SimulationIsRunning() )
				{
					// Update System Time, move transactions from FEC to CEC
					UpdateSystemTime();

					// Reset Change Flag (if it is ON)
					if( ChangeFlag ) ChangeFlag = false;

					// Get First Transaction from CEC
					CurrentTransactionNumber = GetFirstTransactionNumberFromCEC();
				}
				else
				{
					if( ChangeFlag && State == SimulationState.Running )
					{
						ChangeFlag = false;

						// Get First Transaction from CEC (if there are transactions)
						CurrentTransactionNumber = GetFirstTransactionNumberFromCEC();
					}
				}
			}

			// Simulation Ended
			State = SimulationState.Finished;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private bool SimulationIsRunning()
		{
			return ( State == SimulationState.Running || State == SimulationState.RunStepByStep );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iTargetTime"></param>
		/// <returns></returns>
		private bool SimulationIsFinished()
		{
			// if target time has not been achieved and termination count is greater than zero
			if( SystemTime <= TargetTime && TerminationCount > 0 && 
				State != SimulationState.Finished && State != SimulationState.Error ) 
				return false;

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oGenerateBlock"></param>
		/// <returns></returns>
		public bool ScheduleNewTransactionFromGenerate( GenerateBlock oGenerateBlock )
		{
			int iTransactionNumber = m_iTransactionCounter + 1;
			Transaction oTransaction = new Transaction( iTransactionNumber );
			if( oGenerateBlock.Process( ref oTransaction ) == BlockProcessResult.TRANSACTION_PROCESSED )
			{
				// Effectively increment by 1
				TransactionCounter++;

				return true;
			}

			return false;
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
		/// <returns></returns>
		public bool RemoveTransactonFromFEC( Transaction oTransaction )
		{
			return m_ltFutureEventsChain.Remove( oTransaction );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Transaction GetFirstTransactionFromFEC()
		{
			if( m_ltFutureEventsChain.Count == 0 ) return null;

			return m_ltFutureEventsChain.OrderBy( t => t.NextSystemTime ).First();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		public void InsertTransactionIntoCEC( Transaction oTransaction )
		{
			m_ltCurrentEventsChain.Add( oTransaction );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		public bool RemoveTransactionFromCEC( Transaction oTransaction )
		{
			return m_ltCurrentEventsChain.Remove( oTransaction );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int GetFirstTransactionNumberFromCEC()
		{
			if( m_ltCurrentEventsChain.Count == 0 ) return Constants.DEFAULT_ZERO_VALUE;

			Transaction firstTransaction = m_ltCurrentEventsChain.OrderByDescending( t => t.Priority ).First();
			return firstTransaction.Number;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iTransactionNumber"></param>
		/// <returns></returns>
		public Transaction GetTransactionFromCECByNumber( int iTransactionNumber )
		{
			Transaction oTransaction = ( from t in m_ltCurrentEventsChain
										 where t.Number == iTransactionNumber
										 select t ).FirstOrDefault();
			return oTransaction;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int GetNextTransactionNumberFromCEC()
		{
			if( m_ltCurrentEventsChain.Count > 0 )
			{
				Transaction oTransaction = ( from t in m_ltCurrentEventsChain
											 where t.Number != CurrentTransactionNumber &&
											 t.AlreadyProcessed == false
											 select t ).OrderByDescending( t => t.Priority ).FirstOrDefault();

				if( oTransaction != null ) return oTransaction.Number;
			}
			return Constants.DEFAULT_ZERO_VALUE;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iDecrement"></param>
		/// <returns></returns>
		public bool DecrementTerminationCounter( int iDecrement )
		{
			if( iDecrement < 0 ) return false;

			if( iDecrement > 0 ) TerminationCount -= iDecrement;

			return ( TerminationCount >= 0 );
		}
		#endregion
	}
}
