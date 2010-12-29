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
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;

namespace ASCOM.Simulator
{
    //
    // Your driver's ID is ASCOM.Telescope.Telescope
    //
    // The Guid attribute sets the CLSID for ASCOM.Telescope.Telescope
    // The ClassInterface/None addribute prevents an empty interface called
    // _Telescope from being created and used as the [default] interface
    //

    [Guid("86931eac-1f52-4918-b6aa-7e9b0ff361bd"), ClassInterface(ClassInterfaceType.None)]
    public class Telescope : ReferenceCountedObjectBase, ITelescopeV2
    {
        //
        // Driver private data (rate collections)
        //
        private AxisRates[] m_AxisRates;
        private TrackingRates m_TrackingRates;
        private TrackingRatesSimple m_TrackingRatesSimple;
        private ASCOM.Utilities.Util m_Util;

        const string SlewToHA = "SlewToHA"; const string SlewToHAUpper = "SLEWTOHA";
        const string AssemblyVersionNumber = "AssemblyVersionNumber"; const string AssemblyVersionNumberUpper = "ASSEMBLYVERSIONNUMBER";

        //
        // Constructor - Must be public for COM registration!
        //
        public Telescope()
        {
            m_AxisRates = new AxisRates[3];
            m_AxisRates[0] = new AxisRates(TelescopeAxes.axisPrimary);
            m_AxisRates[1] = new AxisRates(TelescopeAxes.axisSecondary);
            m_AxisRates[2] = new AxisRates(TelescopeAxes.axisTertiary);
            m_TrackingRates = new TrackingRates();
            m_TrackingRatesSimple = new TrackingRatesSimple();

            m_Util = new ASCOM.Utilities.Util();
        }

        //
        // PUBLIC COM INTERFACE ITelescope IMPLEMENTATION
        //

        #region ITelescope Members

        public string Action(string ActionName, string ActionParameters)
        {
            //throw new MethodNotImplementedException("Action");
            string Response = "";
            if (ActionName == null)
                throw new InvalidValueException("no ActionName is provided");
            switch (ActionName.ToUpper(CultureInfo.InvariantCulture))
            {
                case AssemblyVersionNumberUpper:
                    Response = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    break;
                case SlewToHAUpper:
                    //Assume that we have just been supplied with an HA
                    //Let errors just go straight back to the caller
                    double HA = double.Parse(ActionParameters, CultureInfo.InvariantCulture);
                    double RA = this.SiderealTime - HA;
                    this.SlewToCoordinates(RA, 0.0);
                    Response = "Slew successful!";
                    break;
                default:
                    throw new ASCOM.InvalidOperationException("Command: '" + ActionName + "' is not recognised by the Scope Simulator .NET driver. " + AssemblyVersionNumberUpper + " " + SlewToHAUpper);
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
                ArrayList sa = new ArrayList();
                sa.Add(AssemblyVersionNumber); // Add a test action to return a value
                sa.Add(SlewToHA); // Expects an numeric HA Parameter

                return sa;
            }
        }

