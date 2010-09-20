using ASCOM.Internal;
using Xunit;
using System.IO;

namespace ASCOM.Platform.Test
{
    
    
    /// <summary>
    ///This is a test class for FileInfoExtensionsTest and is intended
    ///to contain all FileInfoExtensionsTest Unit Tests
    ///</summary>
	public class FileInfoExtensionsTest
	{

		/// <summary>
		///A test for IsDeviceSpecific
		///</summary>
		public void IsDeviceSpecificTest()
		{
			FileInfo file = null; // TODO: Initialize to an appropriate value
			bool expected = false; // TODO: Initialize to an appropriate value
			bool actual;
			actual = FileInfoExtensions.IsDeviceSpecific(file);
			Assert.Equal(expected, actual);
		}
	}
}
