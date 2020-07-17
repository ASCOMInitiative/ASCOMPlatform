using System;
using System.ComponentModel;
using System.Diagnostics;

namespace ASCOM.DeviceHub
{
	public class DevicePropertiesBase : INotifyPropertyChanged
	{
		protected bool ThrowOnInvalidPropertyName { get; set; }
		protected DeviceManagerBase DeviceManager { get; set; }
		protected PropertyExceptions Exceptions { get; set; }

		public DevicePropertiesBase()
		{
			ThrowOnInvalidPropertyName = true;
			DeviceManager = null;
			Exceptions = new PropertyExceptions();
		}

		public Exception GetException( [System.Runtime.CompilerServices.CallerMemberName] string propName = "???" )
		{
			return Exceptions.GetException( propName );
		}

		public void ClearException( [System.Runtime.CompilerServices.CallerMemberName] string propName = "???" )
		{
			Exceptions.SetException( propName, null );
		}

		#region Helper Methods

		protected void GetExceptionFromManager( [System.Runtime.CompilerServices.CallerMemberName] string propName = "???" )
		{
			Exception xcp = null;

			if ( DeviceManager != null )
			{
				xcp = DeviceManager.GetLastPropertyException( propName );
			}

			Exceptions.SetException( propName, xcp );
		}

		#endregion Helper Methods

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
	}
}
