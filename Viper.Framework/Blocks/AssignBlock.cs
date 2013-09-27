using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Enums;

namespace Viper.Framework.Blocks
{
	/// <summary>
	/// Assign Block Class. Can create and modify transaction parameters.
	/// </summary>
	public class AssignBlock : BlockTransactional, IParseable
	{
		#region Operands
		/// <summary>
		/// Parameter Number, could be suffixed with a '+' or '-' sign, indicating
		/// the value in OperandB should be add or substract from current value.
		/// If no suffix is indicated, value in OperandB is assigned to the Parameter.
		/// </summary>
		public BlockOperand OperandA
		{
			get;
			set;
		}

		/// <summary>
		/// Value to assign to paremeter or add/substract from parameter.
		/// </summary>
		public BlockOperand OperandB
		{
			get;
			set;
		}

		/// <summary>
		/// Optional, represents a Function Number used as a Function Modifier. 
		/// Operand B is used to evaluate the Function and the result is assigned
		/// to the parameter, or add/substract from the parameter.
		/// </summary>
		public BlockOperand OperandC
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor Default
		/// </summary>
		public AssignBlock()
			: base()
		{
			this.OperandA = null;
			this.OperandB = null;
			this.OperandC = null;
		}

		/// <summary>
		/// Constructor with Parameters
		/// </summary>
		/// <param name="iLineNumber"></param>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public AssignBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = null;
			this.OperandB = null;
			this.OperandC = null;
		}
		#endregion

		#region IParseable Members
		/// <summary>
		/// Parse Plain Text Block and returns a Viper Assign Block
		/// </summary>
		/// <returns></returns>
		public BlockParseResult Parse()
		{
			throw new NotImplementedException();
		}

		public event EventHandler ParseSuccess;
		public event EventHandler ParseFailed;

		public void OnParseSuccess( ParseEventArgs eventArgs )
		{
			if( ParseSuccess != null ) ParseSuccess( this, eventArgs );
		}

		public void OnParseFailed( ParseEventArgs eventArgs )
		{
			if( ParseFailed != null ) ParseFailed( this, eventArgs );
		}

		#endregion
	}
}
