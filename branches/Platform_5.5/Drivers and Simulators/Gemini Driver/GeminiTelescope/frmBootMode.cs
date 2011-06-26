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

        public GeminiHardwareBase.GeminiBootMode BootMode
        {
            get
            {
                if (radioButton1.Checked)
                    return GeminiHardwareBase.GeminiBootMode.WarmRestart;
                else if (radioButton2.Checked)
                    return GeminiHardwareBase.GeminiBootMode.WarmStart;
                else if (radioButton3.Checked)                
                    return GeminiHardwareBase.GeminiBootMode.ColdStart;

                return GeminiHardwareBase.GeminiBootMode.Prompt;
            }
            set
            {
                switch (value)
                {
                    case GeminiHardwareBase.GeminiBootMode.ColdStart :
                        radioButton3.Checked = true;
                        break;
                    case GeminiHardwareBase.GeminiBootMode.WarmStart:
                        radioButton2.Checked = true;
                        break;
                    case GeminiHardwareBase.GeminiBootMode.WarmRestart:
                        radioButton1.Checked = true;
                        break;
                }
            }
        }

        private void btnBoot_Click(object sender, EventArgs e)
        {
            if (BootMode == GeminiHardwareBase.GeminiBootMode.Prompt)
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
