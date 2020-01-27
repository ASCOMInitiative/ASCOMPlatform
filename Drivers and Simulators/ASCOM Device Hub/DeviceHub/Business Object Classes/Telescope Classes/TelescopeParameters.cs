using System;
using System.Collections;
using System.Collections.Generic;
using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public class TelescopeParameters : DevicePropertiesBase, ICloneable
	{
		// This class contains static telescope properties, other than the Can... properties.

		public TelescopeParameters()
		{}

		public TelescopeParameters( TelescopeManager mgr )
		{
			DeviceManager = mgr;

			AlignmentMode = mgr.AlignmentMode;
			ApertureArea = mgr.ApertureArea;
			ApertureDiameter = mgr.ApertureDiameter;
			Description = mgr.Description;
			DriverInfo = mgr.DriverInfo;
			DoesRefraction = mgr.DoesRefraction;
			DriverVersion = mgr.DriverVersion;
			EquatorialSystem = mgr.EquatorialSystem;
			FocalLength = mgr.FocalLength;
			InterfaceVersion = mgr.InterfaceVersion;
			SiteElevation = mgr.SiteElevation;
			SiteLatitude = mgr.SiteLatitude;
			SiteLongitude = mgr.SiteLongitude;
			SlewSettleTime = mgr.SlewSettleTime;

			ArrayList arr = mgr.SupportedActions;
			this.SupportedActions = (string[])arr.ToArray( typeof( string ) );

			List<TrackingRateItem> temp = new List<TrackingRateItem>();

			ITrackingRates rates = mgr.TrackingRates;

			if ( rates != null )
			{
				foreach ( DriveRates rate in rates )
				{
					string name = "";

					switch ( rate )
					{
						case DriveRates.driveKing:
							name = "King";
							break;

						case DriveRates.driveLunar:
							name = "Lunar";
							break;

						case DriveRates.driveSidereal:
							name = "Sidereal";
							break;

						case DriveRates.driveSolar:
							name = "Solar";
							break;
					}

					temp.Add( new TrackingRateItem( name, rate ) );
				}
			}

			TrackingRates = temp.ToArray();
		}


		public TelescopeParameters( TelescopeParameters other )
		{
			this.DeviceManager = other.DeviceManager;
			this.Exceptions = other.Exceptions.Clone();

			// Don't go through the property setter so we can avoid throwing exceptions.

			this._alignmentMode = other.AlignmentMode;
			this._apertureArea = other.ApertureArea;
			this._apertureDiameter = other.ApertureDiameter;
			this._description = other.Description;
			this._driverInfo = other.DriverInfo;
			this._doesRefraction = other.DoesRefraction;
            this._driverVersion = other.DriverVersion;
			this._equatorialSystem = other.EquatorialSystem;
			this._focalLength = other.FocalLength;
			this._interfaceVersion = other.InterfaceVersion;
			this._siteElevation = other.SiteElevation;
			this._siteLatitude = other.SiteLatitude;
			this._siteLongitude = other.SiteLongitude;
			this._slewSettleTime = other.SlewSettleTime;
			this._supportedActions = other.SupportedActions;
			this._trackingRates = other.TrackingRates;
		}

		#region Change Notification Properties

		private short _interfaceVersion;

		public short InterfaceVersion
		{
			get { return _interfaceVersion; }
			set
			{
				if ( value != _interfaceVersion )
				{
					GetExceptionFromManager();
					_interfaceVersion = value;
					OnPropertyChanged();
				}
			}
		}

		private AlignmentModes _alignmentMode;

		public AlignmentModes AlignmentMode
		{
			get { return _alignmentMode; }
			set
			{
				if ( value != _alignmentMode )
				{
					GetExceptionFromManager();
					_alignmentMode = value;
					OnPropertyChanged();
				}
			}
		}

		private double _apertureArea;

		public double ApertureArea
		{
			get { return _apertureArea; }
			set
			{
				if ( value != _apertureArea )
				{
					GetExceptionFromManager();
					_apertureArea = value;
					OnPropertyChanged();
				}
			}
		}

		private double _apertureDiameter;

		public double ApertureDiameter
		{
			get { return _apertureDiameter; }
			set
			{
				if ( value != _apertureDiameter )
				{
					GetExceptionFromManager();
					_apertureDiameter = value;
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
					GetExceptionFromManager();
					_description = value;
					OnPropertyChanged();
				}
			}
		}

		private string _driverInfo;

		public string DriverInfo
		{
			get { return _driverInfo; }
			set
			{
				if ( value != _driverInfo )
				{
					GetExceptionFromManager();
					_driverInfo = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _doesRefraction;

		public bool DoesRefraction
		{
			get { return _doesRefraction; }
			set
			{
				if ( value != _doesRefraction )
				{
					GetExceptionFromManager();
					_doesRefraction = value;
					OnPropertyChanged();
				}
			}
		}

        private string _driverVersion;

        public string DriverVersion
        {
            get { return _driverVersion; }
            set
            {
                if ( value != _driverVersion )
                {
					GetExceptionFromManager();
					_driverVersion = value;
					OnPropertyChanged();
				}
			}
        }

		private EquatorialCoordinateType _equatorialSystem;

		public EquatorialCoordinateType EquatorialSystem
		{
			get { return _equatorialSystem; }
			set
			{
				if ( value != _equatorialSystem )
				{
					GetExceptionFromManager();
					_equatorialSystem = value;
					OnPropertyChanged();
				}
			}
		}

		private double _focalLength;

		public double FocalLength
		{
			get { return _focalLength; }
			set
			{
				if ( value != _focalLength )
				{
					GetExceptionFromManager();
					_focalLength = value;
					OnPropertyChanged();
				}
			}
		}

		private double _siteElevation;

		public double SiteElevation
		{
			get { return _siteElevation; }
			set
			{
				if ( value != _siteElevation )
				{
					GetExceptionFromManager();
					_siteElevation = value;
					OnPropertyChanged();
				}
			}
		}

		private double _siteLatitude;

		public double SiteLatitude
		{
			get { return _siteLatitude; }
			set
			{
				if ( value != _siteLatitude )
				{
					GetExceptionFromManager();
					_siteLatitude = value;
					OnPropertyChanged();
				}
			}
		}

		private double _siteLongitude;

		public double SiteLongitude
		{
			get { return _siteLongitude; }
			set
			{
				if ( value != _siteLongitude )
				{
					GetExceptionFromManager();
					_siteLongitude = value;
					OnPropertyChanged();
				}
			}
		}

		private short _slewSettleTime;

		public short SlewSettleTime
		{
			get { return _slewSettleTime; }
			set
			{
				if ( value != _slewSettleTime )
				{
					GetExceptionFromManager();
					_slewSettleTime = value;
					OnPropertyChanged();
				}
			}
		}

		private string[] _supportedActions;

		public string[] SupportedActions
		{
			get { return _supportedActions; }
			set
			{
				if ( value != _supportedActions )
				{
					GetExceptionFromManager();
					_supportedActions = value;
					OnPropertyChanged();
				}
			}
		}

		private TrackingRateItem[] _trackingRates;

		public TrackingRateItem[] TrackingRates
		{
			get { return _trackingRates; }
			set
			{
				if ( value != _trackingRates )
				{
					GetExceptionFromManager();
					_trackingRates = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region ICloneable Methods

		object ICloneable.Clone()
		{
			return new TelescopeParameters( this );
		}

		public TelescopeParameters Clone()
		{
			return new TelescopeParameters( this );
		}

		#endregion
	}
}
