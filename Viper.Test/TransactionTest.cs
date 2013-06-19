using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Viper.Framework.Entities;
using Viper.Framework.Enums;
using Viper.Framework.Utils;

namespace Viper.Test
{
	/// <summary>
	/// Summary description for TransactionTest
	/// </summary>
	[TestClass]
	public class TransactionTest
	{
		public TransactionTest()
		{
			
		}

		private TestContext testContextInstance;
		private Transaction m_oDefaultTransaction;
		private int m_iDefaultParameter;
		private int m_iDefaultParameterUpdateValue;

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
		[TestInitialize()]
		public void MyTestInitialize() 
		{
			m_oDefaultTransaction = new Transaction( 1 );
			m_iDefaultParameter = 20;
			m_iDefaultParameterUpdateValue = 250;
		}
		
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		#region Default Constructor Tests
		[TestMethod]
		public void TestDefaultConstructorTransactionNotPreemptedNorDelayedNorTraceEnabled()
		{
			// Default Created transaction should have the following state:
			// IsPreempted = false, IsDelayed = false, IsTraceEnabled = false,
			// Marktime = 0, Current/Next Block = null, Number = 1
			// State = SUSPENDED, Priority = DEFAULT (0)

			// None of this properties should be true:
			Assert.AreEqual( false, m_oDefaultTransaction.IsPreempted || m_oDefaultTransaction.IsDelayed || m_oDefaultTransaction.IsTraceEnabled );
		}

		[TestMethod]
		public void TestDefaultConstructorTransactionIsSuspended()
		{
			// Default Created transaction should have the following state:
			// IsPreempted = false, IsDelayed = false, IsTraceEnabled = false,
			// Marktime = 0, Current/Next Block = null, Number = 1
			// State = SUSPENDED, Priority = DEFAULT (0)

			// Transaction should be SUSPENDED:
			Assert.AreEqual( TransactionState.SUSPENDED, m_oDefaultTransaction.State );
		}

		[TestMethod]
		public void TestDefaultConstructorTransactionMarkTimeIsZero()
		{
			// Created transaction should have the following state:
			// IsPreempted = false, IsDelayed = false, IsTraceEnabled = false,
			// Marktime = 0, Current/Next Block = null, Number = 1
			// State = SUSPENDED, Priority = DEFAULT (0)

			// Transaction should be zero (0):
			Assert.AreEqual( 0, m_oDefaultTransaction.MarkTime );
		}

		[TestMethod]
		public void TestDefaultConstructorTransactionPriorityIsZero()
		{
			// Default Created transaction should have the following state:
			// IsPreempted = false, IsDelayed = false, IsTraceEnabled = false,
			// Marktime = 0, Current/Next Block = null, Number = 1
			// State = SUSPENDED, Priority = DEFAULT (0)

			// Transaction should be zero (0):
			Assert.AreEqual( Constants.DEFAULT_PRIORITY, m_oDefaultTransaction.Priority );
		}

		[TestMethod]
		public void TestDefaultConstructorTransactionCurrentBlockIsNull()
		{
			// Default Created transaction should have the following state:
			// IsPreempted = false, IsDelayed = false, IsTraceEnabled = false,
			// Marktime = 0, Current/Next Block = null, Number = 1
			// State = SUSPENDED, Priority = DEFAULT (0)

			// Transaction should be zero (0):
			Assert.IsNull( m_oDefaultTransaction.CurrentBlock );
		}

		[TestMethod]
		public void TestDefaultConstructorTransactionNextBlockIsNull()
		{
			// Default Created transaction should have the following state:
			// IsPreempted = false, IsDelayed = false, IsTraceEnabled = false,
			// Marktime = 0, Current/Next Block = null, Number = 1
			// State = SUSPENDED, Priority = DEFAULT (0)

			// Transaction should be zero (0):
			Assert.IsNull( m_oDefaultTransaction.NextBlock );
		}
		#endregion

		#region Parameters Tests
		[TestMethod]
		public void TestParameterOfTransactionCreatedIfNotExists()
		{
			// Default Created transaction shouldn't have any parameter. Default parameter
			// should be created and assigned a value of zero (0).

			Assert.AreEqual( Constants.DEFAULT_ZERO_VALUE, m_oDefaultTransaction.GetParameter( m_iDefaultParameter ) );
		}

		[TestMethod]
		public void TestParameterOfTransactionUpdatedAndReturnedOK()
		{
			// Default Created transaction shouldn't have any parameter. A default parameter is created with a zero (0)
			// value. Then it is assigned a new value of: current value + default update value.
			// The new parameter value should be updated ok

			int iCurrentParameterValue = m_oDefaultTransaction.GetParameter( m_iDefaultParameter ) + m_iDefaultParameterUpdateValue;
			m_oDefaultTransaction.SetParameter( m_iDefaultParameter, iCurrentParameterValue ); // updates value

			Assert.AreEqual( iCurrentParameterValue, m_oDefaultTransaction.GetParameter( m_iDefaultParameter ) );
		}
		#endregion

		#region Assembly Set Tests
		[TestMethod]
		public void TestAssemblySetDefaultTransactionWithOnlyOneTransaction()
		{
			Assert.IsTrue( m_oDefaultTransaction.GetTransactionsFromAssemblySet().Length == 1 );
		}

		[TestMethod]
		public void TestAssemblySetDefaultTransactionHasItselftAsUniqueTransaction()
		{
			Transaction[] m_tAssemblySet = m_oDefaultTransaction.GetTransactionsFromAssemblySet();

			Assert.AreEqual( m_tAssemblySet[0], m_oDefaultTransaction );
		}

		[TestMethod]
		public void TestAssemblySetDefaultTransactionAddNewTransactionTwoTransactionInSet()
		{
			m_oDefaultTransaction.AddTransactionToAssemblySet( new Transaction( 2 ) );

			Assert.IsTrue( m_oDefaultTransaction.GetTransactionsFromAssemblySet().Length == 2 );
		}

		[TestMethod]
		public void TestAssemblySetDefaultTransactionAddNewTransactionThenRemoveOnlyOneTransactionInSet()
		{
			Transaction oTransationToAdd = new Transaction( 2 );
			m_oDefaultTransaction.AddTransactionToAssemblySet( oTransationToAdd );

			Assert.IsTrue( m_oDefaultTransaction.GetTransactionsFromAssemblySet().Length == 2 );

			m_oDefaultTransaction.RemoveTransactionFromAssemblySet( oTransationToAdd );

			Assert.IsTrue( m_oDefaultTransaction.GetTransactionsFromAssemblySet().Length == 1 );
		}

		[TestMethod]
		public void TestAssemblySetDefaultTransactionAddTwoTransactionsThenRemoveAllOnlyOneIsLeft()
		{
			m_oDefaultTransaction.AddTransactionToAssemblySet( new Transaction( 2 ) );
			m_oDefaultTransaction.AddTransactionToAssemblySet( new Transaction( 3 ) );

			m_oDefaultTransaction.ClearTransactionsFromAssemblySet();

			Assert.IsTrue( m_oDefaultTransaction.GetTransactionsFromAssemblySet().Length == 1 );
		}
		#endregion
	}
}
