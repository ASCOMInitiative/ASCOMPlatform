using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using Optec;
using System.Diagnostics;
using System.IO;


namespace Optec_TCF_S_Focuser
{

    class XMLSettings
    {
        private static string xpath = string.Empty;
        private static string XmlFilename = "TCFSettings.XML";
        
 
        private static XDocument TCFSettingsDocument;

        static XMLSettings()
        {
            LoadXML();

        }

        /// <summary>
        /// Finds the XML file that contains the device settings. If the file doesn't exist
        /// it creates the file with all of the default values.
        /// </summary>
        public static void LoadXML()
        {
            try
            {
                EventLogger.LogMessage("Loading XML Settings Data.", TraceLevel.Info);
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                xpath = appDataPath + "\\Optec\\" + "TCF_S" + "\\" + "SingleInstance";
                if (!Directory.Exists(xpath))
                    Directory.CreateDirectory(xpath);

                xpath = Path.Combine(xpath, XmlFilename);

                if (!System.IO.File.Exists(xpath))
                {
                    // Create the settings file...

                    XElement SettingsElement = new XElement("TCFSettings");
                    XElement FocusOffsetsElement = new XElement("FocusOffsets");
                    XElement AbsolutePresetsElement = new XElement("AbsolutePresets");

                    SettingsElement.Add(FocusOffsetsElement);
                    SettingsElement.Add(AbsolutePresetsElement);

                    XDeclaration dec = new XDeclaration("1.0", "UTF-8", "yes");
                    TCFSettingsDocument = new XDocument(dec, SettingsElement);
                    TCFSettingsDocument.Save(xpath);
                }

                else TCFSettingsDocument = XDocument.Load(xpath);
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }

        }

        private static object getPropertyFromXML(string PropertyName, Type t, object defaultValue)
        {
            try
            {
                EventLogger.LogMessage("Getting property  " + PropertyName + " from xml file.", TraceLevel.Verbose);
                XElement e = TCFSettingsDocument.Descendants("TCFSettings").Descendants().Single(i => i.Name == PropertyName);

                if (t.IsEnum)
                {
                    object x = Enum.Parse(t, e.Value);
                    return x;
                }

                else return Convert.ChangeType(e.Value, t);

            }
            catch (System.InvalidOperationException ex)
            {
                EventLogger.LogMessage(ex);
                CreatePropertyInXML(PropertyName, defaultValue);
                return getPropertyFromXML(PropertyName, t, defaultValue);
            }

        }

        private static void setPropertyInXML(string PropertyName, object NewValue)
        {
            try
            {
                EventLogger.LogMessage("Getting property  " + PropertyName + "to " + NewValue.ToString() + " in xml file.", TraceLevel.Verbose);
                XElement x = TCFSettingsDocument.Descendants("TCFSettings").Descendants().Single(i => i.Name == PropertyName);
                x.SetValue(NewValue);
                TCFSettingsDocument.Save(xpath);
            }
            catch (InvalidOperationException ex)
            {
                EventLogger.LogMessage(ex);
                CreatePropertyInXML(PropertyName, NewValue);
            }
        }

        private static void CreatePropertyInXML(string newPropertyName, object DefaultValue)
        {
            XElement MainElement = TCFSettingsDocument.Descendants().Single(i => i.Name == "TCFSettings");
            XElement NewElement = new XElement(newPropertyName, DefaultValue.ToString());
            MainElement.Add(NewElement);
            TCFSettingsDocument.Save(xpath);
        }

        public static OptecFocuser.DisplayBrightnessValues DisplayBrightness
        {
            get
            {
                return (OptecFocuser.DisplayBrightnessValues)getPropertyFromXML("DisplayBrightness",
                    typeof(OptecFocuser.DisplayBrightnessValues), OptecFocuser.DisplayBrightnessValues.MEDIUM);
            }
            set
            {
                setPropertyInXML("DisplayBrightness", value);
            }
        }

        public static bool DeviceTypeManuallySet
        {
            get
            {
                return (bool)getPropertyFromXML("DeviceTypeManuallySet", typeof(bool), false);
            }
            set
            {
                setPropertyInXML("DeviceTypeManuallySet", value);
            }
        }

        public static OptecFocuser.DeviceTypes DeviceType
        {
            get
            {
                return (OptecFocuser.DeviceTypes)getPropertyFromXML("DeviceType",
                    typeof(OptecFocuser.DeviceTypes), OptecFocuser.DeviceTypes.TCF_S);
            }
            set { setPropertyInXML("DeviceType", value); }
        }

        public static bool TemperatureProbeDisabled
        {
            get { return (bool)getPropertyFromXML("TemperatureProbeDisabled", typeof(bool), false); }
            set { setPropertyInXML("TemperatureProbeDisabled", value); }
        }

        public static double TemperatureOffsetC
        {
            get { return (double)getPropertyFromXML("TemperatureOffsetC", typeof(double), 0); }
            set { setPropertyInXML("TemperatureOffsetC", value); }
        }

        public static OptecFocuser.TempCompModes TempCompMode
        {
            get { return (OptecFocuser.TempCompModes)getPropertyFromXML("TempCompMode", typeof(OptecFocuser.TempCompModes), OptecFocuser.TempCompModes.A); }
            set { setPropertyInXML("TempCompMode", value); }
        }

        public static OptecFocuser.TemperatureUnits DisplayTempUnits
        {
            get
            {
                return (OptecFocuser.TemperatureUnits)getPropertyFromXML("DisplayTempUnits",
                    typeof(OptecFocuser.TemperatureUnits),
                    OptecFocuser.TemperatureUnits.Celsius);
            }
            set { setPropertyInXML("DisplayTempUnits", value); }
        }

