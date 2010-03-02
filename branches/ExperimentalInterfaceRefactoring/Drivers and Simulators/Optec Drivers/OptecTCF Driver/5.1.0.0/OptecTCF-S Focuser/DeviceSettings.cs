using System;
using System.Collections.Generic;
using System.Text;
using ASCOM.Utilities;

namespace ASCOM.OptecTCF_S
{
    public class DeviceSettings
    {
        private static ASCOM.Utilities.Profile Prof;
        public enum DeviceTypes { TCF_S, TCF_Si, TCF_S3, TCF_S3i, Unknown }
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

        static DeviceSettings() //Constructor
        {
            try
            {
                Prof = new ASCOM.Utilities.Profile();
                Prof.DeviceType = "Focuser";
            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError initializing device settings. \n " + Ex.ToString(), Ex);
            }
        }
      

        //Properties

        internal static string COMPort
        {
            get { return Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.LastUsedCOMPort.ToString(),"", "COM1"); }
            set 
            {
                //Verify that the value given is acceptable
                if (!value.Contains("COM")) throw new InvalidValueException("COMPort", value, "COMnn");
                if (value.Length <= 3) throw new InvalidValueException("COMPort", value, "COMnn");
                //Store the COM port name in the profile
                Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.LastUsedCOMPort.ToString(), value); 
            }
        }

        internal static bool TempProbePresent
        {
            get 
            { 
                return bool.Parse(Prof.GetValue(Focuser.s_csDriverID, 
                ProfileStrings.TempProbeAttached.ToString(), "", true.ToString())); 
            }
            set 
            {
                Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.TempProbeAttached.ToString(), value.ToString());
            }
        }

        internal static char ActiveTempCompMode
        {
            get 
            {
                string AorB = Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.AutoMode.ToString(), "", "A");
                if (char.Parse(AorB) != 'A' && char.Parse(AorB) != 'B')
                    throw new InvalidValueException("AorB", AorB.ToString(), "A or B");
                return char.Parse(AorB);
            }
            set 
            {
                if ((value != 'A') && (value != 'B'))
                    throw new InvalidValueException("AorB", value.ToString(), "A or B");
                else
                {
                    Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.AutoMode.ToString(), value.ToString());
                }
            }
        }

        internal static string ModeA_Name
        {
            get 
            {
                return Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.ModeAName.ToString(), "", "Temp Comp Mode A");
            }
            set 
            {
                Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.ModeAName.ToString(), value);
            }
        }

        internal static string ModeB_Name
        {
            get 
            {

                return Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.ModeBName.ToString(), "", "Temp Comp Mode B");
            }
            set 
            {
                Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.ModeBName.ToString(), value);

            }
        }

        internal static int ModeA_Delay
        {
            get
            {
                string d = Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.DelayA.ToString(), "", "1");
                return int.Parse(d);
            }
            set
            {
                if (value < 1) throw new InvalidValueException("ModeA_Delay", value.ToString(), "1 to 999");
                if (value > 999) throw new InvalidValueException("ModeA_Delay", value.ToString(), "1 to 999");
                Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.DelayA.ToString(), value.ToString());
            }
        }

        internal static int ModeB_Delay
        {
            get
            {
                string d = Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.DelayB.ToString(), "", "1");
                return int.Parse(d);
            }
            set
            {
                if (value < 1) throw new InvalidValueException("ModeB_Delay", value.ToString(), "1 to 999");
                if (value > 999) throw new InvalidValueException("ModeB_Delay", value.ToString(), "1 to 999");
                Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.DelayB.ToString(), value.ToString());
            }
        }

        internal static DeviceTypes DeviceType
        {
            get { 
                    string type = Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.DeviceType.ToString(), "", "?");
                    switch(type)
                    {
                        case "TCF_S":
                            return DeviceTypes.TCF_S;
                            
                        case "TCF_S3":
                            return DeviceTypes.TCF_S3;
                            
                        case "TCF_S3i":
                            return DeviceTypes.TCF_S3i;
                            
                        case "TCF_Si":
                            return DeviceTypes.TCF_Si;

                        case "?":
                            return DeviceTypes.Unknown;

                        default:
                            throw new InvalidValueException("DeviceSettings.DeviceType", type, "TCF_S, TCF_Si, TCF-S3, TCF-S3i");
                    }
                }
            set { Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.DeviceType.ToString(), value.ToString()); }
        }

        internal static string FirmwareVersion
        {
            get{ return Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.FirmwareVersion.ToString(), "", "?"); }
            set { Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.FirmwareVersion.ToString(), value); }
        }

        internal static int MaxStep
        {
            get
            {
                if (DeviceType == DeviceTypes.TCF_S3 || DeviceType == DeviceTypes.TCF_S3i ||DeviceSettings.DeviceType == DeviceTypes.Unknown)
                    return 10000;
                else if (DeviceType == DeviceTypes.TCF_S || DeviceType == DeviceTypes.TCF_Si)
                    return 7000;
                else
                    throw new InvalidValueException("Device Type in GetMaxStep",
                        DeviceType.ToString(), "TCF_S, TCF_Si, TCF-S3, TCF-S3i, Unknown"); 
            }
        }

        internal static SlopePoint StartPoint
        {
            get
            {
                SlopePoint tempPt = new SlopePoint();

                tempPt.Position = int.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.StartPtPos.ToString(), "", "11111"));
                tempPt.Temperature = double.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.StartPtTemp.ToString(), "", "11111"));

                tempPt.DateAndTime = DateTime.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.StartPtDateTime.ToString(), "", "7/17/1985"));
                return tempPt;
            }
            set {
                    try
                    {
                        string Pos = ""; string Temp = ""; string DateTime = "";
                        Pos = value.Position.ToString();
                        Temp = value.Temperature.ToString();
                        DateTime = value.DateAndTime.ToString();
                        Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.StartPtPos.ToString(), Pos, "");
                        Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.StartPtTemp.ToString(), Temp, "");
                        Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.StartPtDateTime.ToString(), DateTime, "");
                    }
                    catch (Exception ex)
                    {
                        throw new ASCOM.DriverException("An Error Occured in SetStartPoint.\n" + ex.ToString(), ex);
                    }
            }
        }

        internal static int CalculateSlope(int Pos1, double Temp1, int Pos2, double Temp2)
        {
            if (Temp1 == Temp2) throw new InvalidOperationException("Temperature1 and Temperature2 can not be equal.\n");
            //if (Pos1 == Pos2) throw new InvalidOperationException("Position1 and Position2 can not be equal.\n");

            //Calculate Slope,  Slope = Steps per Degree
            int slope = (Pos1 - Pos2) / (Convert.ToInt32(Temp2) - Convert.ToInt32(Temp1));

            if (slope < -998 || slope > 999)
            {
                //MessageBox.Show("Could not save time constant to device. TC must be between 2 and 99.\n"
                //    + "Calculated TC = " + slope.ToString() + ".\n");
                throw new InvalidOperationException("ABS(Slope) must be beween 000 and 999");
            }
            return slope;
        }

    }

    class SlopePoint
    {
        int s_pos;
        double s_temp;
        DateTime s_dateTime;

        internal SlopePoint()
        {
            s_pos = 0;
            s_temp = 0;
            s_dateTime = DateTime.Now;
        }

        internal int Position
        {
            get { return s_pos; }
            set{ s_pos = value; }
        }

        internal double Temperature
        {
            get { return s_temp; }
            set { s_temp = value; }
        }

        internal DateTime DateAndTime
        {
            get { return s_dateTime; }
            set { s_dateTime = value; }
        }
        
    }

}
