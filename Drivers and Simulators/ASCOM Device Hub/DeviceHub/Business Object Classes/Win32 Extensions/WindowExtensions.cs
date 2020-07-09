using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace ASCOM.DeviceHub
{
    public static class WindowExtensions
    {
        private const int SW_SHOWNORMAL = 1;

        internal static void DisableButtons( this Window window )
        {
            const int GWL_STYLE = -16;
            const int WS_SYSMENU = 0x80000;
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper( window ).Handle;
            int value = User32.GetWindowLong( hwnd, GWL_STYLE );
            User32.SetWindowLong( hwnd, GWL_STYLE, value & ~WS_SYSMENU );
        }

        internal static void AdjustWindowPlacement( this Window window )
        {
            // Don't adjust the window size, just it's location.
			// Using SetWindowPlacement will ensure that the window
			// will not be displayed off the screen!

            try
            {
                WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
                wp.length = Marshal.SizeOf( typeof( WINDOWPLACEMENT ) );
                wp.flags = 0;
                wp.showCmd = SW_SHOWNORMAL;
                wp.normalPosition.Left = (int)window.Left;
                wp.normalPosition.Top = (int)window.Top;
                wp.normalPosition.Bottom = (int)( window.Top + window.Height );
                wp.normalPosition.Right = (int)( window.Left + window.Width );
                IntPtr hwnd = new WindowInteropHelper( window ).Handle;
                User32.SetWindowPlacement( hwnd, ref wp );
            }
            catch { }
        }

		internal static void EnableMinimizeToTray( this Window window )
		{
			// No need to track this instance; its event handlers will keep it alive
			new MinimizeToTrayInstance( window );
		}

		/// <summary>
		/// Class implementing "minimize to tray" functionality for a Window instance.
		/// </summary>
		private class MinimizeToTrayInstance
		{
			private Window _window;
			private NotifyIcon _notifyIcon;
			private bool _balloonShown;

			/// <summary>
			/// Initializes a new instance of the MinimizeToTrayInstance class.
			/// </summary>
			/// <param name="window">Window instance to attach to.</param>
			public MinimizeToTrayInstance( Window window )
			{
				Debug.Assert( window != null, "window parameter is null." );
				_window = window;
				_window.StateChanged += HandleStateChanged;
				_window.Closed += HandleWindowClosed;
			}

			private void HandleWindowClosed( object sender, EventArgs e )
			{
				_window.StateChanged -= HandleStateChanged;

				if ( _notifyIcon != null )
				{
					_notifyIcon.MouseClick -= HandleNotifyIconOrBalloonClicked;
					_notifyIcon.BalloonTipClicked -= HandleNotifyIconOrBalloonClicked;
					_notifyIcon.Dispose();
					_notifyIcon = null;
				}
			}

			/// <summary>
			/// Handles the Window's StateChanged event.
			/// </summary>
			/// <param name="sender">Event source.</param>
			/// <param name="e">Event arguments.</param>
			private void HandleStateChanged( object sender, EventArgs e )
			{
				if ( _notifyIcon == null )
				{
					// Initialize NotifyIcon instance "on demand"
					_notifyIcon = new NotifyIcon();
					_notifyIcon.Icon = Icon.ExtractAssociatedIcon( Assembly.GetEntryAssembly().Location );
					_notifyIcon.MouseClick += new MouseEventHandler( HandleNotifyIconOrBalloonClicked );
					_notifyIcon.BalloonTipClicked += new EventHandler( HandleNotifyIconOrBalloonClicked );
				}

				// Update copy of Window Title in case it has changed

				_notifyIcon.Text = _window.Title;

				// Show/hide Window and NotifyIcon

				bool minimized = ( _window.WindowState == WindowState.Minimized );
				_window.ShowInTaskbar = !minimized;
				_notifyIcon.Visible = minimized;

				// If this is the first time minimizing to the tray, show the user what happened

				if ( minimized && !_balloonShown && !Globals.SuppressTrayBubble )
				{
					_notifyIcon.ShowBalloonTip( 1000, null, _window.Title, ToolTipIcon.None );
					_balloonShown = true;
				}
			}

			/// <summary>
			/// Handles a click on the notify icon or its balloon.
			/// </summary>
			/// <param name="sender">Event source.</param>
			/// <param name="e">Event arguments.</param>
			private void HandleNotifyIconOrBalloonClicked( object sender, EventArgs e )
			{
				// Restore the Window

				_window.WindowState = WindowState.Normal;
			}
		}
	}
}
