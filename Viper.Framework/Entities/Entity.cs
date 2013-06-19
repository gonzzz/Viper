using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Utils;

namespace Viper.Framework.Entities
{
	/// <summary>
	/// Common Entity Class. 
	/// </summary>
	public class Entity
	{
		#region Private Members
		private String m_sName;
		private int m_iNumber;
		#endregion

		#region Public Properties
		public String Name
		{
			get 
			{
				return m_sName;
			}
			set
			{
				m_sName = value;
			}
		}

		public int Number
		{
			get
			{
				return m_iNumber;
			}
			set
			{
				m_iNumber = value;
			}
		}
		#endregion

		#region Constructors
		public Entity()
		{
			m_iNumber = Constants.DEFAULT_ZERO_VALUE;
			m_sName = String.Empty;
		}

		public Entity( String sName ) : this()
		{
			m_sName = sName;
		}
		#endregion
	}
}
