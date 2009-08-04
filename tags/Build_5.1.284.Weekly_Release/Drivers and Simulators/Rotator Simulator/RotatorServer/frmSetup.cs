using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
	public partial class frmSetup : Form
	{
		private float m_fUpdateInterval;
		private float m_fRotationRate;

		public frmSetup()
		{
			InitializeComponent();
		}

		public bool CanReverse 
		{ 
			get { return chkCanReverse.Checked; }
			set 
			{ 
				chkCanReverse.Checked = value;
				UpdateUI();
			}
		}

		public bool Reverse
		{
			get { return chkReverse.Checked; }
			set { chkReverse.Checked = value; }
		}

		public float RotationRate 
		{
			get { return m_fRotationRate; }
			set 
			{ 
				m_fRotationRate = value;
				txtRotationRate.Text = value.ToString("0.0"); 
			}
		}

		public float StepSize 
		{ 
			get { return (m_fRotationRate / 1000) * m_fUpdateInterval; }
		}

		public float UpdateInterval
		{
			set
			{
				m_fUpdateInterval = value; 
				UpdateUI();
			}
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{

		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{

		}

		private void chkCanReverse_CheckedChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void txtRotationRate_TextChanged(object sender, EventArgs e)
		{
			if (txtRotationRate.Text != "")
			{
				try
				{
					m_fRotationRate = Convert.ToSingle(txtRotationRate.Text);
				}
				catch (Exception)
				{
					m_fRotationRate = 0;
					txtRotationRate.Text = "";
				}
			}
			else
				m_fRotationRate = 0;
			UpdateUI();
		}

		private void UpdateUI()
		{
			if (!chkCanReverse.Checked)
			{
				chkReverse.Checked = false;
				chkReverse.Enabled = false;
			}
			else
			{
				chkReverse.Enabled = true;
			}
			lblStepSize.Text = "Step size " + this.StepSize.ToString("0.0") + " deg. (4 steps/sec.)";
		}

	}
}