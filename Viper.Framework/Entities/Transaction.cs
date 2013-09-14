using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Viper.Framework.Blocks;
using Viper.Framework.Utils;
using Viper.Framework.Enums;

namespace Viper.Framework.Entities
{
	/// <summary>
	/// Common Transaction Class. Represents a Transaction entity which moves throughout the simulation model.
	/// </summary>
	public class Transaction
	{
		#region Private Members
		private int m_iNumber;
		private Block m_oCurrentBlock;
		private Block m_oNextBlock;
		private int m_iPriority;
		private TransactionState m_tsState;
		private bool m_bPreempted;
		private bool m_bDelayed;
		private bool m_bTrace;
		private int m_iMarkTime;
		private Dictionary<int, int> m_dParameters;
		private List<Transaction> m_lAssemblySet;
		#endregion

		#region Public SNA Properties
		/// <summary>
		/// Transaction Number set by the Transaction Scheduler.
		/// Related SNA: XN1 (transaction number of active transaction)
		/// </summary>
		public int Number
		{
			get
			{
				return m_iNumber;
			}
		}

		/// <summary>
		/// Current Block in the current model.
		/// </summary>
		public Block CurrentBlock
		{
			get
			{
				return m_oCurrentBlock;
			}
			set
			{
				m_oCurrentBlock = value;
			}
		}

		/// <summary>
		/// Next Block in the current model.
		/// </summary>
		public Block NextBlock
		{
			get
			{
				return m_oNextBlock;
			}
			set
			{
				m_oNextBlock = value;
			}
		}

		/// <summary>
		/// Transaction Priority set by the GENERATE block or PRIORITY block.
		/// Related SNA: PR
		/// </summary>
		public int Priority
		{
			get
			{
				return m_iPriority;
			}
			set
			{
				m_iPriority = value;
			}
		}

		/// <summary>
		/// Current Transaction State. Can be ACTIVE, SUSPENDED, PASSIVE or TERMINATED.
		/// </summary>
		public TransactionState State
		{
			get
			{
				return m_tsState;
			}
			set
			{
				m_tsState = value;
			}
		}

		/// <summary>
		/// Returns TRUE if the transaction has been preempted. FALSE otherwise.
		/// </summary>
		public bool IsPreempted
		{
			get
			{
				return m_bPreempted;
			}
			set
			{
				m_bPreempted = value;
			}
		}

		/// <summary>
		/// Returns TRUE if the transaction has been refused to entry some block. 
		/// </summary>
		public bool IsDelayed
		{
			get
			{
				return m_bDelayed;
			}
			set
			{
				m_bDelayed = value;
			}
		}

		/// <summary>
		/// Returns TRUE if the transaction should generate a message on every block entry. FALSE otherwise.
		/// </summary>
		public bool IsTraceEnabled
		{
			get
			{
				return m_bTrace;
			}
			set
			{
				m_bTrace = value;
			}
		}

		/// <summary>
		/// Returns absolute clock time that the transaction first entered the simulation, or entered a 
		/// MARK block with no A operand. 
		/// Related SNAs: M1, MPentnum
		/// </summary>
		public int MarkTime
		{
			get
			{
				return m_iMarkTime;
			}
			set
			{
				m_iMarkTime = value;
			}
		}
		#endregion

		#region Public Engine Control Flags Properties
		/// <summary>
		/// 
		/// </summary>
		public int NextSystemTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool ChangeFlag { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool ScanStatus { get; set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public Transaction( int iNumber )
		{
			m_iNumber = iNumber;
			m_iPriority = Constants.DEFAULT_PRIORITY;
			m_iMarkTime = Constants.DEFAULT_ZERO_VALUE;
			m_oCurrentBlock = null;
			m_oNextBlock = null;
			m_tsState = TransactionState.SUSPENDED;
			m_bPreempted = false;
			m_bDelayed = false;
			m_bTrace = false;
			m_dParameters = new Dictionary<int, int>();
			m_lAssemblySet = new List<Transaction>();
			m_lAssemblySet.Add( this );
		}

		/// <summary>
		/// Constructor with Transaction Number and Transaction Priority and Mark Time
		/// </summary>
		/// <param name="iNumber"></param>
		/// <param name="iPriority"></param>
		public Transaction( int iNumber, int iPriority, int iMarkTime )
		{
			m_iNumber = iNumber;
			m_iPriority = iPriority;
			m_iMarkTime = iMarkTime;
			m_oCurrentBlock = null;
			m_oNextBlock = null;
			m_tsState = TransactionState.SUSPENDED;
			m_bPreempted = false;
			m_bDelayed = false;
			m_bTrace = false;
			m_dParameters = new Dictionary<int, int>();
			m_lAssemblySet = new List<Transaction>();
			m_lAssemblySet.Add( this );
		}
		#endregion

		#region Parameters Methods
		/// <summary>
		/// Creates the parameter with number iParameterNumber if it doesn't exists. Default value is zero (0).
		/// </summary>
		/// <param name="iParameterNumber"></param>
		private void CreateParameterIfNotExists( int iParameterNumber )
		{
			if( !m_dParameters.ContainsKey( iParameterNumber ) )
			{
				m_dParameters.Add( iParameterNumber, Constants.DEFAULT_ZERO_VALUE );
			}
		}

		/// <summary>
		/// Returns parameter iParameterNumber value.
		/// </summary>
		/// <param name="iParameterNumber"></param>
		/// <returns></returns>
		public int GetParameter( int iParameterNumber )
		{
			CreateParameterIfNotExists( iParameterNumber );
			return m_dParameters[ iParameterNumber ];
		}

		/// <summary>
		/// Update parameter iParameterNumber 's value.
		/// </summary>
		/// <param name="iParameterNumber"></param>
		/// <param name="iParameterValue"></param>
		public void SetParameter( int iParameterNumber, int iParameterValue )
		{
			CreateParameterIfNotExists( iParameterNumber );
			m_dParameters[ iParameterNumber ] = iParameterValue;
		}
		#endregion

		#region Assembly Set Methods
		/// <summary>
		/// Returns array of transactions each of which is the descendant of the same generated transaction.
		/// Descendant transactions are generated in SPLIT blocks. By default every transaction assembly set
		/// has only one transaction: the transaction itself.
		/// </summary>
		/// <returns></returns>
		public Transaction[] GetTransactionsFromAssemblySet()
		{
			return m_lAssemblySet.ToArray();
		}

		/// <summary>
		/// Adds a transaction to the set of transactions in the Assembly Set. Functionality provided for the
		/// SPLIT block.
		/// </summary>
		/// <param name="oTransaction"></param>
		public void AddTransactionToAssemblySet( Transaction oTransaction )
		{
			if( !m_lAssemblySet.Contains( oTransaction ) )
			{
				m_lAssemblySet.Add( oTransaction );
			}
		}

		/// <summary>
		/// Removes a transaction from the set of transactions in the Assembly Set. Returns true if it was
		/// successful, false otherwise. Functionality provided for the ASSEMBLY block.
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <returns></returns>
		public bool RemoveTransactionFromAssemblySet( Transaction oTransaction )
		{
			if( m_lAssemblySet.Contains( oTransaction ) )
			{
				return m_lAssemblySet.Remove( oTransaction );
			}
			return false;
		}

		/// <summary>
		/// Removes all transactions from the Assembly Set except the current transaction.
		/// </summary>
		public void ClearTransactionsFromAssemblySet()
		{
			// Removes all transactions except this
			m_lAssemblySet.RemoveAll(	delegate( Transaction oTransaction )
										{
											return (oTransaction.Number != this.Number);
										} 
									);
		}
		#endregion
	}
}
