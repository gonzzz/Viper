using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Viper.Framework.Engine;
using System.Diagnostics;

namespace Viper.Test
{
	[TestClass]
	public class BasicEngineTest
	{
		[TestMethod]
		public void TestSimulationGenerateAdvanceTerminate()
		{
			// 1) Create basic GPSS Model
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "		GENERATE	30,5", Environment.NewLine );
			strGPSSModel += String.Concat( "		ADVANCE		75,25", Environment.NewLine );
			strGPSSModel += String.Concat( "		TERMINATE	1", Environment.NewLine );

			ViperSystem.Instance().Simulate( strGPSSModel, 30 );

			Assert.AreEqual( ViperSystem.Instance().TerminationCount, 0 );
			Assert.AreNotEqual( ViperSystem.Instance().TransactionCounter, 0 );

			String strReport = ViperSystem.Instance().GetFinalReport();

			Trace.Write( strReport );
		}


		[TestMethod]
		public void TestSimulationGenerateAdvanceTerminateWithStorage()
		{
			// 1) Create basic GPSS Model
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "SALON	STORAGE		10", Environment.NewLine );
			strGPSSModel += String.Concat( "		GENERATE	30,5", Environment.NewLine );
			strGPSSModel += String.Concat( "		ENTER		SALON", Environment.NewLine );
			strGPSSModel += String.Concat( "		ADVANCE		75,25", Environment.NewLine );
			strGPSSModel += String.Concat( "		LEAVE		SALON", Environment.NewLine );
			strGPSSModel += String.Concat( "		TERMINATE	1", Environment.NewLine );

			ViperSystem.Instance().Simulate( strGPSSModel, 30 );

			Assert.AreEqual( ViperSystem.Instance().TerminationCount, 0 );
			Assert.AreNotEqual( ViperSystem.Instance().TransactionCounter, 0 );

			String strReport = ViperSystem.Instance().GetFinalReport();

			Trace.Write( strReport );
		}


		[TestMethod]
		public void TestSimulationGenerateAdvanceTerminateWithStorageAndQueue()
		{
			// 1) Create basic GPSS Model
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "SALON	STORAGE		10", Environment.NewLine );
			strGPSSModel += String.Concat( "		GENERATE	30,5", Environment.NewLine );
			strGPSSModel += String.Concat( "		QUEUE		COLA", Environment.NewLine );
			strGPSSModel += String.Concat( "		ENTER		SALON", Environment.NewLine );
			strGPSSModel += String.Concat( "		DEPART		COLA", Environment.NewLine );
			strGPSSModel += String.Concat( "		ADVANCE		75,25", Environment.NewLine );
			strGPSSModel += String.Concat( "		LEAVE		SALON", Environment.NewLine );
			strGPSSModel += String.Concat( "		TERMINATE	1", Environment.NewLine );

			ViperSystem.Instance().Simulate( strGPSSModel, 30 );

			Assert.AreEqual( ViperSystem.Instance().TerminationCount, 0 );
			Assert.AreNotEqual( ViperSystem.Instance().TransactionCounter, 0 );

			String strReport = ViperSystem.Instance().GetFinalReport();

			Trace.Write( strReport );
		}

		[TestMethod]
		public void TestSimulationGenerateAdvanceTerminateWithFacilityAndQueue()
		{
			// 1) Create basic GPSS Model
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "		GENERATE	30,5", Environment.NewLine );
			strGPSSModel += String.Concat( "		QUEUE		COLA", Environment.NewLine );
			strGPSSModel += String.Concat( "		SEIZE		VENDEDOR", Environment.NewLine );
			strGPSSModel += String.Concat( "		DEPART		COLA", Environment.NewLine );
			strGPSSModel += String.Concat( "		ADVANCE		45,15", Environment.NewLine );
			strGPSSModel += String.Concat( "		RELEASE		VENDEDOR", Environment.NewLine );
			strGPSSModel += String.Concat( "		TERMINATE	1", Environment.NewLine );

			ViperSystem.Instance().Simulate( strGPSSModel, 30 );

			Assert.AreEqual( ViperSystem.Instance().TerminationCount, 0 );
			Assert.AreNotEqual( ViperSystem.Instance().TransactionCounter, 0 );

			String strReport = ViperSystem.Instance().GetFinalReport();

			Trace.Write( strReport );
		}

	}
}
