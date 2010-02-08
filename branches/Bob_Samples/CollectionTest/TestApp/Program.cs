using System;
using System.Text;
using ASCOMTest;

namespace TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			CollectionTest T = new CollectionTest();
			CollectionTest.PeopleWrapper PL = T.People;
			foreach (Person P in PL)
				Console.WriteLine(P.Name + " is a " + P.Role);
			Console.WriteLine("Press return to exit...");
			Console.ReadLine();
		}
	}
}
