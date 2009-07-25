using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using Microsoft.Win32;
using System.Diagnostics;
using Helper = ASCOM.HelperNET;

namespace ASCOM.Controls
	{
	/// <summary>
	/// Most Recently Used list for the Chooser.
	/// </summary>
	/// <remarks>
	/// The MRU list remembers which components where last used by the user. Each time the chooser
	/// is used, the MRU list is updated. This functionality is encapsulated in its own class so that
	/// In this implementation, the data happens to be stored in the ASCOM registry area.
	/// </remarks>
	public class ChooserMRU
		{
		const string csChooserMRURegKey = "Software\\ASCOM\\.net\\Chooser\\MRU";

		/// <summary>
		/// Default public constructor, sets up the default device class.
		/// </summary>
		public ChooserMRU()
			{
			MRUType = "Telescope";	// Set the default DeviceClass to Telescope.
			}

		/// <summary>
		/// Sets the DriverClass that is to be considered. If not set, defaults to "Telescope".
		/// </summary>
		public string MRUType   {get; set;}

		/// <summary>
		/// Gets or sets the MRU entry for the current device type (as specified in the
		/// <see cref="MRUType"/> property).
		/// </summary>
		public string MostRecentlyUsedDeviceID
			{
			get
				{
				try
					{
					return Properties.Settings.Default[MRUType] as string;
					}
				catch (Exception ex)
					{
					// Fail silently, but log the exception details to the debug console.
					Debug.WriteLine(ex, "ChooserMRU.MostRecentlyUsedDeviceID (get)");
					}
				return String.Empty;	// If we can't read the MRU, return the empty string.
				}
			set
				{
				try
					{
					Properties.Settings.Default[MRUType] = value;
					}
				catch (Exception ex)
					{
					// Fail silently (but log the exception details if debugging).
					Debug.WriteLine(ex, "ChooserMRU.MostRecentlyUsedDeviceID (set)");
					}
				}
			}
		}
	}
