using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using ASCOM.Utilities.Interfaces;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

/*
 * Facts about SettingsProvider
 * 
 * For a driver under development that has never been registered (i.e. has no device profile),
 * SettingsProvider should:
 *	- return default values for all settings.
 *	- throw an exception when attempting to persist settings.
 *	
 * For a driver that has been registered with the platform (i.e. has a device profile),
 * SettingsProvider should:
 *	- return default values for settings that have no corresponding item in the profile store.
 *	- return the value from the profile store if it exists.
 *	- persist settings to the profile store, overwriting any existing entries or
 *		creating new entries as necessary.
 */

namespace ASCOM.Platform.UnitTest
{


    /// <summary>
    /// The base context
    ///</summary>
    public class With_mock_ascom_profile
    {
        protected const string csUnitTest = "UnitTest";
        protected const string ItemName = "Item Name";
        protected const string SettingName = "UnitTestSetting";
        protected const string SettingDefaultValue = "Default";
        protected const string SettingSetValue = "You've Been Set";
        protected const string DeviceName = "My.UnitTest";
        protected const string DeviceType = "Switch";
        protected static Mock<IProfile> MockProfile;
        protected static SettingsProvider Target;
        protected static string SetDeviceType;
        protected static SettingsContext SettingsContext;
        protected static SettingsAttributeDictionary FakeAttributeDictionary;
        protected static string DeviceId;
        protected static SettingsPropertyCollection SettingsProperties;

        Establish context = () =>
            {
                MockProfile=new Mock<IProfile>();
                Target = new SettingsProvider(MockProfile.Object);
                // Create and populate a fake SettingsAttributeSictionary
                DeviceId = String.Format("{0}.{1}", DeviceName, DeviceType);
                var deviceAttribute = new ASCOM.DeviceIdAttribute(DeviceId);
                var FakeAttributeDictionary = new SettingsAttributeDictionary();
                FakeAttributeDictionary.Add(deviceAttribute.GetType(), deviceAttribute);
            };
    }

    [Subject(typeof(SettingsProvider), "Construction")]
    public class When_constructed : With_mock_ascom_profile
    {
        Because of = () => { /* default contructor */ };
        It should_have_empty_app_name = () => Target.ApplicationName.ShouldBeEmpty();
        It shoul_have_a_non_empty_description = () => Target.Description.ShouldNotBeEmpty();
        It should_have_non_empty_name = () => Target.Name.ShouldNotBeEmpty();
    }

    [Subject(typeof(SettingsProvider))]
    public class When_initialized_with_null_name : With_mock_ascom_profile
    {
        Because of = () => Target.Initialize(null, null);

        It should_return_assembly_name_for_app_name =
            () => Target.ApplicationName.ShouldEqual(Assembly.GetExecutingAssembly().GetName().Name);

        It should_have_a_non_empty_description = () => Target.Description.ShouldNotBeEmpty();
        It should_have_non_empty_name = () => Target.Name.ShouldNotBeEmpty();
    }

    [Subject(typeof(SettingsProvider))]
    public class When_initialised_with_name : With_mock_ascom_profile
    {
        Because of = () => Target.Initialize(csUnitTest, null);
        It should_return_name_for_app_name = () => Target.ApplicationName.ShouldEqual(csUnitTest);
        It shoul_have_a_non_empty_description = () => Target.Description.ShouldNotBeEmpty();
        It should_have_non_empty_name = () => Target.Name.ShouldNotBeEmpty();
    }

    // Tests that a property retrieved from a device that hasn't been registered returns the property's default value.
    [Subject(typeof(SettingsProvider), "Property defaults")]
    public class When_getting_properties_for_an_unregistered_device : With_mock_ascom_profile
    {
        static SettingsPropertyValueCollection result;
        static SettingsPropertyCollection Properties;
        // Context:
        //	- mockProfile should capture whatever the DeviceType is configured to.
        Establish context = () =>
            {
                var settingsProperty = new SettingsProperty(SettingName, typeof(string), null, false,
                                                            SettingDefaultValue, SettingsSerializeAs.String, FakeAttributeDictionary,
                                                            true, true);
                Properties = new SettingsPropertyCollection();
                Properties.Add(settingsProperty);
                SetDeviceType = String.Empty;   // Captures the device type that has been set.
                MockProfile.SetupSet(x => x.DeviceType).Callback(y => SetDeviceType = y);
                Target = new SettingsProvider(MockProfile.Object);
                SettingsContext = new SettingsContext();
            };

        Because of = () => result = Target.GetPropertyValues(SettingsContext, Properties);

        // Assertions:
        //	- MockProfile's device type (captured in SetDeviceType) must match deviceType.
        //	- The returned SettingsPropertyValueCollection must not be empty.
        //	- The returned SettingsPropertyValueCollection must have exactly one entry.
        //	- The entry must match the value of SettingDefaultValue. 

        //It should_have_the_correct_device_type = () => SetDeviceType.ShouldEqual(DeviceType);
        It should_return_a_non_empty_collection = () => result.ShouldNotBeEmpty();
        It should_have_exactly_one_property_item = () => result.Count.ShouldEqual(1);

        It should_have_propert_value_equal_setting_default_value = () =>
            {
                var settingsPropertyValue = result.OfType<SettingsPropertyValue>().First();
                settingsPropertyValue.IsDirty.ShouldBeFalse();
                settingsPropertyValue.PropertyValue.ShouldEqual(SettingDefaultValue);
            };
    }

