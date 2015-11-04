//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM ObservingConditions driver for OpenWeatherMap
//
// Description:	ASCOM ObservingConditions driver for the online OpenWeatherMap weather web site.
//              The user must get an API code from OpenWeatherMap.
//
// Implements:	ASCOM ObservingConditions interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//

using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;

namespace ASCOM.OpenWeatherMap
{
    //
    // Your driver's DeviceID is ASCOM.OpenWeatherMap.ObservingConditions
    //
    // The Guid attribute sets the CLSID for ASCOM.OpenWeatherMap.ObservingConditions
    // The ClassInterface/None addribute prevents an empty interface called
    // _OpenWeatherMap from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM ObservingConditions Driver for OpenWeatherMap.
    /// </summary>
    [Guid("f6670ab1-fe72-44db-bf35-418beb8c2d9f")]
    [ClassInterface(ClassInterfaceType.None)]
    public class ObservingConditions : IObservingConditions
    {
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "OpenWeatherMap ObservingConditions";

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenWeatherMap"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public ObservingConditions()
        {
            OpenWeatherMap.ReadProfile(); // Read device configuration from the ASCOM Profile store

            Log.LogMessage("ObservingConditions", "Starting initialisation");

            //TODO: Implement your additional construction here

            //Log.LogMessage("ObservingConditions", "Completed initialisation");
        }


        //
        // PUBLIC COM INTERFACE IObservingConditions IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (OpenWeatherMap.Connected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm())
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    OpenWeatherMap.WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                Log.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the tracelogger and util objects
        }

        public bool Connected
        {
            get
            {
                return OpenWeatherMap.Connected;
            }
            set
            {
                OpenWeatherMap.Connected = value;
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                try
                {
                    var owmp = OpenWeatherMap.Description;
                    var dd = driverDescription;
                    if (!string.IsNullOrEmpty(owmp))
                    {
                        dd = "OpenWeatherMap: " + owmp;
                    }
                    Log.LogMessage("Description Get", dd);
                    return dd;
                }
                catch (Exception ex)
                {
                    Log.LogMessage("Description", "Error {0}", ex);
                    return "Description error " + ex.Message;
                }
            }
        }

        public string DriverInfo
        {
            get
            {
                try
                {
                    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    // TODO customise this driver description
                    string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                    Log.LogMessage("DriverInfo Get", driverInfo);
                    return driverInfo;
                }
                catch (Exception ex)
                {
                    Log.LogMessage("DriverInfo", "Error {0}", ex);
                    return "DriverInfo error " + ex.Message;
                }
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                Log.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                Log.LogMessage("InterfaceVersion Get", "1");
                return Convert.ToInt16("1");
            }
        }

