using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.Simulator;

namespace ASCOM.Simulator
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private List<LocalSwitch> ls;

        public SetupDialogForm()
        {
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile 
            chkTrace.Checked = Switch.traceState;
            ls = new List<LocalSwitch>();
            // get a copy of the switches for editing
            ls.AddRange(Switch.switches);
            var bl = new BindingList<LocalSwitch>(ls);
            dataGridViewSwitches.DataSource = bl;
            //dataGridViewSwitches.Rows.Clear();
            //foreach (var item in Switch.switches)
            //{
            //    dataGridViewSwitches.Rows.Add(item.Name, item.Minimum, item.Maximum, item.StepSize, item.ReadOnly, item.Value);
            //}
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here

            Switch.traceState = chkTrace.Checked;
            Switch.switches.Clear();
            // update the switch list
            Switch.switches.AddRange(ls);
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
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

        #region DataGridView Validation

        private void dataGridViewSwitches_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox itemID = e.Control as TextBox;
            if (itemID == null) return;

            e.Control.KeyPress -= itemID_NumKeyPress;

            switch (dataGridViewSwitches.CurrentCell.OwningColumn.Name)
            {
                case "colMin":
                case "colMax":
                case "colStep":
                case "colValue":
                    itemID.KeyPress += itemID_NumKeyPress;
                    break;
            }
        }

        /// <summary>
        /// only allow numeric characters in the cells intended to hold numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemID_NumKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// checks that the numeric fields really contain a number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewSwitches_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewSwitches.Rows[e.RowIndex].IsNewRow) return;

            switch (dataGridViewSwitches.Columns[e.ColumnIndex].Name)
            {
                case "colMin":
                case "colMax":
                case "colStep":
                case "colValue":
                    double d;
                    if (!double.TryParse(e.FormattedValue.ToString(), out d))
                    {
                        e.Cancel = true;
                    }
                    break;
            }
        }

        private void dataGridViewSwitches_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateRowId();
        }

        private void dataGridViewSwitches_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateRowId();
        }

        /// <summary>
        /// update the "id" column with the switch number
        /// </summary>
        private void UpdateRowId()
        {
            var n = ((BindingList<LocalSwitch>)dataGridViewSwitches.DataSource).Count;
            for (int i = 0; i < n; i++)
            {
                if (i < this.dataGridViewSwitches.RowCount)
                {
                    if (dataGridViewSwitches.Rows[i].IsNewRow)
                    {
                        continue;
                    }
                    this.dataGridViewSwitches.Rows[i].Cells["colId"].Value = i.ToString();
                }
            }
        }

        private void dataGridViewSwitches_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var row = dataGridViewSwitches.Rows[e.RowIndex];
            if (row.IsNewRow) return;
 
            var ls = row.DataBoundItem as LocalSwitch;
            if (ls == null) return;
            string reason;
            if (ls.IsValid(out reason))
            {
                row.ErrorText = "";
            }
            else
            {
                e.Cancel = true;
                row.ErrorText = "Switch parameters invalid: " + reason;
            }
        }
        #endregion
    }
}