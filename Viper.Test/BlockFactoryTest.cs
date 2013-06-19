using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Viper.Framework.Blocks;
using Viper.Framework.Enums;

namespace Viper.Test
{
	/// <summary>
	/// Summary description for BlockFactoryTest
	/// </summary>
	[TestClass]
	public class BlockFactoryTest
	{
		public BlockFactoryTest()
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

		#region Generate Block Tests
		[TestMethod]
		public void TestGenerateBlockParseWithAOperand()
		{
			String sGeneratePlainTextBlock = "	GENERATE	120";

			GenerateBlock oGenerate = new GenerateBlock( 1, 1, sGeneratePlainTextBlock );

			Assert.IsTrue( oGenerate.Parse() == BlockParseResult.PARSED_OK );
			Assert.IsTrue(	oGenerate.OperandA.HasValidValue && oGenerate.OperandB == null &&
							oGenerate.OperandC == null && oGenerate.OperandD == null &&
							oGenerate.OperandE == null );
			Assert.AreEqual( 120, oGenerate.OperandA.PosInteger );
		}

		[TestMethod]
		public void TestGenerateBlockParseWithABOperands()
		{
			String sGeneratePlainTextBlock = "	GENERATE	120,30";

			GenerateBlock oGenerate = new GenerateBlock( 1, 1, sGeneratePlainTextBlock );

			Assert.IsTrue( oGenerate.Parse() == BlockParseResult.PARSED_OK );
			Assert.IsTrue( oGenerate.OperandA.HasValidValue && oGenerate.OperandB.HasValidValue &&
							oGenerate.OperandC == null && oGenerate.OperandD == null &&
							oGenerate.OperandE == null );
			Assert.AreEqual( 120, oGenerate.OperandA.PosInteger );
			Assert.AreEqual( 30, oGenerate.OperandB.PosInteger );
		}

		[TestMethod]
		public void TestGenerateBlockParseWithBOperandAsFunctionModifier()
		{
			String sGeneratePlainTextBlock = "	GENERATE	120,FN$FMULT0";

			GenerateBlock oGenerate = new GenerateBlock( 1, 1, sGeneratePlainTextBlock );

			Assert.IsTrue( oGenerate.Parse() == BlockParseResult.PARSED_OK );
			Assert.IsTrue(	oGenerate.OperandA.HasValidValue && oGenerate.OperandB.HasValidValue &&
							oGenerate.OperandC == null && oGenerate.OperandD == null &&
							oGenerate.OperandE == null );
			Assert.AreEqual( 120, oGenerate.OperandA.PosInteger );
			Assert.IsTrue( oGenerate.OperandB.SNA.Type == SNAType.Function && oGenerate.OperandB.SNA.Parameter.Value == "FMULT0" );
		}

		[TestMethod]
		public void TestGenerateBlockParseWithABCOperands()
		{
			String sGeneratePlainTextBlock = "	GENERATE	120,30,200";

			GenerateBlock oGenerate = new GenerateBlock( 1, 1, sGeneratePlainTextBlock );

			Assert.IsTrue( oGenerate.Parse() == BlockParseResult.PARSED_OK );
			Assert.IsTrue(	oGenerate.OperandA.HasValidValue && oGenerate.OperandB.HasValidValue &&
							oGenerate.OperandC.HasValidValue && oGenerate.OperandD == null &&
							oGenerate.OperandE == null );
			Assert.AreEqual( 120, oGenerate.OperandA.PosInteger);
			Assert.AreEqual( 30, oGenerate.OperandB.PosInteger );
			Assert.AreEqual( 200, oGenerate.OperandC.PosInteger );
		}

		[TestMethod]
		public void TestGenerateBlockParseWithABCDOperands()
		{
			String sGeneratePlainTextBlock = "	GENERATE	120,30,200,50";

			GenerateBlock oGenerate = new GenerateBlock( 1, 1, sGeneratePlainTextBlock );

			Assert.IsTrue( oGenerate.Parse() == BlockParseResult.PARSED_OK );
			Assert.IsTrue(	oGenerate.OperandA.HasValidValue && oGenerate.OperandB.HasValidValue &&
							oGenerate.OperandC.HasValidValue && oGenerate.OperandD.HasValidValue &&
							oGenerate.OperandE == null );
			Assert.AreEqual( 120, oGenerate.OperandA.PosInteger );
			Assert.AreEqual( 30, oGenerate.OperandB.PosInteger );
			Assert.AreEqual( 200, oGenerate.OperandC.PosInteger );
			Assert.AreEqual( 50, oGenerate.OperandD.PosInteger );
		}

