using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Usno;

namespace Usno.NovasTestConsoleApp
	{
	class Program
		{
		static void Main(string[] args)
			{
			double tjd = 2454970.5563889;
			DateTime expected = new DateTime(2009, 5, 19, 01, 21, 12, 10, DateTimeKind.Utc);
			DateTime actual;
			actual = Novas.cal_date(tjd);
			Console.WriteLine("Julian date {0} is equivalent to Gregorian date {1}#{2} UT", tjd, actual, actual.Millisecond);
			Console.ReadLine();
			}
		}
	}
