//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Properties
//
// Description:	This implements property source for Gemini
//
// Author:		(pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 08-SEP-2009  pk  1.0.0   Added full complement of modeling, custom mount, and other Gemini property
//                          Implemented profile saving and editing
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ASCOM.GeminiTelescope
{
    public class GeminiProperties : Component, INotifyPropertyChanged
    {

        private SerializableDictionary<string, object> mProfile = new SerializableDictionary<string, object>();

        
        public SerializableDictionary<string, object> Profile
        {
            get { return mProfile; }
            set { mProfile = value; }
        }

        public class ItemList
        {
            public ItemList(int _id, string _name)
            {
                id = _id;
                name = _name;
            }

            public override string ToString()
            {
                return name;
            }
            
            public int id;

            public string name;
        }

        static public string[] Mount_names = {"Custom", "GM-8", "G-11", "HGM-200", "MI-250", "Titan", "Titan50"};
        static public string[] TrackingRate_names = { "Sidereal", "King Rate", "Lunar", "Solar", "Terrestrial", "Closed Loop", "Comet Rate" };
        static public string[] HandController_names = { "Visual", "Photo", "All Speeds" };
        static public string[] Brightness_names = { "100%", "53%", "40%", "27%", "20%", "13%", "6.6%" };


        public event PropertyChangedEventHandler PropertyChanged;


        public GeminiProperties()
        {
        }


        /// <summary>
        /// Erase all entries in the current profile
        /// </summary>
        public void ClearProfile()
        {
            mProfile.Clear();
            RTProfile.Clear();
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                try
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
                }
                catch { }
            }
        }
        
        private string get_Prop(string s)
        {
            if (!GeminiHardware.Connected) return null;

            string res = null;
            try
            {
                res = GeminiHardware.DoCommandResult(s, 2000, false);
            }
            catch
            {
                return null;
            }
            return res;
        }


        private Dictionary<string, bool> WaitList = new Dictionary<string, bool>();
        private Dictionary<string, string> RTProfile = new Dictionary<string, string>();


        void OnHardwarePropertyUpdate(string sCmd, string sResult)
        {
            lock (RTProfile)
            {
                if (sResult != null)
                    RTProfile[sCmd] = sResult;
                WaitList[sCmd] = false;
            }
        }

        private string get_PropAsync(string sCmd)
        {
            string res = null;

            if (!GeminiHardware.Connected) return null;
            lock (RTProfile)
            {
                if (RTProfile.ContainsKey(sCmd))
                    res = RTProfile[sCmd] as string;

                if (!WaitList.ContainsKey(sCmd) || !WaitList[sCmd])
                {
                    if (RTProfile.ContainsKey(sCmd))
                    {
                        GeminiHardware.DoCommandAsync(sCmd, 5000, new HardwareAsyncDelegate(OnHardwarePropertyUpdate), false);
                        WaitList[sCmd] = true;
                    }
                    else
                    {
                        res = GeminiHardware.DoCommandResult(sCmd, GeminiHardware.MAX_TIMEOUT, false);    //first time, get the value non-async
                        RTProfile[sCmd] = res;
                    }
                }
            }
            return res;
        }



        private int get_int_Prop(string s)
        {
            string prop = get_Prop(s);
            int val;
            if (!int.TryParse(prop, out val)) return 0;
            return val;
        }

        private double get_double_Prop(string s)
        {
            string prop = get_Prop(s);
            double val;
            if (!double.TryParse(prop, out val)) return 0;
            return val;
        }

        private bool get_bool_Prop(string s)
        {
            string prop = get_Prop(s);
            bool val;
            if (!bool.TryParse(prop, out val)) return false;
            return val;
        }

        private string get_string_Prop(string s)
        {
            string prop = get_Prop(s);
            return prop??"";
        }

        private DateTime get_time_Prop(string s)
        {
            string prop = get_Prop(s);
            DateTime val;
            if (!DateTime.TryParseExact(prop, "HH:mm:ss", new System.Globalization.DateTimeFormatInfo(), System.Globalization.DateTimeStyles.AssumeLocal, out val)) return DateTime.Now;
            return val;
        }

        private object get_Profile(string s, object def_value)
        {
            if (mProfile.ContainsKey(s)) return mProfile[s];
            return def_value;
        }

        // Real-time properites by-pass profile and go directly to Gemini hardware:
