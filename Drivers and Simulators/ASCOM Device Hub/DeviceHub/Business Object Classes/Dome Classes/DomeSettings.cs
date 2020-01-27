using System;
using System.Globalization;
using ASCOM.Utilities;

namespace ASCOM.DeviceHub
{
	public class DomeSettings
	{
		private const string _domeIDProfileName = "Dome ID";
		private const string _domeIDDefault = "ASCOM.Simulator.Dome";
		private const string _traceStateProfileName = "Trace Level";
		private const string _traceStateDefault = "false";
		private const string _xOffsetProfileName = "X Offset";
		private const string _yOffsetProfileName = "Y Offset";
		private const string _zOffsetProfileName = "Z Offset";
		private const string _offsetDefault = "0.0";
		private const string _radiusProfileName = "Dome Radius";
		private const string _radiusDefault = "0";
		private const string _gemAxisOffsetProfileName = "GEM Axis Offset";
		private const string _gemAxisOffsetDefault = "0";
		private const string _azimuthAccuracyProfileName = "Azimuth Accuracy";
		private const string _azimuthAccuracyDefault = "2";
		private const string _azimuthAdjustmentProfileName = "Azimuth Adjustment";
		private const string _azimuthAdjustmentDefault = "0";
		private const string _slaveIntervalProfileName = "Slave Interval";
		private const string _slaveIntervalDefault = "30";
		private const string _usePOTHSlavingCalculationProfileName = "Use POTH Slaving Calculation";
		private const string _usePOTHSlavingCalculationDefault = "false";

		private static string DriverID => Globals.DevHubDomeID;

		public static DomeSettings FromProfile()
		{
			string domeID;
			double layoutXOffset;
			double layoutYOffset;
			double layoutZOffset;
			int domeRadius;
			int gemAxisOffset;
			int azimuthAccuracy;
			int azimuthAdjustment;
			int slaveInterval;
			bool loggerEnabled;
			bool usePOTHCalculation;

			using ( Profile profile = new Profile() )
			{
				profile.DeviceType = "Dome";
				domeID = profile.GetValue( DriverID, _domeIDProfileName, String.Empty, _domeIDDefault );
				layoutXOffset = Convert.ToDouble( profile.GetValue( DriverID, _xOffsetProfileName, String.Empty, _offsetDefault ), CultureInfo.InvariantCulture );
				layoutYOffset = Convert.ToDouble( profile.GetValue( DriverID, _yOffsetProfileName, String.Empty, _offsetDefault ), CultureInfo.InvariantCulture );
				layoutZOffset = Convert.ToDouble( profile.GetValue( DriverID, _zOffsetProfileName, String.Empty, _offsetDefault ), CultureInfo.InvariantCulture );
				domeRadius = Convert.ToInt32( profile.GetValue( DriverID, _radiusProfileName, String.Empty, _radiusDefault ) );
				gemAxisOffset = Convert.ToInt32( profile.GetValue( DriverID, _gemAxisOffsetProfileName, String.Empty, _gemAxisOffsetDefault ) );
				azimuthAccuracy = Convert.ToInt32( profile.GetValue( DriverID, _azimuthAccuracyProfileName, String.Empty, _azimuthAccuracyDefault ) );
				azimuthAdjustment = Convert.ToInt32( profile.GetValue( DriverID, _azimuthAdjustmentProfileName, String.Empty, _azimuthAdjustmentDefault ) );
				slaveInterval = Convert.ToInt32( profile.GetValue( DriverID, _slaveIntervalProfileName, String.Empty, _slaveIntervalDefault ) );
				loggerEnabled = Convert.ToBoolean( profile.GetValue( DriverID, _traceStateProfileName, String.Empty, _traceStateDefault ) );
				usePOTHCalculation = Convert.ToBoolean( profile.GetValue( DriverID, _usePOTHSlavingCalculationProfileName, String.Empty, _usePOTHSlavingCalculationDefault ) );
			}

			DomeLayoutSettings layoutSettings = new DomeLayoutSettings
			{
				DomeScopeOffset = new System.Windows.Media.Media3D.Point3D( layoutXOffset, layoutYOffset, layoutZOffset ),
				DomeRadius = domeRadius,
				GemAxisOffset = gemAxisOffset,
				AzimuthAccuracy = azimuthAccuracy,
				SlaveInterval = slaveInterval
			};

			DomeSettings settings = new DomeSettings
			{
				DomeID = domeID,
				DomeLayout = layoutSettings,
				AzimuthAdjustment = azimuthAdjustment,
				UsePOTHDomeSlaveCalculation = usePOTHCalculation,
				IsLoggingEnabled = loggerEnabled
			};

			return settings;
		}

		public DomeSettings()
		{ }

		public string DomeID { get; set; }
		public DomeLayoutSettings DomeLayout { get; set; }
		public bool IsLoggingEnabled { get; set; }
		public double AzimuthAdjustment { get; set; }
		public bool UsePOTHDomeSlaveCalculation { get; set; }

		public void ToProfile()
		{
			using ( Profile profile = new Profile() )
			{
				profile.DeviceType = "Dome";
				profile.WriteValue( DriverID, _domeIDProfileName, DomeID );
				profile.WriteValue( DriverID, _xOffsetProfileName, DomeLayout.DomeScopeOffset.X.ToString() );
				profile.WriteValue( DriverID, _yOffsetProfileName, DomeLayout.DomeScopeOffset.Y.ToString() );
				profile.WriteValue( DriverID, _zOffsetProfileName, DomeLayout.DomeScopeOffset.Z.ToString() );
				profile.WriteValue( DriverID, _radiusProfileName, DomeLayout.DomeRadius.ToString() );
				profile.WriteValue( DriverID, _gemAxisOffsetProfileName, DomeLayout.GemAxisOffset.ToString() );
				profile.WriteValue( DriverID, _azimuthAccuracyProfileName, DomeLayout.AzimuthAccuracy.ToString() );
				profile.WriteValue( DriverID, _azimuthAdjustmentProfileName, AzimuthAdjustment.ToString() );
				profile.WriteValue( DriverID, _slaveIntervalProfileName, DomeLayout.SlaveInterval.ToString() );
				profile.WriteValue( DriverID, _traceStateProfileName, IsLoggingEnabled.ToString() );
				profile.WriteValue( DriverID, _usePOTHSlavingCalculationProfileName, UsePOTHDomeSlaveCalculation.ToString() );
			}
		}
	}
}
