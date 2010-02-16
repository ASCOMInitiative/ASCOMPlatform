using System;
using System.Threading;
using System.Diagnostics;
using ASCOM.Conform;


namespace ASCOM.FocVide
{
    public class FocuserHardware
    {
        #region Public variables
        public enum eLogKind { LogMove, LogTemp, LogIsMoving, LogOther };
        public static bool HaltRequested;
        public static frmMain Gui = new frmMain();
        public static bool IsTempCompMove = false;
        public static Random xRand, xRandFail;
        public static System.Windows.Forms.Timer Chrono = new System.Windows.Forms.Timer();
        #endregion

        protected static bool _Link;

        #region Focuser.Constructor

        static FocuserHardware()
		{
            // Temporary renamed Properties to _Properties
            _Properties.Settings.Default.Reload();
            _Link = false;
            HaltRequested = false;
            Properties.Settings.Default.IsMoving = false;
            xRand = new Random();
            xRandFail = new Random(30000);
            Chrono.Enabled = false;
            Chrono.Interval = 3000;
            Chrono.Tick += new EventHandler(Chrono_Tick);
            
            Gui.Show();
        }

        #endregion

        #region Asynchronous timer
        static void Chrono_Tick(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsMoving = false;
            Chrono.Enabled = false;
        }
        #endregion

        #region Focuser.SetupDialog() method
        public static void DoSetup()
        {
            MyLog(eLogKind.LogOther, "Calling SetupDialog()");
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }
        #endregion

        #region Focuser.Link() method
        public static bool Link
        {
            get { MyLog(eLogKind.LogOther, "Link property request"); return _Link; }
            set { MyLog(eLogKind.LogOther, "Set Link property to " + value.ToString()); _Link = value; }
        }
        #endregion

        #region Focuser.MaxIncrement property
        public static int MaxIncrement
        {
            get 
            {
                MyLog(eLogKind.LogOther, "MaxIncrement property request");
                return (int)Properties.Settings.Default.sMaxIncrement;
            }
        }
        #endregion

        #region Focuser.MaxStep property
        public static int MaxStep
        {
            get 
            {
                MyLog(eLogKind.LogOther, "MaxStep property request");
                return (int)Properties.Settings.Default.sMaxStep;
            }
        }
        #endregion

        #region Focuser.Position property
        public static int Position
        {
            get
            {
                MyLog(eLogKind.LogOther, "Position request");
                if (Properties.Settings.Default.sAbsolute) return (int)Properties.Settings.Default.sPosition;
                else throw new ASCOM.DriverException();
            }
        }
        #endregion

        #region Focuser.StepSize property
        public static double StepSize
        {
            get 
            {
                MyLog(eLogKind.LogOther, "StepSize property request");
                if (!Properties.Settings.Default.sIsStepSize) { throw new ASCOM.DriverException(); }
                else { return (double)Properties.Settings.Default.sStepSize; }
            }
        }
        #endregion

        #region Focuser.Absolute property
        public static bool Absolute
        {
            get 
            {
                MyLog(eLogKind.LogOther, "Absolute property request");
                return Properties.Settings.Default.sAbsolute; 
            }
        }
        #endregion

        #region Focuser.Halt() method
        public static void Halt()
        {
            MyLog(eLogKind.LogMove, "HALT requested");
            if (!Properties.Settings.Default.sEnableHalt) { throw new ASCOM.DriverException(); }
            else 
            { 
                HaltRequested = true;
                Properties.Settings.Default.IsMoving = false;
                Chrono.Enabled = false;
            }
        }
        #endregion

