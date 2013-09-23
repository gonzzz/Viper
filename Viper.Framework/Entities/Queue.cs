using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Engine;

namespace Viper.Framework.Entities
{
	/// <summary>
	/// Represents an Queue Entity in a GPSS model.
	/// </summary>
	public class Queue : Entity
	{
		#region Constants
		public static int DEFAULT_QUEUE_AMOUNT = 1;
		#endregion

		#region Private Members
		private int m_iEntryCount;
		private int m_iZeroDelayCount;
		private int m_iCurrentCount;
		private int m_iMaxCount;
		private int m_iTotalTimeOfUse;
		private int m_iLastUse;
		private List<TransactionTimeOnChain> m_ltTransactions;
		#endregion

		#region Public properties
		/// <summary>
		/// Current Count
		/// </summary>
		public int CurrentCount
		{
			get
			{
				return m_iCurrentCount;
			}
		}

		/// <summary>
		/// Total Count
		/// </summary>
		public int TotalCount
		{
			get
			{
				return m_iEntryCount;
			}
		}

		/// <summary>
		/// Zero Delay Total Count
		/// </summary>
		public int ZeroDelayTotalCount
		{
			get
			{
				return m_iZeroDelayCount;
			}
		}
		#endregion

		#region SNA Properties
		/// <summary>
		/// Q = Query Current Count
		/// </summary>
		public int Q
		{
			get
			{
				return m_iCurrentCount;
			}
		}

		/// <summary>
		/// QC = Query Total Count
		/// </summary>
		public int QC
		{
			get
			{
				return m_iEntryCount;
			}
		}

		/// <summary>
		/// QM = Queue Max Count (at the same time)
		/// </summary>
		public int QM
		{
			get
			{
				return m_iMaxCount;
			}
		}

		/// <summary>
		/// QZ = Queue Entry Count with No delay (zero time in queue)
		/// </summary>
		public int QZ
		{
			get
			{
				return m_iZeroDelayCount;
			}
		}

		/// <summary>
		/// QA = Queue Average Time of Use
		/// </summary>
		public double QA
		{
			get
			{
				int iSystemTime = ViperSystem.Instance().SystemTime;
				if( iSystemTime > 0 )
				{
					return ( double )( m_iTotalTimeOfUse / iSystemTime );
				}
				else
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// QT = Queue Average Time of Use per Transaction
		/// </summary>
		public double QT
		{
			get
			{
				if( m_iEntryCount > 0 )
				{
					return ( double )( m_iTotalTimeOfUse / m_iEntryCount );
				}
				else
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// QX = Queue Average Time of User per Transaction with No delay
		/// </summary>
		public double QX
		{
			get
			{
				if( (m_iEntryCount > 0) && (m_iEntryCount != m_iZeroDelayCount) )
				{
					return ( double )( m_iTotalTimeOfUse / (m_iEntryCount - m_iZeroDelayCount) );
				}
				else
				{
					return 0;
				}
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public Queue() : base()
		{
			m_iEntryCount = 0;
			m_iZeroDelayCount = 0;
			m_iCurrentCount = 0;
			m_iMaxCount = 0;
			m_iTotalTimeOfUse = 0;
			m_iLastUse = 0;

			m_ltTransactions = new List<TransactionTimeOnChain>();
		}

		/// <summary>
		/// Constructor with params
		/// </summary>
		/// <param name="sName"></param>
		public Queue( String sName ) : base( sName )
		{
			m_iEntryCount = 0;
			m_iZeroDelayCount = 0;
			m_iCurrentCount = 0;
			m_iMaxCount = 0;
			m_iTotalTimeOfUse = 0;
			m_iLastUse = 0;

			m_ltTransactions = new List<TransactionTimeOnChain>();
		}
		#endregion

		#region Entity Methods
		/// <summary>
		/// Adds a Transaction into the Queue
		/// </summary>
		/// <param name="oTransaction"></param>
		public void DoQueue( Transaction oTransaction, int iAmountToQueue )
		{
			// Add Transaction to Queue and increment current count
			m_ltTransactions.Add( new TransactionTimeOnChain( oTransaction, ViperSystem.Instance().SystemTime, 0 ) );	
			m_iCurrentCount += iAmountToQueue;

			// Update Max Count
			m_iEntryCount += iAmountToQueue;
			if( m_iMaxCount < this.Q ) m_iMaxCount = this.Q;

			// Update Total Usage Time
			int iTimeLength = ViperSystem.Instance().SystemTime - m_iLastUse;
			m_iTotalTimeOfUse += iTimeLength * m_iCurrentCount;

			// Save Last Usage
			m_iLastUse = ViperSystem.Instance().SystemTime;
		}

		/// <summary>
		/// Removes a Transaction from the Queue
		/// </summary>
		/// <param name="oTransaction"></param>
		public void DoDepart( Transaction oTransaction, int iAmountToDequeue )
		{
			TransactionTimeOnChain ttime = null;
			if( (ttime = m_ltTransactions.Find( tt => tt.Transaction == oTransaction ) ) != null )
			{
				// Calculate Queue Time, remove Transaction from Queue and decrement current count
				ttime.OutTime = ViperSystem.Instance().SystemTime;
				int iTimeInQueue = ttime.OutTime - ttime.InTime;
				m_ltTransactions.Remove( ttime );
				m_iCurrentCount -= iAmountToDequeue;

				// Update zero delay entry counter
				if( iTimeInQueue == 0 ) m_iZeroDelayCount += 1;

				// Update Total Usage Time
				int iTimeLength = ViperSystem.Instance().SystemTime - m_iLastUse;
				m_iTotalTimeOfUse += iTimeLength * m_iCurrentCount;

				// Save Last Usage
				m_iLastUse = ViperSystem.Instance().SystemTime;
			}
		}
		#endregion
	}
}
