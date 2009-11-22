//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for MultiInstanceSimulator
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Focuser Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Interface;
using ASCOM.HelperNet;

namespace ASCOM.MultiInstanceSimulator
{
    //
    // Your driver's ID is ASCOM.MultiInstanceSimulator.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.MultiInstanceSimulator.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Focuser from being created and used as the [default] interface
    //
    [Guid("86719815-b766-41e4-ab40-7cb66e3f4059")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuser
    {
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        private static string s_csDriverID = "ASCOM.MultiInstanceSimulator.Focuser";
        // TODO Change the descriptive string for your driver then remove this line
        private static string s_csDriverDescription = "MultiInstanceSimulator Focuser";

        internal string driverConfiguration;

        internal bool isMoving;

        internal bool linked;
        internal bool isAbsolute;
        internal int maxIncrement;
        internal int maxStep;
        internal int position;
        internal int target;
        internal double stepSize;
        internal int roc = 100;
        internal bool tempComp;
        internal bool tempCompAvailable;
        internal double temperature;

        private System.Timers.Timer moveTimer;

        //
        // Constructor - Must be public for COM registration!
        //
        public Focuser() :
            this(s_csDriverID)
        {
        }

        #region Multi Instance Setup

        /// <summary>
        /// Multi instance constructor, driver configuration is provided
        /// </summary>
        /// <param name="driverConfiguration"></param>
        public Focuser(string driverConfiguration)
        {
            this.driverConfiguration = driverConfiguration;
            // TODO Implement your additional construction here
            moveTimer = new System.Timers.Timer(100);
            moveTimer.Interval = 100;
            moveTimer.Enabled = false;
            moveTimer.AutoReset = true;
            moveTimer.Elapsed += new System.Timers.ElapsedEventHandler(moveTimer_Elapsed);
            LoadProfile();
        }

        public string DriverConfiguration
        {
            set
            {
                if (this.linked)
                {
                    throw new ASCOM.DriverException("Can't set the driver configuration while connected");
                }
                this.driverConfiguration = value;
                LoadProfile();
            }
            get { return this.driverConfiguration; }
        }

        #endregion

        void moveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (target == position)
            {
                isMoving = false;
                moveTimer.Enabled = false;
            }
            else if (target > position + roc)
            {
                position += roc;
            }
            else if (target < position - roc)
            {
                position -= roc;
            }
            else
            {
                position = target;
            }
        }

        #region ASCOM Registration
        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
        {
            Profile P = new Profile();
            P.DeviceType = "Focuser";					//  Requires Helper 5.0.3 or later
            if (bRegister)
                P.Register(s_csDriverID, s_csDriverDescription);
            else
                P.Unregister(s_csDriverID);
            try										// In case Helper becomes native .NET
            {
                Marshal.ReleaseComObject(P);
            }
            catch (Exception) { }
            P = null;
        }

        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }
        #endregion

        //
        // PUBLIC COM INTERFACE IFocuser IMPLEMENTATION
        //

        #region IFocuser Members

        public bool Absolute
        {
            get { return isAbsolute; }
        }

        public void Halt()
        {
            isMoving = false;
            moveTimer.Enabled = false;
            target = position;
        }

        public bool IsMoving
        {
            get { return isMoving; }
        }

        public bool Link
        {
            get { return linked; }
            set { linked = value; }
        }

        public int MaxIncrement
        {
            get { return maxIncrement; }
        }

        public int MaxStep
        {
            get { return maxStep; }
        }

        public void Move(int val)
        {
            if (tempComp)
                throw new ASCOM.InvalidOperationException("Move not allowed when temperature compensation is on");

            if (isAbsolute)
            {
                target = Math.Min(Math.Max(0, val), maxStep);
            }
            else
            {
                position = 0;
                target = Math.Min(Math.Max(-maxIncrement, val), maxIncrement);
            }
            isMoving = true;
            moveTimer.Enabled = true;
        }

        public int Position
        {
            get
            {
                if (!isAbsolute)
                    throw new ASCOM.InvalidOperationException("It's a relative focuser, we don't know where it is");
                return this.position; 
            }
        }

        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm(this);
            F.ShowDialog();
            SaveProfile();
        }

        public double StepSize
        {
            get
            {
                if (stepSize == 0)
                    throw new ASCOM.InvalidOperationException("Step Size is not known");
                return stepSize; 
            }
        }

        public bool TempComp
        {
            get 
            {
                if (!tempCompAvailable)
                    return false;
                return tempComp; 
            }
            set 
            {
                if (!tempCompAvailable && value == true)
                    throw new ASCOM.InvalidOperationException("Can't set temp comp is it's not available");
                tempComp = value; 
            }
        }

        public bool TempCompAvailable
        {
            get { return TempCompAvailable; }
        }

        public double Temperature
        {
            get 
            {
                if (!this.tempCompAvailable)
                {
                    throw new ASCOM.InvalidOperationException("Temperature is not available");
                }
                return temperature;
            }
        }

        #endregion

        private void LoadProfile()
        {
            FocusProfile fp = new FocusProfile(driverConfiguration);
            this.isAbsolute = fp.GetValue("IsAbsolute", true);
            this.maxIncrement = fp.GetValue("MaxIncrement", 65000);
            this.maxStep = fp.GetValue("MaxStep", 65000);
            this.position = fp.GetValue("Position", 32000);
            this.stepSize = fp.GetValue("StepSize", 0.0);
            this.tempCompAvailable = fp.GetValue("TempCompAvailable", false);
        }

        private void SaveProfile()
        {
            FocusProfile fp = new FocusProfile(driverConfiguration);
            fp.SetValue("IsAbsolute", this.isAbsolute);
            fp.SetValue("MaxIncrement", this.maxIncrement);
            fp.SetValue("MaxStep", this.maxStep);
            fp.SetValue("Position", this.position);
            fp.SetValue("StepSize", this.stepSize);
            fp.SetValue("TempCompAvailable", this.tempCompAvailable);
        }

    }


    internal class FocusProfile
    {
        private Profile profile = new Profile();
        private string id;

        internal FocusProfile(string id)
        {
            profile = new Profile();
            profile.DeviceType= "Focuser";
            this.id = id;
        }

        internal string GetValue(string name, string def)
        {
            string buf = profile.GetValue(id, name, "");
            if (buf.Length == 0)
            {
                return def;
            }
            return buf.Trim();
        }

        internal int GetValue(string name, int def)
        {
            string buf = profile.GetValue(id, name, "");
            if (buf.Length == 0)
            {
                return def;
            }
            return Convert.ToInt32(buf.Trim());
        }
        internal void SetValue(string name, int value)
        {
            profile.WriteValue(id, name, value.ToString(), "");
        }

        internal bool GetValue(string name, bool def)
        {
            string buf = profile.GetValue(id, name, "");
            if (buf.Length == 0)
                return def;
            return Convert.ToBoolean(buf.Trim());
        }
        internal void SetValue(string name, bool value)
        {
            profile.WriteValue(id, name, value.ToString(), "");
        }

        internal double GetValue(string name, double def)
        {
            string buf = profile.GetValue(id, name, "");
            if (buf.Length == 0)
                return def;
            return Convert.ToDouble(buf.Trim());
        }
        internal void SetValue(string name, double value)
        {
            profile.WriteValue(id, name, value.ToString(), "");
        }
    }

}
