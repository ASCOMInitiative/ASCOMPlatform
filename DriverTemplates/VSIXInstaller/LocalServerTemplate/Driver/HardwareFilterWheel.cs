// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM;

static class FilterWheelHardware
{
    #region IFilerWheel Implementation
    private static int[] fwOffsets = new int[4] { 0, 0, 0, 0 }; //class level variable to hold focus offsets
    private static string[] fwNames = new string[4] { "Red", "Green", "Blue", "Clear" }; //class level variable to hold the filter names
    private static short fwPosition = 0; // class level variable to retain the current filter wheel position

    /// <summary>
    /// Focus offset of each filter in the wheel
    /// </summary>
    internal static int[] FocusOffsets
    {
        get
        {
            foreach (int fwOffset in fwOffsets) // Write filter offsets to the log
            {
                LogMessage("FocusOffsets Get", fwOffset.ToString());
            }

            return fwOffsets;
        }
    }

    /// <summary>
    /// Name of each filter in the wheel
    /// </summary>
    internal static string[] Names
    {
        get
        {
            foreach (string fwName in fwNames) // Write filter names to the log
            {
                LogMessage("Names Get", fwName);
            }

            return fwNames;
        }
    }

    /// <summary>
    /// Sets or returns the current filter wheel position
    /// </summary>
    internal static short Position
    {
        get
        {
            LogMessage("Position Get", fwPosition.ToString());
            return fwPosition;
        }
        set
        {
            LogMessage("Position Set", value.ToString());
            if ((value < 0) | (value > fwNames.Length - 1))
            {
                LogMessage("", "Throwing InvalidValueException - Position: " + value.ToString() + ", Range: 0 to " + (fwNames.Length - 1).ToString());
                throw new InvalidValueException("Position", value.ToString(), "0 to " + (fwNames.Length - 1).ToString());
            }
            fwPosition = value;
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