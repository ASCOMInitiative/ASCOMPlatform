using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using System.IO;
using System.Diagnostics;

namespace ASCOM.Setup
{
    public class LocalServerWizard : IWizard
    {
        #region Variables and constants

        private LocalServerForm inputForm;

        const string INTERFACE_METHODS_INSERTION_POINT = "//INTERFACECODEINSERTIONPOINT"; // Find the insertion point in the Driver.xx item.
        const string START_OF_COMMANDXXX_METHODS = "//STARTOFCOMMANDXXXMETHODS"; // Start of the CommandXXX method definitions.
        const string END_OF_COMMANDXXX_METHODS = "//ENDOFCOMMANDXXXMETHODS"; // End of the CommandXXX definitions.
        const string END_OF_INSERTED_FILE = "//ENDOFINSERTEDFILE";

        // These GUIDs are placeholder GUIDs. Wherever they are used in the template project, they'll be replaced with new values when the template is expanded. THE TEMPLATE PROJECTS MUST USE THESE GUIDS.
        private const string csTemplateAssemblyGuid = "28D679BA-2AF1-4557-AE15-C528C5BF91E0";
        private const string csTemplateInterfaceGuid = "3A02C211-FA08-4747-B0BD-4B00EB159297";
        private const string csTemplateRateGuid = "AD6248B3-3F51-4FFF-B62B-E3E942DD817E";
        private const string csTemplateAxisRatesGuid = "99DB28A6-0132-43BF-91C0-D723124813C8";
        private const string csTemplateTrackingRatesGuid = "49A4CA43-46B2-4D66-B9D3-FBE3ABE13DEB";

        // Private properties
        private string DeviceClass { get; set; }
        private string DeviceName { get; set; }
        private string DeviceId { get; set; }
        private string Namespace { get; set; }
        private string DeviceInterface { get; set; }
        private int InterfaceVersion { get; set; }

        private ASCOM.Utilities.TraceLogger TL = new ASCOM.Utilities.TraceLogger("TemplateWizardLS");

        private ProjectItem driverFile; // Pointer to the Driver.VB or Driver.CS file.
        private ProjectItem hardwareFile; // Pointer to the Hardware.CS file.

        private TextSelection documentSelection;

        private int deleteCount;

        #endregion

        /// <summary>
        /// Runs custom wizard logic at the beginning of a template wizard run.
        /// </summary>
        /// <param name="automationObject"></param>
        /// <param name="replacementsDictionary"></param>
        /// <param name="runKind"></param>
        /// <param name="customParams"></param>
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            TL.LogMessage("RunStarted", $"Started wizard");

            DialogResult dialogResult = DialogResult.Cancel;
            try
            {
                // Display a form to the user. The form collects 
                // input for the custom message.
                inputForm = new LocalServerForm(TL); // Pass our trace logger into the form so all Wizard trace goes into one file
                dialogResult = inputForm.ShowDialog();
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("RunStarted", "Form Exception: " + ex.ToString());
                MessageBox.Show("Form Exception: " + ex.ToString());
            }

            if (dialogResult != DialogResult.OK) throw new WizardCancelledException("The wizard has been cancelled");

