using System;
using System.Collections.Generic;
using System.Linq;
using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public class JogRates : List<JogRate>
	{
		private static readonly double SIDEREAL_RATE = 0.004178; // degrees per second

		public static JogRates DefaultJogRates()
		{
			JogRates rates = new JogRates
			{
				new JogRate { Name = "16X Sidereal", Rate = 0.06685 },
				new JogRate { Name = "64X Sidereal", Rate = 0.26740 },
				new JogRate { Name = "2 deg/sec", Rate = 2.0 }
			};

			return rates;
		}

		public static JogRates FromAxisRates( IRate[] axisRates )
		{
			JogRates rates = null;

			if ( axisRates != null && axisRates.Length > 0 )
			{
				rates = new JogRates();

				if ( AreDiscreteRates( axisRates ))
				{
					rates = CreateDiscreteJogRates( axisRates );
				}
				else
				{
					rates = CreateRangedJogRates( axisRates );
				}
			}

			return rates;
		}

		private static bool AreDiscreteRates( IRate[] axisRates )
		{
			bool retval = true;

			if ( axisRates == null && axisRates.Length == 0 )
			{
				retval = false;
			}
			else
			{
				foreach ( IRate rate in axisRates )
				{
					if ( rate.Maximum - rate.Minimum > 0.001 )
					{
						retval = false;

						break;
					}
				}
			}

			return retval;
		}

		private static JogRates CreateDiscreteJogRates( IRate[] axisRates )
		{
			JogRates jogRates = new JogRates();

			foreach ( IRate rate in axisRates )
			{
				// This is a discrete rate value.

				JogRate jogRate = BuildJogRate( rate.Minimum );

				jogRates.Add( jogRate );
			}

			if ( jogRates.Count == 0 )
			{
				jogRates = null;
			}

			return jogRates;
		}

		private static JogRates CreateRangedJogRates( IRate[] axisRates )
		{
			JogRates jogRates = null;

			if ( axisRates == null || axisRates.Length == 0 )
			{
				return jogRates;
			}

			if ( axisRates.Length == 1 )
			{
				jogRates = DefaultJogRates();

				if ( jogRates.First().Rate > axisRates[0].Minimum && jogRates.Last().Rate < axisRates[0].Maximum )
				{
					// Here the default rates are valid, so use them.

					return jogRates;
				}
			}

			// We can't use the default jog rates, so build a valid list.
			// The provided list of valid axis rates consists of multiple ranges so just take the average of each range.

			jogRates = new JogRates();

			foreach ( IRate axisRate in axisRates )
			{
				double rateValue = ( axisRate.Minimum + axisRate.Maximum ) / 2.0;

				JogRate jogRate = BuildJogRate( rateValue );

				jogRates.Add( jogRate );
			}

			return jogRates;
		}

		private static JogRate BuildJogRate( double rateValue )
		{
			string rateString = GetRateString( rateValue );

			return new JogRate { Name = rateString, Rate = rateValue };
		}

		private static string GetRateString( double rateValue )
		{
			string retval = null;

			double factor = GetSiderealRateFactor( rateValue );

			if ( factor < 100 )
			{
				retval = String.Format( "{0:###}X Sidereal", factor );
			}
			else 
			{			
				retval = String.Format( "{0:0.##} °/sec", rateValue );
			}

			return retval;
		}

		private static double GetSiderealRateFactor( double rateValue )
		{
			return Math.Round( rateValue / SIDEREAL_RATE, 0 );
		}
	}
}
