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
	public class LeaveBlock : BlockTransactional, IParseable
	{
		#region Operands
		/// <summary>
		/// Operand A: Storage Entity Name or Number.
		/// </summary>
		public BlockOperand OperandA
		{
			get;
			set;
		}

		/// <summary>
		/// Operand B: Number of Units by which to increase available storage capacity.
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
		public LeaveBlock()
			: base()
		{
			this.OperandA = BlockOperand.EmptyOperand();
			this.OperandB = BlockOperand.EmptyOperand();
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public LeaveBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = BlockOperand.EmptyOperand();
			this.OperandB = BlockOperand.EmptyOperand();
		}
		#endregion

		#region IParseable Methods
		/// <summary>
		/// Parse Plain Text Block and returns a Viper Leave Block
		/// </summary>
		/// <returns></returns>
		public BlockParseResult Parse()
		{
			try
			{
				if( String.IsNullOrEmpty( this.Text ) )
				{
					OnParseFailed( new ParseEventArgs( BlockNames.LEAVE, this.Line, String.Empty ) );
					return BlockParseResult.PARSED_ERROR;
				}

				// Get Plain Text Block Parts
				String[] sBlockParts = BlockFactory.GetBlockParts( this.Text );

				// CORRECT SINTAX FORMAT in '[]' optional part: 
				// [NAME]	LEAVE	A[,B]
				if( sBlockParts[ 0 ].Equals( BlockNames.LEAVE ) && sBlockParts.Length == 2 )
				{
					// FORMATS: LEAVE A / LEAVE A,B
					m_sBlockLabel = String.Empty;

					String[] operands = sBlockParts[ 1 ].Split( ',' );

					if ( operands.Length == 0 || operands.Length > 2 )
					{
						OnParseFailed( new ParseEventArgs( BlockNames.LEAVE , this.Line , String.Empty ) );
						return BlockParseResult.PARSED_ERROR;
					}

					if ( operands.Length >= 1 ) BlockOperand.TranslateOperand( this.OperandA , operands[ 0 ] , true );
					if ( operands.Length >= 2 ) BlockOperand.TranslateOperand( this.OperandB , operands[ 1 ] );

					// Operand A is required, Operand B optional, both have to get valid values
					if ( this.OperandA.HasValidValue && this.OperandB.HasValidValue )
						return BlockParseResult.PARSED_OK;
				}
				else if( sBlockParts[ 1 ].Equals( BlockNames.LEAVE ) && sBlockParts.Length == 3 )
				{
					// FORMATS: NAME LEAVE A / NAME LEAVE A,B
					m_sBlockLabel = sBlockParts[ 0 ];

					String[] operands = sBlockParts[ 2 ].Split( ',' );

					if ( operands.Length == 0 || operands.Length > 2 )
					{
						OnParseFailed( new ParseEventArgs( BlockNames.LEAVE , this.Line , String.Empty ) );
						return BlockParseResult.PARSED_ERROR;
					}

					if ( operands.Length >= 1 ) BlockOperand.TranslateOperand( this.OperandA , operands[ 0 ] , true );
					if ( operands.Length >= 2 ) BlockOperand.TranslateOperand( this.OperandB , operands[ 1 ] );

					// Operand A is required, Operand B optional, both have to get valid values
					if ( this.OperandA.HasValidValue && this.OperandB.HasValidValue )
						return BlockParseResult.PARSED_OK;
				}

				OnParseFailed( new ParseEventArgs( BlockNames.LEAVE, this.Line, String.Empty ) );
			}
			catch( Exception ex )
			{
				throw new BlockParseException( ex.Message, ex.InnerException, BlockNames.LEAVE, this.Line );
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
				// Get Storage Entity (by Name, Number or SNA)
				Storage storage = ViperSystem.InstanceModel().GetStorageFromOperands( oTransaction, this.OperandA );
				if( storage == null )
				{
					string strMessage = String.Format( Resources.SyntaxErrorMessagesEN.EXCEPTION_STORAGE_UNAVAILABLE, this.Line );
					if( ViperSystem.Instance().SystemLanguage == Languages.Spanish )
						strMessage = String.Format( Resources.SyntaxErrorMessagesES.EXCEPTION_STORAGE_UNAVAILABLE, this.Line );

					throw new BlockProcessException( strMessage, null, BlockNames.LEAVE, this.Line );
				}

				// Get Amount To Leave
				int iAmountToLeave = ViperSystem.InstanceModel().GetAmountToOccupyOrLeaveInStorage( oTransaction, this.OperandB );

				if( storage.IsLeaveAvailable( iAmountToLeave ) )
				{
					// Common Process
					base.Process( ref oTransaction );

					// Remove transaction from Storage
					storage.DoLeave( oTransaction, iAmountToLeave );

					if( storage.TransactionCountInDelayChain() > 0 )
					{
						for( int i = 0 ; i < iAmountToLeave ; i++ )
						{
							// Remove first transaction from Delay Chain (ordered by priority)
							Transaction transactionFreed = storage.RemoveFirstTransactionFromDelayChain();

							// Add it to the CEC (with nextsystemtime, state and isdelayed updated)
							transactionFreed.NextSystemTime = ViperSystem.Instance().SystemTime;
							transactionFreed.State = TransactionState.WAITING;
							transactionFreed.IsDelayed = false;

							ViperSystem.Instance().InsertTransactionIntoCEC( transactionFreed );
						}
					}

					// Notify Success
					OnProcessSuccess( new ProcessEventArgs( BlockNames.LEAVE, this.Line, String.Empty ) );

					return BlockProcessResult.TRANSACTION_PROCESSED;
				}
				else
				{
					string strMessage = String.Format( Resources.SyntaxErrorMessagesEN.EXCEPTION_STORAGE_NOT_NEGATIVE, this.Line );
					if( ViperSystem.Instance().SystemLanguage == Languages.Spanish )
						strMessage = String.Format( Resources.SyntaxErrorMessagesES.EXCEPTION_STORAGE_NOT_NEGATIVE, this.Line );

					throw new BlockProcessException( strMessage, null, BlockNames.LEAVE, this.Line );
				}
			}
			catch ( Exception ex )
			{
				// Notify Fail
				OnProcessFailed( new ProcessEventArgs( BlockNames.LEAVE , this.Line , ex.Message ) );

				// Return Exception
				return BlockProcessResult.TRANSACTION_EXCEPTION;
			}
		}
		#endregion
	}
}
