using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Utilities;

namespace ASCOM.Controls
	{
	/// <summary>
	/// This class defines a Device Descriptor for ASCOM devices and is convenient
	/// for populating ComboBoxes with lists of ASCOM devices.
	/// </summary>
	public class DeviceDescriptor
		{
		/// <summary>
		/// Initialize a new DeviceDescriptor object.
		/// </summary>
		/// <param name="s1">DeviceID of the device (Device ID)</param>
		/// <param name="s2">Human-readable description</param>
		public DeviceDescriptor(string s1, string s2)
			{
			DeviceID = s1;
			Description = s2;
			}
		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceDescriptor"/> class
		/// from a generic <see cref="T:System.Collections.Generic.KeyValuePair{string,string}"/>.
		/// These values are typically returned from methods in <see cref="ASCOM.Utilities"/>.
		/// </summary>
		/// <param name="deviceProfileKey">The device profile key.</param>
		public DeviceDescriptor(KeyValuePair<string, string> deviceProfileKey)
			{
			DeviceID = deviceProfileKey.Key;
			Description = deviceProfileKey.Value;
			}
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDescriptor"/> class
        /// from a generic <see cref="T:ASCOM.Utilities.KeyValuePair"/>
		/// These values are typically returned from methods in <see cref="ASCOM.Utilities"/>.
        /// </summary>
        /// <param name="deviceProfileKey">The device profile key.</param>
		public DeviceDescriptor(KeyValuePair deviceProfileKey)
			{
			DeviceID = deviceProfileKey.Key;
			Description = deviceProfileKey.Value;
			}

		/// <summary>
		/// Gets or sets the ASCOM DeviceID.
		/// </summary>
		/// <value>The ASCOM DeviceID (synonymous with the COM ClsId) of the device.</value>
		public string DeviceID { get; set; }
		/// <summary>
		/// Gets or sets the description of the ASCOM device.
		/// </summary>
		/// <value>The description.</value>
		public string Description { get; set; }
		/// <summary>
		/// Returns a <see cref="T:System.String"/> containing a description of the ASCOM device.
		/// When this item is added to a <see cref="T:System.Windows.Forms.ComboBox"/> then this
		/// method determines what will be displayed in the Combo Box control.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> containing a description of the ASCOM device.
		/// </returns>
		public override string ToString()
			{
			return Description;
			}
		}
	}
