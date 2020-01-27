using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ASCOM.DeviceHub
{
    /// <summary>
    /// Interaction logic for SetupView.xaml
    /// </summary>
    public partial class SetupView : DeviceHubDialogView
    {
        public SetupView()
        {
            InitializeComponent();
		}

		public void OnDialogClose( object sender, DialogCloseEventArgs args )
		{
			DialogResult = args.DialogResult;
			Close();
		}

		private void DeviceHubDialogView_Loaded( object sender, RoutedEventArgs e )
		{
			this.DisableButtons();

			SelectActiveTab();
		}

		private void TabControl_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			if ( e.Source is TabControl )
			{
				TabControl ctrl = sender as TabControl;
				int index = ctrl.SelectedIndex;
				TabItem item = ctrl.Items[index] as TabItem;
				string header = item.Header.ToString();

				SetupViewModel vm = GetViewModel();
				vm.ChangeActiveFunction( header );
			}
		}

		private void SelectActiveTab()
		{
			DeviceTypeEnum device = Globals.ActiveDevice;

			string devName = device.ToString();
			bool canBeActive = true;

			switch ( devName )
			{
				case "Telescope":
					if ( TelescopeManager.Instance.IsConnected )
					{
						canBeActive = false;
					}

					break;

				case "Dome":
					if ( DomeManager.Instance.IsConnected )
					{
						canBeActive = false;
					}

					break;

				case "Focuser":
					if ( FocuserManager.Instance.IsConnected )
					{
						canBeActive = false;
					}

					break;
			}

			// This only works if all of the tab items have a defined Tag property.

			if ( canBeActive )
			{
				TabItem item = _deviceTab.Items.OfType<TabItem>().SingleOrDefault( i => i.Tag.ToString() == devName );

				_deviceTab.SelectedItem = item;
			}
		}

		private SetupViewModel GetViewModel()
		{
			return this.DataContext as SetupViewModel;
		}
	}
}
