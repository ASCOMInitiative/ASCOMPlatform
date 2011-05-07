using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Conform;
using System.Collections;
using System.Globalization;

namespace ASCOM.DriverAccess
    
{
    /// <summary>
    /// Base class for ASCOM driver access toolkit device classes. This class contains the methods common to all devices
    /// so that they can be maintained in just one place.
    /// </summary>
    public class AscomDriver: IDisposable
    {
        private int interfaceVersion;
        #region AscomDriver Constructors and Dispose

        MemberFactory memberFactory;
        private bool disposedValue = false;        // To detect redundant calls
        private string deviceType;

        /// <summary>
        /// Creates a new instance of the <see cref="AscomDriver"/> class.
        /// </summary>
        /// <param name="deviceProgId">The prog id. of the device being created.</param>
        public AscomDriver(string deviceProgId)
        {
            memberFactory = new MemberFactory(deviceProgId);
            interfaceVersion = this.InterfaceVersion;
            //deviceType = deviceProgId.Substring(deviceProgId.LastIndexOf(".") + 1).ToUpper();
            deviceType = this.GetType().Name.ToUpper();
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
        /// Set True to connect the driver to the device. Set False to disconnect the driver and device.
        /// You can also read the property to check whether it is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        /// <exception cref="ASCOM.DriverException">Must throw an exception if unsuccessful.</exception>
        public bool Connected
        {
            get 
            { 
                if ((deviceType=="FOCUSER") & (interfaceVersion == 1)) //Focuser interface V1 doesn't use connected, only Link
                {
                    return (bool)memberFactory.CallMember(1, "Link", new Type[] { }, new object[] { }); 
                }
                else //Everything else uses Connected!
                {
                    return (bool)memberFactory.CallMember(1, "Connected", new Type[] { }, new object[] { }); 
                }
            }
            set 
            { 
                if ((deviceType == "FOCUSER") & (interfaceVersion == 1)) //Focuser interface V1 doesn't use connected, only Link
                {
                    memberFactory.CallMember(2, "Link", new Type[] { }, new object[] { value });
                }
                else //Everything else uses Connected!
                {
                    memberFactory.CallMember(2, "Connected", new Type[] { }, new object[] { value });
                }
            }
        }

        /// <summary>
        /// Returns a description of the driver, such as manufacturer and model
        /// number. Any ASCII characters may be used. For camera devices, the string shall not exceed 68
        /// characters (for compatibility with FITS headers).
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref=" ASCOM.DriverException">Must throw an exception if description unavailable</exception>
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
                                return "";
                            default:
                                throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }

                }
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <remarks>
        /// This string may contain line endings and may be hundreds to thousands of characters long.
        /// It is intended to display detailed information on the ASCOM driver, including version and copyright data.
        /// See the Description property for descriptive info on the device itself.
        /// To get the driver version in a parseable string, use the DriverVersion property.
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
                                return "";
                            default:
                                throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }

                }
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// This must be in the form "n.n".
        /// </summary>
        /// <remarks>
        /// Not to be confused with the InterfaceVersion property, which is the version of this specification supported by the driver (currently 2). 
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
                                return "0.0";
                            default:
                                throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }

                }
            }
        }

        /// <summary>
        /// The version of the implementd device interface.
        /// </summary>
        /// <remarks>
        /// Clients can detect legacy V1 drivers by trying to read ths property.
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
                    return 1;
                }
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
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
                                return "";
                            default:
                                throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }

                }
            }
        }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="ASCOM.DriverException">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            memberFactory.CallMember(3, "SetupDialog", new Type[] { }, new object[] { });
        }
        #endregion

        #region IDeviceControl Members

        /// <summary>
        /// Invokes the specified device-specific action.
        /// </summary>
        /// <param name="actionName">
        /// A well known name agreed by interested parties that represents the action to be carried out. 
        /// </param>
        /// <param name="actionParameters">List of required parameters or String.Empty if none are required.
        /// </param>
        /// <returns>A string response and sets the <c>IDeviceControl.LastResult</c> property.</returns>
        /// <remarks>
        /// <example>Suppose filter wheels start to appear with automatic wheel changers; new actions could 
        /// be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
        /// formatted list of wheel names and the second taking a wheel name and making the change.
        /// </example>
        /// </remarks>
        /// <exception cref="ASCOM.MethodNotImplementedException">Throws an exception if not implemented.</exception>
        public string Action(string actionName, string actionParameters)
        {
            return (string)memberFactory.CallMember(3, "Action", new Type[] { typeof(string), typeof(string) }, new object[] { actionName, actionParameters });
        }

        /// <summary>
        /// Gets an ArrayList (SafeArray Collection) of the supported actions.
        /// </summary>
        /// <value>The supported actions.</value>
        /// <remarks>This method must return an emtyy arraylist if not actions are supported. Please do not throw a 
        /// MethodNotImplementedException.</remarks>
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
                        return new ArrayList();
                    }
                    else //All later device interfaces should have returned an arraylist but we have received an exception, so pass it on!
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="command">The literal command string to be transmitted.</param>
        /// <param name="raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added by the driver prior to transmission.
        /// </param>
        /// <exception cref="ASCOM.MethodNotImplementedException">Throws an exception if not implemented.</exception>
        public void CommandBlind(string command, bool raw)
        {
            memberFactory.CallMember(3, "CommandBlind", new Type[] { typeof(string), typeof(bool) }, new object[] { command, raw });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="command">The literal command string to be transmitted.</param>
        /// <param name="raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        /// <exception cref="ASCOM.MethodNotImplementedException">Throws an exception if not implemented.</exception>
        public bool CommandBool(string command, bool raw)
        {
            return (bool)memberFactory.CallMember(3, "CommandBool", new Type[] { typeof(string), typeof(bool) }, new object[] { command, raw });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="command">The literal command string to be transmitted.</param>
        /// <param name="raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        /// <exception cref="ASCOM.MethodNotImplementedException">Throws an exception if not implemented.</exception>
        public string CommandString(string command, bool raw)
        {
            return (string)memberFactory.CallMember(3, "CommandString", new Type[] { typeof(string), typeof(bool) }, new object[] { command, raw });
        }

        #endregion

    }
}
