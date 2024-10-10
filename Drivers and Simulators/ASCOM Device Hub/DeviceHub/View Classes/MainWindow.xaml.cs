using ASCOM.DeviceHub.MvvmMessenger;

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Topmost = Globals.AlwaysOnTop;

            // Call the extension method to allow us to minimize the app to
            // the system tray, rather than the task bar.

            this.EnableMinimizeToTray();

            _expanders = new List<Expander>
            {
                _domeControl,
                _focuserControl
            };

            if (Globals.UseExpandedScreenLayout)
            {
                // We are in expanded view mode, check which expanders to expand.

                if (Globals.IsDomeExpanded)
                {
                    _domeControl.IsExpanded = true;
                }

                if (Globals.IsFocuserExpanded)
                {
                    _focuserControl.IsExpanded = true;
                }
            }

            // Now that we have restored the IsExpanded states, we can hook up the event handlers

            _domeControl.Expanded += new RoutedEventHandler(_domeControl_Expanded);
            _domeControl.Collapsed += new RoutedEventHandler(_domeControl_Collapsed);
            _focuserControl.Expanded += new RoutedEventHandler(_focuserControl_Expanded);
            _focuserControl.Collapsed += new RoutedEventHandler(_focuserControl_Collapsed);

            Messenger.Default.Register<SignalWaitMessage>(this, (action) => ShowHideWaitCursor(action));
        }

        private void ShowHideWaitCursor(SignalWaitMessage action)
        {
            // Used by ViewModels to cause a wait cursor to be displayed when Connecting devices,
            // in case connect takes time to complete.

            Cursor cursor = (action.Wait) ? Cursors.Wait : null;

            Mouse.OverrideCursor = cursor;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Messenger.Default.Unregister<SignalWaitMessage>(this);

            base.OnClosing(e);
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            if (sender is Window main)
            {
                // When the window is minimized, the window position is set to (-32000,-32000). 
                // We don't want to remember this!
                Globals.MainWindowLeft = main.Left > 0 ? main.Left : Globals.MainWindowLeft;
                Globals.MainWindowTop = main.Top > 0 ? main.Top : Globals.MainWindowTop;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create the activity log dialogue so it runs hidden from the start of the application
            ActivityLogViewModel ActivityLogVm = new ActivityLogViewModel();
            IActivityLogService svc = ServiceContainer.Instance.GetService<IActivityLogService>();
            svc.CreateActivityLog(ActivityLogVm, Globals.ActivityWindowLeft, Globals.ActivityWindowTop, Globals.ActivityWindowWidth, Globals.ActivityWindowHeight);

            // By setting the window state after the main window has loaded, it will
            // trigger the StateChanged event which will put the icon in the tray rather
            // than on the task bar.

            if (Server.StartedByCOM)
            {
                WindowState = WindowState.Minimized;
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is TabControl ctrl)
            {
                if (ctrl.SelectedItem is TabItem item)
                {
                    string label = item.Header.ToString().ToLower();

                    DeviceTypeEnum device = DeviceTypeEnum.Telescope;

                    if (label == "dome")
                    {
                        device = DeviceTypeEnum.Dome;
                    }
                    else if (label == "focuser")
                    {
                        device = DeviceTypeEnum.Focuser;
                    }

                    Globals.ActiveDevice = device;
                }
            }
        }

        private void _domeControl_Expanded(object sender, RoutedEventArgs e)
        {
            Globals.IsDomeExpanded = true;

            AdjustOtherExpanders(sender);
        }

        private void _focuserControl_Expanded(object sender, RoutedEventArgs e)
        {
            Globals.IsFocuserExpanded = true;

            AdjustOtherExpanders(sender);
        }

        private void AdjustOtherExpanders(object sender)
        {
            Expander sendingExpander = sender as Expander;

            if (sendingExpander == null)
            {
                return;
            }

            bool collapseOthers = true;

            if (collapseOthers && Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                collapseOthers = false;
            }

            if (collapseOthers)
            {
                foreach (Expander expander in _expanders)
                {
                    if (expander != sendingExpander)
                    {
                        expander.IsExpanded = false;
                    }
                }
            }
        }

        private void _domeControl_Collapsed(object sender, RoutedEventArgs e)
        {
            Globals.IsDomeExpanded = false;
        }

        private void _focuserControl_Collapsed(object sender, RoutedEventArgs e)
        {
            Globals.IsFocuserExpanded = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Check whether the key being pressed is the F12 key
            if (e.Key == Key.F12) // The F12 key is pressed
            {
                // Toggle the TopMost state so the main windows either will or will not stay on top.
                Topmost = !Topmost;

                // Flag the event as handled, so no further action is required by the application
                e.Handled = true;
            }
        }
    }
}
