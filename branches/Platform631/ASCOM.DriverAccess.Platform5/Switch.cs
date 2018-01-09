//-----------------------------------------------------------------------
// <summary>Defines the Switch class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using System.Collections;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{

    #region Switch wrapper

    /// <summary>
    ///   Provides universal access to Switch drivers
    /// </summary>
    public class Switch: ASCOM.Interface.ISwitch, IDisposable
    {
        private TraceLogger TL;
        #region Switch constructors

        private readonly MemberFactory _memberFactory;

        /// <summary>
        ///   Creates a Switch object with the given Prog ID
        /// </summary>
        /// <param name = "switchId"></param>
        public Switch(string switchId)
        {
            TL = new TraceLogger("", "DriverAccessSwitch");
            TL.Enabled = RegistryCommonCode.GetBool(GlobalConstants.DRIVERACCESS_TRACE, GlobalConstants.DRIVERACCESS_TRACE_DEFAULT);
            _memberFactory = new MemberFactory(switchId, TL);
        }

        #endregion

        #region Common Methods
        /// <summary>
        ///   Set True to Connect to the switches; set False to terminate the Connection.
        ///   The current Connected status can also be read back as this property.
        ///   An exception will be raised if the Connected fails to change state for any reason.
        /// </summary>
        public bool Connected
        {
            get { return (bool) _memberFactory.CallMember(1, "Connected", new Type[] {}, new object[] {}); }
            set { _memberFactory.CallMember(2, "Connected", new Type[] {}, new object[] {value}); }
        }

        /// <summary>
        ///   Returns a description of the Switch controller
        /// </summary>
        public string Description
        {
            get { return (string) _memberFactory.CallMember(1, "Description", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        ///   Returns the driver info
        /// </summary>
        public string DriverInfo
        {
            get { return (string) _memberFactory.CallMember(1, "DriverInfo", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        ///   Returns the driver version
        /// </summary>
        public string DriverVersion
        {
            get { return (string) _memberFactory.CallMember(1, "DriverVersion", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        ///   Returns the Switch Interface version
        /// </summary>
        public short InterfaceVersion
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "InterfaceVersion", new Type[] {}, new object[] {})); }
        }

        /// <summary>
        ///   returns the name of the switch interface
        /// </summary>
        public string Name
        {
            get { return (string) _memberFactory.CallMember(1, "Name", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        ///   Dispose the late-bound interface, if needed. Will release it via COM
        ///   if it is a COM object, else if native .NET will just dereference it
        ///   for GC.
        /// </summary>
        public void Dispose()
        {
            _memberFactory.Dispose();
        }

        ///<summary>
        ///  Launches a configuration dialog box for the driver.  The call will not return
        ///  until the user clicks OK or cancel manually.
        ///</summary>
        ///<exception cref = " System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            _memberFactory.CallMember(3, "SetupDialog", new Type[] {}, new object[] {});
        }

        /// <summary>
        ///   Brings up the ASCOM Chooser Dialog to choose a Switch
        /// </summary>
        /// <param name = "switchId">Switch Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen Switch or null for none</returns>
        public static string Choose(string switchId)
        {
            var oChooser = new Chooser {DeviceType = "Switch"};
            return oChooser.Choose(switchId);
        }
        #endregion

        #region Device Methods
        /// <summary>
        /// Return the state of switch n
        /// </summary>
        /// <param name="ID">The switch number to return</param>
        /// <returns>True or false</returns>
        public bool GetSwitch(short ID) 
        {
            return (bool)_memberFactory.CallMember(3, "GetSwitch", new Type[] { typeof(short) }, new object[] { ID });
        }

        /// <summary>
        /// Return the name of switch n
        /// </summary>
        /// <param name="ID">The switch number to return</param>
        /// <returns>The name of the switch</returns>
        public string GetSwitchName(short ID)
        {
            return (string)_memberFactory.CallMember(3, "GetSwitchName", new Type[] { typeof(short) }, new object[] { ID });
        }

        /// <summary>
        /// The number of switches managed by this driver
        /// </summary>
        public short MaxSwitch
        {
            get { return (short)_memberFactory.CallMember(1, "MaxSwitch", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Sets a switch to the specified state
        /// </summary>
        /// <param name="ID">The number of the switch to set</param>
        /// <param name="State">The required switch state</param>
        public void SetSwitch(short ID, bool State)
        {
            _memberFactory.CallMember(3, "SetSwitch", new Type[] { typeof(short), typeof(bool) }, new object[] { ID, State });
        }

        /// <summary>
        /// Sets a switch name to a specified value
        /// </summary>
        /// <param name="ID">The number of the switch whose name is to be set</param>
        /// <param name="State">The name of the switch</param>
        public void SetSwitchName(short ID, string State)
        {
            _memberFactory.CallMember(3, "SetSwitchName", new Type[] { typeof(short), typeof(string) }, new object[] { ID, State });
        }
    }
        #endregion

    #endregion
}