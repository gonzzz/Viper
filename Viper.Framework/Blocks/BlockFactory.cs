using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Viper.Framework.Utils;
using System.Text.RegularExpressions;
using Viper.Framework.Exceptions;
using Viper.Framework.Enums;

namespace Viper.Framework.Blocks
{
	/// <summary>
	/// Blocks Model Generator. Translates GPSS Model in plain text to a Viper Block Model.
	/// </summary>
	public class BlockFactory
	{
		#region Singleton Implementation
		// Singleton Block Factory object
		private static BlockFactory m_oBlockFactory = null;

		/// <summary>
		/// Private constructor
		/// </summary>
		private BlockFactory()
		{
			ErrorMessageLog = new List<string>();
			CurrentLanguage = Languages.Spanish;
		}

		/// <summary>
		/// Block Generator Singleton Instance
		/// </summary>
		/// <returns></returns>
		public static BlockFactory Instance()
		{
			if( m_oBlockFactory == null )
			{
				m_oBlockFactory = new BlockFactory();
			}

			return m_oBlockFactory;
		}
		#endregion

		#region Public Properties
		public List<String> ErrorMessageLog { get; set; }
		public Languages CurrentLanguage { get; set; }
		#endregion

		#region Public Methods
		/// <summary>
		/// We assume the plain text has one block per line. The method scans line by line, translates each line into a 
		/// Viper Block and creates a new Block Object.
		/// </summary>
		/// <param name="sPlainTextModel">Plain Text GPSS Model</param>
		/// <returns>Viper Block List</returns>
		public List<Block> CreateModel( String sPlainTextModel )
		{
			List<Block> lbViperModel = new List<Block>();
			String[] sPlainModelBlocks = sPlainTextModel.Split( new string[] { "\r\n", "\n" }, StringSplitOptions.None );

			ErrorMessageLog.Clear();
			int iLineNumber = 0;
			foreach( String sPlainModelBlock in sPlainModelBlocks )
			{
				iLineNumber++;

				// First check if line is not a comment or empty
				if( !sPlainModelBlock.StartsWith( ";" ) && !String.IsNullOrEmpty( sPlainModelBlock ) )
				{
					try
					{
						// Check if it is a GENERATE Block
						if( IsGenerateBlock( lbViperModel, iLineNumber, sPlainModelBlock ) != BlockParseResult.NOT_PARSED ) continue;

						// Check if it is a ADVANCE Block
						if( IsAdvanceBlock( lbViperModel, iLineNumber, sPlainModelBlock ) != BlockParseResult.NOT_PARSED ) continue;

						// Check if it is a TERMINATE Block
						if( IsTerminateBlock( lbViperModel, iLineNumber, sPlainModelBlock ) != BlockParseResult.NOT_PARSED ) continue;

						// Check if it is a STORAGE Block
						if( IsStorageBlock( lbViperModel, iLineNumber, sPlainModelBlock ) != BlockParseResult.NOT_PARSED ) continue;

						// Check if it is a ENTER Block
						if( IsEnterBlock( lbViperModel, iLineNumber, sPlainModelBlock ) != BlockParseResult.NOT_PARSED ) continue;

						// Check if it is a LEAVE Block
						if( IsLeaveBlock( lbViperModel, iLineNumber, sPlainModelBlock ) != BlockParseResult.NOT_PARSED ) continue;

						// Check if it is a SEIZE Block
						if( IsSeizeBlock( lbViperModel, iLineNumber, sPlainModelBlock ) != BlockParseResult.NOT_PARSED ) continue;

						// Check if it is a RELEASE Block
						if( IsReleaseBlock( lbViperModel, iLineNumber, sPlainModelBlock ) != BlockParseResult.NOT_PARSED ) continue;

						// Check if it is a QUEUE Block
						if( IsQueueBlock( lbViperModel, iLineNumber, sPlainModelBlock ) != BlockParseResult.NOT_PARSED ) continue;

						// Check if it is a DEPART Block
						if( IsDepartBlock( lbViperModel, iLineNumber, sPlainModelBlock ) != BlockParseResult.NOT_PARSED ) continue;
						
						// Check if it is a PRIORITY Block
						if( IsPriorityBlock( lbViperModel, iLineNumber, sPlainModelBlock ) != BlockParseResult.NOT_PARSED ) continue;

						// If it arrives here it not a valid or supported block
						HandleInvalidOrNotSupportedBlockError( iLineNumber );
					}
					catch( BlockParseException ex )
					{
						HandleException( ex.BlockName, ex.LineNumber, ex.Message );
					}
					catch( Exception ex )
					{
						HandleException( String.Empty, iLineNumber, ex.Message );
					}
				}
			}

			return lbViperModel;
		}

