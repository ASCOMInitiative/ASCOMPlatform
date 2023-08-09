// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.Utilities;

class DeviceSafetyMonitor
{
    Util util = new Util();
    TraceLogger tl = new TraceLogger();

	#region ISafetyMonitor Implementation

	/// <summary>
	/// Indicates whether the monitored state is safe for use.
	/// </summary>
	/// <value>True if the state is safe, False if it is unsafe.</value>
	public bool IsSafe
    {
        get
        {
            tl.LogMessage("IsSafe Get", "true");
            return true;
        }
    }

    #endregion

    //ENDOFINSERTEDFILE
}