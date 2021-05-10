using System;
using System.ComponentModel;
using System.Diagnostics;

using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public class JogDirection : INotifyPropertyChanged
	{
		protected bool ThrowOnInvalidPropertyName { get; set; }

		public JogDirection()
		{
			ThrowOnInvalidPropertyName = true;

			GuideDirection = null;
		}

		private string _name;

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != _name )
				{
					_name = value;
					OnPropertyChanged();
				}
			}
		}

		private string _description;

		public string Description
		{
			get { return _description; }
			set
			{
				if ( value != _description )
				{
					_description = value;
					OnPropertyChanged();
				}
			}
		}

		private MoveDirections _moveDirection;

		public MoveDirections MoveDirection
		{
			get { return _moveDirection; }
			set
			{
				if ( value != _moveDirection )
				{
					_moveDirection = value;
					OnPropertyChanged();
				}
			}
		}

		private TelescopeAxes _axis;

		public TelescopeAxes Axis
		{
			get { return _axis; }
			set
			{
				if ( value != _axis )
				{
					_axis = value;
					OnPropertyChanged();
				}
			}
		}

		private double _rateSign;

		public double RateSign
		{
			get { return _rateSign; }
			set
			{
				if ( value != _rateSign )
				{
					_rateSign = value;
					OnPropertyChanged();
				}
			}
		}

		private GuideDirections? _guideDirection;

		public GuideDirections? GuideDirection
		{
			get { return _guideDirection; }
			set
			{
				if ( value != _guideDirection )
				{
					_guideDirection = value;
					OnPropertyChanged();
				}
			}
		}

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
