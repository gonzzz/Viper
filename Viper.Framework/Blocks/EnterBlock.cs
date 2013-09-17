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

		#region Entity Member
		/// <summary>
		/// The Storage Entity this Block is related to
		/// </summary>
		private Storage m_oStorageEntity;
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
			m_oStorageEntity = null;
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
			m_oStorageEntity = null;
		}
		#endregion

		#region IParseable Methods
		/// <summary>
		/// Parse Plain Text Block and returns a Viper Enter Block
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

		#region Entity Methods
		/// <summary>
		/// Attach the Storage Entity to the Block
		/// </summary>
		/// <param name="oStorage"></param>
		public void AttachStorage( Storage oStorage )
		{
			m_oStorageEntity = oStorage;
		}

		/// <summary>
		/// Detachs the Storage Entity from the Block
		/// </summary>
		public void DetachStorage()
		{
			m_oStorageEntity = null;
		}
		#endregion

		#region IProcessable Implementation
		public override BlockProcessResult Process( ref Transaction oTransaction )
		{
			try
			{
				base.Process( ref oTransaction );

				// Notify Success
				OnProcessSuccess( new ProcessEventArgs( BlockNames.ENTER , this.Line , String.Empty ) );

				return BlockProcessResult.TRANSACTION_PROCESSED;
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