        public void AbortSlew()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "AbortSlew: ");
            if (TelescopeHardware.AtPark)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "At park!");
                throw new ParkedException();
            }
            TelescopeHardware.SlewState = SlewType.SlewNone;

            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "(done)");
        }

        public AlignmentModes AlignmentMode
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Capabilities, "AlignmentMode: ");
                if (!TelescopeHardware.CanAlignmentMode)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Capabilities, "false");
                    throw new MethodNotImplementedException("AlignmentMode");
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Capabilities, TelescopeHardware.AlignmentMode.ToString());

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
                if (!TelescopeHardware.CanAltAz)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Not Implemented");
                    throw new PropertyNotImplementedException("Altitude", false);
                }
                if ((TelescopeHardware.AtPark || TelescopeHardware.SlewState == SlewType.SlewPark) && TelescopeHardware.NoCoordinatesAtPark)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "No coordinates at park!");
                    throw new PropertyNotImplementedException("Altitude", false);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Gets, m_Util.DegreesToDMS(TelescopeHardware.Altitude));
                return TelescopeHardware.Altitude;
            }
        }

        public double ApertureArea
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "ApertureArea: ");
                if (!TelescopeHardware.CanOptics)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "can't");
                    throw new MethodNotImplementedException("ApertureArea");
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, TelescopeHardware.ApertureArea.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.ApertureArea;
            }
        }

        public double ApertureDiameter
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "ApertureDiameter: ");
                if (!TelescopeHardware.CanOptics)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Can't");
                    throw new MethodNotImplementedException("ApertureDiameter");
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, TelescopeHardware.ApertureDiameter.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.ApertureDiameter;
            }
        }

        public bool AtHome
        {
            get
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Polls, "AtHome: Not Implemented");
                    throw new MethodNotImplementedException("AtHome");
                }
                SharedResources.TrafficLine(SharedResources.MessageType.Polls, "AtHome: " + TelescopeHardware.AtHome);
                return TelescopeHardware.AtHome;
            }
        }

        public bool AtPark
        {
            get
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Polls, "AtPark: Not Implemented");
                    throw new MethodNotImplementedException("AtPark");
                }
                SharedResources.TrafficLine(SharedResources.MessageType.Polls, "AtPark: " + TelescopeHardware.AtPark);
                return TelescopeHardware.AtPark;
            }
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary:
                    return m_AxisRates[0];
                case TelescopeAxes.axisSecondary:
                    return m_AxisRates[1];
                case TelescopeAxes.axisTertiary:
                    return m_AxisRates[2];
                default:
                    return null;
            }
        }

        public double Azimuth
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Polls, "Azimuth: ");

                if (!TelescopeHardware.CanAltAz)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Polls, "Not Implemented");
                    throw new PropertyNotImplementedException("Azimuth", false);
                }

                if ((TelescopeHardware.AtPark || TelescopeHardware.SlewState == SlewType.SlewPark) && TelescopeHardware.NoCoordinatesAtPark)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Polls, "No coordinates at park!");
                    throw new PropertyNotImplementedException("Azimuth", false);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Polls, m_Util.DegreesToDMS(TelescopeHardware.Azimuth));
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
            if (TelescopeHardware.VersionOneOnly)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Capabilities, "false");
                throw new MethodNotImplementedException("CanMoveAxis");
            }
            SharedResources.TrafficEnd(SharedResources.MessageType.Capabilities, TelescopeHardware.CanMoveAxis(Axis).ToString());

            return TelescopeHardware.CanMoveAxis(Axis);
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
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSetDeclinationRate: false");
                    throw new MethodNotImplementedException("CanSetDeclinationRate");
                }
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSetDeclinationRate: " + TelescopeHardware.CanSetDeclinationRate);
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
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSetPierSide: false");
                    throw new MethodNotImplementedException("CanSetPierSide");
                }
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSetPierSide: " + TelescopeHardware.CanSetPierSide);
                return TelescopeHardware.CanSetPierSide;
            }
        }

        public bool CanSetRightAscensionRate
        {
            get
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSetRightAscensionRate: false");
                    throw new MethodNotImplementedException("CanSetRightAscensionRate");
                }
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSetRightAscensionRate: " + TelescopeHardware.CanSetRightAscensionRate);
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
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSlewAltAz: false");
                    throw new MethodNotImplementedException("CanSlewAltAz");
                }
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSlewAltAz: " + TelescopeHardware.CanSlewAltAz);
                return TelescopeHardware.CanSlewAltAz;
            }
        }

        public bool CanSlewAltAzAsync
        {
            get
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSlewAltAzAsync: false");
                    throw new MethodNotImplementedException("CanSlewAltAzAsync");
                }
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSlewAltAzAsync: " + TelescopeHardware.CanSlewAltAzAsync);
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
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSyncAltAz: false");
                    throw new MethodNotImplementedException("CanSyncAltAz");
                }
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "CanSyncAltAz: " + TelescopeHardware.CanSyncAltAz);
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
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "Connected = " + TelescopeHardware.Connected.ToString());
                return TelescopeHardware.Connected;
            }
            set
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "Set Connected to " + value.ToString());
                TelescopeHardware.Connected = value;
            }
        }

        public double Declination
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "Declination: ");

                if (!TelescopeHardware.CanEquatorial)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Not Implemented");
                    throw new PropertyNotImplementedException("Declination", false);
                }

                if ((TelescopeHardware.AtPark || TelescopeHardware.SlewState == SlewType.SlewPark) && TelescopeHardware.NoCoordinatesAtPark)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "No coordinates at park!");
                    throw new PropertyNotImplementedException("Declination", false);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Gets, m_Util.DegreesToDMS(TelescopeHardware.Declination));
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
                SharedResources.TrafficLine(SharedResources.MessageType.Gets, "DeclinationRate:-> " + value);
                TelescopeHardware.DeclinationRate = value;
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
            if (TelescopeHardware.VersionOneOnly)
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "DestinationSideOfPier: Not Implemented");
                throw new MethodNotImplementedException("DestinationSideOfPier");
            }
            SharedResources.TrafficStart(SharedResources.MessageType.Other, "DestinationSideOfPier RA " + RightAscension + " DEC " + Declination);

            PierSide ps = TelescopeHardware.SideOfPierRaDec(RightAscension, Declination);
            SharedResources.TrafficEnd(SharedResources.MessageType.Other, ps.ToString());
            return ps;
        }

        public bool DoesRefraction
        {
            get
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "DoesRefraction: false");
                    throw new MethodNotImplementedException("DoesRefraction");
                }
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "DoesRefraction: " + TelescopeHardware.Refraction.ToString());
                return TelescopeHardware.Refraction;
            }
            set
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "DoesRefraction: ->");
                    throw new MethodNotImplementedException("DoesRefraction");
                }
                SharedResources.TrafficLine(SharedResources.MessageType.Capabilities, "DoesRefraction:-> " + value.ToString());
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
                Assembly asm = Assembly.GetExecutingAssembly();

                string driverinfo = asm.GetName().Version.ToString();

                SharedResources.TrafficLine(SharedResources.MessageType.Other, "DriverVersion: " + driverinfo);
                return driverinfo;
            }
        }

        public EquatorialCoordinateType EquatorialSystem
        {
            get
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Other, "EquatorialSystem: Unknown");
                    throw new MethodNotImplementedException("EquatorialSystem");
                }
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "EquatorialSystem: ");
                string output = "";
                EquatorialCoordinateType eq = EquatorialCoordinateType.equOther;

                switch (TelescopeHardware.EquatorialSystem)
                {
                    case 0:
                        eq = EquatorialCoordinateType.equOther;
                        output = "Other";
                        break;

                    case 1:
                        eq = EquatorialCoordinateType.equLocalTopocentric;
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
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, output);
                return eq;
            }
        }

        public void FindHome()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "FindHome: ");
            if (!TelescopeHardware.CanFindHome)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "can't");
                throw new MethodNotImplementedException("FindHome");
            }

            if (TelescopeHardware.AtPark)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "can't, parked");
                throw new ParkedException();
            }

            TelescopeHardware.FindHome();

            while (TelescopeHardware.SlewState == SlewType.SlewHome || TelescopeHardware.SlewState == SlewType.SlewSettle)
            {
                System.Windows.Forms.Application.DoEvents();
            }

            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "(done)");
        }

        public double FocalLength
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "FocalLength: ");
                if (!TelescopeHardware.CanOptics)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "");
                    throw new MethodNotImplementedException("FocalLength");
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, TelescopeHardware.FocalLength.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.FocalLength;
            }
        }

        public double GuideRateDeclination
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "GuideRateDeclination: ");
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Not Implemented in V1");
                    throw new MethodNotImplementedException("GuideRateDeclination");
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Gets, TelescopeHardware.GuideRateDeclination.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.GuideRateDeclination;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "GuideRateDeclination->: ");
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Not Implemented in V1");
                    throw new MethodNotImplementedException("GuideRateDeclination");
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Gets, value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.GuideRateDeclination = value;
            }
        }

        public double GuideRateRightAscension
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "GuideRateRightAscension: ");
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Not Implemented in V1");
                    throw new MethodNotImplementedException("GuideRateRightAscension");
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, TelescopeHardware.GuideRateRightAscension.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.GuideRateRightAscension;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "GuideRateRightAscension->: ");
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Not Implemented in V1");
                    throw new MethodNotImplementedException("GuideRateRightAscension");
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.GuideRateRightAscension = value;
            }
        }

        public short InterfaceVersion
        {
            get
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Other, "InterfaceVersion: V1");
                    throw new MethodNotImplementedException("InterfaceVersion");
                }
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "InterfaceVersion: 2");
                return 2;
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Polls, "IsPulseGuiding: ");
                if (!TelescopeHardware.CanPulseGuide)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Polls, "Can't");
                    // is this correct? should it just return false?
                    throw new MethodNotImplementedException("IsPulseGuiding");
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Polls, TelescopeHardware.IsPulseGuiding.ToString());

                return TelescopeHardware.IsPulseGuiding;
            }
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, string.Format(CultureInfo.CurrentCulture, "MoveAxis {0} {1}:  ", Axis.ToString(), Rate));
            if (TelescopeHardware.VersionOneOnly)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "false");
                throw new MethodNotImplementedException("MoveAxis");
            }
            CheckRate(Rate);

            if (!CanMoveAxis(Axis))
                throw new MethodNotImplementedException("CanMoveAxis " + Enum.GetName(typeof(TelescopeAxes), Axis));

            if (TelescopeHardware.AtPark)
                throw new ParkedException();

            if (TelescopeHardware.SlewState == SlewType.SlewMoveAxis || Rate != 0)
            {
                if (TelescopeHardware.SlewState != SlewType.SlewMoveAxis)
                {
                    TelescopeHardware.SlewState = SlewType.SlewNone;
                    TelescopeHardware.ChangePark(false);
                    TelescopeHardware.m_DeltaAlt = 0;
                    TelescopeHardware.m_DeltaAz = 0;
                    TelescopeHardware.m_DeltaDec = 0;
                    TelescopeHardware.m_DeltaRa = 0;
                }

                switch (Axis)
                {
                    case ASCOM.DeviceInterface.TelescopeAxes.axisPrimary:
                        TelescopeHardware.m_DeltaAz = Rate;
                        break;
                    case ASCOM.DeviceInterface.TelescopeAxes.axisSecondary:
                        TelescopeHardware.m_DeltaAlt = Rate;
                        break;
                    case ASCOM.DeviceInterface.TelescopeAxes.axisTertiary:
                        TelescopeHardware.m_DeltaDec = Rate;
                        break;
                }
                if (TelescopeHardware.m_DeltaAz == 0 && TelescopeHardware.m_DeltaAlt == 0 && TelescopeHardware.m_DeltaDec == 0)
                {
                    if (TelescopeHardware.SlewState == SlewType.SlewMoveAxis)
                    {
                        TelescopeHardware.SlewState = SlewType.SlewNone;
                        SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "(stopped)");
                        return;
                    }
                }
                else
                {
                    TelescopeHardware.SlewState = SlewType.SlewMoveAxis;
                }
            }

            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "(done)");
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
            if (!TelescopeHardware.CanPark)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Can't Park");
                throw new MethodNotImplementedException("Park");
            }

            if (TelescopeHardware.IsParked)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "(Is Parked)");
                return;
            }

            TelescopeHardware.Park();

            while (TelescopeHardware.SlewState == SlewType.SlewPark)
            {
                System.Windows.Forms.Application.DoEvents();
            }

            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "(done)");
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            if (TelescopeHardware.AtPark) throw new ParkedException();

            SharedResources.TrafficStart(SharedResources.MessageType.Slew, string.Format(CultureInfo.CurrentCulture, "Pulse Guide: {0}, {1}", Direction, Duration.ToString(CultureInfo.InvariantCulture)));

            if (!TelescopeHardware.CanPulseGuide)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Can't");
                throw new ASCOM.MethodNotImplementedException("PulseGuide");
            }

            if (Duration < 0)
                throw new ASCOM.InvalidValueException("PulseGuide", Duration.ToString(CultureInfo.InvariantCulture), ">0");
            else if (Duration == 0)
            {
            }
            else
            {

                DateTime endTime = DateTime.Now + TimeSpan.FromMilliseconds(Duration);

                switch (Direction)
                {
                    case GuideDirections.guideNorth:
                        TelescopeHardware.m_GuideRateDeclination = Math.Abs(TelescopeHardware.m_GuideRateDeclination);
                        TelescopeHardware.pulseGuideDecEndTime = endTime;
                        TelescopeHardware.isPulseGuidingDec = true;
                        break;
                    case GuideDirections.guideSouth:
                        TelescopeHardware.m_GuideRateDeclination = -Math.Abs(TelescopeHardware.m_GuideRateDeclination);
                        TelescopeHardware.pulseGuideDecEndTime = endTime;
                        TelescopeHardware.isPulseGuidingDec = true;
                        break;

                    case GuideDirections.guideEast:
                        TelescopeHardware.m_GuideRateRightAscension = Math.Abs(TelescopeHardware.m_GuideRateRightAscension);
                        TelescopeHardware.pulseGuideRaEndTime = endTime;
                        TelescopeHardware.isPulseGuidingRa = true;
                        break;
                    case GuideDirections.guideWest:
                        TelescopeHardware.m_GuideRateRightAscension = -Math.Abs(TelescopeHardware.m_GuideRateRightAscension);
                        TelescopeHardware.pulseGuideRaEndTime = endTime;
                        TelescopeHardware.isPulseGuidingRa = true;
                        break;
                }
            }

            if (!TelescopeHardware.CanDualAxisPulseGuide)
            {
                System.Threading.Thread.Sleep(Duration); // Must be synchronous so wait out the pulseguide duration here
            }

            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, " (done) ");
        }

        public double RightAscension
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "Right Ascension: ");

                if (!TelescopeHardware.CanEquatorial)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Not Implemented");
                    throw new PropertyNotImplementedException("RightAscension", false);
                }

                if ((TelescopeHardware.AtPark || TelescopeHardware.SlewState == SlewType.SlewPark) && TelescopeHardware.NoCoordinatesAtPark)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "No coordinates at park!");
                    throw new PropertyNotImplementedException("RightAscension", false);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Gets, m_Util.HoursToHMS(TelescopeHardware.RightAscension));
                return TelescopeHardware.RightAscension;
            }
        }

        public double RightAscensionRate
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "RightAscensionRate->: (done)");
                return TelescopeHardware.RightAscensionRate;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "RightAscensionRate:<- " + value.ToString(CultureInfo.InvariantCulture));
                if (!TelescopeHardware.CanSetEquatorialRates)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Not Implemented");
                    throw new PropertyNotImplementedException("RightAscensionRate", false);
                }
                TelescopeHardware.RightAscensionRate = value;
            }
        }

        public void SetPark()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Other, "Set Park: ");
            if (!TelescopeHardware.CanSetPark)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Not Implemented");
                throw new PropertyNotImplementedException("SetPark", false);
            }

            TelescopeHardware.ParkAltitude = TelescopeHardware.Altitude;
            TelescopeHardware.ParkAzimuth = TelescopeHardware.Azimuth;

            SharedResources.TrafficEnd(SharedResources.MessageType.Other, "(done)");
        }

        public void SetupDialog()
        {
            if (TelescopeHardware.Connected)
                throw new DriverException("The hardware is connected, cannot do SetupDialog()",
                                    unchecked(ErrorCodes.DriverBase + 4));
            TelescopeSimulator.m_MainForm.DoSetupDialog();
        }

        public PierSide SideOfPier
        {
            get { return TelescopeHardware.m_SideOfPier; }
            set { throw new PropertyNotImplementedException("SideOfPier", true); }
        }

        public double SiderealTime
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Time, "Sidereal Time: ");
                if (!TelescopeHardware.CanSiderealTime)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Time, "Not Implemented");
                    throw new PropertyNotImplementedException("SiderealTime", false);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Time, m_Util.DegreesToHMS(TelescopeHardware.SiderealTime));
                return TelescopeHardware.SiderealTime;
            }
        }

        public double SiteElevation
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteElevation: ");

                if (!TelescopeHardware.CanLatLongElev)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Not Implemented");
                    throw new PropertyNotImplementedException("SiteElevation", false);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, TelescopeHardware.Elevation.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.Elevation;
            }
            set
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "SiteElevation: ->");
                if (!TelescopeHardware.CanLatLongElev)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Not Implemented");
                    throw new PropertyNotImplementedException("SiteElevation", false);
                }
                if (value < -300 || value > 10000)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Out of Range");
                    throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.Elevation = value;
            }
        }

        public double SiteLatitude
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteLatitude: ");

                if (!TelescopeHardware.CanLatLongElev)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Not Implemented");
                    throw new PropertyNotImplementedException("SiteLatitude", false);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, TelescopeHardware.Latitude.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.Latitude;
            }
            set
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "SiteLatitude: ->");
                if (!TelescopeHardware.CanLatLongElev)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Not Implemented");
                    throw new PropertyNotImplementedException("SiteLatitude", false);
                }
                if (value < -90 || value > 90)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Out of Range");
                    throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.Latitude = value;
            }
        }

        public double SiteLongitude
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteLongitude: ");

                if (!TelescopeHardware.CanLatLongElev)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Not Implemented");
                    throw new PropertyNotImplementedException("SiteLongitude", false);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, TelescopeHardware.Longitude.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.Longitude;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteLongitude: ->");
                if (!TelescopeHardware.CanLatLongElev)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Not Implemented");
                    throw new PropertyNotImplementedException("SiteLongitude", false);
                }
                if (value < -180 || value > 180)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Out of Range");
                    throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.Longitude = value;
            }
        }

        public short SlewSettleTime
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "SlewSettleTime: " + (TelescopeHardware.SlewSettleTime * 1000).ToString(CultureInfo.InvariantCulture));
                return (short)(TelescopeHardware.SlewSettleTime * 1000);
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Other, "SlewSettleTime:-> ");
                if (value > 100 || value < 0)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Out Of Range ");
                    throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Other, value + " (done)");
                TelescopeHardware.SlewSettleTime = value / 1000;
            }
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SlewToAltAz: ");
            if (!TelescopeHardware.CanSlew)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Can't");
                throw new MethodNotImplementedException("SlewToAltAz");
            }
            if (Azimuth > 360 || Azimuth < 0 || Altitude < -90 || Altitude > 90)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Out of Range");
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            SharedResources.TrafficStart(SharedResources.MessageType.Slew, " Alt " + m_Util.DegreesToDMS(Altitude) + " Az " + m_Util.DegreesToDMS(Azimuth));

            TelescopeHardware.StartSlewAltAz(Altitude, Azimuth, true, SlewType.SlewAltAz);

            while (TelescopeHardware.SlewState == SlewType.SlewAltAz || TelescopeHardware.SlewState == SlewType.SlewSettle)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SlewToAltAzAsync: ");
            if (!TelescopeHardware.CanSlew)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Can't");
                throw new MethodNotImplementedException("SlewToAltAzAsync");
            }
            if (Azimuth > 360 || Azimuth < 0 || Altitude < -90 || Altitude > 90)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Out of Range");
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            SharedResources.TrafficStart(SharedResources.MessageType.Slew, " Alt " + m_Util.DegreesToDMS(Altitude) + " Az " + m_Util.DegreesToDMS(Azimuth));

            TelescopeHardware.StartSlewAltAz(Altitude, Azimuth, true, SlewType.SlewAltAz);
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SlewToCoordinates: ");
            if (!TelescopeHardware.CanSlew)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Can't");
                throw new MethodNotImplementedException("SlewToCoordinates");
            }
            if (RightAscension > 24 || RightAscension < 0 || Declination < -180 || Declination > 180)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Out of Range");
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            SharedResources.TrafficStart(SharedResources.MessageType.Slew, " RA " + m_Util.HoursToHMS(RightAscension) + " DEC " + m_Util.DegreesToDMS(Declination));

            if (TelescopeHardware.AtPark)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Can't, scope is parked");
                throw new ParkedException();
            }

            TelescopeHardware.StartSlewRaDec(RightAscension, Declination, true);

            while (TelescopeHardware.SlewState == SlewType.SlewRaDec || TelescopeHardware.SlewState == SlewType.SlewSettle)
            {
                System.Windows.Forms.Application.DoEvents();
            }
            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "done");
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SlewToCoordinatesAsync: ");
            if (!TelescopeHardware.CanSlew)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Can't");
                throw new MethodNotImplementedException("SlewToCoordinatesAsync");
            }
            if (RightAscension > 24 || RightAscension < 0 || Declination < -180 || Declination > 180)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Out of Range");
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            SharedResources.TrafficStart(SharedResources.MessageType.Slew, " RA " + m_Util.HoursToHMS(RightAscension) + " DEC " + m_Util.DegreesToDMS(Declination));

            if (TelescopeHardware.AtPark)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Can't, scope is parked");
                throw new ParkedException();
            }

            TelescopeHardware.StartSlewRaDec(RightAscension, Declination, true);
            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "started");
        }

        public void SlewToTarget()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SlewToTarget: ");
            if (!TelescopeHardware.CanSlew)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Can't");
                throw new MethodNotImplementedException("SlewToTarget");
            }
            if (TelescopeHardware.TargetRightAscension == SharedResources.INVALID_COORDINATE || TelescopeHardware.Declination == SharedResources.INVALID_COORDINATE)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "no target");
                throw new DriverException(SharedResources.MSG_NO_TARGET_COORDS, (int)SharedResources.SCODE_NO_TARGET_COORDS);
            }
            if (TelescopeHardware.TargetRightAscension > 24 || TelescopeHardware.TargetRightAscension < 0 || TelescopeHardware.TargetDeclination < -90 || TelescopeHardware.TargetDeclination > 90)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Out of Range");
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            if (TelescopeHardware.AtPark)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "can't, scope is parked");
                throw new ParkedException();
            }

            TelescopeHardware.StartSlewRaDec(TelescopeHardware.TargetRightAscension, TelescopeHardware.TargetDeclination, true);

            while (TelescopeHardware.SlewState == SlewType.SlewRaDec || TelescopeHardware.SlewState == SlewType.SlewSettle)
            {
                System.Windows.Forms.Application.DoEvents();
            }
            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "done");
        }

        public void SlewToTargetAsync()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SlewToTargetAsync: ");
            if (!TelescopeHardware.CanSlew)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "can't");
                throw new MethodNotImplementedException("SlewToTargetAsync");
            }
            if (TelescopeHardware.TargetRightAscension == SharedResources.INVALID_COORDINATE || TelescopeHardware.Declination == SharedResources.INVALID_COORDINATE)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "invalid coordinates");
                throw new DriverException(SharedResources.MSG_NO_TARGET_COORDS, (int)SharedResources.SCODE_NO_TARGET_COORDS);
            }
            if (TelescopeHardware.RightAscension > 24 || TelescopeHardware.RightAscension < 0 || TelescopeHardware.Declination < -90 || TelescopeHardware.Declination > 90)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "out of range");
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            if (TelescopeHardware.AtPark)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "can't when parked");
                throw new ParkedException();
            }
            TelescopeHardware.StartSlewRaDec(TelescopeHardware.TargetRightAscension, TelescopeHardware.TargetDeclination, true);
        }

        public bool Slewing
        {
            get
            {
                SharedResources.TrafficLine(SharedResources.MessageType.Slew, string.Format(CultureInfo.CurrentCulture, "Slewing: {0}", TelescopeHardware.SlewState != SlewType.SlewNone));
                return TelescopeHardware.SlewState != SlewType.SlewNone;
            }
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SyncToAltAz: ");
            if (!TelescopeHardware.CanSync)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "can't");
                throw new MethodNotImplementedException("SyncToAltAz");
            }
            if (Azimuth > 360 || Azimuth < 0 || Altitude < -90 || Altitude > 90)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "out of range");
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            SharedResources.TrafficStart(SharedResources.MessageType.Slew, " Alt " + m_Util.DegreesToDMS(Altitude) + " Az " + m_Util.DegreesToDMS(Azimuth));

            if (TelescopeHardware.AtPark)
                throw new ParkedException();

            TelescopeHardware.ChangePark(false);

            TelescopeHardware.Altitude = Altitude;
            TelescopeHardware.Azimuth = Azimuth;

            TelescopeHardware.CalculateRaDec();
            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "done");
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SyncToCoordinates: ");
            if (!TelescopeHardware.CanSync)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "can't");
                throw new MethodNotImplementedException("SyncToCoordinates");
            }
            if (RightAscension > 24 || RightAscension < 0 || Declination < -90 || Declination > 90)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "out of range");
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            SharedResources.TrafficStart(SharedResources.MessageType.Slew, string.Format(CultureInfo.CurrentCulture, " RA {0} DEC {1}", m_Util.HoursToHMS(RightAscension), m_Util.DegreesToDMS(Declination)));

            if (TelescopeHardware.AtPark)
                throw new ParkedException();

            TelescopeHardware.TargetDeclination = Declination;
            TelescopeHardware.TargetRightAscension = RightAscension;

            TelescopeHardware.ChangePark(false);

            TelescopeHardware.RightAscension = RightAscension;
            TelescopeHardware.Declination = Declination;

            TelescopeHardware.CalculateAltAz();
            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "done");
        }

        public void SyncToTarget()
        {
            SharedResources.TrafficStart(SharedResources.MessageType.Slew, "SyncToTarget: ");
            if (!TelescopeHardware.CanSync)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "can't");
                throw new MethodNotImplementedException("SyncToTarget");
            }

            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, " RA " + m_Util.HoursToHMS(TelescopeHardware.TargetRightAscension) + " DEC " + m_Util.DegreesToDMS(TelescopeHardware.TargetDeclination));

            if (TelescopeHardware.AtPark)
                throw new ParkedException();

            TelescopeHardware.ChangePark(false);

            TelescopeHardware.RightAscension = TelescopeHardware.TargetRightAscension;
            TelescopeHardware.Declination = TelescopeHardware.TargetDeclination;

            TelescopeHardware.CalculateAltAz();
            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "done");
        }

        public double TargetDeclination
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "TargetDeclination: ");
                if (!TelescopeHardware.CanSlew)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Not Implemented");
                    throw new MethodNotImplementedException("TargetDeclination");
                }
                if (TelescopeHardware.TargetDeclination == SharedResources.INVALID_COORDINATE)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Not Set");
                    throw new DriverException(SharedResources.MSG_PROP_NOT_SET, (int)SharedResources.SCOPE_PROP_NOT_SET);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Gets, m_Util.DegreesToDMS(TelescopeHardware.TargetDeclination));
                return TelescopeHardware.TargetDeclination;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "TargetDeclination:-> ");
                if (!TelescopeHardware.CanSlew)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Not Implemented");
                    throw new MethodNotImplementedException("TargetDeclination");
                }
                if (value < -90 || value > 90)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Out of Range");
                    throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Gets, m_Util.DegreesToDMS(value));
                TelescopeHardware.TargetDeclination = value;
            }
        }

        public double TargetRightAscension
        {
            get
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "TargetRightAscension: ");
                if (!TelescopeHardware.CanSlew)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Not Implemented");
                    throw new MethodNotImplementedException("TargetRightAscension");
                }
                if (TelescopeHardware.TargetRightAscension == SharedResources.INVALID_COORDINATE)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Not Set");
                    throw new DriverException(SharedResources.MSG_PROP_NOT_SET, (int)SharedResources.SCOPE_PROP_NOT_SET);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Gets, m_Util.HoursToHMS(TelescopeHardware.TargetRightAscension));
                return TelescopeHardware.TargetRightAscension;
            }
            set
            {
                SharedResources.TrafficStart(SharedResources.MessageType.Gets, "TargetRightAscension:-> ");
                if (!TelescopeHardware.CanSlew)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Not Implemented");
                    throw new MethodNotImplementedException("TargetRightAscension");
                }
                if ((value < 0.0) || (value >= 24.0))
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Gets, "Out of Range");
                    throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE + " " + value, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
                }
                SharedResources.TrafficEnd(SharedResources.MessageType.Gets, m_Util.DegreesToHMS(value));
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
                SharedResources.TrafficLine(SharedResources.MessageType.Polls, "Tracking:-> " + value.ToString());
                TelescopeHardware.Tracking = value;
            }
        }

        public DriveRates TrackingRate
        {
            get
            {
                string output = "";
                DriveRates rate = DriveRates.driveSidereal;
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "TrackingRate: ");
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Not Implemented");
                    throw new MethodNotImplementedException("TrackingRate");
                }
                switch (TelescopeHardware.TrackingRate)
                {
                    case 0:
                        output = "King";
                        rate = DriveRates.driveKing;
                        break;
                    case 1:
                        output = "Lunar";
                        rate = DriveRates.driveLunar;
                        break;
                    case 2:
                        output = "Sidereal";
                        rate = DriveRates.driveSidereal;
                        break;
                    case 3:
                        output = "Solar";
                        rate = DriveRates.driveSolar;
                        break;
                }

                SharedResources.TrafficEnd(SharedResources.MessageType.Other, output);
                return rate;
            }
            set
            {
                string output = "";
                SharedResources.TrafficLine(SharedResources.MessageType.Other, "TrackingRate: -> ");
                if (TelescopeHardware.VersionOneOnly)
                {
                    SharedResources.TrafficEnd(SharedResources.MessageType.Other, "Not Implemented");
                    throw new MethodNotImplementedException("TrackingRate");
                }
                switch (value)
                {
                    case DriveRates.driveKing:
                        output = "King";
                        TelescopeHardware.TrackingRate = 0;
                        break;
                    case DriveRates.driveLunar:
                        output = "Lunar";
                        TelescopeHardware.TrackingRate = 1;
                        break;
                    case DriveRates.driveSidereal:
                        TelescopeHardware.TrackingRate = 2;
                        output = "Sidereal";
                        break;
                    case DriveRates.driveSolar:
                        output = "Solar";
                        TelescopeHardware.TrackingRate = 3;
                        break;
                }

                SharedResources.TrafficEnd(SharedResources.MessageType.Other, output + "(done)");
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
            if (!TelescopeHardware.CanUnpark)
            {
                SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "Not Implemented");
                throw new MethodNotImplementedException("UnPark");
            }

            TelescopeHardware.ChangePark(false);
            TelescopeHardware.Tracking = true;

            SharedResources.TrafficEnd(SharedResources.MessageType.Slew, "(done)");
        }

        #endregion

        #region private methods

        private void CheckRate(double rate)
        {
            // TODO add traffic reports for these exceptions
            if (rate > 50.0)
                throw new InvalidValueException("MoveAxis", rate.ToString(CultureInfo.InvariantCulture), "-50 to -10, 0 and 10 to 50");
            if ((rate < 10.0) && (rate > 0.0))
                throw new InvalidValueException("MoveAxis", rate.ToString(CultureInfo.InvariantCulture), "-50 to -10, 0 and 10 to 50");
            if (rate < -50.0)
                throw new InvalidValueException("MoveAxis", rate.ToString(CultureInfo.InvariantCulture), "-50 to -10, 0 and 10 to 50");
            if ((rate > -10.0) && (rate < 0.0))
                throw new InvalidValueException("MoveAxis", rate.ToString(CultureInfo.InvariantCulture), "-50 to -10, 0 and 10 to 50");
        }


        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    //
    // The Rate class implements IRate, and is used to hold values
    // for AxisRates. You do not need to change this class.
    //
    // The Guid attribute sets the CLSID for ASCOM.Telescope.Rate
    // The ClassInterface/None addribute prevents an empty interface called
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
    // The ClassInterface/None addribute prevents an empty interface called
    // _AxisRates from being created and used as the [default] interface
    //
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), Guid("af5510b9-3108-4237-83da-ae70524aab7d"), ClassInterface(ClassInterfaceType.None), ComVisible(true)]
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
            switch (m_axis)
            {
                case TelescopeAxes.axisPrimary:
                    // TODO Initialize this array with any Primary axis rates that your driver may provide
                    // Example: m_Rates = new Rate[] { new Rate(10.5, 30.2), new Rate(54.0, 43.6) }
                    m_Rates = new Rate[] { new Rate(10.0, 30.2), new Rate(43.6, 50.0) };
                    break;
                case TelescopeAxes.axisSecondary:
                    // TODO Initialize this array with any Secondary axis rates that your driver may provide
                    m_Rates = new Rate[] { new Rate(10.0, 30.2), new Rate(43.6, 50.0) };
                    break;
                case TelescopeAxes.axisTertiary:
                    // TODO Initialize this array with any Tertiary axis rates that your driver may provide
                    m_Rates = new Rate[] { new Rate(10.0, 30.2), new Rate(43.6, 50.0) };
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
                if (index <= 1 || index > this.Count)
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
    // The ClassInterface/None addribute prevents an empty interface called
    // _TrackingRates from being created and used as the [default] interface
    //
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), Guid("4bf5c72a-8491-49af-8668-626eac765e91")]
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
                if (index <= 1 || index > this.Count)
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
                m_TrackingRates = null;
            }
        }
        #endregion
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), Guid("46753368-42d1-424a-85fa-26eee8f4c178")]
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
                if (m_TrackingRates != null)
                {
                    m_TrackingRates = null;
                }
            }
        }
        #endregion
    }
}
