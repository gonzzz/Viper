using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Viper.Framework.Blocks;
using Viper.Framework.Engine;
using Viper.Framework.Entities;

namespace Viper.Test
{
	/// <summary>
	/// Summary description for SystemTest
	/// </summary>
	[TestClass]
	public class SystemTest
	{
		public SystemTest()
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
		[TestMethod]
		public void TestErrorHandlingInBlockCreationAreCaptured()
		{
			// 1) Create basic GPSS Model with a Storage
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "SALON	STORAGE		200" , Environment.NewLine );
			strGPSSModel += String.Concat( "		GENERATE	120,30" , Environment.NewLine );
			strGPSSModel += String.Concat( "		ENTER		SALON,2,3" , Environment.NewLine ); // Wrong Number of Operands
			strGPSSModel += String.Concat( "		ADVANCE		*8" , Environment.NewLine );
			strGPSSModel += String.Concat( "		LEAVE		SALON,2" , Environment.NewLine );
			strGPSSModel += String.Concat( "		TERMINATOR	1" , Environment.NewLine ); // Invalid Block

			// 2) Create Block Model
			List<Block> viperModel = BlockFactory.Instance().CreateModel( strGPSSModel );
			
			// 3) Verify it has errors!
			List<String> strErrors = BlockFactory.Instance().ErrorMessageLog;
			Assert.IsTrue( strErrors.Count == 2 );
		}

		[TestMethod]
		public void TestErrorHandlingInBlockCreationAreCaptured2()
		{
			// 1) Create basic GPSS Model with a Storage
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "SALON	STORAGE		200" , Environment.NewLine );
			strGPSSModel += String.Concat( "		GENERATE	120,30,,,3,BLEH" , Environment.NewLine ); // Wrong Number of Operands
			strGPSSModel += String.Concat( "		ENTER		SALON" , Environment.NewLine ); 
			strGPSSModel += String.Concat( "		ADVANCE		*8,FN$2,34,2" , Environment.NewLine ); // Wrong Number of Operands
			strGPSSModel += String.Concat( "		LEAVE		SALON" , Environment.NewLine );
			strGPSSModel += String.Concat( "		TERMINATE" ); 

			// 2) Create Block Model
			List<Block> viperModel = BlockFactory.Instance().CreateModel( strGPSSModel );

			// 3) Verify it has errors!
			List<String> strErrors = BlockFactory.Instance().ErrorMessageLog;
			Assert.IsTrue( strErrors.Count == 2 );
		}

		[TestMethod]
		public void TestNonTransactionalBlocksStorageCreationInSystemModel()
		{
			// 1) Create basic GPSS Model with a Storage
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "SALON	STORAGE		100", Environment.NewLine );
			strGPSSModel += String.Concat( "		GENERATE	120,30", Environment.NewLine );
			strGPSSModel += String.Concat( "		ENTER		SALON", Environment.NewLine );
			strGPSSModel += String.Concat( "		ADVANCE		15,2", Environment.NewLine );
			strGPSSModel += String.Concat( "		LEAVE		SALON", Environment.NewLine );
			strGPSSModel += String.Concat( "		TERMINATE	1", Environment.NewLine );

			// 2) Create Block Model
			List<Block> viperModel = BlockFactory.Instance().CreateModel( strGPSSModel );
			Assert.IsTrue( BlockFactory.Instance().ErrorMessageLog.Count == 0 );

			// 3) Create Viper Model with Entities
			Model oModel = new Model();
			
			// 3a) Get non transactional blocks (STORAGE, FUNCTION, VARIABLE, FVARIABLE, BVARIABLE, EQU, MATRIX, TABLE, QTABLE, INITIAL)
			var oNonTransactionalBlocks = ( from b in viperModel
											where b.Executable == false
											select b ).ToList();

			foreach( Block block in oNonTransactionalBlocks )
			{
				// If Non-Transactional block is a Storage Block => Create Storage Entity 
				if( block is StorageBlock )
				{
					StorageBlock storageBlock = block as StorageBlock;

					// Create new Storage in Model with Storage Block parameters
					Storage newStorage = new Storage( storageBlock.Label, storageBlock.OperandA.PosInteger );
					oModel.AddStorage( newStorage );
				}
			}

			// Storage with name 'SALON' should have been created in the model
			Assert.IsNotNull( oModel.GetStorageByName( "SALON" ) );

			// Storage number 1 should have been created in the model
			Assert.IsNotNull( oModel.GetStorageByNumber( 1 ) );
		}
	}
}