        #region Focuser.TempComp property
        public static bool TempComp
        {
            get 
            {
                MyLog(eLogKind.LogTemp, "Retrieving TempComp property");
                if (!Properties.Settings.Default.sTempCompAvailable) { return false; }
                else return Properties.Settings.Default.sTempComp; 
            }
            set
            {
                
                if (Properties.Settings.Default.sTempCompAvailable)
                {
                    MyLog(eLogKind.LogTemp, "Setting temperature compensation " + (value ? "ON" : "OFF")+" : OK");
                    Properties.Settings.Default.sTempComp = value;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    MyLog(eLogKind.LogTemp, "Setting temperature compensation " + (value ? "ON" : "OFF")+" : ERROR");
                    throw new ASCOM.DriverException();
                }
            }
        }
        #endregion

        #region Focuser.TempCompAvailable property
        public static bool TempCompAvailable
        {
            get 
            {
                MyLog(eLogKind.LogTemp, "TempCompAvailable request");
                return Properties.Settings.Default.sTempCompAvailable; 
            }
        }
        #endregion

        #region Focuser.Temperature property
        public static double Temperature
        {
            get 
            {
                if (Properties.Settings.Default.sIsTemperature) 
                {
                    // Get a simulated temperature near the user specified range
                    double xTemp = (double)((int)Properties.Settings.Default.sTempMin + 10 * xRand.NextDouble());
                    MyLog(eLogKind.LogTemp, "Temperature request : "+xTemp.ToString("F1"));
                    return xTemp;
                }
                else throw new ASCOM.DriverException();  // Unless otherwise specified in the specs, this excep
            }
        }
        #endregion

        #region Focuser.IsMoving property
        public static bool IsMoving
        {
            get { return Properties.Settings.Default.IsMoving; }
        }
        #endregion

