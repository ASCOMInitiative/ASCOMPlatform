using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    class TextBoxArray : System.Collections.CollectionBase
    {

        const string validOffsetCharacters = "0123456789+-\b"; // List of valid character in an Offset text box: 0 to 9, plus, minus and backspace
        private Form HostForm;

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
            {
                aTextBox.KeyPress += new KeyPressEventHandler(KeyPressHandler);
                aTextBox.Validating += ATextBox_Validating;
            }
            return aTextBox;
        }

        private void ATextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!IsInt(textBox.Text))
            {
                MessageBox.Show($"The offset '{textBox.Text}' is not a valid integer, please correct it.", "Invalid Offset", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                e.Cancel = true;
            }
        }

        public static bool IsInt(string text)
        {
            return int.TryParse(text, out _);
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

        /// <summary>
        /// Validate  key presses to restrict input to the keys: 0 to 9, plus, minus and backspace.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void KeyPressHandler(Object sender, KeyPressEventArgs e)
        {
            // Validate keys as they are pushed.
            if (!validOffsetCharacters.Contains(e.KeyChar.ToString())) e.Handled = true; // Character is not valid so set handled true to prevent the character being added to the text box.
        }
    }
}