		/// <summary>
		/// Get Plain Text Block Parts
		/// </summary>
		/// <param name="sPlainTextBlock"></param>
		/// <returns></returns>
		public static String[] GetBlockParts( String sPlainTextBlock )
		{
			// Replace Tabs for Spaces
			sPlainTextBlock = Regex.Replace( sPlainTextBlock, "\t", " " );
			// Clear Spaces at the start and at the end of line
			sPlainTextBlock = sPlainTextBlock.Trim();
			// Get block operands, including block name
			String[] sBlockParts = sPlainTextBlock.Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );

			return sBlockParts;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Tries to Parse a GENERATE Block
		/// </summary>
		/// <param name="lbViperModel"></param>
		/// <param name="iLineNumber"></param>
		/// <param name="sPlainTextBlock"></param>
		/// <returns></returns>
		private BlockParseResult IsGenerateBlock( List<Block> lbViperModel, int iLineNumber, String sPlainTextBlock )
		{
			if( sPlainTextBlock.Contains( BlockNames.GENERATE ) )
			{
				// Create Generate Block with line number, block number and raw plain text
				GenerateBlock oGenerate = new GenerateBlock( iLineNumber,( lbViperModel.Count + 1 ), sPlainTextBlock );
				
				// Attach OnParseFailed Event
				oGenerate.ParseFailed += new EventHandler( OnParseFailed );
				
				// Parse Block
				BlockParseResult bBlockParsedResult = oGenerate.Parse();
				
				// If result is OK add Generate Block in Viper Model
				if( bBlockParsedResult == BlockParseResult.PARSED_OK ) 
				{
					try
					{
						SetPreviousAndNextBlock( lbViperModel , oGenerate );
					}
					catch( BlockIntegrityException ex )
					{
						HandleException( BlockNames.GENERATE, iLineNumber, ex.Message );
					}
					lbViperModel.Add( oGenerate );
				}

				// Detach OnParseFailed Event
				oGenerate.ParseFailed -= OnParseFailed;

				// Return Parse Result
				return bBlockParsedResult;
			}

			return BlockParseResult.NOT_PARSED;
		}

		/// <summary>
		/// Tries to Parse a ADVANCE Block
		/// </summary>
		/// <param name="lbViperModel"></param>
		/// <param name="iLineNumber"></param>
		/// <param name="sPlainTextBlock"></param>
		/// <returns></returns>
		private BlockParseResult IsAdvanceBlock( List<Block> lbViperModel, int iLineNumber, String sPlainTextBlock )
		{
			if( sPlainTextBlock.Contains( BlockNames.ADVANCE ) )
			{
				// Create Advance Block with line number, block number and raw plain text
				AdvanceBlock oAdvance = new AdvanceBlock( iLineNumber, ( lbViperModel.Count + 1 ), sPlainTextBlock );
				
				// Attach OnParseFailed Event
				oAdvance.ParseFailed += new EventHandler( OnParseFailed );
				
				// Parse Block
				BlockParseResult bBlockParsedResult = oAdvance.Parse();
				
				// If result is OK add Advance Block in Viper Model
				if( bBlockParsedResult == BlockParseResult.PARSED_OK ) 
				{
					try
					{
						SetPreviousAndNextBlock( lbViperModel , oAdvance );
					}
					catch( BlockIntegrityException ex )
					{
						HandleException( BlockNames.ADVANCE, iLineNumber, ex.Message );
					}
					lbViperModel.Add( oAdvance );
				}
					
				// Detach OnParseFailed Event
				oAdvance.ParseFailed -= OnParseFailed;

				// Return Parse Result
				return bBlockParsedResult;
			}

			return BlockParseResult.NOT_PARSED;
		}

