using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Utilities;

namespace ASCOM.SwitchSimulator
{
    [Guid("EADEF200-5861-4a86-B9AB-5C11E680D69E")]
    [ClassInterface(ClassInterfaceType.None)]
    class Driver : ReferenceCountedObjectBase, IDriver
    {
        public IAuthor author = null;
        public ISwitch aswitch = null;
        public ISwitches switches = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Switch"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Driver()
        {
            //TODO: Implement your additional construction here
        }

        #region IDriver Members

        public bool Connected
        {
            get { return SwitchHardware.Connected; }
            set { SwitchHardware.Connected = value; }
        }

        public string DriverName
        {
            get { return SwitchHardware.Name; }
            set { SwitchHardware.Name = value; }
        }
        
        public string Information
        {
            get { return SwitchHardware.DriverInfo; }
            set { SwitchHardware.DriverInfo = value; }
        }

        public string Version
        {
            get { return SwitchHardware.DriverVersion; }
            set { SwitchHardware.DriverVersion = value; }
        }

        public ushort InterfaceVersion
        {
            get { return SwitchHardware.InterfaceVersion; }
            set { SwitchHardware.InterfaceVersion = value; }
        }

        public string Description
        {
            get { return SwitchHardware.Description; }
            set { SwitchHardware.Description = value; }
        }

        string IAuthor.Name
        {
            get { return author.Name; }
            set { author.Name = value; }
        }

        string IAuthor.Email
        {
            get { return author.Email; }
            set { author.Email = value; }
        }

        byte ISwitch.Id
        {
            get { return aswitch.Id; }
            set { aswitch.Id = value; }
        }

        string ISwitch.Name
        {
            get { return aswitch.Name; }
            set { aswitch.Name = value; }
        }

        bool ISwitch.State
        {
            get { return aswitch.State; }
            set { aswitch.State = value; }
        }

        #endregion

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
