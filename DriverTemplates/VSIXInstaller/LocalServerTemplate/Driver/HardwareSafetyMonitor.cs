// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

static class SafetyMonitorHardware
{
    #region ISafetyMonitor Implementation

    /// <summary>
    /// Indicates whether the monitored state is safe for use.
    /// </summary>
    /// <value>True if the state is safe, False if it is unsafe.</value>
    public static bool IsSafe
    {
        get
        {
            // Return false per the ASOCM specification
            LogMessage("IsSafe Get", "false");
            return false;
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
