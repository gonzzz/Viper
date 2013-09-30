using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Enums;
using Viper.Framework.Exceptions;
using Viper.Framework.Utils;

namespace Viper.Framework.Blocks
{
	/// <summary>
	/// Initial Block. Initializes a LogicSwitch, SaveValue or Matrix Element entities.
	/// </summary>
	public class InitialBlock : Block, IParseable
	{
		#region Operands
		/// <summary>
		/// A LogicSwitch, SaveValue or Matrix Element specified as SNA with 
		/// the form of LS.., X.. or MX.. SNA.
		/// </summary>
		public BlockOperand OperandA
		{
			get;
			set;
		}

		/// <summary>
		/// Value to be assigned. Default 1. Optional.
		/// </summary>
		public BlockOperand OperandB
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor Default
		/// </summary>
		public InitialBlock() : base()
		{
			this.OperandA = BlockOperand.EmptyOperand();
			this.OperandB = BlockOperand.EmptyOperand();
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="iLineNumber"></param>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public InitialBlock( int iLineNumber, int iBlockNumber, String sBlockText ) : base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = BlockOperand.EmptyOperand();
			this.OperandB = BlockOperand.EmptyOperand();
		}
		#endregion

		#region IParseable Implementation
		// <summary>
		/// Parse Plain Text Block and returns a Viper Initial Block
		/// </summary>
		/// <returns></returns>
		public BlockParseResult Parse()
		{
			try
			{
				if ( String.IsNullOrEmpty( this.Text ) )
				{
					OnParseFailed( new ParseEventArgs( BlockNames.INITIAL , this.Line , String.Empty ) );
					return BlockParseResult.PARSED_ERROR;
				}

				// Get Plain Text Block Parts
				String[] sBlockParts = BlockFactory.GetBlockParts( this.Text );

				// CORRECT SINTAX FORMAT in '[]' optional part: 
				//		INITIAL	A[,B]
				if( sBlockParts[ 0 ].Equals( BlockNames.INITIAL ) && ( sBlockParts.Length == 2 ) )
				{
					// FORMATS: INITIAL A; INITIAL A,B

					// NO label in this block
					m_sBlockLabel = String.Empty;

					// ALL OPERANDS: A,B
					// Check for parenthesis "(x,y)" = MatrixSaveValue case
					String[] operands = new String[] { "", "" };
					if( sBlockParts[ 1 ].Contains("(") || sBlockParts[ 1 ].Contains(")") )
					{
						int iIndexOfOpenParenthesis = -1;
						int iIndexOfCloseParenthesis = -1;
						if( sBlockParts[ 1 ].Contains("(") ) iIndexOfOpenParenthesis = sBlockParts[ 1 ].IndexOf( "(" );
						if ( sBlockParts[ 1 ].Contains( ")" ) ) iIndexOfCloseParenthesis = sBlockParts[ 1 ].IndexOf( ")" );

						if( iIndexOfOpenParenthesis > 0 && iIndexOfCloseParenthesis > 0 )
						{
							int iLastComma = sBlockParts[ 1 ].IndexOf( "," , iIndexOfCloseParenthesis );
							operands[ 0 ] = sBlockParts[ 1 ].Substring( 0, iLastComma );
							operands[ 1 ] = sBlockParts[ 1 ].Substring( iLastComma + 1 );
						}
						else
						{
							OnParseFailed( new ParseEventArgs( BlockNames.INITIAL , this.Line , String.Empty ) );
							return BlockParseResult.PARSED_ERROR;
						}
					}
					else
					{
						operands = sBlockParts[ 1 ].Split( ',' );
					}

					if ( operands.Length == 0 || operands.Length > 2 )
					{
						OnParseFailed( new ParseEventArgs( BlockNames.INITIAL , this.Line , String.Empty ) );
						return BlockParseResult.PARSED_ERROR;
					}

					if ( operands.Length >= 1 ) BlockOperand.TranslateOperand( this.OperandA , operands[ 0 ] , true );
					if ( operands.Length >= 2 ) BlockOperand.TranslateOperand( this.OperandB , operands[ 1 ] );

					// Operand A is required, Operand B optional, both have to get valid values
					if ( ( this.OperandA.HasValidValue && this.OperandB.HasValidValue )
						 && 
						 ( 
							( this.OperandA.IsSNA ) 
							&& 
							(	
							this.OperandA.SNA.Type == SNAType.LogicSwitch 
							|| this.OperandA.SNA.Type == SNAType.SaveValue 
							|| this.OperandA.SNA.Type == SNAType.MatrixSaveValue 
							)
							&& 
							( !this.OperandA.SNA.Parameter.IndirectAddressing )
						 )
						 &&
						 ( this.OperandB.IsEmpty || (!this.OperandB.IsEmpty && !this.OperandB.IsSNA) )
						)
						return BlockParseResult.PARSED_OK;
				} 

				OnParseFailed( new ParseEventArgs( BlockNames.INITIAL , this.Line , String.Empty ) );
			}
			catch( Exception ex )
			{
				throw new BlockParseException( ex.Message , ex.InnerException , BlockNames.INITIAL , this.Line );
			}

			return BlockParseResult.PARSED_ERROR;
		}

		public event EventHandler ParseSuccess;
		public event EventHandler ParseFailed;

		public void OnParseSuccess( ParseEventArgs eventArgs )
		{
			if ( ParseSuccess != null ) ParseSuccess( this , eventArgs );
		}

		public void OnParseFailed( ParseEventArgs eventArgs )
		{
			if ( ParseFailed != null ) ParseFailed( this , eventArgs );
		}
		#endregion
	}
}
