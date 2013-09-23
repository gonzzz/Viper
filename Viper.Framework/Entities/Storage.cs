using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Utils;
using Viper.Framework.Engine;
using Viper.Framework.Blocks;
using Viper.Framework.Enums;

namespace Viper.Framework.Entities
{
	/// <summary>
	/// Represents an Storage Entity in a GPSS model.
	/// </summary>
	public class Storage : Entity
	{
		#region Constants
		public static int DEFAULT_OCCUPY_AMOUNT = 1;
		#endregion

		#region Private Members
		private int m_iTotalCapacity;
		private int m_iAvailableCapacity;
		private bool m_bAvailable;
		private int m_iEntryCount;
		private int m_iMaxCount;
		private int m_iTotalTimeOfUse;
		private int m_iLastUse;
		private List<Transaction> m_ltTransactions;
		private List<Transaction> m_ltTransactionsDelayChain;
		#endregion

		#region Public Properties
		/// <summary>
		/// Returns total capacity for storage
		/// </summary>
		public int CapacityUnits
		{
			get
			{
				return m_iTotalCapacity;
			}
			private set
			{
				m_iTotalCapacity = value;
			}
		}

		/// <summary>
		/// Returns available units for storage.
		/// </summary>
		public int FreeUnits
		{
			get
			{
				return m_iAvailableCapacity;
			}
			private set
			{
				m_iAvailableCapacity = value;
			}
		}

		/// <summary>
		/// Returns current units in use for the storage.
		/// </summary>
		public int UsedUnits
		{
			get
			{
				return m_iTotalCapacity - m_iAvailableCapacity;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int TotalCount
		{
			get
			{
				return m_iEntryCount;
			}
		}

		/// <summary>
		/// Returns true if Storage is Availables, false otherwise
		/// </summary>
		public bool IsAvailable
		{
			get
			{
				return m_bAvailable;
			}
			private set
			{
				m_bAvailable = value;
			}
		}
		#endregion

		#region SNA Properties
		/// <summary>
		/// R = Storage available units (S+R = Storage Capacity)
		/// </summary>
		public int R
		{
			get
			{
				return FreeUnits;
			}
		}

		/// <summary>
		/// S = Storage used units (S+R = Storage Capacity)
		/// </summary>
		public int S
		{
			get
			{
				return UsedUnits;
			}
		}

		/// <summary>
		/// SC = Storage total count
		/// </summary>
		public int SC
		{
			get
			{
				return m_iEntryCount;
			}
		}

		/// <summary>
		/// SE = Storage if empty? (true if available units equals to total capacity units)
		/// </summary>
		public bool SE
		{
			get
			{
				return ( FreeUnits == CapacityUnits );
			}
		}

		/// <summary>
		/// SF = Storage is full? (true if available units equals to zero)
		/// </summary>
		public bool SF
		{
			get
			{
				return ( FreeUnits == 0 );
			}
		}

		/// <summary>
		/// SM = Storage max count
		/// </summary>
		public int SM
		{
			get
			{
				return ( m_iMaxCount );
			}
		}

		/// <summary>
		/// SV = Storage is available? 
		/// </summary>
		public bool SV
		{
			get
			{
				return ( m_bAvailable );
			}
		}

		/// <summary>
		/// SR = Storage average time of use per current use (in thousands percentile)
		/// </summary>
		public double SR
		{
			get
			{
				int iSystemTime = ViperSystem.Instance().SystemTime;
				if( iSystemTime > 0 )
				{
					return ( double )( 1000 * m_iTotalTimeOfUse ) / ( double )( iSystemTime * UsedUnits );
				}
				else
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// SA = Storage Time of Use
		/// </summary>
		public double SA
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
		/// ST = Storage Time of Use per Transaction
		/// </summary>
		public double ST
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
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public Storage() : base()
		{
			m_iEntryCount = 0;
			m_iMaxCount = 0;
			m_iTotalTimeOfUse = 0;
			m_iLastUse = 0;

			m_ltTransactions = new List<Transaction>();
			m_ltTransactionsDelayChain = new List<Transaction>();

			m_bAvailable = true;
			m_iAvailableCapacity = 0;
			m_iTotalCapacity = 0;
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="sName"></param>
		public Storage( String sName, int iCapacity ) : base( sName )
		{
			m_iEntryCount = 0;
			m_iMaxCount = 0;
			m_iTotalTimeOfUse = 0;
			m_iLastUse = 0;
			
			m_ltTransactions = new List<Transaction>();
			m_ltTransactionsDelayChain = new List<Transaction>();
			
			m_bAvailable = true;
			m_iAvailableCapacity = iCapacity;
			m_iTotalCapacity = iCapacity;
		}
		#endregion

		#region Entity Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <param name="iAmountToOccupy"></param>
		public void DoEnter( Transaction oTransaction, int iAmountToOccupy )
		{
			// Add Transaction to Storage and update FreeUnits
			m_ltTransactions.Add( oTransaction );
			FreeUnits -= iAmountToOccupy;
			
			// Update Entry Count and Max Count
			m_iEntryCount++;
			if( m_iMaxCount < UsedUnits ) m_iMaxCount = UsedUnits;

			// Update Total Usage Time
			int iTimeLength = ViperSystem.Instance().SystemTime - m_iLastUse;
			m_iTotalTimeOfUse += iTimeLength * UsedUnits;

			// Save Last Usage
			m_iLastUse = ViperSystem.Instance().SystemTime;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iAmountToOccupy"></param>
		/// <returns></returns>
		public bool IsEnterAvailable( int iAmountToOccupy )
		{
			return ( FreeUnits - iAmountToOccupy >= 0 );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iAmountToLiberate"></param>
		/// <returns></returns>
		public bool IsLeaveAvailable( int iAmountToLiberate )
		{
			return ( FreeUnits + iAmountToLiberate <= CapacityUnits );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <param name="iAmountToLiberate"></param>
		public void DoLeave( Transaction oTransaction, int iAmountToLiberate )
		{
			if( m_ltTransactions.Find( t => t == oTransaction ) != null )
			{
				// Remove Transaction from Storage
				m_ltTransactions.Remove( oTransaction );
				FreeUnits += iAmountToLiberate;

				// Update Total Usage Time
				int iTimeLength = ViperSystem.Instance().SystemTime - m_iLastUse;
				m_iTotalTimeOfUse += iTimeLength * UsedUnits;

				// Save Last Usage
				m_iLastUse = ViperSystem.Instance().SystemTime;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iNewCapacity"></param>
		public void ChangeCapacity( int iNewCapacity )
		{
			CapacityUnits = iNewCapacity;
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
	}
}
