using System;
using System.IO;

namespace Stand_Alone_Tester
{
    internal class StandAloneLogger : IDisposable
    {
        private bool disposedValue;
        StreamWriter logWriter;

        public StandAloneLogger(string folder)
        {
            Console.WriteLine($"StandAloneLogger - Creating file in folder: {folder}");
            logWriter = new StreamWriter(folder + "\\StandAloneLogger.txt", false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    logWriter.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put clean-up code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void LogMessage(string section, string message)
        {
            Console.WriteLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss.fff} {section} {message}");
            logWriter.WriteLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss.fff} {section} {message}");
        }
    }
}
