using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ASCOM.FocuserSimulator
{
    public class FocuserHardware
    {
        private static bool _Link;
        
        //
		// Sync object
		//
		private static object s_objSync = new object();	// Better than lock(this) - Jeffrey Richter, MSDN Jan 2003

		//
		// Constructor - initialize state
		//
		static FocuserHardware()
		{
            Properties.Settings.Default.Reload();
            _Link = false;
            Properties.Settings.Default.IsMoving = false;
            frmMain.ActiveForm.TopMost = true;
		}


        public static void DoSetup()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }

        public static bool Link
        {
            get { return _Link; }
            set { _Link = value; }
        }

        public static int MaxIncrement
        {
            get { return (int)Properties.Settings.Default.sMaxIncrement; }
        }

        public static int MaxStep
        {
            get { return (int)Properties.Settings.Default.sMaxStep; }
        }

        public static int Position
        {
            get
            {
                if (Properties.Settings.Default.sAbsolute) return (int)Properties.Settings.Default.sPosition;
                else throw new PropertyNotImplementedException("Position", false);
            }
        }


        public static double StepSize
        {
            get { return (double)Properties.Settings.Default.sStepSize; }
        }


        public static bool Absolute
        {
            get { return Properties.Settings.Default.sAbsolute; }
        }


        public static bool TempComp
        {
            get { return Properties.Settings.Default.sTempComp; }
            set
            {
                if (Properties.Settings.Default.sTempCompAvailable) Properties.Settings.Default.sTempComp = value;
                else throw new MethodNotImplementedException("TempComp");
            }
        }

        public static void Halt()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("Halt");
        }

        public static bool TempCompAvailable
        {
            get { return Properties.Settings.Default.sTempCompAvailable; }
        }


        public static double Temperature
        {
            get { throw new PropertyNotImplementedException("Temperature", false); }
        }

        public static bool IsMoving
        {
            get { return Properties.Settings.Default.IsMoving; }
        }

        private static void Deplace(int pStep)
        {
            Thread.Sleep(1);
        }

        public static void Move(int val)
        {
            int DestPosition = 0;

            System.Windows.Forms.MessageBox.Show("Test");
            // TODO Replace this with your implementation
            if (Properties.Settings.Default.sAbsolute)
            {
                if (val == Properties.Settings.Default.sPosition) return;
                if (val < 0) return;
                DestPosition = (val > (int)Properties.Settings.Default.sMaxStep ? (int)Properties.Settings.Default.sMaxStep : val);
                Properties.Settings.Default.IsMoving = true;
                if (val > Properties.Settings.Default.sPosition)
                {
                    for (int i = (int)Properties.Settings.Default.sPosition; i < val; i ++)
                    {
                        Deplace(1);
                        Properties.Settings.Default.sPosition ++;
                    }
                }
                else
                {
                    /*
                    for (int i = (int)Properties.Settings.Default.sPosition; i > val; i -= (int)Properties.Settings.Default.sStepSize)
                    {
                        Deplace((int)Properties.Settings.Default.sStepSize);
                        Properties.Settings.Default.sPosition -= (int)Properties.Settings.Default.sStepSize;
                    }
                     */
                    for (int i = (int)Properties.Settings.Default.sPosition; i > val; i --)
                    {
                        Deplace(1);
                        Properties.Settings.Default.sPosition --;
                    }
                }
                Properties.Settings.Default.IsMoving = false;
            }
            else
            {
                for (int i = 0; i < Math.Abs(val); i ++)
                {
                    Deplace(1);
                    if (val < 0) Properties.Settings.Default.sPosition--;
                    else Properties.Settings.Default.sPosition++;
                }
            }
        }
    }
}
