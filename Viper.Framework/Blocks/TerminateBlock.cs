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
	public class TerminateBlock : BlockTransactional, IParseable
	{
		#region Operands
		/// <summary>
		/// Operand A: Termination Count Decrement
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
		public TerminateBlock() : base()
		{
			this.OperandA = null;
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public TerminateBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = null;
		}
		#endregion

		#region IParseable Methods
		/// <summary>
		/// Parse Plain Text Block and returns a Viper Terminate Block
		/// </summary>
		/// <returns></returns>
		public BlockParseResult Parse()
		{
			try
			{
				if( String.IsNullOrEmpty( this.Text ) )
				{
					OnParseFailed( new ParseEventArgs( BlockNames.TERMINATE, this.Line, String.Empty ) );
					return BlockParseResult.PARSED_ERROR;
				}

				// Get Plain Text Block Parts
				String[] sBlockParts = BlockFactory.GetBlockParts( this.Text );

				// CORRECT SINTAX FORMAT in '[]' optional part: 
				// [NAME]	TERMINATE	[A]
				if( sBlockParts[ 0 ].Equals( BlockNames.TERMINATE ) && ( sBlockParts.Length == 1 || sBlockParts.Length == 2 ) )
				{
					// FORMATS: TERMINATE / TERMINATE A
					if( sBlockParts.Length == 1 )
					{
						// NO label and NO operand A
						m_sBlockLabel = String.Empty;
						this.OperandA = null;
					}
					else
					{
						// NO label and with operand A
						m_sBlockLabel = String.Empty;
						this.OperandA = BlockOperand.TranslateOperand( sBlockParts[ 1 ] );
					}

					return BlockParseResult.PARSED_OK;
				}
				else if( sBlockParts[ 1 ].Equals( BlockNames.TERMINATE ) && ( sBlockParts.Length == 2 || sBlockParts.Length == 3 ) )
				{
					// FORMATS: NAME TERMINATE / NAME TERMINATE A
					if( sBlockParts.Length == 2 )
					{
						// With Label and NO operand A
						m_sBlockLabel = sBlockParts[ 0 ];
						OperandA = null;
					}
					else
					{
						// With Label and operand A
						m_sBlockLabel = sBlockParts[ 0 ];
						this.OperandA = BlockOperand.TranslateOperand( sBlockParts[ 2 ] );
					}

					return BlockParseResult.PARSED_OK;
				}
				
				OnParseFailed( new ParseEventArgs( BlockNames.TERMINATE, this.Line, String.Empty ) );
			}
			catch( Exception ex )
			{
				throw new BlockParseException( ex.Message, ex.InnerException, BlockNames.TERMINATE, this.Line );
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
				int iTerminationAmount = GetTerminationAmount();

				// We do common process here
				base.Process( ref oTransaction );

				if( iTerminationAmount > Constants.DEFAULT_ZERO_VALUE ) {
					ViperSystem.Instance().DecrementTerminationCounter( iTerminationAmount );
				}

				oTransaction.State = TransactionState.TERMINATED;

				// Remove Transaction From CEC
				ViperSystem.Instance().RemoveTransactionFromCEC( oTransaction );

				// Notify Success
				OnProcessSuccess( new ProcessEventArgs( BlockNames.TERMINATE , this.Line , String.Empty ) );

				return BlockProcessResult.TRANSACTION_PROCESSED;
			}
			catch( Exception ex )
			{
				// Notify Fail
				OnProcessFailed( new ProcessEventArgs( BlockNames.TERMINATE , this.Line , ex.Message ) );

				// Return Exception
				return BlockProcessResult.TRANSACTION_EXCEPTION;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private int GetTerminationAmount()
		{
			int iTerminationAmount = Constants.DEFAULT_ZERO_VALUE; // From Operand A

			if ( !this.OperandA.IsEmpty )
			{
				if( this.OperandA.IsPosInteger )
				{
					iTerminationAmount = this.OperandA.PosInteger;
				}
				else if( this.OperandA.IsName )
				{
					// TODO: TERMINATE NUM - USE NAME TO GET VALUE
					throw new NotImplementedException();
				}
				else if( this.OperandA.IsSNA )
				{
					// TODO: TERMINATE NUM - USE SNA TO GET VALUE
					throw new NotImplementedException();
				}
			}

			return iTerminationAmount;
		}
		#endregion
	}
}
