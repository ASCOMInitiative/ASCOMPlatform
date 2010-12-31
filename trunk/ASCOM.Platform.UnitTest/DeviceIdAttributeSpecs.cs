// --------------------------------------------------------------------------------
// <summary
// MSpec Specifications for ASCOM.Attributes.DeviceIdAttribute
// </summary>
// 
// <copyright company="TiGra Astronomy">
// 	Copyright © 2009-2010 Timothy P. Long, .
// 	http://www.tigranetworks.co.uk/Astronomy
// </copyright>
// 
// <license>
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </license>
// 
// Original Author:		[TPL] Timothy P. Long <Tim@tigranetworks.co.uk>
using Machine.Specifications;

namespace ASCOM.Platform.UnitTest
{
    /// <summary>
    ///   Context base class for three part DeviceIDs "ASCOM.DeviceName.DeviceType".
    /// </summary>
    public class With_with_device_id_attribute
    {
        protected const string cExpectedDeviceId = "ASCOM.UnitTest.TestDevice";
        protected const string cExpectedDeviceName = "UnitTest Device";
        protected static DeviceIdAttribute target;

        Establish context = () => { target = new DeviceIdAttribute(cExpectedDeviceId); };
    }


    [Subject(typeof (DeviceIdAttribute), "constructor, positional parameter")]
    public class When_creating_with_single_positional_parameter : With_with_device_id_attribute
    {
        // There is no Act because we're testing the default constructor.

        It should_return_device_id_that_it_was_initialised_with =
            () => target.DeviceId.ShouldEqual(cExpectedDeviceId);

        It should_return_device_name_same_as_device_id = () => target.DeviceName.ShouldEqual(cExpectedDeviceId);
    }

    [Subject(typeof (DeviceIdAttribute), "constructor, named parameter")]
    public class When_creating_with_positional_and_named_parameters : With_with_device_id_attribute
    {
        Because of = () =>
                     target = new DeviceIdAttribute(cExpectedDeviceId) {DeviceName = cExpectedDeviceName};

        It should_have_expected_device_id = () => target.DeviceId.ShouldEqual(cExpectedDeviceId);
        It should_have_expected_device_name = () => target.DeviceName.ShouldEqual(cExpectedDeviceName);
    }
}