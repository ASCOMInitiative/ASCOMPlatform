using System.Windows;

namespace ASCOM.DeviceHub
{
	public class MessageBoxService : IMessageBoxService
	{
		private Window Parent { get; set; }

		public MessageBoxService( Window parent )
		{
			Parent = parent;
		}

		public MessageBoxResult Show( string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, 
										MessageBoxResult defaultResult, MessageBoxOptions options )
		{
			MessageBoxResult result = MessageBox.Show( Parent, messageBoxText, caption, button, icon, defaultResult, options );

			return result;
		}
	}
}
