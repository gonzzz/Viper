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
		#endregion

		#region Public Properties

		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructors
		/// </summary>
		public BlockTransactional()
			: base()
		{
			this.m_bExecutable = true;
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
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oTransaction"></param>
		/// <returns></returns>
		public abstract BlockProcessResult Process( Transaction oTransaction );
	}
}
