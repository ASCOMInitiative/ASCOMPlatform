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
    public class ObservingConditions : ReferenceCountedObjectBase, IObservingConditions
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
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            OCSimulator.SetupDialog(clientNumber);
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

        #region ObservingConditions Implementation

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

        public double SkyFWHM
        {
            get { return OCSimulator.SkyFWHM(clientNumber); }
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
    }
}
