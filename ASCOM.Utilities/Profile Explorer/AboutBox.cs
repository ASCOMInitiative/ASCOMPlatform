using System;
using System.Reflection;

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
            string ApplicationTitle;
            ApplicationTitle = Assembly.GetExecutingAssembly().FullName;
            Text = string.Format("About {0}", ApplicationTitle);
            // Initialize all of the text displayed on the About Box.
            // TODO: Customize the application's assembly information in the "Application" pane of the project 
            // properties dialog (under the "Project" menu).
            LabelProductName.Text = "";
            LabelVersion.Text = "";
            LabelCopyright.Text = "";
            LabelCompanyName.Text = "";
            TextBoxDescription.Text = "";
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}