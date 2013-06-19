using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Entities;
using Viper.Framework.Engine;
using Viper.Framework.Utils;

namespace Viper.Framework.Blocks
{
	/// <summary>
	/// BlockOperand Class which can hold a SNA, a Name (string) or a Positive Integer as a operand of a given block.
	/// </summary>
	public class BlockOperand
	{
		#region Private Members
		private SNATranslated m_oSNA;
		private String m_sName;
		private int m_iPosInteger;
		private bool m_bIsSNA;
		private bool m_bIsName;
		private bool m_bIsPosInteger;
		#endregion

		#region Public Properties
		/// <summary>
		/// SNA Translated operand value
		/// </summary>
		public SNATranslated SNA
		{
			get
			{
				return m_oSNA;
			}
			set
			{
				if( IsPosInteger )
				{
					m_iPosInteger = Constants.DEFAULT_ZERO_VALUE;
					m_bIsPosInteger = false;
				}
				else if( IsName )
				{
					m_sName = String.Empty;
					m_bIsName = false;
				}
				m_bIsSNA = true;
				m_oSNA = value;
			}
		}

		/// <summary>
		/// Name operand value
		/// </summary>
		public String Name
		{
			get
			{
				return m_sName;
			}
			set
			{
				if( IsSNA )
				{
					m_oSNA = null;
					m_bIsSNA = false;
				}
				else if( IsPosInteger )
				{
					m_iPosInteger = Constants.DEFAULT_ZERO_VALUE;
					m_bIsPosInteger = false;
				}
				m_bIsName = true;
				m_sName = value;
			}
		}

		/// <summary>
		/// Positive Integer operand value
		/// </summary>
		public int PosInteger
		{
			get
			{
				return m_iPosInteger;
			}
			set
			{
				if( IsSNA )
				{
					m_oSNA = null;
					m_bIsSNA = false;
				}
				else if( IsName )
				{
					m_sName = String.Empty;
					m_bIsName = false;
				}
				m_bIsPosInteger = true;
				m_iPosInteger = value;
			}
		}

		/// <summary>
		/// Returns true if Block Operand holds a SNA, false otherwise.
		/// </summary>
		public bool IsSNA
		{
			get
			{
				return m_bIsSNA;
			}
		}

		/// <summary>
		/// Returns true if Block Operand holds a Name, false otherwise.
		/// </summary>
		public bool IsName
		{
			get
			{
				return m_bIsName;
			}
		}

		/// <summary>
		/// Returns true if Block Operand holds a Positive Integer, false otherwise.
		/// </summary>
		public bool IsPosInteger
		{
			get
			{
				return m_bIsPosInteger;
			}
		}

		/// <summary>
		/// Returns true if Block Operand has a valid SNA, Name or Positive Integer.
		/// </summary>
		public bool HasValidValue
		{
			get
			{
				return ( m_bIsPosInteger || m_bIsName || m_bIsSNA );
			}
		}
		#endregion

		#region Constructor
		public BlockOperand()
		{
			m_oSNA = null;
			m_sName = String.Empty;
			m_iPosInteger = Constants.DEFAULT_ZERO_VALUE;
			m_bIsName = false;
			m_bIsPosInteger = false;
			m_bIsSNA = false;
		}
		#endregion

		/// <summary>
		/// BlockOperand Creator from String Operand
		/// </summary>
		/// <param name="operand"></param>
		/// <returns></returns>
		public static BlockOperand TranslateOperand( String operand )
		{
			BlockOperand blockOperand = new BlockOperand();

			SNATranslated sna = ViperSNATranslator.Translate( operand );
			if( sna != null )
			{
				blockOperand.SNA = sna;
			}
			else
			{
				if( ViperSNATranslator.IsValidPositiveInteger( operand ) )
				{
					int iDummyValue = Constants.DEFAULT_ZERO_VALUE;
					if( Int32.TryParse( operand, out iDummyValue ) ) blockOperand.PosInteger = iDummyValue;
				}
				else if( ViperSNATranslator.IsValidName( operand ) )
				{
					blockOperand.Name = operand;
				}
			}

			return blockOperand;
		}
	}
}
