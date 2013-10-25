using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Diagnostics;
using ASCOM.Utilities;

namespace ASCOM.Setup
{
    public partial class DeviceDriverForm : Form
    {
        /// <summary>
        /// Format string for ASCOM DeviceId.
        /// </summary>
        private const string csDeviceIdFormat = "ASCOM.{0}.{1}";
        /// <summary>
        /// Format string for driver namespace.
        /// </summary>
        private const string csNamespaceFormat = "ASCOM.{0}";
        /// <summary>
        /// Placeholder text used for null or invalid DeviceName.
        /// </summary>
        private const string csDeviceNamePlaceholder = "<DeviceName>";
        /// <summary>
        /// Placeholder text used for null or invalid DeviceClass.
        /// </summary>
        private const string csDeviceClassPlaceholder = "<DeviceClass>";

        // key is the class name, value is the ASCOM interface properties
        Dictionary<string, ASCOMInterface> interfaceList;

        TraceLogger TL;

        public DeviceDriverForm()
        {
            InitializeComponent();
            TL = new TraceLogger("TemplateForm");
            TL.Enabled = true;

            InitASCOMClasses();
        }

        /// <summary>
        /// Handles the Validating event of the txtDeviceName control.
        /// Checks that the typed text is a valid ASCOM DeviceName.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// The device name must start with an alphabetic character and may be followed by any number of alphanumeric
        /// characters. There may be no whitespace or punctuation.
        /// </remarks>
        private void txtDeviceName_Validating(object sender, CancelEventArgs e)
        {
            Regex rxValidateDeviceName = new Regex(@"^[a-zA-Z]([a-zA-Z0-9]*)$");
            if (!rxValidateDeviceName.IsMatch(this.txtDeviceName.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(this.txtDeviceName, "Invalid DeviceId. Must begin with a letter and contain only alphanumeric characters");
            }

            if (string.IsNullOrEmpty(this.txtDeviceName.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(this.txtDeviceName, "Please enter a device name");
            }

        }

        /// <summary>
        /// Handles the Validated event of the <see cref="txtDeviceName"/> control
        /// and clears the control's error state.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void txtDeviceName_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(this.txtDeviceName, String.Empty);
        }

        /// <summary>
        /// Gets the full ASCOM device id (COM ProgID), in the format
        /// ASCOM.{DeviceName}.{DeviceClass}
        /// </summary>
        /// <value>The full ASCOM device id (COM ProgID).</value>
        internal string DeviceId
        {
            get
            {
                return String.Format(csDeviceIdFormat, DeviceName, DeviceClass);
            }
        }
        /// <summary>
        /// Gets the ASCOM DeviceClass of the device.
        /// </summary>
        /// <value>The ASCOM device class.</value>
        internal string DeviceClass
        {
            get
            {
                return (string)this.cbDeviceClass.SelectedItem ?? csDeviceClassPlaceholder;
            }
        }
        /// <summary>
        /// Gets the ASCOM DeviceName of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        internal string DeviceName
        {
            get
            {
                if (String.IsNullOrEmpty(this.txtDeviceName.Text))
                {
                    return csDeviceNamePlaceholder;
                }
                return (this.txtOrganizationName.Text ?? String.Empty) + this.txtDeviceName.Text;
            }
        }

        /// <summary>
        /// return the device interface from the device class
        /// </summary>
        internal string DeviceInterface
        {
            get
            {
                return interfaceList[DeviceClass].InterfaceName;
            }
        }

        /// <summary>
        /// Return the interface version from the device class
        /// </summary>
        internal string InterfaceVersion
        {
            get
            {
                return interfaceList[DeviceClass].InterfaceVersion;
            }
        }

        internal string Namespace
        {
            get
            {
                return String.Format(csNamespaceFormat, this.DeviceName);
            }
        }

        private void cbDeviceClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblDeviceId.Text = this.DeviceId;
        }

        /// <summary>
        /// Handles the TextChanged event of the txtDeviceName and txtDeviceName controls.
        /// Sets the value of lblDeviceId based on user input.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void txtDeviceName_TextChanged(object sender, EventArgs e)
        {
            this.lblDeviceId.Text = this.DeviceId;
        }

        /// <summary>
        /// Handles the Click event of the btnCreate control.
        /// Closes the form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Before dismissing the dialog, run one final validation pass.
            if (!this.ValidateChildren(ValidationConstraints.Enabled))
                return;

            if (cbDeviceClass.SelectedIndex < 0 || !cbDeviceClass.Items.Contains(cbDeviceClass.Text) || String.IsNullOrEmpty(cbDeviceClass.Text))
            {
                errorProvider.SetError(this.cbDeviceClass, "Please select a device type from the list.");
                return;
            }

            if (string.IsNullOrEmpty(txtDeviceName.Text.Trim()))
            {
                errorProvider.SetError(this.txtDeviceName, "Please enter a driver name.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        /// <summary>
        /// Handles the Validating event of the txtOrganizationName control.
        /// The value of this control must either be empty, or must contain only alphabetics.
        /// </summary>
        /// <param name="sender">The source of the event, not used.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// The organization name must either be blank, or contain a sequence of alphabetic characters.
        /// No white space or punctuation is allowed.
        /// </remarks>
        private void txtOrganizationName_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtOrganizationName.Text))
                return;
            Regex rxValidateOrgName = new Regex(@"^[a-zA-Z]+$");
            if (!rxValidateOrgName.IsMatch(this.txtOrganizationName.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(this.txtOrganizationName, "Invalid organization name. Must either be empty, or contain only alphabetic characters.\nBy convention, the first character should be upper case.");
            }
        }

