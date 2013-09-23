using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Entities
{
	public class TransactionTimeOnChain
	{
		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public Transaction Transaction
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public int InTime
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public int OutTime
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		public TransactionTimeOnChain( Transaction oTransaction )
		{
			this.Transaction = oTransaction;
			this.InTime = 0;
			this.OutTime = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <param name="inTime"></param>
		/// <param name="outTime"></param>
		public TransactionTimeOnChain( Transaction oTransaction, int inTime, int outTime )
		{
			this.Transaction = oTransaction;
			this.InTime = inTime;
			this.OutTime = outTime;
		}
		#endregion
	}
}
