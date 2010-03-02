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
		private const string ItemName = "Item Name";
		private const string SettingName = "UnitTestSetting";
		private const string SettingDefaultValue = "Default";
		private const string SettingSetValue = "You've Been Set";
		private const string DeviceName = "My.UnitTest";
		private const string DeviceType = "Switch";



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
		public void Description_Should_ContainText()
			{
			SettingsProvider target = new SettingsProvider(); // TODO: Initialize to an appropriate value
			string actual = target.Description;
			Assert.IsFalse(string.IsNullOrEmpty(actual));
			}

		/// <summary>
		///A test for Name
		///</summary>
		[TestMethod()]
		public void Name_Should_Contain_Text()
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
			// ARRANGE
			var deviceId = String.Format("{0}.{1}", DeviceName, DeviceType);
			Mock<IProfile> mockProfile = new Mock<IProfile>();
			var deviceAttribute = new ASCOM.DeviceIdAttribute(deviceId);
			var attributes = new SettingsAttributeDictionary();
			attributes.Add(deviceAttribute.GetType(), deviceAttribute);
			var settingsProperty = new SettingsProperty(SettingName, typeof(string), null, false, SettingDefaultValue, SettingsSerializeAs.String, attributes, true, true);
			var propertyCollection = new SettingsPropertyCollection();
			propertyCollection.Add(settingsProperty);

			// Expectations:
			//	- mockProfile must have it's DeviceType set.
			//	- mockProfile's device type (captured in setDeviceType) must match deviceType.
			//	- The returned SettingsPropertyValueCollection must not be empty.
			//	- The returned SettingsPropertyValueCollection must have exactly one entry.
			//	- The entry must match the value of SettingDefaultValue. 
			var setDeviceType = String.Empty;
			mockProfile.SetupSet(x => x.DeviceType).Callback(y => setDeviceType = y);
			var settingsProvider = new SettingsProvider(mockProfile.Object);
			var context = new SettingsContext();

			// ACT
			var result = settingsProvider.GetPropertyValues(context, propertyCollection);

			// ASSERT
			Assert.AreEqual(DeviceType, setDeviceType);
			mockProfile.VerifyAll();
			Assert.IsTrue(result.Count > 0);
			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(result.OfType<SettingsPropertyValue>().Count() > 0);
			var settingsPropertyValue = result.OfType<SettingsPropertyValue>().First();
			Assert.IsFalse(settingsPropertyValue.IsDirty);
			Assert.AreEqual(SettingDefaultValue, settingsPropertyValue.PropertyValue);
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
			// ARRANGE
			var deviceId = String.Format("{0}.{1}", DeviceName, DeviceType);
			var deviceAttribute = new ASCOM.DeviceIdAttribute(deviceId);
			var attributes = new SettingsAttributeDictionary();
			attributes.Add(deviceAttribute.GetType(), deviceAttribute);
			var settingsProperty = new SettingsProperty(SettingName, typeof(string), null, false, SettingDefaultValue, SettingsSerializeAs.String, attributes, true, true);
			var settingsPropertyValue = new SettingsPropertyValue(settingsProperty);
			settingsPropertyValue.PropertyValue = SettingSetValue;
			var settingsPropertyValues = new SettingsPropertyValueCollection();
			settingsPropertyValues.Add(settingsPropertyValue);
			var context = new SettingsContext();

			// Expectations:
			//	- mockProfile must have it's DeviceType set.
			//	- mockProfile's device type (captured in setDeviceType) must match deviceType.
			//	- The returned SettingsPropertyValueCollection must not be empty.
			//	- IProfile.WriteValue should be called with the correct values(deviceId, item.Name, item.SerializedValue.ToString(), String.Empty);
			var setDeviceType = String.Empty;
			Mock<IProfile> mockProfile = new Mock<IProfile>();
			mockProfile.SetupSet(x => x.DeviceType).Callback(y => setDeviceType = y);
			mockProfile.Setup(x => x.WriteValue(deviceId, SettingName, SettingSetValue, String.Empty));
			var settingsProvider = new SettingsProvider(mockProfile.Object);

			// ACT
			settingsProvider.SetPropertyValues(context, settingsPropertyValues);

			// ASSERT
			Assert.AreEqual(DeviceType, setDeviceType);
			mockProfile.VerifyAll();
			}

		/// <summary>
		/// Verifies that, when setting property values,
		/// properties not decorated with an <see cref="ASCOM.DeviceIdAttribute"/>
		/// are silently ignored (per MS documentation) and that no attempt is made to write
		/// anything to the Profile store.
		/// </summary>
		[TestMethod]
		public void Non_Attributed_Settings_Should_Be_Silently_Ignored_When_Setting()
			{
			// ARRANGE
			var deviceId = String.Format("{0}.{1}", DeviceName, DeviceType);
			var deviceAttribute = new ASCOM.ServedClassNameAttribute("Dummy");	// Not a Device ID attribute
			var attributes = new SettingsAttributeDictionary();
			attributes.Add(deviceAttribute.GetType(), deviceAttribute);
			var settingsProperty = new SettingsProperty(SettingName, typeof(string), null, false, SettingDefaultValue, SettingsSerializeAs.String, attributes, true, true);
			var settingsPropertyValue = new SettingsPropertyValue(settingsProperty);
			settingsPropertyValue.PropertyValue = SettingSetValue;
			var settingsPropertyValues = new SettingsPropertyValueCollection();
			settingsPropertyValues.Add(settingsPropertyValue);
			var context = new SettingsContext();

			// Expectations:
			//	- mockProfile must NOT have it's DeviceType set.
			//	- IProfile.WriteValue should NOT be called.
			var setDeviceType = String.Empty;
			Mock<IProfile> mockProfile = new Mock<IProfile>();
			mockProfile.SetupSet(x => x.DeviceType).Callback(y => setDeviceType = y);
			//mockProfile.Setup(x => x.WriteValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
			var settingsProvider = new SettingsProvider(mockProfile.Object);

			// ACT
			settingsProvider.SetPropertyValues(context, settingsPropertyValues);

			// ASSERT
			Assert.AreEqual(String.Empty, setDeviceType);
			mockProfile.Verify(x => x.WriteValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
			}

		/// <summary>
		/// Verifies that, when getting property values,
		/// properties not decorated with an <see cref="ASCOM.DeviceIdAttribute"/>
		/// are silently ignored (per MS documentation) and that no attempt is made to write
		/// anything to the Profile store.
		/// </summary>
		[TestMethod]
		public void Non_Attributed_Settings_Should_Be_Silently_Ignored_When_Getting()
			{
			// ARRANGE
			var deviceId = String.Format("{0}.{1}", DeviceName, DeviceType);
			var deviceAttribute = new ASCOM.ServedClassNameAttribute("Dummy");	// Not a Device ID attribute
			var attributes = new SettingsAttributeDictionary();
			attributes.Add(deviceAttribute.GetType(), deviceAttribute);
			var settingsProperty = new SettingsProperty(SettingName, typeof(string), null, false, SettingDefaultValue, SettingsSerializeAs.String, attributes, true, true);
			var settingsProperties = new SettingsPropertyCollection();
			settingsProperties.Add(settingsProperty);
			var context = new SettingsContext();

			// Expectations:
			//	- mockProfile must NOT have it's DeviceType set.
			//	- IProfile.GetValue() must NOT be called.

			var setDeviceType = String.Empty;
			Mock<IProfile> mockProfile = new Mock<IProfile>();
			mockProfile.SetupSet(x => x.DeviceType).Callback(y => setDeviceType = y);
			//mockProfile.Setup(x => x.WriteValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
			var settingsProvider = new SettingsProvider(mockProfile.Object);

			// ACT
			settingsProvider.GetPropertyValues(context, settingsProperties);

			// ASSERT
			Assert.AreEqual(String.Empty, setDeviceType);
			mockProfile.Verify(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
			}
		}
	}
