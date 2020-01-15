using System.Windows.Forms;

namespace ASCOM.DynamicRemoteClients
{
    public partial class LocalServerForm : Form
    {
        delegate void SetTextCallback(string text);

        public LocalServerForm()
        {
            InitializeComponent();
        }

    }
}