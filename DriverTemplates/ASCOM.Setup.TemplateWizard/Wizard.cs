using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;

namespace ASCOM.Setup
{
	public class Wizard : IWizard
	{
		private UserInputForm inputForm;
		private const string csAscom = "ASCOM.";
		private const string csDeviceIdFormat = "{0}.{1}.{2}";
		private string DeviceClass { get; set; }
		private string DeviceName { get; set; }
		private string DeviceId { get; set; }
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
			//DumpCodeModel(project);

			//if (projectItem.Name == "Driver.cs")
			//{
			//    Diagnostics.TraceInfo("Detected Driver.cs");
			//    // Handle C# interface implementation
			//}
			//else if (projectItem.Name == "Driver.vb")
			//{
			//    Diagnostics.TraceInfo("Detected Driver.vb");
			//    // Handle VB interface implementation
			//}
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
			Diagnostics.Exit();
		}

		public void RunStarted(object automationObject,
			Dictionary<string, string> replacementsDictionary,
			WizardRunKind runKind, object[] customParams)
		{
			Diagnostics.Enter();
			try
			{
				// Display a form to the user. The form collects 
				// input for the custom message.
				inputForm = new UserInputForm();
				inputForm.ShowDialog();

				// Save user inputs.
				DeviceId = inputForm.DeviceId;
				DeviceName = inputForm.DeviceName;
				DeviceClass = inputForm.DeviceClass;
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
				replacementsDictionary.Add("TEMPLATEDRIVERGUID", Guid.NewGuid().ToString());
				replacementsDictionary.Add("GUIDSUBST2", Guid.NewGuid().ToString());
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