    // Tests that a property correctly returns the value it was set to (for a registered device).
    [Subject(typeof(SettingsProvider), "Property defaults")]
    public class When_setting_then_getting_properties_for_a_registered_device : With_mock_ascom_profile
    {
        static SettingsPropertyValueCollection result;
        //static SettingsPropertyCollection properties; Commented out to remove compiler warning
        // Context:
        //	- mockProfile should capture whatever the DeviceType is configured to.
        Establish context = () =>
            {
                var settingsProperty = new SettingsProperty(SettingName, typeof(string), null, false,
                                                            SettingDefaultValue, SettingsSerializeAs.String,
                                                            FakeAttributeDictionary,
                                                            true, true);
                SettingsProperties = new SettingsPropertyCollection();
                SettingsProperties.Add(settingsProperty);

                SetDeviceType = String.Empty; // Captures the device type that has been set.
                MockProfile.SetupSet(x => x.DeviceType).Callback(y => SetDeviceType = y);
                MockProfile.Setup(x => x.WriteValue(DeviceId, SettingName, SettingSetValue, String.Empty));

                Target = new SettingsProvider(MockProfile.Object);
            };

        Because of = () => result = Target.GetPropertyValues(SettingsContext, SettingsProperties);

        // Assertions:
        //	- mockProfile must have it's DeviceType set.
        //	- mockProfile's device type (captured in SetDeviceType) must match deviceType.
        //	- The returned SettingsPropertyValueCollection must not be empty.
        //	- IProfile.WriteValue should be called with the correct values(deviceId, item.Name, item.SerializedValue.ToString(), String.Empty);

        //It should_have_the_correct_device_type = () => SetDeviceType.ShouldEqual(DeviceType);
        It should_return_a_non_empty_collection = () => result.ShouldNotBeEmpty();
        It should_have_exactly_one_property_item = () => result.Count.ShouldEqual(1);
        It should_call_writevalue_correctly = () => MockProfile.Verify();   // ToDo: Verify the actual call values.
    }

    // The tests below this point are still to be converted.

    ///// <summary>
    ///// Verifies that, when setting property values,
    ///// properties not decorated with an <see cref="ASCOM.DeviceIdAttribute"/>
    ///// are silently ignored (per MS documentation) and that no attempt is made to write
    ///// anything to the Profile store.
    ///// </summary>
    //[Fact]
    //public void Non_Attributed_Settings_Should_Be_Silently_Ignored_When_Setting()
    //{
    //    // ARRANGE
    //    var deviceId = String.Format("{0}.{1}", DeviceName, DeviceType);
    //    var deviceAttribute = new ASCOM.ServedClassNameAttribute("Dummy");	// Not a Device ID attribute
    //    var attributes = new SettingsAttributeDictionary();
    //    attributes.Add(deviceAttribute.GetType(), deviceAttribute);
    //    var settingsProperty = new SettingsProperty(SettingName, typeof(string), null, false, SettingDefaultValue, SettingsSerializeAs.String, attributes, true, true);
    //    var settingsPropertyValue = new SettingsPropertyValue(settingsProperty);
    //    settingsPropertyValue.PropertyValue = SettingSetValue;
    //    var settingsPropertyValues = new SettingsPropertyValueCollection();
    //    settingsPropertyValues.Add(settingsPropertyValue);
    //    var context = new SettingsContext();

    //    // Expectations:
    //    //	- mockProfile must NOT have it's DeviceType set.
    //    //	- IProfile.WriteValue should NOT be called.
    //    var setDeviceType = String.Empty;
    //    Mock<IProfile> mockProfile = new Mock<IProfile>();
    //    mockProfile.SetupSet(x => x.DeviceType).Callback(y => setDeviceType = y);
    //    //mockProfile.Setup(x => x.WriteValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
    //    var settingsProvider = new SettingsProvider(mockProfile.Object);

    //    // ACT
    //    settingsProvider.SetPropertyValues(context, settingsPropertyValues);

    //    // ASSERT
    //    Assert.Equal(String.Empty, setDeviceType);
    //    mockProfile.Verify(x => x.WriteValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
    //}

    ///// <summary>
    ///// Verifies that, when getting property values,
    ///// properties not decorated with an <see cref="ASCOM.DeviceIdAttribute"/>
    ///// are silently ignored (per MS documentation) and that no attempt is made to write
    ///// anything to the Profile store.
    ///// </summary>
    //[Fact]
    //public void Non_Attributed_Settings_Should_Be_Silently_Ignored_When_Getting()
    //{
    //    // ARRANGE
    //    var deviceId = String.Format("{0}.{1}", DeviceName, DeviceType);
    //    var deviceAttribute = new ASCOM.ServedClassNameAttribute("Dummy");	// Not a Device ID attribute
    //    var attributes = new SettingsAttributeDictionary();
    //    attributes.Add(deviceAttribute.GetType(), deviceAttribute);
    //    var settingsProperty = new SettingsProperty(SettingName, typeof(string), null, false, SettingDefaultValue, SettingsSerializeAs.String, attributes, true, true);
    //    var settingsProperties = new SettingsPropertyCollection();
    //    settingsProperties.Add(settingsProperty);
    //    var context = new SettingsContext();

    //    // Expectations:
    //    //	- mockProfile must NOT have it's DeviceType set.
    //    //	- IProfile.GetValue() must NOT be called.

    //    var setDeviceType = String.Empty;
    //    Mock<IProfile> mockProfile = new Mock<IProfile>();
    //    mockProfile.SetupSet(x => x.DeviceType).Callback(y => setDeviceType = y);
    //    //mockProfile.Setup(x => x.WriteValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
    //    var settingsProvider = new SettingsProvider(mockProfile.Object);

    //    // ACT
    //    settingsProvider.GetPropertyValues(context, settingsProperties);

    //    // ASSERT
    //    Assert.Equal(String.Empty, setDeviceType);
    //    mockProfile.Verify(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
    //}
}