		[TestMethod]
		public void TestGenerateBlockParseWithABCDEOperands()
		{
			String sGeneratePlainTextBlock = "	GENERATE	120,30,200,50,3";

			GenerateBlock oGenerate = new GenerateBlock( 1, 1, sGeneratePlainTextBlock );

			Assert.IsTrue( oGenerate.Parse() == BlockParseResult.PARSED_OK );
			Assert.IsTrue(	oGenerate.OperandA.HasValidValue && oGenerate.OperandB.HasValidValue &&
							oGenerate.OperandC.HasValidValue && oGenerate.OperandD.HasValidValue &&
							oGenerate.OperandE.HasValidValue );
			Assert.AreEqual( 120, oGenerate.OperandA.PosInteger );
			Assert.AreEqual( 30, oGenerate.OperandB.PosInteger );
			Assert.AreEqual( 200, oGenerate.OperandC.PosInteger );
			Assert.AreEqual( 50, oGenerate.OperandD.PosInteger );
			Assert.AreEqual( 3, oGenerate.OperandE.PosInteger );
		}
		#endregion

		#region Advance Block Tests
		[TestMethod]
		public void TestAdvanceBlockParseWithLabelAndOperandA()
		{
			String sAdvancePlainTextBlock = "WALK	ADVANCE	60";

			AdvanceBlock oAdvance = new AdvanceBlock( 2, 2, sAdvancePlainTextBlock );

			Assert.IsTrue( oAdvance.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "WALK", oAdvance.Label );
			Assert.IsTrue( oAdvance.OperandA.IsPosInteger && oAdvance.OperandB == null );
			Assert.AreEqual( 60, oAdvance.OperandA.PosInteger );
		}

		[TestMethod]
		public void TestAdvanceBlockParseWithLabelAndOperandsAB()
		{
			String sAdvancePlainTextBlock = "WALK	ADVANCE 60,10";

			AdvanceBlock oAdvance = new AdvanceBlock( 2, 2, sAdvancePlainTextBlock );

			Assert.IsTrue( oAdvance.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "WALK", oAdvance.Label );
			Assert.IsTrue( oAdvance.OperandA.IsPosInteger && oAdvance.OperandB.IsPosInteger );
			Assert.AreEqual( 60, oAdvance.OperandA.PosInteger );
			Assert.AreEqual( 10, oAdvance.OperandB.PosInteger );
		}

		[TestMethod]
		public void TestAdvanceBlockParseWithNoLabelAndOperandA()
		{
			String sAdvancePlainTextBlock = "	ADVANCE	60";

			AdvanceBlock oAdvance = new AdvanceBlock( 2, 2, sAdvancePlainTextBlock );

			Assert.IsTrue( oAdvance.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oAdvance.Label );
			Assert.IsTrue( oAdvance.OperandA.IsPosInteger && oAdvance.OperandB == null );
			Assert.AreEqual( 60, oAdvance.OperandA.PosInteger );
		}

		[TestMethod]
		public void TestAdvanceBlockParseWithNoLabelAndOperandsAB()
		{
			String sAdvancePlainTextBlock = "	ADVANCE	60,10";

			AdvanceBlock oAdvance = new AdvanceBlock( 2, 2, sAdvancePlainTextBlock );

			Assert.IsTrue( oAdvance.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oAdvance.Label );
			Assert.IsTrue( oAdvance.OperandA.IsPosInteger && oAdvance.OperandB.IsPosInteger );
			Assert.AreEqual( 60, oAdvance.OperandA.PosInteger );
			Assert.AreEqual( 10, oAdvance.OperandB.PosInteger );
		}
		#endregion

