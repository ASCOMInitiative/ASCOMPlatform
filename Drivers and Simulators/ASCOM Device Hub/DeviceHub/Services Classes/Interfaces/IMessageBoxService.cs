using System.Windows;

namespace ASCOM.DeviceHub
{
	public interface IMessageBoxService
	{
		MessageBoxResult Show( string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, 
								MessageBoxResult defaultResult, MessageBoxOptions options );
	}
}
