using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Enums
{
	public enum TransactionState
	{
		/// <summary>
		/// The transaction is the highest priority transaction on the Current Events Chain.
		/// </summary>
		ACTIVE = 1,

		/// <summary>
		/// The transaction is waiting on the Future Events Chain or the Current Events Chain to become the active transaction.
		/// </summary>
		WAITING = 2,

		/// <summary>
		/// The transaction has come to rest in the current model on a user chain, delay chain or pending chain.
		/// </summary>
		PASSIVE = 3,

		/// <summary>
		/// The transaction has been destroyed and no longer exists in the current model
		/// </summary>
		TERMINATED = 4
	}
}
