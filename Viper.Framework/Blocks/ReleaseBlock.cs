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
		public ReleaseBlock()
			: base()
		{
			this.OperandA = BlockOperand.EmptyOperand();
			this.m_oFacilityEntity = null;
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
			this.m_oFacilityEntity = null;
		}
		#endregion

		#region IParseable Methods
		/// <summary>
		/// Parse Plain Text Block and returns a Viper Release Block
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

		#region IProcessable Implementation
		public override BlockProcessResult Process( ref Transaction oTransaction )
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
