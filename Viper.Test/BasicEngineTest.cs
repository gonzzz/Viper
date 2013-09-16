using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Viper.Framework.Engine;

namespace Viper.Test
{
	[TestClass]
	public class BasicEngineTest
	{
		[TestMethod]
		public void TestSimulationStart()
		{
			// 1) Create basic GPSS Model
			String strGPSSModel = String.Empty;
			strGPSSModel += String.Concat( "		GENERATE	120,20", Environment.NewLine );
			strGPSSModel += String.Concat( "		ADVANCE		75,35", Environment.NewLine );
			strGPSSModel += String.Concat( "		TERMINATE	1", Environment.NewLine );

			ViperSystem.Instance().Simulate( strGPSSModel, 20 );

			Assert.AreEqual( ViperSystem.Instance().TerminationCount, 0 );
			Assert.AreNotEqual( ViperSystem.Instance().TransactionCounter, 0 );
		}
	}
}
