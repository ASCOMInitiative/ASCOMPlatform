using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;
using ASCOM.Utilities.Interfaces;
using Microsoft.VisualBasic;
using static ASCOM.Utilities.Global;

namespace ASCOM.Utilities
{

    /// <summary>
    /// ASCOM Scope Driver Helper Registry Profile Object
    /// </summary>
    /// <remarks>
    /// <para>This object provides facilities for registering ASCOM drivers with the Chooser, and for storing 
    /// persistence information in a shared area of the file system.</para>
    /// <para>Please code to the IProfile interface</para>
    /// </remarks>
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("880840E2-76E6-4036-AD8F-60A326D7F9DA")]
    [ComVisible(true)]
    [DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
    public class Profile : IProfile, IProfileExtra, IDisposable
    {
        // ===========
        // PROFILE.CLS
        // ===========
        // 
        // Written:  21-Jan-01   Robert B. Denny <rdenny@dc3.com>
        // 
        // Edits:
        // 
        // When      Who     What
        // --------- ---     --------------------------------------------------
        // 25-Feb-09 pwgs    5.1.0 - Refactor for Utilities
        // -----------------------------------------------------------------------------

        private string m_sDeviceType; // Device type specified by user
        private RegistryAccess ProfileStore;
        private TraceLogger TL;

        #region New and IDisposable Support 
        private bool disposedValue = false;        // To detect redundant calls

        /// <summary>
        /// Create a new Profile object
        /// </summary>
        /// <remarks></remarks>
        public Profile() : base()
        {
            ProfileStore = new RegistryAccess(VB6COMErrors.ERR_SOURCE_PROFILE); // Get access to the profile store
            m_sDeviceType = "Telescope";
            TL = new TraceLogger("", "Profile");
            // Profile trace logging disabled because it has not been required for many years
            TL.Enabled = GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT); // Get enabled / disabled state from the user registry
            TL.LogMessage("New", "Trace logger created OK");
        }

        /// <summary>
        /// Create a new profile object ignoring profile not found exceptions if generated
        /// </summary>
        /// <param name="IgnoreExceptions">Ignore ProfileNotFound exceptions</param>
        /// <remarks></remarks>
        public Profile(bool IgnoreExceptions) : base()
        {
            ProfileStore = new RegistryAccess(IgnoreExceptions); // Get access to the profile store
            m_sDeviceType = "Telescope";
            TL = new TraceLogger("", "Profile");
            TL.Enabled = GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT); // Get enabled / disabled state from the user registry
            TL.LogMessage("New", "Trace logger created OK - Ignoring any ProfileNotFound exceptions");
        }

        /// <summary>
        /// Disposes of resources used by the profile object - called by IDisposable interface
        /// </summary>
        /// <param name="disposing"></param>
        /// <remarks></remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                if (ProfileStore is not null)
                {
                    ProfileStore.Dispose();
                    ProfileStore = null;
                }
                if (TL is not null)
                {
                    try
                    {
                        TL.LogMessage("Dispose", "Profile Dispose has been Called.");
                    }
                    catch
                    {
                    } // Clean up the logger
                    TL.Enabled = false;
                    TL.Dispose();
                    TL = null;
                }

            }
            disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        /// <summary>
        /// Disposes of resources used by the profile object
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            // Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            // GC.SuppressFinalize(Me)
        }

        // Protected Overrides Sub Finalize()
        // ' Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
        // Dispose(False)
        // MyBase.Finalize()
        // End Sub

        #endregion

        #region IProfile Implementation
        /// <summary>
        /// The type of ASCOM device for which profile data and registration services are provided 
        /// (String, default = "Telescope")
        /// </summary>
        /// <value>String describing the type of device being accessed</value>
        /// <returns>String describing the type of device being accessed</returns>
        /// <exception cref="Exceptions.InvalidValueException">Thrown on setting the device type to empty string</exception>
        /// <remarks></remarks>
        public string DeviceType
        {
            get
            {
                return m_sDeviceType;
            }
            set
            {
                TL.LogMessage("DeviceType Set", value.ToString());
                if (string.IsNullOrEmpty(value))
                    throw new Exceptions.InvalidValueException(VB6COMErrors.MSG_ILLEGAL_DEVTYPE);
                m_sDeviceType = value;
            }
        }

        /// <summary>
        /// List the device types registered in the Profile store
        /// </summary>
        /// <value>List of registered device types</value>
        /// <returns>A sorted arraylist of device type strings</returns>
        /// <remarks>Use this to find the types of device that are registered in the Profile store.
        /// <para>Device types are returned without the suffix " Devices" that is used in key names within the 
        /// profile store; this allows direct use of returned device types inside For Each loops as shown in 
        /// the Profile code example.</para>
        /// </remarks>
        public ArrayList RegisteredDeviceTypes
        {
            get
            {
                System.Collections.Generic.SortedList<string, string> RootKeys;
                // Dim RegDevs As New Generic.List(Of String), 
                string DType;
                var RetVal = new ArrayList();

                RootKeys = ProfileStore.EnumKeys(""); // Get root Keys
                TL.LogMessage("RegisteredDeviceTypes", "Found " + RootKeys.Count + " values");
                foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in RootKeys)
                {
                    TL.LogMessage("RegisteredDeviceTypes", "  " + kvp.Key + " " + kvp.Value);
                    if (Strings.Right(kvp.Key, 8) == " Drivers")
                    {
                        DType = Strings.Left(kvp.Key, Strings.Len(kvp.Key) - 8);
                        TL.LogMessage("RegisteredDeviceTypes", "    Adding: " + DType);
                        // RegDevs.Add(DType) 'Only add keys that contain drivers
                        RetVal.Add(DType);
                    }
                }

                RetVal.Sort();

                return RetVal;

            }
        }

        /// <summary>
        /// List the devices of a given device type that are registered in the Profile store
        /// </summary>
        /// <param name="DeviceType">Type of devices to list</param>
        /// <returns>An ArrayList of installed devices and associated device descriptions</returns>
        /// <exception cref="Exceptions.InvalidValueException">Throw if the supplied DeviceType is empty string or 
        /// null value.</exception>
        /// <remarks>
        /// Use this to find all the registered devices of a given type that are in the Profile store
        /// <para>If a DeviceType is supplied, where no device of that type has been registered before on this system,
        /// an empty list will be returned</para>
        /// </remarks>
        public ArrayList RegisteredDevices(string DeviceType)
        {
            System.Collections.Generic.SortedList<string, string> RegDevs = null;
            var RetVal = new ArrayList();
            if (string.IsNullOrEmpty(DeviceType)) // Null value and empty string are invalid DeviceTypes
            {
                TL.LogMessage("RegisteredDevices", "Empty string or Nothing supplied as DeviceType");
                throw new Exceptions.InvalidValueException("Empty string or Nothing supplied as DeviceType");
            }
            try
            {
                RegDevs = ProfileStore.EnumKeys(DeviceType + " Drivers"); // Get Key-Class pairs
            }
            catch (NullReferenceException ex) // Catch exception thrown if the DeviceType is an invalid value
            {
                TL.LogMessage("RegisteredDevices", "WARNING: there are no devices of type: \"" + DeviceType + "\" registered on this system");
                RegDevs = new System.Collections.Generic.SortedList<string, string>(); // Return an empty list
            }
            TL.LogMessage("RegisteredDevices", "Device type: " + DeviceType + " - found " + RegDevs.Count + " devices");
            foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in RegDevs)
            {
                TL.LogMessage("RegisteredDevices", "  " + kvp.Key + " - " + kvp.Value);
                RetVal.Add(new KeyValuePair(kvp.Key, kvp.Value));
            }
            return RetVal;
        }

        /// <summary>
        /// Confirms whether a specific driver is registered or unregistered in the profile store
        /// </summary>
        /// <param name="DriverID">String representing the device's ProgID</param>
        /// <returns>Boolean indicating registered or unregistered state of the supplied DriverID</returns>
        /// <remarks></remarks>
        public bool IsRegistered(string DriverID)
        {
            return IsRegisteredPrv(DriverID, false);
        }


        /// <summary>
        /// Registers a supplied DriverID and associates a descriptive name with the device
        /// </summary>
        /// <param name="DriverID">ProgID of the device to register</param>
        /// <param name="DescriptiveName">Descriptive name of the device</param>
        /// <remarks>Does nothing if already registered, so safe to call on driver load.</remarks>
        public void Register(string DriverID, string DescriptiveName)
        {
            string CurrentDescription;
            // Register a driver
            if (!IsRegistered(DriverID))
            {
                TL.LogMessage("Register", "Registering " + DriverID);
                ProfileStore.CreateKey(MakeKey(DriverID, ""));
                ProfileStore.WriteProfile(MakeKey(DriverID, ""), "", DescriptiveName);
            }
            else
            {
                // ASCOM-306 - Added code to refresh description if it is missing
                CurrentDescription = GetValue(DriverID, "", "", "");
                TL.LogMessage("Register", DriverID + " is already registered with description: \"" + CurrentDescription + "\"");
                if (string.IsNullOrEmpty(CurrentDescription) & !string.IsNullOrEmpty(DescriptiveName)) // Description is missing so write the new one if given
                {
                    TL.LogMessage("Register", "Description is missing and new value is supplied so refreshing with: \"" + DescriptiveName + "\"");
                    ProfileStore.WriteProfile(MakeKey(DriverID, ""), "", DescriptiveName);
                }
            }
        }

        /// <summary>
        /// Remove all data for the given DriverID from the registry.
        /// </summary>
        /// <param name="DriverID">ProgID of the device to unregister</param>
        /// <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
        /// <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
        /// <remarks><para>This deletes the entire device profile tree, including the DriverID root key.</para>
        /// <para>Unregister must only be called if the driver has already been registered. If you are not sure 
        /// use the IsRegistered function to check the status and only unregister if the driver is registered.</para>
        /// </remarks>
        public void Unregister(string DriverID)
        {
            // Unregister a driver
            TL.LogMessage("Unregister", DriverID);

            CheckRegistered(DriverID);
            TL.LogMessage("Unregister", "Unregistering " + DriverID);

            ProfileStore.DeleteKey(MakeKey(DriverID, ""));
        }

        /// <summary>
        /// Retrieve a string value from the profile using the supplied subkey for the given Driver ID 
        /// and variable name. Set and return the default value if the requested variable name has not yet been set.
        /// </summary>
        /// <param name="DriverID">ProgID of the device to read from</param>
        /// <param name="Name">Name of the variable whose value is retrieved</param>
        /// <param name="SubKey">Subkey from the profile root from which to read the value</param>
        /// <param name="DefaultValue">Default value to be used if there is no value currently set</param>
        /// <returns>Retrieved variable value</returns>
        /// <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
        /// <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
        /// <remarks>
        /// <para>Name may be an empty string for the unnamed value. The unnamed value is also known as the "default" value for a registry key.</para>
        /// <para>Does not provide access to other registry data types such as binary and double-word. </para>
        /// <para>If a default value is supplied and the value is not already present in the profile store,
        /// the default value will be set in the profile store and then returned as the value of the 
        /// DriverID/SubKey/Name. If the default value is set to null (C#) or Nothing (VB) then no value will
        /// be set in the profile and an empty string will be returned as the value of the 
        /// DriverID/SubKey/Name.</para>
        /// </remarks>
        public string GetValue(string DriverID, string Name, string SubKey, string DefaultValue)
        {
            // Get a profile value
            string Rtn;
            TL.LogMessage("GetValue", "Driver: " + DriverID + " Name: " + Name + " Subkey: \"" + SubKey + "\"" + " DefaultValue: \"" + DefaultValue + "\"");

            CheckRegistered(DriverID);
            Rtn = ProfileStore.GetProfile(MakeKey(DriverID, SubKey), Name, DefaultValue);
            TL.LogMessage("  GetValue", "  " + Rtn);

            return Rtn;
        }

        /// <summary>
        /// Writes a string value to the profile using the supplied subkey for the given Driver ID and variable name.
        /// </summary>
        /// <param name="DriverID">ProgID of the device to write to</param>
        /// <param name="Name">Name of the variable whose value is to be written</param>
        /// <param name="Value">The string value to be written</param>
        /// <param name="SubKey">Subkey from the profile root in which to write the value</param>
        /// <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
        /// <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
        /// <exception cref="Exceptions.RestrictedAccessException">Thrown if Name and SubKey are both empty strings. This 
        /// value is reserved for the device description as it appears in Chooser and is set by Profile.Register</exception>
        /// <remarks></remarks>
        public void WriteValue(string DriverID, string Name, string Value, string SubKey)
        {
            // Create or update a profile value
            TL.LogMessage("WriteValue", "Driver: " + DriverID + " Name: " + Name + " Value: " + Value + " Subkey: \"" + SubKey + "\"");
            if (Value is null)
            {
                TL.LogMessage("WriteProfile", "WARNING - Supplied data value is Nothing, not empty string");
                Value = "";
            }

            CheckRegistered(DriverID);
            if (string.IsNullOrEmpty(Name) & string.IsNullOrEmpty(SubKey))
            {
                // Err.Raise(SCODE_ILLEGAL_REGACC, ERR_SOURCE_PROFILE, MSG_ILLEGAL_REGACC)
                throw new Exceptions.RestrictedAccessException("The device default value is protected as it contains the device description and is set by Profile.Register");
            }
            ProfileStore.WriteProfile(MakeKey(DriverID, SubKey), Name, Value);
        }

        /// <summary>
        /// Return a list of the (unnamed and named variables) under the given DriverID and subkey.
        /// </summary>
        /// <param name="DriverID">ProgID of the device to read from</param>
        /// <param name="SubKey">Subkey from the profile root in which to write the value</param>
        /// <returns>An ArrayList of KeyValuePairs</returns>
        /// <remarks>The returned object contains entries for each value. For each entry, 
        /// the Key property is the value's name, and the Value property is the string value itself. Note that the unnamed (default) 
        /// value will be included if it has a value, even if the value is a blank string. The unnamed value will have its entry's 
        /// Key property set to an empty string.
        /// <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
        /// </remarks>
        public ArrayList Values(string DriverID, string SubKey)
        {
            var RetVal = new ArrayList();
            // Return a hash table of all values in a given key
            System.Collections.Generic.SortedList<string, string> Vals;
            TL.LogMessage("Values", "Driver: " + DriverID + " Subkey: \"" + SubKey + "\"");
            CheckRegistered(DriverID);
            Vals = ProfileStore.EnumProfile(MakeKey(DriverID, SubKey));
            TL.LogMessage("  Values", "  Returning " + Vals.Count + " values");
            foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in Vals)
            {
                TL.LogMessage("  Values", "  " + kvp.Key + " = " + kvp.Value);
                RetVal.Add(new KeyValuePair(kvp.Key, kvp.Value));
            }
            return RetVal;
        }

        /// <summary>
        /// Delete the value from the registry. Name may be an empty string for the unnamed value. Value will be deleted from the subkey supplied.
        /// </summary>
        /// <param name="DriverID">ProgID of the device to read from</param>
        /// <param name="Name">Name of the variable whose value is retrieved</param>
        /// <param name="SubKey">Subkey from the profile root in which to write the value</param>
        /// <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
        /// <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
        /// <remarks>Specify "" to delete the unnamed value which is also known as the "default" value for a registry key. </remarks>
        public void DeleteValue(string DriverID, string Name, string SubKey)
        {
            // Delete a value
            TL.LogMessage("DeleteValue", "Driver: " + DriverID + " Name: " + Name + " Subkey: \"" + SubKey + "\"");
            CheckRegistered(DriverID);
            ProfileStore.DeleteProfile(MakeKey(DriverID, SubKey), Name);
        }

        /// <summary>
        /// Create a registry key for the given DriverID.
        /// </summary>
        /// <param name="DriverID">ProgID of the device to read from</param>
        /// <param name="SubKey">Subkey from the profile root in which to write the value</param>
        /// <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
        /// <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
        /// <remarks>If the SubKey argument contains a \ separated path, the intermediate keys will be created if needed. </remarks>
        public void CreateSubKey(string DriverID, string SubKey)
        {
            // Create a subkey
            TL.LogMessage("CreateSubKey", "Driver: " + DriverID + " Subkey: \"" + SubKey + "\"");
            CheckRegistered(DriverID);
            ProfileStore.CreateKey(MakeKey(DriverID, SubKey));
        }

        /// <summary>
        /// Return a list of the sub-keys under the given DriverID
        /// </summary>
        /// <param name="DriverID">ProgID of the device to read from</param>
        /// <param name="SubKey">Subkey from the profile root in which to write the value</param>
        /// <returns>An ArrayList of key-value pairs</returns>
        /// <remarks>The returned object contains entries for each sub-key. For each KeyValuePair in the list, 
        /// the Key property is the sub-key name, and the Value property is the value. The unnamed ("default") value for that key is also returned.
        /// <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
        /// </remarks>
        public ArrayList SubKeys(string DriverID, string SubKey)
        {
            var RetVal = new ArrayList();
            System.Collections.Generic.SortedList<string, string> SKeys;

            TL.LogMessage("SubKeys", "Driver: " + DriverID + " Subkey: \"" + SubKey + "\"");
            if (!string.IsNullOrEmpty(DriverID))
                CheckRegistered(DriverID);
            SKeys = ProfileStore.EnumKeys(MakeKey(DriverID, SubKey));
            TL.LogMessage("  SubKeys", "  Returning " + SKeys.Count + " subkeys");
            foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in SKeys)
            {
                TL.LogMessage("  SubKeys", "  " + kvp.Key + " = " + kvp.Value);
                RetVal.Add(new KeyValuePair(kvp.Key, kvp.Value));
            }
            return RetVal;
        }

        /// <summary>
        /// Delete a registry key for the given DriverID. SubKey may contain \ separated path to key to be deleted.
        /// </summary>
        /// <param name="DriverID">ProgID of the device to read from</param>
        /// <param name="SubKey">Subkey from the profile root in which to write the value</param>
        /// <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
        /// <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
        /// <remarks>The sub-key and all data and keys beneath it are deleted.</remarks>
        public void DeleteSubKey(string DriverID, string SubKey)
        {
            // Delete a subkey
            TL.LogMessage("DeleteSubKey", "Driver: " + DriverID + " Subkey: \"" + SubKey + "\"");
            CheckRegistered(DriverID);
            ProfileStore.DeleteKey(MakeKey(DriverID, SubKey));
        }

        /// <summary>
        /// Read an entire device profile and return it as an XML encoded string
        /// </summary>
        /// <param name="DriverId">The ProgID of the device</param>
        /// <returns>Device profile encoded in XML</returns>
        /// <remarks>Please see the code examples in this help file for a description of how to use this method.
        /// <para>The format of the returned XML is shown below. The SubKey element repeats for as many subkeys as are present while the Value element with its
        /// Name and Data members repeats as often as there are values in a particular subkey.</para>
        /// <para><pre>
        /// &lt;?xml version="1.0" encoding="utf-8" ?&gt; 
        /// &lt;ASCOMProfileAL&gt;
        /// &lt;SubKey&gt;
        /// &lt;SubKeyName /&gt; 
        /// &lt;DefaultValue&gt;Default text value&lt;/DefaultValue&gt; 
        ///    &lt;Values&gt;
        ///      &lt;Value&gt;
        ///        &lt;Name&gt;Valuename 1&lt;/Name&gt; 
        ///        &lt;Data&gt;False&lt;/Data&gt; 
        ///      &lt;/Value&gt;
        ///      &lt;Value&gt;
        ///        &lt;Name&gt;Valuename 2&lt;/Name&gt; 
        ///        &lt;Data&gt;True&lt;/Data&gt; 
        ///      &lt;/Value&gt;
        ///    &lt;/Values&gt;
        /// &lt;/SubKey&gt;
        /// &lt;SubKey&gt;
        ///    &lt;SubKeyName&gt;Settings&lt;/SubKeyName&gt; 
        ///    &lt;DefaultValue /&gt; 
        ///    &lt;Values&gt;
        ///      &lt;Value&gt;
        ///        &lt;Name&gt;Valuename 3&lt;/Name&gt; 
        ///        &lt;Data&gt;1&lt;/Data&gt; 
        ///      &lt;/Value&gt;
        ///      &lt;Value&gt;
        ///        &lt;Name&gt;Valuename 4&lt;/Name&gt; 
        ///        &lt;Data&gt;53.4217&lt;/Data&gt; 
        ///      &lt;/Value&gt;
        ///    &lt;/Values&gt;
        /// &lt;/SubKey&gt;
        /// &lt;/ASCOMProfileAL>
        /// </pre></para></remarks>
        public string GetProfileXML(string DriverId)
        {
            string RetVal;
            ASCOMProfile CurrentProfile;
            byte[] XMLProfileBytes;
            var XMLSer = new XmlSerializer(typeof(ASCOMProfile));
            var UTF8 = new UTF8Encoding();

            TL.LogMessage("GetProfileXML", "Driver: " + DriverId);
            CheckRegistered(DriverId);

            CurrentProfile = ProfileStore.GetProfile(MakeKey(DriverId, ""));
            using (var MemStream = new MemoryStream()) // Create a memory stream to receive the serialised ProfileKey
            {
                XMLSer.Serialize(MemStream, CurrentProfile); // Serialise the ProfileKey object to XML held in the memory stream
                XMLProfileBytes = MemStream.ToArray(); // Retrieve the serialised Unicode XML characters as a byte array
                RetVal = UTF8.GetString(XMLProfileBytes); // Convert the byte array into a UTF8 character string
            }

            TL.LogMessageCrLf("  GetProfileXML", Microsoft.VisualBasic.Constants.vbCrLf + RetVal);
            return RetVal;
        }

        /// <summary>
        /// Set an entire device profile from an XML encoded string
        /// </summary>
        /// <param name="DriverId">The ProgID of the device</param>
        /// <param name="XmlProfile">An XML encoding of the profile</param>
        /// <remarks>Please see the code examples in this help file for a description of how to use this method. See GetProfileXML for a 
        /// description of the XML format.</remarks>
        public void SetProfileXML(string DriverId, string XmlProfile)
        {
            ASCOMProfile NewProfileContents;
            byte[] XMLProfileBytes;
            var XMLSer = new XmlSerializer(typeof(ASCOMProfile));
            var UTF8 = new UTF8Encoding();

            TL.LogMessageCrLf("SetProfileXML", "Driver: " + DriverId + Microsoft.VisualBasic.Constants.vbCrLf + XmlProfile);
            CheckRegistered(DriverId);

            XMLProfileBytes = UTF8.GetBytes(XmlProfile); // Convert the UTF8 XML string into a byte array
            using (var MemStream = new MemoryStream(XMLProfileBytes)) // Present the UTF8 string byte array as a memory stream
            {
                NewProfileContents = (ASCOMProfile)XMLSer.Deserialize(MemStream); // De-serialise the stream to a ProfileKey object holding the new set of values
            }

            ProfileStore.SetProfile(MakeKey(DriverId, ""), NewProfileContents);
            TL.LogMessage("  SetProfileXML", "  Complete");
        }

        #endregion

        #region IProfileExtra Implementation
        /// <summary>
        /// Read an entire device profile and return it as an ASCOMProfile class instance
        /// </summary>
        /// <param name="DriverId">The ProgID of the device</param>
        /// <returns>Device profile represented as a recursive class</returns>
        /// <remarks>Please see the code examples in this help file for a description of how to use this method.</remarks>
        public ASCOMProfile GetProfile(string DriverId)
        {
            ASCOMProfile RetVal;
            TL.LogMessage("GetProfile", "Driver: " + DriverId);
            CheckRegistered(DriverId);
            RetVal = ProfileStore.GetProfile(MakeKey(DriverId, ""));
            TL.LogMessageCrLf("  GetProfile", "Complete");
            return RetVal;
        }

        /// <summary>
        /// Set an entire device profile from an ASCOMProfile class instance
        /// </summary>
        /// <param name="DriverId">The ProgID of the device</param>
        /// <param name="NewProfileKey">A class representing the profile</param>
        /// <remarks>Please see the code examples in this help file for a description of how to use this method.</remarks>
        public void SetProfile(string DriverId, ASCOMProfile NewProfileKey)
        {
            TL.LogMessage("SetProfile", "Driver: " + DriverId);
            CheckRegistered(DriverId);
            ProfileStore.SetProfile(MakeKey(DriverId, ""), NewProfileKey);
            TL.LogMessage("  SetProfile", "  Complete");
        }

        /// <summary>
        /// Migrate the ASCOM profile from registry to file store
        /// </summary>
        /// <param name="CurrentPlatformVersion">The platform version number of the current profile store being migrated</param>
        /// <remarks></remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComVisible(false)]
        public void MigrateProfile(string CurrentPlatformVersion)
        {
            ProfileStore.BackupProfile(CurrentPlatformVersion);
        }

        /// <summary>
        /// Delete the value from the registry. Name may be an empty string for the unnamed value. 
        /// </summary>
        /// <param name="DriverID">ProgID of the device to read from</param>
        /// <param name="Name">Name of the variable whose value is retrieved</param>
        /// <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
        /// <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
        /// <remarks>Specify "" to delete the unnamed value which is also known as the "default" value for a registry key.
        /// <para>This overload is not available through COM, please use 
        /// "DeleteValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String)"
        /// with SubKey set to empty string achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public void DeleteValue(string DriverID, string Name)
        {
            DeleteValue(DriverID, Name, "");
        }

        /// <summary>
        /// Retrieve a string value from the profile for the given Driver ID and variable name
        /// </summary>
        /// <param name="DriverID">ProgID of the device to read from</param>
        /// <param name="Name">Name of the variable whose value is retrieved</param>
        /// <returns>Retrieved variable value</returns>
        /// <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
        /// <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
        /// <remarks>
        /// <para>Name may be an empty string for the unnamed value. The unnamed value is also known as the "default" value for a registry key.</para>
        /// <para>Does not provide access to other registry data types such as binary and double-word. </para>
        /// <para>This overload is not available through COM, please use 
        /// "GetValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String)"
        /// with SubKey set to empty string achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string GetValue(string DriverID, string Name)
        {
            return GetValue(DriverID, Name, "", null);
        }

        /// <summary>
        /// Retrieve a string value from the profile using the supplied subkey for the given Driver ID and variable name.
        /// </summary>
        /// <param name="DriverID">ProgID of the device to read from</param>
        /// <param name="Name">Name of the variable whose value is retrieved</param>
        /// <param name="SubKey">Subkey from the profile root from which to read the value</param>
        /// <returns>Retrieved variable value</returns>
        /// <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
        /// <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
        /// <remarks>
        /// <para>Name may be an empty string for the unnamed value. The unnamed value is also known as the "default" value for a registry key.</para>
        /// <para>Does not provide access to other registry data types such as binary and double-word. </para>
        /// </remarks>
        [ComVisible(false)]
        public string GetValue(string DriverID, string Name, string SubKey)
        {
            return GetValue(DriverID, Name, SubKey, null);
        }

        /// <summary>
        /// Return a list of the sub-keys under the root of the given DriverID
        /// </summary>
        /// <param name="DriverID">ProgID of the device to read from</param>
        /// <returns>An ArrayList of key-value pairs</returns>
        /// <remarks>The returned object contains entries for each sub-key. For each KeyValuePair in the list, 
        /// the Key property is the sub-key name, and the Value property is the value. The unnamed ("default") value for that key is also returned.
        /// <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
        /// </remarks>
        [ComVisible(false)]
        public ArrayList SubKeys(string DriverID)
        {
            return SubKeys(DriverID, "");
        }

        /// <summary>
        /// Return a list of the (unnamed and named variables) under the root of the given DriverID.
        /// </summary>
        /// <param name="DriverID">ProgID of the device to read from</param>
        /// <returns>An ArrayList of KeyValuePairs</returns>
        /// <remarks>The returned object contains entries for each value. For each entry, 
        /// the Key property is the value's name, and the Value property is the string value itself. Note that the unnamed (default) 
        /// value will be included if it has a value, even if the value is a blank string. The unnamed value will have its entry's 
        /// Key property set to an empty string.
        /// <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
        /// </remarks>
        [ComVisible(false)]
        public ArrayList Values(string DriverID)
        {
            return Values(DriverID, "");
        }

        /// <summary>
        /// Writes a string value to the profile using the given Driver ID and variable name.
        /// </summary>
        /// <param name="DriverID">ProgID of the device to write to</param>
        /// <param name="Name">Name of the variable whose value is to be written</param>
        /// <param name="Value">The string value to be written</param>
        /// <exception cref="Exceptions.InvalidValueException">Thrown if DriverID is an empty string.</exception>
        /// <exception cref="Exceptions.DriverNotRegisteredException">Thrown if the driver is not registered,</exception>
        /// <remarks>
        /// This overload is not available through COM, please use 
        /// "WriteValue(ByVal DriverID As String, ByVal Name As String, ByVal Value As String, ByVal SubKey As String)"
        /// with SubKey set to empty string achieve this effect.
        /// </remarks>
        [ComVisible(false)]
        public void WriteValue(string DriverID, string Name, string Value)
        {
            WriteValue(DriverID, Name, Value, "");
        }
        #endregion

        #region Support code
        private bool IsRegisteredPrv(string DriverID, bool Indent)
        {
            bool IsRegisteredPrvRet = default;
            // Confirm that the specified driver is registered
            System.Collections.Generic.SortedList<string, string> keys;
            string IndStr = "";

            if (Indent)
                IndStr = "  ";

            TL.LogStart(IndStr + "IsRegistered", IndStr + DriverID.ToString() + " ");

            IsRegisteredPrvRet = false; // Assume failure
            if (string.IsNullOrEmpty(DriverID))
            {
                TL.LogFinish("Null string so exiting False");
                return IsRegisteredPrvRet; // Nothing is a failure
            }

            try
            {
                keys = ProfileStore.EnumKeys(MakeKey("", ""));

                // Iterate through all returned driver names comparing them to the required driver name, set a flag if the required driver is present
                foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in keys) // Platform 6 version - makes the test case insensitive! ASCOM-235
                {
                    if ((kvp.Key.ToUpperInvariant() ?? "") == (DriverID.ToUpperInvariant() ?? ""))
                    {
                        TL.LogFinish("Key " + DriverID + " found");
                        IsRegisteredPrvRet = true; // Found it
                    }
                }

                if (!IsRegisteredPrvRet)
                    TL.LogFinish("Key " + DriverID + " not found");
            }

            catch (Exception ex)
            {
                TL.LogFinish("Exception: " + ex.ToString());
            }

            return IsRegisteredPrvRet;

        }

        private string MakeKey(string BaseKey, string SubKey)
        {
            string MakeKeyRet = default;
            // Create a full path to a subkey given the driver name and type 
            MakeKeyRet = m_sDeviceType + " Drivers";
            if (!string.IsNullOrEmpty(BaseKey))
                MakeKeyRet = MakeKeyRet + @"\" + BaseKey; // Allow blank BaseKey (See SubKeys())
            if (!string.IsNullOrEmpty(SubKey))
                MakeKeyRet = MakeKeyRet + @"\" + SubKey;
            return MakeKeyRet;
        }

        private void CheckRegistered(string DriverID)
        {
            // Confirm that a given driver exists
            TL.LogMessage("  CheckRegistered", "\"" + DriverID + "\" DeviceType: " + m_sDeviceType);
            if (!IsRegisteredPrv(DriverID, true))
            {
                TL.LogMessage("  CheckRegistered", "Driver is not registered");
                if (string.IsNullOrEmpty(DriverID))
                {
                    TL.LogMessage("  CheckRegistered", "Throwing illegal driver ID exception");
                    throw new Exceptions.InvalidValueException(VB6COMErrors.MSG_ILLEGAL_DRIVERID);
                }
                else
                {
                    TL.LogMessage("  CheckRegistered", "Throwing driver is not registered exception. ProgID: " + DriverID + " DeviceType: " + m_sDeviceType);
                    throw new Exceptions.DriverNotRegisteredException("DriverID " + DriverID + " is not registered as a device of type: " + m_sDeviceType);
                }
            }
            else
            {
                TL.LogMessage("  CheckRegistered", "Driver is registered");
            }
        }
        #endregion

        #region COM Registration
        /// <summary>
        /// Function that is called by RegAsm when the assembly is registered for COM
        /// </summary>
        /// <remarks>This is necessary to ensure that the mscoree.dll can be found when the SetSearchDirectories function has been called in an application e.g. by Inno installer post v5.5.9</remarks>
        [ComRegisterFunction]
        private static void COMRegisterActions(Type typeToRegister)
        {
            COMRegister(typeToRegister);
        }

        /// <summary>
        /// Function that is called by RegAsm when the assembly is registered for COM
        /// </summary>
        [ComUnregisterFunction]
        private static void COMUnRegisterActions(Type typeToRegister)
        {
            // No action on unregister, this method has been included to remove a compiler warning
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }

        #endregion

    }
}