        #region Focuser.Move() method
        public static void Move(int val)
        {
            int DestPosition = 0;

            if (!IsTempCompMove)
            {
                if (Properties.Settings.Default.sTempComp) // No moves when in t° compensation mode
                {
                    MyLog(eLogKind.LogOther, "No move because T° compensation is ON");
                    return;
                }
            }
            else
            {
                MyLog(eLogKind.LogMove, "T° compensation move");
            }
            HaltRequested = false;
            if (Properties.Settings.Default.sAbsolute)
            {
                MyLog(eLogKind.LogMove, "Requested move from " + Properties.Settings.Default.sPosition.ToString() + " to " + val.ToString());
                DestPosition = (val > (int)Properties.Settings.Default.sMaxStep ? (int)Properties.Settings.Default.sMaxStep : val);

                if (val == Properties.Settings.Default.sPosition)
                {
                    MyLog(eLogKind.LogOther, "No move because destination = current");
                    return;
                }
                // Focuser's spec is saying :
                // The Move command tells the focuser to move to an exact step position, and the Position 
                // parameter of the Move() method is an integer between 0 and MaxStep.
                //
                if (val < 0)
                {
                    MyLog(eLogKind.LogOther, "Requested position < 0 : moving to 0");
                    DestPosition = 0;
                }

                if (val > (int)Properties.Settings.Default.sMaxStep)
                {
                    MyLog(eLogKind.LogOther, "Requested position > MaxStep : moving to MaxStep");
                    DestPosition = (int)Properties.Settings.Default.sMaxStep;
                }

                // Focuser's spec is saying :
                // MaxIncrement : Maximum increment size allowed by the focuser; i.e. the maximum number 
                // of steps allowed in one move operation.
                int NbStepsRequired = Math.Abs((int)(Properties.Settings.Default.sPosition - DestPosition));
                if ( NbStepsRequired > Properties.Settings.Default.sMaxIncrement)
                {
                    if (DestPosition < Properties.Settings.Default.sPosition) 
                    { 
                        DestPosition = (int)Properties.Settings.Default.sPosition - (int)Properties.Settings.Default.sMaxIncrement;
                        MyLog(eLogKind.LogOther, "Nb steps required > MaxIncrement : moving to current position - MaxIncrement");
                    }
                    else 
                    { 
                        DestPosition = (int)Properties.Settings.Default.sPosition + (int)Properties.Settings.Default.sMaxIncrement;
                        MyLog(eLogKind.LogOther, "Nb steps required > MaxIncrement : moving to current position + MaxIncrement");
                    }
                }
                
                // Really start moving, now
                Properties.Settings.Default.IsMoving = true;
                int Start = (int)Properties.Settings.Default.sPosition;
                MyLog(eLogKind.LogIsMoving, "Moving from "+Start.ToString()+" to "+val.ToString());
                
                if (Properties.Settings.Default.sIsSynchronous)  // Synchronous moves
                {
                    if (val > Properties.Settings.Default.sPosition)
                    {
                        for (int i = Start; i < DestPosition; i++)
                        {
                            if (HaltRequested)
                            {
                                Properties.Settings.Default.IsMoving = false;
                                HaltRequested = false;
                                MyLog(eLogKind.LogMove, "Focuser stopped");
                                Properties.Settings.Default.Save();
                                return;
                            }
                            FakeMove(1);
                            Properties.Settings.Default.sPosition++;
                        }
                    }
                    else
                    {
                        for (int i = Start; i > val; i--)
                        {
                            if (HaltRequested)
                            {
                                Properties.Settings.Default.IsMoving = false;
                                HaltRequested = false;
                                MyLog(eLogKind.LogMove, "Focuser stopped");
                                Properties.Settings.Default.Save();
                                return;
                            }
                            FakeMove(-1);
                            Properties.Settings.Default.sPosition--;
                        }
                    }

                    Properties.Settings.Default.IsMoving = false;
                }
                else  // Asynchronous moves
                {
                    Properties.Settings.Default.sPosition = val;
                    Properties.Settings.Default.IsMoving = true;
                    Chrono.Enabled = true;
                }
                MyLog(eLogKind.LogIsMoving, "Move done");
            }
            else  // Relative focuser move
            {
                MyLog(eLogKind.LogMove,"Requested relative move "+val.ToString()+" steps");
                Properties.Settings.Default.IsMoving = true;
                MyLog(eLogKind.LogIsMoving, "Moving "+Math.Abs(val)+" steps "+(val >= 0 ? "forward" : "backward"));
                for (int i = 0; i < Math.Abs(val); i ++)
                {
                    // Focuser.Halt() was called
                    if (HaltRequested) 
                    { 
                        Properties.Settings.Default.IsMoving = false; 
                        HaltRequested = false;
                        MyLog(eLogKind.LogMove, "Focuser stopped");
                        Properties.Settings.Default.Save();
                        return; 
                    }
                    FakeMove(1);  // Fake move
                }
                Properties.Settings.Default.IsMoving = false;
                MyLog(eLogKind.LogIsMoving, "Relative move done");
            }
            Properties.Settings.Default.Save();
        }
        #endregion


        #region private methods

        /// <summary>
        /// Fake move. Used to add some delays between each motor step
        /// </summary>
        /// <param name="pStep">The number of steps. Not meaningful here, of course.</param>
        private static void FakeMove(int pStep)
        {
            Thread.Sleep(1);
        }


        /// <summary>
        /// Logs events in the traffic window
        /// </summary>
        /// <param name="Kind">Kind of event.</param>
        /// <param name="Texte">Text describing the event.</param>
        public static void MyLog(eLogKind Kind, string Texte)
        {
            if ((Properties.Settings.Default.LogHaltMove) && (Kind == eLogKind.LogMove)) Trace.WriteLine(Texte);
            if ((Properties.Settings.Default.LogIsMoving) && (Kind == eLogKind.LogIsMoving)) Trace.WriteLine(Texte);
            if ((Properties.Settings.Default.LogTempRelated) && (Kind == eLogKind.LogTemp)) Trace.WriteLine(Texte);
            if ((Properties.Settings.Default.LogOther) && (Kind == eLogKind.LogOther)) Trace.WriteLine(Texte);
        }

        #endregion
    }


}
