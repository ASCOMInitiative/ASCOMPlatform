//
// 02-Oct-09	Bob Denny	ASCOM-24 : Fis conform error, Halt() when ot moving is harmless.
//							Throw error if angles outside 0 <= angle < 360 
//							(this is a reference implementation!)
//
using System;
using ASCOM.Utilities;

namespace ASCOM.Simulator
{
	//
	// This implements the simulated hardware layer.
	//
	public static class RotatorHardware
	{
        /// <summary>
        /// Name of the Driver
        /// </summary>

	    //
		// Settings, persistent
		//
		private static float s_fPosition;
		private static float s_fSpeed;
		private static bool s_bCanReverse;
		private static bool s_bReverse;

		//
		// State variables
		//
		private static bool s_bConnected;
		private static bool s_bMoving;
		private static bool s_bDirection;
		private static float s_fTargetPosition;
		private static int s_iUpdateInterval = 250;			// Milliseconds, default, set by main form
        private static string _rotatorName = "ASCOM.Simulator.Rotator";
        private static string _description = "ASCOM Rotator Driver for RotatorSimulator";
        private static string _driverInfo = "ASCOM.Simulator.Rotator";
        private static string _driverVersion = "6.0";
        private static short _interfaceVersion = 6;
        private static string s_sProgID = "ASCOM.Simulator.Rotator";

        private static Profile Profile;


		//
		// Sync object
		//
		private static object s_objSync = new object();	// Better than lock(this) - Jeffrey Richter, MSDN Jan 2003

		//
		// Constructor - initialize state
		//
		static RotatorHardware()
		{
			Profile = new Profile();
			Profile.DeviceType = "Rotator";
			s_fPosition = 0.0F;
			s_bConnected = false;
			s_bMoving = false;
			s_fTargetPosition = 0.0F;
		}

		//
		// Settings support
		//
		private static string GetSetting(string name, string defValue)
		{
			string s = Profile.GetValue(s_sProgID, name, "");
			if (s == "") s = defValue;
			return s;
		}

		//
		// Initialize/finalize for server startup/shutdown
		//
		public static void Initialize()
		{
			s_fPosition = Convert.ToSingle(GetSetting("Position", "0.0"));
			s_fTargetPosition = s_fPosition;
			RotationRate = Convert.ToSingle(GetSetting("RotationRate", "3.0"));
			s_bCanReverse = Convert.ToBoolean(GetSetting("CanReverse", "true"));
			s_bReverse = Convert.ToBoolean(GetSetting("Reverse", "false"));
		}

		public static void Finalize_()	// "Finalize" exists in parent
		{
			//s_Timer.Dispose();
			Profile.WriteValue(s_sProgID, "Position", s_fPosition.ToString(), "");
			Profile.WriteValue(s_sProgID, "RotationRate", RotationRate.ToString(), "");
			Profile.WriteValue(s_sProgID, "CanReverse", s_bCanReverse.ToString(), "");
			Profile.WriteValue(s_sProgID, "Reverse", s_bReverse.ToString(), "");
		}

		//
		// Properties for setup dialog
		//
		public static float RotationRate
		{
			get { return s_fSpeed * 1000; }				// Internally deg/millisecond
			set { s_fSpeed = value / 1000; }
		}

		public static bool CanReverse
		{
			get { return s_bCanReverse; }
			set 
			{ 
				s_bCanReverse = value;
				if (!value) s_bReverse = false;
			}
		}

		//
		// State properties for clients
		//
        public static string RotatorName
        {
            get { return _rotatorName; }
            set { _rotatorName = value; }
        }

        public static string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public static string DriverInfo
        {
            get { return _driverInfo; }
            set { _driverInfo = value; }
        }

        public static string DriverVersion
        {
            get { return _driverVersion; }
            set { _driverVersion = value; }
        }

        public static short InterfaceVersion
        {
            get { return _interfaceVersion; }
            set { _interfaceVersion = value; }
        }

		public static bool Connected
		{
			get { return s_bConnected; }
			set { s_bConnected = value; }
		}

        public static void SetupDialog()
        {
            frmSetup form = new frmSetup();
            Initialize();
            form.Show();
 
        }

