// <summary>Template Wizard to perform unique ASCOM subsititutions and file manipulations when creating driver templates</summary>

namespace ASCOM.Setup
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TemplateWizard;
    using System.Windows.Forms;
    using EnvDTE;
    using EnvDTE80;
    using System.IO;

    //using ASCOM.Internal;
    public class VideoUsingBaseClassWizard : IWizard
    {
        private DeviceDriverForm inputForm;

        // These Guids are placeholder Guids. Wherever they are used in the template project, they'll be replaced with new
        // values when the template is expanded. THE TEMPLATE PROJECTS MUST USE THESE GUIDS.
        private const string csTemplateAssemblyGuid = "28D679BA-2AF1-4557-AE15-C528C5BF91E0";
        private const string csTemplateInterfaceGuid = "3A02C211-FA08-4747-B0BD-4B00EB159297";
        private const string csTemplateRateGuid = "AD6248B3-3F51-4FFF-B62B-E3E942DD817E";
        private const string csTemplateAxisRatesGuid = "99DB28A6-0132-43BF-91C0-D723124813C8";
        private const string csTemplateTrackingRatesGuid = "49A4CA43-46B2-4D66-B9D3-FBE3ABE13DEB";

        // Private properties
        private string DeviceClass { get; set; }
        private string DeviceName { get; set; }
        private string DeviceInterface { get; set; }
        private string DeviceId { get; set; }
        private string InterfaceVersion { get; set; }
        private string Namespace { get; set; }

        private ASCOM.Utilities.TraceLogger TL;
        private DTE2 myDTE;

        /// <summary>
        /// Runs custom wizard logic at the beginning of a template wizard run.
        /// </summary>
        /// <param name="automationObject"></param>
        /// <param name="replacementsDictionary"></param>
        /// <param name="runKind"></param>
        /// <param name="customParams"></param>
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            Diagnostics.Enter();

            DialogResult dialogResult = DialogResult.Cancel;

            try
            {
                // Create the trace logger
                TL = new ASCOM.Utilities.TraceLogger("", "VideoUsingBaseClassWizard");
                TL.Enabled = true;

                myDTE = (DTE2)automationObject;

                // Display a form to the user. The form collects input for the custom message.
                inputForm = new DeviceDriverForm(TL); // Pass our trace logger into the form so all Wizard trace goes into one file
                dialogResult = inputForm.ShowDialog();
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("RunStarted", "Exception 1: " + ex.ToString());
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

                // Log the parameters returned by the form
                TL.LogMessage("RunStarted", "DeviceId: " + DeviceId);
                TL.LogMessage("RunStarted", "DeviceName: " + DeviceName);
                TL.LogMessage("RunStarted", "DeviceClass: " + DeviceClass);
                TL.LogMessage("RunStarted", "DeviceInterface: " + DeviceInterface);
                TL.LogMessage("RunStarted", "InterfaceVersion: " + InterfaceVersion);
                TL.LogMessage("RunStarted", "Namespace: " + Namespace);
                TL.BlankLine();

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
                replacementsDictionary.Add("TEMPLATEDEVICECLASS", "Video"); // This ensures that the class is named Video and not VideoWithBaseClass
                replacementsDictionary.Add("ITEMPLATEDEVICEINTERFACE", DeviceInterface);
                replacementsDictionary.Add("TEMPLATENAMESPACE", Namespace);
                replacementsDictionary.Add("TEMPLATEINTERFACEVERSION", InterfaceVersion);

                // Log the replacement parameter values
                TL.LogMessage("RunStarted", "$deviceid$: " + DeviceId);
                TL.LogMessage("RunStarted", "$deviceclass$: " + DeviceClass);
                TL.LogMessage("RunStarted", "$devicename$: " + DeviceName);
                TL.LogMessage("RunStarted", "$namespace$: " + Namespace);
                TL.LogMessage("RunStarted", "$projectname$: " + DeviceId);
                TL.LogMessage("RunStarted", "$safeprojectname$: " + DeviceId);
                TL.LogMessage("RunStarted", "TEMPLATEDEVICENAME: " + DeviceName);
                TL.LogMessage("RunStarted", "TEMPLATEDEVICECLASS: " + "Video");
                TL.LogMessage("RunStarted", "ITEMPLATEDEVICEINTERFACE: " + DeviceInterface);
                TL.LogMessage("RunStarted", "TEMPLATENAMESPACE: " + Namespace);
                TL.LogMessage("RunStarted", "TEMPLATEINTERFACEVERSION: " + InterfaceVersion);
                TL.BlankLine();

                // create and replace guids
                replacementsDictionary.Add(csTemplateAssemblyGuid, Guid.NewGuid().ToString());
                replacementsDictionary.Add(csTemplateInterfaceGuid, Guid.NewGuid().ToString());
                replacementsDictionary.Add(csTemplateRateGuid, Guid.NewGuid().ToString());
                replacementsDictionary.Add(csTemplateAxisRatesGuid, Guid.NewGuid().ToString());
                replacementsDictionary.Add(csTemplateTrackingRatesGuid, Guid.NewGuid().ToString());
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("RunStarted", "Exception 2: " + ex.ToString());
                MessageBox.Show("Form result setup exception: " + ex.ToString());
            }

            Diagnostics.Exit();
        }

        /// <summary>
        /// Indicates whether the specified project item should be added to the project.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <remarks>This method is only called for item templates, not for project templates.</remarks>
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        /// <summary>
        /// Runs custom wizard logic when a project item has finished generating.
        /// </summary>
        /// <param name="projectItem"></param>
        /// <remarks>This method is only called for item templates, not for project templates.</remarks>
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            Diagnostics.Enter();
            Diagnostics.Exit();
        }

        /// <summary>
        /// Runs custom wizard logic before opening an item in the template.
        /// </summary>
        /// <param name="projectItem"></param>
        /// <remarks>This method is called before opening any item that has the OpenInEditor attribute.</remarks>
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            Diagnostics.Enter();
            Diagnostics.Exit();
        }

        /// <summary>
        /// Runs custom wizard logic when a project has finished generating.
        /// </summary>
        /// <param name="project"></param>
        public void ProjectFinishedGenerating(Project project)
        {
            Diagnostics.Enter();
            Diagnostics.Exit();
        }

        /// <summary>
        /// Runs custom wizard logic when the wizard has completed all tasks.
        /// </summary>
        public void RunFinished()
        {
            Diagnostics.Enter();
            Diagnostics.Exit();
        }

    }
}