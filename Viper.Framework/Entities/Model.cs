using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Exceptions;
using Viper.Framework.Utils;
using Viper.Framework.Blocks;
using Viper.Framework.Enums;

namespace Viper.Framework.Entities
{
	/// <summary>
	/// Viper Model Class which holds every GPSS entity as Viper objects
	/// </summary>
	public class Model
	{
		#region Private Members
		private List<Facility> m_oFacilities;
		private List<Storage> m_oStorages;
		private List<Queue> m_oQueues;
		private List<SaveValue> m_oSaveValues;
		private List<LogicSwitch> m_oLogicSwitches;
		#endregion

		#region Constructors
		public Model()
		{
			m_oFacilities = new List<Facility>();
			m_oStorages = new List<Storage>();
			m_oQueues = new List<Queue>();
			m_oSaveValues = new List<SaveValue>();
			m_oLogicSwitches = new List<LogicSwitch>();
		}
		#endregion

		#region Facility Methods
		/// <summary>
		/// Adds a new Facility Entity. If a Facility with the same name already exists a ModelIntegrityException will we thrown.
		/// </summary>
		/// <param name="oFacility"></param>
		public void AddFacility( Facility oFacility )
		{
			if( GetFacilityByName( oFacility.Name ) != null )
			{
				throw new ModelIntegrityException( "Facility with the same name already exists in the model" );
			}

			if( oFacility.Number == Constants.DEFAULT_ZERO_VALUE ) oFacility.Number = m_oFacilities.Count + 1;
			m_oFacilities.Add( oFacility );
		}

		/// <summary>
		/// Gets Facility Entity by its Name
		/// </summary>
		/// <param name="sName"></param>
		/// <returns></returns>
		public Facility GetFacilityByName( String sName )
		{
			Facility oFacility = (	from f in m_oFacilities
									where f.Name.Equals( sName )
									select f ).SingleOrDefault();

			return oFacility;
		}

		/// <summary>
		/// Gets Facility Entity by its number
		/// </summary>
		/// <param name="iNumber"></param>
		/// <returns></returns>
		public Facility GetFacilityByNumber( int iNumber )
		{
			Facility oFacility = ( from f in m_oFacilities
								   where f.Number == iNumber
								   select f ).SingleOrDefault();

			return oFacility;
		}

