using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Utilities;
using Machine.Specifications;

namespace ASCOM.Platform.UnitTest
{
    public class with_new_key_value_pair
    {
        protected static KeyValuePair target;
        Establish context = () => { target = new KeyValuePair(); };
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    [Subject(typeof(KeyValuePair), "Construction")]
    public class when_constructing_a_new_key_value_pair : with_new_key_value_pair
    {
        It should_have_empty_key = () => target.Key.ShouldBeEmpty();
        It should_have_empty_value = () => target.Value.ShouldBeEmpty();
    }

    /// <summary>
    /// Fully specified constructor
    /// </summary>
    [Subject(typeof(KeyValuePair), "Construction")]
    public class when_constructing_a_new_key_value_pair_with_values : with_new_key_value_pair
    {
        Because of = () => target = new KeyValuePair("Key", "Value");
        It should_have_have_expected_key = () => target.Key.ShouldEqual("Key");
        It should_have_empty_value = () => target.Value.ShouldEqual("Value");
    }

    /// <summary>
    /// Initializer syntax - equivalent to default contructor followed by setting each property.
    /// </summary>
    [Subject(typeof(KeyValuePair), "Construction")]
    public class when_constructing_a_new_key_value_pair_with_initializer_syntax : with_new_key_value_pair
    {
        Because of = () => target = new KeyValuePair {Key = "Key", Value = "Value"};
        It should_have_have_expected_key = () => target.Key.ShouldEqual("Key");
        It should_have_empty_value = () => target.Value.ShouldEqual("Value");
    }

}
