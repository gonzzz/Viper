using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Utils;
using Viper.Framework.Blocks;
using Viper.Framework.Entities;

namespace Viper.Framework.Engine
{
	public class ViperSystem
	{
		#region Singleton Implementation
		private static ViperSystem m_oSystem = null;

		/// <summary>
		/// Private Constructor
		/// </summary>
		private ViperSystem()
		{
			m_oScheduler = new ViperScheduler();
			m_eLanguage = Languages.Spanish;
			m_lbSimulationBlocks = null;
			m_oModel = new Model();
			m_iTerminationCount = Constants.DEFAULT_ZERO_VALUE;
			ErrorMessageLog = new List<string>();
		}

		/// <summary>
		/// Get Singleton Instance
		/// </summary>
		/// <returns></returns>
		public static ViperSystem Instance()
		{
			if( m_oSystem == null )
			{
				m_oSystem = new ViperSystem();
			}

			return m_oSystem;
		}
		#endregion

		#region Private Members
		private ViperScheduler m_oScheduler;
		private Languages m_eLanguage;
		private List<Block> m_lbSimulationBlocks;
		private Model m_oModel;
		private int m_iTerminationCount;
		#endregion

		#region Public Properties
		/// <summary>
		/// 
		/// </summary>
		public Languages SystemLanguage
		{
			get
			{
				return m_eLanguage;
			}
			set
			{
				m_eLanguage = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public ViperScheduler TransactionScheduler
		{
			get
			{
				return m_oScheduler;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Model Model
		{
			get
			{
				return m_oModel;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int TerminationCount
		{
			get
			{
				return m_iTerminationCount;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public List<String> ErrorMessageLog { get; set; }
		#endregion

		#region Public Methods
		public bool Simulate( String sPlainTextModel, int iTerminationCount )
		{
			// Previous: clean error message log (will be filled with error after parsing the model)
			ErrorMessageLog.Clear();

			// 1) Create Viper Model from Plain Text GPSS model	
			m_lbSimulationBlocks = BlockFactory.Instance().CreateModel( sPlainTextModel );
			ErrorMessageLog.AddRange( BlockFactory.Instance().ErrorMessageLog.ToArray() );

			// 2) Check that Viper Model has been created with NO errors
			if( ErrorMessageLog.Count == 0 )
			{
				// 3) Add Entity Objects to Model
				CreateEntitiesForNonTransactionalBlocks();

				// 4) Start Simulation!

				// 4a) Set termination count (simulation will end when this number gets to zero)
				m_iTerminationCount = iTerminationCount;

				// 4b) Detect GENERATE blocks and "prime" first transactions from there
				

				return true;
			}

			return false;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Creates and initializes Entities for STORAGE, VARIABLE, BVARIABLE, FVARIABLE, INITIAL, FUNCTION, TABLE, QTABLE and MATRIX blocks.
		/// Facilities are initialized the first time a SEIZE or PREEMPT block is entered by a transaction. Queues could be initialized by QTABLE or
		/// the first time a transaction enters a QUEUE block.
		/// </summary>
		private void CreateEntitiesForNonTransactionalBlocks()
		{
			var oNonTransactionalBlocks = ( from b in m_lbSimulationBlocks
										   where b.Executable == false
										   select b ).ToList();

			foreach( Block block in oNonTransactionalBlocks )
			{
				if( block is StorageBlock )
				{
					StorageBlock storageBlock = block as StorageBlock;
					
					// Create new Storage in Model with Storage Block parameters
					Storage newStorage = new Storage( storageBlock.Label );
					newStorage.Capacity = Convert.ToInt32( storageBlock.OperandA );
					newStorage.Available = newStorage.Capacity;
					Model.AddStorage( newStorage );

					// Attach Storage Entity to Storage Block
					storageBlock.AttachStorage( Model.GetStorageByName( storageBlock.Label ) );
				}
			}
		}
		#endregion
	}
}
