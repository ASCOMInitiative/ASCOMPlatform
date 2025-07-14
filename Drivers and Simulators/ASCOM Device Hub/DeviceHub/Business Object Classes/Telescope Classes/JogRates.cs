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

        public static JogRates FromAxisRates(IRate[] axisRates)
        {
            JogRates rates = null;

            if (axisRates != null && axisRates.Length > 0)
            {
                rates = new JogRates();

                if (AreDiscreteRates(axisRates))
                {
                    rates = CreateDiscreteJogRates(axisRates);
                }
                else
                {
                    rates = CreateRangedJogRates(axisRates);
                }
            }

            return rates;
        }

        private static bool AreDiscreteRates(IRate[] axisRates)
        {
            bool retval = true;

            if (axisRates == null && axisRates.Length == 0)
            {
                retval = false;
            }
            else
            {
                foreach (IRate rate in axisRates)
                {
                    if (rate.Maximum - rate.Minimum > 0.001)
                    {
                        retval = false;

                        break;
                    }
                }
            }

            return retval;
        }

        private static JogRates CreateDiscreteJogRates(IRate[] axisRates)
        {
            JogRates jogRates = new JogRates();

            foreach (IRate rate in axisRates)
            {
                // This is a discrete rate value.

                JogRate jogRate = BuildJogRate(rate.Minimum);

                jogRates.Add(jogRate);
            }

            if (jogRates.Count == 0)
            {
                jogRates = null;
            }

            return jogRates;
        }

        private static JogRates CreateRangedJogRates(IRate[] axisRates)
        {
            JogRates jogRates = null;

            // Check whether the driver supports any axis rates
            if (axisRates == null || axisRates.Length == 0) // There are no axis rates
            {
                return jogRates; // Return null jogRates
            }

            // CHekc whether 1 or more rates are supported
            if (axisRates.Length == 1) // Only one rate is available
            {
                jogRates = DefaultJogRates(); // Assign the default jog rates

                // Check whether the default jog rates are valid for the single axis rate
                if (jogRates.First().Rate > axisRates[0].Minimum && jogRates.Last().Rate < axisRates[0].Maximum) // Defaults were valid so use them
                {
                    return jogRates;
                }
            }

            // We can't use the default jog rates, so build a valid list with a selection of jog rates within each supplied rate range

            jogRates = new JogRates();

            foreach (IRate axisRate in axisRates)
            {
                AddJogRate(axisRate.Minimum, jogRates); // Add the minimum rate
                AddJogRate(axisRate.Minimum + (axisRate.Maximum - axisRate.Minimum) * 0.25, jogRates); // Add a rate at 25% of the range
                AddJogRate(axisRate.Minimum + (axisRate.Maximum - axisRate.Minimum) * 0.5, jogRates); // Add a rate at 50% of the range
                AddJogRate(axisRate.Minimum + (axisRate.Maximum - axisRate.Minimum) * 0.75, jogRates); // Add a rate at 75% of the range
                AddJogRate(axisRate.Maximum, jogRates); // Add the maximum rate
            }

            return jogRates;
        }

        /// <summary>
        /// Add a jog rate to the list of jog rates if it is greater than zero.
        /// </summary>
        /// <param name="rateValue">Rate to add</param>
        /// <param name="jogRates">Current list of jog rates.</param>
        private static void AddJogRate(double rateValue, JogRates jogRates)
        {
            // Check whether the rate is greater than zero
            if (rateValue > 0.0) // The rate is greater than zero so add it to the list
            {
                JogRate jogRate = BuildJogRate(rateValue);
                jogRates.Add(jogRate);
            }
        }

        /// <summary>
        /// Create a JogRate object from a rate value.
        /// </summary>
        /// <param name="rateValue">Axis movement rate</param>
        /// <returns></returns>
        private static JogRate BuildJogRate(double rateValue)
        {
            string rateString = GetRateString(rateValue);

            return new JogRate { Name = rateString, Rate = rateValue };
        }

        private static string GetRateString(double rateValue)
        {
            double factor = GetSiderealRateFactor(rateValue);

            string retval;
            // If the rate is a modest multiple of the sidereal rate, return it as such otherwise return the rate in degrees per second
            if ((factor > 0.0) & (factor < 100.0)) // Return as a multiple of the sidereal rate
            {
                retval = $"{factor:###}X Sidereal";
            }
            else // Return as degrees per second
            {
                retval = $"{rateValue:0.##} °/sec";
            }

            return retval;
        }

        /// <summary>
        /// Calculates the factor by which a given rate value compares to the sidereal rate.
        /// </summary>
        /// <param name="rateValue">The rate value to be compared to the sidereal rate. Must be a positive number.</param>
        /// <returns>The rounded factor indicating how many times the given rate value corresponds to the sidereal rate.</returns>
        private static double GetSiderealRateFactor(double rateValue)
        {
            return Math.Round(rateValue / SIDEREAL_RATE, 0);
        }
    }
}
