// The purpose of these classes is to match the standards used in the .Utilities components to the
// standards expected by the VB6 COM  Helper componentsthus allowing allowing the two worlds to be different.

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using static ASCOM.Utilities.Global;

namespace ASCOM.Utilities.VB6HelperSupport // Tuck this out of the way of the main ASCOM.Utilities namespace
{

    #region VB6HelperInterfaces

    /// <summary>
    /// Profile access interface
    /// </summary>
    [Guid("87D14110-BEB7-43ff-991E-AAA11C44E5AF")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [ComVisible(true)]
    public interface IProfileAccess
    {
        /// <summary>
        /// Get a Profile value
        /// </summary>
        /// <param name="p_SubKeyName"></param>
        /// <param name="p_ValueName"></param>
        /// <param name="CName"></param>
        /// <returns></returns>
        [DispId(1)]
        string GetProfile(string p_SubKeyName, string p_ValueName, string CName);

        /// <summary>
        /// Write profile value
        /// </summary>
        /// <param name="p_SubKeyName">Sub-key name</param>
        /// <param name="p_ValueName">Value name</param>
        /// <param name="p_ValueData">Value to be written</param>
        /// <param name="CName"></param>
        [DispId(2)]
        void WriteProfile(string p_SubKeyName, string p_ValueName, string p_ValueData, ref string CName);

        /// <summary>
        /// Get list of Profile values
        /// </summary>
        /// <param name="p_SubKeyName">Sub-key name</param>
        /// <param name="CName">Parameter not used</param>
        /// <returns>ArrayList of <see cref="KeyValuePair"/> objects.</returns>
        [DispId(3)]
        ArrayList EnumProfile(string p_SubKeyName, string CName);

        /// <summary>
        /// Delete a Profile value
        /// </summary>
        /// <param name="p_SubKeyName">Sub-key name</param>
        /// <param name="p_ValueName">Value name</param>
        /// <param name="CName">Parameter not used</param>
        [DispId(4)]
        void DeleteProfile(string p_SubKeyName, string p_ValueName, string CName);

        /// <summary>
        /// Create a Profile sub-key
        /// </summary>
        /// <param name="p_SubKeyName">Sub-key name</param>
        /// <param name="CName">Parameter not used</param>
        [DispId(5)]
        void CreateKey(string p_SubKeyName, string CName);

        /// <summary>
        /// Return a list of sub-key names
        /// </summary>
        /// <param name="p_SubKeyName">Starting sub-key.</param>
        /// <param name="CName">Parameter not used</param>
        /// <returns>ArrayList of <see cref="KeyValuePair"/> containing sub-key names and associated default values</returns>
        [DispId(6)]
        ArrayList EnumKeys(string p_SubKeyName, string CName);

        /// <summary>
        /// Delete a Profile sub-key
        /// </summary>
        /// <param name="p_SubKeyName">Name of sub-key to delete</param>
        /// <param name="CName">Parameter not used</param>
        [DispId(7)]
        void DeleteKey(string p_SubKeyName, string CName);

        /// <summary>
        /// Flag whether a driver ProgID has been registered
        /// </summary>
        /// <param name="p_DriverID">Driver ProgID</param>
        /// <param name="p_DriverType">ASCOM device type</param>
        /// <returns>True if the driver is already registered</returns>
        [DispId(8)]
        bool IsRegistered(string p_DriverID, string p_DriverType);
    }

    /// <summary>
    /// Serial component support
    /// </summary>
    [Guid("ABE720E6-9C2C-47e9-8476-6CE5A3F994E2")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [ComVisible(true)]
    public interface ISerialSupport
    {
        /// <summary>
        /// Clear buffers
        /// </summary>
        [DispId(1)]
        void ClearBuffers();

        /// <summary>
        /// Connect or disconnect the serial device
        /// </summary>
        [DispId(2)]
        bool Connected { get; set; }

        /// <summary>
        /// Serial port number
        /// </summary>
        [DispId(3)]
        short Port { get; set; }

        /// <summary>
        /// Port speed
        /// </summary>
        [DispId(4)]
        int PortSpeed { get; set; }

        /// <summary>
        /// Receive a string from the serial device
        /// </summary>
        /// <returns>The received string</returns>
        [DispId(5)]
        string Receive();

        /// <summary>
        /// Receive a byte from the serial device
        /// </summary>
        /// <returns>The received character</returns>
        [DispId(6)]
        byte ReceiveByte();

        /// <summary>
        /// Receive a specified number of characters from the device
        /// </summary>
        /// <param name="p_Count">Number of characters to receive</param>
        /// <returns>The received string</returns>
        [DispId(7)]
        string ReceiveCounted(short p_Count);

        /// <summary>
        /// Receive a specified number of bytes from the serial device
        /// </summary>
        /// <param name="p_Count">Number of characters to receive</param>
        /// <returns>Byte array of characters</returns>
        [DispId(8)]
        byte[] ReceiveCountedBinary(short p_Count);

        /// <summary>
        /// Wait for the specified character string to be received and return all received characters
        /// </summary>
        /// <param name="p_Terminator">Terminating character string</param>
        /// <returns>The received characters as a string including the terminator</returns>
        [DispId(9)]
        string ReceiveTerminated(string p_Terminator);

        /// <summary>
        /// Wait for the specified character string to be received and return all received characters
        /// </summary>
        /// <param name="p_Terminator">Terminating character string</param>
        /// <returns>The received characters as a byte array including the terminator</returns>
        [DispId(10)]
        byte[] ReceiveTerminatedBinary(ref byte[] p_Terminator);

        /// <summary>
        /// The serial device timeout (seconds)
        /// </summary>
        [DispId(11)]
        short ReceiveTimeout { get; set; }

        /// <summary>
        /// The serial device timeout (milli-seconds)
        /// </summary>
        [DispId(12)]
        int ReceiveTimeoutMs { get; set; }

        /// <summary>
        /// Send the specified characters to the serial device
        /// </summary>
        /// <param name="p_Data">Data to send</param>
        [DispId(13)]
        void Transmit(string p_Data);

        /// <summary>
        /// Send the specified characters to the serial device
        /// </summary>
        /// <param name="p_Data">Data to send</param>
        [DispId(14)]
        void TransmitBinary(byte[] p_Data);
    }

    /// <summary>
    /// Chooser support interface
    /// </summary>
    [Guid("5E3A9439-A1A4-4d8d-8658-53E2470C69F6")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [ComVisible(true)]
    public interface IChooserSupport
    {
        /// <summary>
        /// Device type
        /// </summary>
        [DispId(1)]
        string DeviceType { get; set; }

        /// <summary>
        /// Select a driver
        /// </summary>
        /// <param name="CurrentDriverID">Driver ProgID</param>
        /// <returns></returns>
        [DispId(2)]
        string Choose(string CurrentDriverID = "");
    }

    #endregion

    #region ProfileAccess

    /// <summary>
    /// Profile access class
    /// </summary>
    [ProgId("DriverHelper.ProfileAccess")]
    [ComVisible(true)]
    [Guid("f0acf8ea-ddeb-4869-ae33-b25d4d6195b6")]
    [ClassInterface(ClassInterfaceType.None)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class ProfileAccess : IProfileAccess, IDisposable
    {

        // Class to support the VB6 Helper components

        // This exposes the new profile access mechanic, implemented through XMLAccess, as a COM component
        // that can be accessed by the VB6 helpers instead of using their built in registry access tools to
        // get profile values from the registry

        // The function and sub signatures below exactly match those provided by the registry toolkit 
        // originally used by the helpers.

        private RegistryAccess Profile;
        private TraceLogger TL;
        private string LastDriverID;
        private bool LastResult; // Cache values to improve IsRegistered performance

        #region New and IDisposable Support

        /// <summary>
        /// Static initialiser called once per AppDomain to log the component name.
        /// </summary>
        static ProfileAccess()
        {
            Log.Component(Assembly.GetExecutingAssembly(), "VB6HelperSupport.ProfileAccess");
        }

        /// <summary>
        /// Create a new ProfileAccess instance
        /// </summary>
        public ProfileAccess() : this("Unspecified Component")
        {
        }

        /// <summary>
        /// Create a new ProfileAccess instance
        /// </summary>
        /// <param name="ComponentName">Name of this component for debugging purposes</param>
        public ProfileAccess(string ComponentName) : base()
        {
            try
            {
                Profile = new RegistryAccess(ComponentName);
                TL = new TraceLogger("", "VB6ProfileSupport");
                TL.Enabled = GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT); // Get enabled / disabled state from the user registry
                RunningVersions(TL);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("HelperProfile " + ex.ToString());
            }
        }

        private bool disposedValue = false;        // To detect redundant calls

        /// <summary>
        /// Dispose of the object
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        Profile.Dispose();
                        Profile = null;
                    }
                    catch (Exception ex)
                    {
                        Interaction.MsgBox("VB6HelperSupport ProfileAccess Exception " + ex.ToString());
                    }
                    if (TL is not null)
                    {
                        TL.Enabled = false;
                        TL.Dispose();
                        TL = null;
                    }
                }

            }
            disposedValue = true;
        }

