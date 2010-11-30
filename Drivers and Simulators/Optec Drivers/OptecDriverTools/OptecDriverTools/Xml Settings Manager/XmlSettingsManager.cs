using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;


namespace Optec
{
    public abstract class XmlSettingsManager
    {
        private string productName = string.Empty;
        private string settingsDirectory = string.Empty;
        private const string globalSettingsFileName = "GlobalSettings.xml";
        private const string instanceFilenamePrefix = "Inst_";
        private const string rootElementName_Global = "OptecDeviceGlobalSettings";
        private const string rootElementName_Instance = "SettingsInstance";
        private const string ProcessInfoString = "ProcessID";
        private const string UnusedProcessString = "UNUSED";
        private XDocument GlobalXDoc;
        private XDocument CurrentInstXDoc;

        private string currentInstanceName = String.Empty;
        private string currentInstanceFilename = String.Empty;

        // Public Constructor
        public XmlSettingsManager(string ProductName)
        {
            try
            {
                // Set the path to the settings file.
                EventLogger.LogMessage("Creating new instance of XMLSettingsManager", TraceLevel.Info);
                productName = ProductName;
                string asmVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                settingsDirectory = appDataPath + "\\Optec\\" + ProductName + "\\" + asmVersion + "\\";

                // Check the global settings file
                CheckOrCreateGlobalSettingsFile();

                // Check the instance file
                CheckOrCreateInstanceFiles();

            }
            catch (Exception ex) 
            {
                EventLogger.LogMessage(ex);
            }
        }

      

        private string GlobalSettingsFile
        {
            get { return settingsDirectory + globalSettingsFileName; }
        }

        private XElement GlobalSettingsElement
        {
            get
            {
                return GlobalXDoc.Element(rootElementName_Global);
            }
        }

        private void CheckOrCreateGlobalSettingsFile()
        {
            if (File.Exists(GlobalSettingsFile))
            {
                // The file exists
                EventLogger.LogMessage("Global XML Settings File Found at " + GlobalSettingsFile, TraceLevel.Info);
                // Load the Global settings file
                GlobalXDoc = XDocument.Load(GlobalSettingsFile);
                if (GlobalXDoc.Elements(rootElementName_Global).Any(e => e.Attribute("DeviceName").Value == productName))
                {
                    EventLogger.LogMessage("Global XML Settings File has been verified!", TraceLevel.Info);
                    return;
                }
                else
                {
                    // The file is invalid, create it below.
                    EventLogger.LogMessage("Global XML Settings File is Invalid. Deleting file and recreating...", TraceLevel.Info);
                    File.Delete(GlobalSettingsFile);
                }
            }
            
            // If execution get here the file either does not exist or is invalid.

            EventLogger.LogMessage("XML Settings File NOT Found at " + GlobalSettingsFile, TraceLevel.Info);
            // Check if the folder exists
            FileInfo x = new FileInfo(settingsDirectory);
            if (Directory.Exists(x.DirectoryName))
            {
                // The directory already exists. Create the file below
            }
            else
            {
                // Create the directory
                EventLogger.LogMessage("Creating XML Settings directory at " + x.DirectoryName, TraceLevel.Info);
                Directory.CreateDirectory(x.DirectoryName);
            }

            // Now create the file...
            EventLogger.LogMessage("Creating new Global XML Settings File", TraceLevel.Info);
            XDeclaration dec = new XDeclaration("1.0", "UTF-8", "yes");
            XElement mainElement = new XElement(rootElementName_Global);
            mainElement.SetAttributeValue("DeviceName", productName);
            GlobalXDoc = new XDocument(dec, mainElement);
            GlobalXDoc.Save(GlobalSettingsFile);
            
        }

