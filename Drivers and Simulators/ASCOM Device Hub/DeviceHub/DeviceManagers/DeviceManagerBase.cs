﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;

using ASCOM.DeviceHub.MvvmMessenger;
using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
    public enum ActivityMessageTypes { None = 0, Status, Capabilities, Parameters, Commands, Other };

	public class DeviceManagerBase : NormalizedValueBase, INotifyPropertyChanged
	{
		protected const string Done = "(done)";
		protected const string Failed = "(failed)";
		protected const string SlewStarted = "(slew started)";
		protected const string MoveStarted = "(move started)";

		protected bool ThrowOnInvalidPropertyName { get; set; }
        protected DeviceTypeEnum DeviceType { get; set; }
		protected PropertyExceptions Exceptions { get; set; }

		protected int PollingPeriod { get; set; }
		protected ManualResetEvent PollingChange { get; set; }

		public double FastPollingPeriod { get; set; }

		public DeviceManagerBase( DeviceTypeEnum deviceType )
		{
			string caller = $"DeviceManagerBase ctor for {deviceType}";

			LogAppMessage( "Initializing Instance constructor", caller );

			DeviceType = deviceType;

			ThrowOnInvalidPropertyName = false;
			_messageBoxService = null;

			Exceptions = new PropertyExceptions();

			LogAppMessage( "Instance constructor initialization complete.", caller );
		}

		public Exception GetLastPropertyException( [System.Runtime.CompilerServices.CallerMemberName] string propName = "???" )
		{
			return Exceptions.GetException( propName );
		}

		protected T GetServiceProperty<T>( Func<T> getCmd, T defaultValue
											, ActivityMessageTypes messageType = ActivityMessageTypes.Status
											, [System.Runtime.CompilerServices.CallerMemberName] string propName = "???" )
		{
			T retval = defaultValue;
			Exception xcp = null;

			CheckDevice();

			try
			{
				retval = getCmd();
			}
			catch ( Exception ex )
			{
				xcp = ex;
				string name = ex.GetType().Name;
			}

			if ( xcp == null )
			{
				if ( retval is ArrayList )
				{
					LogActivityStart( messageType, "Get {0}: ", propName );

					StringBuilder sb = new StringBuilder();
					ArrayList list = retval as ArrayList;

					if ( list.Count == 0 )
					{
						sb.Append( "Enpty List" );
					}
					else
					{
						foreach ( var item in list )
						{
							sb.Append( ( sb.Length > 0 ) ? ", " : "" );
							sb.Append( item.ToString() );
						}
					}

					LogActivityEnd( messageType, sb.ToString() );
				}
				else if ( retval is ITrackingRates)
				{
					LogActivityStart( ActivityMessageTypes.Parameters, "Get {0}:", propName );
					StringBuilder sb = new StringBuilder();

					TrackingRates rates = retval as TrackingRates;

					if ( rates == null )
					{
						sb.Append( " Did not return expected ITrackingRates collection!" );
					}
					else if ( rates.Count == 0 )
					{
						sb.Append( "Received invalid Tracking Rates collection." );
					}
					else
					{
						for ( int i = 1; i <= rates.Count; ++i )
						{
							if ( sb.Length > 0 )
							{
								sb.Append( ", " );
							}

							sb.Append( rates[i].ToString() );
						}
					}

					LogActivityEnd( ActivityMessageTypes.Parameters, sb.ToString() );
				}
				else
				{
					LogActivityLine( messageType, "Get {0}: {1}", propName, retval );
				}
			}
			else
			{
				LogActivityLine( messageType, "Get {0} Exception: {1}", propName, xcp.Message );
			}

			Exceptions.SetException( propName, xcp );

			return retval;
		}

		protected void SetServiceProperty<T>( Action setCmd, T value
											, ActivityMessageTypes messageType = ActivityMessageTypes.Status
											, [System.Runtime.CompilerServices.CallerMemberName] string propName = "???" )
		{
			Exception xcp = null;
			string msg = $"Set {propName} -> {value} ";
			CheckDevice();

			try
			{
				setCmd();
				msg += Done;
			}
			catch ( Exception ex )
			{
				xcp = ex;
				msg += Failed;
				throw;
			}
			finally
			{
				LogActivityLine( messageType, msg );

				if ( xcp != null )
				{
					LogActivityLine( messageType, "Set {0} Exception: {1}", propName, xcp.Message );
				}
			}
		}

		protected void SetServiceProperty<T>( Action setCmd, string name, T value, ActivityMessageTypes messageType = ActivityMessageTypes.Status )
		{
			Exception xcp = null;
			string msg = $"Set {name} -> {value} ";
			CheckDevice();

			try
			{
				setCmd();
				msg += Done;
			}
			catch ( Exception ex )
			{
				xcp = ex;
				msg += Failed;
			}
			finally
			{
				LogActivityLine( messageType, msg );

				if ( xcp != null )
				{
					LogActivityLine( messageType, "Set {0} Exception: {1}", name, xcp.Message );
				}
			}
		}

		protected virtual void CheckDevice()
		{ }

		protected void SetFastPolling( string caller = "")
		{
			string fromCaller = String.Empty;

			if ( !String.IsNullOrEmpty( caller ) )
			{
				fromCaller = " from " + caller;
			}

			int newInterval = (int)( FastPollingPeriod * 1000 );

			if ( PollingPeriod != newInterval )
			{
				PollingPeriod = newInterval;
				PollingChange.Set();

				LogActivityLine( ActivityMessageTypes.Commands, $"Started fast polling{fromCaller} every {PollingPeriod} ms." );
			}
			else
			{
				LogActivityLine( ActivityMessageTypes.Commands, $"SetFastPolling called{fromCaller} when already fast polling." );
			}
		}

		#region Activity Log Members

		protected void LogActivityStart( ActivityMessageTypes msgType, string msgFormat, params object[] parms )
        {
            string msg = String.Format( msgFormat, parms );

            LogActivityStart( msgType, msg );
        }

        protected void LogActivityStart( ActivityMessageTypes msgType, string msg )
        {
			string deviceTypeText = DeviceType.ToString();
			string messageTypeText = GetMessageTypeName( msgType );
			string timestamp = DateTime.Now.ToString( "HH:mm:ss.fff" );
			string text = $"{timestamp}: {deviceTypeText} - {messageTypeText}{msg}";
            LogMessage( msgType, text );
        }

        protected void LogActivityEnd( ActivityMessageTypes msgType, string msgFormat, params object[] parms )
        {
            string msg = String.Format( msgFormat, parms );

            LogActivityEnd( msgType, msg );
        }

        protected void LogActivityEnd( ActivityMessageTypes msgType, object objVal )
        {
            string msg = objVal.ToString();

            LogActivityEnd( msgType, msg );
        }

        protected void LogActivityEnd( ActivityMessageTypes msgType, string msg )
        {
            msg = " " + msg + "\r\n";

            LogMessage( msgType, msg );
        }

		protected void LogActivityLine( ActivityMessageTypes msgType, string msg )
		{
			string deviceTypeText = DeviceType.ToString();
			string messageTypeText = GetMessageTypeName( msgType );
			string timestamp = DateTime.Now.ToString( "HH:mm:ss.fff" );
			string text = $"{timestamp}: {deviceTypeText} - {messageTypeText}{msg}\r\n";

			LogMessage( msgType, text );

		}

		protected void LogActivityLine( ActivityMessageTypes msgType, string msgFormat, params object[] parms )
		{
			string msg = String.Format( msgFormat, parms );

			LogActivityLine( msgType, msg );
		}

		private void LogMessage( ActivityMessageTypes msgType, string msgText )
        {
            Messenger.Default.Send( new ActivityMessage( DeviceType, msgType, msgText ) );
        }

		private string GetMessageTypeName( ActivityMessageTypes msgType )
		{
			string typeText = "              ";

			switch ( msgType )
			{
				case ActivityMessageTypes.Capabilities:
					typeText = "Capabilities: ";
					break;

				case ActivityMessageTypes.Commands:
					typeText = "Commands:     ";
					break;

				case ActivityMessageTypes.Other:
					typeText = "Others:       ";
					break;

				case ActivityMessageTypes.Parameters:
					typeText = "Parameters:   ";
					break;

				case ActivityMessageTypes.Status:
					typeText = "Statuses:     ";
					break;
			}

			return typeText;
		}

		#endregion

		#region Application Logging Methods

		protected static void LogAppMessage( string message, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "???" )
		{
			Globals.AppLogger.LogMessage( callerName, message );
		}

		#endregion Application Logging Methods

		#region Message Box Support Members

		private IMessageBoxService _messageBoxService;

		protected IMessageBoxService MessageBoxService
		{
			get
			{
				if ( _messageBoxService == null )
				{
					_messageBoxService = ServiceContainer.Instance.GetService<IMessageBoxService>();
				}

				return _messageBoxService;
			}
		}

		protected MessageBoxResult ShowMessage( string text, string caption )
		{
			return ShowMessage( text, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.None );
		}
			
		protected MessageBoxResult ShowMessage( string text, string caption, MessageBoxButton button, MessageBoxImage icon )
		{
			return ShowMessage( text, caption, button, icon, MessageBoxResult.OK, MessageBoxOptions.None );
		}

		protected MessageBoxResult ShowMessage( string text, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult result, MessageBoxOptions options )
		{
			return MessageBoxService.Show( text, caption, button, icon, result, options );
		}

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

		/// <summary>
		/// Raises this object's PropertyChanged event.
		/// </summary>
		/// <param name="propertyName">The property that has a new value.</param>

		protected virtual void OnPropertyChanged( [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "???" )
		{
			VerifyPropertyName( propertyName );

			PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}

		/// <summary>
		/// Warns the developer if this object does not have
		/// a public property with the specified name. This 
		/// method does not exist in a Release build.
		/// </summary>
		[Conditional( "DEBUG" )]
		[DebuggerStepThrough]
		protected void VerifyPropertyName( string propertyName )
		{
			// Verify that the property name matches a real,  
			// public, instance property on this object.
			if ( TypeDescriptor.GetProperties( this )[propertyName] == null )
			{
				string msg = "Invalid property name: " + propertyName;

				if ( ThrowOnInvalidPropertyName )
				{
					throw new Exception( msg );
				}
				else
				{
					Debug.Fail( msg );
				}
			}
		}

		#endregion
	}
}
