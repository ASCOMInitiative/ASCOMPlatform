using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optec;
using System.Xml.Linq;
using System.Diagnostics;
using System.Reflection;
using System.ComponentModel;
using System.Configuration.Install;


namespace PyxisLE_Control
{
    /// <summary>
    /// Custom Install actions to be carried out during product installation/uninstallation
    /// </summary>
    [RunInstaller(true)]
    public class XMLFileRemover : Installer
    {
        /// <summary>
        /// Custom Uninstall action that removes the xml file used for storing settings.
        /// </summary>
        /// <param name="savedState"></param>
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            try
            {
                // 1. Try to find the xml settings file and remove it

                string XmlFilename = "PyxisLE_Settings.XML";
                string asmpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string xpath = System.IO.Path.Combine(asmpath, XmlFilename);
                if (System.IO.File.Exists(xpath))
                {
                    // Delete the file
                    System.IO.File.Delete(xpath);
                }

            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
            }

            base.Uninstall(savedState);
        }

    }

    class XmlSettings
    {
        private static string XmlFilename = "PyxisLE_Settings.XML";
        private static string asmpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string xpath = System.IO.Path.Combine(asmpath, XmlFilename);
        private static XDocument mySettingsDocument;

        private static string MainElementName = "PyxisLE_Settings";
        static XmlSettings()
        {
            LoadXML();
        }

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
                    mySettingsDocument = new XDocument(dec, SettingsElement);
                    mySettingsDocument.Save(xpath);
                }
                else mySettingsDocument = XDocument.Load(xpath);
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
                XElement e = mySettingsDocument.Descendants(MainElementName).Descendants().Single(i => i.Name == PropertyName);

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
                XElement x = mySettingsDocument.Descendants(MainElementName).Descendants().Single(i => i.Name == PropertyName);
                x.SetValue(NewValue);
                mySettingsDocument.Save(xpath);
            }
            catch (InvalidOperationException ex)
            {
                EventLogger.LogMessage(ex);
                CreatePropertyInXML(PropertyName, NewValue);
            }
        }

        private static void CreatePropertyInXML(string newPropertyName, object DefaultValue)
        {
            XElement MainElement = mySettingsDocument.Descendants().Single(i => i.Name == MainElementName);
            XElement NewElement = new XElement(newPropertyName, DefaultValue.ToString());
            MainElement.Add(NewElement);
            mySettingsDocument.Save(xpath);
        }

        public static float ParkPosition
        {
            get { return (float)getPropertyFromXML("ParkPosition", typeof(float), 0); }
            set { setPropertyInXML("ParkPosition", value); }
        }

        public static bool CheckForUpdates
        {
            get { return (bool)getPropertyFromXML("CheckForUpdates", typeof(bool), true); }
            set { setPropertyInXML("CheckForUpdates", value); }

        }

        public static TraceLevel LoggingLevel 
        { 
            get { return (TraceLevel)getPropertyFromXML("LoggingLevel", typeof(TraceLevel), TraceLevel.Warning);}
            set { setPropertyInXML("LoggingLevel", value); }
        }
    }


    
}
