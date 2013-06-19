﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Utils
{
	public class Constants
	{
		/// <summary>
		/// Default Transaction Priority
		/// </summary>
		public const int DEFAULT_PRIORITY = 1;
		
		/// <summary>
		/// Default zero value to be used
		/// </summary>
		public const int DEFAULT_ZERO_VALUE = 0;

		/// <summary>
		/// Universal SNA token separator
		/// </summary>
		public const String SNA_TOKEN_SEPARATOR = "$";

		/// <summary>
		/// Transaction indirection token
		/// </summary>
		public const String INDIRECT_ADDRESSING_TOKEN = "*";

		/// <summary>
		/// Regular Expression to Validate a Name
		/// </summary>
		public const String REGEX_VALID_NAME = @"^[a-zA-Z]+[a-zA-Z0-9]*$";

		/// <summary>
		/// Regular Expression to Validate a Name (with separator token)
		/// </summary>
		public const String REGEX_VALID_NAME_WITH_TOKEN = @"^\$[a-zA-Z]+[a-zA-Z0-9]*$";

		/// <summary>
		/// Regular Expression to Validate a Positive Integer
		/// </summary>
		public const String REGEX_VALID_POSITIVE_INTEGER = @"^([1-9]?[0-9]*)$";
	}
}
