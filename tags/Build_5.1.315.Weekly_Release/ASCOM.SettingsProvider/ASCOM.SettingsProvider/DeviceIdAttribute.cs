using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM
	{
	/// <summary>
	/// An attribute for specifying an associated ASCOM DeviceID.
	/// This is intended primarily for use with the <see cref="ASCOM.SettingsProvider"/> class.
	/// This attribute is placed on the driver's <c>Properties.Settings</c> class and
	/// informs the <see cref="ASCOM.SettingsProvider"/> which DeviceID to store settings under.
	/// </summary>
	[global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	public sealed class DeviceIdAttribute : Attribute
		{
		// See the attribute guidelines at 
		//  http://go.microsoft.com/fwlink/?LinkId=85236
		readonly string deviceId;

		/// <summary>
		/// Construct a DeviceIdAttribute from teh supplied string.
		/// </summary>
		/// <param name="deviceId">A string containing the ASCOM DeviceID to be referenced.</param>
		public DeviceIdAttribute(string deviceId)
			{
			this.deviceId = deviceId;
			}
	   /// <summary>
	   /// Gets the ASCOM DeviceID that the attribute is associated with.
	   /// </summary>
		public string DeviceId
			{
			get
				{
				return deviceId;
				}
			}
		}
	}
