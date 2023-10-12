using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ASCOM.Utilities.Interfaces;
using Microsoft.VisualBasic;
using static ASCOM.Utilities.Global;

namespace ASCOM.Utilities
{

    /// <summary>
    /// Provides a repeating timer with associated tick event.
    /// </summary>
    /// <remarks>
    /// <para>The interval resolution is about 20ms.If you need beter than this, you could use the WaitForMilliseconds 
    /// method to create your own solution.</para>
    /// <para>You can create multiple instances of this object. When enabled, the Timer delivers Tick events periodically 
    /// (determined by setting the Interval property).</para>
    /// <para>This component is now considered <b>obsolete</b> for use in .NET clients and drivers. It is reliable under almost 
    /// all circumstances but there are some environments, noteably console and some scripted applications, where it fails to fire.
    /// The Platform 6 component improves performance over the Platform 5 component in this respect and can be further tuned 
    /// for particular applications by placing an entry in the ForceSystemTimer Profile key.</para>
    /// <para>For .NET applications, use of System.Timers.Timer is recommended but atention must be paid to getting threading correct
    /// when using this control. The Windows.Forms.Timer control is not an improvement over the ASCOM timer which is based upon it.</para>
    /// <para>Developers using non .NET languages are advised to use timers provided as part of their development environment, only falling 
    /// back to the ASCOM Timer if no viable alternative can be found.</para>
    /// </remarks>
    [Guid("64FEE414-176D-44d0-99DF-47621D9C377F")]
    [ComVisible(true)]
    [ComSourceInterfaces(typeof(ITimerEvent))]
    [ClassInterface(ClassInterfaceType.None)]
    [Obsolete("Please replace it with Systems.Timers.Timer, which is reliable in all console and non-windowed applications.", false)]
    public class Timer : ITimer, IDisposable
    {
        // =========
        // TIMER.CLS
        // =========
        // 
        // Implementation of the ASCOM DriverHelper Timer class.
        // 
        // Written:  28-Jan-01   Robert B. Denny <rdenny@dc3.com>
        // 
        // Edits:
        // 
        // When      Who     What
        // --------- ---     --------------------------------------------------
        // 03-Feb-09 pwgs    5.1.0 - refactored for Utilities
        // ---------------------------------------------------------------------

        // Set up a timer to create the tick events. Use a FORMS timer so that it will fire on the current owner's thread
        // If you use a system timer it will  fire on its own thread and this will be invisble to the application!
        // Private WithEvents m_Timer As System.Windows.Forms.Timer
        private System.Windows.Forms.Timer _FormTimer;

        private System.Windows.Forms.Timer FormTimer
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _FormTimer;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_FormTimer != null)
                {
                    _FormTimer.Tick -= OnTimedEvent;
                }

