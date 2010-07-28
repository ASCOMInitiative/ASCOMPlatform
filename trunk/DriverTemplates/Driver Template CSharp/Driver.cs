//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM TEMPLATEDEVICECLASS driver for TEMPLATEDEVICENAME
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM TEMPLATEDEVICECLASS interface version: <To be completed by friver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	5.1.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;

namespace ASCOM.TEMPLATEDEVICENAME
{
	//
	// Your driver's DeviceID is ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS
	//
	// The Guid attribute sets the CLSID for ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS
	// The ClassInterface/None addribute prevents an empty interface called
	// _TEMPLATEDEVICENAME from being created and used as the [default] interface
	//

	/// <summary>
	/// ASCOM TEMPLATEDEVICECLASS Driver for TEMPLATEDEVICENAME.
	/// </summary>
	[Guid("3A02C211-FA08-4747-B0BD-4B00EB159297")]
	[ClassInterface(ClassInterfaceType.None)]
	public class TEMPLATEDEVICECLASS : ITEMPLATEDEVICECLASS
	{
		/// <summary>
		/// ASCOM DeviceID (COM ProgID) for this driver.
		/// The DeviceID is used by ASCOM applications to load the driver at runtime.
		/// </summary>
		private static string s_csDriverID = "ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS";
		// TODO Change the descriptive string for your driver then remove this line
		/// <summary>
		/// Driver description that displays in the ASCOM Chooser.
		/// </summary>
		private static string s_csDriverDescription = "ASCOM TEMPLATEDEVICECLASS Driver for TEMPLATEDEVICENAME.";


		/// <summary>
		/// Initializes a new instance of the <see cref="TEMPLATEDEVICENAME"/> class.
		/// Must be public for COM registration.
		/// </summary>
		public TEMPLATEDEVICECLASS()
		{
			//TODO: Implement your additional construction here
		}

		#region ASCOM Registration
		//
		// Register or unregister driver for ASCOM. This is harmless if already
		// registered or unregistered. 
		//
		/// <summary>
		/// Register or unregister the driver with the ASCOM Platform.
		/// This is harmless if the driver is already registered/unregistered.
		/// </summary>
		/// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
		private static void RegUnregASCOM(bool bRegister)
		{
			var P = new ASCOM.Utilities.Profile();
			P.DeviceType = "ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS";
			if (bRegister)
			{
				P.Register(s_csDriverID, s_csDriverDescription);
			}
			else
			{
				P.Unregister(s_csDriverID);
			}
			// Utilities.Profile is native .NET so no COM interfaces are involved.
			//try										// In case Helper becomes native .NET
			//{
			//    Marshal.ReleaseComObject(P);
			//}
			//catch (Exception) { }
			P = null;
		}

		/// <summary>
		/// This function registers the driver with the ASCOM Chooser and
		/// is called automatically whenever this class is registered for COM Interop.
		/// </summary>
		/// <param name="t">Type of the class being registered, not used.</param>
		/// <remarks>
		/// This method typically runs in two distinct situations:
		/// <list type="numbered">
		/// <item>
		/// In Visual Studio, when the project is successfully built.
		/// For this to work correctly, the option <c>Register for COM Interop</c>
		/// must be enabled in the project settings.
		/// </item>
		/// <item>During setup, when the installer registers the assembly for COM Interop.</item>
		/// </list>
		/// This technique should mean that it is never necessary to manually register a driver with ASCOM.
		/// </remarks>
		[ComRegisterFunction]
		public static void RegisterASCOM(Type t)
		{
			RegUnregASCOM(true);
		}

		/// <summary>
		/// This function unregisters the driver from the ASCOM Chooser and
		/// is called automatically whenever this class is unregistered from COM Interop.
		/// </summary>
		/// <param name="t">Type of the class being registered, not used.</param>
		/// <remarks>
		/// This method typically runs in two distinct situations:
		/// <list type="numbered">
		/// <item>
		/// In Visual Studio, when the project is cleaned or prior to rebuilding.
		/// For this to work correctly, the option <c>Register for COM Interop</c>
		/// must be enabled in the project settings.
		/// </item>
		/// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
		/// </list>
		/// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
		/// </remarks>
		[ComUnregisterFunction]
		public static void UnregisterASCOM(Type t)
		{
			RegUnregASCOM(false);
		}
		#endregion

		//
		// PUBLIC COM INTERFACE ITEMPLATEDEVICECLASS IMPLEMENTATION
		//

		/// <summary>
		/// Displays the Setup Dialog form.
		/// If the user clicks the OK button to dismiss the form, then
		/// the new settings are saved, otherwise the old values are reloaded.
		/// </summary>
		public void SetupDialog()
		{
			SetupDialogForm F = new SetupDialogForm();
			var result = F.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				Properties.Settings.Default.Save();
				return;
			}
			Properties.Settings.Default.Reload();
		}
	}
}
