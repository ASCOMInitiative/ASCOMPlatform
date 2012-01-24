using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Conform;
using ASCOM.Utilities;
using System.Collections;
using System.Globalization;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Base class for ASCOM driver access toolkit device classes. This class contains the methods common to all devices
    /// so that they can be maintained in just one place.
    /// </summary>
    public class AscomDriver : IDisposable
    {
        internal TraceLogger TL;
        private int interfaceVersion;
        MemberFactory memberFactory;
        private bool disposedValue = false;        // To detect redundant calls
        private string deviceType;

        #region AscomDriver Constructors and Dispose
        /// <summary>
        /// Creates a new instance of the <see cref="AscomDriver"/> class.
        /// </summary>
        /// <param name="deviceProgId">The prog id. of the device being created.</param>
        public AscomDriver(string deviceProgId)
        {
            // Create a new TraceLogger and enable if appropriate
            TL = new TraceLogger("", "DriverAccess");
            TL.Enabled = RegistryCommonCode.GetBool(GlobalConstants.DRIVERACCESS_TRACE, GlobalConstants.DRIVERACCESS_TRACE_DEFAULT);
            TL.LogMessage("AscomDriver", "Successfully created TraceLogger");
            TL.LogMessage("AscomDriver", "Device ProgID: " + deviceProgId);

            //deviceType = deviceProgId.Substring(deviceProgId.LastIndexOf(".") + 1).ToUpper();
            deviceType = this.GetType().Name.ToUpper();
            TL.LogMessage("AscomDriver", "Device type: " + this.GetType().Name);

            memberFactory = new MemberFactory(deviceProgId, TL); // Create a memberfactory object and pass in the TraceLogger

            interfaceVersion = this.InterfaceVersion;
        }

        /// <summary>
        /// Releases the unmanaged late bound COM object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of managed and unmanged resources
        /// </summary>
        /// <param name="disposing">True to dispose of managed resources, false to dispose of unmanged resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (memberFactory != null)
                    {
                        memberFactory.Dispose();
                        memberFactory = null;
                    }
                    if (TL != null) TL.Dispose();
                }
            }
            this.disposedValue = true;
        }
        #endregion

        /// <summary>
        /// Returns the member factory created for this device for use by the device class
        /// </summary>
        /// <value>The member factory object.</value>
        internal MemberFactory MemberFactory
        {
            get { return memberFactory; }
        }

        #region IAscomDriver Members

        /// <summary>
        /// Set True to connect to the device. Set False to disconnect from the device.
        /// You can also read the property to check whether it is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p>Do not use a NotConnectedException here, that exception is for use in other methods that require a connection in order to succeed.</remarks>
        public bool Connected
        {
            get
            {
                if ((deviceType == "FOCUSER") & (interfaceVersion == 1)) //Focuser interface V1 doesn't use connected, only Link
                {
                    TL.LogMessage("Connected Get", "Device is Focuser and Interfaceverison is 1 so issuing Link command");
                    return (bool)memberFactory.CallMember(1, "Link", new Type[] { }, new object[] { });
                }
                else //Everything else uses Connected!
                {
                    TL.LogMessage("Connected Get", "Issuing Connected command");
                    return (bool)memberFactory.CallMember(1, "Connected", new Type[] { }, new object[] { });
                }
            }
            set
            {
                if ((deviceType == "FOCUSER") & (interfaceVersion == 1)) //Focuser interface V1 doesn't use connected, only Link
                {
                    TL.LogMessage("Connected Set", "Device is Focuser and Interfaceverison is 1 so issuing Link command: " + value);
                    memberFactory.CallMember(2, "Link", new Type[] { }, new object[] { value });
                }
                else //Everything else uses Connected!
                {
                    TL.LogMessage("Connected Set", "Issuing Connected command: " + value);
                    memberFactory.CallMember(2, "Connected", new Type[] { }, new object[] { value });
                }
            }
        }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and modelnumber. Any ASCII characters may be used. 
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
        public string Description
        {
            get
            {
                try
                {
                    return (string)memberFactory.CallMember(1, "Description", new Type[] { typeof(string) }, new object[] { });
                }
                catch (Exception ex)
                {
                    if (interfaceVersion == 1)
                    {
                        switch (deviceType)
                        {
                            case "FILTERWHEEL":
                            case "FOCUSER":
                            case "ROTATOR":
                                TL.LogMessage("Description Get", "This is " + deviceType + " interface version 1, so returning empty string");
                                return "";
                            default:
                                TL.LogMessage("Description Get", "Received exception. Device type is " + deviceType + " and interface version is 1 so throwing received exception: " + ex.Message);
                                throw;
                        }
                    }
                    else
                    {
                        TL.LogMessage("Description Get", "Received exception. Device type is " + deviceType + " and interface version is >1 so throwing received exception: " + ex.Message);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p> This string may contain line endings and may be hundreds to thousands of characters long.
        /// It is intended to display detailed information on the ASCOM driver, including version and copyright data.
        /// See the <see cref="Description" /> property for information on the device itself.
        /// To get the driver version in a parseable string, use the <see cref="DriverVersion" /> property.
        /// </remarks>
        public string DriverInfo
        {
            get
            {
                try
                {
                    return (string)memberFactory.CallMember(1, "DriverInfo", new Type[] { typeof(string) }, new object[] { });
                }
                catch (Exception ex)
                {
                    if (interfaceVersion == 1)
                    {
                        switch (deviceType)
                        {
                            case "CAMERA":
                            case "FILTERWHEEL":
                            case "FOCUSER":
                            case "ROTATOR":
                                TL.LogMessage("DriverInfo Get", "This is " + deviceType + " interface version 1, so returning empty string");
                                return "";
                            default:
                                TL.LogMessage("DriverInfo Get", "Received exception. Device type is " + deviceType + " and interface version is 1 so throwing received exception: " + ex.Message);
                                throw;
                        }
                    }
                    else
                    {
                        TL.LogMessage("DriverInfo Get", "Received exception. Device type is " + deviceType + " and interface version is >1 so throwing received exception: " + ex.Message);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> This must be in the form "n.n".
        /// It should not to be confused with the <see cref="InterfaceVersion" /> property, which is the version of this specification supported by the 
        /// driver.
        /// </remarks>
        public string DriverVersion
        {
            get
            {
                try
                {
                    return (string)memberFactory.CallMember(1, "DriverVersion", new Type[] { typeof(string) }, new object[] { });
                }
                catch (Exception ex)
                {
                    if (interfaceVersion == 1)
                    {
                        switch (deviceType)
                        {
                            case "CAMERA":
                            case "DOME":
                            case "FILTERWHEEL":
                            case "FOCUSER":
                            case "ROTATOR":
                                TL.LogMessage("DriverVersion Get", "This is " + deviceType + " interface version 1, so returning empty string");
                                return "0.0";
                            default:
                                TL.LogMessage("DriverVersion Get", "Received exception. Device type is " + deviceType + " and interface version is 1 so throwing received exception: " + ex.Message);
                                throw;
                        }
                    }
                    else
                    {
                        TL.LogMessage("DriverVersion Get", "Received exception. Device type is " + deviceType + " and interface version is >1 so throwing received exception: " + ex.Message);
                        throw;
                    }
                }
            }
        }

        /// The interface version number that this device supports. Should return 2 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> Clients can detect legacy V1 drivers by trying to read ths property.
        /// If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1. 
        /// In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver.
        /// </remarks>
        public short InterfaceVersion
        {
            get
            {
                try // Return the interface version or return 1 if property is not implemented
                {
                    return Convert.ToInt16(memberFactory.CallMember(1, "InterfaceVersion", new Type[] { }, new object[] { }));
                }
                catch (PropertyNotImplementedException)
                {
                    TL.LogMessage("InterfaceVersion Get", "Received PropertyNotImplementedException so returning interface version = 1");
                    return 1;
                }
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
        public string Name
        {
            get
            {
                try
                {
                    return (string)memberFactory.CallMember(1, "Name", new Type[] { typeof(string) }, new object[] { });
                }
                catch (Exception ex)
                {
                    if (interfaceVersion == 1)
                    {
                        switch (deviceType)
                        {
                            case "CAMERA":
                            case "FILTERWHEEL":
                            case "FOCUSER":
                            case "ROTATOR":
                                TL.LogMessage("Name Get", "This is " + deviceType + " interface version 1, so returning empty string");
                                return "";
                            default:
                                TL.LogMessage("Name Get", "Received exception. Device type is " + deviceType + " and interface version is 1 so throwing received exception: " + ex.Message);
                                throw;
                        }
                    }
                    else
                    {
                        TL.LogMessage("Name Get", "Received exception. Device type is " + deviceType + " and interface version is >1 so throwing received exception: " + ex.Message);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
        public void SetupDialog()
        {
            memberFactory.CallMember(3, "SetupDialog", new Type[] { }, new object[] { });
        }
        #endregion

        #region IDeviceControl Members

        /// <summary>
        /// Invokes the specified device-specific action.
        /// </summary>
        /// <param name="ActionName">
        /// A well known name agreed by interested parties that represents the action to be carried out. 
        /// </param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.
        /// </param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.</returns>
        /// <exception cref="ASCOM.MethodNotImplementedException">Throws this exception if no actions are suported.</exception>
        /// <exception cref="ASCOM.ActionNotImplementedException">It is intended that the SupportedActions method will inform clients 
        /// of driver capabilities, but the driver must still throw an ASCOM.ActionNotImplemented exception if it is asked to 
        /// perform an action that it does not support.</exception>
        /// <exception cref="NotConnectedException">If the driver is not connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <example>Suppose filter wheels start to appear with automatic wheel changers; new actions could 
        /// be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
        /// formatted list of wheel names and the second taking a wheel name and making the change, returning appropriate 
        /// values to indicate success or failure.
        /// </example>
        /// <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> 
        /// This method is intended for use in all current and future device types and to avoid name clashes, management of action names 
        /// is important from day 1. A two-part naming convention will be adopted - <b>DeviceType:UniqueActionName</b> where:
        /// <list type="bullet">
        /// <item><description>DeviceType is the same value as would be used by <see cref="ASCOM.Utilities.Chooser.DeviceType"/> e.g. Telescope, Camera, Switch etc.</description></item>
        /// <item><description>UniqueActionName is a single word, or multiple words joined by underscore characters, that sensibly describes the action to be performed.</description></item>
        /// </list>
        /// <para>
        /// It is recommended that UniqueActionNames should be a maximum of 16 characters for legibility.
        /// Should the same function and UniqueActionName be supported by more than one type of device, the reserved DeviceType of 
        /// “General” will be used. Action names will be case insensitive, so FilterWheel:SelectWheel, filterwheel:selectwheel 
        /// and FILTERWHEEL:SELECTWHEEL will all refer to the same action.</para>
        /// <para>The names of all supported actions must bre returned in the <see cref="SupportedActions"/> property.</para>
        /// </remarks>
        public string Action(string ActionName, string ActionParameters)
        {
            return (string)memberFactory.CallMember(3, "Action", new Type[] { typeof(string), typeof(string) }, new object[] { ActionName, ActionParameters });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the driver is not connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
        public ArrayList SupportedActions
        {
            get
            {
                try
                {
                    return (ArrayList)memberFactory.CallMember(1, "SupportedActions", new Type[] { }, new object[] { });
                }
                catch (Exception ex)
                {
                    //No interface version 1 drivers or TelescopeV2 have SupportedActions so just return an empty arraylist for these
                    if ((interfaceVersion == 1) | ((deviceType == "TELESCOPE") & (interfaceVersion == 2)))
                    {
                        TL.LogMessage("SupportedActions Get", "SupportedActions is not implmented in " + deviceType + " version " + interfaceVersion + " returning an empty ArrayList");
                        return new ArrayList();
                    }
                    else //All later device interfaces should have returned an arraylist but we have received an exception, so pass it on!
                    {
                        TL.LogMessage("SupportedActions Get", "Received exception: " + ex.Message);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the driver is not connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
        public void CommandBlind(string command, bool raw)
        {
            memberFactory.CallMember(3, "CommandBlind", new Type[] { typeof(string), typeof(bool) }, new object[] { command, raw });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the driver is not connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
        public bool CommandBool(string command, bool raw)
        {
            return (bool)memberFactory.CallMember(3, "CommandBool", new Type[] { typeof(string), typeof(bool) }, new object[] { command, raw });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the driver is not connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
        public string CommandString(string command, bool raw)
        {
            return (string)memberFactory.CallMember(3, "CommandString", new Type[] { typeof(string), typeof(bool) }, new object[] { command, raw });
        }

        #endregion

    }
}
