using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Viper.Framework.Engine;
using Viper.Framework.Enums;
using System.Text.RegularExpressions;
using Viper.Framework.Utils;
using Viper.Framework.Entities;

namespace Viper.Test
{
	/// <summary>
	/// Summary description for SNATransalatorTest
	/// </summary>
	[TestClass]
	public class SNATranslatorTest
	{
		public SNATranslatorTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		#region System SNA
		[TestMethod]
		public void TestSystemSNA_AC1()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "AC1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.SystemAbsoluteClock && sna.Type == SNAType.System );
		}

		[TestMethod]
		public void TestSystemSNA_C1()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "C1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.SystemCurrentClock && sna.Type == SNAType.System );
		}

		[TestMethod]
		public void TestSystemSNA_TG1()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TG1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.SystemTerminationCount && sna.Type == SNAType.System );
		}

		[TestMethod]
		public void TestSystemSNA_RN_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "RN8" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.SystemRandomNumber && sna.Type == SNAType.System );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "8" ) );
		}

		[TestMethod]
		public void TestSystemSNA_RN_PosInteger_WithIndirectAddressing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "RN*2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.SystemRandomNumber && sna.Type == SNAType.System );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestSystemSNA_RN_Name_WithIndirectAddressing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "RN*RANDOM" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.SystemRandomNumber && sna.Type == SNAType.System );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "RANDOM" ) );
		}

		[TestMethod]
		public void TestSystemSNA_RN_NameAndToken_WithIndirectAddressing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "RN*$RANDOM" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.SystemRandomNumber && sna.Type == SNAType.System );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "RANDOM" ) );
		}
		#endregion

		#region Block SNA
		[TestMethod]
		public void TestBlockSNA_W_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "W5" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BlockCurrentTransactionCount && sna.Type == SNAType.Block );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "5" ) );
		}

		[TestMethod]
		public void TestBlockSNA_W_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "W$SALON" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BlockCurrentTransactionCount && sna.Type == SNAType.Block );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALON" ) );
		}

		[TestMethod]
		public void TestBlockSNA_W_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "W*8" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BlockCurrentTransactionCount && sna.Type == SNAType.Block );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "8" ) );
		}

		[TestMethod]
		public void TestBlockSNA_W_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "W*PADRON" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BlockCurrentTransactionCount && sna.Type == SNAType.Block );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "PADRON" ) );
		}

		[TestMethod]
		public void TestBlockSNA_W_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "W*$PADRON" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BlockCurrentTransactionCount && sna.Type == SNAType.Block );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "PADRON" ) );
		}

		[TestMethod]
		public void TestBlockSNA_N_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "N15" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BlockTotalTransactionCount && sna.Type == SNAType.Block );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "15" ) );
		}

		[TestMethod]
		public void TestBlockSNA_N_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "N$SALON" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BlockTotalTransactionCount && sna.Type == SNAType.Block );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALON" ) );
		}

		[TestMethod]
		public void TestBlockSNA_N_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "N*2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BlockTotalTransactionCount && sna.Type == SNAType.Block );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestBlockSNA_N_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "N*PADRON" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BlockTotalTransactionCount && sna.Type == SNAType.Block );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "PADRON" ) );
		}

		[TestMethod]
		public void TestBlockSNA_N_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "N*$PADRON" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BlockTotalTransactionCount && sna.Type == SNAType.Block );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "PADRON" ) );
		}
		#endregion

		#region Transaction SNA
		[TestMethod]
		public void TestTransactionSNA_M1()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "M1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionTotalTransitTime && sna.Type == SNAType.Transaction );
		}

		[TestMethod]
		public void TestTransactionSNA_XN1()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "XN1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.ActiveTransactionNumber && sna.Type == SNAType.Transaction );
		}

		[TestMethod]
		public void TestTransactionSNA_PR()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "PR" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionPriority && sna.Type == SNAType.Transaction );
		}

		[TestMethod]
		public void TestTransactionSNA_P_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "P2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionParameter && sna.Type == SNAType.Transaction );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestTransactionSNA_P_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "P$SALON" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionParameter && sna.Type == SNAType.Transaction );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALON" ) );
		}

		[TestMethod]
		public void TestTransactionSNA_P_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "*2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionParameter && sna.Type == SNAType.Transaction );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestTransactionSNA_P_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "*PADRON" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionParameter && sna.Type == SNAType.Transaction );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "PADRON" ) );
		}

		[TestMethod]
		public void TestTransactionSNA_P_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "*$PADRON" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionParameter && sna.Type == SNAType.Transaction );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "PADRON" ) );
		}


		[TestMethod]
		public void TestTransactionSNA_MP_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MP1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionTransitTimeRelativeToParameter && sna.Type == SNAType.Transaction );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestTransactionSNA_MP_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MP$TIEMPO" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionTransitTimeRelativeToParameter && sna.Type == SNAType.Transaction );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "TIEMPO" ) );
		}

		[TestMethod]
		public void TestTransactionSNA_MB_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MB10" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionMatchAtBlock && sna.Type == SNAType.Transaction );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "10" ) );
		}

		[TestMethod]
		public void TestTransactionSNA_MB_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MB$ENTRADA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionMatchAtBlock && sna.Type == SNAType.Transaction );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "ENTRADA" ) );
		}

		[TestMethod]
		public void TestTransactionSNA_MB_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MB*2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionMatchAtBlock && sna.Type == SNAType.Transaction );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestTransactionSNA_MB_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MB*ENTRADA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionMatchAtBlock && sna.Type == SNAType.Transaction );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "ENTRADA" ) );
		}

		[TestMethod]
		public void TestTransactionSNA_MB_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MB*$ENTRADA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionMatchAtBlock && sna.Type == SNAType.Transaction );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "ENTRADA" ) );
		}
		#endregion

		#region Storage SNA
		[TestMethod]
		public void TestStorageSNA_R_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "R2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageTotalUnits && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestStorageSNA_R_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "R$SALA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageTotalUnits && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_R_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "R*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageTotalUnits && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestStorageSNA_R_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "R*AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageTotalUnits && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_R_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "R*$AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageTotalUnits && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_S_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "S2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAvailableUnits && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestStorageSNA_S_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "S$SALA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAvailableUnits && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_S_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "S*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAvailableUnits && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestStorageSNA_S_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "S*AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAvailableUnits && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_S_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "S*$AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAvailableUnits && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SA_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SA2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAverageUse && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SA_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SA$SALA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAverageUse && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SA_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SA*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAverageUse && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SA_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SA*AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAverageUse && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SA_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SA*$AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAverageUse && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SC_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SC2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageEntriesCount && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SC_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SC$SALA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageEntriesCount && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SC_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SC*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageEntriesCount && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SC_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SC*AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageEntriesCount && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SC_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SC*$AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageEntriesCount && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SE_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SE2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsEmpty && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SE_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SE$SALA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsEmpty && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SE_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SE*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsEmpty && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SE_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SE*AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsEmpty && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SE_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SE*$AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsEmpty && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SF_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SF2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsFull && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SF_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SF$SALA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsFull && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SF_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SF*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsFull && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SF_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SF*AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsFull && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SF_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SF*$AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsFull && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SR_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SR2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageUtilizationRatio && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SR_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SR$SALA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageUtilizationRatio && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SR_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SR*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageUtilizationRatio && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SR_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SR*AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageUtilizationRatio && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SR_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SR*$AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageUtilizationRatio && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SM_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SM2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageMaximusUsage && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SM_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SM$SALA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageMaximusUsage && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SM_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SM*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageMaximusUsage && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SM_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SM*AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageMaximusUsage && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SM_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SM*$AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageMaximusUsage && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_ST_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "ST2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAverageUsageTime && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestStorageSNA_ST_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "ST$SALA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAverageUsageTime && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_ST_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "ST*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAverageUsageTime && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestStorageSNA_ST_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "ST*AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAverageUsageTime && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_ST_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "ST*$AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageAverageUsageTime && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SV_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SV2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsAvailable && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SV_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SV$SALA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsAvailable && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SALA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SV_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SV*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsAvailable && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SV_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SV*AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsAvailable && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}

		[TestMethod]
		public void TestStorageSNA_SV_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "SV*$AULA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.StorageIsAvailable && sna.Type == SNAType.Storage );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AULA" ) );
		}
		#endregion

		#region Facility SNA
		[TestMethod]
		public void TestFacilitySNA_F_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "F2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsOccupied && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_F_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "F$AUDITOR" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsOccupied && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITOR" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_F_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "F*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsOccupied && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_F_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "F*AUDITA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsOccupied && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITA" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_F_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "F*$AUDITA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsOccupied && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITA" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FI_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FI2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsPreempted && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FI_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FI$AUDITOR" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsPreempted && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITOR" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FI_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FI*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsPreempted && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FI_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FI*AUDITA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsPreempted && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITA" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FI_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FI*$AUDITA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsPreempted && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITA" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FC_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FC2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityEntriesCount && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FC_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FC$AUDITOR" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityEntriesCount && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITOR" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FC_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FC*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityEntriesCount && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FC_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FC*AUDITA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityEntriesCount && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITA" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FC_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FC*$AUDITA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityEntriesCount && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITA" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FR_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FR2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityUtilizationRatio && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FR_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FR$AUDITOR" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityUtilizationRatio && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITOR" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FR_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FR*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityUtilizationRatio && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FR_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FR*AUDITA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityUtilizationRatio && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITA" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FR_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FR*$AUDITA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityUtilizationRatio && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITA" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FT_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FT2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityAverageOccupationTime && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FT_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FT$AUDITOR" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityAverageOccupationTime && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITOR" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FT_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FT*1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityAverageOccupationTime && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FT_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FT*AUDITA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityAverageOccupationTime && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITA" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FT_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FT*$AUDITA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityAverageOccupationTime && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITA" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FV_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FV2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsAvailable && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "2" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FV_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FV$AUDITOR" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsAvailable && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITOR" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FV_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FV*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsAvailable && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FV_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FV*AUDITA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsAvailable && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITA" ) );
		}

		[TestMethod]
		public void TestFacilitySNA_FV_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FV*$AUDITA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FacilityIsAvailable && sna.Type == SNAType.Facility );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "AUDITA" ) );
		}
		#endregion

		#region Queue SNA
		[TestMethod]
		public void TestQueueSNA_Q_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "Q1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueActualContent && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestQueueSNA_Q_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "Q$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueActualContent && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_Q_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "Q*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueActualContent && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestQueueSNA_Q_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "Q*COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueActualContent && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_Q_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "Q*COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueActualContent && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QA_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QA1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageContent && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QA_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QA$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageContent && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QA_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QA*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageContent && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QA_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QA*COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageContent && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QA_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QA*$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageContent && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QC_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QC1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueEntryCount && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QC_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QC$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueEntryCount && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QC_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QC*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueEntryCount && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QC_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QC*COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueEntryCount && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QC_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QC*$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueEntryCount && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QM_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QM1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueMaximumEntryCount && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QM_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QM$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueMaximumEntryCount && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QM_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QM*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueMaximumEntryCount && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QM_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QM*COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueMaximumEntryCount && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QM_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QM*$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueMaximumEntryCount && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QT_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QT1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageStayTime && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QT_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QT$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageStayTime && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QT_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QT*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageStayTime && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QT_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QT*COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageStayTime && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QT_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QT*$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageStayTime && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QX_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QX1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageStayTimeExcludingZeroContentEntries && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QX_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QX$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageStayTimeExcludingZeroContentEntries && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QX_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QX*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageStayTimeExcludingZeroContentEntries && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QX_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QX*COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageStayTimeExcludingZeroContentEntries && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QX_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QX*$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueAverageStayTimeExcludingZeroContentEntries && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QZ_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QZ1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueEntryCountWithZeroContentEntries && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QZ_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QZ$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueEntryCountWithZeroContentEntries && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QZ_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QZ*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueEntryCountWithZeroContentEntries && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QZ_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QZ*COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueEntryCountWithZeroContentEntries && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}

		[TestMethod]
		public void TestQueueSNA_QZ_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "QZ*$COLA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.QueueEntryCountWithZeroContentEntries && sna.Type == SNAType.Queue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "COLA" ) );
		}
		#endregion

		#region User Chain SNA
		[TestMethod]
		public void TestUserChainSNA_CA_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CA1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainAverageContent && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CA_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CA$CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainAverageContent && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CA_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CA*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainAverageContent && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CA_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CA*CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainAverageContent && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CA_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CA*CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainAverageContent && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CC_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CC1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainTotalCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CC_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CC$CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainTotalCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CC_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CC*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainTotalCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CC_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CC*CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainTotalCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CC_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CC*$CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainTotalCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CH_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CH1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainCurrentCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CH_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CH$CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainCurrentCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CH_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CH*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainCurrentCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CH_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CH*CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainCurrentCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CH_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CH*$CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainCurrentCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CM_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CM1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainMaximumCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CM_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CM$CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainMaximumCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CM_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CM*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainMaximumCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CM_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CM*CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainMaximumCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CM_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CM*$CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainMaximumCount && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CT_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CT1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainAverageResidenceTime && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CT_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CT$CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainAverageResidenceTime && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CT_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CT*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainAverageResidenceTime && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CT_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CT*CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainAverageResidenceTime && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}

		[TestMethod]
		public void TestUserChainSNA_CT_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "CT*$CADENA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.UserChainAverageResidenceTime && sna.Type == SNAType.UserChain );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "CADENA" ) );
		}
		#endregion

		#region Function SNA
		[TestMethod]
		public void TestFunctionSNA_FN_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FN1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FunctionEvaluationResult && sna.Type == SNAType.Function );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestFunctionSNA_FN_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FN$MEDIA" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FunctionEvaluationResult && sna.Type == SNAType.Function );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "MEDIA" ) );
		}

		[TestMethod]
		public void TestFunctionSNA_FN_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FN*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FunctionEvaluationResult && sna.Type == SNAType.Function );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestFunctionSNA_FN_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FN*FUNC" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FunctionEvaluationResult && sna.Type == SNAType.Function );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "FUNC" ) );
		}

		[TestMethod]
		public void TestFunctionSNA_FN_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "FN*FUNC" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.FunctionEvaluationResult && sna.Type == SNAType.Function );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "FUNC" ) );
		}
		#endregion

		#region Numeric Group SNA
		[TestMethod]
		public void TestNumericGroupSNA_GN_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "GN1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.NumericGroupCount && sna.Type == SNAType.NumericGroup );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestNumericGroupSNA_GN_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "GN$GRUPO" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.NumericGroupCount && sna.Type == SNAType.NumericGroup );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "GRUPO" ) );
		}

		[TestMethod]
		public void TestNumericGroupSNA_GN_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "GN*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.NumericGroupCount && sna.Type == SNAType.NumericGroup );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestNumericGroupSNA_GN_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "GN*GRUPO" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.NumericGroupCount && sna.Type == SNAType.NumericGroup );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "GRUPO" ) );
		}

		[TestMethod]
		public void TestNumericGroupSNA_GN_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "GN*$GRUPO" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.NumericGroupCount && sna.Type == SNAType.NumericGroup );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "GRUPO" ) );
		}
		#endregion

		#region Transaction Group SNA
		[TestMethod]
		public void TestTransactionGroupSNA_GT_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "GT1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionGroupCount && sna.Type == SNAType.TransactionGroup );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestTransactionGroupSNA_GT_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "GT$GRUP" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionGroupCount && sna.Type == SNAType.TransactionGroup );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "GRUP" ) );
		}

		[TestMethod]
		public void TestTransactionGroupSNA_GT_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "GT*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionGroupCount && sna.Type == SNAType.TransactionGroup );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestTransactionGroupSNA_GT_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "GT*TRANSGROUP" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionGroupCount && sna.Type == SNAType.TransactionGroup );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "TRANSGROUP" ) );
		}

		[TestMethod]
		public void TestTransactionGroupSNA_GT_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "GT*TRANSGROUP" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TransactionGroupCount && sna.Type == SNAType.TransactionGroup );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "TRANSGROUP" ) );
		}
		#endregion

		#region Logic Switch SNA
		[TestMethod]
		public void TestLogicSwitchSNA_LS_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "LS1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.LogicSwitchStateValue && sna.Type == SNAType.LogicSwitch );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestLogicSwitchSNA_LS_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "LS$LLAVE" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.LogicSwitchStateValue && sna.Type == SNAType.LogicSwitch );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "LLAVE" ) );
		}

		[TestMethod]
		public void TestLogicSwitchSNA_LS_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "LS*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.LogicSwitchStateValue && sna.Type == SNAType.LogicSwitch );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestLogicSwitchSNA_LS_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "LS*LLAVE" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.LogicSwitchStateValue && sna.Type == SNAType.LogicSwitch );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "LLAVE" ) );
		}

		[TestMethod]
		public void TestLogicSwitchSNA_LS_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "LS*LLAVE" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.LogicSwitchStateValue && sna.Type == SNAType.LogicSwitch );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "LLAVE" ) );
		}
		#endregion

		#region Save Value SNA
		[TestMethod]
		public void TestSaveValueSNA_X_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "X1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.SaveValueCurrentValue && sna.Type == SNAType.SaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestSaveValueSNA_X_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "X$SVAL" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.SaveValueCurrentValue && sna.Type == SNAType.SaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "SVAL" ) );
		}

		[TestMethod]
		public void TestSaveValueSNA_X_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "X*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.SaveValueCurrentValue && sna.Type == SNAType.SaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestSaveValueSNA_X_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "X*SVAL" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.SaveValueCurrentValue && sna.Type == SNAType.SaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "SVAL" ) );
		}

		[TestMethod]
		public void TestSaveValueSNA_X_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "X*SVAL" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.SaveValueCurrentValue && sna.Type == SNAType.SaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "SVAL" ) );
		}
		#endregion

		#region Matrix Save Value SNA
		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Direct_PosInteger_WithPosIntegerCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX1(3,3)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IsPosInteger && sna.ExtraParameters[ 0 ].Value.Equals( "3" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IsPosInteger && sna.ExtraParameters[ 1 ].Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Direct_PosInteger_WithNameCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX1($ROW,$COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Direct_PosInteger_WithIndirectPosIntegerCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX1(*9,*10)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsPosInteger && sna.ExtraParameters[ 0 ].Value.Equals( "9" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsPosInteger && sna.ExtraParameters[ 1 ].Value.Equals( "10" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Direct_PosInteger_WithIndirectNameCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX1(*ROW,*COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Direct_PosInteger_WithIndirectNameAndTokenCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX1(*$ROW,*$COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Direct_Name_WithPosIntegerCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX$MATRIZ(3,3)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IsPosInteger && sna.ExtraParameters[ 0 ].Value.Equals( "3" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IsPosInteger && sna.ExtraParameters[ 1 ].Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Direct_Name_WithNameCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX$MATRIZ($ROW,$COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Direct_Name_WithIndirectPosIntegerCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX$MATRIZ(*9,*10)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsPosInteger && sna.ExtraParameters[ 0 ].Value.Equals( "9" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsPosInteger && sna.ExtraParameters[ 1 ].Value.Equals( "10" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Direct_Name_WithIndirectNameCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX$MATRIZ(*ROW,*COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Direct_Name_WithIndirectNameAndTokenCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX$MATRIZ(*$ROW,*$COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_PosInteger_WithPosIntegerCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*10(3,3)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "10" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IsPosInteger && sna.ExtraParameters[ 0 ].Value.Equals( "3" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IsPosInteger && sna.ExtraParameters[ 1 ].Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_PosInteger_WithNameCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*10($ROW,$COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "10" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_PosInteger_WithIndirectPosIntegerCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*10(*9,*10)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "10" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsPosInteger && sna.ExtraParameters[ 0 ].Value.Equals( "9" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsPosInteger && sna.ExtraParameters[ 1 ].Value.Equals( "10" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_PosInteger_WithIndirectNameCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*10(*ROW,*COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "10" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_PosInteger_WithIndirectNameAndTokenCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*10(*$ROW,*$COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "10" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_Name_WithPosIntegerCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*MATRIZ(3,3)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IsPosInteger && sna.ExtraParameters[ 0 ].Value.Equals( "3" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IsPosInteger && sna.ExtraParameters[ 1 ].Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_Name_WithNameCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*MATRIZ($ROW,$COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_Name_WithIndirectPosIntegerCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*MATRIZ(*9,*10)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsPosInteger && sna.ExtraParameters[ 0 ].Value.Equals( "9" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsPosInteger && sna.ExtraParameters[ 1 ].Value.Equals( "10" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_Name_WithIndirectNameCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*MATRIZ(*ROW,*COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_Name_WithIndirectNameAndTokenCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*MATRIZ(*$ROW,*$COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_NameAndToken_WithPosIntegerCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*$MATRIZ(3,3)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IsPosInteger && sna.ExtraParameters[ 0 ].Value.Equals( "3" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IsPosInteger && sna.ExtraParameters[ 1 ].Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_NameAndToken_WithNameCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*$MATRIZ($ROW,$COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_NameAndToken_WithIndirectPosIntegerCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*$MATRIZ(*9,*10)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsPosInteger && sna.ExtraParameters[ 0 ].Value.Equals( "9" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsPosInteger && sna.ExtraParameters[ 1 ].Value.Equals( "10" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_NameAndToken_WithIndirectNameCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*$MATRIZ(*ROW,*COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}

		[TestMethod]
		public void TestMatrixSaveValueSNA_MX_Indirect_NameAndToken_WithIndirectNameAndTokenCoordinates()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "MX*$MATRIZ(*$ROW,*$COL)" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.MatrixSaveValueCurrentValue && sna.Type == SNAType.MatrixSaveValue );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "MATRIZ" ) );
			Assert.IsTrue( sna.ExtraParameters.Count == 2 );
			Assert.IsTrue( sna.ExtraParameters[ 0 ].IndirectAddressing && sna.ExtraParameters[ 0 ].IsName && sna.ExtraParameters[ 0 ].Value.Equals( "ROW" ) );
			Assert.IsTrue( sna.ExtraParameters[ 1 ].IndirectAddressing && sna.ExtraParameters[ 1 ].IsName && sna.ExtraParameters[ 1 ].Value.Equals( "COL" ) );
		}
		#endregion

		#region Table SNA
		[TestMethod]
		public void TestTableSNA_TB_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TB1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableAverageEntries && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestTableSNA_TB_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TB$TAB2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableAverageEntries && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "TAB2" ) );
		}

		[TestMethod]
		public void TestTableSNA_TB_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TB*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableAverageEntries && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestTableSNA_TB_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TB*TAB2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableAverageEntries && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "TAB2" ) );
		}

		[TestMethod]
		public void TestTableSNA_TB_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TB*TAB2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableAverageEntries && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "TAB2" ) );
		}

		[TestMethod]
		public void TestTableSNA_TC_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TC1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableEntryCount && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestTableSNA_TC_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TC$TAB2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableEntryCount && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "TAB2" ) );
		}

		[TestMethod]
		public void TestTableSNA_TC_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TC*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableEntryCount && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestTableSNA_TC_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TC*TAB2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableEntryCount && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "TAB2" ) );
		}

		[TestMethod]
		public void TestTableSNA_TC_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TC*TAB2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableEntryCount && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "TAB2" ) );
		}

		[TestMethod]
		public void TestTableSNA_TD_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TD1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableEntriesStandardDeviation && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestTableSNA_TD_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TD$TAB2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableEntriesStandardDeviation && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "TAB2" ) );
		}

		[TestMethod]
		public void TestTableSNA_TD_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TD*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableEntriesStandardDeviation && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestTableSNA_TD_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TD*TAB2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableEntriesStandardDeviation && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "TAB2" ) );
		}

		[TestMethod]
		public void TestTableSNA_TD_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "TD*TAB2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.TableEntriesStandardDeviation && sna.Type == SNAType.Table );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "TAB2" ) );
		}
		#endregion

		#region Boolean Variable SNA
		[TestMethod]
		public void TestBooleanVariableSNA_BV_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "BV1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BooleanVariableResult && sna.Type == SNAType.BVariable );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestBooleanVariableSNA_BV_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "BV$VAR2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BooleanVariableResult && sna.Type == SNAType.BVariable );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "VAR2" ) );
		}

		[TestMethod]
		public void TestBooleanVariableSNA_BV_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "BV*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BooleanVariableResult && sna.Type == SNAType.BVariable );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestBooleanVariableSNA_BV_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "BV*VAR2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BooleanVariableResult && sna.Type == SNAType.BVariable );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "VAR2" ) );
		}

		[TestMethod]
		public void TestBooleanVariableSNA_BV_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "BV*VAR2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.BooleanVariableResult && sna.Type == SNAType.BVariable );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "VAR2" ) );
		}
		#endregion

		#region Variable SNA
		[TestMethod]
		public void TestVariableSNA_V_PosInteger()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "V1" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.VariableResult && sna.Type == SNAType.Variable );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "1" ) );
		}

		[TestMethod]
		public void TestVariableSNA_V_Name()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "V$VAR2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.VariableResult && sna.Type == SNAType.Variable );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IsName && sna.Parameter.Value.Equals( "VAR2" ) );
		}

		[TestMethod]
		public void TestVariableSNA_V_PosInteger_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "V*3" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.VariableResult && sna.Type == SNAType.Variable );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsPosInteger && sna.Parameter.Value.Equals( "3" ) );
		}

		[TestMethod]
		public void TestVariableSNA_V_Name_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "V*VAR2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.VariableResult && sna.Type == SNAType.Variable );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "VAR2" ) );
		}

		[TestMethod]
		public void TestVariableSNA_V_NameAndToken_WithIndirectAddresing()
		{
			SNATranslated sna = ViperSNATranslator.Translate( "V*VAR2" );

			Assert.IsNotNull( sna );
			Assert.IsTrue( sna.SNA == SNA.VariableResult && sna.Type == SNAType.Variable );
			Assert.IsNotNull( sna.Parameter );
			Assert.IsTrue( sna.Parameter.IndirectAddressing && sna.Parameter.IsName && sna.Parameter.Value.Equals( "VAR2" ) );
		}
		#endregion

		#region RegEx Validation
		[TestMethod]
		public void TestRegEx_ValidateNameFail()
		{
			Regex regEx = new Regex( @"^[a-zA-Z]+[a-zA-Z0-9]*$" );

			Assert.IsFalse( regEx.IsMatch( "15" ) );
		}

		[TestMethod]
		public void TestRegEx_ValidateNameSuccess()
		{
			Regex regEx = new Regex( @"^[a-zA-Z]+[a-zA-Z0-9]*$" );

			Assert.IsTrue( regEx.IsMatch( "P9CLASE10J" ) );
		}

		[TestMethod]
		public void TestRegEx_ValidateNotAFacilitySNAButName()
		{
			String strToCompare = @"^F[1-9]?[0-9]*$";
			Regex regEx = new Regex( strToCompare );

			Assert.IsFalse( regEx.IsMatch( "FULL" ) );
			Assert.IsTrue( regEx.IsMatch( "F255" ) );
			Assert.IsFalse( regEx.IsMatch( "F1RACING" ) );
			Assert.IsTrue( regEx.IsMatch( "F5" ) );
		}
		#endregion
	}
}
