using System;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace ASCOM.Utilities
{
    /// <summary>
    /// Class to manage state storage for platform components e.g. profile tracing enabled/disabled
    /// </summary>
    /// <remarks>
    /// To add a new saved value:
    /// 1) Decide on the variable name and its default value
    /// 2) Create appropriately named constants similar to those below
    /// 3) Create a property of the relevant type in the parameters section
    /// 4) Create Get and Set code based on the patterns already implemented
    /// 5) If the property is of a type not already handled,you will need to create a GetXXX function in the Utility code region
    /// </remarks>
    internal class UtilitiesSettings : IDisposable
    {

        private RegistryKey m_HKCU, m_SettingsKey;
        private const string REGISTRY_CONFORM_FOLDER = @"Software\ASCOM\Utilities";

        // Constants used in the Parameters section
        private const string TRACE_XMLACCESS = "Trace XMLAccess";
        private const bool TRACE_XMLACCESS_DEFAULT = false; // Enable XML Access tracing
        private const string TRACE_PROFILE = "Trace Profile";
        private const bool TRACE_PROFILE_DEFAULT = false; // Enable Profile Tracing
        private const string PROFILE_ROOT_EDIT = "Profile Root Edit";
        private const bool PROFILE_ROOT_EDIT_DEFAULT = false; // Allow root editing in Profile Explorer

        #region New, Dispose and Finalize
        public UtilitiesSettings()
        {
            m_HKCU = Registry.CurrentUser;
            m_HKCU.CreateSubKey(REGISTRY_CONFORM_FOLDER);
            m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_CONFORM_FOLDER, true);
        }

        private bool disposedValue = false;        // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                m_SettingsKey.Flush();
                m_SettingsKey.Close();
                m_SettingsKey = null;
                m_HKCU.Flush();
                m_HKCU.Close();
                m_HKCU = null;
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

        ~UtilitiesSettings()
        {
        }
        #endregion

        #region Parameters
        public bool TraceXMLAccess
        {
            get
            {
                return GetBool(TRACE_XMLACCESS, TRACE_XMLACCESS_DEFAULT);
            }
            set
            {
                SetName(m_SettingsKey, TRACE_XMLACCESS, value.ToString());
            }
        }

        public bool TraceProfile
        {
            get
            {
                return GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT);
            }
            set
            {
                SetName(m_SettingsKey, TRACE_PROFILE, value.ToString());
            }
        }

        public bool ProfileRootEdit
        {
            get
            {
                return GetBool(PROFILE_ROOT_EDIT, PROFILE_ROOT_EDIT_DEFAULT);
            }
            set
            {
                SetName(m_SettingsKey, PROFILE_ROOT_EDIT, value.ToString());
            }
        }

        #endregion

        #region Utility Code
        private bool GetBool(string p_Name, bool p_DefaultValue)
        {
            var l_Value = default(bool);
            try
            {
                if (m_SettingsKey.GetValueKind(p_Name) == RegistryValueKind.String) // Value does exist
                {
                    l_Value = Conversions.ToBoolean(m_SettingsKey.GetValue(p_Name));
                }
            }
            catch (System.IO.IOException) // Value doesn't exist so create it
            {
                SetName(m_SettingsKey, p_Name, p_DefaultValue.ToString());
                l_Value = p_DefaultValue;
            }
            catch (Exception)
            {
                // LogMsg("GetBool", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
                l_Value = p_DefaultValue;
            }
            return l_Value;
        }
        private string GetString(string p_Name, string p_DefaultValue)
        {
            string l_Value;
            l_Value = "";
            try
            {
                if (m_SettingsKey.GetValueKind(p_Name) == RegistryValueKind.String) // Value does exist
                {
                    l_Value = m_SettingsKey.GetValue(p_Name).ToString();
                }
            }
            catch (System.IO.IOException) // Value doesn't exist so create it
            {
                SetName(m_SettingsKey, p_Name, p_DefaultValue.ToString());
                l_Value = p_DefaultValue;
            }
            catch (Exception)
            {
                // LogMsg("GetString", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
                l_Value = p_DefaultValue;
            }
            return l_Value;
        }
        private double GetDouble(RegistryKey p_Key, string p_Name, double p_DefaultValue)
        {
            var l_Value = default(double);
            // LogMsg("GetDouble", GlobalVarsAndCode.MessageLevel.msgDebug, p_Name.ToString & " " & p_DefaultValue.ToString)
            try
            {
                if (p_Key.GetValueKind(p_Name) == RegistryValueKind.String) // Value does exist
                {
                    l_Value = Conversions.ToDouble(p_Key.GetValue(p_Name));
                }
            }
            catch (System.IO.IOException) // Value doesn't exist so create it
            {
                SetName(p_Key, p_Name, p_DefaultValue.ToString());
                l_Value = p_DefaultValue;
            }
            catch (Exception)
            {
                // LogMsg("GetDouble", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
                l_Value = p_DefaultValue;
            }
            return l_Value;
        }
        private DateTime GetDate(string p_Name, DateTime p_DefaultValue)
        {
            var l_Value = default(DateTime);
            try
            {
                if (m_SettingsKey.GetValueKind(p_Name) == RegistryValueKind.String) // Value does exist
                {
                    l_Value = Conversions.ToDate(m_SettingsKey.GetValue(p_Name));
                }
            }
            catch (System.IO.IOException) // Value doesn't exist so create it
            {
                SetName(m_SettingsKey, p_Name, p_DefaultValue.ToString());
                l_Value = p_DefaultValue;
            }
            catch (Exception)
            {
                // LogMsg("GetDate", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
                l_Value = p_DefaultValue;
            }
            return l_Value;
        }
        private void SetName(RegistryKey p_Key, string p_Name, string p_Value)
        {
            p_Key.SetValue(p_Name, p_Value.ToString(), RegistryValueKind.String);
            p_Key.Flush();
        }
        #endregion

    }
}