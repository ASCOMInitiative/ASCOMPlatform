using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Utilities;
using ASCOM.Interface;
using System.Collections;
using System.Diagnostics;

namespace ASCOM.SwitchSimulator
{
	//
    // Your driver's DeviceID is ASCOM.SwitchSimulator.Switch
	//
    // The Guid attribute sets the CLSID for ASCOM.SwitchSimulator.Switch
	// The ClassInterface/None addribute prevents an empty interface called
	// _Conceptual from being created and used as the [default] interface
	//

	/// <summary>
	/// ASCOM Switch Driver for a conceptual switch (proof of concept).
	/// This class is the implementation of the public ASCOM interface.
	/// </summary>
    [Guid("dc17f874-056a-41c2-b7aa-7cd6df8ecf63")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Switch : ISwitch
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        private static string s_csDriverID = "ASCOM.SwitchSimulator.Switch";

        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string s_csDriverDescription = "SwitchSimulator ASCOM Switch Driver.";

        /// <summary>
        /// The number of physical switches that this device has.
        /// </summary>
        private const int numSwitches = 8;

        /// <summary>
        /// Backing store for the private switch collection.
        /// </summary>
        private ArrayList switchCollection = new ArrayList(numSwitches);

        /// <summary>
        /// Initializes a new instance of the <see cref="Conceptual"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Switch()
        {
            for (int i = 0; i < numSwitches; i++)
            {
                switchCollection.Add(new SwitchDevice("White Lights"));
                switchCollection.Add(new SwitchDevice("Red Lights"));
                switchCollection.Add(new SwitchDevice("Telescope Power"));
                switchCollection.Add(new SwitchDevice("Camera Power"));
                switchCollection.Add(new SwitchDevice("Focuser Power"));
                switchCollection.Add(new SwitchDevice("Dew Heaters"));
                switchCollection.Add(new SwitchDevice("Dome Power"));
                switchCollection.Add(new SwitchDevice("Self Destruct"));
            }
        }

        #region ASCOM Registration
        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            var P = new ASCOM.Utilities.Profile();
            P.DeviceType = "ASCOM.SwitchSimulator.Switch";
            if (bRegister)
            {
                P.Register(s_csDriverID, s_csDriverDescription);
            }
            else
            {
                P.Unregister(s_csDriverID);
            }
            // Utilities.Profile is native .NET so no COM interfaces are involved.
            //try										// In case Helper becomes native .NET
            //{
            //    Marshal.ReleaseComObject(P);
            //}
            //catch (Exception) { }
            P = null;
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            Trace.WriteLine("Registering -> {0} with ASCOM Profile", s_csDriverID);
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            Trace.WriteLine("Unregistering -> {0} with ASCOM Profile", s_csDriverID);
            RegUnregASCOM(false);
        }
        #endregion

        //
        // PUBLIC COM INTERFACE ISwitch IMPLEMENTATION
        //

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// </summary>
        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            var result = F.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.Save();
                return;
            }
            Properties.Settings.Default.Reload();
        }

        #region ISwitch Members
        // I haven't implemented all these methods because they aren't needed to demonstrate the concept.

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Switch"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets the driver info.
        /// </summary>
        /// <value>The driver info.</value>
        public string DriverInfo
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets the driver version.
        /// </summary>
        /// <value>The driver version.</value>
        public string DriverVersion
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets the interface version.
        /// </summary>
        /// <value>The interface version.</value>
        public short InterfaceVersion
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Yields a collection of ISwitchController objects.
        /// </summary>
        /// <value></value>
        public System.Collections.ArrayList SwitchCollection
        {
            get
            {
                return switchCollection;
            }
        }

        #endregion
    }
}
