//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM.SwitchSimulator driver for TEMPLATEDEVICENAME
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM.SwitchSimulator interface version: <To be completed by friver developer>
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Utilities;
//using ASCOM.Interface;

namespace ASCOM.SwitchSimulator
{
	/// <summary>
    /// ASCOM.SwitchSimulator Driver for Switch.
	/// </summary>
	[Guid("28D679BA-2AF1-4557-AE15-C528C5BF91E0")]
	[ClassInterface(ClassInterfaceType.None)]
    public class Switch : ReferenceCountedObjectBase, ISwitch
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="Switch"/> class.
		/// Must be public for COM registration.
		/// </summary>
		public Switch()
		{
			//TODO: Implement your additional construction here
		}

		//
		// PUBLIC COM INTERFACE ISwitch IMPLEMENTATION
		//
        #region ISwitch Members

        public byte Id
        {
            get { return SwitchHardware.Id; }
            set { SwitchHardware.Id = value; }
        }

        public string Name
        {
            get { return SwitchHardware.Name; }
            set { SwitchHardware.Name = value; }
        }

        public bool State
        {
            get { return SwitchHardware.State; }
            set { SwitchHardware.State = value; }
        }

        #endregion
    }
}