		#region Terminate Block Tests
		[TestMethod]
		public void TestTerminateBlockParseWithLabelAndOperandA()
		{
			String sTerminatePlainTextBlock = "FIN	TERMINATE 1";

			TerminateBlock oTerminate = new TerminateBlock( 10, 10, sTerminatePlainTextBlock );

			Assert.IsTrue( oTerminate.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "FIN", oTerminate.Label );
			Assert.AreEqual( "1", oTerminate.OperandA );
		}

		[TestMethod]
		public void TestTerminateBlockParseWithLabelAndNoOperand()
		{
			String sTerminatePlainTextBlock = "FIN	TERMINATE";

			TerminateBlock oTerminate = new TerminateBlock( 10, 10, sTerminatePlainTextBlock );

			Assert.IsTrue( oTerminate.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "FIN", oTerminate.Label );
			Assert.AreEqual( "", oTerminate.OperandA );
		}

		[TestMethod]
		public void TestTerminateBlockParseWithNoLabelAndOperandA()
		{
			String sTerminatePlainTextBlock = "		TERMINATE 1";

			TerminateBlock oTerminate = new TerminateBlock( 10, 10, sTerminatePlainTextBlock );

			Assert.IsTrue( oTerminate.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oTerminate.Label );
			Assert.AreEqual( "1", oTerminate.OperandA );
		}

		[TestMethod]
		public void TestTerminateBlockParseWithNoLabelAndNoOperand()
		{
			String sTerminatePlainTextBlock = "		TERMINATE";

			TerminateBlock oTerminate = new TerminateBlock( 10, 10, sTerminatePlainTextBlock );

			Assert.IsTrue( oTerminate.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oTerminate.Label );
			Assert.AreEqual( "", oTerminate.OperandA );
		}
		#endregion

		#region Storage Block Tests
		[TestMethod]
		public void TestStorageBlockParseWithLabelAndOperandA()
		{
			String sStoragePlainTextBlock = "SALON	STORAGE 10";

			StorageBlock oStorage = new StorageBlock( 3, 3, sStoragePlainTextBlock );

			Assert.IsTrue( oStorage.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "SALON", oStorage.Label );
			Assert.AreEqual( "10", oStorage.OperandA );
		}
		#endregion

		#region Enter Block Tests
		[TestMethod]
		public void TestEnterBlockParseWithLabelAndOperandA()
		{
			String sEnterPlainTextBlock = "ADENTRO	ENTER	SALON";

			EnterBlock oEnter = new EnterBlock( 2, 2, sEnterPlainTextBlock );

			Assert.IsTrue( oEnter.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "ADENTRO", oEnter.Label );
			Assert.AreEqual( "SALON", oEnter.OperandA );
			Assert.AreEqual( "", oEnter.OperandB );
		}

		[TestMethod]
		public void TestEnterBlockParseWithLabelAndOperandsAB()
		{
			String sEnterPlainTextBlock = "ADENTRO	ENTER	SALON,2";

			EnterBlock oEnter = new EnterBlock( 2, 2, sEnterPlainTextBlock );

			Assert.IsTrue( oEnter.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "ADENTRO", oEnter.Label );
			Assert.AreEqual( "SALON", oEnter.OperandA );
			Assert.AreEqual( "2", oEnter.OperandB );
		}

		[TestMethod]
		public void TestEnterBlockParseWithNoLabelAndOperandA()
		{
			String sEnterPlainTextBlock = "	ENTER	SALON";

			EnterBlock oEnter = new EnterBlock( 2, 2, sEnterPlainTextBlock );

			Assert.IsTrue( oEnter.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oEnter.Label );
			Assert.AreEqual( "SALON", oEnter.OperandA );
			Assert.AreEqual( "", oEnter.OperandB );
		}

		[TestMethod]
		public void TestEnterBlockParseWithNoLabelAndOperandsAB()
		{
			String sEnterPlainTextBlock = "	ENTER	SALON,2";

			EnterBlock oEnter = new EnterBlock( 2, 2, sEnterPlainTextBlock );

			Assert.IsTrue( oEnter.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oEnter.Label );
			Assert.AreEqual( "SALON", oEnter.OperandA );
			Assert.AreEqual( "2", oEnter.OperandB );
		}
		#endregion

