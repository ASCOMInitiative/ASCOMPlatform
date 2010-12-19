using System;
using ASCOM.Controls;
using Machine.Specifications;

namespace ASCOM.Platform.UnitTest
{
    [Subject(typeof (LedIndicator), "Control Disposed")]
    public class when_led_control_is_disposed : with_clean_cadencemanager
    {
        private Because of = () =>
            {
                var control = new LedIndicator();
                control.Enabled = true; // This ensures the control is active and registered with CadenceManager
                control.Dispose();
            };

        private It should_unregister_from_cadence_manager = () => CadenceManager.UpdateList.Count.ShouldEqual(0);
    }

    [Subject(typeof (LedIndicator), "Construction")]
    public class when_led_control_is_created : with_clean_cadencemanager
    {
        private Because of = () =>
            {
                var control = new LedIndicator();
                control.Enabled = true; // This ensures the control is active and registered with CadenceManager
            };

        private It should_register_with_cadence_manager = () => CadenceManager.UpdateList.Count.ShouldEqual(1);
    }

    [Subject(typeof(LedIndicator), "Disposal")]
    public class when_led_control_is_double_disposed
    {
        protected static LedIndicator led;
        protected static Exception ex;

        private Establish context = () => { led = new LedIndicator(); };

        private Because of = () => ex = Catch.Exception(() =>
        {
            led.Dispose();
            led.Dispose();
        }
                                            );
        private It should_not_throw = () => ex.ShouldBeNull();
    }

    [Subject(typeof(LedIndicator), "Disposal")]
    public class when_led_control_is_updated_after_disposal : with_clean_cadencemanager
    {
        protected static LedIndicator led;
        protected static Exception ex;

        private Because of = () => ex = Catch.Exception(() =>
        {
            led = new LedIndicator();
            led.Dispose();
            led.CadenceUpdate(true);
            led.CadenceUpdate(false);
        }
                                            );

        private It should_throw = () => ex.ShouldBeOfType<ObjectDisposedException>();
    }
}