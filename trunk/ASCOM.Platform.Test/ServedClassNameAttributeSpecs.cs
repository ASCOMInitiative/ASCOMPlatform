using Machine.Specifications;

namespace ASCOM.Platform.Test
{
    using ASCOM;
    using Xunit;

    /// <summary>
    ///This is a test class for ServedClassNameAttributeTest and is intended
    ///to contain all ServedClassNameAttributeTest Unit Tests
    ///</summary>
    public class When_constructing_a_servedclassnameattribute
    {
        const string cExpectedName = "Unit Test";
        protected static ServedClassNameAttribute target;
        Establish context = () => { target = new ServedClassNameAttribute(cExpectedName); };
        Because of = () => { };
        It should_have_the_name_passed_in_the_constructor = () => target.DisplayName.ShouldEqual(cExpectedName);
    }
}
