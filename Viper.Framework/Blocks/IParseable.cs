using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Enums;

namespace Viper.Framework.Blocks
{
	public class ParseEventArgs : EventArgs
	{
		public String BlockName;
		public int LineNumber;
		public String CustomMessage;

		public ParseEventArgs()
			: base()
		{
		}

		public ParseEventArgs( String sBlockName, int iLineNumber, String sCustomMessage )
			: base()
		{
			BlockName = sBlockName;
			LineNumber = iLineNumber;
			CustomMessage = sCustomMessage;
		}
	}

	public interface IParseable
	{
		BlockParseResult Parse(); 

		event EventHandler ParseSuccess;
		event EventHandler ParseFailed;

		void OnParseSuccess( ParseEventArgs eventArgs );
		void OnParseFailed( ParseEventArgs eventArgs );
	}
}
