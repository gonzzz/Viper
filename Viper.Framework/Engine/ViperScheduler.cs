using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Viper.Framework.Entities;
using Viper.Framework.Blocks;

namespace Viper.Framework.Engine
{
	/// <summary>
	/// Transaction Scheduler class. Selects the Active Transaction and moves transactions from the FEC 
	/// to the CEC and viceversa. 
	/// </summary>
	public class ViperScheduler
	{
		#region Private Members
		private List<Transaction> m_ltCurrentEventsChain;
		private List<Transaction> m_ltFutureEventsChain;
		#endregion

		#region Constructor
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ViperScheduler()
		{
			m_ltCurrentEventsChain = new List<Transaction>();
			m_ltFutureEventsChain = new List<Transaction>();
		}
		#endregion
	}
}
