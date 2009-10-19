using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.GeminiTelescope
{
    public partial class frmProgress : Form
    {
        public delegate void CancelDelegate();

        public CancelDelegate m_CancelDelegate = null;

        private static frmProgress m_Form = null;

        private double m_Pos = 0;

        public bool Exponential { get; set; }

        public frmProgress()
        {
            InitializeComponent();
        }

        public static void Initialize(int start, int range, string title, CancelDelegate cancel, bool exponential)
        {
            m_Form = new frmProgress();
            m_Form.progressBar1.Minimum = start;
            m_Form.progressBar1.Maximum = start + range;
            m_Form.progressBar1.Value = start;
            m_Form.Text = title;
            m_Form.label2.Text = title;
            if (cancel == null) m_Form.button1.Enabled = false;
            m_Form.m_CancelDelegate = cancel;
            m_Form.Exponential = exponential;
        }

        public static void Initialize(int start, int range, string title, CancelDelegate cancel)
        {
            Initialize(start, range, title, cancel, false);
        }

        public static void ShowProgress(Form owner)
        {
            if (m_Form != null && !m_Form.Visible)
            {
                if (owner != null)
                {
                    m_Form.StartPosition = FormStartPosition.Manual;
                    m_Form.Left = owner.Left + (owner.Width - m_Form.Width) / 2;
                    m_Form.Top = owner.Top + (owner.Height - m_Form.Height) / 2;
                }
                m_Form.Show(owner);
            }
        }

        public static void HideProgress()
        {
            if (m_Form != null && m_Form.Visible)
            {
                m_Form.Close();
                m_Form = null;
            }
        }

        public static void Update(double pos, string title)
        {
            if (m_Form != null && m_Form.Visible)
            {
                m_Form.m_Pos += pos;

                int newpos = (int)(m_Form.m_Pos+0.5);

                if (m_Form.Exponential)
                {
                    newpos = (int)(Math.Log10((newpos - m_Form.progressBar1.Minimum + 1)) / 3.0 * (m_Form.progressBar1.Maximum - m_Form.progressBar1.Minimum) + m_Form.progressBar1.Minimum + 0.5);
                }
                if (newpos < m_Form.progressBar1.Minimum) newpos = m_Form.progressBar1.Minimum;
                if (newpos > m_Form.progressBar1.Maximum) newpos = m_Form.progressBar1.Maximum;

                m_Form.progressBar1.Value = newpos;
                m_Form.label1.Text = (100.0 * ((double)(newpos - m_Form.progressBar1.Minimum)) / ((double)(m_Form.progressBar1.Maximum - m_Form.progressBar1.Minimum))).ToString("0") + "%";
                if (!string.IsNullOrEmpty(title)) m_Form.label2.Text = title;
                Application.DoEvents();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (m_CancelDelegate != null) m_CancelDelegate();
        }
    }
}
