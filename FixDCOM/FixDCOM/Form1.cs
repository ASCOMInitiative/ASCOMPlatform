using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Reflection;

namespace FixDCOM
{
    public partial class Form1 : Form
    {
        TraceLogger TL;
        int errorCount = 0;

        [DllImport("ole32.dll")]
        static extern int CLSIDFromProgID([MarshalAs(UnmanagedType.LPWStr)] string lpszProgID, out Guid pclsid);

        enum DCOMAuthenticationLevel
        {
            RPC_C_AUTHN_LEVEL_DEFAULT = 0, //Same as RPC_C_AUTHN_LEVEL_CONNECT
            RPC_C_AUTHN_LEVEL_NONE = 1, //No authentication.
            RPC_C_AUTHN_LEVEL_CONNECT = 2, //Authenticates the credentials of the client and server.
            RPC_C_AUTHN_LEVEL_CALL = 3, //Same as RPC_C_AUTHN_LEVEL_PKT.
            RPC_C_AUTHN_LEVEL_PKT = 4, //Same as RPC_C_AUTHN_LEVEL_CONNECT but also prevents replay attacks.
            RPC_C_AUTHN_LEVEL_PKT_INTEGRITY = 5, //Same as RPC_C_AUTHN_LEVEL_PKT but also verifies that none of the data transferred between the client and server has been modified.
            RPC_C_AUTHN_LEVEL_PKT_PRIVACY = 6 //Same as RPC_C_AUTHN_LEVEL_PKT_INTEGRITY but also ensures that the data transferred can only be seen unencrypted by the client and the server.
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TL = new TraceLogger("FixDCOM")
            {
                Enabled = true
            };
            cmbOptions.SelectedIndex = 0; // Preselect the Fix DCOM option rather than restor original option
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = string.Format("{0} Version {1}.{2}", this.Text, version.Major, version.MajorRevision);
            LogMessage("Form Load", "Application ready, please select an option from the dropdown list.");
        }

        private void LogMessage(string component, string message)
        {
            lblMessage.Text = string.Format("{0} - {1} - Error count: {2}", component, message, errorCount);
            TL.LogMessage(component, message);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            lblMessage.ForeColor = Color.Black;

            switch (cmbOptions.SelectedIndex)
            {
                case 0:
                    ApplyAppIds(DCOMAuthenticationLevel.RPC_C_AUTHN_LEVEL_CALL);
                    break;
                case 1:
                    ApplyAppIds(DCOMAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE);
                    break;
                default:
                    MessageBox.Show("Please select one of the options from the dropdown list before clicking Apply.");
                    break;
            }

            //errorCount = 1;
            if (errorCount == 0)
            {
                lblMessage.ForeColor = Color.Green;
                LogMessage("Apply", "Completed successfully!");
            }
            else
            {
                lblMessage.ForeColor = Color.Red;
                LogMessage("Error", @"An error occured, please check the DCOMFix log in your My Documents\ASCOM\2018-XX-YY folder.");
            }
        }

