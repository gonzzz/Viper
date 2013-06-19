using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Enums;

namespace Viper.Framework.Entities
{
	/// <summary>
	/// SNA translated utility class
	/// </summary>
	public class SNATranslated
	{
		#region Properties
		public SNAType Type
		{
			get;
			set;
		}
		public SNA SNA
		{
			get;
			set;
		}
		public SNAParameter Parameter
		{
			get;
			set;
		}
		public List<SNAParameter> ExtraParameters
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public SNATranslated()
		{
			ExtraParameters = new List<SNAParameter>();
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="type"></param>
		/// <param name="sna"></param>
		public SNATranslated( SNAType type, SNA sna )
			: this()
		{
			Type = type;
			SNA = sna;
			Parameter = null;
		}
		#endregion
	}
}
