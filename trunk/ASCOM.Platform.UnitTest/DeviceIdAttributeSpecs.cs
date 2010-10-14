using Machine.Specifications;

namespace ASCOM.Platform.Test
{
    /// <summary>
    ///   Context base class for three part DeviceIDs "ASCOM.DeviceName.DeviceType".
    /// </summary>
    public class With_with_device_id_attribute
    {
        protected const string cExpectedDeviceId = "ASCOM.UnitTest.TestDevice";
        protected const string cExpectedDeviceName = "UnitTest Device";
        protected static DeviceIdAttribute target;

        Establish context = () =>
        {
            target = new DeviceIdAttribute(cExpectedDeviceId);
        };
    }


    [Subject(typeof(DeviceIdAttribute), "constructor, positional parameter")]
    public class When_creating_with_single_positional_parameter : With_with_device_id_attribute
    {
        // There is no Act because we're testing the default constructor.

        It should_return_device_id_that_it_was_initialised_with =
            () => target.DeviceId.ShouldEqual(cExpectedDeviceId);

        It should_return_device_name_same_as_device_id = () => target.DeviceName.ShouldEqual(cExpectedDeviceId);
    }

    [Subject(typeof(DeviceIdAttribute), "constructor, named parameter")]
    public class When_creating_with_positional_and_named_parameters : With_with_device_id_attribute
    {
        Because of = () =>
                     target = new DeviceIdAttribute(cExpectedDeviceId) { DeviceName = cExpectedDeviceName };
        It should_have_expected_device_id = () => target.DeviceId.ShouldEqual(cExpectedDeviceId);
        It should_have_expected_device_name = () => target.DeviceName.ShouldEqual(cExpectedDeviceName);
    }
}