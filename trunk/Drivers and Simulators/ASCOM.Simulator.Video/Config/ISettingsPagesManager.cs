using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Simulator.Properties;

namespace ASCOM.Simulator.Config
{
	public interface ISettingsPagesManager
	{
		void CameraTypeChanged(SimulatedCameraType cameraType);
	}
}
