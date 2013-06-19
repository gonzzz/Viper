using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Exceptions;
using Viper.Framework.Utils;

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
		#endregion
	}
}
