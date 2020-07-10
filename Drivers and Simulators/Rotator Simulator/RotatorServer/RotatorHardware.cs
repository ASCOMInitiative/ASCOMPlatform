//
// 02-Oct-09	Bob Denny	ASCOM-24 : Fix conform error, Halt() when not moving is harmless.
//							Throw error if angles outside 0 <= angle < 360 
//							(this is a reference implementation!)
//
using System;
using ASCOM.Utilities;
using System.Globalization;

namespace ASCOM.Simulator
{
    //
    // This implements the simulated hardware layer.
    //
    public static class RotatorHardware
    {

        // IRotatorV3 introduces the concept of synced position in addition to the rotator's mechanical position
        // To convert a mechanical position to a synced position the relationship is: SyncPosition = MechanicalPosition + SyncOffset
        // To convert a synced position to a mechanical position the relationship is: MechanicalPosition = SyncPosition - SyncOffset
        // The units of the mechanicalPosition and syncOffset variables are degrees from 0 to 359.9999...
        // The simulator state machine uses the "position" and "syncOffset" variables to hold the mechanical position and offset to the synced position respectively
        // The targetMechanicalPosition variable works in mechanical coordinates
        // The driver's Position and InstrumentalPosition property values are calculated on demand allowing for or omitting the sync offset as determined by the value of the "canSync" variable

        // Settings, persistent
        private static float mechanicalPosition; // Degrees - 0 to 359.99999...
        private static float rotationSpeed; // Degrees per millisecond - Rotation speed
        private static bool canReverse;
        private static bool reverse;

        // IRotatorV3 variables
        private static float syncOffset; // Degrees - Offset between the mechanical instrumental position and the synced position 

        // State variables
        private static bool connected;
        private static bool isMoving;
        private static bool direction;
        private static float targetMechanicalPosition; // Degrees - Destination rotator angle to which the rotator should move Sky position if synced, mechanical position if not synced
        private static int updateInterval = 250; // Milliseconds, default, set by main form
        private static string rotatorName = "ASCOM.Simulator.Rotator";
        private static string description = "ASCOM Rotator Driver for RotatorSimulator";
        private static string driverInfo = "ASCOM.Simulator.Rotator";
        private static string driverVersion = "6.5";
        private static short interfaceVersion = 3;
        private static string progID = "ASCOM.Simulator.Rotator";

        // Sync object
        private static object syncLockObject = new object(); // Better than lock(this) - Jeffrey Richter, MSDN Jan 2003

        // Constructor - initialize state
        static RotatorHardware()
        {
            mechanicalPosition = 0.0F;
            connected = false;
            isMoving = false;
            targetMechanicalPosition = 0.0F;
        }

        // Read configuration from the Profile
        public static void ReadConfiguration()
        {
            using (var profile = new Profile())
            {
                profile.DeviceType = "Rotator";
                mechanicalPosition = Convert.ToSingle(profile.GetValue(progID, "Position", "", "0.0"), CultureInfo.InvariantCulture);
                RotationRate = Convert.ToSingle(profile.GetValue(progID, "RotationRate", "", "3.0"), CultureInfo.InvariantCulture);
                canReverse = Convert.ToBoolean(profile.GetValue(progID, "CanReverse", "", bool.TrueString));
                reverse = Convert.ToBoolean(profile.GetValue(progID, "Reverse", "", bool.FalseString));

                syncOffset = Convert.ToSingle(profile.GetValue(progID, "SyncOffset", "", "0.0"), CultureInfo.InvariantCulture);
                targetMechanicalPosition = RangeAngle(mechanicalPosition + syncOffset, 0.0F, 360.0F); ; // Initialise the target position to the current synced position
            }
        }

        // Write configuration to the Profile
        public static void WriteConfiguration()
        {
            using (var profile = new Profile())
            {
                profile.DeviceType = "Rotator";
                profile.WriteValue(progID, "Position", mechanicalPosition.ToString(CultureInfo.InvariantCulture), "");
                profile.WriteValue(progID, "RotationRate", RotationRate.ToString(CultureInfo.InvariantCulture), "");
                profile.WriteValue(progID, "CanReverse", canReverse.ToString(), "");
                profile.WriteValue(progID, "Reverse", reverse.ToString(), "");

                profile.WriteValue(progID, "SyncOffset", syncOffset.ToString(CultureInfo.InvariantCulture), "");
            }
        }

        // Properties for setup dialogue
        public static float RotationRate
        {
            get { return rotationSpeed * 1000; } // Internally deg/millisecond
            set { rotationSpeed = value / 1000; }
        }

        public static float SyncOffset
        {
            get
            {
                return syncOffset;
            }
            set { syncOffset = value; }
        }

        public static bool CanReverse
        {
            get { return canReverse; }
            set
            {
                canReverse = value;
                if (!value) reverse = false;
            }
        }

        // State properties for clients
        public static string RotatorName
        {
            get { return rotatorName; }
            set { rotatorName = value; }
        }

        public static string Description
        {
            get { return description; }
            set { description = value; }
        }

        public static string DriverInfo
        {
            get { return driverInfo; }
            set { driverInfo = value; }
        }

        public static string DriverVersion
        {
            get { return driverVersion; }
            set { driverVersion = value; }
        }

        public static short InterfaceVersion
        {
            get { return interfaceVersion; }
            set { interfaceVersion = value; }
        }

        public static bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }

        public static void SetupDialog()
        {
            ReadConfiguration();
            using (frmSetup form = new frmSetup())
            {
                form.RotationRate = RotationRate;
                form.CanReverse = CanReverse;
                form.Reverse = Reverse;
                form.UpdateInterval = updateInterval;
                form.SyncOffset = syncOffset;
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Reverse = form.Reverse;
                    CanReverse = form.CanReverse;
                    RotationRate = form.RotationRate;
                    syncOffset = form.SyncOffset;
                    //syncOffset = RangeAngle(syncOffset, 0.0F, 360.0F);
                    WriteConfiguration(); // Added to force persistence after values are changed in the setup dialogue
                }
            }
        }

        public static float Position
        {
            get
            {
                CheckConnected();
                lock (syncLockObject)
                {
                    return RangeAngle(mechanicalPosition + syncOffset, 0.0F, 360.0F);
                }
            }
        }

        public static float TargetPosition
        {
            get
            {
                CheckConnected();
                lock (syncLockObject)
                {
                    return RangeAngle(targetMechanicalPosition + syncOffset, 0.0F, 360.0F);
                }
            }
        }

        public static bool Reverse
        {
            get { return reverse; }
            set { reverse = value; }
        }

        public static bool IsMoving
        {
            get { CheckConnected(); lock (syncLockObject) { return isMoving; } }
        }

        public static float StepSize
        {
            get { return rotationSpeed * updateInterval; }
        }

        // IRotatorV2 Methods for clients
        public static void Move(float relativePosition)
        {
            CheckConnected();
            lock (syncLockObject)
            {
                // add check for relative position limits rather than using the check on the target.
                if (relativePosition <= -360.0 || relativePosition >= 360.0)
                {
                    throw new ASCOM.InvalidValueException("Relative Angle out of range", relativePosition.ToString(), "-360 < angle < 360");
                }
                var target = targetMechanicalPosition + relativePosition;
                // force to the range 0 to 360
                if (target >= 360.0) target -= 360.0F;
                if (target < 0.0) target += 360.0F;
                targetMechanicalPosition = target;
                isMoving = true;
            }
        }

        public static void MoveAbsolute(float position)
        {
            CheckConnected();
            CheckAngle(position);
            lock (syncLockObject)
            {
                targetMechanicalPosition = RangeAngle(position - syncOffset, 0.0F, 360.0F); // Calculate the mechanical rotator angle from the supplied sky position
                isMoving = true;
            }
        }

        public static void Halt()
        {
            // CheckMoving(true);	// ASCOM-24: Fails Conform, should be harmless.
            lock (syncLockObject)
            {
                targetMechanicalPosition = mechanicalPosition;
                isMoving = false;
            }
        }

        // IRotatorV3 methods for clients

        public static float InstrumentalPosition
        {
            get
            {
                CheckConnected();

                lock (syncLockObject)
                {
                    return mechanicalPosition;
                }
            }
        }

        public static void Sync(float syncPosition)
        {
            CheckConnected();

            CheckAngle(syncPosition);
            lock (syncLockObject)
            {
                // Calculate the new sync offset
                syncOffset = syncPosition - mechanicalPosition;
            }
        }

        public static void MoveMechanical(float position)
        {
            CheckConnected();
            CheckAngle(position);
            lock (syncLockObject)
            {
                lock (syncLockObject)
                {
                    targetMechanicalPosition = position; // Calculate the mechanical rotator angle from the supplied sky position
                    isMoving = true;
                }
            }
        }

        //
        // Members used by frmMain to run the machine using its timer. This avoids having two timers. Since it has to poll the machinery anyway, it just calls the UpdateState method here just 
        // before reading the state variables. It also sets the update rate for motion calculations here, based on the update rate of its timer.
        //
        public static int UpdateInterval
        {
            set { updateInterval = value; }
        }

        public static void UpdateState()
        {
            lock (syncLockObject)
            {
                float dPA = RangeAngle(targetMechanicalPosition - mechanicalPosition, -180, 180);
                if (Math.Abs(dPA) == 0)
                {
                    isMoving = false;
                    return;
                }
                //
                // Must move
                //
                float fDelta = rotationSpeed * updateInterval;
                if (mechanicalPosition == 180)                                             // Inhibit sneaking past 180
                {
                    if (direction && Math.Sign(dPA) > 0)
                        dPA = -1;
                    else if (!direction && Math.Sign(dPA) < 0)
                        dPA = 1;
                }
                if (dPA > 0 && mechanicalPosition < 180 && RangeAngle((mechanicalPosition + dPA), 0, 360) > 180)
                    mechanicalPosition -= fDelta;
                else if (dPA < 0 && mechanicalPosition > 180 && RangeAngle((mechanicalPosition + dPA), 0, 360) < 180)
                    mechanicalPosition += fDelta;
                else if (Math.Abs(dPA) >= fDelta)
                    mechanicalPosition += (fDelta * Math.Sign(dPA));
                else
                    mechanicalPosition += dPA;
                mechanicalPosition = RangeAngle(mechanicalPosition, 0, 360);
                direction = Math.Sign(dPA) > 0;                                  // Remember last direction for 180 check
                isMoving = true;
            }
        }

        //
        // Private utilities
        //
        private static void CheckConnected()
        {
            if (!connected) throw new NotConnectedException("The rotator is not connected");
        }

        private static void CheckAngle(float angle)
        {
            if (angle < 0.0F || angle >= 360.0F)
                throw new ASCOM.InvalidValueException("Angle out of range", angle.ToString(), "0 <= angle < 360");
        }

        private static float RangeAngle(float angle, float min, float max)
        {
            while (angle >= max) angle -= 360.0F;
            while (angle < min) angle += 360.0F;
            return angle;
        }

    }
}