		/// <summary>
		/// Tries to Parse a TERMINATE Block
		/// </summary>
		/// <param name="lbViperModel"></param>
		/// <param name="iLineNumber"></param>
		/// <param name="sPlainTextBlock"></param>
		/// <returns></returns>
		private BlockParseResult IsTerminateBlock( List<Block> lbViperModel, int iLineNumber, String sPlainTextBlock )
		{
			if( sPlainTextBlock.Contains( BlockNames.TERMINATE ) )
			{
				// Create Terminate Block with line number, block number and raw plain text
				TerminateBlock oTerminate = new TerminateBlock( iLineNumber, ( lbViperModel.Count + 1 ), sPlainTextBlock );
				
				// Attach OnParseFailed Event
				oTerminate.ParseFailed += new EventHandler( OnParseFailed );
				
				// Parse Block
				BlockParseResult bBlockParsedResult = oTerminate.Parse();

				// If result is OK add Terminate Block in Viper Model
				if( bBlockParsedResult == BlockParseResult.PARSED_OK ) 
				{
					try
					{
						SetPreviousAndNextBlock( lbViperModel , oTerminate );
					}
					catch( BlockIntegrityException ex )
					{
						HandleException( BlockNames.TERMINATE, iLineNumber, ex.Message );
					}
					lbViperModel.Add( oTerminate );
				}

				// Detach OnParseFailed Event
				oTerminate.ParseFailed -= OnParseFailed;

				// Return Parse Result
				return bBlockParsedResult;
			}

			return BlockParseResult.NOT_PARSED;
		}

		/// <summary>
		/// Tries to Parse a STORAGE Block
		/// </summary>
		/// <param name="lbViperModel"></param>
		/// <param name="iLineNumber"></param>
		/// <param name="sPlainTextBlock"></param>
		/// <returns></returns>
		private BlockParseResult IsStorageBlock( List<Block> lbViperModel, int iLineNumber, String sPlainTextBlock )
		{
			if( sPlainTextBlock.Contains( BlockNames.STORAGE ) )
			{
				// Create Storage Block with line number, block number and raw plain text
				StorageBlock oStorage = new StorageBlock( iLineNumber, ( lbViperModel.Count + 1 ), sPlainTextBlock );
				
				// Attach OnParseFailed Event
				oStorage.ParseFailed += new EventHandler( OnParseFailed );

				// Parse Block
				BlockParseResult bBlockParsedResult = oStorage.Parse();

				// If result is OK add Storage Block in Viper Model
				if( bBlockParsedResult == BlockParseResult.PARSED_OK ) lbViperModel.Add( oStorage );

				// Detach OnParseFailed Event
				oStorage.ParseFailed -= OnParseFailed;

				// Return Parse Result
				return bBlockParsedResult;
			}

			return BlockParseResult.NOT_PARSED;
		}

		/// <summary>
		/// Tries to Parse a ENTER Block
		/// </summary>
		/// <param name="lbViperModel"></param>
		/// <param name="iLineNumber"></param>
		/// <param name="sPlainTextBlock"></param>
		/// <returns></returns>
		private BlockParseResult IsEnterBlock( List<Block> lbViperModel, int iLineNumber, String sPlainTextBlock )
		{
			if( sPlainTextBlock.Contains( BlockNames.ENTER ) )
			{
				// Create Enter Block with line number, block number and raw plain text
				EnterBlock oEnter = new EnterBlock( iLineNumber, ( lbViperModel.Count + 1 ), sPlainTextBlock );
				
				// Attach OnParseFailed Event
				oEnter.ParseFailed += new EventHandler( OnParseFailed );

				// Parse Block
				BlockParseResult bBlockParsedResult = oEnter.Parse();

				// If result is OK add Enter Block in Viper Model
				if( bBlockParsedResult == BlockParseResult.PARSED_OK ) 
				{
					try
					{
						SetPreviousAndNextBlock( lbViperModel, oEnter );
					}
					catch( BlockIntegrityException ex )
					{
						HandleException( BlockNames.ENTER, iLineNumber, ex.Message );
					}
					lbViperModel.Add( oEnter );
				}

				// Detach OnParseFailed Event
				oEnter.ParseFailed -= OnParseFailed;

				// Return Parse Result
				return bBlockParsedResult;
			}

			return BlockParseResult.NOT_PARSED;
		}