        /// <summary>
        /// Dispose of the object
        /// </summary>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finaliser
        /// </summary>
        ~ProfileAccess()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }

        #endregion

        #region ProfileAccess Implementation

        /// <inheritdoc/>
        public string GetProfile(string p_SubKeyName, string p_ValueName, string CName)
        {
            // Get a single profile value
            string Ret;

            Ret = Profile.GetProfile(p_SubKeyName, p_ValueName);
            TL.LogMessage("GetProfile", "SubKey: \"" + p_SubKeyName + "\" Value: \"" + p_ValueName + "\" Data: \"" + Ret + "\"");

            if ((p_ValueName ?? "") == PLATFORM_VERSION_NAME)
                Ret = ConditionPlatformVersion(Ret, Profile, TL); // Check for forced version if we are looking for PlatformVersion

            return Ret;
        }

        /// <inheritdoc/>
        public void WriteProfile(string p_SubKeyName, string p_ValueName, string p_ValueData, ref string CName)
        {
            // Write a single profile value
            TL.LogMessage("WriteProfile", "SubKey: \"" + p_SubKeyName + "\" Value: \"" + p_ValueName + "\" Data: \"" + p_ValueData + "\"");
            if (p_ValueData is null)
                TL.LogMessage("WriteProfile", "WARNING - Supplied data value is Nothing, not empty string");
            Profile.WriteProfile(p_SubKeyName, p_ValueName, p_ValueData);
        }

        /// <inheritdoc/>
        public ArrayList EnumProfile(string p_SubKeyName, string CName)
        {
            // Enumerate values within a given profile key
            // Return these as a Scripting.Dictionary object

            var RetVal = new ArrayList();
            System.Collections.Generic.SortedList<string, string> ReturnedProfile;
            ReturnedProfile = Profile.EnumProfile(p_SubKeyName); // Get the requested values as a hashtable
            TL.LogMessage("EnumProfile", "SubKey: \"" + p_SubKeyName + "\" found " + ReturnedProfile.Count + " values");
            foreach (System.Collections.Generic.KeyValuePair<string, string> de in ReturnedProfile) // Copy the hashtable entries to the scripting.dictionary
            {
                var kvp = new KeyValuePair(de.Key, de.Value);
                RetVal.Add(kvp);
                TL.LogMessage("  EnumProfile", "  Key: \"" + de.Key + "\" Value: \"" + de.Value + "\"");
            }
            return RetVal;
        }

        /// <inheritdoc/>
        public void DeleteProfile(string p_SubKeyName, string p_ValueName, string CName)
        {
            // Delete a profile key
            TL.LogMessage("DeleteProfile", "SubKey: \"" + p_SubKeyName + "\" Value: \"" + p_ValueName + "\"");

            Profile.DeleteProfile(p_SubKeyName, p_ValueName);
        }

        /// <inheritdoc/>
        public void CreateKey(string p_SubKeyName, string CName)
        {
            // Create a new profile key
            TL.LogMessage("CreateKey", "SubKey: \"" + p_SubKeyName + "\"");

            Profile.CreateKey(p_SubKeyName);
        }

        /// <inheritdoc/>
        public ArrayList EnumKeys(string p_SubKeyName, string CName)
        {
            // Enuerate the subkeys in a specified key
            // Return these as a Scripting.Dictionary object

            var RetVal = new ArrayList();
            System.Collections.Generic.SortedList<string, string> Keys;
            Keys = Profile.EnumKeys(p_SubKeyName); // Get the list of subkeys
            TL.LogMessage("EnumKeys", "SubKey: \"" + p_SubKeyName + "\" found " + Keys.Count + " values");

            foreach (System.Collections.Generic.KeyValuePair<string, string> de in Keys) // Copy into the scripting.dictionary
            {
                var kvp = new KeyValuePair(de.Key, de.Value);
                RetVal.Add(kvp);
                TL.LogMessage("  EnumKeys", "  Key: \"" + de.Key + "\" Value: \"" + de.Value + "\"");
            }
            return RetVal;
        }

        /// <inheritdoc/>
        public void DeleteKey(string p_SubKeyName, string CName)
        {
            // Delete a key and all its contents
            TL.LogMessage("DeleteKey", "SubKey: \"" + p_SubKeyName + "\"");

            Profile.DeleteKey(p_SubKeyName);
        }

        /// <inheritdoc/>
        public bool IsRegistered(string DriverID, string DriverType)
        {
            // Confirm that the specified driver is registered
            System.Collections.Generic.SortedList<string, string> keys;
            string IndStr = "  ";

            // If DriverID = LastDriverID Then
            // TL.LogMessage(IndStr & "IsRegistered", IndStr & DriverID.ToString & " - using cached value: " & LastResult)
            // Return LastResult
            // End If
            TL.LogStart(IndStr + "IsRegistered", IndStr + DriverID.ToString() + " ");

            bool IsRegisteredRet = false;
            if (string.IsNullOrEmpty(DriverID))
            {
                TL.LogFinish("Null string so exiting False");
                return IsRegisteredRet; // Nothing is a failure
            }

            try
            {
                keys = Profile.EnumKeys(DriverType + " Drivers");

                // Iterate through all returned driver names comparing them to the required driver name, set a flag if the required driver is present
                foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in keys) // Platform 6 version - makes the test case insensitive! ASCOM-235
                {
                    if ((kvp.Key.ToUpperInvariant() ?? "") == (DriverID.ToUpperInvariant() ?? ""))
                    {
                        TL.LogFinish("Key " + DriverID + " found");
                        IsRegisteredRet = true; // Found it
                    }
                }
                if (!IsRegisteredRet)
                    TL.LogFinish("Key " + DriverID + " not found");
            }

            // If keys.ContainsKey(DriverID) Then Platform 5.5 version
            // TL.LogFinish("Key " & DriverID & " found")
            // IsRegistered = True ' Found it
            // Else
            // TL.LogFinish("Key " & DriverID & " not found")
            // End If
            catch (Exception ex)
            {
                TL.LogFinish("Exception: " + ex.ToString());
            }

            LastDriverID = DriverID;
            LastResult = IsRegisteredRet;
            return IsRegisteredRet;
        }

        #endregion

    }

    #endregion

    #region SerialSupport

    /// <summary>
    /// Support for the serial class
    /// </summary>
    [ProgId("DriverHelper.SerialSupport")]
    [ComVisible(true)]
    [Guid("114EBEC4-7887-4ab9-B750-98BB5F1C8A8F")]
    [ClassInterface(ClassInterfaceType.None)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class SerialSupport : ISerialSupport, IDisposable
    {
        // Class to support the VB6 Helper serial component
        // This exposes a new .Utilities serial port so that it can be accessed through the VB6 helper component

        private Serial SerPort;
        private RegistryAccess SerialProfile = null;
        private string TraceFilename;
        private TraceLogger TL;
        private bool DebugTrace;

        #region New and IDisposable Support

        /// <summary>
        /// Static initialiser called once per AppDomain to log the component name.
        /// </summary>
        static SerialSupport()
        {
            Log.Component(Assembly.GetExecutingAssembly(), "VB6HelperSupport.SerialSupport");
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        public SerialSupport() : base()
        {

            try
            {
                SerPort = new Serial();

                SerialProfile = new RegistryAccess(); // Profile class that can retrieve the value of tracefile
                TraceFilename = SerialProfile.GetProfile("", SERIAL_FILE_NAME_VARNAME);
                TL = new TraceLogger(TraceFilename, "VB6Serial");
                if (!string.IsNullOrEmpty(TraceFilename))
                    TL.Enabled = true;

                // Get debug trace level on / off
                DebugTrace = GetBool(SERIAL_TRACE_DEBUG, SERIAL_TRACE_DEBUG_DEFAULT);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("SERIALSUPPORT: " + ex.ToString());
            }
            finally
            {
                // Clean up
                try
                {
                    SerialProfile.Dispose();
                }
                catch
                {
                }
                SerialProfile = null;
            }

        }

        private bool disposedValue = false;        // To detect redundant calls

        /// <summary>
        /// Dispose of the instance
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                try
                {
                    SerPort.Connected = false;
                }
                catch
                {
                }
                try
                {
                    SerPort.Dispose();
                }
                catch
                {
                }
                SerPort = null;
            }
            disposedValue = true;
        }

        /// <summary>
        /// Dispose of the instance
        /// </summary>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Class finaliser
        /// </summary>
        ~SerialSupport()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }

        #endregion

        #region SerialSupport Implementation

        /// <inheritdoc/>
        public void ClearBuffers()
        {
            var sw = new Stopwatch();
            sw.Start();
            SerPort.ClearBuffers();
            sw.Stop();
            TL.LogMessage("ClearBuffers", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms");
        }

        /// <inheritdoc/>
        public bool Connected
        {
            get
            {
                var sw = new Stopwatch();
                bool RetVal;
                sw.Start();
                RetVal = SerPort.Connected;
                sw.Stop();
                TL.LogMessage("Connected Get", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + RetVal.ToString());
                return RetVal;
            }
            set
            {
                var sw = new Stopwatch();
                sw.Start();
                SerPort.Connected = value;
                sw.Stop();
                TL.LogMessage("Connected Set", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + value.ToString());
            }
        }

        /// <inheritdoc/>
        public short Port
        {
            get
            {
                var sw = new Stopwatch();
                short RetVal;
                sw.Start();
                RetVal = (short)SerPort.Port;
                sw.Stop();
                TL.LogMessage("Port Get", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + RetVal);
                return RetVal;
            }
            set
            {
                var sw = new Stopwatch();
                sw.Start();
                SerPort.Port = value;
                sw.Stop();
                TL.LogMessage("Port Set", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + value);
            }
        }

        /// <inheritdoc/>
        public int PortSpeed
        {
            get
            {
                var sw = new Stopwatch();
                var RetVal = default(int);
                sw.Start();
                switch (SerPort.Speed)
                {
                    case SerialSpeed.ps300:
                        {
                            RetVal = 300;
                            break;
                        }
                    case SerialSpeed.ps1200:
                        {
                            RetVal = 1200;
                            break;
                        }
                    case SerialSpeed.ps2400:
                        {
                            RetVal = 2400;
                            break;
                        }
                    case SerialSpeed.ps4800:
                        {
                            RetVal = 4800;
                            break;
                        }
                    case SerialSpeed.ps9600:
                        {
                            RetVal = 9600;
                            break;
                        }
                    case SerialSpeed.ps14400:
                        {
                            RetVal = 14400;
                            break;
                        }
                    case SerialSpeed.ps19200:
                        {
                            RetVal = 19200;
                            break;
                        }
                    case SerialSpeed.ps28800:
                        {
                            RetVal = 28800;
                            break;
                        }
                    case SerialSpeed.ps38400:
                        {
                            RetVal = 38400;
                            break;
                        }
                    case SerialSpeed.ps57600:
                        {
                            RetVal = 57600;
                            break;
                        }
                    case SerialSpeed.ps115200:
                        {
                            RetVal = 115200;
                            break;
                        }
                }
                sw.Stop();
                TL.LogMessage("PortSpeed Get", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + RetVal);
                return RetVal;
            }
            set
            {
                var sw = new Stopwatch();
                sw.Start();
                switch (value)
                {
                    case 300:
                        {
                            SerPort.Speed = SerialSpeed.ps300;
                            break;
                        }
                    case 1200:
                        {
                            SerPort.Speed = SerialSpeed.ps1200;
                            break;
                        }
                    case 2400:
                        {
                            SerPort.Speed = SerialSpeed.ps2400;
                            break;
                        }
                    case 4800:
                        {
                            SerPort.Speed = SerialSpeed.ps4800;
                            break;
                        }
                    case 9600:
                        {
                            SerPort.Speed = SerialSpeed.ps9600;
                            break;
                        }
                    case 14400:
                        {
                            SerPort.Speed = SerialSpeed.ps14400;
                            break;
                        }
                    case 19200:
                        {
                            SerPort.Speed = SerialSpeed.ps19200;
                            break;
                        }
                    case 28800:
                        {
                            SerPort.Speed = SerialSpeed.ps28800;
                            break;
                        }
                    case 34000:
                        {
                            SerPort.Speed = SerialSpeed.ps38400;
                            break;
                        }
                    case 57600:
                        {
                            SerPort.Speed = SerialSpeed.ps57600;
                            break;
                        }
                    case 115200:
                        {
                            SerPort.Speed = SerialSpeed.ps115200;
                            break;
                        }
                }
                sw.Stop();
                TL.LogMessage("PortSpeed Set", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + value);
            }
        }

        /// <inheritdoc/>
        public string Receive()
        {
            var sw = new Stopwatch();
            string RetVal;
            sw.Start();
            RetVal = SerPort.Receive();
            sw.Stop();
            TL.LogMessage("Receive", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + RetVal);
            return RetVal;
        }

        /// <inheritdoc/>
        public byte ReceiveByte()
        {
            var sw = new Stopwatch();
            byte RetVal;
            sw.Start();
            RetVal = SerPort.ReceiveByte();
            sw.Stop();
            TL.LogMessage("ReceiveByte", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + RetVal.ToString());
            return RetVal;
        }

        /// <inheritdoc/>
        public string ReceiveCounted(short p_Count)
        {
            var sw = new Stopwatch();
            string RetVal;
            sw.Start();
            RetVal = SerPort.ReceiveCounted(p_Count);
            sw.Stop();
            TL.LogMessage("ReceiveCounted", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + RetVal);
            return RetVal;
        }

        /// <inheritdoc/>
        public byte[] ReceiveCountedBinary(short p_Count)
        {
            var sw = new Stopwatch();
            byte[] RetVal;
            System.Text.Encoding TextEncoding;
            TextEncoding = System.Text.Encoding.GetEncoding(1252);
            sw.Start();
            RetVal = SerPort.ReceiveCountedBinary(p_Count);
            sw.Stop();
            TL.LogMessage("ReceiveCountedBinary ", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + TextEncoding.GetString(RetVal), true);
            return RetVal;
        }

        /// <inheritdoc/>
        public string ReceiveTerminated(string p_Terminator)
        {
            var sw = new Stopwatch();
            string RetVal;
            sw.Start();
            RetVal = SerPort.ReceiveTerminated(p_Terminator);
            sw.Stop();
            TL.LogMessage("ReceiveTerminated", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + RetVal);
            return RetVal;
        }

        /// <inheritdoc/>
        public byte[] ReceiveTerminatedBinary(ref byte[] p_Terminator)
        {
            var sw = new Stopwatch();
            byte[] RetVal;
            System.Text.Encoding TextEncoding;
            TextEncoding = System.Text.Encoding.GetEncoding(1252);
            sw.Start();
            RetVal = SerPort.ReceiveTerminatedBinary(p_Terminator);
            sw.Stop();
            TL.LogMessage("Port Set", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + TextEncoding.GetString(RetVal));
            return RetVal;
        }

        /// <inheritdoc/>
        public short ReceiveTimeout
        {
            get
            {
                var sw = new Stopwatch();
                short RetVal;
                sw.Start();
                RetVal = (short)SerPort.ReceiveTimeout;
                sw.Stop();
                TL.LogMessage("ReceiveTimeout Get", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + RetVal);
                return RetVal;
            }
            set
            {
                var sw = new Stopwatch();
                sw.Start();
                SerPort.ReceiveTimeout = value;
                sw.Stop();
                TL.LogMessage("ReceiveTimeout Set", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + value);
            }
        }

        /// <inheritdoc/>
        public int ReceiveTimeoutMs
        {
            get
            {
                var sw = new Stopwatch();
                int RetVal;
                sw.Start();
                RetVal = SerPort.ReceiveTimeoutMs;
                sw.Stop();
                TL.LogMessage("ReceiveTimeoutMs Get", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + RetVal);
                return RetVal;
            }
            set
            {
                var sw = new Stopwatch();
                sw.Start();
                SerPort.ReceiveTimeoutMs = value;
                sw.Stop();
                TL.LogMessage("ReceiveTimeoutMs Set", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + value);
            }
        }

        /// <inheritdoc/>
        public void Transmit(string p_Data)
        {
            var sw = new Stopwatch();
            sw.Start();
            SerPort.Transmit(p_Data);
            sw.Stop();
            TL.LogMessage("Transmit", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + p_Data);
        }

        /// <inheritdoc/>
        public void TransmitBinary(byte[] p_Data)
        {
            var sw = new Stopwatch();
            System.Text.Encoding TextEncoding;
            TextEncoding = System.Text.Encoding.GetEncoding(1252);
            sw.Start();
            SerPort.TransmitBinary(p_Data);
            sw.Stop();
            TL.LogMessage("TransmitBinary", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + TextEncoding.GetString(p_Data));
        }

        #endregion
    }

    #endregion

    #region ChooserSupport

    /// <summary>
    /// ChooserSupport class
    /// </summary>
    [ProgId("DriverHelper.ChooserSupport")]
    [ComVisible(true)]
    [Guid("9289B6A5-CAF1-4da1-8A36-999BEBCDD5E9")]
    [ClassInterface(ClassInterfaceType.None)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class ChooserSupport : IChooserSupport, IDisposable
    {

        // Class to support the VB6 Helper chooser component

        // This exposes a new .Utilities chooser so that it can be accessed through the VB6 helper component
        private Chooser myChooser;
        private bool disposedValue = false;        // To detect redundant calls

        #region New, IDisposable and Finalize

        /// <summary>
        /// Static initialiser called once per AppDomain to log the component name.
        /// </summary>
        static ChooserSupport()
        {
            Log.Component(Assembly.GetExecutingAssembly(), "VB6HelperSupport.ChooserSupport");
        }

        /// <summary>
        /// Create a new instance and instantiate the XMLAccess object to do all the hard work
        /// </summary>
        public ChooserSupport() : base()
        {
            try
            {
                // MsgBox("VB6Helper Before New Utilities.Chooser")
                myChooser = new Chooser();
            }
            // MsgBox("VB6Helper After New Utilities.Chooser")
            catch (Exception ex)
            {
                Interaction.MsgBox("CHOOSERSUPPORT: " + ex.ToString());
            }
        }

        /// <summary>
        /// Dispose of the class
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                if (myChooser is not null)
                {
                    myChooser.Dispose();
                    myChooser = null;
                }
            }
            disposedValue = true;
        }

        /// <summary>
        /// Dispose of the class
        /// </summary>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Class finaliser
        /// </summary>
        ~ChooserSupport()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }

        #endregion

        #region ChooserSupport Implementation

        /// <summary>
        /// Device type
        /// </summary>
        public string DeviceType
        {
            get
            {
                return myChooser.DeviceType;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    Information.Err().Raise(VB6COMErrors.SCODE_ILLEGAL_DEVTYPE, VB6COMErrors.ERR_SOURCE_PROFILE, VB6COMErrors.MSG_ILLEGAL_DEVTYPE);
                myChooser.DeviceType = value;
            }
        }

        /// <summary>
        /// Select the driver
        /// </summary>
        /// <param name="CurrentDriverID">Driver ProgID</param>
        /// <returns></returns>
        public string Choose(string CurrentDriverID = "")
        {
            try
            {
                return myChooser.Choose(CurrentDriverID);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("VB6HELPERSUPPORT.CHOOSE Exception " + ex.ToString());
                return "EXCEPTION VALUE";
            }
        }

        #endregion

    }

    #endregion

}