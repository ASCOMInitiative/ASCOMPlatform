// The purpose of these classes is to match the standards used in the .Utilities components to the
// standards expected by the VB6 COM  Helper componentsthus allowing allowing the two worlds to be different.

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using static ASCOM.Utilities.Global;

namespace ASCOM.Utilities.VB6HelperSupport // Tuck this out of the way of the main ASCOM.Utilities namespace
{

    #region VB6HelperInterfaces
    [Guid("87D14110-BEB7-43ff-991E-AAA11C44E5AF")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [ComVisible(true)]
    public interface IProfileAccess
    {
        [DispId(1)]
        string GetProfile(string p_SubKeyName, string p_ValueName, string CName);
        [DispId(2)]
        void WriteProfile(string p_SubKeyName, string p_ValueName, string p_ValueData, ref string CName);
        [DispId(3)]
        ArrayList EnumProfile(string p_SubKeyName, string CName); // Scripting.Dictionary 'Hashtable
        [DispId(4)]
        void DeleteProfile(string p_SubKeyName, string p_ValueName, string CName);
        [DispId(5)]
        void CreateKey(string p_SubKeyName, string CName);
        [DispId(6)]
        ArrayList EnumKeys(string p_SubKeyName, string CName); // Scripting.Dictionary 'Hashtable
        [DispId(7)]
        void DeleteKey(string p_SubKeyName, string CName);
        [DispId(8)]
        bool IsRegistered(string p_DriverID, string p_DriverType);
    }

    [Guid("ABE720E6-9C2C-47e9-8476-6CE5A3F994E2")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [ComVisible(true)]
    public interface ISerialSupport
    {
        [DispId(1)]
        void ClearBuffers();
        [DispId(2)]
        bool Connected { get; set; }
        [DispId(3)]
        short Port { get; set; }
        [DispId(4)]
        int PortSpeed { get; set; }
        [DispId(5)]
        string Receive();
        [DispId(6)]
        byte ReceiveByte();
        [DispId(7)]
        string ReceiveCounted(short p_Count);
        [DispId(8)]
        byte[] ReceiveCountedBinary(short p_Count);
        [DispId(9)]
        string ReceiveTerminated(string p_Terminator);
        [DispId(10)]
        byte[] ReceiveTerminatedBinary(ref byte[] p_Terminator);
        [DispId(11)]
        short ReceiveTimeout { get; set; }
        [DispId(12)]
        int ReceiveTimeoutMs { get; set; }
        [DispId(13)]
        void Transmit(string p_Data);
        [DispId(14)]
        void TransmitBinary(byte[] p_Data);
    }

    [Guid("5E3A9439-A1A4-4d8d-8658-53E2470C69F6")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [ComVisible(true)]
    public interface IChooserSupport
    {
        [DispId(1)]
        string DeviceType { get; set; }
        [DispId(2)]
        string Choose(string CurrentDriverID = "");
    }
    #endregion

    #region ProfileAccess
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
        // Create a new instance and instanciate the XMLAccess object to do all the hard work
        // MyBase.New()
        // Try
        // Profile = New ASCOM.Utilities.XMLAccess("Unspecified Component")
        // Catch ex As Exception
        // MsgBox("HelperProfile " & ex.ToString)
        // End Try
        public ProfileAccess() : this("Unspecified Component")
        {
        }

        // As New() excpet that it allows the calling component to identify itself, this name is used in error messages
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

        // IDisposable
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

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ProfileAccess()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }

        #endregion

        #region ProfileAccess Implementation

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

        public void WriteProfile(string p_SubKeyName, string p_ValueName, string p_ValueData, ref string CName)
        {
            // Write a single profile value
            TL.LogMessage("WriteProfile", "SubKey: \"" + p_SubKeyName + "\" Value: \"" + p_ValueName + "\" Data: \"" + p_ValueData + "\"");
            if (p_ValueData is null)
                TL.LogMessage("WriteProfile", "WARNING - Supplied data value is Nothing, not empty string");
            Profile.WriteProfile(p_SubKeyName, p_ValueName, p_ValueData);
        }

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

        public void DeleteProfile(string p_SubKeyName, string p_ValueName, string CName)
        {
            // Delete a profile key
            TL.LogMessage("DeleteProfile", "SubKey: \"" + p_SubKeyName + "\" Value: \"" + p_ValueName + "\"");

            Profile.DeleteProfile(p_SubKeyName, p_ValueName);
        }

        public void CreateKey(string p_SubKeyName, string CName)
        {
            // Create a new profile key
            TL.LogMessage("CreateKey", "SubKey: \"" + p_SubKeyName + "\"");

            Profile.CreateKey(p_SubKeyName);
        }

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

        public void DeleteKey(string p_SubKeyName, string CName)
        {
            // Delete a key and all its contents
            TL.LogMessage("DeleteKey", "SubKey: \"" + p_SubKeyName + "\"");

            Profile.DeleteKey(p_SubKeyName);
        }

        public bool IsRegistered(string DriverID, string DriverType)
        {
            bool IsRegisteredRet = default;
            // Confirm that the specified driver is registered
            System.Collections.Generic.SortedList<string, string> keys;
            string IndStr = "  ";

            // If DriverID = LastDriverID Then
            // TL.LogMessage(IndStr & "IsRegistered", IndStr & DriverID.ToString & " - using cached value: " & LastResult)
            // Return LastResult
            // End If
            TL.LogStart(IndStr + "IsRegistered", IndStr + DriverID.ToString() + " ");

            IsRegisteredRet = false; // Assume failure
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
        // Create a new instance and instanciate the Serial object to do all the hard work
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

        // IDisposable
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

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SerialSupport()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }

        #endregion

        #region SerialSupport Implementation

        public void ClearBuffers()
        {
            var sw = new Stopwatch();
            sw.Start();
            SerPort.ClearBuffers();
            sw.Stop();
            TL.LogMessage("ClearBuffers", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms");
        }

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

        public void Transmit(string p_Data)
        {
            var sw = new Stopwatch();
            sw.Start();
            SerPort.Transmit(p_Data);
            sw.Stop();
            TL.LogMessage("Transmit", Strings.Format(sw.ElapsedMilliseconds, "0").PadLeft(4) + "ms " + p_Data);
        }

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
        // Create a new instance and instanciate the XMLAccess object to do all the hard work
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

        // IDisposable
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

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ChooserSupport()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }
        #endregion

        #region ChooserSupport Implementation
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