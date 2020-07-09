using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private List<Expander> _expanders;

		public MainWindow()
		{
			InitializeComponent();

			Left = Globals.MainWindowLeft;
			Top = Globals.MainWindowTop;

			// Call the extension method to allow us to minimize the app to
			// the system tray, rather than the task bar.

			this.EnableMinimizeToTray();

			_expanders = new List<Expander>
			{
				_domeControl,
				_focuserControl
			};
		}

		private void Window_LocationChanged( object sender, EventArgs e )
		{
			if ( sender is Window main )
			{
				// When the window is minimized, the window position is set to (-32000,-32000). 
				// We don't want to remember this!
				Globals.MainWindowLeft = main.Left > 0 ? main.Left : Globals.MainWindowLeft;
				Globals.MainWindowTop = main.Top > 0 ? main.Top : Globals.MainWindowTop;
			}
		}

		private void Window_Loaded( object sender, RoutedEventArgs e )
		{
			// By setting the window state after the main window has loaded, it will
			// trigger the StateChanged event which will put the icon in the tray rather
			// than on the task bar.

			if ( Server.StartedByCOM )
			{
				WindowState = WindowState.Minimized;
			}
		}

		private void ExitMenuItem_Click( object sender, RoutedEventArgs e )
		{
			this.Close();
		}

		private void TabControl_SelectionChanged( object sender, System.Windows.Controls.SelectionChangedEventArgs e )
		{
			if ( sender is TabControl ctrl )
			{
				if ( ctrl.SelectedItem is TabItem item )
				{
					string label = item.Header.ToString().ToLower();

					DeviceTypeEnum device = DeviceTypeEnum.Telescope;

					if ( label == "dome" )
					{
						device = DeviceTypeEnum.Dome;
					}
					else if ( label == "focuser")
					{
						device = DeviceTypeEnum.Focuser;
					}

					Globals.ActiveDevice = device;
				}
			}
		}

		private void Expander_Expanded( object sender, RoutedEventArgs e )
		{
			bool collapseOthers = true;

			if ( Keyboard.IsKeyDown( Key.LeftCtrl ) || Keyboard.IsKeyDown( Key.RightCtrl ) )
			{
				collapseOthers = false;
			}

			Expander sendingExpander = sender as Expander;

			if ( !collapseOthers || sendingExpander == null || _expanders == null )
			{
				return;
			}

			foreach ( Expander expander in _expanders )
			{
				if ( expander != sendingExpander && expander.IsExpanded )
				{
					expander.IsExpanded = false;
				}
			}
		}

	}
}
