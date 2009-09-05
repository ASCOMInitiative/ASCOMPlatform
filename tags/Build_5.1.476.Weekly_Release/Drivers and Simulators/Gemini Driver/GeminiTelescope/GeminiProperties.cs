using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace ASCOM.GeminiTelescope
{
    class GeminiProperties : Component, INotifyPropertyChanged
    {
        static string[] Mounts = {"Custom", "GM-8", "G-11", "HGM-200", "MI-250", "Titan", "Titan50"};
        static string[] TrackingRates = { "Sidereal", "King Rate", "Lunar", "Solar", "Terrestrial", "Closed Loop", "Comet Rate" };
        static string[] HandController = { "Visual", "Photo", "All Speeds" };

        public event PropertyChangedEventHandler PropertyChanged;


        public GeminiProperties()
        {
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
            string res;
            try
            {
                res = GeminiHardware.DoCommandResult(s, 1000, false);
            }
            catch
            {
                return null;
            }
            return res;
        }

        public string GeminiVersion
        {
            get
            {
                if (!GeminiHardware.Connected) return null;
                string res = get_Prop(":GVN");
                return string.Format("L{0} v{1}.{2}", GeminiHardware.Version.Substring(0, 1), GeminiHardware.Version.Substring(1, 1), GeminiHardware.Version.Substring(2,1));
            }
        }

        public string MountType
        {
            get
            {
                string res = get_Prop("<0:");
                int idx = 0;
                if (!int.TryParse(res, out idx)) return null;
                return Mounts[idx];
            }
        }

        public string LocalTime
        {
            get
            {
                string res = get_Prop(":GL");
                return res;
            }
        }

        public string LocalDate
        {
            get
            {
                string res = get_Prop(":GC");
                return res;
            }
        }

        public string TrackingRate
        {
            get
            {
                string res = get_Prop("<130:");
                int rate ;
                if (!int.TryParse(res, out rate)) return null;
                return TrackingRates[rate-131];
            }
        }

        public string TimeZone
        {
            get
            {
                if (!GeminiHardware.Connected) return null;
                // gemini stores timezone with sign reversed from UTC:
                return "UTC" + (GeminiHardware.UTCOffset > 0? " - " : " + ") + GeminiHardware.UTCOffset.ToString();
            }
        }

        public string HandControllerMode
        {
            get
            {
                string prop = get_Prop("<160:");
                int hc;
                if (!int.TryParse(prop, out hc)) return null;
                return HandController[hc - 161];
            }
        }

        public string GotoSlewRate
        {
            get
            {
                string prop = get_Prop("<140:");
                int rate;
                if (!int.TryParse(prop, out rate)) return null;
                return rate.ToString() + "x";
            }
        }

        public string ManualSlewRate
        {
            get
            {
                string prop = get_Prop("<120:");
                int rate;
                if (!int.TryParse(prop, out rate)) return null;
                return rate.ToString() + "x";
            }
        }

        public string GuideRate
        {
            get
            {
                string prop = get_Prop("<150:");
                double rate;
                if (!double.TryParse(prop, out rate)) return null;
                return rate.ToString("0.0") + "x";
            }
        }


        public string CenteringRate
        {
            get
            {
                string prop = get_Prop("<170:");
                int rate;
                if (!int.TryParse(prop, out rate)) return null;
                return rate.ToString() + "x";
            }
        }

        public string PECStatus
        {
            get
            {
                string prop = get_Prop("<509:");
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
                double ra  = GeminiHardware.RightAscension;
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

    }
}
