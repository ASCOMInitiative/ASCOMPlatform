using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.Setup
{
	public class TestObject
	{
		internal Wizard testWiz;

		private TestObject()
		{
			testWiz = new Wizard();
		}

		public void RunStarted()
		{
			testWiz.RunStarted(new object(), new Dictionary<string, string>(), Microsoft.VisualStudio.TemplateWizard.WizardRunKind.AsNewProject, new object[0]);
		}
	}
}
