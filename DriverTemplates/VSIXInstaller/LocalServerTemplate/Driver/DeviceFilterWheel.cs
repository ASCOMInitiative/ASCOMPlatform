// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;

class DeviceFilerWheel
{
    Util util = new Util();
    TraceLogger tl = new TraceLogger();

    #region IFilerWheel Implementation

    /// <summary>
    /// Focus offset of each filter in the wheel
    /// </summary>
    public int[] FocusOffsets
    {
        get
        {
            try
            {
                CheckConnected("FocusOffsets");
                int[] focusoffsets = FilterWheelHardware.FocusOffsets;
                LogMessage("FocusOffsets", focusoffsets.ToString());
                return focusoffsets;
            }
            catch (Exception ex)
            {
                LogMessage("FocusOffsets", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Name of each filter in the wheel
    /// </summary>
    public string[] Names
    {
        get
        {
            try
            {
                CheckConnected("Names");
                string[] names = FilterWheelHardware.Names;
                LogMessage("Names", names.ToString());
                return names;
            }
            catch (Exception ex)
            {
                LogMessage("Names", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Sets or returns the current filter wheel position
    /// </summary>
    public short Position
    {
        get
        {
            try
            {
                CheckConnected("Position Get");
                short position = FilterWheelHardware.Position;
                LogMessage("Position Get", position.ToString());
                return position;
            }
            catch (Exception ex)
            {
                LogMessage("Position Get", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
        set
        {
            try
            {
                CheckConnected("Position Set");
                LogMessage("Position Set", value.ToString());
                FilterWheelHardware.Position = value;
            }
            catch (Exception ex)
            {
                LogMessage("Position Set", $"Threw an exception: \r\n{ex}");
                throw;
            }
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