// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

static class CoverCalibratorHardware
{
    private static Util util = new Util();
    private static TraceLogger tl = new TraceLogger();

    #region ICoverCalibrator Implementation

    /// <summary>
    /// Returns the state of the device cover, if present, otherwise returns "NotPresent"
    /// </summary>
    internal static CoverStatus CoverState
    {
        get
        {
            tl.LogMessage("CoverState Get", "Not implemented");
            throw new PropertyNotImplementedException("CoverState", false);
        }
    }

    /// <summary>
    /// Initiates cover opening if a cover is present
    /// </summary>
    internal static void OpenCover()
    {
        tl.LogMessage("OpenCover", "Not implemented");
        throw new MethodNotImplementedException("OpenCover");
    }

    /// <summary>
    /// Initiates cover closing if a cover is present
    /// </summary>
    internal static void CloseCover()
    {
        tl.LogMessage("CloseCover", "Not implemented");
        throw new MethodNotImplementedException("CloseCover");
    }

    /// <summary>
    /// Stops any cover movement that may be in progress if a cover is present and cover movement can be interrupted.
    /// </summary>
    internal static void HaltCover()
    {
        tl.LogMessage("HaltCover", "Not implemented");
        throw new MethodNotImplementedException("HaltCover");
    }

    /// <summary>
    /// Returns the state of the calibration device, if present, otherwise returns "NotPresent"
    /// </summary>
    internal static CalibratorStatus CalibratorState
    {
        get
        {
            tl.LogMessage("CalibratorState Get", "Not implemented");
            throw new PropertyNotImplementedException("CalibratorState", false);
        }
    }

    /// <summary>
    /// Returns the current calibrator brightness in the range 0 (completely off) to <see cref="MaxBrightness"/> (fully on)
    /// </summary>
    internal static int Brightness
    {
        get
        {
            tl.LogMessage("Brightness Get", "Not implemented");
            throw new PropertyNotImplementedException("Brightness", false);
        }
    }

    /// <summary>
    /// The Brightness value that makes the calibrator deliver its maximum illumination.
    /// </summary>
    internal static int MaxBrightness
    {
        get
        {
            tl.LogMessage("MaxBrightness Get", "Not implemented");
            throw new PropertyNotImplementedException("MaxBrightness", false);
        }
    }

    /// <summary>
    /// Turns the calibrator on at the specified brightness if the device has calibration capability
    /// </summary>
    /// <param name="Brightness"></param>
    internal static void CalibratorOn(int Brightness)
    {
        tl.LogMessage("CalibratorOn", $"Not implemented. Value set: {Brightness}");
        throw new MethodNotImplementedException("CalibratorOn");
    }

    /// <summary>
    /// Turns the calibrator off if the device has calibration capability
    /// </summary>
    internal static void CalibratorOff()
    {
        tl.LogMessage("CalibratorOff", "Not implemented");
        throw new MethodNotImplementedException("CalibratorOff");
    }

    #endregion

    //ENDOFINSERTEDFILE
}