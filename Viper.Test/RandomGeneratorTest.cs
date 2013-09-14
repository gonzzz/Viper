using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Viper.Framework.Utils;

namespace Viper.Test
{
    /// <summary>
    ///This is a test class for RandomGeneratorTest and is intended
    ///to contain all RandomGeneratorTest Unit Tests
    ///</summary>
	[TestClass()]
	public class RandomGeneratorTest
	{
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
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion

		[TestMethod]
		public void TestRandomGeneratorSameFamiliesThrowsTwoDifferentNumbers()
		{
			double dRandom1 = RandomGenerator.Instance().GenerateRandom( 0 );
			double dRandom2 = RandomGenerator.Instance().GenerateRandom( 0 );

			Assert.AreNotEqual( dRandom1 , dRandom2 );
		}

		[TestMethod]
		public void TestRandomGeneratorDifferentFamiliesThrowsTwoDifferentNumbers()
		{
			double dRandom1 = RandomGenerator.Instance().GenerateRandom( 0 );
			double dRandom2 = RandomGenerator.Instance().GenerateRandom( 1 );

			Assert.AreNotEqual( dRandom1 , dRandom2 );
		}

		[TestMethod]
		public void TestRandomGeneratorThrowsAllDifferentNumbers()
		{
			List<double> randomNumbers = new List<double>();

			for( int i = 0; i < 100; i++ )
			{
				randomNumbers.Add( RandomGenerator.Instance().GenerateRandom( 0 ) );
			}

			List<double> differentNumbers =  (  from rm in randomNumbers
								  				select rm ).Distinct().ToList();

			Assert.AreEqual( 100, differentNumbers.Count );							  
		}

		[TestMethod]
		public void TestRandomGeneratorGetDifferentRandomDesviations()
		{
			int iRandomDesviation1 = RandomGenerator.Instance().GenerateRandomWithDesviation( 0 , 10 );
			int iRandomDesviation2 = RandomGenerator.Instance().GenerateRandomWithDesviation( 0 , 10 );

			Assert.AreNotEqual( iRandomDesviation1 , iRandomDesviation2 );
		}
	}
}
