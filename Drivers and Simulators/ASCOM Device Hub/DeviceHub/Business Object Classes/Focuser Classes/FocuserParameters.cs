using System;
using System.Collections;

namespace ASCOM.DeviceHub
{
	public class FocuserParameters : DevicePropertiesBase, ICloneable
	{
		// this class contains static focuser properties.

		public FocuserParameters()
		{}

		public FocuserParameters( FocuserParameters other )
		{
			this.Absolute = other.Absolute;
			this.Description = other.Description;
			this.DriverInfo = other.DriverInfo;
			this.DriverVersion = other.DriverVersion;
			this.InterfaceVersion = other.InterfaceVersion;
			this.MaxIncrement = other.MaxIncrement;
			this.MaxStep = other.MaxStep;
			this.StepSize = other.StepSize;
			this.SupportedActions = other.SupportedActions;
			this.TempCompAvailable = other.TempCompAvailable;
		}

		#region Public Methods

		public void InitializeFromManager( FocuserManager mgr )
		{
			this.Absolute = mgr.Absolute;
			this.Description = mgr.Description;
			this.DriverInfo = mgr.DriverInfo;
			this.DriverVersion = mgr.DriverVersion;
			this.InterfaceVersion = mgr.InterfaceVersion;
			this.MaxIncrement = mgr.MaxIncrement;
			this.MaxStep = mgr.MaxStep;
			this.StepSize = mgr.StepSize;

			ArrayList arr = mgr.SupportedActions;
			this.SupportedActions = (string[])arr.ToArray( typeof( string ) );

			this.TempCompAvailable = mgr.TempCompAvailable;
		}

		#endregion Public Methods

		#region Change Notification Properties


		private bool _absolute;

		public bool Absolute
		{
			get { return _absolute; }
			set
			{
				if ( value != _absolute )
				{
					_absolute = value;
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

		private int _maxIncrement;

		public int MaxIncrement
		{
			get { return _maxIncrement; }
			set
			{
				if ( value != _maxIncrement )
				{
					_maxIncrement = value;
					OnPropertyChanged();
				}
			}
		}

		private int _maxStep;

		public int MaxStep
		{
			get { return _maxStep; }
			set
			{
				if ( value != _maxStep )
				{
					_maxStep = value;
					OnPropertyChanged();
				}
			}
		}
		
		private double _stepSize;

		public double StepSize
		{
			get { return _stepSize; }
			set
			{
				if ( value != _stepSize )
				{
					_stepSize = value;
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

		private bool _tempCompAvailable;

		public bool TempCompAvailable
		{
			get { return _tempCompAvailable; }
			set
			{
				if ( value != _tempCompAvailable )
				{
					_tempCompAvailable = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region ICloneable Methods

		object ICloneable.Clone()
		{
			return new FocuserParameters( this );
		}

		public FocuserParameters Clone()
		{
			return new FocuserParameters( this );
		}

		#endregion ICloneable Methods
	}
}
