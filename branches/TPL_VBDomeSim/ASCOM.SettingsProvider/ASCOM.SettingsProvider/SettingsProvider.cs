//tabs=4
// --------------------------------------------------------------------------------
// ASCOM.SettingsProvider class.
// Provides persistent storage for ASCOM Device Driver settings and enables
// integration with the Visual Studio Designers.
//
// Copyright © 2009 Timothy P. Long, TiGra Astronomy.
// http://www.tigranetworks.co.uk/Astronomy
// 
//* Permission is hereby granted, free of charge, to any person obtaining
//* a copy of this software and associated documentation files (the
//* "Software"), to deal in the Software without restriction, including
//* without limitation the rights to use, copy, modify, merge, publish,
//* distribute, sublicense, and/or sell copies of the Software, and to
//* permit persons to whom the Software is furnished to do so, subject to
//* the following conditions:
//* 
//* The above copyright notice and this permission notice shall be
//* included in all copies or substantial portions of the Software.
//* 
//* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//* LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//* OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//* WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//
// Implements:	SettingsProvider
// Author:		(TPL) Timothy P. Long <Tim@tigranetworks.co.uk>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 10-Mar-2009	TPL	5.0.*	Initial edit.
// --------------------------------------------------------------------------------
//
using System;
using System.Configuration;
using System.Reflection;
using ASCOM.HelperNET;
using TiGra.ExtensionMethods;
using TiGra;

