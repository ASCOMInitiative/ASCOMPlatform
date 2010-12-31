// --------------------------------------------------------------------------------
// <summary
// MSpec Specifications for ASCOM.Controls.LedIndicator control
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

using System;
using ASCOM.Controls;
using Machine.Specifications;

namespace ASCOM.Platform.UnitTest
{
    [Subject(typeof (LedIndicator), "Control Disposed")]
    public class when_led_control_is_disposed : with_clean_cadencemanager
    {
        Because of = () =>
            {
                LedIndicator control = new LedIndicator();
                control.Enabled = true; // This ensures the control is active and registered with CadenceManager
                control.Dispose();
            };

        It should_unregister_from_cadence_manager = () => CadenceManager.UpdateList.Count.ShouldEqual(0);
    }

    [Subject(typeof (LedIndicator), "Construction")]
    public class when_led_control_is_created : with_clean_cadencemanager
    {
        Because of = () =>
            {
                LedIndicator control = new LedIndicator();
                control.Enabled = true; // This ensures the control is active and registered with CadenceManager
            };

        It should_register_with_cadence_manager = () => CadenceManager.UpdateList.Count.ShouldEqual(1);
    }

    [Subject(typeof (LedIndicator), "Disposal")]
    public class when_led_control_is_double_disposed
    {
        protected static LedIndicator led;
        protected static Exception ex;

        Establish context = () => { led = new LedIndicator(); };

        Because of = () => ex = Catch.Exception(() =>
            {
                led.Dispose();
                led.Dispose();
            }
                                    );

        It should_not_throw = () => ex.ShouldBeNull();
    }

    [Subject(typeof (LedIndicator), "Disposal")]
    public class when_led_control_is_updated_after_disposal : with_clean_cadencemanager
    {
        protected static LedIndicator led;
        protected static Exception ex;

        Because of = () => ex = Catch.Exception(() =>
            {
                led = new LedIndicator();
                led.Dispose();
                led.CadenceUpdate(true);
                led.CadenceUpdate(false);
            }
                                    );

        It should_throw = () => ex.ShouldBeOfType<ObjectDisposedException>();
    }
}