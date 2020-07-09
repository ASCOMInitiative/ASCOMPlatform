using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Diagnostics;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;

namespace ASCOM.Setup
{
    public partial class DeviceDriverForm : Form
    {
        #region Constants, class variables and initialiser

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

        private const string DEVICE_TYPE = "DeviceType"; // Name of the Regex group that will hold the device type
        private const string INTERFACE_VERSION = "InterfaceVersion"; // Name of the Regex group that will hold the interface version number

        /// <summary>
        /// REGEX format string to extract device type and version number fropm the interface name
        /// </summary>
        private const string REGEX_FORMAT = @"^[Ii](?'" + DEVICE_TYPE + @"'\w+)[Vv](?'" + INTERFACE_VERSION + @"'\d+)";

        // key is the class name, value is the ASCOM interface properties
        Dictionary<string, ASCOMInterface> interfaceList;

        TraceLogger TL;

        public DeviceDriverForm(TraceLogger TLSupplied)
        {
            InitializeComponent();
            TL = TLSupplied; // Make sure we use the trace logger that has been created by the calling method
            InitASCOMClasses();
        }

        #endregion

        #region External Properties

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
        internal int InterfaceVersion
        {
            get
            {
                return interfaceList[DeviceClass].InterfaceVersion;
            }
        }

        /// <summary>
        /// Returns this device's namespace
        /// </summary>
        internal string Namespace
        {
            get
            {
                return String.Format(csNamespaceFormat, this.DeviceName);
            }
        }

        #endregion

        #region Form Event Handlers

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

        /// <summary>
        /// Event handler for when the user selects a device type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDeviceClass_SelectedIndexChanged(object sender, EventArgs e)
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

        #endregion

