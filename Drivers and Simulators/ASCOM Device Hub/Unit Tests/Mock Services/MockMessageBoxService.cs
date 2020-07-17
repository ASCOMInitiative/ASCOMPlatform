using System.Windows;

using ASCOM.DeviceHub;

namespace Unit_Tests
{
	public class MockMessageBoxService : IMessageBoxService
	{
		public MessageBoxResult Show( string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options )
		{
			return MessageBoxResult.OK;
		}
	}
}
