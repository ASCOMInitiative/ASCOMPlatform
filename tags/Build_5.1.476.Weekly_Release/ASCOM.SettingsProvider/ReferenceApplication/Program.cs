using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ASCOM.SettingsProvider.ReferenceApplication
	{
	static class Program
		{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
			{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			TestForm frm = new TestForm();
			Application.Run(frm);
			}
		}
	}
