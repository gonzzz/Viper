using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viper.Framework.Entities;
using Viper.Framework.Enums;
using Viper.Framework.Utils;

namespace Viper.Framework.Blocks
{
	public abstract class BlockTransactional : Block, IProcessable
	{
		#region Protected Members
		protected List<Transaction> m_ltCurrentTransactions;
		protected Block m_bPreviousBlock;
		protected Block m_bNextBlock;
		protected int m_iEntryCount;
		#endregion

		#region Public Properties
		/// <summary>
		/// Returns Block Entry Count. Total number of transactions which have entered the block.
		/// Related SNA: Nentnum. Where entnum is Block.Number property.
		/// </summary>
		public int EntryCount
		{
			get
			{
				return m_iEntryCount;
			}
		}

		/// <summary>
		/// Returns Block Current Count. Current number of transactions in the block.
		/// Related SNA: Wentnum. Where entnum is Block.Number property.
		/// </summary>
		public int CurrentCount
		{
			get
			{
				return m_ltCurrentTransactions.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public List<Transaction> CurrentTransactions
		{
			get
			{
				return m_ltCurrentTransactions;
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
			this.m_ltCurrentTransactions = new List<Transaction>();
			this.m_iEntryCount = Constants.DEFAULT_ZERO_VALUE;
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
			this.m_ltCurrentTransactions = new List<Transaction>();
			this.m_iEntryCount = Constants.DEFAULT_ZERO_VALUE;
		}
		#endregion

		public virtual BlockProcessResult Process( ref Transaction oTransaction )
		{
			BlockTransactional previousBlock = oTransaction.CurrentBlock;
			if( previousBlock != null ) previousBlock.CurrentTransactions.Remove( oTransaction );

			oTransaction.CurrentBlock = this;
			if( this.NextBlock != null && this.NextBlock is BlockTransactional ) {
				oTransaction.NextBlock = (BlockTransactional)this.NextBlock;
			} 
			else {
				oTransaction.NextBlock = null;
			}

			this.CurrentTransactions.Add( oTransaction );
			this.m_iEntryCount++;

			return BlockProcessResult.TRANSACTION_PROCESSED;
		}

		public event EventHandler ProcessSuccess;
		public event EventHandler ProcessFailed;

		public void OnProcessSuccess( ProcessEventArgs eventArgs )
		{
			if( ProcessSuccess != null ) ProcessSuccess( this, eventArgs );
		}

		public void OnProcessFailed( ProcessEventArgs eventArgs )
		{
			if ( ProcessFailed != null ) ProcessFailed( this , eventArgs );
		}
	}
}