#region Real-Time Properties

        public string GeminiVersion
        {
            get
            {
                string res = get_PropAsync(":GVN");
                if (res == null) return null;

                return string.Format("L{0} v{1}.{2}", GeminiHardware.Version.Substring(0, 1), GeminiHardware.Version.Substring(1, 1), GeminiHardware.Version.Substring(2,1));
            }
        }

        public string MountType
        {
            get
            {
                string res = get_PropAsync("<0:");
                int idx = 0;
                if (!int.TryParse(res, out idx)) return null;
                return Mount_names[idx];
            }
        }

        public string LocalTime
        {
            get
            {
                string res = get_PropAsync(":GL");
                return res;
            }
        }

        public string LocalDate
        {
            get
            {
                string res = get_PropAsync(":GC");
                if (res != null)
                {
                    DateTime tm = DateTime.ParseExact(res, "MM/dd/yy", new System.Globalization.DateTimeFormatInfo()); // Parse to a local datetime using the given format                    
                    return tm.ToShortDateString();
                }
                return null;
            }
        }


        public string TrackingRate
        {
            get
            {
                string res = get_PropAsync("<130:");
                int rate ;
                if (!int.TryParse(res, out rate)) return null;
                return TrackingRate_names[rate-131];
            }
        }

        public string TimeZone
        {
            get
            {
                if (!GeminiHardware.Connected) return null;
                // gemini stores timezone with sign reversed from UTC:
                return "UTC" + (GeminiHardware.UTCOffset > 0? " - " : " + ") + Math.Abs(GeminiHardware.UTCOffset).ToString();
            }
        }


        public string HandControllerMode
        {
            get
            {
                string prop = get_PropAsync("<160:");
                int hc;
                if (!int.TryParse(prop, out hc)) return null; 
                return HandController_names[hc - 161];
            }
        }

        public string GotoSlewRate
        {
            get
            {
                string prop = get_PropAsync("<140:");
                int rate;
                if (!int.TryParse(prop, out rate)) return null;
                return rate.ToString() + "x";
            }
        }

        public string ManualSlewRate
        {
            get
            {
                string prop = get_PropAsync("<120:");
                int rate;
                if (!int.TryParse(prop, out rate)) return null;
                return rate.ToString() + "x";
            }
        }

        public string GuideRate
        {
            get
            {
                string prop = get_PropAsync("<150:");
                double rate;
                if (!double.TryParse(prop, out rate)) return null;
                return rate.ToString("0.0") + "x";
            }
        }


        public string CenteringRate
        {
            get
            {
                string prop = get_PropAsync("<170:");
                int rate;
                if (!int.TryParse(prop, out rate)) return null;
                return rate.ToString() + "x";
            }
        }

        public string PECStatus
        {
            get
            {
                string prop = get_PropAsync("<509:");
                int stat;
                if (!int.TryParse(prop, out stat)) return null;
                return (stat & 1) == 0 ? "PEC OFF" : "PEC ON";
            }
        }

        public string RA
        {
            get
            {
                if (!GeminiHardware.Connected) return null;
                double ra = GeminiHardware.RightAscension;
                return GeminiHardware.m_Util.HoursToHMS(ra, ":", ":", "");
            }
        }

        public string DEC
        {
            get
            {
                if (!GeminiHardware.Connected) return null;
                double dec = GeminiHardware.Declination;
                return GeminiHardware.m_Util.DegreesToDMS(dec, ":", ":", "");
            }
        }

        public string ALT
        {
            get
            {
                if (!GeminiHardware.Connected) return null;
                double alt = GeminiHardware.Altitude;
                return GeminiHardware.m_Util.DegreesToDMS(alt, ":", ":", "");
            }
        }

        public string AZ
        {
            get
            {
                if (!GeminiHardware.Connected) return null;
                double az = GeminiHardware.Azimuth;
                return GeminiHardware.m_Util.DegreesToDMS(az, ":", ":", "");
            }
        }

        public bool IsConnected
        {
            get { return GeminiHardware.Connected; }
        }