        public string Name
        {
            get
            {
                string name = "Short driver name - please customise";
                Log.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IObservingConditions Implementation

        /// <summary>
        /// Gets and sets the time period over which observations wil be averaged
        /// </summary>
        /// <remarks>
        /// Get must be implemented, if it can't be changed it must return 0
        /// Time period (hours) over which the property values will be averaged 0.0 =
        /// current value, 0.5= average for the last 30 minutes, 1.0 = average for the
        /// last hour
        /// </remarks>
        public double AveragePeriod
        {
            get
            {
                Log.LogMessage("AveragePeriod", "get 0");
                return 0;
            }
            set
            {
                Log.LogMessage("AveragePeriod", "set {0}", value);
                if (value != 0)
                    throw new PropertyNotImplementedException("AveragePeriod", true);
            }
        }

        /// <summary>
        /// Amount of sky obscured by cloud
        /// </summary>
        /// <remarks>0%= clear sky, 100% = 100% cloud coverage</remarks>
        public double CloudCover
        {
            get
            {
                var cc = OpenWeatherMap.CloudCover;
                Log.LogMessage("CloudCover", "get {0}", cc);
                return cc;
                //throw new PropertyNotImplementedException("CloudCover", false);
            }
        }

        /// <summary>
        /// Atmospheric dew point at the observatory in deg C
        /// </summary>
        /// <remarks>
        /// Normally optional but mandatory if <see cref=" ASCOM.DeviceInterface.IObservingConditions.Humidity"/>
        /// Is provided
        /// </remarks>
        public double DewPoint
        {
            get
            {
                var dp = OpenWeatherMap.DewPoint;
                Log.LogMessage("DewPoint", "get {0}", dp);
                return Math.Round(dp, 1);
            }
        }

        /// <summary>
        /// Atmospheric relative humidity at the observatory in percent
        /// </summary>
        /// <remarks>
        /// Normally optional but mandatory if <see cref="ASCOM.DeviceInterface.IObservingConditions.DewPoint"/> 
        /// Is provided
        /// </remarks>
        public double Humidity
        {
            get
            {
                var hum = OpenWeatherMap.Humidity;
                Log.LogMessage("Humidity", "get {0}", hum);
                return hum;
            }
        }

        /// <summary>
        /// Atmospheric pressure at the observatory in hectoPascals (mB)
        /// </summary>
        /// <remarks>
        /// This must be the pressure at the observatory and not the "reduced" pressure
        /// at sea level. Please check whether your pressure sensor delivers local pressure
        /// or sea level pressure and adjust if required to observatory pressure.
        /// </remarks>
        public double Pressure
        {
            get
            {
                var p = OpenWeatherMap.Pressure;
                //Log.LogMessage("Pressure", "get {0}", p);
                return p;
            }
        }

        /// <summary>
        /// Rain rate at the observatory
        /// </summary>
        /// <remarks>
        /// This property can be interpreted as 0.0 = Dry any positive nonzero value
        /// = wet.
        /// </remarks>
        public double RainRate
        {
            get
            {
                var rr = OpenWeatherMap.RainRate;
                Log.LogMessage("RainRate", "get {0}", rr);
                return rr;
            }
        }

        /// <summary>
        /// Forces the driver to immediatley query its attached hardware to refresh sensor
        /// values
        /// </summary>
        public void Refresh()
        {
            OpenWeatherMap.UpdateWeather();
        }

        /// <summary>
        /// Provides a description of the sensor providing the requested property
        /// </summary>
        /// <param name="PropertyName">Name of the property whose sensor description is required</param>
        /// <returns>The sensor description string</returns>
        /// <remarks>
        /// PropertyName must be one of the sensor properties, 
        /// properties that are not implemented must throw the MethodNotImplementedException
        /// </remarks>
        public string SensorDescription(string PropertyName)
        {
            switch (PropertyName)
            {
                case "AveragePeriod":
                    return "Average period in hours, immediate values only are available";
                case "CloudCover":
                    return "Cloud cover, percent";
                case "DewPoint":
                    return "Dew Point, deg C";
                case "Humidity":
                    return "Relative humidity, percent";
                case "Pressure":
                    return "Pressure in hPa";
                case "RainRate":
                    return "Rainfall rate mm/Hr";
                case "Temperature":
                    return "Temperature deg C";
                case "WindDirection":
                    return "Wind direction, deg";
                case "WindSpeed":
                    return "Wind speed, m/s";
                case "SkyBrightness":
                case "SkyQuality":
                case "SkySeeing":
                case "SkyTemperature":
                case "WindGust":
                    Log.LogMessage("SensorDescription", PropertyName + " - not implemented");
                    throw new MethodNotImplementedException("SensorDescription(" + PropertyName + ")");
                default:
                    Log.LogMessage("SensorDescription", PropertyName + " - unrecognised");
                    throw new ASCOM.InvalidValueException("SensorDescription(" + PropertyName + ")");
            }
        }

        /// <summary>
        /// Sky brightness at the observatory
        /// </summary>
        public double SkyBrightness
        {
            get
            {
                Log.LogMessage("SkyBrightness", "get - not implemented");
                throw new PropertyNotImplementedException("SkyBrightness", false);
            }
        }

        /// <summary>
        /// Sky quality at the observatory
        /// </summary>
        public double SkyQuality
        {
            get
            {
                Log.LogMessage("SkyQuality", "get - not implemented");
                throw new PropertyNotImplementedException("SkyQuality", false);
            }
        }

        /// <summary>
        /// Seeing at the observatory
        /// </summary>
        public double SkySeeing
        {
            get
            {
                Log.LogMessage("SkySeeing", "get - not implemented");
                throw new PropertyNotImplementedException("SkySeeing", false);
            }
        }

        /// <summary>
        /// Sky temperature at the observatory in deg C
        /// </summary>
        public double SkyTemperature
        {
            get
            {
                Log.LogMessage("SkyTemperature", "get - not implemented");
                throw new PropertyNotImplementedException("SkyTemperature", false);
            }
        }

        /// <summary>
        /// Temperature at the observatory in deg C
        /// </summary>
        public double Temperature
        {
            get
            {
                var t = OpenWeatherMap.Temperature;
                Log.LogMessage("Temperature", "get {0}", t);
                return t;
            }
        }

        /// <summary>
        /// Provides the time since the sensor value was last updated
        /// </summary>
        /// <param name="PropertyName">Name of the property whose time since last update Is required</param>
        /// <returns>Time in seconds since the last sensor update for this property</returns>
        /// <remarks>
        /// PropertyName should be one of the sensor properties Or empty string to get
        /// the last update of any parameter. A negative value indicates no valid value
        /// ever received.
        /// </remarks>
        public double TimeSinceLastUpdate(string PropertyName)
        {
            var lu = OpenWeatherMap.TimeSinceLastUpdate;
            Log.LogMessage("TimeSinceLastUpdate", "{0}, {1}", PropertyName, lu);
            if (!string.IsNullOrEmpty(PropertyName))
            {
                try { var sd = SensorDescription(PropertyName); }
                catch { throw new MethodNotImplementedException(PropertyName); }
            }
            return lu;
        }

        /// <summary>
        /// Wind direction at the observatory in degrees
        /// </summary>
        /// <remarks>
        /// 0..360.0, 360=N, 180=S, 90=E, 270=W. When there Is no wind the driver will
        /// return a value of 0 for wind direction
        /// </remarks>
        public double WindDirection
        {
            get
            {
                var wd = OpenWeatherMap.WindDirection;
                Log.LogMessage("WindDirection", "get {0}", wd);
                return wd;
            }
        }

        /// <summary>
        /// Peak 3 second wind gust at the observatory over the last 2 minutes in m/s
        /// </summary>
        public double WindGust
        {
            get
            {
                Log.LogMessage("WindGust", "get - not implemented");
                throw new PropertyNotImplementedException("WindGust", false);
            }
        }

        /// <summary>
        /// Wind speed at the observatory in m/s
        /// </summary>
        public double WindSpeed
        {
            get
            {
                var ws = OpenWeatherMap.WindSpeed;
                Log.LogMessage("WindSpeed", "get {0}", ws);
                return ws;
            }
        }

        #endregion

        #region private methods


        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            Log.LogMessage(identifier, string.Format(message, args));
        }

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "ObservingConditions";
                if (bRegister)
                {
                    P.Register(OpenWeatherMap.driverID, driverDescription);
                }
                else
                {
                    P.Unregister(OpenWeatherMap.driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return OpenWeatherMap.Connected;
            }
        }

        #endregion

    }
}
