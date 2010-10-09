using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using ASCOM.DeviceInterface;

namespace ASCOM.Simulator
{
    public partial class SetupDialogForm : Form
    {
        private static readonly IFocuser Focuser = new Focuser();
        Assembly _assembly;

        public SetupDialogForm()
        {
            InitializeComponent();
        }
    }
}
