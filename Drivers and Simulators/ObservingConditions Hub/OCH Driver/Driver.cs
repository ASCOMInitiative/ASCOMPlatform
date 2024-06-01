using System;
using System.Collections;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;

namespace ASCOM.Simulator
{

    /// <summary>
    /// ASCOM ObservingConditions Driver for Observing Conditions Hub.
    /// </summary>
    [Guid("6E5ED281-7149-44E9-9DFE-EB5425C00273")]
    [ProgId(Hub.DRIVER_PROGID)]
    [ServedClassName(Hub.DRIVER_DISPLAY_NAME)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ObservingConditions : ReferenceCountedObjectBase, IObservingConditionsV2
    {
        #region Variables and Constants

        internal static TraceLoggerPlus TL; // Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        private int clientNumber;
        private bool testMode, testConnected;

        private bool connecting; // Flag used when emulating the Connect / Disconnect methods
        private Exception connectException = null; // Placeholder for any exception generated when emulating asynchronous connection / disconnection on a Platform 6 or earlier device

        #endregion

        #region Class initialiser
        /// <summary>
        /// Initializes a new instance of the <see cref="Hub"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public ObservingConditions()
        {
            try
            {
                TL = Hub.TL;
                TL.LogMessage("ObservingConditions", "Starting initialisation");

                clientNumber = Hub.GetUniqueClientNumber();
                TL.LogMessage(clientNumber, "ObservingConditions", "This instance's unique client number: " + clientNumber);

                testMode = false;
                testConnected = false;
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
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            Hub.SetupDialog(clientNumber);
        }

        public ArrayList SupportedActions
        {
            get { return Hub.SupportedActions(clientNumber); }
        }

        public string Action(string actionName, string actionParameters)
        {
            if (actionName.ToUpperInvariant() == "SETTESTMODE")
            {
                testMode = true;
                TL.LogMessage(clientNumber, "Action", "SETTESTMODE received: Test mode now active");
                return "Test mode active";
            }
            return Hub.Action(clientNumber, actionName, actionParameters);
        }

        public void CommandBlind(string command, bool raw)
        {
            Hub.CommandBlind(clientNumber, command, raw);
        }

        public bool CommandBool(string command, bool raw)
        {
            return Hub.CommandBool(clientNumber, command, raw);
        }

        public string CommandString(string command, bool raw)
        {
            return Hub.CommandString(clientNumber, command, raw);
        }

        public bool Connected
        {
            get
            {
                if (testMode)
                {
                    TL.LogMessage(clientNumber, "Connected", "Test mode, returning: " + testConnected.ToString());
                    return testConnected;
                }
                return Hub.IsClientConnected(clientNumber);
            }
            set
            {
                if (testMode)
                {
                    TL.LogMessage(clientNumber, "Connected", "Setting connected to: " + value.ToString());
                    testConnected = value;
                }
                else
                {
                    if (value) Hub.ConnectDevices(clientNumber);
                    else Hub.DisconnectDevices(clientNumber);
                }
            }
        }

        public string Description
        {
            get
            {
                if (testMode)
                {
                    TL.LogMessage(clientNumber, "Description", "ObservingConditionsHub test mode description");
                    return "ObservingConditionsHub test mode description";
                }
                return Hub.Description(clientNumber);
            }
        }

        public void Dispose()
        {
        }

        public string DriverInfo
        {
            get
            {
                if (testMode)
                {
                    TL.LogMessage(clientNumber, "DriverInfo", "ObservingConditionsHub test mode driver information");
                    return "ObservingConditionsHub test mode driver information";
                }
                return Hub.DriverInfo(clientNumber);
            }
        }

        public string DriverVersion
        {
            get
            {
                if (testMode)
                {
                    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);

                    TL.LogMessage(clientNumber, "DriverVersion", driverVersion);
                    return driverVersion;
                }
                return Hub.DriverVersion(clientNumber);
            }
        }

        public short InterfaceVersion
        {
            get
            {
                if (testMode)
                {
                    TL.LogMessage(clientNumber, "InterfaceVersion", "2");
                    return 2;
                }
                return Hub.InterfaceVersion(clientNumber);
            }
        }

        public string Name
        {
            get
            {
                if (testMode)
                {
                    TL.LogMessage(clientNumber, "Name", "ASCOM Observing Conditions Hub (OCH)");
                    return "ASCOM Observing Conditions Hub (OCH)";
                }
                return Hub.Name(clientNumber);
            }
        }

        public void Connect()
        {
            TL.LogMessage("Connect", "Starting Connect process...");

            // Set Connecting to true and clear any previous exception
            connecting = true;
            connectException = null;

            // Run a task to set the Connected property to True
            Task connectingTask = Task.Factory.StartNew(() =>
            {
                // Ensure that no exceptions can escape
                try
                {
                    // Set Connected True
                    TL.LogMessage("ConnectTask", "About to set Connected True");
                    Connected = true;
                    TL.LogMessage("ConnectTask", "Connected Set True OK");
                }
                catch (Exception ex)
                {
                    // Something went wrong so log the issue and save the exception
                    TL.LogMessage("ConnectTask", $"Connected threw an exception: {ex.Message}");
                    connectException = ex;
                }
                // Ensure that Connecting is always set False at the end of the task
                finally
                {
                    TL.LogMessage("ConnectTask", "Setting Connecting to False");
                    connecting = false;
                }
            });

            TL.LogMessage("Connect", "Connect completed");
        }

        public void Disconnect()
        {
            TL.LogMessage("Disconnect", "Starting Disconnect process...");

            // Set Connecting to true and clear any previous exception
            connecting = true;
            connectException = null;

            // Run a task to set the Connected property to False
            Task disConnectingTask = Task.Factory.StartNew(() =>
            {
                // Ensure that no exceptions can escape
                try
                {
                    // Set Connected False
                    TL.LogMessage("DisconnectTask", "About to set Connected False");
                    Connected = false;
                    TL.LogMessage("DisconnectTask", "Connected Set False OK");
                }
                catch (Exception ex)
                {
                    // Something went wrong so save the exception
                    TL.LogMessage("DisconnectTask", $"Connected threw an exception: {ex.Message}");
                    connectException = ex;
                }
                // Ensure that Connecting is always set False at the end of the task
                finally
                {
                    TL.LogMessage("DisconnectTask", "Setting Connecting to False");
                    connecting = false;
                }
            });

            TL.LogMessage("Disconnect", "Disconnect completed");
        }

        public bool Connecting
        {
            get
            {
                // If Connected or disconnected threw an exception, throw this to the client
                if (!(connectException is null))
                {
                    TL.LogMessage("Connecting Get", $"Throwing exception from Connected to the client: {connectException.Message}\r\n{connectException}");
                    throw connectException;
                }

                // No exception so return emulated state
                return connecting;
            }
        }

        public IStateValueCollection DeviceState
        {
            get { return Hub.DeviceState(clientNumber); }
        }

        #endregion

        #region ObservingConditions Implementation

        public double AveragePeriod
        {
            get
            {
                if (testMode)
                {
                    TL.LogMessage(clientNumber, "AveragePeriod", "Test mode - returning 0.0");
                    return 0.0;
                }
                else
                {
                    return Hub.AveragePeriodGet(clientNumber);
                }
            }
            set { Hub.AveragePeriodSet(clientNumber, value); }
        }

        public double CloudCover
        {
            get { return Hub.CloudCover(clientNumber); }
        }

        public double DewPoint
        {
            get { return Hub.DewPoint(clientNumber); }
        }

        public double Humidity
        {
            get { return Hub.Humidity(clientNumber); }
        }

        public double Pressure
        {
            get { return Hub.Pressure(clientNumber); }
        }

        public double RainRate
        {
            get { return Hub.RainRate(clientNumber); }
        }

        public void Refresh()
        {
            Hub.Refresh(clientNumber);
        }

        public string SensorDescription(string PropertyName)
        {
            return Hub.SensorDescription(clientNumber, PropertyName);
        }

        public double SkyBrightness
        {
            get { return Hub.SkyBrightness(clientNumber); }
        }

        public double SkyQuality
        {
            get { return Hub.SkyQuality(clientNumber); }
        }

        public double StarFWHM
        {
            get { return Hub.StarFWHM(clientNumber); }
        }

        public double SkyTemperature
        {
            get { return Hub.SkyTemperature(clientNumber); }
        }

        public double Temperature
        {
            get { return Hub.Temperature(clientNumber); }
        }

        public double TimeSinceLastUpdate(string PropertyName)
        {
            if (testMode)
            {
                TL.LogMessage(clientNumber, "TimeSinceLastUpdate", "Test mode - returning 1.0");
                return 1.0;
            }
            else
            {
                return Hub.TimeSinceLastUpdate(clientNumber, PropertyName);
            }
        }

        public double WindDirection
        {
            get { return Hub.WindDirection(clientNumber); }
        }

        public double WindGust
        {
            get { return Hub.WindGust(clientNumber); }
        }

        public double WindSpeed
        {
            get { return Hub.WindSpeed(clientNumber); }
        }

        #endregion

    }
}
