using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ASCOM;
using ASCOM.Helper;

namespace ASCOM.Simulator
{
	//
	// This implements the simulated hardware layer.
	//
	public class RotatorHardware
	{
		private static Helper.Profile Profile;
		private static string s_sProgID = "ASCOM.Simulator.Rotator";

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

		//
		// Sync object
		//
		private static object s_objSync = new object();	// Better than lock(this) - Jeffrey Richter, MSDN Jan 2003

		//
		// Constructor - initialize state
		//
		static RotatorHardware()
		{
			Profile = new Helper.Profile();
			Profile.DeviceTypeV = "Rotator";
			s_fPosition = 0.0F;
			s_bConnected = false;
			s_bMoving = false;
			s_fTargetPosition = 0.0F;
		}

		//
		// Settings support
		//
		private static string GetSetting(string Name, string DefValue)
		{
			string s = Profile.GetValue(s_sProgID, Name, "");
			if (s == "") s = DefValue;
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
		public static bool Connected
		{
			get { return s_bConnected; }
			set { s_bConnected = value; }
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
		public static void Move(float RelativePosition)
		{
			CheckConnected(); 
			lock (s_objSync) 
			{ 
				s_fTargetPosition += RelativePosition; 
				s_bMoving = true;
			}
		}

		public static void MoveAbsolute(float Position)
		{
			CheckConnected(); 
			lock (s_objSync) 
			{ 
				s_fTargetPosition = Position;
				s_bMoving = true;
			}
		}

		public static void Halt()
		{
			CheckMoving(true);
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
			if (!s_bConnected) throw new ASCOM.DriverException("The rotator is not connected", 
								unchecked(ASCOM.ErrorCodes.DriverBase + 2));
		}

		private static void CheckMoving(bool bAssert)
		{
			CheckConnected();
			lock (s_objSync)
			{
				if (s_bMoving != bAssert)
					throw new ASCOM.DriverException(
						"Illegal - the rotator is " + (s_bMoving ? "" : "not " + "moving"),
						unchecked(ASCOM.ErrorCodes.DriverBase + 3));
			}
		}

		private static float RangeAngle(float Angle, float Min, float Max)
		{
			while(Angle >= Max) Angle -= 360.0F;
			while (Angle < Min) Angle += 360.0F;
			return Angle;
		}
	}
}
