using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Utils;

namespace Viper.Framework.Entities
{
	/// <summary>
	/// Represents a SaveValue Entity in a GPSS model.
	/// </summary>
	public class SaveValue : Entity
	{
		#region Private Members
		private int m_iValue;
		#endregion

		#region Public Properties
		/// <summary>
		/// 
		/// </summary>
		public int Value
		{
			get
			{
				return m_iValue;
			}
			set
			{
				m_iValue = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public SaveValue()
		{
			m_iValue = Constants.DEFAULT_ZERO_VALUE;
		}

		/// <summary>
		/// Constructur With Params
		/// </summary>
		/// <param name="iNumber"></param>
		public SaveValue( String sName ) : base(sName)
		{
			m_iValue = Constants.DEFAULT_ZERO_VALUE;
		}
		#endregion
	}
}
