using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Entities
{
	/// <summary>
	/// Represents a LogicSwitch Entity in a GPSS model.
	/// </summary>
	public class LogicSwitch : Entity
	{
		#region Private Member
		private bool m_bState;
		#endregion

		#region Public Properties
		public bool State
		{
			get
			{
				return m_bState;
			}
			set
			{
				m_bState = value;
			}
		}
		#endregion

		#region Constructors
		public LogicSwitch()
			: base()
		{
			m_bState = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sName"></param>
		public LogicSwitch( String sName )
			: base( sName )
		{
			m_bState = false;
		}
		#endregion
	}
}
