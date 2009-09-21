using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Optec
{
    public partial class OffsetsForm : Form
    {
        public OffsetsForm()
        {
            InitializeComponent();
        }
        public OffsetsForm(int currentPos)
        {
            InitializeComponent();
            PortNumber_CB.SelectedIndex = currentPos;
        }

        private void OK_Btn_Click(object sender, EventArgs e)
        {

        }

  
    }
}