            try
            {
                // Save user inputs.
                DeviceId = inputForm.DeviceId;
                DeviceName = inputForm.DeviceName;
                DeviceClass = inputForm.DeviceClass;
                DeviceInterface = inputForm.DeviceInterface;
                InterfaceVersion = inputForm.InterfaceVersion;
                Namespace = inputForm.Namespace;
                TL.Enabled = true;
                TL.LogMessage("DeviceId", DeviceId);
                TL.LogMessage("DeviceName", DeviceName);
                TL.LogMessage("DeviceClass", DeviceClass);
                TL.LogMessage("DeviceInterface", DeviceInterface);
                TL.LogMessage("InterfaceVersion", InterfaceVersion.ToString());
                TL.LogMessage("Namespace", Namespace);

                inputForm.Dispose();
                inputForm = null;

                // Add custom parameters.
                replacementsDictionary.Add("$deviceid$", DeviceId);
                replacementsDictionary.Add("$deviceclass$", DeviceClass);
                replacementsDictionary.Add("$devicename$", DeviceName);
                replacementsDictionary.Add("$namespace$", Namespace);
                replacementsDictionary["$projectname$"] = DeviceId;
                replacementsDictionary["$safeprojectname$"] = DeviceId;
                replacementsDictionary.Add("TEMPLATEDEVICENAME", DeviceName);
                replacementsDictionary.Add("TEMPLATEDEVICEID", DeviceId);
                if (DeviceClass == "VideoUsingBaseClass") // Special handling for "VideoWithBaseClass" template because its file name is not the same as the device type "Video"
                {
                    replacementsDictionary.Add("TEMPLATEDEVICECLASS", "Video"); // This ensures that the class is named Video and not VideoWithBaseClass
                }
                else // ALl other templates process normally because the selected device name exactly matches the device type e.g. Telescope, Rotator etc.
                {
                    replacementsDictionary.Add("TEMPLATEDEVICECLASS", DeviceClass);
                }
                replacementsDictionary.Add("TEMPLATEHARDWARECLASS", $"{DeviceClass}Hardware"); // Hardware class name
                replacementsDictionary.Add("ITEMPLATEDEVICEINTERFACE", DeviceInterface);
                replacementsDictionary.Add("TEMPLATENAMESPACE", Namespace);
                replacementsDictionary.Add("TEMPLATEINTERFACEVERSION", InterfaceVersion.ToString());

                // create and replace GUIDs
                replacementsDictionary.Add(csTemplateAssemblyGuid, Guid.NewGuid().ToString());
                replacementsDictionary.Add(csTemplateInterfaceGuid, Guid.NewGuid().ToString());
                replacementsDictionary.Add(csTemplateRateGuid, Guid.NewGuid().ToString());
                replacementsDictionary.Add(csTemplateAxisRatesGuid, Guid.NewGuid().ToString());
                replacementsDictionary.Add(csTemplateTrackingRatesGuid, Guid.NewGuid().ToString());
                TL.LogMessage("RunStarted", $"Completed");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("RunStarted", "Result Exception: " + ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Runs custom wizard logic when a project has finished generating.
        /// </summary>
        /// <param name="project"></param>
        public void ProjectFinishedGenerating(Project project)
        {
            TL.LogMessage("ProjectFinishedGenerating", $"Started");

            ProjectItems driversFolderItems = null;
            ProjectItem driverFolder = null;

            // All processing in this method applies to files that are held in the Drivers folder within the Local Server template project

            // Find the drivers folder and get its project object
            foreach (ProjectItem item in project.ProjectItems)
            {
                TL.LogMessage("FindDriverFolder", $"Item name: {item.Name}, type: {item.Kind}, file count: {item.FileCount}, item count: {item.ProjectItems.Count}");
                if (item.Name.StartsWith("Driver", StringComparison.OrdinalIgnoreCase))
                {
                    TL.LogMessage("FindDriverFolder", $"Found Driver folder item");
                    driversFolderItems = item.ProjectItems;
                    driverFolder = item;
                }
            }

            if (driversFolderItems is null) throw new Exception("Unable to find Drivers folder");

            // Iterate through the project items and remove any files that begin with the word "Placeholder" and the Rates class unless it's the Telescope class. Done this way to avoid removing items from inside a for-each loop
            List<string> rems = new List<string>();
            foreach (ProjectItem item in driversFolderItems)
            {
                TL.LogMessage("Placeholder And Rate", $"Item name: {item.Name}, type: {item.Kind}, file count: {item.FileCount}, item count: {item.ProjectItems.Count}");
                if (item.Name.StartsWith("Placeholder", StringComparison.OrdinalIgnoreCase) ||
                    (item.Name.StartsWith("Rate", StringComparison.OrdinalIgnoreCase) && !DeviceClass.Equals("Telescope", StringComparison.OrdinalIgnoreCase)))
                {
                    TL.LogMessage("Placeholder And Rate", $"Adding item {item.Name}");
                    rems.Add(item.Name);
                }
            }
            foreach (string item in rems)
            {
                TL.LogMessage("Placeholder And Rate", $"Deleting item {item}");
                driversFolderItems.Item(item).Delete();
            }

            // Special handling for VB and C# driver template projects to add the interface implementation to the core driver code
            try
            {
                // Check the name of each item in the project and execute if this is a driver template project (contains Driver.vb or Driver.cs)
                foreach (ProjectItem projectItem in driversFolderItems)
                {
                    TL.LogMessage("ProjectFinishedGenerating", "Item name: " + projectItem.Name);

                    // Process DRIVER files
                    if ((projectItem.Name.ToUpperInvariant() == "DRIVER.CS") | (projectItem.Name.ToUpperInvariant() == "DRIVER.VB"))
                    {
                        driverFile = projectItem; // Save the driver item
                        // This is a driver template
                        // Get the filename and directory of the Driver.xx file
                        string directory = Path.GetDirectoryName(projectItem.FileNames[1].ToString());
                        TL.LogMessage("ProjectFinishedGenerating", "Driver file name: " + projectItem.FileNames[1].ToString() + ", Directory: " + directory);
                        TL.LogMessage("ProjectFinishedGenerating", "Found " + projectItem.Name);

                        projectItem.Open(); // Open the item for editing
                        TL.LogMessage("ProjectFinishedGenerating", "Done Open");

                        Document itemDocument = projectItem.Document; // Get the open file's document object
                        TL.LogMessage("ProjectFinishedGenerating", "Created Document");

                        itemDocument.Activate(); // Make this the current document
                        TL.LogMessage("ProjectFinishedGenerating", "Activated Document");

                        //
                        // Remove the CommandXXX methods if this device does not support them
                        //
                        TL.LogMessage("ProjectFinishedGenerating", "Removing the CommandXXX methods if the device interface does not support them");

                        if (DeviceClass.ToUpperInvariant() == "VIDEO") // Special handling for the Video interface that does not have CommmandXXX methods
                        {
                            TL.LogMessage("ProjectFinishedGenerating", $"{DeviceClass} device - Removing the CommandXXX method markers and all CommandXXX members .");
                            documentSelection = (TextSelection)itemDocument.Selection; // Create a document selection
                            TL.LogMessage("ProjectFinishedGenerating", "Created a selection object for the start of CommandXXX methods marker.");

                            bool foundStartOfCommandXxx = documentSelection.FindText(START_OF_COMMANDXXX_METHODS, (int)vsFindOptions.vsFindOptionsMatchWholeWord);
                            TL.LogMessage("ProjectFinishedGenerating", $"Found {START_OF_COMMANDXXX_METHODS}: {foundStartOfCommandXxx}, Text: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");

                            documentSelection.SelectLine(); // Select the current line
                            TL.LogMessage("ProjectFinishedGenerating", $"Selected {START_OF_COMMANDXXX_METHODS} line: " + documentSelection.Text);

                            // Delete lines until we get to the end of CommandXXX methods marker
                            deleteCount = 0;
                            while ((!documentSelection.Text.ToUpperInvariant().Contains(END_OF_COMMANDXXX_METHODS)) & (deleteCount < 100))
                            {
                                TL.LogMessage("ProjectFinishedGenerating", $"Deleting line: {documentSelection.Text} at line {documentSelection.CurrentLine}, Line length: {documentSelection.Text.Length}");
                                documentSelection.Delete(); // Delete the current line
                                documentSelection.SelectLine(); // Select the new current line ready to test on the next loop 
                                TL.LogMessage("ProjectFinishedGenerating", $"Found end line: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}. Delete count: {deleteCount}");
                                deleteCount += 1;
                            }

                            TL.LogMessage("ProjectFinishedGenerating", $"Deleting end of CommandXXX marker: {documentSelection.Text} at line {documentSelection.CurrentLine}, Line length: {documentSelection.Text.Length}");
                            documentSelection.Delete(); // Delete the current line

                            TL.LogMessage("ProjectFinishedGenerating", $"CommandXXX members removed.");
                        }
                        else // Normal behaviour for all other interfaces that do have CommandXXX methods
                        {
                            TL.LogMessage("ProjectFinishedGenerating", $"{DeviceClass} device - Just removing the CommandXXX method markers. Retaining all CommandXXX members.");

                            documentSelection = (TextSelection)itemDocument.Selection; // Create a document selection
                            TL.LogMessage("ProjectFinishedGenerating", "Created a selection object for start of CommandXXX methods.");

                            bool foundStartOfCommandXxx = documentSelection.FindText(START_OF_COMMANDXXX_METHODS, (int)vsFindOptions.vsFindOptionsMatchWholeWord);
                            TL.LogMessage("ProjectFinishedGenerating", $"Found {START_OF_COMMANDXXX_METHODS}: {foundStartOfCommandXxx}, Text: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");

                            documentSelection.SelectLine(); // Select the current line
                            TL.LogMessage("ProjectFinishedGenerating", $"Selected {START_OF_COMMANDXXX_METHODS} line: " + documentSelection.Text);

                            TL.LogMessage("ProjectFinishedGenerating", $"Deleting line: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");
                            documentSelection.Delete(); // Delete the current line


                            documentSelection = (TextSelection)itemDocument.Selection; // Create a document selection
                            TL.LogMessage("ProjectFinishedGenerating", "Created a selection object for end of CommandXXX methods.");

                            bool foundEndOfCommandXxx = documentSelection.FindText(END_OF_COMMANDXXX_METHODS, (int)vsFindOptions.vsFindOptionsMatchWholeWord);
                            TL.LogMessage("ProjectFinishedGenerating", $"Found {END_OF_COMMANDXXX_METHODS}: {foundEndOfCommandXxx}, Text: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");

                            documentSelection.SelectLine(); // Select the current line
                            TL.LogMessage("ProjectFinishedGenerating", $"Selected {END_OF_COMMANDXXX_METHODS} line: " + documentSelection.Text);

                            TL.LogMessage("ProjectFinishedGenerating", $"Deleting line: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");
                            documentSelection.Delete(); // Delete the current line

                            TL.LogMessage("ProjectFinishedGenerating", $"CommandXXX markers removed.");
                        }

                        //
                        // Insert the device specific methods into the generic driver file
                        //
                        TL.LogMessage("ProjectFinishedGenerating", "Inserting the device specific methods into the generic driver ");

                        documentSelection = (TextSelection)itemDocument.Selection; // Create a document selection
                        TL.LogMessage("ProjectFinishedGenerating", "Created Selection object");

                        documentSelection.FindText(INTERFACE_METHODS_INSERTION_POINT, (int)vsFindOptions.vsFindOptionsMatchWholeWord);
                        TL.LogMessage("ProjectFinishedGenerating", $"Done {INTERFACE_METHODS_INSERTION_POINT} FindText: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");

                        // Create the name of the device interface file to be inserted
                        string insertFile = directory + "\\Device" + this.DeviceClass + Path.GetExtension(projectItem.Name);
                        TL.LogMessage("ProjectFinishedGenerating", "Opening file: " + insertFile);

                        documentSelection.InsertFromFile(insertFile); // Insert the required file at the current selection point
                        TL.LogMessage("ProjectFinishedGenerating", "Done InsertFromFile");

                        // Remove the top lines of the inserted file until we get to #Region
                        // These lines are only there to make the file error free in the template development project and are not required here
                        documentSelection.SelectLine(); // Select the current line
                        TL.LogMessage("ProjectFinishedGenerating", "Selected initial line: " + documentSelection.Text);
                        while (!documentSelection.Text.ToUpperInvariant().Contains("#REGION"))
                        {
                            TL.LogMessage("ProjectFinishedGenerating", $"Deleting start line: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");
                            documentSelection.Delete(); // Delete the current line
                            documentSelection.SelectLine(); // Select the new current line ready to test on the next loop 
                        }

                        // Find the end of inserted file marker that came from the inserted file
                        bool foundEndOfInsertedFile = documentSelection.FindText(END_OF_INSERTED_FILE, (int)vsFindOptions.vsFindOptionsMatchWholeWord);
                        TL.LogMessage("ProjectFinishedGenerating", $"Found {END_OF_INSERTED_FILE} {foundEndOfInsertedFile} Text: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}, Line length: {documentSelection.Text.Length}");

                        // Delete the end of inserted file marker line and any remaining lines from the inserted file
                        documentSelection.SelectLine();
                        TL.LogMessage("ProjectFinishedGenerating", $"Found initial end line: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}, Line length: {documentSelection.Text.Length}");

                        deleteCount = 0;
                        while ((!documentSelection.Text.ToUpperInvariant().Contains("#REGION")) & (deleteCount < 10))
                        {
                            TL.LogMessage("ProjectFinishedGenerating", $"Deleting line: {documentSelection.Text} at line {documentSelection.CurrentLine}, Line length: {documentSelection.Text.Length}");
                            documentSelection.Delete(); // Delete the current line
                            documentSelection.SelectLine(); // Select the new current line ready to test on the next loop 
                            TL.LogMessage("ProjectFinishedGenerating", $"Found end line: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}. Delete count: {deleteCount}");
                            deleteCount += 1;
                        }

                        // Reformat the document to make it look pretty
                        documentSelection.SelectAll();
                        TL.LogMessage("ProjectFinishedGenerating", "Done SelectAll");
                        documentSelection.SmartFormat();
                        TL.LogMessage("ProjectFinishedGenerating", "Done SmartFormat");

                        itemDocument.Save(); // Save the edited file ready for use!
                        TL.LogMessage("ProjectFinishedGenerating", "Done Save");
                        itemDocument.Close(vsSaveChanges.vsSaveChangesYes);
                        TL.LogMessage("ProjectFinishedGenerating", "Done Close");

                    }

                    // Process HARDWARE files
                    if (projectItem.Name.ToUpperInvariant() == "COMMONHARDWARE.CS")
                    {
                        hardwareFile = projectItem; // Save the driver item
                        // This is a driver template
                        // Get the filename and directory of the Driver.xx file
                        string directory = Path.GetDirectoryName(projectItem.FileNames[1].ToString());
                        TL.LogMessage("ProjectFinishedGenerating", "Hardware file name: " + projectItem.FileNames[1].ToString() + ", Directory: " + directory);
                        TL.LogMessage("ProjectFinishedGenerating", "Found " + projectItem.Name);

                        projectItem.Open(); // Open the item for editing
                        TL.LogMessage("ProjectFinishedGenerating", "Done Open");

                        Document itemDocument = projectItem.Document; // Get the open file's document object
                        TL.LogMessage("ProjectFinishedGenerating", "Created Document");

                        itemDocument.Activate(); // Make this the current document
                        TL.LogMessage("ProjectFinishedGenerating", "Activated Document");

                        //
                        // Remove the CommandXXX methods if this device does not support them
                        //
                        TL.LogMessage("ProjectFinishedGenerating", "Removing the CommandXXX methods if the device interface does not support them");

                        if (DeviceClass.ToUpperInvariant() == "VIDEO") // Special handling for the Video interface that does not have CommmandXXX methods
                        {
                            TL.LogMessage("ProjectFinishedGenerating", $"{DeviceClass} device - Removing the CommandXXX method markers and all CommandXXX members .");
                            documentSelection = (TextSelection)itemDocument.Selection; // Create a document selection
                            TL.LogMessage("ProjectFinishedGenerating", "Created a selection object for the start of CommandXXX methods marker.");

                            bool foundStartOfCommandXxx = documentSelection.FindText(START_OF_COMMANDXXX_METHODS, (int)vsFindOptions.vsFindOptionsMatchWholeWord);
                            TL.LogMessage("ProjectFinishedGenerating", $"Found {START_OF_COMMANDXXX_METHODS}: {foundStartOfCommandXxx}, Text: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");

                            documentSelection.SelectLine(); // Select the current line
                            TL.LogMessage("ProjectFinishedGenerating", $"Selected {START_OF_COMMANDXXX_METHODS} line: " + documentSelection.Text);

                            // Delete lines until we get to the end of CommandXXX methods marker
                            deleteCount = 0;
                            while ((!documentSelection.Text.ToUpperInvariant().Contains(END_OF_COMMANDXXX_METHODS)) & (deleteCount < 100))
                            {
                                TL.LogMessage("ProjectFinishedGenerating", $"Deleting line: {documentSelection.Text} at line {documentSelection.CurrentLine}, Line length: {documentSelection.Text.Length}");
                                documentSelection.Delete(); // Delete the current line
                                documentSelection.SelectLine(); // Select the new current line ready to test on the next loop 
                                TL.LogMessage("ProjectFinishedGenerating", $"Found end line: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}. Delete count: {deleteCount}");
                                deleteCount += 1;
                            }

                            TL.LogMessage("ProjectFinishedGenerating", $"Deleting end of CommandXXX marker: {documentSelection.Text} at line {documentSelection.CurrentLine}, Line length: {documentSelection.Text.Length}");
                            documentSelection.Delete(); // Delete the current line

                            TL.LogMessage("ProjectFinishedGenerating", $"CommandXXX members removed.");
                        }
                        else // Normal behaviour for all other interfaces that do have CommandXXX methods
                        {
                            TL.LogMessage("ProjectFinishedGenerating", $"{DeviceClass} device - Just removing the CommandXXX method markers. Retaining all CommandXXX members.");

                            documentSelection = (TextSelection)itemDocument.Selection; // Create a document selection
                            TL.LogMessage("ProjectFinishedGenerating", "Created a selection object for start of CommandXXX methods.");

                            bool foundStartOfCommandXxx = documentSelection.FindText(START_OF_COMMANDXXX_METHODS, (int)vsFindOptions.vsFindOptionsMatchWholeWord);
                            TL.LogMessage("ProjectFinishedGenerating", $"Found {START_OF_COMMANDXXX_METHODS}: {foundStartOfCommandXxx}, Text: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");

                            documentSelection.SelectLine(); // Select the current line
                            TL.LogMessage("ProjectFinishedGenerating", $"Selected {START_OF_COMMANDXXX_METHODS} line: " + documentSelection.Text);

                            TL.LogMessage("ProjectFinishedGenerating", $"Deleting line: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");
                            documentSelection.Delete(); // Delete the current line


                            documentSelection = (TextSelection)itemDocument.Selection; // Create a document selection
                            TL.LogMessage("ProjectFinishedGenerating", "Created a selection object for end of CommandXXX methods.");

                            bool foundEndOfCommandXxx = documentSelection.FindText(END_OF_COMMANDXXX_METHODS, (int)vsFindOptions.vsFindOptionsMatchWholeWord);
                            TL.LogMessage("ProjectFinishedGenerating", $"Found {END_OF_COMMANDXXX_METHODS}: {foundEndOfCommandXxx}, Text: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");

                            documentSelection.SelectLine(); // Select the current line
                            TL.LogMessage("ProjectFinishedGenerating", $"Selected {END_OF_COMMANDXXX_METHODS} line: " + documentSelection.Text);

                            TL.LogMessage("ProjectFinishedGenerating", $"Deleting line: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");
                            documentSelection.Delete(); // Delete the current line

                            TL.LogMessage("ProjectFinishedGenerating", $"CommandXXX markers removed.");
                        }

                        //
                        // Insert the device specific methods into the generic driver file
                        //
                        TL.LogMessage("ProjectFinishedGenerating", "Inserting the device specific methods into the generic driver ");

                        documentSelection = (TextSelection)itemDocument.Selection; // Create a document selection
                        TL.LogMessage("ProjectFinishedGenerating", "Created Selection object");

                        documentSelection.FindText(INTERFACE_METHODS_INSERTION_POINT, (int)vsFindOptions.vsFindOptionsMatchWholeWord);
                        TL.LogMessage("ProjectFinishedGenerating", $"Done {INTERFACE_METHODS_INSERTION_POINT} FindText: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");

                        // Create the name of the device interface file to be inserted
                        string insertFile = $"{directory}\\Hardware{DeviceClass}{Path.GetExtension(projectItem.Name)}";
                        TL.LogMessage("ProjectFinishedGenerating", "Opening file: " + insertFile);

                        documentSelection.InsertFromFile(insertFile); // Insert the required file at the current selection point
                        TL.LogMessage("ProjectFinishedGenerating", "Done InsertFromFile");

                        // Remove the top lines of the inserted file until we get to #Region
                        // These lines are only there to make the file error free in the template development project and are not required here
                        documentSelection.SelectLine(); // Select the current line
                        TL.LogMessage("ProjectFinishedGenerating", "Selected initial line: " + documentSelection.Text);
                        while (!documentSelection.Text.ToUpperInvariant().Contains("#REGION"))
                        {
                            TL.LogMessage("ProjectFinishedGenerating", $"Deleting start line: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}");
                            documentSelection.Delete(); // Delete the current line
                            documentSelection.SelectLine(); // Select the new current line ready to test on the next loop 
                        }

                        // Find the end of inserted file marker that came from the inserted file
                        bool foundEndOfInsertedFile = documentSelection.FindText(END_OF_INSERTED_FILE, (int)vsFindOptions.vsFindOptionsMatchWholeWord);
                        TL.LogMessage("ProjectFinishedGenerating", $"Found {END_OF_INSERTED_FILE} {foundEndOfInsertedFile} Text: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}, Line length: {documentSelection.Text.Length}");

                        // Delete the end of inserted file marker line and any remaining lines from the inserted file
                        documentSelection.SelectLine();
                        TL.LogMessage("ProjectFinishedGenerating", $"Found initial end line: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}, Line length: {documentSelection.Text.Length}");

                        deleteCount = 0;
                        while ((!documentSelection.Text.ToUpperInvariant().Contains("#REGION")) & (deleteCount < 10))
                        {
                            TL.LogMessage("ProjectFinishedGenerating", $"Deleting line: {documentSelection.Text} at line {documentSelection.CurrentLine}, Line length: {documentSelection.Text.Length}");
                            documentSelection.Delete(); // Delete the current line
                            documentSelection.SelectLine(); // Select the new current line ready to test on the next loop 
                            TL.LogMessage("ProjectFinishedGenerating", $"Found end line: '{documentSelection.Text}'. Line number: {documentSelection.CurrentLine}. Delete count: {deleteCount}");
                            deleteCount += 1;
                        }

                        // Reformat the document to make it look pretty
                        documentSelection.SelectAll();
                        TL.LogMessage("ProjectFinishedGenerating", "Done SelectAll");
                        documentSelection.SmartFormat();
                        TL.LogMessage("ProjectFinishedGenerating", "Done SmartFormat");

                        itemDocument.Save(); // Save the edited file ready for use!
                        TL.LogMessage("ProjectFinishedGenerating", "Done Save");
                        itemDocument.Close(vsSaveChanges.vsSaveChangesYes);
                        TL.LogMessage("ProjectFinishedGenerating", "Done Close");

                    }
                }
                TL.LogMessage("ProjectFinishedGenerating", "Completed processing project items");

                // Iterate through the project items and remove any files that begin with the words "Device" or "Hardware". 
                // These are the partial device implementations that are merged in to create a complete device driver template by the code above
                // They are not required in the final project
                // Done this way to avoid removing items from inside a for-each loop

                TL.LogMessage("ProjectFinishedGenerating", $"Identifying files to delete");

                rems = new List<string>();
                foreach (ProjectItem item in driversFolderItems)
                {
                    if ((item.Name.StartsWith("Device", StringComparison.OrdinalIgnoreCase)) | (item.Name.StartsWith("Hardware", StringComparison.OrdinalIgnoreCase)))
                    {
                        TL.LogMessage("RemoveList", "adding " + item.Name);
                        rems.Add(item.Name);
                    }
                }

                foreach (string item in rems)
                {
                    TL.LogMessage("ProjectFinishedGenerating", "Deleting file: " + item);
                    driversFolderItems.Item(item).Delete();
                }

                // Rename the Driver file
                TL.LogMessage("ProjectFinishedGenerating", $"About to rename driver file - driver template exists: {!(driverFile is null)}.");
                TL.LogMessage("ProjectFinishedGenerating", $"Renaming driver: '{driverFile.Name}' to '{DeviceClass}{driverFile.Name}'");
                driverFile.Name = $"{DeviceClass}{driverFile.Name}";
                TL.LogMessage("ProjectFinishedGenerating", $"New driver name: '{driverFile.Name}'");

                // Rename the Hardware file
                TL.LogMessage("ProjectFinishedGenerating", $"About to rename hardware file - driver template exists: {!(hardwareFile is null)}.");
                TL.LogMessage("ProjectFinishedGenerating", $"Renaming hardware: '{hardwareFile.Name}' to '{DeviceClass}Hardware.{Path.GetExtension(hardwareFile.Name)}'");
                hardwareFile.Name = $"{DeviceClass}Hardware.{Path.GetExtension(hardwareFile.Name)}";
                TL.LogMessage("ProjectFinishedGenerating", $"New hardware name: '{hardwareFile.Name}'");

                // Rename the Driver folder
                TL.LogMessage("ProjectFinishedGenerating", $"Renaming driver folder: '{driverFolder.Name}' to '{DeviceClass}Driver'");
                driverFolder.DTE.SuppressUI = true;
                TL.LogMessage("ProjectFinishedGenerating", $"Set DTE.SuppressUI = true");
                driverFolder.Name = $"{DeviceClass}Driver";
                TL.LogMessage("ProjectFinishedGenerating", $"Completed rename");
                driverFolder.DTE.SuppressUI = false;
                TL.LogMessage("ProjectFinishedGenerating", $"New driver folder name: '{driverFolder.Name}'");

            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("ProjectFinishedGenerating Exception", ex.ToString()); // Log any error message
                MessageBox.Show(ex.ToString(), "ProjectFinishedGenerating Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show an error message
            }

            TL.LogMessage("ProjectFinishedGenerating", "End");

        }

        /// <summary>
        /// Runs custom wizard logic when the wizard has completed all tasks.
        /// </summary>
        public void RunFinished()
        {
            TL.LogMessage("RunFinished", $"Completed");
        }

        #region Unused interface members not applicable to Project templates

        /// <summary>
        /// Runs custom wizard logic before opening an item in the template. (Not used for Project templates.)
        /// </summary>
        /// <param name="projectItem"></param>
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            TL.LogMessage("BeforeOpeningFile", $"ProjectItem: {projectItem.Name}");
        }

        /// <summary>
        /// Runs custom wizard logic when a project item has finished generating. (Not used for Project templates.)
        /// </summary>
        /// <param name="projectItem"></param>
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            TL.LogMessage("ProjectItemFinishedGenerating", $"ProjectItem: {projectItem.Name}");
        }

        /// <summary>
        /// Indicates whether the specified project item should be added to the project. (Not used for project templates.)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool ShouldAddProjectItem(string filePath)
        {
            TL.LogMessage("ShouldAddProjectItem", $"File: {filePath}");
            return true;
        }

        #endregion

    }
}