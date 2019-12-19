namespace ASCOM.Utilities.Support
{
    /// <summary>
    /// Interface to enable the VB ASCOM.Utilities assembly to pass a TraceLogger instance to the C# ASCOM.Utilities.Support assembly.
    /// </summary>
    /// <remarks>Use of this interface works round the need for ASCOM.Utilities.Support to have a dependency on ASCOM.Utilities. This is required because ASCOM.Utilities needs a 
    /// reference to ASCOM.Utilities.Support and adding the reverse dependency would create a circular dependency, which is not permitted.
    /// Use of this interface, which is defined in ASCOM.Utilities.Support and is implemented in the TraceLogger class, means that both ASCOM.Utilities and ASCOM.Utilities.Support can deal with  
    /// objects that expose the ITraceLoggerUtility interface without ASCOM.Utilities,Support requiring a reference the concrete TraceLogger class.</remarks>
    public interface ITraceLoggerUtility
    {
        public void LogStart(string Identifier, string Message);
        public void LogContinue(string Message, bool HexDump);
        public void LogFinish(string Message, bool HexDump);
        public void LogMessage(string Identifier, string Message, bool HexDump);
        public bool Enabled { get; set; }
        public void LogIssue(string Identifier, string Message);
        public void SetLogFile(string LogFileName, string LogFileType);
        public void BlankLine();
        public string LogFileName { get; }
        public void LogMessageCrLf(string Identifier, string Message);
        public string LogFilePath { get; set; }
        public int IdentifierWidth { get; set; }
        public void LogContinue(string Message);
        public void LogFinish(string Message);
        public void LogMessage(string Identifier, string Message);
        public void Dispose();
    }
}
