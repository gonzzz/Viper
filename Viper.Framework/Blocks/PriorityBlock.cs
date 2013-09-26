using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Enums;
using Viper.Framework.Utils;
using Viper.Framework.Exceptions;
using Viper.Framework.Entities;

namespace Viper.Framework.Blocks
{
	/// <summary>
	/// 
	/// </summary>
	public class PriorityBlock : BlockTransactional , IParseable
	{
		#region Operands
		/// <summary>
		/// Operand A: Priority to be set to Current Transaction
		/// </summary>
		public BlockOperand OperandA
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public PriorityBlock() : base()
		{
			this.OperandA = BlockOperand.EmptyOperand();
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="iLineNumber"></param>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public PriorityBlock( int iLineNumber , int iBlockNumber , String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = BlockOperand.EmptyOperand();
		}
		#endregion

		#region IParseable Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public BlockParseResult Parse()
		{
			try
			{
				if ( String.IsNullOrEmpty( this.Text ) )
				{
					OnParseFailed( new ParseEventArgs( BlockNames.PRIORITY , this.Line , String.Empty ) );
					return BlockParseResult.PARSED_ERROR;
				}

				// Get Plain Text Block Parts
				String[] sBlockParts = BlockFactory.GetBlockParts( this.Text );

				// CORRECT SINTAX FORMAT in '[]' optional part: 
				// [NAME] PRIORITY	A
				if ( sBlockParts[ 0 ].Equals( BlockNames.PRIORITY ) && ( sBlockParts.Length == 2 ) )
				{
					// FORMAT: PRIORITY A

					// NO label and with operand A
					m_sBlockLabel = String.Empty;

					// ALL OPERANDS: A
					String operand = sBlockParts[ 1 ];
					BlockOperand.TranslateOperand( this.OperandA , operand , true );

					// Operand A is required, Operand B optional, both have to get valid values
					if ( this.OperandA.HasValidValue ) return BlockParseResult.PARSED_OK;
				}
				else if ( sBlockParts[ 1 ].Equals( BlockNames.PRIORITY ) && sBlockParts.Length == 3 )
				{
					// FORMAT: NAME PRIORITY A

					// With Label and with operand A
					m_sBlockLabel = sBlockParts[ 0 ];

					// ALL OPERANDS: A
					String operand = sBlockParts[ 2 ];

					BlockOperand.TranslateOperand( this.OperandA , operand , true );
					
					// Operand A is required, Operand B optional, both have to get valid values
					if ( this.OperandA.HasValidValue ) return BlockParseResult.PARSED_OK;
				}

				OnParseFailed( new ParseEventArgs( BlockNames.PRIORITY , this.Line , String.Empty ) );
			}
			catch ( Exception ex )
			{
				throw new BlockParseException( ex.Message , ex.InnerException , BlockNames.PRIORITY , this.Line );
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

		#region IProcessable Implementation
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <returns></returns>
		public override BlockProcessResult Process( ref Transaction oTransaction )
		{
			try
			{
				// Calculate Priority using operand A
				int iNewPriority = CalculatePriorityFromOperand();

				// We do common process here
				base.Process( ref oTransaction );

				// Update Transaction Priority
				oTransaction.Priority = iNewPriority;

				// Notify Success
				OnProcessSuccess( new ProcessEventArgs( BlockNames.PRIORITY , this.Line , String.Empty ) );

				// Return Success
				return BlockProcessResult.TRANSACTION_PROCESSED;
			}
			catch( Exception ex )
			{
				// Notify Fail
				OnProcessFailed( new ProcessEventArgs( BlockNames.PRIORITY , this.Line , ex.Message ) );

				// Return Exception
				return BlockProcessResult.TRANSACTION_EXCEPTION;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private int CalculatePriorityFromOperand()
		{
			// Calculate New Transaction Priority
			int iNewPriority = Constants.DEFAULT_ZERO_VALUE;

			if ( !this.OperandA.IsEmpty )
			{
				if ( this.OperandA.IsPosInteger )
				{
					iNewPriority = this.OperandA.PosInteger;
				}
				else if ( this.OperandA.IsName )
				{
					// TODO: PRIORITY - USE NAME TO GET VALUE
					throw new NotImplementedException();
				}
				else if ( this.OperandA.IsSNA )
				{
					// TODO: PRIORITY - USE SNA TO GET VALUE
					throw new NotImplementedException();
				}
			}

			return iNewPriority;
		}
		#endregion
	}
}
