using Microsoft.Win32;
using System;

namespace ASCOM.DynamicRemoteClients
{
    internal static class ProfileAccess
    {
        private const string ASCOM_PROFILE_BASE_KEY = @"SOFTWARE\ASCOM\";

        public static bool GetBool(string subkey, string valueName, bool defaultValue)
        {
            RegistryKey rKey = null;

            try
            {
                // Create a writable registry key at the root of the ASCOM Profile stored in the registry: //HKLM/Software/ASCOM
                RegistryKey profileBaseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(ASCOM_PROFILE_BASE_KEY);

                if (profileBaseKey is null)
                    throw new Exception($"Profile base key did not open: {ASCOM_PROFILE_BASE_KEY}");

                // Determine whether we are accessing the Profile base key or a sub-key and set the appropriate registry key
                if (string.IsNullOrEmpty(subkey)) // Accessing the Profile base key
                    rKey = profileBaseKey;
                else // Accessing a sub-key
                    rKey = profileBaseKey.OpenSubKey(subkey);

                if (rKey is null)
                    throw new Exception($"RKey did not open: for sub-key: {subkey}");

                // Return the specified value, or the default, if no value has been set
                return Convert.ToBoolean(rKey.GetValue(valueName, defaultValue.ToString()));
            }
            finally
            {
                rKey?.Dispose();
            }
        }

        public static void SetBool(string valueName, string subkey, bool newValue)
        {
            RegistryKey rKey = null;

            try
            {
                // Create a writable registry key at the root of the ASCOM Profile stored in the registry: //HKLM/Software/ASCOM
                RegistryKey profileBaseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(ASCOM_PROFILE_BASE_KEY);

                if (profileBaseKey is null)
                    throw new Exception($"Profile base key did not open: {ASCOM_PROFILE_BASE_KEY}");
                // Determine whether we are accessing the Profile base key or a sub-key and set the appropriate registry key
                if (string.IsNullOrEmpty(subkey)) // Accessing the Profile base key
                    rKey = profileBaseKey;
                else // Accessing a sub-key
                    rKey = profileBaseKey.OpenSubKey(subkey, true);

                // Return the specified value, or the default, if no value has been set
                rKey.SetValue(valueName, newValue.ToString());
            }
            finally
            {
                rKey.Dispose();
            }
        }

    }
}