        private void CheckOrCreateInstanceFiles()
        {
            // Is there at least on instance file?

            // Is each instance file syntactically correct? If not, delete it.

            int validInstances = 0;

            foreach (string fname in Directory.GetFiles(settingsDirectory))
            {
                if(fname.Contains(instanceFilenamePrefix))
                {
                    XDocument potentialInstance = XDocument.Load(fname);
                    // Verify it contains the root instance element
                    if (potentialInstance.Elements(rootElementName_Instance).Any())
                    {
                        // The file structure is good. 
                        EventLogger.LogMessage("Instance file " + fname + " found and verified.", TraceLevel.Info);
                        validInstances++;
                    }
                    else 
                    {
                        // The file is does not contain the correct root element. Delete it
                        EventLogger.LogMessage("Invalid Instance file " + fname + " found and Deleted.", TraceLevel.Warning);
                        File.Delete(fname);
                    }
                    
                }
            }

            // If no instances exist, create one.
            if (validInstances < 1)
            {
                // Create the first (default) instance and set it as the current instance
                EventLogger.LogMessage("Creating new Instance XML Settings File", TraceLevel.Info);
                XDeclaration dec = new XDeclaration("1.0", "UTF-8", "yes");
                XElement rootElement = new XElement(rootElementName_Instance);
                rootElement.SetAttributeValue("Name", productName);
                XDocument newInstanceDoc = new XDocument(dec, rootElement);
                currentInstanceFilename = settingsDirectory + instanceFilenamePrefix + getNewInstanceFilenameSuffix();
                newInstanceDoc.Save(currentInstanceFilename);
                CurrentInstance = productName;
                DefaultInstance = productName;
            }
            else
            {
                // Verify that the default instance exists.
                List<string> instances = getListOfInstances();

                if (!instances.Contains(DefaultInstance))
                    DefaultInstance = instances[0];
                
                // The default instance is set and verified, now set the current instance     

                // Try the default instance first...
                string defInst = DefaultInstance;
                if (!CheckIfInstanceIsInUse(defInst))
                {
                    CurrentInstance = defInst;
                }
                else
                {
                    // Check for other unused instances, and use the first one available...
                    bool instanceFound = false;
                    foreach (string name in instances)
                    {
                        if (!CheckIfInstanceIsInUse(name))
                        {
                            CurrentInstance = name;
                            instanceFound = true;
                            break;
                        }
                    }

                    if (!instanceFound)// Create a new instance to use
                    {
                        // Determine the new name
                        int i = 1;
                        string autoName =  productName + "(Auto-Created " + i.ToString() + ")";
                        while(instances.Contains(autoName))
                        {
                            i++;
                            autoName =  productName + "(Auto-Created " + i.ToString() + ")";
                        }
                        // Name is unique. Create the new instance.
                        CreateNewInstance(autoName);
                        CurrentInstance = autoName;
                    }
                }
            }
        }

        private string getNewInstanceFilenameSuffix()
        {
            DateTime t = DateTime.Now;
            string sec = t.Second.ToString("00");
            string suff =
                t.Month.ToString("00") +
                t.Day.ToString("00") +
                t.Year.ToString().Substring(2, 2) +
                "_" +
                t.Hour.ToString("00") +
                t.Minute.ToString("00") +
                sec +
                ".xml";

            // Wait until the second changes...
            while (DateTime.Now.Second.ToString("00") == sec) { }
            return suff;
        }

        private List<string> getListOfInstances()
        {
            List<string> list = new List<string> { };

            foreach (string fname in Directory.GetFiles(settingsDirectory))
            {
                if (fname.Contains(instanceFilenamePrefix))
                {
                    XDocument potentialInstance = XDocument.Load(fname);
                    // Verify it contains the root instance element
                    if (potentialInstance.Elements(rootElementName_Instance).Any())
                    {
                        list.Add(getInstanceNameFromFile(fname));
                    }
                    else
                    {
                        // The file is does not contain the correct root element. Delete it
                        EventLogger.LogMessage("Invalid Instance file " + fname + " found and Deleted.", TraceLevel.Warning);
                        File.Delete(fname);
                    }
                }
            }
            return list;
        }

        private string getFilenameOfInstance(string instanceName)
        {
            foreach (string fname in Directory.GetFiles(settingsDirectory))
            {
                if (fname.Contains(instanceFilenamePrefix))
                {
                    XDocument x = XDocument.Load(fname);
                    if(x.Elements(rootElementName_Instance).Any(i =>i.Attribute("Name").Value == instanceName))
                    {
                        return fname;
                    }
                }
            }
            EventLogger.LogMessage("getFilenameOfInstance did not find the specified instance named " + instanceName, TraceLevel.Error);
            throw new ApplicationException("Instance with that name was not found");
        }

