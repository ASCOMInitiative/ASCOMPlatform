﻿using System;
using System.Threading;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Add un-handled exception handlers
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(HandleApplicationException);
            Application.ThreadException += new ThreadExceptionEventHandler(HandleThreadException);

            // Start the application UI
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DiagnosticsForm());
        }

        /// <summary>
        /// Handle an un-handled application exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        static void HandleApplicationException(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            try
            {
                using (TraceLogger logger = new("DiagnosticsException"))
                {
                    logger.Enabled = true;
                    Exception ex = eventArgs.ExceptionObject as Exception;
                    logger.LogMessageCrLf("Diagnostics", $"An un-handled application exception occurred, please report this on the ASCOM-Talk groups.io forum: {ex.Message}.\r\n{ex}");
                    logger.Enabled=false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An un-handled application exception occurred but it was not possible to create a diagnostics log: {ex.Message}.\r\n{ex}\r\n" +
                    $"Diagnostics application exception: \r\n{(Exception)eventArgs.ExceptionObject}");
            }
        }

        /// <summary>
        /// Handle an un-handled thread exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        static void HandleThreadException(object sender, ThreadExceptionEventArgs eventArgs)
        {
            try
            {
                using (TraceLogger logger = new("DiagnosticsThreadException"))
                {
                    logger.Enabled = true;
                    logger.LogMessageCrLf("Diagnostics", $"An un-handled thread exception occurred, please report this on the ASCOM-Talk groups.io forum: {eventArgs.Exception.Message}.\r\n{eventArgs.Exception}");
                    logger.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An un-handled thread exception occurred but it was not possible to create a diagnostics log: {ex.Message}.\r\n{ex}\r\n" +
                    $"Diagnostics thread exception: \r\n{(Exception)eventArgs.Exception}");
            }
        }
    }
}
