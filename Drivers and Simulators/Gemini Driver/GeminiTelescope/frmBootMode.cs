using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.GeminiTelescope
{
    public partial class frmBootMode : Form
    {
        public frmBootMode()
        {
            InitializeComponent();
        }

        private void frmBootMode_Load(object sender, EventArgs e)
        {
            SharedResources.SetTopWindow(this);
        }

        public GeminiHardware.GeminiBootMode BootMode
        {
            get
            {
                if (radioButton1.Checked)
                    return GeminiHardware.GeminiBootMode.WarmRestart;
                else if (radioButton2.Checked)
                    return GeminiHardware.GeminiBootMode.WarmStart;
                else if (radioButton3.Checked)                
                    return GeminiHardware.GeminiBootMode.ColdStart;

                return GeminiHardware.GeminiBootMode.Prompt;
            }
            set
            {
                switch (value)
                {
                    case GeminiHardware.GeminiBootMode.ColdStart :
                        radioButton3.Checked = true;
                        break;
                    case GeminiHardware.GeminiBootMode.WarmStart:
                        radioButton2.Checked = true;
                        break;
                    case GeminiHardware.GeminiBootMode.WarmRestart:
                        radioButton1.Checked = true;
                        break;
                }
            }
        }

        private void btnBoot_Click(object sender, EventArgs e)
        {
            if (BootMode == GeminiHardware.GeminiBootMode.Prompt)
                return; // nothing selected!

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

       

    }
}
