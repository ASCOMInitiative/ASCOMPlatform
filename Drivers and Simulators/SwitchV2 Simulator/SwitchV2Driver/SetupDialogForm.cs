using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.Simulator;
using System.Diagnostics;

namespace ASCOM.Simulator
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile 
            chkTrace.Checked = Switch.traceState;
            // get a copy of the switches for editing
            dataGridViewSwitches.Rows.Clear();
            var i = 0;
            foreach (var item in Switch.switches)
            {
                dataGridViewSwitches.Rows.Add(i, item.Name, item.Description, item.Value, item.Minimum, item.Maximum, item.StepSize, item.CanWrite);
                i++;
            }
            checkBoxSetupSimulator_CheckedChanged(null, null);
            FileVersionInfo FV = Process.GetCurrentProcess().MainModule.FileVersionInfo; //Get the name of the executable without path or file extension
            labelVersion.Text = "Version: " + FV.FileVersion;
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here

            Switch.traceState = chkTrace.Checked;
            Switch.switches.Clear();
            // update the switch list
            foreach (DataGridViewRow row in dataGridViewSwitches.Rows)
            {
                if (row.IsNewRow)
                    continue;
                string reason;
                if (LocalSwitch.IsValid(row.Cells, out reason))
                {
                    Switch.switches.Add(new LocalSwitch(row.Cells));
                }
                else
                {
                    row.ErrorText = "Invalid row contents: " + reason;
                }
            }
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

        private void InitRow(DataGridViewCellCollection cells)
        {
            // init the switch as a boolean read/write switch
            InitCell(cells["colMin"], "0");
            InitCell(cells["colMax"], "1");
            InitCell(cells["colStep"], "1");
            InitCell(cells["colCanWrite"], true);
            InitCell(cells["colValue"], "0");
        }

        private void InitCell(DataGridViewCell cell, object obj)
        {
            if (cell.Value == null)
            {
                cell.Value = obj;
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
            var n = dataGridViewSwitches.Rows.Count;
            for (int i = 0; i < n; i++)
            {
                if (i < this.dataGridViewSwitches.RowCount)
                {
                    if (dataGridViewSwitches.Rows[i].IsNewRow)
                    {
                        continue;
                    }
                    this.dataGridViewSwitches.Rows[i].Cells["colId"].Value = i.ToString();
                    InitRow(this.dataGridViewSwitches.Rows[i].Cells);
                }
            }
        }

        private void dataGridViewSwitches_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var row = dataGridViewSwitches.Rows[e.RowIndex];
            if (row.IsNewRow) return;
            string reason;
            if (LocalSwitch.IsValid(row.Cells, out reason))
            {
                row.ErrorText = "";
            }
            else
            {
                row.ErrorText = "Switch parameters invalid: " + reason;
                e.Cancel = true;
            }
        }
        #endregion

        private void checkBoxSetupSimulator_CheckedChanged(object sender, EventArgs e)
        {
            dataGridViewSwitches.AllowUserToAddRows = checkBoxSetupSimulator.Checked;

            colCanWrite.DefaultCellStyle.BackColor =
            colMin.DefaultCellStyle.BackColor =
            colMax.DefaultCellStyle.BackColor =
            colStep.DefaultCellStyle.BackColor = checkBoxSetupSimulator.Checked ? switchName.DefaultCellStyle.BackColor : SystemColors.Control;

            colMin.ReadOnly =
            colMax.ReadOnly =
            colStep.ReadOnly =
            colCanWrite.ReadOnly = !checkBoxSetupSimulator.Checked;

            dataGridViewSwitches.Invalidate();
            dataGridViewSwitches.Update();
        }
    }
}