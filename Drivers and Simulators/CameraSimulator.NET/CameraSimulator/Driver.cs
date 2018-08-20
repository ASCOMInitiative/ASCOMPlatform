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

namespace ASCOM.Simulator
{
    /// <summary>
    /// Your driver's ID is ASCOM.Simulator.Camera
    /// The Guid attribute sets the CLSID for ASCOM.Simulator.Camera
    /// The ClassInterface/None attribute prevents an empty interface called
    /// _Camera from being created and used as the [default] interface
    /// </summary>
    [Guid("12229c31-e7d6-49e8-9c5d-5d7ff05c3bfe"), ClassInterface(ClassInterfaceType.None), ComVisible(true)]
    public class Camera : ICameraV2
    {
        // Driver ID and descriptive string that shows in the Chooser
        private static string s_csDriverID = "ASCOM.Simulator.Camera";
        private static string s_csDriverDescription = "Camera V2 simulator";

        #region profile string constants
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
        private const string STR_CoolerAmbientTemperature = "CoolerAmbientTemperature";
        private const string STR_CoolerSetPoint = "CoolerSetPoint";
        private const string STR_CoolerDeltaTMax = "CoolerDeltaTMax";
        private const string STR_CoolerMode = "CoolerMode";
        private const string STR_CoolerTimeToSetPoint = "CoolerTimeToSetPoint";
        private const string STR_CoolerResetToAmbient = "CoolerResetToAmbient";
        #endregion

        #region Camera cooler modes and initial conditions
        // When adding a new cooler mode, define its name here and add it the coolerModes array so that it will show up in the list of options. Then update all places where switch statements are used to configure behaviour beased on coolingMode.
        internal const string COOLERMODE_START_AT_SETPOINT = "Straight to set point";
        internal const string COOLERMODE_DAMPED = "Damped approach to set point";
        internal const string COOLERMODE_UNDER_DAMPED = "Under damped approach to set point";
        internal const string COOLERMODE_NEVER_GETS_TO_SETPOINT = "Never gets to set point";
        internal List<string> coolerModes = new List<string>() { COOLERMODE_START_AT_SETPOINT, COOLERMODE_DAMPED, COOLERMODE_UNDER_DAMPED, COOLERMODE_NEVER_GETS_TO_SETPOINT };

        // Cooler default characteristics - See void coolerTimer_Elapsed(object sender, ElapsedEventArgs e) for a description of cooler operation
        internal const double COOLER_AMBIENT_TEMPERATURE_DEFAULT = 10; // Ambient temperarure (C) when the camera is initially created  
        internal const double COOLER_CCD_SET_POINT_DEFAULT = -20; // Camera initial set point
        internal const double COOLER_DELTAT_MAX_DEFAULT = 40; // Maximum temperature (C) below ambient to which the camera cooler can cool when the cooler is running at 100%
        internal const double COOLER_TIME_TO_SETPOINT_DEFAULT = 120; // Time (seconds) to reach the CCD temperature set point when starting from ambient
        internal const bool COOLER_RESET_TO_AMBIENT_DEFAULT = true; // Will the CCD temperature reset to ambient on connect or behave like a normal cooler where temperatuyre depends on past cooling experience
        internal const string COOLER_COOLERMODE_DEFAULT = COOLERMODE_DAMPED; // Default mode on initial installation
        private const double NEVER_GETS_TO_SETPOINT_INDEX_REDUCTION_FACTOR = 0.7; // Arbitary factor to reduce the cooler profile index so that the returned curve position is earlier than exoected. This produces a higher CCD temperature, ensuring that the CCD never reaches the setpoint
        private const double COOLER_WARMING_RATE = 0.05; // Cooler will warm by 5% of the difference between the CCD temperature and ambient temperature each second when the cooler is off
        private const double COOLER_USE_FULL_POWER = 0.0; // Fraction of the cooling curve above which cooler power will be reported as 100%. e.g. 0.9 means the first 90% of the curve will be reported as 100% cooler power and the last 20% as the calculated power.
        internal const int NUMBER_OF_COOLER_CURVE_POINTS = 60;

        // Cooler beahioural curves comprising 60 points that can be scaled to time and temperature as required. The first curve element must be 1.0 and the last curve element must be 0.0
        private double[] curveDamped = new double[NUMBER_OF_COOLER_CURVE_POINTS] {
            1.0,0.89259,0.750047908,0.63045638,0.530111475,0.445907425,0.375240012,0.315925552,0.266132976,0.22432687,
            0.189219723,0.159731887,0.134958005,0.114138861,0.096637777,0.081920823,0.069540227,0.059120457,0.050346559,0.042954372,
            0.03672233,0.031464585,0.027025236,0.0232735,0.020099654,0.017411639,0.015132207,0.013196532,0.011550205,0.010147557,
            0.008950245,0.007926074,0.007048005,0.006293324,0.005642946,0.005080829,0.004593486,0.004169575,0.003799549,0.003475373,
            0.003190281,0.002938566,0.00271542,0.002516782,0.002339222,0.002179841,0.002036183,0.001906168,0.001788032,0.001680274,
            0.001581616,0.001490971,0.001407406,0.001330125,0.001258445,0.001191775,0.001129608,0.001071504,0.001017081,0.0
            };

        private double[] curveUnderDamped = new double[NUMBER_OF_COOLER_CURVE_POINTS] {
            1.0,0.8572,0.67627184,0.519728877,0.385775548,0.272496269,0.177925516,0.100103627,0.037120191,-0.01285323,
            -0.051539444,-0.080539592,-0.101323405,-0.115224605,-0.12344042,-0.127034314,-0.12694117,-0.123974258,-0.118833454,-0.112114224,
            -0.10431702,-0.095856773,-0.087072246,-0.078235068,-0.069558309,-0.061204508,-0.05329308,-0.045907084,-0.039099326,-0.03289781,
            -0.027310557,-0.02232982,-0.017935724,-0.014099395,-0.010785593,-0.007954915,-0.005565614,-0.003575064,-0.001940923,-0.00062204,
            0.000420871,0.001224746,0.001823844,0.002249557,0.002530323,0.002691629,0.002756079,0.002743511,0.002671156,0.002553817,
            0.002404066,0.002232447,0.002047684,0.001856882,0.00166572,0.001478634,0.001298995,0.001129259,0.000971118,0.0
            };

        //private int coolerCurveIndex;
        private DateTime coolerStartedTime;
        private TimeSpan coolerRunningTime;

        #endregion

        #region internal properties

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
        internal double setCcdTemperature;

        // Cooler simulator parameters
        //internal double coolerSetPoint;
        internal double coolerDeltaTMax;
        internal string coolerMode;
        internal double coolerTimeToSetPoint;
        internal bool coolerResetToAmbient;


        // Gain
        internal ArrayList gains;
        internal short gainMin;
        internal short gainMax;
        private short gain;

        // Exposure
        internal bool canAbortExposure;
        internal bool canStopExposure;
        internal double exposureMax;
        internal double exposureMin;
        internal double exposureResolution;
        private double lastExposureDuration;
        private string lastExposureStartTime;

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Member")]
        private float[,,] imageData;    // room for a 3 plane colour image
        private bool darkFrame;
        internal bool omitOddBins; // True if bins of 3, 5, 7 etc. should throw NotImplementedExceptions

        internal bool connected = false;
        internal CameraStates cameraState = CameraStates.cameraIdle;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Member")]
        private int[,] imageArray;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Member")]
        private object[,] imageArrayVariant;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Member")]
        private int[,,] imageArrayColour;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Member")]
        private object[,,] imageArrayVariantColour;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string lastError = string.Empty;

        private System.Timers.Timer exposureTimer;
        private System.Timers.Timer coolerTimer;

        // supported actions
        // should really use consts for the action names, 
        private ArrayList supportedActions = new ArrayList { "SetFanSpeed", "GetFanSpeed" };

