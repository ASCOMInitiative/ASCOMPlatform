//
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
//
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Interface;
using ASCOM.HelperNET;

namespace ASCOM.DriverAccess
{
    #region SafetyMonitor wrapper
    /// <summary>
    /// Provides universal access to SafetyMonitor drivers
    /// </summary>
    public class SafetyMonitor : ASCOM.Interface.ISafetyMonitor, IDisposable
    {
        object objSafetyMonitorLateBound;
        ASCOM.Interface.ISafetyMonitor ISafetyMonitor;
        Type objTypeSafetyMonitor;

        /// <summary>
        /// Creates a SafetyMonitor object with the given Prog ID
        /// </summary>
        /// <param name="SafetyMonitorID"></param>
        public SafetyMonitor(string SafetyMonitorID)
        {
            // Get Type Information 
            objTypeSafetyMonitor = Type.GetTypeFromProgID(SafetyMonitorID);

            // Create an instance of the SafetyMonitor object
            objSafetyMonitorLateBound = Activator.CreateInstance(objTypeSafetyMonitor);

            // Try to see if this driver has an ASCOM.SafetyMonitor interface
            try
            {
                ISafetyMonitor = (ASCOM.Interface.ISafetyMonitor)objSafetyMonitorLateBound;
            }
            catch (Exception)
            {
                ISafetyMonitor = null;
            }

        }

        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a SafetyMonitor
        /// </summary>
        /// <param name="SafetyMonitorID">SafetyMonitor Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen SafetyMonitor or null for none</returns>
        public static string Choose(string SafetyMonitorID)
        {
            Chooser oChooser = new Chooser();
            oChooser.DeviceType = "SafetyMonitor";			// Requires Helper 5.0.3 (May '07)
            return oChooser.Choose(SafetyMonitorID);
        }

        #region ISafetyMonitor Members

        /// <summary>
        /// Confirms the ability to flag an  emergency shutdown
        /// </summary>
        public bool CanEmergencyShutdown
        {
            get
            {
                if (ISafetyMonitor != null)
                    return ISafetyMonitor.CanEmergencyShutdown;
                else
                    return Convert.ToBoolean(objTypeSafetyMonitor.InvokeMember("CanEmergencyShutdown",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSafetyMonitorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Flags the ability to return usability condition
        /// </summary>
        public bool CanIsGood
        {
            get
            {
                if (ISafetyMonitor != null)
                    return ISafetyMonitor.CanIsGood;
                else
                    return Convert.ToBoolean(objTypeSafetyMonitor.InvokeMember("CanIsGood",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSafetyMonitorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Set True to Connect to the SafetyMonitores; set False to terminate the Connection.
        /// The current Connected status can also be read back as this property.
        /// An exception will be raised if the Connected fails to change state for any reason.
        /// </summary>
        public bool Connected
        {
            get
            {
                if (ISafetyMonitor != null)
                    return ISafetyMonitor.Connected;
                else
                    return Convert.ToBoolean(objTypeSafetyMonitor.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSafetyMonitorLateBound, new object[] { }));
            }
            set
            {
                if (ISafetyMonitor != null)
                    ISafetyMonitor.Connected = value;
                else
                    objTypeSafetyMonitor.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objSafetyMonitorLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Returns a description of the SafetyMonitor controller
        /// </summary>
        public string Description
        {
            get
            {
                if (ISafetyMonitor != null)
                    return ISafetyMonitor.Description;
                else
                    return Convert.ToString(objTypeSafetyMonitor.InvokeMember("Description",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSafetyMonitorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Returns the driver info
        /// </summary>
        public string DriverInfo
        {
            get
            {
                if (ISafetyMonitor != null)
                    return ISafetyMonitor.DriverInfo;
                else
                    return Convert.ToString(objTypeSafetyMonitor.InvokeMember("DriverInfo",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSafetyMonitorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Returns the driver version
        /// </summary>
        public string DriverVersion
        {
            get
            {
                if (ISafetyMonitor != null)
                    return ISafetyMonitor.DriverVersion;
                else
                    return Convert.ToString(objTypeSafetyMonitor.InvokeMember("DriverVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSafetyMonitorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Returns the emergency shutdown state
        /// </summary>
        public bool EmergencyShutdown
        {
            get
            {
                if (ISafetyMonitor != null)
                    return ISafetyMonitor.EmergencyShutdown;
                else
                    return Convert.ToBoolean(objTypeSafetyMonitor.InvokeMember("EmergencyShutdown",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSafetyMonitorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Returns the usability conditions
        /// </summary>
        public bool IsGood
        {
            get
            {
                if (ISafetyMonitor != null)
                    return ISafetyMonitor.IsGood;
                else
                    return Convert.ToBoolean(objTypeSafetyMonitor.InvokeMember("IsGood",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSafetyMonitorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Returns the safety conditions
        /// </summary>
        public bool IsSafe
        {
            get
            {
                if (ISafetyMonitor != null)
                    return ISafetyMonitor.IsSafe;
                else
                    return Convert.ToBoolean(objTypeSafetyMonitor.InvokeMember("IsSafe",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSafetyMonitorLateBound, new object[] { }));
            }
        }

 
        ///<summary>
        ///Launches a configuration dialog box for the driver.  The call will not return
        ///until the user clicks OK or cancel manually.
        ///</summary>
        ///<exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            if (ISafetyMonitor != null)
                ISafetyMonitor.SetupDialog();
            else
                objTypeSafetyMonitor.InvokeMember("SetupDialog",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objSafetyMonitorLateBound, new object[] { });
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        public void Dispose()
        {
            if (this.objSafetyMonitorLateBound != null)
            {
                try { Marshal.ReleaseComObject(objSafetyMonitorLateBound); }
                catch (Exception) { }
                objSafetyMonitorLateBound = null;
            }
        }

        #endregion
    }
    #endregion
}
