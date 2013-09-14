using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Enums
{
	public enum BlockProcessResult
	{
		/// <summary>
		/// Transaction couldn't be processed due to an exception
		/// </summary>
		TRANSACTION_EXCEPTION = -1,

		/// <summary>
		/// Transaction has been processed
		/// </summary>
		TRANSACTION_PROCESSED = 1,

		/// <summary>
		/// Transaction has been refused entry
		/// </summary>
		TRANSACTION_ENTRY_REFUSED = 2
	}
}
