using System;

using ASCOM.Astrometry.AstroUtils;
using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
    public class DevHubTelescopeStatus : AscomTelescopeStatus
    {
        public static DevHubTelescopeStatus GetEmptyStatus()
        {
            DevHubTelescopeStatus status = new DevHubTelescopeStatus();
            status.Clean();

            return status;
        }

        public DevHubTelescopeStatus()
            : base()
        { }

        public DevHubTelescopeStatus( TelescopeManager mgr )
            : base( mgr )
        {
            try
            {
                if ( mgr.ParkingState != ParkingStateEnum.ParkInProgress )
                {
                    ParkingState = ( AtPark ) ? ParkingStateEnum.IsAtPark : ParkingStateEnum.Unparked;
                }

                double sidTime = SiderealTime;
                double ra = RightAscension;

                if ( Double.IsNaN( sidTime ) || Double.IsNaN( ra ) )
                {
                    LocalHourAngle = Double.NaN;
                }
                else
                {
                    LocalHourAngle = CalculateHourAngle( RightAscension );
                }

				IsCounterWeightUp = ( mgr.Parameters.AlignmentMode == AlignmentModes.algGermanPolar
										&& CalculateCounterWeightUp( SideOfPier, LocalHourAngle ) );

			}
			catch ( Exception )
            {
                IsCounterWeightUp = false;
                ParkingState = ParkingStateEnum.Unparked;
                LocalHourAngle = Double.NaN;
            }
        }

        #region Public Properties

        public bool IsReadyToSlew => ParkingState == ParkingStateEnum.Unparked && !Slewing;

        #endregion

        #region Change Notification Properties

        private bool _isCounterWeightUp;

        public bool IsCounterWeightUp
        {
            get { return _isCounterWeightUp; }
            set
            {
                if ( value != _isCounterWeightUp )
                {
                    _isCounterWeightUp = value;
                    OnPropertyChanged();
                }
            }
        }

        private ParkingStateEnum _parkingState;

        public ParkingStateEnum ParkingState
        {
            get { return _parkingState; }
            set
            {
                if ( value != _parkingState )
                {
                    _parkingState = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _localHourAngle;

        public double LocalHourAngle
        {
            get { return _localHourAngle; }
            set
            {
                if ( value != _localHourAngle )
                {
                    _localHourAngle = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Public Methods

		public double CalculateHourAngle( double rightAscension )
		{
			// Calculate the hour angle given our internal sidereal time and the pass RA value.
			// This method does not validate the RA!!

			double retval = SiderealTime - rightAscension;
			retval = ConditionHA( retval );

			return retval;
		}

        public override void Clean()
        {
            base.Clean();

            IsCounterWeightUp = false;
            ParkingState = ( AtPark ) ? ParkingStateEnum.IsAtPark : ParkingStateEnum.Unparked;
            LocalHourAngle = Double.NaN;
        }

        #endregion

        #region Helper Methods

        private bool CalculateCounterWeightUp( PierSide pierSide, double hourAngle )
        {
			//	The CW state is determined by looking at two things:  pier side and hour angle (HA = LST – RA).  
			//		Pier Side = West;                    0 > HA > -12              CW Down
			//		Pier Side = East;                    0 < HA < +12              CW Down
			//
			//		Pier Side = East;                    0 > HA > -12              CW UP
			//		Pier Side = West;                    0 < HA < +12              CW UP		

			bool retval = false;

            if ( pierSide == PierSide.pierEast && -12 < hourAngle && hourAngle < 0 )
            {
                retval = true;
            }
            else if ( pierSide == PierSide.pierWest && 0 < hourAngle && hourAngle < 12 )
            {
                retval = true;
            }

            return retval;
        }

		private double ConditionHA( double ha )
        {
            double lowerBound = -12.0;
            double upperBound = 12.0;
            double range = upperBound - lowerBound;

            double retval = ha;

			while ( retval < lowerBound )
			{
				retval += range;
			}

			while ( retval > upperBound )
			{
				retval -= range;
			}

            return retval;
        }

        #endregion
    }
}