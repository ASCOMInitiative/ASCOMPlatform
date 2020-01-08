using ASCOM.DynamicRemoteClients;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.DynamicRemoteClients
{
    /// <summary>
    /// Create a checked list box where item backgrounds can be coloured as required, otherwise behaves as a normal checked list box
    /// </summary>
    public class ColouredCheckedListBox : CheckedListBox
    {

        /// <summary>
        /// Class initialiser
        /// </summary>
        public ColouredCheckedListBox()
        {
            DoubleBuffered = true;
        }

        /// <summary>
        /// Override the draw event so that the background colour can bet set
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Color backgroundColor = e.BackColor; // Initialise the background colour to the supplied default
            if (((DynamicDriverRegistration)this.Items[e.Index]).InstallState != InstallationState.Ok)
            {
                backgroundColor = Color.LightPink;
            }

            // Call the base class with modified background colour so that any attached delegates will fire
            base.OnDrawItem(new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State, e.ForeColor, backgroundColor));
        }
    }
}
