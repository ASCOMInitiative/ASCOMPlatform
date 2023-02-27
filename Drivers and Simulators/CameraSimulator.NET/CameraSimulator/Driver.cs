//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Camera driver for Simulator
//
// Description:	A very basic Camera simulator.
//
// Implements:	ASCOM Camera interface version: 1.0
// Author:		Bob Denny <rdenny@dc3.com>
//				using Matthias Busch's VB6 Camera Simulator and Chris Rowland's
//				C#.NET Camera Driver template.
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 14-Oct-2007	rbd	1.0.0	Initial edit, from ASCOM Camera Driver template
// 09-Jan-2009  cdr 6.0.0   Get the basic functionality working, some V2 properties in place but no interface
// 22-Jan-2009  cdr 6.0.0   More functionality, including temperature, noise, image
// 03-Oct-2010  cdr 6.0.0   Should be close to complete.
// 09-Jan-2010  cdr 6.0.0   Simple implementation of PulseGuide. Does nothing except manipulate isPulseGuiding
// --------------------------------------------------------------------------------
//

// Cooler behaviour revised by Peter Simpson in August 2018. Operation of the cooler is described in the cooler timer event handler: void coolerTimer_Elapsed(object sender, ElapsedEventArgs e)

// COOLER CAPABILITIES - The user can configure
//     1) Cooling behaviour - a) well behaved fall to the setpoint, b) falls to the setpoint but overshoots before returning to it, c) starts at and maintains the setpoint, d) falls but never gets to the setpoint
//     2) The ambient temperature
//     3) The cooler setpoint
//     4) The cooler maximum delta T
//     5) The time to reach the setpoint
//     6) The amount of temperature overshoot beyond the setpoint
//     7) The extent of random temperature fluctuations that are applied to the CCD temperature and heat sink temperature properties
//     8) Whether the cooler always starts at ambient temperature when the cooler is turned on or whether it starts at the temperature it has "warmed up to" since the cooler was turned off.
//     9) Whether or not the camera powers up with cooling enabled

using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime;

namespace ASCOM.Simulator
{
    /// <summary>
    /// Your driver's ID is ASCOM.Simulator.Camera
    /// The Guid attribute sets the CLSID for ASCOM.Simulator.Camera
    /// The ClassInterface/None attribute prevents an empty interface called
    /// _Camera from being created and used as the [default] interface
    /// </summary>
    [Guid("12229c31-e7d6-49e8-9c5d-5d7ff05c3bfe"), ClassInterface(ClassInterfaceType.None), ComVisible(true)]
    public class Camera : ICameraV3
    {
        // Driver ID and descriptive string that shows in the Chooser
        private static string s_csDriverID = "ASCOM.Simulator.Camera";
        private static string s_csDriverDescription = "Camera V3 simulator";

        #region Profile string constants
        private const string STR_InterfaceVersion = "InterfaceVersion";
        private const string STR_PixelSizeX = "PixelSizeX";
        private const string STR_PixelSizeY = "PixelSizeY";
        private const string STR_FullWellCapacity = "FullWellCapacity";
        private const string STR_MaxADU = "MaxADU";
        private const string STR_ElectronsPerADU = "ElectronsPerADU";
        private const string STR_CameraXSize = "CameraXSize";
        private const string STR_CameraYSize = "CameraYSize";
        private const string STR_CanAsymmetricBin = "CanAsymmetricBin";
        private const string STR_MaxBinX = "MaxBinX";
        private const string STR_MaxBinY = "MaxBinY";
        private const string STR_HasShutter = "HasShutter";
        private const string STR_SensorName = "SensorName";
        private const string STR_SensorType = "SensorType";
        private const string STR_BayerOffsetX = "BayerOffsetX";
        private const string STR_BayerOffsetY = "BayerOffsetY";
        private const string STR_HasCooler = "HasCooler";
        private const string STR_CanSetCCDTemperature = "CanSetCCDTemperature";
        private const string STR_CanGetCoolerPower = "CanGetCoolerPower";
        private const string STR_SetCCDTemperature = "SetCCDTemperature";
        private const string STR_CanAbortExposure = "CanAbortExposure";
        private const string STR_CanStopExposure = "CanStopExposure";
        private const string STR_MinExposure = "MinExposure";
        private const string STR_MaxExposure = "MaxExposure";
        private const string STR_ExposureResolution = "ExposureResolution";
        private const string STR_ImagePath = "ImageFile";
        private const string STR_ApplyNoise = "ApplyNoise";
        private const string STR_CanPulseGuide = "CanPulseGuide";
        private const string STR_OmitOddBins = "OmitOddBins";
        private const string STR_CanFastReadout = "CanFastReadout";
        private const string STR_GainMode = "GainMode";
        private const string STR_GainMin = "GainMin";
        private const string STR_GainMax = "GainMax";
        private const string STR_Gains = "Gains";
        private const string STR_Gain = "Gain";
        private const string STR_OffsetMode = "OffsetMode";
        private const string STR_OffsetMin = "OffsetMin";
        private const string STR_OffsetMax = "OffsetMax";
        private const string STR_Offsets = "Offsets";
        private const string STR_Offset = "Offset";
        private const string STR_HasSubExposure = "HasSubExposure";
        private const string STR_SubExposureInterval = "SubExposureInterval";

        // Cooler configuration strings
        private const string STR_CoolerAmbientTemperature = "CoolerAmbientTemperature";
        private const string STR_CoolerSetPoint = "CoolerSetPoint";
        private const string STR_CoolerDeltaTMax = "CoolerDeltaTMax";
        private const string STR_CoolerMode = "CoolerMode";
        private const string STR_CoolerTimeToSetPoint = "CoolerTimeToSetPoint";
        private const string STR_CoolerResetToAmbient = "CoolerResetToAmbient";
        private const string STR_CoolerFluctuations = "CoolerFluctuations";
        private const string STR_CoolerOvershoot = "CoolerOvershoot";
        private const string STR_CoolerPowerUpState = "CoolerPowerUpState";
        private const string STR_CoolerUnderDampedCycles = "CoolerUnderDampedCycles";
        private const string STR_CoolerSetPointMinimum = "CoolerSetPointMinimum";
        private const string STR_CoolerGraphRange = "CoolerGraphRange";
        #endregion

        #region Camera cooler modes and initial conditions

        // Available cooler behavioural modes - When adding a new cooler mode, define its name here and add it the coolerModes array to ensure that it will appear in the list of options. Next update places where switch statements configure behaviour based on cooling Mode.
        internal const string COOLERMODE_WELL_BEHAVED = "Well behaved approach to setpoint (Default)";
        internal const string COOLERMODE_ALWAYS_AT_SETPOINT = "Always at setpoint";
        internal const string COOLERMODE_SINGLE_OVERSHOOT = "Single overshoot to setpoint";
        internal const string COOLERMODE_UNDERDAMPED = "Under damped approach to setpoint";
        internal const string COOLERMODE_NEVER_GETS_TO_SETPOINT = "Never gets to setpoint";
        internal List<string> coolerModes = new List<string>() { COOLERMODE_WELL_BEHAVED, COOLERMODE_SINGLE_OVERSHOOT, COOLERMODE_UNDERDAMPED, COOLERMODE_ALWAYS_AT_SETPOINT, COOLERMODE_NEVER_GETS_TO_SETPOINT }; // Collection containing all available cooler modes

        // Cooler default characteristics - See void coolerTimer_Elapsed(object sender, ElapsedEventArgs e) for a description of cooler operation
        private const double COOLER_AMBIENT_TEMPERATURE_DEFAULT = 10; // Ambient temperature (C) when the camera is initially created  
        private const double COOLER_CCD_SET_POINT_DEFAULT = -20; // Camera initial set point
        private const double COOLER_DELTAT_MAX_DEFAULT = 40; // Maximum temperature (C) below ambient to which the camera cooler can cool when the cooler is running at 100%
        private const double COOLER_TIME_TO_SETPOINT_DEFAULT = 30; // Time (seconds) to reach the CCD temperature set point when starting from ambient
        private const double COOLER_FLUCTUATIONS_DEFAULT = 0.0; // +- Default Size of random CCD temperature fluctuations
        private const double COOLER_OVERSHOOT_DEFAULT = 5.0; // Size of CCD temperature overshoot
        private const string COOLER_COOLERMODE_DEFAULT = COOLERMODE_WELL_BEHAVED; // Default cooler mode on initial installation
        private const bool COOLER_RESET_TO_AMBIENT_DEFAULT = false; // Will the CCD temperature reset to ambient on connect or behave like a normal cooler where temperature depends on past cooling experience
        private const bool COOLER_POWER_UP_STATE_DEFAULT = false; // Default power up state for the cooler
        private const double COOLER_UNDERDAMPED_CYCLES_DEFAULT = 5.0;
        private const double COOLER_SETPOINT_MINIMUM_DEFAULT = -40.0;
        private const bool COOLER_GRAPH_RANGE_DEFAULT = false;

        // Cooler behavioural configuration
        internal const double COOLER_NEVER_GETS_TO_SETPOINT_REDUCTION_FACTOR = 0.1; // Arbitrary factor to increase the returned CCD temperature so that it never reaches the setpoint. The achieved temperature will be (1.0 - REDUCTION_FACTOR) of the setpoint
        private const double COOLER_USE_FULL_POWER = 0.75; // Fraction of the cooling curve temperature change above which cooler power will be reported as 100%. e.g. 0.95 means the first 95% of the temperature change will be reported as 100% cooler power and the last 5% as the calculated power.
        internal const double COOLER_SETPOINT_REACHED_OFFSET = 0.1; // Temperature offset from the setpoint at which the cooler will deem that it has arrived at the setpoint. i.e. when the CCD is within +-COOLER_SETPOINT_REACHED_OFFSET of the setpoint
        private const double OVERSHOOT_INCREASE_TO_TIME_FRACTION = 0.5; // Fraction of the cooling time during which the overshoot component increases to its configured value
        private const double OVERSHOOT_DECREASE_TO_TIME_FRACTION = 0.7; // Fraction of the cooling time during which the overshoot diminishes to 0.0. In the remaining fraction of the cooling curve the temperature follows Newton's cooling equation
        private const double COOLER_UNDERDAMPED_CYCLES_COMPLETE_FRACTION = 1.0;

        // Cooler simulator variables
        internal double coolerDeltaTMax; // Maximum difference between ambient temperature and the cooler setpoint
        internal string coolerMode; // Description of current cooler behaviour
        internal double coolerTimeToSetPoint; // Length of time (seconds) that the cooler will take getting to the setpoint from ambient
        internal bool coolerResetToAmbient; // Flag to indicate whether the cooler should reset to ambient whenever cooling is set on
        internal double coolerFluctuation; // Size of random fluctuations at setpoint
        internal double coolerOvershoot; // Size of cooler overshoot when changing setpoint
        internal bool coolerPowerUpState; // Initial state of the cooler when connected
        internal double coolerUnderDampedCycles = COOLER_UNDERDAMPED_CYCLES_DEFAULT;
        internal double coolerSetPointMinimum; // Setpoint below which an exception will be thrown
        internal bool coolerGraphRange; // Flag that determines whether a cooling curve centric or full temperature range graph is displayed

        internal double coolerConstant; // The current value of the Newton's cooling equation constant
        internal double ccdStartTemperature; // CCD temperature at the start of this cooling cycle - depends on previous cooling history - will not be ambient if the CCD is still warming up from a previous cooling cycle
        private DateTime coolerTargetChangedTime; // Time at which the cooler temperature target was last changed
        private bool coolerAtTemperature; // Flag indicating whether the cooler has reached its final temperature
        private static Random randomGenerator; // Ensure that there is only one of these for all camera instances so that fluctuations are not correlated between cameras

        #endregion

        #region Internal properties

        // Testing values
        internal string returnImageAs;

        //Interface version
        internal short interfaceVersion;

        //Pixel
        internal double pixelSizeX;
        internal double pixelSizeY;
        internal double fullWellCapacity;
        internal int maxADU;
        internal double electronsPerADU;

        //CCD
        internal int cameraXSize;
        internal int cameraYSize;
        internal bool canAsymmetricBin;
        internal short maxBinX;
        internal short maxBinY;
        internal short binX;
        internal short binY;
        internal bool hasShutter;
        internal string sensorName;
        internal SensorType sensorType;
        internal short bayerOffsetX;
        internal short bayerOffsetY;

        internal int startX;
        internal int startY;
        internal int numX;
        internal int numY;

        //cooling
        internal bool hasCooler;
        private bool coolerOn;
        internal bool canSetCcdTemperature;
        internal bool canGetCoolerPower;
        internal double coolerPower;
        internal double ccdTemperature;
        internal double heatSinkTemperature;
        internal double targetCcdTemperature;
        internal double setCcdTemperature;

        // Gain
        internal ArrayList gains;
        internal short gainMin;
        internal short gainMax;
        internal short gain;
        internal GainMode gainMode;

        // Offset
        internal ArrayList offsets;
        internal int offsetMin;
        internal int offsetMax;
        internal int offset;
        internal OffsetMode offsetMode;

        // Exposure
        internal bool canAbortExposure;
        internal bool canStopExposure;
        internal double exposureMax;
        internal double exposureMin;
        internal double exposureResolution;
        private double lastExposureDuration;
        private string lastExposureStartTime;
        internal bool hasSubExposure;
        internal double subExposureInterval;

        private DateTime exposureStartTime;
        private double exposureDuration;
        private bool imageReady = false;

        // readout
        internal bool canFastReadout;
        private bool fastReadout;
        private short readoutMode;
        internal ArrayList readoutModes;

        // guiding
        internal bool canPulseGuide;
        private System.Timers.Timer pulseGuideRaTimer;
        private volatile bool isPulseGuidingRa;
        private System.Timers.Timer pulseGuideDecTimer;
        private volatile bool isPulseGuidingDec;

        // simulation
        internal string imagePath;
        internal bool applyNoise;
        private float[,,] imageData;    // room for a 3 plane colour image
        private bool darkFrame;
        internal bool omitOddBins; // True if bins of 3, 5, 7 etc. should throw NotImplementedExceptions

        internal bool connected = false;
        internal CameraStates cameraState = CameraStates.cameraIdle;

        private int[,] imageArray;
        private object[,] imageArrayVariant;
        private int[,,] imageArrayColour;
        private object[,,] imageArrayVariantColour;
        private string lastError = string.Empty;

        private System.Timers.Timer exposureTimer;
        private System.Timers.Timer coolerTimer;

        // supported actions
        // should really use constants for the action names, 
        private ArrayList supportedActions = new ArrayList { "SetFanSpeed", "GetFanSpeed" };

        // SetFanSpeed, GetFanSpeed. These commands control a hypothetical CCD camera heat sink fan, range 0 (off) to 3 (full speed) 
        private int fanMode;

        #endregion

        #region Enums

        internal enum GainMode
        {
            None = 0,
            Gains = 1,
            GainMinMax = 2
        }

        internal enum OffsetMode
        {
            None = 0,
            Offsets = 1,
            OffsetMinMax = 2
        }

        #endregion

        #region Camera Constructor and Dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// Must be public for COM registration!
        /// </summary>
        public Camera()
        {
            InitialiseSimulator();
            Log.LogMessage("Constructor", "Done");
        }

        public void Dispose()
        {
            Log.LogMessage("Dispose", "Releasing memory and components");
            if (exposureTimer != null) exposureTimer.Dispose();
            if (coolerTimer != null) coolerTimer.Dispose();
            imageArray = null;
            imageArrayVariant = null;
            imageArrayColour = null;
            imageArrayVariantColour = null;
            imageData = null;
            GC.Collect();
        }

        #endregion

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        private static void RegUnregASCOM(bool bRegister)
        {
            using (Profile P = new Profile())
            {
                P.DeviceType = "Camera";                    //  Requires Helper 5.0.3 or later
                if (bRegister)
                    P.Register(s_csDriverID, s_csDriverDescription);
                else
                    P.Unregister(s_csDriverID);
                try                        // In case Helper becomes native .NET
                {
                    Marshal.ReleaseComObject(P);
                }
                catch (Exception) { }
            }
        }

        [ComRegisterFunction]
        private static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        [ComUnregisterFunction]
        private static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        #region Cooler Timer

        /// <summary>
        /// Adjust the CCD temperature and power periodically to simulate cooling and warming up
        /// </summary>
        /// <remarks>
        ///  
        /// NOTES
        ///    
        /// 1) The cooler timer only runs while the camera is cooling or warming up, at other times it is disabled.
        /// 
        /// 2) Each cooler mode, including warming up, is responsible for determining when it has reached its target temperature and indicating this by setting the coolerAtTemperature variable to True.
        /// 
        /// 3) Newton's cooling equation is used to calculate how the CCD temperature changes over time. The equation describes an exponential approach to the setpoint that, theoretically, will only be reached in an infinite period of time.
        ///    To work round this the camera deems that the CCD is at the setpoint temperature when the calculated temperature reaches a small offset from the setpoint, e.g. 0.1C, by which time the rate of temperature change is low. 
        ///    
        ///    The advantage of this approach is that the time to arrive at the specified offset can be controlled precisely by suitable choice of cooling constant. Please see the CalculateCoolerConstant method for details of 
        ///    Newton's cooling equation and how the cooler calculates the cooling constant in practice.
        /// 
        /// 4) The reported CCD temperature is adjusted to allow for the specified offset so that the CCD temperature smoothly approaches the required setpoint temperature rather than the (setpoint + offset) temperature.
        ///    
        /// 5) All temperature changes are managed through the same process, regardless of mode:
        ///    a) The Camera.SetCCTTemperature or Camera.CoolerOn properties set the coolerAtTemperature variable to False and start the cooler timer
        ///    b) The coolerTimer_Elapsed event is called multiple times as time passes and the relevant "mode code" calculates a new CCD temperature and cooler power on each call
        ///    c) At some point, the mode code determines that the setpoint has been reached and sets the coolerAtTemperature variable to True as well as setting the CCD temperature precisely to the setpoint
        ///    d) If the coolerAtTemperature variable is True, a cooling cycle completed log message is written and the cooler timer is disabled
        ///    
        /// 6) At "times to temperature" of less than 100 seconds, the cooler timer period is calculated from the "time to temperature" to ensure that at least 100 CCD temperature and power updates are made during
        ///    a full cooling cycle from ambient to maximum cooler temperature. At longer "times to temperature", updates will occur every second.
        ///    
        /// 7) Random temperature fluctuations, if configured (coolerFluctuation > 0.0), are applied in the Camera.CCDTemperature and Camera.HeatSinkTemperature properties before the calculated values are returned to the caller.
        /// 
        /// </remarks>
        private void coolerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Initialise some variables
            TimeSpan currentCoolingCycleTime = DateTime.Now.Subtract(coolerTargetChangedTime); // Calculate how long this cooling cycle has been running
            double overallTimeToSetpointU = -(Math.Log(COOLER_SETPOINT_REACHED_OFFSET / Math.Abs(ccdStartTemperature - targetCcdTemperature))) / coolerConstant; // Calculate the time required to reach the setpoint from the starting CCD temperature
            double currentCoolingTimeFractionU = currentCoolingCycleTime.TotalSeconds / overallTimeToSetpointU; // Calculate the current fraction of the cooling cycle based on current cycle time and expected overall cycle time
            bool cooling = ccdStartTemperature >= targetCcdTemperature; // Determine whether we are cooling down or heating up

