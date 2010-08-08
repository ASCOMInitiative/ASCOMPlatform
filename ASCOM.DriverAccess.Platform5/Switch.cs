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
    public class Switch //: ISwitch, IDisposable
    {
        #region Switch constructors

        private readonly MemberFactory _memberFactory;

        /// <summary>
        ///   Creates a Switch object with the given Prog ID
        /// </summary>
        /// <param name = "switchId"></param>
        public Switch(string switchId)
        {
            _memberFactory = new MemberFactory(switchId);
        }

        #endregion

        /// <summary>
        ///   Yields a collection of ISwitchController objects.
        /// </summary>
        public ArrayList SwitchCollection
        {
            get { return (ArrayList) _memberFactory.CallMember(1, "SwitchCollection", new Type[] {}, new object[] {}); }
        }


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
            get { return (string) _memberFactory.CallMember(1, "Description", new[] {typeof (string)}, new object[] {}); }
        }

        /// <summary>
        ///   Returns the driver info
        /// </summary>
        public string DriverInfo
        {
            get { return (string) _memberFactory.CallMember(1, "DriverInfo", new[] {typeof (string)}, new object[] {}); }
        }

        /// <summary>
        ///   Returns the driver version
        /// </summary>
        public string DriverVersion
        {
            get { return (string) _memberFactory.CallMember(1, "DriverVersion", new[] {typeof (string)}, new object[] {}); }
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
            get { return (string) _memberFactory.CallMember(1, "Name", new[] {typeof (string)}, new object[] {}); }
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
    }

    #endregion
}