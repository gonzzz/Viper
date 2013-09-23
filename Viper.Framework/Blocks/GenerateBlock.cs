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
			this.OperandA = BlockOperand.EmptyOperand();
			this.OperandB = BlockOperand.EmptyOperand();
			this.OperandC = BlockOperand.EmptyOperand();
			this.OperandD = BlockOperand.EmptyOperand();
			this.OperandE = BlockOperand.EmptyOperand();
		}

		/// <summary>
		/// Constructor With parameters
		/// </summary>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public GenerateBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = BlockOperand.EmptyOperand();
			this.OperandB = BlockOperand.EmptyOperand();
			this.OperandC = BlockOperand.EmptyOperand();
			this.OperandD = BlockOperand.EmptyOperand();
			this.OperandE = BlockOperand.EmptyOperand();
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

					if( operands.Length == 0 || operands.Length > 5 ) {
						OnParseFailed( new ParseEventArgs( BlockNames.GENERATE , this.Line , String.Empty ) );
						return BlockParseResult.PARSED_ERROR;
					}

					if( operands.Length >= 1 ) BlockOperand.TranslateOperand( this.OperandA, operands[ 0 ], true );
					if( operands.Length >= 2 ) BlockOperand.TranslateOperand( this.OperandB, operands[ 1 ] );
					if( operands.Length >= 3 ) BlockOperand.TranslateOperand( this.OperandC, operands[ 2 ] );
					if( operands.Length >= 4 ) BlockOperand.TranslateOperand( this.OperandD, operands[ 3 ], true );
					if( operands.Length >= 5 ) BlockOperand.TranslateOperand( this.OperandE, operands[ 4 ] );

					if( this.OperandA.HasValidValue && this.OperandB.HasValidValue && 
						this.OperandC.HasValidValue && this.OperandD.HasValidValue && 
						this.OperandE.HasValidValue ) {
						return BlockParseResult.PARSED_OK;
					}
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

		#region IProcessable methods
		public override BlockProcessResult Process( ref Transaction oTransaction )
		{
			try
			{
				// Calculate Transaction Arrival Time
				int iArrivalTime = CalculateArrivalTime(); // From Operands A,B,C
				int iMaxTransactionsToGenerate = GetMaxTransactionsToGenerate(); // From Operand D
				int iTransactionPriority = GetTransactionPriority(); // From Operand E

				if( iArrivalTime < ViperSystem.Instance().SystemTime )
				{
					string strMessage = Resources.SyntaxErrorMessagesEN.EXCEPTION_GENERATE_TRANSACTION_ARRIVAL_TIME;
					if( ViperSystem.Instance().SystemLanguage == Languages.Spanish )
						strMessage = Resources.SyntaxErrorMessagesES.EXCEPTION_GENERATE_TRANSACTION_ARRIVAL_TIME;

					throw new BlockProcessException( strMessage, null, BlockNames.GENERATE, this.Line );
				}

				// Verify MaxTransactionsToGenerate has a value and it is not exceeded
				if( iMaxTransactionsToGenerate > Constants.DEFAULT_ZERO_VALUE &&
					this.m_iEntryCount > iMaxTransactionsToGenerate )
				{
					// Notify Fail
					OnProcessFailed( new ProcessEventArgs( BlockNames.GENERATE , this.Line , String.Empty ) );

					// Return Fail
					return BlockProcessResult.TRANSACTION_ENTRY_REFUSED;
				}

				// We do common process here
				base.Process( ref oTransaction );

				// Update Transaction: NextSystemTime, State and optionaly its Priority
				oTransaction.NextSystemTime = iArrivalTime;
				if ( iTransactionPriority > Constants.DEFAULT_ZERO_VALUE ) oTransaction.Priority = iTransactionPriority;

				// Put Transaction in the FEC
				ViperSystem.Instance().InsertTransactionIntoFEC( oTransaction );

				// Notify Success
				OnProcessSuccess( new ProcessEventArgs( BlockNames.GENERATE , this.Line , String.Empty ) );

				// Return Success
				return BlockProcessResult.TRANSACTION_PROCESSED;
			}
			catch( Exception ex ) 
			{
				// Notify Fail
				OnProcessFailed( new ProcessEventArgs( BlockNames.GENERATE , this.Line , ex.Message ) );

				// Return Exception
				return BlockProcessResult.TRANSACTION_EXCEPTION;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private int CalculateArrivalTime()
		{
			int iArrivalTime = Constants.DEFAULT_ZERO_VALUE;
			int iMean = Constants.DEFAULT_ZERO_VALUE; // From Operand A
			int iDesviation = Constants.DEFAULT_ZERO_VALUE; // From Operand B
			int iDelayForFirstTransaction = Constants.DEFAULT_ZERO_VALUE; // From Operand C

			if( !this.OperandA.IsEmpty )
			{
				if( this.OperandA.IsPosInteger )
				{
					iMean = this.OperandA.PosInteger;
				}
				else if( this.OperandA.IsName )
				{
					// TODO: GENERATE MEAN - USE NAME TO GET VALUE
					throw new NotImplementedException();
				}
				else if( this.OperandA.IsSNA )
				{
					// TODO: GENERATE MEAN - USE SNA TO GET VALUE
					throw new NotImplementedException();
				}
			}

			if( !this.OperandB.IsEmpty )
			{
				if( this.OperandB.IsPosInteger )
				{
					iDesviation = this.OperandB.PosInteger;
				}
				else if( this.OperandB.IsName )
				{
					// TODO: GENERATE DESVIATION - USE NAME TO GET VALUE
					throw new NotImplementedException();
				}
				else if( this.OperandB.IsSNA )
				{
					// TODO: GENERATE DESVIATION - USE SNA TO GET VALUE
					throw new NotImplementedException();
				}
			}

			if( !this.OperandC.IsEmpty )
			{
				if( this.OperandC.IsPosInteger )
				{
					iDelayForFirstTransaction = this.OperandC.PosInteger;
				}
				else if( this.OperandB.IsName )
				{
					// TODO: GENERATE DELAY FIRST TX - USE NAME TO GET VALUE
					throw new NotImplementedException();
				}
				else if( this.OperandB.IsSNA )
				{
					// TODO: GENERATE DELAY FIRST TX - USE SNA TO GET VALUE
					throw new NotImplementedException();
				}
			}

			// Calculate Arrival Time with Mean, Desviation and DelayForFirst (optional)
			iArrivalTime = ViperSystem.Instance().SystemTime;
			if(	this.m_iEntryCount == Constants.DEFAULT_ZERO_VALUE &&
				iDelayForFirstTransaction > Constants.DEFAULT_ZERO_VALUE )
			{
				iArrivalTime += iDelayForFirstTransaction;
			}
			iArrivalTime += ( iMean + RandomGenerator.Instance().GenerateRandomWithDesviation( 0, iDesviation ) );

			return iArrivalTime;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private int GetMaxTransactionsToGenerate()
		{
			int iMaxTransactionsToGenerate = Int32.MaxValue;
			if( !this.OperandD.IsEmpty )
			{
				if( this.OperandD.IsPosInteger )
				{
					iMaxTransactionsToGenerate = this.OperandD.PosInteger;
				}
				else if( this.OperandD.IsName )
				{
					// TODO: GENERATE MAX TRANSACTIONS - USE NAME TO GET VALUE
					throw new NotImplementedException();
				}
				else if( this.OperandD.IsSNA )
				{
					// TODO: GENERATE MAX TRANSACTIONS - USE SNA TO GET VALUE
					throw new NotImplementedException();
				}

			}
			return iMaxTransactionsToGenerate;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private int GetTransactionPriority()
		{
			int iTransactionPriority = Constants.DEFAULT_ZERO_VALUE;
			if( !this.OperandE.IsEmpty )
			{
				if( this.OperandE.IsPosInteger )
				{
					iTransactionPriority = this.OperandE.PosInteger;
				}
				else if( this.OperandE.IsName )
				{
					// TODO: GENERATE TX PRIORITY - USE NAME TO GET VALUE
					throw new NotImplementedException();
				}
				else if( this.OperandE.IsSNA )
				{
					// TODO: GENERATE TX PRIORITY - USE SNA TO GET VALUE
					throw new NotImplementedException();
				}
			}
			return iTransactionPriority;
		}
		#endregion
	}
}
