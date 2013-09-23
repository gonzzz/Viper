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
	public class EnterBlock : BlockTransactional, IParseable
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
		/// Operand B: Number of Units by which to decrease available storage capacity.
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
		public EnterBlock()
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
		public EnterBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = BlockOperand.EmptyOperand();
			this.OperandB = BlockOperand.EmptyOperand();
		}
		#endregion

		#region IParseable Methods
		/// <summary>
		/// Parse Plain Text Block and returns a Viper DoEnter Block
		/// </summary>
		/// <returns></returns>
		public BlockParseResult Parse()
		{
			try
			{
				if( String.IsNullOrEmpty( this.Text ) )
				{
					OnParseFailed( new ParseEventArgs( BlockNames.ENTER, this.Line, String.Empty ) );
					return BlockParseResult.PARSED_ERROR;
				}

				// Get Plain Text Block Parts
				String[] sBlockParts = BlockFactory.GetBlockParts( this.Text );

				// CORRECT SINTAX FORMAT in '[]' optional part: 
				// [NAME]	ENTER	A[,B]
				if( sBlockParts[ 0 ].Equals( BlockNames.ENTER ) && sBlockParts.Length == 2 )
				{
					// FORMATS: ENTER A / ENTER A,B
					m_sBlockLabel = String.Empty;

					String[] operands = sBlockParts[ 1 ].Split( ',' );
					
					if ( operands.Length == 0 || operands.Length > 2 )
					{
						OnParseFailed( new ParseEventArgs( BlockNames.ENTER , this.Line , String.Empty ) );
						return BlockParseResult.PARSED_ERROR;
					}

					if ( operands.Length >= 1 ) BlockOperand.TranslateOperand( this.OperandA, operands[ 0 ], true );
					if ( operands.Length >= 2 ) BlockOperand.TranslateOperand( this.OperandB, operands[ 1 ] );

					// Operand A is required, Operand B optional, both have to get valid values
					if ( this.OperandA.HasValidValue && this.OperandB.HasValidValue )
						return BlockParseResult.PARSED_OK;
				}
				else if( sBlockParts[ 1 ].Equals( BlockNames.ENTER ) && sBlockParts.Length == 3 )
				{
					// FORMATS: NAME ENTER A / NAME ENTER A,B
					m_sBlockLabel = sBlockParts[ 0 ];

					String[] operands = sBlockParts[ 2 ].Split( ',' );

					if ( operands.Length == 0 || operands.Length > 2 )
					{
						OnParseFailed( new ParseEventArgs( BlockNames.ENTER , this.Line , String.Empty ) );
						return BlockParseResult.PARSED_ERROR;
					}

					if ( operands.Length >= 1 ) BlockOperand.TranslateOperand( this.OperandA , operands[ 0 ] , true );
					if ( operands.Length >= 2 ) BlockOperand.TranslateOperand( this.OperandB , operands[ 1 ] );

					// Operand A is required, Operand B optional, both have to get valid values
					if ( this.OperandA.HasValidValue && this.OperandB.HasValidValue )
						return BlockParseResult.PARSED_OK;
				}

				OnParseFailed( new ParseEventArgs( BlockNames.ENTER, this.Line, String.Empty ) );
			}
			catch( Exception ex )
			{
				throw new BlockParseException( ex.Message, ex.InnerException, BlockNames.ENTER, this.Line );
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <returns></returns>
		public override BlockProcessResult Process( ref Transaction oTransaction )
		{
			try
			{
				// Get Storage Entity (by Name, Number or SNA)
				Storage storage = ViperSystem.InstanceModel().GetStorageFromOperands( oTransaction, this.OperandA );
				if( storage == null )
				{
					string strMessage = Resources.SyntaxErrorMessagesEN.EXCEPTION_STORAGE_UNAVAILABLE;
					if( ViperSystem.Instance().SystemLanguage == Languages.Spanish )
						strMessage = Resources.SyntaxErrorMessagesES.EXCEPTION_STORAGE_UNAVAILABLE;

					throw new BlockProcessException( strMessage, null, BlockNames.ENTER, this.Line );
				}

				// Get Amount To Occupy
				int iAmountToOccupy = ViperSystem.InstanceModel().GetAmountToOccupyOrLeaveInStorage( oTransaction, this.OperandB );

				if( storage.IsEnterAvailable( iAmountToOccupy ) )
				{
					// Common Process
					base.Process( ref oTransaction );

					// Add Transaction to Storage
					storage.DoEnter( oTransaction, iAmountToOccupy );

					// Notify Success
					OnProcessSuccess( new ProcessEventArgs( BlockNames.ENTER, this.Line, String.Empty ) );

					return BlockProcessResult.TRANSACTION_PROCESSED;
				}
				else
				{
					// Update Transaction
					oTransaction.IsDelayed = true;
					oTransaction.State = TransactionState.PASSIVE; // will wait in Storage Delay Chain

					// Add Transaction into Storage Delay Chain
					storage.AddTransactionIntoDelayChain( oTransaction );

					// Remove from the CEC
					ViperSystem.Instance().RemoveTransactionFromCEC( oTransaction );

					// Notify Failed
					OnProcessFailed( new ProcessEventArgs( BlockNames.ENTER, this.Line, String.Empty ) );

					return BlockProcessResult.TRANSACTION_ENTRY_REFUSED;
				}
			}
			catch( Exception ex ) 
			{
				// Notify Fail
				OnProcessFailed( new ProcessEventArgs( BlockNames.ENTER , this.Line , ex.Message ) );

				// Return Exception
				return BlockProcessResult.TRANSACTION_EXCEPTION;
			}
		}
		#endregion
	}
}