		/// <summary>
		/// Tries to Parse a LEAVE Block
		/// </summary>
		/// <param name="lbViperModel"></param>
		/// <param name="iLineNumber"></param>
		/// <param name="sPlainTextBlock"></param>
		/// <returns></returns>
		private BlockParseResult IsLeaveBlock( List<Block> lbViperModel, int iLineNumber, String sPlainTextBlock )
		{
			if( sPlainTextBlock.Contains( BlockNames.LEAVE ) )
			{
				// Create Leave Block with line number, block number and raw plain text
				LeaveBlock oLeave = new LeaveBlock( iLineNumber, ( lbViperModel.Count + 1 ), sPlainTextBlock );
				
				// Attach OnParseFailed Event
				oLeave.ParseFailed += new EventHandler( OnParseFailed );
				
				// Parse Block
				BlockParseResult bBlockParsedResult = oLeave.Parse();

				// If result is OK add Leave Block in Viper Model
				if( bBlockParsedResult == BlockParseResult.PARSED_OK ) 
				{
					try
					{
						SetPreviousAndNextBlock( lbViperModel , oLeave );
					}
					catch( BlockIntegrityException ex )
					{
						HandleException( BlockNames.LEAVE, iLineNumber, ex.Message );
					}
					lbViperModel.Add( oLeave );
				}

				// Detach OnParseFailed Event
				oLeave.ParseFailed -= OnParseFailed;

				// Return Parse Result
				return bBlockParsedResult;
			}

			return BlockParseResult.NOT_PARSED;
		}

		/// <summary>
		/// Tries to Parse a SEIZE Block
		/// </summary>
		/// <param name="lbViperModel"></param>
		/// <param name="iLineNumber"></param>
		/// <param name="sPlainTextBlock"></param>
		/// <returns></returns>
		private BlockParseResult IsSeizeBlock( List<Block> lbViperModel, int iLineNumber, String sPlainTextBlock )
		{
			if( sPlainTextBlock.Contains( BlockNames.SEIZE ) )
			{
				// Create Seize Block with line number, block number and raw plain text
				SeizeBlock oSeize = new SeizeBlock( iLineNumber, ( lbViperModel.Count + 1 ), sPlainTextBlock );
				
				// Attach OnParseFailed Event
				oSeize.ParseFailed += new EventHandler( OnParseFailed );

				// Parse Block
				BlockParseResult bBlockParsedResult = oSeize.Parse();

				// If result is OK add Seize Block in Viper Model
				if( bBlockParsedResult == BlockParseResult.PARSED_OK ) 
				{
					try
					{
						SetPreviousAndNextBlock( lbViperModel , oSeize );
					}
					catch( BlockIntegrityException ex )
					{
						HandleException( BlockNames.SEIZE, iLineNumber, ex.Message );
					}
					lbViperModel.Add( oSeize );
				}

				// Detach OnParseFailed Event
				oSeize.ParseFailed -= OnParseFailed;

				// Return Parse Result
				return bBlockParsedResult;
			}

			return BlockParseResult.NOT_PARSED;
		}

		/// <summary>
		/// Tries to Parse a RELEASE Block
		/// </summary>
		/// <param name="lbViperModel"></param>
		/// <param name="iLineNumber"></param>
		/// <param name="sPlainTextBlock"></param>
		/// <returns></returns>
		private BlockParseResult IsReleaseBlock( List<Block> lbViperModel, int iLineNumber, String sPlainTextBlock )
		{
			if( sPlainTextBlock.Contains( BlockNames.RELEASE ) )
			{
				// Create Release Block with line number, block number and raw plain text
				ReleaseBlock oRelease = new ReleaseBlock( iLineNumber, ( lbViperModel.Count + 1 ), sPlainTextBlock );
				
				// Attach OnParseFailed Event
				oRelease.ParseFailed += new EventHandler( OnParseFailed );

				// Parse Block
				BlockParseResult bBlockParsedResult = oRelease.Parse();

				// If result is OK add Release Block in Viper Model
				if( bBlockParsedResult == BlockParseResult.PARSED_OK ) 
				{
					try
					{
						SetPreviousAndNextBlock( lbViperModel , oRelease );
					}
					catch( BlockIntegrityException ex )
					{
						HandleException( BlockNames.RELEASE, iLineNumber, ex.Message );
					}
					lbViperModel.Add( oRelease );
				}

				// Detach OnParseFailed Event
				oRelease.ParseFailed -= OnParseFailed;

				// Return Parse Result
				return bBlockParsedResult;
			}

			return BlockParseResult.NOT_PARSED;
		}

