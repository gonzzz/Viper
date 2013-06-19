using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Exceptions
{
	/// <summary>
	/// Represents an unhandled error from Block Parsing methods
	/// </summary>
	public class BlockParseException : Exception
	{
		public String BlockName
		{
			get;
			set;
		}

		public int LineNumber
		{
			get;
			set;
		}

		public BlockParseException()
			: base()
		{
		}

		public BlockParseException( String message )
			: base( message )
		{
		}

		public BlockParseException( String message, Exception innerException )
			: base( message, innerException )
		{
		}

		public BlockParseException( String message, Exception innerException, String sBlockName, int iLineNumber )
			: base( message, innerException )
		{
			BlockName = sBlockName;
			LineNumber = iLineNumber;
		}
	}
}
