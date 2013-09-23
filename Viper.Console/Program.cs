using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Viper.Framework.Engine;

namespace Viper.Console
{
	class Program
	{
		/// <summary>
		/// 
		/// </summary>
		private static void printHelp()
		{
			System.Console.Out.WriteLine( "" );
			System.Console.Out.WriteLine( "HELP INFO:" );
			System.Console.Out.WriteLine( "=============" );
			System.Console.Out.WriteLine( "Usage: /Viper.Console.exe -F <filepath> -S <termination_count>" );
			System.Console.Out.WriteLine( "" );
			System.Console.Out.WriteLine( "Use '/F <filepath>' to use a text file as the gpss model." );
			System.Console.Out.WriteLine( "Use '/S <termination_count>' to set the termination count." );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filepath"></param>
		/// <returns></returns>
		private static String getGPSSModelFromFile( String filepath )
		{
			String strGPSSModel = String.Empty;

			strGPSSModel = File.ReadAllText( filepath );

			return strGPSSModel;
		}

		public static void Main( string[] args )
		{
			System.Console.Clear();
			System.Console.Out.WriteLine( "Viper Console" );
			System.Console.Out.WriteLine( "=============" );
			System.Console.Out.WriteLine( "" );
			if( args.Length == 1 )
			{
				System.Console.Out.WriteLine( "Wrong arguments, please enter '-?' or '-H' to see Help Info." );
			}
			else
			{
				bool bNextArgumentIsFilename = false;
				bool bNextArgumentIsTerminationCount = false;
				String strGPSSModel = String.Empty;
				int iTerminationCount = 1;
				foreach( string argument in args )
				{
					String theArgument = argument.Trim();

					if( bNextArgumentIsFilename )
					{
						bNextArgumentIsFilename = false;
						if( !String.IsNullOrEmpty( theArgument ) )
						{
							try
							{
								strGPSSModel = getGPSSModelFromFile( theArgument );
							}
							catch( Exception ex )
							{
								System.Console.Out.WriteLine( "An exception occurred while getting GPSS model from file '{0}'", theArgument );
								System.Console.Out.WriteLine( "Error Message: {0}", ex.Message );
								break;
							}
						}
					}
					else if( bNextArgumentIsTerminationCount )
					{
						bNextArgumentIsTerminationCount = false;
						if( !String.IsNullOrEmpty( theArgument ) )
						{
							try
							{
								iTerminationCount = Convert.ToInt32( theArgument );
							}
							catch( Exception ex )
							{
								System.Console.Out.WriteLine( "An exception occurred while getting Simulation Termination Count" );
								System.Console.Out.WriteLine( "Error Message: {0}", ex.Message );
								break;
							}
						}
					}
					else
					{
						theArgument = theArgument.ToUpperInvariant();
						if( theArgument.Equals( "-?" ) || theArgument.Equals( "-H" ) )
						{
							printHelp();
							break;
						}
						else if( theArgument.Equals( "-F" ) )
						{
							bNextArgumentIsFilename = true;
						}
						else if( theArgument.Equals( "-S" ) )
						{
							bNextArgumentIsTerminationCount = true;
						}
					}
				}

				if( !String.IsNullOrEmpty( strGPSSModel ) )
				{
					try
					{
						System.Console.WriteLine( "Starting Simulation at {0}...", DateTime.Now );
						System.Console.WriteLine( "" );
						
						ViperSystem.Instance().Simulate( strGPSSModel, iTerminationCount );

						System.Console.WriteLine( "Ending Simulation at {0}...", DateTime.Now );
						System.Console.WriteLine( "" );

						System.Console.WriteLine( "Press any key to Print Simulation Report..." );
						System.Console.WriteLine( "" );

						System.Console.ReadKey( true );

						String strReport = ViperSystem.Instance().GetFinalReport();
						System.Console.Write( strReport );

						System.Console.WriteLine( "Press any key to Exit..." );
						System.Console.WriteLine( "" );

						System.Console.ReadKey( true );
					}
					catch( Exception ex )
					{
						System.Console.Out.WriteLine( "An exception occurred during Viper Simulation." );
						System.Console.Out.WriteLine( "Error Message: {0}", ex.Message );
					}
				}
				else
				{
					System.Console.Out.WriteLine( "GPSS Model in file is empty" );
				}
			}
		}
	}
}