        public static OptecFocuser.PositionUnits DisplayPositionUnits
        {
            get
            {
                return (OptecFocuser.PositionUnits)getPropertyFromXML(
                    "DisplayPositionUnits", typeof(OptecFocuser.PositionUnits),
                    OptecFocuser.PositionUnits.Steps);
            }
            set
            {
                setPropertyInXML("DisplayPositionUnits", value);
            }
        }

        public static int AutoADelay
        {
            get { return (int)getPropertyFromXML("AutoADelay", typeof(int), 1); }
            set { setPropertyInXML("AutoADelay", value); }
        }

        public static int AutoBDelay
        {
            get { return (int)getPropertyFromXML("AutoBDelay", typeof(int), 1); }
            set { setPropertyInXML("AutoBDelay", value); }
        }

        public static List<FocusOffset> SavedFocusOffsets
        {
            get
            {
                List<FocusOffset> returnList = new List<FocusOffset>();
                XElement MainElement = TCFSettingsDocument.Descendants().Single(i => i.Name == "TCFSettings").
                Descendants().Single(j => j.Name == "FocusOffsets");

                foreach (XElement x in MainElement.Descendants())
                {
                    if (x.Name == "Offset")
                    {
                        string name = x.Element("Name").Value;
                        int value = int.Parse(x.Element("Steps").Value);
                        FocusOffset fo = new FocusOffset(name, value);
                        returnList.Add(fo);
                    }
                }
                return returnList;
            }
        }

        public static List<FocusOffset> SavedAbsolutePresets
        {
            get
            {
                List<FocusOffset> returnList = new List<FocusOffset>();
                XElement MainElement = TCFSettingsDocument.Descendants().Single(i => i.Name == "TCFSettings").
                Descendants().Single(j => j.Name == "AbsolutePresets");

                foreach (XElement x in MainElement.Descendants())
                {
                    if (x.Name == "Offset")
                    {
                        string name = x.Element("Name").Value;
                        int value = int.Parse(x.Element("Steps").Value);
                        FocusOffset fo = new FocusOffset(name, value);
                        returnList.Add(fo);
                    }
                }
                return returnList;
            }
        }

        public static void AddFocusOffset(FocusOffset f)
        {
            XElement Name = new XElement("Name", f.OffsetName);
            XElement Steps = new XElement("Steps", f.OffsetSteps.ToString());

            XElement Offset = new XElement("Offset", new object[] { Name, Steps });
            // Add the offset to the offsets section of the file.
            XElement MainElement = TCFSettingsDocument.Descendants().Single(i => i.Name == "TCFSettings").
                Descendants().Single(j => j.Name == "FocusOffsets");
            MainElement.Add(Offset);
            TCFSettingsDocument.Save(xpath);
        }

        public static void AddAbsolutePreset(FocusOffset f)
        {
            XElement Name = new XElement("Name", f.OffsetName);
            XElement Steps = new XElement("Steps", f.OffsetSteps.ToString());

            XElement Offset = new XElement("Offset", new object[] { Name, Steps });
            // Add the offset to the offsets section of the file.
            XElement MainElement = TCFSettingsDocument.Descendants().Single(i => i.Name == "TCFSettings").
                Descendants().Single(j => j.Name == "AbsolutePresets");
            MainElement.Add(Offset);
            TCFSettingsDocument.Save(xpath);
        }

        public static void RemoveFocusOffset(FocusOffset f)
        {
            XElement ElementToRemove = TCFSettingsDocument.Descendants().Single(i => i.Name == "TCFSettings").
                Descendants().Single(j => j.Name == "FocusOffsets").Descendants("Offset").Single(k => k.Value == f.OffsetName + f.OffsetSteps.ToString());

            ElementToRemove.Remove();
            TCFSettingsDocument.Save(xpath);

        }

        public static void RemoveAbsolutePreset(FocusOffset f)
        {
            XElement ElementToRemove = TCFSettingsDocument.Descendants().Single(i => i.Name == "TCFSettings").
                Descendants().Single(j => j.Name == "AbsolutePresets").Descendants("Offset").Single(k => k.Value == f.OffsetName + f.OffsetSteps.ToString());

            ElementToRemove.Remove();
            TCFSettingsDocument.Save(xpath);
        }

        public static bool CheckOffsetNameUnique(string name)
        {
            List<FocusOffset> savedoffsets = SavedFocusOffsets;
            foreach (FocusOffset f in savedoffsets)
            {
                if (f.OffsetName.ToUpper() == name.ToUpper()) return false;
            }
            return true;
        }

        public static bool CheckPresetNameUnique(string name)
        {
            List<FocusOffset> savedPresets = SavedAbsolutePresets;
            foreach (FocusOffset p in savedPresets)
            {
                if (p.OffsetName.ToUpper() == name.ToUpper()) return false;
            }
            return true;
        }

        public static string SavedSerialPortName
        {
            get
            {
                return (string)getPropertyFromXML("SerialPortName", typeof(string), "COM1");
            }
            set
            {
                setPropertyInXML("SerialPortName", value);
            }
        }

        public static TraceLevel LoggerTraceLevel
        {
            get
            {
                return (TraceLevel)getPropertyFromXML("LoggerTraceLevel", typeof(TraceLevel), TraceLevel.Warning);
            }
            set
            {
                setPropertyInXML("LoggerTraceLevel", value);
            }
        }
    }



}
