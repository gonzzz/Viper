using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Viper.Framework.Utils;
using Viper.Framework.Enums;
using Viper.Framework.Exceptions;
using Viper.Framework.Entities;

namespace Viper.Framework.Engine
{
	public class ViperSNATranslator
	{
		/// <summary>
		/// Translates a Block's Operand into a SNA Translated Object
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public static SNATranslated Translate( String parameter )
		{
			SNATranslated sna = null;

			// Check for System SNAs
			if( ( sna = CheckForSystemSNA( parameter ) ) != null ) return sna;

			// Check for Block SNAs
			if( ( sna = CheckForBlockSNA( parameter ) ) != null ) return sna;

			// Check for Transaction SNAs
			if( ( sna = CheckForTransactionSNA( parameter ) ) != null ) return sna;

			// Check for Storage SNAs
			if( ( sna = CheckForStorageSNA( parameter ) ) != null ) return sna;

			// Check for Function SNAs
			if( ( sna = CheckForFunctionSNA( parameter ) ) != null ) return sna;

			// Check for Facility SNAs
			if( ( sna = CheckForFacilitySNA( parameter ) ) != null ) return sna;

			// Check for Queue SNAs
			if( ( sna = CheckForQueueSNA( parameter ) ) != null ) return sna;

			// Check for User Chain SNAs
			if( ( sna = CheckForUserChainSNA( parameter ) ) != null ) return sna;

			// Check for Table SNAs
			if( ( sna = CheckForTableSNA( parameter ) ) != null ) return sna;

			// Check for Logic Switch SNAs
			if( ( sna = CheckForLogicSwitchSNA( parameter ) ) != null ) return sna;

			// Check for Save Value SNAs
			if( ( sna = CheckForSaveValueSNA( parameter ) ) != null ) return sna;

			// Check for Matrix Save Value SNAs
			if( ( sna = CheckForMatrixSaveValueSNA( parameter ) ) != null ) return sna;

			// Check for Numeric Group SNAs
			if( ( sna = CheckForNumericGroupSNA( parameter ) ) != null ) return sna;

			// Check for Transaction Group SNAs
			if( ( sna = CheckForTransactionGroupSNA( parameter ) ) != null ) return sna;

			// Check for Boolean Variable SNAs
			if( ( sna = CheckForBooleanVariableSNA( parameter ) ) != null ) return sna;

			// Check for Arithmetic or Float Variable SNAs
			if( ( sna = CheckForVariableSNA( parameter ) ) != null ) return sna;

			return null;
		}

