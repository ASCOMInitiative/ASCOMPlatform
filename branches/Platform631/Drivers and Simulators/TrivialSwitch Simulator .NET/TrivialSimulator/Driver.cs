//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Switch driver for TrivialSimulator
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Switch interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Switch Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;

using ASCOM;
using Helper = ASCOM.HelperNET;
using ASCOM.Interface;
using System.Reflection;

namespace ASCOM.TrivialSimulator
	{
	//
	// Your driver's ID is ASCOM.TrivialSimulator.Switch
	//
	// The Guid attribute sets the CLSID for ASCOM.TrivialSimulator.Switch
	// The ClassInterface/None addribute prevents an empty interface called
	// _Switch from being created and used as the [default] interface
	//
	[Guid("6e391152-a698-44b6-bae7-d23438c50f2e")]
	[ClassInterface(ClassInterfaceType.None)]
	public class Switch : ISwitch
		{
		//
		// Driver ID and descriptive string that shows in the Chooser
		//
		private static string s_csDriverID = "ASCOM.TrivialSimulator.Switch";
		// TODO Change the descriptive string for your driver then remove this line
		private static string s_csDriverDescription = "TrivialSimulator Switch";
		private const int numSwitches = 8;
		private SwitchStatus GUI = null;

		/// <summary>
		/// Local storage for simulated switchStates.
		/// </summary>
		//internal List<bool> switchStates = new List<bool>(numSwitches);
		//internal List<string> switchNames = new List<string>(numSwitches);

		public SwitchCollection switches = new SwitchCollection(numSwitches);

		//
		// Constructor - Must be public for COM registration!
		//
		public Switch()
			{
			GUI = new SwitchStatus(this.switches);
			GUI.Show();
			}

		#region ASCOM Registration
		//
		// Register or unregister driver for ASCOM. This is harmless if already
		// registered or unregistered. 
		//
		private static void RegUnregASCOM(bool bRegister)
			{
			Helper.Profile P = new Helper.Profile();
			P.DeviceType = "Switch";					//  Requires Helper 5.0.3 or later
			if (bRegister)
				P.Register(s_csDriverID, s_csDriverDescription);
			else
				P.Unregister(s_csDriverID);
			try										// In case Helper becomes native .NET
				{
				Marshal.ReleaseComObject(P);
				}
			catch (Exception) { }
			P = null;
			}

		[ComRegisterFunction]
		public static void RegisterASCOM(Type t)
			{
			RegUnregASCOM(true);
			}

		[ComUnregisterFunction]
		public static void UnregisterASCOM(Type t)
			{
			RegUnregASCOM(false);
			}
		#endregion

		//
		// PUBLIC COM INTERFACE ISwitch IMPLEMENTATION
		//

		#region ISwitch Members

		public bool GetSwitch(short ID)
			{
			return switches[ID].State;	// My switchStates are always on
			}
		public void SetSwitch(short ID, bool State)
			{
			switches[ID].State = State;
			}
		public string GetSwitchName(short ID)
			{
			return switches[ID].Name;
			}
		public void SetSwitchName(short ID, string Name)
			{
			switches[ID].Name = Name;
			}
		public bool Connected  {get; set;}
		public string Description
			{
			get { return s_csDriverDescription; }
			}
		public string DriverInfo
			{
			get { return s_csDriverDescription; }
			}
		public string DriverVersion
			{
			get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
			}
		public short InterfaceVersion
			{
			get { return 1; }
			}
		public short MaxSwitch
			{
			get { return (short)switches.Count; }
			}
		public string Name
			{
			get { return s_csDriverDescription; }
			}
		public void SetupDialog()
			{
			SetupDialogForm F = new SetupDialogForm();
			F.ShowDialog();
			}
		#endregion
		}
	}
