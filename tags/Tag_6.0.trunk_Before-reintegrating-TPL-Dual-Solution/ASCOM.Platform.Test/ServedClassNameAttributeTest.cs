namespace ASCOM.Platform.Test
{
	using ASCOM;
	using Xunit;

    /// <summary>
    ///This is a test class for ServedClassNameAttributeTest and is intended
    ///to contain all ServedClassNameAttributeTest Unit Tests
    ///</summary>
	public class ServedClassNameAttributeTest
	{
		/// <summary>
		/// Constructor test
		///</summary>
		[Fact]
		public void DisplayName_Should_Equal_Value_Passed_In_Constructor()
		{
			string expected = "Unit Test";
			string actual;

			var attr = new ServedClassNameAttribute("Unit Test");
			actual = attr.DisplayName;

			Assert.Equal(expected, actual);
		}
	}
}
