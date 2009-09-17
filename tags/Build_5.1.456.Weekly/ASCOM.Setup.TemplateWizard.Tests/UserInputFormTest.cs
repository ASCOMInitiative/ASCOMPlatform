using ASCOM.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ASCOM.Setup.TemplateWizard.Tests
{
    
    
    /// <summary>
    ///This is a test class for UserInputFormTest and is intended
    ///to contain all UserInputFormTest Unit Tests
    ///</summary>
	[TestClass()]
	public class UserInputFormTest
	{


		private TestContext testContextInstance;

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
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//


		/// <summary>
		/// Gets or sets the FRM. Form instance used for all tests.
		/// </summary>
		/// <value>The FRM.</value>
		static UserInputForm frm { get; set; }

		[ClassInitialize()]
		public static void MyClassInitialize(TestContext testContext)
		{
			frm = new UserInputForm();
			frm.ShowDialog();
		}


		#endregion


		/// <summary>
		///A test for Namespace
		///</summary>
		[TestMethod()]
		public void NamespaceTest()
		{
			UserInputForm target = new UserInputForm(); // TODO: Initialize to an appropriate value
			string actual;
			actual = target.Namespace;
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for DeviceName
		///</summary>
		[TestMethod()]
		public void DeviceNameTest()
		{
			UserInputForm target = new UserInputForm(); // TODO: Initialize to an appropriate value
			string actual;
			actual = target.DeviceName;
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for DeviceId
		///</summary>
		[TestMethod()]
		public void DeviceIdTest()
		{
			UserInputForm target = new UserInputForm(); // TODO: Initialize to an appropriate value
			string actual;
			actual = target.DeviceId;
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for DeviceClass
		///</summary>
		[TestMethod()]
		public void DeviceClassTest()
		{
			UserInputForm target = new UserInputForm(); // TODO: Initialize to an appropriate value
			string actual;
			actual = target.DeviceClass;
			Assert.Inconclusive("Verify the correctness of this test method.");
		}
	}
}
