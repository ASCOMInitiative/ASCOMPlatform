using System;
using System.IO;
using System.Security.Permissions;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ASCOM.PlatformInstaller
{
	[RunInstaller(true)]
	public partial class CustomActions : Installer
	{
		[DllImport("ole32.dll")]
		static extern int CLSIDFromProgID(
				[MarshalAs(UnmanagedType.LPWStr)]string lpszProgID,
				out Guid pclsid);

		private Type tProfile;												// Late bound Helper.Profile
		private object oProfile;
		private const string sMsgTitle = "ASCOM Platform 2008";

		//
		// Add AppId stuff for the given ProgID
		// http://go.microsoft.com/fwlink/?linkid=32831
		//
		private void AddAppID(string progID, string exeName, string sAPPID)
		{
			Guid gCLSID;
			int hr = CLSIDFromProgID(progID, out gCLSID);
			string sCLSID = "{" + new GuidConverter().ConvertToString(gCLSID) + "}";

			RegistryKey rkCLSID = null;
			RegistryKey rkAPPID = null;
			RegistryKey rkAPPIDExe = null;
			try
			{
				rkCLSID = Registry.ClassesRoot.OpenSubKey(
								"CLSID\\" + sCLSID,
								RegistryKeyPermissionCheck.ReadWriteSubTree);
				rkCLSID.SetValue("AppId", sAPPID);
				rkAPPID = Registry.ClassesRoot.CreateSubKey(
								"APPID\\" + sAPPID,
								RegistryKeyPermissionCheck.ReadWriteSubTree);
				rkAPPID.SetValue("", rkCLSID.GetValue(""));				// Same description as class
				rkAPPID.SetValue("AppId", sAPPID);
				rkAPPID.SetValue("AuthenticationLevel", 3, RegistryValueKind.DWord);    // Changed to RPC_C_AUTHN_LEVEL_USER (3) from RPC_C_AUTHN_LEVEL_NONE (1) becuase of issue caused by Microsoft security update

                if (exeName != "")										// If want AppId\Exe.name
				{
					rkAPPIDExe = Registry.ClassesRoot.CreateSubKey(
									"AppId\\" + exeName,
									RegistryKeyPermissionCheck.ReadWriteSubTree);
					rkAPPIDExe.SetValue("", rkCLSID.GetValue(""));		// Same description as class
					rkAPPIDExe.SetValue("AppId", sAPPID);
				}
			}
			catch (Exception ex) 
			{
				MessageBox.Show("Failed to add AppID info for " + progID + ": " + ex.Message,
					sMsgTitle,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
			}
			finally
			{
				if (rkCLSID != null) rkCLSID.Close();
				if (rkAPPID != null) rkAPPID.Close();
				if (rkAPPIDExe != null) rkAPPIDExe.Close();
			}
		}

		//
		// Remove AppId stuff for the given APPID
		// http://go.microsoft.com/fwlink/?linkid=32831
		//
		private void RemAppID(string exeName, string sAPPID)
		{
			if(exeName == "" || sAPPID == "") return;						// Ohmygod exit

			RegistryKey rkAPPID = null;
			try
			{
				rkAPPID = Registry.ClassesRoot.OpenSubKey(
								"APPID",
								RegistryKeyPermissionCheck.ReadWriteSubTree);
				rkAPPID.DeleteSubKey(sAPPID);
				rkAPPID.DeleteSubKey(exeName);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to remove AppID info for " + exeName + ": " + ex.Message,
					sMsgTitle,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
			}
			finally
			{
				if (rkAPPID != null) rkAPPID.Close();
			}
		}

		//
		// Do COM (un)registration of EXE simulators here so they can be worked
		// on while the platform is installed. Letting MSI do this causes it
		// to lock out any work on the simulators!
		//
		private void RegUnregExe(string ExePath, bool Register)
		{
			Process proc = null;
			try
			{
				proc = Process.Start(ExePath, (Register ? "/" : "/un") + "regserver");	// Works for .NET and VB6 localservers
				proc.WaitForExit();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to " + (Register ? "" : "un") + "register " + Path.GetFileName(ExePath) + ": " + ex.Message,
					sMsgTitle,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
			}
			finally { if (proc != null) proc.Close(); }
		}

		//
		// Do COM (un)registration of DLL simulators here so they can be worked
		// on while the platform is installed. Letting MSI do this causes it
		// to lock out any work on the simulators!
		//
		private void RegUnregDll(string DllPath, bool Register)
		{
			Process proc = null;
			try
			{
				proc = Process.Start("regsvr32.exe", (Register ? "-s " : "-u -s ") + "\"" + DllPath + "\"");
				proc.WaitForExit();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to " + (Register ? "" : "un") + "register " + Path.GetFileName(DllPath) + ": " + ex.Message,
					sMsgTitle,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
			}
			finally { if (proc != null) proc.Close(); }
		}

		//
		// ASCOM Register/Unregister - Use Helper.Profile so that we're
		// protected against moving from registry to XML or ???
		//
		private void RegAscom(string ProgID, string Desc)
		{
			string sType = ProgID.Substring(ProgID.IndexOf('.') + 1);

			try
			{
				tProfile.InvokeMember("DeviceType",
					BindingFlags.Default | BindingFlags.SetProperty,
					null, oProfile, new object[] { sType });

				tProfile.InvokeMember("Register",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, oProfile, new object[] { ProgID, Desc });
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to register " + ProgID + "  for ASCOM: " + ex.Message,
					sMsgTitle,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
			}
		}

		private void UnregAscom(string ProgID)
		{
			string sType = ProgID.Substring(ProgID.IndexOf('.') + 1);

			try
			{
				tProfile.InvokeMember("DeviceType",
					BindingFlags.Default | BindingFlags.SetProperty,
					null, oProfile, new object[] { sType });

			tProfile.InvokeMember("Unregister",
				BindingFlags.Default | BindingFlags.InvokeMethod,
				null, oProfile, new object[] { ProgID });
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to unregister " + ProgID + "  for ASCOM: " + ex.Message,
					sMsgTitle,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
			}
		}

		private void FixPiaLinkage(string LibID, string AssyClass)
		{
			RegistryKey rkTLIB = null;
			try
			{
				rkTLIB = Registry.ClassesRoot.OpenSubKey(
								"TypeLib\\" + LibID + "\\1.0",
								RegistryKeyPermissionCheck.ReadWriteSubTree);
				rkTLIB.SetValue("PrimaryInteropAssemblyName",
								AssyClass + ", Version=1.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to fix up TypeLib for " + AssyClass + ": " + ex.Message,
					sMsgTitle,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
			}
			finally
			{
				if (rkTLIB != null) rkTLIB.Close();
			}
		}

		//
		// Constructor
		//
		public CustomActions()
		{
			InitializeComponent();
			//
			// It is amazing that this works on a clean system!
			// The custom action must be the last thing to run
			// during a commit and the first one to be run during
			// uninstall????? I don't think so because the TypeLibs
			// aren't registered during the time this runs for commit!
			//
            tProfile = Type.GetTypeFromProgID("DriverHelper.Profile");					// Late bound Helper.Profile
            oProfile = Activator.CreateInstance(tProfile);
		}

		//
		// Destructor
		//
		~CustomActions()
		{
			try {
				Marshal.ReleaseComObject(oProfile);										// May fail (if managed Helper)
			} catch(Exception) { }
			oProfile = null;
		}

		//
		// NOTE: In order to avoid an MSI error (Cannot find xxx.InstallState) we must
		// arrange for this CustomAction assembly to be invoked for the Install phase
		// as well as the Commit phase. 
		//

		//
		// Installer entry point for Install phase. Warn upgraders.
		//
		[SecurityPermission(SecurityAction.Demand)]
		public override void Install(IDictionary savedState)
		{
			base.Install(savedState);

			//Debugger.Break();
			//MessageBox.Show(
			//    "Install()",
			//    "Test",
			//    MessageBoxButtons.OK,
			//    MessageBoxIcon.Information);

			try
			{
				string ver = this.Context.Parameters["platformversion"];
				if (ver != "" && (Convert.ToInt32(ver.Substring(0, 1)) < 5))	// Be careful of '.' vs ',' and just test first digit!
					MessageBox.Show(
						"You have an earlier ASCOM Platform installed. Your existing " +
						"drivers will be preserved during the upgrade.",
						sMsgTitle,
						MessageBoxButtons.OK,
						MessageBoxIcon.Information);
			}
			catch (Exception) { }
		}

		//
		// Installer entry point for Commit phase
		//
		[SecurityPermission(SecurityAction.Demand)]
		public override void Commit(IDictionary savedState)
		{
			base.Commit(savedState);

			//Debugger.Break();
			//MessageBox.Show(
			//    "Commit()",
			//    "Test",
			//    MessageBoxButtons.OK,
			//    MessageBoxIcon.Information);

			//
			// Force TypeLib linkages to PIAs (bug on repair & upgrades).
			//
			FixPiaLinkage("{12DC26BD-E8F7-4328-8A8B-B16C6D87A077}", "ASCOM.Helper");
			FixPiaLinkage("{55B16037-26C0-4F1E-A6DC-4C0E702D9FBE}", "ASCOM.Helper2");
			FixPiaLinkage("{76618F90-032F-4424-A680-802467A55742}", "ASCOM.Interfaces");

			//
			// Remove Platform 4.1 installer entry in Add/Remove Programs (TESTED)
			//
			RegistryKey rkAddRem = null;
			try
			{
				rkAddRem = Registry.LocalMachine.OpenSubKey(
							"Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall",
							RegistryKeyPermissionCheck.ReadWriteSubTree);

				rkAddRem.DeleteSubKeyTree("ASCOM Platform 4.1");
				rkAddRem.DeleteSubKeyTree("ASCOM Platform 4.0");
			}
			catch (Exception) { }
			finally { if (rkAddRem != null) 	rkAddRem.Close(); }

			//
			// Attempt to remove some old Platform Start Menu items
			// No special folder for all users start menu?
			//
			string startPath = "C:\\Documents and Settings\\All Users\\Start Menu\\Programs\\ASCOM Platform";
			try
			{
				File.Delete(startPath + "\\Other Drivers (web).url");
				File.Delete(startPath + "\\RoboFocus Control.lnk");
			}
			catch (Exception) { }
			startPath = Environment.SpecialFolder.Programs + "\\ASCOM Platform";	// Try individual user too
			try
			{
				File.Delete(startPath + "\\Other Drivers (web).url");
				File.Delete(startPath + "\\RoboFocus Control.lnk");
			}
			catch (Exception) { }

			//
			// Set up some paths...
			//
			string ascomPath =  Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\ASCOM";
			//string xmlProfPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM";

			//
			// Remove old Wise uninstaller and install log. It's weird, but this
			// stuff was installed in the Telescope subfolder. Always in program
			// files, common files, ASCOM\Telescope. 
			try
			{
				File.Delete(ascomPath + "\\Telescope\\INSTALL.LOG");
				File.Delete(ascomPath + "\\Telescope\\UNWISE.EXE");
			}
			catch (Exception) { }
			//
			// Register all of the simulators here, keeping MSI's dirty mits off them 
			// so they can be worked on and replaced with uupdates, etc.
			//
			// MIRROR CALLS IN UNINSTALL!
			//
			RegUnregExe(ascomPath +	"\\Dome\\DomeSim.exe",						true);
			RegUnregExe(ascomPath +	"\\Dome\\ASCOMDome.exe",					true);
			RegUnregExe(ascomPath +	"\\Focuser\\FocusSim.exe",					true);
			RegUnregExe(ascomPath +	"\\Telescope\\ScopeSim.exe",				true);
			RegUnregDll(ascomPath + "\\Camera\\CCDSimulator.dll",               true);
			RegUnregExe(ascomPath +	"\\Telescope\\POTH.exe",					true);
			RegUnregExe(ascomPath +	"\\Telescope\\Pipe.exe",					true);
			RegUnregExe(ascomPath +	"\\Telescope\\Hub.exe",						true);
			RegUnregExe(ascomPath +	"\\Rotator\\ASCOM.RotatorSimulator.exe",	true);
			RegUnregExe(ascomPath +	"\\Switch\\SwitchSim.exe",					true);

			//
			// Add AppID stuff for EXE simulators Must be in commit because
			// these things must be registered now! Don't bother for Switch.
			//
			// MIRROR CALLS IN UNINSTALL!
			//
			AddAppID("ScopeSim.Telescope",		"ScopeSim.exe",					"{4597A685-11FD-47ae-A5D2-A8DC54C90CDC}");
			AddAppID("FocusSim.Focuser",		"FocusSim.exe",					"{289BFF60-3B9E-417c-8DA1-F96773679342}");
			AddAppID("DomeSim.Dome",			"DomeSim.exe",					"{C47DAA29-5788-464e-BBA7-9711FBC7A01F}");
			AddAppID("ASCOM.Simulator.Rotator",	"ASCOM.RotatorSimulator.exe",	"{DF2EB077-4D59-4231-9CB4-C61AD4ECB874}");
			AddAppID("POTH.Telescope",			"POTH.exe",						"{0A21726F-E58B-4461-85A9-9AA739E6E42F}");
			AddAppID("POTH.Dome",				"",								"{0A21726F-E58B-4461-85A9-9AA739E6E42F}");
			AddAppID("POTH.Focuser",			"",								"{0A21726F-E58B-4461-85A9-9AA739E6E42F}");
			AddAppID("Pipe.Telescope",			"Pipe.exe",						"{080B23A0-3190-45e5-A7C8-53C61F22DFF2}");
			AddAppID("Pipe.Focuser",			"",								"{080B23A0-3190-45e5-A7C8-53C61F22DFF2}");
			AddAppID("Pipe.Dome",				"",								"{080B23A0-3190-45e5-A7C8-53C61F22DFF2}");
			AddAppID("Hub.Telescope",           "Hub.exe",                      "{3213D452-2A8F-4624-9D65-E04E175E4CB2}");
			AddAppID("Hub.Focuser",             "",                             "{3213D452-2A8F-4624-9D65-E04E175E4CB2}");
			AddAppID("Hub.Dome",                "",                             "{3213D452-2A8F-4624-9D65-E04E175E4CB2}");
			AddAppID("ASCOMDome.Telescope",     "ASCOMDome.exe",                "{B5863239-0A6E-48d4-A9EA-0DDA4D942390}");
			AddAppID("ASCOMDome.Dome",			"",								"{B5863239-0A6E-48d4-A9EA-0DDA4D942390}");

			//
			// Register VB6 simulators for ASCOM. I set these friendly names.
			//
			RegAscom("ScopeSim.Telescope",		"Simulator");
			RegAscom("FocusSim.Focuser",		"Simulator");
			RegAscom("DomeSim.Dome",			"Simulator");
			RegAscom("CCDSimulator.Camera",     "Simulator");
			RegAscom("POTH.Telescope",			"POTH Hub");
			RegAscom("POTH.Dome",				"POTH Hub");
			RegAscom("POTH.Focuser",			"POTH Hub");
			RegAscom("Pipe.Telescope",			"Pipe diagnostic tool");
			RegAscom("Pipe.Focuser",			"Pipe diagnostic tool");
			RegAscom("Pipe.Dome",				"Pipe diagnostic tool");
			RegAscom("Hub.Telescope",			"Generic Hub");
			RegAscom("Hub.Focuser",				"Generic Hub");
			RegAscom("Hub.Dome",				"Generic Hub");
			RegAscom("ASCOMDome.Telescope",		"ASCOM Dome Control");
			RegAscom("ASCOMDome.Dome",			"ASCOM Dome Control");

			//
			// If the templates were requested, run the Visual Studio Content Installer
			// In the CustomActionData we have /templates=[VSTEMPLATES] where VSTEMPLATES
			// is a Checkbox (A) property. The Checked variable ends up here as "" or "1".
			//
			//Process proc = null;
			if (this.Context.Parameters["templates"] == "1")
			{
				//
				// This now advises to use the new link "Install Templates".
				// It's marked "not vital"
				//
				MessageBox.Show(
					"You requested installation of the VS 2005 driver templates. " +
					"Before trying to use them, please review \"Before Using the " +
					"Templates!\" in your Windows Start menu, ASCOM Platform, Doc.",
					"ASCOM Platform Setup",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);
				//
				// VSCI Fails with the NoImpersonate option that is set by the 
				// Post-Build event script, which is needed for Vista!!
				//
				//try
				//{
				//    proc = Process.Start(ascomPath + "\\.net\\ASCOMDriverTemplates.vsi");
				//    if(proc != null)
				//        proc.WaitForExit();
				//}
				//catch (Exception) { }
				//finally { if (proc != null) proc.Close(); }
			}

			//
			// Pop up the ReadMe document in the default browser
			//
			try
			{
				Process.Start(ascomPath + "\\Doc\\readme.html");
			}
			catch (Exception) { }

		}

		//
		// Installer entry point for Uninstall phase - Call the Rotator sim to unregister it
		// then remove all of the AppId stuff. The other sims & tools are self-unregistering.
		//
		// MIRROR CALLS IN COMMIT
		//
		[SecurityPermission(SecurityAction.Demand)]
		public override void Uninstall(IDictionary savedState)
		{
			base.Uninstall(savedState);

			//Debugger.Break();
			//MessageBox.Show(
			//    "Uninstall()",
			//    "Test",
			//    MessageBoxButtons.OK,
			//    MessageBoxIcon.Information);

			string ascomPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\ASCOM";

			RemAppID("ScopeSim.exe",				"{4597A685-11FD-47ae-A5D2-A8DC54C90CDC}");
			RemAppID("FocusSim.exe",				"{289BFF60-3B9E-417c-8DA1-F96773679342}");
			RemAppID("DomeSim.exe",					"{C47DAA29-5788-464e-BBA7-9711FBC7A01F}");
			RemAppID("POTH.exe",					"{0A21726F-E58B-4461-85A9-9AA739E6E42F}");
			RemAppID("Pipe.exe",					"{080B23A0-3190-45e5-A7C8-53C61F22DFF2}");
			RemAppID("Hub.exe",                     "{3213D452-2A8F-4624-9D65-E04E175E4CB2}");
			RemAppID("ASCOMDome.exe",               "{B5863239-0A6E-48d4-A9EA-0DDA4D942390}");
			RemAppID("ASCOM.RotatorSimulator.exe",	"{DF2EB077-4D59-4231-9CB4-C61AD4ECB874}");

			//
			// Unregister VB6 simulators for ASCOM
			//
			UnregAscom("ScopeSim.Telescope");
			UnregAscom("FocusSim.Focuser");
			UnregAscom("DomeSim.Dome");
			UnregAscom("CCDSimulator.Camera");
			UnregAscom("POTH.Telescope");
			UnregAscom("POTH.Dome");
			UnregAscom("POTH.Focuser");
			UnregAscom("Pipe.Telescope");
			UnregAscom("Pipe.Focuser");
			UnregAscom("Pipe.Dome");
			UnregAscom("Hub.Telescope");
			UnregAscom("Hub.Focuser");
			UnregAscom("Hub.Dome");
			UnregAscom("ASCOMDome.Telescope");
			UnregAscom("ASCOMDome.Dome");

			//
			// Unregister all of the simulators here. They had to be manually registered
			// in Commit() in order to be able to work on them while the Platform is
			// installed.
			//
			RegUnregExe(ascomPath +	"\\Dome\\DomeSim.exe",						false);
			RegUnregExe(ascomPath +	"\\Dome\\ASCOMDome.exe",					false);
			RegUnregExe(ascomPath +	"\\Focuser\\FocusSim.exe",					false);
			RegUnregExe(ascomPath +	"\\Telescope\\ScopeSim.exe",				false);
			RegUnregDll(ascomPath + "\\Camera\\CCDSimulator.dll",               false);
			RegUnregExe(ascomPath + "\\Telescope\\POTH.exe", false);
			RegUnregExe(ascomPath +	"\\Telescope\\Pipe.exe",					false);
			RegUnregExe(ascomPath +	"\\Telescope\\Hub.exe",						false);
			RegUnregExe(ascomPath +	"\\Rotator\\ASCOM.RotatorSimulator.exe",	false);
			RegUnregExe(ascomPath +	"\\Switch\\SwitchSim.exe",					false);

			//
			// We need to positively delete the PlatformVersion value. Why doesn't the 
			// F***ING installer do this?????
			//
			RegistryKey rkAscom = null;
			try
			{
				rkAscom = Registry.LocalMachine.OpenSubKey(
							"Software\\ASCOM",
							RegistryKeyPermissionCheck.ReadWriteSubTree);
				rkAscom.DeleteValue("PlatformVersion");
			}
			catch (Exception) { }
			finally { if (rkAscom != null) 	rkAscom.Close(); }
		}
	}
}