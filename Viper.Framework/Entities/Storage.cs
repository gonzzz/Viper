using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Utils;

namespace Viper.Framework.Entities
{
	/// <summary>
	/// Represents an Storage Entity in a GPSS model.
	/// </summary>
	public class Storage : Entity
	{
		#region Private Members
		private int m_iTotalCapacity;
		private int m_iAvailableCapacity;
		#endregion

		#region Public Members
		/// <summary>
		/// Returns total capacity for storage
		/// </summary>
		public int Capacity
		{
			get
			{
				return m_iTotalCapacity;
			}
			set
			{
				m_iTotalCapacity = value;
			}
		}

		/// <summary>
		/// Returns available units for storage.
		/// </summary>
		public int Available
		{
			get
			{
				return m_iAvailableCapacity;
			}
			set
			{
				m_iAvailableCapacity = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public Storage() : base()
		{
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="sName"></param>
		public Storage( String sName ) : base( sName )
		{
		}
		#endregion
	}
}
