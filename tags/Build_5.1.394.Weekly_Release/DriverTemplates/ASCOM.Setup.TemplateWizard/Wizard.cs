using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using EnvDTE;

namespace ASCOM.Setup
{
	public class Wizard : IWizard
	{
		private UserInputForm inputForm;

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

				// Add custom parameters.
				replacementsDictionary.Add("$deviceid$", inputForm.DeviceId);
				replacementsDictionary.Add("$deviceclass$", inputForm.DeviceClass);
				replacementsDictionary.Add("$devicename$", inputForm.DeviceName);
				replacementsDictionary.Add("$namespace$", inputForm.Namespace);
				replacementsDictionary["$projectname$"] = inputForm.DeviceId;
				replacementsDictionary["$safeprojectname$"] = inputForm.DeviceId;
				inputForm.Dispose();
				inputForm = null;
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
	}
}