		#region Leave Block Tests
		[TestMethod]
		public void TestLeaveBlockParseWithLabelAndOperandA()
		{
			String sLeavePlainTextBlock = "SALIR	LEAVE	SALON";

			LeaveBlock oLeave = new LeaveBlock( 4, 4, sLeavePlainTextBlock );

			Assert.IsTrue( oLeave.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "SALIR", oLeave.Label );
			Assert.AreEqual( "SALON", oLeave.OperandA );
			Assert.AreEqual( "", oLeave.OperandB );
		}

		[TestMethod]
		public void TestLeaveBlockParseWithLabelAndOperandsAB()
		{
			String sLeavePlainTextBlock = "SALIR	LEAVE	SALON,1";

			LeaveBlock oLeave = new LeaveBlock( 4, 4, sLeavePlainTextBlock );

			Assert.IsTrue( oLeave.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "SALIR", oLeave.Label );
			Assert.AreEqual( "SALON", oLeave.OperandA );
			Assert.AreEqual( "1", oLeave.OperandB );
		}

		[TestMethod]
		public void TestLeaveBlockParseWithNoLabelAndOperandA()
		{
			String sLeavePlainTextBlock = "	LEAVE	SALON";

			LeaveBlock oLeave = new LeaveBlock( 4, 4, sLeavePlainTextBlock );

			Assert.IsTrue( oLeave.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oLeave.Label );
			Assert.AreEqual( "SALON", oLeave.OperandA );
			Assert.AreEqual( "", oLeave.OperandB );
		}

		[TestMethod]
		public void TestLeaveBlockParseWithNoLabelAndOperandsAB()
		{
			String sLeavePlainTextBlock = "	LEAVE	SALON,1";

			LeaveBlock oLeave = new LeaveBlock( 4, 4, sLeavePlainTextBlock );

			Assert.IsTrue( oLeave.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oLeave.Label );
			Assert.AreEqual( "SALON", oLeave.OperandA );
			Assert.AreEqual( "1", oLeave.OperandB );
		}
		#endregion

		#region Seize Block Tests
		[TestMethod]
		public void TestSeizeBlockParseWithLabelAndOperandA()
		{
			String sSeizePlainTextBlock = "PREGUNTAR	SEIZE	SECRETARIO";

			SeizeBlock oSeize = new SeizeBlock( 7, 7, sSeizePlainTextBlock );

			Assert.IsTrue( oSeize.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "PREGUNTAR", oSeize.Label );
			Assert.AreEqual( "SECRETARIO", oSeize.OperandA );
		}

		[TestMethod]
		public void TestSeizeBlockParseWithNoLabelAndOperandA()
		{
			String sEnterPlainTextBlock = "	SEIZE	SECRETARIO";

			SeizeBlock oEnter = new SeizeBlock( 7, 7, sEnterPlainTextBlock );

			Assert.IsTrue( oEnter.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oEnter.Label );
			Assert.AreEqual( "SECRETARIO", oEnter.OperandA );
		}
		#endregion

		#region Release Block Tests
		[TestMethod]
		public void TestReleaseBlockParseWithLabelAndOperandA()
		{
			String sReleasePlainTextBlock = "PREGUNTAR	RELEASE	SECRETARIO";

			ReleaseBlock oRelease = new ReleaseBlock( 10, 10, sReleasePlainTextBlock );

			Assert.IsTrue( oRelease.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "PREGUNTAR", oRelease.Label );
			Assert.AreEqual( "SECRETARIO", oRelease.OperandA );
		}

		[TestMethod]
		public void TestReleaseBlockParseWithNoLabelAndOperandA()
		{
			String sReleasePlainTextBlock = "	RELEASE	SECRETARIO";

			ReleaseBlock oRelease = new ReleaseBlock( 10, 10, sReleasePlainTextBlock );

			Assert.IsTrue( oRelease.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oRelease.Label );
			Assert.AreEqual( "SECRETARIO", oRelease.OperandA );
		}
		#endregion