                _FormTimer = value;
                if (_FormTimer != null)
                {
                    _FormTimer.Tick += OnTimedEvent;
                }
            }
        }
        private System.Timers.Timer _TimersTimer;

        private System.Timers.Timer TimersTimer
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _TimersTimer;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_TimersTimer != null)
                {
                    _TimersTimer.Elapsed -= OnTimedEvent;
                }

                _TimersTimer = value;
                if (_TimersTimer != null)
                {
                    _TimersTimer.Elapsed += OnTimedEvent;
                }
            }
        }

        private bool IsForm, TraceEnabled;
        private TraceLogger TL;

        /// <summary>
        /// Timer tick event handler
        /// </summary>
        /// <remarks></remarks>
        [ComVisible(false)]
        public delegate void TickEventHandler();

        /// <summary>
        /// Fired once per Interval when timer is Enabled.
        /// </summary>
        /// <remarks>To sink this event in Visual Basic, declare the object variable using the WithEvents keyword.</remarks>
        public event TickEventHandler Tick; // Implements ITimer.Tick ' Declare the tick event

        #region New and IDisposable Support
        /// <summary>
        /// Create a new timer component
        /// </summary>
        /// <remarks></remarks>
        public Timer()
        {
            TL = new TraceLogger("", "Timer");
            TraceEnabled = GetBool(TRACE_TIMER, TRACE_TIMER_DEFAULT);
            TL.Enabled = TraceEnabled;

            TL.LogMessage("New", "Started on thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            FormTimer = new System.Windows.Forms.Timer();
            TL.LogMessage("New", "Created FormTimer");
            FormTimer.Enabled = false; // Default settings
            FormTimer.Interval = 1000; // Inital period - 1 second
            TL.LogMessage("New", "Set FormTimer interval");

            TimersTimer = new System.Timers.Timer();
            TL.LogMessage("New", "Created TimersTimer");

            TimersTimer.Enabled = false; // Default settings
            TimersTimer.Interval = 1000d; // Inital period - 1 second
            TL.LogMessage("New", "Set TimersTimer interval");

            try
            {
                TL.LogMessage("New", "Process FileName " + "\"" + Process.GetCurrentProcess().MainModule.FileName + "\"");
                var PE = new PEReader(Process.GetCurrentProcess().MainModule.FileName, TL);
                TL.LogMessage("New", "SubSystem " + PE.SubSystem().ToString());
                switch (PE.SubSystem())
                {
                    case PEReader.SubSystemType.WINDOWS_GUI: // Windows GUI app
                        {
                            IsForm = true;
                            break;
                        }
                    case PEReader.SubSystemType.WINDOWS_CUI: // Windows Console app
                        {
                            IsForm = false; // Unknown app type
                            break;
                        }

                    default:
                        {
                            IsForm = false;
                            break;
                        }

                }
                // If Process.GetCurrentProcess.MainModule.FileName.ToUpper.Contains("WSCRIPT.EXE") Then 'WScript is an exception that is marked GUI but behaves like console!
                // TL.LogMessage("New", "WScript.Exe found - Overriding IsForm to: False")
                // IsForm = False
                // End If
                // If Process.GetCurrentProcess.MainModule.FileName.ToUpper.Contains("ASTROART.EXE") Then 'WScript is an exception that is marked GUI but behaves like console!
                // TL.LogMessage("New", "AstroArt.Exe found - Overriding IsForm to: False")
                // IsForm = False
                // End If
                IsForm = !ForceTimer(IsForm); // Override the value of isform if required
                TL.LogMessage("New", "IsForm: " + IsForm);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("New Exception", ex.ToString()); // Log error and record in the event log
                LogEvent("Timer:New", "Exception", EventLogEntryType.Error, EventLogErrors.TimerSetupException, ex.ToString());
            }

            Tick += Timer_Tick;
        }

        private bool ForceTimer(bool CurrentIsForm)
        {
            var Profile = new RegistryAccess();
            System.Collections.Generic.SortedList<string, string> ForcedSystemTimers;
            string ProcessFileName;
            bool ForceSystemTimer, MatchedName;

            bool ForceTimerRet = !CurrentIsForm;
            TL.LogMessage("ForceTimer", "Current IsForm: " + CurrentIsForm.ToString() + ", this makes the default ForceTimer value: " + ForceTimerRet);

            ProcessFileName = Process.GetCurrentProcess().MainModule.FileName.ToUpperInvariant(); // Get the current process processname
            TL.LogMessage("ForceTimer", "Main process file name: " + ProcessFileName);

            MatchedName = false;
            ForcedSystemTimers = Profile.EnumProfile(FORCE_SYSTEM_TIMER); // Get the list of applications requiring special timer handling
            foreach (System.Collections.Generic.KeyValuePair<string, string> ForcedFileName in ForcedSystemTimers) // Check each forced file in turn 
            {
                if (ProcessFileName.Contains(Strings.Trim(ForcedFileName.Key.ToUpperInvariant()))) // We have matched the filename
                {
                    TL.LogMessage("ForceTimer", "  Found: \"" + ForcedFileName.Key + "\" = \"" + ForcedFileName.Value + "\"");
                    MatchedName = true;
                    if (bool.TryParse(ForcedFileName.Value, out ForceSystemTimer))
                    {
                        ForceTimerRet = ForceSystemTimer;
                        TL.LogMessage("ForceTimer", "    Parsed OK: " + ForceTimerRet.ToString() + ", ForceTimer set to: " + ForceTimerRet);
                    }
                    else
                    {
                        TL.LogMessage("ForceTimer", "    ***** Error - Value is not boolean!");
                    }
                }
                else
                {
                    TL.LogMessage("ForceTimer", "  Tried: \"" + ForcedFileName.Key + "\" = \"" + ForcedFileName.Value + "\"");
                }
            }
            if (!MatchedName)
                TL.LogMessage("ForceTimer", "  Didn't match any force timer application names");

            TL.LogMessage("ForceTimer", "Returning: " + ForceTimerRet.ToString());
            return ForceTimerRet;
        }

        private bool disposedValue = false;        // To detect redundant calls

        // IDisposable
        /// <summary>
        /// Disposes of resources used by the profile object - called by IDisposable interface
        /// </summary>
        /// <param name="disposing"></param>
        /// <remarks></remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    TL.Enabled = false;
                    TL.Dispose();
                }
                if (FormTimer is not null)
                {
                    if (FormTimer is not null)
                        FormTimer.Enabled = false;
                    FormTimer.Dispose();
                    FormTimer = null;
                }

            }
            disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        /// <summary>
        /// Disposes of resources used by the profile object
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finaliser
        /// </summary>
        ~Timer()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }

        #endregion

        #region Timer Implementation
        /// <summary>
        /// The interval between Tick events when the timer is Enabled in milliseconds, (default = 1000)
        /// </summary>
        /// <value>The interval between Tick events when the timer is Enabled (milliseconds, default = 1000)</value>
        /// <returns>The interval between Tick events when the timer is Enabled in milliseconds</returns>
        /// <remarks></remarks>
        public int Interval
        {
            // Get and set the timer period
            get
            {
                if (IsForm)
                {
                    TL.LogMessage("Interval FormTimer Get", FormTimer.Interval.ToString() + ", Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                    return FormTimer.Interval;
                }
                else
                {
                    TL.LogMessage("Interval TimersTimer Get", TimersTimer.Interval.ToString() + ", Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                    return FormTimer.Interval;
                }
            }
            set
            {
                if (IsForm)
                {
                    TL.LogMessage("Interval FormTimer Set", value.ToString() + ", Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                    if (value > 0)
                    {
                        FormTimer.Interval = value;
                    }
                    else
                    {
                        FormTimer.Enabled = false;
                    }
                }
                else
                {
                    TL.LogMessage("Interval TimersTimer Set", value.ToString() + ", Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                    if (value > 0)
                    {
                        TimersTimer.Interval = value;
                    }
                    else
                    {
                        TimersTimer.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Enable the timer tick events
        /// </summary>
        /// <value>True means the timer is active and will deliver Tick events every Interval milliseconds.</value>
        /// <returns>Enabled state of timer tick events</returns>
        /// <remarks></remarks>
        public bool Enabled
        {
            // Enable and disable the timer
            get
            {
                if (IsForm)
                {
                    TL.LogMessage("Enabled FormTimer Get", FormTimer.Enabled.ToString() + ", Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                    return FormTimer.Enabled;
                }
                else
                {
                    TL.LogMessage("Enabled TimersTimer Get", TimersTimer.Enabled.ToString() + ", Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                    return TimersTimer.Enabled;
                }
            }
            set
            {
                if (IsForm)
                {
                    TL.LogMessage("Enabled FormTimer Set", value.ToString() + ", Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                    FormTimer.Enabled = value;
                }
                else
                {
                    TL.LogMessage("Enabled TimersTimer Set", value.ToString() + ", Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                    TimersTimer.Enabled = value;
                }
            }
        }
        #endregion

        #region Support code
        // Raise the external event whenever a timer tick event occurs
        /// <summary>
        /// Timer event handler
        /// </summary>
        /// <remarks>Raises the Tick event</remarks>
        private void OnTimedEvent(object sender, object e)
        {
            if (IsForm)
            {
                EventArgs ec;
            }
            else
            {
                System.Timers.ElapsedEventArgs ec;
                ec = (System.Timers.ElapsedEventArgs)e;
                if (TraceEnabled)
                    TL.LogMessage("OnTimedEvent", "SignalTime: " + ec.SignalTime.ToString());

            }
            if (TraceEnabled)
                TL.LogMessage("OnTimedEvent", "Raising Tick" + ", Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()); // Ensure minimum hit on timing under normal, non-trace conditions
            Tick?.Invoke();
            if (TraceEnabled)
                TL.LogMessage("OnTimedEvent", "Raised Tick" + ", Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()); // Ensure minimum hit on timing under normal, non-trace, conditions
        }
        #endregion



        private void Timer_Tick()
        {

        }
    }
}