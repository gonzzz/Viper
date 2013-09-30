using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Enums;
using Viper.Framework.Entities;
using Viper.Framework.Utils;
using Viper.Framework.Exceptions;

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
			try
			{
				if( String.IsNullOrEmpty( this.Text ) )
				{
					OnParseFailed( new ParseEventArgs( BlockNames.ASSIGN, this.Line, String.Empty ) );
					return BlockParseResult.PARSED_ERROR;
				}

				// Get Plain Text Block Parts
				String[] sBlockParts = BlockFactory.GetBlockParts( this.Text );

				// CORRECT SINTAX FORMAT in '[]' optional part: 
				// [NAME]	ASSIGN	A,B[,C]
				if( sBlockParts[ 0 ].Equals( BlockNames.ASSIGN ) && sBlockParts.Length == 2 )
				{
					// FORMATS:		ASSIGN A,B /	ASSIGN A,B,C
					m_sBlockLabel = String.Empty;

					String[] operands = sBlockParts[ 1 ].Split( ',' );

					if( operands.Length == 0 || operands.Length == 1 || operands.Length > 3 )
					{
						OnParseFailed( new ParseEventArgs( BlockNames.ASSIGN, this.Line, String.Empty ) );
						return BlockParseResult.PARSED_ERROR;
					}

					if( operands.Length >= 1 ) BlockOperand.TranslateOperand( this.OperandA, operands[ 0 ], true );
					if( operands.Length >= 2 ) BlockOperand.TranslateOperand( this.OperandB, operands[ 1 ], true );
					if( operands.Length >= 3 ) BlockOperand.TranslateOperand( this.OperandC, operands[ 2 ] );

					// Operands A & B are required, Operand C optional, all must have valid values
					if ( this.OperandA.HasValidValue && this.OperandB.HasValidValue && this.OperandC.HasValidValue )
						return BlockParseResult.PARSED_OK;
				}
				else if( sBlockParts[ 1 ].Equals( BlockNames.ASSIGN ) && sBlockParts.Length == 3 )
				{
					// FORMATS: NAME ASSIGN A,B / NAME ASSIGN A,B,C
					m_sBlockLabel = sBlockParts[ 0 ];

					String[] operands = sBlockParts[ 2 ].Split( ',' );

					if( operands.Length == 0 || operands.Length == 1 || operands.Length > 3 )
					{
						OnParseFailed( new ParseEventArgs( BlockNames.ASSIGN, this.Line, String.Empty ) );
						return BlockParseResult.PARSED_ERROR;
					}

					if( operands.Length >= 1 ) BlockOperand.TranslateOperand( this.OperandA, operands[ 0 ], true );
					if( operands.Length >= 2 ) BlockOperand.TranslateOperand( this.OperandB, operands[ 1 ], true );
					if( operands.Length >= 3 ) BlockOperand.TranslateOperand( this.OperandC, operands[ 2 ] );

					// Operands A & B are required, Operand C optional, all must have valid values
					if( this.OperandA.HasValidValue && this.OperandB.HasValidValue && this.OperandC.HasValidValue )
						return BlockParseResult.PARSED_OK;
				}

				OnParseFailed( new ParseEventArgs( BlockNames.ASSIGN, this.Line, String.Empty ) );
			}
			catch( Exception ex )
			{
				throw new BlockParseException( ex.Message, ex.InnerException, BlockNames.ASSIGN, this.Line );
			}
			return BlockParseResult.PARSED_ERROR;
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

		#region IProcessable Implementation
		public override BlockProcessResult Process( ref Transaction oTransaction )
		{
			try
			{
				

				// Common Process
				base.Process( ref oTransaction );

				// Notify Success
				OnProcessSuccess( new ProcessEventArgs( BlockNames.ASSIGN, this.Line, String.Empty ) );

				return BlockProcessResult.TRANSACTION_PROCESSED;
			}
			catch( Exception ex )
			{
				// Notify Fail
				OnProcessFailed( new ProcessEventArgs( BlockNames.ASSIGN, this.Line, ex.Message ) );

				// Return Exception
				return BlockProcessResult.TRANSACTION_EXCEPTION;
			}
		}
		#endregion
	}
}
