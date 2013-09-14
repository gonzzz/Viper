using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Utils
{
	public class RandomGenerator
	{
		private static RandomGenerator rm = null;
		private Dictionary<int, Random> privateRandoms;

		private RandomGenerator()
		{
			privateRandoms = new Dictionary<int , Random>();
		}

		public static RandomGenerator Instance()
		{
			if( rm == null ) rm = new RandomGenerator();

			return rm;
		}

		private int GenerateSeed()
		{
			string strNowTicks = DateTime.Now.Ticks.ToString(); // yyyymmddhhmmssnnnn

			return Convert.ToInt32( strNowTicks.Substring( 10, strNowTicks.Length - 10 ) );
		}


		/// <summary>
		/// Returns a random number of a given family
		/// </summary>
		/// <param name="iRandomFamily"></param>
		/// <param name="newRandomizer"></param>
		/// <returns></returns>
		public double GenerateRandom( int iRandomFamily, bool newRandomizer = false )
		{
			if( newRandomizer || !privateRandoms.ContainsKey( iRandomFamily ) ) {
				if( privateRandoms.ContainsKey( iRandomFamily ) ) privateRandoms.Remove( iRandomFamily );
				privateRandoms.Add( iRandomFamily, new Random( GenerateSeed() ) );
			}

			return ( privateRandoms[ iRandomFamily ].NextDouble() * 2 ) - 1;
		}

		/// <summary>
		/// Applies a random number of a given family to a given Max Desviation (integer value) 
		/// in order to retreive a random desviation
		/// </summary>
		/// <param name="iRandomFamily"></param>
		/// <param name="iDesviation"></param>
		/// <returns></returns>
		public int GenerateRandomWithDesviation( int iRandomFamily, int iDesviation )
		{
			return Convert.ToInt32( Math.Round( GenerateRandom( iRandomFamily ) * iDesviation, 0 ) );
		}
	}
}
