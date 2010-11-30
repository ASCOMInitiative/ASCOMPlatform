using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PyxisAPI;

namespace Pyxis_Rotator_Control
{
    public partial class AdvancedSettingsForm : Form
    {
        OptecPyxis myPyxis;
        public AdvancedSettingsForm(OptecPyxis somePyxis)
        {
            InitializeComponent();
            myPyxis = somePyxis;
        }

        private void AdvancedSettingsFormcs_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = new PyxisPropertyGrid(myPyxis);
        }

        private void RestoreDefaults_BTN_Click(object sender, EventArgs e)
        {
            myPyxis.RestoreDeviceDefaults();
        }
    }
}
