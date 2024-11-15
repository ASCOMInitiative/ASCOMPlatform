using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ASCOM.Utilities.Exceptions;
using ASCOM.Utilities.Interfaces;
using Microsoft.VisualBasic;
using static ASCOM.Utilities.Global;

namespace ASCOM.Utilities
{

    /// <summary>
    /// The Chooser object provides a way for your application to let your user select the telescope to use.
    /// </summary>
    /// <remarks>
    /// <para>Calling Chooser.Choose() causes a chooser window to appear, with a drop down selector list containing all of the registered Telescope 
    /// drivers, listed by the driver's friendly/display name. The user sees a list of telescope types and selects one. 
    /// Before the OK button will light up, however, the user must click the Setup button, causing the selected driver's setup dialog to appear 
    /// (it calls the driver's Telescope.SetupDialog() method). When the setup dialog is closed, the OK button will light and allow the user 
    /// to close the Chooser itself.</para>
    /// 
    /// <para>The Choose() method returns the DriverID of the selected driver. Choose() allows you to optionally pass the DriverID of a "current" 
    /// driver (you probably save this in the registry), and the corresponding telescope type is pre-selected in the Chooser's list. In this case, 
    /// the OK button starts out enabled (lit-up); the assumption is that the pre-selected driver has already been configured. </para>
    /// </remarks>
    [Guid("B7A1F5A0-71B4-44f9-91E9-468697957D6B")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Chooser : IChooser, IChooserExtra, IDisposable
    {
        // ===========
        // CHOOSER.CLS
        // ===========
        // 
        // Implementation of the ASCOM telescope driver Chooser class
        // 
        // Written:  24-Aug-00   Robert B. Denny <rdenny@dc3.com>
        // 
        // Edits:
        // 
        // When      Who     What
        // --------- ---     --------------------------------------------------
        // 25-Feb-09 pwgs     5.1.0 - Refactored for Utilities
        // ---------------------------------------------------------------------

        private string deviceTypeValue = "";

        #region  New and IDisposable Support 

        private bool disposedValue = false;        // To detect redundant calls

        /// <summary>
        /// Creates a new Chooser object
        /// </summary>
        /// <remarks></remarks>
        public Chooser() : base()
        {

            deviceTypeValue = "Telescope"; // Default to Telescope chooser
        }



        // IDisposable
        /// <summary>
        /// Does the work of cleaning up objects used by Chooser
        /// </summary>
        /// <param name="disposing">True if called by the user, false if called by the system</param>
        /// <remarks>You can't call this directly, use Dispose with no arguments instead.</remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

            }
            disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        /// <summary>
        /// Cleans up and disposes objects used by Chooser
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            // Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalise the Chooser
        /// </summary>
        ~Chooser()
        {
            // Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }

        #endregion

        #region IChooser Implementation
        /// <summary>
        /// The type of device for which the Chooser will select a driver. (String, default = "Telescope")
        /// </summary>
        /// <value>The type of device for which the Chooser will select a driver. (String, default = "Telescope") 
        /// </value>
        /// <returns>The device type that has been set</returns>
        /// <exception cref="Exceptions.InvalidValueException">Thrown on setting the device type to empty string</exception>
        /// <remarks>This property changes the "personality" of the Chooser, allowing it to be used to select a driver for any arbitrary 
        /// ASCOM device type. The default value for this is "Telescope", but it could be "Focuser", "Camera", etc. 
        /// <para>This property is independent of the Profile object's DeviceType property. Setting Chooser's DeviceType 
        /// property doesn't set the DeviceType property in Profile, you must set that also when needed.</para>
        /// </remarks>
        public string DeviceType
        {
            get
            {
                return deviceTypeValue;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exceptions.InvalidValueException("Chooser:DeviceType - " + VB6COMErrors.MSG_ILLEGAL_DEVTYPE); // Err.Raise(SCODE_ILLEGAL_DEVTYPE, ERR_SOURCE_PROFILE, MSG_ILLEGAL_DEVTYPE)

                deviceTypeValue = value;
            }
        }

        /// <summary>
        /// Select ASCOM driver to use including pre-selecting one in the drop-down list
        /// </summary>
        /// <param name="DriverProgID">Driver to preselect in the chooser dialogue</param>
        /// <returns>Driver ID of chosen driver</returns>
        /// <exception cref="Exceptions.InvalidValueException">Thrown if the Chooser.DeviceType property has not been set before Choose is called. 
        /// It must be set in order for Chooser to know which list of devices to display.</exception>
        /// <remarks>The supplied driver will be pre-selected in the Chooser's list when the chooser window is first opened.
        /// </remarks>
        public string Choose(string DriverProgID)
        {
            string selectedProgId;
            ChooserForm chooserFormInstance;

            try
            {

                if (string.IsNullOrEmpty(deviceTypeValue))
                    throw new Exceptions.InvalidValueException("Unknown device type, DeviceType property has not been set");
                chooserFormInstance = new ChooserForm(); // Initially hidden

                chooserFormInstance.DeviceType = deviceTypeValue;
                chooserFormInstance.SelectedProgId = DriverProgID;
                chooserFormInstance.ShowDialog(); // Display MODAL Chooser dialogue

                selectedProgId = chooserFormInstance.SelectedProgId;

                chooserFormInstance.Dispose();
            }

            catch (DriverNotRegisteredException ex)
            {
                Interaction.MsgBox("Chooser Exception: " + ex.Message);
                LogEvent("Chooser", "Exception", EventLogEntryType.Error, EventLogErrors.ChooserException, ex.ToString());
                selectedProgId = "";
            }

            catch (Exception ex)
            {
                Interaction.MsgBox("Chooser Exception: " + ex.ToString());
                LogEvent("Chooser", "Exception", EventLogEntryType.Error, EventLogErrors.ChooserException, ex.ToString());
                selectedProgId = "";
            }

            return selectedProgId;
        }
        #endregion

        #region IChooserExtra Implementation
        /// <summary>
        /// Select ASCOM driver to use without pre-selecting in the dropdown list
        /// </summary>
        /// <returns>Driver ID of chosen driver</returns>
        /// <exception cref="Exceptions.InvalidValueException">Thrown if the Chooser.DeviceType property has not been set before Choose is called. 
        /// It must be set in order for Chooser to know which list of devices to display.</exception>
        /// <remarks>No driver will be pre-selected in the Chooser's list when the chooser window is first opened. 
        /// <para>This overload is not available through COM, please use "Choose(ByVal DriverProgID As String)"
        /// with an empty string parameter to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string Choose()
        {
            return Choose("");
        }
        #endregion

    }
}