		#region Queue Block Tests
		[TestMethod]
		public void TestQueueBlockParseWithLabelAndOperandA()
		{
			String sQueuePlainTextBlock = "ENCOLAR	QUEUE	COLA";

			QueueBlock oQueue = new QueueBlock( 2, 2, sQueuePlainTextBlock );

			Assert.IsTrue( oQueue.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "ENCOLAR", oQueue.Label );
			Assert.AreEqual( "COLA", oQueue.OperandA );
			Assert.AreEqual( "", oQueue.OperandB );
		}

		[TestMethod]
		public void TestQueueBlockParseWithLabelAndOperandsAB()
		{
			String sQueuePlainTextBlock = "ENCOLAR	QUEUE	COLA,2";

			QueueBlock oQueue = new QueueBlock( 2, 2, sQueuePlainTextBlock );

			Assert.IsTrue( oQueue.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "ENCOLAR", oQueue.Label );
			Assert.AreEqual( "COLA", oQueue.OperandA );
			Assert.AreEqual( "2", oQueue.OperandB );
		}

		[TestMethod]
		public void TestQueueBlockParseWithNoLabelAndOperandA()
		{
			String sQueuePlainTextBlock = "	QUEUE	COLA";

			QueueBlock oQueue = new QueueBlock( 2, 2, sQueuePlainTextBlock );

			Assert.IsTrue( oQueue.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oQueue.Label );
			Assert.AreEqual( "COLA", oQueue.OperandA );
			Assert.AreEqual( "", oQueue.OperandB );
		}

		[TestMethod]
		public void TestQueueBlockParseWithNoLabelAndOperandsAB()
		{
			String sQueuePlainTextBlock = "	QUEUE	COLA,2";

			QueueBlock oQueue = new QueueBlock( 2, 2, sQueuePlainTextBlock );

			Assert.IsTrue( oQueue.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oQueue.Label );
			Assert.AreEqual( "COLA", oQueue.OperandA );
			Assert.AreEqual( "2", oQueue.OperandB );
		}
		#endregion

		#region Depart Block Tests
		[TestMethod]
		public void TestDepartBlockParseWithLabelAndOperandA()
		{
			String sDepartPlainTextBlock = "DESENCOLAR	DEPART	COLA";

			DepartBlock oDepart = new DepartBlock( 5, 5, sDepartPlainTextBlock );

			Assert.IsTrue( oDepart.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "DESENCOLAR", oDepart.Label );
			Assert.AreEqual( "COLA", oDepart.OperandA );
			Assert.AreEqual( "", oDepart.OperandB );
		}

		[TestMethod]
		public void TestDepartBlockParseWithLabelAndOperandsAB()
		{
			String sDepartPlainTextBlock = "DESENCOLAR	DEPART	COLA,2";

			DepartBlock oDepart = new DepartBlock( 5, 5, sDepartPlainTextBlock );

			Assert.IsTrue( oDepart.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "DESENCOLAR", oDepart.Label );
			Assert.AreEqual( "COLA", oDepart.OperandA );
			Assert.AreEqual( "2", oDepart.OperandB );
		}

		[TestMethod]
		public void TestDepartBlockParseWithNoLabelAndOperandA()
		{
			String sDepartPlainTextBlock = "	DEPART	COLA";

			DepartBlock oDepart = new DepartBlock( 5, 5, sDepartPlainTextBlock );

			Assert.IsTrue( oDepart.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oDepart.Label );
			Assert.AreEqual( "COLA", oDepart.OperandA );
			Assert.AreEqual( "", oDepart.OperandB );
		}

		[TestMethod]
		public void TestDepartBlockParseWithNoLabelAndOperandsAB()
		{
			String sDepartPlainTextBlock = "	DEPART	COLA,2";

			DepartBlock oDepart = new DepartBlock( 5, 5, sDepartPlainTextBlock );

			Assert.IsTrue( oDepart.Parse() == BlockParseResult.PARSED_OK );
			Assert.AreEqual( "", oDepart.Label );
			Assert.AreEqual( "COLA", oDepart.OperandA );
			Assert.AreEqual( "2", oDepart.OperandB );
		}
		#endregion

		#region Multiple Blocks Tests
		[TestMethod]
		public void TestGenerateAdvanceTerminateModelWithNoErrors()
		{
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "	GENERATE	120,30", Environment.NewLine );
			strGPSSModel += String.Concat( "	ADVANCE		20,5", Environment.NewLine );
			strGPSSModel += String.Concat( "	TERMINATE	1", Environment.NewLine );

