using System.Collections.Generic;
using System.Linq;

namespace ASCOM.Platform.UnitTest
	{
	/// <summary>
	/// Extension methods that make it easier to assert various things.
	/// </summary>
	public static class AssertExtensions
		{
		public static bool IsEmpty<T>(this IEnumerable<T> collection)
			{
			int count = collection.Count();
			return (count == 0);
			}
		public static bool NotEmpty<T>(this IEnumerable<T> collection)
			{
			return (!collection.IsEmpty());
			}
		}
	}