        /// <summary>
        /// Handles the Validated event of the <see cref="txtOrganizationName"/> control
        /// and clears the control's error state.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void txtOrganizationName_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(this.txtOrganizationName, String.Empty);
        }

        private void CheckType(int level)
        {
            StackFrame stack1 = new StackFrame(level, true);
            TL.LogMessage("CheckType", "Level: " + level + ", Type Name: " + stack1.GetMethod().Name + ", ReflectedType Name: " + stack1.GetMethod().ReflectedType.Name + ", ReflectedType Namespace: " + stack1.GetMethod().ReflectedType.Namespace);

            Type type1 = stack1.GetMethod().ReflectedType;
            TL.LogMessage("CheckType", "Method Type Name: " + type1.Name);
            if (type1 == typeof(ASCOM.Setup.VideoUsingBaseClassWizard))
            {
                TL.LogMessage("CheckType", "VideoUsingBaseClassWizard: Types match!!!");
            }
            else TL.LogMessage("CheckType", "VideoUsingBaseClassWizard: Types don't match");
            if (type1 == typeof(ASCOM.Setup.DriverWizard))
            {
                TL.LogMessage("CheckType", "DriverWizard: Types match!!!");
            }
            else TL.LogMessage("CheckType", "DriverWizard: Types don't match");
            TL.BlankLine();
        }

        /// <summary>
        /// Goes through the ASCOM.DeviceInterfaces assembly extracting the interfacces that implement drivers
        /// Use this information to build a list of interfaces with the associated class name and interfacce version
        /// put the class names in the device names combo box
        /// </summary>
        private void InitASCOMClasses()
        {
            TL.LogMessage("InitASCOMClasses", "Started");
            interfaceList = new Dictionary<string, ASCOMInterface>();
            cbDeviceClass.Items.Clear();
            try
            {
                CheckType(1);
                CheckType(2);
                CheckType(3);
                CheckType(4);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("InitASCOMClasses", "Exception: " + ex.ToString());
            }

            if (new StackFrame(3, true).GetMethod().ReflectedType == typeof(ASCOM.Setup.DriverWizard)) // Form called from DriverWizard
            {
                // get a list of the current interfaces
                Assembly asm = Assembly.ReflectionOnlyLoad("ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7");
                // look for the interfaces
                foreach (var type in asm.GetExportedTypes())
                {
                    if (type.IsInterface)
                    {
                        // only add those with the SetupDialog method,
                        // so the extra telescope classes are not added
                        var p = type.GetMethod("SetupDialog");
                        if (p != null && p.Name == "SetupDialog")
                        {
                            // get the class name by removing the leading I and the training Vn
                            string name = type.Name.Substring(1);
                            name = name.TrimEnd('1', '2', '3', '4', 'V');
                            ASCOMInterface ai = new ASCOMInterface(type.Name);
                            interfaceList.Add(ai.Name, ai);
                            cbDeviceClass.Items.Add(ai.Name);
                            TL.LogMessage("InitASCOMClasses", "Added: " + ai.Name + " " + name + " " + type.Name + " " + type.Namespace.ToString() + " " + type.FullName);
                        }
                    }
                }
                this.cbDeviceClass.SelectedIndex = 7; // Select Telescope as the default
            }

            if (new StackFrame(3, true).GetMethod().ReflectedType == typeof(ASCOM.Setup.VideoUsingBaseClassWizard)) // Form called from VideoUsingBaseClass wizard
            {
                Assembly asm = Assembly.ReflectionOnlyLoad("ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7");
                Type aiVbcType = asm.GetType("ASCOM.DeviceInterface.IVideo", true, true);

                ASCOMInterface aivbc = new ASCOMInterface(aiVbcType.Name);
                aivbc.InterfaceName = "DirectShowVideoBase, IVideo";
                interfaceList.Add("VideoUsingBaseClass", aivbc);
                cbDeviceClass.Items.Add("VideoUsingBaseClass");

                this.cbDeviceClass.SelectedIndex = 0; // Select VideoUsingBaseClass as the default
            }
        }

        private class ASCOMInterface
        {
            internal string Name { get; private set; }
            internal string InterfaceName { get; set; }
            internal string InterfaceVersion { get; set; }

            internal ASCOMInterface(string interfaceName)
            {
                InterfaceName = interfaceName;
                // get the class name by removing the leading I and the training Vn
                Name = interfaceName.Substring(1);
                Name = Name.TrimEnd('1', '2', '3', '4', 'V');
                // get the interface version by looking for a trailing Vn
                if (InterfaceName.LastIndexOf('V') == InterfaceName.Length - 2)
                {
                    InterfaceVersion = InterfaceName.Substring(InterfaceName.Length - 1);
                }
                else
                    InterfaceVersion = "1";
            }

        }
    }
}
