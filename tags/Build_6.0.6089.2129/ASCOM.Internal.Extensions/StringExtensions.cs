using System;
using System.Linq;
using System.Text;

namespace ASCOM.Internal
{
    /// <summary>
    ///   String extension methods.
    /// </summary>
    /// <remarks>
    ///   Internal use only. Driver and application developers should not rely on this class,
    ///   because the interface and method signatures may change at any time.
    /// </remarks>
    public static class StringExtensions
    {
        /// <summary>
        ///   Returns the specified number of characters from the head of a string.
        /// </summary>
        /// <param name = "source">The source string.</param>
        /// <param name = "length">The number of characters to be returned, must not be greater than the length of the string.</param>
        /// <returns>The specified number of characters from the head of the source string, as a new string.</returns>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown if the requested number of characters exceeds the string length.</exception>
        public static string Head(this string source, int length)
        {
            if (length > source.Length)
                throw new ArgumentOutOfRangeException("source",
                                                      "The specified length is greater than the length of the string.");
            return source.Substring(0, length);
        }

        /// <summary>
        ///   Returns the specified number of characters from the tail of a string.
        /// </summary>
        /// <param name = "source">The source string.</param>
        /// <param name = "length">The number of characters to be returned, must not be greater than the length of the string.</param>
        /// <returns>The specified number of characters from the tail of the source string, as a new string.</returns>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown if the requested number of characters exceeds the string length.</exception>
        public static string Tail(this string source, int length)
        {
            int srcLength = source.Length;
            if (length > srcLength)
                throw new ArgumentOutOfRangeException("source",
                                                      "The specified length is greater than the length of the string.");
            return source.Substring(srcLength - length, length);
        }

        /// <summary>
        ///   Cleans (that is, removes all unwanted characters) from the string.
        /// </summary>
        /// <param name = "source">The source string.</param>
        /// <param name = "allowedCharacters">A list of the allowed characters. All other characters will be removed.</param>
        /// <returns>A new string with all of the unwanted characters deleted.</returns>
        public static string Clean(this string source, string allowedCharacters)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;
            var cleanString = new StringBuilder(source.Length);
            foreach (char ch in source)
            {
                if (allowedCharacters.Contains(ch)) cleanString.Append(ch);
            }
            return cleanString.ToString();
        }

        /// <summary>
        /// Remove the head of the string, leaving the tail.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="length">Number of characters to remove from the head.</param>
        /// <returns>
        /// A new string containing the old string with <paramref name="length"/> characters removed from the head.
        /// </returns>
        public static string RemoveHead(this string source, int length)
        {
            if (length < 1) return source;
            return source.Tail(source.Length - length);
        }

        /// <summary>
        ///   Remove the tail of the string, leaving the head.
        /// </summary>
        /// <param name = "source">The source string.</param>
        /// <param name = "length">Number of characters to remove from the tail.</param>
        /// <returns>A new string containing the old string with <paramref name="length"/> characters removed from the tail.</returns>
        public static string RemoveTail(this string source, int length)
        {
            if (length < 1) return source;
            return source.Head(source.Length - length);
        }
    }
}