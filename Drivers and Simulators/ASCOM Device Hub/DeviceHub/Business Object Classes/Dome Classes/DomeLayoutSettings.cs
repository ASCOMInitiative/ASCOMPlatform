using System;
using System.Windows.Media.Media3D;

namespace ASCOM.DeviceHub
{
	public class DomeLayoutSettings : DevicePropertiesBase, ICloneable
	{
		#region Constructors

		public DomeLayoutSettings()
		{}

		public DomeLayoutSettings( DomeLayoutSettings other )
		{
			this._domeScopeOffset = other._domeScopeOffset;
			this._domeRadius = other._domeRadius;
			this._gemAxisOffset = other._gemAxisOffset;
			this._azimuthAccuracy = other._azimuthAccuracy;
			this._slaveInterval = other._slaveInterval;
		}

		#endregion Constructors

		#region Change Notification Properties

		private Point3D _domeScopeOffset;

		public Point3D DomeScopeOffset
		{
			get { return _domeScopeOffset; }
			set
			{
				if ( value != _domeScopeOffset )
				{
					_domeScopeOffset = value;
					OnPropertyChanged();
				}
			}
		}

		private int _domeRadius;

		public int DomeRadius
		{
			get { return _domeRadius; }
			set
			{
				if ( value != _domeRadius )
				{
					_domeRadius = value;
					OnPropertyChanged();
				}
			}
		}

		private int _gemAxisOffset;

		public int GemAxisOffset
		{
			get { return _gemAxisOffset; }
			set
			{
				if ( value != _gemAxisOffset )
				{
					_gemAxisOffset = value;
					OnPropertyChanged();
				}
			}
		}

		private int _azimuthAccuracy;

		public int AzimuthAccuracy
		{
			get { return _azimuthAccuracy; }
			set
			{
				if ( value != _azimuthAccuracy )
				{
					_azimuthAccuracy = value;
					OnPropertyChanged();
				}
			}
		}

		private int _slaveInterval;

		public int SlaveInterval
		{
			get { return _slaveInterval; }
			set
			{
				if ( value != _slaveInterval )
				{
					_slaveInterval = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return new DomeLayoutSettings( this );
		}

		public DomeLayoutSettings Clone()
		{
			return new DomeLayoutSettings( this );
		}

		#endregion ICloneable Implementation
	}
}
