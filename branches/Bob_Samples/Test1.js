var A = new ActiveXObject("ASCOMTest.CollectionTest");

var e = new Enumerator(A.Names);		// An ArrayList of Person in C#
for (;!e.atEnd(); e.moveNext())
	WScript.StdOut.WriteLine(e.item());

WScript.StdOut.WriteLine();

e = new Enumerator(A.People)			// A List<Person> in C#
for (;!e.atEnd(); e.moveNext())
	WScript.StdOut.WriteLine(e.item().Name + " is a " + e.item().Role);

WScript.StdOut.WriteLine();
WScript.StdOut.WriteLine("Press return to exit...");
WScript.StdIn.ReadLine();