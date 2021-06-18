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

    public int[] FocusOffsets
    {
        get
        {
            foreach (int fwOffset in fwOffsets) // Write filter offsets to the log
            {
                tl.LogMessage("FocusOffsets Get", fwOffset.ToString());
            }

            return fwOffsets;
        }
    }

    public string[] Names
    {
        get
        {
            foreach (string fwName in fwNames) // Write filter names to the log
            {
                tl.LogMessage("Names Get", fwName);
            }

            return fwNames;
        }
    }

    public short Position
    {
        get
        {
            tl.LogMessage("Position Get", fwPosition.ToString());
            return fwPosition;
        }
        set
        {
            tl.LogMessage("Position Set", value.ToString());
            if ((value < 0) | (value > fwNames.Length - 1))
            {
                tl.LogMessage("", "Throwing InvalidValueException - Position: " + value.ToString() + ", Range: 0 to " + (fwNames.Length - 1).ToString());
                throw new InvalidValueException("Position", value.ToString(), "0 to " + (fwNames.Length - 1).ToString());
            }
            fwPosition = value;
        }
    }

    #endregion

    //ENDOFINSERTEDFILE
}