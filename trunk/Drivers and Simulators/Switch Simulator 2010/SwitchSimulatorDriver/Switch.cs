using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ASCOM.SwitchSimulator
{
    /// <summary>
    /// Summary description for Switch.
    /// </summary>
    public class Switch : SwitchDevice, ISwitch, ASCOM.SwitchSimulator.ISwitch1
    {
        private static bool isConnected;   //keeping tracking state
        private static string authorName = "Rob Morgan";
        private static string authorEmail = "Rob.Morgan.E@Gmail.Com";
        private static string driverName = "ASCOM.SwitchSimulator.Driver";
        private static string description = "The ASCOM Initiative";
        private static string driverInfo = Assembly.GetExecutingAssembly().FullName;
        private static string driverVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public Switch(){}

        public bool IsConnected
        {
            get
            {
                isConnected = SwitchHardware.IsConnected;
                return isConnected;
            }
            set
            {
                SwitchHardware.IsConnected = isConnected;
                isConnected = value;
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
        }
        public string AuthorName
        {
            get { return authorName; }
        }
        public string AuthorEmail
        {
            get { return authorEmail; }
        }
        public string DriverName
        {
            get { return driverName; }
        }
        public string DriverInfo
        {
            get { return driverInfo; }
            set { driverInfo = value; }
        }
        public string DriverVersion
        {
            get { return driverVersion; }
        }

        public SwitchDevice GetSwitchDevice(int i)
        {
            return SwitchHardware.GetSwitchDevice(i);
        }
        public ArrayList GetSwitchDevices()
        {
            return SwitchHardware.GetSwitchDevices();
        }
        public int Count()
        {
            return SwitchHardware.Count();
        }
        public void SetSwitchOn(int i)
        {
        }
        public void SetSwitchOff(int i)
        {
        }

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// </summary>
        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            var result = F.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.Save();
                return;
            }
            Properties.Settings.Default.Reload();
        }
    }
}
