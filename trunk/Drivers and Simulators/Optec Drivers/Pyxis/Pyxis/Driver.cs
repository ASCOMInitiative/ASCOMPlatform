//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Rotator driver for Pyxis
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Rotator interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Rotator Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Utilities;
using ASCOM.Interface;
using Optec;

namespace ASCOM.Pyxis
{
    //
    // Your driver's ID is ASCOM.Pyxis.Rotator
    //
    // The Guid attribute sets the CLSID for ASCOM.Pyxis.Rotator
    // The ClassInterface/None addribute prevents an empty interface called
    // _Rotator from being created and used as the [default] interface
    //
    [Guid("e5a8e095-1c9e-40b6-9f11-65fbc5f75901")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Rotator : IRotator
    {
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        private static string s_csDriverID = "ASCOM.Pyxis.Rotator";
        // TODO Change the descriptive string for your driver then remove this line
        private static string s_csDriverDescription = "Pyxis Rotator";
        

        //
        // Constructor - Must be public for COM registration!
        //
        public Rotator()
        {
            // Setup the Event Logger (User should manually edit XML file to change level.)
            EventLogger.LoggingLevel = XMLSettings.LoggerTraceLevel;
        }

        #region ASCOM Registration
        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
        {
            Utilities.Profile P = new Utilities.Profile();
            P.DeviceType = "Rotator";					//  Requires Helper 5.0.3 or later
            if (bRegister)
            {
                P.Register(s_csDriverID, s_csDriverDescription);
            }
            else
            {
                P.Unregister(s_csDriverID);
            }
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
        // PUBLIC COM INTERFACE IRotator IMPLEMENTATION
        //

        #region IRotator Members

        public bool CanReverse
        {
            get { return true; }
        }

        public bool Connected
        {
            // TODO Replace this with your implementation
            get 
            {
                if (OptecPyxis.CurrentDeviceState == OptecPyxis.DeviceStates.Connected) return true;
                else return false;

            }
            set 
            {
                if (value)
                {
                    try
                    {
                        if (!XMLSettings.PositionIsValid)
                        {
                            throw new ApplicationException("The Pyxis must be homed before you can connect"); 
                        }
                        OptecPyxis.Connect();

                    }
                    catch (UnauthorizedAccessException)
                    {
                        throw new ASCOM.NotConnectedException("The selected COM Port is already open or inaccessable. " + 
                            "Verify that another program is not currently using it.");
                    }
                }
                else
                {
                    OptecPyxis.Disconnect();
                }
            }
        }

        public void Halt()
        {
            // This is a trick so that we can tell what the device type of the Pyxis device in
            // the derotation program.
            string x = "";
            if (OptecPyxis.DeviceType == OptecPyxis.DeviceTypes.TwoInch) x = "2";
            else x = "3";

            throw new MethodNotImplementedException("Halt (" + x + ")");
        }

        public bool IsMoving
        {
            get 
            {
                try
                {
                    //EventLogger.LogMessage("ASCOM IsMoving =" + OptecPyxis.IsMoving.ToString(), System.Diagnostics.TraceLevel.Verbose);
                    return OptecPyxis.IsMoving;
                }
                catch(Exception ex)
                {
                    EventLogger.LogMessage(ex);
                    throw;
                }

            }
        }

        public void Move(float Position)
        {
            try
            {
                System.Windows.Forms.MessageBox.Show("Relative Move of " + Position + " Requested");
                if (Position >= 1000000)
                {
                    // This is a special case for the Pyxis that when used by the Optec Derotation program
                    // that tells the rotator to move just one step.

                    OptecPyxis.Derotate1Step();
                    return;
                }

                // Now check for normal values 
                if (Position <= -360 || Position >= 360)
                {
                    throw new ASCOM.InvalidValueException("Move", Position.ToString(), "-359.99 through 359.99");
                }
                if ((int)Position == 0 || (int)Position == 360) return;

                // Finally do the move
                string msg = "";
                msg += "CurrentAdjustedPA = " + OptecPyxis.CurrentAdjustedPA.ToString();
                int finalPos = OptecPyxis.CurrentAdjustedPA + (int)Position;
                
                msg += " Final Pos = " + finalPos.ToString();
                if (finalPos > 360) finalPos -= 360;
                else if (finalPos < 0) finalPos = 360 + finalPos;

                msg += " Moving to " + (OptecPyxis.CurrentAdjustedPA + finalPos).ToString();
                System.Windows.Forms.MessageBox.Show(msg);
                OptecPyxis.CurrentAdjustedPA = finalPos;

                // Don't return until the move is complete
               // while (OptecPyxis.IsMoving) { }
                
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }
        }

        public void MoveAbsolute(float Position)
        {
            try
            {
                // Check that the new position is in the range of possible values
                System.Windows.Forms.MessageBox.Show("Moving to " + Position);
                if (Position < 0 || Position >= 360)
                {
                    throw new ASCOM.InvalidValueException("MoveAbsolute", Position.ToString(), "0 through 359.99");
                }
                if(OptecPyxis.CurrentAdjustedPA != (int)Position) 
                    OptecPyxis.CurrentAdjustedPA = (int)Position;
               // while (OptecPyxis.IsMoving) { System.Windows.Forms.Application.DoEvents(); }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }
        }

        public float Position
        {
            // TODO Replace this with your implementation
            get
            {
                try
                {
                    return (float)OptecPyxis.CurrentAdjustedPA;
                }
                catch (Exception ex)
                {
                    EventLogger.LogMessage(ex);
                    throw;
                }
            }
        }

        public bool Reverse
        {
            // TODO Replace this with your implementation
            get
            {
                return OptecPyxis.Reverse;
            }
            set
            {
                OptecPyxis.Reverse = value;
            }
        }

        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }

        public float StepSize
        {
            get 
            {
                return 1.0F;    // The pyxis rotators can only move in increments of 1 degree.
            }
        }

        public float TargetPosition
        {
            // TODO Replace this with your implementation
            get
            {
                try
                {
                    //verifyConnected();
                    return (float)OptecPyxis.AdjustedTargetPosition;
                }
                catch (Exception ex)
                {
                    EventLogger.LogMessage(ex);
                    throw;
                }
            }
        }

        #endregion
    }
}
