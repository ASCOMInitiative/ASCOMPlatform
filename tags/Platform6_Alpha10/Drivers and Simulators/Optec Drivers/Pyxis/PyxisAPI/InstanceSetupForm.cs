using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PyxisAPI
{
    public partial class InstanceSetupForm : Form
    {
        private OptecPyxis myPyxis;
        private Form setupDialogForm;

        public InstanceSetupForm(OptecPyxis myPyxis)
        {
            InitializeComponent();
            this.myPyxis = myPyxis;
        }

        public InstanceSetupForm(OptecPyxis myPyxis, Form setupDialogForm)
        {
            InitializeComponent();
            SetupDialogBtn.Visible = true;
            this.myPyxis = myPyxis;
            this.setupDialogForm = setupDialogForm;
        }

        private void InstanceSetupForm_Load(object sender, EventArgs e)
        {
            listBox1.DataSource = myPyxis.SettingsInstanceNames;
            CurrentInstanceLBL.Text = myPyxis.CurrentInstance;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addBtn.Enabled = false;
            addNowBtn.Visible = true;
            newNameTb.Visible = true;
            newNameTb.Text = "";
            newNameTb.Focus();
        }

        private void addNowBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                myPyxis.CreateNewInstance(newNameTb.Text);
                myPyxis.ReloadSettings(myPyxis.DeviceType);

                listBox1.DataSource = null;
                listBox1.DataSource = myPyxis.SettingsInstanceNames;
                listBox1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                addBtn.Enabled = true;
                addNowBtn.Visible = false;
                newNameTb.Visible = false;
            }
            

        }

        private void oKBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DialogResult rlt = MessageBox.Show("Are you sure you want to remove the settings for the instance named " +
                        listBox1.SelectedItem.ToString() + "?", "Remove Instance?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rlt == System.Windows.Forms.DialogResult.Yes)
                {
                    myPyxis.DeleteSettingsInstance(listBox1.SelectedItem.ToString());
                    listBox1.DataSource = null;
                    listBox1.DataSource = myPyxis.SettingsInstanceNames;
                    listBox1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                updateBtn.Visible = true;
                editNameTb.Text = listBox1.SelectedItem.ToString();
                editNameTb.Visible = true;
                editBtn.Visible = false;
                editNameTb.Focus();
                editNameTb.SelectAll();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                myPyxis.RenameInstance(listBox1.SelectedItem.ToString(), editNameTb.Text);
                listBox1.DataSource = null;
                listBox1.DataSource = myPyxis.SettingsInstanceNames;
                listBox1.Refresh();
                CurrentInstanceLBL.Text = myPyxis.CurrentInstance;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                updateBtn.Visible = false;
                editNameTb.Visible = false;
                editBtn.Visible = true;
                OkBtn.Focus();
                this.Cursor = Cursors.Default;
            }
        }

        private void SetupDialogBtn_Click(object sender, EventArgs e)
        {
            setupDialogForm.ShowDialog();   
        }

        private void SelectInstanceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (myPyxis.CurrentDeviceState != OptecPyxis.DeviceStates.Disconnected)
                {
                    DialogResult result = MessageBox.Show("You can not change the current instance while the device is connected. Would "
                        + "you like to disconnect now?", "Disconnect?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                        myPyxis.Disconnect();
                    else return;
                }
                myPyxis.CurrentInstance = listBox1.SelectedItem.ToString();
                myPyxis.ReloadSettings(myPyxis.DeviceType);

                // Update the display box for the new current instance
                CurrentInstanceLBL.Text = myPyxis.CurrentInstance;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void newNameTb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                addNowBtn_Click(this, EventArgs.Empty);
            }
        }

        private void editNameTb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                updateBtn_Click(this, EventArgs.Empty);
            }
        }

        
    }
}
