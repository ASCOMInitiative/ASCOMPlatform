using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using ASCOM.Internal;
using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;
using TiGra;

namespace ASCOM
{
    /// <summary>
    ///   Provides settings storage for ASCOM device drivers.
    ///   Settings are persisted in the ASCOM Device Profile store.
    /// </summary>
    /// <remarks>
    ///   Original version by Tim Long, March 2009.
    ///   Copyright © 2009 TiGra Astronomy, all rights reserved.
    ///   http://www.tigranetworks.co.uk/Astronomy
    /// </remarks>
    public class SettingsProvider : System.Configuration.SettingsProvider
    {
        /// <summary>
        ///   A reference to an ASCOM profile provider. Normally, this will be the default implementation defined in
        ///   <see cref = "ASCOM.Utilities.Profile" />, but unit tests can also use dependency injection to provide
        ///   a mock provider. This value will be initialized (once) in the constructor.
        /// </summary>
        readonly IProfile ascomProfile;

        /// <summary>
        ///   Backing store for the ApplicationName property.
        /// </summary>
        string appName = String.Empty;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SettingsProvider" /> class with the default
        ///   profile provider <see cref = "ASCOM.Utilities.Profile" />.
        /// </summary>
        public SettingsProvider()
        {
            ascomProfile = new Profile(true); // Ignores 'Profile not found' exceptions.
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SettingsProvider" /> class with the supplied
        ///   Profile Provider. This is useful for injecting a mock profile object during unit testing.
        /// </summary>
        /// <param name = "profileProvider">The <see cref = "IProfile" /> to be used.</param>
        public SettingsProvider(IProfile profileProvider)
        {
            ascomProfile = profileProvider;
        }

        /// <summary>
        ///   Returns the provider's friendly name used during configuration.
        /// </summary>
        public override string Name
        {
            get { return "ASCOM Settings Provider"; }
        }

        /// <summary>
        ///   Gets the provider's friendly description.
        /// </summary>
        public override string Description
        {
            get { return "Stores settings in the ASCOM Device Profile store."; }
        }

        /// <summary>
        ///   Gets the name of the driver or application for which settings are being managed.
        ///   This value is set during provider initialization and cannot be changed thereafter.
        /// </summary>
        public override string ApplicationName
        {
            get { return appName; }
            set
            {
                Diagnostics.TraceWarning("Unexpected setting of ApplicationName to {0}", value);
                appName = value;
            }
        }

        /// <summary>
        ///   Initializes the ASCOM Settings Provider.
        /// </summary>
        /// <param name = "name">Ignored.</param>
        /// <param name = "config">Not used.</param>
        /// <remarks>
        ///   This method is called by <see cref = "ApplicationSettingsBase" /> to initialize the settings provider.
        ///   Normally, this will set the provider's <see cref = "ApplicationName" /> property to the assembly name
        ///   of the application. This is later used to determine the storage location of each of the settings.
        ///   However, for ASCOM purposes, we can't use the application name and we need to key off the
        ///   ASCOM DeviceID of the driver, so in ASCOM.SettingsProvider, the application name is set but never used.
        /// </remarks>
        public override void Initialize(string name, NameValueCollection config)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            appName = assembly.GetName().Name;
            base.Initialize(name ?? appName, config);
            ApplicationName = name ?? appName;
        }