		/// <summary>
		/// Tries to Parse a QUEUE Block
		/// </summary>
		/// <param name="lbViperModel"></param>
		/// <param name="iLineNumber"></param>
		/// <param name="sPlainTextBlock"></param>
		/// <returns></returns>
		private BlockParseResult IsQueueBlock( List<Block> lbViperModel, int iLineNumber, String sPlainTextBlock )
		{
			if( sPlainTextBlock.Contains( BlockNames.QUEUE ) )
			{
				// Create Queue Block with line number, block number and raw plain text
				QueueBlock oQueue = new QueueBlock( iLineNumber, ( lbViperModel.Count + 1 ), sPlainTextBlock );
				
				// Attach OnParseFailed Event
				oQueue.ParseFailed += new EventHandler( OnParseFailed );
				
				// Parse Block
				BlockParseResult bBlockParsedResult = oQueue.Parse();

				// If result is OK add Queue Block in Viper Model
				if( bBlockParsedResult == BlockParseResult.PARSED_OK ) 
				{
					try
					{
						SetPreviousAndNextBlock( lbViperModel , oQueue );
					}
					catch( BlockIntegrityException ex )
					{
						HandleException( BlockNames.QUEUE, iLineNumber, ex.Message );
					}
					lbViperModel.Add( oQueue );
				}

				// Detach OnParseFailed Event
				oQueue.ParseFailed -= OnParseFailed;

				// Return Parse Result
				return bBlockParsedResult;
			}

			return BlockParseResult.NOT_PARSED;
		}

