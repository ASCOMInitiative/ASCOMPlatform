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
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection;


using ASCOM;
using ASCOM.HelperNET;
using ASCOM.Interface;


namespace ASCOM.TelescopeSimulator
{
    //
    // Your driver's ID is ASCOM.Telescope.Telescope
    //
    // The Guid attribute sets the CLSID for ASCOM.Telescope.Telescope
    // The ClassInterface/None addribute prevents an empty interface called
    // _Telescope from being created and used as the [default] interface
    //
    
    [Guid("86931eac-1f52-4918-b6aa-7e9b0ff361bd")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Telescope : ReferenceCountedObjectBase, ITelescope
    {
        //
        // Driver private data (rate collections)
        //
        private AxisRates[] m_AxisRates;
        private TrackingRates m_TrackingRates;
        private TrackingRatesSimple m_TrackingRatesSimple;

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
            // TODO Implement your additional construction here
        }


        //
        // PUBLIC COM INTERFACE ITelescope IMPLEMENTATION
        //

        #region ITelescope Members

        public void AbortSlew()
        {
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart("AbortSlew: ");

                }
            }
            if (TelescopeHardware.AtPark)
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Slew)
                    {
                        SharedResources.TrafficForm.TrafficEnd("At park!");

                    }
                }
                throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);
            }
            TelescopeHardware.SlewState = SlewType.SlewNone;

            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficEnd("(done)");

                }
            }
        }
        public AlignmentModes AlignmentMode
        {
            
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficStart("AlignmentMode: ");

                    }
                }
                if (!TelescopeHardware.CanAlignmentMode)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Capabilities)
                        {
                            SharedResources.TrafficForm.TrafficEnd("false");
                        }
                    }
                    throw new MethodNotImplementedException("AlignmentMode");
                }
                switch (TelescopeHardware.AlignmentMode)
                {
                    case 0:
                        if (SharedResources.TrafficForm != null)
                        {
                            if (SharedResources.TrafficForm.Capabilities)
                            {
                                SharedResources.TrafficForm.TrafficEnd("Alt-Azimuth");
                            }
                        }
                        
                        return AlignmentModes.algAltAz;
                    case 1:
                        if (SharedResources.TrafficForm != null)
                        {
                            if (SharedResources.TrafficForm.Capabilities)
                            {
                                SharedResources.TrafficForm.TrafficEnd("German Equatorial");
                            }
                        }
                        
                        return AlignmentModes.algGermanPolar;
                    case 2:
                        if (SharedResources.TrafficForm != null)
                        {
                            if (SharedResources.TrafficForm.Capabilities)
                            {
                                SharedResources.TrafficForm.TrafficEnd("Equatorial");
                            }
                        }
                        return AlignmentModes.algPolar;
                    default:
                        if (SharedResources.TrafficForm != null)
                        {
                            if (SharedResources.TrafficForm.Capabilities)
                            {
                                SharedResources.TrafficForm.TrafficEnd("German Equatorial");
                            }
                        }
                        return AlignmentModes.algGermanPolar;
                }
            }
        }

        public double Altitude
        {
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficStart("Altitude: ");

                    }
                }

                if (!TelescopeHardware.CanAltAz)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new PropertyNotImplementedException("Altitude", false);
                }

                if ((TelescopeHardware.AtPark || TelescopeHardware.SlewState == SlewType.SlewPark) && TelescopeHardware.NoCoordinatesAtPark)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("No coordinates at park!");

                        }
                    }
                    throw new PropertyNotImplementedException("Altitude", false);
                }
                SharedResources.TrafficForm.TrafficEnd(AstronomyFunctions.ConvertDoubleToDMS(TelescopeHardware.Altitude));
                return TelescopeHardware.Altitude;
            }
        }

        public double ApertureArea
        {
            
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficStart("ApertureArea: ");

                    }
                }
                if (!TelescopeHardware.CanOptics)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("");
                        }
                    }
                    throw new MethodNotImplementedException("ApertureArea");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficEnd(TelescopeHardware.ApertureArea.ToString());

                    }
                }
                return TelescopeHardware.ApertureArea;
            }
        }

        public double ApertureDiameter
        {
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficStart("ApertureDiameter: ");

                    }
                }
                if (!TelescopeHardware.CanOptics)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("");
                        }
                    }
                    throw new MethodNotImplementedException("ApertureDiameter");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficEnd(TelescopeHardware.ApertureDiameter.ToString());

                    }
                }
                return TelescopeHardware.ApertureDiameter;
            }
        }

        public bool AtHome
        {
            get
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Capabilities)
                        {
                            SharedResources.TrafficForm.TrafficLine("AtHome: Not Implemented");
                        }
                    }
                    throw new MethodNotImplementedException("AtHome");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("AtHome: " + TelescopeHardware.AtHome);
                    }
                }
                return TelescopeHardware.AtHome;
            }
        }

        public bool AtPark
        {
            get
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Capabilities)
                        {
                            SharedResources.TrafficForm.TrafficLine("AtPark: Not Implemented");
                        }
                    }
                    throw new MethodNotImplementedException("AtPark");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("AtPark: " + TelescopeHardware.AtPark);
                    }
                }
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
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficStart("Azimuth: ");

                    }
                }

                if (!TelescopeHardware.CanAltAz)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new PropertyNotImplementedException("Azimuth", false);
                }

                if ((TelescopeHardware.AtPark || TelescopeHardware.SlewState == SlewType.SlewPark) && TelescopeHardware.NoCoordinatesAtPark)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("No coordinates at park!");

                        }
                    }
                    throw new PropertyNotImplementedException("Azimuth", false);
                }
                SharedResources.TrafficForm.TrafficEnd(AstronomyFunctions.ConvertDoubleToDMS(TelescopeHardware.Azimuth));
                return TelescopeHardware.Azimuth;
            }
        }

        public bool CanFindHome
        {
            
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanFindHome: " + TelescopeHardware.CanFindHome);
                    }
                }
                return TelescopeHardware.CanFindHome; 
            }
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Capabilities)
                {
                    switch (Axis)
                    {
                        case ASCOM.Interface.TelescopeAxes.axisPrimary:
                            SharedResources.TrafficForm.TrafficStart("CanMoveAxis Primary: ");
                            break;
                        case ASCOM.Interface.TelescopeAxes.axisSecondary:
                            SharedResources.TrafficForm.TrafficStart("CanMoveAxis Secondary: ");
                            break;
                        case ASCOM.Interface.TelescopeAxes.axisTertiary:
                            SharedResources.TrafficForm.TrafficStart("CanMoveAxis Tertiary: ");
                            break;
                    }

                }
            }
            if (TelescopeHardware.VersionOneOnly)
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficEnd("false");
                    }
                }
                throw new MethodNotImplementedException("CanMoveAxis");
            }
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Capabilities)
                {
                    SharedResources.TrafficForm.TrafficEnd(TelescopeHardware.CanMoveAxis(int.Parse(Axis.ToString())).ToString());
                }
            }
            
            return TelescopeHardware.CanMoveAxis(int.Parse(Axis.ToString()));
        }

        public bool CanPark
        {
            
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanPark: " + TelescopeHardware.CanPark);
                    }
                }
                return TelescopeHardware.CanPark; 
            }
        }

        public bool CanPulseGuide
        {
            // TODO Replace this with your implementation
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanPulseGuide: " + TelescopeHardware.CanPulseGuide);
                    }
                }
                return TelescopeHardware.CanPulseGuide; 
            }
        }

        public bool CanSetDeclinationRate
        {
            
            get 
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Capabilities)
                        {
                            SharedResources.TrafficForm.TrafficLine("CanSetDeclinationRate: false");
                        }
                    }
                    throw new MethodNotImplementedException("CanSetDeclinationRate");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSetDeclinationRate: " + TelescopeHardware.CanSetDeclinationRate);
                    }
                }
                return TelescopeHardware.CanSetDeclinationRate; 
            }
        }

        public bool CanSetGuideRates
        {
            // TODO Replace this with your implementation
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSetGuideRates: " + TelescopeHardware.CanSetGuideRates);
                    }
                }
                return TelescopeHardware.CanSetGuideRates; 
            }
        }

        public bool CanSetPark
        {
            
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSetPark: " + TelescopeHardware.CanSetPark);
                    }
                }
                return TelescopeHardware.CanSetPark; 
            }
        }

        public bool CanSetPierSide
        {
            
            get 
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Capabilities)
                        {
                            SharedResources.TrafficForm.TrafficLine("CanSetPierSide: false");
                        }
                    }
                    throw new MethodNotImplementedException("CanSetPierSide");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSetPierSide: " + TelescopeHardware.CanSetPierSide);
                    }
                }
                return TelescopeHardware.CanSetPierSide; 
            }
        }

        public bool CanSetRightAscensionRate
        {
            
            get 
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Capabilities)
                        {
                            SharedResources.TrafficForm.TrafficLine("CanSetRightAscensionRate: false");
                        }
                    }
                    throw new MethodNotImplementedException("CanSetRightAscensionRate");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSetRightAscensionRate: " + TelescopeHardware.CanSetRightAscensionRate);
                    }
                }
                return TelescopeHardware.CanSetRightAscensionRate; 
            }
        }

        public bool CanSetTracking
        {
            
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSetTracking: " + TelescopeHardware.CanSetTracking);
                    }
                }
                return TelescopeHardware.CanSetTracking; 
            }
        }

        public bool CanSlew
        {
            
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSlew: " + TelescopeHardware.CanSlew);
                    }
                }
                return TelescopeHardware.CanSlew; 
            }
        }

        public bool CanSlewAltAz
        {
            
            get 
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Capabilities)
                        {
                            SharedResources.TrafficForm.TrafficLine("CanSlewAltAz: false");
                        }
                    }
                    throw new MethodNotImplementedException("CanSlewAltAz");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSlewAltAz: " + TelescopeHardware.CanSlewAltAz);
                    }
                }
                return TelescopeHardware.CanSlewAltAz;
            }
        }

        public bool CanSlewAltAzAsync
        {
            get
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Capabilities)
                        {
                            SharedResources.TrafficForm.TrafficLine("CanSlewAltAzAsync: false");
                        }
                    }
                    throw new MethodNotImplementedException("CanSlewAltAzAsync");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSlewAltAzAsync: " + TelescopeHardware.CanSlewAltAzAsync);
                    }
                }
                return TelescopeHardware.CanSlewAltAzAsync;
            }
        }

        public bool CanSlewAsync
        {
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSlewAsync: " + TelescopeHardware.CanSlewAsync);
                    }
                }
                return TelescopeHardware.CanSlewAsync;
            }
        }

        public bool CanSync
        {
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSync: " + TelescopeHardware.CanSync);
                    }
                }
                return TelescopeHardware.CanSync;
            }
        }

        public bool CanSyncAltAz
        {
            get
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Capabilities)
                        {
                            SharedResources.TrafficForm.TrafficLine("CanSyncAltAz: false");
                        }
                    }
                    throw new MethodNotImplementedException("CanSyncAltAz");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSyncAltAz: " + TelescopeHardware.CanSyncAltAz);
                    }
                }
                return TelescopeHardware.CanSyncAltAz;
            }
        }

        public bool CanUnpark
        {
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanUnPark: " + TelescopeHardware.CanUnpark);
                    }
                }
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
            // TODO Replace this with your implementation
            get { return TelescopeHardware.Connected; }
            set { TelescopeHardware.Connected = value; }
        }

        public double Declination
        {
            
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficStart("Declination: ");

                    }
                }

                if (!TelescopeHardware.CanEquatorial)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new PropertyNotImplementedException("Declination", false);
                }

                if ((TelescopeHardware.AtPark || TelescopeHardware.SlewState == SlewType.SlewPark) && TelescopeHardware.NoCoordinatesAtPark)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("No coordinates at park!");

                        }
                    }
                    throw new PropertyNotImplementedException("Declination", false);
                }
                SharedResources.TrafficForm.TrafficEnd(AstronomyFunctions.ConvertDoubleToHMS(TelescopeHardware.Declination));
                return TelescopeHardware.Declination;
            }
        }

        public double DeclinationRate
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("DeclinationRate", false); }
            set { throw new PropertyNotImplementedException("DeclinationRate", true); }
        }

        public string Description
        {
            
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("Description: " + SharedResources.INSTRUMENT_DESCRIPTION);
                    }
                }
                return SharedResources.INSTRUMENT_DESCRIPTION;
            }
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("DestinationSideOfPier");
        }

        public bool DoesRefraction
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("DoesRefraction", false); }
            set { throw new PropertyNotImplementedException("DoesRefraction", true); }
        }

        public string DriverInfo
        {
            // TODO Replace this with your implementation
            get 
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                
                string driverinfo = asm.FullName;

                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("DriverInfo: " + driverinfo);
                    }
                }
                return driverinfo;
            }
        }

        public string DriverVersion
        {
            // TODO Replace this with your implementation
            get 
            {
                Assembly asm = Assembly.GetExecutingAssembly();

                string driverinfo = asm.GetName().Version.ToString();

                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("DriverVersion: " + driverinfo);
                    }
                }
                return driverinfo;
            }
        }

        public EquatorialCoordinateType EquatorialSystem
        {
            // TODO Replace this with your implementation
            get 
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Capabilities)
                        {
                            SharedResources.TrafficForm.TrafficLine("EquatorialSystem: Unknown");
                        }
                    }
                    throw new MethodNotImplementedException("EquatorialSystem");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficStart("EquatorialSystem: ");
                    }
                }

                string output = "";
                EquatorialCoordinateType eq = EquatorialCoordinateType.equOther;

                switch (TelescopeHardware.EquatorialSystem)
                {
                    case 0:
                        eq= EquatorialCoordinateType.equOther;
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
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficEnd(output);
                    }
                }
                return eq;
            }
        }

        public void FindHome()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("FindHome");
        }

        public double FocalLength
        {

            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficStart("FocalLength: ");

                    }
                }
                if (!TelescopeHardware.CanOptics)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("");
                        }
                    }
                    throw new MethodNotImplementedException("FocalLength");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficEnd(TelescopeHardware.FocalLength.ToString());

                    }
                }
                return TelescopeHardware.FocalLength;
            }
        }

        public double GuideRateDeclination
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("GuideRateDeclination", false); }
            set { throw new PropertyNotImplementedException("GuideRateDeclination", true); }
        }

        public double GuideRateRightAscension
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("GuideRateRightAscension", false); }
            set { throw new PropertyNotImplementedException("GuideRateRightAscension", true); }
        }

        public short InterfaceVersion
        {
            
            get 
            {
                if (TelescopeHardware.VersionOneOnly)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Capabilities)
                        {
                            SharedResources.TrafficForm.TrafficLine("InterfaceVersion:");
                        }
                    }
                    throw new MethodNotImplementedException("InterfaceVersion");
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficStart("EquatorialSystem: 2");
                    }
                }
                return 2;
            }
        }

        public bool IsPulseGuiding
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("IsPulseGuiding", false); }
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("MoveAxis");
        }

        public string Name
        {

            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("Description: " + SharedResources.INSTRUMENT_NAME);
                    }
                }
                return SharedResources.INSTRUMENT_NAME;
            }
        }

        public void Park()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("Park");
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("PulseGuide");
        }

        public double RightAscension
        {
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficStart("Right Ascension: ");

                    }
                }

                if (!TelescopeHardware.CanEquatorial)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new PropertyNotImplementedException("RightAscension", false);
                }

                if ((TelescopeHardware.AtPark || TelescopeHardware.SlewState == SlewType.SlewPark) && TelescopeHardware.NoCoordinatesAtPark)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("No coordinates at park!");

                        }
                    }
                    throw new PropertyNotImplementedException("RightAscension", false);
                }
                SharedResources.TrafficForm.TrafficEnd(AstronomyFunctions.ConvertDoubleToHMS(TelescopeHardware.RightAscension));
                return TelescopeHardware.RightAscension;
            }
        }

        public double RightAscensionRate
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("RightAscensionRate", false); }
            set { throw new PropertyNotImplementedException("RightAscensionRate", true); }
        }

        public void SetPark()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SetPark");
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
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("SideOfPier", false); }
            set { throw new PropertyNotImplementedException("SideOfPier", true); }
        }

        public double SiderealTime
        {
            
            get {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficStart("Sidereal Time: ");

                    }
                }

                if (!TelescopeHardware.CanSiderealTime)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new PropertyNotImplementedException("SiderealTime", false);
                }

                SharedResources.TrafficForm.TrafficEnd(AstronomyFunctions.ConvertDoubleToHMS(TelescopeHardware.SiderealTime));
                return TelescopeHardware.SiderealTime;
            }
        }

        public double SiteElevation
        {
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficStart("SiteElevation: ");

                    }
                }

                if (!TelescopeHardware.CanLatLongElev)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new PropertyNotImplementedException("SiteElevation", false);
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficEnd(TelescopeHardware.Elevation.ToString());

                    }
                }
                return TelescopeHardware.Elevation;
            }
            set
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficLine("SiteElevation: ->");

                    }
                }
                if (!TelescopeHardware.CanLatLongElev)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new PropertyNotImplementedException("SiteElevation", false);
                }
                if (value < -300 || value > 10000)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Out of Range");

                        }
                    }
                    throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficEnd(value.ToString());

                    }
                }
                TelescopeHardware.Elevation = value;
            }
        }

        public double SiteLatitude
        {
            
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficStart("SiteLatitude: ");

                    }
                }

                if (!TelescopeHardware.CanLatLongElev)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new PropertyNotImplementedException("SiteLatitude", false);
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficEnd(TelescopeHardware.Latitude.ToString());

                    }
                }
                return TelescopeHardware.Latitude;
            }
            set 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficLine("SiteLatitude: ->");

                    }
                }
                if (!TelescopeHardware.CanLatLongElev)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new PropertyNotImplementedException("SiteLatitude", false);
                }
                if (value < -90 || value > 90)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Out of Range");

                        }
                    }
                    throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficEnd(value.ToString());

                    }
                }
                TelescopeHardware.Latitude = value;
            }
        }

        public double SiteLongitude
        {
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficStart("SiteLongitude: ");

                    }
                }

                if (!TelescopeHardware.CanLatLongElev)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new PropertyNotImplementedException("SiteLongitude", false);
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficEnd(TelescopeHardware.Longitude.ToString());

                    }
                }
                return TelescopeHardware.Longitude;
            }
            set
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficLine("SiteLongitude: ->");

                    }
                }
                if (!TelescopeHardware.CanLatLongElev)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new PropertyNotImplementedException("SiteLongitude", false);
                }
                if (value < -180 || value > 180)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Out of Range");

                        }
                    }
                    throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficEnd(value.ToString());

                    }
                }
                TelescopeHardware.Longitude = value;
            }
        }

        public short SlewSettleTime
        {
            // TODO Replace this with your implementation
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficLine("SlewSettleTime: " + (TelescopeHardware.SlewSettleTime * 1000).ToString());

                    }
                }
                return (short)(TelescopeHardware.SlewSettleTime * 1000);
            }
            set 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficStart("SlewSettleTime:-> ");

                    }
                }
                if (value > 100 || value < 0)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Out Of Range ");

                        }
                    }
                    throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficEnd(value + " (done)");

                    }
                }
                TelescopeHardware.SlewSettleTime = value / 1000;
            }
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart("SlewToAltAz: ");

                }
            }
            if (!TelescopeHardware.CanSlew)
            {
                throw new MethodNotImplementedException("SlewToAltAz");
            }
            if (Azimuth > 360 || Azimuth < 0 || Altitude < -90 || Altitude > 90)
            {
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart(" Alt " + AstronomyFunctions.ConvertDoubleToDMS(Altitude) + " Az " + AstronomyFunctions.ConvertDoubleToDMS(Azimuth));

                }
            }


            TelescopeHardware.StartSlewAltAz(Altitude, Azimuth, true);


            while (TelescopeHardware.SlewState == SlewType.SlewRaDec || TelescopeHardware.SlewState == SlewType.SlewSettle)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart("SlewToAltAzAsync: ");

                }
            }
            if (!TelescopeHardware.CanSlew)
            {
                throw new MethodNotImplementedException("SlewToAltAzAsync");
            }
            if (Azimuth > 360 || Azimuth < 0 || Altitude < -90 || Altitude > 90)
            {
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart(" Alt " + AstronomyFunctions.ConvertDoubleToDMS(Altitude) + " Az " + AstronomyFunctions.ConvertDoubleToDMS(Azimuth));

                }
            }


            TelescopeHardware.StartSlewAltAz(Altitude, Azimuth, true);
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart("SlewToCoordinates: ");

                }
            }
            if (!TelescopeHardware.CanSlew)
            {
                throw new MethodNotImplementedException("SlewToTarget");
            }
            if (RightAscension > 24 || RightAscension < 0 || Declination < -90 || Declination > 90)
            {
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart(" RA " + AstronomyFunctions.ConvertDoubleToHMS(RightAscension) + " DEC " + AstronomyFunctions.ConvertDoubleToDMS(Declination));

                }
            }


            TelescopeHardware.StartSlewRaDec(RightAscension, Declination, true);

 
            while (TelescopeHardware.SlewState == SlewType.SlewRaDec || TelescopeHardware.SlewState == SlewType.SlewSettle)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart("SlewToCoordinatesAsync: ");

                }
            }
            if (!TelescopeHardware.CanSlew)
            {
                throw new MethodNotImplementedException("SlewToTarget");
            }
            if (RightAscension > 24 || RightAscension < 0 || Declination < -90 || Declination > 90)
            {
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart(" RA " + AstronomyFunctions.ConvertDoubleToHMS(RightAscension) + " DEC " + AstronomyFunctions.ConvertDoubleToDMS(Declination));

                }
            }

            TelescopeHardware.StartSlewRaDec(RightAscension, Declination, true);
        }

        public void SlewToTarget()
        {
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart("SlewToTarget: ");

                }
            }
            if (!TelescopeHardware.CanSlew)
            {
                throw new MethodNotImplementedException("SlewToTarget");
            }
            if (TelescopeHardware.TargetRightAscension == SharedResources.INVALID_COORDINATE || TelescopeHardware.Declination == SharedResources.INVALID_COORDINATE)
            {
                throw new DriverException(SharedResources.MSG_NO_TARGET_COORDS, (int)SharedResources.SCODE_NO_TARGET_COORDS);
            }
            if (TelescopeHardware.RightAscension > 24 || TelescopeHardware.RightAscension < 0 || TelescopeHardware.Declination < -90 || TelescopeHardware.Declination > 90)
            {
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            TelescopeHardware.StartSlewRaDec(TelescopeHardware.TargetRightAscension, TelescopeHardware.Declination, true);

            while (TelescopeHardware.SlewState == SlewType.SlewRaDec || TelescopeHardware.SlewState == SlewType.SlewSettle)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public void SlewToTargetAsync()
        {
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart("SlewToTargetAsync: ");

                }
            }
            if (!TelescopeHardware.CanSlew)
            {
                throw new MethodNotImplementedException("SlewToTarget");
            }
            if (TelescopeHardware.TargetRightAscension == SharedResources.INVALID_COORDINATE || TelescopeHardware.Declination == SharedResources.INVALID_COORDINATE)
            {
                throw new DriverException(SharedResources.MSG_NO_TARGET_COORDS, (int)SharedResources.SCODE_NO_TARGET_COORDS);
            }
            if (TelescopeHardware.RightAscension > 24 || TelescopeHardware.RightAscension < 0 || TelescopeHardware.Declination < -90 || TelescopeHardware.Declination > 90)
            {
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }
            TelescopeHardware.StartSlewRaDec(TelescopeHardware.TargetRightAscension, TelescopeHardware.Declination, true);
        }

        public bool Slewing
        {
            // TODO Replace this with your implementation
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Slew)
                    {
                        SharedResources.TrafficForm.TrafficStart("Slewing: ");

                    }
                }
                if (TelescopeHardware.SlewState == SlewType.SlewNone)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Slew)
                        {
                            SharedResources.TrafficForm.TrafficEnd("False");

                        }
                    }
                    return false; 
                }
                else
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Slew)
                        {
                            SharedResources.TrafficForm.TrafficEnd("False");

                        }
                    }
                    return true; 
                }
            }
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart("SyncToAltAz: ");

                }
            }
            if (!TelescopeHardware.CanSync)
            {
                throw new MethodNotImplementedException("SyncToAltAz");
            }
            if (Azimuth > 360 || Azimuth < 0 || Altitude < -90 || Altitude > 90)
            {
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart(" Alt " + AstronomyFunctions.ConvertDoubleToDMS(Altitude) + " Az " + AstronomyFunctions.ConvertDoubleToDMS(Azimuth));

                }
            }

            
            TelescopeHardware.ChangeHome(false);
            TelescopeHardware.ChangePark(false);

            TelescopeHardware.Altitude = Altitude;
            TelescopeHardware.Azimuth = Azimuth;

            TelescopeHardware.CalculateRaDec();

        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart("SyncToCoordinates: ");

                }
            }
            if (!TelescopeHardware.CanSync)
            {
                throw new MethodNotImplementedException("SyncToCoordinates");
            }
            if (RightAscension > 24 || RightAscension < 0 || Declination < -90 || Declination > 90)
            {
                throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
            }

            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart(" RA " + AstronomyFunctions.ConvertDoubleToHMS(RightAscension) + " DEC " + AstronomyFunctions.ConvertDoubleToDMS(Declination));

                }
            }

            TelescopeHardware.TargetDeclination = Declination;
            TelescopeHardware.TargetRightAscension = RightAscension;

            TelescopeHardware.ChangeHome(false);
            TelescopeHardware.ChangePark(false);

            TelescopeHardware.RightAscension = RightAscension;
            TelescopeHardware.Declination = Declination;

            TelescopeHardware.CalculateAltAz();

        }

        public void SyncToTarget()
        {
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart("SyncToTarget: ");

                }
            }
            if (!TelescopeHardware.CanSync)
            {
                throw new MethodNotImplementedException("SyncToTarget");
            }

            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficEnd(" RA " + AstronomyFunctions.ConvertDoubleToHMS(TelescopeHardware.TargetRightAscension) + " DEC " + AstronomyFunctions.ConvertDoubleToDMS(TelescopeHardware.TargetDeclination));

                }
            }


            TelescopeHardware.ChangeHome(false);
            TelescopeHardware.ChangePark(false);

            TelescopeHardware.RightAscension = TelescopeHardware.TargetRightAscension;
            TelescopeHardware.Declination = TelescopeHardware.TargetDeclination;

            TelescopeHardware.CalculateAltAz();
        }

        public double TargetDeclination
        {
            // TODO Replace this with your implementation
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficStart("TargetDeclination: ");

                    }
                }
                if (!TelescopeHardware.CanSlew)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Gets)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new MethodNotImplementedException("TargetDeclination");
                }
                if (TelescopeHardware.TargetDeclination == SharedResources.INVALID_COORDINATE)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Gets)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Set");

                        }
                    }
                    throw new DriverException(SharedResources.MSG_PROP_NOT_SET, (int)SharedResources.SCOPE_PROP_NOT_SET);
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficEnd(AstronomyFunctions.ConvertDoubleToDMS(TelescopeHardware.TargetDeclination));

                    }
                }
                return TelescopeHardware.TargetDeclination;
            }
            set 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficStart("TargetDeclination:-> ");

                    }
                }
                if (!TelescopeHardware.CanSlew)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Gets)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new MethodNotImplementedException("TargetDeclination");
                }
                if (value < -90 || value > 90)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Gets)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Out of Range");

                        }
                    }
                    throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficEnd(AstronomyFunctions.ConvertDoubleToDMS(value));

                    }
                }
                TelescopeHardware.TargetDeclination = value;
            }
        }

        public double TargetRightAscension
        {
            // TODO Replace this with your implementation
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficStart("TargetRightAscension: ");

                    }
                }
                if (!TelescopeHardware.CanSlew)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Gets)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new MethodNotImplementedException("TargetRightAscension");
                }
                if (TelescopeHardware.TargetDeclination == SharedResources.INVALID_COORDINATE)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Gets)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Set");

                        }
                    }
                    throw new DriverException(SharedResources.MSG_PROP_NOT_SET, (int)SharedResources.SCOPE_PROP_NOT_SET);
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficEnd(AstronomyFunctions.ConvertDoubleToHMS(TelescopeHardware.TargetRightAscension));

                    }
                }
                return TelescopeHardware.TargetRightAscension;
            }
            set
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficStart("TargetRightAscension:-> ");

                    }
                }
                if (!TelescopeHardware.CanSlew)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Gets)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                        }
                    }
                    throw new MethodNotImplementedException("TargetRightAscension");
                }
                if (value < 0 || value >= 24)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Gets)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Out of Range");

                        }
                    }
                    throw new DriverException(SharedResources.MSG_VAL_OUTOFRANGE, (int)SharedResources.SCODE_VAL_OUTOFRANGE);
                }
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficEnd(AstronomyFunctions.ConvertDoubleToHMS(value));

                    }
                }
                TelescopeHardware.TargetRightAscension = value;
            }
        }

        public bool Tracking
        {
            // TODO Replace this with your implementation
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficLine("Tracking: " + TelescopeHardware.Tracking.ToString());

                    }
                }
                return TelescopeHardware.Tracking;
            }
            set
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficLine("Tracking:-> " + value.ToString());

                    }
                }
                TelescopeHardware.Tracking = value;
            }
        }

        public DriveRates TrackingRate
        {
            // TODO Replace this with your implementation
            get 
            {
                string output = "";
                DriveRates rate = DriveRates.driveSidereal;
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficLine("TrackingRate: ");

                    }
                }
                if (TelescopeHardware.VersionOneOnly)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");
                        }
                    }
                    throw new MethodNotImplementedException("TrackingRate");
                }
                switch (TelescopeHardware.TrackingRate)
                {
                    case 0:
                        output = "King";
                        rate= DriveRates.driveKing;
                        break;
                    case 1:
                        output = "Lunar";
                        rate = DriveRates.driveLunar;
                        break;
                    case 2:
                        output = "Sidereal";
                        rate= DriveRates.driveSidereal;
                        break;
                    case 3:
                        output = "Solar";
                        rate= DriveRates.driveSolar;
                        break;
                }

                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficEnd(output);
                    }
                }
                return rate;
            }
            set 
            {
                string output = "";
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficLine("TrackingRate: -> ");

                    }
                }
                if (TelescopeHardware.VersionOneOnly)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Other)
                        {
                            SharedResources.TrafficForm.TrafficEnd("Not Implemented");
                        }
                    }
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
                
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Other)
                    {
                        SharedResources.TrafficForm.TrafficEnd(output +  "(done)");
                    }
                }
            }
        }

        public ITrackingRates TrackingRates
        {
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficLine("TrackingRates: (done)");

                    }
                }
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
            // TODO Replace this with your implementation
            get 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficLine("UTCDate: " + DateTime.UtcNow.AddSeconds((double)TelescopeHardware.DateDelta).ToString());

                    }
                }
                return DateTime.UtcNow.AddSeconds((double)TelescopeHardware.DateDelta);
            }
            set 
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Gets)
                    {
                        SharedResources.TrafficForm.TrafficLine("UTCDate-> " + value.ToString());

                    }
                }
                TelescopeHardware.DateDelta = (int)value.Subtract(DateTime.UtcNow).TotalSeconds;
            }
        }

        public void Unpark()
        {
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficStart("UnPark: ");

                }
            }
            if (!TelescopeHardware.CanUnpark)
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Slew)
                    {
                        SharedResources.TrafficForm.TrafficEnd("Not Implemented");

                    }
                }
                throw new MethodNotImplementedException("UnPark");
            }

            TelescopeHardware.Tracking = true;

            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Slew)
                {
                    SharedResources.TrafficForm.TrafficEnd("(done)");

                }
            }
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
    [ClassInterface(ClassInterfaceType.None)]
    public class Rate : IRate
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
    [Guid("af5510b9-3108-4237-83da-ae70524aab7d")]
    [ClassInterface(ClassInterfaceType.None)]
    public class AxisRates : IAxisRates, IEnumerable
    {
        private TelescopeAxes m_Axis;
        private Rate[] m_Rates;

        //
        // Constructor - Internal prevents public creation
        // of instances. Returned by Telescope.AxisRates.
        //
        internal AxisRates(TelescopeAxes Axis)
        {
            m_Axis = Axis;
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
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary:
                    // TODO Initialize this array with any Primary axis rates that your driver may provide
                    // Example: m_Rates = new Rate[] { new Rate(10.5, 30.2), new Rate(54.0, 43.6) }
                    m_Rates = new Rate[0];
                    break;
                case TelescopeAxes.axisSecondary:
                    // TODO Initialize this array with any Secondary axis rates that your driver may provide
                    m_Rates = new Rate[0];
                    break;
                case TelescopeAxes.axisTertiary:
                    // TODO Initialize this array with any Tertiary axis rates that your driver may provide
                    m_Rates = new Rate[0];
                    break;
            }
        }

        #region IAxisRates Members

        public int Count
        {
            get { return m_Rates.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            return m_Rates.GetEnumerator();
        }

        public IRate this[int Index]
        {
            get { return (IRate)m_Rates[Index - 1]; }	// 1-based
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
    [Guid("4bf5c72a-8491-49af-8668-626eac765e91")]
    [ClassInterface(ClassInterfaceType.None)]
    public class TrackingRates : ITrackingRates, IEnumerable
    {
        private DriveRates[] m_TrackingRates;

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
            m_TrackingRates = new DriveRates[] { DriveRates.driveSidereal,DriveRates.driveKing,DriveRates.driveLunar,DriveRates.driveSolar };
            // TODO Initialize this array with any additional tracking rates that your driver may provide
        }

        #region ITrackingRates Members

        public int Count
        {
            get { return m_TrackingRates.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            return m_TrackingRates.GetEnumerator();
        }

        public DriveRates this[int Index]
        {
            get { return m_TrackingRates[Index - 1]; }	// 1-based
        }

        #endregion
    }
    [Guid("46753368-42d1-424a-85fa-26eee8f4c178")]
    [ClassInterface(ClassInterfaceType.None)]
    public class TrackingRatesSimple : ITrackingRates, IEnumerable
    {
        private DriveRates[] m_TrackingRates;

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
            // TODO Initialize this array with any additional tracking rates that your driver may provide
        }

        #region ITrackingRates Members

        public int Count
        {
            get { return m_TrackingRates.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            return m_TrackingRates.GetEnumerator();
        }

        public DriveRates this[int Index]
        {
            get { return m_TrackingRates[Index - 1]; }	// 1-based
        }

        #endregion
    }
}
