using System;
using System.Windows.Forms;
using static ASCOM.Utilities.Global;

namespace ASCOM.Utilities
{
    public partial class CheckedMessageBox
    {
        /// <summary>
        /// Create a checked message box
        /// </summary>
        public CheckedMessageBox()
        {

            // This call is required by the designer.
            InitializeComponent();

            // Initialise the state of the suppress dialogue checkbox
            ChkDoNotShowAgain.Checked = GetBool(SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE, SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE_DEFAULT);
            CenterToParent();

        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ChkDoNotShowAgain_CheckedChanged(object sender, EventArgs e)
        {
            // The checkbox has been clicked so record the new value
            SetName(SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE, ChkDoNotShowAgain.Checked.ToString());
        }
    }
}