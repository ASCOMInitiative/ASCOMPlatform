//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Telescope driver for Telescope
//
// Description:	ASCOM Driver for Simulated Telescope 
//
// Implements:	ASCOM Telescope interface version: 2.0
// Author:		(rbt) Robert Turner <robert@robertturnerastro.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 07-JUL-2009	rbt	1.0.0	Initial edit, from ASCOM Telescope Driver template
// 29 Dec 2010  cdr         Extensive refactoring and bug fixes
// --------------------------------------------------------------------------------
//
// Ignore Spelling: Dialog

using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using static ASCOM.Utilities.Global;
using ASCOM.DriverAccess;

namespace ASCOM.Simulator
{
    //
    // Your driver's ID is ASCOM.Telescope.Telescope
    //
    // The Guid attribute sets the CLSID for ASCOM.Telescope.Telescope
    // The ClassInterface/None attribute prevents an empty interface called
    // _Telescope from being created and used as the [default] interface
    //

    [Guid("86931eac-1f52-4918-b6aa-7e9b0ff361bd")]
    [ServedClassName("Telescope Simulator for .NET")]
    [ProgId("ASCOM.Simulator.Telescope")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Telescope : ReferenceCountedObjectBase, ITelescopeV4
    {
        private const int UNPARK_COMPLETION_DELAY = 1000; // Delay time (ms) before an Unpark operation completes.

        // Constants for Action / SupportedActions
        private const string SLEW_TO_HA = "SlewToHA";
        private const string SLEW_TO_HA_UPPER = "SLEWTOHA";
        private const string ASSEMBLY_VERSION_NUMBER = "AssemblyVersionNumber";
        private const string ASSEMBLY_VERSION_NUMBER_UPPER = "ASSEMBLYVERSIONNUMBER";
        private const string TIME_UNTIL_POINTINSTATE_CAN_CHANGE = "TIMEUNTILPOINTINGSTATECANCHANGE";
        private const string AVAILABLE_TIME_IN_THIS_POINTING_STATE = "AVAILABLETIMEINTHISPOINTINGSTATE";

        // Driver private data (rate collections)
        private AxisRates[] m_AxisRates;
        private TrackingRates m_TrackingRates;
        private TrackingRatesSimple m_TrackingRatesSimple;
        private ASCOM.Utilities.Util m_Util;
        private string driverID;
        private long objectId;
        private bool connecting = false;

        private bool connected = false; // Holds connected state for this instance

        // Local copies of the guide rates are kept here because the TelescopeHardware GuideRateRightAscension and GuideRateDeclination values
        // can have their direction signs changed during PulseGuide operations.
        private double currentGuideRateRightAscension;
        private double currentGuideRateDeclination;

        // Constructor - Must be public for COM registration!
        public Telescope()
        {
            try
            {
                driverID = Marshal.GenerateProgIdForType(this.GetType());
                m_AxisRates = new AxisRates[3];
                m_AxisRates[0] = new AxisRates(TelescopeAxes.axisPrimary);
                m_AxisRates[1] = new AxisRates(TelescopeAxes.axisSecondary);
                m_AxisRates[2] = new AxisRates(TelescopeAxes.axisTertiary);
                m_TrackingRates = new TrackingRates();
                m_TrackingRatesSimple = new TrackingRatesSimple();
                m_Util = new ASCOM.Utilities.Util();
                // get a unique instance id
                objectId = TelescopeHardware.GetId();
                TelescopeHardware.TL.LogMessage("New", "Instance ID: " + objectId + ", new: " + "Driver ID: " + driverID);

                // Initialise the guide rates from the Telescope hardware default values
                currentGuideRateRightAscension = TelescopeHardware.GuideRateRightAscension;
                currentGuideRateDeclination = TelescopeHardware.GuideRateDeclination;
                if (TelescopeHardware.InterfaceVersion >= 4)
                    Connecting = false;
            }
            catch (Exception ex)
            {
                LogEvent("ASCOM.Simulator.Telescope", "Exception on New", EventLogEntryType.Error, EventLogErrors.TelescopeSimulatorNew, ex.ToString());
                System.Windows.Forms.MessageBox.Show("Telescope New: " + ex.ToString());
            }

        }

        #region ITelescopeV4 members

        /// <summary>
        /// Connect to the telescope asynchronously
        /// </summary>
        public void Connect()
        {
            // This method is only valid in interface V4 and later
            CheckCapability(TelescopeHardware.InterfaceVersion >= 4, "Connect");

            TelescopeHardware.TL.LogMessage("Connect Operation", $"Starting Connect()...");

            // Set the completion variable to the "process running" state
            Connecting = true;

            // Start a task that will flag the Connect operation as complete after a set time interval
            Task.Run(() =>
            {
                // Simulate a long connection phase
                Thread.Sleep(1000);

                // Set the Connected state to true
                Connected = true;

                // Set the completion variable to the "process complete" state to show that the Connect operation has completed
                Connecting = false;

                TelescopeHardware.TL.LogMessage("Connect Operation", $"Completed Connect()");
            });

            // End of the Connect operation initiator
        }

        /// <summary>
        /// Disconnect from the telescope asynchronously
        /// </summary>
        public void Disconnect()
        {
            // This method is only valid in interface V4 and later
            CheckCapability(TelescopeHardware.InterfaceVersion >= 4, "Disconnect");

            TelescopeHardware.TL.LogMessage("Disconnect Operation", $"Starting Disconnect...");

            // Set the completion variable to the "process running" state
            Connecting = true;

            // Start a task that will flag the Disconnect operation as complete after a set time interval
            Task.Run(() =>
            {
                // Simulate a long connection phase
                Thread.Sleep(1000);

                // Set the Connected state to true
                Connected = false;

                // Set the completion variable to the "process complete" state to show that the Disconnect operation has completed
                Connecting = false;

                TelescopeHardware.TL.LogMessage("Disconnect Operation", $"Completed Disconnect()");
            });

            // End of the Disconnect operation initiator
        }

        /// <summary>
        /// Connect / Disconnect completion variable. Returns true when an operation is underway, otherwise false
        /// </summary>
        public bool Connecting
        {
            get
            {
                // This method is only valid in interface V4 and later
                CheckCapability(TelescopeHardware.InterfaceVersion >= 4, "Connecting", false);

                return connecting;
            }

            private set
            {
                // This method is only valid in interface V4 and later
                CheckCapability(TelescopeHardware.InterfaceVersion >= 4, "Connecting", true);

                connecting = value;
            }
        }

        /// <summary>
        /// Return the device's operational state in one call
        /// </summary>
        public ArrayList DeviceState
        {
            get
            {
                // This method is only valid in interface V4 and later
                CheckCapability(TelescopeHardware.InterfaceVersion >= 4, "DeviceState", false);

                // Create an array list to hold the IStateValue entries
                ComArrayList deviceState = new ComArrayList();

                // Add one entry for each operational state, if possible
                try { deviceState.Add(new StateValue(nameof(ITelescopeV4.Altitude), Altitude)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ITelescopeV4.AtHome), AtHome)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ITelescopeV4.AtPark), AtPark)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ITelescopeV4.Azimuth), Azimuth)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ITelescopeV4.Declination), Declination)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ITelescopeV4.IsPulseGuiding), IsPulseGuiding)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ITelescopeV4.RightAscension), RightAscension)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ITelescopeV4.SideOfPier), SideOfPier)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ITelescopeV4.SiderealTime), SiderealTime)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ITelescopeV4.Slewing), Slewing)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ITelescopeV4.Tracking), Tracking)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ITelescopeV4.UTCDate), UTCDate)); } catch { }
                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                // Return the overall device state
                return deviceState;
            }
        }

        #endregion

        #region ITelescope Members

        public string Action(string ActionName, string ActionParameters)
        {
            //throw new MethodNotImplementedException("Action");
            string Response;
            if (ActionName == null)
                throw new InvalidValueException("no ActionName is provided");
            switch (ActionName.ToUpper(CultureInfo.InvariantCulture))
            {
                case ASSEMBLY_VERSION_NUMBER_UPPER:
                    Response = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    break;
                case SLEW_TO_HA_UPPER:
                    //Assume that we have just been supplied with an HA
                    //Let errors just go straight back to the caller
                    double HA = double.Parse(ActionParameters, CultureInfo.InvariantCulture);
                    double RA = this.SiderealTime - HA;
                    this.SlewToCoordinates(RA, 0.0);
                    Response = "Slew successful!";
                    break;
                case AVAILABLE_TIME_IN_THIS_POINTING_STATE:
                    Response = TelescopeHardware.AvailableTimeInThisPointingState.ToString();
                    break;
                case TIME_UNTIL_POINTINSTATE_CAN_CHANGE:
                    Response = TelescopeHardware.TimeUntilPointingStateCanChange.ToString();
                    break;
                default:
                    throw new ASCOM.InvalidOperationException("Command: '" + ActionName + "' is not recognised by the Scope Simulator .NET driver. " + ASSEMBLY_VERSION_NUMBER_UPPER + " " + SLEW_TO_HA_UPPER);
            }
            return Response;
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        public ArrayList SupportedActions
        {
            // no supported actions, return empty array
            get
            {
                ArrayList sa = new ArrayList
                {
                    ASSEMBLY_VERSION_NUMBER, // Add a test action to return a value
                    SLEW_TO_HA, // Expects a numeric HA Parameter
                    "AvailableTimeInThisPointingState",
                    "TimeUntilPointingStateCanChange"
                };

                return sa;
            }
        }

        public void AbortSlew()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "AbortSlew: ");
            CheckParked("AbortSlew");
            TelescopeHardware.AbortSlew();

            SharedResources.TrafficEnd("(done)");
        }

        public AlignmentModes AlignmentMode
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Capabilities, "AlignmentMode: ");
                CheckCapability(TelescopeHardware.CanAlignmentMode, "AlignmentMode");
                SharedResources.TrafficEnd(TelescopeHardware.AlignmentMode.ToString());

                switch (TelescopeHardware.AlignmentMode)
                {
                    case AlignmentModes.algAltAz:
                        return AlignmentModes.algAltAz;
                    case AlignmentModes.algGermanPolar:
                        return AlignmentModes.algGermanPolar;
                    case AlignmentModes.algPolar:
                        return AlignmentModes.algPolar;
                    default:
                        return AlignmentModes.algGermanPolar;
                }
            }
        }

        public double Altitude
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "Altitude: ");
                CheckCapability(TelescopeHardware.CanAltAz, "Altitude", false);
                if ((TelescopeHardware.AtPark || TelescopeHardware.SlewState == SlewType.SlewPark) && TelescopeHardware.NoCoordinatesAtPark)
                {
                    SharedResources.TrafficEnd("No coordinates at park!");
                    throw new PropertyNotImplementedException("Altitude", false);
                }
                SharedResources.TrafficEnd(m_Util.DegreesToDMS(TelescopeHardware.Altitude));
                return TelescopeHardware.Altitude;
            }
        }

        public double ApertureArea
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "ApertureArea: ");
                CheckCapability(TelescopeHardware.CanOptics, "ApertureArea", false);
                SharedResources.TrafficEnd(TelescopeHardware.ApertureArea.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.ApertureArea;
            }
        }

        public double ApertureDiameter
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "ApertureDiameter: ");
                CheckCapability(TelescopeHardware.CanOptics, "ApertureDiameter", false);
                SharedResources.TrafficEnd(TelescopeHardware.ApertureDiameter.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.ApertureDiameter;
            }
        }

        public bool AtHome
        {
            get
            {
                CheckVersionOne("AtHome", false);
                SharedResources.TrafficLine(SharedResources.MessageType.Polls, "AtHome: " + TelescopeHardware.AtHome);
                return TelescopeHardware.AtHome;
            }
        }

        public bool AtPark
        {
            get
            {
                CheckVersionOne("AtPark", false);
                SharedResources.TrafficLine(SharedResources.MessageType.Polls, "AtPark: " + TelescopeHardware.AtPark);
                return TelescopeHardware.AtPark;
            }
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary:
                    return new AxisRates(TelescopeAxes.axisPrimary);

                case TelescopeAxes.axisSecondary:
                    return new AxisRates(TelescopeAxes.axisSecondary);

                case TelescopeAxes.axisTertiary:
                    return new AxisRates(TelescopeAxes.axisTertiary);

                default: // Anything else is invalid so throw an exception
                    throw new InvalidValueException($"AxisRates - Invalid Axis parameter: {Axis}, The valid range is {Convert.ToInt32(Enum.GetValues(typeof(TelescopeAxes)).Cast<TelescopeAxes>().Min())} to {Convert.ToInt32(Enum.GetValues(typeof(TelescopeAxes)).Cast<TelescopeAxes>().Max())}");
            }
        }

        public double Azimuth
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "Azimuth: ");

                CheckCapability(TelescopeHardware.CanAltAz, "Azimuth", false);

                if ((TelescopeHardware.AtPark || TelescopeHardware.SlewState == SlewType.SlewPark) && TelescopeHardware.NoCoordinatesAtPark)
                {
                    SharedResources.TrafficEnd("No coordinates at park!");
                    throw new PropertyNotImplementedException("Azimuth", false);
                }
                SharedResources.TrafficEnd(m_Util.DegreesToDMS(TelescopeHardware.Azimuth));
                return TelescopeHardware.Azimuth;
            }
        }

        public bool CanFindHome
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanFindHome: " + TelescopeHardware.CanFindHome);
                return TelescopeHardware.CanFindHome;
            }
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Capabilities, string.Format(CultureInfo.CurrentCulture, "CanMoveAxis {0}: ", Axis.ToString()));

            // Validate the supplied axis parameter and action if valid
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary: // Valid values
                case TelescopeAxes.axisSecondary:
                case TelescopeAxes.axisTertiary:
                    CheckVersionOne("CanMoveAxis");
                    SharedResources.TrafficEnd(TelescopeHardware.CanMoveAxis(Axis).ToString());
                    return TelescopeHardware.CanMoveAxis(Axis);

                default:  // Anything else is invalid so throw an exception
                    throw new InvalidValueException($"CanMoveAxis - Invalid Axis parameter: {Axis}, The valid range is {Convert.ToInt32(Enum.GetValues(typeof(TelescopeAxes)).Cast<TelescopeAxes>().Min())} to {Convert.ToInt32(Enum.GetValues(typeof(TelescopeAxes)).Cast<TelescopeAxes>().Max())}");
            }
        }

        public bool CanPark
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanPark: " + TelescopeHardware.CanPark);
                return TelescopeHardware.CanPark;
            }
        }

        public bool CanPulseGuide
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanPulseGuide: " + TelescopeHardware.CanPulseGuide);
                return TelescopeHardware.CanPulseGuide;
            }
        }

        public bool CanSetDeclinationRate
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Capabilities, "CanSetDeclinationRate: ");
                CheckVersionOne("CanSetDeclinationRate", false);
                SharedResources.TrafficEnd(TelescopeHardware.CanSetDeclinationRate.ToString());
                return TelescopeHardware.CanSetDeclinationRate;
            }
        }

        public bool CanSetGuideRates
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSetGuideRates: " + TelescopeHardware.CanSetGuideRates);
                return TelescopeHardware.CanSetGuideRates;
            }
        }

        public bool CanSetPark
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSetPark: " + TelescopeHardware.CanSetPark);
                return TelescopeHardware.CanSetPark;
            }
        }

        public bool CanSetPierSide
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Capabilities, "CanSetPierSide: ");
                CheckVersionOne("CanSetPierSide", false);

                // The ASCOM interface specification states that Set SideOfPier is only valid for German equatorial mounts
                if (TelescopeHardware.AlignmentMode != AlignmentModes.algGermanPolar)
                {
                    SharedResources.TrafficEnd(false.ToString());
                    return false;
                }

                SharedResources.TrafficEnd(TelescopeHardware.CanSetPierSide.ToString());
                return TelescopeHardware.CanSetPierSide;
            }
        }

        public bool CanSetRightAscensionRate
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Capabilities, "CanSetRightAscensionRate: ");
                CheckVersionOne("CanSetRightAscensionRate", false);
                SharedResources.TrafficEnd(TelescopeHardware.CanSetRightAscensionRate.ToString());
                return TelescopeHardware.CanSetRightAscensionRate;
            }
        }

        public bool CanSetTracking
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSetTracking: " + TelescopeHardware.CanSetTracking);
                return TelescopeHardware.CanSetTracking;
            }
        }

        public bool CanSlew
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSlew: " + TelescopeHardware.CanSlew);
                return TelescopeHardware.CanSlew;
            }
        }

        public bool CanSlewAltAz
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Capabilities, "CanSlewAltAz: ");
                CheckVersionOne("CanSlewAltAz", false);
                SharedResources.TrafficEnd(TelescopeHardware.CanSlewAltAz.ToString());
                return TelescopeHardware.CanSlewAltAz;
            }
        }

        public bool CanSlewAltAzAsync
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Capabilities, "CanSlewAltAzAsync: ");
                CheckVersionOne("CanSlewAltAzAsync", false);
                SharedResources.TrafficEnd(TelescopeHardware.CanSlewAltAzAsync.ToString());
                return TelescopeHardware.CanSlewAltAzAsync;
            }
        }

        public bool CanSlewAsync
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSlewAsync: " + TelescopeHardware.CanSlewAsync);
                return TelescopeHardware.CanSlewAsync;
            }
        }

        public bool CanSync
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSync: " + TelescopeHardware.CanSync);
                return TelescopeHardware.CanSync;
            }
        }

        public bool CanSyncAltAz
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Capabilities, "CanSyncAltAz: ");
                CheckVersionOne("CanSyncAltAz", false);
                SharedResources.TrafficEnd(TelescopeHardware.CanSyncAltAz.ToString());
                return TelescopeHardware.CanSyncAltAz;
            }
        }

        public bool CanUnpark
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanUnPark: " + TelescopeHardware.CanUnpark);
                return TelescopeHardware.CanUnpark;
            }
        }

        public void CommandBlind(string Command, bool Raw)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string Command, bool Raw)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string Command, bool Raw)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("CommandString");
        }

        public bool Connected
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "Connected = " + connected.ToString());
                TelescopeHardware.TL.LogMessage("Connected Get", connected.ToString());
                return connected;
            }
            set
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "Set Connected to " + value.ToString());
                TelescopeHardware.TL.LogMessage("Connected Set", value.ToString());
                TelescopeHardware.SetConnected(objectId, value);
                connected = value;
            }
        }

        public double Declination
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "Declination: ");

                CheckCapability(TelescopeHardware.CanEquatorial, "Declination", false);

                if ((TelescopeHardware.AtPark || TelescopeHardware.SlewState == SlewType.SlewPark) && TelescopeHardware.NoCoordinatesAtPark)
                {
                    SharedResources.TrafficEnd("No coordinates at park!");
                    throw new PropertyNotImplementedException("Declination", false);
                }
                SharedResources.TrafficEnd(m_Util.DegreesToDMS(TelescopeHardware.Declination));
                return TelescopeHardware.Declination;
            }
        }

        public double DeclinationRate
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Gets, "DeclinationRate: " + TelescopeHardware.DeclinationRate);
                return TelescopeHardware.DeclinationRate;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "DeclinationRate:-> ");
                CheckCapability(TelescopeHardware.CanSetEquatorialRates, "DeclinationRate", true);
                TelescopeHardware.DeclinationRate = value;
                SharedResources.TrafficEnd(value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public string Description
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Gets, "Description: " + SharedResources.INSTRUMENT_DESCRIPTION);
                return SharedResources.INSTRUMENT_DESCRIPTION;
            }
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Other, "DestinationSideOfPier: ");
            CheckVersionOne("DestinationSideOfPier");
            SharedResources.TrafficStart(string.Format(CultureInfo.CurrentCulture, "Ra {0}, Dec {1} - ", RightAscension, Declination));
            CheckCapability(TelescopeHardware.CanDestinationSideOfPier, "DestinationSideOfPier");

            PierSide ps = TelescopeHardware.SideOfPierRaDec(RightAscension, Declination);
            SharedResources.TrafficEnd(ps.ToString());
            return ps;
        }

        public bool DoesRefraction
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Capabilities, "DoesRefraction: ");
                CheckVersionOne("DoesRefraction", false);
                SharedResources.TrafficEnd(TelescopeHardware.Refraction.ToString());
                return TelescopeHardware.Refraction;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Capabilities, "DoesRefraction: ->");
                CheckVersionOne("DoesRefraction", true);
                SharedResources.TrafficEnd(value.ToString());
                TelescopeHardware.Refraction = value;
            }
        }

        public string DriverInfo
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();

                string driverinfo = asm.FullName;

                SharedResources.TrafficLine(SharedResources.MessageType.Other, "DriverInfo: " + driverinfo);
                return driverinfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "DriverVersion: ");
                CheckVersionOne("DriverVersion", false);
                Assembly asm = Assembly.GetExecutingAssembly();

                string driverinfo = $"{asm.GetName().Version.Major}.{asm.GetName().Version.Minor}"; // Present the assembly number major and minor version numbers.

                SharedResources.TrafficEnd(driverinfo);
                return driverinfo;
            }
        }

        public EquatorialCoordinateType EquatorialSystem
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "EquatorialSystem: ");
                CheckVersionOne("EquatorialSystem", false);
                string output = "";
                EquatorialCoordinateType eq = EquatorialCoordinateType.equOther;

                switch (TelescopeHardware.EquatorialSystem)
                {
                    case 0:
                        eq = EquatorialCoordinateType.equOther;
                        output = "Other";
                        break;

                    case 1:
                        eq = EquatorialCoordinateType.equTopocentric;
                        output = "Local";
                        break;
                    case 2:
                        eq = EquatorialCoordinateType.equJ2000;
                        output = "J2000";
                        break;
                    case 3:
                        eq = EquatorialCoordinateType.equJ2050;
                        output = "J2050";
                        break;
                    case 4:
                        eq = EquatorialCoordinateType.equB1950;
                        output = "B1950";
                        break;
                }
                SharedResources.TrafficEnd(output);
                return eq;
            }
        }

        public void FindHome()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "FindHome: ");
            CheckCapability(TelescopeHardware.CanFindHome, "FindHome");

            CheckParked("FindHome");

            TelescopeHardware.FindHome();

            if (TelescopeHardware.InterfaceVersion < 4)
            {
                // Interface v3 and earlier behaviour
                while (TelescopeHardware.SlewState == SlewType.SlewHome || TelescopeHardware.SlewState == SlewType.SlewSettle)
                {
                    Thread.Sleep(50);
                    System.Windows.Forms.Application.DoEvents();
                }
            }

            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "(done)");
        }

        public double FocalLength
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "FocalLength: ");
                CheckVersionOne("FocalLength", false);
                CheckCapability(TelescopeHardware.CanOptics, "FocalLength", false);
                SharedResources.TrafficEnd(TelescopeHardware.FocalLength.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.FocalLength;
            }
        }

        public double GuideRateDeclination
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "GuideRateDeclination: ");
                CheckVersionOne("GuideRateDeclination", false);
                SharedResources.TrafficEnd(TelescopeHardware.GuideRateDeclination.ToString(CultureInfo.InvariantCulture));
                return currentGuideRateDeclination; // Return the value set by the user
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "GuideRateDeclination->: ");
                CheckVersionOne("GuideRateDeclination", true);
                SharedResources.TrafficEnd(value.ToString(CultureInfo.InvariantCulture));
                currentGuideRateDeclination = value; // Save the value set by the user so that it can be returned by GET GuideRateDeclination
                TelescopeHardware.GuideRateDeclination = value;
            }
        }

        public double GuideRateRightAscension
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "GuideRateRightAscension: ");
                CheckVersionOne("GuideRateRightAscension", false);
                SharedResources.TrafficEnd(TelescopeHardware.GuideRateRightAscension.ToString(CultureInfo.InvariantCulture));
                return currentGuideRateRightAscension; // Return the value set by the user
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "GuideRateRightAscension->: ");
                CheckVersionOne("GuideRateRightAscension", true);
                SharedResources.TrafficEnd(value.ToString(CultureInfo.InvariantCulture));
                currentGuideRateRightAscension = value; // Save the value set by the user so that it can be returned by GET GuideRateRightAscension
                TelescopeHardware.GuideRateRightAscension = value;
            }
        }

        public short InterfaceVersion
        {
            get
            {
                CheckVersionOne("InterfaceVersion", false);
                SharedResources.TrafficLine(SharedResources.MessageType.Other, $"InterfaceVersion: {TelescopeHardware.InterfaceVersion}");
                return TelescopeHardware.InterfaceVersion;
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Polls, "IsPulseGuiding: ");
                // TODO Is this correct, should it just return false?
                CheckCapability(TelescopeHardware.CanPulseGuide, "IsPulseGuiding", false);
                SharedResources.TrafficEnd(TelescopeHardware.IsPulseGuiding.ToString());

                return TelescopeHardware.IsPulseGuiding;
            }
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, string.Format(CultureInfo.CurrentCulture, "MoveAxis {0} {1}:  ", Axis.ToString(), Rate));
            CheckVersionOne("MoveAxis");

            // Validate the supplied axis parameter and action if valid
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary: // Valid values
                case TelescopeAxes.axisSecondary:
                case TelescopeAxes.axisTertiary:
                    CheckRate(Axis, Rate);

                    if (!CanMoveAxis(Axis))
                        throw new MethodNotImplementedException("CanMoveAxis " + Enum.GetName(typeof(TelescopeAxes), Axis));

                    CheckParked("MoveAxis");

                    TelescopeHardware.TL.LogMessage("MoveAxis", $"Axis {Axis} set to {Rate} degrees per second");

                    // Manage the operation status based on whether or not the supplied rate is zero
                    if (Rate == 0.0) // Rate is zero
                    {
                        switch (TelescopeHardware.CurrentOperation)
                        {
                            case Operation.Uninitialised: // These should never happen!
                            case Operation.All:
                                throw new InvalidValueException($"TelescopeSimulator.MoveAxis - Operation state is {TelescopeHardware.CurrentOperation}, which should never happen!");

                            case Operation.None: // No operation currently underway
                                // No operation currently underway so ignore this request to stop movement.
                                TelescopeHardware.TL.LogMessage("MoveAxis", $"Request to set the MoveAxis rate to 0.0 and no operation currently running - ignoring request. Slewing: {TelescopeHardware.IsSlewing}");
                                break;

                            case Operation.MoveAxis: // A MoveAxis operation is underway
                                if (Axis == TelescopeAxes.axisPrimary) // Rate is being set for the Primary axis
                                {
                                    // Check whether the secondary axis is currently moving
                                    if (TelescopeHardware.rateMoveAxes.Y == 0.0) // Secondary axis does not have a rate set
                                    {
                                        // Both axes now have a rate of 0.0 so end the MoveAxis operation
                                        TelescopeHardware.EndOperation("MoveAxis - Primary");
                                    }
                                    else // Secondary axis does have a rate set
                                    {
                                        // No action - A MoveAxis operation is still in progress on the secondary axis so leave the current operation state intact
                                    }
                                }
                                else if (Axis == TelescopeAxes.axisSecondary) // Rate is being set for the Secondary axis
                                {
                                    // Check whether the primary axis is currently moving
                                    if (TelescopeHardware.rateMoveAxes.X == 0.0) // Primary axis does not have a rate set
                                    {
                                        // Both axes now have a rate of 0.0 so end the MoveAxis operation
                                        TelescopeHardware.EndOperation("MoveAxis - Secondary");
                                    }
                                    else // Primary axis does have a rate set
                                    {
                                        // No action - A MoveAxis operation is still in progress on the primary axis so leave the current operation state intact
                                    }
                                }
                                else // Tertiary axis
                                {
                                    // We don't support the tertiary axis in this simulator so ignore
                                }
                                break;

                            default: // Some other operation is underway
                                // No need to change the current operation state.
                                break;
                        }
                    }
                    else // Non-zero rate
                    {
                        switch (TelescopeHardware.CurrentOperation)
                        {
                            case Operation.Uninitialised: // These should never happen!
                            case Operation.All:
                                throw new InvalidValueException($"TelescopeSimulator.MoveAxis - Operation state is {TelescopeHardware.CurrentOperation}, which should never happen!");

                            default: // No operation, MoveAxis or some other operation is underway so start a MoveAxis operation
                                if ((Axis == TelescopeAxes.axisPrimary) | (Axis == TelescopeAxes.axisSecondary)) // Rate is being set for the Primary or Secondary axis
                                {
                                    // Start a MoveAxis operation
                                    TelescopeHardware.StartOperation(Operation.MoveAxis);
                                }
                                else // Tertiary axis
                                {
                                    // We don't support the tertiary axis in this simulator so ignore
                                }
                                break;
                        }
                    }

                    // Set the supplied rate
                    switch (Axis)
                    {
                        case TelescopeAxes.axisPrimary:
                            TelescopeHardware.rateMoveAxes.X = Rate;
                            break;
                        case TelescopeAxes.axisSecondary:
                            TelescopeHardware.rateMoveAxes.Y = Rate;
                            break;
                        case TelescopeAxes.axisTertiary:
                            // not implemented
                            break;
                    }

                    SharedResources.TrafficEnd("(done)");
                    break;

                default:  // Anything else is invalid so throw an exception
                    throw new InvalidValueException($"MoveAxis - Invalid Axis parameter: {Axis}, The valid range is {Convert.ToInt32(Enum.GetValues(typeof(TelescopeAxes)).Cast<TelescopeAxes>().Min())} to {Convert.ToInt32(Enum.GetValues(typeof(TelescopeAxes)).Cast<TelescopeAxes>().Max())}");
            }

        }

        public string Name
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "Description: " + SharedResources.INSTRUMENT_NAME);
                return SharedResources.INSTRUMENT_NAME;
            }
        }

        public void Park()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "Park: ");
            CheckCapability(TelescopeHardware.CanPark, "Park");

            if (TelescopeHardware.IsParked)
            {
                SharedResources.TrafficEnd("(Is Parked)");
                return;
            }
            TelescopeHardware.Park();

            if (TelescopeHardware.InterfaceVersion < 4)
            {
                // Interface v3 and earlier behaviour
                while (TelescopeHardware.SlewState == SlewType.SlewPark)
                {
                    Thread.Sleep(50);
                    System.Windows.Forms.Application.DoEvents();
                }
            }
            SharedResources.TrafficEnd("(done)");
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            if (TelescopeHardware.AtPark) throw new ParkedException();

            // Validate the supplied direction and action if valid
            switch (Direction)
            {
                case GuideDirections.guideNorth: // Valid direction parameter
                case GuideDirections.guideSouth:
                case GuideDirections.guideEast:
                case GuideDirections.guideWest:
                    SharedResources.TrafficStart(SharedResources.MessageType.Slew, string.Format(CultureInfo.CurrentCulture, "Pulse Guide: {0}, {1}", Direction, Duration.ToString(CultureInfo.InvariantCulture)));

                    CheckCapability(TelescopeHardware.CanPulseGuide, "PulseGuide");
                    CheckRange(Duration, 0, 30000, "PulseGuide", "Duration");

                    if (Duration == 0)
                    {
                        // stops the current guide command
                        switch (Direction)
                        {
                            case GuideDirections.guideNorth:
                            case GuideDirections.guideSouth:
                                TelescopeHardware.isPulseGuidingDec = false;
                                TelescopeHardware.guideDuration.Y = 0;
                                break;
                            case GuideDirections.guideEast:
                            case GuideDirections.guideWest:
                                TelescopeHardware.isPulseGuidingRa = false;
                                TelescopeHardware.guideDuration.X = 0;
                                break;
                        }
                    }
                    else // Start the pulse guide
                    {
                        // Start a PulseGuide operation if necessary
                        if (!IsPulseGuiding)
                        {
                            TelescopeHardware.StartOperation(Operation.PulseGuide);
                        }

                        switch (Direction)
                        {
                            case GuideDirections.guideNorth:
                                TelescopeHardware.guideRate.Y = Math.Abs(TelescopeHardware.guideRate.Y);
                                TelescopeHardware.isPulseGuidingDec = true;
                                TelescopeHardware.guideDuration.Y = Duration / 1000.0;
                                break;
                            case GuideDirections.guideSouth:
                                TelescopeHardware.guideRate.Y = -Math.Abs(TelescopeHardware.guideRate.Y);
                                //TelescopeHardware.pulseGuideDecEndTime = endTime;
                                TelescopeHardware.isPulseGuidingDec = true;
                                TelescopeHardware.guideDuration.Y = Duration / 1000.0;
                                break;

                            case GuideDirections.guideEast:
                                TelescopeHardware.guideRate.X = -Math.Abs(TelescopeHardware.guideRate.X);
                                //TelescopeHardware.pulseGuideRaEndTime = endTime;
                                TelescopeHardware.isPulseGuidingRa = true;
                                TelescopeHardware.guideDuration.X = Duration / 1000.0;
                                break;
                            case GuideDirections.guideWest:
                                TelescopeHardware.guideRate.X = Math.Abs(TelescopeHardware.guideRate.X);
                                //TelescopeHardware.pulseGuideRaEndTime = endTime;
                                TelescopeHardware.isPulseGuidingRa = true;
                                TelescopeHardware.guideDuration.X = Duration / 1000.0;
                                break;
                        }
                    }

                    if (!TelescopeHardware.CanDualAxisPulseGuide) // Single axis synchronous pulse guide
                    {
                        System.Threading.Thread.Sleep(Duration); // Must be synchronous so wait out the pulseguide duration here
                        TelescopeHardware.isPulseGuidingRa = false; // Make sure that IsPulseGuiding will return false
                        TelescopeHardware.isPulseGuidingDec = false;
                    }
                    SharedResources.TrafficEnd(" (done) ");
                    break;

                default: // Anything else is invalid
                    throw new InvalidValueException($"PulseGuide - Invalid Direction parameter: {Direction}, The valid range is {Convert.ToInt32(Enum.GetValues(typeof(GuideDirections)).Cast<GuideDirections>().Min())} to {Convert.ToInt32(Enum.GetValues(typeof(GuideDirections)).Cast<GuideDirections>().Max())}");
            }
        }

        public double RightAscension
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "Right Ascension: ");

                CheckCapability(TelescopeHardware.CanEquatorial, "RightAscension", false);

                if ((TelescopeHardware.AtPark || TelescopeHardware.SlewState == SlewType.SlewPark) && TelescopeHardware.NoCoordinatesAtPark)
                {
                    SharedResources.TrafficEnd("No coordinates at park!");
                    throw new PropertyNotImplementedException("RightAscension", false);
                }
                SharedResources.TrafficEnd(m_Util.HoursToHMS(TelescopeHardware.RightAscension));
                return TelescopeHardware.RightAscension;
            }
        }

        public double RightAscensionRate
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Gets, "RightAscensionRate: " + TelescopeHardware.RightAscensionRate);
                return TelescopeHardware.RightAscensionRate;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "RightAscensionRate:-> ");
                CheckCapability(TelescopeHardware.CanSetEquatorialRates, "RightAscensionRate", true);
                TelescopeHardware.RightAscensionRate = value;
                SharedResources.TrafficEnd(value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public void SetPark()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Other, "Set Park: ");
            CheckCapability(TelescopeHardware.CanSetPark, "SetPark");

            TelescopeHardware.ParkAltitude = TelescopeHardware.Altitude;
            TelescopeHardware.ParkAzimuth = TelescopeHardware.Azimuth;

            SharedResources.TrafficEnd("(done)");
        }

        public void SetupDialog()
        {
            if (TelescopeHardware.Connected)
                throw new InvalidOperationException("The hardware is connected, cannot do SetupDialog()");
            try
            {
                TelescopeSimulator.m_MainForm.DoSetupDialog();
            }
            catch (Exception ex)
            {
                LogEvent("ASCOM.Simulator.Telescope", "Exception on SetupDialog", EventLogEntryType.Error, EventLogErrors.TelescopeSimulatorSetup, ex.ToString());
                System.Windows.Forms.MessageBox.Show("Telescope SetUp: " + ex.ToString());
            }
        }

        public PierSide SideOfPier
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Polls, string.Format("SideOfPier: {0}", TelescopeHardware.SideOfPier));
                CheckCapability(TelescopeHardware.CanPierSide, "SideOfPier", false);
                return TelescopeHardware.SideOfPier;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SideOfPier: ");
                CheckCapability(TelescopeHardware.CanSetPierSide, "SideOfPier", true);

                // The ASCOM interface specification states that Set SideOfPier is only valid for German equatorial mounts
                CheckCapability(TelescopeHardware.AlignmentMode == AlignmentModes.algGermanPolar, "SideOfPier", true);

                if (value == TelescopeHardware.SideOfPier)
                {
                    SharedResources.TrafficEnd("(no change needed)");
                    return;
                }

                // TODO implement this correctly, it needs an overlap which can be reached on either side
                TelescopeHardware.SideOfPier = value;

                // slew to the same position, changing the side of pier appropriately if possible
                TelescopeHardware.StartSlewRaDec(TelescopeHardware.RightAscension, TelescopeHardware.Declination, true, Operation.SideOfPier);
                SharedResources.TrafficEnd("(started)");
            }
        }

        public double SiderealTime
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Time, "Sidereal Time: ");
                CheckCapability(TelescopeHardware.CanSiderealTime, "SiderealTime", false);
                SharedResources.TrafficEnd(m_Util.HoursToHMS(TelescopeHardware.SiderealTime));
                return TelescopeHardware.SiderealTime;
            }
        }

        public double SiteElevation
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteElevation: ");

                CheckCapability(TelescopeHardware.CanLatLongElev, "SiteElevation", false);
                SharedResources.TrafficEnd(TelescopeHardware.Elevation.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.Elevation;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteElevation: ->");
                CheckCapability(TelescopeHardware.CanLatLongElev, "SiteElevation", true);
                CheckRange(value, -300, 10000, "SiteElevation");
                SharedResources.TrafficEnd(value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.Elevation = value;
            }
        }

        public double SiteLatitude
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteLatitude: ");

                CheckCapability(TelescopeHardware.CanLatLongElev, "SiteLatitude", false);
                SharedResources.TrafficEnd(TelescopeHardware.Latitude.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.Latitude;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteLatitude: ->");
                CheckCapability(TelescopeHardware.CanLatLongElev, "SiteLatitude", true);
                CheckRange(value, -90, 90, "SiteLatitude");
                SharedResources.TrafficEnd(value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.Latitude = value;
            }
        }

        public double SiteLongitude
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteLongitude: ");
                CheckCapability(TelescopeHardware.CanLatLongElev, "SiteLongitude", false);
                SharedResources.TrafficEnd(TelescopeHardware.Longitude.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.Longitude;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteLongitude: ->");
                CheckCapability(TelescopeHardware.CanLatLongElev, "SiteLongitude", true);
                CheckRange(value, -180, 180, "SiteLongitude");
                SharedResources.TrafficEnd(value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.Longitude = value;
            }
        }

        public short SlewSettleTime
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "SlewSettleTime: " + (TelescopeHardware.SlewSettleTime * 1000).ToString(CultureInfo.InvariantCulture));
                return (short)(TelescopeHardware.SlewSettleTime);
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "SlewSettleTime:-> ");
                CheckRange(value, 0, 100, "SlewSettleTime");
                SharedResources.TrafficEnd(value + " (done)");
                TelescopeHardware.SlewSettleTime = value;
            }
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SlewToAltAz: ");
            CheckCapability(TelescopeHardware.CanSlewAltAz, "SlewToAltAz");
            CheckParked("SlewToAltAz");
            TelescopeHardware.RestoreTrackingStateIfNecessary(); // Restore the configured Tracking state if the client is prevented from doing this by simulator configuration
            CheckTracking(false, "SlewToAltAz");
            CheckRange(Azimuth, 0, 360, "SlewToltAz", "azimuth");
            CheckRange(Altitude, -90, 90, "SlewToAltAz", "Altitude");

            SharedResources.TrafficStart(" Alt " + m_Util.DegreesToDMS(Altitude) + " Az " + m_Util.DegreesToDMS(Azimuth));

            TelescopeHardware.StartSlewAltAz(Altitude, Azimuth, Operation.SlewToAltAzAsync);

            while (TelescopeHardware.SlewState == SlewType.SlewAltAz || TelescopeHardware.SlewState == SlewType.SlewSettle)
            {
                Thread.Sleep(500);
                System.Windows.Forms.Application.DoEvents();
            }
            SharedResources.TrafficEnd(" done");
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SlewToAltAzAsync: ");
            CheckCapability(TelescopeHardware.CanSlewAltAzAsync, "SlewToAltAzAsync");
            CheckParked("SlewToAltAz");
            TelescopeHardware.RestoreTrackingStateIfNecessary(); // Restore the configured Tracking state if the client is prevented from doing this by simulator configuration
            CheckTracking(false, "SlewToAltAzAsync");
            CheckRange(Azimuth, 0, 360, "SlewToAltAzAsync", "Azimuth");
            CheckRange(Altitude, -90, 90, "SlewToAltAzAsync", "Altitude");

            SharedResources.TrafficStart(" Alt " + m_Util.DegreesToDMS(Altitude) + " Az " + m_Util.DegreesToDMS(Azimuth));

            TelescopeHardware.StartSlewAltAz(Altitude, Azimuth, Operation.SlewToAltAzAsync);
            SharedResources.TrafficEnd(" started");
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SlewToCoordinates: ");
            CheckCapability(TelescopeHardware.CanSlew, "SlewToCoordinates");
            CheckRange(RightAscension, 0, 24, "SlewToCoordinates", "RightAscension");
            CheckRange(Declination, -90, 90, "SlewToCoordinates", "Declination");
            CheckParked("SlewToCoordinates");
            TelescopeHardware.RestoreTrackingStateIfNecessary(); // Restore the configured Tracking state if the client is prevented from doing this by simulator configuration
            CheckTracking(true, "SlewToCoordinates");

            SharedResources.TrafficStart(" RA " + m_Util.HoursToHMS(RightAscension) + " DEC " + m_Util.DegreesToDMS(Declination));

            TelescopeHardware.TargetRightAscension = RightAscension; // Set the Target RA and Dec prior to the Slew attempt per the ASCOM Telescope specification
            TelescopeHardware.TargetDeclination = Declination;

            TelescopeHardware.StartSlewRaDec(RightAscension, Declination, true, Operation.SlewToCoordinatesAsync);

            //while (TelescopeHardware.SlewState == SlewType.SlewRaDec || TelescopeHardware.SlewState == SlewType.SlewSettle)
            while (TelescopeHardware.IsSlewing)
            {
                Thread.Sleep(500);
                System.Windows.Forms.Application.DoEvents();
            }
            SharedResources.TrafficEnd("done");
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SlewToCoordinatesAsync: ");
            CheckCapability(TelescopeHardware.CanSlewAsync, "SlewToCoordinatesAsync");
            CheckRange(RightAscension, 0, 24, "SlewToCoordinatesAsync", "RightAscension");
            CheckRange(Declination, -90, 90, "SlewToCoordinatesAsync", "Declination");
            CheckParked("SlewToCoordinatesAsync");
            TelescopeHardware.RestoreTrackingStateIfNecessary(); // Restore the configured Tracking state if the client is prevented from doing this by simulator configuration
            CheckTracking(true, "SlewToCoordinatesAsync");

            TelescopeHardware.TargetRightAscension = RightAscension; // Set the Target RA and Dec prior to the Slew attempt per the ASCOM Telescope specification
            TelescopeHardware.TargetDeclination = Declination;

            SharedResources.TrafficStart(" RA " + m_Util.HoursToHMS(RightAscension) + " DEC " + m_Util.DegreesToDMS(Declination));

            TelescopeHardware.StartSlewRaDec(RightAscension, Declination, true, Operation.SlewToCoordinatesAsync);
            SharedResources.TrafficEnd("started");
        }

        public void SlewToTarget()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SlewToTarget: ");
            CheckCapability(TelescopeHardware.CanSlew, "SlewToTarget");
            CheckRange(TelescopeHardware.TargetRightAscension, 0, 24, "SlewToTarget", "TargetRightAscension");
            CheckRange(TelescopeHardware.TargetDeclination, -90, 90, "SlewToTarget", "TargetDeclination");
            CheckParked("SlewToTarget");
            TelescopeHardware.RestoreTrackingStateIfNecessary(); // Restore the configured Tracking state if the client is prevented from doing this by simulator configuration
            CheckTracking(true, "SlewToTarget");

            TelescopeHardware.StartSlewRaDec(TelescopeHardware.TargetRightAscension, TelescopeHardware.TargetDeclination, true, Operation.SlewToTargetAsync);

            while (TelescopeHardware.SlewState == SlewType.SlewRaDec || TelescopeHardware.SlewState == SlewType.SlewSettle)
            {
                Thread.Sleep(500);
                System.Windows.Forms.Application.DoEvents();
            }
            SharedResources.TrafficEnd("done");
        }

        public void SlewToTargetAsync()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SlewToTargetAsync: ");
            CheckCapability(TelescopeHardware.CanSlewAsync, "SlewToTargetAsync");
            CheckRange(TelescopeHardware.TargetRightAscension, 0, 24, "SlewToTargetAsync", "TargetRightAscension");
            CheckRange(TelescopeHardware.TargetDeclination, -90, 90, "SlewToTargetAsync", "TargetDeclination");
            CheckParked("SlewToTargetAsync");
            TelescopeHardware.RestoreTrackingStateIfNecessary(); // Restore the configured Tracking state if the client is prevented from doing this by simulator configuration
            CheckTracking(true, "SlewToTargetAsync");

            TelescopeHardware.StartSlewRaDec(TelescopeHardware.TargetRightAscension, TelescopeHardware.TargetDeclination, true, Operation.SlewToTargetAsync);
        }

        public bool Slewing
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Polls, string.Format(CultureInfo.CurrentCulture, "Slewing: {0}", TelescopeHardware.SlewState != SlewType.SlewNone));
                return TelescopeHardware.IsSlewing;
            }
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SyncToAltAz: ");
            CheckCapability(TelescopeHardware.CanSyncAltAz, "SyncToAltAz");
            CheckRange(Azimuth, 0, 360, "SyncToAltAz", "Azimuth");
            CheckRange(Altitude, -90, 90, "SyncToAltAz", "Altitude");
            CheckParked("SyncToAltAz");
            TelescopeHardware.RestoreTrackingStateIfNecessary(); // Restore the configured Tracking state if the client is prevented from doing this by simulator configuration
            CheckTracking(false, "SyncToAltAz");

            SharedResources.TrafficStart(" Alt " + m_Util.DegreesToDMS(Altitude) + " Az " + m_Util.DegreesToDMS(Azimuth));

            TelescopeHardware.ChangePark(false);

            TelescopeHardware.SyncToAltAz(Azimuth, Altitude);

            //TelescopeHardware.Altitude = Altitude;
            //TelescopeHardware.Azimuth = Azimuth;

            //TelescopeHardware.CalculateRaDec();
            SharedResources.TrafficEnd("done");
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SyncToCoordinates: ");
            CheckCapability(TelescopeHardware.CanSync, "SyncToCoordinates");
            CheckRange(RightAscension, 0, 24, "SyncToCoordinates", "RightAscension");
            CheckRange(Declination, -90, 90, "SyncToCoordinates", "Declination");
            CheckParked("SyncToCoordinates");
            TelescopeHardware.RestoreTrackingStateIfNecessary(); // Restore the configured Tracking state if the client is prevented from doing this by simulator configuration
            CheckTracking(true, "SyncToCoordinates");

            SharedResources.TrafficStart(string.Format(CultureInfo.CurrentCulture, " RA {0} DEC {1}", m_Util.HoursToHMS(RightAscension), m_Util.DegreesToDMS(Declination)));

            TelescopeHardware.TargetDeclination = Declination;
            TelescopeHardware.TargetRightAscension = RightAscension;

            TelescopeHardware.ChangePark(false);

            TelescopeHardware.SyncToTarget();

            SharedResources.TrafficEnd("done");
        }

        public void SyncToTarget()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SyncToTarget: ");
            CheckCapability(TelescopeHardware.CanSync, "SyncToTarget");
            CheckRange(TelescopeHardware.TargetRightAscension, 0, 24, "SyncToTarget", "TargetRightAscension");
            CheckRange(TelescopeHardware.TargetDeclination, -90, 90, "SyncToTarget", "TargetDeclination");

            SharedResources.TrafficStart(" RA " + m_Util.HoursToHMS(TelescopeHardware.TargetRightAscension) + " DEC " + m_Util.DegreesToDMS(TelescopeHardware.TargetDeclination));

            CheckParked("SyncToTarget");
            TelescopeHardware.RestoreTrackingStateIfNecessary(); // Restore the configured Tracking state if the client is prevented from doing this by simulator configuration
            CheckTracking(true, "SyncToTarget");

            TelescopeHardware.ChangePark(false);

            TelescopeHardware.SyncToTarget();

            SharedResources.TrafficEnd("done");
        }

        public double TargetDeclination
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "TargetDeclination: ");
                CheckCapability(TelescopeHardware.CanSlew, "TargetDeclination", false);
                CheckRange(TelescopeHardware.TargetDeclination, -90, 90, "TargetDeclination");
                SharedResources.TrafficEnd(m_Util.DegreesToDMS(TelescopeHardware.TargetDeclination));
                return TelescopeHardware.TargetDeclination;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "TargetDeclination:-> ");
                CheckCapability(TelescopeHardware.CanSlew, "TargetDeclination", true);
                CheckRange(value, -90, 90, "TargetDeclination");
                SharedResources.TrafficEnd(m_Util.DegreesToDMS(value));
                TelescopeHardware.TargetDeclination = value;
            }
        }

        public double TargetRightAscension
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "TargetRightAscension: ");
                CheckCapability(TelescopeHardware.CanSlew, "TargetRightAscension", false);
                CheckRange(TelescopeHardware.TargetRightAscension, 0, 24, "TargetRightAscension");
                SharedResources.TrafficEnd(m_Util.HoursToHMS(TelescopeHardware.TargetRightAscension));
                return TelescopeHardware.TargetRightAscension;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "TargetRightAscension:-> ");
                CheckCapability(TelescopeHardware.CanSlew, "TargetRightAscension", true);
                CheckRange(value, 0, 24, "TargetRightAscension");

                SharedResources.TrafficEnd(m_Util.HoursToHMS(value));
                TelescopeHardware.TargetRightAscension = value;
            }
        }

        public bool Tracking
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Polls, "Tracking: " + TelescopeHardware.Tracking.ToString());
                return TelescopeHardware.Tracking;
            }
            set
            {
                CheckCapability(TelescopeHardware.CanSetTracking, "Tracking", true);
                SharedResources.TrafficLine(SharedResources.MessageType.Polls, "Tracking:-> " + value.ToString());
                TelescopeHardware.Tracking = value;
            }
        }

        public DriveRates TrackingRate
        {
            get
            {
                DriveRates rate = TelescopeHardware.TrackingRate;
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "TrackingRate: ");
                CheckVersionOne("TrackingRate", false);
                SharedResources.TrafficEnd(rate.ToString());
                return rate;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "TrackingRate: -> ");
                CheckVersionOne("TrackingRate", true);
                if ((value < DriveRates.driveSidereal) || (value > DriveRates.driveKing)) throw new InvalidValueException("TrackingRate", value.ToString(), "0 (driveSidereal) to 3 (driveKing)");
                TelescopeHardware.TrackingRate = value;
                SharedResources.TrafficEnd(value.ToString() + "(done)");
            }
        }

        public ITrackingRates TrackingRates
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "TrackingRates: (done)");
                if (TelescopeHardware.CanTrackingRates)
                {
                    return m_TrackingRates;
                }
                else
                {
                    return m_TrackingRatesSimple;
                }
            }
        }

        public DateTime UTCDate
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Time, "UTCDate: " + DateTime.UtcNow.AddSeconds((double)TelescopeHardware.DateDelta).ToString());
                return DateTime.UtcNow.AddSeconds((double)TelescopeHardware.DateDelta);
            }
            set
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Time, "UTCDate-> " + value.ToString());
                TelescopeHardware.DateDelta = (int)value.Subtract(DateTime.UtcNow).TotalSeconds;
            }
        }

        public void Unpark()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "UnPark: ");
            CheckCapability(TelescopeHardware.CanUnpark, "UnPark");

            // Flag that the operation has started
            TelescopeHardware.StartOperation(Operation.Unpark);

            // Interface v3 and earlier behaviour
            if (TelescopeHardware.InterfaceVersion < 4)// Interface is V3 or earlier
            {
                TelescopeHardware.Tracking = TelescopeHardware.AutoTrack;
                TelescopeHardware.ChangePark(false);
                TelescopeHardware.EndOperation("Unpark V3");
            }
            else // Interface is v4 or later
            {
                // If parked, run a task to wait before completing the Unpark operation
                if (AtPark)
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            TelescopeHardware.TL.LogMessage("UnparkTask", $"Waiting for {UNPARK_COMPLETION_DELAY}ms delay...");
                            WaitFor(UNPARK_COMPLETION_DELAY); // Wait

                            TelescopeHardware.Tracking = TelescopeHardware.AutoTrack;
                            TelescopeHardware.ChangePark(false);
                            TelescopeHardware.EndOperation("Unpark V4 - Parked");
                            TelescopeHardware.TL.LogMessage("UnparkTask", $"Unpark completed.");
                        }
                        catch (Exception ex)
                        {
                            TelescopeHardware.TL.LogMessage("UnparkTask", $"Exception {ex.Message}\r\n{ex}.");
                            TelescopeHardware.EndOperation("Unpark V4 - Exception", ex);
                        }
                    });
                }
                else
                {
                    TelescopeHardware.Tracking = TelescopeHardware.AutoTrack;
                    TelescopeHardware.ChangePark(false);
                    TelescopeHardware.EndOperation("Unpark V4 - Not Parked");
                }
            }

            SharedResources.TrafficEnd("(done)");
        }

        #endregion

        #region New pier side properties 

        //public double AvailableTimeInThisPointingState
        //{
        //    get
        //    {
        //        if (AlignmentMode != AlignmentModes.algGermanPolar)
        //        {
        //            return 86400;
        //        }
        //        return TelescopeHardware.AvailableTimeInThisPointingState;
        //    }
        //}

        //public double TimeUntilPointingStateCanChange
        //{
        //    get
        //    {
        //        if (AlignmentMode != AlignmentModes.algGermanPolar)
        //        {
        //            return 0;
        //        }
        //        return TelescopeHardware.TimeUntilPointingStateCanChange;
        //    }
        //}

        #endregion

        #region Private methods

        /// <summary>
        /// Wait for the given number of milliseconds
        /// </summary>
        /// <param name="waitTime"></param>
        private void WaitFor(int waitTime)
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < waitTime)
            {
                Thread.Sleep(50);
                Application.DoEvents();
            }
        }

        private void CheckRate(TelescopeAxes axis, double rate)
        {
            IAxisRates rates = AxisRates(axis);
            string ratesStr = string.Empty;
            foreach (Rate item in rates)
            {
                if (Math.Abs(rate) >= item.Minimum && Math.Abs(rate) <= item.Maximum)
                {
                    return;
                }
                ratesStr = string.Format("{0}, {1} to {2}", ratesStr, item.Minimum, item.Maximum);
            }
            throw new InvalidValueException("MoveAxis", rate.ToString(CultureInfo.InvariantCulture), ratesStr);
        }

        private static void CheckRange(double value, double min, double max, string propertyOrMethod, string valueName)
        {
            if (double.IsNaN(value))
            {
                SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0}:{1} value has not been set", propertyOrMethod, valueName));
                throw new ValueNotSetException(propertyOrMethod + ":" + valueName);
            }
            if (value < min || value > max)
            {
                SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0}:{4} {1} out of range {2} to {3}", propertyOrMethod, value, min, max, valueName));
                throw new InvalidValueException(propertyOrMethod, value.ToString(CultureInfo.CurrentCulture), string.Format(CultureInfo.CurrentCulture, "{0}, {1} to {2}", valueName, min, max));
            }
        }

        private static void CheckRange(double value, double min, double max, string propertyOrMethod)
        {
            if (double.IsNaN(value))
            {
                SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0} value has not been set", propertyOrMethod));
                throw new ValueNotSetException(propertyOrMethod);
            }
            if (value < min || value > max)
            {
                SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0} {1} out of range {2} to {3}", propertyOrMethod, value, min, max));
                throw new InvalidValueException(propertyOrMethod, value.ToString(CultureInfo.CurrentCulture), string.Format(CultureInfo.CurrentCulture, "{0} to {1}", min, max));
            }
        }

        private static void CheckVersionOne(string property, bool accessorSet)
        {
            if (TelescopeHardware.VersionOneOnly)
            {
                SharedResources.TrafficEnd(property + " invalid in version 1");
                throw new PropertyNotImplementedException(property, accessorSet);
            }
        }

        private static void CheckVersionOne(string property)
        {
            if (TelescopeHardware.VersionOneOnly)
            {
                SharedResources.TrafficEnd(property + " is not implemented in version 1");
                throw new NotImplementedException(property);
            }
        }

        private static void CheckCapability(bool capability, string method)
        {
            if (!capability)
            {
                SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0} not implemented in {1}", capability, method));
                throw new MethodNotImplementedException(method);
            }
        }

        private static void CheckCapability(bool capability, string property, bool isSetter)
        {
            if (!capability)
            {
                SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{2} {0} not implemented in {1}", capability, property, isSetter ? "set" : "get"));
                SharedResources.TrafficEnd($"{(isSetter ? "Set" : "Get")} {property} is not implemented.");
                throw new PropertyNotImplementedException(property, isSetter);
            }
        }

        private static void CheckParked(string property)
        {
            if (TelescopeHardware.AtPark)
            {
                SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0} not possible when parked", property));
                throw new ParkedException(property);
            }
        }

        /// <summary>
        /// Checks the slew type and tracking state and raises an exception if they don't match.
        /// </summary>
        /// <param name="raDecSlew">if set to <c>true</c> this is a Ra Dec slew if  <c>false</c> an Alt Az slew.</param>
        /// <param name="method">The method name.</param>
        private static void CheckTracking(bool raDecSlew, string method)
        {
            if (raDecSlew != TelescopeHardware.Tracking)
            {
                SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0} not possible when tracking is {1}", method, TelescopeHardware.Tracking));
                throw new ASCOM.InvalidOperationException(string.Format("{0} is not allowed when tracking is {1}", method, TelescopeHardware.Tracking));
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Connected = false;
            m_AxisRates[0].Dispose();
            m_AxisRates[1].Dispose();
            m_AxisRates[2].Dispose();
            m_AxisRates = null;
            m_TrackingRates.Dispose();
            m_TrackingRates = null;
            m_TrackingRatesSimple.Dispose();
            m_TrackingRatesSimple = null;
            m_Util.Dispose();
            m_Util = null;
        }

        #endregion

        public ComTest GetComTest()
        {
            ComTest retVal=new ComTest();
            retVal.Add(new StateValue("GetComTest","Additional value"));
            return retVal;
        }

    }

    #region Data classes

    //
    // The Rate class implements IRate, and is used to hold values
    // for AxisRates. You do not need to change this class.
    //
    // The Guid attribute sets the CLSID for ASCOM.Telescope.Rate
    // The ClassInterface/None attribute prevents an empty interface called
    // _Rate from being created and used as the [default] interface
    //
    [Guid("d0acdb0f-9c7e-4c53-abb7-576e9f2b8225")]
    [ClassInterface(ClassInterfaceType.None), ComVisible(true)]
    public class Rate : IRate, IDisposable
    {
        private double m_dMaximum = 0;
        private double m_dMinimum = 0;

        //
        // Default constructor - Internal prevents public creation
        // of instances. These are values for AxisRates.
        //
        internal Rate(double Minimum, double Maximum)
        {
            m_dMaximum = Maximum;
            m_dMinimum = Minimum;
        }

        #region IRate Members

        public IEnumerator GetEnumerator()
        {
            return null;
        }

        public double Maximum
        {
            get { return m_dMaximum; }
            set { m_dMaximum = value; }
        }

        public double Minimum
        {
            get { return m_dMinimum; }
            set { m_dMinimum = value; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            // nothing to do?
        }

        #endregion
    }

    //
    // AxisRates is a strongly-typed collection that must be enumerable by
    // both COM and .NET. The IAxisRates and IEnumerable interfaces provide
    // this polymorphism. 
    //
    // The Guid attribute sets the CLSID for ASCOM.Telescope.AxisRates
    // The ClassInterface/None attribute prevents an empty interface called
    // _AxisRates from being created and used as the [default] interface
    //
    [Guid("af5510b9-3108-4237-83da-ae70524aab7d"), ClassInterface(ClassInterfaceType.None), ComVisible(true)]
    public class AxisRates : IAxisRates, IEnumerable, IEnumerator, IDisposable
    {
        private TelescopeAxes m_axis;
        private Rate[] m_Rates;
        private int pos;

        //
        // Constructor - Internal prevents public creation
        // of instances. Returned by Telescope.AxisRates.
        //
        internal AxisRates(TelescopeAxes Axis)
        {
            m_axis = Axis;
            //
            // This collection must hold zero or more Rate objects describing the 
            // rates of motion ranges for the Telescope.MoveAxis() method
            // that are supported by your driver. It is OK to leave this 
            // array empty, indicating that MoveAxis() is not supported.
            //
            // Note that we are constructing a rate array for the axis passed
            // to the constructor. Thus we switch() below, and each case should 
            // initialize the array for the rate for the selected axis.
            //
            double maxRate = TelescopeHardware.MaximumSlewRate;
            switch (m_axis)
            {
                case TelescopeAxes.axisPrimary:
                    // TODO Initialize this array with any Primary axis rates that your driver may provide
                    // Example: m_Rates = new Rate[] { new Rate(10.5, 30.2), new Rate(54.0, 43.6) }
                    m_Rates = new Rate[] { new Rate(0.0, maxRate / 3), new Rate(maxRate / 2, maxRate) };
                    break;
                case TelescopeAxes.axisSecondary:
                    // TODO Initialize this array with any Secondary axis rates that your driver may provide
                    m_Rates = new Rate[] { new Rate(0.0, maxRate / 3), new Rate(maxRate / 2, maxRate) };
                    break;
                case TelescopeAxes.axisTertiary:
                    // TODO Initialize this array with any Tertiary axis rates that your driver may provide
                    m_Rates = new Rate[] { new Rate(0.0, maxRate / 3), new Rate(maxRate / 2, maxRate) };
                    break;
            }
            pos = -1;
        }

        #region IAxisRates Members

        public int Count
        {
            get { return m_Rates.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            pos = -1; //Reset pointer as this is assumed by .NET enumeration
            return this as IEnumerator;
        }

        public IRate this[int index]
        {
            get
            {
                if (index < 1 || index > this.Count)
                    throw new InvalidValueException("AxisRates.index", index.ToString(CultureInfo.CurrentCulture), string.Format(CultureInfo.CurrentCulture, "1 to {0}", this.Count));
                return (IRate)m_Rates[index - 1]; 	// 1-based
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                m_Rates = null;
            }
        }

        #endregion

        #region IEnumerator implementation

        public bool MoveNext()
        {
            if (++pos >= m_Rates.Length) return false;
            return true;
        }

        public void Reset()
        {
            pos = -1;
        }

        public object Current
        {
            get
            {
                if (pos < 0 || pos >= m_Rates.Length) throw new System.InvalidOperationException();
                return m_Rates[pos];
            }
        }

        #endregion
    }
    //
    // TrackingRates is a strongly-typed collection that must be enumerable by
    // both COM and .NET. The ITrackingRates and IEnumerable interfaces provide
    // this polymorphism. 
    //
    // The Guid attribute sets the CLSID for ASCOM.Telescope.TrackingRates
    // The ClassInterface/None attribute prevents an empty interface called
    // _TrackingRates from being created and used as the [default] interface
    //
    [Guid("4bf5c72a-8491-49af-8668-626eac765e91")]
    [ClassInterface(ClassInterfaceType.None)]
    public class TrackingRates : ITrackingRates, IEnumerable, IEnumerator, IDisposable
    {
        private DriveRates[] m_TrackingRates;
        private static int _pos = -1;

        //
        // Default constructor - Internal prevents public creation
        // of instances. Returned by Telescope.AxisRates.
        //
        internal TrackingRates()
        {
            //
            // This array must hold ONE or more DriveRates values, indicating
            // the tracking rates supported by your telescope. The one value
            // (tracking rate) that MUST be supported is driveSidereal!
            //
            m_TrackingRates = new DriveRates[] { DriveRates.driveSidereal, DriveRates.driveKing, DriveRates.driveLunar, DriveRates.driveSolar };
        }

        #region ITrackingRates Members

        public int Count
        {
            get { return m_TrackingRates.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            _pos = -1; //Reset pointer as this is assumed by .NET enumeration
            return this as IEnumerator;
        }


        public DriveRates this[int index]
        {
            get
            {
                if (index < 1 || index > this.Count)
                    throw new InvalidValueException("TrackingRates.this", index.ToString(CultureInfo.CurrentCulture), string.Format(CultureInfo.CurrentCulture, "1 to {0}", this.Count));
                return m_TrackingRates[index - 1];
            }	// 1-based
        }
        #endregion

        #region IEnumerator implementation

        public bool MoveNext()
        {
            if (++_pos >= m_TrackingRates.Length) return false;
            return true;
        }

        public void Reset()
        {
            _pos = -1;
        }

        public object Current
        {
            get
            {
                if (_pos < 0 || _pos >= m_TrackingRates.Length) throw new System.InvalidOperationException();
                return m_TrackingRates[_pos];
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                /* Following code commented out in Platform 6.4 because m_TrackingRates is a global variable for the whole driver and there could be more than one 
                 * instance of the TrackingRates class (created by the calling application). One instance should not invalidate the variable that could be in use
                 * by other instances of which this one is unaware.

                m_TrackingRates = null;

                */
            }
        }
        #endregion
    }

    [Guid("46753368-42d1-424a-85fa-26eee8f4c178")]
    [ClassInterface(ClassInterfaceType.None)]
    public class TrackingRatesSimple : ITrackingRates, IEnumerable, IEnumerator, IDisposable
    {
        private DriveRates[] m_TrackingRates;
        private static int _pos = -1;

        //
        // Default constructor - Internal prevents public creation
        // of instances. Returned by Telescope.AxisRates.
        //
        internal TrackingRatesSimple()
        {
            //
            // This array must hold ONE or more DriveRates values, indicating
            // the tracking rates supported by your telescope. The one value
            // (tracking rate) that MUST be supported is driveSidereal!
            //
            m_TrackingRates = new DriveRates[] { DriveRates.driveSidereal };
        }

        #region ITrackingRates Members

        public int Count
        {
            get { return m_TrackingRates.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            _pos = -1; //Reset pointer as this is assumed by .NET enumeration
            return this as IEnumerator;
        }

        public DriveRates this[int index]
        {
            get
            {
                if (index <= 1 || index > this.Count)
                    throw new InvalidValueException("TrackingRatesSimple.this", index.ToString(CultureInfo.CurrentCulture), string.Format(CultureInfo.CurrentCulture, "1 to {0}", this.Count));
                return m_TrackingRates[index - 1];
            }	// 1-based
        }
        #endregion

        #region IEnumerator implementation

        public bool MoveNext()
        {
            if (++_pos >= m_TrackingRates.Length) return false;
            return true;
        }

        public void Reset()
        {
            _pos = -1;
        }

        public object Current
        {
            get
            {
                if (_pos < 0 || _pos >= m_TrackingRates.Length) throw new System.InvalidOperationException();
                return m_TrackingRates[_pos];
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                /* Following code commented out in Platform 6.4 because m_TrackingRates is a global variable for the whole driver and there could be more than one 
                 * instance of the TrackingRatesSimple class (created by the calling application). One instance should not invalidate the variable that could be in use
                 * by other instances of which this one is unaware.

                if (m_TrackingRates != null)
                {
                    m_TrackingRates = null;
                }
                */
            }
        }
        #endregion
    }

    #endregion
}
