﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ASCOM.Simulator.Config
{
    public partial class ucVideoSource : SettingsPannel
	{
		public ucVideoSource()
		{
			InitializeComponent();
		}

		internal override void LoadSettings()
		{
			rbUserBitmaps.Checked = !Properties.Settings.Default.UseEmbeddedVideoSource;
			tbxBitmapFolder.Text = Properties.Settings.Default.SourceBitmapFilesLocation;

			cbxBuffering.Checked = Properties.Settings.Default.UseBuffering;
			nudBufferSize.Value = Properties.Settings.Default.BufferSize;

			SetDriverDefaultSourceVisibility(!rbDriverDefaultSource.Checked);
			SetBufferingVisibility(cbxBuffering.Checked);
		}

		internal override bool ValidateSettings()
		{
			if (rbUserBitmaps.Checked)
			{
				if (!ValidateBitmapFilesLocation())
					return false;
			}

			return true;
		}

		internal override void SaveSettings()
		{
			if (rbUserBitmaps.Checked)
			{
				Properties.Settings.Default.UseEmbeddedVideoSource = false;
				Properties.Settings.Default.SourceBitmapFilesLocation = tbxBitmapFolder.Text;
			}
			else
			{
				Properties.Settings.Default.UseEmbeddedVideoSource = true;
			}


			Properties.Settings.Default.UseBuffering = cbxBuffering.Checked;
			if (Properties.Settings.Default.UseBuffering)
				Properties.Settings.Default.BufferSize = (short)nudBufferSize.Value;
		}

		private bool ValidateBitmapFilesLocation()
		{
			string fullPath = tbxBitmapFolder.Text;
			bool pathExists = false;
			try
			{
				fullPath = Path.GetFullPath(fullPath);
				pathExists = Directory.Exists(fullPath);
			}
			catch { }

			if (!pathExists)
			{
				MessageBox.Show(
					this,
					string.Format("Bitmap files location '{0}' does not exist.", fullPath),
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);

				tbxBitmapFolder.Focus();

				return false;
			}

			string[] allBitmapFiles = Directory.GetFiles(fullPath, "*.bmp");
			if (allBitmapFiles.Length == 0)
			{
				MessageBox.Show(
					this,
					string.Format("There are no bitmap files in '{0}'.", fullPath),
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);

				tbxBitmapFolder.Focus();

				return false;
			}

			return true;
		}

		private void btnBrowseForFolder_Click(object sender, EventArgs e)
		{
			folderBrowserDialog.SelectedPath = tbxBitmapFolder.Text;
			if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
				tbxBitmapFolder.Text = folderBrowserDialog.SelectedPath;
		}

		private void rbDriverDefaultSource_CheckedChanged(object sender, EventArgs e)
		{
			SetDriverDefaultSourceVisibility(!rbDriverDefaultSource.Checked);
		}

		private void cbxBuffering_CheckedChanged(object sender, EventArgs e)
		{
			SetBufferingVisibility(cbxBuffering.Checked);
		}

		private void SetDriverDefaultSourceVisibility(bool enabled)
		{
			if (enabled)
			{
				pnlUserBitmaps.Enabled = true;
				lblBmpLocation.ForeColor = Color.White;
			}
			else
			{
				lblBmpLocation.ForeColor = SystemColors.ControlDark;
				pnlUserBitmaps.Enabled = false;
			}
		}

		private void SetBufferingVisibility(bool enabled)
		{
			if (enabled)
			{
				nudBufferSize.Enabled = true;
				lblBuff1.ForeColor = SystemColors.Window;
				lblBuff2.ForeColor = SystemColors.Window;
			}
			else
			{
				lblBuff1.ForeColor = SystemColors.ControlDark;
				lblBuff2.ForeColor = SystemColors.ControlDark;
				nudBufferSize.Enabled = false;
			}
		}
	}
}
