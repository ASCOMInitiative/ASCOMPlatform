using System;

namespace ASCOM.DeviceHub
{
	public class DomeCapabilities : DevicePropertiesBase, ICloneable
	{
		public static DomeCapabilities GetFullCapabilities()
		{
			return new DomeCapabilities( true );
		}

		#region Constructor

		public DomeCapabilities()
		{
			Initialize();
		}

		private DomeCapabilities( bool fullCapabilities )
		{
			Initialize( fullCapabilities );
		}

		public DomeCapabilities( DomeCapabilities other )
		{
			ThrowOnInvalidPropertyName = true;

			this._canFindHome = other._canFindHome;
			this._canPark = other._canPark;
			this._canSetAltitude = other._canSetAltitude;
			this._canSetAzimuth = other._canSetAzimuth;
			this._canSetPark = other._canSetPark;
			this._canSetShutter = other._canSetShutter;
			this._canSlave = other._canSlave;
			this._canSyncAzimuth = other._canSyncAzimuth;
		}

		#endregion Constructor

		#region Change Notification Properties

		private bool _canFindHome;

		public bool CanFindHome
		{
			get { return _canFindHome; }
			set
			{
				if ( value != _canFindHome )
				{
					_canFindHome = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _canPark;

		public bool CanPark
		{
			get { return _canPark; }
			set
			{
				if ( value != _canPark )
				{
					_canPark = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _canSetAltitude;

		public bool CanSetAltitude
		{
			get { return _canSetAltitude; }
			set
			{
				if ( value != _canSetAltitude )
				{
					_canSetAltitude = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _canSetAzimuth;

		public bool CanSetAzimuth
		{
			get { return _canSetAzimuth; }
			set
			{
				if ( value != _canSetAzimuth )
				{
					_canSetAzimuth = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _canSetPark;

		public bool CanSetPark
		{
			get { return _canSetPark; }
			set
			{
				if ( value != _canSetPark )
				{
					_canSetPark = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _canSetShutter;

		public bool CanSetShutter
		{
			get { return _canSetShutter; }
			set
			{
				if ( value != _canSetShutter )
				{
					_canSetShutter = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _canSlave;

		public bool CanSlave
		{
			get { return _canSlave; }
			set
			{
				if ( value != _canSlave )
				{
					_canSlave = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _canSyncAzimuth;

		public bool CanSyncAzimuth
		{
			get { return _canSyncAzimuth; }
			set
			{
				if ( value != _canSyncAzimuth )
				{
					_canSyncAzimuth = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Public Methods

		public void InitializeFromManager( DomeManager mgr )
		{
			Initialize();

			CanFindHome = mgr.CanFindHome;
			CanPark = mgr.CanPark;
			CanSetAltitude = mgr.CanSetAltitude;
			CanSetAzimuth = mgr.CanSetAzimuth;
			CanSetPark = mgr.CanSetPark;
			CanSetShutter = mgr.CanSetShutter;
			CanSlave = mgr.CanSlave;
			CanSyncAzimuth = mgr.CanSyncAzimuth;
		}

		#endregion

		#region Helper Methods

		private void Initialize()
		{
			Initialize( false );
		}

		private void Initialize( bool canValue )
		{
			CanFindHome = canValue;
			CanPark = canValue;
			CanSetAltitude = canValue;
			CanSetAzimuth = canValue;
			CanSetPark = canValue;
			CanSetShutter = canValue;
			CanSlave = canValue;
			CanSyncAzimuth = canValue;
		}

		#endregion HelperMethods

		#region ICloneable Methods

		object ICloneable.Clone()
		{
			return new DomeCapabilities( this );
		}

		public DomeCapabilities Clone()
		{
			return new DomeCapabilities( this );
		}

		#endregion
	}
}
