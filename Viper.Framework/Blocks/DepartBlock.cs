﻿using System;
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
	public class DepartBlock : BlockTransactional, IParseable
	{
		#region Operands
		/// <summary>
		/// Operand A: Queue Entity Name or Number.
		/// </summary>
		public BlockOperand OperandA
		{
			get;
			set;
		}

		/// <summary>
		/// Operand B: Number of units by which to decrease queue entity content.
		/// </summary>
		public BlockOperand OperandB
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructors
		/// </summary>
		public DepartBlock()
			: base()
		{
			this.OperandA = BlockOperand.EmptyOperand();
			this.OperandB = BlockOperand.EmptyOperand();
		}

		/// <summary>
		/// Constructors with parameters
		/// </summary>
		/// <param name="iLineNumber"></param>
		/// <param name="iBlockNumber"></param>
		/// <param name="sPlainTextBlock"></param>
		public DepartBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = BlockOperand.EmptyOperand();
			this.OperandB = BlockOperand.EmptyOperand();
		}
		#endregion

		#region IParseable Members
		/// <summary>
		/// Parse Plain Text Block and returns a Viper Depart Block
		/// </summary>
		/// <returns></returns>
		public BlockParseResult Parse()
		{
			try
			{
				if( String.IsNullOrEmpty( this.Text ) )
				{
					OnParseFailed( new ParseEventArgs( BlockNames.DEPART, this.Line, String.Empty ) );
					return BlockParseResult.PARSED_ERROR;
				}

				// Get Plain Text Block Parts
				String[] sBlockParts = BlockFactory.GetBlockParts( this.Text );

				// CORRECT SINTAX FORMAT in '[]' optional part: 
				// [NAME]	DEPART	A[,B]
				if( sBlockParts[ 0 ].Equals( BlockNames.DEPART ) && sBlockParts.Length == 2 )
				{
					// FORMATS: DEPART A / DEPART A,B
					m_sBlockLabel = String.Empty;

					String[] operands = sBlockParts[ 1 ].Split( ',' );

					if ( operands.Length == 0 || operands.Length > 2 )
					{
						OnParseFailed( new ParseEventArgs( BlockNames.DEPART , this.Line , String.Empty ) );
						return BlockParseResult.PARSED_ERROR;
					}

					if( operands.Length >= 1 ) BlockOperand.TranslateOperand( this.OperandA, operands[ 0 ], true );
					if( operands.Length >= 2 ) BlockOperand.TranslateOperand( this.OperandB, operands[ 1 ] );

					// Operand A is required
					if( this.OperandA.HasValidValue && this.OperandB.HasValidValue ) return BlockParseResult.PARSED_OK;
				}
				else if( sBlockParts[ 1 ].Equals( BlockNames.DEPART ) && sBlockParts.Length == 3 )
				{
					// FORMATS: NAME DEPART A / NAME DEPART A,B
					m_sBlockLabel = sBlockParts[ 0 ];

					String[] operands = sBlockParts[ 2 ].Split( ',' );

					if ( operands.Length == 0 || operands.Length > 2 )
					{
						OnParseFailed( new ParseEventArgs( BlockNames.DEPART , this.Line , String.Empty ) );
						return BlockParseResult.PARSED_ERROR;
					}

					if ( operands.Length >= 1 ) BlockOperand.TranslateOperand( this.OperandA , operands[ 0 ], true );
					if ( operands.Length >= 2 ) BlockOperand.TranslateOperand( this.OperandB , operands[ 1 ] );

					// Operand A is required
					if ( this.OperandA.HasValidValue && this.OperandB.HasValidValue ) return BlockParseResult.PARSED_OK;
				}

				OnParseFailed( new ParseEventArgs( BlockNames.DEPART, this.Line, String.Empty ) );
			}
			catch( Exception ex )
			{
				throw new BlockParseException( ex.Message, ex.InnerException, BlockNames.DEPART, this.Line );
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
				// Get Queue Entity (by Name, Number or SNA)
				Queue queue = ViperSystem.InstanceModel().GetQueueFromOperands( oTransaction, this.OperandA );

				// Get Amount To Occupy
				int iAmountToDequeue = ViperSystem.InstanceModel().GetAmountToQueueOrDequeueFromQueue( oTransaction, this.OperandB );

				// Common Process
				base.Process( ref oTransaction );

				// Add Transaction to Storage
				queue.DoDepart( oTransaction, iAmountToDequeue );

				// Notify Success
				OnProcessSuccess( new ProcessEventArgs( BlockNames.DEPART, this.Line, String.Empty ) );

				return BlockProcessResult.TRANSACTION_PROCESSED;
			}
			catch( Exception ex )
			{
				// Notify Fail
				OnProcessFailed( new ProcessEventArgs( BlockNames.DEPART, this.Line, ex.Message ) );

				// Return Exception
				return BlockProcessResult.TRANSACTION_EXCEPTION;
			}
		}
		#endregion
	}
}
