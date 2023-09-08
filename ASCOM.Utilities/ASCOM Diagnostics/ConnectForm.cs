using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ASCOM.Utilities
{
    public partial class ConnectForm
    {

        private const string DEFAULT_DEVICE_TYPE = "Telescope";
        private const string DEFAULT_DEVICE = "ScopeSim.Telescope";

        private string CurrentDevice, CurrentDeviceType;
        private bool Connected;
        private dynamic Device;
        private Util Util;

        public ConnectForm()
        {
            InitializeComponent();
        }

        // API's for auto drop down combo
        [DllImport("user32", EntryPoint = "SendMessageA")]
        private static extern long SendMessage(long hwnd, long wMsg, long wParam, long lParam);
        private const int CB_SHOWDROPDOWN = 0x14F;

        private void ConnectForm_Load(object sender, EventArgs e)
        {
            ArrayList DeviceTypes;
            var Profile = new Profile();

            cmbDeviceType.SelectedIndexChanged += (_, __) => DevicetypeChangedhandler();
            try
            {
                Util = new Util();
                DeviceTypes = Profile.RegisteredDeviceTypes;
                foreach (string DeviceType in DeviceTypes)
                    cmbDeviceType.Items.Add(DeviceType);
                CurrentDevice = DEFAULT_DEVICE;
                CurrentDeviceType = DEFAULT_DEVICE_TYPE;
                cmbDeviceType.SelectedItem = CurrentDeviceType;
                btnProperties.Enabled = false;
                txtDevice.Text = CurrentDevice;
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
        }

        private void DevicetypeChangedhandler()
        {
            CurrentDeviceType = cmbDeviceType.SelectedItem.ToString();
            CurrentDevice = "";
            txtDevice.Text = "";
            SetScriptButton();
            btnConnect.Enabled = false;
            btnProperties.Enabled = false;
            btnScript.Enabled = false;
            btnGetProfile.Enabled = false;
        }

        public void SetScriptButton()
        {
            if (CurrentDeviceType == "Telescope" & !string.IsNullOrEmpty(CurrentDevice)) // Enable or disable the run script button as appropriate
            {
                btnScript.Enabled = true;
            }
            else
            {
                btnScript.Enabled = false;
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            Chooser Chooser;
            string NewDevice;

            CurrentDeviceType = Conversions.ToString(cmbDeviceType.SelectedItem);
            Chooser = new Chooser();
            Chooser.DeviceType = CurrentDeviceType;
            if (string.IsNullOrEmpty(CurrentDevice))
            {
                switch (CurrentDeviceType ?? "")
                {
                    case "Telescope":
                        {
                            CurrentDevice = "ScopeSim.Telescope";
                            break;
                        }
                    case "Focuser":
                        {
                            CurrentDevice = "ASCOM.Simulator.Focuser";
                            break;
                        }

                }
            }
            NewDevice = Chooser.Choose(CurrentDevice);
            if (!string.IsNullOrEmpty(NewDevice))
                CurrentDevice = NewDevice;

            if (!string.IsNullOrEmpty(CurrentDevice))
            {
                btnProperties.Enabled = true;
                btnConnect.Enabled = true;
                btnGetProfile.Enabled = true;
            }
            else
            {
                btnProperties.Enabled = false;
                btnConnect.Enabled = false;
                btnGetProfile.Enabled = false;
            }

            txtDevice.Text = CurrentDevice;
            SetScriptButton();
            Chooser.Dispose();

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Type TypeCurrentDevice;

            if (!string.IsNullOrEmpty(CurrentDevice))
            {
                if (Connected) // Disconnect
                {
                    try
                    {
                        txtStatus.Text = "Disconnecting...";
                        Application.DoEvents();
                        switch (CurrentDeviceType ?? "")
                        {
                            case "Focuser":
                                {
                                    Device.Link = (object)false;
                                    break;
                                }

                            default:
                                {
                                    Device.Connected = (object)false;
                                    break;
                                }
                        }
                        Connected = false;
                        txtStatus.Text = "Disconnected OK";
                        btnConnect.Text = "Connect";
                        btnChoose.Enabled = true;
                        if (!string.IsNullOrEmpty(CurrentDevice))
                            btnProperties.Enabled = true;
                        SetScriptButton(); // Enable or disable script button according todevice type
                        cmbDeviceType.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        txtStatus.Text = "Disconnect Failed..." + ex.Message + "\r\n" + "\r\n" + ex.ToString();
                    }
                    finally
                    {
                        try
                        {
                            Device.Dispose();
                        }
                        catch
                        {
                        }
                        try
                        {
                            Marshal.ReleaseComObject(Device);
                        }
                        catch
                        {
                        }
                        Device = null;
                    }
                }

                else // Disconnected so connect
                {
                    try
                    {
                        txtStatus.Text = "Connecting...";
                        Application.DoEvents();
                        // Device = CreateObject(CurrentDevice)
                        TypeCurrentDevice = Type.GetTypeFromProgID(CurrentDevice); // Try Activator approach as this may give more meaningful eror messages
                        Device = Activator.CreateInstance(TypeCurrentDevice);
                        switch (CurrentDeviceType ?? "")
                        {
                            case "Focuser":
                                {
                                    Device.Link = (object)true;
                                    break;
                                }

                            default:
                                {
                                    Device.Connected = (object)true;
                                    break;
                                }
                        }
                        Connected = true;
                        txtStatus.Text = "Connected OK";
                        btnConnect.Text = "Disconnect";
                        btnChoose.Enabled = false;
                        btnProperties.Enabled = false;
                        btnScript.Enabled = false;
                        cmbDeviceType.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        txtStatus.Text = "Connect Failed..." + ex.Message + "\r\n" + "\r\n" + ex.ToString();
                        try
                        {
                            Device.Dispose();
                        }
                        catch (Exception dex)
                        {
                            txtStatus.AppendText("\r\n" + "Dispose Failed..." + dex.Message + "\r\n" + "\r\n" + dex.ToString());
                        }
                        try
                        {
                            Marshal.ReleaseComObject(Device);
                        }
                        catch (Exception rex)
                        {
                            txtStatus.AppendText("\r\n" + "Release Failed..." + rex.Message + "\r\n" + "\r\n" + rex.ToString());
                        }
                    }
                }
            }
            else
            {
                txtStatus.Text = "Cannot connect, no device has been set";
            }
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            Type DeviceType;

            // Device = CreateObject(CurrentDevice)
            DeviceType = Type.GetTypeFromProgID(CurrentDevice);
            Device = Activator.CreateInstance(DeviceType);
            Device.SetupDialog();
            try
            {
                Marshal.ReleaseComObject(Device);
            }
            catch
            {
            }
            Device = null;
        }

        private TraceLogger TL;
        private dynamic DeviceObject;

        private void btnScript_Click(object sender, EventArgs e)
        {
            txtStatus.Clear();
            TL = new TraceLogger("", "DiagnosticScript");
            TL.Enabled = true;
            LogMsg("Script", "Diagnostic Script Started");
            ExecuteCommand("CreateObject");
            ExecuteCommand("Connect");
            ExecuteCommand("Description");
            ExecuteCommand("DriverInfo");
            ExecuteCommand("DriverVersion");
            for (int i = 1; i <= 3; i++)
            {
                ExecuteCommand("RightAscension");
                ExecuteCommand("Declination");
            }
            ExecuteCommand("Disconnect");
            ExecuteCommand("DestroyObject");
            LogMsg("Script", "Diagnostic Script Completed");
            TL.Enabled = false;
            TL.Dispose();
        }

        public void ExecuteCommand(string Command)
        {
            var sw = new Stopwatch();
            var StartTime = default(DateTime);
            string Result = "";
            Type DeviceType;

            try
            {
                StartTime = DateTime.Now;
                sw.Start();
                LogMsg(Command, "Started");

                switch (Command ?? "")
                {
                    case "CreateObject":
                        {
                            // DeviceObject = CreateObject(CurrentDevice)
                            DeviceType = Type.GetTypeFromProgID(CurrentDevice);
                            DeviceObject = Activator.CreateInstance(DeviceType);
                            break;
                        }
                    case "Connect":
                        {
                            DeviceObject.Connected = (object)true;
                            break;
                        }
                    case "DriverInfo":
                        {
                            Result = Conversions.ToString(DeviceObject.DriverInfo);
                            break;
                        }
                    case "Description":
                        {
                            Result = Conversions.ToString(DeviceObject.Description);
                            break;
                        }
                    case "DriverVersion":
                        {
                            Result = Conversions.ToString(DeviceObject.DriverVersion);
                            break;
                        }
                    case "RightAscension":
                        {
                            Result = Util.DegreesToHMS(Conversions.ToDouble(DeviceObject.RightAscension));
                            break;
                        }
                    case "Declination":
                        {
                            Result = Util.DegreesToDMS(Conversions.ToDouble(DeviceObject.Declination), ":", ":", "");
                            break;
                        }
                    case "Disconnect":
                        {
                            DeviceObject.Connected = (object)false;
                            break;
                        }
                    case "DestroyObject":
                        {
                            Marshal.ReleaseComObject(DeviceObject);
                            DeviceObject = null;
                            break;
                        }

                    default:
                        {
                            LogMsg(Command, "***** Unknown command *****");
                            break;
                        }
                }
                sw.Stop();

                LogMsg(Command, "Finished - Started: " + Strings.Format(StartTime, "HH:mm:ss.fff") + " duration: " + sw.ElapsedMilliseconds + " ms - " + Result);
            }
            catch (Exception ex)
            {
                LogMsg(Command, "Exception - Started: " + Strings.Format(StartTime, "HH:mm:ss.fff") + " duration: " + sw.ElapsedMilliseconds + " ms - Exception: " + ex.ToString());
            }
        }
        public void LogMsg(string Command, string Msg)
        {
            TL.LogMessage(Command, Msg);
            txtStatus.Text = txtStatus.Text + Command + " " + Msg + "\r\n";
        }

        private void btnGetProfile_Click(object sender, EventArgs e)
        {
            var Prof = new Profile();
            string Result = "";
            txtStatus.Clear();
            TL = new TraceLogger("", "DiagnosticScript");
            TL.Enabled = true;
            Prof.DeviceType = CurrentDeviceType;
            Result = Prof.GetProfileXML(txtDevice.Text);
            LogMsg("GetProfile", Result);
            LogMsg("Script", "Diagnostic Script Completed");
            TL.Enabled = false;
            TL.Dispose();

        }


        public void cmbDeviceType_Click()
        {
            // If Not cmbDeviceType.DroppedDown Then cmbDeviceType.
            SendMessage((long)cmbDeviceType.Handle, CB_SHOWDROPDOWN, 1L, 0L);
            cmbDeviceType.Cursor = Cursors.Arrow;
        }

        private void cmbDeviceType_Click(object sender, MouseEventArgs e) => cmbDeviceType_Click();

    }
}