	    public static float Position
		{
			get { CheckConnected(); lock (s_objSync) { return s_fPosition; } }
		}

		public static float TargetPosition
		{
			get { CheckConnected(); return s_fTargetPosition; }
			set 
			{ 
				CheckConnected();
				lock (s_objSync) 
				{ 
					s_fTargetPosition = value; 
					s_bMoving = true;									// Avoid timing window!(typ.)
				}
			}
		}

		public static bool Reverse
		{
			get { return s_bReverse; }
			set { s_bReverse = value; }
		}

		public static bool Moving
		{
			get { CheckConnected(); lock (s_objSync) { return s_bMoving; } }
		}

		public static float StepSize
		{
			get { return s_fSpeed * s_iUpdateInterval; }
		}

		//
		// Methods for clients
		//
		public static void Move(float relativePosition)
		{
			CheckConnected();
			lock (s_objSync) 
			{
				CheckAngle(s_fTargetPosition + relativePosition);
				s_fTargetPosition += relativePosition; 
				s_bMoving = true;
			}
		}

		public static void MoveAbsolute(float position)
		{
			CheckConnected();
			CheckAngle(position);
			lock (s_objSync) 
			{ 
				s_fTargetPosition = position;
				s_bMoving = true;
			}
		}

		public static void Halt()
		{
			// CheckMoving(true);	// ASCOM-24: Fails Conform, should be harmless.
			lock (s_objSync) 
			{ 
				s_fTargetPosition = s_fPosition;
				s_bMoving = false;
			}
		}
		//
		// Members used by frmMain to run the machine using its timer. This
		// avoids having two timers. Since it has to poll the machinery anyway,
		// it just calls the UpdateState method here just before reading the 
		// state variables. It also sets the update rate for motion calculations
		// here, based on the update rate of its timer.
		//
		public static int UpdateInterval
		{
			set { s_iUpdateInterval = value; }
		}

		public static void UpdateState()
		{
			lock (s_objSync)
			{
				float dPA = RangeAngle(s_fTargetPosition - s_fPosition, -180, 180);
				if (Math.Abs(dPA) == 0)
				{
					s_bMoving = false;
					return;
				}
				//
				// Must move
				//
				float fDelta = s_fSpeed * s_iUpdateInterval;
				if (s_fPosition == 180)												// Inhibit sneaking past 180
				{
					if (s_bDirection && Math.Sign(dPA) > 0)
						dPA = -1;
					else if (!s_bDirection && Math.Sign(dPA) < 0)
						dPA = 1;
				}
				if (dPA > 0 && s_fPosition < 180 && RangeAngle((s_fPosition + dPA), 0, 360) > 180)
					s_fPosition -= fDelta;
				else if (dPA < 0 && s_fPosition > 180 && RangeAngle((s_fPosition + dPA), 0, 360) < 180)
					s_fPosition += fDelta;
				else if (Math.Abs(dPA) >= fDelta)
					s_fPosition += (fDelta * Math.Sign(dPA));
				else
					s_fPosition += dPA;
				s_fPosition = RangeAngle(s_fPosition, 0, 360);
				s_bDirection = Math.Sign(dPA) > 0;									// Remember last direction for 180 check
				s_bMoving = true;
			}
		}

		//
		// Private utilities
		//
		private static void CheckConnected()
		{
			if (!s_bConnected) throw new DriverException("The rotator is not connected", 
								unchecked(ErrorCodes.DriverBase + 2));
		}

		private static void CheckAngle(float angle)
		{
			if (angle < 0.0F || angle >= 360.0F) 
				throw new DriverException("Angle out of range, must be 0 <= angle < 360",
								unchecked(ErrorCodes.DriverBase + 5));
		}

		private static void CheckMoving(bool bAssert)
		{
			CheckConnected();
			lock (s_objSync)
			{
				if (s_bMoving != bAssert)
					throw new DriverException(
						"Illegal - the rotator is " + (s_bMoving ? "" : "not " + "moving"),
						unchecked(ErrorCodes.DriverBase + 3));
			}
		}

		private static float RangeAngle(float angle, float min, float max)
		{
			while(angle >= max) angle -= 360.0F;
			while (angle < min) angle += 360.0F;
			return angle;
		}
	}
}
