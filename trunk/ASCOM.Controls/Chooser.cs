using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using ASCOM.DriverAccess;
using Microsoft.Win32;

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
		/// Private class used by the chooser to create a collection of devices.
		/// Used to populate the device selection combo-box.
		/// </summary>
		private class DeviceDescriptor
			{
			/// <summary>
			/// Initialize a new DeviceDescriptor object.
			/// </summary>
			/// <param name="s1">DeviceID of the device (Device ID)</param>
			/// <param name="s2">Human-readable description</param>
			public DeviceDescriptor(string s1, string s2)
				{
				DeviceID = s1;
				Description = s2;
				}
			/// <summary>
			/// Gets or sets the ASCOM DeviceID.
			/// </summary>
			/// <value>The ASCOM DeviceID (synonymous with the COM ClsId) of the device.</value>
			public string DeviceID { get; set; }
			/// <summary>
			/// Gets or sets the description of the ASCOM device.
			/// </summary>
			/// <value>The description.</value>
			public string Description { get; set; }
			/// <summary>
			/// Returns a <see cref="T:System.String"/> containing a description of the ASCOM device.
			/// </summary>
			/// <returns>
			/// A <see cref="T:System.String"/> containing a description of the ASCOM device.
			/// </returns>
			public override string ToString()
				{
				return Description;
				}
			}
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
		[Category("ASCOM"), Description("Configures the chooser for a particular class of device."),
			Browsable(true), DefaultValue("Telescope")]
		public string DeviceClass
		{
			get
			{
				return m_eDriverClass;
			}
			set
			{
				m_eDriverClass = value;
				m_MRU.MRUType = value;
				this.DeviceID = m_MRU.MostRecentlyUsedDeviceID;	// This repopulates the device list.
				this.labelPrompt.Text = string.Format(cstrPromptLabel, DeviceClass.ToString());
			}
		}

		/// <summary>
		/// The class of device being chosen. Defaults to "Telescope".
		/// </summary>
		private string m_eDriverClass = "Telescope";
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
		/// Populates the device list by enumerating the registry entries for devices in the given DriverClass.
		/// The DriverClass comes from the DeviceClass property.
		/// </summary>
		internal void PopulateDeviceList()
		{
			if (this.DesignMode)
				return;

			string strDeviceClass = this.DeviceClass.ToString();
			string strSubKey = strDeviceClass + " Drivers";
			RegistryKey keyASCOMRoot = RegistryRoot;
			RegistryKey keyDriverRoot = keyASCOMRoot.OpenSubKey(strSubKey, false);
			if (keyDriverRoot == null)
			{	// There is no registry key and therefore no drivers of this type are registered.
				// Populate the combo box with a dummy entry and disable the control.
				// The entry will read "no %DeviceClass% drivers installed".
				this.m_strDeviceID = null;	// Ensure we don't return an obsolete device ID.
				this.comboSelectDriver.BeginUpdate();
				this.comboSelectDriver.Items.Clear();
				string strDescription = string.Format("(no {0} drivers installed)", strDeviceClass);
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
			foreach (string strProgID in keyDriverRoot.GetSubKeyNames())
			{
				RegistryKey keyDriver = keyDriverRoot.OpenSubKey(strProgID, false);
				string strDescription = (string)keyDriver.GetValue(null, "No description");
				DeviceDescriptor objDevice = new DeviceDescriptor(strProgID, strDescription);
				int nItemIndex = this.comboSelectDriver.Items.Add(objDevice);
				if (strProgID == DeviceID)
				{
					nDefaultIndex = nItemIndex;	// We've found our default item, use it's index
				}
				keyDriver.Close();
			}
			// Now, the ComboBox is populated. Set the default selection.
			this.comboSelectDriver.SelectedIndex = nDefaultIndex;
			this.m_strDeviceID = ((DeviceDescriptor)comboSelectDriver.Items[nDefaultIndex]).DeviceID;
			this.comboSelectDriver.Enabled = true;	// Enable the control now that it has valid data.
			this.buttonDriverProperties.Enabled = true;
			this.comboSelectDriver.EndUpdate();
			keyDriverRoot.Close();
			keyASCOMRoot.Close();
		}

		/// <summary>
		/// Gets the root key in the registry beneath which ASCOM device profiles are stored,
		/// as a RegistryKey object.
		/// </summary>
		public static RegistryKey RegistryRoot
		{
			get
			{
				RegistryKey keyRoot = Registry.LocalMachine;	// HKEY_LOCAL_MACHINE
				keyRoot = keyRoot.OpenSubKey(@"Software\ASCOM", false);
				return keyRoot;
			}
		}

		/// <summary>
		/// The value of the most-recently-used DeviceID. Set by the user and used to set the default selection.
		/// </summary>
		private string m_strDeviceID = "";

		private void comboSelectDriver_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			DeviceDescriptor objDevice = (DeviceDescriptor)comboSelectDriver.SelectedItem;
			this.m_strDeviceID = objDevice.DeviceID;
			m_MRU.MostRecentlyUsedDeviceID = objDevice.DeviceID;
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
		/// Format string used to construct the on-screen prompt,
		/// which reflects the currently selected device class.
		/// </summary>
		private const string cstrPromptLabel = "Select your {0} type from the drop-down list. Click the Properties button to adjust settings.";

		private void buttonDriverProperties_Click(object sender, System.EventArgs e)
		{
			Telescope objDriver = new Telescope(this.DeviceID);
			//LateBoundDriver objDriver = new LateBoundDriver(this.DeviceID);	// Create a late-bound driver instance
			objDriver.SetupDialog();
			// TODO: Invoke Dispose() if possible, but silently succeed if not.
			objDriver = null;
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
