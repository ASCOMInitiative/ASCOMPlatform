using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOMTest
{
	//
	// COM-Visible interfaces
	//
	[Guid("E3B3FF7E-E8B8-418c-918F-38E23C0BA011")]						// Force InterfaceID for stability (typ.)
	public interface ICollectionTest
	{
		ArrayList Names { get; }
		CollectionTest.PeopleWrapper People { get; }
	}

	[Guid("A62EB3AA-ABA6-4a25-A9E0-35852C8004B9")]
	public interface IPerson
	{
		string Name { get; }
		string Role { get; }
	}

	//
	// Hide this type from COM, but still need the GUID to 
	// nail down its appearance in the registry (typ.)
	//
	[
		Guid("C1AB3273-CE24-4822-9DD2-1E547DEAB7BD"),
		ClassInterfaceAttribute(ClassInterfaceType.None)
	]
	public class Person
	{
		private string _role;
		private string _name;

		public Person(string name, string role)
		{
			_name = name;
			_role = role;
		}

		public string Role
		{
			get { return _role; }
		}

		public string Name
		{
			get { return _name; }
		}
	}

	[
		Guid("55E73C4D-0287-44e0-974B-3D361D541056"),
		ClassInterfaceAttribute(ClassInterfaceType.None)
	]
	public class CollectionTest : ICollectionTest
	{
		// Provide generic enumerator to COM and List<Person> enumerator to C#
		public class PeopleWrapper : IEnumerable, IEnumerable<Person>
		{
			private List<Person> _list;

			public PeopleWrapper(List<Person> people) { _list = people; }

			IEnumerator<Person> IEnumerable<Person>.GetEnumerator() { return _list.GetEnumerator(); }

			[DispId(-4)]	// COM-known method ID 
			public IEnumerator GetEnumerator() { return _list.GetEnumerator(); }
		}

		private ArrayList _al;
		private List<Person> _people;
		private PeopleWrapper _pl;

		public CollectionTest()
		{
			string[] _names = new string[] { "Tom", "Dick", "Harry" };
			string[] _roles = new string[] { "manager", "flunky", "manager" };
			_al = new ArrayList();
			_people = new List<Person>();

			for (int i = 0; i < _names.Length; i++)
			{
				_al.Add(_names[i]);
				_people.Add(new Person(_names[i], _roles[i]));
			}
			_pl = new PeopleWrapper(_people);
		}

		public ArrayList Names
		{
			get { return _al; }
		}

		public PeopleWrapper People
		{
			get { return _pl; }
		}

	}
}
