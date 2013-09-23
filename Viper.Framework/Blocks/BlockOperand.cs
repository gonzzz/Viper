using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Entities;
using Viper.Framework.Engine;
using Viper.Framework.Utils;
using Viper.Framework.Enums;
using Viper.Framework.Exceptions;

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
		private bool m_bIsRequired;
		private bool m_bIsEmpty;
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
				m_bIsEmpty = false;
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
				m_bIsEmpty = String.IsNullOrEmpty( value );
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
				m_bIsEmpty = false;
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
				// If has a positive integer value, a string or a sna value OR
				// If it is empty but not required => Operand is Valid
				return ( m_bIsPosInteger || m_bIsName || m_bIsSNA ) || ( m_bIsEmpty && !m_bIsRequired );
			}
		}

		/// <summary>
		/// Returns true if operand is required, false otherwise.
		/// </summary>
		public bool IsRequired
		{
			get
			{
				return m_bIsRequired;
			}
			set
			{
				m_bIsRequired = value;
			}
		}

		/// <summary>
		/// Returns if operand is empty, false otherwise
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return m_bIsEmpty;
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
			m_bIsRequired = false;
			m_bIsEmpty = true;
		}
		#endregion

		#region Public Static Methods
		/// <summary>
		/// Returns a new Empty Block Operand
		/// </summary>
		/// <returns></returns>
		public static BlockOperand EmptyOperand()
		{
			return new BlockOperand();
		}

		/// <summary>
		/// BlockOperand Creator from String Operand, passing an existent BlockOperand object.
		/// </summary>
		/// <param name="blockOperand"></param>
		/// <param name="operandStr"></param>
		/// <param name="required"></param>
		/// <returns></returns>
		public static BlockOperand TranslateOperand( BlockOperand blockOperand, String operandStr, bool required = false )
		{
			blockOperand.IsRequired = required;
			return ParseOperand( operandStr , blockOperand );
		}

		/// <summary>
		/// BlockOperand Creator from String Operand
		/// </summary>
		/// <param name="operand"></param>
		/// <param name="required"></param>
		/// <returns></returns>
		public static BlockOperand TranslateOperand( String operandStr, bool required = false )
		{
			BlockOperand blockOperand = new BlockOperand();
			blockOperand.IsRequired = required;
			return ParseOperand( operandStr, blockOperand );
		}
		#endregion

		#region Private Static Methods
		/// <summary>
		/// Private Method that do the parsing
		/// </summary>
		/// <param name="operand"></param>
		/// <param name="blockOperand"></param>
		/// <returns></returns>
		private static BlockOperand ParseOperand( String operand , BlockOperand blockOperand )
		{
			if ( !String.IsNullOrEmpty( operand ) )
			{
				SNATranslated sna = ViperSNATranslator.Translate( operand );
				if ( sna != null )
				{
					blockOperand.SNA = sna;
				}
				else
				{
					if ( ViperSNATranslator.IsValidPositiveInteger( operand ) )
					{
						int iDummyValue = Constants.DEFAULT_ZERO_VALUE;
						if ( Int32.TryParse( operand , out iDummyValue ) ) blockOperand.PosInteger = iDummyValue;
					}
					else if ( ViperSNATranslator.IsValidName( operand ) )
					{
						blockOperand.Name = operand;
					}
				}
			}
			return blockOperand;
		}
		#endregion
	}
}
