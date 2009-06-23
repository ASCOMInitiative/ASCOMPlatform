using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Microsoft.Win32;
using ASCOM.DriverAccess;
using System.Collections.Generic;
using ASCOM.HelperNET;
using System.Diagnostics;

namespace ASCOM.Controls
{
	/// <summary>
	/// Summary description for Chooser.
	/// </summary>
	public class Chooser : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.PictureBox pictureASCOMLogo;
		private System.Windows.Forms.Label labelPrompt;
		private System.Windows.Forms.Button buttonDriverProperties;
		private System.Windows.Forms.ComboBox comboSelectDriver;
		private System.Windows.Forms.Label labelAboutASCOM;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolTip toolTipChooser;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Default public constructor; create a new chooser object and populate the device selection combo box.
		/// </summary>
		public Chooser()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			// Create the Most-Recently-Used list
			this.m_MRU = new ChooserMRU();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
				if (m_MRU != null)
				{
					m_MRU = null;
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chooser));
			this.pictureASCOMLogo = new System.Windows.Forms.PictureBox();
			this.labelPrompt = new System.Windows.Forms.Label();
			this.buttonDriverProperties = new System.Windows.Forms.Button();
			this.comboSelectDriver = new System.Windows.Forms.ComboBox();
			this.labelAboutASCOM = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.toolTipChooser = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.pictureASCOMLogo)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureASCOMLogo
			// 
			this.pictureASCOMLogo.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureASCOMLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureASCOMLogo.Image")));
			this.pictureASCOMLogo.Location = new System.Drawing.Point(8, 8);
			this.pictureASCOMLogo.Name = "pictureASCOMLogo";
			this.pictureASCOMLogo.Size = new System.Drawing.Size(48, 56);
			this.pictureASCOMLogo.TabIndex = 1;
			this.pictureASCOMLogo.TabStop = false;
			this.toolTipChooser.SetToolTip(this.pictureASCOMLogo, "Click to open a browser window and visit the ASCOM Standards web site.");
			this.pictureASCOMLogo.Click += new System.EventHandler(this.pictureASCOMLogo_Click);
			// 
			// labelPrompt
			// 
			this.labelPrompt.Location = new System.Drawing.Point(64, 7);
			this.labelPrompt.Name = "labelPrompt";
			this.labelPrompt.Size = new System.Drawing.Size(272, 29);
			this.labelPrompt.TabIndex = 4;
			this.labelPrompt.Text = "Select your {0} from the drop-down list. Click the Properties button to adjust se" +
				"ttings.";
			// 
			// buttonDriverProperties
			// 
			this.buttonDriverProperties.Location = new System.Drawing.Point(264, 40);
			this.buttonDriverProperties.Name = "buttonDriverProperties";
			this.buttonDriverProperties.Size = new System.Drawing.Size(75, 23);
			this.buttonDriverProperties.TabIndex = 6;
			this.buttonDriverProperties.Text = "Properties...";
			this.buttonDriverProperties.Click += new System.EventHandler(this.buttonDriverProperties_Click);
			// 
			// comboSelectDriver
			// 
			this.comboSelectDriver.Location = new System.Drawing.Point(64, 40);
			this.comboSelectDriver.Name = "comboSelectDriver";
			this.comboSelectDriver.Size = new System.Drawing.Size(192, 21);
			this.comboSelectDriver.TabIndex = 5;
			this.comboSelectDriver.Text = "Select Driver";
			this.comboSelectDriver.SelectionChangeCommitted += new System.EventHandler(this.comboSelectDriver_SelectionChangeCommitted);
			// 
			// labelAboutASCOM
			// 
			this.labelAboutASCOM.Location = new System.Drawing.Point(8, 70);
			this.labelAboutASCOM.Name = "labelAboutASCOM";
			this.labelAboutASCOM.Size = new System.Drawing.Size(328, 32);
			this.labelAboutASCOM.TabIndex = 7;
			this.labelAboutASCOM.Text = "Click the ASCOM logo to learn more about the ASCOM initiative, a set of standards" +
				" for interoperation of astronomy software.";
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.labelPrompt);
			this.panel1.Controls.Add(this.buttonDriverProperties);
			this.panel1.Controls.Add(this.comboSelectDriver);
			this.panel1.Controls.Add(this.labelAboutASCOM);
			this.panel1.Controls.Add(this.pictureASCOMLogo);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(344, 104);
			this.panel1.TabIndex = 8;
			// 
			// Chooser
			// 
			this.Controls.Add(this.panel1);
			this.Name = "Chooser";
			this.Size = new System.Drawing.Size(344, 104);
			this.Load += new System.EventHandler(this.Chooser_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureASCOMLogo)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Get or set the Driver Class (or device type) being chosen, as a string.
		/// When this property is changed, the ComboBox is refreshed to reflect the new device type.
		/// The selection list is positioned at the most recently used DeviceID, if possible.
		/// </summary>
		[Category("ASCOM")]
		[Description("Configures the chooser for a particular class of device.")]
		[Browsable(true)]
		//[DefaultValue("Telescope")]
		public string DeviceClass
			{
			get
				{
				return m_eDriverClass;
				}
			set
				{
				Trace.WriteLine(String.Format("Chooser device class changing to {0}", value));
				m_eDriverClass = value;
				m_MRU.MRUType = value;
				this.DeviceID = m_MRU.MostRecentlyUsedDeviceID;	// This repopulates the device list.
				this.labelPrompt.Text = string.Format(cstrPromptLabel, DeviceClass);
				}
			}

		/// <summary>
		/// The class of device being chosen. Defaults to "Telescope".
		/// </summary>
		private string m_eDriverClass;
		/// <summary>
		/// Handles a click event on the ASCOM logo image.
		/// Starts a web browser and navigates to <see cref="URL"/>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureASCOMLogo_Click(object sender, System.EventArgs e)
		{
			Cursor = Cursors.AppStarting;
			System.Diagnostics.Process.Start(Chooser.URL);
			Cursor = Cursors.Default;
		}
		/// <summary>
		/// Returns the full URL of the ASCOM-Standards web site, as a string.
		/// </summary>
		/// <value>The full URL of the ASCOM-Standards web site, as a string.</value>
		public static string URL
		{
			get
			{
				return Properties.Settings.Default.ASCOMStandardsURL;
			}
		}
		/// <summary>
		/// Populates the device list by enumerating the entries for devices in the given DriverClass.
		/// The DriverClass comes from the DeviceClass property.
		/// </summary>
		internal void PopulateDeviceList()
			{
			if (this.DesignMode)
				return;

			using (Profile P = new Profile())
				{
				SortedList<string, string> installedDrivers = P.get_RegisteredDevices(this.DeviceClass);

				if (installedDrivers.Count == 0)
					{	// There is no registry key and therefore no drivers of this type are registered.
					// Populate the combo box with a dummy entry and disable the control.
					// The entry will read "no %DeviceClass% drivers installed".
					this.m_strDeviceID = null;	// Ensure we don't return an obsolete device ID.
					this.comboSelectDriver.BeginUpdate();
					this.comboSelectDriver.Items.Clear();
					string strDescription = string.Format("(no {0} drivers installed)", this.DeviceClass);
					DeviceDescriptor objDummy = new DeviceDescriptor("null.null", strDescription);
					this.comboSelectDriver.Items.Add(objDummy);
					this.comboSelectDriver.SelectedIndex = 0;
					this.comboSelectDriver.Enabled = false;	// Control is disabled while there are no items
					this.buttonDriverProperties.Enabled = false;
					this.comboSelectDriver.EndUpdate();
					return;
					}
				this.comboSelectDriver.BeginUpdate();
				this.comboSelectDriver.Items.Clear();
				int nDefaultIndex = 0;	// Default selection, defaults to first item in the list.
				foreach (KeyValuePair<string, string> deviceProfileKey in installedDrivers)
					{
					DeviceDescriptor deviceDescriptor = new DeviceDescriptor(deviceProfileKey);
					string strDescription = String.IsNullOrEmpty(deviceDescriptor.Description) ? "No description" : deviceDescriptor.Description;
					int nItemIndex = this.comboSelectDriver.Items.Add(deviceDescriptor);
					if (deviceDescriptor.DeviceID == DeviceID)
						{
						nDefaultIndex = nItemIndex;	// We've found our default item, use it's index
						}
					}
				// Now, the ComboBox is populated. Set the default selection.
				this.comboSelectDriver.SelectedIndex = nDefaultIndex;
				this.m_strDeviceID = ((DeviceDescriptor)comboSelectDriver.Items[nDefaultIndex]).DeviceID;
				this.comboSelectDriver.Enabled = true;	// Enable the control now that it has valid data.
				this.buttonDriverProperties.Enabled = true;
				this.comboSelectDriver.EndUpdate();
				}
			}

		/// <summary>
		/// Gets the root key in the registry beneath which ASCOM device profiles are stored,
		/// as a RegistryKey object.
		/// </summary>
		public static RegistryKey RegistryRoot
		{
			get
			{
				RegistryKey keyRoot = Registry.LocalMachine.OpenSubKey(@"Software\ASCOM");
				if (keyRoot == null)
					keyRoot = Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\ASCOM");
				if (keyRoot == null)
					throw new ApplicationException("Unable to locate ASCOM registry");
				return keyRoot;
			}
		}

		/// <summary>
		/// The value of the most-recently-used DeviceID. Set by the user and used to set the default selection.
		/// </summary>
		private string m_strDeviceID = "";
		/// <summary>
		/// Handles the SelectionChangeCommitted event from the device selector drop-down.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboSelectDriver_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			DeviceDescriptor objDevice = (DeviceDescriptor)comboSelectDriver.SelectedItem;
			this.m_strDeviceID = objDevice.DeviceID;
			m_MRU.MostRecentlyUsedDeviceID = objDevice.DeviceID;
		}
		/// <summary>
		/// This event occurs when the device selection changes.
		/// </summary>
		[Category("ASCOM")]
		[Description("Occurs when the ASCOM device selection changes")]
		public event System.EventHandler SelectionChanged;
		/// <summary>
		/// Raise the <see cref="SelectionChanged"/> event.
		/// </summary>
		protected void OnSelectionChanged()
			{
			if (SelectionChanged != null)
				{
				SelectionChanged(this, EventArgs.Empty);
				}
			}

		/// <summary>
		/// Get or Set the DeviceID.
		/// Set this property prior to suggest a default selection. The chooser will then attempt
		/// to match one of the installed drivers and will preselect it in the combo box if a match is found.
		/// If the supplied DeviceID does not match any installed device, it is ignored.
		/// Read this property to retrieve the user's device selection. A null reference is returned if
		/// there has been no selection, or if there are no installed drivers.
		/// This property is reset when the DeviceClass property is set.
		/// </summary>
		[Category("ASCOM")]
		[Description("The DeviceID that is currently selected. This may be set (or bound to a Setting) to pre-select the default device.")]
		[Browsable(true)]
		public string DeviceID
		{
			get
			{
				return m_strDeviceID;
			}
			set
			{
				m_strDeviceID = value;
				PopulateDeviceList();	// (re)populate the combo box and try to set a default entry.
			}
		}
		/// <summary>
		/// Gets the currently selected ASCOM device as a <see cref="DeviceDescriptor"/>.
		/// If there is no currently selected device, or if the selected item is not a valid ASCOM
		/// device, then the value <c>null</c> is returned.
		/// </summary>
		public DeviceDescriptor SelectedDevice
			{
			get 
				{
				if (!(this.comboSelectDriver.SelectedItem is DeviceDescriptor))
					return null;
				if (string.IsNullOrEmpty((this.comboSelectDriver.SelectedItem as DeviceDescriptor).DeviceID))
					return null;
				return this.comboSelectDriver.SelectedItem as DeviceDescriptor;
				}
			}

		/// <summary>
		/// Format string used to construct the on-screen prompt,
		/// which reflects the currently selected device class.
		/// </summary>
		private const string cstrPromptLabel = "Select your {0} type from the drop-down list. Click the Properties button to adjust settings.";

		private void buttonDriverProperties_Click(object sender, System.EventArgs e)
		{
			// ToDo: Instatiate the selected driver and show its settings dialog.
		}

		private void Chooser_Load(object sender, System.EventArgs e)
		{
			PopulateDeviceList();
		}

		/// <summary>
		/// The most-recently-used devices
		/// </summary>
		private ChooserMRU m_MRU;
	}

	//public enum DriverClass { Telescope, Dome, Camera, Focuser, Filter }
}
