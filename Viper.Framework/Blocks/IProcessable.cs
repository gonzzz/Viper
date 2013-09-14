using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Enums;
using Viper.Framework.Entities;

namespace Viper.Framework.Blocks
{
	public class ProcessEventArgs : EventArgs
	{
		public String BlockName;
		public int LineNumber;
		public String CustomMessage;

		public ProcessEventArgs()
			: base()
		{
		}

		public ProcessEventArgs( String sBlockName , int iLineNumber , String sCustomMessage )
			: base()
		{
			BlockName = sBlockName;
			LineNumber = iLineNumber;
			CustomMessage = sCustomMessage;
		}
	}

	public interface IProcessable
	{
		BlockProcessResult Process( ref Transaction oTransaction );

		event EventHandler ProcessSuccess;
		event EventHandler ProcessFailed;

		void OnProcessSuccess( ProcessEventArgs eventArgs );
		void OnProcessFailed( ProcessEventArgs eventArgs );
	}
}
