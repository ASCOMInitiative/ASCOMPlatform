using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ASCOM.Test
	{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class DriverTemplateTests
		{
		public DriverTemplateTests()
			{
			DevicesToTest.Clear();
			foreach (string deviceClass in DeviceClasses)
				{
				string DeviceID = String.Format("ASCOM.{0}TemplateProject.{0}", deviceClass);
				DevicesToTest.Add(DeviceID, deviceClass);
				}
			}

		private TestContext testContextInstance;
		private List<string> DeviceClasses = new List<string>()
			{ "Camera", "Dome", "FilterWheel", "Focuser", "Rotator", "Switch", "Telescope" };
		private SortedList<string, string> DevicesToTest = new SortedList<string,string>();
		//private List<string> Devices; // Populated in the constructor.

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
			{
			get
				{
				return testContextInstance;
				}
			set
				{
				testContextInstance = value;
				}
			}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void TestDriverCreate()
			{
			foreach (var TestDevice in DevicesToTest)
				{
				//string loaderTypeName = String.Format("ASCOM.DriverAccess.{0}", TestDevice.Value);
				string loaderAssemblyName = String.Format("ASCOM.DriverAccess, Version=1.0.4.0, Culture=neutral, PublicKeyToken=565de7938946fba7, processorArchitecture=MSIL", TestDevice.Value);
				string loaderAssemblyQualifiedName = string.Format("ASCOM.DriverAccess.{0}, {1}", TestDevice.Value, loaderAssemblyName);
				Type loaderType = Type.GetType(loaderAssemblyQualifiedName);
				object loader = Activator.CreateInstance(loaderType, new object[] { TestDevice.Key });
				//object deviceName = loaderType.InvokeMember("Description", BindingFlags.GetProperty, null, loader, null);
				loaderType.InvokeMember("SetupDialog", BindingFlags.InvokeMethod, null, loader, new object[]{});
				//Console.WriteLine(deviceName);
				}
			}
		}
	}
