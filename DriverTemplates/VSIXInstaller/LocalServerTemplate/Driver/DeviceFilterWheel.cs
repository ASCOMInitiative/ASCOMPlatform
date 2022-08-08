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
    private int[] fwOffsets = new int[4] { 0, 0, 0, 0 }; //class level variable to hold focus offsets
    private string[] fwNames = new string[4] { "Red", "Green", "Blue", "Clear" }; //class level variable to hold the filter names
    private short fwPosition = 0; // class level variable to retain the current filterwheel position

    /// <summary>
    /// Focus offset of each filter in the wheel
    /// </summary>
    public int[] FocusOffsets
    {
        get
        {
            int[] focusoffsets = FilterWheelHardware.FocusOffsets;
            LogMessage("FocusOffsets Get", focusoffsets.ToString());
            return focusoffsets;
        }
    }

    /// <summary>
    /// Name of each filter in the wheel
    /// </summary>
    public string[] Names
    {
        get
        {
            string[] names = FilterWheelHardware.Names;
            LogMessage("Names Get", names.ToString());
            return names;
        }
    }

    /// <summary>
    /// Sets or returns the current filter wheel position
    /// </summary>
    public short Position
    {
        get
        {
            short position = FilterWheelHardware.Position;
            LogMessage("Position Get", position.ToString());
            return position;
        }
        set
        {
            LogMessage("Position Set", value.ToString());
            FilterWheelHardware.Position = value;
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
}