        private void ApplyAppIds(DCOMAuthenticationLevel authenticationLevel)
        {
            AddAppID("ScopeSim.Telescope", "ScopeSim.exe", "{4597A685-11FD-47ae-A5D2-A8DC54C90CDC}", authenticationLevel);
            AddAppID("FocusSim.Focuser", "FocusSim.exe", "{289BFF60-3B9E-417c-8DA1-F96773679342}", authenticationLevel);
            AddAppID("DomeSim.Dome", "DomeSim.exe", "{C47DAA29-5788-464e-BBA7-9711FBC7A01F}", authenticationLevel);
            AddAppID("POTH.Telescope", "POTH.exe", "{0A21726F-E58B-4461-85A9-9AA739E6E42F}", authenticationLevel);
            AddAppID("POTH.Dome", "", "{0A21726F-E58B-4461-85A9-9AA739E6E42F}", authenticationLevel);
            AddAppID("POTH.Focuser", "", "{0A21726F-E58B-4461-85A9-9AA739E6E42F}", authenticationLevel);
            AddAppID("Pipe.Telescope", "Pipe.exe", "{080B23A0-3190-45e5-A7C8-53C61F22DFF2}", authenticationLevel);
            AddAppID("Pipe.Focuser", "", "{080B23A0-3190-45e5-A7C8-53C61F22DFF2}", authenticationLevel);
            AddAppID("Pipe.Dome", "", "{080B23A0-3190-45e5-A7C8-53C61F22DFF2}", authenticationLevel);
            AddAppID("Hub.Telescope", "Hub.exe", "{3213D452-2A8F-4624-9D65-E04E175E4CB2}", authenticationLevel);
            AddAppID("Hub.Focuser", "", "{3213D452-2A8F-4624-9D65-E04E175E4CB2}", authenticationLevel);
            AddAppID("Hub.Dome", "", "{3213D452-2A8F-4624-9D65-E04E175E4CB2}", authenticationLevel);
            AddAppID("ASCOMDome.Telescope", "ASCOMDome.exe", "{B5863239-0A6E-48d4-A9EA-0DDA4D942390}", authenticationLevel);
            AddAppID("ASCOMDome.Dome", "", "{B5863239-0A6E-48d4-A9EA-0DDA4D942390}", authenticationLevel);

            AddAppID("ASCOM.Simulator.FilterWheel", "ASCOM.FilterWheelSim.exe", "{AE139A96-FF4D-4F22-A44C-141A9873E823}", authenticationLevel);
            AddAppID("ASCOM.Simulator.Rotator", "ASCOM.RotatorSimulator.exe", "{5D4BBF44-2573-401A-AEE1-F9716D0BAEC3}", authenticationLevel);
            AddAppID("ASCOM.Simulator.Telescope", "ASCOM.TelescopeSimulator.exe", "{1620DCB8-0352-4717-A966-B174AC868FA0}", authenticationLevel);

            AddAppID("ASCOM.Simulator.Switch", "ASCOM.Simulator.Server.exe", "{05FA1687-DAAB-4003-BC69-5BDE762AE94C}", authenticationLevel);
            AddAppID("ASCOM.Simulator.ObservingConditions", "ASCOM.OCSimulator.Server.exe", "{861B2761-A31E-4413-82BF-BE90D2AB7AF5}", authenticationLevel);
            AddAppID("ASCOM.OCH.ObservingConditions", "ASCOM.OCH.Server.exe", "{DF5B974C-385A-437F-BC0D-F67111F12452}", authenticationLevel);
        }


        private void AddAppID(string progID, string exeName, string sAPPID, DCOMAuthenticationLevel authenticationLevel)
        {
            RegistryKey rkCLSID = null;
            RegistryKey rkAPPID = null;
            RegistryKey rkAPPIDExe = null;

            try
            {
                LogMessage("AddAppID", "ProgID: " + progID + ", ExeName: " + exeName + ", Appid: " + sAPPID);

                int hr = CLSIDFromProgID(progID, out Guid gCLSID);
                string sCLSID = "{" + new GuidConverter().ConvertToString(gCLSID) + "}";
                LogMessage("AddAppID", "  CLSID: " + sCLSID);


                rkCLSID = Registry.ClassesRoot.OpenSubKey("CLSID\\" + sCLSID, RegistryKeyPermissionCheck.ReadWriteSubTree);
                LogMessage("AddAppID", string.Format("Registry key rkCLSID is null: {0}", rkCLSID == null));
                rkCLSID.SetValue("AppId", sAPPID);
                rkAPPID = Registry.ClassesRoot.CreateSubKey("APPID\\" + sAPPID, RegistryKeyPermissionCheck.ReadWriteSubTree);
                rkAPPID.SetValue("", rkCLSID.GetValue(""));				// Same description as class
                rkAPPID.SetValue("AppId", sAPPID);
                rkAPPID.SetValue("AuthenticationLevel", authenticationLevel, RegistryValueKind.DWord);	// RPC_C_AUTHN_LEVEL_NONE

                if (exeName != "")										// If want AppId\Exe.name
                {
                    rkAPPIDExe = Registry.ClassesRoot.CreateSubKey("AppId\\" + exeName, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    rkAPPIDExe.SetValue("", rkCLSID.GetValue(""));		// Same description as class
                    rkAPPIDExe.SetValue("AppId", sAPPID);
                }
                LogMessage("AddAppID", "OK - Completed");
            }
            catch (Exception ex)
            {
                errorCount += 1;
                LogMessage("AddAppID", string.Format("Failed to add AppID info for {0}", progID));
                TL.LogMessageCrLf("AddAppID", "Exception: " + ex.ToString());
            }
            finally
            {
                if (rkCLSID != null) rkCLSID.Close();
                if (rkAPPID != null) rkAPPID.Close();
                if (rkAPPIDExe != null) rkAPPIDExe.Close();
            }
        }


    }
}
