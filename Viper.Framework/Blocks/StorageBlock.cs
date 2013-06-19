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
	/// <summary>
	/// 
	/// </summary>
	public class StorageBlock : Block, IParseable
	{
		#region Operands
		/// <summary>
		/// Operand A: Total Storage Capacity
		/// </summary>
		public BlockOperand OperandA
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
		public StorageBlock() : base()
		{
			this.OperandA = null;
			m_oStorageEntity = null;
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public StorageBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = null;
			m_oStorageEntity = null;
		}
		#endregion

		#region IParseable Methods
		/// <summary>
		/// Parse Plain Text Block and returns a Viper Storage Block
		/// </summary>
		/// <returns></returns>
		public BlockParseResult Parse()
		{
			try
			{
				if( String.IsNullOrEmpty( this.Text ) )
				{
					OnParseFailed( new ParseEventArgs( BlockNames.STORAGE, this.Line, String.Empty ) );
					return BlockParseResult.PARSED_ERROR;
				}

				// Get Plain Text Block Parts
				String[] sBlockParts = BlockFactory.GetBlockParts( this.Text );

				// CORRECT SINTAX FORMAT (all required): 
				// NAME	STORAGE	A
				// First Element should be 'NAME' block label, Second Element should be the 'STORAGE' block name, Third Element the block operand 'A':
				if( sBlockParts[ 1 ].Equals( BlockNames.STORAGE ) && ( sBlockParts.Length == 3 ) )
				{
					// FORMATS: NAME STORAGE A
					m_sBlockLabel = sBlockParts[ 0 ];
					this.OperandA = BlockOperand.TranslateOperand( sBlockParts[ 2 ] );

					if( !String.IsNullOrEmpty( m_sBlockLabel ) && this.OperandA.HasValidValue ) return BlockParseResult.PARSED_OK;
				}

				OnParseFailed( new ParseEventArgs( BlockNames.STORAGE, this.Line, String.Empty ) );
			}
			catch( Exception ex )
			{
				throw new BlockParseException( ex.Message, ex.InnerException, BlockNames.STORAGE, this.Line );
			}
			return BlockParseResult.PARSED_ERROR;
		}

		public event EventHandler ParseSuccess;
		public event EventHandler ParseFailed;

		public void OnParseSuccess( ParseEventArgs eventArgs )
		{
			ParseSuccess( this, eventArgs );
		}

		public void OnParseFailed( ParseEventArgs eventArgs )
		{
			ParseFailed( this, eventArgs );
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
	}
}
