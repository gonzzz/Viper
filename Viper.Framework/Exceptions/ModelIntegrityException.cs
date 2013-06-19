using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Exceptions
{
	public class ModelIntegrityException : Exception
	{
		public ModelIntegrityException()
			: base()
		{
		}

		public ModelIntegrityException( String message )
			: base( message )
		{
		}

		public ModelIntegrityException( String message, Exception innerException )
			: base( message, innerException )
		{
		}
	}
}
