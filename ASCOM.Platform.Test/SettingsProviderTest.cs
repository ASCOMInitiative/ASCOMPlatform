using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using ASCOM;
using ASCOM.Utilities.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
		private const string itemName = "Item Name";
		private readonly Mock<SettingsProperty> mockSettingsProperty = new Mock<SettingsProperty>();
		private readonly Mock<IProfile> mockProfile = new Mock<IProfile>();
		private readonly Mock<SettingsContext> mockSettingsContext = new Mock<SettingsContext>();


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

		// It is a good idea to have a method that handles the instantiation of the class we want to test
		// as that gives us only a single line to change if we change something in the class constructor(s).
		private SettingsProvider CreateSettingsProvider()
			{
			return new SettingsProvider(mockProfile.Object);
			}


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
		/// Verifies that teh SettingsProvider returns default values for settings when the device
		/// has not been registered with the ASCOM Chooser. This is necessary so that settings 
		/// work correctly at design-time.
		/// </summary>
		[TestMethod]
		public void GetPropertySettings_Should_Return_Default_Values_When_Device_Not_Registered()
			{
			const string deviceName = "My.UnitTest";
			const string deviceType = "Switch";
			var deviceId = String.Format("{0}.{1}", deviceName, deviceType);

			// Create our SettingsProvider instance
			var settingsProvider = CreateSettingsProvider();

			// The first part of the method parses the DeviceId. I would consider isolating this part in one method
			// to test it independently from the rest of the method. 

			// We con't need to fire up all of System.Configuration just for our test, so we keep this mock
			var mockItem = new Mock<SettingsProperty>();
			// Here we instantiate the DeviceIdAttribute and set its DeviceId property explicitely at the same time
			var deviceAttribute = new ASCOM.DeviceIdAttribute(deviceId);
			// Now we let the Attributes array property on our itemMock always return the deviceAttribute

			//[TPL] this is where it all goes a bit pear-shaped.
			var attributes = new SettingsAttributeDictionary();
			attributes.Add(typeof(DeviceIdAttribute), deviceAttribute);
			var mockAttributes = new Mock<SettingsAttributeDictionary>();
			//mockAttributes.SetupGet(g => g.Values).Returns(keys);
			mockItem.SetupGet(x => x.Attributes[It.IsAny<System.Type>()]).Returns(deviceAttribute);

			// There's no need to mock the collection, but we instantiate it with our itemMock object
			var propertyCollection = new SettingsPropertyCollection();
			propertyCollection.Add(mockItem.Object);

			// We wish to log what the device type of our IProfile is set to
			var setDeviceType = "";
			mockProfile.SetupSet(x => x.DeviceType).Callback(y => setDeviceType = y);

			// Now comes the interesting part where we call our IProfile - this is where we really need Moq.
			// Again, it would be easier if we could just write a few dedicated tests that test only this
			// try-catch part of the method...

			// For this we need to name our itemMock and give it a default value
			mockItem.SetupGet(x => x.Name).Returns(itemName);
			var defaultvalue = new Object();
			mockItem.SetupGet(x => x.DefaultValue).Returns(defaultvalue);
			// We need to set up the GetValue method. As we know all the expected parameters for it, we can specify
			// them already and make the mock stronger. In this test case, we test the handling of an empty return.
			mockProfile.Setup(x => x.GetValue(deviceId, itemName, null, String.Empty)).Returns("");

			// Finally, it is time to call the method we want to test
			var result = settingsProvider.GetPropertyValues(mockSettingsContext.Object, propertyCollection);

			// Now lets verify that everything went as expected

			// First, let's test that the parsing of DeviceId was as expected: IProvider.DeviceType was set to the expected value
			Assert.AreEqual(deviceType, setDeviceType);
			// Then let's test that the methods of IProvider that we mocked were called
			mockProfile.VerifyAll();
			// Instead of these two checks, we can use the VerifySet and Verify methods explicitly on our providerMock
			mockProfile.VerifySet(x => x.DeviceType = deviceType, Times.Once());
			mockProfile.Verify(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());

			// With this done, let's turn to the output of the method

			// Firstly, we test that the resulting collection contains exactly one item of the type SettingsPropertyValue
			Assert.IsTrue(result.Count > 0);
			Assert.IsTrue(result.OfType<SettingsPropertyValue>().Count() > 0);
			Assert.AreEqual(1, result.Count);
			// Then let's inspect the contained SettingsProviderValue further
			var settingsPropertyValue = result.OfType<SettingsPropertyValue>().First();
			// The IsDirty flag must never be set
			Assert.IsFalse(settingsPropertyValue.IsDirty);
			// The PropertyValue must be the default value we passed in in our itemMock as we set our
			// IProvider to return an empty value for the specified parameters
			Assert.AreEqual(defaultvalue, settingsPropertyValue.PropertyValue);
			}

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
