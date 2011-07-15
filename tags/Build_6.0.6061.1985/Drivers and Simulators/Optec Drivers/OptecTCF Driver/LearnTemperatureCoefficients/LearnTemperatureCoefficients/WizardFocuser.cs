using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM;
using ASCOM.Interface;
using ASCOM.Utilities;
using ASCOM.DriverAccess;

namespace TempCoeffWizard
{
    public interface IOptecFocuser
    {
        string GetModeName(char AorB);
    }

    static class WizardFocuser
    {
        public static Focuser myFocuser;
        private static DateTime _StartTime = new DateTime();
        private static ASCOM.Utilities.Profile driverProfile = new ASCOM.Utilities.Profile();
        private static int _StartPos = 0;
        private static double _StartTemp = 0;
        private static int _EndPos = 0;
        private static double _EndTemp = 0;
        private static int _Coefficient = 123;

        static WizardFocuser()
        {
            driverProfile.DeviceType = "Focuser";
        }
    

        public static DateTime StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

        public static int StartPos
        {
            get { return _StartPos; }
            set { _StartPos = value; }
        }

        public static double StartTemp
        {
            get { return _StartTemp; }
            set { _StartTemp = value; }
        }

        public static int EndPos
        {
            get { return _EndPos; }
            set { _EndPos = value; }
        }

        public static double EndTemp
        {
            get { return _EndTemp; }
            set { _EndTemp = value; }
        }

        public static int Coefficient
        {
            get { return _Coefficient; }
            set { _Coefficient = value; }
        }

        internal static void CalculateCoefficient()
        {
            try
            {
                if (Math.Abs(EndTemp - StartTemp) < 5) throw new ApplicationException("Temperature difference must be at least 5°C.\n" +
                    "Measured Temperature difference = " + Math.Abs(EndTemp - StartTemp).ToString() + "°C");
                int coeff = (StartPos - EndPos) / (Convert.ToInt32(EndTemp) - Convert.ToInt32(StartTemp));
                if (coeff < -998 || coeff > 999)
                {
                    throw new ApplicationException("The calculated coefficient was not in the range of -998 to 999.\n" + 
                    "This could result from to small of a temperature/position change. The calculated coefficient was " + coeff.ToString() + "\n" + 
                    "Coefficient calculated using the following data:\n" + 
                    "Start Temp = " + StartTemp.ToString() + "\n" + 
                    "End Temp = " + EndTemp.ToString() + "\n" + 
                    "Start Position = " + StartPos.ToString() + "\n" + 
                    "End Position = " + EndPos.ToString() );
                }
                _Coefficient = coeff;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        internal static void StoreCoefficient(char ModeAorB)
        {
            try
            {
                if ((ModeAorB != 'A') && (ModeAorB != 'B'))
                    throw new ApplicationException("ModeAorB must = A or B");
                SetSlope(ModeAorB);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //These are from the driver code...
        private enum ProfileStrings
        {
            LastUsedCOMPort,
            AutoMode,
            DeviceType,
            FirmwareVersion,

            StartPtDateTime,
            StartPtTemp,
            StartPtPos,

            EndPtDateTimeA,
            EndPtTempA,
            EndPtPosA,

            EndPtDateTimeB,
            EndPtTempB,
            EndPtPosB,

            ModeAName,
            ModeBName,

            DelayA,
            DelayB,

            TempProbeAttached
        }
        internal static string s_csDriverID = "ASCOM.OptecTCF_S.Focuser";
        
        internal static string ModeA_Name
        {
            get
            {
                return driverProfile.GetValue(s_csDriverID, ProfileStrings.ModeAName.ToString(), "", "Temp Comp Mode A");
            }
            set
            {
                driverProfile.WriteValue(s_csDriverID, ProfileStrings.ModeAName.ToString(), value);
            }
        }

        internal static string ModeB_Name
        {
            get
            {

                return driverProfile.GetValue(s_csDriverID, ProfileStrings.ModeBName.ToString(), "", "Temp Comp Mode B");
            }
            set
            {
                driverProfile.WriteValue(s_csDriverID, ProfileStrings.ModeBName.ToString(), value);

            }
        }

        internal static string COMPort
        {
            get { return driverProfile.GetValue(s_csDriverID, ProfileStrings.LastUsedCOMPort.ToString(), "", "COM1"); }
        }   

        internal static void SetSlope(char AorB)
        {
            try
            {
                string cmd = "";
                string slp = "";
                int UnsignedCoeff = Math.Abs(_Coefficient);

                if (UnsignedCoeff > 999) throw new ApplicationException("Temperature coefficient is greater than max value.");
                
                slp = UnsignedCoeff.ToString().PadLeft(3, '0');
                if (AorB == 'A')
                {
                    cmd = "FLA" + slp;
                }
                else if (AorB == 'B')
                {
                    cmd = "FLB" + slp;
                }
                else
                {
                    throw new ApplicationException("AorB must equal A or B");
                }
                SendCommand(cmd, "DONE", 500);

            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError executing LoadSlope method.\n" + Ex.ToString(), Ex);
            }


            // Now set the slope sign...

            try
            {
                char sign;
                if (_Coefficient < 0) sign = '-';
                else sign = '+';
                if (sign != '+' && sign != '-') throw new InvalidValueException("SetSlopeSign", "slope", "+ or -");
                if (AorB != 'A' && AorB != 'B') throw new InvalidValueException("SetSlopeSign", "slope", "+ or -");
                string cmd = "";
                if (AorB == 'A')
                {
                    cmd = "FZAxx";
                }
                else if (AorB == 'B')
                {
                    cmd = "FZBxx";
                }
                if (sign == '+')
                {
                    cmd = cmd + "0";
                }
                else if (sign == '-')
                {
                    cmd = cmd + "1";
                }
                SendCommand(cmd, "DONE", 500);
            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError executing SetSlopeSign method./n" + Ex.ToString(), Ex);
            }
        }

        internal static void SendCommand(string cmd, string resp, int timeout)
        {
            System.IO.Ports.SerialPort mySerialPort = new System.IO.Ports.SerialPort();
            try
            {
                
                if (myFocuser.Link) myFocuser.Link = false;

                mySerialPort.PortName = COMPort;
                mySerialPort.BaudRate = 19200;
                mySerialPort.Open();
                mySerialPort.ReadTimeout = 700;
                mySerialPort.DiscardOutBuffer();
                mySerialPort.DiscardInBuffer();
                
                mySerialPort.Write("FMMODE");
                string received = mySerialPort.ReadLine();
                if (!received.Contains("!"))
                {
                    throw new ApplicationException("Attempt to put device in serial mode failed.");
                }
                mySerialPort.ReadTimeout = timeout;
                mySerialPort.Write(cmd);
                received = mySerialPort.ReadLine();
                if (!received.Contains(resp))
                {
                    throw new ApplicationException("Did not receive DONE response after sending coefficient to device");
                }
                mySerialPort.ReadTimeout = 700;
                mySerialPort.Write("FFXXXX");
                received = mySerialPort.ReadLine();
                if (!received.Contains("END"))
                {
                    throw new ApplicationException("Did not receive DONE response after sending coefficient to device");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySerialPort.Close();
                mySerialPort.Dispose();
            }
        }
    
    }
}