		#region Check SNA Types
		/// <summary>
		/// Check if SNA is a System SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForSystemSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.AC1 ) )
			{
				// Absolute Time Clock - No need of specifiers
				return new SNATranslated( parameter, SNAType.System, SNA.SystemAbsoluteClock );
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.C1 ) )
			{
				// Relative Time Clock - No need of specifiers
				return new SNATranslated( parameter , SNAType.System , SNA.SystemCurrentClock );
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.TG1 ) )
			{
				// Termination Count - No need of specifiers
				return new SNATranslated( parameter , SNAType.System , SNA.SystemTerminationCount );
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.RN ) )
			{
				// Randon Number
				SNATranslated sna = new SNATranslated( parameter , SNAType.System , SNA.SystemRandomNumber );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.RN.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Block SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForBlockSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.W ) )
			{
				// Current Transactions Count
				SNATranslated sna = new SNATranslated( parameter, SNAType.Block, SNA.BlockCurrentTransactionCount );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.W.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.N ) )
			{
				// Total Transactions Count
				SNATranslated sna = new SNATranslated( parameter, SNAType.Block, SNA.BlockTotalTransactionCount );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.N.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Transaction SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForTransactionSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.M1 ) )
			{
				// Transaction Total Transit Time - No need of specifiers
				return new SNATranslated( parameter, SNAType.Transaction, SNA.TransactionTotalTransitTime );
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.XN1 ) )
			{
				// Active Transaction Number - No need of specifiers
				return new SNATranslated( parameter, SNAType.Transaction, SNA.ActiveTransactionNumber );
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.PR ) )
			{
				// Transaction Priority
				return new SNATranslated( parameter, SNAType.Transaction, SNA.TransactionPriority );
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.P ) )
			{
				// Transaction Parameter
				SNATranslated sna = new SNATranslated( parameter, SNAType.Transaction, SNA.TransactionParameter );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.P.Length );
				sna = TranslateTransactionParameterSpecifier( sna, specifier );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.P_indirect ) )
			{
				// Transaction Parameter
				SNATranslated sna = new SNATranslated( parameter, SNAType.Transaction, SNA.TransactionParameter );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.P_indirect.Length );
				sna = TranslateTransactionParameterSpecifier( sna, specifier );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.MP ) )
			{
				// Transaction Transite time minus parameter value content
				SNATranslated sna = new SNATranslated( parameter, SNAType.Transaction, SNA.TransactionTransitTimeRelativeToParameter );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.MP.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.MB ) )
			{
				// Transaction Match at Block
				SNATranslated sna = new SNATranslated( parameter, SNAType.Transaction, SNA.TransactionMatchAtBlock );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.MB.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Storage SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForStorageSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.R ) )
			{
				// Storage Total Units
				SNATranslated sna = new SNATranslated( parameter, SNAType.Storage, SNA.StorageTotalUnits );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.R.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}		
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.SA ) )
			{
				// Storage Average Usage
				SNATranslated sna = new SNATranslated( parameter, SNAType.Storage, SNA.StorageAverageUse );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.SA.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.SC ) )
			{
				// Storage Entries Count
				SNATranslated sna = new SNATranslated( parameter, SNAType.Storage, SNA.StorageEntriesCount );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.SC.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.SE ) )
			{
				// Storage is Empty
				SNATranslated sna = new SNATranslated( parameter, SNAType.Storage, SNA.StorageIsEmpty );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.SE.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.SF ) )
			{
				// Storage is Full
				SNATranslated sna = new SNATranslated( parameter, SNAType.Storage, SNA.StorageIsFull );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.SF.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.SR ) )
			{
				// Storage Utilization Ratio
				SNATranslated sna = new SNATranslated( parameter, SNAType.Storage, SNA.StorageUtilizationRatio );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.SR.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.SM ) )
			{
				// Storage Maximus Usage
				SNATranslated sna = new SNATranslated( parameter, SNAType.Storage, SNA.StorageMaximusUsage );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.SM.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.ST ) )
			{
				// Storage Average Usage Time
				SNATranslated sna = new SNATranslated( parameter, SNAType.Storage, SNA.StorageAverageUsageTime );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.ST.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.SV ) )
			{
				// Storage is FreeUnits
				SNATranslated sna = new SNATranslated( parameter, SNAType.Storage, SNA.StorageIsAvailable );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.SV.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.S ) )
			{
				// Storage Total Units
				SNATranslated sna = new SNATranslated( parameter, SNAType.Storage, SNA.StorageAvailableUnits );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.S.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Facility SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForFacilitySNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.FC ) )
			{
				// Facility Entries Count
				SNATranslated sna = new SNATranslated( parameter, SNAType.Facility, SNA.FacilityEntriesCount );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.FC.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if ( ViperSNATranslator.StartsWithSNAPrefix( parameter , SNAPrefixes.FI ) )
			{
				// Facility is Preempted
				SNATranslated sna = new SNATranslated( parameter, SNAType.Facility, SNA.FacilityIsPreempted );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.FI.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if ( ViperSNATranslator.StartsWithSNAPrefix( parameter , SNAPrefixes.FR ) )
			{
				// Facility Utilization Ratio
				SNATranslated sna = new SNATranslated( parameter, SNAType.Facility, SNA.FacilityUtilizationRatio );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.FR.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if ( ViperSNATranslator.StartsWithSNAPrefix( parameter , SNAPrefixes.FT ) )
			{
				// Facility Average Usage Time
				SNATranslated sna = new SNATranslated( parameter, SNAType.Facility, SNA.FacilityAverageOccupationTime );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.FT.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if ( ViperSNATranslator.StartsWithSNAPrefix( parameter , SNAPrefixes.FV ) )
			{
				// Facility is FreeUnits
				SNATranslated sna = new SNATranslated( parameter, SNAType.Facility, SNA.FacilityIsAvailable );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.FV.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if ( ViperSNATranslator.StartsWithSNAPrefix( parameter , SNAPrefixes.F ) )
			{
				// Facility is Occupied
				SNATranslated sna = new SNATranslated( parameter, SNAType.Facility, SNA.FacilityIsOccupied );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.F.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Queue SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForQueueSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.QA ) )
			{
				// Queue Average Content
				SNATranslated sna = new SNATranslated( parameter, SNAType.Queue, SNA.QueueAverageContent );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.QA.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.QC ) )
			{
				// Queue Entry Count
				SNATranslated sna = new SNATranslated( parameter, SNAType.Queue, SNA.QueueEntryCount );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.QC.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}			
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.QM ) )
			{
				// Queue Maximum Entry Count
				SNATranslated sna = new SNATranslated( parameter, SNAType.Queue, SNA.QueueMaximumEntryCount );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.QM.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.QT ) )
			{
				// Queue Average Stay Time
				SNATranslated sna = new SNATranslated( parameter, SNAType.Queue, SNA.QueueAverageStayTime );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.QT.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.QX ) )
			{
				// Queue Average Time Excluding Zero-Content Entries
				SNATranslated sna = new SNATranslated( parameter, SNAType.Queue, SNA.QueueAverageStayTimeExcludingZeroContentEntries );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.QX.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.QZ ) )
			{
				// Queue Entry Count with Zero-Content Entries
				SNATranslated sna = new SNATranslated( parameter, SNAType.Queue, SNA.QueueEntryCountWithZeroContentEntries );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.QZ.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.Q ) )
			{
				// Queue Current Content
				SNATranslated sna = new SNATranslated( parameter, SNAType.Queue, SNA.QueueActualContent );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.Q.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a User Chain SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForUserChainSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.CA ) )
			{
				// User Chain Average Content
				SNATranslated sna = new SNATranslated( parameter, SNAType.UserChain, SNA.UserChainAverageContent );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.CA.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.CC ) )
			{
				// User Chain Total Count
				SNATranslated sna = new SNATranslated( parameter, SNAType.UserChain, SNA.UserChainTotalCount );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.CC.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.CH ) )
			{
				// User Chain Current Count
				SNATranslated sna = new SNATranslated( parameter, SNAType.UserChain, SNA.UserChainCurrentCount );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.CH.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.CM ) )
			{
				// User Chain Maximum Count
				SNATranslated sna = new SNATranslated( parameter, SNAType.UserChain, SNA.UserChainMaximumCount );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.CM.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.CT ) )
			{
				// User Chain Average Residence Time
				SNATranslated sna = new SNATranslated( parameter, SNAType.UserChain, SNA.UserChainAverageResidenceTime );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.CT.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Logic Switch SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForLogicSwitchSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.LS ) )
			{
				// Logic Switch State Value
				SNATranslated sna = new SNATranslated( parameter, SNAType.LogicSwitch, SNA.LogicSwitchStateValue );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.LS.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Save Value SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForSaveValueSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.X ) )
			{
				// Save Value Current Value
				SNATranslated sna = new SNATranslated( parameter, SNAType.SaveValue, SNA.SaveValueCurrentValue );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.X.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Matrix Save Value SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForMatrixSaveValueSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.MX ) )
			{
				// Matrix Save Value Current Value
				SNATranslated sna = new SNATranslated( parameter, SNAType.MatrixSaveValue, SNA.MatrixSaveValueCurrentValue );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.MX.Length );
				sna = TranslateMatrixSaveValueSpecifier( sna, specifier );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Function SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForFunctionSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.FN ) )
			{
				// Function Evaluation Result
				SNATranslated sna = new SNATranslated( parameter, SNAType.Function, SNA.FunctionEvaluationResult );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.FN.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Table SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForTableSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.TB ) )
			{
				// Table Average Entries
				SNATranslated sna = new SNATranslated( parameter, SNAType.Table, SNA.TableAverageEntries );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.TB.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.TC ) )
			{
				// Table Entry Count
				SNATranslated sna = new SNATranslated( parameter, SNAType.Table, SNA.TableEntryCount );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.TC.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}
			else if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.TD ) )
			{
				// Table Standard Deviation of Entries
				SNATranslated sna = new SNATranslated( parameter, SNAType.Table, SNA.TableEntriesStandardDeviation );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.TD.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Boolean Variable SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForBooleanVariableSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.BV ) )
			{
				// Boolean Variable Evaluation Result
				SNATranslated sna = new SNATranslated( parameter, SNAType.BVariable, SNA.BooleanVariableResult );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.BV.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Variable SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForVariableSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.V ) )
			{
				// Arithmetic/Float Variable Evaluation Result
				SNATranslated sna = new SNATranslated( parameter, SNAType.Variable, SNA.VariableResult );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.V.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Numeric Group SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForNumericGroupSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.GN ) )
			{
				// Numeric Group Count
				SNATranslated sna = new SNATranslated( parameter, SNAType.NumericGroup, SNA.NumericGroupCount );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.GN.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}

		/// <summary>
		/// Checks if SNA is a Transaction Group SNA and returns null if not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static SNATranslated CheckForTransactionGroupSNA( String parameter )
		{
			if( ViperSNATranslator.StartsWithSNAPrefix( parameter, SNAPrefixes.GT ) )
			{
				// Transaction Group Count
				SNATranslated sna = new SNATranslated( parameter, SNAType.TransactionGroup, SNA.TransactionGroupCount );

				// Get specifier
				String specifier = parameter.Substring( SNAPrefixes.GT.Length );
				sna = TranslateSpecifier( sna, specifier, SNAEntitySpecifierType.PositiveInteger | SNAEntitySpecifierType.Name | SNAEntitySpecifierType.IndirectAddressing );

				return sna;
			}

			return null;
		}
		#endregion

		#region Specifier Format Translation
		/// <summary>
		/// Special Case to Process. Transaction Parameter.
		/// </summary>
		/// <param name="snaTranslated"></param>
		/// <param name="specifier"></param>
		/// <returns></returns>
		private static SNATranslated TranslateTransactionParameterSpecifier( SNATranslated snaTranslated, String specifier )
		{
			// TRANSACTION PARAMETER AVAILABLE FORMATS: P5, P$name, *5, *name, *$name
			SNAParameter parameter = new SNAParameter();
			// Only take value (no tokens)
			parameter.Value = specifier;
			parameter.IndirectAddressing = true;
			if( parameter.Value.StartsWith( Constants.SNA_TOKEN_SEPARATOR ) ) parameter.Value = parameter.Value.Substring( 1 );

			// Validate
			if( IsValidName( parameter.Value ) )
			{
				parameter.IsName = true;
			}
			else if( IsValidPositiveInteger( parameter.Value ) )
			{
				parameter.IsPosInteger = true;
			}
			else
			{
				throw new InvalidSNASpecifierException( "Invalid SNA Parameter" );
			}

			snaTranslated.Parameter = parameter;
			return snaTranslated;
		}

		/// <summary>
		/// Special Case to Process. Matrix Save Value Current Value.
		/// </summary>
		/// <param name="snaTranslated"></param>
		/// <param name="specifier"></param>
		/// <returns></returns>
		private static SNATranslated TranslateMatrixSaveValueSpecifier( SNATranslated snaTranslated, String specifier )
		{
			// MATRIX SAVEVALUE AVAILABLE FORMATS: 
			// DIRECT MATRIX:	MX5(3,3), 
			//					MX$MAT(3,3)
			// INDIRECT MATRIX: MX5(*2,*3), MX5(*ROW,*COL), MX5(*$ROW,*$COL)
			//					MX$MAT(*2,*3), MX$MAT(*ROW,*COL), MX$MAT(*$ROW,*$COL)
			//					MX*10(3,3), MX*10(*11,*12), MX*10(*ROW,*COL), MX*10(*$ROW,*$COL)
			//					MX*PRSMAT(3,3), MX*PRSMAT(*11,*12), MX*PRSMAT(*ROW,*COL), MX*PRSMAT(*$ROW,*$COL)
			//					MX*PRSMAT(3,3), MX*PRSMAT(*11,*12), MX*PRSMAT(*ROW,*COL), MX*PRSMAT(*$ROW,*$COL)

			// There are 25 possibly combinations (5 x 5)
			// FIRST PART (5): MX[posinteger|$name] | MX[*posinteger|*name|*$name]
			// SECOND PART (5): (posinteger,posinteger) | ($name,$name) | (*posinteger,*posinteger) | (*name,*name) | (*$name,*$name)

			// 1) Separate both parts
			int firstP = specifier.IndexOf( "(" );
			int lastP = specifier.IndexOf( ")" );
			if( firstP == -1 || lastP == -1 ) throw new InvalidSNASpecifierException( "Invalid Matrix SaveValue Specifier" );
			String firstPart = specifier.Substring( 0, firstP );
			String secondPart = specifier.Substring( firstP );

			// 2) Parse First Part
			#region First Part Parse
			if( firstPart.StartsWith( Constants.INDIRECT_ADDRESSING_TOKEN ) )
			{
				// FORMATS: MX[*posinteger|*name|*$name]
				SNAParameter parameter = new SNAParameter();
				// Only take value (no tokens)
				parameter.Value = firstPart.Substring( 1 );
				parameter.IndirectAddressing = true;
				if( parameter.Value.StartsWith( Constants.SNA_TOKEN_SEPARATOR ) ) parameter.Value = parameter.Value.Substring( 1 );

				// Validate
				if( IsValidName( parameter.Value ) )
				{
					parameter.IsName = true;
				}
				else if( IsValidPositiveInteger( parameter.Value ) )
				{
					parameter.IsPosInteger = true;
				}
				else
				{
					throw new InvalidSNASpecifierException( "Invalid Matrix SaveValue Parameter" );
				}

				snaTranslated.Parameter = parameter;
			}
			else
			{
				// FORMATS: MX[posinteger|$name]
				
				SNAParameter parameter = new SNAParameter();
				parameter.Value = firstPart;
				if( parameter.Value.StartsWith( Constants.SNA_TOKEN_SEPARATOR ) ) parameter.Value = parameter.Value.Substring( 1 );

				// Validate
				if( IsValidName( parameter.Value ) )
				{
					parameter.IsName = true;
				}
				else if( IsValidPositiveInteger( parameter.Value ) )
				{
					parameter.IsPosInteger = true;
				}
				else
				{
					throw new InvalidSNASpecifierException( "Invalid Matrix SaveValue Parameter" );
				}

				snaTranslated.Parameter = parameter;
			}
			#endregion

			// 3) Parse Second Part
			#region Second Part Parse
			// Remove '(' and ')' characters
			secondPart = secondPart.Replace( "(", "" ).Replace( ")", "" );
			// Process row and column separately
			String[] coordinates = secondPart.Split( ',' );
			if( coordinates.Length != 2 ) throw new InvalidSNASpecifierException( "Invalid Matrix SaveValue Coordinates Parameters" );
			foreach( String coordinate in coordinates )
			{
				if( coordinate.StartsWith( Constants.INDIRECT_ADDRESSING_TOKEN ) )
				{
					// FORMAT: *posinteger | *name | *$name
					SNAParameter parameter = new SNAParameter();
					// Only take value (no tokens)
					parameter.Value = coordinate.Substring( 1 );
					parameter.IndirectAddressing = true;
					if( parameter.Value.StartsWith( Constants.SNA_TOKEN_SEPARATOR ) ) parameter.Value = parameter.Value.Substring( 1 );

					// Validate
					if( IsValidName( parameter.Value ) )
					{
						parameter.IsName = true;
					}
					else if( IsValidPositiveInteger( parameter.Value ) )
					{
						parameter.IsPosInteger = true;
					}
					else
					{
						throw new InvalidSNASpecifierException( "Invalid Matrix SaveValue Coordinate Parameter" );
					}

					snaTranslated.ExtraParameters.Add( parameter );
				}
				else
				{
					// FORMAT: posinteger | $name
					SNAParameter parameter = new SNAParameter();
					parameter.Value = coordinate;
					if( parameter.Value.StartsWith( Constants.SNA_TOKEN_SEPARATOR ) ) parameter.Value = parameter.Value.Substring( 1 );

					// Validate
					if( IsValidName( parameter.Value ) )
					{
						parameter.IsName = true;
					}
					else if( IsValidPositiveInteger( parameter.Value ) )
					{
						parameter.IsPosInteger = true;
					}
					else
					{
						throw new InvalidSNASpecifierException( "Invalid Matrix SaveValue Coordinate Parameter" );
					}

					snaTranslated.ExtraParameters.Add( parameter );
				}
			}
			#endregion

			return snaTranslated;
		}

		/// <summary>
		/// Process an SNA Entity Specifier. Could be a SNA with direct or indirect addressing.
		/// Throws an InvalidSNASpecifierException Exception if no valid format can be detected.
		/// </summary>
		/// <param name="snaTranslated"></param>
		/// <param name="specifier"></param>
		/// <returns></returns>
		private static SNATranslated TranslateSpecifier( SNATranslated snaTranslated, String specifier, SNAEntitySpecifierType especifierTypes ) 
		{
			int iDummyValue = Constants.DEFAULT_ZERO_VALUE;

			if( ( ( especifierTypes & SNAEntitySpecifierType.IndirectAddressing ) == SNAEntitySpecifierType.IndirectAddressing ) 
				&& specifier.StartsWith( Constants.INDIRECT_ADDRESSING_TOKEN ) )
			{
				// SNA WITH INDIRECT ADDRESING, AVAILABLE FORMATS: [SNA]*posinteger | [SNA]*name | [SNA]*$name
				SNAParameter parameter = new SNAParameter();
				// Only take value (no tokens)
				parameter.Value = specifier.Substring( 1 );
				parameter.IndirectAddressing = true;
				if( parameter.Value.StartsWith( Constants.SNA_TOKEN_SEPARATOR ) ) parameter.Value = parameter.Value.Substring( 1 );

				// Validate
				if( IsValidName( parameter.Value ) ) 
				{
					parameter.IsName = true;
				}
				else if( IsValidPositiveInteger( parameter.Value ) )
				{
					parameter.IsPosInteger = true;
				}
				else
				{
					throw new InvalidSNASpecifierException( "Invalid SNA Parameter" );
				}

				snaTranslated.Parameter = parameter;
				return snaTranslated;
			}
			else if( ( ( especifierTypes & SNAEntitySpecifierType.Name ) == SNAEntitySpecifierType.Name )
					&& specifier.StartsWith( Constants.SNA_TOKEN_SEPARATOR ) )
			{
				// SNA WITH NAME ADDRESSING, AVAILABLE FORMAT: [SNA]$name
				SNAParameter parameter = new SNAParameter();
				// Only take value (no tokens)
				parameter.Value = specifier.Substring( 1 );
				if( IsValidName( parameter.Value ) )
				{
					parameter.IsName = true;
				}
				else 
				{
					throw new InvalidSNASpecifierException( "Invalid SNA Parameter" );
				}

				snaTranslated.Parameter = parameter;
				return snaTranslated;
			}		
			else if( ( ( especifierTypes & SNAEntitySpecifierType.PositiveInteger ) == SNAEntitySpecifierType.PositiveInteger )
					&& Int32.TryParse( specifier, out iDummyValue ) )
			{
				// SNA WITH POSINTEGER ADDRESSING, AVAILABLE FORMAT: [SNA]posinteger
				SNAParameter parameter = new SNAParameter();
				parameter.Value = specifier;
				if( IsValidPositiveInteger( parameter.Value ) )
				{
					parameter.IsPosInteger = true;
				}
				else
				{
					throw new InvalidSNASpecifierException( "Invalid SNA Parameter" );
				}

				snaTranslated.Parameter = parameter;
				return snaTranslated;
			}
			else
			{
				throw new InvalidSNASpecifierException( "Invalid SNA Parameter" );
			}
		}
		#endregion

		#region Is Valid Methods
		/// <summary>
		/// Check if a expression is a valid Name. True if valid, False otherwise.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static bool IsValidName( String expression )
		{
			Regex regEx = new Regex( Constants.REGEX_VALID_NAME );

			return regEx.IsMatch( expression );
		}

		/// <summary>
		/// Check if a expression is a valid Positive Integer. True if valid, False otherwise.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static bool IsValidPositiveInteger( String expression )
		{
			Regex regEx = new Regex( Constants.REGEX_VALID_POSITIVE_INTEGER );

			return regEx.IsMatch( expression );
		}
		#endregion

		#region StartWithSNA
		public static bool StartsWithSNAPrefix( String expression, String prefix )
		{
			
			if( prefix.Equals( SNAPrefixes.P_indirect ) ) 
			{
				// First Special Case for Parameter Indirect SNA Addressing
				
				if ( expression.StartsWith( prefix ) ) return true;
			} 
			else if ( prefix.Equals( SNAPrefixes.MX ) )
			{
				// Second Special Case for Matrix Save Values SNA
				
				String strToCompareWithSeparatorToken = String.Format( "{0}{1}" , prefix , Constants.SNA_TOKEN_SEPARATOR );
				if ( expression.StartsWith( strToCompareWithSeparatorToken ) ) return true;

				String strToCompareWithIndirectAddressingToken = String.Empty;
				strToCompareWithIndirectAddressingToken = String.Format( "{0}{1}" ,
																		 prefix , Constants.INDIRECT_ADDRESSING_TOKEN );
				if ( expression.StartsWith( strToCompareWithIndirectAddressingToken ) ) return true;

				String strToCompareWithPositiveInteger = String.Format( "^{0}{1}" , prefix , Constants.REGEX_VALID_SNA_WITH_POS_INTEGER_SUFIX );
				Regex regEx = new Regex( strToCompareWithPositiveInteger );
				if( regEx.IsMatch( expression ) ) return true;
			} 
			else 
			{
				// Rest of SNA run through here...

				String strToCompareWithSeparatorToken = String.Format( "{0}{1}" , prefix , Constants.SNA_TOKEN_SEPARATOR );
				if( expression.StartsWith( strToCompareWithSeparatorToken ) ) return true;

				String strToCompareWithIndirectAddressingToken = String.Empty;
				strToCompareWithIndirectAddressingToken = String.Format( "{0}{1}" , 
																		 prefix , Constants.INDIRECT_ADDRESSING_TOKEN );		
				if ( expression.StartsWith( strToCompareWithIndirectAddressingToken ) ) return true;

				String strToCompareWithPositiveInteger = String.Format( "^{0}{1}$" , prefix , Constants.REGEX_VALID_SNA_WITH_POS_INTEGER_SUFIX );
				Regex regEx = new Regex( strToCompareWithPositiveInteger );
				if( regEx.IsMatch( expression ) ) return true;
			}
			return false;
		}
		#endregion
	}
}
