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
using ASCOM.Utilities;
using ASCOM.Interface;
using ASCOM.OptecFocuserHubTools;

namespace ASCOM.OptecFocuserHub
{
    //
    // Your driver's ID is ASCOM.OptecFocuserHub.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.OptecFocuserHub.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Focuser from being created and used as the [default] interface
    //
    [Guid("e1028dba-d482-4620-8142-dd1385d0661a")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : ReferenceCountedObjectBase, ASCOM.Interface.IFocuser
    {
        private FocuserManager myFocuserManager;
       
        //
        // Constructor - Must be public for COM registration!
        //
        public Focuser()
        {
            myFocuserManager = ASCOM.OptecFocuserHub.SharedResources.SharedFocuserManager;
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
            myFocuserManager.Focuser1.Halt();
        }

        public bool IsMoving
        {
            // TODO Replace this with your implementation
            get { return myFocuserManager.Focuser1.IsMoving || myFocuserManager.Focuser1.IsHoming; }
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
            get { return myFocuserManager.Focuser1.MaxPosition; }
        }

        public void Move(int val)
        {
            // TODO Replace this with your implementation
            myFocuserManager.Focuser1.MoveAbsolute(val);
        }

        public int Position
        {
            // TODO Replace this with your implementation
            get { return myFocuserManager.Focuser1.CurrentPositionSteps; }
        }

        public void SetupDialog()
        {
            SetupDialogForm sdf = new SetupDialogForm();
            sdf.ShowDialog();
        }

        public double StepSize
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("StepSize", false); }
        }

        public bool TempComp
        {
            get { return myFocuserManager.Focuser1.TempCompEnabled; }
            set 
            {
                if (!myFocuserManager.Focuser1.TempProbeAttached)
                    throw new ApplicationException("Cannot enable Temp Comp when probe is not attached.");
                myFocuserManager.Focuser1.TempCompEnabled = value; 
            }
        }

        public bool TempCompAvailable
        {
            get { return myFocuserManager.Focuser1.TempProbeAttached; }
        }

        public double Temperature
        {
            // TODO Replace this with your implementation
            get
            {
                return myFocuserManager.Focuser1.CurrentTempC;
            }
        }

        #endregion
    }
}