        private string getInstanceNameFromFile(string file)
        {
            try
            {
                XDocument x = XDocument.Load(file);
                return x.Elements(rootElementName_Instance).Attributes().Single(a => a.Name == "Name").Value;
            }
            catch
            {
                return "";
            }
        }

        public string DefaultInstance
        {
            get
            {
                // No need to check if it exists becuase we will always check this in the constructor.
                return GetGlobalSettingFromXml("DefaultInstance", productName);
            }
            set
            {
                SetGlobalSettingsInXml("DefaultInstance", value);
            }
        }

        protected void SetGlobalSettingsInXml(string settingName, string newValue)
        {
            // Refresh the global document
            GlobalXDoc = XDocument.Load(GlobalSettingsFile);
            GlobalSettingsElement.SetElementValue(settingName, newValue);
            GlobalXDoc.Save(GlobalSettingsFile);
        }

        public string CurrentInstance
        {
            get
            {
                return currentInstanceName;
            }
            set
            {
                if (value == currentInstanceName) return;
                else
                {
                    if(getListOfInstances().Contains(value))
                    {
                        // Verify that the instance is not in use
                        if (CheckIfInstanceIsInUse(value))
                        {
                            throw new ApplicationException("The requested instance is already in use.");
                        }
                        else
                        {
                            // Set the old instance as unused
                            //if (currentInstanceName != string.Empty)
                            //{
                            //    SetPropertyInXml(ProcessInfoString, UnusedProcessString);
                            //}
                            // Set the new instance as current

                            currentInstanceName = value;
                            currentInstanceFilename = getFilenameOfInstance(value);
                            CurrentInstXDoc = XDocument.Load(currentInstanceFilename);


                            //SetPropertyInXml(ProcessInfoString, Process.GetCurrentProcess().Id.ToString() + "@" +
                            //    Process.GetCurrentProcess().StartTime.TimeOfDay.ToString());

                            // Change the default instance to remember the last selected current instance
                            
                        }
                        if(!value.Contains("Auto-Created"))
                            DefaultInstance = value;
                    }
                    else throw new System.InvalidOperationException("The requested instance does not exist");
                }

               
            }
        }

        private XElement CurrentInstanceElement
        {
            get
            {
                return CurrentInstXDoc.Element(rootElementName_Instance);
            }
        }

        protected string GetGlobalSettingFromXml(string settingName, string defaultValue)
        {
            // Refresh the global settings
            GlobalXDoc = XDocument.Load(GlobalSettingsFile);
            // Check if the property exists
            if (GlobalSettingsElement.Elements(settingName).Any())
            {
                return GlobalSettingsElement.Elements(settingName).Single().Value;
            }
            else
            {
                GlobalSettingsElement.SetElementValue(settingName, defaultValue);
                GlobalXDoc.Save(GlobalSettingsFile);
                return defaultValue;
            }
        }

        private string getPropertyFromSpecificInstance(string instanceName, string propertyName, string defaultValue)
        {
            string fname = string.Empty;
            XDocument x = new XDocument();
            fname = getFilenameOfInstance(instanceName);
            x = XDocument.Load(fname);
            XElement props = x.Elements(rootElementName_Instance).Single();
            if (props.Elements().Any(p => p.Name == propertyName))
            {
                // The property exists, return the value found.
                return props.Elements(propertyName).Single().Value;
            }
            else
            {
                // Create the property and set to default
                props.SetElementValue(propertyName, defaultValue);
                x.Save(fname);
                return defaultValue;
            }
        }

        protected void SetPropertyInXml(string propertyName, string newValue)
        {
            CurrentInstanceElement.SetElementValue(propertyName, newValue);
            CurrentInstXDoc.Save(currentInstanceFilename);
        }

        protected string GetPropertyFromXml(string propertyName, string defaultValue)
        {
            try
            {
                EventLogger.LogMessage("Attempting to get xml property " + propertyName, TraceLevel.Verbose);
                // Reload the settings document 

                // If the property doesn't exist, create it
                if (!CurrentInstanceElement.Elements(propertyName).Any())
                    SetPropertyInXml(propertyName, defaultValue);

                return CurrentInstanceElement.Element(propertyName).Value;
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                return defaultValue;
            }
        }

