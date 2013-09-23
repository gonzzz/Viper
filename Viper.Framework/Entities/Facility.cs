using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Utils;
using Viper.Framework.Engine;

namespace Viper.Framework.Entities
{
	/// <summary>
	/// Facility Class. Represents a GPSS Facility Entity.
	/// </summary>
	public class Facility : Entity
	{
		#region Private Members
		private int m_iEntryCount;
		private bool m_bAvailable;
		private Transaction m_tOwner;
		private bool m_bPreempted;
		private int m_iTotalTimeOfUse;
		private int m_iLastUse;
		private List<Transaction> m_ltTransactionsDelayChain;
		private List<Transaction> m_ltTransactionsPendingChain;
		#endregion

		#region Public Properties
		/// <summary>
		/// Transaction Owner
		/// </summary>
		public Transaction Owner
		{
			get
			{
				return m_tOwner;
			}
		}

		/// <summary>
		/// True if Facility is available
		/// </summary>
		public bool IsAvailable
		{
			get
			{
				return m_bAvailable;
			}
		}

		/// <summary>
		/// True if Facility has been preempted
		/// </summary>
		public bool IsPreempted
		{
			get
			{
				return m_bPreempted;
			}
		}

		/// <summary>
		/// Facility Entry Count
		/// </summary>
		public int TotalCount
		{
			get
			{
				return m_iEntryCount;
			}
		}
		#endregion

		#region SNA Properties
		/// <summary>
		/// F = Facility is occupy (by seize or preempt) and has an owner Transaction
		/// </summary>
		public bool F
		{
			get
			{
				return ( m_tOwner != null );
			}
		}

		/// <summary>
		/// FC = Facility Entry Count
		/// </summary>
		public int FC
		{
			get
			{
				return m_iEntryCount;
			}
		}

		/// <summary>
		/// FI = Facility has been Interrupted
		/// </summary>
		public bool FI
		{
			get
			{
				return m_bPreempted;
			}
		}

		/// <summary>
		/// FT = Facility Time of Use per Transaction
		/// </summary>
		public double FT
		{
			get
			{
				if( m_iEntryCount > 0 )
				{
					return ( double )m_iTotalTimeOfUse / ( double )m_iEntryCount;
				}
				else
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// FA = Facility Time of Use
		/// </summary>
		public double FA
		{
			get
			{
				int iSystemTime = ViperSystem.Instance().SystemTime;
				if( iSystemTime > 0 )
				{
					return ( double )m_iTotalTimeOfUse / ( double )iSystemTime;
				}
				else
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// FV = Facility is Available
		/// </summary>
		public bool FV
		{
			get
			{
				return m_bAvailable;
			}
		}
		#endregion

		#region Constructor
		public Facility() : base()
		{
			m_iEntryCount = 0;
			m_bAvailable = true;
			m_tOwner = null;
			m_bPreempted = false;
			m_ltTransactionsDelayChain = new List<Transaction>(); // tx waiting to occupy facility by seize
			m_ltTransactionsPendingChain = new List<Transaction>(); // tx waiting to ocupy facility by preempt
		}

		public Facility( String sName ) : base(sName)
		{
			m_iEntryCount = 0;
			m_bAvailable = true;
			m_tOwner = null;
			m_bPreempted = false;
			m_ltTransactionsDelayChain = new List<Transaction>(); // tx waiting to occupy facility by seize
			m_ltTransactionsPendingChain = new List<Transaction>(); // tx waiting to ocupy facility by preempt
		}
		#endregion

		#region Entity Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		public void DoSeize( Transaction oTransaction )
		{
			// Update Owner (not preempted)
			m_tOwner = oTransaction;
			m_bPreempted = false;

			// Update Entry Count and Max Count
			m_iEntryCount++;
			
			// Update Total Usage Time
			int iTimeLength = ViperSystem.Instance().SystemTime - m_iLastUse;
			m_iTotalTimeOfUse += iTimeLength;

			// Save Last Usage
			m_iLastUse = ViperSystem.Instance().SystemTime;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		public void DoRelease( Transaction oTransaction )
		{
			// Update Owner (not preempted)
			m_tOwner = null;
			m_bPreempted = false;

			// Update Total Usage Time
			int iTimeLength = ViperSystem.Instance().SystemTime - m_iLastUse;
			m_iTotalTimeOfUse += iTimeLength;

			// Save Last Usage
			m_iLastUse = ViperSystem.Instance().SystemTime;
		}
		#endregion

		#region Delay Chain Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		public void AddTransactionIntoDelayChain( Transaction oTransaction )
		{
			m_ltTransactionsDelayChain.Add( oTransaction );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Transaction RemoveFirstTransactionFromDelayChain()
		{
			Transaction oTransactionT = m_ltTransactionsDelayChain.OrderByDescending( t => t.Priority ).FirstOrDefault();
			if( oTransactionT != null )
			{
				m_ltTransactionsDelayChain.Remove( oTransactionT );

				return oTransactionT;
			}
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int TransactionCountInDelayChain()
		{
			return m_ltTransactionsDelayChain.Count;
		}
		#endregion

		#region Pending Chain Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		public void AddTransactionIntoPendingChain( Transaction oTransaction )
		{
			m_ltTransactionsPendingChain.Add( oTransaction );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Transaction RemoveFirstTransactionFromPendingChain()
		{
			Transaction oTransactionT = m_ltTransactionsPendingChain.OrderByDescending( t => t.Priority ).FirstOrDefault();
			if( oTransactionT != null )
			{
				m_ltTransactionsPendingChain.Remove( oTransactionT );

				return oTransactionT;
			}
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int TransactionCountInPendingChain()
		{
			return m_ltTransactionsPendingChain.Count;
		}
		#endregion
	}
}
