using System.Windows.Forms;
using ASCOM.DeviceInterface;

namespace ASCOM.Simulator
{
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
        }

        private void Timer1Tick(object sender, System.EventArgs e)
        {
            vuMeter1.Level = trackBar1.Value;
        }
    }
}