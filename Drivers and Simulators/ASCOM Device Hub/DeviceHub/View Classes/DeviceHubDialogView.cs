using System.Windows;

namespace ASCOM.DeviceHub
{
	public class DeviceHubDialogView : Window
	{
		public void OnRequestClose( object sender, DialogCloseEventArgs e )
		{
			DialogResult = e.DialogResult;
			Close();
		}
	}
}
