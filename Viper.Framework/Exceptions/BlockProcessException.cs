using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Exceptions
{
	/// <summary>
	/// 
	/// </summary>
	public class BlockProcessException : Exception
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

		public BlockProcessException()
			: base()
		{
		}

		public BlockProcessException( String message )
			: base( message )
		{
		}

		public BlockProcessException( String message, Exception innerException )
			: base( message, innerException )
		{
		}

		public BlockProcessException( String message , Exception innerException , String sBlockName , int iLineNumber )
			: base( message, innerException )
		{
			BlockName = sBlockName;
			LineNumber = iLineNumber;
		}
	}
}
