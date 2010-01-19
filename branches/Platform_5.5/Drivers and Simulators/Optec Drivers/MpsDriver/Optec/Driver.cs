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
// ??-???-2009  JS          Fill in some things
// 16-Sep-2009  RBD         Help with construction and some refactoring
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
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
        string Name                 { get; set; }
        double RightAscensionOffset { get; set; }
        double DeclinationOffset    { get; set; }
        short FocusOffset           { get; set; }
        double RotationOffset       { get; set; }
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
        void SetupDialog();
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

        private static int indexCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Port"/> class.
        /// Set's the port's index value in strict order of creation.
        /// </summary>

		internal Port() 
        {
            this.Index = indexCount++;
            //Accessing the SetupDialogForm twice in a row will create two instances 
            //of the MultiPortSelector. The first one will have port numbers 0 through 3
            //the second will have ports numbers 4 through 7. I'm not sure how to prevent 
            //that so I added the following line.
            if (indexCount == MultiPortSelector.s_sNumberOfPorts) indexCount = 0;
        }								   
        internal int Index
        {
            get;  private set;
        }
		public string Name
		{
			get { return _name; }
			set 
            {  
                _name = value;
                DeviceSettings.SetName(this.Index + 1, value);
            }
		}
		public double RightAscensionOffset
		{
			get { return _rightAscensionOffset; }
			set 
            {
                _rightAscensionOffset = value;
                DeviceSettings.SetRightAscensionOffset(this.Index + 1, value);
            }
		}
		public double DeclinationOffset
		{
			get { return _declinationOffset; }
			set 
            { 
                _declinationOffset = value;
                DeviceSettings.SetDeclinationOffset(this.Index + 1, value);
            }
		}
		public short FocusOffset
		{
			get { return _focusOffset; }
			set 
            { 
                _focusOffset = value;
                DeviceSettings.SetFocusOffset(this.Index + 1, value); 
            }
		}
		public double RotationOffset
		{
			get { return _rotationOffset; }
			set 
            {
                _rotationOffset = value;
                DeviceSettings.SetRotationOffset(this.Index + 1, value);
            }
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
		internal static string s_csDriverID = "ASCOM.Optec.MultiPortSelector";
		private static string s_csDriverDescription = "Optec MultiPortSelector";
        internal static short s_sNumberOfPorts = 4;

		private ArrayList _ports;

		//
		// Constructor - Must be public for COM registration!
		//
		public MultiPortSelector()
		{
			_ports = new ArrayList();
			// Here read config, construct Port objects and add them to _ports.
            for (int i = 1; i <= s_sNumberOfPorts; i++)
            {
                Port P = new Port();
                P.Name = DeviceSettings.Name(i);
                P.RightAscensionOffset = DeviceSettings.RightAscensionOffset(i);
                P.DeclinationOffset = DeviceSettings.DeclinationOffset(i);
                P.FocusOffset = DeviceSettings.FocusOffset(i);
                P.RotationOffset = DeviceSettings.RotationOffset(i);
                _ports.Add(P);
            }
		}

		#region ASCOM Registration
		//
		// Register or unregister driver for ASCOM. This is harmless if already
		// registered or unregistered. 
		//
		private static void RegUnregASCOM(bool bRegister)
		{
            Utilities.Profile P = new Utilities.Profile();
            P.DeviceType = "MultiPortSelector";
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
			get 
            {
                if (DeviceComm.ComState == 2) return true;
                else return false;
            }
            set
            {
                if (value)  //connect
                {
                    if (DeviceComm.ComState != 2) DeviceComm.Connect();
                }
                else        //disconnect
                {
                    DeviceComm.Disconnect();
                }
            }
		}

		public short Position
		{
			// TODO Replace this with your implementation
			get 
            {
                return DeviceComm.CurrentPosition;
            }
            set { DeviceComm.CurrentPosition = value; }
		}

		public string DriverName
		{
			get 
            {
                string D_Name = "Optec MPS ASCOM Driver";
                return D_Name;
            }
		}

		public string Description
		{
			
			get 
            {
                string D_Descript = "This driver has been written in compliance with ASCOM standards!\n" +
                    "This driver is designed to control the Optec Perseus 4-Port Selector\n";
                return D_Descript;
            }
		}

		public string DriverInfo
		{ 
			get 
            {
                string D_Info = "This driver handles all commnincation to the device via the RS-232 Serial Port.\n" +
                    "This driver is designed to control the Optec Perseus 4-Port Selector\n";
                return D_Info;
            }
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
