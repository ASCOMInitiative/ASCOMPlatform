using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pyxis_Rotator_Control
{
    public partial class AdvancedSettingsForm : Form
    {
        public AdvancedSettingsForm()
        {
            InitializeComponent();
        }

        private void AdvancedSettingsFormcs_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = new PyxisPropertyGrid();
        }

        private void RestoreDefaults_BTN_Click(object sender, EventArgs e)
        {
            OptecPyxis.RestoreDeviceDefaults();
        }
    }
}