            if (coolerOn) // The cooler is on and we are cooling or warming to the setpoint or have just arrived at it
            {

                // Calculate the cooler temperature based on the supplied parameters. The CCD temperature is returned in the ccdTemperature global variable and whether or not the CCD is at temperature is returned in the coolerAtTemperature global variable
                CalculateCoolerTemperature(currentCoolingTimeFractionU, overallTimeToSetpointU, ccdStartTemperature, targetCcdTemperature, coolerUnderDampedCycles, coolerOvershoot, heatSinkTemperature, coolerDeltaTMax, coolerConstant, coolerMode);

                // Set the cooler power
                double powerFractionAtSetPoint = (heatSinkTemperature - targetCcdTemperature) / coolerDeltaTMax; // Calculate the power used at the setpoint as a linear fraction of the maximum cooling that the cooler can achieve

                if (targetCcdTemperature == heatSinkTemperature) // Deal with a special case where the equation below fails
                {
                    coolerPower = 0.0;
                }
                else // Calculate the cooler power from the CCD temperature
                {
                    coolerPower = 100.0 * (powerFractionAtSetPoint + ((1.0 - powerFractionAtSetPoint) * ((ccdTemperature - targetCcdTemperature) / (heatSinkTemperature - targetCcdTemperature)))); // Calculate current cooler power based on the offset of the CCD temperature from the setpoint
                }
                if (coolerPower < 0.0) coolerPower = 0.0; // Above equation can give negative power values when warming up so constrain negative values to 0.0

                if (cooling) // Setpoint is below start temperature so we are cooling
                {
                    // Now force the value to 100% for the first part of the cooling curve to simulate the cooler really going for it...
                    if ((heatSinkTemperature - ccdTemperature) / (heatSinkTemperature - targetCcdTemperature) < COOLER_USE_FULL_POWER)
                    {
                        coolerPower = 100.0;
                    }
                }
            }
            else // Cooler is off and we are warning up or at ambient temperature
            {
                double newtonsEquationTemperature = heatSinkTemperature + (ccdStartTemperature - heatSinkTemperature) * Math.Exp(-coolerConstant * currentCoolingCycleTime.TotalSeconds); // Calculate the "correct" new warmed CCD temperature

                ccdTemperature = newtonsEquationTemperature - -COOLER_SETPOINT_REACHED_OFFSET; // Set the correct value since we are warming up
                ccdTemperature = ConstrainToTemperatureRange(ccdTemperature, ccdStartTemperature, heatSinkTemperature); // Constrain the CCD temperature to the required temperature range

                Log.LogMessage("Timer", "Warming up - CCD temperature:{0:+0.00;-0.00;' '0.00}, Cooler running time: {1:0.00}", ccdTemperature, currentCoolingCycleTime.TotalSeconds);

                if (Math.Abs(newtonsEquationTemperature - heatSinkTemperature) < COOLER_SETPOINT_REACHED_OFFSET) // This test must be based on Newton's equation temperature rather than the adjusted CCD temperature
                {
                    ccdTemperature = heatSinkTemperature; // Set the CCD temperature exactly to the ambient temperature
                    Log.LogMessage("Timer", "CCD has arrived at ambient temperature: {0:+0.00;-0.00;' '0.00}", ccdTemperature);

                    coolerAtTemperature = true;
                }
            }

            // If we have arrived at the setpoint, log this and disable the timer
            if (coolerAtTemperature)
            {
                Log.LogMessage("Timer", "CCD is at its final temperature: {0:+0.00;-0.00;' '0.00} and power: {1:0.0}% - Cooling timer disabled", ccdTemperature, coolerPower);
                coolerTimer.Enabled = false; // Disable the cooler timer because we've reached the setpoint
            }
        }

        /// <summary>
        /// Calculate the CCD temperature for the current cooler mode using the supplied parameters. 
        /// </summary>
        /// <param name="currentCoolingTimeFraction"></param>
        /// <param name="overallTimeToSetpoint"></param>
        /// <param name="ccdStartTemperature"></param>
        /// <param name="targetCcdTemperature"></param>
        /// <param name="coolerUnderDampedCycles"></param>
        /// <param name="coolerOvershoot"></param>
        /// <param name="heatSinkTemperature"></param>
        /// <param name="coolerDeltaTMax"></param>
        /// <param name="coolerConstant"></param>
        /// <param name="coolerMode"></param>
        /// <remarks>Results are returned in the ccdTemperature and coolerAtTemperature global variables.</remarks>
        internal void CalculateCoolerTemperature(double currentCoolingTimeFraction, double overallTimeToSetpoint, double ccdStartTemperature, double targetCcdTemperature, double coolerUnderDampedCycles, double coolerOvershoot, double heatSinkTemperature, double coolerDeltaTMax, double coolerConstant, string coolerMode)
        {
            double currentOvershootCorrection, currentOvershootFraction;

            // From Newton's cooling equation: NewTemperature = SetPoint + (StartTemperature - SetPoint)*e^-Kt
            double newtonsEquationTemperature = targetCcdTemperature + (ccdStartTemperature - targetCcdTemperature) * Math.Exp(-coolerConstant * currentCoolingTimeFraction * overallTimeToSetpoint); // Calculate the new CCD temperature from Newton's cooling equation

            bool cooling = ccdStartTemperature >= targetCcdTemperature; // Determine whether we are cooling down or heating up
            double coolerOffsetSigned = cooling ? COOLER_SETPOINT_REACHED_OFFSET : -COOLER_SETPOINT_REACHED_OFFSET; // Set the sign of the cooler offset depending on whether we are cooling or warming
            double coolerOvershootSigned = cooling ? -coolerOvershoot : coolerOvershoot; // Set the overshoot direction sign correctly : negative when cooling, positive when warming

            // Deal with the special case where start and target temperatures are the same
            if (ccdStartTemperature == targetCcdTemperature)
            {
                ccdTemperature = targetCcdTemperature;
                coolerAtTemperature = true;
                Log.LogMessage("CalculateTemperature", "Start and target temperatures are identical, setting CCD temperature to target temperature: {0:+0.000;-0.000;' '0.000}, Cooler at temperature: {1}", ccdTemperature, coolerAtTemperature);
                return;
            }

            switch (coolerMode)
            {
                case COOLERMODE_ALWAYS_AT_SETPOINT: // No time dependent behaviour required for this mode
                    ccdTemperature = targetCcdTemperature; // Immediately return the required temperature

                    Log.LogMessage("CalculateTemperature", "WellBehaved - CCD temperature is {0}, Last power: {1:0}%", ccdTemperature, coolerPower);
                    coolerAtTemperature = true; // No need for further change because we are at temperature by definition
                    break;

                case COOLERMODE_WELL_BEHAVED:
                    ccdTemperature = newtonsEquationTemperature - coolerOffsetSigned; // Calculate the CCD temperature, including any overshoot correction
                    ccdTemperature = ConstrainToTemperatureRange(ccdTemperature, heatSinkTemperature, heatSinkTemperature - coolerDeltaTMax); // Make sure the temperature stays in the valid range Ambient to (Ambient- DeltaTMax)

                    Log.LogMessage("CalculateTemperature", "WellBehaved - Newton's equation temperature: {0:+0.000;-0.000;' '0.000}, CCD temperature: {1:+0.000;-0.000;' '0.000}, Last power: {2:0}%, Time fraction: {3:0.000}", newtonsEquationTemperature, ccdTemperature, coolerPower, currentCoolingTimeFraction); //);

                    // Check whether we have arrived close enough to the setpoint to say that we have fully arrived:
                    if (Math.Abs(newtonsEquationTemperature - targetCcdTemperature) < COOLER_SETPOINT_REACHED_OFFSET) // This test must be based on Newton's equation temperature rather than the adjusted CCD temperature
                    {
                        ccdTemperature = targetCcdTemperature; // Set the CCD temperature exactly to the target temperature
                        Log.LogMessage("CalculateTemperature", "WellBehaved - CCD has arrived at setpoint temperature: {0:+0.00;-0.00;' '0.00}", ccdTemperature);

                        coolerAtTemperature = true;
                    }
                    break;

                case COOLERMODE_UNDERDAMPED:

                    double sineValue = Math.Sin(currentCoolingTimeFraction / COOLER_UNDERDAMPED_CYCLES_COMPLETE_FRACTION * Math.PI * coolerUnderDampedCycles);

                    // Calculate the fraction of the overshoot temperature that should be applied at this fraction of the cooling cycle
                    if (currentCoolingTimeFraction <= COOLER_UNDERDAMPED_CYCLES_COMPLETE_FRACTION)
                    {
                        // We are in the first phase of the cooling cycle where the overshoot needs to be applied
                        currentOvershootFraction = (COOLER_UNDERDAMPED_CYCLES_COMPLETE_FRACTION - currentCoolingTimeFraction) / (COOLER_UNDERDAMPED_CYCLES_COMPLETE_FRACTION);
                    }
                    else
                    {
                        // We are in the second phase of the cooling cycle with no overshoot and just the Newton's curve temperature
                        currentOvershootFraction = 0.0;
                    }

                    // Calculate the correction to the Newton's equation temperature that will generate overshoot at this time
                    currentOvershootCorrection = coolerOvershootSigned * sineValue * currentOvershootFraction;

                    ccdTemperature = newtonsEquationTemperature - coolerOffsetSigned + currentOvershootCorrection; // Calculate the CCD temperature, including any overshoot correction
                    ccdTemperature = ConstrainToTemperatureRange(ccdTemperature, heatSinkTemperature, heatSinkTemperature - coolerDeltaTMax); // Make sure the temperature stays in the valid range Ambient to (Ambient- DeltaTMax)

                    Log.LogMessage("CalculateTemperature", "UnderDamped - Newton's equation temperature: {0:+0.000;-0.000;' '0.000}, CCD temperature: {1:+0.000;-0.000;' '0.000}, Last power: {2:0}%, Time fraction: {3:0.000} ----- Overshoot correction: {4}, Sine value: {5}, Cooler offset signed: {6}, Cooler overshoot: {7}, Overshoot fraction: {8}",
                        newtonsEquationTemperature, ccdTemperature, coolerPower, currentCoolingTimeFraction, currentOvershootCorrection, sineValue, coolerOffsetSigned, coolerOvershootSigned, currentOvershootFraction); //);

                    // Check whether we have arrived close enough to the setpoint to say that we have fully arrived:
                    if (Math.Abs(newtonsEquationTemperature - targetCcdTemperature) < COOLER_SETPOINT_REACHED_OFFSET) // This test must be based on Newton's equation temperature rather than the adjusted CCD temperature
                    {
                        ccdTemperature = targetCcdTemperature; // Set the CCD temperature exactly to the target temperature
                        Log.LogMessage("CalculateTemperature", "UnderDamped - CCD has arrived at setpoint temperature: {0:+0.00;-0.00;' '0.00}", ccdTemperature);

                        coolerAtTemperature = true;
                    }
                    break;

                case COOLERMODE_SINGLE_OVERSHOOT:
                    // Newton's cooling curve is used as the foundation for CCD temperature prediction in this mode.
                    // However, overshoot delivery requires that the Newton's temperature be modified to create the more complex overshoot behaviour. This is achieved by defining phases, 
                    // specified as fractions of the expected cooling time to the setpoint, which introduce specific incremental temperature offsets that are added to the Newton's temperature 
                    // to create the desired CCD temperature.

                    // This mode caters for two general cases:
                    // 1) No overshoot (coolerOvershoot = 0.0) - CCD temperature is managed in one phase throughout the cooling cycle
                    //    Phase 1) Cooling time fraction 0.0 to 1.0 - Follow Newton's cooling curve from the initial CCD temperature to the setpoint
                    //
                    // 2) Some overshoot (coolerOvershoot > 0.0) - CCD temperature behaviour is managed in three phases through the cooling cycle:
                    //    Phase 1) Cooling time fraction 0.0 to OVERSHOOT_INCREASE_TO_TIME_FRACTION ................................. Proceed from the current CCD temperature through the setpoint to the required overshoot temperature. 
                    //    Phase 2) Cooling time fraction OVERSHOOT_INCREASE_TO_TIME_FRACTION to OVERSHOOT_DECREASE_TO_TIME_FRACTION . Return from the overshoot temperature through the setpoint to the Newton's curve temperature
                    //    Phase 3) Cooling time fraction OVERSHOOT_DECREASE_TO_TIME_FRACTION to 1.0 ................................. Follow Newton's cooling curve to the setpoint

                    // Calculate the offset from the Newton's curve temperature at the time of maximum overshoot
                    if (coolerOvershoot == 0.0) // No overshoot so there is no overshoot correction to be applied to the newton's equation temperature
                    {
                        currentOvershootCorrection = 0.0; // Specify no overshoot correction
                    }
                    else // Calculate the offset that needs to be added to the Newton's curve temperature to generate the configured overshoot at the time of maximum overshoot
                    {
                        double newtonsTemperatureAtMaximumOvershoot = targetCcdTemperature + (ccdStartTemperature - targetCcdTemperature) * Math.Exp(-coolerConstant * overallTimeToSetpoint * OVERSHOOT_INCREASE_TO_TIME_FRACTION); // Calculate the Newton's curve temperature at the time of maximum overshoot

                        double offsetFromNewtonsTemperatureAtMaximumOvershoot = targetCcdTemperature + coolerOvershootSigned - newtonsTemperatureAtMaximumOvershoot; // Calculate the offset from Newton's temperature that will create the required overshoot at the time of maximum overshoot

                        // Calculate the fraction of the overshoot temperature that should be applied at this fraction of the cooling cycle
                        if (currentCoolingTimeFraction <= OVERSHOOT_INCREASE_TO_TIME_FRACTION)
                        {
                            // We are in the fist phase of the overshoot where the temperature needs to exceed the setpoint by the configured amount when OVERSHOOT_INCREASE_TO_TIME_FRACTION of the way through the cooling cycle
                            currentOvershootFraction = Math.Sin((currentCoolingTimeFraction / OVERSHOOT_INCREASE_TO_TIME_FRACTION) * (Math.PI / 2.0)); // Convert the fraction to a sine value to create a smoother transition through the maximum
                        }
                        else if (currentCoolingTimeFraction <= OVERSHOOT_DECREASE_TO_TIME_FRACTION)
                        {
                            // We are in the second phase of the cooling cycle where the overshoot needs to decrease as the time fraction progresses from OVERSHOOT_INCREASE_TO_TIME_FRACTION towards OVERSHOOT_DECREASE_TO_TIME_FRACTION
                            currentOvershootFraction = Math.Sin(((OVERSHOOT_DECREASE_TO_TIME_FRACTION - currentCoolingTimeFraction) / (OVERSHOOT_DECREASE_TO_TIME_FRACTION - OVERSHOOT_INCREASE_TO_TIME_FRACTION)) * (Math.PI / 2.0));
                        }
                        else
                        {
                            // We are in the third phase of the cooling cycle with no overshoot and just the Newton's curve temperature
                            currentOvershootFraction = 0.0;
                        }

                        // Calculate the correction to the Newton's equation temperature that will generate overshoot at this time
                        currentOvershootCorrection = (offsetFromNewtonsTemperatureAtMaximumOvershoot + coolerOffsetSigned) * currentOvershootFraction;
                    }

                    ccdTemperature = newtonsEquationTemperature - coolerOffsetSigned + currentOvershootCorrection; // Calculate the CCD temperature, including any overshoot correction
                    ccdTemperature = ConstrainToTemperatureRange(ccdTemperature, heatSinkTemperature, heatSinkTemperature - coolerDeltaTMax); // Make sure the temperature stays in the valid range Ambient to (Ambient- DeltaTMax)

                    Log.LogMessage("CalculateTemperature", "DampedMode - Newton's equation temperature: {0:+0.00;-0.00;' '0.00}, CCD temperature: {1:+0.00;-0.00;' '0.00}, Last power: {2:0}%, Cooler running time: {3:0.00}", newtonsEquationTemperature, ccdTemperature, coolerPower, currentCoolingTimeFraction * overallTimeToSetpoint);

                    // Check whether we have arrived close enough to the setpoint to say that we have fully arrived:
                    if (Math.Abs(newtonsEquationTemperature - targetCcdTemperature) < COOLER_SETPOINT_REACHED_OFFSET) // This test must be based on Newton's equation temperature rather than the adjusted CCD temperature
                    {
                        ccdTemperature = targetCcdTemperature; // Set the CCD temperature exactly to the target temperature
                        Log.LogMessage("CalculateTemperature", "DampedMode - CCD has arrived at setpoint temperature: {0:+0.00;-0.00;' '0.00}", ccdTemperature);

                        coolerAtTemperature = true;
                    }
                    break;

                case COOLERMODE_NEVER_GETS_TO_SETPOINT: // Return a higher temperature than calculated from Newton's equation
                    double adjustedSetpoint = ((targetCcdTemperature - heatSinkTemperature) * (1.0 - COOLER_NEVER_GETS_TO_SETPOINT_REDUCTION_FACTOR)) + heatSinkTemperature; // Calculate the adjusted setpoint temperature that misses the real setpoint by the COOLER_NEVER_GETS_TO_SETPOINT_REDUCTION_FACTOR
                    double adjustedNewtonsEquationTemperature = adjustedSetpoint + (ccdStartTemperature - adjustedSetpoint) * Math.Exp(-coolerConstant * currentCoolingTimeFraction * overallTimeToSetpoint); // Calculate the "adjusted" Newton's equation temperature

                    ccdTemperature = adjustedNewtonsEquationTemperature - coolerOffsetSigned; // Calculate the adjusted CCD temperature
                    ccdTemperature = ConstrainToTemperatureRange(ccdTemperature, ccdStartTemperature, adjustedSetpoint);  // Constrain the CCD temperature to the required temperature range

                    Log.LogMessage("CalculateTemperature", "MissesSetpoint - Newton's equation temperature:{0:+0.00;-0.00;' '0.00}, Adjusted Newton's equation temperature:{1:+0.00;-0.00;' '0.00}, Adjusted setpoint temperature:{2:+0.00;-0.00;' '0.00}, CCD temperature: {3:+0.00;-0.00;' '0.00}, Last power: {4:0}%, Cooler running time: {5:0.00}", newtonsEquationTemperature, adjustedNewtonsEquationTemperature, adjustedSetpoint, ccdTemperature, coolerPower, currentCoolingTimeFraction * overallTimeToSetpoint);

                    // Check whether we have arrived close enough to the setpoint to say that we have fully arrived:
                    if (Math.Abs(adjustedNewtonsEquationTemperature - adjustedSetpoint) < COOLER_SETPOINT_REACHED_OFFSET) // This test must be based on Newton's equation temperature rather than the adjusted CCD temperature
                    {
                        ccdTemperature = adjustedSetpoint;
                        Log.LogMessage("CalculateTemperature", @"CCD has arrived at ""missed setpoint"" temperature: {0:+0.00;-0.00;' '0.00}", ccdTemperature);

                        coolerAtTemperature = true;
                    }
                    break;

                default: // Warning message for future camera driver developers that they need to update this code if a new curve is introduced
                    Log.LogMessage("Timer", "Unknown cooler mode: {0} - Cooling cycle will be abandoned.", coolerMode);
                    coolerAtTemperature = true; // Terminate this cooling cycle
                    break;
            }
        }

