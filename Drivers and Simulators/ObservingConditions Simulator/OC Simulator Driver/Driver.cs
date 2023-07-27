using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;

namespace ASCOM.Simulator
{

    /// <summary>
    /// ASCOM ObservingConditions Driver for Observing Conditions OCSimulator.
    /// </summary>
    [Guid("3B0CE559-C92F-46A4-BCD8-1CCEC6E33F58")]
    [ProgId(OCSimulator.DRIVER_PROGID)]
    [ServedClassName(OCSimulator.DRIVER_DISPLAY_NAME)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ObservingConditions : ReferenceCountedObjectBase, IObservingConditionsV2
    {
        #region Variables and Constants

        internal static TraceLoggerPlus TL; // Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        private int clientNumber;
        private bool clientIsConnected;

        #endregion

        #region Class initialiser
        /// <summary>
        /// Initializes a new instance of the <see cref="OCSimulator"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public ObservingConditions()
        {
            try
            {
                TL = OCSimulator.TL;
                TL.LogMessage("ObservingConditions", "Starting initialisation");

                clientNumber = OCSimulator.GetUniqueClientNumber();
                TL.LogMessage(clientNumber, "ObservingConditions", "This instance's unique client number: " + clientNumber);

                TL.LogMessage(clientNumber, "ObservingConditions", "Completed initialisation");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("ObservingConditions", ex.ToString());
            }
        }

        #endregion

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            //OCSimulator.SetupDialog(clientNumber);
            if (OCSimulator.IsHardwareConnected())
                throw new InvalidOperationException("The hardware is connected, cannot do SetupDialog()");
            try
            {
                TL.LogMessage("SetupDialog()", $"Calling DoSetupDialog");
                Server.s_MainForm.DoSetupDialog(clientNumber);
            }
            catch (Exception ex)
            {
                //EventLogCode.LogEvent("ASCOM.Simulator.Telescope", "Exception on SetupDialog", EventLogEntryType.Error, GlobalConstants.EventLogErrors.TelescopeSimulatorSetup, ex.ToString());
                System.Windows.Forms.MessageBox.Show("ObservingConditions Simulator SetUp: " + ex.ToString());
            }

        }

        public ArrayList SupportedActions
        {
            get { return OCSimulator.SupportedActions(clientNumber); }
        }

        public string Action(string actionName, string actionParameters)
        { return OCSimulator.Action(clientNumber, actionName, actionParameters); }

        public void CommandBlind(string command, bool raw)
        {
            OCSimulator.CommandBlind(clientNumber, command, raw);
        }

        public bool CommandBool(string command, bool raw)
        {
            return OCSimulator.CommandBool(clientNumber, command, raw);
        }

        public string CommandString(string command, bool raw)
        {
            return OCSimulator.CommandString(clientNumber, command, raw);
        }

        public void Dispose()
        {
        }

        public bool Connected
        {
            get
            {
                //return OCSimulator.IsConnected(clientNumber);
                return clientIsConnected;
            }
            set
            {
                clientIsConnected = value;
                if (value) OCSimulator.Connect(clientNumber);
                else OCSimulator.Disconnect(clientNumber);
            }
        }

        public string Description
        {
            get { return OCSimulator.Description(clientNumber); }
        }

        public string DriverInfo
        {
            get { return OCSimulator.DriverInfo(clientNumber); }
        }

        public string DriverVersion
        {
            get { return OCSimulator.DriverVersion(clientNumber); }
        }

        public short InterfaceVersion
        {
            get { return OCSimulator.InterfaceVersion(clientNumber); }
        }

        public string Name
        {
            get { return OCSimulator.Name(clientNumber); }
        }

        #endregion

        #region IObservingConditionsV1 Implementation

        public double AveragePeriod
        {
            get { return OCSimulator.AveragePeriodGet(clientNumber); }
            set { OCSimulator.AveragePeriodSet(clientNumber, value); }
        }

        public double CloudCover
        {
            get { return OCSimulator.CloudCover(clientNumber); }
        }

        public double DewPoint
        {
            get { return OCSimulator.DewPoint(clientNumber); }
        }

        public double Humidity
        {
            get { return OCSimulator.Humidity(clientNumber); }
        }

        public double Pressure
        {
            get { return OCSimulator.Pressure(clientNumber); }
        }

        public double RainRate
        {
            get { return OCSimulator.RainRate(clientNumber); }
        }

        public void Refresh()
        {
            OCSimulator.Refresh(clientNumber);
        }

        public string SensorDescription(string PropertyName)
        {
            return OCSimulator.SensorDescription(clientNumber, PropertyName);
        }

        public double SkyBrightness
        {
            get { return OCSimulator.SkyBrightness(clientNumber); }
        }

        public double SkyQuality
        {
            get { return OCSimulator.SkyQuality(clientNumber); }
        }

        public double StarFWHM
        {
            get { return OCSimulator.StarFWHM(clientNumber); }
        }

        public double SkyTemperature
        {
            get { return OCSimulator.SkyTemperature(clientNumber); }
        }

        public double Temperature
        {
            get { return OCSimulator.Temperature(clientNumber); }
        }

        public double TimeSinceLastUpdate(string PropertyName)
        {
            return OCSimulator.TimeSinceLastUpdate(clientNumber, PropertyName);
        }

        public double WindDirection
        {
            get { return OCSimulator.WindDirection(clientNumber); }
        }

        public double WindGust
        {
            get { return OCSimulator.WindGust(clientNumber); }
        }

        public double WindSpeed
        {
            get { return OCSimulator.WindSpeed(clientNumber); }
        }

        #endregion

        #region IObservingConditionsV2 implementation

        public void Connect()
        {
            Connected = true;
        }

        public void Disconnect()
        {
            Connected = false;
        }

        public bool Connecting
        {
            get
            {
                return false;
            }
        }

        public ArrayList DeviceState
        {
            get
            {
                ArrayList deviceState = new ArrayList();

                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.CloudCover), CloudCover)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.DewPoint), DewPoint)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.Humidity), Humidity)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.Pressure), Pressure)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.RainRate), RainRate)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.SkyBrightness), SkyBrightness)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.SkyQuality), SkyQuality)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.SkyTemperature), SkyTemperature)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.StarFWHM), StarFWHM)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.Temperature), Temperature)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.WindDirection), WindDirection)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.WindSpeed), WindSpeed)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.WindGust), WindGust)); } catch { }
                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                return deviceState;
            }
        }

        #endregion
    }
}