        /// <summary>
        /// Goes through the ASCOM.DeviceInterfaces assembly extracting the interfacces that implement drivers
        /// Use this information to build a list of interfaces with the associated class name and interfacce version
        /// put the class names in the device names combo box
        /// 
        /// Can also adapt its behaviour depending on which Wizard called the form. This capability was added to support additional 
        /// wizards beyond the basic wizard that creates empty projects. At present this capability is not exploited becuase the 
        /// Video Project is no longer going to distribute a second template based on an inherited class. The capability is left in to support
        /// similar requirements should they arise in future. Peter Simpson - October 2013.
        /// </summary>
        private void InitASCOMClasses()
        {
            Regex interfaceRegex = new Regex(REGEX_FORMAT); // Create a regex to parse out the interface name and version number from type names of form: IaaaaaVn

            TL.LogMessage("InitASCOMClasses", "Started");
            interfaceList = new Dictionary<string, ASCOMInterface>();
            cbDeviceClass.Items.Clear();

            // Get an assembly reference to the DeviceInterfaces assembly
            Assembly DeviceInterfacesAssembly = Assembly.GetAssembly(typeof(DriveRates)); // DriveRates is used because it is unlikely to change names in future unlike device interfaces.
            TL.LogMessage("InitASCOMClasses", "Found interface assembly: " + DeviceInterfacesAssembly.FullName);
            TL.LogMessage("InitASCOMClasses", "Interface is located at: " + DeviceInterfacesAssembly.Location);

            // Get the calling type, this allows us to customise the list depending on which wizard called the form
            Type CallingType = new StackFrame(2, false).GetMethod().ReflectedType;
            TL.LogMessage("InitASCOMClasses", "Found calling type: " + CallingType.FullName);

            // Add the relevant device types into the drop-down combo box.
            if (CallingType == typeof(ASCOM.Setup.DriverWizard)) // Form called from DriverWizard
            {
                // get a list of the current interfaces
                // look for the interfaces
                foreach (var type in DeviceInterfacesAssembly.GetExportedTypes())
                {
                    if (type.IsInterface)
                    {
                        // Only add those with the SetupDialog method, so the extra telescope classes are not added
                        var p = type.GetMethod("SetupDialog");
                        if (p != null && p.Name == "SetupDialog")
                        {
                            TL.LogMessage("InitASCOMClasses", "Found interface: " + type.Name);
                            // get the class name by removing the leading I and the training Vn

                            Match m = interfaceRegex.Match(type.Name);
                            if (m.Success) // We do have a match
                            {
                                string deviceType = m.Groups[DEVICE_TYPE].Value;
                                int interfaceVersion = int.Parse(m.Groups[INTERFACE_VERSION].Value);
                                ASCOMInterface ai = new ASCOMInterface(type.Name, deviceType, interfaceVersion);
                                TL.LogMessage("InitASCOMClasses", string.Format("Created ASCOMInterface: {0}, device type: {1}, interface version {2}", type.Name, deviceType, interfaceVersion));

                                if (!interfaceList.ContainsKey(ai.DeviceType)) // Only add an entry if the interface isn't already in the list
                                {
                                    interfaceList.Add(ai.DeviceType, ai); // Revised to support DeviceInterfaces having more than 1 version of a device interface e.g. IFocuserV2 and IFocuserV3
                                    TL.LogMessage("InitASCOMClasses", "Added device: " + ai.DeviceType + " " + deviceType + " " + type.Name + " " + type.Namespace.ToString() + " " + type.FullName);
                                }
                                else
                                {
                                    // Update the interface version if necessary
                                    if (interfaceList[deviceType].InterfaceVersion < interfaceVersion)
                                    {
                                        TL.LogMessage("InitASCOMClasses", string.Format("Updating {0} interface version number from v{1} to v{2}", deviceType, interfaceList[deviceType].InterfaceVersion, interfaceVersion));
                                        interfaceList[deviceType].InterfaceName = type.Name;
                                        interfaceList[deviceType].InterfaceVersion = interfaceVersion;
                                    }
                                    else
                                    {
                                        TL.LogMessage("InitASCOMClasses", string.Format("Ignoring duplicate interface: v{0} that is a lower than that currently stored: v{1}", interfaceVersion, interfaceList[deviceType].InterfaceVersion));
                                    }
                                }
                            }
                            else // Type name didn't match the regex formula so it must be a V1 type that doesn't have a Vx designator at the end of the name
                            {
                                TL.LogMessage("InitASCOMClasses", string.Format("Interface version regex didn't match interface {0}, including it as a V1 interface", type.Name));
                                string deviceType = type.Name.Substring(1); // Remove the leading I character, the rest is the interface name
                                int interfaceVersion = 1; // Set this as a V1 interface

                                ASCOMInterface ai = new ASCOMInterface(type.Name, deviceType, interfaceVersion);
                                TL.LogMessage("InitASCOMClasses", string.Format("Created ASCOMInterface: {0}, device type: {1}, interface version {2}", type.Name, deviceType, interfaceVersion));
                                if (!interfaceList.ContainsKey(ai.DeviceType)) // Add the device type so long as it is not already present
                                {
                                    interfaceList.Add(ai.DeviceType, ai);
                                    TL.LogMessage("InitASCOMClasses", "Added device: " + ai.DeviceType + " " + deviceType + " " + type.Name + " " + type.Namespace.ToString() + " " + type.FullName);
                                }
                                else
                                {
                                    TL.LogMessage("InitASCOMClasses", string.Format("Ignoring duplicate V1 interface: {0} ", type.Name));
                                }
                            }
                        }
                    }
                }

                // Add the interfaces to the dropdown list and pre-select the Telescope type
                foreach (KeyValuePair<string, ASCOMInterface> ai in interfaceList)
                {
                    cbDeviceClass.Items.Add(ai.Value.DeviceType);
                }
                this.cbDeviceClass.SelectedIndex = this.cbDeviceClass.FindString("Telescope"); // Select Telescope as the default
            }

            /* The following is left in as an example of how to introduce different handling for another Wizard type

            if (CallingType == typeof(ASCOM.Setup.VideoUsingBaseClassWizard)) // Form called from VideoUsingBaseClass wizard
            {
                Type aiVbcType = DeviceInterfacesAssembly.GetType("ASCOM.DeviceInterface.IVideo", true, true);

                ASCOMInterface aivbc = new ASCOMInterface(aiVbcType.Name);
                aivbc.InterfaceName = "DirectShowVideoBase, IVideo";
                interfaceList.Add("VideoUsingBaseClass", aivbc);
                cbDeviceClass.Items.Add("VideoUsingBaseClass");
                TL.LogMessage("InitASCOMClasses", "Added device: VideoUsingBaseClass");

                this.cbDeviceClass.SelectedIndex = 0; // Select VideoUsingBaseClass as the default
            }
             */
        }

        /// <summary>
        /// Class to hold information about a device type
        /// </summary>
        private class ASCOMInterface
        {
            internal string DeviceType { get; private set; }
            internal string InterfaceName { get; set; }
            internal int InterfaceVersion { get; set; }

            internal ASCOMInterface(string interfaceName, string deviceType, int interfaceVersion)
            {
                InterfaceName = interfaceName;
                DeviceType = deviceType;
                InterfaceVersion = interfaceVersion;
            }

        }
    }
}
