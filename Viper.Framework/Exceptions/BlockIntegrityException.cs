using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Exceptions
{
	/// <summary>
	/// Represents an unhandled error from Block integrity check methods
	/// </summary>
	public class BlockIntegrityException : Exception
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

		public BlockIntegrityException()
			: base()
		{
		}

		public BlockIntegrityException( String message )
			: base( message )
		{
		}

		public BlockIntegrityException( String message, Exception innerException )
			: base( message, innerException )
		{
		}

		public BlockIntegrityException( String message, Exception innerException, String sBlockName, int iLineNumber )
			: base( message, innerException )
		{
			BlockName = sBlockName;
			LineNumber = iLineNumber;
		}
	}
}
