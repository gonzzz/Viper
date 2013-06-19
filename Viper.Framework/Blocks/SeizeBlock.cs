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
	public class SeizeBlock : BlockTransactional, IParseable
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

		#region Entity Member
		/// <summary>
		/// The Facility Entity this Block is related to
		/// </summary>
		private Facility m_oFacilityEntity;
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public SeizeBlock()
			: base()
		{
			this.OperandA = null;
			m_oFacilityEntity = null;
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="iLineNumber"></param>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public SeizeBlock( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.OperandA = null;
			m_oFacilityEntity = null;
		}
		#endregion

		#region IParseable Methods
		/// <summary>
		/// Parse Plain Text Block and returns a Viper Seize Block
		/// </summary>
		/// <returns></returns>
		public BlockParseResult Parse()
		{
			try
			{
				if( String.IsNullOrEmpty( this.Text ) )
				{
					OnParseFailed( new ParseEventArgs( BlockNames.SEIZE, this.Line, String.Empty ) );
					return BlockParseResult.PARSED_ERROR;
				}

				// Get Plain Text Block Parts
				String[] sBlockParts = BlockFactory.GetBlockParts( this.Text );

				// CORRECT SINTAX FORMAT in '[]' optional part: 
				// [NAME]	SEIZE	A
				if( sBlockParts[ 0 ].Equals( BlockNames.SEIZE ) && sBlockParts.Length == 2 )
				{
					// FORMATS: SEIZE A
					m_sBlockLabel = String.Empty;
					this.OperandA = BlockOperand.TranslateOperand( sBlockParts[ 1 ] );
					
					// Operand A is required
					if( this.OperandA.HasValidValue ) return BlockParseResult.PARSED_OK;
				}
				else if( sBlockParts[ 1 ].Equals( BlockNames.SEIZE ) && sBlockParts.Length == 3 )
				{
					// FORMATS: NAME SEIZE A
					m_sBlockLabel = sBlockParts[ 0 ];
					this.OperandA = BlockOperand.TranslateOperand( sBlockParts[ 2 ] );
					
					// Operand A is required
					if( this.OperandA.HasValidValue ) return BlockParseResult.PARSED_OK;
				}

				OnParseFailed( new ParseEventArgs( BlockNames.SEIZE, this.Line, String.Empty ) );
			}
			catch( Exception ex )
			{
				throw new BlockParseException( ex.Message, ex.InnerException, BlockNames.SEIZE, this.Line );
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
		/// Attach the Facility Entity to the Block
		/// </summary>
		/// <param name="oFacility"></param>
		public void AttachFacility( Facility oFacility )
		{
			m_oFacilityEntity = oFacility;
		}

		/// <summary>
		/// Detachs the Facility Entity from the Block
		/// </summary>
		public void DetachFacility()
		{
			m_oFacilityEntity = null;
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
