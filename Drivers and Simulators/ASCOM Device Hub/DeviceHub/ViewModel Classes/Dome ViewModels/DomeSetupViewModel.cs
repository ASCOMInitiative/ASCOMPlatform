﻿using System;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace ASCOM.DeviceHub
{
	public class DomeSetupViewModel : DeviceHubViewModelBase
    {
		#region Constructor

		public DomeSetupViewModel()
		{
			_fastUpdateMinimum = Globals.DOME_FAST_UPDATE_MIN;
			_fastUpdateMaximum = Globals.DOME_FAST_UPDATE_MAX;
		}

		#endregion Constructor

		#region Public Properties

		public string DomeName { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void InitializeLayout( DomeLayoutSettings settings )
		{
			_domeRadius = settings.DomeRadius;
			_gemAxisOffset = settings.GemAxisOffset;
			_scopeOffsetEWX = (int)settings.DomeScopeOffset.X;
			_scopeOffsetNSY = (int)settings.DomeScopeOffset.Y;
			_scopeOffsetUDZ = (int)settings.DomeScopeOffset.Z;
			_azimuthAccuracy = settings.AzimuthAccuracy;
			_slaveInterval = settings.SlaveInterval;
		}

		#endregion Public Methods

		#region Change Notification Properties

		private string _domeID;

		public string DomeID
		{
			get { return _domeID; }
			set
			{
				if ( value != _domeID )
				{
					_domeID = value;
					OnPropertyChanged();
				}
			}
		}

		// This property holds a measurement of the X-direction (east-west) offset from the center of the dome
		// to the intersection of the RA and Dec axes on the mount. The units are millimeters and the sign is 
		// positive in the eastward direction.

		private int _scopeOffsetEWX;

		public int ScopeOffsetEWX
		{
			get { return _scopeOffsetEWX; }
			set
			{
				if ( value != _scopeOffsetEWX )
				{
					_scopeOffsetEWX = value;
					OnPropertyChanged();
				}
			}
		}

		// This property holds a measurement of the Y-direction (north-south) offset from the center of the dome
		// to the intersection of the RA and Dec axes on the mount. The units are millimeters and the sign is 
		// positive in the northward direction.

		private int _scopeOffsetNSY;

		public int ScopeOffsetNSY
		{
			get { return _scopeOffsetNSY; }
			set
			{
				if ( value != _scopeOffsetNSY )
				{
					_scopeOffsetNSY = value;
					OnPropertyChanged();
				}
			}
		}

		// This property holds a measurement of the Z-direction (up-down) offset from the center of the dome
		// to the intersection of the RA and Dec axes on the mount. The units are millimeters and the sign is 
		// positive in the upward direction.

		private int _scopeOffsetUDZ;

		public int ScopeOffsetUDZ
		{
			get { return _scopeOffsetUDZ; }
			set
			{
				if ( value != _scopeOffsetUDZ )
				{
					_scopeOffsetUDZ = value;
					OnPropertyChanged();
				}
			}
		}

		private int _domeRadius; // in millimeters

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

		// This property holds a measurement of the distance from the intersection of the RA and Dec axes on the mount 
		// to the centerline of the telescope. The units are millimeters.

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

		// This is the allowed slop in the azimuth position, in degrees.

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

		// This is the time interval, in seconds, for checking and adjusting the azimuth position of the dome.

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

		private double _fastUpdatePeriod;

		public double FastUpdatePeriod
		{
			get { return _fastUpdatePeriod; }
			set
			{
				if ( value != _fastUpdatePeriod )
				{
					_fastUpdatePeriod = value;
					OnPropertyChanged();
				}
			}
		}

		private double _fastUpdateMinimum;

		public double FastUpdateMinimum
		{
			get { return _fastUpdateMinimum; }
			set
			{
				if ( value != _fastUpdateMinimum )
				{
					_fastUpdateMinimum = value;
					OnPropertyChanged();
				}
			}
		}

		private double _fastUpdateMaximum;

		public double FastUpdateMaximum
		{
			get { return _fastUpdateMaximum; }
			set
			{
				if ( value != _fastUpdateMaximum )
				{
					_fastUpdateMaximum = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Public Methods

		public DomeLayoutSettings GetLayout()
		{
			DomeLayoutSettings settings = new DomeLayoutSettings
			{
				DomeRadius = _domeRadius,
				GemAxisOffset = _gemAxisOffset,
				DomeScopeOffset = new Point3D( (double)_scopeOffsetEWX, (double)_scopeOffsetNSY, (double)_scopeOffsetUDZ ),
				AzimuthAccuracy = _azimuthAccuracy,
				SlaveInterval = _slaveInterval
			};

			return settings;
		}

		#endregion Public Methods

		#region Helper Methods

		private int EditTheOffset( int offset, string negativeText, string positiveText )
		{
			string directions = $"{positiveText}/{negativeText}";

			ScopeDomeOffsetViewModel vm = new ScopeDomeOffsetViewModel( directions )
			{
				NegativeText = negativeText,
				PositiveText = positiveText
			};

			vm.InitializeValues( new int[1] { offset } );

			IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
			bool? result = svc.ShowDialog( vm );

			if ( result.HasValue && result.Value )
			{
				int[] values = vm.GetValues();

				offset = values[0];
			}

			vm.Dispose();
            return offset;
		}

		protected override void DoDispose()
		{
			_chooseDomeCommand = null;
			_editEastWestOffsetCommand = null;
			_editNorthSouthOffsetCommand = null;
			_editUpDownOffsetCommand = null;
			_editGemAxisOffsetCommand = null;
			_editDomeRadiusCommand = null;
			_editAzimuthAccuracyCommand = null;
			_editSlaveIntervalCommand = null;
		}

		#endregion Helper Methods

		#region Relay Commands

		#region ChooseDomeCommand

		private ICommand _chooseDomeCommand;

		public ICommand ChooseDomeCommand
		{
			get
			{
				if ( _chooseDomeCommand == null )
				{
					_chooseDomeCommand = new RelayCommand(
						param => this.ChooseDome() );
				}

				return _chooseDomeCommand;
			}
		}

		private void ChooseDome()
		{
			string oldID = DomeID;
			string newID;

			try
			{
				newID = DomeManager.Choose( oldID );
			}
			catch ( Exception xcp )
			{
				string msg = "An error occurred when trying to launch the ASCOM Dome Chooser. "
					+ $"Details follow:\r\n\r\n{xcp}";
				ShowMessage( msg, "ASCOM Dome Chooser Error" );

				return;
			}

			if ( String.IsNullOrEmpty( newID ) )
			{
				return;
			}

			// Prevent us from choosing ourselves as our dome.

			if ( newID == Globals.DevHubDomeID )
			{
				string msg = $"{Globals.DevHubDomeID} cannot be chosen as the dome!";
				ShowMessage( msg, "Invalid Dome Selected" );

				return;
			}

			if ( newID != oldID )
			{
				DomeID = newID;
			}
		}

		#endregion

		#region EditEastWestOffsetCommand

		private ICommand _editEastWestOffsetCommand;

		public ICommand EditEastWestOffsetCommand
		{
			get
			{
				if ( _editEastWestOffsetCommand == null )
				{
					_editEastWestOffsetCommand = new RelayCommand(
						param => this.EditEastWestOffset() );
				}

				return _editEastWestOffsetCommand;
			}
		}

		private void EditEastWestOffset()
		{
			int offset = ScopeOffsetEWX;

			string negativeText = "West";
			string positiveText = "East";

			offset = EditTheOffset( offset, negativeText, positiveText );

			ScopeOffsetEWX = offset;
		}

		#endregion

		#region EditNorthSouthOffsetCommand

		private ICommand _editNorthSouthOffsetCommand;

		public ICommand EditNorthSouthOffsetCommand
		{
			get
			{
				if ( _editNorthSouthOffsetCommand == null )
				{
					_editNorthSouthOffsetCommand = new RelayCommand(
						param => this.EditNorthSouthOffset() );
				}

				return _editNorthSouthOffsetCommand;
			}
		}

		private void EditNorthSouthOffset()
		{
			int offset = ScopeOffsetNSY;

			string negativeText = "South";
			string positiveText = "North";

			offset = EditTheOffset( offset, negativeText, positiveText );

			ScopeOffsetNSY = offset;
		}

		#endregion

		#region EditUpDownOffsetCommand

		private ICommand _editUpDownOffsetCommand;

		public ICommand EditUpDownOffsetCommand
		{
			get
			{
				if ( _editUpDownOffsetCommand == null )
				{
					_editUpDownOffsetCommand = new RelayCommand(
						param => this.EditUpDownOffset() );
				}

				return _editUpDownOffsetCommand;
			}
		}

		private void EditUpDownOffset()
		{
			int offset = ScopeOffsetUDZ;
			string negativeText = "Down";
			string positiveText = "Up";

			offset = EditTheOffset( offset, negativeText, positiveText );

			ScopeOffsetUDZ = offset;
		}

		#endregion

		#region EditDomeRadiusCommand

		private ICommand _editDomeRadiusCommand;

		public ICommand EditDomeRadiusCommand
		{
			get
			{
				if ( _editDomeRadiusCommand == null )
				{
					_editDomeRadiusCommand = new RelayCommand(
						param => this.EditDomeRadius() );
				}

				return _editDomeRadiusCommand;
			}
		}

		private void EditDomeRadius()
		{
			int radius = DomeRadius;

			SimpleValueEntryViewModel vm = new SimpleValueEntryViewModel( "DomeRadius"
				, "Enter the radius of the dome, in mm", 0, 20000, 0, 20000, ' ' );
			vm.InitializeValues( new int[1] { radius } );

			IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
			bool? result = svc.ShowDialog( vm );

			if ( result.HasValue && result.Value )
			{
				int[] values = vm.GetValues();

				radius = values[0];
			}

			vm.Dispose();
            DomeRadius = radius;
		}

		#endregion

		#region EditGemAxisOffsetCommand

		private ICommand _editGemAxisOffsetCommand;

		public ICommand EditGemAxisOffsetCommand
		{
			get
			{
				if ( _editGemAxisOffsetCommand == null )
				{
					_editGemAxisOffsetCommand = new RelayCommand(
						param => this.EditGemAxisOffset() );
				}

				return _editGemAxisOffsetCommand;
			}
		}

		private void EditGemAxisOffset()
		{
			int offset = GemAxisOffset;

			SimpleValueEntryViewModel vm = new SimpleValueEntryViewModel( "GemAxisOffset"
				, "Enter the GEM axis offset distance, in mm", 0, 2000, 0, 2000, ' ' );
			vm.InitializeValues( new int[1] { offset } );

			IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
			bool? result = svc.ShowDialog( vm );

			if ( result.HasValue && result.Value )
			{
				int[] values = vm.GetValues();

				offset = values[0];
			}

			vm.Dispose();
            GemAxisOffset = offset;
		}

		#endregion

		#region EditAzimuthAccuracyCommand

		private ICommand _editAzimuthAccuracyCommand;

		public ICommand EditAzimuthAccuracyCommand
		{
			get
			{
				if ( _editAzimuthAccuracyCommand == null )
				{
					_editAzimuthAccuracyCommand = new RelayCommand(
						param => this.EditAzimuthAccuracy() );
				}

				return _editAzimuthAccuracyCommand;
			}
		}

		private void EditAzimuthAccuracy()
		{
			int accuracy = AzimuthAccuracy;

			SimpleValueEntryViewModel vm = new SimpleValueEntryViewModel( "AzimuthAccuracy"
				, "Enter the allowed azimuth difference, in degrees", 0, 10, 0, 10, ' ' );
			vm.InitializeValues( new int[1] { accuracy } );

			IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
			bool? result = svc.ShowDialog( vm );

			if ( result.HasValue && result.Value )
			{
				int[] values = vm.GetValues();

				accuracy = values[0];
			}

			vm.Dispose();
            AzimuthAccuracy = accuracy;
		}

		#endregion

		#region EditSlaveIntervalCommand

		private ICommand _editSlaveIntervalCommand;

		public ICommand EditSlaveIntervalCommand
		{
			get
			{
				if ( _editSlaveIntervalCommand == null )
				{
					_editSlaveIntervalCommand = new RelayCommand(
						param => this.EditSlaveInterval() );
				}

				return _editSlaveIntervalCommand;
			}
		}

		private void EditSlaveInterval()
		{
			int interval = SlaveInterval;

			SimpleValueEntryViewModel vm = new SimpleValueEntryViewModel( "SlaveInterval"
				, "Enter the azimuth adjustment interval, in seconds", 0, 300, 5, 300, ' ' );
			vm.InitializeValues( new int[1] { interval } );

			IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
			bool? result = svc.ShowDialog( vm );

			if ( result.HasValue && result.Value )
			{
				int[] values = vm.GetValues();

				interval = values[0];
			}

			vm.Dispose();

            // Don't allow a value less than 5 seconds.

            SlaveInterval = Math.Max( interval, 5 );
		}

		#endregion

		#endregion Relay Commands
	}
}
