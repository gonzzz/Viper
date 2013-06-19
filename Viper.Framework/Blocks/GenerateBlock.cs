using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Utils;
using Viper.Framework.Exceptions;
using Viper.Framework.Enums;
using Viper.Framework.Entities;

namespace Viper.Framework.Blocks
{
	/// <summary>
	/// Generate Block Class. Creates transactions and places them in the FEC.
	/// </summary>
	public class GenerateBlock : BlockTransactional, IParseable
	{
		#region Operands
		/// <summary>
		/// Operand A: Mean intergeneration time.
		/// </summary>
		public BlockOperand OperandA
		{
			get;
			set;
		}

		/// <summary>
		/// Operand B: Intergeneration time half-range or function modifier.
		/// </summary>
		public BlockOperand OperandB
		{
			get;
			set;
		}

		/// <summary>
		/// Operand C: Start Delay Time. Time increment for the first transaction.
		/// </summary>
		public BlockOperand OperandC
		{
			get;
			set;
		}

		/// <summary>
		/// Operand D: Creation limit. 
		/// </summary>
		public BlockOperand OperandD
		{
			get;
			set;
		}

		/// <summary>
		/// Operand E: Priority Level.
		/// </summary>
		public BlockOperand OperandE
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public GenerateBlock()
			: base()
		{
			this.OperandA = null;
			this.OperandB = null;
			this.OperandC = null;
			this.OperandD = null;
			this.OperandE = null;
		}

		/// <summary>
		/// Constructor With parameters
		/// </summary>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public GenerateBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = null;
			this.OperandB = null;
			this.OperandC = null;
			this.OperandD = null;
			this.OperandE = null;
		}
		#endregion

		#region IParseable Methods
		/// <summary>
		/// Parse Plain Text Block and returns a Viper Generate Block
		/// </summary>
		/// <returns></returns>
		public BlockParseResult Parse()
		{
			try
			{
				if( String.IsNullOrEmpty( this.Text ) )
				{
					OnParseFailed( new ParseEventArgs( BlockNames.GENERATE, this.Line, String.Empty ) );
					return BlockParseResult.PARSED_ERROR;
				}

				// Get Plain Text Block Parts
				String[] sBlockParts = BlockFactory.GetBlockParts( this.Text );

				// CORRECT SINTAX FORMAT: 
				// GENERATE	A,B,C,D,E
				// First Element should be the 'GENERATE' block name, Second Element the block operands 'A,B,C,D,E':
				if( sBlockParts[ 0 ].Equals( BlockNames.GENERATE ) && sBlockParts.Length == 2 )
				{
					// UNIQUE FORMAT: GENERATE A,B,C,D,E (all operands optionals, but A or D must be defined)

					// ALL OPERANDS: A,B,C,D,E
					String[] operands = sBlockParts[ 1 ].Split( ',' );

					if( operands.Length >= 1 ) this.OperandA = BlockOperand.TranslateOperand( operands[ 0 ] );
					if( operands.Length >= 2 ) this.OperandB = BlockOperand.TranslateOperand( operands[ 1 ] );
					if( operands.Length >= 3 ) this.OperandC = BlockOperand.TranslateOperand( operands[ 2 ] );
					if( operands.Length >= 4 ) this.OperandD = BlockOperand.TranslateOperand( operands[ 3 ] );
					if( operands.Length >= 5 ) this.OperandE = BlockOperand.TranslateOperand( operands[ 4 ] );

					if( this.OperandA.HasValidValue || this.OperandD.HasValidValue ) return BlockParseResult.PARSED_OK;
				}

				OnParseFailed( new ParseEventArgs( BlockNames.GENERATE, this.Line, String.Empty ) );
			}
			catch( Exception ex )
			{
				throw new BlockParseException( ex.Message, ex.InnerException, BlockNames.GENERATE, this.Line );
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

		public override BlockProcessResult Process( Transaction oTransaction )
		{
			throw new NotImplementedException();
		}
	}
}
