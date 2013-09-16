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
	public class AdvanceBlock : BlockTransactional, IParseable, IProcessable
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
			this.OperandA = BlockOperand.EmptyOperand();
			this.OperandB = BlockOperand.EmptyOperand();
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public AdvanceBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = BlockOperand.EmptyOperand();
			this.OperandB = BlockOperand.EmptyOperand();
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

					if ( operands.Length == 0 || operands.Length > 2 )
					{
						OnParseFailed( new ParseEventArgs( BlockNames.ADVANCE , this.Line , String.Empty ) );
						return BlockParseResult.PARSED_ERROR;
					}

					if ( operands.Length >= 1 ) BlockOperand.TranslateOperand( this.OperandA , operands[ 0 ] , true );
					if ( operands.Length >= 2 ) BlockOperand.TranslateOperand( this.OperandB , operands[ 1 ] );

					// Operand A is required, Operand B optional, both have to get valid values
					if ( this.OperandA.HasValidValue && this.OperandB.HasValidValue )
						return BlockParseResult.PARSED_OK;
				}
				else if( sBlockParts[ 1 ].Equals( BlockNames.ADVANCE ) && sBlockParts.Length == 3 )
				{
					// FORMAT: NAME ADVANCE A / NAME ADVANCE A,B

					// With Label and with operands A[,B]
					m_sBlockLabel = sBlockParts[ 0 ];

					// ALL OPERANDS: A,B
					String[] operands = sBlockParts[ 2 ].Split( ',' );

					if ( operands.Length == 0 || operands.Length > 2 )
					{
						OnParseFailed( new ParseEventArgs( BlockNames.ADVANCE , this.Line , String.Empty ) );
						return BlockParseResult.PARSED_ERROR;
					}

					if( operands.Length >= 1 ) BlockOperand.TranslateOperand( this.OperandA, operands[ 0 ], true );
					if( operands.Length >= 2 ) BlockOperand.TranslateOperand( this.OperandB, operands[ 1 ] );

					// Operand A is required, Operand B optional, both have to get valid values
					if ( this.OperandA.HasValidValue && this.OperandB.HasValidValue )
						return BlockParseResult.PARSED_OK;
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

		#region IProcessable Implementation
		public override BlockProcessResult Process( ref Transaction oTransaction )
		{
			try
			{
				int iNextTime = CalculateNextTime();

				if( iNextTime < ViperSystem.Instance().SystemTime )
				{
					string strMessage = Resources.SyntaxErrorMessagesEN.EXCEPTION_ADVANCE_TRANSACTION_NEXT_TIME;
					if( ViperSystem.Instance().SystemLanguage == Languages.Spanish ) 
						strMessage = Resources.SyntaxErrorMessagesES.EXCEPTION_ADVANCE_TRANSACTION_NEXT_TIME;

					throw new BlockProcessException( strMessage, null, BlockNames.ADVANCE, this.Line );
				}

				// We do common process here
				base.Process( ref oTransaction );

				// Update Transaction Time and ScanStatus (transaction will remain in FEC until next time equal system time)
				oTransaction.NextSystemTime = iNextTime;
				oTransaction.State = TransactionState.PASSIVE;
				oTransaction.ScanStatus = true;

				// Remove Transaction From CEC
				ViperSystem.Instance().RemoveTransactionFromCEC( oTransaction );

				// Insert Transaction Into the FEC
				ViperSystem.Instance().InsertTransactionIntoFEC( oTransaction );
				
				// Notify Success
				OnProcessSuccess( new ProcessEventArgs( BlockNames.ADVANCE , this.Line , String.Empty ) );

				// Return Success
				return BlockProcessResult.TRANSACTION_PROCESSED;
			}
			catch( Exception ex )
			{
				// Notify Fail
				OnProcessFailed( new ProcessEventArgs( BlockNames.ADVANCE , this.Line , ex.Message ) );

				// Return Exception
				return BlockProcessResult.TRANSACTION_EXCEPTION;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private int CalculateNextTime()
		{
			// Calculate Transaction Next Time
			int iNextTime = Constants.DEFAULT_ZERO_VALUE;
			int iMean = Constants.DEFAULT_ZERO_VALUE; // From Operand A
			int iDesviation = Constants.DEFAULT_ZERO_VALUE; // From Operand B

			if( !this.OperandA.IsEmpty )
			{
				iMean = BlockOperand.GetIntValueFromOperand( this.OperandA, BlockNames.ADVANCE, this.Line );
			}

			if( !this.OperandB.IsEmpty )
			{
				iDesviation = BlockOperand.GetIntValueFromOperand( this.OperandB, BlockNames.ADVANCE, this.Line );
			}

			// Calculate Next Transaction Time with Mean, Desviation
			iNextTime = ViperSystem.Instance().SystemTime;
			iNextTime += ( iMean + RandomGenerator.Instance().GenerateRandomWithDesviation( 0, iDesviation ) );

			return iNextTime;
		}
		#endregion
	}
}
