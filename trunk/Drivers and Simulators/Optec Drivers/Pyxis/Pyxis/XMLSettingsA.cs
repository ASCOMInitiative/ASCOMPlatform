using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using Optec;
using System.Diagnostics;


namespace ASCOM.Pyxis
{

    class XMLSettings
    {
        private static string XmlFilename = "PyxisSettings.XML";
        private static string asmpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string xpath = System.IO.Path.Combine(asmpath, XmlFilename);
        private static XDocument PyxisSettingsDocument;
        private const string MainElementName = "PyxisSettings";

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
                if (!System.IO.File.Exists(xpath))
                {
                    // Create the settings file...
                    XElement SettingsElement = new XElement(MainElementName);    
                    XDeclaration dec = new XDeclaration("1.0", "UTF-8", "yes");
                    PyxisSettingsDocument = new XDocument(dec, SettingsElement);
                    PyxisSettingsDocument.Save(xpath);
                }
                else PyxisSettingsDocument = XDocument.Load(xpath);
            }
            catch (Exception)
            {

                throw;
            }

        }

        private static object getPropertyFromXML(string PropertyName, Type t, object defaultValue)
        {
            try
            {
                EventLogger.LogMessage("Getting property  " + PropertyName + " from xml file.", TraceLevel.Verbose);
                XElement e = PyxisSettingsDocument.Descendants(MainElementName).Descendants().Single(i => i.Name == PropertyName);

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
                XElement x = PyxisSettingsDocument.Descendants(MainElementName).Descendants().Single(i => i.Name == PropertyName);
                x.SetValue(NewValue);
                PyxisSettingsDocument.Save(xpath);
            }
            catch (System.InvalidOperationException ex)
            {
                EventLogger.LogMessage(ex);
                CreatePropertyInXML(PropertyName, NewValue);
            }
        }

        private static void CreatePropertyInXML(string newPropertyName, object DefaultValue)
        {
            XElement MainElement = PyxisSettingsDocument.Descendants().Single(i => i.Name == MainElementName);
            XElement NewElement = new XElement(newPropertyName, DefaultValue.ToString());
            MainElement.Add(NewElement);
            PyxisSettingsDocument.Save(xpath);
        }

        public static OptecPyxis.DeviceTypes DeviceType
        {
            get
            {
                return (OptecPyxis.DeviceTypes)getPropertyFromXML("DeviceType",
                    typeof(OptecPyxis.DeviceTypes), OptecPyxis.DeviceTypes.TwoInch);
            }
            set { setPropertyInXML("DeviceType", value); }
        }
       
        public static double ParkPosition
        {
            get { return (double)getPropertyFromXML("ParkPosition", typeof(double), 0); }
            set { setPropertyInXML("ParkPosition", value);}
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

        public static bool PositionIsValid
        {
            get { return (bool)getPropertyFromXML("PositionIsValid", typeof(bool), true); }
            set { setPropertyInXML("PositionIsValid", value); }
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

        public static int StepRate
        {
            get { return (int)getPropertyFromXML("StepRate", typeof(int), 8); }
            set { setPropertyInXML("StepRate", value); }
        }

        public static double SkyPAOffset
        {
            get { return (double)getPropertyFromXML("SkyPAOffset", typeof(double), 0); }
            set { setPropertyInXML("SkyPAOffset", value); }
        }

        public static bool HalfStep
        {
            get { return (bool)getPropertyFromXML("HalfStep", typeof(bool), false); }
            set { setPropertyInXML("HalfStep", value); }
        }

        public static bool HomeOnStart
        {
            get { return (bool)getPropertyFromXML("HomeOnStart", typeof(bool), true); }
            set { setPropertyInXML("HomeOnStart", value); }
        }
    }



}
