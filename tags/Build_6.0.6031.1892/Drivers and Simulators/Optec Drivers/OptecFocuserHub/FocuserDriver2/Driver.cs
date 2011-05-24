//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for OptecFocuserHub
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Focuser Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Interface;
using ASCOM.OptecFocuserHubTools;

namespace ASCOM.OptecFocuserHub2
{
    //
    // Your driver's ID is ASCOM.OptecFocuserHub.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.OptecFocuserHub.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Focuser from being created and used as the [default] interface
    //
    [Guid("3c763f42-b927-4463-b04c-0f5debc9c131")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : ASCOM.OptecFocuserHub.ReferenceCountedObjectBase, IFocuser
    {
        FocuserManager myFocuserManager;
     
        //
        // Constructor - Must be public for COM registration!
        //
        public Focuser()
        {
            myFocuserManager = OptecFocuserHub.SharedResources.SharedFocuserManager;
        
        }

        //
        // PUBLIC COM INTERFACE IFocuser IMPLEMENTATION
        //

        #region IFocuser Members

        public bool Absolute
        {
            // TODO Replace this with your implementation
            get { return true; }
        }

        public void Halt()
        {
            myFocuserManager.Focuser2.Halt();
        }

        public bool IsMoving
        {
            // TODO Replace this with your implementation
            get { return myFocuserManager.Focuser2.IsMoving || myFocuserManager.Focuser2.IsHoming; }
        }
 
        public bool Link
        {
            // TODO Replace this with your implementation
            get { return myFocuserManager.Connected; }
            set { myFocuserManager.Connected = value; }
        }

        public int MaxIncrement
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("MaxIncrement", false); }
        }

        public int MaxStep
        {
            // TODO Replace this with your implementation
            get { return myFocuserManager.Focuser2.MaxPosition; }
        }

        public void Move(int val)
        {
            myFocuserManager.Focuser2.MoveAbsolute(val);
        }

        public int Position
        {
            get { return myFocuserManager.Focuser2.CurrentPositionSteps; }
        }

        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }

        public double StepSize
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("StepSize", false); }
        }

        public bool TempComp
        {
            get { return myFocuserManager.Focuser2.TempCompEnabled; }
            set {
                if (!myFocuserManager.Focuser2.TempProbeAttached)
                    throw new ApplicationException("Cannot enable Temp Comp when probe is not attached.");
                myFocuserManager.Focuser2.TempCompEnabled = value; }
        }

        public bool TempCompAvailable
        {
            get { return myFocuserManager.Focuser2.TempProbeAttached; }
        }

        public double Temperature
        {
            get { return myFocuserManager.Focuser2.CurrentTempC; }
        }

        #endregion
    }
}