		/// <summary>
		/// Tries to Parse a DEPART Block
		/// </summary>
		/// <param name="lbViperModel"></param>
		/// <param name="iLineNumber"></param>
		/// <param name="sPlainTextBlock"></param>
		/// <returns></returns>
		private BlockParseResult IsDepartBlock( List<Block> lbViperModel, int iLineNumber, String sPlainTextBlock )
		{
			if( sPlainTextBlock.Contains( BlockNames.DEPART ) )
			{
				// Create Depart Block with line number, block number and raw plain text
				DepartBlock oDepart = new DepartBlock( iLineNumber, ( lbViperModel.Count + 1 ), sPlainTextBlock );
				
				// Attach OnParseFailed Event
				oDepart.ParseFailed += new EventHandler( OnParseFailed );
				
				// Parse Block
				BlockParseResult bBlockParsedResult = oDepart.Parse();

				// If result is OK add Depart Block in Viper Model
				if( bBlockParsedResult == BlockParseResult.PARSED_OK ) 
				{
					try
					{
						SetPreviousAndNextBlock( lbViperModel , oDepart );
					}
					catch( BlockIntegrityException ex )
					{
						HandleException( BlockNames.DEPART, iLineNumber, ex.Message );
					}
					lbViperModel.Add( oDepart );
				}

				// Detach OnParseFailed Event
				oDepart.ParseFailed -= OnParseFailed;

				// Return Parse Result
				return bBlockParsedResult;
			}

			return BlockParseResult.NOT_PARSED;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lbViperModel"></param>
		/// <param name="iLineNumber"></param>
		/// <param name="sPlainTextBlock"></param>
		/// <returns></returns>
		private BlockParseResult IsPriorityBlock( List<Block> lbViperModel , int iLineNumber , String sPlainTextBlock )
		{
			if( sPlainTextBlock.Contains( BlockNames.PRIORITY ) )
			{
				// Create Depart Block with line number, block number and raw plain text
				PriorityBlock oPriority = new PriorityBlock( iLineNumber , ( lbViperModel.Count + 1 ) , sPlainTextBlock );

				// Attach OnParseFailed Event
				oPriority.ParseFailed += new EventHandler( OnParseFailed );

				// Parse Block
				BlockParseResult bBlockParsedResult = oPriority.Parse();

				// If result is OK add Depart Block in Viper Model
				if ( bBlockParsedResult == BlockParseResult.PARSED_OK )
				{
					try
					{
						SetPreviousAndNextBlock( lbViperModel , oPriority );
					}
					catch ( BlockIntegrityException ex )
					{
						HandleException( BlockNames.DEPART , iLineNumber , ex.Message );
					}
					lbViperModel.Add( oPriority );
				}

				// Detach OnParseFailed Event
				oPriority.ParseFailed -= OnParseFailed;

				// Return Parse Result
				return bBlockParsedResult;
			}

			return BlockParseResult.NOT_PARSED;
		}

		/// <summary>
		/// On Block Parse Failed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="a"></param>
		private void OnParseFailed( object sender, EventArgs a )
		{
			ParseEventArgs parseArgs = ( ParseEventArgs )a;

			HandleSyntaxError( parseArgs.BlockName, parseArgs.LineNumber );
		}

		/// <summary>
		/// Common Block Syntax Error handler. Adds an error message to error log.
		/// </summary>
		/// <param name="sBlockName"></param>
		/// <param name="iLineNumber"></param>
		private void HandleSyntaxError( String sBlockName, int iLineNumber )
		{
			// Get Message Error Format in current language
			String sErrorMessageFormat = String.Empty;
			if( CurrentLanguage == Languages.English )
			{
				sErrorMessageFormat = Resources.SyntaxErrorMessagesEN.WRONG_BLOCK_SYNTAX;
			}
			else if( CurrentLanguage == Languages.Spanish )
			{
				sErrorMessageFormat = Resources.SyntaxErrorMessagesES.WRONG_BLOCK_SYNTAX;
			}

			// Add new message error to message log
			ErrorMessageLog.Add( String.Format( sErrorMessageFormat, sBlockName, iLineNumber ) );
		}

		/// <summary>
		/// Not supported or Invalid Block Error Handler. Adds an error message to error log.
		/// </summary>
		/// <param name="iLineNumber"></param>
		private void HandleInvalidOrNotSupportedBlockError( int iLineNumber )
		{
			// Get Message Error Format in current language
			String sErrorMessageFormat = String.Empty;
			if( CurrentLanguage == Languages.English )
			{
				sErrorMessageFormat = Resources.SyntaxErrorMessagesEN.INVALID_OR_NOT_SUPPORTED_BLOCK;
			}
			else if( CurrentLanguage == Languages.Spanish )
			{
				sErrorMessageFormat = Resources.SyntaxErrorMessagesES.INVALID_OR_NOT_SUPPORTED_BLOCK;
			}

			// Add new message error to message log
			ErrorMessageLog.Add( String.Format( sErrorMessageFormat, iLineNumber ) );
		}

		/// <summary>
		/// Unhandled error as exceptions handler. Adds an error message to error log.
		/// </summary>
		/// <param name="sBlockName"></param>
		/// <param name="iLineNumber"></param>
		/// <param name="sMessage"></param>
		private void HandleException( String sBlockName, int iLineNumber, String sMessage )
		{
			// Get Message Error Format in current language
			String sErrorMessageFormat = String.Empty;
			if( CurrentLanguage == Languages.English )
			{
				if( String.IsNullOrEmpty( sBlockName ) ) sErrorMessageFormat = Resources.SyntaxErrorMessagesEN.EXCEPTION_ON_BLOCK_PARSING_NO_BLOCK_NAME;
				else sErrorMessageFormat = Resources.SyntaxErrorMessagesEN.EXCEPTION_ON_BLOCK_PARSING_WITH_BLOCK_NAME;
			}
			else if( CurrentLanguage == Languages.Spanish )
			{
				if( String.IsNullOrEmpty( sBlockName ) ) sErrorMessageFormat = Resources.SyntaxErrorMessagesES.EXCEPTION_ON_BLOCK_PARSING_NO_BLOCK_NAME;
				else sErrorMessageFormat = Resources.SyntaxErrorMessagesES.EXCEPTION_ON_BLOCK_PARSING_WITH_BLOCK_NAME;
			}

			// Add new message error to message log
			if( String.IsNullOrEmpty( sBlockName ) ) 
				ErrorMessageLog.Add( String.Format( sErrorMessageFormat, iLineNumber, sMessage ) );
			else
				ErrorMessageLog.Add( String.Format( sErrorMessageFormat, sBlockName, iLineNumber, sMessage ) );
		}

		/// <summary>
		/// Set Next and Previous Block
		/// </summary>
		/// <param name="lbViperModel"></param>
		/// <param name="oBlock"></param>
		private void SetPreviousAndNextBlock( List<Block> lbViperModel , BlockTransactional oBlock )
		{
			if ( lbViperModel.Count > 0 )
			{
				Block lastBlock = lbViperModel.Last();
				if ( lastBlock is BlockTransactional ) // only chain to transactional blocks
				{
					BlockTransactional lastBlockTransactional = lastBlock as BlockTransactional;
					lastBlockTransactional.NextBlock = oBlock;
					oBlock.PreviousBlock = lastBlockTransactional;
				}
				else
				{
					List<Block> lbTransactionalBlocks = lbViperModel.FindAll( b => b is BlockTransactional );
					if ( lbTransactionalBlocks.Count > 0 )
					{
						throw new BlockIntegrityException( "Non-Transactional Blocks cannot be used between transactional blocks", null, String.Empty, oBlock.Line );
					}
				}
			}
		}
		#endregion
	}
}
