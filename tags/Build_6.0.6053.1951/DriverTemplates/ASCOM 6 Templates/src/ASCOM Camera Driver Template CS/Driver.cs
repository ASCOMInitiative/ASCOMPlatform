//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Camera driver for $safeprojectname$
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Camera interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Camera Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Globalization;

namespace ASCOM.$safeprojectname$
{
	//
	// Your driver's ID is ASCOM.$safeprojectname$.Camera
	//
	// The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Camera
	// The ClassInterface/None addribute prevents an empty interface called
	// _Camera from being created and used as the [default] interface
	//
    [Guid("$guid2$")]
    [ServedClassName("$safeprojectname$ Camera")]
    [ProgId("ASCOM.$safeprojectname$.Camera")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Camera : ICameraV2
    {
        #region Camera Constants
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        private static string driverId = "ASCOM.$safeprojectname$.Camera";
        // TODO Change the descriptive string for your driver then remove this line
        private const string driverDescription = "$safeprojectname$ Camera";
        #endregion

        //
        // Constructor - Must be public for COM registration!
        //
        public Camera()
        {
            driverId = Marshal.GenerateProgIdForType(this.GetType());
            // TODO Add your constructor code
        }

        #region ASCOM Registration
        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var p = new Profile {DeviceType = "Camera"})
            {
                if (bRegister)
                    p.Register(driverId, driverDescription);
                else
                    p.Unregister(driverId);
            }
        }

        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }
        #endregion

        #region Implementation of ICameraV2

        public void SetupDialog()
        {
            using (var f = new SetupDialogForm())
            {
                f.ShowDialog();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            throw new ASCOM.MethodNotImplementedException("Action");
        }

        public void CommandBlind(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void AbortExposure()
        {
            throw new MethodNotImplementedException("AbortExposure");
        }

        public void PulseGuide(GuideDirections direction, int duration)
        {
            throw new MethodNotImplementedException("PulseGuide");
        }

        public void StartExposure(double duration, bool light)
        {
            throw new MethodNotImplementedException("StartExposure");
        }

        public void StopExposure()
        {
            throw new MethodNotImplementedException("StopExposure");
        }

        public bool Connected
        {
            get { throw new PropertyNotImplementedException("Connected", false); }
            set { throw new PropertyNotImplementedException(); }
        }

        public string Description
        {
            get { throw new PropertyNotImplementedException("Description", false); }
        }

        public string DriverInfo
        {
            get { throw new PropertyNotImplementedException("DriverInfo", false); }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
            }
        }

        public short InterfaceVersion
        {
            get { return 2; }
        }

        public string Name
        {
            get { throw new PropertyNotImplementedException("Name", false); }
        }

        public ArrayList SupportedActions
        {
            get { return new ArrayList(); }
        }

        public short BinX
        {
            get { throw new PropertyNotImplementedException("BinX", false); }
            set { throw new PropertyNotImplementedException("BinX", true); }
        }

        public short BinY
        {
            get { throw new PropertyNotImplementedException("BinY", false); }
            set { throw new PropertyNotImplementedException("BinY", true); }
        }

        public CameraStates CameraState
        {
            get { throw new PropertyNotImplementedException("CameraState", false); }
        }

        public int CameraXSize
        {
            get { throw new PropertyNotImplementedException("CameraXSize", false); }
        }

        public int CameraYSize
        {
            get { throw new PropertyNotImplementedException("CameraYSize", false); }
        }

        public bool CanAbortExposure
        {
            get { throw new PropertyNotImplementedException("CanAbortExposure", false); }
        }

        public bool CanAsymmetricBin
        {
            get { throw new PropertyNotImplementedException("CanAsymmetricBin", false); }
        }

        public bool CanGetCoolerPower
        {
            get { throw new PropertyNotImplementedException("CanGetCoolerPower", false); }
        }

        public bool CanPulseGuide
        {
            get { throw new PropertyNotImplementedException("CanPulseGuide", false); }
        }

        public bool CanSetCCDTemperature
        {
            get { throw new PropertyNotImplementedException("CanSetCCDTemperature", false); }
        }

        public bool CanStopExposure
        {
            get { throw new PropertyNotImplementedException("CanStopExposure", false); }
        }

        public double CCDTemperature
        {
            get { throw new PropertyNotImplementedException("CCDTemperature", false); }
        }

        public bool CoolerOn
        {
            get { throw new PropertyNotImplementedException("CoolerOn", false); }
            set { throw new PropertyNotImplementedException("CoolerOn", true); }
        }

        public double CoolerPower
        {
            get { throw new PropertyNotImplementedException("CoolerPower", false); }
        }

        public double ElectronsPerADU
        {
            get { throw new PropertyNotImplementedException("ElectronsPerADU", false); }
        }

        public double FullWellCapacity
        {
            get { throw new PropertyNotImplementedException("FullWellCapacity", false); }
        }

        public bool HasShutter
        {
            get { throw new PropertyNotImplementedException("HasShutter", false); }
        }

        public double HeatSinkTemperature
        {
            get { throw new PropertyNotImplementedException("HeatSinkTemperature", false); }
        }

        public object ImageArray
        {
            get { throw new PropertyNotImplementedException("ImageArray", false); }
        }

        public object ImageArrayVariant
        {
            get { throw new PropertyNotImplementedException("ImageArrayVariant", false); }
        }

        public bool ImageReady
        {
            get { throw new PropertyNotImplementedException("ImageReady", false); }
        }

        public bool IsPulseGuiding
        {
            get { throw new PropertyNotImplementedException("IsPulseGuiding", false); }
        }

        public double LastExposureDuration
        {
            get { throw new PropertyNotImplementedException("LastExposureDuration", false); }
        }

        public string LastExposureStartTime
        {
            get { throw new PropertyNotImplementedException("LastExposureStartTime", false); }
        }

        public int MaxADU
        {
            get { throw new PropertyNotImplementedException("MaxADU", false); }
        }

        public short MaxBinX
        {
            get { throw new PropertyNotImplementedException("MaxBinX", false); }
        }

        public short MaxBinY
        {
            get { throw new PropertyNotImplementedException("MaxBinY", false); }
        }

        public int NumX
        {
            get { throw new PropertyNotImplementedException("NumX", false); }
            set { throw new PropertyNotImplementedException("NumX", true); }
        }

        public int NumY
        {
            get { throw new PropertyNotImplementedException("NumY", false); }
            set { throw new PropertyNotImplementedException("NumY", true); }
        }

        public double PixelSizeX
        {
            get { throw new PropertyNotImplementedException("PixelSizeX", false); }
        }

        public double PixelSizeY
        {
            get { throw new PropertyNotImplementedException("PixelSizeY", false); }
        }

        public double SetCCDTemperature
        {
            get { throw new PropertyNotImplementedException("SetCCDTemperature", false); }
            set { throw new PropertyNotImplementedException("SetCCDTemperature", true); }
        }

        public int StartX
        {
            get { throw new PropertyNotImplementedException("StartX", false); }
            set { throw new PropertyNotImplementedException("StartX", true); }
        }

        public int StartY
        {
            get { throw new PropertyNotImplementedException("StartY", false); }
            set { throw new PropertyNotImplementedException("StartY", true); }
        }

        public short BayerOffsetX
        {
            get { throw new PropertyNotImplementedException("BayerOffsetX", false); }
        }

        public short BayerOffsetY
        {
            get { throw new PropertyNotImplementedException("BayerOffsetY", false); }
        }

        public bool CanFastReadout
        {
            get { throw new PropertyNotImplementedException("CanFastReadout", false); }
        }

        public double ExposureMax
        {
            get { throw new PropertyNotImplementedException("ExposureMax", false); }
        }

        public double ExposureMin
        {
            get { throw new PropertyNotImplementedException("ExposureMin", false); }
        }

        public double ExposureResolution
        {
            get { throw new PropertyNotImplementedException("ExposureResolution", false); }
        }

        public bool FastReadout
        {
            get { throw new PropertyNotImplementedException("FastReadout", false); }
            set { throw new PropertyNotImplementedException("FastReadout", true); }
        }

        public short Gain
        {
            get { throw new PropertyNotImplementedException("Gain", false); }
            set { throw new PropertyNotImplementedException("Gain", true); }
        }

        public short GainMax
        {
            get { throw new PropertyNotImplementedException("GainMax", false); }
        }

        public short GainMin
        {
            get { throw new PropertyNotImplementedException("GainMin", false); }
        }

        public ArrayList Gains
        {
            get { throw new PropertyNotImplementedException("Gains", false); }
        }

        public short PercentCompleted
        {
            get { throw new PropertyNotImplementedException("PercentCompleted", false); }
        }

        public short ReadoutMode
        {
            get { throw new PropertyNotImplementedException("ReadoutMode", false); }
        }

        public ArrayList ReadoutModes
        {
            get { throw new PropertyNotImplementedException("ReadoutModes", false); }
        }

        public string SensorName
        {
            get { throw new PropertyNotImplementedException("SensorName", false); }
        }

        public SensorType SensorType
        {
            get { throw new PropertyNotImplementedException("SensorType", false); }
        }

        #endregion
    }
}
