using ASCOM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Specialized;
using Moq;
using ASCOM.Utilities.Interfaces;
using System.Reflection;

namespace ASCOM.Platform.Test
{
    
    
    /// <summary>
    ///This is a test class for SettingsProviderTest and is intended
    ///to contain all SettingsProviderTest Unit Tests
    ///</summary>
	[TestClass()]
	public class SettingsProviderTest
		{
		private const string csUnitTest = "UnitTest";


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
		#endregion


		/// <summary>
		///A test for ApplicationName
		///</summary>
		[TestMethod()]
		public void ApplicationName_ShouldReturn_EmptyString_When_ProviderNotInitialized()
			{
			var mockProfile = new Mock<IProfile>();
			SettingsProvider_Accessor target = new SettingsProvider_Accessor(mockProfile.Object);
			string expected = string.Empty;
			string actual = target.ApplicationName;
			Assert.AreEqual(expected, actual);
			}

		[TestMethod]
		public void ApplicationName_ShouldReturn_AssemblyName_When_ProviderInitializedWith_NullName()
			{
			var mockProfile = new Mock<IProfile>();
			Assembly assy = Assembly.GetExecutingAssembly();
			string expected = assy.GetName().Name;
			SettingsProvider target = new SettingsProvider(mockProfile.Object);
			target.Initialize(null, null);
			string actual = target.ApplicationName;
			Assert.AreEqual(expected, actual);
			}

		[TestMethod]
		public void ApplicationName_ShouldReturn_Name_When_ProviderInitializedWith_Name()
			{
			var mockProfile = new Mock<IProfile>();
			Assembly assy = Assembly.GetExecutingAssembly();
			string expected = csUnitTest;
			SettingsProvider target = new SettingsProvider(mockProfile.Object);
			target.Initialize(csUnitTest, null);
			string actual = target.ApplicationName;
			Assert.AreEqual(expected, actual);
			}


		/// <summary>
		///A test for Description
		///</summary>
		[TestMethod()]
		public void DescriptionShouldContainText()
			{
			SettingsProvider target = new SettingsProvider(); // TODO: Initialize to an appropriate value
			string actual = target.Description;
			Assert.IsFalse(string.IsNullOrEmpty(actual));
			}

		/// <summary>
		///A test for Name
		///</summary>
		[TestMethod()]
		public void NameShouldContainText()
			{
			SettingsProvider target = new SettingsProvider();
			string actual = target.Name;
			Assert.IsFalse(string.IsNullOrEmpty(actual));
			}

		/// <summary>
		///A test for GetPropertyValues
		///</summary>
		[TestMethod()]
		public void GetPropertyValues_ShouldReturn_DefaultValue_When_DeviceNotRegistered()
			{
			////ARRANGE
			//SettingsProvider target = new SettingsProvider();
			//string expected = "DefaultValue";
			//string settingName = "SettingName";
			//// SettingsContext just needs a placeholder.
			//var mockContext = new Mock<SettingsContext>();
			//var mockAttributes = new Mock<SettingsAttributeDictionary>();
			//mockAttributes.Setup(a => a[It.Is<bool>(k => k != null)]).Returns<DeviceIdAttribute>(d => new DeviceIdAttribute("ASCOM.SettingsProvider.UnitTest"));
			//// SettingsProperty
			//var mockProperty = new Mock<SettingsProperty>();
			//mockProperty.Setup(p => p.DefaultValue).Returns(expected);
			//mockProperty.Setup(p => p.Name).Returns(settingName);
			//mockProperty.Setup(p => p.Attributes).Returns(mockAttributes.Object);
			//// Mock up a SettingsPropertyCollection
			//var mockCollection = new Mock<SettingsPropertyCollection>();
			//mockCollection.Setup(item => item[It.IsAny<string>()]).Returns(mockProperty.Object);

			//// ACT
			//var values = target.GetPropertyValues(mockContext.Object, mockCollection.Object);
			//var actual = values[settingName].PropertyValue;

			//// ASSERT
			//Assert.AreEqual(expected, actual);
			}

		/// <summary>
		///A test for Initialize
		///</summary>
		[TestMethod()]
		public void Initialize_ShouldSet_ApplicationName_Property()
			{
			SettingsProvider target = new SettingsProvider(); // TODO: Initialize to an appropriate value
			string name = csUnitTest;
			var mockNameValues = new Mock<NameValueCollection>();
			target.Initialize(name, mockNameValues.Object);
			Assert.AreEqual(target.ApplicationName, name);
			}

		/// <summary>
		///A test for SetPropertyValues
		///</summary>
		[TestMethod()]
		public void SetPropertyValuesTest()
			{
			//SettingsProvider target = new SettingsProvider(); // TODO: Initialize to an appropriate value
			//SettingsContext context = null; // TODO: Initialize to an appropriate value
			//SettingsPropertyValueCollection collection = null; // TODO: Initialize to an appropriate value
			//target.SetPropertyValues(context, collection);
			//Assert.Inconclusive("A method that does not return a value cannot be verified.");
			}
		}
}
