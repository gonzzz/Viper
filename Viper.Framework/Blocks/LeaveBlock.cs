﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Utils;
using Viper.Framework.Exceptions;
using Viper.Framework.Enums;
using Viper.Framework.Entities;

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
		public LeaveBlock()
			: base()
		{
			this.OperandA = null;
			this.OperandB = null;
			m_oStorageEntity = null;
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public LeaveBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = null;
			this.OperandB = null;
			m_oStorageEntity = null;
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
					if( operands.Length >= 1 ) this.OperandA = BlockOperand.TranslateOperand( operands[ 0 ] );
					if( operands.Length >= 2 ) this.OperandB = BlockOperand.TranslateOperand( operands[ 1 ] );

					// Operand A is required
					if( this.OperandA.HasValidValue ) return BlockParseResult.PARSED_OK;
				}
				else if( sBlockParts[ 1 ].Equals( BlockNames.LEAVE ) && sBlockParts.Length == 3 )
				{
					// FORMATS: NAME LEAVE A / NAME LEAVE A,B
					m_sBlockLabel = sBlockParts[ 0 ];

					String[] operands = sBlockParts[ 2 ].Split( ',' );
					if( operands.Length >= 1 ) this.OperandA = BlockOperand.TranslateOperand( operands[ 0 ] );
					if( operands.Length >= 2 ) this.OperandB = BlockOperand.TranslateOperand( operands[ 1 ] );

					// Operand A is required
					if( this.OperandA.HasValidValue ) return BlockParseResult.PARSED_OK;
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

		#region Process Transaction Methods
		public override BlockProcessResult Process( Transaction oTransaction )
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}