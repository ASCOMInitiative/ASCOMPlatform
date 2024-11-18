using ASCOM.Utilities;
using System;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    /// <summary>
    /// Create device form
    /// </summary>
    public partial class GetDeviceDescriptionForm : Form
    {
        private readonly TraceLogger TL;

        /// <summary>
        /// The device description entered by the user
        /// </summary>
        public string Descrption;

        /// <summary>
        /// Base initialiser for the create client form
        /// </summary>
        public GetDeviceDescriptionForm()
        {
            InitializeComponent();

            // Initialise to a default value that can be checked by the Chooser on return from the form
            Descrption = null;
        }

        /// <summary>
        /// Create the form using a supplied TraceLogger
        /// </summary>
        /// <param name="traceLogger">The logger to use.</param>
        public GetDeviceDescriptionForm(TraceLogger traceLogger) : this()
        {
            TL = traceLogger;
            LogMessage($"GetDescriptionForm initialised");
        }

        /// <summary>
        /// OK button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            // Check whether there is a device description
            if (!string.IsNullOrEmpty(TxtDeviceDescription.Text)) // There is a device description
            {
                Descrption = TxtDeviceDescription.Text;
                LogMessage($"Device description entered by the user: {Descrption}");
                DialogResult = DialogResult.OK;
                Close();
            }
            else // There is no device description
            {
                LogMessage($"The user pressed the OK button with an empty description.");
            }
        }

        /// <summary>
        /// Cancel button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            LogMessage($"The user pressed the Cancel button, so close the form with no value.");
            Close(); // Close the application
        }

        private void LogMessage(string message)
        {
            TL?.LogMessage("GetDeviceDescriptionForm", message);
        }
    }
}