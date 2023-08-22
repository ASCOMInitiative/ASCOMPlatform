﻿using System;
using ASCOM.Utilities;
using System.Collections;
using System.Threading.Tasks;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Base class for ASCOM driver access toolkit device classes. This class contains the methods common to all devices
    /// so that they can be maintained in just one place.
    /// </summary>
    public class AscomDriver : IDisposable
    {
        internal TraceLogger TL;
        private short? interfaceVersion = null; // Create a nullable short to hold the interface version and initialise it to null - i.e. the value has not yet been retrieved from the driver
        MemberFactory memberFactory;
        private bool disposedValue = false;        // To detect redundant calls
        private string deviceType;

        #region AscomDriver Constructors and Dispose

        /// <summary>
        /// 
        /// </summary>
        public AscomDriver()
        {
        }


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
            deviceType = this.GetType().Name.ToUpperInvariant();
            TL.LogMessage("AscomDriver", "Device type: " + this.GetType().Name);

            memberFactory = new MemberFactory(deviceProgId, TL); // Create a MemberFactory object and pass in the TraceLogger

        }

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
		public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of managed and unmanaged resources
        /// </summary>
        /// <param name="disposing">True to dispose of managed resources, false to dispose of unmanaged resources</param>
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

        #region Internal members

        /// <summary>
        /// Returns the member factory created for this device for use by the device class
        /// </summary>
        /// <value>The member factory object.</value>
        internal MemberFactory MemberFactory
        {
            get { return memberFactory; }
        }

        /// <summary>
        /// Return the driver interface version number
        /// </summary>
        /// <returns>The driver's interface version</returns>
        /// <remarks>
        /// This method reads the interface version on the first call and caches it, returning the cached value on subsequent calls.
        /// It also handles interface version 1 drivers that don't have InterfaceVersion properties
        /// </remarks>
        internal short DriverInterfaceVersion
        {
            get
            {
                // Test whether the interface version has already been retrieved
                if (!interfaceVersion.HasValue) // This is the first time the method has been called so get the interface version number from the driver and cache it
                {
                    try { interfaceVersion = this.InterfaceVersion; } // Get the interface version
                    catch { interfaceVersion = 1; } // The method failed so assume that the driver has a version 1 interface where the InterfaceVersion method is not implemented
                }

                return interfaceVersion.Value; // Return the newly retrieved or already cached value
            }
        }

        #endregion

        #region IAscomDriver Members

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p>Do not use a NotConnectedException here. That exception is for use in other methods that require a connection in order to succeed.
        /// <para>The Connected property sets and reports the state of connection to the device hardware.
        /// For a hub this means that Connected will be true when the first driver connects and will only be set to false
        /// when all drivers have disconnected.  A second driver may find that Connected is already true and
        /// setting Connected to false does not report Connected as false.  This is not an error because the physical state is that the
        /// hardware connection is still true.</para>
        /// <para>Multiple calls setting Connected to true or false will not cause an error.</para>
        /// </remarks>
        public bool Connected
        {
            get
            {
                if ((deviceType == "FOCUSER") & (DriverInterfaceVersion == 1)) //Focuser interface V1 doesn't use connected, only Link
                {
                    TL.LogMessage("Connected Get", "Device is Focuser and InterfaceVerison is 1 so issuing Link command");
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
                if ((deviceType == "FOCUSER") & (DriverInterfaceVersion == 1)) //Focuser interface V1 doesn't use connected, only Link
                {
                    TL.LogMessage("Connected Set", "Device is Focuser and InterfaceVerison is 1 so issuing Link command: " + value);
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
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used. 
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p> 
        /// <para>The description length must be a maximum of 64 characters so that it can be used in FITS image headers, which are limited to 80 characters including the header name.</para>
        /// </remarks>
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
                    if (DriverInterfaceVersion == 1)
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
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p> This string may contain line endings and may be hundreds to thousands of characters long.
        /// It is intended to display detailed information on the ASCOM driver, including version and copyright data.
        /// See the <see cref="Description" /> property for information on the device itself.
        /// To get the driver version in a parse-able string, use the <see cref="DriverVersion" /> property.
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
                    if (DriverInterfaceVersion == 1)
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
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p> This must be in the form "n.n".
        /// It should not to be confused with the <see cref="InterfaceVersion" /> property, which is the version of this specification supported by the driver.
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
                    if (DriverInterfaceVersion == 1)
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

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> Clients can detect legacy V1 drivers by trying to read this property.
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
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p>
        /// </remarks>
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
                    if (DriverInterfaceVersion == 1)
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
        /// Launches a configuration dialogue box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
        public void SetupDialog()
        {
            memberFactory.CallMember(3, "SetupDialog", new Type[] { }, new object[] { });
        }

        #endregion

        #region IDeviceControl Members

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        /// <exception cref="MethodNotImplementedException">Thrown if no actions are supported.</exception>
        /// <exception cref="ActionNotImplementedException">It is intended that the <see cref="SupportedActions"/> method will inform clients of driver capabilities, but the driver must still throw 
        /// an <see cref="ASCOM.ActionNotImplementedException"/> exception  if it is asked to perform an action that it does not support.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented.</b></p>
        /// <para>Action names are case insensitive, so SelectWheel, selectwheel and SELECTWHEEL all refer to the same action.</para>
        /// <para>The names of all supported actions must be returned in the <see cref="SupportedActions" /> property.</para>
        /// </remarks>
        public string Action(string ActionName, string ActionParameters)
        {
            return (string)memberFactory.CallMember(3, "Action", new Type[] { typeof(string), typeof(string) }, new object[] { ActionName, ActionParameters });
        }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p>
        /// <para>This method must return an empty <see cref="ArrayList" /> if no actions are supported. Do not throw a <see cref="ASCOM.PropertyNotImplementedException" />.</para>
        /// <para>SupportedActions is a "discovery" mechanism that enables clients to know which Actions a device supports without having to exercise the Actions themselves. This mechanism is necessary because there could be
        /// people / equipment safety issues if actions are called unexpectedly or out of a defined process sequence.
        /// It follows from this that SupportedActions must return names that match the spelling of Action names exactly, without additional descriptive text. However, returned names may use any casing
        /// because the <see cref="Action" /> ActionName parameter is case insensitive.</para>
        /// </remarks>
        public ArrayList SupportedActions
        {
            get
            {
                try
                {
                    return memberFactory.CallMember(1, "SupportedActions", new Type[] { }, new object[] { }).ComObjToArrayList();
                }
                catch (Exception ex)
                {
                    //No interface version 1 drivers or TelescopeV2 have SupportedActions so just return an empty arraylist for these
                    if ((DriverInterfaceVersion == 1) | ((deviceType == "TELESCOPE") & (DriverInterfaceVersion == 2)))
                    {
                        TL.LogMessage("SupportedActions Get", "SupportedActions is not implemented in " + deviceType + " version " + DriverInterfaceVersion + " returning an empty ArrayList");
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
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks><p style="color:red"><b>May throw a NotImplementedException.</b></p>
        /// <para>The CommandXXX methods are a historic mechanic that provides clients with direct and unimpeded access to change device hardware configuration. While highly enabling for clients, this mechanic is inherently risky
        /// because clients can fundamentally change hardware operation without the driver being aware that a change is taking / has taken place.</para>
        /// <para>The newer Action / SupportedActions mechanic provides discrete, named, functions that can deliver any functionality required.They do need driver authors to make provision for them within the 
        /// driver, but this approach is much lower risk than using the CommandXXX methods because it enables the driver to resolve conflicts between standard device interface commands and extended commands 
        /// provided as Actions.The driver is always aware of what is happening and can adapt more effectively to client needs.</para>
        /// </remarks>
        public void CommandBlind(string Command, bool Raw)
        {
            memberFactory.CallMember(3, "CommandBlind", new Type[] { typeof(string), typeof(bool) }, new object[] { Command, Raw });
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
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks><p style="color:red"><b>May throw a NotImplementedException.</b></p>
        /// <para>The CommandXXX methods are a historic mechanic that provides clients with direct and unimpeded access to change device hardware configuration. While highly enabling for clients, this mechanic is inherently risky
        /// because clients can fundamentally change hardware operation without the driver being aware that a change is taking / has taken place.</para>
        /// <para>The newer Action / SupportedActions mechanic provides discrete, named, functions that can deliver any functionality required.They do need driver authors to make provision for them within the 
        /// driver, but this approach is much lower risk than using the CommandXXX methods because it enables the driver to resolve conflicts between standard device interface commands and extended commands 
        /// provided as Actions.The driver is always aware of what is happening and can adapt more effectively to client needs.</para>
        /// </remarks>
        public bool CommandBool(string Command, bool Raw)
        {
            return (bool)memberFactory.CallMember(3, "CommandBool", new Type[] { typeof(string), typeof(bool) }, new object[] { Command, Raw });
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
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks><p style="color:red"><b>May throw a NotImplementedException.</b></p>
        /// <para>The CommandXXX methods are a historic mechanic that provides clients with direct and unimpeded access to change device hardware configuration. While highly enabling for clients, this mechanic is inherently risky
        /// because clients can fundamentally change hardware operation without the driver being aware that a change is taking / has taken place.</para>
        /// <para>The newer Action / SupportedActions mechanic provides discrete, named, functions that can deliver any functionality required.They do need driver authors to make provision for them within the 
        /// driver, but this approach is much lower risk than using the CommandXXX methods because it enables the driver to resolve conflicts between standard device interface commands and extended commands 
        /// provided as Actions.The driver is always aware of what is happening and can adapt more effectively to client needs.</para>
        /// </remarks>
        public string CommandString(string Command, bool Raw)
        {
            return (string)memberFactory.CallMember(3, "CommandString", new Type[] { typeof(string), typeof(bool) }, new object[] { Command, Raw });
        }

        #endregion

    }
}
