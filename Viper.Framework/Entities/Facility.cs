using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Utils;

namespace Viper.Framework.Entities
{
	/// <summary>
	/// Facility Class. Represents a GPSS Facility Entity.
	/// </summary>
	public class Facility : Entity
	{
		#region Constructor
		public Facility() : base()
		{
		}

		public Facility( String sName ) : base(sName)
		{
		}
		#endregion
	}
}
