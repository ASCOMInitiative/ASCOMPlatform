﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using ASCOM.DeviceInterface;
using ASCOM.Simulator.Properties;

namespace ASCOM.Simulator.Config
{
    public partial class ucGeneral : SettingsPannel
	{
		public ucGeneral(ISettingsPagesManager settingsManager)
			: base(settingsManager)
		{
			Initialize();
		}

		public ucGeneral()
		{
			Initialize();
		}

		private void Initialize()
		{
			InitializeComponent();

			cbxSensorType.Items.Clear();
			cbxSensorType.Items.AddRange(Enum.GetValues(typeof(SensorType)).Cast<SensorType>().Select(x => x.ToString()).ToArray());

			bcxTraceLevel.Items.Clear();
			bcxTraceLevel.Items.AddRange(Enum.GetValues(typeof(TraceLevel)).Cast<TraceLevel>().Select(x => x.ToString()).ToArray());
		}

		internal override void LoadSettings()
		{
			tbxSupportedActionsList.Lines = Properties.Settings.Default.SupportedActionsList.Split(new string[] { "#;" }, StringSplitOptions.RemoveEmptyEntries);

			switch (Properties.Settings.Default.CameraType)
			{
				case SimulatedCameraType.AnalogueNonIntegrating:
					rbAnalogueNonIntegrating.Checked = true;
					break;

				case SimulatedCameraType.AnalogueIntegrating:
					rbAnalogueIntegrating.Checked = true;
					break;

				case SimulatedCameraType.Digital:
					rbDigitalCamera.Checked = true;
					break;

				case SimulatedCameraType.VideoSystem:
					rbVideoSystem.Checked = true;
					break;
			}

			tbxSensorName.Text = Properties.Settings.Default.SensorName;

			string selectedSensor = Properties.Settings.Default.SensorType.ToString();

			cbxSensorType.SelectedIndex = cbxSensorType.Items.IndexOf(selectedSensor);
			if (cbxSensorType.SelectedIndex == -1) cbxSensorType.SelectedIndex = 0;

			nudPixelSizeX.Value = (decimal)Properties.Settings.Default.PixelSizeX;
			nudPixelSizeY.Value = (decimal)Properties.Settings.Default.PixelSizeY;

			bcxTraceLevel.SelectedIndex = bcxTraceLevel.Items.IndexOf(Properties.Settings.Default.TraceLevel.ToString());
			if (bcxTraceLevel.SelectedIndex == -1) bcxTraceLevel.SelectedIndex = 0;

			SetBitDepthSelection(Properties.Settings.Default.BitDepth);

			CameraTypeChanged();
		}

		private SimulatedCameraType GetSelectedCameraType()
		{
			if (rbAnalogueNonIntegrating.Checked)
				return SimulatedCameraType.AnalogueNonIntegrating;
			else if (rbAnalogueIntegrating.Checked)
				return SimulatedCameraType.AnalogueIntegrating;
			else if (rbDigitalCamera.Checked)
				return SimulatedCameraType.Digital;
			else if (rbVideoSystem.Checked)
				return SimulatedCameraType.VideoSystem;

			throw new IndexOutOfRangeException();
		}

		internal override void SaveSettings()
		{
			Properties.Settings.Default.SupportedActionsList = string.Join("#;", tbxSupportedActionsList.Lines);
			Properties.Settings.Default.CameraType = GetSelectedCameraType();

			Properties.Settings.Default.SensorName = tbxSensorName.Text;
			Properties.Settings.Default.SensorType = (SensorType)cbxSensorType.SelectedIndex;
			Properties.Settings.Default.PixelSizeX = (double)nudPixelSizeX.Value;
			Properties.Settings.Default.PixelSizeY = (double)nudPixelSizeY.Value;
			Properties.Settings.Default.BitDepth = GetBitDepthSelection();
			if (bcxTraceLevel.SelectedIndex == -1)
				Properties.Settings.Default.TraceLevel = TraceLevel.Off;
			else
				Properties.Settings.Default.TraceLevel = (TraceLevel) bcxTraceLevel.SelectedIndex;

		}

		private void CameraTypeChanged(object sender, EventArgs e)
		{
			CameraTypeChanged();
		}

		private void CameraTypeChanged()
		{
			SimulatedCameraType cameraType = GetSelectedCameraType();
			if (settingsManager != null)
				settingsManager.CameraTypeChanged(cameraType);

			cbxBitDepth.Enabled = rbDigitalCamera.Checked || rbVideoSystem.Checked;
			if (rbAnalogueIntegrating.Checked || rbAnalogueNonIntegrating.Checked)
				SetBitDepthSelection(8);
		}

		private void SetBitDepthSelection(int bitDepth)
		{
			if (bitDepth == 16)
				cbxBitDepth.SelectedIndex = 2;
			else if (bitDepth == 12)
				cbxBitDepth.SelectedIndex = 1;
			else
				cbxBitDepth.SelectedIndex = 0;
		}

		private int GetBitDepthSelection()
		{
			if (cbxBitDepth.SelectedIndex == 0)
				return 8;
			else if (cbxBitDepth.SelectedIndex == 1)
				return 12;
			else if (cbxBitDepth.SelectedIndex == 2)
				return 16;

			throw new IndexOutOfRangeException();
		}

		internal override bool ValidateSettings()
		{
			return ValidateSensorType();
		}

		private void cbxSensorType_SelectedIndexChanged(object sender, EventArgs e)
		{
			ValidateSensorType();
		}

		private bool ValidateSensorType()
		{
			if ((SensorType)cbxSensorType.SelectedIndex != SensorType.Monochrome)
			{
				MessageBox.Show(
					"Only Monochrome sensor type is supported by the simulator at the moment.",
					"Video Simulator",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);

				cbxSensorType.Focus();

				return false;
			}

			return true;
		}
	}
}