			List<Block> viperModel = BlockFactory.Instance().CreateModel( strGPSSModel );

			Assert.IsTrue( String.IsNullOrEmpty( BlockFactory.Instance().ErrorMessageLog ) );
		}

		[TestMethod]
		public void TestGenerateEnterAdvanceLeaveTerminateModelWithNoErrors()
		{
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "	GENERATE	120,30", Environment.NewLine );
			strGPSSModel += String.Concat( "	ENTER		SALON,2", Environment.NewLine );
			strGPSSModel += String.Concat( "	ADVANCE		20,5", Environment.NewLine );
			strGPSSModel += String.Concat( "	LEAVE		SALON,2", Environment.NewLine );
			strGPSSModel += String.Concat( "	TERMINATE	1", Environment.NewLine );

			List<Block> viperModel = BlockFactory.Instance().CreateModel( strGPSSModel );

			Assert.IsTrue( String.IsNullOrEmpty( BlockFactory.Instance().ErrorMessageLog ) );
		}

		[TestMethod]
		public void TestGenerateSeizeAdvanceReleaseTerminateModelWithNoErrors()
		{
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "	GENERATE	120,30", Environment.NewLine );
			strGPSSModel += String.Concat( "	SEIZE		CAJA", Environment.NewLine );
			strGPSSModel += String.Concat( "	ADVANCE		20,5", Environment.NewLine );
			strGPSSModel += String.Concat( "	RELEASE		CAJA", Environment.NewLine );
			strGPSSModel += String.Concat( "	TERMINATE	1", Environment.NewLine );

			List<Block> viperModel = BlockFactory.Instance().CreateModel( strGPSSModel );

			Assert.IsTrue( String.IsNullOrEmpty( BlockFactory.Instance().ErrorMessageLog ) );
		}

		[TestMethod]
		public void TestGenerateQueueSeizeDepartAdvanceReleaseTerminateModelWithNoErrors()
		{
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "	GENERATE	120,30", Environment.NewLine );
			strGPSSModel += String.Concat( "	QUEUE		COLA", Environment.NewLine );
			strGPSSModel += String.Concat( "	SEIZE		CAJA", Environment.NewLine );
			strGPSSModel += String.Concat( "	DEPART		COLA", Environment.NewLine );
			strGPSSModel += String.Concat( "	ADVANCE		20,5", Environment.NewLine );
			strGPSSModel += String.Concat( "	RELEASE		CAJA", Environment.NewLine );
			strGPSSModel += String.Concat( "	TERMINATE	1", Environment.NewLine );

			List<Block> viperModel = BlockFactory.Instance().CreateModel( strGPSSModel );

			Assert.IsTrue( String.IsNullOrEmpty( BlockFactory.Instance().ErrorMessageLog ) );
		}

		[TestMethod]
		public void TestGenerateEnterQueueSeizeDepartAdvanceReleaseLeaveTerminateModelWithNoErrors()
		{
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "	GENERATE	120,30", Environment.NewLine );
			strGPSSModel += String.Concat( "	ENTER		SALON", Environment.NewLine );
			strGPSSModel += String.Concat( "	ADVANCE		15,2", Environment.NewLine );
			strGPSSModel += String.Concat( "	QUEUE		COLA", Environment.NewLine );
			strGPSSModel += String.Concat( "	SEIZE		CAJA", Environment.NewLine );
			strGPSSModel += String.Concat( "	DEPART		COLA", Environment.NewLine );
			strGPSSModel += String.Concat( "	ADVANCE		20,5", Environment.NewLine );
			strGPSSModel += String.Concat( "	RELEASE		CAJA", Environment.NewLine );
			strGPSSModel += String.Concat( "	LEAVE		SALON", Environment.NewLine );
			strGPSSModel += String.Concat( "	TERMINATE	1", Environment.NewLine );

			List<Block> viperModel = BlockFactory.Instance().CreateModel( strGPSSModel );

			Assert.IsTrue( String.IsNullOrEmpty( BlockFactory.Instance().ErrorMessageLog ) );
		}
		#endregion
	}
}
