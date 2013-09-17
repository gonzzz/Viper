using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Viper.Framework.Entities;
using Viper.Framework.Utils;

namespace Viper.Framework.Blocks
{
	/// <summary>
	/// Primordial Block Class. Every Block in the Model will be inheriting this functionality.
	/// </summary>
	abstract public class Block
	{
		#region Protected Members
		protected int m_iLineNumber;
		protected String m_sBlockLabel;
		protected int m_iBlockNumber;
		protected String m_sBlockText;
		
		protected bool m_bExecutable;
		#endregion

		#region Public Properties
		/// <summary>
		/// Current Line Number in the Model
		/// </summary>
		public int Line
		{
			get
			{
				return m_iLineNumber;
			}
		}

		/// <summary>
		/// Current Block Label in the Model
		/// </summary>
		public String Label
		{
			get
			{
				return m_sBlockLabel;
			}
		}

		/// <summary>
		/// Current Block Number in the model.
		/// </summary>
		public int Number
		{
			get
			{
				return m_iBlockNumber;
			}
		}

		/// <summary>
		/// Current Block Text in the model.
		/// </summary>
		public String Text
		{
			get
			{
				return m_sBlockText;
			}
		}

		

		/// <summary>
		/// Returns True if Block is executable by a transaction. False otherwise.
		/// </summary>
		public bool Executable
		{
			get
			{
				return m_bExecutable;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public Block()
		{
			m_iLineNumber = Constants.DEFAULT_ZERO_VALUE;
			m_iBlockNumber = Constants.DEFAULT_ZERO_VALUE;
			m_sBlockText = String.Empty;
			m_sBlockLabel = String.Empty;
			m_bExecutable = false;
		}

		/// <summary>
		/// Constructor with Parameters
		/// </summary>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public Block( int iLineNumber, int iBlockNumber, String sBlockText )
		{
			m_iLineNumber = iLineNumber;
			m_iBlockNumber = iBlockNumber;
			m_sBlockText = sBlockText;
			m_sBlockLabel = String.Empty;
			m_bExecutable = false;
		}
		#endregion
	}
}
