using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ASCOM.GeminiTelescope
{
    public partial class frmOpticsSetup : Form
    {
        public frmOpticsSetup()
        {
            InitializeComponent();
        }

        private ArrayList m_OpticInfos = new ArrayList();

        public ArrayList OpticInfos
        {
            get { return m_OpticInfos; }
            set 
            { 
                m_OpticInfos = value;
                if (m_OpticInfos.Count > 1)
                {
                    for (int i = 1; i < m_OpticInfos.Count; i++)
                    {
                        OpticsInfo oi = (OpticsInfo)m_OpticInfos[i];
                        listBoxOptics.Items.Add(oi.Name);
                    }
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text == "")
            {
                MessageBox.Show("Name cannot be blank.");
                textBoxName.Focus();
            }
            else
            {
                OpticsInfo oi = new OpticsInfo();
                oi.Name = textBoxName.Text;

                if (radioButtonInches.Checked)
                {
                    oi.FocalLength = double.Parse(textBoxFocalLength.Text) * 25.4;
                    oi.ApertureDiameter = double.Parse(textBoxAperture.Text) * 25.4;
                    oi.UnitOfMeasure = "inches";
                }
                else
                {
                    oi.FocalLength = double.Parse(textBoxFocalLength.Text);
                    oi.ApertureDiameter = double.Parse(textBoxAperture.Text);
                    oi.UnitOfMeasure = "millimeters";
                }

                m_OpticInfos.Add(oi);
                listBoxOptics.Items.Add(oi.Name);
                textBoxAperture.Text = "";
                textBoxName.Text = "";
                textBoxFocalLength.Text = "";
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            m_OpticInfos.RemoveAt(listBoxOptics.SelectedIndex + 1);
            listBoxOptics.Items.RemoveAt(listBoxOptics.SelectedIndex);
            
        }

        private void textBoxFocalLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;

            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;

            string groupSeparator = numberFormatInfo.NumberGroupSeparator;

            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();
            if (Char.IsDigit(e.KeyChar))
            {
            }
            else if (e.KeyChar.ToString() == Keys.Back.ToString())
            {
            }

            else
            {
                e.Handled = true;
            }
        }

        private void textBoxAperture_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;

            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;

            string groupSeparator = numberFormatInfo.NumberGroupSeparator;

            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();
            if (Char.IsDigit(e.KeyChar))
            {
            }
            else if (e.KeyChar.ToString() == Keys.Back.ToString())
            {
            }

            else
            {
                e.Handled = true;
            }
        }
    }
}
