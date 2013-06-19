using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Enums
{
	/// <summary>
	/// System Numerical Attributes Types
	/// </summary>
	public enum SNAType
	{
		/// <summary>
		/// System SNA
		/// </summary>
		System = 1,

		/// <summary>
		/// Block SNA
		/// </summary>
		Block = 2,
		
		/// <summary>
		/// Transaction SNA
		/// </summary>
		Transaction = 3,

		/// <summary>
		/// Storage SNA
		/// </summary>
		Storage = 4,

		/// <summary>
		/// Facility SNA
		/// </summary>
		Facility = 5,

		/// <summary>
		/// Queue SNA
		/// </summary>
		Queue = 6,

		/// <summary>
		/// User Chain SNA
		/// </summary>
		UserChain = 7,

		/// <summary>
		/// Function SNA
		/// </summary>
		Function = 8,

		/// <summary>
		/// Logic Switch SNA
		/// </summary>
		LogicSwitch = 9,

		/// <summary>
		/// Variable SNA
		/// </summary>
		Variable = 10,

		/// <summary>
		/// Boolean Variable SNA
		/// </summary>
		BVariable = 11,

		/// <summary>
		/// Numeric Group SNA
		/// </summary>
		NumericGroup = 12,

		/// <summary>
		/// Transaction Group SNA
		/// </summary>
		TransactionGroup = 13,

		/// <summary>
		/// Save Value SNA
		/// </summary>
		SaveValue = 14,

		/// <summary>
		/// Matrix Save Value SNA
		/// </summary>
		MatrixSaveValue = 15,

		/// <summary>
		/// Table SNA
		/// </summary>
		Table = 16
	}

	/// <summary>
	/// System Numerical Attributes
	/// </summary>
	public enum SNA
	{
		/// <summary>
		/// AC1 = System Absolute Time Clock
		/// </summary>
		SystemAbsoluteClock = 1,

		/// <summary>
		/// C1 = System Relative Time Clock after last RESET
		/// </summary>
		SystemCurrentClock = 2,

		/// <summary>
		/// TG1 = System Termination Count
		/// </summary>
		SystemTerminationCount = 3,

		/// <summary>
		/// RN = System Randon Number
		/// </summary>
		SystemRandomNumber = 4,
		
		/// <summary>
		/// W = Block Current Transaction Count
		/// </summary>
		BlockCurrentTransactionCount = 5,

		/// <summary>
		/// N = Block Total Transaction Count
		/// </summary>
		BlockTotalTransactionCount = 6,

		/// <summary>
		/// M1 = Transaction Total Transit Time
		/// </summary>
		TransactionTotalTransitTime = 7,

		/// <summary>
		/// XN1 = Active Transaction Number
		/// </summary>
		ActiveTransactionNumber = 8,

		/// <summary>
		/// PR = Transaction Priority
		/// </summary>
		TransactionPriority = 9,

		/// <summary>
		/// P = Transaction Parameter
		/// </summary>
		TransactionParameter = 10,

		/// <summary>
		/// MP = Transaction Transit Time relative to Parameter value
		/// </summary>
		TransactionTransitTimeRelativeToParameter = 11,

		/// <summary>
		/// MB = Transaction Match at Block
		/// </summary>
		TransactionMatchAtBlock = 12,

		/// <summary>
		/// R = Storage Total Units
		/// </summary>
		StorageTotalUnits = 13,

		/// <summary>
		/// S = Storage Available Units
		/// </summary>
		StorageAvailableUnits = 14,

		/// <summary>
		/// SA = Storage Average Use
		/// </summary>
		StorageAverageUse = 15,

		/// <summary>
		/// SC = Storage Entries Count
		/// </summary>
		StorageEntriesCount = 16,

		/// <summary>
		/// SE = Storage is Empty?
		/// </summary>
		StorageIsEmpty = 17,

		/// <summary>
		/// SF = Storage is Full?
		/// </summary>
		StorageIsFull = 18,

		/// <summary>
		/// SR = Storage Utilization Ratio
		/// </summary>
		StorageUtilizationRatio = 19,

		/// <summary>
		/// SM = Storage Maximum Usage
		/// </summary>
		StorageMaximusUsage = 20,

		/// <summary>
		/// ST = Storage Average Usage Time
		/// </summary>
		StorageAverageUsageTime = 21,

		/// <summary>
		/// SV = Storage Is Available
		/// </summary>
		StorageIsAvailable = 22,

		/// <summary>
		/// F = Facility is Occupied?
		/// </summary>
		FacilityIsOccupied = 23,

		/// <summary>
		/// FC = Facility Entry Count
		/// </summary>
		FacilityEntriesCount = 24,

		/// <summary>
		/// FI = Facility is Preempted?
		/// </summary>
		FacilityIsPreempted = 25,

		/// <summary>
		/// FR = Facility Utilization Ratio
		/// </summary>
		FacilityUtilizationRatio = 26,

		/// <summary>
		/// FT = Facility Average Occupation Time
		/// </summary>
		FacilityAverageOccupationTime = 27,

		/// <summary>
		/// FV = Facility Is Available?
		/// </summary>
		FacilityIsAvailable = 28,

		/// <summary>
		/// Q = Queue Actual Content
		/// </summary>
		QueueActualContent = 29,

		/// <summary>
		/// QA = Queue Average Content
		/// </summary>
		QueueAverageContent = 30,

		/// <summary>
		/// QC = Queue Entry Count
		/// </summary>
		QueueEntryCount = 31,

		/// <summary>
		/// QM = Queue Maximum Entry Count
		/// </summary>
		QueueMaximumEntryCount = 32,

		/// <summary>
		/// QT = Queue Average Stay Time
		/// </summary>
		QueueAverageStayTime = 33,

		/// <summary>
		/// QX = Queue Average Stay Time excluding zero-content entries
		/// </summary>
		QueueAverageStayTimeExcludingZeroContentEntries = 34,

		/// <summary>
		/// QZ = Queue Entry Count With zero-content entries
		/// </summary>
		QueueEntryCountWithZeroContentEntries = 35,

		/// <summary>
		/// CA = User Chain Average Content
		/// </summary>
		UserChainAverageContent = 36,

		/// <summary>
		/// CC = User Chain Total count
		/// </summary>
		UserChainTotalCount = 37,

		/// <summary>
		/// CH = User Chain Current count
		/// </summary>
		UserChainCurrentCount = 38,

		/// <summary>
		/// CM = User Chain Maximum count
		/// </summary>
		UserChainMaximumCount = 39,

		/// <summary>
		/// CT = User Chain Average residence time
		/// </summary>
		UserChainAverageResidenceTime = 40,

		/// <summary>
		/// FN = Function evaluation result
		/// </summary>
		FunctionEvaluationResult = 41,

		/// <summary>
		/// GN = Numeric Group count
		/// </summary>
		NumericGroupCount = 42,

		/// <summary>
		/// GT = Transaction Group count
		/// </summary>
		TransactionGroupCount = 43,

		/// <summary>
		/// LS = Logic Switch state value
		/// </summary>
		LogicSwitchStateValue = 44,

		/// <summary>
		/// X = Save Value current value
		/// </summary>
		SaveValueCurrentValue = 45,

		/// <summary>
		/// MX = Matrix Save Value current value
		/// </summary>
		MatrixSaveValueCurrentValue = 46,

		/// <summary>
		/// TB = Table Nonweigthed Average Entries
		/// </summary>
		TableAverageEntries = 47,

		/// <summary>
		/// TC = Table Nonweigthed Entry Count
		/// </summary>
		TableEntryCount = 48,

		/// <summary>
		/// TD = Table Nonweighted Standard Deviation of entries
		/// </summary>
		TableEntriesStandardDeviation = 49,

		/// <summary>
		/// BV = Boolean Variable evaluation result
		/// </summary>
		BooleanVariableResult = 50,

		/// <summary>
		/// V = Arithmetic/Floating Variable evaluation result
		/// </summary>
		VariableResult = 51
	}

	public enum SNAEntitySpecifierType
	{
		/// <summary>
		/// Use Positive Integer
		/// </summary>
		PositiveInteger = 1,

		/// <summary>
		/// Use Name
		/// </summary>
		Name = 2,

		/// <summary>
		/// Use Indirect Addressing
		/// </summary>
		IndirectAddressing = 4
	}
}
