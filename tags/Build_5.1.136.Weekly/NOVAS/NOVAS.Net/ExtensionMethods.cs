using System;
using System.Text;

namespace TiGra.ExtensionMethods
	{
	/// <summary>
	/// Extension methods for manipulating strings.
	/// </summary>
	public static class StringExtensions
		{
		/// <summary>
		/// Returns the specified number of characters from the head of a string.
		/// </summary>
		/// <param name="source">The source string.</param>
		/// <param name="length">The number of characters to be returned, must not be greater than the length of the string.</param>
		/// <returns>The specified number of characters from the head of the source string, as a new string.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the requested number of characters exceeds the string length.</exception>
		public static string Head(this string source, int length)
			{
			if (length > source.Length)
				throw new ArgumentOutOfRangeException("The specified length is greater than the length of the string.");
			return source.Substring(0, length);
			}
		/// <summary>
		/// Returns the specified number of characters from the tail of a string.
		/// </summary>
		/// <param name="source">The source string.</param>
		/// <param name="length">The number of characters to be returned, must not be greater than the length of the string.</param>
		/// <returns>The specified number of characters from the tail of the source string, as a new string.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the requested number of characters exceeds the string length.</exception>
		public static string Tail(this string source, int length)
			{
			int srcLength = source.Length;
			if (length > srcLength)
				throw new ArgumentOutOfRangeException("The specified length is greater than the length of the string.");
			return source.Substring(srcLength - length, length);
			}
		/// <summary>
		/// Cleans (that is, removes all unwanted characters) from the string.
		/// </summary>
		/// <param name="source">The source string.</param>
		/// <param name="allowedCharacters">A list of the allowed characters. All other characters will be removed.</param>
		/// <returns>A new string with all of the unwanted characters deleted.</returns>
		public static string Clean(this string source, string allowedCharacters)
			{
			StringBuilder cleanString = new StringBuilder(source.Length);
			foreach (char ch in source)
				{
				if (allowedCharacters.IndexOf(ch) >= 0)
					cleanString.Append(ch);
				}
			return cleanString.ToString();
			}
		/// <summary>
		/// Remove the head of the string, leaving the tail.
		/// </summary>
		/// <param name="source">The source string.</param>
		/// <param name="length">Number of characters to remove from the head.</param>
		/// <returns>A new string containing the old string with <see cref="P:length"/> characters removed from the head.</returns>
		public static string RemoveHead(this string source, int length)
			{
			if (length < 1)
				return source;
			return source.Tail(source.Length - length);
			}
		/// <summary>
		/// Remove the tail of the string, leaving the head.
		/// </summary>
		/// <param name="source">The source string.</param>
		/// <param name="length">Number of characters to remove from the tail.</param>
		/// <returns>A new string containing the old string with <see cref="P:length"/> characters removed from the tail.</returns>
		public static string RemoveTail(this string source, int length)
			{
			if (length < 1)
				return source;
			return source.Head(source.Length - length);
			}
		/// <summary>
		/// Indicates whether a string is null or empty.
		/// <seealso cref="System.String.IssNullOrEmpty"/>
		/// </summary>
		/// <param name="source">The string to be tested.</param>
		/// <returns>Returns <c>true</c> if the string is
		/// null or equal to <see cref="String.Empty"/>.</returns>
		public static bool IsNullOrEmpty(this string source)
			{
			return String.IsNullOrEmpty(source);
			}
		}

	/// <summary>
	/// Contains extension methods that are used when performing trigonometry.
	/// </summary>
	public static class TrigonometryExtensions
		{
		public static double DegreesToRadians(this double degrees)
			{
			return degrees * Usno.Constants.DegreesToRadians;
			}
		public static double RadiansToDegrees(this double radians)
			{
			return radians * Usno.Constants.RadiansToDegrees;
			}
		public static double RadiansToArcseconds(this double radians)
			{
			return radians * Usno.Constants.RadiansToSeconds;
			}
		}
	}