        #endregion

        #region Common Methods

        /// <summary>
        /// the following additional Actions are available:
        ///  "SetFanSpeed" sets the fan speed as required, parameters are:
        ///    "0" - Off
        ///    "1" - Slow speed
        ///    "2" - medium speed
        ///    "3" - high speed
        ///    An empty string is returned
        ///  "GetFanSpeed gets the current fan speed, returns "0" to "3"
        ///    as defined above.
        /// </summary>
        public ArrayList SupportedActions
        {
            get { return supportedActions; }
        }

        public string CommandString(string Command, bool Raw)
        {
            throw new MethodNotImplementedException("CommandString");
        }

        public void CommandBlind(string Command, bool Raw)
        {
            throw new MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string Command, bool Raw)
        {
            throw new MethodNotImplementedException("CommandBool");
        }

        public string Action(string ActionName, string ActionParameters)
        {
            if (supportedActions.Contains(ActionName))
            {
                switch (ActionName)
                {
                    case "SetFanSpeed":
                        int fanModeNew;
                        if (int.TryParse(ActionParameters, out fanModeNew))
                        {
                            if (fanMode >= 0 && fanMode <= 3)
                            {
                                fanMode = fanModeNew;
                                return string.Empty;
                            }
                        }
                        // value not in range
                        throw new InvalidValueException("Action-SetFanMode", ActionParameters, "0 to 3");
                    case "GetFanSpeed":
                        return fanMode.ToString();
                    default:
                        break;
                }
            }
            // failed to find this supported action
            throw new MethodNotImplementedException("Action-" + ActionName);
        }

        #endregion

        #region ICamera Members

        /// <summary>
        /// Aborts the current exposure, if any, and returns the camera to Idle state.
        /// Must throw exception if camera is not idle and abort is
        ///  unsuccessful (or not possible, e.g. during download).
        /// Must throw exception if hardware or communications error
        ///  occurs.
        /// Must NOT throw an exception if the camera is already idle.
        /// </summary>
        public void AbortExposure()
        {
            CheckConnected("Can't abort exposure when not connected");
            if (!canAbortExposure)
            {
                Log.LogMessage("AbortExposure", "CanAbortExposure is false");
                throw new ASCOM.MethodNotImplementedException("AbortExposure");
            }

            Log.LogMessage("AbortExposure", "start");
            switch (cameraState)
            {
                case CameraStates.cameraWaiting:
                case CameraStates.cameraExposing:
                case CameraStates.cameraReading:
                case CameraStates.cameraDownload:
                    // these are all possible exposure states so we can abort the exposure
                    exposureTimer.Enabled = false;
                    cameraState = CameraStates.cameraIdle;
                    imageReady = false;
                    break;
                case CameraStates.cameraIdle:
                    break;
                case CameraStates.cameraError:
                    Log.LogMessage("AbortExposure", "Camera Error");
                    throw new ASCOM.InvalidOperationException("AbortExposure not possible because of an error");
            }
            Log.LogMessage("AbortExposure", "done");
        }

        /// <summary>
        /// Sets the binning factor for the X axis.  Also returns the current value.  Should
        /// default to 1 when the camera link is established.  Note:  driver does not check
        /// for compatible subframe values when this value is set; rather they are checked
        /// upon StartExposure.
        /// </summary>
        /// <value>BinX sets/gets the X binning value</value>
        /// <exception>Must throw an exception for illegal binning values</exception>
        public short BinX
        {
            get
            {
                CheckConnected("Can't read BinX when not connected");
                return binX;
            }
            set
            {
                CheckConnected("Can't set BinX when not connected");
                CheckRange("BinX", 1, value, maxBinX);
                if ((maxBinX >= 4) & omitOddBins & ((value % 2) > 0) & (value >= 3)) // Must be an odd value of 3 or greater when maxbin is 4 or greater
                {
                    Log.LogMessage("BinX", "Odd bin value {0} is invalid", value);
                    throw new InvalidValueException("BinX", value.ToString("d2", CultureInfo.InvariantCulture), string.Format(CultureInfo.InvariantCulture, "1 and even bin values between 2 and {0}", MaxBinX));
                }

                binX = value;
                if (!canAsymmetricBin)
                    binY = value;
            }
        }

        /// <summary>
        /// Sets the binning factor for the Y axis  Also returns the current value.  Should
        /// default to 1 when the camera link is established.  Note:  driver does not check
        /// for compatible subframe values when this value is set; rather they are checked
        /// upon StartExposure.
        /// </summary>
        /// <exception>Must throw an exception for illegal binning values</exception>
        public short BinY
        {
            get
            {
                CheckConnected("Can't read BinY when not connected");
                return binY;
            }
            set
            {
                CheckConnected("Can't set BinY when not connected");
                CheckRange("BinY", 1, value, maxBinY);

                if ((maxBinY >= 4) & omitOddBins & ((value % 2) > 0) & (value >= 3)) // Must be an odd value of 3 or greater when maxbin is 4 or greater
                {
                    Log.LogMessage("BinY", "Odd bin value {0} is invalid", value);
                    throw new InvalidValueException("BinY", value.ToString("d2", CultureInfo.InvariantCulture), string.Format(CultureInfo.InvariantCulture, "1 and even bin values between 2 and {0}", MaxBinY));
                }

                binY = value;
                if (!canAsymmetricBin)
                    binX = value;
            }
        }

        /// <summary>
        /// Returns the current CCD temperature in degrees Celsius. Only valid if
        /// CanControlTemperature is True.
        /// </summary>
        /// <exception>Must throw exception if data unavailable.</exception>
        public double CCDTemperature
        {
            get
            {
                CheckConnected("Can't read the CCD temperature when not connected");
                double returnValue = AddRandomFluctuation(ccdTemperature);
                Log.LogMessage("CCDTemperature", "get {0}", returnValue);
                return returnValue;
            }
        }

        /// <summary>
        /// Returns one of the following status information:
        /// <list type="bullet">
        ///  <listheader>
        ///   <description>Value  State          Meaning</description>
        ///  </listheader>
        ///  <item>
        ///   <description>0      CameraIdle      At idle state, available to start exposure</description>
        ///  </item>
        ///  <item>
        ///   <description>1      CameraWaiting   Exposure started but waiting (for shutter, trigger,
        ///                        filter wheel, etc.)</description>
        ///  </item>
        ///  <item>
        ///   <description>2      CameraExposing  Exposure currently in progress</description>
        ///  </item>
        ///  <item>
        ///   <description>3      CameraReading   CCD array is being read out (digitized)</description>
        ///  </item>
        ///  <item>
        ///   <description>4      CameraDownload  Downloading data to PC</description>
        ///  </item>
        ///  <item>
        ///   <description>5      CameraError     Camera error condition serious enough to prevent
        ///                        further operations (link fail, etc.).</description>
        ///  </item>
        /// </list>
        /// </summary>
        /// <exception cref="System.Exception">Must return an exception if the camera status is unavailable.</exception>
        public CameraStates CameraState
        {
            get
            {
                CheckConnected("Can't read the camera state when not connected");
                Log.LogMessage("CameraState", cameraState.ToString());
                return cameraState;
            }
        }

        /// <summary>
        /// Returns the width of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <exception cref="System.Exception">Must throw exception if the value is not known</exception>
        public int CameraXSize
        {
            get
            {
                CheckConnected("Can't read the camera Xsize when not connected");
                Log.LogMessage("CameraXSize", "get {0}", cameraXSize);
                return cameraXSize;
            }
        }

        /// <summary>
        /// Returns the height of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <exception cref="System.Exception">Must throw exception if the value is not known</exception>
        public int CameraYSize
        {
            get
            {
                CheckConnected("Can't read the camera Ysize when not connected");
                Log.LogMessage("CameraYSize", "get {0}", cameraYSize);
                return cameraYSize;
            }
        }

        /// <summary>
        /// Returns True if the camera can abort exposures; False if not.
        /// </summary>
        public bool CanAbortExposure
        {
            get
            {
                CheckConnected("Can't read CanAbortExposure when not connected");
                Log.LogMessage("CanAbortExposure", "get {0}", canAbortExposure);
                return canAbortExposure;
            }
        }

        /// <summary>
        /// If True, the camera can have different binning on the X and Y axes, as
        /// determined by BinX and BinY. If False, the binning must be equal on the X and Y
        /// axes.
        /// </summary>
        /// <exception cref="System.Exception">Must throw exception if the value is not known (n.b. normally only
        ///            occurs if no link established and camera must be queried)</exception>
        public bool CanAsymmetricBin
        {
            get
            {
                CheckConnected("Can't read CanAsymmetricBin when not connected");
                Log.LogMessage("CanAsymmetricBin", "get {0}", canAsymmetricBin);
                return canAsymmetricBin;
            }
        }

        /// <summary>
        /// If True, the camera's cooler power setting can be read.
        /// </summary>
        public bool CanGetCoolerPower
        {
            get
            {
                CheckConnected("Can't read CanGetCoolerPower when not connected");
                Log.LogMessage("CanGetCoolerPower", "get {0}", canGetCoolerPower);
                return canGetCoolerPower;
            }
        }

        /// <summary>
        /// Returns True if the camera can send autoguider pulses to the telescope mount;
        /// False if not.  (Note: this does not provide any indication of whether the
        /// autoguider cable is actually connected.)
        /// </summary>
        public bool CanPulseGuide
        {
            get
            {
                Log.LogMessage("CanPulseGuide", "get {0}", canPulseGuide);
                return canPulseGuide;
            }
        }

        /// <summary>
        /// If True, the camera's cooler setpoint can be adjusted. If False, the camera
        /// either uses open-loop cooling or does not have the ability to adjust temperature
        /// from software, and setting the TemperatureSetpoint property has no effect.
        /// </summary>
        public bool CanSetCCDTemperature
        {
            get
            {
                CheckConnected("Can't read CanSetCCDTemperature when not connected");
                Log.LogMessage("CanSetCCDTemperature", "get {0}", canSetCcdTemperature);
                return canSetCcdTemperature;
            }
        }

        /// <summary>
        /// Some cameras support StopExposure, which allows the exposure to be terminated
        /// before the exposure timer completes, but will still read out the image.  Returns
        /// True if StopExposure is available, False if not.
        /// </summary>
        /// <exception cref=" System.Exception">not supported</exception>
        /// <exception cref=" System.Exception">an error condition such as link failure is present</exception>
        public bool CanStopExposure
        {
            get
            {
                CheckConnected("Can't read CanStopExposure when not connected");
                Log.LogMessage("CanStopExposure", "get {0}", canStopExposure);
                return canStopExposure;
            }
        }

        /// <summary>
        /// Controls the link between the driver and the camera. Set True to enable the
        /// link. Set False to disable the link (this does not switch off the cooler).
        /// You can also read the property to check whether it is connected.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if unsuccessful.</exception>
        public bool Connected
        {
            get
            {
                Log.LogMessage("Connected", "get {0}", connected);
                return connected;
            }
            set
            {
                Log.LogMessage("Connected", "set {0}", value);
                if (value & !connected) // We are connecting and are not already connected
                {
                    ReadImageFile();

                    // Restore valid settings if necessary
                    imageReady = false;
                    //  Bin X test
                    if ((binX > maxBinX) || (binX < 1))
                    {
                        binX = 1;
                    }

                    //  Bin Y test
                    if ((binY > maxBinY) || (binY < 1))
                    {
                        binY = 1;
                    }

                    // Check the start position is in range, start is in binned pixels
                    if (startX < 0 || startX * binX > cameraXSize)
                    {
                        startX = 0;
                    }
                    if (startY < 0 || startY * binY > cameraYSize)
                    {
                        startY = 0;
                    }

                    // Check that the acquisition is at least 1 pixel in size and fits in the camera area
                    if (numX < 1 || (numX + startX) * binX > cameraXSize)
                    {
                        numX = cameraXSize / binX;
                    }
                    if (numY < 1 || (numY + startY) * binY > cameraYSize)
                    {
                        numY = cameraYSize / binY;
                    }
                }

                connected = value;

                // Create the cooler timer, if required, on connect
                if (coolerTimer == null)
                {
                    coolerTimer = new System.Timers.Timer();
                    coolerTimer.Elapsed += new ElapsedEventHandler(coolerTimer_Elapsed);

                    // Calculate the cooler timer period from the "time to temperature" ensuring at least 200 CCD temperature updates will occur during a full cooling cycle from ambient to maximum cooler temperature
                    double coolerTimeInterval = Math.Min(coolerTimeToSetPoint * 5.0, 500);
                    coolerTimeInterval = Math.Max(1.0, coolerTimeInterval); // Ensure that the interval time is at least 1ms, 0 is an invalid value
                    coolerTimer.Interval = coolerTimeInterval; // Set the cooler timer interval

                    coolerTimer.Enabled = false;
                    Log.LogMessage("Connected", "Cooler timer created");
                }

                if (connected & coolerPowerUpState) CoolerOn = true; // If we are connecting enable the cooler immediately if required through configuration


                if (!connected) // We have just disconnected so free memory used by large objects
                {
                    imageArray = null;
                    imageArrayVariant = null;
                    imageArrayColour = null;
                    imageArrayVariantColour = null;
                    imageData = null;
                }

                // Force a garbage collection on both connect and disconnect
                GC.Collect();
            }
        }

