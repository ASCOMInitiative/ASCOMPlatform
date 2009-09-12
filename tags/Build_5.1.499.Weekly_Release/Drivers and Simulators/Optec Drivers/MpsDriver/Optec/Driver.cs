//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM MultiPortSelector driver for Optec
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM MultiPortSelector interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 25-Aug-2009	RBD	1.0.0	Initial edit, from ASCOM FilterWheel Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Helper;
using ASCOM.Helper2;
using ASCOM.Interface;

namespace ASCOM.Optec
{

	//
	// COM-Visible interfaces: These will eventually be rolled into
	// the ASCOM Master interfaces. For now you're in control!
	//
	[Guid("4843F695-0253-4e02-8CC8-1D9A07C8A959")]						// Force InterfaceID for stability (typ.)
	public interface IPort
	{
		string Name					{ get; }
		double RightAscensionOffset { get; }
		double DeclinationOffset	{ get; }
		short FocusOffset			{ get; }
		double RotationOffset		{ get; }
	}

	[Guid("B8B816DC-D5E5-4a26-8649-ADEF40A3BBB1")]
	public interface IMultiPortSelector
	{
		bool Connected				{ get; set; }
		short Position				{ get; set; }
		string DriverName			{ get; }
		string Description			{ get; }
		string DriverInfo			{ get; }
		ArrayList Ports				{ get; }
	}

	//
	// Port class - this is what's in the ArrayList 'Ports'.
	//
	// The Guid attribute sets the CLSID for ASCOM.Optec.MultiPortSelector
	// The ClassInterface.None addribute prevents an empty interface called
	// _FilterWheel from being created and used as the [default] interface
	//
	[Guid("7B6E87C8-CA14-42EC-B6CE-DD8D1063D381")]
	[ClassInterface(ClassInterfaceType.None)]
	public class Port : IPort
	{
		private string _name;
		private double _rightAscensionOffset;
		private double _declinationOffset;
		private short _focusOffset;
		private double _rotationOffset;						// Can't be a Short

		private Port() { }									// Prevent creation of this object

		public string Name									// Port.PortName is redundant, Port.Name seems better?
		{
			get { return _name; }
			set { _name = value; }
		}
		public double RightAscensionOffset
		{
			get { return _rightAscensionOffset; }
			set { _rightAscensionOffset = value; }
		}
		public double DeclinationOffset
		{
			get { return _declinationOffset; }
			set { _declinationOffset = value; }
		}
		public short FocusOffset
		{
			get { return _focusOffset; }
			set { _focusOffset = value; }
		}
		public double RotationOffset
		{
			get { return _rotationOffset; }
			set { _rotationOffset = value; }
		}
	}

	//
	// Your driver's ID is ASCOM.Optec.MultiPortSelector
	//
	// The Guid attribute sets the CLSID for ASCOM.Optec.MultiPortSelector
	// The ClassInterface/None addribute prevents an empty interface called
	// _FilterWheel from being created and used as the [default] interface
	//
	[Guid("2dcb0232-7909-462c-879f-6802a6ca2830")]
	[ClassInterface(ClassInterfaceType.None)]
	public class MultiPortSelector : IMultiPortSelector
	{
		//
		// Driver ID and descriptive string that shows in the Chooser
		//
		private static string s_csDriverID = "ASCOM.Optec.MultiPortSelector";
		private static string s_csDriverDescription = "Optec MultiPortSelector";

		private ArrayList _ports;

		//
		// Constructor - Must be public for COM registration!
		//
		public MultiPortSelector()
		{
			_ports = new ArrayList();
			// Here read config, construct Port objects and add them to _ports.

            short NumberOfPorts = 4;

            //Port P0 = new Port();   //
            //Port P1 = new Port();     //    One for each physical  
            //Port P2 = new Port();     //    port on the device.
            //Port P3 = new Port();   //

            //_ports.Add(P0);
            //_ports.Add(P1);
            //_ports.Add(P2);
            //_ports.Add(P3);

            //string[] PNames = new string[NumberOfPorts];					
            //double[] RAOffsets = new double[NumberOfPorts]; 
            //double[] DecOffsets = new double[NumberOfPorts];	
            //short[] FocusOffsets = new short[NumberOfPorts];
            //double[] RotationOffsets = new double[NumberOfPorts];

            //PNames = DeviceSettings.RetrievePNames(NumberOfPorts);
            //RAOffsets = DeviceSettings.RetrieveRAOffsets(NumberOfPorts);
            //DecOffsets = DeviceSettings.RetrieveDecOffsets(NumberOfPorts);
            //FocusOffsets = DeviceSettings.RetrieveFocusOffsets(NumberOfPorts);
            //RotationOffsets = DeviceSettings.RetrieveRotationOffsets(NumberOfPorts);

            //foreach (Port i in _ports)
            //{
            //    i.Name = PNames[i];
            //    i.RightAscensionOffset = RAOffsets[i];
            //    i.DeclinationOffset = DecOffsets[i];
            //    i.FocusOffset = FocusOffsets[i];
            //    i.RotationOffset = RotationOffsets[i];
            //}

		}

		#region ASCOM Registration
		//
		// Register or unregister driver for ASCOM. This is harmless if already
		// registered or unregistered. 
		//
		private static void RegUnregASCOM(bool bRegister)
		{
			Helper.Profile P = new Helper.Profile();
			P.DeviceTypeV = "MultiPortSelector";					//  Requires Helper 5.0.3 or later
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
		// PUBLIC COM INTERFACE IMultiPortSelector IMPLEMENTATION
		//

		#region IMultiPortSelector Members
		public bool Connected
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Connected", false); }
			set { throw new PropertyNotImplementedException("Connected", true); }
		}

		public short Position
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Position", false); }
			set { throw new PropertyNotImplementedException("Position", true); }
		}

		public string DriverName
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("DriverName", false); }
		}

		public string Description
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Description", false); }
		}

		public string DriverInfo
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("DriverInfo", false); }
		}

		public ArrayList Ports
		{
			get { return _ports; }
		}

		public void SetupDialog()
		{
			SetupDialogForm F = new SetupDialogForm();
			F.ShowDialog();
		}

		#endregion
	}
}
