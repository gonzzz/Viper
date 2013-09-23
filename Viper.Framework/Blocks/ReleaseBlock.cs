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
	public class ReleaseBlock : BlockTransactional, IParseable
	{
		#region Operands
		/// <summary>
		/// Operand A: Facility Entity Name or Number.
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
		public ReleaseBlock()
			: base()
		{
			this.OperandA = BlockOperand.EmptyOperand();
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="iLineNumber"></param>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public ReleaseBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = BlockOperand.EmptyOperand();
		}
		#endregion

		#region IParseable Methods
		/// <summary>
		/// Parse Plain Text Block and returns a Viper DoRelease Block
		/// </summary>
		/// <returns></returns>
		public BlockParseResult Parse()
		{
			try
			{
				if( String.IsNullOrEmpty( this.Text ) )
				{
					OnParseFailed( new ParseEventArgs( BlockNames.RELEASE, this.Line, String.Empty ) );
					return BlockParseResult.PARSED_ERROR;
				}

				// Get Plain Text Block Parts
				String[] sBlockParts = BlockFactory.GetBlockParts( this.Text );

				// CORRECT SINTAX FORMAT in '[]' optional part: 
				// [NAME]	RELEASE	A
				if( sBlockParts[ 0 ].Equals( BlockNames.RELEASE ) && sBlockParts.Length == 2 )
				{
					// FORMATS: RELEASE A
					m_sBlockLabel = String.Empty;
					BlockOperand.TranslateOperand( this.OperandA, sBlockParts[ 1 ], true );
					
					// Operand A is required
					if( this.OperandA.HasValidValue ) return BlockParseResult.PARSED_OK;
				}
				else if( sBlockParts[ 1 ].Equals( BlockNames.RELEASE ) && sBlockParts.Length == 3 )
				{
					// FORMATS: NAME RELEASE A
					m_sBlockLabel = sBlockParts[ 0 ];
					BlockOperand.TranslateOperand( this.OperandA, sBlockParts[ 2 ] , true );
					
					// Operand A is required
					if( this.OperandA.HasValidValue ) return BlockParseResult.PARSED_OK;
				}

				OnParseFailed( new ParseEventArgs( BlockNames.RELEASE, this.Line, String.Empty ) );
			}
			catch( Exception ex )
			{
				throw new BlockParseException( ex.Message, ex.InnerException, BlockNames.RELEASE, this.Line );
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
				// Get Facility Entity (by Name, Number or SNA)
				Facility facility = ViperSystem.InstanceModel().GetFacilityFromOperands( oTransaction, this.OperandA );

				if( facility.Owner != oTransaction )
				{
					string strMessage = String.Format( Resources.SyntaxErrorMessagesEN.EXCEPTION_FACILITY_ONLY_OWNER_TX_CAN_RELEASE, this.Line );
					if( ViperSystem.Instance().SystemLanguage == Languages.Spanish )
						strMessage = String.Format( Resources.SyntaxErrorMessagesES.EXCEPTION_FACILITY_ONLY_OWNER_TX_CAN_RELEASE, this.Line );

					throw new BlockProcessException( strMessage, null, BlockNames.RELEASE, this.Line );
				}
				else
				{
					// Common Process
					base.Process( ref oTransaction );

					// Release Facility
					facility.DoRelease( oTransaction );

					if( facility.TransactionCountInPendingChain() > 0 )
					{
						// Remove first transaction from Pending Chain (ordered by priority)
						Transaction transactionFreed = facility.RemoveFirstTransactionFromPendingChain();

						// Add it to the CEC (with nextsystemtime, state and isdelayed updated)
						transactionFreed.NextSystemTime = ViperSystem.Instance().SystemTime;
						transactionFreed.State = TransactionState.WAITING;
						transactionFreed.IsDelayed = false;
						transactionFreed.IsPreempted = false;

						ViperSystem.Instance().InsertTransactionIntoCEC( transactionFreed );
					}
					else if( facility.TransactionCountInDelayChain() > 0 )
					{
						// Remove first transaction from Delay Chain (ordered by priority)
						Transaction transactionFreed = facility.RemoveFirstTransactionFromDelayChain();

						// Add it to the CEC (with nextsystemtime, state and isdelayed updated)
						transactionFreed.NextSystemTime = ViperSystem.Instance().SystemTime;
						transactionFreed.State = TransactionState.WAITING;
						transactionFreed.IsDelayed = false;
						transactionFreed.IsPreempted = false;

						ViperSystem.Instance().InsertTransactionIntoCEC( transactionFreed );
					}

					// Notify Success
					OnProcessSuccess( new ProcessEventArgs( BlockNames.RELEASE, this.Line, String.Empty ) );

					return BlockProcessResult.TRANSACTION_PROCESSED;
				}
			}
			catch( Exception ex )
			{
				// Notify Fail
				OnProcessFailed( new ProcessEventArgs( BlockNames.RELEASE, this.Line, ex.Message ) );

				// Return Exception
				return BlockProcessResult.TRANSACTION_EXCEPTION;
			}
		}
		#endregion
	}
}
