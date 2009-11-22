using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.Interface;
using ASCOM.DriverAccess;
using System.Collections.Specialized;

namespace TestFocus
{
    public partial class FormFocuser : Form
    {
        private IFocuser focuser;
        private String ID = "ASCOM.ArduinoFocus.Focuser";

        private BindingList<FocusPosition> focusPositions = new BindingList<FocusPosition>();

        public FormFocuser()
        {
            InitializeComponent();
            comboBoxPositions.DataSource = focusPositions;
            comboBoxPositions.DisplayMember = "Name";
            comboBoxPositions.ValueMember = "Position";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ID = ASCOM.DriverAccess.Focuser.Choose(ID);
        }

        object objFocuserLateBound;
        ASCOM.Interface.IFocuser IFocuser;
        Type objTypeFocuser;
        private void button2_Click(object sender, EventArgs e)
        {
            if (focuser == null)
            {
                // Get Type Information 
                objTypeFocuser = Type.GetTypeFromProgID(ID);
                string driverName = textBoxDriverName.Text;
                if (driverName.Length > 0)
                {
                    // Create an instance of the Focuser object
                    object[] name = {driverName};
                    objFocuserLateBound = Activator.CreateInstance(objTypeFocuser, name);

                    // Try to see if this driver has an ASCOM.Focuser interface
                    try
                    {
                        focuser = (ASCOM.Interface.IFocuser)objFocuserLateBound;
                    }
                    catch (Exception)
                    {
                        focuser = null;
                    }
                }
                else
                {
                    focuser = new Focuser(ID);
                }
            
                focuser.Link = true;
            }
            else
            {
                focuser.Link = false;
                focuser = null;
            }
            if (focuser != null && focuser.Link)
            {
                if (Properties.Settings.Default.Position != null &&
                    Properties.Settings.Default.FocuserID == ID)
                {
                    textBoxPosition.Text = Properties.Settings.Default.Position;
                    //focuser.Position = Convert.ToInt32(Properties.Settings.Default.Position);
                }
                button2.Text = "Disconnect";
                timer1.Enabled = focuser.Link;
            }
            else
            {
                button2.Text = "Connect";
                timer1.Enabled = false;
            }
        }

        private bool lockPos = false;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (focuser != null && focuser.Link)
            {
                if (!lockPos)
                {
                    textBoxPosition.Text = focuser.Position.ToString();
                    if (focuser.IsMoving)
                        textBoxPosition.BackColor = Color.Pink;
                    else
                        textBoxPosition.BackColor = Color.LightGreen;
                }
            }
        }

        private void buttonIn_Click(object sender, EventArgs e)
        {
            if (focuser != null && focuser.Link)
            {
                try
                {
                    int pos = focuser.Position;
                    pos -= Convert.ToInt32(comboBoxSteps.SelectedItem);
                    focuser.Move(pos);
                }
                finally
                {
                }
            }
        }

        private void buttonOut_Click(object sender, EventArgs e)
        {
            if (focuser != null && focuser.Link)
            {
                try
                {
                    int pos = focuser.Position;
                    pos += Convert.ToInt32(comboBoxSteps.SelectedItem);
                    focuser.Move(pos);
                }
                finally
                {
                }
            }

        }

        private void buttonSetPosition_Click(object sender, EventArgs e)
        {
            if (focuser != null && focuser.Link)
            {
                try
                {
                    int pos = Convert.ToInt32(textBoxTarget.Text);
                    focuser.Move(pos);
                }
                finally
                {
                    lockPos = false;
                }
            }

        }

        private void textBoxPosition_KeyPress(object sender, KeyPressEventArgs e)
        {
            //lockPos = true;
            //textBoxPosition.BackColor = Color.FromKnownColor(KnownColor.ControlText);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (focuser != null && focuser.Link)
            {
                try
                {
                    focuser.Halt();
                }
                finally
                {
                    lockPos = false;
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FocuserID != null)
                ID = Properties.Settings.Default.FocuserID;
            if (Properties.Settings.Default.Steps != null)
                comboBoxSteps.SelectedItem = Properties.Settings.Default.Steps;
            else
                comboBoxSteps.SelectedItem = 200;
            if (Properties.Settings.Default.Position != null)
                textBoxPosition.Text = Properties.Settings.Default.Position;
            if (Properties.Settings.Default.FocusPositions != null)
            {
                focusPositions.Clear();
                foreach (var item in Properties.Settings.Default.FocusPositions)
                {
                    focusPositions.Add(new FocusPosition(item));
                }
            }
            try
            {
                FocusPosition fp = new FocusPosition(Properties.Settings.Default.CurrentItem);
                if (focusPositions.Contains(fp))
                    comboBoxPositions.SelectedItem = fp;
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Steps = (string)comboBoxSteps.SelectedItem;
            Properties.Settings.Default.FocuserID = ID;
            Properties.Settings.Default.Position = textBoxPosition.Text;
            Properties.Settings.Default.FocusPositions = new StringCollection();
            foreach (var item in focusPositions)
            {
                Properties.Settings.Default.FocusPositions.Add(item.ToString());
            }
            FocusPosition pos = new FocusPosition(comboBoxPositions.Text, textBoxPosition.Text);
            Properties.Settings.Default.CurrentItem = pos.ToString();
            Properties.Settings.Default.Save();
        }

        private void buttonSync_Click(object sender, EventArgs e)
        {
            if (focuser != null && focuser.Link)
            {
                try
                {
                    int pos = Convert.ToInt32(textBoxPosition.Text);
                    //focuser.Position = pos;
                }
                finally
                {
                    lockPos = false;
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FocusPosition fp = new FocusPosition(comboBoxPositions.Text, textBoxPosition.Text);
            if (!focusPositions.Contains(fp))
                focusPositions.Add(fp);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (comboBoxPositions.SelectedItem != null)
            {
                focusPositions.Remove((FocusPosition)(comboBoxPositions.SelectedItem));
            }
        }

        private void comboBoxPositions_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxPositions.SelectedValue != null)
            {
                var val = (FocusPosition)comboBoxPositions.SelectedItem;
                textBoxTarget.Text = val.Position.ToString();
            }
        }


    }

    internal struct FocusPosition
    {
        private string name;
        private int position;

        public FocusPosition(string name, int position)
        {
            this.name = name;
            this.position = position;
        }

        public FocusPosition(string name, string position)
            : this(name, Convert.ToInt32(position)) { }
            
        public FocusPosition(string nameAndPosition)
        {
            string[] strs = nameAndPosition.Split('|');
            this.name = strs[0];
            this.position = Convert.ToInt32(strs[1]);
        }
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        public int Position
        {
            get
            {
                return this.position;
            }
        }
        public override string ToString()
        {
            return String.Format("{0}|{1}", this.name,this.position);
        }
    }
}