namespace ASCOM
	{
	/// <summary>
	/// Provides settings storage for ASCOM device drivers.
	/// Settings are persisted in the ASCOM Device Profile store.
	/// </summary>
	/// <remarks>
	/// Original version by Tim Long, March 2009.
	/// Copyright © 2009 TiGra Astronomy, all rights reserved.
	/// http://www.tigranetworks.co.uk/Astronomy
	/// </remarks>
	public class SettingsProvider : System.Configuration.SettingsProvider
		{
		//static Profile ascomProfile = new Profile();

		/// <summary>
		/// Returns the provider's friendly name used during configuration.
		/// </summary>
		public override string Name
			{
			get
				{
				return "ASCOM Settings Provider";
				}
			}
		/// <summary>
		/// Gets the provider's friendly description.
		/// </summary>
		public override string Description
			{
			get
				{
				return "Stores settings in the ASCOM Device Profile store.";
				}
			}
		/// <summary>
		/// Backing store for the ApplicationName property.
		/// </summary>
		private string appName = String.Empty;
		/// <summary>
		/// Gets the name of the driver or application for which settings are being managed.
		/// This value is set during provider initialization and cannot be changed thereafter.
		/// </summary>
		public override string ApplicationName
			{
			get
				{
				return appName;
				}
			set
				{
				// Do nothing.
				}
			}
		/// <summary>
		/// Initializes the ASCOM Settings Provider.
		/// </summary>
		/// <param name="name">Ignored.</param>
		/// <param name="config">Not used.</param>
		/// <remarks>
		/// The value of <see cref="ApplicationName"/> is determined here by reflection. This value
		/// should correspond to an ASCOM DeviceID and this is passed to the underlying
		/// ASCOM Helper assembly when reading and writing settings values.	Determining the correct
		/// value for <see cref="ApplicationName"/> is a tricky procedure as there is a very loose
		/// coupling between the settings class and the settings provider.   The way we discover
		/// the correct value is to walk up the call stack until we find an assembly name
		/// beginning with "ASCOM.".
		/// </remarks>
		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
			{
			appName = Assembly.GetEntryAssembly().GetName().Name;
			base.Initialize(appName, config);
			//EnsureRegistered(appName);	// Ensure the driver is registered with ASCOM Chooser.
			}
		/// <summary>
		/// Retrieves a collection of settings values from the underlying ASCOM Profile store.
		/// </summary>
		/// <param name="context">Not used.</param>
		/// <param name="collection">The list of properties to be retrieved.</param>
		/// <returns>
		/// Returns a collection of the retrieved property values as a
		/// <see cref="SettingsPropertyValueCollection"/>.
		/// </returns>
		/// <remarks>
		/// If any properties are absent in the underlying store, or if an error occurs while
		/// retrieving them, then the property's default value is used.
		/// </remarks>
		public override System.Configuration.SettingsPropertyValueCollection GetPropertyValues(System.Configuration.SettingsContext context, System.Configuration.SettingsPropertyCollection collection)
			{
			Diagnostics.TraceInfo("Retrieving ASCOM Profile Properties for DeviceID={0}, {1} properties", ApplicationName, collection.Count);
			SettingsPropertyValueCollection pvc = new SettingsPropertyValueCollection();
			foreach (SettingsProperty item in collection)
				{
				SettingsPropertyValue spv = new SettingsPropertyValue(item);
				// Parse the ASCOM DeviceID or use default values.
				string deviceName = null;
				string deviceType = null;
				string deviceId;
				try
					{
					DeviceIdAttribute idAttribute = item.Attributes[typeof(DeviceIdAttribute)] as DeviceIdAttribute;
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
					deviceId = deviceName + "." + deviceType;
					Diagnostics.TraceWarning("Unable to parse DeviceID, using {0}.{1}", deviceName, deviceType);
					}
				Profile ascomProfile = new Profile();
				ascomProfile.DeviceType = deviceType;
				try
					{
					string value = ascomProfile.GetValue(deviceId, item.Name, String.Empty);
					if (String.IsNullOrEmpty(value))
						{
						spv.PropertyValue = item.DefaultValue;
						Diagnostics.TraceVerbose("Defaulted/empty ASCOM Profile DeviceID={0}, Key={1}, Value={2}", deviceId, item.Name, item.DefaultValue.ToString());
						}
					else
						{
						spv.SerializedValue = value;
						Diagnostics.TraceVerbose("Retrieved ASCOM Profile DeviceID={0}, Key={1}, Value={2}", deviceId, item.Name, value);
						}
					}
				catch
					{
					spv.PropertyValue = spv.Property.DefaultValue;
					Diagnostics.TraceVerbose("Defaulted/missing ASCOM Profile DeviceID={0}, Key={1}, Value={2}", deviceId, item.Name, spv.PropertyValue);
					}
				spv.IsDirty = false;
				pvc.Add(spv);
				}
			return pvc;
			}
		/// <summary>
		/// Persists a collection of settings values to the underlying ASCOM Profile store.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="collection"></param>
		public override void SetPropertyValues(System.Configuration.SettingsContext context, System.Configuration.SettingsPropertyValueCollection collection)
			{
			Diagnostics.TraceInfo("Persisting ASCOM Profile Properties for DeviceID={0}, {1} properties", ApplicationName, collection.Count);
			foreach (SettingsPropertyValue item in collection)
				{
				//ToDo: better error checking and handling needed below.
				DeviceIdAttribute idAttribute = item.Property.Attributes[typeof(DeviceIdAttribute)] as DeviceIdAttribute;
				string deviceId = idAttribute.DeviceId;
				// Split the Device ID into a Device Name and a Device Type.
				// eg. "ASCOM.MyDriver.Switch" --> DeviceName="ASCOM.MyDriver", DeviceType="Switch"
				int split = deviceId.LastIndexOf('.');
				string deviceName = deviceId.Head(split);
				string deviceType = deviceId.RemoveHead(split + 1);
				Profile ascomProfile = new Profile();
				ascomProfile.DeviceType = deviceType;
				try
					{
					Diagnostics.TraceVerbose("Writing ASCOM Profile DeviceID={0}, Key={1}, Value={2}", deviceId, item.Name, item.SerializedValue);
					ascomProfile.WriteValue(deviceId, item.Name, item.SerializedValue.ToString(), String.Empty);
					}
				catch
					{
					Diagnostics.TraceError("Failed to persist property Key={0}", item.Name);
					}
				}
			}
		/// <summary>
		/// Checks whether the driver is registered with ASCOM and, if not, registers it.
		/// </summary>
		/// <param name="ascomProfile">
		/// A reference to a <see cref="Profile"/> object
		/// that is used to query the ASCOM Device Profile.
		/// </param>
		/// <param name="DriverID">The full ASCOM DeviceID to be verified.</param>
		private void EnsureRegistered(Profile ascomProfile, string DriverID)
			{
			if (!ascomProfile.IsRegistered(DriverID))
				{
				ascomProfile.Register(DriverID, "Auto-registered");
				}
			}
		}
	}
