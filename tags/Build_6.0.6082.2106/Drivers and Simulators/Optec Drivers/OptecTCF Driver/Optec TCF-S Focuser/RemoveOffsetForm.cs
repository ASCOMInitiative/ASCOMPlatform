using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Optec_TCF_S_Focuser
{
    public partial class RemoveOffsetForm : Form
    {
        private RemoveFormStates removeState;

        public RemoveOffsetForm( RemoveFormStates s)
        {
            InitializeComponent();
            removeState = s;
            if (s == RemoveFormStates.Relative)
            {
                this.Text = "Remove Focus Offsets";
                Prompt_TB.Text = "Click an item to select it, then click 'Remove' to delete all checked focus offsets.";
            }
            else
            {
                this.Text = "Remove Absolute Presets";
                Prompt_TB.Text = "Click an item to select it, then click 'Remove' to delete all checked absolute position presets.";
            }
        }

        private void RemoveFocusOffsetFormcs_Load(object sender, EventArgs e)
        {
            Items_CB.Items.Clear();
            if (removeState == RemoveFormStates.Relative)
            {
                foreach (FocusOffset f in XMLSettings.SavedFocusOffsets)
                {
                    Items_CB.Items.Add(f.OffsetName + ", " + f.OffsetSteps.ToString());
                }
            }
            else
            {
                foreach (FocusOffset f in XMLSettings.SavedAbsolutePresets)
                {
                    Items_CB.Items.Add(f.OffsetName + ", " + f.OffsetSteps.ToString());
                }
            }

            //Items_CB.FormatString = 
        }

        private void Remove_BTN_Click(object sender, EventArgs e)
        {
            foreach (var x in Items_CB.CheckedItems)
            {
                string fulltext = Items_CB.GetItemText(x);
                int fullLength = fulltext.Length;
                int indexOfComma = fulltext.IndexOf (",");
                string name = fulltext.Substring(0, indexOfComma);
                string value = fulltext.Substring(indexOfComma + 2, fullLength - (indexOfComma + 2));
                if (removeState == RemoveFormStates.Relative)
                {
                    XMLSettings.RemoveFocusOffset(new FocusOffset(name, int.Parse(value)));
                }
                else XMLSettings.RemoveAbsolutePreset(new FocusOffset(name, int.Parse(value)));
            }
            RemoveFocusOffsetFormcs_Load(this, EventArgs.Empty);
        }

        public enum RemoveFormStates { Relative, Absolute }

    }
}
