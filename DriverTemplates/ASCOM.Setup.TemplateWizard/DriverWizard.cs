

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

	public class DriverWizard : IWizard
	{
		private DeviceDriverForm inputForm;
        //private const string csAscom = "ASCOM.";
        //private const string csDeviceIdFormat = "ASCOM.{0}.{1}";

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

		// This method is called before opening any item that 
		// has the OpenInEditor attribute.
		public void BeforeOpeningFile(ProjectItem projectItem)
		{
			Diagnostics.Enter();
			Diagnostics.Exit();
		}

		public void ProjectFinishedGenerating(Project project)
		{
			Diagnostics.Enter();
			// Iterate through the project items and 
			// remove any files that begin with the word "Placeholder".
            // and the Rates class unless it's the Telescope class
            // done this way to avoid removing items from inside a foreach loop
            List<string> rems = new List<string>();
            foreach (ProjectItem item in project.ProjectItems)
            {
                if (item.Name.StartsWith("Placeholder", StringComparison.OrdinalIgnoreCase) ||
                    item.Name.StartsWith("Rate", StringComparison.OrdinalIgnoreCase) &&
                    !this.DeviceClass.Equals("Telescope", StringComparison.OrdinalIgnoreCase))
                {
                    //MessageBox.Show("adding " + item.Name);
                    rems.Add(item.Name);
                }
            }
            foreach (string item in rems)
            {
                //MessageBox.Show("Deleting " + item);
                project.ProjectItems.Item(item).Delete();
            }
            // removed because it messes up the project name
            //project.Name = String.Format(csDeviceIdFormat, DeviceId, DeviceClass);
			Diagnostics.Exit();
		}

		// This method is only called for item templates,
		// not for project templates.
		public void ProjectItemFinishedGenerating(ProjectItem projectItem)
		{
			Diagnostics.Enter();
			Diagnostics.Exit();
		}

		// This method is called after the project is created.
		public void RunFinished()
		{
			Diagnostics.Enter();
			// Remove all device-specific files that do not match the created DeviceClass.
            //List<FileInfo> deviceSpecificFiles = GetDeviceSpecificFiles();
            //foreach (var file in deviceSpecificFiles)
            //{
            //    // ToDo: implement this!
            //    throw new NotImplementedException();
            //}
			Diagnostics.Exit();
		}

		/// <summary>
		/// Gets a list of the device specific files in the current directory.
		/// </summary>
		/// <returns></returns>
        //private List<FileInfo> GetDeviceSpecificFiles()
        //{
        //    string[] allFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
        //    List<FileInfo> deviceSpecificFiles = new List<FileInfo>(allFiles.Length);
        //    foreach (var file in allFiles)
        //    {
        //        try
        //        {
        //            var fileInfo = new FileInfo(file);
        //            if (fileInfo.IsDeviceSpecific())
        //                deviceSpecificFiles.Add(fileInfo);
        //        }
        //        catch (Exception ex)
        //        {
        //            Diagnostics.TraceError(ex);
        //        }
        //    }
        //    return deviceSpecificFiles;
        //}

		public void RunStarted(object automationObject,
			Dictionary<string, string> replacementsDictionary,
			WizardRunKind runKind, object[] customParams)
		{
			Diagnostics.Enter();
			try
			{
				// Display a form to the user. The form collects 
				// input for the custom message.
				inputForm = new DeviceDriverForm();
				inputForm.ShowDialog();

				// Save user inputs.
				DeviceId = inputForm.DeviceId;
				DeviceName = inputForm.DeviceName;
				DeviceClass = inputForm.DeviceClass;
                DeviceInterface = inputForm.DeviceInterface;
                InterfaceVersion = inputForm.InterfaceVersion;
				Namespace = inputForm.Namespace;
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
				replacementsDictionary.Add("TEMPLATEDEVICECLASS", DeviceClass);
				replacementsDictionary.Add("ITEMPLATEDEVICEINTERFACE", DeviceInterface);
				replacementsDictionary.Add("TEMPLATENAMESPACE", Namespace);
				replacementsDictionary.Add("TEMPLATEINTERFACEVERSION", InterfaceVersion);
                // create and replace guids
				replacementsDictionary.Add(csTemplateAssemblyGuid, Guid.NewGuid().ToString());
				replacementsDictionary.Add(csTemplateInterfaceGuid, Guid.NewGuid().ToString());
				replacementsDictionary.Add(csTemplateRateGuid, Guid.NewGuid().ToString());
				replacementsDictionary.Add(csTemplateAxisRatesGuid, Guid.NewGuid().ToString());
				replacementsDictionary.Add(csTemplateTrackingRatesGuid, Guid.NewGuid().ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			Diagnostics.Exit();
		}

		// This method is only called for item templates,
		// not for project templates.
		public bool ShouldAddProjectItem(string filePath)
		{
			return true;
		}

#if DEBUG
		void DumpCodeModel(Project project)
		{
			MessageBox.Show("Attach to process now", "Debug");
			ProjectItem item = project.ProjectItems.Item("Driver.cs");
			FileCodeModel fcm = item.FileCodeModel;
			CodeClass codeClass = (CodeClass2)fcm.CodeElements.Item(DeviceClass);
			// Implement the I<DeviceClass> interface on the driver class. Does not insert method stubs.
			codeClass.AddImplementedInterface("ASCOM.Interface.I" + DeviceClass, 0);
		}
#endif


    }
}