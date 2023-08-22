// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;

class DeviceCoverCalibrator
{
    #region ICoverCalibrator Implementation

    /// <summary>
    /// Returns the state of the device cover, if present, otherwise returns "NotPresent"
    /// </summary>
    public CoverStatus CoverState
    {
        get
        {
            try
            {
                CheckConnected("CoverStatus");
                CoverStatus coverStatus = CoverCalibratorHardware.CoverState;
                LogMessage("CoverStatus", $"{coverStatus}");
                return coverStatus;
            }
            catch (Exception ex)
            {
                LogMessage("CoverState", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Initiates cover opening if a cover is present
    /// </summary>
    public void OpenCover()
    {
        try
        {
            CheckConnected("OpenCover");
            LogMessage("OpenCover", "Calling OpenCover");
            CoverCalibratorHardware.OpenCover();
            LogMessage("OpenCover", "OpenCover complete");
        }
        catch (Exception ex)
        {
            LogMessage("OpenCover", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Initiates cover closing if a cover is present
    /// </summary>
    public void CloseCover()
    {
        try
        {
            CheckConnected("CloseCover");
            LogMessage("CloseCover", "Calling CloseCover");
            CoverCalibratorHardware.CloseCover();
            LogMessage("CloseCover", "CloseCover complete");
        }
        catch (Exception ex)
        {
            LogMessage("CloseCover", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Stops any cover movement that may be in progress if a cover is present and cover movement can be interrupted.
    /// </summary>
    public void HaltCover()
    {
        try
        {
            CheckConnected("HaltCover");
            LogMessage("HaltCover", "Calling HaltCover");
            CoverCalibratorHardware.HaltCover();
            LogMessage("HaltCover", "HaltCover complete");
        }
        catch (Exception ex)
        {
            LogMessage("HaltCover", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Returns the state of the calibration device, if present, otherwise returns "NotPresent"
    /// </summary>
    public CalibratorStatus CalibratorState
    {
        get
        {
            try
            {
                CheckConnected("CalibratorStatus");
                CalibratorStatus calibratorStatus = CoverCalibratorHardware.CalibratorState;
                LogMessage("CalibratorStatus", $"{calibratorStatus}");
                return calibratorStatus;
            }
            catch (Exception ex)
            {
                LogMessage("CalibratorState", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Returns the current calibrator brightness in the range 0 (completely off) to <see cref="MaxBrightness"/> (fully on)
    /// </summary>
    public int Brightness
    {
        get
        {
            try
            {
                CheckConnected("Brightness");
                int brightness = CoverCalibratorHardware.Brightness;
                LogMessage("Brightness", $"{brightness}");
                return brightness;
            }
            catch (Exception ex)
            {
                LogMessage("Brightness", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// The Brightness value that makes the calibrator deliver its maximum illumination.
    /// </summary>
    public int MaxBrightness
    {
        get
        {
            try
            {
                CheckConnected("MaxBrightness");
                int maxBrightness = CoverCalibratorHardware.MaxBrightness;
                LogMessage("MaxBrightness", $"{maxBrightness}");
                return maxBrightness;
            }
            catch (Exception ex)
            {
                LogMessage("MaxBrightness", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Turns the calibrator on at the specified brightness if the device has calibration capability
    /// </summary>
    /// <param name="Brightness"></param>
    public void CalibratorOn(int Brightness)
    {
        try
        {
            CheckConnected("CalibratorOn");
            LogMessage("CalibratorOn", "Calling CalibratorOn");
            CoverCalibratorHardware.CalibratorOn(Brightness);
            LogMessage("CalibratorOn", "CalibratorOn complete");
        }
        catch (Exception ex)
        {
            LogMessage("CalibratorOn", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Turns the calibrator off if the device has calibration capability
    /// </summary>
    public void CalibratorOff()
    {
        try
        {
            CheckConnected("CalibratorOff");
            LogMessage("CalibratorOff", "Calling CalibratorOff");
            CoverCalibratorHardware.CalibratorOff();
            LogMessage("CalibratorOff", "CalibratorOff complete");
        }
        catch (Exception ex)
        {
            LogMessage("CalibratorOff", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    #endregion

    //ENDOFINSERTEDFILE

    /// <summary>
    /// Dummy LogMessage class that removes compilation errors in the Platform source code and that will be omitted when the project is built
    /// </summary>
    static void LogMessage(string method, string message)
    {
    }

    /// <summary>
    /// Dummy CheckConnected class that removes compilation errors in the Platform source code and that will be omitted when the project is built
    /// </summary>
    /// <param name="message"></param>
    private void CheckConnected(string message)
    {
    }
}