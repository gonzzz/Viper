using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Utils;
using Viper.Framework.Exceptions;
using Viper.Framework.Enums;
using Viper.Framework.Entities;
using Viper.Framework.Engine;

namespace Viper.Framework.Blocks
{
	/// <summary>
	/// Advance Block Class. Calculate next simulation time and puts the Transaction in the FEC.
	/// </summary>
	public class AdvanceBlock : BlockTransactional, IParseable
	{
		#region Operands
		/// <summary>
		/// Operand A: Mean time increment.
		/// </summary>
		public BlockOperand OperandA
		{
			get;
			set;
		}

		/// <summary>
		/// Operand B: Time half-range or function modifier.
		/// </summary>
		public BlockOperand OperandB
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public AdvanceBlock() : base()
		{
			this.OperandA = null;
			this.OperandB = null;
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public AdvanceBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = null;
			this.OperandB = null;
		}
		#endregion

		#region IParseable Methods
		/// <summary>
		/// Parse Plain Text Block and returns a Viper Advance Block
		/// </summary>
		/// <returns></returns>
		public BlockParseResult Parse()
		{
			try
			{
				if( String.IsNullOrEmpty( this.Text ) )
				{
					OnParseFailed( new ParseEventArgs( BlockNames.ADVANCE, this.Line, String.Empty ) );
					return BlockParseResult.PARSED_ERROR;
				}

				// Get Plain Text Block Parts
				String[] sBlockParts = BlockFactory.GetBlockParts( this.Text );

				// CORRECT SINTAX FORMAT in '[]' optional part: 
				// [NAME] ADVANCE	A,B
				if( sBlockParts[ 0 ].Equals( BlockNames.ADVANCE ) && ( sBlockParts.Length == 2 ) )
				{
					// FORMATS: ADVANCE A / ADVANCE A,B
					
					// NO label and with operands A[,B]
					m_sBlockLabel = String.Empty;

					// ALL OPERANDS: A,B
					String[] operands = sBlockParts[ 1 ].Split( ',' );
					if( operands.Length >= 1 ) this.OperandA = BlockOperand.TranslateOperand( operands[ 0 ] );
					if( operands.Length >= 2 ) this.OperandB = BlockOperand.TranslateOperand( operands[ 1 ] );

					// OPERAND A is REQUIRED
					if( this.OperandA.HasValidValue ) return BlockParseResult.PARSED_OK;
				}
				else if( sBlockParts[ 1 ].Equals( BlockNames.ADVANCE ) && sBlockParts.Length == 3 )
				{
					// FORMAT: NAME ADVANCE A / NAME ADVANCE A,B

					// With Label and with operands A[,B]
					m_sBlockLabel = sBlockParts[ 0 ];

					// ALL OPERANDS: A,B
					String[] operands = sBlockParts[ 2 ].Split( ',' );
					if( operands.Length >= 1 ) this.OperandA = BlockOperand.TranslateOperand( operands[ 0 ] );
					if( operands.Length >= 2 ) this.OperandB = BlockOperand.TranslateOperand( operands[ 1 ] );

					// OPERAND A is REQUIRED
					if( this.OperandA.HasValidValue ) return BlockParseResult.PARSED_OK;
				}
				
				OnParseFailed( new ParseEventArgs( BlockNames.ADVANCE, this.Line, String.Empty ) );
			}
			catch( Exception ex )
			{
				throw new BlockParseException( ex.Message, ex.InnerException, BlockNames.ADVANCE, this.Line );
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
