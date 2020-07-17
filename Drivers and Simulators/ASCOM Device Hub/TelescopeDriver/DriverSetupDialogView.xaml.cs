using System.Windows;
using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	/// <summary>
	/// Interaction logic for DriverSetupDialogView.xaml
	/// </summary>
	public partial class DriverSetupDialogView : DeviceHubDialogView
	{
		public bool IsLoggingEnabled { get; set; }

		public DriverSetupDialogView()
		{
			InitializeComponent();
		}

		private void Image_MouseLeftButtonUp( object sender, MouseButtonEventArgs e )
		{
			try
			{
				System.Diagnostics.Process.Start( "http://ascom-standards.org/" );
			}
			catch ( System.ComponentModel.Win32Exception noBrowser )
			{
				if ( noBrowser.ErrorCode == -2147467259 )
					MessageBox.Show( noBrowser.Message );
			}
			catch ( System.Exception other )
			{
				MessageBox.Show( other.Message );
			}
		}

		//public void OnDialogClose( bool? result )
		//{
		//	DialogResult = result;
		//	Close();
		//}

		//private void _cancelButton_Click( object sender, RoutedEventArgs e )
		//{
		//	DialogResult = false;
		//}

		//private void _okButton_Click( object sender, RoutedEventArgs e )
		//{
		//	DialogResult = true;
		//}
	}
}
