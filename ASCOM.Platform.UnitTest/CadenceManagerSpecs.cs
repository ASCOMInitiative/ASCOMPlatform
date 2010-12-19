using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Controls;
using Machine.Specifications;

namespace ASCOM.Platform.UnitTest
{
    [Subject(typeof (CadenceManager), "Duplicate detection")]
    public class when_the_same_control_is_added_twice : with_clean_cadencemanager
    {

        private Because of = () =>
            {
                CadenceManager.Instance.Add(testControl);
                CadenceManager.Instance.Add(testControl);
            };

        private It should_only_add_one_instance = () => CadenceManager.UpdateList.Count.ShouldEqual(1);
    }


    public class with_clean_cadencemanager
    {
        protected static ICadencedControl testControl;
        protected static CadenceManager manager;

        private Establish context = () =>
            {
                testControl = new Annunciator();
                manager = CadenceManager.Instance;
                CadenceManager.UpdateList.Clear();
            };
    }
}
