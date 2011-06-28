using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Optec_TCF_S_Focuser
{
    public partial class SettingsForm : Form
    {
        private OptecFocuser myFocuser;
        public SettingsForm(OptecFocuser myFoc)
        {
            InitializeComponent();
            myFocuser = myFoc;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = myFocuser;

            // Changes the horizontal splitter position
            Type propertygridtype = propertyGrid1.GetType();
            FieldInfo y = propertygridtype.GetField("gridView",
                BindingFlags.NonPublic | BindingFlags.Instance);
            y.FieldType.GetMethod("MoveSplitterTo",
                BindingFlags.NonPublic | BindingFlags.Instance).Invoke(y.GetValue(propertyGrid1), new object[] { 175 });

            try
            {
                var info = propertyGrid1.GetType().GetProperty("Controls");
                var collection = (Control.ControlCollection)info.GetValue(propertyGrid1, null);

                foreach (var control in collection)
                {
                    var type = control.GetType();

                    if ("DocComment" == type.Name)
                    {
                        const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
                        var field = type.BaseType.GetField("userSized", Flags);
                        field.SetValue(control, true);

                        info = type.GetProperty("Lines");
                        info.SetValue(control, 6, null);

                        propertyGrid1.HelpVisible = true;
                        break;
                    }
                }
            }

            catch (Exception)
            {
                //Debug.WriteLine(ex);
            }
        
        
        }
    }
}
