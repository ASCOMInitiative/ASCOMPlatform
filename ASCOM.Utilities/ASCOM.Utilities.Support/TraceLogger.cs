using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.Utilities.Support
{
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
