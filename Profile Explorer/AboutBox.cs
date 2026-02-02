using System;
using System.Reflection;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    public sealed partial class AboutBox
    {
        /// <summary>
        /// Initialise about box class
        /// </summary>
        public AboutBox()
        {
            InitializeComponent();
        }

        private void AboutBox1_Load(object sender, EventArgs e)
        {
            // Set the title of the form.
            Text = $"Profile Explorer";

            LabelProductName.Text = Application.ProductName;
            LabelVersion.Text = Application.ProductVersion;
            LabelCopyright.Text = "Copyright (c) Peter Simpson 2026;";
            LabelCompanyName.Text = "ASCOM Initiative";
            TextBoxDescription.Text = "Profile Explorer enables you to edit the ASCOM profile directly through a graphical interface.";
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}