using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ASCOM.FilterWheelSim
{
    class PictureBoxArray : System.Collections.CollectionBase
    {
        private System.Windows.Forms.Form HostForm;

        public PictureBoxArray(Form host)
        {
            HostForm = host;
            //Me.AddNewPictureBox()
        }

        public PictureBox AddNewPictureBox()
        {
            // Create a new instance of the PictureBox class.
            PictureBox aPictureBox = new PictureBox();
            ToolTip aTooltip = new ToolTip();
            // Add the PictureBox to the collection's internal list.
            this.List.Add(aPictureBox);
            // Add the PictureBox to the controls collection of the form 
            // referenced by the HostForm field.
            HostForm.Controls.Add(aPictureBox);
            // Set intial properties for the PictureBox object.
            aPictureBox.Top = this.Count * 25;
            aPictureBox.Left = 100;
            aPictureBox.BorderStyle = BorderStyle.FixedSingle;
            aPictureBox.Tag = this.Count;
            aPictureBox.BackColor = Color.DarkGray;
            aTooltip.SetToolTip(aPictureBox, "Click to select colour");

            aPictureBox.Click += new System.EventHandler(ClickHandler);

            return aPictureBox;
        }

         public PictureBox this[int Index]
        {
            get { return (PictureBox)this.List[Index]; }
        }

        public void Remove()
        {
            // Check to be sure there is a PictureBox to remove.
            if (this.Count >= 1)
            {
                // Remove the last PictureBox added to the array from the host form 
                // controls collection. Note the use of the default property in 
                // accessing the array.
                HostForm.Controls.Remove(this[this.Count - 1]);
                this.List.RemoveAt(this.Count - 1);
            }
        }

        // When the picturebox is clicked show a colour picker dialog
        // set initially to the current colour. If OK clicked on colour picker
        // set the current picture box background to the selected colour
        public void ClickHandler(Object sender, System.EventArgs e)
        {
            ColorDialog colourDiag = new ColorDialog();
            colourDiag.AllowFullOpen = true;
            colourDiag.AnyColor = true;
            colourDiag.Color = ((PictureBox)sender).BackColor;
            if (colourDiag.ShowDialog() == DialogResult.OK)
            ((PictureBox)sender).BackColor = colourDiag.Color;
        }

    }
}
