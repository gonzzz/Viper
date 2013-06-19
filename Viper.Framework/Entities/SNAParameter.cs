using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Entities
{
	/// <summary>
	/// SNA parameter utility class
	/// </summary>
	public class SNAParameter
	{
		#region Properties
		public String Value
		{
			get;
			set;
		}
		public bool IsName
		{
			get;
			set;
		}
		public bool IsPosInteger
		{
			get;
			set;
		}
		public bool IndirectAddressing
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		public SNAParameter()
		{
			Value = String.Empty;
			IsName = false;
			IsPosInteger = false;
			IndirectAddressing = false;
		}

		public SNAParameter( String value, bool isName, bool isPosInteger, bool indirectAddressing )
		{
			Value = value;
			IsName = isName;
			IsPosInteger = isPosInteger;
			IndirectAddressing = indirectAddressing;
		}
		#endregion
	}
}
