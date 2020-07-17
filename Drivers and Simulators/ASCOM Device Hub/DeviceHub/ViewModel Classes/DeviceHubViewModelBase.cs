using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace ASCOM.DeviceHub
{
	public class DeviceHubViewModelBase : INotifyPropertyChanged, IDisposable
	{
		public event DialogCloseEventHandler RequestClose;
		public delegate void DialogCloseEventHandler( object sender, DialogCloseEventArgs e );

		protected bool ThrowOnInvalidPropertyName { get; set; }

		#region Constructor

		public DeviceHubViewModelBase()
		{
			_messageBoxService = null;
		}

		#endregion

		protected virtual void OnRequestClose( bool? result )
		{
			RequestClose( this, new DialogCloseEventArgs( result ) );
		}

		#region MessageBox Helpers

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

		#endregion MessageBox Helpers

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

		#endregion INotifyPropertyChanged Members

		#region IDisposable implementation

		protected virtual void DoDispose()
		{ }

		public void Dispose()
		{
			DoDispose();
		}

		#endregion IDisposable implementation
	}
}