        /// <summary>
        ///   Retrieves a collection of settings values from the underlying ASCOM Profile store.
        /// </summary>
        /// <param name = "context">Not used.</param>
        /// <param name = "collection">The list of properties to be retrieved.</param>
        /// <returns>
        ///   Returns a collection of the retrieved property values as a
        ///   <see cref = "SettingsPropertyValueCollection" />.
        /// </returns>
        /// <remarks>
        ///   If any properties are absent in the underlying store, or if an error occurs while
        ///   retrieving them, then the property's default value is used.	This will be the case
        ///   if the driver has never been registered with ASCOM.
        /// </remarks>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context,
                                                                          SettingsPropertyCollection collection)
        {
            Diagnostics.TraceInfo("Retrieving ASCOM Profile Properties for DeviceID={0}, {1} properties",
                                  ApplicationName, collection.Count);
            var pvc = new SettingsPropertyValueCollection();
            foreach (SettingsProperty item in collection)
            {
                var spv = new SettingsPropertyValue(item);
                // Parse the ASCOM DeviceID or use default values.
                string deviceName = null;
                string deviceType = null;
                string deviceId;
                try
                {
                    var idAttribute = item.Attributes[typeof (DeviceIdAttribute)] as DeviceIdAttribute;
                    if (idAttribute == null)
                    {
                        Diagnostics.TraceError("Setting {0} is not decorated with a DeviceID attribute.", spv.Name);
                        continue; // Silently ignore this property value (per MS documentation).
                        //throw new ASCOM.PropertyNotAttributedException(spv.Name);
                    }
                    deviceId = idAttribute.DeviceId;
                    // Split the Device ID into a Device Name and a Device Type.
                    // eg. "ASCOM.MyDriver.Switch" --> DeviceName="ASCOM.MyDriver", DeviceType="Switch"
                    int split = deviceId.LastIndexOf('.');
                    deviceName = deviceId.Head(split);
                    deviceType = deviceId.RemoveHead(split + 1);
                    Diagnostics.TraceVerbose("Parsed DeviceID as {0}.{1}", deviceName, deviceType);
                }
                catch (Exception)
                {
                    if (String.IsNullOrEmpty(deviceName))
                        deviceName = "Unnamed";
                    if (String.IsNullOrEmpty(deviceType))
                        deviceType = "Non-Device";
                    deviceId = String.Format("{0}.{1}", deviceName, deviceType);
                    Diagnostics.TraceWarning("Unable to parse DeviceID, using {0}.{1}", deviceName, deviceType);
                }
                ascomProfile.DeviceType = deviceType;
                EnsureRegistered(ascomProfile, deviceId);
                try
                {
                    string value = ascomProfile.GetValue(deviceId, item.Name, null, String.Empty);
                    if (String.IsNullOrEmpty(value))
                    {
                        spv.SerializedValue = item.DefaultValue; //[ASCOM-256]
                        Diagnostics.TraceVerbose("Defaulted/empty ASCOM Profile DeviceID={0}, Key={1}, Value={2}",
                                                 deviceId, item.Name, item.DefaultValue.ToString());
                    }
                    else
                    {
                        spv.SerializedValue = value;
                        Diagnostics.TraceVerbose("Retrieved ASCOM Profile DeviceID={0}, Key={1}, Value={2}", deviceId,
                                                 item.Name, value);
                    }
                }
                catch
                {
                    spv.SerializedValue = spv.Property.DefaultValue;
                    Diagnostics.TraceVerbose("Defaulted/missing ASCOM Profile DeviceID={0}, Key={1}, Value={2}",
                                             deviceId, item.Name, spv.PropertyValue);
                }
                spv.IsDirty = false;
                pvc.Add(spv);
            }
            return pvc;
        }

        /// <summary>
        ///   Persists a collection of settings values to the underlying ASCOM Profile store.
        /// </summary>
        /// <param name = "context">Context to which the settings will be saved</param>
        /// <param name = "collection">Settings to be saved</param>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            Diagnostics.TraceInfo("Persisting ASCOM Profile Properties for DeviceID={0}, {1} properties",
                                  ApplicationName, collection.Count);
            foreach (SettingsPropertyValue item in collection)
            {
                if (!item.Property.Attributes.ContainsKey(typeof (DeviceIdAttribute)))
                {
                    //throw new PropertyNotAttributedException(item.Name);
                    continue; // Silently ignore, per MS documentation.
                }
                var idAttribute = item.Property.Attributes[typeof (DeviceIdAttribute)] as DeviceIdAttribute;
                if (idAttribute != null)
                {
                    string deviceId = idAttribute.DeviceId;
                    // Split the Device ID into a Device Name and a Device Type.
                    // eg. "ASCOM.MyDriver.Switch" --> DeviceName="ASCOM.MyDriver", DeviceType="Switch"
                    int split = deviceId.LastIndexOf('.');
                    string deviceName = deviceId.Head(split);
                    string deviceType = deviceId.RemoveHead(split + 1);
                    ascomProfile.DeviceType = deviceType;
                    EnsureRegistered(ascomProfile, deviceId);
                    try
                    {
                        Diagnostics.TraceVerbose("Writing ASCOM Profile DeviceID={0}, Key={1}, Value={2}", deviceId,
                                                 item.Name, item.SerializedValue);
                        ascomProfile.WriteValue(deviceId, item.Name, item.SerializedValue.ToString(), String.Empty);
                    }
                    catch
                    {
                        Diagnostics.TraceError("Failed to persist property Key={0} - make sure your driver is properly registered", item.Name);
                    }
                }
                else
                {
                    Diagnostics.TraceWarning("Property name {0} did not have a DeviceId attribute", item.Name);
                }
            }
        }

        /// <summary>
        ///   Checks whether the driver is registered with ASCOM and, if not, registers it.
        /// </summary>
        /// <param name = "ascomProfile">
        ///   A reference to a <see cref = "Profile" /> object
        ///   that is used to query the ASCOM Device Profile.
        /// </param>
        /// <param name = "driverId">The full ASCOM DeviceID to be verified.</param>
        static void EnsureRegistered(IProfile ascomProfile, string driverId)
        {
            if (!ascomProfile.IsRegistered(driverId))
            {
                ascomProfile.Register(driverId, driverId + " Auto-registered by SettingsProvider");
                Diagnostics.TraceWarning("Your driver has been auto-registered with ASCOM.Utilities.profile for easy debugging. You must provide a correct registration in your setup before deploying to an end user system.");
            }
        }
    }
}