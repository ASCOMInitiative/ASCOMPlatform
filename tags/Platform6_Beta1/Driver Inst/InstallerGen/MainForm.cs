using System;
using System.Collections;
//using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace ASCOM.InstallerGen
{
	public partial class MainForm : Form
	{

		public MainForm()
		{
			InitializeComponent();
			lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			cbDriverType2.Text = "(none)";
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void pbAscomLogo_Click(object sender, EventArgs e)
		{
			OpenWebPage("http://ascom-standards.org/");
		}

		private void llblInno_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			OpenWebPage("http://www.jrsoftware.org/isinfo.php");
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			MakeIss();																	// Below
			this.Dispose();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.Dispose();
		}

		private void cmdBrowseSourceFolder_Click(object sender, EventArgs e)
		{
			fldrBrowse.ShowNewFolderButton = false;
			fldrBrowse.Description = "Select folder containing driver component(s) and files...";
			if (DialogResult.OK != fldrBrowse.ShowDialog())
				return;
			this.txtSourceFolder.Text = fldrBrowse.SelectedPath;
			//UpdateUI();
		}

		private void cmdBrowseDriverFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "In-Proc Drivers (*.dll)|*.dll|Local Servers (*.exe)|*.exe|All Files (*.*)|*.*"; // Filter files by extension
			if (cbDriverTechnology.Text.ToLower().Contains("local server"))
			{
				dlg.DefaultExt = ".exe";
				dlg.FilterIndex = 2;
			}
			else
			{
				dlg.DefaultExt = ".dll";
				dlg.FilterIndex = 1;
			}
			if (this.txtSourceFolder.Text != "")
			{
				dlg.InitialDirectory = this.txtSourceFolder.Text;
			}
			else
			{
				MessageBox.Show(this, "Select source directory first.", "Driver File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			dlg.Title = "Select the driver DLL or EXE...";
			if (DialogResult.OK != dlg.ShowDialog(this))
				return;
			this.txtDriverFile.Text = Path.GetFileName(dlg.FileName);
			//UpdateUI();
		}

		private void cmdBrowseReadMe_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "HTML files (*.html *.htm)|*.html;*.htm|Text files (*.txt)|*.txt"; // Filter files by extension
			dlg.DefaultExt = ".html";
			dlg.FilterIndex = 1;
			if (this.txtSourceFolder.Text != "")
			{
				dlg.InitialDirectory = this.txtSourceFolder.Text;
			}
			else
			{
				MessageBox.Show(this, "Select source directory first.", "ReadMe File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			dlg.Title = "Select the driver's read-me file...";
			if (DialogResult.OK != dlg.ShowDialog(this))
				return;
			this.txtReadMeFile.Text = Path.GetFileName(dlg.FileName);
			//UpdateUI();
		}

		private void txtDriverName_TextChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void txtDrvrShortName_TextChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void cbDriverType_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void txtDriverVersion_TextChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void txtSourceFolder_TextChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void txtDriverFile_TextChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void txtReadMeFile_TextChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void txtDeveloperName_TextChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void txtDeveloperEmail_TextChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void cbDriverTechnology_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void UpdateUI()
		{
			bool bOK = (this.txtSourceFolder.Text != "");
			this.cmdBrowseDriverFile.Enabled = bOK;
			this.cmdBrowseReadMe.Enabled = bOK;
			if (this.cbDriverTechnology.Text.StartsWith(".NET"))
			{
				this.txtDriverName.Text = "(set by .NET driver's ASCOM reg. info)";
				this.txtDriverName.Enabled = false;
			}
			else
			{
				if (this.txtDriverName.Text == "(set by .NET driver's ASCOM reg. info)")
					this.txtDriverName.Text = "";
				this.txtDriverName.Enabled = true;
			}
			this.cbDriverType2.Enabled = this.cbDriverTechnology.Text.StartsWith("Local server");
			if (this.txtDeveloperEmail.Text != "" && this.txtDeveloperName.Text != "" &&
					this.txtDriverFile.Text != "" && this.txtDriverName.Text != "" &&
					this.txtDriverVersion.Text != "" && this.txtDrvrShortName.Text != "" &&
					this.txtSourceFolder.Text != "" && this.cbDriverTechnology.Text != "" &&
					this.cbDriverType.Text != "" && this.txtReadMeFile.Text != "")
				this.cmdSave.Enabled = true;
			else
				this.cmdSave.Enabled = false;
		}

		//
		// Here's where the dirty work is done. 
		//
		private void MakeIss()
		{
			string classId = "{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}";						// Default primary CLSID for driver not reg'd on developer's system
			string classId2 = "{yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyyyyyyy}";						// Default secondary CLSID for driver not reg'd on developer's system
			if (this.cbDriverTechnology.Text.StartsWith("Local server"))					// COM Local Server only, for AppId stuff
			{
				string progId = this.txtDrvrShortName.Text + "." + this.cbDriverType.Text;
				Type drvrType = Type.GetTypeFromProgID(progId);
				if (drvrType != null)
					classId = Type.GetTypeFromProgID(progId).GUID.ToString("B");
				else
					MessageBox.Show(this, "Unable to determine the " + this.cbDriverType.Text + 
							" CLSID of your driver. You must edit the generated template where indicated and insert the " + 
							this.cbDriverType.Text + " CLSID of your driver",
							"Driver Primary CLSID", MessageBoxButtons.OK, MessageBoxIcon.Information);
				if (this.cbDriverType2.Text != "(none)")									// If have secondary interface, get its CLSID too
				{
					progId = this.txtDrvrShortName.Text + "." + this.cbDriverType2.Text;
					drvrType = Type.GetTypeFromProgID(progId);
					if (drvrType != null)
						classId2 = Type.GetTypeFromProgID(progId).GUID.ToString("B");
					else
						MessageBox.Show(this, "Unable to determine the " + this.cbDriverType2.Text + 
								" CLSID of your driver. You must edit the generated template where indicated and insert the " +
								this.cbDriverType2.Text + " CLSID of your driver",
								"Driver Secondary CLSID", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string rsrcPath = appPath + "\\Resources";
			StreamReader fsTmpl = new StreamReader(rsrcPath + "\\DriverInstallTemplate.iss");
			string tBuf = fsTmpl.ReadToEnd();											// Suck it all in
			fsTmpl.Close();
			tBuf = Regex.Replace(tBuf, ";;.*\r\n", "");									// Get rid of template comments
			tBuf = tBuf.Replace("%fnam%", this.txtDriverName.Text);
			tBuf = tBuf.Replace("%name%", this.txtDrvrShortName.Text);
			tBuf = tBuf.Replace("%type%", this.cbDriverType.Text);
			tBuf = tBuf.Replace("%typ2%", this.cbDriverType2.Text);
			tBuf = tBuf.Replace("%vers%", this.txtDriverVersion.Text);
			tBuf = tBuf.Replace("%file%", this.txtDriverFile.Text);
			tBuf = tBuf.Replace("%rdmf%", this.txtReadMeFile.Text);
			tBuf = tBuf.Replace("%devn%", this.txtDeveloperName.Text);
			tBuf = tBuf.Replace("%deve%", this.txtDeveloperEmail.Text);
			tBuf = tBuf.Replace("%date%", DateTime.UtcNow.ToShortDateString());
			tBuf = tBuf.Replace("%gver%", Assembly.GetExecutingAssembly().GetName().Version.ToString());
			tBuf = tBuf.Replace("%rgsm%", "{%FrameworkDir|{win}\\Microsoft.NET\\Framework}\\V2.0.50727\\regasm.exe");
			tBuf = tBuf.Replace("%srcp%", this.txtSourceFolder.Text);
			tBuf = tBuf.Replace("%rscf%", rsrcPath);
			tBuf = tBuf.Replace("%cid1%", classId);
			tBuf = tBuf.Replace("%cid2%", classId2);
			tBuf = tBuf.Replace("%rs32%", (this.cbDriverTechnology.Text.StartsWith("In-process COM") ? ";AfterInstall: RegASCOM(); Flags: regserver" : ";AfterInstall: RegASCOM()"));
			// DEPENDS ON txtTechnologyTypes! Cheesy, but this is a quickie anyway
			tBuf = CondLine(tBuf, "coms", this.cbDriverTechnology.Text.Contains("COM"));
			tBuf = CondLine(tBuf, "cdll", this.cbDriverTechnology.Text.StartsWith("In-process"));
			tBuf = CondLine(tBuf, "cexe", this.cbDriverTechnology.Text.StartsWith("Local server"));
			tBuf = CondLine(tBuf, "cex2", this.cbDriverTechnology.Text.StartsWith("Local server") && this.cbDriverType2.Text != "(none)");
			tBuf = CondLine(tBuf, "nbth", this.cbDriverTechnology.Text.StartsWith(".NET"));
			tBuf = CondLine(tBuf, "nasm", this.cbDriverTechnology.Text.StartsWith(".NET assembly"));
			tBuf = CondLine(tBuf, "nlcs", this.cbDriverTechnology.Text.StartsWith(".NET local server"));
			tBuf = CondLine(tBuf, "srce", this.checkBox1.Checked);	// Rename-refactor failed miserably!
			string scrptPath = this.txtSourceFolder.Text + "\\" + this.txtDrvrShortName.Text + " Setup.iss";
			StreamWriter strmScript = new StreamWriter(scrptPath);
			strmScript.Write(tBuf);
			strmScript.Close();

			string testPath = this.txtSourceFolder.Text + "\\" + this.txtDrvrShortName.Text + " Test.js";
			StreamWriter testScript = new StreamWriter(testPath);
			testScript.WriteLine("//*** CHECK THIS ProgID ***");
			string buf = "var X = new ActiveXObject(\"";
			if(this.cbDriverTechnology.Text.StartsWith(".NET"))
				buf += "ASCOM.";
			buf += this.txtDrvrShortName.Text + "." + this.cbDriverType.Text;
			buf += "\");";
			testScript.WriteLine(buf);
			testScript.WriteLine("WScript.Echo(\"This is \" + X.Name + \")\");");
			testScript.WriteLine("// You may want to uncomment this...");
			testScript.WriteLine("// X.Connected = true;");
			testScript.WriteLine("X.SetupDialog();");
			testScript.Close();

			System.Diagnostics.Process.Start(scrptPath);
		}

		private string CondLine(string sBuf, string sToken, bool bCond)
		{
			return Regex.Replace(sBuf, "\\%" + sToken + "\\%(.*)\r\n", bCond ? "$1\r\n" : "");
		}

		private static void OpenWebPage(string uri)
		{
			try
			{
				ProcessStartInfo psi = new ProcessStartInfo();
				psi.UseShellExecute = true;
				psi.FileName = uri;
				Process.Start(psi);
			}
			catch (Exception) { }
		}
	}
}