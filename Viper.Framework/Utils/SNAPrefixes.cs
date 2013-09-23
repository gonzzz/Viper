using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Utils
{
	public class SNAPrefixes
	{
		#region System SNAs
		/// <summary>
		/// System Absolute Time Clock
		/// </summary>
		public const String AC1 = "AC1";

		/// <summary>
		/// System Relative Time Clock since last RESET
		/// </summary>
		public const String C1 = "C1";

		/// <summary>
		/// System Randon Number value
		/// </summary>
		public const String RN = "RN";

		/// <summary>
		/// System Remaining Termination Count
		/// </summary>
		public const String TG1 = "TG1";
		#endregion

		#region Block SNAs
		/// <summary>
		/// Block Current Transaction Count
		/// </summary>
		public const String W = "W";

		/// <summary>
		/// Block Total Transaction Count
		/// </summary>
		public const String N = "N";
		#endregion

		#region Transaction SNAs
		/// <summary>
		/// Transaction Transit Time
		/// </summary>
		public const String M1 = "M1";

		/// <summary>
		/// Transaction Parameter Content
		/// </summary>
		public const String P = "P";

		/// <summary>
		/// Transaction Parameter Content by indirect addressing
		/// </summary>
		public const String P_indirect = "*";

		/// <summary>
		/// Transaction Transit Time minus parameter value content
		/// </summary>
		public const String MP = "MP";

		/// <summary>
		/// Transaction Priority
		/// </summary>
		public const String PR = "PR";

		/// <summary>
		/// Active Transaction Number
		/// </summary>
		public const String XN1 = "XN1";

		/// <summary>
		/// Transaction Match at Block
		/// </summary>
		public const String MB = "MB";
		#endregion

		#region Storage SNAs
		/// <summary>
		/// Storage Total Units
		/// </summary>
		public const String R = "R";

		/// <summary>
		/// Storage FreeUnits Units
		/// </summary>
		public const String S = "S";

		/// <summary>
		/// Storage Average Usage
		/// </summary>
		public const String SA = "SA";

		/// <summary>
		/// Storage Entries Count
		/// </summary>
		public const String SC = "SC";

		/// <summary>
		/// Storage is Empty
		/// </summary>
		public const String SE = "SE";

		/// <summary>
		/// Storage is Full
		/// </summary>
		public const String SF = "SF";

		/// <summary>
		/// Storage Utilization Ratio
		/// </summary>
		public const String SR = "SR";

		/// <summary>
		/// Storage Maximus Usage
		/// </summary>
		public const String SM = "SM";

		/// <summary>
		/// Storage Average Usage Time
		/// </summary>
		public const String ST = "ST";

		/// <summary>
		/// Storage is FreeUnits
		/// </summary>
		public const String SV = "SV";
		#endregion

		#region Facility SNAs
		/// <summary>
		/// Facility is occupied
		/// </summary>
		public const String F = "F";

		/// <summary>
		/// Facility Entries Count
		/// </summary>
		public const String FC = "FC";

		/// <summary>
		/// Facility is preempted
		/// </summary>
		public const String FI = "FI";

		/// <summary>
		/// Facility Utilization Ratio
		/// </summary>
		public const String FR = "FR";

		/// <summary>
		/// Facility Average Occupation Time
		/// </summary>
		public const String FT = "FT";

		/// <summary>
		/// Facility is FreeUnits
		/// </summary>
		public const String FV = "FV";
		#endregion

		#region Queue SNAs
		/// <summary>
		/// Queue Actual Content
		/// </summary>
		public const String Q = "Q";

		/// <summary>
		/// Queue Average Content
		/// </summary>
		public const String QA = "QA";

		/// <summary>
		/// Queue Entry Count
		/// </summary>
		public const String QC = "QC";

		/// <summary>
		/// Queue Maximum Entry Count
		/// </summary>
		public const String QM = "QM";

		/// <summary>
		/// Queue Average Stay Time
		/// </summary>
		public const String QT = "QT";

		/// <summary>
		/// Queue Average Stay Time excluding zero-content entries
		/// </summary>
		public const String QX = "QX";

		/// <summary>
		/// Queue Entry Count with zero-content entries
		/// </summary>
		public const String QZ = "QZ";
		#endregion

		#region UserChain SNAs
		/// <summary>
		/// User Chain Average content
		/// </summary>
		public const String CA = "CA";

		/// <summary>
		/// User Chain Total count
		/// </summary>
		public const String CC = "CC";

		/// <summary>
		/// User Chain Current count
		/// </summary>
		public const String CH = "CH";

		/// <summary>
		/// User Chain Maximum count
		/// </summary>
		public const String CM = "CM";

		/// <summary>
		/// User Chain Average residence time
		/// </summary>
		public const String CT = "CT";
		#endregion	

		#region Function SNAs
		/// <summary>
		/// Function evaluation result
		/// </summary>
		public const String FN = "FN";
		#endregion

		#region Numeric Group SNAs
		/// <summary>
		/// Numeric Group count
		/// </summary>
		public const String GN = "GN";
		#endregion

		#region Transaction Group SNAs
		/// <summary>
		/// Transaction Group count
		/// </summary>
		public const String GT = "GT";
		#endregion

		#region Logic Switch SNAs
		/// <summary>
		/// Logic Switch state value
		/// </summary>
		public const String LS = "LS";
		#endregion

		#region Save Value SNAs
		/// <summary>
		/// Save Value current value
		/// </summary>
		public const String X = "X";
		#endregion

		#region Matrix Save Value SNAs
		/// <summary>
		/// Matrix Save Value current value
		/// </summary>
		public const String MX = "MX";
		#endregion

		#region Table SNAs
		/// <summary>
		/// Table Nonweigthed Average Entries
		/// </summary>
		public const String TB = "TB";

		/// <summary>
		/// Table Nonweigthed Entry Count
		/// </summary>
		public const String TC = "TC";

		/// <summary>
		/// Table Nonweighted Standard Deviation of entries
		/// </summary>
		public const String TD = "TD";
		#endregion

		#region BVariable SNAs
		/// <summary>
		/// Boolean Variable evaluation result
		/// </summary>
		public const String BV = "BV";
		#endregion

		#region Arithmetic/Float Variable SNAs
		/// <summary>
		/// Arithmetic/Floating Variable evaluation result
		/// </summary>
		public const String V = "V";
		#endregion
	}
}
