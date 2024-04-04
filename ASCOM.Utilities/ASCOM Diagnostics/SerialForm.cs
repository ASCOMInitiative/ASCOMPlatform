using System;
using Microsoft.VisualBasic;

namespace ASCOM.Utilities
{
    public partial class SerialForm
    {
        /// <summary>
        /// Serial form initialiser
        /// </summary>
        public SerialForm()
        {
            InitializeComponent();
        }
        private void SerialForm_Load(object sender, EventArgs e)
        {
            var SerPort = new Serial();
            string[] Ports;
            Ports = SerPort.AvailableCOMPorts;
            SerPort.Dispose();
            foreach (string Port in Ports)
                lstSerialASCOM.Items.Add(Interaction.IIf(string.IsNullOrEmpty(Port), "Bad value - Null or empty COM port name!", Port));

        }

        private void btnSerialExit_Click(object sender, EventArgs e)
        {
            Visible = false;
            Close();
        }
    }
}