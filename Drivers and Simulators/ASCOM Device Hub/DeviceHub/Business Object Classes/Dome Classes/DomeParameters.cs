using System;
using System.Collections;

namespace ASCOM.DeviceHub
{
	public class DomeParameters : DevicePropertiesBase, ICloneable
    {
		// This class contains static dome properties, other than the Can... properties.

		public DomeParameters()
		{}

		public DomeParameters( DomeParameters other )
		{
			this.Description = other.Description;
			this.DriverInfo = other.DriverInfo;
			this.DriverVersion = other.DriverVersion;
			this.InterfaceVersion = other.InterfaceVersion;
			this.SupportedActions = other.SupportedActions;
		}

		#region Public Methods

		public void InitializeFromManager( DomeManager mgr )
		{
			this.Description = mgr.Description;
			this.DriverInfo = mgr.DriverInfo;
			this.DriverVersion = mgr.DriverVersion;
			this.InterfaceVersion = mgr.InterfaceVersion;

			ArrayList arr = mgr.SupportedActions;
			this.SupportedActions = (string[])arr.ToArray( typeof( string ) );
		}

		#endregion Public Methods

		#region Change Notification Properties

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

		private string _driverInfo;

		public string DriverInfo
		{
			get { return _driverInfo; }
			set
			{
				if ( value != _driverInfo )
				{
					_driverInfo = value;
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
					_driverVersion = value;
					OnPropertyChanged();
				}
			}
		}

		private short _interfaceVersion;

		public short InterfaceVersion
		{
			get { return _interfaceVersion; }
			set
			{
				if ( value != _interfaceVersion )
				{
					_interfaceVersion = value;
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
					_supportedActions = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region ICloneable Methods

		object ICloneable.Clone()
		{
			return new DomeParameters( this );
		}

		public DomeParameters Clone()
		{
			return new DomeParameters( this );
		}

		#endregion
	}
}
