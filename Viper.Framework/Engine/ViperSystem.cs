using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Utils;
using Viper.Framework.Blocks;
using Viper.Framework.Entities;
using Viper.Framework.Enums;

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
			ErrorMessageLog = new List<string>();
			m_eLanguage = Languages.Spanish;
			m_lbSimulationBlocks = null;
			m_oModel = new Model();
			m_tCurrentActiveTransaction = null;
			m_ltCurrentEventsChain = new List<Transaction>();
			m_ltFutureEventsChain = new List<Transaction>();
			m_iTerminationCount = Constants.DEFAULT_ZERO_VALUE;
			m_iSystemTime = Constants.DEFAULT_ZERO_VALUE;
			m_eState = SimulationState.Ready;
		}

		/// <summary>
		/// Get System Instance
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

		/// <summary>
		/// Get System Instance Model
		/// </summary>
		/// <returns></returns>
		public static Model InstanceModel()
		{
			return Instance().Model;
		}
		#endregion

		#region Private Attributes
		private const int DEFAULT_MAX_TARGET_TIME = Int32.MaxValue;
		private Languages m_eLanguage;
		private Model m_oModel;
		private SimulationState m_eState;
		private int m_iSystemTime;
		private int m_iTargetTime;
		private int m_iTerminationCount;
		private int m_iTransactionCounter;
		private List<Block> m_lbSimulationBlocks;
		private Transaction m_tCurrentActiveTransaction;
		private List<Transaction> m_ltCurrentEventsChain;
		private List<Transaction> m_ltFutureEventsChain;
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
		public List<Block> Blocks
		{
			get
			{
				return m_lbSimulationBlocks;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public List<Block> BlocksTransactionals
		{
			get
			{
				return m_lbSimulationBlocks.FindAll( b => b is BlockTransactional ).ToList();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public List<Block> BlocksNonTransactionals
		{
			get
			{
				return m_lbSimulationBlocks.FindAll( b => !(b is BlockTransactional) ).ToList();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int BlocksCount
		{
			get
			{
				return m_lbSimulationBlocks.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int BlocksTransactionalCount
		{
			get
			{
				return m_lbSimulationBlocks.FindAll( b => b is BlockTransactional ).Count;
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
			private set
			{
				m_iTerminationCount = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int SystemTime
		{
			get
			{
				return m_iSystemTime;
			}
			private set
			{
				m_iSystemTime = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int TargetTime
		{
			get
			{
				return m_iTargetTime;
			}
			private set
			{
				m_iTargetTime = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Transaction CurrentActiveTransaction
		{
			get
			{
				return m_tCurrentActiveTransaction;
			}
			private set
			{
				m_tCurrentActiveTransaction = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int TransactionCounter
		{
			get
			{
				return m_iTransactionCounter;
			}
			private set
			{
				m_iTransactionCounter = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SimulationState State
		{
			get
			{
				return m_eState;
			}
			private set
			{
				m_eState = value;
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
			if( ErrorMessageLog.Count > 0 ) return false;

			// 3) Add Entity Objects to Model
			CreateEntitiesForNonTransactionalBlocks();

			// 4) Set termination count (simulation will end when this number gets to zero or simulation time is up)
			m_iTerminationCount = iTerminationCount;

			// 5) Start Simulation
			Simulate();				

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public String GetFinalReport()
		{
			return Report();
		}
		#endregion

		#region Simulation Methods
		/// <summary>
		/// 
		/// </summary>
		public void Reset()
		{
			SystemTime = 0;
			TargetTime = DEFAULT_MAX_TARGET_TIME;
			State = SimulationState.Ready;
			CurrentActiveTransaction = null;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			SystemTime = 0;
			TargetTime = DEFAULT_MAX_TARGET_TIME;
			State = SimulationState.Ready;
			CurrentActiveTransaction = null;
		}
		
		/// <summary>
		/// 
		/// </summary>
		public void Pause()
		{
			State = SimulationState.Paused;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Resume()
		{
			if( State == SimulationState.Paused ) State = SimulationState.Running;
		}

		/// <summary>
		/// Creates and initializes Entities for STORAGE, VARIABLE, BVARIABLE, FVARIABLE, INITIAL, FUNCTION, TABLE, QTABLE and MATRIX blocks.
		/// Facilities are initialized the first time a SEIZE or PREEMPT block is entered by a transaction. Queues could be initialized by QTABLE or
		/// the first time a transaction enters a QUEUE block.
		/// </summary>
		private void CreateEntitiesForNonTransactionalBlocks()
		{
			foreach( Block block in BlocksNonTransactionals )
			{
				// At September/2013 i only have STORAGE
				if( block is StorageBlock )
				{
					StorageBlock storageBlock = block as StorageBlock;
					
					// Create new Storage in Model with Storage Block parameters
					Storage newStorage = new Storage( storageBlock.Label, Convert.ToInt32( storageBlock.OperandA.PosInteger ) );
					Model.AddStorage( newStorage );
				}
				else if( block is InitialBlock )
				{
					InitialBlock initialBlock = block as InitialBlock;

					if( initialBlock.OperandA.IsSNA )
					{
						SNATranslated initialSNA = initialBlock.OperandA.SNA;

						if( initialSNA.Type == SNAType.LogicSwitch )
						{
							// Create and initializes new LogicSwitch in Model with Initial Block parameters
							// TODO: Create LogicSwith Entity object and add it to the Model
						}
						else if ( initialSNA.Type == SNAType.SaveValue ) 
						{
							// Create and initializes new SaveValue in Model with Initial Block parameters
							// TODO: Create SaveValue Entity object and add it to the Model
						}
						else if ( initialSNA.Type == SNAType.MatrixSaveValue )
						{
							// Initializes Matrix SaveValue entity (Matrix must have been created before this)
							// TODO: Create Matrix Entity object before this, get element and initialize it
						}
						else
						{
							// Wrong SNA (should be handle after Parse process)
						}
					}
					else
					{
						// Wront Initial Block (should be handle after Parse process)
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void GenerateFirstTransactions()
		{
			// Find all Generate Blocks
			List<Block> generateBlocks = BlocksTransactionals.FindAll( b => b is GenerateBlock );

			// For each generate block, create next transactions
			foreach( Block block in generateBlocks )
			{
				GenerateBlock generateBlock = block as GenerateBlock;

				ScheduleNewTransactionFromGenerate( generateBlock );
			}
		}

		/// <summary>
		/// Set System Time to Next Transaction Time, move Transactions from FEC to CEC
		/// </summary>
		private void UpdateSystemTime()
		{
			// Order by NextTime ascending
			m_ltFutureEventsChain = m_ltFutureEventsChain.OrderBy( t => t.NextSystemTime ).ToList();

			// Get first next transaction in the FEC
			Transaction nextTransaction = m_ltFutureEventsChain.First();

			// Update System Time
			m_iSystemTime = nextTransaction.NextSystemTime;

			// Add transaction to the CEC
			m_ltCurrentEventsChain.Add( nextTransaction );

			// Remove it from the FEC
			m_ltFutureEventsChain.RemoveAt( 0 );

			// Get all transactions in the FEC with the same NextSystemTime
			List<Transaction> anotherTransactions = m_ltFutureEventsChain.
													FindAll( t => t.NextSystemTime == m_iSystemTime );

			// If there are one or more transaction with the same system time
			if( anotherTransactions.Count > 0 )
			{
				// Add all transactions to the CEC
				m_ltCurrentEventsChain.AddRange( anotherTransactions );

				// Remove them all from the FEC
				m_ltFutureEventsChain.RemoveAll( t => t.NextSystemTime == m_iSystemTime );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="currentTransaction"></param>
		private void MoveTransaction( Transaction oTransaction )
		{
			// While Transaction State is ACTIVE, try move it through the model
			while ( oTransaction.State == TransactionState.ACTIVE )
			{
				// Process next Model Block and continue until transaction change its state 
				BlockTransactional nextBlock = oTransaction.NextBlock;

				// If transaction is leaving a generate block, schedule a new transaction there
				CheckAfterGenerate( oTransaction );

				nextBlock.Process( ref oTransaction );
			}
			
			// Remove current transaction from TERMINATE block
			if( oTransaction.State == TransactionState.TERMINATED )
			{
				oTransaction.CurrentBlock.CurrentTransactions.Remove( oTransaction );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		private void CheckAfterGenerate( Transaction oTransaction )
		{
			// Generate new transactions after current transaction leaves the generate block
			if ( oTransaction.CurrentBlock is GenerateBlock )
			{
				// transaction is leaving generate block, generate new transactions
				ScheduleNewTransactionFromGenerate( ( GenerateBlock )oTransaction.CurrentBlock );

				// save current system time as mark time
				oTransaction.MarkTime = SystemTime;
			}
		}

		/// <summary>
		/// Simulation Process
		/// </summary>
		/// <param name="iTargetTime"></param>
		public void Simulate( int iTargetTime = DEFAULT_MAX_TARGET_TIME )
		{
			// Reset System Time
			Clear();

			// Start Simulation
			State = SimulationState.Running;
			TargetTime = iTargetTime;

			// Generate First Transactions (from each Generate Block)
			GenerateFirstTransactions();

			// While Simulation is Running (Normaly or StepByStep) and Finish Conditions are not achieved
			while( !SimulationIsFinished() && SimulationIsRunning() )
			{
				while( CurrentActiveTransaction != null && SimulationIsRunning() && !SimulationIsFinished() )
				{
					// If current transaction is Active, we try to move it to the next block
					if ( CurrentActiveTransaction.State == TransactionState.ACTIVE )
					{
						MoveTransaction( CurrentActiveTransaction );
					}
					else
					{
						// if not anymore (waiting in the FEC, suspended or terminated)
						// select next transaction from CEC and set it Active
						CurrentActiveTransaction = GetNextTransactionFromCEC();
						if ( CurrentActiveTransaction != null ) CurrentActiveTransaction.State = TransactionState.ACTIVE;
					}
				}

				if( SimulationIsRunning() && !SimulationIsFinished() )
				{
					// Update System Time, move transactions from FEC to CEC
					UpdateSystemTime();

					// Get First Transaction from the CEC and set it Active
					CurrentActiveTransaction = GetFirstTransactionFromCEC();
					if( CurrentActiveTransaction != null ) CurrentActiveTransaction.State = TransactionState.ACTIVE;
				}
			}

			// Simulation Ended
			State = SimulationState.Finished;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private bool SimulationIsRunning()
		{
			return ( State == SimulationState.Running || State == SimulationState.RunStepByStep );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private bool SimulationIsFinished()
		{
			// if target time has not been achieved and termination count is greater than zero: not yet finished
			if( (SystemTime <= TargetTime) && (TerminationCount > 0) ) 
				return false;

			// Otherwise, we end the simulation
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oGenerateBlock"></param>
		/// <returns></returns>
		public bool ScheduleNewTransactionFromGenerate( GenerateBlock oGenerateBlock )
		{
			int iTransactionNumber = m_iTransactionCounter + 1;
			Transaction oTransaction = new Transaction( iTransactionNumber );
			if( oGenerateBlock.Process( ref oTransaction ) == BlockProcessResult.TRANSACTION_PROCESSED )
			{
				// Effectively increment by 1
				TransactionCounter++;

				return true;
			}

			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		public void InsertTransactionIntoFEC( Transaction oTransaction )
		{
			// Set Transaction State to WAITING (in FEC)
			oTransaction.State = TransactionState.WAITING;

			m_ltFutureEventsChain.Add( oTransaction );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <returns></returns>
		public bool RemoveTransactonFromFEC( Transaction oTransaction )
		{
			return m_ltFutureEventsChain.Remove( oTransaction );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Transaction GetFirstTransactionFromFEC()
		{
			if( m_ltFutureEventsChain.Count == 0 ) return null;

			return m_ltFutureEventsChain.OrderBy( t => t.NextSystemTime ).First();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		public void InsertTransactionIntoCEC( Transaction oTransaction )
		{
			// Set Transaction State to WAITING (in the CEC)
			oTransaction.State = TransactionState.WAITING;

			m_ltCurrentEventsChain.Add( oTransaction );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		public bool RemoveTransactionFromCEC( Transaction oTransaction )
		{
			return m_ltCurrentEventsChain.Remove( oTransaction );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private Transaction GetFirstTransactionFromCEC()
		{
			if( m_ltCurrentEventsChain.Count == 0 ) return null;

			return m_ltCurrentEventsChain.OrderByDescending( t => t.Priority ).First();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iTransactionNumber"></param>
		/// <returns></returns>
		private Transaction GetTransactionFromCECByNumber( int iTransactionNumber )
		{
			Transaction oTransaction = ( from t in m_ltCurrentEventsChain
										 where t.Number == iTransactionNumber
										 select t ).FirstOrDefault();
			return oTransaction;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Transaction GetNextTransactionFromCEC()
		{
			if( m_ltCurrentEventsChain.Count > 0 )
			{
				return ( from t in m_ltCurrentEventsChain
						 where t != CurrentActiveTransaction &&
						 t.State == TransactionState.WAITING
						 select t ).OrderByDescending( t => t.Priority ).FirstOrDefault();

			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iDecrement"></param>
		/// <returns></returns>
		public bool DecrementTerminationCounter( int iDecrement )
		{
			if( iDecrement < 0 ) return false;

			if( iDecrement > 0 ) TerminationCount -= iDecrement;

			return ( TerminationCount >= 0 );
		}
		#endregion

		#region Report Methods
		private String Report()
		{
			String strOutput = String.Empty;
			strOutput = String.Concat( strOutput, "== SIMULATION RESUME ==", Environment.NewLine );
			strOutput = String.Concat( strOutput, "FINAL SYSTEM TIME:	", this.SystemTime, Environment.NewLine );
			strOutput = String.Concat( strOutput, "TRANSACTIONS CREATED:	", this.TransactionCounter, Environment.NewLine );
			strOutput = String.Concat( strOutput, "BLOCKS COUNT:	", this.BlocksTransactionalCount, Environment.NewLine );
			strOutput = String.Concat( strOutput, "FACILITIES:	", this.Model.FacilitiesCount(), Environment.NewLine );
			strOutput = String.Concat( strOutput, "STORAGES:	", this.Model.StoragesCount(), Environment.NewLine );
			strOutput = String.Concat( strOutput, "QUEUES:	", this.Model.QueuesCount(), Environment.NewLine );

			strOutput = String.Concat( strOutput, Environment.NewLine );
			strOutput = String.Concat( strOutput, "== MODEL ==", Environment.NewLine );
			strOutput = String.Concat( strOutput, "LINE			BLOCK					ENTRY_COUNT		CURRENT_COUNT", Environment.NewLine );
			foreach( BlockTransactional bt in BlocksTransactionals )
			{
				strOutput = String.Concat( strOutput, bt.Line, "	" );
				strOutput = String.Concat( strOutput, bt.Text, "			" );
				strOutput = String.Concat( strOutput, bt.EntryCount, "			" );
				strOutput = String.Concat( strOutput, bt.CurrentCount, Environment.NewLine );
			}

			if( this.Model.FacilitiesCount() > 0 )
			{
				strOutput = String.Concat( strOutput, Environment.NewLine );
				strOutput = String.Concat( strOutput, "== FACILITIES ==", Environment.NewLine );
				strOutput = String.Concat( strOutput, "FACILITY     ENTRIES UTIL.   AVE.TIME	AVAIL.	OWNER	PEND	INTER	RETRY	DELAY", Environment.NewLine );
				foreach( Facility facility in this.Model.GetFacilities() )
				{
					strOutput = String.Concat( strOutput,
												String.Format( "{0}		{1}		{2}	{3}		{4}		{5}		{6}		{7}		{8}		{9}",
												facility.Name, facility.TotalCount, facility.FT, facility.FA, facility.IsAvailable, 
												(facility.Owner!=null)?facility.Owner.Number:0, facility.TransactionCountInPendingChain(), 
												0, 0, facility.TransactionCountInDelayChain() ),
												Environment.NewLine );
				}
			}

			if( this.Model.StoragesCount() > 0 )
			{
				strOutput = String.Concat( strOutput, Environment.NewLine );
				strOutput = String.Concat( strOutput, "== STORAGES ==", Environment.NewLine );
				strOutput = String.Concat( strOutput, "STORAGE		ENTRIES MAX CURRENT AVAIL.	AVG.USE AVG.TX.USE", Environment.NewLine );
				foreach( Storage storage in this.Model.GetStorages() )
				{
					strOutput = String.Concat( strOutput,
												String.Format( "{0}		{1}		{2}	{3}		{4}		{5}	{6}",
																storage.Name, storage.TotalCount, storage.SM, 
																storage.S, storage.R, storage.SA, storage.ST ),
												Environment.NewLine );
				}
			}

			if( this.Model.QueuesCount() > 0 )
			{
				strOutput = String.Concat( strOutput, Environment.NewLine );
				strOutput = String.Concat( strOutput, "== QUEUES ==", Environment.NewLine );
				strOutput = String.Concat( strOutput, "QUEUE		MAX CONT.	ENTRY	ENTRY(0)	AVE.CONT.	AVE.TIME	AVE.(-0)", Environment.NewLine );
				foreach( Queue queue in this.Model.GetQueues() )
				{
					strOutput = String.Concat( strOutput,
												String.Format( "{0}			{1}	{2}		{3}		{4}			{5}		{6}		{7}",
																queue.Name, queue.QM, queue.Q, queue.TotalCount, queue.QZ, queue.QT, queue.QA, queue.QX ),
												Environment.NewLine );
				}
			}

			strOutput = String.Concat( strOutput, Environment.NewLine );
			strOutput = String.Concat( strOutput, "== CURRENT EVENTS CHAIN ==", Environment.NewLine );
			strOutput = String.Concat( strOutput, "TX_NUM	PRIORITY		TX_TIME		CURRENT_LINE	NEXT_LINE		STATE", Environment.NewLine );
			foreach( Transaction tx in m_ltCurrentEventsChain )
			{
				strOutput = String.Concat( strOutput, tx.Number, "	" );
				strOutput = String.Concat( strOutput, tx.Priority, "		" );
				strOutput = String.Concat( strOutput, tx.SimulationTime, "		" );
				strOutput = String.Concat( strOutput, tx.CurrentBlock.Line, "			" );
				if( tx.NextBlock != null ) 
					strOutput = String.Concat( strOutput, tx.NextBlock.Line, "			" );
				else
					strOutput = String.Concat( strOutput, "-			" );
				strOutput = String.Concat( strOutput, tx.State, Environment.NewLine );
			}

			strOutput = String.Concat( strOutput, Environment.NewLine );
			strOutput = String.Concat( strOutput, "== FUTURE EVENTS CHAIN ==", Environment.NewLine );
			strOutput = String.Concat( strOutput, "TX_NUM	NEXT_TIME	CURRENT_LINE	NEXT_LINE		STATE", Environment.NewLine );
			foreach( Transaction tx in m_ltFutureEventsChain )
			{
				strOutput = String.Concat( strOutput, tx.Number, "	" );
				strOutput = String.Concat( strOutput, tx.NextSystemTime, "		" );
				strOutput = String.Concat( strOutput, tx.CurrentBlock.Line, "			" );
				if( tx.NextBlock != null )
					strOutput = String.Concat( strOutput, tx.NextBlock.Line, "			" );
				else
					strOutput = String.Concat( strOutput, "-		" );
				strOutput = String.Concat( strOutput, tx.State, Environment.NewLine );
			}

			strOutput = String.Concat( strOutput, Environment.NewLine );
			strOutput = String.Concat( strOutput, "== END SIMULATION RESUME ==", Environment.NewLine );

			return strOutput;
		}
		#endregion
	}
}
