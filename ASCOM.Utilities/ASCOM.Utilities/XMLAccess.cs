using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
// Class to read and write profile values in an XML format

using System.Xml;
using ASCOM.Utilities.Exceptions;
using ASCOM.Utilities.Interfaces;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using static ASCOM.Utilities.Global;

namespace ASCOM.Utilities
{

    internal class XMLAccess : IAccess, IDisposable
    {

        private const int RETRY_MAX = 1; // Number of persistence failure re-trys
        private const int RETRY_INTERVAL = 200; // Length between persistence failure re-trys in milliseconds

        private IFileStoreProvider FileStore; // File store containing the new ASCOM XML profile 
        private bool disposedValue = false;        // To detect redundant calls to IDisposable

        private System.Threading.Mutex ProfileMutex;
        private bool GotMutex;

        private TraceLogger TL;

        private Stopwatch sw, swSupport;

        #region New and IDisposable Support
        public XMLAccess() : this(false) // Create but respect any exceptions thrown
        {
        }

        public XMLAccess(string p_CallingComponent) : this(false)
        {
        }

        public XMLAccess(bool p_IgnoreTest)
        {
            string PlatformVersion;

            TL = new TraceLogger("", "XMLAccess"); // Create a new trace logger
            TL.Enabled = GetBool(TRACE_XMLACCESS, TRACE_XMLACCESS_DEFAULT); // Get enabled / disabled state from the user registry
            RunningVersions(TL); // Include version information

            sw = new Stopwatch(); // Create the stowatch instances
            swSupport = new Stopwatch();

            FileStore = new AllUsersFileSystemProvider();
            // FileStore = New IsolatedStorageFileStoreProvider

            ProfileMutex = new System.Threading.Mutex(false, PROFILE_MUTEX_NAME);

            // Bypass test for initial setup by MigrateProfile because the profile isn't yet set up
            if (!p_IgnoreTest)
            {
                try
                {
                    if (!FileStore.get_Exists(@"\" + VALUES_FILENAME))
                        throw new ProfileNotFoundException("Utilities Error Base key does not exist");
                    PlatformVersion = GetProfile(@"\", "PlatformVersion");
                }
                // OK, no exception so assume that we are initialised
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("XMLAccess.New Unexpected exception:", ex.ToString());
                    throw;
                }
            }
        }

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                try
                {
                    FileStore = null; // Clean up the filestore and keycache
                    TL.Enabled = false; // Clean up the logger
                    TL.Dispose();
                    TL = null;
                    sw.Stop(); // Clean up the stopwatches
                    sw = null;
                    swSupport.Stop();
                    swSupport = null;
                    ProfileMutex.Close();
                    ProfileMutex = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("XMLAccess:Dispose Exception - " + ex.ToString());
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

        ~XMLAccess()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }

        #endregion

        #region IAccess Implementation

        internal void CreateKey(string p_SubKeyName)
        {
            // Create a key
            var InitalValues = new SortedList<string, string>();
            string[] SubKeys;
            string SubKey;
            int i, j;

            try
            {
                GetProfileMutex("CreateKey", p_SubKeyName);
                sw.Reset();
                sw.Start(); // Start timing this call
                TL.LogMessage("CreateKey", "SubKey: \"" + p_SubKeyName + "\"");

                p_SubKeyName = Strings.Trim(p_SubKeyName); // Normalise the string:
                SubKeys = Strings.Split(p_SubKeyName, @"\", Compare: CompareMethod.Text); // Parse p_SubKeyName into its elements
                switch (p_SubKeyName ?? "")  // Null path so do nothing
                {
                    case var @case when @case == "":
                        {
                            break;
                        }
                    case @"\": // Root node so just create this
                        {
                            if (!FileStore.get_Exists(@"\" + VALUES_FILENAME)) // Test whether the key already exists
                            {
                                TL.LogMessage("  CreateKey", @"  Creating root key ""\""");
                                InitalValues.Clear(); // Now add the file containing the contents of the key
                                InitalValues.Add(COLLECTION_DEFAULT_VALUE_NAME, COLLECTION_DEFAULT_UNSET_VALUE);
                                WriteValues(@"\", ref InitalValues, false); // Write the profile file, don't check if it already exists
                            }
                            else
                            {
                                TL.LogMessage("  CreateKey", "  Root key alread exists");
                            } // Create the directory and its intermediate directories

                            break;
                        }

                    default:
                        {
                            var loopTo = SubKeys.Length - 1;
                            for (i = 0; i <= loopTo; i++)
                            {
                                SubKey = "";
                                var loopTo1 = i;
                                for (j = 0; j <= loopTo1; j++)
                                    SubKey = SubKey + @"\" + SubKeys[j];
                                if (!FileStore.get_Exists(SubKey + @"\" + VALUES_FILENAME))
                                {
                                    FileStore.CreateDirectory(SubKey, TL);  // It doesn't exist so create it
                                    InitalValues.Clear(); // Now add the file containing the contents of the key
                                    InitalValues.Add(COLLECTION_DEFAULT_VALUE_NAME, COLLECTION_DEFAULT_UNSET_VALUE);
                                    WriteValues(SubKey, ref InitalValues, false); // Write the profile file
                                }
                            }

                            break;
                        }
                }
                sw.Stop();
                TL.LogMessage("  ElapsedTime", "  " + sw.ElapsedMilliseconds + " milliseconds");
            }
            finally
            {
                ProfileMutex.ReleaseMutex();
            }
        }

        void IAccess.CreateKey(string p_SubKeyName) => CreateKey(p_SubKeyName);

        internal void DeleteKey(string p_SubKeyName)
        {
            // Delete a key
            try
            {
                GetProfileMutex("DeleteKey", p_SubKeyName);
                sw.Reset();
                sw.Start(); // Start timing this call
                TL.LogMessage("DeleteKey", "SubKey: \"" + p_SubKeyName + "\"");

                try
                {
                    FileStore.DeleteDirectory(p_SubKeyName);
                }
                catch (Exception)
                {
                } // Remove it if at all possible but don't throw any errors
                sw.Stop();
                TL.LogMessage("  ElapsedTime", "  " + sw.ElapsedMilliseconds + " milliseconds");
            }
            finally
            {
                ProfileMutex.ReleaseMutex();
            }
        }

        void IAccess.DeleteKey(string p_SubKeyName) => DeleteKey(p_SubKeyName);

        internal void RenameKey(string CurrentSubKeyName, string NewSubKeyName)
        {
            try
            {
                GetProfileMutex("RenameKey", CurrentSubKeyName + " " + NewSubKeyName);
                sw.Reset();
                sw.Start(); // Start timing this call
                TL.LogMessage("RenameKey", "Current SubKey: \"" + CurrentSubKeyName + "\"" + " New SubKey: \"" + NewSubKeyName + "\"");

                FileStore.RenameDirectory(CurrentSubKeyName, NewSubKeyName);
                sw.Stop();
                TL.LogMessage("  ElapsedTime", "  " + sw.ElapsedMilliseconds + " milliseconds");
            }
            finally
            {
                ProfileMutex.ReleaseMutex();
            }
        }

        void IAccess.RenameKey(string CurrentSubKeyName, string NewSubKeyName) => RenameKey(CurrentSubKeyName, NewSubKeyName);

        internal void DeleteProfile(string p_SubKeyName, string p_ValueName)
        {
            // Delete a value from a key
            SortedList<string, string> Values;

            try
            {
                GetProfileMutex("DeleteProfile", p_SubKeyName + " " + p_ValueName);
                sw.Reset();
                sw.Start(); // Start timing this call
                TL.LogMessage("DeleteProfile", "SubKey: \"" + p_SubKeyName + "\" Name: \"" + p_ValueName + "\"");

                Values = ReadValues(p_SubKeyName); // Read current contents of key
                try // Remove value if it exists
                {
                    if (string.IsNullOrEmpty(p_ValueName)) // Just set the default name to the unset value
                    {
                        Values[COLLECTION_DEFAULT_VALUE_NAME] = COLLECTION_DEFAULT_UNSET_VALUE;
                        TL.LogMessage("DeleteProfile", "  Default name was changed to unset value");
                    }
                    else // Actually delete the value
                    {
                        Values.Remove(p_ValueName);
                        TL.LogMessage("DeleteProfile", "  Value was deleted");
                    }
                }
                catch
                {
                    TL.LogMessage("DeleteProfile", "  Value did not exist");
                }
                WriteValues(p_SubKeyName, ref Values); // Write the new list of values back to the key
                Values = null;
                sw.Stop();
                TL.LogMessage("  ElapsedTime", "  " + sw.ElapsedMilliseconds + " milliseconds");
            }
            finally
            {
                ProfileMutex.ReleaseMutex();
            }
        }

        void IAccess.DeleteProfile(string p_SubKeyName, string p_ValueName) => DeleteProfile(p_SubKeyName, p_ValueName);

        internal SortedList<string, string> EnumKeys(string p_SubKeyName)
        {
            // Return a sorted list of subkeys
            SortedList<string, string> Values;
            var RetValues = new SortedList<string, string>();
            string[] Directories;
            string DefaultValue;

            try
            {
                GetProfileMutex("EnumKeys", p_SubKeyName);
                sw.Reset();
                sw.Start(); // Start timing this call
                TL.LogMessage("EnumKeys", "SubKey: \"" + p_SubKeyName + "\"");

                Directories = FileStore.get_GetDirectoryNames(p_SubKeyName); // Get a list of the keys
                foreach (string Directory in Directories) // Process each key in trun
                {
                    try // If there is an error reading the data don't include in the returned list
                    {
                        Values = ReadValues(p_SubKeyName + @"\" + Directory); // Read the values of this key to find the default value
                        DefaultValue = Values[COLLECTION_DEFAULT_VALUE_NAME]; // Save the default value
                        if ((DefaultValue ?? "") == COLLECTION_DEFAULT_UNSET_VALUE)
                            DefaultValue = "";
                        RetValues.Add(Directory, DefaultValue); // Add the directory name and default value to the hashtable
                    }
                    catch
                    {
                    }
                    Values = null;
                }
                sw.Stop();
                TL.LogMessage("  ElapsedTime", "  " + sw.ElapsedMilliseconds + " milliseconds");
            }
            finally
            {
                ProfileMutex.ReleaseMutex();
            }
            return RetValues;
        }

        SortedList<string, string> IAccess.EnumKeys(string p_SubKeyName) => EnumKeys(p_SubKeyName);

        internal SortedList<string, string> EnumProfile(string p_SubKeyName)
        {
            // Returns a sorted list of key values
            SortedList<string, string> Values;
            var RetValues = new SortedList<string, string>();

            try
            {
                GetProfileMutex("EnumProfile", p_SubKeyName);
                sw.Reset();
                sw.Start(); // Start timing this call
                TL.LogMessage("EnumProfile", "SubKey: \"" + p_SubKeyName + "\"");

                Values = ReadValues(p_SubKeyName); // Read values from profile XML file
                foreach (KeyValuePair<string, string> kvp in Values) // Retrieve each key/value  pair in turn
                {
                    if ((kvp.Key ?? "") == COLLECTION_DEFAULT_VALUE_NAME)
                    {
                        if ((kvp.Value ?? "") == COLLECTION_DEFAULT_UNSET_VALUE)
                        {
                        }
                        // Do nothing if the value is unset
                        else
                        {
                            RetValues.Add("", kvp.Value);
                        } // Add any other value to the return value
                    }
                    else
                    {
                        RetValues.Add(kvp.Key, kvp.Value);
                    }
                }
                Values = null;
                sw.Stop();
                TL.LogMessage("  ElapsedTime", "  " + sw.ElapsedMilliseconds + " milliseconds");
            }
            finally
            {
                ProfileMutex.ReleaseMutex();
            }
            return RetValues;
        }

        SortedList<string, string> IAccess.EnumProfile(string p_SubKeyName) => EnumProfile(p_SubKeyName);

        internal string GetProfile(string p_SubKeyName, string p_ValueName, string p_DefaultValue)
        {
            // Read a single value from a key
            SortedList<string, string> Values;
            string RetVal;

            try
            {
                GetProfileMutex("GetProfile", p_SubKeyName + " " + p_ValueName + " " + p_DefaultValue);
                sw.Reset();
                sw.Start(); // Start timing this call
                TL.LogMessage("GetProfile", "SubKey: \"" + p_SubKeyName + "\" Name: \"" + p_ValueName + "\"" + "\" DefaultValue: \"" + p_DefaultValue + "\"");

                RetVal = ""; // Initialise return value to null string
                try
                {
                    Values = ReadValues(p_SubKeyName); // Read in the key values
                    if (string.IsNullOrEmpty(p_ValueName)) // Requested the default value
                    {
                        RetVal = Values[COLLECTION_DEFAULT_VALUE_NAME];
                    }
                    else // Requested a particular value
                    {
                        RetVal = Values[p_ValueName];
                    }
                }
                catch (KeyNotFoundException ) // Missing value generates a KeyNotFound exception and should return a null string or the supplied default value
                {
                    if (p_DefaultValue is not null) // We have been supplied a default value so set it and then return it
                    {
                        WriteProfile(p_SubKeyName, p_ValueName, p_DefaultValue);
                        RetVal = p_DefaultValue;
                        TL.LogMessage("GetProfile", "Value not yet set, returning supplied default value: " + p_DefaultValue);
                    }
                    else
                    {
                        TL.LogMessage("GetProfile", "Value not yet set and no default value supplied, returning null string");
                    }
                }
                catch (Exception ex) // Any other exception
                {
                    if (p_DefaultValue is not null) // We have been supplied a default value so set it and then return it
                    {
                        WriteProfile(p_SubKeyName, p_ValueName, p_DefaultValue);
                        RetVal = p_DefaultValue;
                        TL.LogMessage("GetProfile", "Key not yet set, returning supplied default value: " + p_DefaultValue);
                    }
                    else
                    {
                        TL.LogMessage("GetProfile", "Key not yet set and no default value supplied, throwing exception: " + ex.Message);
                        throw;
                    }
                }

                Values = null;
                sw.Stop();
                TL.LogMessage("  ElapsedTime", "  " + sw.ElapsedMilliseconds + " milliseconds");
            }
            finally
            {
                ProfileMutex.ReleaseMutex();
            }

            return RetVal;
        }

        string IAccess.GetProfile(string p_SubKeyName, string p_ValueName, string p_DefaultValue) => GetProfile(p_SubKeyName, p_ValueName, p_DefaultValue);

        internal string GetProfile(string p_SubKeyName, string p_ValueName)
        {
            return GetProfile(p_SubKeyName, p_ValueName, null);
        }

        string IAccess.GetProfile(string p_SubKeyName, string p_ValueName) => GetProfile(p_SubKeyName, p_ValueName);

        internal void WriteProfile(string p_SubKeyName, string p_ValueName, string p_ValueData)
        {
            // Write a single value to a key
            SortedList<string, string> Values;

            try
            {
                GetProfileMutex("WriteProfile", p_SubKeyName + " " + p_ValueName + " " + p_ValueData);
                sw.Reset();
                sw.Start(); // Start timing this call
                TL.LogMessage("WriteProfile", "SubKey: \"" + p_SubKeyName + "\" Name: \"" + p_ValueName + "\" Value: \"" + p_ValueData + "\"");

                // Check if the directory exists
                if (!FileStore.get_Exists(p_SubKeyName + @"\" + VALUES_FILENAME))
                    CreateKey(p_SubKeyName); // Create the subkey if it doesn't already exist
                Values = ReadValues(p_SubKeyName); // Read the key values

                if (string.IsNullOrEmpty(p_ValueName)) // Write the deault value
                {
                    if (Values.ContainsKey(COLLECTION_DEFAULT_VALUE_NAME)) // Does exist so update it
                    {
                        Values[COLLECTION_DEFAULT_VALUE_NAME] = p_ValueData; // Update the existing value
                    }
                    else // Doesn't exist so add it
                    {
                        Values.Add(COLLECTION_DEFAULT_VALUE_NAME, p_ValueData);
                    } // Add the new value
                    WriteValues(p_SubKeyName, ref Values); // Write the values back to the XML profile
                }
                else // Write a named value
                {
                    if (Values.ContainsKey(p_ValueName))
                        Values.Remove(p_ValueName); // Remove old value if it exists
                    Values.Add(p_ValueName, p_ValueData); // Add the new value
                    WriteValues(p_SubKeyName, ref Values);
                } // Write the values back to the XML profile
                Values = null;
                sw.Stop();
                TL.LogMessage("  ElapsedTime", "  " + sw.ElapsedMilliseconds + " milliseconds");
            }
            finally
            {
                ProfileMutex.ReleaseMutex();
            }
        }

        void IAccess.WriteProfile(string p_SubKeyName, string p_ValueName, string p_ValueData) => WriteProfile(p_SubKeyName, p_ValueName, p_ValueData);

        internal void SetSecurityACLs()
        {
            bool LogEnabled;
            try
            {
                GetProfileMutex("SetSecurityACLs", "");
                sw.Reset();
                sw.Start(); // Start timing this call
                TL.LogMessage("SetSecurityACLs", "");

                // Force logging to be enabled for this...
                LogEnabled = TL.Enabled; // Save logging state
                TL.Enabled = true;
                RunningVersions(TL); // Capture date in case logging wasn't initially enabled

                // Set security ACLs on profile root directory
                TL.LogMessage("SetSecurityACLs", "Setting security ACLs on ASCOM root directory ");
                FileStore.SetSecurityACLs(TL);
                sw.Stop();
                TL.LogMessage("  ElapsedTime", "  " + sw.ElapsedMilliseconds + " milliseconds");
                TL.Enabled = LogEnabled; // Restore logging state
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("SetSecurityACLs", "Exception: " + ex.ToString());
                throw;
            }
        }

        internal void MigrateProfile(string CurrentPlatformVersion)
        {
            RegistryKey FromKey;
            bool LogEnabled;

            try
            {
                GetProfileMutex("MigrateProfile", "");
                sw.Reset();
                sw.Start(); // Start timing this call
                TL.LogMessage("MigrateProfile", "");

                // Force logging to be enabled for this...
                LogEnabled = TL.Enabled; // Save logging state
                TL.Enabled = true;
                RunningVersions(TL); // Capture date in case logging wasn't initially enabled

                TL.LogMessage("MigrateProfile", "Migrating keys");
                // Create the root directory if it doesn't already exist
                if (!FileStore.get_Exists(@"\" + VALUES_FILENAME))
                {
                    FileStore.CreateDirectory(@"\", TL);
                    CreateKey(@"\"); // Create the root key
                    TL.LogMessage("MigrateProfile", "Successfully created root directory and root key");
                }
                else
                {
                    TL.LogMessage("MigrateProfile", "Root directory already exists");
                }

                // Set security ACLs on profile root directory
                TL.LogMessage("MigrateProfile", "Setting security ACLs on ASCOM root directory ");
                FileStore.SetSecurityACLs(TL);

                TL.LogMessage("MigrateProfile", "Copying Profile from Registry");
                // Get the registry root key depending. Success here depends on us running as 32bit as the Platform 5 registry 
                // is located under HKLM\Software\Wow6432Node!
                FromKey = Registry.LocalMachine.OpenSubKey(REGISTRY_ROOT_KEY_NAME); // Source to copy from 
                if (FromKey is not null) // Got a key
                {
                    TL.LogMessage("MigrateProfile", "FromKey Opened OK: " + FromKey.Name + ", SubKeyCount: " + FromKey.SubKeyCount.ToString() + ", ValueCount: " + FromKey.ValueCount.ToString());
                    MigrateKey(FromKey, ""); // Use recursion to copy contents to new tree
                    TL.LogMessage("MigrateProfile", "Successfully migrated keys");
                    FromKey.Close();
                    // Restore original logging state
                    TL.Enabled = GetBool(TRACE_XMLACCESS, TRACE_XMLACCESS_DEFAULT); // Get enabled / disabled state from the user registry
                }
                else // Didn't get a key from either location so throw an error
                {
                    throw new ProfileNotFoundException(@"Cannot find ASCOM Profile in HKLM\" + REGISTRY_ROOT_KEY_NAME + " Is Platform 5 installed?");
                }
                sw.Stop();
                TL.LogMessage("  ElapsedTime", "  " + sw.ElapsedMilliseconds + " milliseconds");
                TL.Enabled = LogEnabled; // Restore logging state
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("MigrateProfile", "Exception: " + ex.ToString());
                throw;
            }
        }

        void IAccess.MigrateProfile(string CurrentPlatformVersion) => MigrateProfile(CurrentPlatformVersion);

        internal ASCOMProfile GetProfileXML(string DriverId)
        {
            throw new MethodNotImplementedException("XMLAccess:GetProfileXml");
        }

        ASCOMProfile IAccess.GetProfile(string DriverId) => GetProfileXML(DriverId);
        internal void SetProfileXML(string DriverId, ASCOMProfile Profile)
        {
            throw new MethodNotImplementedException("XMLAccess:SetProfileXml");
        }

        void IAccess.SetProfile(string DriverId, ASCOMProfile Profile) => SetProfileXML(DriverId, Profile);

        #endregion

        #region Support Functions

        private SortedList<string, string> ReadValues(string p_SubKeyName)
        {

            // Read all values in a key - SubKey has to be absolute from the profile store root
            var Retval = new SortedList<string, string>();
            XmlReaderSettings ReaderSettings;
            bool ReadOK = false;
            int RetryCount;
            bool ErrorOccurred = false;
            string ValuesFileName; // Name of the profile file from which to read
            bool ExistsValues, ExistsValuesOriginal, ExistsValuesNew;

            swSupport.Reset();
            swSupport.Start(); // Start timing this call
            if (Strings.Left(p_SubKeyName, 1) != @"\")
                p_SubKeyName = @"\" + p_SubKeyName; // Condition to have leading \
            TL.LogMessage("  ReadValues", "  SubKeyName: " + p_SubKeyName);

            ValuesFileName = VALUES_FILENAME; // Initialise to the file holding current values
            RetryCount = -1; // Initialise to ensure we get RETRY_Max number of retrys

            // Determine what files exist and handle the case where this key has not yet been created
            ExistsValues = FileStore.get_Exists(p_SubKeyName + @"\" + VALUES_FILENAME);
            ExistsValuesOriginal = FileStore.get_Exists(p_SubKeyName + @"\" + VALUES_FILENAME_ORIGINAL);
            ExistsValuesNew = FileStore.get_Exists(p_SubKeyName + @"\" + VALUES_FILENAME_NEW);
            if (!ExistsValues & !ExistsValuesOriginal)
                throw new ProfileNotFoundException("No profile files exist for this key: " + p_SubKeyName);
            do
            {
                RetryCount += 1;
                try
                {
                    ReaderSettings = new XmlReaderSettings();
                    ReaderSettings.IgnoreWhitespace = true;
                    using (var Reader = XmlReader.Create(FileStore.get_FullPath(p_SubKeyName + @"\" + ValuesFileName), ReaderSettings))
                    {
                        Reader.Read(); // Get rid of the XML version string
                        Reader.Read(); // Read in the Profile name tag

                        // Start reading profile strings
                        while (Reader.Read())
                        {
                            switch (Reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        switch (Reader.Name ?? "")
                                        {
                                            case DEFAULT_ELEMENT_NAME: // Found default value
                                                {
                                                    Retval.Add(COLLECTION_DEFAULT_VALUE_NAME, Reader.GetAttribute(VALUE_ATTRIBUTE_NAME));
                                                    TL.LogMessage("    ReadValues", "    found " + COLLECTION_DEFAULT_VALUE_NAME + " = " + Retval[COLLECTION_DEFAULT_VALUE_NAME]);
                                                    break;
                                                }
                                            case VALUE_ELEMENT_NAME: // Fount an element name
                                                {
                                                    string ValueName = Reader.GetAttribute(NAME_ATTRIBUTE_NAME);
                                                    Retval.Add(ValueName, Reader.GetAttribute(VALUE_ATTRIBUTE_NAME));
                                                    TL.LogMessage("    ReadValues", "    found " + ValueName + " = " + Retval[ValueName]); // Do nothing
                                                    break;
                                                }

                                            default:
                                                {
                                                    TL.LogMessage("    ReadValues", "    ## Found unexpected Reader.Name: " + Reader.Name.ToString());
                                                    break;
                                                }
                                        } // Do nothing

                                        break;
                                    }

                                default:
                                    {
                                        break;
                                    }
                            }
                        }

                        Reader.Close(); // Close the IO readers
                    }
                    swSupport.Stop();
                    TL.LogMessage("  ReadValues", "  added to cache - " + swSupport.ElapsedMilliseconds + " milliseconds");
                    ReadOK = true; // Set the exit flag here when a read has been successful
                }
                catch (Exception ex)
                {
                    ErrorOccurred = true;
                    if (RetryCount == RETRY_MAX)
                    {
                        if ((ValuesFileName ?? "") == VALUES_FILENAME) // Attempt to recover information from the last good file
                        {
                            ValuesFileName = VALUES_FILENAME_ORIGINAL;
                            RetryCount = -1;
                            LogEvent("XMLAccess:ReadValues", "Error reading profile on final retry - attempting recovery from previous version", EventLogEntryType.Warning, EventLogErrors.XMLAccessRecoveryPreviousVersion, ex.ToString());
                            TL.LogMessageCrLf("  ReadValues", "Final retry exception - attempting recovery from previous version: " + ex.ToString());
                        }
                        else // Recovery not possible so throw exception
                        {
                            LogEvent("XMLAccess:ReadValues", "Error reading profile on final retry", EventLogEntryType.Error, EventLogErrors.XMLAccessReadError, ex.ToString());
                            TL.LogMessageCrLf("  ReadValues", "Final retry exception: " + ex.ToString());
                            throw new ProfilePersistenceException("XMLAccess Exception", ex);
                        }
                    }

                    else
                    {
                        LogEvent("XMLAccess:ReadValues", "Error reading profile - retry: " + RetryCount, EventLogEntryType.Warning, EventLogErrors.XMLAccessRecoveryPreviousVersion, ex.Message);
                        TL.LogMessageCrLf("  ReadValues", "Retry " + RetryCount + " exception: " + ex.ToString());
                    }
                }
                if (ErrorOccurred)
                    System.Threading.Thread.Sleep(RETRY_INTERVAL); // Wait if an error occurred
            }
            while (!ReadOK);
            if (ErrorOccurred) // Update the logs as we seem to have got round it
            {
                LogEvent("XMLAccess:ReadValues", "Recovered from read error OK", EventLogEntryType.SuccessAudit, EventLogErrors.XMLAccessRecoveredOK, null);
                TL.LogMessage("  ReadValues", "Recovered from read error OK");
            }

            return Retval;
        }

        private void WriteValues(string p_SubKeyName, ref SortedList<string, string> p_KeyValuePairs)
        {
            // Make the general case check for existence of a current Profile.xml file. Most cases need this
            // The exception is the CreateKey where the Profile.xmldefinitlkey won't exist as we are about to create it for the first time
            WriteValues(p_SubKeyName, ref p_KeyValuePairs, true);
        }

        private void WriteValues(string p_SubKeyName, ref SortedList<string, string> p_KeyValuePairs, bool p_CheckForCurrentProfileStore)
        {
            // Write  all key values to an XML file
            // SubKey has to be absolute from the profile store root
            XmlWriterSettings WriterSettings;
            string FName;
            int Ct;

            swSupport.Reset();
            swSupport.Start(); // Start timing this call
            TL.LogMessage("  WriteValues", "  SubKeyName: " + p_SubKeyName);
            if (Strings.Left(p_SubKeyName, 1) != @"\")
                p_SubKeyName = @"\" + p_SubKeyName;

            try
            {
                Ct = 0;
                foreach (KeyValuePair<string, string> kvp in p_KeyValuePairs)
                {
                    Ct += 1;
                    TL.LogMessage("  WriteValues List", "  " + Ct.ToString() + " " + kvp.Key + " = " + kvp.Value);
                }

                WriterSettings = new XmlWriterSettings();
                WriterSettings.Indent = true;
                FName = FileStore.get_FullPath(p_SubKeyName + @"\" + VALUES_FILENAME_NEW);
                XmlWriter Writer;
                FileStream FStream;
                FStream = new FileStream(FName, FileMode.Create, FileAccess.Write, FileShare.None, 2048, FileOptions.WriteThrough);
                Writer = XmlWriter.Create(FStream, WriterSettings);
                // Writer = XmlWriter.Create(FName, WriterSettings)
                using (Writer)
                {
                    Writer.WriteStartDocument();
                    Writer.WriteStartElement(PROFILE_NAME); // Write the profile element
                    Writer.WriteStartElement(DEFAULT_ELEMENT_NAME); // Write the default element
                    Writer.WriteAttributeString(VALUE_ATTRIBUTE_NAME, p_KeyValuePairs[COLLECTION_DEFAULT_VALUE_NAME]); // Write the default value
                    Writer.WriteEndElement();
                    Ct = 0;
                    foreach (KeyValuePair<string, string> kvp in p_KeyValuePairs) // Write each named value in turn
                    {
                        Ct += 1;
                        TL.LogMessage("  Writing Value", "  " + Ct.ToString() + " " + kvp.Key + " = " + kvp.Value);
                        if (kvp.Value is null)
                            TL.LogMessage("  Writing Value", "  WARNING - Suppplied Value is Nothing not empty string");
                        switch (kvp.Key ?? "")
                        {
                            case COLLECTION_DEFAULT_VALUE_NAME: // Ignore the default value entry
                                                                // Write everything else to the file
                                {
                                    break;
                                }

                            default:
                                {
                                    Writer.WriteStartElement(VALUE_ELEMENT_NAME); // Write the element name
                                    Writer.WriteAttributeString(NAME_ATTRIBUTE_NAME, kvp.Key); // Write the name attribute
                                    Writer.WriteAttributeString(VALUE_ATTRIBUTE_NAME, kvp.Value); // Write the value attribute
                                    Writer.WriteEndElement(); // Close this element
                                    break;
                                }
                        }
                    }
                    Writer.WriteEndElement();

                    // Flush and close the writer object to complete writing of the XML file. 
                    Writer.Close(); // Actualy write the XML to a file
                }
                try
                {
                    FStream.Flush();
                    FStream.Close();
                    FStream.Dispose();
                    FStream = null;
                }
                catch (Exception) // Ensure no error occur from this tidying up
                {
                }

                Writer = null;
                try // New file successfully created so now rename the current file to original and rename the new file to current
                {
                    if (p_CheckForCurrentProfileStore) // Check for existence for current profile store if required
                    {
                        FileStore.Rename(p_SubKeyName + @"\" + VALUES_FILENAME, p_SubKeyName + @"\" + VALUES_FILENAME_ORIGINAL);
                    }
                    try
                    {
                        FileStore.Rename(p_SubKeyName + @"\" + VALUES_FILENAME_NEW, p_SubKeyName + @"\" + VALUES_FILENAME);
                    }
                    catch (Exception ex2)
                    {
                        // Attempt to rename new file as current failed so try and restore the original file
                        TL.Enabled = true;
                        TL.LogMessage("XMLAccess:WriteValues", "Unable to rename new profile file to current - " + p_SubKeyName + @"\" + VALUES_FILENAME_NEW + "to " + p_SubKeyName + @"\" + VALUES_FILENAME + " " + ex2.ToString());
                        try
                        {
                            FileStore.Rename(p_SubKeyName + @"\" + VALUES_FILENAME_ORIGINAL, p_SubKeyName + @"\" + VALUES_FILENAME);
                        }
                        catch (Exception ex3)
                        {
                            // Restoration also failed so no clear recovery from this point
                            TL.Enabled = true;
                            TL.LogMessage("XMLAccess:WriteValues", "Unable to rename original profile file to current - " + p_SubKeyName + @"\" + VALUES_FILENAME_ORIGINAL + "to " + p_SubKeyName + @"\" + VALUES_FILENAME + " " + ex3.ToString());
                        }
                    }
                }
                catch (Exception ex1)
                {
                    // No clear remedial action as the current file rename failed so just leave as is
                    TL.Enabled = true;
                    TL.LogMessage("XMLAccess:WriteValues", "Unable to rename current profile file to original - " + p_SubKeyName + @"\" + VALUES_FILENAME + "to " + p_SubKeyName + @"\" + VALUES_FILENAME_ORIGINAL + " " + ex1.ToString());
                }

                WriterSettings = null;

                swSupport.Stop();
                TL.LogMessage("  WriteValues", "  Created cache entry " + p_SubKeyName + " - " + swSupport.ElapsedMilliseconds + " milliseconds");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("  WriteValues", "  Exception " + p_SubKeyName + " " + ex.ToString());
                MessageBox.Show("XMLAccess:Writevalues " + p_SubKeyName + " " + ex.ToString());
            }
        }

        private int _MigrateKey_RecurseDepth = default;

        private void MigrateKey(RegistryKey p_FromKey, string p_ToDir)
        {
            // Subroutine used for one off copy of registry profile to new XML profile
            string[] ValueNames, SubKeyNames;
            RegistryKey FromKey;
            var Values = new SortedList<string, string>();
            // Recusively copy contents from one key to the other
            Stopwatch swLocal;

            _MigrateKey_RecurseDepth += 1; // Increment the recursion depth indicator

            swLocal = Stopwatch.StartNew();
            TL.LogMessage("MigrateKeys " + _MigrateKey_RecurseDepth.ToString(), "To Directory: " + p_ToDir);
            try
            {
                TL.LogMessage("MigrateKeys" + _MigrateKey_RecurseDepth.ToString(), "From Key: " + p_FromKey.Name + ", SubKeyCount: " + p_FromKey.SubKeyCount.ToString() + ", ValueCount: " + p_FromKey.ValueCount.ToString());
            }
            catch (Exception ex)
            {
                TL.LogMessage("MigrateKeys", "Exception processing \"" + p_ToDir + "\": " + ex.ToString());
                TL.LogMessage("MigrateKeys", "Exception above: no action taken, continuing...");
            }

            // First copy values from the from key to the to key
            ValueNames = p_FromKey.GetValueNames();
            Values.Add(COLLECTION_DEFAULT_VALUE_NAME, COLLECTION_DEFAULT_UNSET_VALUE);
            foreach (string ValueName in ValueNames)
            {
                if (string.IsNullOrEmpty(ValueName)) // Deal with case where the customer has specified a value for the deault value
                {
                    Values.Remove(COLLECTION_DEFAULT_VALUE_NAME); // Remove the default unset value and replace with actual value
                    Values.Add(COLLECTION_DEFAULT_VALUE_NAME, p_FromKey.GetValue(ValueName).ToString());
                }
                else
                {
                    Values.Add(ValueName, p_FromKey.GetValue(ValueName).ToString());
                }

            }
            WriteValues(p_ToDir, ref Values); // Write values to XML file

            // Now process the keys
            SubKeyNames = p_FromKey.GetSubKeyNames();
            foreach (string SubKeyName in SubKeyNames)
            {
                FromKey = p_FromKey.OpenSubKey(SubKeyName); // Point at the source to copy to it
                CreateKey(p_ToDir + @"\" + SubKeyName);
                MigrateKey(FromKey, p_ToDir + @"\" + SubKeyName); // Recursively process each key
                FromKey.Close();
            }
            swLocal.Stop();
            TL.LogMessage("  ElapsedTime " + _MigrateKey_RecurseDepth.ToString(), "  " + swLocal.ElapsedMilliseconds + " milliseconds, Completed Directory: " + p_ToDir);
            _MigrateKey_RecurseDepth -= 1; // Decrement the recursion depth counter
        }

        private void GetProfileMutex(string Method, string Parameters)
        {
            // Get the profile mutex or log an error and throw an exception that will terminate this profile call and return to the calling application
            GotMutex = ProfileMutex.WaitOne(PROFILE_MUTEX_TIMEOUT, false);
            if (!GotMutex)
            {
                TL.LogMessage("GetProfileMutex", "***** WARNING ***** Timed out waiting for Profile mutex in " + Method + ", parameters: " + Parameters);
                LogEvent(Method, "Timed out waiting for Profile mutex in " + Method + ", parameters: " + Parameters, EventLogEntryType.Error, EventLogErrors.XMLProfileMutexTimeout, null);
                throw new ProfilePersistenceException("Timed out waiting for Profile mutex in " + Method + ", parameters: " + Parameters);
            }
        }
        #endregion

    }
}