        private bool CheckIfInstanceIsInUse(string instanceName)
        {
            // This method checks the running process to see if the owner is still alive.
            try
            {
                string OwningProcessInfo = getPropertyFromSpecificInstance(instanceName, ProcessInfoString, UnusedProcessString);
                if (OwningProcessInfo == UnusedProcessString) return false;
                else
                {
                    try
                    {
                        int i = OwningProcessInfo.IndexOf('@');
                        if (i > 1)
                        {
                            int procID = int.Parse(OwningProcessInfo.Substring(0, i));
                            Process proc = Process.GetProcessById(procID);
                            if (proc.Id.ToString() + "@" + proc.StartTime.TimeOfDay.ToString() == OwningProcessInfo)
                            {
                                return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                Trace.WriteLine("Exception Thrown while getting Processes");
                return false;
                //throw; 
            }
        }

        public List<string> SettingsInstanceNames
        {
            get { return getListOfInstances(); }
        }

        public void RenameInstance(string oldName, string newName)
        {

            // Make sure the instance exists
            if (!getListOfInstances().Contains(oldName))
            {
                throw new ApplicationException("An instance with the requested name could not be found");
            }

            // Check who's using it
            if (CheckIfInstanceIsInUse(oldName) && (CurrentInstance != oldName))
            {
                // Another program is using this instance
                throw new ApplicationException("Another program (or process) is using the selected instance. " +
                    "You must close out of the other program to release the instance before changing it's name.");
            }

            // Change the name
            string tempFname = getFilenameOfInstance(oldName);
            XDocument x = XDocument.Load(tempFname);
            x.Element(rootElementName_Instance).SetAttributeValue("Name", newName);


            // Change the default instance if necessary
            if (oldName == DefaultInstance)
                DefaultInstance = newName;

            // Rename the current instance if necessary
            if (oldName == currentInstanceName)
            {
                currentInstanceName = newName;
                CurrentInstXDoc = x;
            }

            // Save the changes
            x.Save(tempFname);

        }

        public void CreateNewInstance(string instanceName)
        {
            EventLogger.LogMessage("New xml settings instance requested with name: "
                + instanceName.ToString(), TraceLevel.Verbose);
 
            // Check if there are any instances yet and find a unique name
            if (getListOfInstances().Contains(instanceName))
                throw new ApplicationException("An instance with the name " + instanceName + " already exists!");

            // The name is unique so create the instance

            XDeclaration dec = new XDeclaration("1.0", "UTF-8", "yes");
            XElement rootElement = new XElement(rootElementName_Instance);
            rootElement.SetAttributeValue("Name", instanceName);
            XDocument newInstanceDoc = new XDocument(dec, rootElement);
            string newInstFilename = settingsDirectory + instanceFilenamePrefix + getNewInstanceFilenameSuffix();
            newInstanceDoc.Save(newInstFilename);

            EventLogger.LogMessage("New XML Settings Instance added with name: " +
                instanceName.ToString(), TraceLevel.Info);
        }

        public void DeleteSettingsInstance(string instanceName)
        {
            if (getListOfInstances().Count <= 1)
            {
                throw new ApplicationException("You can not delete this settings instance because it is the only one. You may use the Edit option to rename the instance if you wish.");
            }
            if (!getListOfInstances().Contains(instanceName))
                throw new ApplicationException("The requested instance does not exist");
            if (CheckIfInstanceIsInUse(instanceName))
                throw new ApplicationException("The requested instance can not be deleted because it is in use. " +
                    "You must close out of the program that is using this instance or switch to a different instance in order to delete this one.");

            // It is safe to Delete the instance now.
            EventLogger.LogMessage("Deleting Instance named " + instanceName + " from xml settings file", TraceLevel.Warning);
            
            File.Delete(getFilenameOfInstance(instanceName));

            // Change the default instance if necessary
            if (DefaultInstance == instanceName)
            {
                DefaultInstance = getListOfInstances()[0];
            }

        }

        protected void SetGroupProperty(string groupName, string groupItem, string itemProperty, string newValue)
        {
            EventLogger.LogMessage("Setting group " + groupName +
                " property " + itemProperty + " to " + newValue, TraceLevel.Verbose);
            try
            {
                // Check if the Group Exists
                XElement grpElement;
                if (!CurrentInstanceElement.Elements("PropertyGroup").Any(g => g.Attribute("Name").Value == groupName))
                {
                    grpElement = new XElement("PropertyGroup");
                    grpElement.SetAttributeValue("Name", groupName);
                    CurrentInstanceElement.Add(grpElement);
                }
                else
                    grpElement = CurrentInstanceElement.Elements("PropertyGroup").Single(g => g.Attribute("Name").Value == groupName);


                // Check if the Group Item exists
                XElement itmElement;
                if (!grpElement.Elements("GroupItem").Any(i => i.Attribute("Designator").Value == groupItem))
                {
                    itmElement = new XElement("GroupItem");
                    itmElement.SetAttributeValue("Designator", groupItem);
                    grpElement.Add(itmElement);
                }
                else itmElement = grpElement.Elements("GroupItem").Single(i => i.Attribute("Designator").Value == groupItem);

                // Check if the Item Property exists
                XElement propElement;
                if (!itmElement.Elements("ItemProperty").Any(ip => ip.Attribute("Name").Value == itemProperty))
                {
                    propElement = new XElement("ItemProperty");
                    propElement.SetAttributeValue("Name", itemProperty);
                    itmElement.Add(propElement);
                }
                else propElement = itmElement.Elements("ItemProperty").Single(p => p.Attribute("Name").Value == itemProperty);

                propElement.SetValue(newValue);

                // Save the settings file
                CurrentInstXDoc.Save(currentInstanceFilename);

                //EventLogger.LogMessage(propertyName + " property set to " + newValue + " in xml settings file", TraceLevel.Info);
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
            }
        }

        protected List<string> GetPropertyGroupItemList(string groupName)
        {
            List<string> list = new List<string> { };
            if (!CurrentInstanceElement.Elements("PropertyGroup").Any(g => g.Attribute("Name").Value == groupName))
                return list;
            else
            {
                foreach (XElement g in CurrentInstanceElement.Element("PropertyGroup").Elements())
                {
                    list.Add(g.Attribute("Designator").Value);
                }
            }

            return list;
        }

        protected List<string> GetGroupItemPropertyNameList(string groupName, string groupItem)
        {
            List<string> list = new List<string> { };

            if (!CurrentInstanceElement.Elements("PropertyGroup").
                Single(i => i.Attribute("Name").Value == groupName).
                Elements("GroupItem").Any(j => j.Attribute("Designator").Value == groupItem))
                return list;
            else
            {
                foreach (XElement e in CurrentInstanceElement.Elements("PropertyGroup").
                Single(i => i.Attribute("Name").Value == groupName).
                Elements("GroupItem").Single(j => j.Attribute("Designator").Value == groupItem).Elements())
                {
                    list.Add(e.Attribute("Name").Value);
                }
            }
            return list;
        }

        protected string GetGroupItemPropertyValue(string groupName, string groupItem, string itemProperty)
        {
            string value = "";
            if (!CurrentInstanceElement
                .Elements("PropertyGroup").Single(a => a.Attribute("Name").Value == groupName)
                .Elements("GroupItem").Single(b => b.Attribute("Designator").Value == groupItem)
                .Elements("ItemProperty").Any(c => c.Attribute("Name").Value == itemProperty))
                return value;
            else
            {
                return CurrentInstanceElement
                .Elements("PropertyGroup").Single(a => a.Attribute("Name").Value == groupName)
                .Elements("GroupItem").Single(b => b.Attribute("Designator").Value == groupItem)
                .Elements("ItemProperty").Single(c => c.Attribute("Name").Value == itemProperty).Value;
            }
        }

        public void MarkInstanceInUse(bool inUse)
        {
            if (inUse)
            {
                SetPropertyInXml(ProcessInfoString, Process.GetCurrentProcess().Id.ToString() + "@" +
                                Process.GetCurrentProcess().StartTime.TimeOfDay.ToString());
            }
            else
            {
                SetPropertyInXml(ProcessInfoString, UnusedProcessString);
            }
        }

    }

}
