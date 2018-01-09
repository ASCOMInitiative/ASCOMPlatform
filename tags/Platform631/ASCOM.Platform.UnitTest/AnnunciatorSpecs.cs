using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Controls;
using Machine.Specifications;

namespace ASCOM.Platform.UnitTest
{
    [Subject(typeof(Annunciator), "Control Disposed")]
    public class When_annunciator_control_is_disposed : with_clean_cadencemanager
    {
        private Establish context = () => { };

        private Because of = () =>
        {
            var control = new Annunciator();
            control.Enabled = true; // This ensures the control is active and registered with CadenceManager
            control.Dispose();
        };

        private It should_unregister_from_cadence_manager = () => CadenceManager.UpdateList.Count.ShouldEqual(0);
    }

    [Subject(typeof(Annunciator), "Construction")]
    public class when_annunciator_control_is_created : with_clean_cadencemanager
    {
        private Because of = () =>
        {
            var control = new Annunciator();
            control.Enabled = true; // This ensures the control is active and registered with CadenceManager
        };

        private It should_register_with_cadence_manager = () => CadenceManager.UpdateList.Count.ShouldEqual(1);
    }

    [Subject(typeof(Annunciator), "Disposal")]
    public class when_annunciator_control_is_double_disposed
    {
        protected static Annunciator annunciator;
        protected static Exception ex;

        private Establish context = () => { annunciator = new Annunciator(); };

        private Because of = () => ex = Catch.Exception(() =>
        {
            annunciator.Dispose();
            annunciator.Dispose();
        }
                                            );
        private It should_not_throw = () => ex.ShouldBeNull();
    }

    [Subject(typeof(Annunciator), "Disposal")]
    public class when_annunciator_control_is_updated_after_disposal : with_clean_cadencemanager
    {
        protected static Annunciator annunciator;
        protected static Exception ex;

        private Because of = () => ex = Catch.Exception(() =>
        {
            annunciator = new Annunciator();
            annunciator.Dispose();
            annunciator.CadenceUpdate(true);
            annunciator.CadenceUpdate(false);
        }
                                            );

        private It should_throw = () => ex.ShouldBeOfType<ObjectDisposedException>();
    }

}
