namespace ASCOM.Platform.Test
{
	using ASCOM;
	using Xunit;

	/// <summary>
	///This is a test class for DeviceIdAttributeTest and is intended
	///to contain all DeviceIdAttributeTest Unit Tests
	///</summary>
	public class DeviceIdAttributeTest
	{
		/// <summary>
		/// Constructor test.
		///</summary>
		[Fact]
		public void DeviceId_Should_Equal_Value_Passed_In_Constructor()
		{
			string expected = "Unit Test";
			string actual;

			var attr = new DeviceIdAttribute("Unit Test");
			actual = attr.DeviceId;

			Assert.Equal(expected, actual);
		}
	}
}
