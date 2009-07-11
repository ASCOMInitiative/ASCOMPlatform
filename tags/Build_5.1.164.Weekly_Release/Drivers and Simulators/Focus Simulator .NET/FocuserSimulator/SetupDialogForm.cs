using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.FocuserSimulator
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
            Properties.Settings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Default_PropertyChanged);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            Properties.Settings.Default.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(Default_PropertyChanged);
            Dispose();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            Properties.Settings.Default.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(Default_PropertyChanged);
            Dispose();
        }

        private void BrowseToAscom(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            HelpTempComp.Text = "If probed temperature is between " + TempMini.Value.ToString() + "° and " +
                TempMaxi.Value.ToString() + "°, then the focuser will move forward by " +
                Properties.Settings.Default.sStepPerDeg.ToString() + " steps each time the t° increases by 1° (and move backwards if t° decreases). " +
                "Temperature is checked every " + Properties.Settings.Default.sTempCompPeriod.ToString() + " seconds.";
        }

        private void IsTemperature_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.sTempCompAvailable = Properties.Settings.Default.sIsTemperature;
            Properties.Settings.Default.sTempComp = Properties.Settings.Default.sIsTemperature;
        }

        private void IsTempCompAvailable_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.sTempComp = Properties.Settings.Default.sTempCompAvailable;
        }

        private void TempComp_TextChanged(object sender, EventArgs e)
        {
            
        }

        void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            HelpTempComp.Text = "If probed temperature is between " + TempMini.Value.ToString() + "° and " +
                TempMaxi.Value.ToString() + "°, then the focuser will move forward by " +
                Properties.Settings.Default.sStepPerDeg.ToString() + " steps each time the t° increases by 1° (and move backwards if t° decreases). " +
                "Temperature is checked every " + Properties.Settings.Default.sTempCompPeriod.ToString() + " seconds.";

        }

    }
}