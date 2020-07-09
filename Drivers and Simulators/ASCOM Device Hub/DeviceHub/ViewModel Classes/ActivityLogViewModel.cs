using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class ActivityLogViewModel : DeviceHubDialogViewModelBase
	{
		[DllImport( "user32.dll" )]
		static extern IntPtr GetOpenClipboardWindow();

		[DllImport( "user32" )]
		static extern uint GetWindowThreadProcessId( IntPtr hWnd, out int lpdwProcessId );

		private object _bufferLock = new Object();
		private int _bufferInterval = 1000; // empty buffer to log once/second
		private TaskScheduler UISyncContext { get; set; }
				private StringBuilder DataBuffer { get; set; }
		private Task BufferTask { get; set; }
		private CancellationTokenSource BufferingCts { get; set; }

		public ActivityLogViewModel()
			: base( "Activity Log" )
		{
			_isActive = false;
			_logContents = String.Empty;
			_allLoggingPaused = false;
			_pausedCommandText = "Pause";
			UpdateMemoryUsage();
			Messenger.Default.Register<ActivityMessage>( this, ( action ) => AppendToLog( action ) );
			UISyncContext = TaskScheduler.FromCurrentSynchronizationContext();
			DataBuffer = new StringBuilder();

			BufferingCts = new CancellationTokenSource();
			BufferTask = Task.Factory.StartNew( () => AppendBufferToLog( BufferingCts.Token ),
												BufferingCts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default );

		}

		private bool _isActive;

		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if ( value != _isActive )
				{
					_isActive = value;

					if ( _isActive )
					{
						// Clear the log when the log viewer is activated.

						ClearLog();
					}
				}
			}
		}

		#region Change Notification Properties

		private bool _enableTelescopeLogging;

		public bool EnableTelescopeLogging
		{
			get { return _enableTelescopeLogging; }
			set
			{
				if ( value != _enableTelescopeLogging )
				{
					_enableTelescopeLogging = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _enableDomeLogging;

		public bool EnableDomeLogging
		{
			get { return _enableDomeLogging; }
			set
			{
				if ( value != _enableDomeLogging )
				{
					_enableDomeLogging = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _enableFocuserLogging;

		public bool EnableFocuserLogging
		{
			get { return _enableFocuserLogging; }
			set
			{
				if ( value != _enableFocuserLogging )
				{
					_enableFocuserLogging = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _enableCommandLogging;

		public bool EnableCommandLogging
		{
			get { return _enableCommandLogging; }
			set
			{
				if ( value != _enableCommandLogging )
				{
					_enableCommandLogging = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _enableStatusLogging;

		public bool EnableStatusLogging
		{
			get { return _enableStatusLogging; }
			set
			{
				if ( value != _enableStatusLogging )
				{
					_enableStatusLogging = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _enableParametersLogging;

		public bool EnableParametersLogging
		{
			get { return _enableParametersLogging; }
			set
			{
				if ( value != _enableParametersLogging )
				{
					_enableParametersLogging = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _enableCapabilitiesLogging;

		public bool EnableCapabilitiesLogging
		{
			get { return _enableCapabilitiesLogging; }
			set
			{
				if ( value != _enableCapabilitiesLogging )
				{
					_enableCapabilitiesLogging = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _enableOthersLogging;

		public bool EnableOthersLogging
		{
			get { return _enableOthersLogging; }
			set
			{
				if ( value != _enableOthersLogging )
				{
					_enableOthersLogging = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _allLoggingPaused;

		public bool AllLoggingPaused
		{
			get { return _allLoggingPaused; }
			set
			{
				if ( value != _allLoggingPaused )
				{
					_allLoggingPaused = value;
					OnPropertyChanged();
					PausedCommandText = _allLoggingPaused ? "Resume" : "Pause";
				}
			}
		}

		private string _pausedCommandText;

		public string PausedCommandText
		{
			get { return _pausedCommandText; }
			set
			{
				if ( value != _pausedCommandText )
				{
					_pausedCommandText = value;
					OnPropertyChanged();
				}
			}
		}

		private string _logContents;

		public string LogContents
		{
			get { return _logContents; }
			set
			{
				if ( value != _logContents )
				{
					_logContents = value;
					OnPropertyChanged();
				}
			}
		}

		private double _memoryUsage;

		public double MemoryUsage
		{
			get { return _memoryUsage; }
			set
			{
				if ( value != _memoryUsage )
				{
					_memoryUsage = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Helper Methods

		private string FormatEntry( string msg )
		{
			return msg;
		}

		private void AppendToLog( ActivityMessage action )
		{
			if ( !IsActive || AllLoggingPaused )
			{
				return;
			}

			if ( action.DeviceType == DeviceTypeEnum.Telescope && EnableTelescopeLogging
				|| action.DeviceType == DeviceTypeEnum.Dome && EnableDomeLogging
				|| action.DeviceType == DeviceTypeEnum.Focuser && EnableFocuserLogging )
			{
				if ( action.MessageType == ActivityMessageTypes.Capabilities && EnableCapabilitiesLogging
					|| action.MessageType == ActivityMessageTypes.Commands && EnableCommandLogging
					|| action.MessageType == ActivityMessageTypes.Other && EnableOthersLogging
					|| action.MessageType == ActivityMessageTypes.Parameters && EnableParametersLogging
					|| action.MessageType == ActivityMessageTypes.Status && EnableStatusLogging )
				{
					string newText = FormatEntry( action.MessageText );

					// UpdateTheLog( newText );

					lock ( _bufferLock )
					{
						DataBuffer.Append( newText );
					}
				}
			}
		}

		private void AppendBufferToLog( CancellationToken token )
		{
			bool taskCancelled = false;

			while ( !taskCancelled )
			{
				string newData = "";

				lock ( _bufferLock )
				{
					if ( DataBuffer.Length > 0 )
					{
						newData = DataBuffer.ToString();
						DataBuffer.Clear();
					}
				}

				if ( newData.Length > 0 )
				{
					UpdateTheLog( newData );
				}

				taskCancelled = token.WaitHandle.WaitOne( _bufferInterval );
			}
		}

		private async void UpdateTheLog( string newText )
		{
			Task task = Task.Factory.StartNew( () =>
			{
				string contents = LogContents;

				// Figure out how many lines to delete to get the size under our limit;

				int lengthLimit = Properties.Settings.Default.ActivityLogCapacity;
				int charsToTrim = 0;

				// Trim whole lines from the LogContents so that when the new text is appended, the total
				// length is less than the allowed limit.

				if ( contents.Length > 0 )
				{
					// We can only trim the log contents if it has data!

					int minToDelete = contents.Length + newText.Length - lengthLimit;

					if ( minToDelete > 0 )
					{
						// We need to delete chars from the start of contents to keep below the length limit.

						if ( minToDelete > contents.Length )
						{
							contents = String.Empty;
						}
						else
						{
							int index = contents.IndexOf( Environment.NewLine, minToDelete, StringComparison.InvariantCulture ) + 1;

							if ( index >= 0 )
							{
								charsToTrim = index;
							}
						}
					}
				}
				
				StringBuilder sb = new StringBuilder();

				if ( contents.Length > 0 )
				{
					sb.Append( contents.Substring( charsToTrim ) );
				}

				sb.Append( newText );

				LogContents = sb.ToString();
				UpdateMemoryUsage();

			}, CancellationToken.None, TaskCreationOptions.None, UISyncContext );

			await task;

			task.Dispose();
			task = null;
		}

		private void UpdateMemoryUsage()
		{
			MemoryUsage = (double)GC.GetTotalMemory( false ) / Math.Pow( 1024.0, 2.0) ;

		}

		protected override void DoDispose()
		{
			_pauseLoggingCommand = null;
			_clearLogCommand = null;
			_copyLogCommand = null;
			_closeLogCommand = null;
		}

		#endregion Helper Methods

		#region Relay Commands

		#region PauseLoggingCommand

		private ICommand _pauseLoggingCommand;

		public ICommand PauseLoggingCommand
		{
			get
			{
				if ( _pauseLoggingCommand == null )
				{
					_pauseLoggingCommand = new RelayCommand(
						param => this.PauseLogging() );
				}

				return _pauseLoggingCommand;
			}
		}

		private void PauseLogging()
		{
			AllLoggingPaused = !AllLoggingPaused;
		}

		#endregion PauseLoggingCommand

		#region ClearLogCommand

		private ICommand _clearLogCommand;

		public ICommand ClearLogCommand
		{
			get
			{
				if ( _clearLogCommand == null )
				{
					_clearLogCommand = new RelayCommand(
						param => this.ClearLog() );
				}

				return _clearLogCommand;
			}
		}

		private void ClearLog()
		{
			LogContents = String.Empty;
		}

		#endregion ClearLogCommand

		#region CopyLogCommand

		private ICommand _copyLogCommand;

		public ICommand CopyLogCommand
		{
			get
			{
				if ( _copyLogCommand == null )
				{
					_copyLogCommand = new RelayCommand(
						param => this.CopyLog(),
						param => this.CanCopyLog() );
				}

				return _copyLogCommand;
			}
		}

		private void CopyLog()
		{
			try
			{
				Clipboard.SetText( LogContents );
			}
			catch (COMException xcp )
			{
				string msg = "An unexpected error occurred when attempting to copy to the Windows Clipboard";

				if ( (uint)xcp.HResult == 0x800401d0 )
				{
					string processName = GetClipboardLocker();
					msg = String.Format( "Control of the Windows Clipboard has been taken by a process named {0}.\r\n\r\nPlease wait and try again or close {0} to release the Clipboard..",
						( String.IsNullOrEmpty( processName ) ) ? "" : processName );
				}

				IMessageBoxService svc = ServiceContainer.Instance.GetService<IMessageBoxService>();
				svc.Show( msg, "Clipboard Copy Error", MessageBoxButton.OK, MessageBoxImage.Error
							, MessageBoxResult.None, MessageBoxOptions.None );
			}
		}

		private string GetClipboardLocker()
		{
			string retval = String.Empty;

			IntPtr hwnd = GetOpenClipboardWindow();

			if ( hwnd != IntPtr.Zero )
			{
				int pid = 0;
				GetWindowThreadProcessId( hwnd, out pid );

				if ( pid > 0 )
				{
					retval = Process.GetProcessById( pid ).ProcessName;
				}
			}

			return retval;
		}

		private bool CanCopyLog()
		{
			return !String.IsNullOrWhiteSpace( LogContents );
		}

		#endregion CopyLogCommand

		#region CloseLogCommand

		private ICommand _closeLogCommand;

		public ICommand CloseLogCommand
		{
			get
			{
				if ( _closeLogCommand == null )
				{
					_closeLogCommand = new RelayCommand(
						param => this.CloseLog() );
				}

				return _closeLogCommand;
			}
		}

		internal void CloseLog()
		{
			Messenger.Default.Unregister<ActivityMessage>( this );

			BufferingCts.Cancel();
			BufferingCts.Dispose();
			BufferingCts = null;
			DataBuffer.Clear();
			DataBuffer = null;
			OnRequestClose( true );
			IsActive = false;
		}

		#endregion CloseLogCommand

		#endregion Relay Commands
	}
}
