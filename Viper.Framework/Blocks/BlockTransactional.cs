using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Entities;
using Viper.Framework.Enums;

namespace Viper.Framework.Blocks
{
	public abstract class BlockTransactional : Block
	{
		#region Protected Members
		protected Transaction m_oActiveTransaction;
		protected List<Transaction> m_ltRetryTransactionChain;
		protected Block m_bPreviousBlock;
		protected Block m_bNextBlock;
		#endregion

		#region Public Properties
		/// <summary>
		/// 
		/// </summary>
		public Transaction ActiveTransaction
		{
			get 
			{
				return m_oActiveTransaction;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public List<Transaction> RetryChain
		{
			get
			{
				return m_ltRetryTransactionChain;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Block PreviousBlock
		{
			get
			{
				return m_bPreviousBlock;
			}
			set
			{
				m_bPreviousBlock = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Block NextBlock
		{
			get
			{
				return m_bNextBlock;
			}
			set
			{
				m_bNextBlock = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructors
		/// </summary>
		public BlockTransactional()
			: base()
		{
			this.m_bExecutable = true;
			this.m_bPreviousBlock = null;
			this.m_bNextBlock = null;
			this.m_oActiveTransaction = null;
			this.m_ltRetryTransactionChain = new List<Transaction>();
		}

		/// <summary>
		/// Constructor with Parameters
		/// </summary>
		/// <param name="iLineNumber"></param>
		/// <param name="iBlockNumber"></param>
		/// <param name="sBlockText"></param>
		public BlockTransactional( int iLineNumber, int iBlockNumber, String sBlockText )
			: base( iLineNumber, iBlockNumber, sBlockText )
		{
			this.m_bExecutable = true;
			this.m_bPreviousBlock = null;
			this.m_bNextBlock = null;
			this.m_oActiveTransaction = null;
			this.m_ltRetryTransactionChain = new List<Transaction>();
		}
		#endregion
	}
}