        /// <summary>
        /// Turns on and off the camera cooler, and returns the current on/off state.
        /// Warning: turning the cooler off when the cooler is operating at high delta-T
        /// (typically >20C below ambient) may result in thermal shock.  Repeated thermal
        /// shock may lead to damage to the sensor or cooler stack.  Please consult the
        /// documentation supplied with the camera for further information.
        /// </summary>
        /// <exception cref=" System.Exception">not supported</exception>
        /// <exception cref=" System.Exception">an error condition such as link failure is present</exception>
        public bool CoolerOn
        {
            get
            {
                CheckConnected("Can't read CoolerOn when not connected");
                CheckCapabilityEnabled("CoolerOn", hasCooler);
                Log.LogMessage("CoolerOn", "get {0}", coolerOn);
                return coolerOn;
            }
            set
            {

                CheckConnected("Can't set CoolerOn when not connected");
                CheckCapabilityEnabled("CoolerOn", hasCooler);
                Log.LogMessage("CoolerOn", "set {0}", value);
                coolerOn = value;

                if (coolerOn)// Set the correct cooler temperature starting point, depending on configuration and usage history
                {

                    if (coolerResetToAmbient)
                    {
                        ccdTemperature = heatSinkTemperature; // Configuration requires that CCD temperature is reset to ambient
                        Log.LogMessage("CoolerOn", "Cooling cycle will start with the CCD temperature reset to ambient: {0:+0.00;-0.00;' '0.00}", ccdTemperature);
                    }
                    else
                    {
                        Log.LogMessage("CoolerOn", "Cooling cycle will start with the CCD at its current temperature: {0:+0.00;-0.00;' '0.00}", ccdTemperature);
                    }

                    coolerPower = 100; // Set the cooler power to full on
                    ccdStartTemperature = ccdTemperature; // Use the current CCD temperature as the start point
                    coolerTargetChangedTime = DateTime.Now; // Save the time that the cooler was started

                    if (ccdTemperature != targetCcdTemperature) coolerAtTemperature = false; // We are not at the setpoint so a cooling cycle is required
                    else coolerAtTemperature = true; // We are at the setpoint so no cooling cycle is required

                    coolerTimer.Enabled = true; // Start the cooler timer if its not already running
                }
                else // Cooler off
                {
                    if (coolerResetToAmbient) // CCD temperature immediately returns to ambient by configuration
                    {
                        ccdTemperature = heatSinkTemperature; // Set the CCD temperature to ambient
                        coolerAtTemperature = true; // Indicate that we are already at setpoint temperature
                        Log.LogMessage("CoolerOn", "CCD temperature reset to ambient due to configuration: {0:+0.00;-0.00;' '0.00}", ccdTemperature);
                    }
                    else // Normal behaviour where the cooler warms up from its current temperature
                    {
                        coolerAtTemperature = false;
                        Log.LogMessage("CoolerOn", "CCD temperature left unchanged due to configuration: {0:+0.00;-0.00;' '0.00}", ccdTemperature);
                    }

                    coolerPower = 0; // Set cooler power to zero because the cooler is off
                    ccdStartTemperature = ccdTemperature; // Set the start temperature as the current CCD temperature
                    coolerTargetChangedTime = DateTime.Now; // Set the cooling cycle start time
                    coolerTimer.Enabled = true; // Make sure that the cooler timer is running
                }
            }
        }

        /// <summary>
        /// Returns the present cooler power level, in percent.  Returns zero if CoolerOn is
        /// False.
        /// </summary>
        /// <exception cref=" System.Exception">not supported</exception>
        /// <exception cref=" System.Exception">an error condition such as link failure is present</exception>
        public double CoolerPower
        {
            get
            {
                CheckConnected("Can't read Cooler Power when not connected");
                CheckCapabilityEnabled("CoolerPower", hasCooler);
                Log.LogMessage("CoolerPower", "get {0}", coolerPower);
                return coolerPower;
            }
        }

        /// <summary>
        /// Returns a description of the camera model, such as manufacturer and model
        /// number. Any ASCII characters may be used. The string shall not exceed 68
        /// characters (for compatibility with FITS headers).
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if description unavailable</exception>
        public string Description
        {
            get
            {
                CheckConnected("Can't read Description when not connected");
                if (interfaceVersion == 1)
                {
                    Log.LogMessage("Description", "Simulated V1 Camera");
                    return "Simulated V1 Camera";
                }
                else
                {
                    Log.LogMessage("Description", "Simulated {0} camera {1}", sensorType, sensorName);
                    return string.Format(CultureInfo.CurrentCulture, "Simulated {0} camera {1}", sensorType, sensorName);
                }
            }
        }

        /// <summary>
        /// Returns the gain of the camera in photoelectrons per A/D unit. (Some cameras have
        /// multiple gain modes; these should be selected via the SetupDialog and thus are
        /// static during a session.)
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public double ElectronsPerADU
        {
            get
            {
                CheckConnected("Can't read ElectronsPerADU when not connected");
                Log.LogMessage("ElectronsPerADU", "get {0}", electronsPerADU);
                return electronsPerADU;
            }
        }

        /// <summary>
        /// Reports the full well capacity of the camera in electrons, at the current camera
        /// settings (binning, SetupDialog settings, etc.)
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public double FullWellCapacity
        {
            get
            {
                CheckConnected("Can't read FullWellCapacity when not connected");
                Log.LogMessage("FullWellCapacity", "get {0}", fullWellCapacity);
                return fullWellCapacity;
            }
        }

        /// <summary>
        /// If True, the camera has a mechanical shutter. If False, the camera does not have
        /// a shutter.  If there is no shutter, the StartExposure command will ignore the
        /// Light parameter.
        /// </summary>
        public bool HasShutter
        {
            get
            {
                CheckConnected("Can't read HasShutter when not connected");
                Log.LogMessage("HasShutter", "get {0}", hasShutter);
                return hasShutter;
            }
        }

        /// <summary>
        /// Returns the current heat sink temperature (called "ambient temperature" by some
        /// manufacturers) in degrees Celsius. Only valid if CanControlTemperature is True.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public double HeatSinkTemperature
        {
            get
            {
                CheckConnected("Can't read HeatSinkTemperature when not connected");
                CheckCapabilityEnabled("HeatSinkTemperature", canSetCcdTemperature);
                double returnValue = AddRandomFluctuation(heatSinkTemperature);
                Log.LogMessage("HeatSinkTemperature", "get {0}", returnValue);
                return returnValue;
            }
        }

        /// <summary>
        /// Returns a safearray of int of size NumX * NumY containing the pixel values from
        /// the last exposure. The application must inspect the Safearray parameters to
        /// determine the dimensions. Note: if NumX or NumY is changed after a call to
        /// StartExposure it will have no effect on the size of this array. This is the
        /// preferred method for programs (not scripts) to download images since it requires
        /// much less memory.
        ///
        /// For color or multispectral cameras, will produce an array of NumX * NumY *
        /// NumPlanes.  If the application cannot handle multispectral images, it should use
        /// just the first plane.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public object ImageArray
        {
            get
            {
                CheckConnected("Can't read ImageArray when not connected");
                if (!imageReady)
                {
                    Log.LogMessage("ImageArray", "No Image Available");
                    throw new ASCOM.InvalidOperationException("There is no image available");
                }

                Log.LogMessage("ImageArray", "get");
                if (sensorType == SensorType.Color)
                {
                    // Test code to demonstrate the order of returned array values. Not required in production code!
                    //for (int k = 0; k < 3; k++)
                    //{
                    //    for (int j = 0; j < numY; j++)
                    //    {
                    //        for (int i = 0; i < numX; i++)
                    //        {
                    //            imageArrayColour[i, j, k] = i + 10 * j + 100 * k;
                    //        }
                    //    }
                    //}
                    return imageArrayColour;
                }
                else
                {
                    // Test code to demonstrate the order of returned array values. Not required in production code!
                    //for (int j = 0; j < numY; j++)
                    //{
                    //    for (int i = 0; i < numX; i++)
                    //    {
                    //        imageArray[i, j] = i + 10 * j;
                    //    }
                    //}
                    // Test code to provide a consistent array every time. Not required in production code!
                    //for (int i = 0; i < numX; i++)
                    //{
                    //    for (int j = 0; j < numY; j++)
                    //    {
                    //        imageArray[i, j] = i + j;
                    //    }
                    //}
                    return imageArray;
                }
            }
        }

        /// <summary>
        /// Returns a safearray of Variant of size NumX * NumY containing the pixel values
        /// from the last exposure. The application must inspect the Safearray parameters to
        /// determine the dimensions. Note: if NumX or NumY is changed after a call to
        /// StartExposure it will have no effect on the size of this array. This property
        /// should only be used from scripts due to the extremely high memory utilization on
        /// large image arrays (26 bytes per pixel). Pixels values should be in Short, int,
        /// or Double format.
        ///
        /// For color or multispectral cameras, will produce an array of NumX * NumY *
        /// NumPlanes.  If the application cannot handle multispectral images, it should use
        /// just the first plane.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public object ImageArrayVariant
        {
            get
            {
                CheckConnected("Can't read ImageArrayVariant when not connected");
                if (!imageReady)
                {
                    Log.LogMessage("ImageArrayVariant", "No Image Available");
                    throw new ASCOM.InvalidOperationException("There is no image available");
                }

                // Clear out any previous memory allocations
                ReleaseArrayMemory();

                // convert to variant
                if (sensorType == SensorType.Color)
                {
                    imageArrayVariantColour = new object[imageArrayColour.GetLength(0), imageArrayColour.GetLength(1), 3];
                    Parallel.For(0, imageArrayColour.GetLength(0), i =>
                    //for (int i = 0; i < imageArrayColour.GetLength(1); i++)
                    {
                        for (int j = 0; j < imageArrayColour.GetLength(1); j++)
                        {
                            for (int k = 0; k < 3; k++)
                                imageArrayVariantColour[i, j, k] = imageArrayColour[i, j, k];
                        }

                    });
                    return imageArrayVariantColour;
                }
                else
                {
                    imageArrayVariant = new object[imageArray.GetLength(0), imageArray.GetLength(1)];
                    Parallel.For(0, imageArray.GetLength(0), i =>
                    //for (int i = 0; i < imageArray.GetLength(1); i++)
                    {
                        for (int j = 0; j < imageArray.GetLength(1); j++)
                        {
                            imageArrayVariant[i, j] = imageArray[i, j];
                        }

                    });
                    return imageArrayVariant;
                }
            }
        }

        /// <summary>
        /// If True, there is an image from the camera available. If False, no image
        /// is available and attempts to use the ImageArray method will produce an
        /// exception.
        /// </summary>
        /// <exception cref=" System.Exception">hardware or communications link error has occurred.</exception>
        public bool ImageReady
        {
            get
            {
                CheckConnected("Can't read ImageReady when not connected");
                Log.LogMessage("ImageReady", "get {0}", imageReady);
                return imageReady;
            }
        }

        /// <summary>
        /// If True, pulse guiding is in progress. Required if the PulseGuide() method
        /// (which is non-blocking) is implemented. See the PulseGuide() method.
        /// </summary>
        /// <exception cref=" System.Exception">hardware or communications link error has occurred.</exception>
        public bool IsPulseGuiding
        {
            get
            {
                CheckCapabilityEnabled("IsPulseGuiding", canPulseGuide);
                var ipg = isPulseGuidingRa || isPulseGuidingDec;
                Log.LogMessage("IsPulseGuiding", "get {0}", ipg);
                return ipg;
            }
        }

        /// <summary>
        /// Reports the actual exposure duration in seconds (i.e. shutter open time).  This
        /// may differ from the exposure time requested due to shutter latency, camera timing
        /// precision, etc.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if not supported or no exposure has been taken</exception>
        public double LastExposureDuration
        {
            get
            {
                CheckConnected("Can't read LastExposureDuration when not connected");
                CheckReady("LastExposureDuration");
                Log.LogMessage("LastExposureDuration", "get {0}", lastExposureDuration);
                return lastExposureDuration;
            }
        }

        /// <summary>
        /// Reports the actual exposure start in the FITS-standard
        /// CCYY-MM-DDThh:mm:ss[.sss...] format.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if not supported or no exposure has been taken</exception>
        public string LastExposureStartTime
        {
            get
            {
                CheckConnected("Can't read LastExposureStartTime when not connected");
                CheckReady("LastExposureStartTime");
                Log.LogMessage("LastExposureStartTime", "get {0}", lastExposureStartTime);
                return lastExposureStartTime;
            }
        }

        /// <summary>
        /// Reports the maximum ADU value the camera can produce.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public int MaxADU
        {
            get
            {
                CheckConnected("Can't read MaxADU when not connected");
                Log.LogMessage("MaxADU", "get {0}", maxADU);
                return maxADU;
            }
        }

        /// <summary>
        /// If AsymmetricBinning = False, returns the maximum allowed binning factor. If
        /// AsymmetricBinning = True, returns the maximum allowed binning factor for the X axis.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public short MaxBinX
        {
            get
            {
                CheckConnected("Can't read MaxBinX when not connected");
                Log.LogMessage("MaxBinX", "get {0}", maxBinX);
                return maxBinX;
            }
        }

        /// <summary>
        /// If AsymmetricBinning = False, equals MaxBinX. If AsymmetricBinning = True,
        /// returns the maximum allowed binning factor for the Y axis.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public short MaxBinY
        {
            get
            {
                CheckConnected("Can't read MaxBinY when not connected");
                Log.LogMessage("MaxBinY", "get {0}", maxBinY);
                return maxBinY;
            }
        }

        /// <summary>
        /// Sets the subframe width. Also returns the current value.  If binning is active,
        /// value is in binned pixels.  No error check is performed when the value is set.
        /// Should default to CameraXSize.
        /// </summary>
        public int NumX
        {
            get
            {
                CheckConnected("Can't read NumX when not connected");
                Log.LogMessage("NumX", "get {0}", numX);
                return numX;
            }
            set
            {
                CheckConnected("Can't set NumX when not connected");
                Log.LogMessage("NumX", "set {0}", value);
                numX = value;
            }
        }

        /// <summary>
        /// Sets the subframe height. Also returns the current value.  If binning is active,
        /// value is in binned pixels.  No error check is performed when the value is set.
        /// Should default to CameraYSize.
        /// </summary>
        public int NumY
        {
            get
            {
                CheckConnected("Can't read NumY when not connected");
                Log.LogMessage("NumY", "get {0}", numY);
                return numY;
            }
            set
            {
                CheckConnected("Can't set NumY when not connected");
                Log.LogMessage("NumY", "set {0}", value);
                numY = value;
            }
        }

        /// <summary>
        /// Returns the width of the CCD chip pixels in microns, as provided by the camera
        /// driver.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public double PixelSizeX
        {
            get
            {
                CheckConnected("Can't read PixelSizeX when not connected");
                Log.LogMessage("PixelSizeX", "get {0}", pixelSizeX);
                return pixelSizeX;
            }
        }

        /// <summary>
        /// Returns the height of the CCD chip pixels in microns, as provided by the camera
        /// driver.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public double PixelSizeY
        {
            get
            {
                CheckConnected("Can't read PixelSizeY when not connected");
                Log.LogMessage("PixelSizeY", "get {0}", pixelSizeY);
                return pixelSizeY;
            }
        }

        /// <summary>
        /// This method returns only after the move has completed.
        ///
        /// symbolic Constants
        /// The (symbolic) values for GuideDirections are:
        /// Constant     Value      Description
        /// --------     -----      -----------
        /// guideNorth     0        North (+ declination/elevation)
        /// guideSouth     1        South (- declination/elevation)
        /// guideEast      2        East (+ right ascension/azimuth)
        /// guideWest      3        West (+ right ascension/azimuth)
        ///
        /// Note: directions are nominal and may depend on exact mount wiring.  guideNorth
        /// must be opposite guideSouth, and guideEast must be opposite guideWest.
        /// </summary>
        /// <param name="Direction">Direction of guide command</param>
        /// <param name="Duration">Duration of guide in milliseconds</param>
        /// <exception cref=" System.Exception">PulseGuide command is unsupported</exception>
        /// <exception cref=" System.Exception">PulseGuide command is unsuccessful</exception>
        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            if (!canPulseGuide)
            {
                Log.LogMessage("PulseGuide", "Not Implemented");
                throw new ASCOM.MethodNotImplementedException("PulseGuide");
            }

