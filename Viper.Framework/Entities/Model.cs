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
		#endregion

		#region Constructors
		public Model()
		{
			m_oFacilities = new List<Facility>();
			m_oStorages = new List<Storage>();
			m_oQueues = new List<Queue>();
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
	}
}
