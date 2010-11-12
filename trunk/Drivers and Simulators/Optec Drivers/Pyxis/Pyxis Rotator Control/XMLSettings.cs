using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using Optec;
using System.Diagnostics;



namespace Pyxis_Rotator_Control
{

    class XMLSettings
    {
        private static string XmlFilename = "PyxisSettings.XML";
        private static string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Optec\\Pyxis";
        private static string AppDataSettingsFile = System.IO.Path.Combine(AppDataPath, XmlFilename);
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
                if (!System.IO.File.Exists(AppDataSettingsFile))
                {
                    // First Find out if the folder exists
                    if(!System.IO.Directory.Exists(AppDataPath))
                        System.IO.Directory.CreateDirectory(AppDataPath);

                    // Create the settings file...
                    XElement SettingsElement = new XElement(MainElementName);
                    XDeclaration dec = new XDeclaration("1.0", "UTF-8", "yes");
                    PyxisSettingsDocument = new XDocument(dec, SettingsElement);
                    PyxisSettingsDocument.Save(AppDataSettingsFile);
                }
                else PyxisSettingsDocument = XDocument.Load(AppDataSettingsFile);
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
                PyxisSettingsDocument.Save(AppDataSettingsFile);
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
            PyxisSettingsDocument.Save(AppDataSettingsFile);
        }

        public static int ParkPosition
        {
            get { return (int)getPropertyFromXML("ParkPosition", typeof(int), 0); }
            set { setPropertyInXML("ParkPosition", value); }
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

        public static int StepTime
        {
            get { return (int)getPropertyFromXML("StepTime", typeof(int), 8); }
            set { setPropertyInXML("StepTime", value); }
        }

        /// <summary>
        /// Do Not set this property from anywhere other than the OptecPyxis class.
        /// </summary>
        public static int SkyPAOffset
        {
            get { return (int)getPropertyFromXML("SkyPAOffset", typeof(int), 0); }
            set { setPropertyInXML("SkyPAOffset", value); }
        }

        public static OptecPyxis.SteppingModes SteppingMode
        {
            get { return (OptecPyxis.SteppingModes)getPropertyFromXML("SteppingMode", typeof(OptecPyxis.SteppingModes), OptecPyxis.SteppingModes.FullStep); }
            set { setPropertyInXML("SteppingMode", value); }
        }

        public static bool HomeOnStart
        {
            get { return (bool)getPropertyFromXML("HomeOnStart", typeof(bool), true); }
            set { setPropertyInXML("HomeOnStart", value); }
        }
    }



}
