using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Optec
{
    internal partial class UpToDateForm : Form
    {
        public UpToDateForm(Icon IconImg)
        {
            InitializeComponent();
            this.Icon = IconImg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
