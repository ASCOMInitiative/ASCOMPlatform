using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.FilterWheelSim
{
    class TextBoxArray : System.Collections.CollectionBase
    {
        private  Form HostForm;

        public TextBoxArray(Form host)
        {
            HostForm = host;
            //Me.AddNewTextBox()
        }

        public TextBox AddNewTextBox(bool bOffsetBox)
        {
            // Create a new instance of the TextBox class.
            TextBox aTextBox = new TextBox();
            // Add the TextBox to the collection's internal list.
            this.List.Add(aTextBox);
            // Add the TextBox to the controls collection of the form 
            // referenced by the HostForm field.
            HostForm.Controls.Add(aTextBox);
            // Set intial properties for the TextBox object.
            aTextBox.Top = this.Count * 25;
            aTextBox.Left = 100;
            aTextBox.Tag = this.Count;
            aTextBox.Text = "TextBox " + this.Count;

            if (bOffsetBox)
                aTextBox.KeyPress += new KeyPressEventHandler(KeyPressHandler);

            return aTextBox;
        }

        public TextBox this[int Index]
        {
            get { return (TextBox)this.List[Index]; }
        }

        public void Remove()
        {
            // Check to be sure there is a textbox to remove.
            if (this.Count >= 1)
            {
                // Remove the last textbox added to the array from the host form 
                // controls collection. Note the use of the default property in 
                // accessing the array.
                HostForm.Controls.Remove(this[this.Count - 1]);
                this.List.RemoveAt(this.Count - 1);
            }
        }

        public void KeyPressHandler(Object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < '0' || e.KeyChar > '9')
                if (e.KeyChar != '\b')  e.Handled = true;

        }


    }
}
