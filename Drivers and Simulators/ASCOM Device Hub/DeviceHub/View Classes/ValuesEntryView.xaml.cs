using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace ASCOM.DeviceHub
{
	/// <summary>
	/// Interaction logic for ValuesEntryWindow.xaml
	/// </summary>
	public partial class ValuesEntryView
    {
        public ValuesEntryView()
        {
            InitializeComponent();
        }

		private void Window_Loaded( object sender, RoutedEventArgs e )
		{
			this.DisableButtons();
		}

		private void _cancel_Click( object sender, RoutedEventArgs e )
		{
			DialogResult = false;
			Close();
		}

		private void _ok_Click( object sender, RoutedEventArgs e )
		{
			DialogResult = true;
			Close();
		}
	}
}