        // SetFanSpeed, GetFanSpeed. These commands control a hypothetical CCD camera heat sink fan, range 0 (off) to 3 (full speed) 
        private int fanMode;

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
            if (this.exposureTimer != null)
                this.exposureTimer.Dispose();
            if (this.coolerTimer != null)
                this.coolerTimer.Dispose();
        }
        #endregion

        #region ASCOM Registration
        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
        {
            using (Profile P = new Profile())
            {
                P.DeviceType = "Camera";					//  Requires Helper 5.0.3 or later
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "t"), ComRegisterFunction]
        private static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "t"), ComUnregisterFunction]
        private static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }
        #endregion

        #region Cooler Timer
        /// <summary>
        /// Adjust the ccd temperature and power once a second
        /// </summary>
        /// <remarks>
        /// COOLER CAPABILITIES - The user can configure
        ///     1) Cooling bhaviour - a) well behaved fall to the setpoint, b) falls to the setpoint but overshoots before returing to it, c) starts at and maintains the setpoint, d) falls but never gets to the setpoint
        ///     2) The ambient temperature
        ///     3) The cooler setpoint
        ///     4) The cooler maximum delta T
        ///     5) The time to reach the setpoint
        ///     6) Whether the cooler always starts at ambient temperature when the cooler is turned on or whether it starts at the temperature it has "warmed up to" since the cooler was turned off.
        ///     
        /// To provide extensibility and configurablilty, cooler behaviour is determined by a set of predefined "cooler curves". Each of these is held in an array such as curveDamped and curveUnderDamped. Array values lie between 1.0 and -1.0 and represent the 
        /// CCD temperature as a fraction of the difference between the ambient and setpoint temperatures; negative values represent overshoot beyond the configured setpoint. The array index represents increasing time from turning on the cooler to arrival at the setpoint.
        /// 
        /// This approach has been adopted because the curve values can reperesnt any desired cooling characteristic, can be scaled to accommodate any ambient and setpoint temperatures and can also be scaled to operate over any length of time to 
        /// reach the setpoint. At the time of wrting, each curve contains 60 points, which are considered enough to describe the curve in sufficient detail for simulation purposes. Linear interpolation between array index 
        /// values is used to provide smooth, unquantised changes in curve values.
        /// 
        /// </remarks>
        private void coolerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (coolerOn) // We are cooling or at setpoint
            {
                coolerRunningTime = DateTime.Now.Subtract(coolerStartedTime); // Update the cooler running time

                // Calculate the cooler index position and the interpolation fraction
                double coolerCurveFraction = (double)NUMBER_OF_COOLER_CURVE_POINTS * coolerRunningTime.TotalSeconds / coolerTimeToSetPoint; // Fraction of the cooler curve that has been traversed so far - as a double
                int coolerCurveIndex = Math.Min((int)Math.Floor(coolerCurveFraction), NUMBER_OF_COOLER_CURVE_POINTS - 1); // Convert the cooler curve fraction into an integer, which is the array index to use to find the base temperature fraction. Also ensure that the index is within the array range
                double interpolationFraction = coolerCurveFraction - (double)coolerCurveIndex; // Calculate the remaiing fraction of an index position that wil be used during interpolation

                //Log.LogMessage("Timer", "Cooler running time: {0}, Cooler curve fraction: {1}, Cooler curve index: {2}, Interpolation fraction: {3}", coolerRunningTime, coolerCurveFraction, coolerCurveIndex, interpolationFraction);

                // Determine whether we are still cooling down or whether we are at the end of the cooling down period
                if (coolerCurveIndex < NUMBER_OF_COOLER_CURVE_POINTS) // We are still cooling so calculate the new CCD temperature
                {
                    switch (coolerMode)
                    {
                        case COOLERMODE_START_AT_SETPOINT: // No curve needed for this behaviour mode
                            ccdTemperature = setCcdTemperature;
                            Log.LogMessage("Timer", "StartAtSetPoint - CCD temperature is {0}", ccdTemperature);
                            break;
                        case COOLERMODE_DAMPED: // Temperature calculated from the damped curve
                            ccdTemperature = setCcdTemperature + (heatSinkTemperature - setCcdTemperature) * ReturnCurveValue(curveDamped, coolerCurveIndex, interpolationFraction);
                            Log.LogMessage("Timer", "DampedMode - CCD temperature is {0}, index: {1}", ccdTemperature, coolerCurveIndex);
                            break;
                        case COOLERMODE_UNDER_DAMPED: // Temperature calculated from the under damped curve
                            ccdTemperature = setCcdTemperature + (heatSinkTemperature - setCcdTemperature) * ReturnCurveValue(curveUnderDamped, coolerCurveIndex, interpolationFraction); ;
                            Log.LogMessage("Timer", "UnderDampedMode - CCD temperature is {0}, index: {1}", ccdTemperature, coolerCurveIndex);
                            break;
                        case COOLERMODE_NEVER_GETS_TO_SETPOINT: // Temperature calculated from the damped curve but using the value from an earlier index position that will yield a smaller temperature reduction, ensuring that the CCD never reaches the setpoint
                            ccdTemperature = setCcdTemperature + (heatSinkTemperature - setCcdTemperature) * ReturnCurveValue(curveDamped, (int)(NEVER_GETS_TO_SETPOINT_INDEX_REDUCTION_FACTOR * coolerCurveIndex), interpolationFraction);
                            Log.LogMessage("Timer", "MissesSetpoint - CCD temperature is {0}, index: {1}", ccdTemperature, coolerCurveIndex);
                            break;
                        default: // Warning messge for future camera driver developers that they need to update this code if a new curve is introduced
                            System.Windows.Forms.MessageBox.Show("Unknown cooler mode! - " + coolerMode);
                            break;
                    }

                    if (ccdTemperature < (heatSinkTemperature - coolerDeltaTMax))
                    {
                        ccdTemperature = heatSinkTemperature - coolerDeltaTMax; // Ensure that the temperature can never go below the value corresponding to 100% cooler power
                        Log.LogMessage("Timer", "CCD temperature limited to Ambient - DeltaTMax", ccdTemperature, coolerCurveIndex);
                    }
                }
                else // The cooler has now reached the set point
                {
                    switch (coolerMode)
                    {
                        case COOLERMODE_START_AT_SETPOINT: // The cooler has reached the setpoint so ensure that the CCD temperature is exactly the setpoint temperature
                        case COOLERMODE_DAMPED:
                        case COOLERMODE_UNDER_DAMPED:
                            Log.LogMessage("Timer", "CCD is at temperature: {0}, index: {1}", ccdTemperature, coolerCurveIndex);
                            ccdTemperature = setCcdTemperature;
                            break;
                        case COOLERMODE_NEVER_GETS_TO_SETPOINT: // Adjust the CCD temperature so that it is at its final offset from the setpoint.
                            ccdTemperature = setCcdTemperature + (heatSinkTemperature - setCcdTemperature) * ReturnCurveValue(curveDamped, (int)(NEVER_GETS_TO_SETPOINT_INDEX_REDUCTION_FACTOR * NUMBER_OF_COOLER_CURVE_POINTS - 1), interpolationFraction);
                            Log.LogMessage("Timer", "CCD never gets to temperature: {0}, index: {1}", ccdTemperature, coolerCurveIndex);
                            break;
                        default: // Warning messge for future camera driver developers that they need to update this code if a new curve is introduced
                            System.Windows.Forms.MessageBox.Show("Unknown cooler mode! - " + coolerMode);
                            break;
                    }
                }

                // Set the cooler power
                switch (coolerMode)
                {
                    case COOLERMODE_START_AT_SETPOINT: // Set cooler power based on fraction of the maxium delta T that is in use
                    case COOLERMODE_DAMPED:
                    case COOLERMODE_UNDER_DAMPED:
                        double powerFractionAtSetPoint = (heatSinkTemperature - setCcdTemperature) / coolerDeltaTMax;

                        coolerPower = 100.0 * (powerFractionAtSetPoint + ((1.0 - powerFractionAtSetPoint) * ((ccdTemperature - setCcdTemperature) / (heatSinkTemperature - setCcdTemperature))));
                        //Log.LogMessage("Timer", "Cooler power parameters: Power fraction at setpoint: {0}, Folded ccd temperature: {1}, Cooler power: {2}",powerFractionAtSetPoint,foldedCcdTemperature, coolerPower);

                        // Now force the value to 100% for the first part of the cooling curve
                        if ((heatSinkTemperature - ccdTemperature) / (heatSinkTemperature - setCcdTemperature) < COOLER_USE_FULL_POWER)
                        {
                            coolerPower = 100.0;
                            Log.LogMessage("Timer", "Cooler power - Forced to 100% - {0}", coolerPower);
                        }
                        else
                        {
                            Log.LogMessage("Timer", "Cooler power - Normal behaviour - Power: {0}, AmbientTemperature: {1}, CCDTemperature: {2}, Delta TMax: {3}", coolerPower, heatSinkTemperature, ccdTemperature, coolerDeltaTMax);
                        }
                        break;
                    case COOLERMODE_NEVER_GETS_TO_SETPOINT: // Set cooler power to 100% because we cannot make the set point
                        coolerPower = 100.0;
                        break;
                    default: // Warning messge for future camera driver developers that they need to update this code if a new curve is introduced
                        System.Windows.Forms.MessageBox.Show("Unknown cooler mode! - " + coolerMode);
                        break;
                }
            }
            else // We are warning up or at ambient temperature
            {
                double distanceFromAmbient = ccdTemperature - heatSinkTemperature; // Calculate how far the CCD is from ambient temperature
                if (distanceFromAmbient != 0.0) // The CCD is not at ambient temperature
                {
                    if (Math.Abs(distanceFromAmbient) > 0.01) // We are still not at ambient so update the CCD temperature using the cooler warming rate 
                    {
                        ccdTemperature += (heatSinkTemperature - ccdTemperature) * COOLER_WARMING_RATE;
                        Log.LogMessage("Timer", "CCD is NOT at ambient temperature: {0}, CCD temperature changed to {1}, Cooler power: {2}", heatSinkTemperature, ccdTemperature, coolerPower);
                    }
                    else // We are now very close to ambient so align the values
                    {
                        ccdTemperature = heatSinkTemperature;
                        Log.LogMessage("Timer", "CCD is NOW AT ambient temperature: {0}, Cooler power: {1}", ccdTemperature, coolerPower);
                    }
                }
                else // The CCD is at ambient temperature
                {
                    Log.LogMessage("Timer", "CCD is at ambient temperature: {0}, Cooler power: {1}", heatSinkTemperature, coolerPower);
                }
            }
        }

        double ReturnCurveValue(double[] curve, int index, double interpolationFraction)
        {
            double curveValue = curve[index];
            double retVal = curveValue;

            if (index < curve.GetUpperBound(0)) // If we are not at the last curve array member interpolate the fractional value
            {
                double fraction = interpolationFraction * (curve[index + 1] - curve[index]);
                retVal = curveValue + fraction;
                Log.LogMessage("ReturnCurveValue", "Index: {0}, Interpolation fraction: {1}, Curve value: {2}, Fraction: {3}, Return value: {4}", index, interpolationFraction, curveValue, fraction, retVal);
            }
            else
            {
                Log.LogMessage("ReturnCurveValue", "Index: {0} is >=curve.GetUpperBound(0): {1}", index, curve.GetUpperBound(0));
            }

            return retVal;
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
            get { return this.supportedActions; }
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
            if (this.supportedActions.Contains(ActionName))
            {
                switch (ActionName)
                {
                    case "SetFanSpeed":
                        int fanMode;
                        if (int.TryParse(ActionParameters, out fanMode))
                        {
                            if (fanMode >= 0 && fanMode <= 3)
                            {
                                this.fanMode = fanMode;
                                return string.Empty;
                            }
                        }
                        // value not in range
                        throw new InvalidValueException("Action-SetFanMode", ActionParameters, "0 to 3");
                    case "GetFanSpeed":
                        return this.fanMode.ToString();
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
            if (!this.canAbortExposure)
            {
                Log.LogMessage("AbortExposure", "CanAbortExposure is false");
                throw new ASCOM.MethodNotImplementedException("AbortExposure");
            }

            Log.LogMessage("AbortExposure", "start");
            switch (this.cameraState)
            {
                case CameraStates.cameraWaiting:
                case CameraStates.cameraExposing:
                case CameraStates.cameraReading:
                case CameraStates.cameraDownload:
                    // these are all possible exposure states so we can abort the exposure
                    this.exposureTimer.Enabled = false;
                    this.cameraState = CameraStates.cameraIdle;
                    this.imageReady = false;
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
                return this.binX;
            }
            set
            {
                CheckConnected("Can't set BinX when not connected");
                CheckRange("BinX", 1, value, maxBinX);
                if ((maxBinX >= 4) & omitOddBins & ((value % 2) > 0) & (value >= 3)) // Must be an odd value of 3 or greater when maxbin is 4 or greater
                {
                    Log.LogMessage("BinX", "Odd bin value {0} is invalid", value);
                    throw new InvalidValueException("BinX", value.ToString("d2", CultureInfo.InvariantCulture), string.Format(CultureInfo.InvariantCulture, "1 and even bin values between 2 and {0}", this.MaxBinX));
                }

                this.binX = value;
                if (!this.canAsymmetricBin)
                    this.binY = value;
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
                return this.binY;
            }
            set
            {
                CheckConnected("Can't set BinY when not connected");
                CheckRange("BinY", 1, value, this.maxBinY);

                if ((maxBinY >= 4) & omitOddBins & ((value % 2) > 0) & (value >= 3)) // Must be an odd value of 3 or greater when maxbin is 4 or greater
                {
                    Log.LogMessage("BinY", "Odd bin value {0} is invalid", value);
                    throw new InvalidValueException("BinY", value.ToString("d2", CultureInfo.InvariantCulture), string.Format(CultureInfo.InvariantCulture, "1 and even bin values between 2 and {0}", this.MaxBinY));
                }

                this.binY = value;
                if (!this.canAsymmetricBin)
                    this.binX = value;
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
                Log.LogMessage("CCDTemperature", "get {0}", this.ccdTemperature);
                return this.ccdTemperature;
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
                Log.LogMessage("CameraState", this.cameraState.ToString());
                return this.cameraState;
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
                Log.LogMessage("CameraXSize", "get {0}", this.cameraXSize);
                return this.cameraXSize;
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
                Log.LogMessage("CameraYSize", "get {0}", this.cameraYSize);
                return this.cameraYSize;
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
                Log.LogMessage("CanAbortExposure", "get {0}", this.canAbortExposure);
                return this.canAbortExposure;
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
                return this.canAsymmetricBin;
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
                return this.canGetCoolerPower;
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
                return this.canPulseGuide;
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
                return this.canSetCcdTemperature;
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
                return this.canStopExposure;
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
                return this.connected;
            }
            set
            {
                Log.LogMessage("Connected", "set {0}", value);
                if (value)
                    ReadImageFile();
                this.connected = value;

                // Start the cooler timer on initial connect
                if (this.coolerTimer == null)
                {
                    coolerTimer = new System.Timers.Timer();
                    coolerTimer.Elapsed += new ElapsedEventHandler(coolerTimer_Elapsed);
                    coolerTimer.Interval = 1000;
                    coolerTimer.Enabled = true;
                    Log.LogMessage("Connected", "Cooler timer started");
                }
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
                CheckCapability("CoolerOn", this.hasCooler);
                Log.LogMessage("CoolerOn", "get {0}", coolerOn);
                return this.coolerOn;
            }
            set
            {

                CheckConnected("Can't set CoolerOn when not connected");
                CheckCapability("CoolerOn", this.hasCooler);
                Log.LogMessage("CoolerOn", "set {0}", value);
                this.coolerOn = value;

                if (coolerOn)// Set the correct cooler temperature starting point, depending on configuration and usage hostory
                {
                    coolerStartedTime = DateTime.Now; // Save the time that the cooler was started
                    coolerPower = 100; // Set the cooler power to full on

                    // Set the starting position on the temperature curve based on how much the CCD has warmed up since cooling was turned off
                    switch (coolerMode)
                    {
                        case COOLERMODE_START_AT_SETPOINT:
                            coolerRunningTime = new TimeSpan(0, 0, (int)coolerTimeToSetPoint);
                            Log.LogMessage("CoolerOn", "StartAtSetPoint - Setting cooler curve index to 0", ccdTemperature);
                            break;
                        case COOLERMODE_DAMPED:
                            coolerRunningTime = CalculateCoolerRunningTime("DampedMode", curveDamped, 1.0);
                            break;
                        case COOLERMODE_UNDER_DAMPED:
                            coolerRunningTime = CalculateCoolerRunningTime("UnderDampedMode", curveUnderDamped, 1.0);
                            break;
                        case COOLERMODE_NEVER_GETS_TO_SETPOINT:
                            coolerRunningTime = CalculateCoolerRunningTime("NeverGetsToSetPoint", curveDamped, NEVER_GETS_TO_SETPOINT_INDEX_REDUCTION_FACTOR);
                            break;
                        default:
                            System.Windows.Forms.MessageBox.Show("Unknown cooler mode! - " + coolerMode);
                            break;
                    }
                }
                else // Cooler off
                {
                    coolerPower = 0; // Set cooler power to zero because the cooler is off
                }
            }
        }

        private TimeSpan CalculateCoolerRunningTime(string name, double[] array, double scaleFactor)
        {
            double currentCoolerCurveValue;
            int coolerCurveIndex;
            int runningTime;
            TimeSpan newCoolerRunningTime;

            if (coolerResetToAmbient) // Configured to always start at ambient
            {
                newCoolerRunningTime = new TimeSpan(0, 0, 0);
                Log.LogMessage("CoolerRunningTime", "Cooler is configured to reset to ambient when cooling is turned on");
            }
            else
            {
                // Find the current CCD temperature position on the cooling curve and then find the index position of the nearewst value and set this as the start point
                currentCoolerCurveValue = (ccdTemperature - setCcdTemperature) / (heatSinkTemperature - setCcdTemperature);

                coolerCurveIndex = -1;
                do
                {
                    coolerCurveIndex += 1;
                    //Log.LogMessage("CoolerOn", "DampedMode FindIndex - Curve damped: {0}, Current cooler curve vValue: {1}, Cooler curve index: {2}", curveDamped[coolerCurveIndex], currentCoolerCurveValue, coolerCurveIndex);
                } while ((array[coolerCurveIndex] > currentCoolerCurveValue) & (coolerCurveIndex != NUMBER_OF_COOLER_CURVE_POINTS - 1));

                // Calculate cooler running time that would give rise to this curve index being used
                runningTime = Convert.ToInt32(coolerTimeToSetPoint * (double)coolerCurveIndex * scaleFactor / (double)NUMBER_OF_COOLER_CURVE_POINTS);
                newCoolerRunningTime = new TimeSpan(0, 0, runningTime);

                Log.LogMessage("CoolerRunningTime", "{0} - CCD temperature is {0}, index: {1}, running time: {2}", name, ccdTemperature, coolerCurveIndex, runningTime);
            }

            return newCoolerRunningTime;
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
                CheckCapability("CoolerPower", hasCooler);
                Log.LogMessage("CoolerPower", "get {0}", coolerPower);
                return this.coolerPower;
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
                if (this.interfaceVersion == 1)
                {
                    Log.LogMessage("Description", "Simulated V1 Camera");
                    return "Simulated V1 Camera";
                }
                else
                {
                    Log.LogMessage("Description", "Simulated {0} camera {1}", this.sensorType, this.sensorName);
                    return string.Format(CultureInfo.CurrentCulture, "Simulated {0} camera {1}", this.sensorType, this.sensorName);
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
                return this.electronsPerADU;
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
                return this.fullWellCapacity;
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
                return this.hasShutter;
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
                CheckCapability("HeatSinkTemperature", canSetCcdTemperature);
                Log.LogMessage("HeatSinkTemperature", "get {0}", heatSinkTemperature);
                return this.heatSinkTemperature;
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
                if (!this.imageReady)
                {
                    Log.LogMessage("ImageArray", "No Image Available");
                    throw new ASCOM.InvalidOperationException("There is no image available");
                }

                Log.LogMessage("ImageArray", "get");
                if (this.sensorType == SensorType.Color)
                    return this.imageArrayColour;
                else
                    return this.imageArray;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body")]
        public object ImageArrayVariant
        {
            get
            {
                CheckConnected("Can't read ImageArrayVariant when not connected");
                if (!this.imageReady)
                {
                    Log.LogMessage("ImageArrayVariant", "No Image Available");
                    throw new ASCOM.InvalidOperationException("There is no image available");
                }
                // convert to variant
                if (this.sensorType == SensorType.Color)
                {
                    this.imageArrayVariantColour = new object[imageArrayColour.GetLength(0), imageArrayColour.GetLength(1), 3];
                    for (int i = 0; i < imageArrayColour.GetLength(1); i++)
                    {
                        for (int j = 0; j < imageArrayColour.GetLength(0); j++)
                        {
                            for (int k = 0; k < 3; k++)
                                imageArrayVariantColour[j, i, k] = imageArrayColour[j, i, k];
                        }

                    }
                    return imageArrayVariantColour;
                }
                else
                {
                    this.imageArrayVariant = new object[imageArray.GetLength(0), imageArray.GetLength(1)];
                    for (int i = 0; i < imageArray.GetLength(1); i++)
                    {
                        for (int j = 0; j < imageArray.GetLength(0); j++)
                        {
                            imageArrayVariant[j, i] = imageArray[j, i];
                        }

                    }
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
                Log.LogMessage("ImageReady", "get {0}", this.imageReady);
                return this.imageReady;
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
                CheckCapability("IsPulseGuiding", canPulseGuide);
                var ipg = this.isPulseGuidingRa || this.isPulseGuidingDec;
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
                return this.lastExposureDuration;
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
                return this.lastExposureStartTime;
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
                return this.maxADU;
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
                return this.maxBinX;
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
                return this.maxBinY;
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
                return this.numX;
            }
            set
            {
                CheckConnected("Can't set NumX when not connected");
                Log.LogMessage("NumX", "set {0}", value);
                this.numX = value;
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
                return this.numY;
            }
            set
            {
                CheckConnected("Can't set NumY when not connected");
                Log.LogMessage("NumY", "set {0}", value);
                this.numY = value;
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
                return this.pixelSizeX;
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
                return this.pixelSizeY;
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
            if (!this.canPulseGuide)
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
                    if (this.pulseGuideRaTimer == null)
                    {
                        this.pulseGuideRaTimer = new System.Timers.Timer();
                        this.pulseGuideRaTimer.Elapsed += new System.Timers.ElapsedEventHandler(pulseGuideRaTimer_Elapsed);
                    }
                    this.isPulseGuidingRa = true;
                    this.pulseGuideRaTimer.Interval = Duration;
                    this.pulseGuideRaTimer.AutoReset = false;     // only one tick
                    this.pulseGuideRaTimer.Enabled = true;
                    break;
                case GuideDirections.guideNorth:
                case GuideDirections.guideSouth:
                    if (this.pulseGuideDecTimer == null)
                    {
                        this.pulseGuideDecTimer = new System.Timers.Timer();
                        this.pulseGuideDecTimer.Elapsed += new System.Timers.ElapsedEventHandler(pulseGuideDecTimer_Elapsed);
                    }
                    this.isPulseGuidingDec = true;
                    this.pulseGuideDecTimer.Interval = Duration;
                    this.pulseGuideDecTimer.AutoReset = false;     // only one tick
                    this.pulseGuideDecTimer.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void pulseGuideRaTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.isPulseGuidingRa = false;
        }

        private void pulseGuideDecTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.isPulseGuidingDec = false;
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
                CheckCapability("SetCCDTemperature", canSetCcdTemperature);
                Log.LogMessage("SetCCDTemperature", "get {0}", setCcdTemperature);
                return this.setCcdTemperature;
            }
            set
            {
                CheckConnected("Can't set SetCCDTemperature when not connected");
                CheckCapability("SetCCDTemperature", canSetCcdTemperature);
                CheckRange("SetCCDTemperature", heatSinkTemperature - coolerDeltaTMax, value, heatSinkTemperature); // Make sure the set value is in the supported range
                Log.LogMessage("SetCCDTemperature", "set {0}", value);
                if (coolerOn) // Cooler is on so this is an adjustment to the current setpoint
                {

                }
                this.setCcdTemperature = value;
            }
        }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            if (this.connected)
                throw new NotConnectedException("Can't set the CCD properties when connected");
            using (SetupDialogForm F = new SetupDialogForm())
            {
                F.InitProperties(this);
                if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    this.SaveToProfile();
            }
        }

        /// <summary>
        /// Starts an exposure. Use ImageReady to check when the exposure is complete.
        /// </summary>
        /// <param name="Duration">exxposure duration in seconds</param>
        /// <param name="Light">True if light frame, only valid if the camera has a shutter</param>
        /// <exception cref=" System.Exception">NumX, NumY, XBin, YBin, StartX, StartY, or Duration parameters are invalid.</exception>
        /// <exception cref=" System.Exception">CanAsymmetricBin is False and BinX != BinY</exception>
        /// <exception cref=" System.Exception">the exposure cannot be started for any reason, such as a hardware or communications error</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body")]
        public void StartExposure(double Duration, bool Light)
        {
            Log.LogStart("StartExposure", "Duration {0}, Light {1}", Duration, Light);
            CheckConnected("Can't set StartExposure when not connected");
            // check the duration, light frames only
            if (Light && (Duration > this.exposureMax || Duration < this.exposureMin))
            {
                this.lastError = "Incorrect exposure duration";
                Log.LogMessage("StartExposure", "Incorrect exposure Duration {0}", Duration);
                throw new ASCOM.InvalidValueException("StartExposure Duration",
                                                     Duration.ToString(CultureInfo.InvariantCulture),
                                                     string.Format(CultureInfo.InvariantCulture, "{0} to {1}", this.exposureMax, this.exposureMin));
            }
            //  binning tests
            if ((this.binX > this.maxBinX) || (this.binX < 1))
            {
                this.lastError = "Incorrect bin X factor";
                Log.LogMessage("StartExposure", "Incorrect BinX {0}", binX);
                throw new ASCOM.InvalidValueException("StartExposure BinX",
                                                    this.binX.ToString(CultureInfo.InvariantCulture),
                                                    string.Format(CultureInfo.InvariantCulture, "1 to {0}", this.maxBinX));
            }
            if ((this.binY > this.maxBinY) || (this.binY < 1))
            {
                this.lastError = "Incorrect bin Y factor";
                Log.LogMessage("StartExposure", "Incorrect BinY {0}", binY);
                throw new ASCOM.InvalidValueException("StartExposure BinY",
                                                    this.binY.ToString(CultureInfo.InvariantCulture),
                                                    string.Format(CultureInfo.InvariantCulture, "1 to {0}", this.maxBinY));
            }
            // check the start position is in range
            // start is in binned pixels
            if (this.startX < 0 || this.startX * this.binX > this.cameraXSize)
            {
                this.lastError = "Incorrect Start X position";
                Log.LogMessage("StartExposure", "Incorrect Start X {0}", startX);
                throw new ASCOM.InvalidValueException("StartExposure StartX",
                                                    this.startX.ToString(CultureInfo.InvariantCulture),
                                                    string.Format(CultureInfo.InvariantCulture, "0 to {0}", cameraXSize / this.binX));
            }
            if (this.startY < 0 || this.startY * this.binY > this.cameraYSize)
            {
                this.lastError = "Incorrect Start Y position";
                Log.LogMessage("StartExposure", "Incorrect Start Y {0}", startY);
                throw new ASCOM.InvalidValueException("StartExposure StartX",
                                                    this.startX.ToString(CultureInfo.InvariantCulture),
                                                    string.Format(CultureInfo.InvariantCulture, "0 to {0}", cameraXSize / this.binX));
            }
            // check that the acquisition is at least 1 pixel in size and fits in the camera area
            if (this.numX < 1 || (this.numX + this.startX) * this.binX > this.cameraXSize)
            {
                this.lastError = "Incorrect Num X value";
                Log.LogMessage("StartExposure", "Incorrect Num X {0}", numX);
                throw new ASCOM.InvalidValueException("StartExposure NumX",
                                                    this.numX.ToString(CultureInfo.InvariantCulture),
                                                    string.Format(CultureInfo.InvariantCulture, "1 to {0}", cameraXSize / this.binX));
            }
            if (this.numY < 1 || (this.numY + this.startY) * this.binY > this.cameraYSize)
            {
                this.lastError = "Incorrect Num Y value";
                Log.LogMessage("StartExposure", "Incorrect Num Y {0}", numY);
                throw new ASCOM.InvalidValueException("StartExposure NumY",
                                                    this.numY.ToString(CultureInfo.InvariantCulture),
                                                    string.Format(CultureInfo.InvariantCulture, "1 to {0}", cameraYSize / this.binY));
            }

            // set up the things to do at the start of the exposure
            this.imageReady = false;
            if (this.hasShutter)
            {
                this.darkFrame = !Light;
            }
            this.lastExposureStartTime = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss", CultureInfo.InvariantCulture);
            // set the image array dimensions
            if (this.sensorType == SensorType.Color)
                this.imageArrayColour = new int[this.numX, this.numY, 3];
            else
                this.imageArray = new int[this.numX, this.numY];

            if (this.exposureTimer == null)
            {
                this.exposureTimer = new System.Timers.Timer();
                this.exposureTimer.Elapsed += exposureTimer_Elapsed;
            }
            // force the minimum exposure to be 1 millisec to keep the exposureTimer happy
            this.exposureTimer.Interval = Math.Max((int)(Duration * 1000), 1);
            this.cameraState = CameraStates.cameraExposing;
            this.exposureStartTime = DateTime.Now;
            this.exposureDuration = Duration;
            this.exposureTimer.Enabled = true;
            Log.LogFinish(" started");
        }

        private void exposureTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.exposureTimer.Enabled = false;
            this.lastExposureDuration = (DateTime.Now - this.exposureStartTime).TotalSeconds;
            this.cameraState = CameraStates.cameraDownload;
            this.FillImageArray();
            this.imageReady = true;
            this.cameraState = CameraStates.cameraIdle;
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
                if (!this.connected)
                    throw new NotConnectedException("Can't read StartX when not connected");
                Log.LogMessage("StartX", "get {0}", startX);
                return this.startX;
            }
            set
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't set StartX when not connected");
                Log.LogMessage("StartX", "set {0}", value);
                this.startX = value;
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
                if (!this.connected)
                    throw new NotConnectedException("Can't read StartY when not connected");
                Log.LogMessage("StartY", "get {0}", startY);
                return this.startY;
            }
            set
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't set StartY when not connected");
                Log.LogMessage("StartY", "set {0}", value);
                this.startY = value;
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
            CheckCapability("StopExposure", canStopExposure);
            Log.LogMessage("StopExposure", "state {0}", cameraState);
            switch (this.cameraState)
            {
                case CameraStates.cameraWaiting:
                case CameraStates.cameraExposing:
                case CameraStates.cameraReading:
                case CameraStates.cameraDownload:
                    // these are all possible exposure states so we can stop the exposure
                    this.exposureTimer.Enabled = false;
                    this.lastExposureDuration = (DateTime.Now - this.exposureStartTime).TotalSeconds;
                    this.FillImageArray();
                    this.cameraState = CameraStates.cameraIdle;
                    this.imageReady = true;
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
        /// <value>The bayer offset X.</value>
        public short BayerOffsetX
        {
            get
            {
                CheckInterface("BayerOffsetX");
                CheckConnected("BayerOffsetX");
                CheckCapability("BayerOffsetX", this.sensorType != DeviceInterface.SensorType.Monochrome);
                Log.LogMessage("BayerOffsetX", "get {0}", bayerOffsetX);
                return this.bayerOffsetX;
            }
        }

        /// <summary>
        /// Returns the Y offset of the Bayer matrix, as defined in <see cref=""SensorType/>.
        /// Value returned must be in the range 0 to M-1, where M is the height of the Bayer matrix.
        /// The offset is relative to the 0,0 pixel in the sensor array, and does not change to reflect
        /// subframe settings. 
        /// </summary>
        /// <value>The bayer offset Y.</value>
        public short BayerOffsetY
        {
            get
            {
                CheckInterface("BayerOffsetY");
                CheckConnected("BayerOffsetY");
                CheckCapability("BayerOffsetY", this.sensorType != DeviceInterface.SensorType.Monochrome);
                Log.LogMessage("BayerOffsetY", "get {0}", bayerOffsetY);
                return this.bayerOffsetY;
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
                CheckInterface("CanFastReadout");
                CheckConnected("CanFastReadout");
                Log.LogMessage("CanFastReadout", "get {0}", canFastReadout);
                return this.canFastReadout;
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
                CheckInterface("DriverInfo");
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
                CheckInterface("DriverVersion");
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
                CheckInterface("ExposureMax");
                CheckConnected("ExposureMax");
                Log.LogMessage("ExposureMax", "get {0}", exposureMax);
                return this.exposureMax;
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
                CheckInterface("ExposureMin");
                CheckConnected("ExposureMin");
                Log.LogMessage("ExposureMin", "get {0}", exposureMin);
                return this.exposureMin;
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
                CheckInterface("ExposureResolution");
                CheckConnected("ExposureResolution");
                Log.LogMessage("ExposureResolution", "get {0}", exposureResolution);
                return this.exposureResolution;
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
                CheckInterface("FastReadout");
                CheckConnected("FastReadout");
                CheckCapability("FastReadout", canFastReadout);
                Log.LogMessage("FastReadout", "get {0}", fastReadout);
                return this.fastReadout;
            }
            set
            {
                CheckInterface("FastReadout");
                CheckConnected("FastReadout");
                CheckCapability("FastReadout", canFastReadout);
                Log.LogMessage("FastReadout", "get {0}", value);
                this.fastReadout = value;
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
                CheckInterface("Gain");
                CheckConnected("Gain");
                CheckCapability("Gain", this.gainMax > this.gainMin);
                Log.LogMessage("Gain", "get {0}", gain);
                return this.gain;
            }
            set
            {
                CheckInterface("Gain");
                CheckConnected("Gain");
                CheckCapability("Gain", this.gainMax > this.gainMin, true);
                CheckRange("Gain", gainMin, value, gainMax);
                Log.LogMessage("Gain", "set {0}", value);
                this.gain = value;
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
                CheckInterface("GainMax");
                CheckConnected("GainMax");
                CheckCapability("GainMax", this.gainMax > this.gainMin);
                if (this.gains != null && this.gains.Count > 0)
                {
                    Log.LogMessage("GainMax", "cannot be read if there is an array of Gains in use");
                    throw new InvalidOperationException("GainMax cannot be read if there is an array of Gains in use");
                }
                Log.LogMessage("GainMax", "get {0}", gainMax);
                return this.gainMax;
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
                CheckInterface("GainMin");
                CheckConnected("GainMin");
                CheckCapability("GainMin", this.gainMax > this.gainMin);
                if (this.gains != null && this.gains.Count > 0)
                {
                    Log.LogMessage("GainMin", "cannot be read if there is an array of Gains in use");
                    throw new InvalidOperationException("GainMin cannot be read if there is an array of Gains in use");
                }
                Log.LogMessage("GainMin", "get {0}", gainMin);
                return this.gainMin;
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
                CheckInterface("Gains");
                CheckConnected("Gains");
                CheckCapability("Gains", !(this.gains == null || this.gains.Count == 0));
                Log.LogMessage("Gains", "get {0}", gains);
                return this.gains;
            }
        }

        /// <summary>
        /// Reports the version of this interface. Will return 2 for this version.
        /// </summary>
        /// <value>The interface version.</value>
        public short InterfaceVersion
        {
            get { return this.interfaceVersion; }
        }

        /// <summary>
        /// The short name of the camera, for display purposes.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                CheckInterface("Name");
                CheckConnected("Name");
                Log.LogMessage("Name", "Sim {0}", SensorName);
                return "Sim " + this.SensorName;
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
                CheckInterface("PercentCompleted");
                CheckConnected("PercentCompleted");
                switch (this.cameraState)
                {
                    case CameraStates.cameraWaiting:
                    case CameraStates.cameraExposing:
                    case CameraStates.cameraReading:
                    case CameraStates.cameraDownload:
                        var pc = (short)(((DateTime.Now - this.exposureStartTime).TotalSeconds / this.exposureDuration) * 100);
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
                CheckInterface("ReadoutMode");
                CheckConnected("ReadoutMode");
                if (this.readoutModes == null || this.readoutModes.Count < 1)
                {
                    Log.LogMessage("ReadoutMode", "PropertyNotImplemented because readoutModes == null || readoutModes.Count < 1");
                    throw new ASCOM.PropertyNotImplementedException("ReadoutMode", false);
                    //if (this.canFastReadout)
                    //    throw new PropertyNotImplementedException("ReadoutMode", false);
                }
                var rm = this.readoutMode;
                Log.LogMessage("ReadoutMode", "get {0}, mode {1}", rm, readoutModes[rm]);

                return rm;
            }
            set
            {
                Log.LogMessage("ReadoutMode", "set {0}", value);
                CheckInterface("ReadoutMode");
                CheckConnected("ReadoutMode");
                if (this.readoutModes == null || this.readoutModes.Count < 1)
                {
                    Log.LogMessage("ReadoutMode", "PropertyNotImplemented readoutModes == null || readoutModes.Count < 1");
                    throw new PropertyNotImplementedException("ReadoutMode", true);
                }
                if (this.canFastReadout)
                {
                    Log.LogMessage("ReadoutMode", "PropertyNotImplemented canFastReadout is true");
                    throw new PropertyNotImplementedException("ReadoutMode", true);
                }
                if (value < 0 || value > this.readoutModes.Count - 1)
                {
                    Log.LogMessage("ReadoutMode", "InvalidValueException, value {0}, range 0 to {1}", value, readoutModes.Count - 1);
                    throw new InvalidValueException("ReadoutMode", value.ToString(CultureInfo.InvariantCulture), string.Format(CultureInfo.InvariantCulture, "0 to {0}", this.readoutModes.Count - 1));
                }
                this.readoutMode = value;
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
                CheckInterface("ReadoutModes");
                CheckConnected("ReadoutModes");
                //CheckCapability("ReadoutModes", !this.canFastReadout);
                //CheckCapability("ReadoutModes", !(this.readoutModes == null || this.readoutModes.Count < 1));
                Log.LogMessage("ReadoutModes", "ReadoutModes {0}", readoutModes.Count);
                foreach (var item in readoutModes)
                {
                    Log.LogMessage("", "       {0}", item);
                }
                return this.readoutModes;
            }
        }

        /// <summary>
        /// Returns the name (datasheet part number) of the sensor, e.g. ICX285AL.
        /// The format is to be exactly as shown on manufacturer data sheet, subject to the following rules.
        /// All letter shall be uppercase.  Spaces shall not be included.
        /// </summary>
        /// <value>The name of the sensor.</value>
        public string SensorName
        {
            get
            {
                CheckInterface("SensorName");
                CheckConnected("SensorName");
                Log.LogMessage("SensorName", "get {0}", sensorName);
                return this.sensorName;
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
                CheckInterface("SensorType");
                CheckConnected("SensorType");
                Log.LogMessage("SensorType", "get {0}", sensorType);
                return this.sensorType;
            }
        }

        #endregion

        #region private

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

                // read properties from profile
                Log.Enabled = Convert.ToBoolean(profile.GetValue(s_csDriverID, "Trace", string.Empty, "false"), CultureInfo.InvariantCulture);
                this.interfaceVersion = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_InterfaceVersion, string.Empty, "2"), CultureInfo.InvariantCulture);
                this.pixelSizeX = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_PixelSizeX, string.Empty, "5.6"), CultureInfo.InvariantCulture);
                this.pixelSizeY = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_PixelSizeY, string.Empty, "5.6"), CultureInfo.InvariantCulture);
                this.fullWellCapacity = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_FullWellCapacity, string.Empty, "30000"), CultureInfo.InvariantCulture);
                this.maxADU = Convert.ToInt32(profile.GetValue(s_csDriverID, STR_MaxADU, string.Empty, "65535"), CultureInfo.InvariantCulture);
                this.electronsPerADU = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_ElectronsPerADU, string.Empty, "0.8"), CultureInfo.InvariantCulture);

                this.cameraXSize = Convert.ToInt32(profile.GetValue(s_csDriverID, STR_CameraXSize, string.Empty, "800"), CultureInfo.InvariantCulture);
                this.cameraYSize = Convert.ToInt32(profile.GetValue(s_csDriverID, STR_CameraYSize, string.Empty, "600"), CultureInfo.InvariantCulture);
                this.canAsymmetricBin = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanAsymmetricBin, string.Empty, "true"), CultureInfo.InvariantCulture);
                this.maxBinX = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_MaxBinX, string.Empty, "4"), CultureInfo.InvariantCulture);
                this.maxBinY = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_MaxBinY, string.Empty, "4"), CultureInfo.InvariantCulture);
                this.hasShutter = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_HasShutter, string.Empty, "false"), CultureInfo.InvariantCulture);
                this.sensorName = profile.GetValue(s_csDriverID, STR_SensorName, string.Empty, "");
                this.sensorType = (SensorType)Convert.ToInt32(profile.GetValue(s_csDriverID, STR_SensorType, string.Empty, "0"), CultureInfo.InvariantCulture);
                this.omitOddBins = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_OmitOddBins, string.Empty, "false"), CultureInfo.InvariantCulture);

                this.bayerOffsetX = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_BayerOffsetX, string.Empty, "0"), CultureInfo.InvariantCulture);
                this.bayerOffsetY = Convert.ToInt16(profile.GetValue(s_csDriverID, STR_BayerOffsetY, string.Empty, "0"), CultureInfo.InvariantCulture);

                this.hasCooler = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_HasCooler, string.Empty, "true"), CultureInfo.InvariantCulture);
                this.canSetCcdTemperature = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanSetCCDTemperature, string.Empty, "false"), CultureInfo.InvariantCulture);
                this.canGetCoolerPower = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanGetCoolerPower, string.Empty, "false"), CultureInfo.InvariantCulture);
                this.setCcdTemperature = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_SetCCDTemperature, string.Empty, "-10"), CultureInfo.InvariantCulture);

                this.canAbortExposure = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanAbortExposure, string.Empty, "true"), CultureInfo.InvariantCulture);
                this.canStopExposure = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanStopExposure, string.Empty, "true"), CultureInfo.InvariantCulture);
                this.exposureMax = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_MaxExposure, string.Empty, "3600"), CultureInfo.InvariantCulture);
                this.exposureMin = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_MinExposure, string.Empty, "0.001"), CultureInfo.InvariantCulture);
                this.exposureResolution = Convert.ToDouble(profile.GetValue(s_csDriverID, STR_ExposureResolution, string.Empty, "0.001"), CultureInfo.InvariantCulture);
                string fullPath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly((this.GetType())).Location);
                this.imagePath = profile.GetValue(s_csDriverID, STR_ImagePath, string.Empty, Path.Combine(fullPath, @"m42-800x600.jpg"));
                this.applyNoise = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_ApplyNoise, string.Empty, "false"), CultureInfo.InvariantCulture);

                this.canPulseGuide = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanPulseGuide, string.Empty, "false"), CultureInfo.InvariantCulture);

                string gs = profile.GetValue(s_csDriverID, "Gains");
                if (string.IsNullOrEmpty(gs))
                {
                    this.gainMin = Convert.ToInt16(profile.GetValue(s_csDriverID, "GainMin", string.Empty, "0"), CultureInfo.InvariantCulture);
                    this.gainMax = Convert.ToInt16(profile.GetValue(s_csDriverID, "GainMax", string.Empty, "0"), CultureInfo.InvariantCulture);
                }
                else
                {
                    string[] gsa = gs.Split(',');
                    this.gains = new ArrayList();
                    foreach (var item in gsa)
                    {
                        this.gains.Add(item);
                    }
                    this.gainMin = 0;
                    this.gainMax = (short)(this.gains.Count - 1);
                }

                this.canFastReadout = Convert.ToBoolean(profile.GetValue(s_csDriverID, STR_CanFastReadout, string.Empty, "false"), CultureInfo.InvariantCulture);

                gs = profile.GetValue(s_csDriverID, "ReadoutModes");
                this.readoutModes = new ArrayList();
                if (string.IsNullOrEmpty(gs))
                {
                    this.readoutModes.Add("Default");
                }
                else
                {
                    string[] rms = gs.Split(',');
                    foreach (var item in rms)
                    {
                        this.readoutModes.Add(item);
                    }
                }
            }
        }

        private void SaveToProfile()
        {
            using (Profile profile = new Profile(true))
            {
                profile.DeviceType = "Camera";

                // Save the cooler configuration to the Profile
                profile.WriteValue(s_csDriverID, STR_CoolerAmbientTemperature, this.heatSinkTemperature.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerDeltaTMax, this.coolerDeltaTMax.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerMode, this.coolerMode.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerTimeToSetPoint, this.coolerTimeToSetPoint.ToString(CultureInfo.InvariantCulture), "Cooler");
                profile.WriteValue(s_csDriverID, STR_CoolerResetToAmbient, this.coolerResetToAmbient.ToString(CultureInfo.InvariantCulture), "Cooler");

                // Save camera configuration to the Profile
                profile.WriteValue(s_csDriverID, "Trace", Log.Enabled.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_InterfaceVersion, this.interfaceVersion.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_PixelSizeX, this.pixelSizeX.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_PixelSizeY, this.pixelSizeY.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_FullWellCapacity, this.fullWellCapacity.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_MaxADU, this.maxADU.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_ElectronsPerADU, this.electronsPerADU.ToString(CultureInfo.InvariantCulture));

                profile.WriteValue(s_csDriverID, STR_CameraXSize, this.cameraXSize.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_CameraYSize, this.cameraYSize.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_CanAsymmetricBin, this.canAsymmetricBin.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_MaxBinX, this.maxBinX.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_MaxBinY, this.maxBinY.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_HasShutter, this.hasShutter.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_SensorName, this.sensorName.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_SensorType, ((int)this.sensorType).ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_BayerOffsetX, this.bayerOffsetX.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_BayerOffsetY, this.bayerOffsetY.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_OmitOddBins, this.omitOddBins.ToString(CultureInfo.InvariantCulture));

                profile.WriteValue(s_csDriverID, STR_HasCooler, this.hasCooler.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_CanSetCCDTemperature, this.canSetCcdTemperature.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_CanGetCoolerPower, this.canGetCoolerPower.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_SetCCDTemperature, this.setCcdTemperature.ToString(CultureInfo.InvariantCulture));

                profile.WriteValue(s_csDriverID, STR_CanAbortExposure, this.canAbortExposure.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_CanStopExposure, this.canStopExposure.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_MaxExposure, this.exposureMax.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_MinExposure, this.exposureMin.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_ExposureResolution, this.exposureResolution.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(s_csDriverID, STR_ImagePath, this.imagePath);
                profile.WriteValue(s_csDriverID, STR_ApplyNoise, this.applyNoise.ToString(CultureInfo.InvariantCulture));

                profile.WriteValue(s_csDriverID, STR_CanPulseGuide, this.canPulseGuide.ToString(CultureInfo.InvariantCulture));

                if (this.gains != null && this.gains.Count > 0)
                {
                    // gain control using Gains string array
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (var item in this.gains)
                    {
                        if (sb.Length > 0)
                            sb.Append(",");
                        sb.Append(item.ToString());
                    }
                    profile.WriteValue(s_csDriverID, "Gains", sb.ToString());
                    profile.WriteValue(s_csDriverID, "GainMin", "0");
                    profile.WriteValue(s_csDriverID, "GainMax", Convert.ToString(gains.Count - 1, CultureInfo.InvariantCulture));
                }
                else if (this.gainMax > this.gainMin)
                {
                    // gain control using min and max
                    profile.DeleteValue(s_csDriverID, "Gains");
                    profile.WriteValue(s_csDriverID, "GainMin", this.gainMin.ToString(CultureInfo.InvariantCulture));
                    profile.WriteValue(s_csDriverID, "GainMax", this.gainMax.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    // no gain control
                    profile.DeleteValue(s_csDriverID, "Gains");
                    profile.DeleteValue(s_csDriverID, "GainMin");
                    profile.DeleteValue(s_csDriverID, "GainMax");
                }

                profile.WriteValue(s_csDriverID, STR_CanFastReadout, this.canFastReadout.ToString(CultureInfo.InvariantCulture));

                if (this.readoutModes == null || this.readoutModes.Count <= 1)
                {
                    profile.DeleteValue(s_csDriverID, "ReadoutModes");
                }
                else
                {
                    // Readout Modes use string array
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (var item in this.readoutModes)
                    {
                        if (sb.Length > 0)
                            sb.Append(",");
                        sb.Append(item.ToString());
                    }
                    profile.WriteValue(s_csDriverID, "ReadoutModes", sb.ToString());
                }
            }
        }

        private void InitialiseSimulator()
        {
            this.ReadFromProfile();

            this.startX = 0;
            this.startY = 0;
            this.binX = 1;
            this.binY = 1;
            this.numX = this.cameraXSize;
            this.numY = this.cameraYSize;

            this.cameraState = CameraStates.cameraIdle;
            this.coolerOn = false;
            this.coolerPower = 0;
            this.ccdTemperature = 15;
            this.readoutMode = 0;
            this.fastReadout = false;
            this.ccdTemperature = this.heatSinkTemperature; // Set the CCD termperature to ambient
            Log.LogMessage("InitialiseSimulator", "Set camera temperature to ambient: {0}", heatSinkTemperature);
        }

        private delegate int PixelProcess(double value);

        private void FillImageArray()
        {
            PixelProcess pixelProcess = new PixelProcess(NoNoise);
            ShutterProcess shutterProcess = new ShutterProcess(BinData);

            if (this.applyNoise)
                pixelProcess = new PixelProcess(Poisson);
            if (this.HasShutter && this.darkFrame)
                shutterProcess = new ShutterProcess(DarkData);

            double readNoise = 3;
            // dark current 1 ADU/sec at 0C doubling for every 5C increase
            double darkCurrent = Math.Pow(2, this.ccdTemperature / 5);
            darkCurrent *= this.lastExposureDuration;
            // add read noise, should be in quadrature
            darkCurrent += readNoise;
            // fill the array using binning and image offsets
            // indexes into the imageData
            for (int y = 0; y < this.numY; y++)
            {
                for (int x = 0; x < this.numX; x++)
                {
                    double s;
                    if (this.sensorType == SensorType.Color)
                    {
                        s = shutterProcess((x + this.startX) * this.binX, (y + this.startY) * this.binY, 0);
                        this.imageArrayColour[x, y, 0] = pixelProcess(s + darkCurrent);
                        s = shutterProcess((x + this.startX) * this.binX, (y + this.startY) * this.binY, 1);
                        this.imageArrayColour[x, y, 1] = pixelProcess(s + darkCurrent);
                        s = shutterProcess((x + this.startX) * this.binX, (y + this.startY) * this.binY, 2);
                        this.imageArrayColour[x, y, 2] = pixelProcess(s + darkCurrent);
                    }
                    else
                    {
                        s = shutterProcess((x + this.startX) * this.binX, (y + this.startY) * this.binY, 0);
                        this.imageArray[x, y] = pixelProcess(s + darkCurrent);
                    }
                    //s *= this.lastExposureDuration;
                }
            }
        }

        private Random R = new Random();

        private int Poisson(double lambda)
        {
            // use normal distribution for large values
            // because Poisson falls over and gets slow
            if (lambda > 50)
                return Math.Min((int)BoxMuller(lambda, Math.Sqrt(lambda)), this.maxADU);

            double L = Math.Exp(-lambda);
            double p = 1.0;
            int k = 0;
            do
            {
                k++;
                p *= R.NextDouble();
            }
            while (p > L);
            return Math.Min(k - 1, this.maxADU);
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
            return Convert.ToInt32(Math.Min(value, this.maxADU));
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
            for (int k = 0; k < this.binY; k++)
            {
                for (int l = 0; l < this.binX; l++)
                {
                    s += imageData[l + x, k + y, p];
                }
            }
            return s * this.lastExposureDuration;
        }

        private double DarkData(int x, int y, int p)
        {
            return 5.0 * this.binX * this.binY;
        }

        private delegate void GetData(int x, int y);
        private Bitmap bmp;
        // bayer offsets
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body")]
        private void ReadImageFile()
        {
            this.imageData = new float[this.cameraXSize, this.cameraYSize, 1];
            try
            {
                bmp = (Bitmap)Image.FromFile(this.imagePath);

                //x0 = bayerOffsetX;
                //x1 = (bayerOffsetX + 1) & 1;
                //y0 = bayerOffsetY;
                //y1 = (bayerOffsetY + 1) & 1;

                GetData getData = new GetData(MonochromeData);
                switch (this.sensorType)
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
                        //y0 = (bayerOffsetY) & 3;
                        //y1 = (bayerOffsetY + 1) & 3;
                        //y2 = (bayerOffsetY + 2) & 3;
                        //y3 = (bayerOffsetY + 3) & 3;
                        break;
                    case SensorType.LRGB:
                        getData = new GetData(LRGBData);
                        stepX = 4;
                        stepY = 4;
                        break;
                    case SensorType.Color:
                        this.imageData = new float[this.cameraXSize, this.cameraYSize, 3];
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

                int w = Math.Min(this.cameraXSize, bmp.Width * stepX);
                int h = Math.Min(this.cameraYSize, bmp.Height * stepY);
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

        #endregion

        #region checks

        private void CheckConnected(string message)
        {
            if (!this.connected)
            {
                Log.LogMessage("NotConnected", message);
                throw new NotConnectedException(message);
            }
        }

        private void CheckRange(string identifier, double min, double value, double max)
        {
            if (value > max || value < min)
            {
                Log.LogMessage(identifier, "{0} is not in rang {1} to {2}", value, min, max);
                throw new InvalidValueException(identifier, value.ToString(), string.Format(CultureInfo.InvariantCulture, "{0} to {1}", min, max));
            }
        }

        private void CheckCapability(string identifier, bool capability)
        {
            CheckCapability(identifier, capability, false);
        }

        private void CheckCapability(string identifier, bool capability, bool accessorSet)
        {
            if (!capability)
            {
                Log.LogMessage(identifier, "Not Implemented");
                throw new PropertyNotImplementedException(identifier, accessorSet);
            }
        }

        private void CheckReady(string identifier)
        {
            if (!this.imageReady)
            {
                Log.LogMessage(identifier, "image not ready");
                throw new NotConnectedException("Can't read " + identifier + " when no image is ready");
            }
        }

        private void CheckInterface(string identifier)
        {
            if (interfaceVersion == 1)
            {
                Log.LogMessage(identifier, "Not supported for interface version 1");
                throw new System.NotSupportedException(identifier + " (not supported for Interface V1)");
            }
        }


        #endregion
    }
}
