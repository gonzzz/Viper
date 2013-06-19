using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Exceptions
{
	public class InvalidSNASpecifierException : Exception
	{
		public InvalidSNASpecifierException()
			: base()
		{
		}

		public InvalidSNASpecifierException( String message )
			: base( message )
		{
		}

		public InvalidSNASpecifierException( String message, Exception innerException )
			: base( message, innerException )
		{
		}
	}
}