            Log.LogMessage("PulseGuide", "Direction {0}, Duration (1)", Direction, Duration);
            // very simple implementation, starts a timer that turns isPulseGuiding off when it elapses
            // consider calculating and applying a shift to the image
            // separate Ra and Dec timers
            switch (Direction)
            {
                case GuideDirections.guideEast:
                case GuideDirections.guideWest:
                    if (pulseGuideRaTimer == null)
                    {
                        pulseGuideRaTimer = new System.Timers.Timer();
                        pulseGuideRaTimer.Elapsed += new System.Timers.ElapsedEventHandler(pulseGuideRaTimer_Elapsed);
                    }
                    isPulseGuidingRa = true;
                    pulseGuideRaTimer.Interval = Duration;
                    pulseGuideRaTimer.AutoReset = false;     // only one tick
                    pulseGuideRaTimer.Enabled = true;
                    break;
                case GuideDirections.guideNorth:
                case GuideDirections.guideSouth:
                    if (pulseGuideDecTimer == null)
                    {
                        pulseGuideDecTimer = new System.Timers.Timer();
                        pulseGuideDecTimer.Elapsed += new System.Timers.ElapsedEventHandler(pulseGuideDecTimer_Elapsed);
                    }
                    isPulseGuidingDec = true;
                    pulseGuideDecTimer.Interval = Duration;
                    pulseGuideDecTimer.AutoReset = false;     // only one tick
                    pulseGuideDecTimer.Enabled = true;
                    break;
                default:
                    throw new InvalidValueException($"PulseGuide - Invalid Direction parameter: {Direction}, The valid range is {Convert.ToInt32(Enum.GetValues(typeof(GuideDirections)).Cast<GuideDirections>().Min())} to {Convert.ToInt32(Enum.GetValues(typeof(GuideDirections)).Cast<GuideDirections>().Max())}");
            }
        }

        private void pulseGuideRaTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            isPulseGuidingRa = false;
        }

        private void pulseGuideDecTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            isPulseGuidingDec = false;
        }

        /// <summary>
        /// Sets the camera cooler setpoint in degrees Celsius, and returns the current
        /// setpoint.
        /// Note:  camera hardware and/or driver should perform cooler ramping, to prevent
        /// thermal shock and potential damage to the CCD array or cooler stack.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw exception if command not successful.</exception>
        /// <exception cref=" System.Exception">Must throw exception if CanSetCCDTemperature is False.</exception>
        public double SetCCDTemperature
        {
            get
            {
                CheckConnected("Can't read SetCCDTemperature when not connected");
                CheckCapabilityEnabled("SetCCDTemperature", canSetCcdTemperature);
                Log.LogMessage("SetCCDTemperature", "get {0}", setCcdTemperature);
                return setCcdTemperature;
            }
            set
            {
                CheckConnected("Can't set SetCCDTemperature when not connected");
                CheckCapabilityEnabled("SetCCDTemperature", canSetCcdTemperature);
                CheckRange("SetCCDTemperature", coolerSetPointMinimum, value, heatSinkTemperature); // Make sure the set value is in the supported range
                Log.LogMessage("SetCCDTemperature", "set {0}", value);
                if (value != setCcdTemperature) // Only try and do something if the new setpoint is different to the current value
                {
                    setCcdTemperature = value; // Record the new setpoint temperature

                    // Reset the CCD start temperature if required by configuration
                    if (coolerResetToAmbient)
                    {
                        ccdTemperature = heatSinkTemperature;
                        Log.LogMessage("SetCCDTemperature", "Starting the cooling cycle with the CCD temperature reset to ambient: {0:+0.00;-0.00;' '0.00}", ccdTemperature);
                    }
                    else
                    {
                        Log.LogMessage("SetCCDTemperature", "Starting the cooling cycle with the CCD at its current temperature: {0:+0.00;-0.00;' '0.00}", ccdTemperature);
                    }

                    // Set the cooler target temperature, usually the setpoint - but may not be the setpoint if this is beyond the cooler's capacity... the target temperature will never exceed the cooler's capacity regardless of the setpoint temperature
                    targetCcdTemperature = setCcdTemperature; // Set a default value
                    if (targetCcdTemperature < (heatSinkTemperature - coolerDeltaTMax)) // Target temperature is below what the cooler can achieve
                    {
                        Log.LogMessage("SetCCDTemperature", "Target CCD temperature < Maximum supported: {0:+0.00;-0.00;' '0.00}, Resetting to: {1}", targetCcdTemperature, heatSinkTemperature - coolerDeltaTMax);
                        targetCcdTemperature = heatSinkTemperature - coolerDeltaTMax; // Set the target temperature to the maximum that the cooler can achieve.
                        if (ccdTemperature == targetCcdTemperature) // We are already at the target temperature so flag this
                        {
                            Log.LogMessage("SetCCDTemperature", "CCD temperature is already at target, setting coolerAtTemperature to True");
                            coolerAtTemperature = true;  // Indicate that the cooler is at its setpoint
                        }
                        else // Not at the new target so start a cooling cycle
                        {
                            Log.LogMessage("SetCCDTemperature", "CCD temperature is not at target, setting coolerAtTemperature to False");
                            coolerAtTemperature = false;
                        }
                    }
                    else // Target temperature is above what the cooler can achieve
                    {
                        // Handle setting temperature to ambient
                        if (value < heatSinkTemperature) // Temperature has been set below ambient
                        {
                            coolerAtTemperature = false; // Indicate that the cooler is not yet at its setpoint
                        }
                        else // The setpoint temperature has been set to ambient
                        {
                            if (coolerResetToAmbient) // The new setpoint is ambient and we are configured to start at ambient so, by definition, we are already at the setpoint
                            {
                                coolerPower = 0.0; // Set the cooler power to 0.0 because it is at ambient temperature
                                coolerAtTemperature = true;  // Indicate that the cooler is at its setpoint
                            }
                            else
                            {
                                coolerAtTemperature = false; // Indicate that the cooler is not yet at its setpoint
                            }
                        }
                    }

                    // Start a new cooling cycle if required
                    if (ccdTemperature != targetCcdTemperature)
                    {
                        coolerTargetChangedTime = DateTime.Now; // Record the time that the cooling cycle started
                        ccdStartTemperature = ccdTemperature; // Set the start temperature as the current CCD temperature
                        coolerTimer.Enabled = true; // Make sure that the cooler timer is running
                    }
                }
            }
        }

        /// <summary>
        /// Launches a configuration dialogue box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            if (connected)
                throw new InvalidOperationException("Can't set the CCD properties when connected");
            using (SetupDialogForm F = new SetupDialogForm())
            {
                try
                {
                    F.InitProperties(this);
                    if (F.ShowDialog() == DialogResult.OK) SaveToProfile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Setup exception: " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Starts an exposure. Use ImageReady to check when the exposure is complete.
        /// </summary>
        /// <param name="Duration">exposure duration in seconds</param>
        /// <param name="Light">True if light frame, only valid if the camera has a shutter</param>
        /// <exception cref=" System.Exception">NumX, NumY, XBin, YBin, StartX, StartY, or Duration parameters are invalid.</exception>
        /// <exception cref=" System.Exception">CanAsymmetricBin is False and BinX != BinY</exception>
        /// <exception cref=" System.Exception">the exposure cannot be started for any reason, such as a hardware or communications error</exception>
        public void StartExposure(double Duration, bool Light)
        {
            Log.LogMessage("StartExposure", "Duration {0}, Light {1}", Duration, Light);
            CheckConnected("Can't set StartExposure when not connected");
            // check the duration, light frames only
            if (Light && (Duration > exposureMax || Duration < exposureMin))
            {
                lastError = "Incorrect exposure duration";
                Log.LogMessage("StartExposure", "Incorrect exposure Duration {0}", Duration);
                throw new ASCOM.InvalidValueException("StartExposure Duration", Duration.ToString(CultureInfo.InvariantCulture), string.Format(CultureInfo.InvariantCulture, "{0} to {1}", exposureMax, exposureMin));
            }
            //  Binning tests
            if ((binX > maxBinX) || (binX < 1))
            {
                lastError = "Incorrect bin X factor";
                Log.LogMessage("StartExposure", "Incorrect BinX {0}", binX);
                throw new ASCOM.InvalidValueException("StartExposure BinX",
                                                    binX.ToString(CultureInfo.InvariantCulture),
                                                    string.Format(CultureInfo.InvariantCulture, "1 to {0}", maxBinX));
            }
            if ((binY > maxBinY) || (binY < 1))
            {
                lastError = "Incorrect bin Y factor";
                Log.LogMessage("StartExposure", "Incorrect BinY {0}", binY);
                throw new ASCOM.InvalidValueException("StartExposure BinY",
                                                    binY.ToString(CultureInfo.InvariantCulture),
                                                    string.Format(CultureInfo.InvariantCulture, "1 to {0}", maxBinY));
            }

            // Check the start position is in range, start is in binned pixels
            if (startX < 0 || startX * binX > cameraXSize)
            {
                lastError = "Incorrect Start X position";
                Log.LogMessage("StartExposure", "Incorrect Start X {0}", startX);
                throw new ASCOM.InvalidValueException("StartExposure StartX",
                                                    startX.ToString(CultureInfo.InvariantCulture),
                                                    string.Format(CultureInfo.InvariantCulture, "0 to {0}", cameraXSize / binX));
            }
            if (startY < 0 || startY * binY > cameraYSize)
            {
                lastError = "Incorrect Start Y position";
                Log.LogMessage("StartExposure", "Incorrect Start Y {0}", startY);
                throw new ASCOM.InvalidValueException("StartExposure StartX",
                                                    startX.ToString(CultureInfo.InvariantCulture),
                                                    string.Format(CultureInfo.InvariantCulture, "0 to {0}", cameraXSize / binX));
            }

            // Check that the acquisition is at least 1 pixel in size and fits in the camera area
            if (numX < 1 || (numX + startX) * binX > cameraXSize)
            {
                lastError = "Incorrect Num X value";
                Log.LogMessage("StartExposure", "Incorrect Num X {0}", numX);
                throw new ASCOM.InvalidValueException("StartExposure NumX",
                                                    numX.ToString(CultureInfo.InvariantCulture),
                                                    string.Format(CultureInfo.InvariantCulture, "1 to {0}", cameraXSize / binX));
            }
            if (numY < 1 || (numY + startY) * binY > cameraYSize)
            {
                lastError = "Incorrect Num Y value";
                Log.LogMessage("StartExposure", "Incorrect Num Y {0}", numY);
                throw new ASCOM.InvalidValueException("StartExposure NumY",
                                                    numY.ToString(CultureInfo.InvariantCulture),
                                                    string.Format(CultureInfo.InvariantCulture, "1 to {0}", cameraYSize / binY));
            }

            // Clear out any previous memory allocations
            ReleaseArrayMemory();

            // set up the things to do at the start of the exposure
            imageReady = false;
            if (hasShutter)
            {
                darkFrame = !Light;
            }
            lastExposureStartTime = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss", CultureInfo.InvariantCulture);

            // set the image array dimensions
            if (sensorType == SensorType.Color) // Colour sensor
            {
                if (imageArrayColour == null) // No image array so create a new one
                {
                    Log.LogMessage("StartExposure", $"Creating first time colour array of dimensions {numX} x {numY}");
                    imageArrayColour = new int[numX, numY, 3];
                }
                else // Image array already exists so check whether we can re-use it or whether we have to create a new one
                {
                    if ((imageArrayColour.GetLength(0) == numX) & (imageArrayColour.GetLength(1) == numY)) // Existing array has the required dimensions so we can just re-use it
                    {
                        // No action required because we are re-using the existing array
                        Log.LogMessage("StartExposure", $"Reusing existing colour array of dimensions {numX} x {numY}");
                    }
                    else // Different array size required so remove the old one and create new
                    {
                        Log.LogMessage("StartExposure", $"Creating new colour array of dimensions {numX} x {numY}");
                        imageArrayColour = null; // Discard the current array
                        imageArrayColour = new int[numX, numY, 3];
                        GC.Collect(); // Force a garbage collection to clear out the old array
                    }
                }
            }
            else // Monochrome sensor
            {
                if (imageArray == null) // No image array so create a new one
                {
                    Log.LogMessage("StartExposure", $"Creating first time monochrome array of dimensions {numX} x {numY}");
                    imageArray = new int[numX, numY];
                }
                else // Image array already exists so check whether we can re-use it or whether we have to create a new one
                {
                    if ((imageArray.GetLength(0) == numX) & (imageArray.GetLength(1) == numY)) // Existing array has the required dimensions so we can just re-use it
                    {
                        // No action required because we are re-using the existing array
                        Log.LogMessage("StartExposure", $"Reusing existing monochrome array of dimensions {numX} x {numY}");
                    }
                    else // Different array size required so remove the old one and create new
                    {
                        Log.LogMessage("StartExposure", $"Creating new monochrome array of dimensions {numX} x {numY}");
                        imageArray = null; // Discard the current array
                        imageArray = new int[numX, numY]; // Create a new array
                        GC.Collect(); // Force a garbage collection to clear out the old array
                    }
                }
            }

            if (exposureTimer == null)
            {
                exposureTimer = new System.Timers.Timer();
                exposureTimer.Elapsed += exposureTimer_Elapsed;
            }
            // force the minimum exposure to be 1 millisecond to keep the exposureTimer happy
            exposureTimer.Interval = Math.Max((int)(Duration * 1000), 1);
            cameraState = CameraStates.cameraExposing;
            exposureStartTime = DateTime.Now;
            exposureDuration = Duration;
            exposureTimer.Enabled = true;
            Log.LogMessage("StartExposure", "Completed");
        }

        private void exposureTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            exposureTimer.Enabled = false;
            lastExposureDuration = (DateTime.Now - exposureStartTime).TotalSeconds;
            cameraState = CameraStates.cameraDownload;
            FillImageArray();
            imageReady = true;
            cameraState = CameraStates.cameraIdle;
            Log.LogMessage("ExposureTimer_Elapsed", "done");
        }

        /// <summary>
        /// Sets the subframe start position for the X axis (0 based). Also returns the
        /// current value.  If binning is active, value is in binned pixels.
        /// </summary>
        public int StartX
        {
            get
            {
                if (!connected)
                    throw new NotConnectedException("Can't read StartX when not connected");
                Log.LogMessage("StartX", "get {0}", startX);
                return startX;
            }
            set
            {
                if (!connected)
                    throw new NotConnectedException("Can't set StartX when not connected");
                Log.LogMessage("StartX", "set {0}", value);
                startX = value;
            }
        }

        /// <summary>
        /// Sets the subframe start position for the Y axis (0 based). Also returns the
        /// current value.  If binning is active, value is in binned pixels.
        /// </summary>
        public int StartY
        {
            get
            {
                if (!connected)
                    throw new NotConnectedException("Can't read StartY when not connected");
                Log.LogMessage("StartY", "get {0}", startY);
                return startY;
            }
            set
            {
                if (!connected)
                    throw new NotConnectedException("Can't set StartY when not connected");
                Log.LogMessage("StartY", "set {0}", value);
                startY = value;
            }
        }

        /// <summary>
        /// Stops the current exposure, if any.  If an exposure is in progress, the readout
        /// process is initiated.  Ignored if readout is already in process.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if CanStopExposure is False</exception>
        /// <exception cref=" System.Exception">Must throw an exception if no exposure is in progress</exception>
        /// <exception cref=" System.Exception">Must throw an exception if the camera or link has an error condition</exception>
        /// <exception cref=" System.Exception">Must throw an exception if for any reason no image readout will be available.</exception>
        public void StopExposure()
        {
            CheckConnected("Can't stop exposure when not connected");
            CheckCapabilityEnabled("StopExposure", canStopExposure);
            Log.LogMessage("StopExposure", "state {0}", cameraState);
            switch (cameraState)
            {
                case CameraStates.cameraWaiting:
                case CameraStates.cameraExposing:
                case CameraStates.cameraReading:
                case CameraStates.cameraDownload:
                    // these are all possible exposure states so we can stop the exposure
                    exposureTimer.Enabled = false;
                    lastExposureDuration = (DateTime.Now - exposureStartTime).TotalSeconds;
                    FillImageArray();
                    cameraState = CameraStates.cameraIdle;
                    imageReady = true;
                    break;
                case CameraStates.cameraIdle:
                    break;
                case CameraStates.cameraError:
                default:
                    Log.LogMessage("StopExposure", "Not exposing");
                    // these states are this where it isn't possible to stop an exposure
                    throw new ASCOM.InvalidOperationException("StopExposure not possible if not exposing");
            }
        }

        #endregion

        #region ICameraV2 properties

        /// <summary>
        /// Returns the X offset of the Bayer matrix, as defined in <see cref=""SensorType/>.
        /// Value returned must be in the range 0 to M-1, where M is the width of the Bayer matrix.
        /// The offset is relative to the 0,0 pixel in the sensor array, and does not change to reflect
        /// subframe settings. 
        /// </summary>
        /// <value>The Bayer offset X.</value>
        public short BayerOffsetX
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("BayerOffsetX", 2);
                CheckConnected("BayerOffsetX");
                CheckCapabilityEnabled("BayerOffsetX", sensorType != DeviceInterface.SensorType.Monochrome);
                Log.LogMessage("BayerOffsetX", "get {0}", bayerOffsetX);
                return bayerOffsetX;
            }
        }

        /// <summary>
        /// Returns the Y offset of the Bayer matrix, as defined in <see cref=""SensorType/>.
        /// Value returned must be in the range 0 to M-1, where M is the height of the Bayer matrix.
        /// The offset is relative to the 0,0 pixel in the sensor array, and does not change to reflect
        /// subframe settings. 
        /// </summary>
        /// <value>The Bayer offset Y.</value>
        public short BayerOffsetY
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("BayerOffsetY", 2);
                CheckConnected("BayerOffsetY");
                CheckCapabilityEnabled("BayerOffsetY", sensorType != DeviceInterface.SensorType.Monochrome);
                Log.LogMessage("BayerOffsetY", "get {0}", bayerOffsetY);
                return bayerOffsetY;
            }
        }

        /// <summary>
        /// If True, the <see cref="FastReadout"/> function is available. 
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the <see cref="FastReadout"/> function is available; otherwise, <c>false</c>.
        /// </value>
        public bool CanFastReadout
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("CanFastReadout", 2);
                CheckConnected("CanFastReadout");
                Log.LogMessage("CanFastReadout", "get {0}", canFastReadout);
                return canFastReadout;
            }
        }

        /// <summary>
        /// Returns descriptive and version information about this ASCOM Camera driver.
        /// This string may contain line endings and may be hundreds to thousands of characters long.
        /// It is intended to display detailed information on the ASCOM driver, including version
        /// and copyright data.. See the <see cref="Description"/> property for descriptive info on the camera itself.
        /// To get the driver version for compatibility reasons, use the <see cref="InterfaceVersion"/> property. 
        /// </summary>
        /// <value>The driver info string</value>
        public string DriverInfo
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("DriverInfo", 2);
                CheckConnected("DriverInfo");
                String strVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                Log.LogMessage("DriverInfo", "{0} - Version {1}", s_csDriverDescription, strVersion);
                return (s_csDriverDescription + " - Version " + strVersion);
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver. This must be in the form "n.n".
        /// Not to be confused with the InterfaceVersion property, which is the version of this specification
        /// supported by the driver (currently 2). 
        /// </summary>
        public string DriverVersion
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("DriverVersion", 2);
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                var str = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                Log.LogMessage("DriverVersion", "get {0}", str);
                return str;
            }
        }

        /// <summary>
        /// Returns the maximum exposure time in seconds supported by <see cref="StartExposure"/>. 
        /// </summary>
        /// <value>The max exposure.</value>
        public double ExposureMax
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("ExposureMax", 2);
                CheckConnected("ExposureMax");
                Log.LogMessage("ExposureMax", "get {0}", exposureMax);
                return exposureMax;
            }
        }

        /// <summary>
        /// Returns the minimum exposure time in seconds supported by <see cref="StartExposure"/>. 
        /// </summary>
        /// <value>The min exposure.</value>
        public double ExposureMin
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("ExposureMin", 2);
                CheckConnected("ExposureMin");
                Log.LogMessage("ExposureMin", "get {0}", exposureMin);
                return exposureMin;
            }
        }

        /// <summary>
        /// Returns the smallest increment of exposure time supported by <see cref="StartExposure"/>.
        /// </summary>
        /// <value>The exposure resolution.</value>
        public double ExposureResolution
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("ExposureResolution", 2);
                CheckConnected("ExposureResolution");
                Log.LogMessage("ExposureResolution", "get {0}", exposureResolution);
                return exposureResolution;
            }
        }

        /// <summary>
        /// When set to True, the camera will operate in Fast mode; when set False,
        /// the camera will operate normally. This property should default to False. 
        /// </summary>
        /// <value><c>true</c> if [fast readout]; otherwise, <c>false</c>.</value>
        public bool FastReadout
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("FastReadout", 2);
                CheckConnected("FastReadout");
                CheckCapabilityEnabled("FastReadout", canFastReadout);
                Log.LogMessage("FastReadout", "get {0}", fastReadout);
                return fastReadout;
            }
            set
            {
                CheckSupportedInThisInterfaceVersion("FastReadout", 2);
                CheckConnected("FastReadout");
                CheckCapabilityEnabled("FastReadout", canFastReadout);
                Log.LogMessage("FastReadout", "get {0}", value);
                fastReadout = value;
            }
        }

        /// <summary>
        /// Camera.Gain can be used to adjust the gain setting of the camera, if supported.
        /// The Gain, Gains, GainMin and GainMax operation is complex adjust this at your peril!
        /// </summary>
        /// <value>The gain.</value>
        public short Gain
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("Gain", 2);
                CheckConnected("Gain");
                CheckCapabilityEnabled("Gain", gainMode != GainMode.None);
                Log.LogMessage("Gain", "get {0}", gain);
                return gain;
            }
            set
            {
                CheckSupportedInThisInterfaceVersion("Gain", 2);
                CheckConnected("Gain");
                CheckCapabilityEnabled("Gain", gainMode != GainMode.None);

                switch (gainMode)
                {
                    case GainMode.Gains:
                        CheckRange("Gain", 0, value, gains.Count - 1);
                        break;
                    case GainMode.GainMinMax:
                        CheckRange("Gain", gainMin, value, gainMax);
                        break;
                }

                Log.LogMessage("Gain", $"set {value}, GainMode: {gainMode}");
                gain = value;
            }
        }

        /// <summary>
        /// When specifying the gain setting with an integer value, Camera.GainMax is used
        /// in conjunction with Camera.GainMin to specify the range of valid settings.
        /// </summary>
        /// <value>The max gain.</value>
        public short GainMax
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("GainMax", 2);
                CheckConnected("GainMax");
                CheckCapabilityEnabled("GainMax", gainMode == GainMode.GainMinMax);

                Log.LogMessage("GainMax", "get {0}", gainMax);
                return gainMax;
            }
        }

        /// <summary>
        /// When specifying the gain setting with an integer value, Camera.GainMin is used
        /// in conjunction with Camera.GainMax to specify the range of valid settings.
        /// </summary>
        /// <value>The min gain.</value>
        public short GainMin
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("GainMin", 2);
                CheckConnected("GainMin");
                CheckCapabilityEnabled("GainMax", gainMode == GainMode.GainMinMax);

                Log.LogMessage("GainMin", "get {0}", gainMin);
                return gainMin;
            }
        }

        /// <summary>
        /// Gains provides a 0-based array of available gain settings.
        /// The Gain, Gains, GainMin and GainMax operation is complex adjust this at your peril!
        /// </summary>
        /// <value>The gains.</value>
        public ArrayList Gains
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("Gains", 2);
                CheckConnected("Gains");
                CheckCapabilityEnabled("Gains", gainMode == GainMode.Gains);

                Log.LogMessage("Gains", "get {0}", gains);
                return gains;
            }
        }

        /// <summary>
        /// Reports the version of this interface. Will return 2 for this version.
        /// </summary>
        /// <value>The interface version.</value>
        public short InterfaceVersion
        {
            get { return interfaceVersion; }
        }

        /// <summary>
        /// The short name of the camera, for display purposes.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("Name", 2);
                CheckConnected("Name");
                Log.LogMessage("Name", "Sim {0}", SensorName);
                return "Sim " + SensorName;
            }
        }

        /// <summary>
        /// If valid, returns an integer between 0 and 100, where 0 indicates 0% progress
        /// (function just started) and 100 indicates 100% progress (i.e. completion). 
        /// </summary>
        /// <value>The percent completed.</value>
        public short PercentCompleted
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("PercentCompleted", 2);
                CheckConnected("PercentCompleted");
                switch (cameraState)
                {
                    case CameraStates.cameraWaiting:
                    case CameraStates.cameraExposing:
                    case CameraStates.cameraReading:
                    case CameraStates.cameraDownload:
                        var pc = (short)(((DateTime.Now - exposureStartTime).TotalSeconds / exposureDuration) * 100);
                        Log.LogMessage("PercentCompleted", "state {0}, get {1}", cameraState, pc);
                        return pc;
                    case CameraStates.cameraIdle:
                        Log.LogMessage("PercentCompleted", "imageready {0}", (short)(imageReady ? 100 : 0));
                        return (short)(imageReady ? 100 : 0);
                    default:
                        Log.LogMessage("PercentCompleted", "state {0}, invalid", cameraState);
                        throw new ASCOM.InvalidOperationException("get PercentCompleted is not valid if the camera is not active");
                }
            }
        }

        /// <summary>
        /// ReadoutMode is an index into the array <see cref="ReadoutModes"/>, and selects
        /// the desired readout mode for the camera.
        /// Defaults to 0 if not set.  Throws an exception if the selected mode is not available
        /// </summary>
        /// <value>The readout mode.</value>
        public short ReadoutMode
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("ReadoutMode", 2);
                CheckConnected("ReadoutMode");
                if (readoutModes == null || readoutModes.Count < 1)
                {
                    Log.LogMessage("ReadoutMode", "PropertyNotImplemented because readoutModes == null || readoutModes.Count < 1");
                    throw new ASCOM.PropertyNotImplementedException("ReadoutMode", false);
                    //if (canFastReadout)
                    //    throw new PropertyNotImplementedException("ReadoutMode", false);
                }
                var rm = readoutMode;
                Log.LogMessage("ReadoutMode", "get {0}, mode {1}", rm, readoutModes[rm]);

                return rm;
            }
            set
            {
                Log.LogMessage("ReadoutMode", "set {0}", value);
                CheckSupportedInThisInterfaceVersion("ReadoutMode", 2);
                CheckConnected("ReadoutMode");
                if (readoutModes == null || readoutModes.Count < 1)
                {
                    Log.LogMessage("ReadoutMode", "PropertyNotImplemented readoutModes == null || readoutModes.Count < 1");
                    throw new PropertyNotImplementedException("ReadoutMode", true);
                }
                if (canFastReadout)
                {
                    Log.LogMessage("ReadoutMode", "PropertyNotImplemented canFastReadout is true");
                    throw new PropertyNotImplementedException("ReadoutMode", true);
                }
                if (value < 0 || value > readoutModes.Count - 1)
                {
                    Log.LogMessage("ReadoutMode", "InvalidValueException, value {0}, range 0 to {1}", value, readoutModes.Count - 1);
                    throw new InvalidValueException("ReadoutMode", value.ToString(CultureInfo.InvariantCulture), string.Format(CultureInfo.InvariantCulture, "0 to {0}", readoutModes.Count - 1));
                }
                readoutMode = value;
            }
        }

        /// <summary>
        /// This property provides an array of strings, each of which describes an available readout mode
        /// of the camera. 
        /// </summary>
        /// <value>The readout modes.</value>
        public ArrayList ReadoutModes
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("ReadoutModes", 2);
                CheckConnected("ReadoutModes");
                Log.LogMessage("ReadoutModes", "ReadoutModes {0}", readoutModes.Count);
                foreach (var item in readoutModes)
                {
                    Log.LogMessage("", "       {0}", item);
                }
                return readoutModes;
            }
        }

        /// <summary>
        /// Returns the name (data sheet part number) of the sensor, e.g. ICX285AL.
        /// The format is to be exactly as shown on manufacturer data sheet, subject to the following rules.
        /// All letter shall be upper-case.  Spaces shall not be included.
        /// </summary>
        /// <value>The name of the sensor.</value>
        public string SensorName
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("SensorName", 2);
                CheckConnected("SensorName");
                Log.LogMessage("SensorName", "get {0}", sensorName);
                return sensorName;
            }
        }

        /// <summary>
        /// SensorType returns a value indicating whether the sensor is monochrome,
        /// or what Bayer matrix it encodes. 
        /// </summary>
        /// <value>The type of the sensor.</value>
        public SensorType SensorType
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("SensorType", 2);
                CheckConnected("SensorType");
                Log.LogMessage("SensorType", "get {0}", sensorType);
                return sensorType;
            }
        }

        #endregion

        #region ICameraV3 members

        /// <summary>
        /// Camera.Offset can be used to adjust the offset setting of the camera, if supported.
        /// The Offset, Offsets, OffsetMin and OffsetMax operation is complex adjust this at your peril!
        /// </summary>
        /// <value>The offset.</value>
        public int Offset
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("Offset", 3);
                CheckConnected("Offset");
                CheckCapabilityEnabled("Offset", offsetMode != OffsetMode.None);
                Log.LogMessage("Offset", "get {0}", offset);
                return offset;
            }
            set
            {
                CheckSupportedInThisInterfaceVersion("Offset", 3);
                CheckConnected("Offset");
                CheckCapabilityEnabled("Offset", offsetMode != OffsetMode.None);

                switch (offsetMode)
                {
                    case OffsetMode.Offsets:
                        CheckRange("Offset", 0, value, offsets.Count - 1);
                        break;
                    case OffsetMode.OffsetMinMax:
                        CheckRange("Offset", offsetMin, value, offsetMax);
                        break;
                }

                Log.LogMessage("Offset", $"set {value}, OffsetMode: {offsetMode}");
                offset = value;
            }
        }

        /// <summary>
        /// When specifying the offset setting with an integer value, Camera.OffsetMax is used
        /// in conjunction with Camera.OffsetMin to specify the range of valid settings.
        /// </summary>
        /// <value>The max offset.</value>
        public int OffsetMax
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("OffsetMax", 3);
                CheckConnected("OffsetMax");
                CheckCapabilityEnabled("OffsetMax", offsetMode == OffsetMode.OffsetMinMax);

                Log.LogMessage("OffsetMax", "get {0}", offsetMax);
                return offsetMax;
            }
        }

        /// <summary>
        /// When specifying the offset setting with an integer value, Camera.OffsetMin is used
        /// in conjunction with Camera.OffsetMax to specify the range of valid settings.
        /// </summary>
        /// <value>The min offset.</value>
        public int OffsetMin
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("OffsetMin", 3);
                CheckConnected("OffsetMin");
                CheckCapabilityEnabled("OffsetMax", offsetMode == OffsetMode.OffsetMinMax);

                Log.LogMessage("OffsetMin", "get {0}", offsetMin);
                return offsetMin;
            }
        }

        /// <summary>
        /// Offsets provides a 0-based array of available offset settings.
        /// The Offset, Offsets, OffsetMin and OffsetMax operation is complex adjust this at your peril!
        /// </summary>
        /// <value>The offsets.</value>
        public ArrayList Offsets
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("Offsets", 3);
                CheckConnected("Offsets");
                CheckCapabilityEnabled("Offsets", offsetMode == OffsetMode.Offsets);

                Log.LogMessage("Offsets", "get {0}", offsets);
                return offsets;
            }
        }

        /// <summary>
        /// Camera.SubExposureDuration can be used to specify that multiple exposures be aggregated by the camera into a single composite exposure
        /// </summary>
        /// <value>The sub exposure duration (s).</value>
        public double SubExposureDuration
        {
            get
            {
                CheckSupportedInThisInterfaceVersion("SubExposureDuration", 3);
                CheckConnected("SubExposureDuration");
                CheckCapabilityEnabled("SubExposureDuration", hasSubExposure);
                Log.LogMessage("SubExposureDuration", "get {0}", subExposureInterval);
                return subExposureInterval;
            }
            set
            {
                CheckSupportedInThisInterfaceVersion("SubExposureDuration", 3);
                CheckConnected("SubExposureDuration");
                CheckCapabilityEnabled("SubExposureDuration", hasSubExposure);
                Log.LogMessage("SubExposureDuration", $"set {value}");
                if (value < double.Epsilon) throw new InvalidValueException($"The sub exposure duration must not be negative or zero: {value}");
                subExposureInterval = value;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// Release memory allocated to the large arrays on the large object heap.
        /// </summary>
        private void ReleaseArrayMemory()
        {
            // Clear out any previous memory allocations
            //imageArray= null;
            //imageArrayColour= null;
            imageArrayVariant = null;
            imageArrayVariantColour = null;
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(2, GCCollectionMode.Forced, true, true);
        }

        private void ReadFromProfile()
        {
            using (Profile profile = new Profile(true))
            {
                profile.DeviceType = "Camera";

                // Read cooler configuration properties
                heatSinkTemperature = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_CoolerAmbientTemperature, "Cooler", COOLER_AMBIENT_TEMPERATURE_DEFAULT.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
                coolerDeltaTMax = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_CoolerDeltaTMax, "Cooler", COOLER_DELTAT_MAX_DEFAULT.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
                coolerMode = profile.GetValue(s_csDriverID, STR_CoolerMode, "Cooler", COOLER_COOLERMODE_DEFAULT.ToString(CultureInfo.InvariantCulture));
                coolerTimeToSetPoint = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_CoolerTimeToSetPoint, "Cooler", COOLER_TIME_TO_SETPOINT_DEFAULT.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
                coolerResetToAmbient = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CoolerResetToAmbient, "Cooler", COOLER_RESET_TO_AMBIENT_DEFAULT.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
                coolerFluctuation = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_CoolerFluctuations, "Cooler", COOLER_FLUCTUATIONS_DEFAULT.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
                coolerOvershoot = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_CoolerOvershoot, "Cooler", COOLER_OVERSHOOT_DEFAULT.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
                coolerPowerUpState = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CoolerPowerUpState, "Cooler", COOLER_POWER_UP_STATE_DEFAULT.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
                coolerUnderDampedCycles = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_CoolerUnderDampedCycles, "Cooler", COOLER_UNDERDAMPED_CYCLES_DEFAULT.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
                coolerSetPointMinimum = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_CoolerSetPointMinimum, "Cooler", COOLER_SETPOINT_MINIMUM_DEFAULT.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
                coolerGraphRange = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CoolerGraphRange, "Cooler", COOLER_GRAPH_RANGE_DEFAULT.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);

                // Read testing properties - not settable through the Setup GUI
                returnImageAs = profile.GetValue(s_csDriverID, "ReturnImageAs", string.Empty, "SimulatedImage");

                // read properties from profile
                Log.Enabled = Convert.ToBoolean(profile.GetValue(s_csDriverID, "Trace", string.Empty, "false"), CultureInfo.InvariantCulture);
                interfaceVersion = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_InterfaceVersion, string.Empty, "3"), CultureInfo.InvariantCulture);
                pixelSizeX = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_PixelSizeX, string.Empty, "5.6"), CultureInfo.InvariantCulture);
                pixelSizeY = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_PixelSizeY, string.Empty, "5.6"), CultureInfo.InvariantCulture);
                fullWellCapacity = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_FullWellCapacity, string.Empty, "30000"), CultureInfo.InvariantCulture);
                maxADU = Convert.ToInt32(profile.GetValue(s_csDriverID, STR_MaxADU, string.Empty, "65535"), CultureInfo.InvariantCulture);
                electronsPerADU = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_ElectronsPerADU, string.Empty, "0.8"), CultureInfo.InvariantCulture);

                cameraXSize = Convert.ToInt32(profile.GetValue(s_csDriverID, STR_CameraXSize, string.Empty, "800"), CultureInfo.InvariantCulture);
                cameraYSize = Convert.ToInt32(profile.GetValue(s_csDriverID, STR_CameraYSize, string.Empty, "600"), CultureInfo.InvariantCulture);
                canAsymmetricBin = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanAsymmetricBin, string.Empty, "true"), CultureInfo.InvariantCulture);
                maxBinX = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_MaxBinX, string.Empty, "4"), CultureInfo.InvariantCulture);
                maxBinY = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_MaxBinY, string.Empty, "4"), CultureInfo.InvariantCulture);
                hasShutter = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_HasShutter, string.Empty, "false"), CultureInfo.InvariantCulture);
                sensorName = profile.GetValue(s_csDriverID, STR_SensorName, string.Empty, "");
                sensorType = (SensorType)Convert.ToInt32(profile.GetValue(s_csDriverID, STR_SensorType, string.Empty, "0"), CultureInfo.InvariantCulture);
                omitOddBins = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_OmitOddBins, string.Empty, "false"), CultureInfo.InvariantCulture);

                bayerOffsetX = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_BayerOffsetX, string.Empty, "0"), CultureInfo.InvariantCulture);
                bayerOffsetY = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_BayerOffsetY, string.Empty, "0"), CultureInfo.InvariantCulture);

                hasCooler = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_HasCooler, string.Empty, "true"), CultureInfo.InvariantCulture);
                canSetCcdTemperature = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanSetCCDTemperature, string.Empty, "false"), CultureInfo.InvariantCulture);
                canGetCoolerPower = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanGetCoolerPower, string.Empty, "false"), CultureInfo.InvariantCulture);
                setCcdTemperature = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_SetCCDTemperature, string.Empty, "-10"), CultureInfo.InvariantCulture);

                canAbortExposure = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanAbortExposure, string.Empty, "true"), CultureInfo.InvariantCulture);
                canStopExposure = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanStopExposure, string.Empty, "true"), CultureInfo.InvariantCulture);
                exposureMax = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_MaxExposure, string.Empty, "3600"), CultureInfo.InvariantCulture);
                exposureMin = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_MinExposure, string.Empty, "0.001"), CultureInfo.InvariantCulture);
                exposureResolution = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_ExposureResolution, string.Empty, "0.001"), CultureInfo.InvariantCulture);

                string fullPath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly((GetType())).Location);
                imagePath = profile.GetValue(s_csDriverID, STR_ImagePath, string.Empty, Path.Combine(fullPath, @"m42-800x600.jpg"));
                applyNoise = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_ApplyNoise, string.Empty, "false"), CultureInfo.InvariantCulture);

                canPulseGuide = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanPulseGuide, string.Empty, "false"), CultureInfo.InvariantCulture);

                // Get Gain configuration
                gainMode = (GainMode)Convert.ToInt32(profile.GetValue(s_csDriverID, STR_GainMode, string.Empty, "0"), CultureInfo.InvariantCulture);
                gain = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_Gain, string.Empty, "0"), CultureInfo.InvariantCulture);
                gainMin = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_GainMin, string.Empty, "0"), CultureInfo.InvariantCulture);
                gainMax = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_GainMax, string.Empty, "0"), CultureInfo.InvariantCulture);

                string[] gainsStringArray = profile.GetValue(s_csDriverID, STR_Gains, string.Empty, "ISO 100,ISO 200,ISO 400,ISO 800,ISO 1600").Split(',');
                gains = new ArrayList();
                foreach (string gain in gainsStringArray)
                {
                    gains.Add(gain.Trim());
                }

                // Get Offset configuration
                offsetMode = (OffsetMode)Convert.ToInt32(profile.GetValue(s_csDriverID, STR_OffsetMode, string.Empty, "0"), CultureInfo.InvariantCulture);
                offset = Convert.ToInt32(profile.GetValue(s_csDriverID, STR_Offset, string.Empty, "0"), CultureInfo.InvariantCulture);
                offsetMin = Convert.ToInt32(profile.GetValue(s_csDriverID, STR_OffsetMin, string.Empty, "0"), CultureInfo.InvariantCulture);
                offsetMax = Convert.ToInt32(profile.GetValue(s_csDriverID, STR_OffsetMax, string.Empty, "0"), CultureInfo.InvariantCulture);

                // Get sub exposure configuration
                hasSubExposure = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_HasSubExposure, string.Empty, "false"), CultureInfo.InvariantCulture);
                subExposureInterval = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_SubExposureInterval, string.Empty, "1.5"), CultureInfo.InvariantCulture);

                string[] offsetStringArray = profile.GetValue(s_csDriverID, STR_Offsets, string.Empty, "Offset for ISO 100,Offset for ISO 200,Offset for ISO 400,Offset for ISO 800,Offset for ISO 1600").Split(',');
                offsets = new ArrayList();
                foreach (string offset in offsetStringArray)
                {
                    offsets.Add(offset.Trim());
                }

                canFastReadout = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanFastReadout, string.Empty, "false"), CultureInfo.InvariantCulture);

                string gainsString = profile.GetValue(s_csDriverID, "ReadoutModes");
                readoutModes = new ArrayList();
                if (string.IsNullOrEmpty(gainsString))
                {
                    readoutModes.Add("Default");
                }
                else
                {
                    string[] rms = gainsString.Split(',');
                    foreach (var item in rms)
                    {
                        readoutModes.Add(item);
                    }
                }
            }

            // Validate the cooler mode, if invalid select the default and write this back to the profile.
            if (!coolerModes.Contains(coolerMode))
            {
                coolerMode = COOLER_COOLERMODE_DEFAULT;
                SaveCoolerToProfile();
            }
        }

        /// <summary>
        /// Save only the cooler variables to the Profile store
        /// </summary>
        internal void SaveCoolerToProfile()
        {
            using (Profile profile = new Profile(true))
            {
                profile.DeviceType = "Camera";

                // Save the cooler configuration to the Profile
                profile.WriteValue(s_csDriverID, STR_CoolerAmbientTemperature, heatSinkTemperature.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerDeltaTMax, coolerDeltaTMax.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerMode, coolerMode.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerTimeToSetPoint, coolerTimeToSetPoint.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerResetToAmbient, coolerResetToAmbient.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerFluctuations, coolerFluctuation.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerOvershoot, coolerOvershoot.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_SetCCDTemperature, setCcdTemperature.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_CoolerPowerUpState, coolerPowerUpState.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerUnderDampedCycles, coolerUnderDampedCycles.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerSetPointMinimum, coolerSetPointMinimum.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerGraphRange, coolerGraphRange.ToString(CultureInfo.InvariantCulture), "Cooler");
            }
        }

        /// <summary>
        ///  Save all variables to the Profile store, including cooler variables
        /// </summary>
        private void SaveToProfile()
        {
            using (Profile profile = new Profile(true))
            {
                profile.DeviceType = "Camera";

                // Save camera configuration to the Profile
                profile.WriteValue(s_csDriverID, "Trace", Log.Enabled.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_InterfaceVersion, interfaceVersion.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_PixelSizeX, pixelSizeX.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_PixelSizeY, pixelSizeY.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_FullWellCapacity, fullWellCapacity.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_MaxADU, maxADU.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_ElectronsPerADU, electronsPerADU.ToString(CultureInfo.InvariantCulture));

                profile.WriteValue(s_csDriverID, STR_CameraXSize, cameraXSize.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_CameraYSize, cameraYSize.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_CanAsymmetricBin, canAsymmetricBin.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_MaxBinX, maxBinX.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_MaxBinY, maxBinY.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_HasShutter, hasShutter.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_SensorName, sensorName.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_SensorType, ((int)sensorType).ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_BayerOffsetX, bayerOffsetX.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_BayerOffsetY, bayerOffsetY.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_OmitOddBins, omitOddBins.ToString(CultureInfo.InvariantCulture));

                profile.WriteValue(s_csDriverID, STR_HasCooler, hasCooler.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_CanSetCCDTemperature, canSetCcdTemperature.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_CanGetCoolerPower, canGetCoolerPower.ToString(CultureInfo.InvariantCulture));

                profile.WriteValue(s_csDriverID, STR_CanAbortExposure, canAbortExposure.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_CanStopExposure, canStopExposure.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_MaxExposure, exposureMax.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_MinExposure, exposureMin.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_ExposureResolution, exposureResolution.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_ImagePath, imagePath);
                profile.WriteValue(s_csDriverID, STR_ApplyNoise, applyNoise.ToString(CultureInfo.InvariantCulture));

                profile.WriteValue(s_csDriverID, STR_CanPulseGuide, canPulseGuide.ToString(CultureInfo.InvariantCulture));

                // Write gain variables
                profile.WriteValue(s_csDriverID, STR_GainMode, ((int)gainMode).ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_Gain, Convert.ToString(gain, CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_GainMin, Convert.ToString(gainMin, CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_GainMax, Convert.ToString(gainMax, CultureInfo.InvariantCulture));

                System.Text.StringBuilder gainStringBuilder = new System.Text.StringBuilder();
                foreach (string gainItem in gains)
                {
                    if (gainStringBuilder.Length > 0) gainStringBuilder.Append(",");
                    gainStringBuilder.Append(gainItem.ToString());
                }
                profile.WriteValue(s_csDriverID, STR_Gains, gainStringBuilder.ToString());

                // Write offset variables
                profile.WriteValue(s_csDriverID, STR_OffsetMode, ((int)offsetMode).ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_Offset, Convert.ToString(offset, CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_OffsetMin, Convert.ToString(offsetMin, CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_OffsetMax, Convert.ToString(offsetMax, CultureInfo.InvariantCulture));

                // Write sub exposure values
                profile.WriteValue(s_csDriverID, STR_HasSubExposure, hasSubExposure.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_SubExposureInterval, subExposureInterval.ToString(CultureInfo.InvariantCulture));

                System.Text.StringBuilder offsetStringBuilder = new System.Text.StringBuilder();
                foreach (string offsetItem in offsets)
                {
                    if (offsetStringBuilder.Length > 0) offsetStringBuilder.Append(",");
                    offsetStringBuilder.Append(offsetItem.ToString());
                }
                profile.WriteValue(s_csDriverID, STR_Offsets, offsetStringBuilder.ToString());

                profile.WriteValue(s_csDriverID, STR_CanFastReadout, canFastReadout.ToString(CultureInfo.InvariantCulture));

                if (readoutModes == null || readoutModes.Count <= 1)
                {
                    profile.DeleteValue(s_csDriverID, "ReadoutModes");
                }
                else
                {
                    // Readout Modes use string array
                    gainStringBuilder = new System.Text.StringBuilder();
                    foreach (var item in readoutModes)
                    {
                        if (gainStringBuilder.Length > 0)
                            gainStringBuilder.Append(",");
                        gainStringBuilder.Append(item.ToString());
                    }
                    profile.WriteValue(s_csDriverID, "ReadoutModes", gainStringBuilder.ToString());
                }
            }
            SaveCoolerToProfile(); // Save the cooler profile as well
        }

        /// <summary>
        /// Add a random fluctuation of ±coolerFluctuation to the supplied temperature and return the revised value
        /// </summary>
        /// <param name="temperature">The temperature to which the random fluctuation must be applied</param>
        /// <returns>The supplied temperature ± a random fluctuation</returns>
        private double AddRandomFluctuation(double temperature)
        {
            double returnValue = temperature; // Initialise the return value with the supplied temperature

            if (coolerFluctuation != 0.0) // Some fluctuation in returned value is required
            {
                double fluctuation = (randomGenerator.NextDouble() * 2.0 * coolerFluctuation) - coolerFluctuation; // Calculate the fluctuation by translating the Random.NextDouble() value (range 0.0 to +1.0) into the range -coolerFluctuation to +coolerFluctuation
                returnValue = temperature + fluctuation; // Calculate the new temperature including the random fluctuation
            }

            return returnValue;
        }

        private void InitialiseSimulator()
        {
            ReadFromProfile();

            randomGenerator = new Random(); // Initialise the random fluctuation generator

            startX = 0;
            startY = 0;
            binX = 1;
            binY = 1;
            numX = cameraXSize;
            numY = cameraYSize;
            readoutMode = 0;
            fastReadout = false;

            cameraState = CameraStates.cameraIdle;

            // Set initial cooler control variables
            targetCcdTemperature = setCcdTemperature; // Set the default value
            if (targetCcdTemperature < (heatSinkTemperature - coolerDeltaTMax)) // Ensure that the cooler target temperature cannot go below the maximum achievable, event if the setpoint is set below what is achievable
            {
                targetCcdTemperature = heatSinkTemperature - coolerDeltaTMax;
            }

            coolerPower = coolerOn ? 100.0 : 0.0; // Set the cooler power depending on whether or not the cooler is on 
            ccdTemperature = heatSinkTemperature; // Set the CCD temperature to ambient
            ccdStartTemperature = heatSinkTemperature; // Set the cooling cycle start temperature to ambient
            coolerAtTemperature = !coolerOn; // Indicate whether we are at temperature as the inverse of whether or not the cooler is on
            coolerConstant = CalculateCoolerConstant(coolerTimeToSetPoint, coolerDeltaTMax); // Set the initial cooling constant in case the camera is just loaded and switched on

            LogCoolerVariables(); // Write the cooler variables to the driver log

            Log.LogMessage("InitialiseSimulator", "Set camera temperature to ambient: {0} with cooler constant {1} - Cooler mode: {2}", heatSinkTemperature, coolerConstant, coolerMode);
        }

        private delegate int PixelProcess(double value);

        private void FillImageArray()
        {
            // Release image array variant memory to make as much headroom as possible
            imageArrayVariant = null;
            imageArrayVariantColour = null;
            GC.Collect();

            PixelProcess pixelProcess = new PixelProcess(NoNoise);
            ShutterProcess shutterProcess = new ShutterProcess(BinData);

            if (applyNoise)
                pixelProcess = new PixelProcess(Poisson);
            if (HasShutter && darkFrame)
                shutterProcess = new ShutterProcess(DarkData);

            double readNoise = 3;
            // dark current 1 ADU/sec at 0C doubling for every 5C increase
            double darkCurrent = Math.Pow(2, ccdTemperature / 5);
            darkCurrent *= lastExposureDuration;
            // add read noise, should be in quadrature
            darkCurrent += readNoise;
            // fill the array using binning and image offsets
            // indexes into the imageData

            switch (returnImageAs)
            {
                case string value when value.Equals("Byte", StringComparison.InvariantCultureIgnoreCase):
                case string value1 when value1.Equals("Int16", StringComparison.InvariantCultureIgnoreCase):
                case string value2 when value2.Equals("UInt16", StringComparison.InvariantCultureIgnoreCase):
                case string value3 when value3.Equals("Int32", StringComparison.InvariantCultureIgnoreCase):

                    // Set the appropriate element value
                    Int32 elementValue;
                    if (returnImageAs.ToLowerInvariant() == "byte") elementValue = 127; // Byte
                    else if (returnImageAs.ToLowerInvariant() == "int16") elementValue = -32768; // Int16
                    else if (returnImageAs.ToLowerInvariant() == "uint16") elementValue = 32767; // UInt16
                    else elementValue = 100000; // Int32

                    if (sensorType == SensorType.Color) // Colour sensor
                    {
                        for (int x = 0; x < numX; x++)
                        {
                            for (int y = 0; y < numY; y++)
                            {
                                imageArrayColour[x, y, 0] = elementValue;
                                imageArrayColour[x, y, 1] = elementValue + 1;
                                imageArrayColour[x, y, 2] = elementValue + 2;
                            }
                        }
                    }
                    else // Monochrome sensor
                    {
                        for (int x = 0; x < numX; x++)
                        {
                            for (int y = 0; y < numY; y++)
                            {
                                imageArray[x, y] = elementValue;
                            }
                        }
                    }
                    break;

                case string value when value.Equals("ImageAsIs", StringComparison.InvariantCultureIgnoreCase):
                    if (sensorType == SensorType.Color) // Colour sensor
                    {
                        for (int x = 0; x < numX; x++)
                        {
                            for (int y = 0; y < numY; y++)
                            {
                                imageArrayColour[x, y, 0] = (int)imageData[x, y, 0];
                                imageArrayColour[x, y, 1] = (int)imageData[x, y, 1];
                                imageArrayColour[x, y, 2] = (int)imageData[x, y, 2];
                            }
                        }
                    }
                    else // Monochrome sensor
                    {
                        for (int x = 0; x < numX; x++)
                        {
                            for (int y = 0; y < numY; y++)
                            {
                                imageArray[x, y] = (int)imageData[x, y, 0];
                            }
                        }
                    }
                    break;

                case string value when value.Equals("RandomByte", StringComparison.InvariantCultureIgnoreCase):
                case string value1 when value1.Equals("RandomInt16", StringComparison.InvariantCultureIgnoreCase):
                case string value2 when value2.Equals("RandomUInt16", StringComparison.InvariantCultureIgnoreCase):
                case string value3 when value3.Equals("RandomInt32", StringComparison.InvariantCultureIgnoreCase):
                    Random random = new Random();
                    int lowerBound = 0; // Random range lower bound
                    int upperBound = 0; // Random range upper bound

                    if (returnImageAs.ToLowerInvariant() == "randombyte") // Byte
                    {
                        lowerBound = Byte.MinValue;
                        upperBound = Byte.MaxValue;
                    }
                    else if (returnImageAs.ToLowerInvariant() == "randomint16") // Int16
                    {
                        lowerBound = Int16.MinValue;
                        upperBound = Int16.MaxValue;
                    }
                    else if (returnImageAs.ToLowerInvariant() == "randomuint16") // UInt16
                    {
                        lowerBound = UInt16.MinValue;
                        upperBound = UInt16.MaxValue;
                    }
                    else // Int32
                    {
                        lowerBound = Int32.MinValue;
                        upperBound = Int32.MaxValue;
                    }

                    if (sensorType == SensorType.Color) // Colour sensor
                    {
                        for (int x = 0; x < numX; x++)
                        {
                            for (int y = 0; y < numY; y++)
                            {
                                imageArrayColour[x, y, 0] = random.Next(lowerBound, upperBound);
                                imageArrayColour[x, y, 1] = random.Next(lowerBound, upperBound);
                                imageArrayColour[x, y, 2] = random.Next(lowerBound, upperBound);
                            }
                        }
                    }
                    else // Monochrome sensor
                    {
                        for (int x = 0; x < numX; x++)
                        {
                            for (int y = 0; y < numY; y++)
                            {
                                imageArray[x, y] = random.Next(upperBound, upperBound);
                            }
                        }
                    }
                    break;

                case string value when value.Equals("IncreasingData", StringComparison.InvariantCultureIgnoreCase):
                    if (sensorType == SensorType.Color) // Colour sensor
                    {
                        for (int x = 0; x < numX; x++)
                        {
                            for (int y = 0; y < numY; y++)
                            {
                                imageArrayColour[x, y, 0] = x + y;
                                imageArrayColour[x, y, 1] = x + y;
                                imageArrayColour[x, y, 2] = x + y;
                            }
                        }

                    }
                    else // Monochrome sensor
                    {
                        for (int x = 0; x < numX; x++)
                        {
                            for (int y = 0; y < numY; y++)
                            {
                                imageArray[x, y] = x + y;
                            }
                        }
                    }
                    break;

                default: // Everything else returns a normal simulated image
                    if (sensorType == SensorType.Color) // Colour sensor
                    {
                        for (int y = 0; y < numY; y++)
                        {
                            for (int x = 0; x < numX; x++)
                            {
                                double s;
                                s = shutterProcess((x + startX) * binX, (y + startY) * binY, 0);
                                imageArrayColour[x, y, 0] = pixelProcess(s + darkCurrent);
                                s = shutterProcess((x + startX) * binX, (y + startY) * binY, 1);
                                imageArrayColour[x, y, 1] = pixelProcess(s + darkCurrent);
                                s = shutterProcess((x + startX) * binX, (y + startY) * binY, 2);
                                imageArrayColour[x, y, 2] = pixelProcess(s + darkCurrent);
                            }
                        }
                    }
                    else // Monochrome sensor
                    {
                        for (int x = 0; x < numX; x++)
                        {
                            for (int y = 0; y < numY; y++)
                            {
                                double s;
                                s = shutterProcess((x + startX) * binX, (y + startY) * binY, 0);
                                imageArray[x, y] = pixelProcess(s + darkCurrent);
                            }
                        }
                    }
                    break;
            }
        }

        private Random R = new Random();

        private int Poisson(double lambda)
        {
            // use normal distribution for large values
            // because Poisson falls over and gets slow
            if (lambda > 50)
                return Math.Min((int)BoxMuller(lambda, Math.Sqrt(lambda)), maxADU);

            double L = Math.Exp(-lambda);
            double p = 1.0;
            int k = 0;
            do
            {
                k++;
                p *= R.NextDouble();
            }
            while (p > L);
            return Math.Min(k - 1, maxADU);
        }

        /// <summary>
        /// normal random variate generator
        /// </summary>
        /// <param name="m">mean</param>
        /// <param name="s">standard deviation</param>
        /// <returns></returns>
        private double BoxMuller(double m, double s)
        {
            double xa, xb, w, ya;
            do
            {
                xa = 2.0 * R.NextDouble() - 1.0;
                xb = 2.0 * R.NextDouble() - 1.0;
                w = xa * xa + xb * xb;
            } while (w >= 1.0);

            w = Math.Sqrt((-2.0 * Math.Log(w)) / w);
            ya = xa * w;
            return (m + ya * s);
        }

        private int NoNoise(double value)
        {
            return Convert.ToInt32(Math.Min(value, maxADU));
        }

        /// <summary>
        /// Delegate to handle getting the binned or unbinned data for each pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private delegate double ShutterProcess(int x, int y, int p);

        /// <summary>
        /// returns the sum of the image data for binX x BinY pixels
        /// </summary>
        /// <param name="x">left of bin area</param>
        /// <param name="y">top of bin area</param>
        /// <returns></returns>
        private double BinData(int x, int y, int p)
        {
            double s = 0;
            for (int k = 0; k < binY; k++)
            {
                for (int l = 0; l < binX; l++)
                {
                    s += imageData[l + x, k + y, p];
                }
            }
            return s * lastExposureDuration;
        }

        private double DarkData(int x, int y, int p)
        {
            return 5.0 * binX * binY;
        }

        private delegate void GetData(int x, int y);
        private Bitmap bmp;
        // Bayer offsets
        private int x0;
        private int x1;
        private int x2;
        private int x3;

        private int y0;
        private int y1;
        private int y2;
        private int y3;

        private int stepX;
        private int stepY;

        /// <summary>
        /// reads image data from a file and puts it into a buffer in a format suitable for
        /// processing into the ImageArray.  The size of the image must be the same as the
        /// full frame image data.
        /// </summary>
        private void ReadImageFile()
        {

            // Create or reuse the image data array
            if (sensorType == SensorType.Monochrome)
            {
                if (imageData == null) imageData = new float[cameraXSize, cameraYSize, 1];
            }
            else
            {
                if (imageData == null) imageData = new float[cameraXSize, cameraYSize, 3];
            }

            try
            {
                bmp = (Bitmap)Image.FromFile(imagePath);

                // x0 = bayerOffsetX;
                // x1 = (bayerOffsetX + 1) & 1;
                // y0 = bayerOffsetY;
                // y1 = (bayerOffsetY + 1) & 1;

                GetData getData = new GetData(MonochromeData);
                switch (sensorType)
                {
                    case SensorType.Monochrome:
                        stepX = 1;
                        stepY = 1;
                        break;
                    case SensorType.RGGB:
                        getData = new GetData(RGGBData);
                        stepX = 2;
                        stepY = 2;
                        break;
                    case SensorType.CMYG:
                        getData = new GetData(CMYGData);
                        stepX = 2;
                        stepY = 2;
                        break;
                    case SensorType.CMYG2:
                        getData = new GetData(CMYG2Data);
                        stepX = 2;
                        stepY = 4;
                        // y0 = (bayerOffsetY) & 3;
                        // y1 = (bayerOffsetY + 1) & 3;
                        // y2 = (bayerOffsetY + 2) & 3;
                        // y3 = (bayerOffsetY + 3) & 3;
                        break;
                    case SensorType.LRGB:
                        getData = new GetData(LRGBData);
                        stepX = 4;
                        stepY = 4;
                        break;
                    case SensorType.Color:
                        getData = new GetData(ColorData);
                        stepX = 1;
                        stepY = 1;
                        break;
                    default:
                        break;
                }
                x0 = bayerOffsetX;
                x1 = (x0 + 1) & (stepX - 1);
                x2 = (x0 + 2) & (stepX - 1);
                x3 = (x0 + 3) & (stepX - 1);
                y0 = bayerOffsetY;
                y1 = (y0 + 1) & (stepY - 1);
                y2 = (y0 + 2) & (stepY - 1);
                y3 = (y0 + 3) & (stepY - 1);

                int w = Math.Min(cameraXSize, bmp.Width * stepX);
                int h = Math.Min(cameraYSize, bmp.Height * stepY);
                for (int y = 0; y < h; y += stepY)
                {
                    for (int x = 0; x < w; x += stepX)
                    {
                        getData(x, y);
                    }
                }

            }
            catch
            {
            }
        }

        // get data using the sensor types
        private void MonochromeData(int x, int y)
        {
            imageData[x, y, 0] = (bmp.GetPixel(x, y).GetBrightness() * 255);
        }
        private void RGGBData(int x, int y)
        {
            Color px = bmp.GetPixel(x / 2, y / 2);
            imageData[x + x0, y + y0, 0] = px.R;      // red
            imageData[x + x1, y + y0, 0] = px.G;      // green
            imageData[x + x0, y + y1, 0] = px.G;      // green
            imageData[x + x1, y + y1, 0] = px.B;      // blue
        }
        private void CMYGData(int x, int y)
        {
            Color px = bmp.GetPixel(x / 2, y / 2);
            imageData[x + x0, y + y0, 0] = (px.R + px.G) / 2;       // yellow
            imageData[x + x1, y + y0, 0] = (px.G + px.B) / 2;       // cyan
            imageData[x + x0, y + y1, 0] = px.G;                    // green
            imageData[x + x1, y + y1, 0] = (px.R + px.B) / 2;       // magenta
        }
        private void CMYG2Data(int x, int y)
        {
            Color px = bmp.GetPixel(x / 2, y / 2);
            imageData[x + x0, y + y0, 0] = (px.G);
            imageData[x + x1, y + y0, 0] = (px.B + px.R) / 2;      // magenta
            imageData[x + x0, y + y1, 0] = (px.G + px.B) / 2;      // cyan
            imageData[x + x1, y + y1, 0] = (px.R + px.G) / 2;      // yellow
            px = bmp.GetPixel(x / 2, (y / 2) + 1);
            imageData[x + x0, y + y2, 0] = (px.B + px.R) / 2;      // magenta
            imageData[x + x1, y + y2, 0] = (px.G);
            imageData[x + x0, y + y3, 0] = (px.G + px.B) / 2;      // cyan
            imageData[x + x1, y + y3, 0] = (px.R + px.G) / 2;      // yellow
        }
        private void LRGBData(int x, int y)
        {
            Color px = bmp.GetPixel(x / 2, y / 2);
            imageData[x + x0, y + y0, 0] = px.GetBrightness() * 255;
            imageData[x + x1, y + y0, 0] = (px.R);
            imageData[x + x0, y + y1, 0] = (px.R);
            imageData[x + x1, y + y1, 0] = px.GetBrightness() * 255;
            px = bmp.GetPixel((x / 2) + 1, y / 2);
            imageData[x + x2, y + y0, 0] = px.GetBrightness() * 255;
            imageData[x + x3, y + y0, 0] = (px.G);
            imageData[x + x2, y + y1, 0] = (px.G);
            imageData[x + x3, y + y1, 0] = px.GetBrightness() * 255;
            px = bmp.GetPixel(x / 2, (y / 2) + 1);
            imageData[x + x0, y + y2, 0] = px.GetBrightness() * 255;
            imageData[x + x1, y + y2, 0] = (px.G);
            imageData[x + x0, y + y3, 0] = (px.G);
            imageData[x + x1, y + y3, 0] = px.GetBrightness() * 255;
            px = bmp.GetPixel((x / 2) + 1, (y / 2) + 1);
            imageData[x + x2, y + y2, 0] = px.GetBrightness() * 255;
            imageData[x + x3, y + y2, 0] = (px.B);
            imageData[x + x2, y + y3, 0] = (px.B);
            imageData[x + x3, y + y3, 0] = px.GetBrightness() * 255;
        }
        private void ColorData(int x, int y)
        {
            imageData[x, y, 0] = (bmp.GetPixel(x, y).R);
            imageData[x, y, 1] = (bmp.GetPixel(x, y).G);
            imageData[x, y, 2] = (bmp.GetPixel(x, y).B);
        }

        /// <summary>
        /// Write all cooler control variable values to the log file
        /// </summary>
        internal void LogCoolerVariables()
        {
            Log.LogMessage("LogCoolerVariables", "Heat sink temperature: {0}", heatSinkTemperature);
            Log.LogMessage("LogCoolerVariables", "Set point temperature: {0}", setCcdTemperature);
            Log.LogMessage("LogCoolerVariables", "Target temperature: {0}", targetCcdTemperature);
            Log.LogMessage("LogCoolerVariables", "Maximum delta T: {0}", coolerDeltaTMax);
            Log.LogMessage("LogCoolerVariables", "Set point minimum temperature: {0}", coolerSetPointMinimum);
            Log.LogMessage("LogCoolerVariables", "Time to set point: {0}", coolerTimeToSetPoint);
            Log.LogMessage("LogCoolerVariables", "Reset to ambient: {0}", coolerResetToAmbient);
            Log.LogMessage("LogCoolerVariables", "Cooler enabled at power up: {0}", coolerPowerUpState);
            Log.LogMessage("LogCoolerVariables", "Cooler fluctuation: {0}", coolerFluctuation);
            Log.LogMessage("LogCoolerVariables", "Temperature overshoot: {0}", coolerOvershoot);
            Log.LogMessage("LogCoolerVariables", "Cooler under damped cycles: {0}", coolerUnderDampedCycles);
            Log.LogMessage("LogCoolerVariables", "Cooler mode: {0}", coolerMode);
        }

        /// <summary>
        /// Constrain the supplied temperature to a specified range
        /// </summary>
        /// <param name="Temperature">Temperature to be constrained</param>
        /// <param name="FirstBound">First temperature range bound</param>
        /// <param name="SecondBound">Second temperature range bound</param>
        /// <returns>Constrained temperature</returns>
        private double ConstrainToTemperatureRange(double Temperature, double FirstBound, double SecondBound)
        {
            double retVal;
            retVal = Temperature; // Initialise the return value with the supplied temperature

            if (FirstBound == SecondBound) // The bounds are identical so only one possible valid value
            {
                if (Temperature != FirstBound) // Temperature is outside the valid value
                {
                    retVal = FirstBound;
                    Log.LogMessage("Timer", @"CCD temperature {0:+0.00;-0.00;' '0.00} constrained to the range {1:+0.00;-0.00;' '0.00} to {2:+0.00;-0.00;' '0.00}", Temperature, FirstBound, SecondBound);
                }
            }
            else if (FirstBound < SecondBound) // First is less than second
            {
                if (Temperature < FirstBound) // Temperature is lower than first bound
                {
                    retVal = FirstBound;
                    Log.LogMessage("Timer", @"CCD temperature {0:+0.00;-0.00;' '0.00} constrained to the range {1:+0.00;-0.00;' '0.00} to {2:+0.00;-0.00;' '0.00}", Temperature, FirstBound, SecondBound);
                }
                if (Temperature > SecondBound) // Temperature is higher than second bound
                {
                    retVal = SecondBound;
                    Log.LogMessage("Timer", @"CCD temperature {0:+0.00;-0.00;' '0.00} constrained to the range {1:+0.00;-0.00;' '0.00} to {2:+0.00;-0.00;' '0.00}", Temperature, FirstBound, SecondBound);
                }
            }
            else // Second is less than first
            {
                if (Temperature > FirstBound) // Temperature is higher than first bound
                {
                    retVal = FirstBound;
                    Log.LogMessage("Timer", @"CCD temperature {0:+0.00;-0.00;' '0.00} constrained to the range {1:+0.00;-0.00;' '0.00} to {2:+0.00;-0.00;' '0.00}", Temperature, FirstBound, SecondBound);
                }
                if (Temperature < SecondBound) // Temperature is lower than second bound
                {
                    retVal = SecondBound;
                    Log.LogMessage("Timer", @"CCD temperature {0:+0.00;-0.00;' '0.00} constrained to the range {1:+0.00;-0.00;' '0.00} to {2:+0.00;-0.00;' '0.00}", Temperature, FirstBound, SecondBound);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Returns the cooling constant that will get the CCD temperature within a specified distance of the endpoint temperature in the given time
        /// </summary>
        /// <param name="TimeToSetpoint">Time in which the change has to be effected</param>
        /// <returns>Cooling constant to use in Newton's cooling equation</returns>
        /// <remarks>
        /// Newton's cooling equation for this cooler gives temperature (T) as a function of time (t):                     T = SetpointTemperature + (StartTemperature - SetpointTemperature) * e^(-k*t)   ...1
        /// Which can also be shown as:                                                                                    T = SetpointTemperature + TemperatureChange * e^(-k*t)   ...2
        /// Rearranging gives the cooling constant k as:                                                                   k = -ln[(T - SetpointTemperature) / TemperatureChange] / t   ...3    Differing bracket styles just used to aid clarity
        ///
        /// The cooling equation shows that the temperature exponentially approaches the setpoint and thus requires infinite time to achieve the final temperature, assuming infinite precision observation and calculation.
        /// To work round this, in the simulator we define that the setpoint is reached when the CCD temperature reaches a small offset (COOLER_SETPOINT_REACHED_OFFSET) from the setpoint e.g. 0.1C.
        /// 
        /// When the cooler is at the offset temperature (setpoint + COOLER_SETPOINT_REACHED_OFFSET, equation 3 gives:     k = -ln[(TemperatureAtoffset - SetpointTemperature) / TemperatureChange] / TimeToOfffset   ...4    
        /// 
        /// "TemperatureAtoffset" in equation 4 can be replaced with (SetpointTemperature + SetPointReachedOffset) giving: k = -ln[(SetpointTemperature + SetPointReachedOffset - SetpointTemperature) / TemperatureChange] / TimeToOfffset   ...5
        /// which simplifies to:                                                                                           k = -ln[SetPointReachedOffset / TemperatureChange] / t   ...6
        ///
        /// Equation 6 allows the cooling constant to be calculated that will cause the cooler reach a temperature of "SetPointReachedOffset" degrees from the setpoint in "t" seconds when cooling over "TemperatureChange" degrees
        /// 
        /// For this cooler, "TemperatureCahnge" is taken as the maximum delta T that the cooler can achieve, which can only be changed through configuration. This means that the cooler shows realistic behaviour where 
        /// small temperature changes are achieved more rapidly than larger changes.
        ///
        /// The COOLER_SETPOINT_REACHED_OFFSET variable determines how far the cooler proceeds down the cooling curve before declaring that it has arrived. Larger values make use of the earlier part of the curve which 
        /// give a slower descent towards the setpoint and less time inching in towards it. Smaller values use more of the curve, which results in a quicker descent to the vicinity of the setpoint and then a longer 
        /// time spent inching towards the exact value. The value of 0.1sec has been determined experimentally to provide realistic behaviour.
        ///    
        ///</remarks>
        internal double CalculateCoolerConstant(double TimeToSetpoint, double coolerDeltaTMax)
        {
            double coolingConstant = -Math.Log(COOLER_SETPOINT_REACHED_OFFSET / coolerDeltaTMax) / TimeToSetpoint;
            Log.LogMessage("CoolingConstant", "Cooler setpoint offset: {0:0.0}, Maximum delta T: {1:+0.00;-0.00;' '0.00}, Time to setpoint: {2:0.0}, Cooling constant: {3}", COOLER_SETPOINT_REACHED_OFFSET, coolerDeltaTMax, TimeToSetpoint, coolingConstant);

            return coolingConstant;
        }

        #endregion

        #region Checks

        private void CheckConnected(string message)
        {
            if (!connected)
            {
                Log.LogMessage("NotConnected", message);
                throw new NotConnectedException(message);
            }
        }

        private void CheckRange(string identifier, double min, double value, double max)
        {
            if (value > max || value < min)
            {
                Log.LogMessage(identifier, "{0} is not in range {1} to {2}", value, min, max);
                throw new InvalidValueException(identifier, value.ToString(), string.Format(CultureInfo.InvariantCulture, "{0} to {1}", min, max));
            }
        }

        /// <summary>
        /// Throw a PropertyNotImplementedException if a property Get is not supported
        /// </summary>
        /// <param name="capabilityName">Property name</param>
        /// <param name="enabled">True for capability supported, false for capability not supported</param>
        private void CheckCapabilityEnabled(string capabilityName, bool enabled)
        {
            if (!enabled)
            {
                Log.LogMessage(capabilityName, "Not Implemented");
                throw new PropertyNotImplementedException(capabilityName, false);
            }
        }

        private void CheckReady(string identifier)
        {
            if (!imageReady)
            {
                Log.LogMessage(identifier, "image not ready");
                throw new InvalidOperationException("Can't read " + identifier + " when no image is ready");
            }
        }

        private void CheckSupportedInThisInterfaceVersion(string memberName, int minimumSupportedInterfaceVersion)
        {
            if (interfaceVersion < minimumSupportedInterfaceVersion)
            {
                Log.LogMessage(memberName, $"The {memberName} member is not present in ICameraV{interfaceVersion}, throwing an ASCOM.InvalidOperationException");
                throw new InvalidOperationException($"The {memberName} member is not present in ICameraV{interfaceVersion}");
            }
        }

        #endregion
    }
}