#endregion

#region Advanced Properties

        public string LEDBrightness
        {
            get { return (string)get_Profile("LEDBrightness", Brightness_names[0]); }
            set { mProfile["LEDBrightness"] = value; }
        }

        private string LEDBrightness_Gemini
        {
            get {
                int res = get_int_Prop(":GB");
                return Brightness_names[res];
            }
            set {
                for (int i = 0; i < Brightness_names.Length; ++i)
                    if (Brightness_names[i].Equals(value))
                        GeminiHardware.DoCommandResult(":SB" + i.ToString(), GeminiHardware.MAX_TIMEOUT, false);
            }
        }


        public bool SyncDoesAlign
        {
            get { return (bool)get_Profile("SyncDoesAlign", false); }
            set { mProfile["SyncDoesAlign"] = value; }
        }


        private bool SyncDoesAlign_Gemini
        {
            get { return GeminiHardware.SwapSyncAdditionalAlign; }
            set { GeminiHardware.SwapSyncAdditionalAlign = value; }
        }


        public bool DoesPrecession
        {
            get { return (bool) get_Profile("DoesPrecession", false); }
            set { mProfile["DoesPrecession"] = value; }
        }

        private bool m_Precession;

        private bool DoesPrecession_Gemini
        {
            get { return GeminiHardware.Precession; }
            set { m_Precession  = value; }
        }


        public bool DoesRefraction
        {
            get { return (bool) get_Profile("DoesRefraction", false); }
            set {mProfile["DoesRefraction"] = value; }
        }

        private bool DoesRefraction_Gemini
        {
            get { return GeminiHardware.Refraction; }
            set { GeminiHardware.SetPrecessionRefraction(m_Precession, value); }
        }

        public DateTime AlarmTime
        {
            get { return (DateTime) get_Profile("AlarmTime", DateTime.Now); }
            set { mProfile["AlarmTime"] = value; }
        }


        private DateTime AlarmTime_Gemini
        {
            get { return get_time_Prop(":GE"); }
            set { GeminiHardware.DoCommandResult(":SE" + value.ToString("HH:mm:ss"), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public bool AlarmSet
        {
            get { return (bool) get_Profile("AlarmSet", false); }
            set { mProfile["AlarmSet"] = value; }
        }


        private bool AlarmSet_Gemini
        {
            get
            {
                int res = get_int_Prop("<180:");
                if (res == 182) return true;
                return false;
            }
            set
            {
                string prop  =GeminiHardware.DoCommandResult(value ? ">182:" : ">181:", GeminiHardware.MAX_TIMEOUT, false);
            }
        }

        public string WestSafetyLimit
        {
            get { return (string)get_Profile("WestSafetyLimit", "Western Safety Limit"); }
            set { mProfile["WestSafetyLimit"] = value; }
        }

        private string WestSafetyLimit_Gemini
        {
            get
            {
                string res = get_Prop("<220:");
                int d = 0, m = 0;
                try
                {
                    // east        west
                    //<ddd>d<mm>;<ddd>d<mm>
                    d = int.Parse(res.Substring(7, 3));
                    m = int.Parse(res.Substring(11, 2));
                }
                catch { }

                return string.Format("Western Safety Limit: {0:0}°{1:00}", d, m);
            }
        }


        public string EastSafetyLimit
        {
            get { return (string)get_Profile("EastSafetyLimit", "Eastern Safety Limit"); }
            set { mProfile["EastSafetyLimit"] = value; }
        }

        private string EastSafetyLimit_Gemini
        {
            get
            {
                string res = get_Prop("<220:");
                int d = 0, m = 0;
                try
                {
                    // east        west
                    //<ddd>d<mm>;<ddd>d<mm>
                    d = int.Parse(res.Substring(0, 3));
                    m = int.Parse(res.Substring(4, 2));
                }
                catch { }
                return string.Format("Eastern Safety Limit: {0:0}°{1:00}", d, m);
            }
        }


        public bool PEC_Is_On
        {
            get { return (bool)get_Profile("PECStatus", false);}
            set { mProfile["PECStatus"] = value; } 
        }

        private bool PEC_Is_On_Gemini
        {
            get {
                string prop = get_Prop("<509:");
                int stat;
                if (!int.TryParse(prop, out stat)) return false;
                return (stat & 1) == 0 ? false : true;
            }

            set {              
                string prop = get_Prop("<509:");
                if (prop == "0" || prop == null) prop = get_Prop("<509:");

                int stat;
                if (!int.TryParse(prop, out stat)) return ;
                stat = (stat & 0xfe) | (value? 1 : 0);
                GeminiHardware.DoCommandResult(">509:" + stat.ToString(), GeminiHardware.MAX_TIMEOUT, false);                
            }
        }


        public string MountTypeSetting
        {
            get { return (string)get_Profile("MountTypeSetting", Mount_names[2]); }
            set { mProfile["MountTypeSetting"] = value; }
        }

        private string MountTypeSetting_Gemini
        {
            get { return MountType??Mount_names[2]; }
            set
            {
                for (int i = 0; i < Mount_names.Length; ++i)
                    if (Mount_names[i].Equals((string)value))
                        GeminiHardware.DoCommandResult(">" + i.ToString(), GeminiHardware.MAX_TIMEOUT, false);
            }
        }



        public string HandController
        {
            get {return (string)get_Profile("HandController", HandController_names[0]);}
            set { mProfile["HandController"] = value; }
        }

        private string HandController_Gemini
        {
            get { return HandControllerMode ?? HandController_names[0]; }
            set {
                for (int i = 0; i < HandController_names.Length; ++i)
                    if (HandController_names[i].Equals((string)value))
                        GeminiHardware.DoCommandResult(">" + (i + 161).ToString(), GeminiHardware.MAX_TIMEOUT, false);
            }
        }

        public string TrackingRateMode
        {
            get { return (string)get_Profile("TrackingRateMode", TrackingRate_names[0]); }
            set { mProfile["TrackingRateMode"] = value; }
        }

        private string TrackingRateMode_Gemini
        {
            get { return this.TrackingRate?? TrackingRate_names[0]; }
            set {
                for (int i = 0; i < TrackingRate_names.Length; ++i)
                    if (TrackingRate_names[i].Equals(value))
                        GeminiHardware.DoCommandResult(">" + (i + 131).ToString(), GeminiHardware.MAX_TIMEOUT, false);
            }
        }

        public int TrackingDivisorRA
        {
            get { return (int)get_Profile("TrackingDivisorRA", 56096); }
            set { mProfile["TrackingDivisorRA"] = value; }
        }

        private int TrackingDivisorRA_Gemini
        {
            get { return get_int_Prop("<411:"); }
            set { 
                if (TrackingRateMode=="Comet Rate")
                    GeminiHardware.DoCommandResult(">411:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false);  
            }
        }

        public int TrackingDivisorDEC
        {
            get { return (int)get_Profile("TrackingDivisorDEC", 0); }
            set { mProfile["TrackingDivisorDEC"] = value; }
        }

        private int TrackingDivisorDEC_Gemini
        {
            get { return get_int_Prop("<412:"); }
            set {
                if (TrackingRateMode == "Comet Rate")
                    GeminiHardware.DoCommandResult(">412:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false);
            }
        }

        public int ManualSlewSpeed
        {
            get { return (int)get_Profile("ManualSlewSpeed", 800); }
            set { mProfile["ManualSlewSpeed"] = value; }
        }

        private int ManualSlewSpeed_Gemini
        {
            get { return get_int_Prop("<120:");  }
            set { GeminiHardware.DoCommandResult(">120:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int GotoSlewSpeed
        {
            get { return (int)get_Profile("GotoSlewSpeed", 800); }
            set { mProfile["GotoSlewSpeed"] = value; }
        }

        private int GotoSlewSpeed_Gemini
        {
            get { return get_int_Prop("<140:"); }
            set { GeminiHardware.DoCommandResult(">140:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int CenteringSpeed
        {
            get { return (int)get_Profile("CenteringSpeed", 20); }
            set { mProfile["CenteringSpeed"] = value; }
        }

        private int CenteringSpeed_Gemini
        {
            get { return get_int_Prop("<170:"); }
            set { GeminiHardware.DoCommandResult(">170:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public double GuideSpeed
        {
            get { return (double)get_Profile("GuideSpeed", 0.5); }
            set { mProfile["GuideSpeed"] = value; }
        }

        private double GuideSpeed_Gemini
        {
            get { return get_double_Prop("<150:"); }
            set { GeminiHardware.DoCommandResult(">150:" + value.ToString("0.0"), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int SlewSettleTime
        {
            get { return (int)get_Profile("SlewSettleTime", 0); }
            set { mProfile["SlewSettleTime"] = value; }
        }

        private int SlewSettleTime_Gemini
        {
            get { return GeminiHardware.SlewSettleTime; }
            set { GeminiHardware.SlewSettleTime  = value; }
        }


        public int WormGearRatioRA
        {
            get { return (int)get_Profile("WormGearRatioRA", 0); }
            set { mProfile["WormGearRatioRA"] = value; }
        }

        private int WormGearRatioRA_Gemini
        {
            get { return get_int_Prop("<21:"); }
            set {
                if (MountType == "Custom")
                    GeminiHardware.DoCommandResult(">21:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int WormGearRatioDEC
        {
            get { return (int)get_Profile("WormGearRatioDEC", 0); }
            set { mProfile["WormGearRatioDEC"] = value; }
        }

        private int WormGearRatioDEC_Gemini
        {
            get { return get_int_Prop("<22:"); }
            set {
                if (MountType == "Custom")
                    GeminiHardware.DoCommandResult(">22:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false);
            }
        }

        public int SpurGearRatioRA
        {
            get { return (int)get_Profile("SpurGearRatioRA", 0); }
            set { mProfile["SpurGearRatioRA"] = value; }
        }

        private int SpurGearRatioRA_Gemini
        {
            get { return get_int_Prop("<23:"); }
            set {
                if (MountType == "Custom")
                    GeminiHardware.DoCommandResult(">23:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false);
            }
        }

        public int SpurGearRatioDEC
        {
            get { return (int)get_Profile("SpurGearRatioDEC", 0); }
            set { mProfile["SpurGearRatioDEC"] = value; }
        }

        private int SpurGearRatioDEC_Gemini
        {
            get { return get_int_Prop("<24:"); }
            set {
                if (MountType == "Custom")
                    GeminiHardware.DoCommandResult(">24:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false);
            }
        }

        public int ServoEncoderResolutionRA
        {
            get { return (int)get_Profile("ServoEncoderResolutionRA", 0); }
            set { mProfile["ServoEncoderResolutionRA"] = value; }
        }

        private int ServoEncoderResolutionRA_Gemini
        {
            get { return get_int_Prop("<25:"); }
            set {
                if (MountType == "Custom")
                    GeminiHardware.DoCommandResult(">25:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false);
            }
        }

        public int ServoEncoderResolutionDEC
        {
            get { return (int)get_Profile("ServoEncoderResolutionDEC", 0); }
            set { mProfile["ServoEncoderResolutionDEC"] = value; }
        }

        private int ServoEncoderResolutionDEC_Gemini
        {
            get { return get_int_Prop("<26:"); }
            set {
                if (MountType == "Custom")
                    GeminiHardware.DoCommandResult(">26:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false);
            }
        }

        public int StepsPerWormRevolutionRA
        {
            get { return (int)get_Profile("StepsPerWormRevolutionRA", 0); }
            set { mProfile["StepsPerWormRevolutionRA"] = value; }
        }

        private int StepsPerWormRevolutionRA_Gemini
        {
            get { return get_int_Prop("<27:"); }
            set
            {
                if (MountType == "Custom")
                    GeminiHardware.DoCommandResult(">27:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false);
            }
        }

        public int StepsPerWormRevolutionDEC
        {
            get { return (int)get_Profile("StepsPerWormRevolutionDEC", 0); }
            set { mProfile["StepsPerWormRevolutionDEC"] = value; }
        }

        private int StepsPerWormRevolutionDEC_Gemini
        {
            get { return get_int_Prop("<28:"); }
            set {
                if (MountType == "Custom")
                    GeminiHardware.DoCommandResult(">28:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false);
            }
        }


        public bool UseEncoders
        {
            get { return (bool)get_Profile("UseEncoders", false); }
            set { mProfile["UseEncoders"] = value; }
        }


        private bool UseEncoders_Gemini
        {
            get
            {
                int res = get_int_Prop("<10:");
                if (res == 11) return true;
                return false;
            }
            set { GeminiHardware.DoCommandResult(value ? ">11:" : ">13:", GeminiHardware.MAX_TIMEOUT, false); }
        }



        public bool UseLimitSwitches
        {
            get { return (bool)get_Profile("UseLimitSwitches", false); }
            set { mProfile["UseLimitSwitches"] = value; }
        }


        private bool UseLimitSwitches_Gemini
        {
            get
            {
                int res = get_int_Prop("<10:");
                if (res == 14) return true;
                return false;
            }
            set { GeminiHardware.DoCommandResult(value ? ">14:" : ">15:", GeminiHardware.MAX_TIMEOUT, false); }
        }


        public int EncoderResolutionRA
        {
            get { return (int)get_Profile("EncoderResolutionRA", 0); }
            set { mProfile["EncoderResolutionRA"] = value; }
        }

        private int EncoderResolutionRA_Gemini
        {
            get { return get_int_Prop("<100:"); }
            set { GeminiHardware.DoCommandResult(">100:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int EncoderResolutionDEC
        {
            get { return (int)get_Profile("EncoderResolutionDEC", 0); }
            set { mProfile["EncoderResolutionDEC"] = value; }
        }

        private int EncoderResolutionDEC_Gemini
        {
            get { return get_int_Prop("<110:"); }
            set { GeminiHardware.DoCommandResult(">110:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }


        public int TVC
        {
            get { return (int)get_Profile("TVC", 0); }
            set { mProfile["TVC"] = value; }
        }

        private int TVC_Gemini
        {
            get { return get_int_Prop("<200:"); }
            set { GeminiHardware.DoCommandResult(">200:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int ModelA
        {
            get { return (int)get_Profile("ModelA", 0); }
            set { mProfile["ModelA"] = value; }
        }

        private int ModelA_Gemini
        {
            get { return get_int_Prop("<201:"); }
            set { GeminiHardware.DoCommandResult(">201:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); } 
        }

        public int ModelE
        {
            get { return (int)get_Profile("ModelE", 0); }
            set { mProfile["ModelE"] = value; }
        }

        private int ModelE_Gemini
        {
            get { return get_int_Prop("<202:"); }
            set { GeminiHardware.DoCommandResult(">202:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int ModelNP
        {
            get { return (int)get_Profile("ModelNP", 0); }
            set { mProfile["ModelNP"] = value; }
        }

        private int ModelNP_Gemini
        {
            get { return get_int_Prop("<203:"); }
            set { GeminiHardware.DoCommandResult(">203:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int ModelNE
        {
            get { return (int)get_Profile("ModelNE", 0); }
            set { mProfile["ModelNE"] = value; }
        }

        private int ModelNE_Gemini
        {
            get { return get_int_Prop("<204:"); }
            set { GeminiHardware.DoCommandResult(">204:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int ModelIH
        {
            get { return (int)get_Profile("ModelIH", 0); }
            set { mProfile["ModelIH"] = value; }
        }

        private int ModelIH_Gemini
        {
            get { return get_int_Prop("<205:"); }
            set { GeminiHardware.DoCommandResult(">205:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int ModelID
        {
            get { return (int)get_Profile("ModelID", 0); }
            set { mProfile["ModelID"] = value; }
        }

        private int ModelID_Gemini
        {
            get { return get_int_Prop("<206:"); }
            set { GeminiHardware.DoCommandResult(">206:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int ModelFR
        {
            get { return (int)get_Profile("ModelFR", 0); }
            set { mProfile["ModelFR"] = value; }
        }

        private int ModelFR_Gemini
        {
            get { return get_int_Prop("<207:"); }
            set { GeminiHardware.DoCommandResult(">207:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int ModelFD
        {
            get { return (int)get_Profile("ModelFD", 0); }
            set { mProfile["ModelFD"] = value; }
        }

        private int ModelFD_Gemini
        {
            get { return get_int_Prop("<208:"); }
            set { GeminiHardware.DoCommandResult(">208:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int ModelCF
        {
            get { return (int)get_Profile("ModelCF", 0); }
            set { mProfile["ModelCF"] = value; }
        }

        private int ModelCF_Gemini
        {
            get { return get_int_Prop("<209:"); }
            set { GeminiHardware.DoCommandResult(">209:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }

        public int ModelTF
        {
            get { return (int)get_Profile("ModelTF", 0); }
            set { mProfile["ModelTF"] = value; }
        }

        private int ModelTF_Gemini
        {
            get { return get_int_Prop("<211:"); }
            set { GeminiHardware.DoCommandResult(">211:" + value.ToString(), GeminiHardware.MAX_TIMEOUT, false); }
        }


        public bool SavePEC
        {
            get { return (bool)get_Profile("SavePEC", false); }
            set {
                if (value == SavePEC) return;   // was already set to true
                mProfile["SavePEC"] = value;
                if (value) PECTable = PECTable_Gemini;  // get the table from Gemini
                else
                    PECTable = new SerializableDictionary<int, string>();
            }
        }

        /// <summary>
        /// this doesn't participate in data binding, and so is marked private
        /// </summary>
        private SerializableDictionary<int, string> PECTable
        {
            get { return SerializableDictionary<int, string>.Parse((string)get_Profile("PECTable", null)); }
            set { mProfile["PECTable"] = value.ToString(); }
        }

        private SerializableDictionary<int, string> PECTable_Gemini
        {
            get
            {
                Cursor.Current = Cursors.WaitCursor;
                SerializableDictionary<int, string> pec = new SerializableDictionary<int, string>();
                int MaxPEC = get_int_Prop("<503:");
                for (int i = 0; i < MaxPEC; )
                {
                    string val = get_string_Prop("<511:" + i.ToString());
                    string[] parts = val.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    pec.Add(i, parts[0]+";"+i.ToString() + ";" + parts[1]);
                    i += int.Parse(parts[1]);
                }
                Cursor.Current = Cursors.Default;
                return pec;
            }
            set
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (KeyValuePair<int, string> kp in value)
                {
                    GeminiHardware.DoCommandResult(">511:" + kp.Value.ToString(), GeminiHardware.MAX_TIMEOUT, false);
                }
                GeminiHardware.PECStatus = 32+2;      // PEC data is available
                Cursor.Current = Cursors.Default;
            }
        }

#endregion


        /// <summary>
        /// Synchronize current profile with Gemini
        /// </summary>
        /// <param name="write">if true, write current profile settings to Gemini,
        /// if false, read the current settings from Gemini
        /// </param>
        /// <returns>success or failure</returns>
        public bool SyncWithGemini(bool write)
        {
            GeminiHardware.Trace.Enter("GeminiProps:SyncWithGemini", write);

            if (!GeminiHardware.Connected)
            {
                GeminiHardware.Trace.Exit("GeminiProps:SyncWithGemini", false, "mount not connected");
                return false;
            }

            PropertyInfo [] ps = typeof(GeminiProperties).GetProperties(BindingFlags.Public|BindingFlags.SetProperty|BindingFlags.GetProperty|BindingFlags.Instance);
            foreach (PropertyInfo p in ps)
            {
                string name = p.Name;
                PropertyInfo pGemini = typeof(GeminiProperties).GetProperty(p.Name + "_Gemini", BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.Instance);

                // for each public property in this class
                // find an equivalent propert with _Gemini appended to it,
                // and synchronize it with the public property.
                // public properties read/write to the Profile dictionary only
                // private properties that end with _Gemini read/write to Gemini directly
                try
                {
                    if (pGemini != null)
                        if (write && pGemini.GetSetMethod(true) !=null && p.GetGetMethod()!=null)
                            pGemini.SetValue(this, p.GetValue(this, null), null);
                        else if (!write && p.GetSetMethod()!=null && pGemini.GetGetMethod(true)!=null)
                            p.SetValue(this, pGemini.GetValue(this, null), null);
                }
                catch (Exception ex)
                {
                    GeminiHardware.Trace.Except(ex);
                }
            }

            // save PEC table if requested:
            if (write && SavePEC)
            {
                PECTable_Gemini = PECTable;
            }
            else if (!write && SavePEC)
            {
                PECTable = PECTable_Gemini;
            }


            GeminiHardware.Trace.Exit("GeminiProps:SyncWithGemini", write);
            return true;
        }

        /// <summary>
        /// Read/write the Profile dictionary to disk
        /// </summary>
        /// <param name="write">true to write, false to read</param>
        /// <param name="FileName">full path to the file to read/write</param>
        /// <returns></returns>
        public bool Serialize(bool write, string FileName)
        {
            // default profile is being read/written to if FileName is null:
            if (FileName == null)
            {
                string path = "";
                try
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME;
                    System.IO.Directory.CreateDirectory(path);
                }
                catch
                {
                }

                FileName = path + "\\" + SharedResources.DEAULT_PROFILE;

                if (!write && !File.Exists(FileName)) return false;
            }

            GeminiHardware.Trace.Enter("GeminiProps:Serialize", write, FileName);

            try
            {
                if (write)
                {
                    TextWriter writer = new StreamWriter(FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, object>));
                    serializer.Serialize(writer, mProfile);
                    writer.Close();
                }
                else
                {
                    TextReader reader = new StreamReader(FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, object>));
                    mProfile = (SerializableDictionary<string, object>)serializer.Deserialize(reader);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                GeminiHardware.Trace.Error("GeminiProperties.Serialize", write, FileName, ex.ToString(), ex.Message);
                return false;
            }

            GeminiHardware.Trace.Exit("GeminiProps:Serialize", write, FileName);

            return true;
        }

    }

    /// <summary>
    /// SerializableDictionary is a serializable generic dictionary class that .NET should've implemented
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable, ICloneable
    {
        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        { 
            return null;
        }

        /// <summary>
        /// deep copy of the dictionary
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            MemoryStream ms = new MemoryStream();

            TextWriter writer = new StreamWriter(ms);
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<TKey, TValue>));
            serializer.Serialize(writer, this);

            ms.Seek(0, SeekOrigin.Begin);

            TextReader reader = new StreamReader(ms);
            object res = (object)serializer.Deserialize(reader);
            writer.Close();
            reader.Close();
            return res;
        }

        /// <summary>
        /// serialize into a string (xml)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            MemoryStream ms = new MemoryStream();

            TextWriter writer = new StreamWriter(ms);
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<TKey, TValue>));
            serializer.Serialize(writer, this);
            ms.Flush();
            long len = ms.Position;
            ms.Seek(0, SeekOrigin.Begin);
            byte[] buf = new byte[len];
            ms.Read(buf, 0, (int)len);
            ms.Close();
            return ASCIIEncoding.ASCII.GetString(buf);

        }

        /// <summary>
        /// deserialize from string (xml)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public  static SerializableDictionary<TKey, TValue> Parse(string s)
        {
            MemoryStream ms = new MemoryStream();

            ms.Write(ASCIIEncoding.ASCII.GetBytes(s), 0, s.Length);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            TextReader reader = new StreamReader(ms);
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<TKey, TValue>));

            object res = (object)serializer.Deserialize(reader);
            reader.Close();
            return (SerializableDictionary<TKey, TValue>)res;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();

            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {

            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
        #endregion
    }


}