		/// <summary>
		/// Get Queue entity by a block operand (Name, Number, SNA -TX indirect addressing-)
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <param name="operand"></param>
		/// <returns></returns>
		public Facility GetFacilityFromOperands( Transaction oTransaction, BlockOperand operand )
		{
			if( !operand.IsEmpty )
			{
				if( operand.IsName )
				{
					String facilityName = String.Empty;
					facilityName = operand.Name;
					if( !String.IsNullOrEmpty( facilityName ) )
					{
						Facility theFacility = GetFacilityByName( facilityName );
						// Facilities are not created before simulation so they should be created the first time they are entered
						if( theFacility == null )
						{
							theFacility = new Facility( facilityName );
							AddFacility( theFacility );
						}
						return theFacility;
					}
				}
				else if( operand.IsPosInteger )
				{
					int iFacilityNumber = Constants.DEFAULT_ZERO_VALUE;
					iFacilityNumber = operand.PosInteger;
					if( iFacilityNumber > Constants.DEFAULT_ZERO_VALUE )
					{
						Facility theFacility = GetFacilityByNumber( iFacilityNumber );
						// Facilities are not created before simulation so they should be created the first time they are entered
						if( theFacility == null )
						{
							theFacility = new Facility( String.Format( "FACILITY_{0}", iFacilityNumber ) );
							AddFacility( theFacility );
						}
						return theFacility;
					}
				}
				else if( operand.IsSNA )
				{
					if( operand.SNA.Type == SNAType.Transaction )
					{
						// Transaction SNA: use Transaction to get Storage Name or Number
						if( operand.SNA.Parameter.IsPosInteger || operand.SNA.Parameter.IsName )
						{
							String sParameterValue = operand.SNA.Parameter.Value;
							int iFacilityNumber = Constants.DEFAULT_ZERO_VALUE;
							iFacilityNumber = oTransaction.GetParameter( sParameterValue );
							if( iFacilityNumber > Constants.DEFAULT_ZERO_VALUE )
							{
								Facility theFacility = GetFacilityByNumber( iFacilityNumber );
								// Facilities are not created before simulation so they should be created the first time they are entered
								if( theFacility == null )
								{
									theFacility = new Facility( String.Format( "FACILITY_{0}", iFacilityNumber ) );
									AddFacility( theFacility );
								}
								return theFacility;
							}
						}
					}
				}
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<Facility> GetFacilities()
		{
			return m_oFacilities;
		}

		/// <summary>
		/// Clear All Facilities in the Model
		/// </summary>
		public void ClearFacilities()
		{

		}

		/// <summary>
		/// Reset All FAcilities in the Model
		/// </summary>
		public void ResetFacilities()
		{
		}

		/// <summary>
		/// Removes all Facilities from Model
		/// </summary>
		public void RemoveAllFacilities()
		{
			m_oFacilities.Clear();
		}

		/// <summary>
		/// Returns Facilities Count
		/// </summary>
		/// <returns></returns>
		public int FacilitiesCount()
		{
			return m_oFacilities.Count;
		}
		#endregion

		#region Storage Methods
		/// <summary>
		/// Adds a new Storage Entity. If a Storage with the same name already exists a ModelIntegrityException will we thrown.
		/// </summary>
		/// <param name="oStorage"></param>
		public void AddStorage( Storage oStorage )
		{
			if( GetStorageByName( oStorage.Name ) != null )
			{
				throw new ModelIntegrityException( "Storage with the same name already exists in the model" );
			}

			if( oStorage.Number == Constants.DEFAULT_ZERO_VALUE ) oStorage.Number = m_oStorages.Count + 1;
			m_oStorages.Add( oStorage );
		}

		/// <summary>
		/// Gets Storage Entity by its Name
		/// </summary>
		/// <param name="sName"></param>
		/// <returns></returns>
		public Storage GetStorageByName( String sName )
		{
			Storage oStorage = ( from s in m_oStorages
								 where s.Name.Equals( sName )
								 select s ).SingleOrDefault();

			return oStorage;
		}

		/// <summary>
		/// Gets Storage Entity by its number
		/// </summary>
		/// <param name="iNumber"></param>
		/// <returns></returns>
		public Storage GetStorageByNumber( int iNumber )
		{
			Storage oStorage = ( from s in m_oStorages
								 where s.Number == iNumber
								 select s ).SingleOrDefault();

			return oStorage;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<Storage> GetStorages()
		{
			return m_oStorages;
		}

		/// <summary>
		/// Returns the total number of Storages in the Model
		/// </summary>
		/// <returns></returns>
		public int StoragesCount()
		{
			return m_oStorages.Count;
		}

		/// <summary>
		/// Clear All Facilities in the Model
		/// </summary>
		public void ClearStorages()
		{

		}

		/// <summary>
		/// Reset All FAcilities in the Model
		/// </summary>
		public void ResetStorages()
		{
		}

		/// <summary>
		/// Removes all Storages from model
		/// </summary>
		public void RemoveAllStorages()
		{
			m_oStorages.Clear();
		}

		/// <summary>
		/// Get Storage entity by a block operand (Name, Number, SNA -TX indirect addressing-)
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <param name="operand"></param>
		/// <returns></returns>
		public Storage GetStorageFromOperands( Transaction oTransaction, BlockOperand operand )
		{
			if( !operand.IsEmpty )
			{
				if( operand.IsName )
				{
					String storageName = String.Empty;
					storageName = operand.Name;
					if( !String.IsNullOrEmpty( storageName ) )
					{
						return GetStorageByName( storageName );
					}
				}
				else if( operand.IsPosInteger )
				{
					int iStorageNumber = Constants.DEFAULT_ZERO_VALUE;
					iStorageNumber = operand.PosInteger;
					if( iStorageNumber > Constants.DEFAULT_ZERO_VALUE )
					{
						return GetStorageByNumber( iStorageNumber );
					}
				}
				else if( operand.IsSNA )
				{
					if( operand.SNA.Type == SNAType.Transaction )
					{
						// Transaction SNA: use Transaction to get Storage Name or Number
						if( operand.SNA.Parameter.IsPosInteger || operand.SNA.Parameter.IsName )
						{
							String sParameterValue = operand.SNA.Parameter.Value;
							int iStorageNumber = Constants.DEFAULT_ZERO_VALUE;
							iStorageNumber = oTransaction.GetParameter( sParameterValue );
							if( iStorageNumber > Constants.DEFAULT_ZERO_VALUE )
							{
								return GetStorageByNumber( iStorageNumber );
							}
						}
					}
				}
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int GetAmountToOccupyOrLeaveInStorage( Transaction oTransaction, BlockOperand operand )
		{
			int iAmountToOccupyOrLeave = Storage.DEFAULT_OCCUPY_AMOUNT;
			if( !operand.IsEmpty )
			{
				if( operand.IsPosInteger )
				{
					iAmountToOccupyOrLeave = operand.PosInteger;
				}
				else if( operand.IsSNA )
				{
					if( operand.SNA.Type == SNAType.Transaction )
					{
						// Transaction SNA: use Transaction to get Storage Name or Number
						if( operand.SNA.Parameter.IsPosInteger || operand.SNA.Parameter.IsName )
						{
							String sParameterValue = operand.SNA.Parameter.Value;
							iAmountToOccupyOrLeave = oTransaction.GetParameter( sParameterValue );
						}
					}
				}
			}

			return iAmountToOccupyOrLeave;
		}
		#endregion

		#region Queue Methods
		/// <summary>
		/// Adds a new Queue Entity. If a Queue with the same name already exists a ModelIntegrityException will we thrown.
		/// </summary>
		/// <param name="oQueue"></param>
		public void AddQueue( Queue oQueue )
		{
			if( GetFacilityByName( oQueue.Name ) != null )
			{
				throw new ModelIntegrityException( "Queue with the same name already exists in the model" );
			}

			if( oQueue.Number == Constants.DEFAULT_ZERO_VALUE ) oQueue.Number = m_oFacilities.Count + 1;
			m_oQueues.Add( oQueue );
		}

		/// <summary>
		/// Gets Queue Entity by its Name
		/// </summary>
		/// <param name="sName"></param>
		/// <returns></returns>
		public Queue GetQueueByName( String sName )
		{
			Queue oQueue = ( from q in m_oQueues
							 where q.Name.Equals( sName )
							 select q ).SingleOrDefault();

			return oQueue;
		}

		/// <summary>
		/// Gets Queue Entity by its number
		/// </summary>
		/// <param name="iNumber"></param>
		/// <returns></returns>
		public Queue GetQueueByNumber( int iNumber )
		{
			Queue oQueue = ( from q in m_oQueues
							 where q.Number == iNumber
							 select q ).SingleOrDefault();

			return oQueue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<Queue> GetQueues()
		{
			return m_oQueues;
		}

		/// <summary>
		/// Clear All Queues in the Model
		/// </summary>
		public void ClearQueues()
		{

		}

		/// <summary>
		/// Reset All Queues in the Model
		/// </summary>
		public void ResetQueues()
		{
		}

		/// <summary>
		/// Removes all Queues from Model
		/// </summary>
		public void RemoveAllQueues()
		{
			m_oQueues.Clear();
		}

		/// <summary>
		/// Returns Queues count
		/// </summary>
		/// <returns></returns>
		public int QueuesCount()
		{
			return m_oQueues.Count;
		}

		/// <summary>
		/// Get Queue entity by a block operand (Name, Number, SNA -TX indirect addressing-)
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <param name="operand"></param>
		/// <returns></returns>
		public Queue GetQueueFromOperands( Transaction oTransaction, BlockOperand operand )
		{
			if( !operand.IsEmpty )
			{
				if( operand.IsName )
				{
					String queueName = String.Empty;
					queueName = operand.Name;
					if( !String.IsNullOrEmpty( queueName ) )
					{
						Queue theQueue = GetQueueByName( queueName );
						// Queues are not created before simulation so they should be created the first time they are entered
						if( theQueue == null ) 
						{
							theQueue = new Queue( queueName );
							AddQueue( theQueue );
						}
						return theQueue;
					}
				}
				else if( operand.IsPosInteger )
				{
					int iQueueNumber = Constants.DEFAULT_ZERO_VALUE;
					iQueueNumber = operand.PosInteger;
					if( iQueueNumber > Constants.DEFAULT_ZERO_VALUE )
					{
						Queue theQueue = GetQueueByNumber( iQueueNumber );
						// Queues are not created before simulation so they should be created the first time they are entered
						if( theQueue == null )
						{
							theQueue = new Queue( String.Format( "QUEUE_{0}", iQueueNumber ) );
							AddQueue( theQueue );
						}
						return theQueue;
					}
				}
				else if( operand.IsSNA )
				{
					if( operand.SNA.Type == SNAType.Transaction )
					{
						// Transaction SNA: use Transaction to get Storage Name or Number
						if( operand.SNA.Parameter.IsPosInteger || operand.SNA.Parameter.IsName )
						{
							String sParameterValue = operand.SNA.Parameter.Value;
							int iQueueNumber = Constants.DEFAULT_ZERO_VALUE;
							iQueueNumber = oTransaction.GetParameter( sParameterValue );
							if( iQueueNumber > Constants.DEFAULT_ZERO_VALUE )
							{
								Queue theQueue = GetQueueByNumber( iQueueNumber );
								// Queues are not created before simulation so they should be created the first time they are entered
								if( theQueue == null )
								{
									theQueue = new Queue( String.Format( "QUEUE_{0}", iQueueNumber ) );
									AddQueue( theQueue );
								}
								return theQueue;
							}
						}
					}
				}
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int GetAmountToQueueOrDequeueFromQueue( Transaction oTransaction, BlockOperand operand )
		{
			int iAmountToQueueOrDequeue = Queue.DEFAULT_QUEUE_AMOUNT;
			if( !operand.IsEmpty )
			{
				if( operand.IsPosInteger )
				{
					iAmountToQueueOrDequeue = operand.PosInteger;
				}
				else if( operand.IsSNA )
				{
					if( operand.SNA.Type == SNAType.Transaction )
					{
						// Transaction SNA: use Transaction to get Storage Name or Number
						if( operand.SNA.Parameter.IsPosInteger || operand.SNA.Parameter.IsName )
						{
							String sParameterValue = operand.SNA.Parameter.Value;
							iAmountToQueueOrDequeue = oTransaction.GetParameter( sParameterValue );
						}
					}
				}
			}

			return iAmountToQueueOrDequeue;
		}
		#endregion

		#region SaveValue Methods
		public void AddSaveValue( SaveValue oSaveValue )
		{
			if( !String.IsNullOrEmpty( oSaveValue.Name ) ) {
				if( GetSaveValueByName( oSaveValue.Name ) != null )
				{
					throw new ModelIntegrityException( "SaveValue with the same name already exists in the model" );
				}
			}
			if( oSaveValue.Number != Constants.DEFAULT_ZERO_VALUE )
			{
				if( GetSaveValueByNumber( oSaveValue.Number ) != null )
				{
					throw new ModelIntegrityException( "SaveValue with the same number already exists in the model" );
				}
			}

			m_oSaveValues.Add( oSaveValue );
		}

		/// <summary>
		/// Gets SaveValue Entity by its Name
		/// </summary>
		/// <param name="sName"></param>
		/// <returns></returns>
		public SaveValue GetSaveValueByName( String sName )
		{
			SaveValue oSaveValue = ( from sv in m_oSaveValues
									 where sv.Name.Equals( sName )
									 select sv ).SingleOrDefault();

			return oSaveValue;
		}

		/// <summary>
		/// Gets SaveValue Entity by its number
		/// </summary>
		/// <param name="iNumber"></param>
		/// <returns></returns>
		public SaveValue GetSaveValueByNumber( int iNumber )
		{
			SaveValue oSaveValue = ( from sv in m_oSaveValues
									 where sv.Number == iNumber
									 select sv ).SingleOrDefault();

			return oSaveValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<SaveValue> GetSaveValues()
		{
			return m_oSaveValues;
		}

		/// <summary>
		/// Clear All SaveValues in the Model
		/// </summary>
		public void ClearSaveValues()
		{

		}

		/// <summary>
		/// Reset All SaveValues in the Model
		/// </summary>
		public void ResetSaveValues()
		{
		}

		/// <summary>
		/// Removes all SaveValues from Model
		/// </summary>
		public void RemoveAllSaveValues()
		{
			m_oSaveValues.Clear();
		}

		/// <summary>
		/// Returns SaveValues count
		/// </summary>
		/// <returns></returns>
		public int SaveValuesCount()
		{
			return m_oSaveValues.Count;
		}

		/// <summary>
		/// Get SaveValue entity by a block operand (Name, Number, SNA -TX indirect addressing-)
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <param name="operand"></param>
		/// <returns></returns>
		public SaveValue GetSaveValueFromOperands( Transaction oTransaction, BlockOperand operand )
		{
			if( !operand.IsEmpty )
			{
				if( operand.IsName )
				{
					String savevalueName = String.Empty;
					savevalueName = operand.Name;
					if( !String.IsNullOrEmpty( savevalueName ) )
					{
						return GetSaveValueByName( savevalueName );
					}
				}
				else if( operand.IsPosInteger )
				{
					int iSavevalueNumber = Constants.DEFAULT_ZERO_VALUE;
					iSavevalueNumber = operand.PosInteger;
					if( iSavevalueNumber > Constants.DEFAULT_ZERO_VALUE )
					{
						return GetSaveValueByNumber( iSavevalueNumber );
					}
				}
				else if( operand.IsSNA )
				{
					if( operand.SNA.Type == SNAType.Transaction )
					{
						// Transaction SNA: use Transaction to get SaveValue Name or Number
						if( operand.SNA.Parameter.IsPosInteger || operand.SNA.Parameter.IsName )
						{
							String sParameterValue = operand.SNA.Parameter.Value;
							int iSavevalueNumber = Constants.DEFAULT_ZERO_VALUE;
							iSavevalueNumber = oTransaction.GetParameter( sParameterValue );
							if( iSavevalueNumber > Constants.DEFAULT_ZERO_VALUE )
							{
								return GetSaveValueByNumber( iSavevalueNumber );
							}
						}
					}
				}
			}

			return null;
		}
		#endregion

		#region LogicSwitch Methods
		public void AddLogicSwitch( LogicSwitch oLogicSwitch )
		{
			if( !String.IsNullOrEmpty( oLogicSwitch.Name ) )
			{
				if( GetLogicSwitchByName( oLogicSwitch.Name ) != null )
				{
					throw new ModelIntegrityException( "LogicSwitch with the same name already exists in the model" );
				}
			}
			if( oLogicSwitch.Number != Constants.DEFAULT_ZERO_VALUE )
			{
				if( GetLogicSwitchByNumber( oLogicSwitch.Number ) != null )
				{
					throw new ModelIntegrityException( "LogicSwitch with the same number already exists in the model" );
				}
			}

			m_oLogicSwitches.Add( oLogicSwitch );
		}

		/// <summary>
		/// Gets LogicSwitch Entity by its Name
		/// </summary>
		/// <param name="sName"></param>
		/// <returns></returns>
		public LogicSwitch GetLogicSwitchByName( String sName )
		{
			LogicSwitch oLogicSwitch = ( from ls in m_oLogicSwitches
										 where ls.Name.Equals( sName )
										 select ls ).SingleOrDefault();

			return oLogicSwitch;
		}

		/// <summary>
		/// Gets LogicSwitch Entity by its number
		/// </summary>
		/// <param name="iNumber"></param>
		/// <returns></returns>
		public LogicSwitch GetLogicSwitchByNumber( int iNumber )
		{
			LogicSwitch oLogicSwitch = ( from ls in m_oLogicSwitches
										 where ls.Number == iNumber
										 select ls ).SingleOrDefault();

			return oLogicSwitch;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<LogicSwitch> GetLogicSwitches()
		{
			return m_oLogicSwitches;
		}

		/// <summary>
		/// Clear All SaveValues in the Model
		/// </summary>
		public void ClearLogicSwitches()
		{

		}

		/// <summary>
		/// Reset All LogicSwitches in the Model
		/// </summary>
		public void ResetLogicSwitches()
		{
		}

		/// <summary>
		/// Removes all LogicSwitches from Model
		/// </summary>
		public void RemoveAllLogicSwitches()
		{
			m_oLogicSwitches.Clear();
		}

		/// <summary>
		/// Returns LogicSwitches count
		/// </summary>
		/// <returns></returns>
		public int LogicSwitchesCount()
		{
			return m_oLogicSwitches.Count;
		}

		/// <summary>
		/// Get LogicSwitch entity by a block operand (Name, Number, SNA -TX indirect addressing-)
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <param name="operand"></param>
		/// <returns></returns>
		public LogicSwitch GetLogicSwitchFromOperands( Transaction oTransaction, BlockOperand operand )
		{
			if( !operand.IsEmpty )
			{
				if( operand.IsName )
				{
					String logicswitchName = String.Empty;
					logicswitchName = operand.Name;
					if( !String.IsNullOrEmpty( logicswitchName ) )
					{
						return GetLogicSwitchByName( logicswitchName );
					}
				}
				else if( operand.IsPosInteger )
				{
					int iLogicSwitchNumber = Constants.DEFAULT_ZERO_VALUE;
					iLogicSwitchNumber = operand.PosInteger;
					if( iLogicSwitchNumber > Constants.DEFAULT_ZERO_VALUE )
					{
						return GetLogicSwitchByNumber( iLogicSwitchNumber );
					}
				}
				else if( operand.IsSNA )
				{
					if( operand.SNA.Type == SNAType.Transaction )
					{
						// Transaction SNA: use Transaction to get LogicSwitch Name or Number
						if( operand.SNA.Parameter.IsPosInteger || operand.SNA.Parameter.IsName )
						{
							String sParameterValue = operand.SNA.Parameter.Value;
							int iLogicSwitchNumber = Constants.DEFAULT_ZERO_VALUE;
							iLogicSwitchNumber = oTransaction.GetParameter( sParameterValue );
							if( iLogicSwitchNumber > Constants.DEFAULT_ZERO_VALUE )
							{
								return GetLogicSwitchByNumber( iLogicSwitchNumber );
							}
						}
					}
				}
			}

			return null;
		}
		#endregion
	}
}
