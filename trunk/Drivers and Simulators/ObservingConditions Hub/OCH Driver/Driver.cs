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
    /// ASCOM ObservingConditions Driver for Observing Conditions Hub.
    /// </summary>
    [Guid("6E5ED281-7149-44E9-9DFE-EB5425C00273")]
    [ProgId(Hub.DRIVER_PROGID)]
    [ServedClassName(Hub.DRIVER_DISPLAY_NAME)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ObservingConditions : ReferenceCountedObjectBase, IObservingConditions
    {
        #region Variables and Constants

        internal static TraceLoggerPlus TL; // Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        private int clientNumber;
        private bool testMode, testConnected;

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
            if (actionName.ToUpper() == "SETTESTMODE")
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
                    if (value) Hub.Connect(clientNumber);
                    else Hub.Disconnect(clientNumber);
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
                    TL.LogMessage(clientNumber, "DriverVersion", "6.2");
                    return "6.2";
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
                    TL.LogMessage(clientNumber, "InterfaceVersion", "1");
                    return 1;
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
