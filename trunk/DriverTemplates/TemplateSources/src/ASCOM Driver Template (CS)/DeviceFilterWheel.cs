// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;

class DeviceFilerWheel
{
    Util util = new Util(); TraceLogger tl = new TraceLogger();

    #region IFilerWheel Implementation
    public int[] FocusOffsets
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public string[] Names
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public short Position
    {
        get
        {
            throw new ASCOM.PropertyNotImplementedException();
        }
        set
        {
            throw new ASCOM.PropertyNotImplementedException();
        }
    }

    #endregion

    //ENDOFINSERTEDFILE
}