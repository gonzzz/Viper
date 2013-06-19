using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Enums
{
	public enum BlockParseResult
	{
		/// <summary>
		/// Block has not been parsed and no result can be get.
		/// </summary>
		NOT_PARSED = 0,

		/// <summary>
		/// Block has been parsed successfully.
		/// </summary>
		PARSED_OK = 1,

		/// <summary>
		/// Block has been parsed incorrectly.
		/// </summary>
		PARSED_ERROR = 2
	}
}
