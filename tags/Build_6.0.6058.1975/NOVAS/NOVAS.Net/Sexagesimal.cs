using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TiGra.Astronomy
{
    /// <summary>
    /// Privides static methods and properties for converting and parsing sexagesimal values.
    /// Fot the purposes of this software, "sexagesimal" means any number having a whole part,
    /// (for example, 0 - 23 hours, 0 - 359 degrees) plus a number of minutes and seconds.
    /// The whole part of the number can be positive or negative and of any magnitude.
    /// Minutes and seconds are unsigned (rather, they take their sign from the whole part)
    /// and are in the range 0 <= x < 60.
    /// </summary>
	/// <remarks>
	/// This helper function is designed to work in a number of different situations
	/// for strings containing angles; right ascension; declination;
	/// altitude and azimuth so it is necessary to accept a variety of delimiters
	/// after the first numberic field. Some of these delimiters may not be obvious but are required
	/// to support the LX-200 protocol, for example.
	/// </remarks>
    public sealed class Sexagesimal
    {
		//const string pattern = @"^(?<Sign>[+-]?)(?<Whole>\d+)(?:([dDhH\:°\x3f]\s*)|\s+)(?<Minutes>\d{1,2})((?:([Mm:']\s*)|\s+)(?<Seconds>\d{1,2})(?:([Ss""]?)))?.*$";
		const string pattern = @"^(?<Sign>[+-]?)(?<Whole>\d+)(?:[^0-9]+)(?<Minutes>\d{1,2})((?:[^0-9]+)(?<Seconds>\d{1,2})(?:[^0-9]*))?.*$";
        static Regex regexSexagesimal = new Regex(pattern);

        /// <summary>
        /// Parses a sexagesimal string and converts it to floating point.
        /// </summary>
        /// <param name="sg">String containing a sexagesimal quatity. Various formats are permitted.</param>
        /// <returns>The equivalent value expressed in floating point.</returns>
        /// <exception cref="">Thrown if the supplied string could not be parsed.</exception>
        /// <remarks>
        /// The following sexagesimal formats are explicitly supported.
        /// <list type="">
        /// <listheader>
        /// <item>Syntax</item>
        /// <description>Description</description>
        /// </listheader>
        /// <item>HH:MM:SS</item>
        /// <description>Time in hours, minutes and seconds.</description>
        /// <item>sDD:MM:SS</item>
        /// <description>Signed degrees minutes and seconds.</description>
        /// </list>
        /// </remarks>
        public static double Parse(string sg)
        {
            if (!IsValid(sg))
                throw new ArgumentException("Not a valid sexagesimal string", "sg");
            Match sgParsed = regexSexagesimal.Match(sg);
            double sign = (sgParsed.Groups["Sign"].Value == "-" ? -1.0 : +1.0);
            double whole = Convert.ToDouble(sgParsed.Groups["Whole"].Value);
			double minutes = 0.0;
			if (sgParsed.Groups["Minutes"].Success)
			{
				minutes = Convert.ToDouble(sgParsed.Groups["Minutes"].Value);
			}
			double seconds = 0.0;
			if (sgParsed.Groups["Seconds"].Success)
			{
				seconds = Convert.ToDouble(sgParsed.Groups["Seconds"].Value);
			}
            double result = (whole + minutes / 60.0 + seconds / 3600.0) * sign;
            return result;
        }
        /// <summary>
        /// Checks whether the supplied string is in a valid sexagesimal format.
        /// This check validates only the syntax of the string, it does not perform
        /// any validity or range checking on the values.
        /// </summary>
        /// <param name="sg">The candidate string to be validated.</param>
        /// <returns></returns>
        public static bool IsValid(string sg)
        {
            return regexSexagesimal.IsMatch(sg);
        }
        /// <summary>
        /// Converts a numeric declination into a string in the format used by the LX200 telescope protocol.
        /// </summary>
        /// <param name="dDec">Numeric declination in degrees.</param>
        /// <returns>A string containing the declination in the format used by the LX200 protocol.</returns>
        public static string LX200LongDeclination(double dDec)
        {
            Declination d = new Declination();
            d.DecimalDegrees = dDec;
            return String.Format("{0}{1:d2}°{2:d2}:{3:d2}", d.IsNorth ? "+" : "-", Math.Abs(d.Degrees), d.Minutes, d.Seconds);
        }
		/// <summary>
		/// Format a decimal representing hours, minutes and seconds as a human-readable string
		/// in a consistent format.
		/// </summary>
		/// <param name="d">The time or hour angle to be formatted.</param>
		/// <returns>A string containing the human-readable representation of the value.</returns>
		public static string FormatHMS(double d)
		{
			HourAngle ha = new HourAngle();
			ha.DecimalDegrees = d;
			return ha.ToString();
		}
		/// <summary>
		/// Format a decimal angle representing degrees, minutes and seconds as a human-readable string.
		/// </summary>
		/// <param name="d">The angle or declination to be formatted.</param>
		/// <returns>A string containing the human-readable representation of the value.</returns>
		public static string FormatDMS(double d)
		{
			Bearing b = new Bearing(Math.Abs(d));
			return String.Format("{0}{1}", (Math.Sign(d) == -1 ? '-' : '+'), b.ToString());
			//Declination dec = new Declination();
			//dec.DecimalDegrees = d;
			//return dec.ToString();
		}
    }
}
