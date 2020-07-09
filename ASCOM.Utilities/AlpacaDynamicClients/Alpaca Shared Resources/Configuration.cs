using System;
using System.Globalization;
using Microsoft.Win32;

namespace ASCOM.Remote
{
    class Configuration : IDisposable
    {
        RegistryKey baseRegistryKey;

        public Configuration()
        {

            try
            {
                if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "Configuration New", "About to create base key");
                baseRegistryKey = SharedConstants.ASCOM_REMOTE_CONFIGURATION_HIVE.CreateSubKey(SharedConstants.REMOTE_DEVICE_CONFIGURATION_KEY);
                if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "Configuration New", "Created base key: " + baseRegistryKey.Name);
            }
            catch (Exception ex)
            {
                ServerForm.LogException(0, 0, 0, "Configuration New", ex.ToString());
            }
        }

        public T GetValue<T>(string KeyName, string SubKey, T DefaultValue)
        {
            if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", string.Format("Getting {0} value '{1}' in subkey '{2}', default: '{3}'", typeof(T).Name, KeyName, SubKey, DefaultValue.ToString()));
            if (typeof(T) == typeof(bool))
            {
                string registryValue = "RegistryTestValue1";
                if (SubKey == "")
                {
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "SubKey is empty so getting value directly");
                    registryValue = (string)baseRegistryKey.GetValue(KeyName);
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "Value retrieved OK: " + registryValue);
                }
                else
                {
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "SubKey has a value so using it...");
                    registryValue = (string)baseRegistryKey.CreateSubKey(SubKey).GetValue(KeyName);
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "Value retrieved OK: " + registryValue);
                }

                if (registryValue == null)
                {
                    SetValue<T>(KeyName, SubKey, DefaultValue);
                    registryValue = DefaultValue.ToString();
                }
                bool RetVal = Convert.ToBoolean(registryValue, CultureInfo.InvariantCulture);
                if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", string.Format("Retrieved {0} = {1}", KeyName, RetVal.ToString()));
                return (T)((object)RetVal);
            }

            if (typeof(T) == typeof(string))
            {
                //string RetVal = (string)baseRegistryKey.CreateSubKey(SubKey, true).GetValue(KeyName);
                string RetVal = "RegistryTestValue2";
                if (SubKey == "")
                {
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "SubKey is empty so getting value directly");
                    RetVal = (string)baseRegistryKey.GetValue(KeyName);
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "Value retrieved OK: " + RetVal);
                }
                else
                {
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "SubKey has a value so using it...");
                    RetVal = (string)baseRegistryKey.CreateSubKey(SubKey).GetValue(KeyName);
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "Value retrieved OK: " + RetVal);
                }

                if (RetVal == null)
                {
                    SetValue<T>(KeyName, SubKey, DefaultValue);
                    RetVal = DefaultValue.ToString();
                }
                if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", string.Format("Retrieved {0} = {1}", KeyName, RetVal.ToString()));
                return (T)((object)RetVal);
            }

            if (typeof(T) == typeof(decimal))
            {
                string registryValue = "RegistryTestValue2";
                if (SubKey == "")
                {
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "SubKey is empty so getting value directly");
                    registryValue = (string)baseRegistryKey.GetValue(KeyName);
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "Value retrieved OK: " + registryValue);
                }
                else
                {
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "SubKey has a value so using it...");
                    registryValue = (string)baseRegistryKey.CreateSubKey(SubKey).GetValue(KeyName);
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "Value retrieved OK: " + registryValue);
                }

                if (registryValue == null)
                {
                    SetValue<T>(KeyName, SubKey, DefaultValue);
                    registryValue = DefaultValue.ToString();
                }
                decimal RetVal = Convert.ToDecimal(registryValue, CultureInfo.InvariantCulture);
                if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", string.Format("Retrieved {0} = {1}", KeyName, RetVal.ToString()));
                return (T)((object)RetVal);
            }

            if (typeof(T) == typeof(Int32))
            {
                string registryValue = "RegistryTestValue3";
                if (SubKey == "")
                {
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "SubKey is empty so getting value directly");
                    registryValue = (string)baseRegistryKey.GetValue(KeyName);
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "Value retrieved OK: " + registryValue);
                }
                else
                {
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "SubKey has a value so using it...");
                    registryValue = (string)baseRegistryKey.CreateSubKey(SubKey).GetValue(KeyName);
                    if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", "Value retrieved OK: " + registryValue);
                }

                if (registryValue == null)
                {
                    SetValue<T>(KeyName, SubKey, DefaultValue);
                    registryValue = DefaultValue.ToString();
                }
                Int32 RetVal = Convert.ToInt32(registryValue, CultureInfo.InvariantCulture);
                if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "GetValue", string.Format("Retrieved {0} = {1}", KeyName, RetVal.ToString()));
                return (T)((object)RetVal);
            }

            throw new DriverException("GetValue: Unknown type: " + typeof(T).Name);
        }

        public void SetValue<T>(string KeyName, string SubKey, T Value)
        {
            if (ServerForm.DebugTraceState) ServerForm.LogMessage(0, 0, 0, "SetValue", string.Format("Setting {0} value '{1}' in subkey '{2}' to: '{3}'", typeof(T).Name, KeyName, SubKey, Value.ToString()));

            if (SubKey == "") baseRegistryKey.SetValue(KeyName, Value.ToString());
            else baseRegistryKey.CreateSubKey(SubKey).SetValue(KeyName, Value.ToString());
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Configuration() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
