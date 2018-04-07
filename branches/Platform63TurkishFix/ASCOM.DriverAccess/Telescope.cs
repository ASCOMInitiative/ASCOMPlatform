//-----------------------------------------------------------------------
// <summary>Defines the Telescope class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using ASCOM.Conform;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Globalization;

namespace ASCOM.DriverAccess
{

    #region Telescope Wrapper
    /// <summary>
    /// Implements a telescope class to access any registered ASCOM telescope
    /// </summary>
    public class Telescope : AscomDriver, ITelescopeV3
    {
        private MemberFactory memberFactory;
        private bool isPlatform6Telescope = false;
        private bool isPlatform5Telescope = false;

        #region Telescope constructors

        /// <summary>
        /// Creates an instance of the telescope class.
        /// </summary>
        /// <param name="telescopeId">The ProgID for the telescope</param>
        public Telescope(string telescopeId)
            : base(telescopeId)
        {
            memberFactory = base.MemberFactory;

            foreach (Type objInterface in memberFactory.GetInterfaces)
            {
                TL.LogMessage("Telescope", "Found interface name: " + objInterface.Name);
                if (objInterface.Equals(typeof(ITelescopeV3))) isPlatform6Telescope = true; //If the type matches the V2 type flag this
                if (objInterface.Equals(typeof(ASCOM.Interface.ITelescope))) isPlatform5Telescope = true; //If the type matches the PIA type flag this
            }
            TL.LogMessage("Telescope", "Platform 5 Telescope: " + isPlatform5Telescope.ToString() + " Platform 6 Telescope: " + isPlatform6Telescope.ToString());
        }

        #region IDisposable Members
        // No member here, we are relying on Dispose in the base class
        #endregion

        #endregion

        #region Convenience members
        /// <summary>
        /// The Choose() method returns the DriverID of the selected driver.
        /// Choose() allows you to optionally pass the DriverID of a "current" driver (you probably save this in the registry),
        /// and the corresponding telescope type is pre-selected in the Chooser's list.
        /// In this case, the OK button starts out enabled (lit-up); the assumption is that the pre-selected driver has already been configured. 
        /// </summary>
        /// <param name="telescopeId">Optional DriverID of the previously selected telescope that is to be the pre-selected telescope in the list. </param>
        /// <returns>The DriverID of the user selected telescope. Null if the dialog is canceled.</returns>
        public static string Choose(string telescopeId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Telescope";
                return chooser.Choose(telescopeId);
            }
        }

        #endregion

        #region ITelescope Members
        /// <summary>
        /// Stops a slew in progress.
        /// </summary>
        /// <remarks>
        /// Effective only after a call to <see cref="SlewToTargetAsync" />, <see cref="SlewToCoordinatesAsync" />, <see cref="SlewToAltAzAsync" />, or <see cref="MoveAxis" />.
        /// Does nothing if no slew/motion is in progress. 
        /// Tracking is returned to its pre-slew state.
        /// Raises an error if <see cref="AtPark" /> is true. 
        /// </remarks>
        public void AbortSlew()
        {
            TL.LogMessage("AbortSlew", "Calling method");
            memberFactory.CallMember(3, "AbortSlew", new Type[] { }, new object[] { });
            TL.LogMessage("AbortSlew", "Finished");
        }

        /// <summary>
        /// The alignment mode of the mount.
        /// </summary>
        /// <remarks>
        /// This is only available for telescope InterfaceVersions 2 and 3
        /// </remarks>
        public AlignmentModes AlignmentMode
        {
            get { return (AlignmentModes)memberFactory.CallMember(1, "AlignmentMode", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// The Altitude above the local horizon of the telescope's current position (degrees, positive up)
        /// </summary>
        public double Altitude
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Altitude", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// The area of the telescope's aperture, taking into account any obstructions (square meters)
        /// </summary>
        /// <remarks>
        /// This is only available for telescope InterfaceVersions 2 and 3
        /// </remarks>
        public double ApertureArea
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "ApertureArea", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// The telescope's effective aperture diameter (meters)
        /// </summary>
        /// <remarks>
        /// This is only available for telescope InterfaceVersions 2 and 3
        /// </remarks>
        public double ApertureDiameter
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "ApertureDiameter", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// True if the telescope is stopped in the Home position. Set only following a FindHome() operation, and reset with any slew operation. This property must be False if the telescope does not support homing. 
        /// </summary>
        public bool AtHome
        {
            get { return (bool)memberFactory.CallMember(1, "AtHome", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if the telescope has been put into the parked state by the seee <see cref="Park" /> method. Set False by calling the Unpark() method.
        /// </summary>
        /// <remarks>
        /// <para>AtPark is True when the telescope is in the parked state. This is achieved by calling the <see cref="Park" /> method. When AtPark is true, 
        /// the telescope movement is stopped (or restricted to a small safe range of movement) and all calls that would cause telescope 
        /// movement (e.g. slewing, changing Tracking state) must not do so, and must raise an error.</para>
        /// <para>The telescope is taken out of parked state by calling the <see cref="Unpark" /> method. If the telescope cannot be parked, 
        /// then AtPark must always return False.</para>
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public bool AtPark
        {
            get { return (bool)memberFactory.CallMember(1, "AtPark", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// A collection of rates at which the telescope may be moved about the specified axis by the <see cref="MoveAxis" /> method.
        /// </summary>
        /// <param name="Axis">The axis about which rate information is desired (TelescopeAxes value)</param>
        /// <returns>Collection of <see cref="AxisRates" /> rate objects</returns>
        /// <remarks>
        /// See the description of <see cref="MoveAxis" /> for more information. This method must return an empty collection if <see cref="MoveAxis" /> is not supported. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// <para>
        /// Please note that the rate objects must contain absolute non-negative values only. Applications determine the direction by applying a
        /// positive or negative sign to the rates provided. This obviates the need for the driver to to present a duplicate set of negative rates 
        /// as well as the positive rates.</para>
        /// </remarks>
        public IAxisRates AxisRates(ASCOM.DeviceInterface.TelescopeAxes Axis)
        {
            TL.LogMessage("AxisRates", "");
            TL.LogMessage("AxisRates", Axis.ToString());

            if (!memberFactory.IsComObject)
            {
                if (isPlatform6Telescope)
                {
                    TL.LogMessage("AxisRates", "Platform 6 .NET Telescope");
                    object ReturnValue = memberFactory.CallMember(3, "AxisRates", new Type[] { typeof(TelescopeAxes) }, new object[] { Axis });//ITelescope.AxisRates(Axis);

                    try
                    {
                        IAxisRates AxisRatesP6 = (IAxisRates)ReturnValue;
                        TL.LogMessage("AxisRates", "Number of returned AxisRates: " + AxisRatesP6.Count);

                        if (AxisRatesP6.Count > 0) // List the contents without using an iterator so we can test with that is implemented properly in Conform
                        {
                            for (int i = 1; i <= AxisRatesP6.Count; i++)
                            {
                                TL.LogMessage("AxisRates", "Found Minimim: " + AxisRatesP6[i].Minimum + ", Maximum: " + AxisRatesP6[i].Maximum);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("AxisRates", ex.ToString()); // Just report errors here and return the object to the caller
                    }
                    return (IAxisRates)ReturnValue;
                }
                else if (isPlatform5Telescope)
                {
                    TL.LogMessage("AxisRates", "Platform 5 .NET Telescope");
                    object ReturnValue = memberFactory.CallMember(3, "AxisRates", new Type[] { typeof(TelescopeAxes) }, new object[] { Axis });//ITelescope.AxisRates(Axis);
                    IAxisRates AxisRatesP6 = new AxisRates(); //Create a new P6 compliant shell

                    try
                    {
                        ASCOM.Interface.IAxisRates AxisRatesP5 = (ASCOM.Interface.IAxisRates)ReturnValue;
                        AxisRatesP6 = new AxisRates(AxisRatesP5, TL); //Create a new P6 compliant shell that presents the P5 object
                        TL.LogMessage("AxisRates", "Number of returned AxisRates: " + AxisRatesP5.Count);

                        if (AxisRatesP5.Count > 0) // List the contents without using an iterator so we can test with that is implemented properly in Conform
                        {
                            for (int i = 1; i <= AxisRatesP5.Count; i++)
                            {
                                TL.LogMessage("AxisRates", "Found Minimim: " + AxisRatesP5[i].Minimum + ", Maximum: " + AxisRatesP5[i].Maximum);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("AxisRates", ex.ToString()); // Just report errors here and return the object to the caller
                    }

                    return AxisRatesP6;

                }
                else
                {
                    TL.LogMessage("AxisRates", "Neither Platform 5 nor Platform 6 .NET Telescope");
                    return new AxisRates();
                }

            }

            else
            {
                TL.LogMessage("AxisRates", "Platform 5/6 COM Telescope");
                _AxisRates ReturnValue = new _AxisRates(Axis, memberFactory.GetObjType, memberFactory.GetLateBoundObject, TL);

                try
                {
                    if (ReturnValue.Count > 0) // List the contents without using an iterator so we can test with that is implemented properly in Conform
                    {
                        for (int i = 1; i <= ReturnValue.Count; i++)
                        {
                            TL.LogMessage("AxisRates", "Found Minimim: " + ReturnValue[i].Minimum + ", Maximum: " + ReturnValue[i].Maximum);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("AxisRates", ex.ToString()); // Just report errors here and return the object to the caller
                }

                return ReturnValue;
            }
        }

        /// <summary>
        /// The azimuth at the local horizon of the telescope's current position (degrees, North-referenced, positive East/clockwise).
        /// </summary>
        public double Azimuth
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Azimuth", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// True if this telescope is capable of programmed finding its home position (<see cref="FindHome" /> method).
        /// </summary>
        /// <remarks>
        /// May raise an error if the telescope is not connected. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public bool CanFindHome
        {
            get { return (bool)memberFactory.CallMember(1, "CanFindHome", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Shows whether the telescope can be controlled about the specified axis via the MoveAxis() method.
        /// </summary>
        /// <returns>Boolean - True if the telescope can be controlled about the specified axis via the MoveAxis() method. </returns>
        /// <remarks>See the description of MoveAxis() for more information. The (symbolic) values for TelescopeAxes are:
        ///<bl>
        ///<li>axisPrimary 0 Primary axis (e.g., Right Ascension or Azimuth)</li>
        ///<li>axisSecondary 1 Secondary axis (e.g., Declination or Altitude)</li>
        ///<li>axisTertiary 2 Tertiary axis (e.g. imager rotator/de-rotator)</li>
        ///</bl></remarks>
        /// <param name="Axis">Primary, Secondary or Tertiary axis</param>
        /// <returns>Boolean indicating can or can not move the requested axis</returns>
        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            return (bool)memberFactory.CallMember(3, "CanMoveAxis", new Type[] { typeof(TelescopeAxes) }, new object[] { Axis });
        }

        /// <summary>
        /// True if this telescope is capable of programmed parking (<see cref="Park" />method)
        /// </summary>
        /// <remarks>
        /// May raise an error if the telescope is not connected. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public bool CanPark
        {
            get { return (bool)memberFactory.CallMember(1, "CanPark", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if this telescope is capable of software-pulsed guiding (via the <see cref="PulseGuide" /> method)
        /// </summary>
        /// <remarks>
        /// May raise an error if the telescope is not connected. 
        /// </remarks>
        public bool CanPulseGuide
        {
            get { return (bool)memberFactory.CallMember(1, "CanPulseGuide", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if the <see cref="DeclinationRate" /> property can be changed to provide offset tracking in the declination axis.
        /// </summary>
        /// <remarks>
        /// May raise an error if the telescope is not connected. 
        /// </remarks>
        public bool CanSetDeclinationRate
        {
            get { return (bool)memberFactory.CallMember(1, "CanSetDeclinationRate", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if the guide rate properties used for <see cref="PulseGuide" /> can ba adjusted.
        /// </summary>
        /// <remarks>
        /// May raise an error if the telescope is not connected. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public bool CanSetGuideRates
        {
            get { return (bool)memberFactory.CallMember(1, "CanSetGuideRates", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if this telescope is capable of programmed setting of its park position (<see cref="SetPark" /> method)
        /// </summary>
        /// <remarks>
        /// May raise an error if the telescope is not connected. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public bool CanSetPark
        {
            get { return (bool)memberFactory.CallMember(1, "CanSetPark", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if the <see cref="SideOfPier" /> property can be set, meaning that the mount can be forced to flip.
        /// </summary>
        /// <remarks>
        /// This will always return False for non-German-equatorial mounts that do not have to be flipped. 
        /// May raise an error if the telescope is not connected. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public bool CanSetPierSide
        {
            get { return (bool)memberFactory.CallMember(1, "CanSetPierSide", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if the <see cref="RightAscensionRate" /> property can be changed to provide offset tracking in the right ascension axis.
        /// </summary>
        /// <remarks>
        /// May raise an error if the telescope is not connected. 
        /// </remarks>
        public bool CanSetRightAscensionRate
        {
            get { return (bool)memberFactory.CallMember(1, "CanSetRightAscensionRate", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if the <see cref="Tracking" /> property can be changed, turning telescope sidereal tracking on and off.
        /// </summary>
        /// <remarks>
        /// May raise an error if the telescope is not connected. 
        /// </remarks>
        public bool CanSetTracking
        {
            get { return (bool)memberFactory.CallMember(1, "CanSetTracking", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if this telescope is capable of programmed slewing (synchronous or asynchronous) to equatorial coordinates
        /// </summary>
        /// <remarks>
        /// If this is true, then only the synchronous equatorial slewing methods are guaranteed to be supported.
        /// See the <see cref="CanSlewAsync" /> property for the asynchronous slewing capability flag. 
        /// May raise an error if the telescope is not connected. 
        /// </remarks>
        public bool CanSlew
        {
            get { return (bool)memberFactory.CallMember(1, "CanSlew", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if this telescope is capable of programmed slewing (synchronous or asynchronous) to local horizontal coordinates
        /// </summary>
        /// <remarks>
        /// If this is true, then only the synchronous local horizontal slewing methods are guaranteed to be supported.
        /// See the <see cref="CanSlewAltAzAsync" /> property for the asynchronous slewing capability flag. 
        /// May raise an error if the telescope is not connected. 
        /// </remarks>
        public bool CanSlewAltAz
        {
            get { return (bool)memberFactory.CallMember(1, "CanSlewAltAz", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if this telescope is capable of programmed asynchronous slewing to local horizontal coordinates
        /// </summary>
        /// <remarks>
        /// This indicates the the asynchronous local horizontal slewing methods are supported.
        /// If this is True, then <see cref="CanSlewAltAz" /> will also be true. 
        /// May raise an error if the telescope is not connected. 
        /// </remarks>
        public bool CanSlewAltAzAsync
        {
            get { return (bool)memberFactory.CallMember(1, "CanSlewAltAzAsync", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if this telescope is capable of programmed asynchronous slewing to equatorial coordinates.
        /// </summary>
        /// <remarks>
        /// This indicates the the asynchronous equatorial slewing methods are supported.
        /// If this is True, then <see cref="CanSlew" /> will also be true.
        /// May raise an error if the telescope is not connected. 
        /// </remarks>
        public bool CanSlewAsync
        {
            get { return (bool)memberFactory.CallMember(1, "CanSlewAsync", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if this telescope is capable of programmed synching to equatorial coordinates.
        /// </summary>
        /// <remarks>
        /// May raise an error if the telescope is not connected. 
        /// </remarks>
        public bool CanSync
        {
            get { return (bool)memberFactory.CallMember(1, "CanSync", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if this telescope is capable of programmed synching to local horizontal coordinates
        /// </summary>
        /// <remarks>
        /// May raise an error if the telescope is not connected. 
        /// </remarks>
        public bool CanSyncAltAz
        {
            get { return (bool)memberFactory.CallMember(1, "CanSyncAltAz", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if this telescope is capable of programmed unparking (<see cref="Unpark" /> method).
        /// </summary>
        /// <remarks>
        /// If this is true, then <see cref="CanPark" /> will also be true. May raise an error if the telescope is not connected.
        /// May raise an error if the telescope is not connected. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public bool CanUnpark
        {
            get { return (bool)memberFactory.CallMember(1, "CanUnpark", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// The declination (degrees) of the telescope's current equatorial coordinates, in the coordinate system given by the <see cref="EquatorialSystem" /> property.
        /// Reading the property will raise an error if the value is unavailable. 
        /// </summary>
        public double Declination
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Declination", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// The declination tracking rate (arcseconds per second, default = 0.0)
        /// </summary>
        /// <remarks>
        /// This property, together with <see cref="RightAscensionRate" />, provides support for "offset tracking".
        /// Offset tracking is used primarily for tracking objects that move relatively slowly against the equatorial coordinate system.
        /// It also may be used by a software guiding system that controls rates instead of using the <see cref="PulseGuide">PulseGuide</see> method. 
        /// <para>
        /// <b>NOTES:</b>
        /// <list type="bullet">
        /// <list></list>
        /// <item><description>The property value represents an offset from zero motion.</description></item>
        /// <item><description>If <see cref="CanSetDeclinationRate" /> is False, this property will always return 0.</description></item>
        /// <item><description>To discover whether this feature is supported, test the <see cref="CanSetDeclinationRate" /> property.</description></item>
        /// <item><description>The supported range of this property is telescope specific, however, if this feature is supported,
        /// it can be expected that the range is sufficient to allow correction of guiding errors caused by moderate misalignment 
        /// and periodic error.</description></item>
        /// <item><description>If this property is non-zero when an equatorial slew is initiated, the telescope should continue to update the slew 
        /// destination coordinates at the given offset rate.</description></item>
        /// <item><description>This will allow precise slews to a fast-moving target with a slow-slewing telescope.</description></item>
        /// <item><description>When the slew completes, the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> properties should reflect the final (adjusted) destination.</description></item>
        /// </list>
        /// </para>
        /// <para>
        ///This is not a required feature of this specification, however it is desirable. 
        /// </para>
        /// </remarks>
        public double DeclinationRate
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "DeclinationRate", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "DeclinationRate", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Predict side of pier for German equatorial mounts
        /// </summary>
        /// <remarks>
        /// This is only available for telescope InterfaceVersions 2 and 3
        /// </remarks>
        /// <param name="RightAscension">The destination right ascension (hours).</param>
        /// <param name="Declination">The destination declination (degrees, positive North).</param>
        /// <returns>The side of the pier on which the telescope would be on if a slew to the given equatorial coordinates is performed at the current instant of time.</returns>
        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            return (PierSide)memberFactory.CallMember(3, "DestinationSideOfPier", new Type[] { typeof(double), typeof(double) }, new object[] { RightAscension, Declination });
        }

        /// <summary>
        /// True if the telescope or driver applies atmospheric refraction to coordinates.
        /// </summary>
        /// <remarks>
        /// If this property is True, the coordinates sent to, and retrieved from, the telescope are unrefracted. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// <para>
        /// <b>NOTES:</b>
        /// <list type="bullet">
        /// <item><description>If the driver does not know whether the attached telescope does its own refraction, and if the driver does not itself calculate 
        /// refraction, this property (if implemented) must raise an error when read.</description></item>
        /// <item><description>Writing to this property is optional. Often, a telescope (or its driver) calculates refraction using standard atmospheric parameters.</description></item>
        /// <item><description>If the client wishes to calculate a more accurate refraction, then this property could be set to False and these 
        /// client-refracted coordinates used.</description></item>
        /// <item><description>If disabling the telescope or driver's refraction is not supported, the driver must raise an error when an attempt to set 
        /// this property to False is made.</description></item> 
        /// <item><description>Setting this property to True for a telescope or driver that does refraction, or to False for a telescope or driver that 
        /// does not do refraction, shall not raise an error. It shall have no effect.</description></item> 
        /// </list>
        /// </para>
        /// </remarks>
        public bool DoesRefraction
        {
            get { return (bool)memberFactory.CallMember(1, "DoesRefraction", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "DoesRefraction", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Equatorial coordinate system used by this telescope.
        /// </summary>
        /// <remarks>
        /// Most amateur telescopes use local topocentric coordinates.
        /// This coordinate system is simply the apparent position in the sky
        /// (possibly uncorrected for atmospheric refraction) for "here and now",
        /// thus these are the coordinates that one would use with digital setting
        /// circles and most amateur scopes. More sophisticated telescopes use one of
        /// the standard reference systems established by professional astronomers.
        /// The most common is the Julian Epoch 2000 (J2000). 
        /// These instruments apply corrections for precession,
        /// nutation, abberration, etc. to adjust the coordinates from the standard system
        /// to the pointing direction for the time and location of "here and now". 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public EquatorialCoordinateType EquatorialSystem
        {
            get { return (EquatorialCoordinateType)memberFactory.CallMember(1, "EquatorialSystem", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Locates the telescope's "home" position (synchronous)
        /// </summary>
        /// <remarks>
        /// Returns only after the home position has been found.
        /// At this point the <see cref="AtHome" /> property will be True.
        /// Raises an error if there is a problem. 
        /// Raises an error if AtPark is true. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public void FindHome()
        {
            memberFactory.CallMember(3, "FindHome", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// The telescope's focal length, meters
        /// </summary>
        /// <remarks>
        /// This property may be used by clients to calculate telescope field of view and plate scale when combined with detector pixel size and geometry. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public double FocalLength
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "FocalLength", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// The current Declination movement rate offset for telescope guiding (degrees/sec)
        /// </summary>
        /// <remarks>
        /// This is the rate for both hardware/relay guiding and the PulseGuide() method. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// <para>
        /// <b>NOTES:</b>
        /// <list type="bullet">
        /// <item><description>To discover whether this feature is supported, test the <see cref="CanSetGuideRates" /> property.</description></item> 
        /// <item><description>The supported range of this property is telescope specific, however,
        /// if this feature is supported, it can be expected that the range is sufficient to
        /// allow correction of guiding errors caused by moderate misalignment and periodic error.</description></item> 
        /// <item><description>If a telescope does not support separate guiding rates in Right Ascension and Declination,
        /// then it is permissible for <see cref="GuideRateRightAscension" /> and GuideRateDeclination to be tied together.
        /// In this case, changing one of the two properties will cause a change in the other.</description></item> 
        /// <item><description>Mounts must start up with a known or default declination guide rate,
        /// and this property must return that known/default guide rate until changed.</description></item> 
        /// </list>
        /// </para>
        /// </remarks>
        public double GuideRateDeclination
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "GuideRateDeclination", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "GuideRateDeclination", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// The current Right Ascension movement rate offset for telescope guiding (degrees/sec)
        /// </summary>
        /// <remarks>
        /// This is the rate for both hardware/relay guiding and the PulseGuide() method. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// <para>
        /// <b>NOTES:</b>
        /// <list type="bullet">
        /// <item><description>To discover whether this feature is supported, test the <see cref="CanSetGuideRates" /> property.</description></item>  
        /// <item><description>The supported range of this property is telescope specific, however, if this feature is supported, 
        /// it can be expected that the range is sufficient to allow correction of guiding errors caused by moderate
        /// misalignment and periodic error.</description></item>  
        /// <item><description>If a telescope does not support separate guiding rates in Right Ascension and Declination,
        /// then it is permissible for GuideRateRightAscension and <see cref="GuideRateDeclination" /> to be tied together. 
        /// In this case, changing one of the two properties will cause a change in the other.</description></item>  
        ///<item><description> Mounts must start up with a known or default right ascension guide rate,
        /// and this property must return that known/default guide rate until changed.</description></item>  
        /// </list>
        /// </para>
        /// </remarks>
        public double GuideRateRightAscension
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "GuideRateRightAscension", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "GuideRateRightAscension", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// True if a <see cref="PulseGuide" /> command is in progress, False otherwise
        /// </summary>
        /// <remarks>
        /// Raises an error if the value of the <see cref="CanPulseGuide" /> property is false
        /// (the driver does not support the <see cref="PulseGuide" /> method). 
        /// </remarks>
        public bool IsPulseGuiding
        {
            get { return (bool)memberFactory.CallMember(1, "IsPulseGuiding", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Move the telescope in one axis at the given rate.
        /// </summary>
        /// <remarks>
        /// This method supports control of the mount about its mechanical axes.
        /// The telescope will start moving at the specified rate about the specified axis and continue indefinitely.
        /// This method can be called for each axis separately, and have them all operate concurrently at separate rates of motion. 
        /// Set the rate for an axis to zero to restore the motion about that axis to the rate set by the <see cref="Tracking"/> property.
        /// Tracking motion (if enabled, see note below) is suspended during this mode of operation. 
        /// <para>
        /// Raises an error if <see cref="AtPark" /> is true. 
        /// This must be implemented for the if the <see cref="CanMoveAxis" /> property returns True for the given axis.</para>
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// <para>
        /// <b>NOTES:</b>
        /// <list type="bullet">
        /// <item><description>The movement rate must be within the value(s) obtained from a <see cref="IRate" /> object in the 
        /// the <see cref="AxisRates" /> collection. This is a signed value with negative rates moving in the oposite direction to positive rates.</description></item>
        /// <item><description>The values specified in <see cref="AxisRates" /> are absolute, unsigned values and apply to both directions, 
        /// determined by the sign used in this command.</description></item>
        /// <item><description>An out of range exception is raised the rate is out of range.</description></item>
        /// <item><description>The value of <see cref="Slewing" /> must be True if the telescope is moving 
        /// about any of its axes as a result of this method being called. 
        /// This can be used to simulate a handbox by initiating motion with the
        /// MouseDown event and stopping the motion with the MouseUp event.</description></item>
        /// <item><description>When the motion is stopped by setting the rate to zero the scope will be set to the previous 
        /// <see cref="TrackingRate" /> or to no movement, depending on the state of the <see cref="Tracking" /> property.</description></item>
        /// <item><description>It may be possible to implement satellite tracking by using the <see cref="MoveAxis" /> method to move the 
        /// scope in the required manner to track a satellite.</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <param name="Axis">The physical axis about which movement is desired</param>
        /// <param name="Rate">The rate of motion (deg/sec) about the specified axis</param>
        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            memberFactory.CallMember(3, "MoveAxis", new Type[] { typeof(TelescopeAxes), typeof(double) }, new object[] { Axis, Rate });
        }

        /// <summary>
        /// Move the telescope to its park position, stop all motion (or restrict to a small safe range), and set <see cref="AtPark" /> to True.
        /// </summary>
        /// <remarks>
        /// Raises an error if there is a problem communicating with the telescope or if parking fails. 
        /// Parking should put the telescope into a state where its pointing accuracy 
        /// will not be lost if it is power-cycled (without moving it).
        /// Some telescopes must be power-cycled before unparking.
        /// Others may be unparked by simply calling the <see cref="Unpark" /> method.
        /// Calling this with <see cref="AtPark" /> = True does nothing (harmless) 
        /// </remarks>
        public void Park()
        {
            memberFactory.CallMember(3, "Park", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Moves the scope in the given direction for the given interval or time at 
        /// the rate given by the corresponding guide rate property 
        /// </summary>
        /// <remarks>
        /// This method returns immediately if the hardware is capable of back-to-back moves,
        /// i.e. dual-axis moves. For hardware not having the dual-axis capability,
        /// the method returns only after the move has completed. 
        /// <para>
        /// <b>NOTES:</b>
        /// <list type="bullet">
        /// <item><description>Raises an error if <see cref="AtPark" /> is true.</description></item>
        /// <item><description>The <see cref="IsPulseGuiding" /> property must be be True during pulse-guiding.</description></item>
        /// <item><description>The rate of motion for movements about the right ascension axis is 
        /// specified by the <see cref="GuideRateRightAscension" /> property. The rate of motion
        /// for movements about the declination axis is specified by the 
        /// <see cref="GuideRateDeclination" /> property. These two rates may be tied together
        /// into a single rate, depending on the driver's implementation
        /// and the capabilities of the telescope.</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <param name="Direction">The direction in which the guide-rate motion is to be made</param>
        /// <param name="Duration">The duration of the guide-rate motion (milliseconds)</param>
        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            memberFactory.CallMember(3, "PulseGuide", new Type[] { typeof(GuideDirections), typeof(int) }, new object[] { (int)Direction, Duration });
        }

        /// <summary>
        /// The right ascension (hours) of the telescope's current equatorial coordinates,
        /// in the coordinate system given by the EquatorialSystem property
        /// </summary>
        /// <remarks>
        /// Reading the property will raise an error if the value is unavailable. 
        /// </remarks>
        public double RightAscension
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "RightAscension", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// The right ascension tracking rate offset from sidereal (seconds per sidereal second, default = 0.0)
        /// </summary>
        /// <remarks>
        /// This property, together with <see cref="DeclinationRate" />, provides support for "offset tracking".
        /// Offset tracking is used primarily for tracking objects that move relatively slowly
        /// against the equatorial coordinate system. It also may be used by a software guiding
        /// system that controls rates instead of using the <see cref="PulseGuide">PulseGuide</see> method.
        /// <para>
        /// <b>NOTES:</b>
        /// The property value represents an offset from the currently selected <see cref="TrackingRate" />. 
        /// <list type="bullet">
        /// <item><description>If this property is zero, tracking will be at the selected <see cref="TrackingRate" />.</description></item>
        /// <item><description>If <see cref="CanSetRightAscensionRate" /> is False, this property must always return 0.</description></item> 
        /// To discover whether this feature is supported, test the <see cref="CanSetRightAscensionRate" />property. 
        /// <item><description>The property value is in in seconds of right ascension per sidereal second.</description></item> 
        /// <item><description>To convert a given rate in (the more common) units of sidereal seconds
        /// per UTC (clock) second, multiply the value by 0.9972695677 
        /// (the number of UTC seconds in a sidereal second) then set the property.
        /// Please note that these units were chosen for the Telescope V1 standard,
        /// and in retrospect, this was an unfortunate choice.
        /// However, to maintain backwards compatibility, the units cannot be changed.
        /// A simple multiplication is all that's needed, as noted. 
        /// The supported range of this property is telescope specific, however,
        /// if this feature is supported, it can be expected that the range
        /// is sufficient to allow correction of guiding errors
        /// caused by moderate misalignment and periodic error. </description></item>
        /// <item><description>If this property is non-zero when an equatorial slew is initiated,
        /// the telescope should continue to update the slew destination coordinates 
        /// at the given offset rate. This will allow precise slews to a fast-moving 
        /// target with a slow-slewing telescope. When the slew completes, 
        /// the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> properties should
        /// reflect the final (adjusted) destination. This is not a required
        /// feature of this specification, however it is desirable. </description></item>
        /// <item><description>Use the <see cref="Tracking" /> property to enable and disable sidereal tracking (if supported). </description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public double RightAscensionRate
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "RightAscensionRate", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "RightAscensionRate", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Sets the telescope Park position to the current telescope position.
        /// </summary>
        public void SetPark()
        {
            memberFactory.CallMember(3, "SetPark", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Indicates the pointing state of the mount.
        /// </summary>
        /// <remarks>
        /// <para>For historical reasons, this property's name does not reflect its true meaning. The name will not be changed (so as to preserve 
        /// compatibility), but the meaning has since become clear. All conventional mounts have two pointing states for a given equatorial (sky) position. 
        /// Mechanical limitations often make it impossible for the mount to position the optics at given HA/Dec in one of the two pointing 
        /// states, but there are places where the same point can be reached sensibly in both pointing states (e.g. near the pole and 
        /// close to the meridian). In order to understand these pointing states, consider the following (thanks to Patrick Wallace for this info):</para>
        /// <para>All conventional telescope mounts have two axes nominally at right angles. For an equatorial, the longitude axis is mechanical 
        /// hour angle and the latitude axis is mechanical declination. Sky coordinates and mechanical coordinates are two completely separate arenas. 
        /// This becomes rather more obvious if your mount is an altaz, but it's still true for an equatorial. Both mount axes can in principle 
        /// move over a range of 360 deg. This is distinct from sky HA/Dec, where Dec is limited to a 180 deg range (+90 to -90).  Apart from 
        /// practical limitations, any point in the sky can be seen in two mechanical orientations. To get from one to the other the HA axis 
        /// is moved 180 deg and the Dec axis is moved through the pole a distance twice the sky codeclination (90 - sky declination).</para>
        /// <para>Mechanical zero HA/Dec will be one of the two ways of pointing at the intersection of the celestial equator and the local meridian. 
        /// In order to support Dome slaving, where it is important to know which side of the pier the mount is actually on, ASCOM has adopted the 
        /// convention that the Normal pointing state will be the state where a German Equatorial mount is on the East side of the pier, looking West, with the 
        /// counterweights below the optical assembly and that <see cref="PierSide.pierEast"></see> will represent this pointing state.</para>
        /// <para>Move your scope to this position and consider the two mechanical encoders zeroed. The two pointing states are, then:
        /// <list type="table">
        /// <item><term><b>Normal (<see cref="PierSide.pierEast"></see>)</b></term><description>Where the mechanical Dec is in the range -90 deg to +90 deg</description></item>
        /// <item><term><b>Beyond the pole (<see cref="PierSide.pierWest"></see>)</b></term><description>Where the mechanical Dec is in the range -180 deg to -90 deg or +90 deg to +180 deg.</description></item>
        /// </list>
        /// </para>
        /// <para>"Side of pier" is a "consequence" of the former definition, not something fundamental. 
        /// Apart from mechanical interference, the telescope can move from one side of the pier to the other without the mechanical Dec 
        /// having changed: you could track Polaris forever with the telescope moving from west of pier to east of pier or vice versa every 12h. 
        /// Thus, "side of pier" is, in general, not a useful term (except perhaps in a loose, descriptive, explanatory sense). 
        /// All this applies to a fork mount just as much as to a GEM, and it would be wrong to make the "beyond pole" state illegal for the 
        /// former. Your mount may not be able to get there if your camera hits the fork, but it's possible on some mounts. Whether this is useful 
        /// depends on whether you're in Hawaii or Finland.</para>
        /// <para>To first order, the relationship between sky and mechanical HA/Dec is as follows:</para>
        /// <para><b>Normal state:</b>
        /// <list type="bullet">
        /// <item><description>HA_sky  = HA_mech</description></item>
        /// <item><description>Dec_sky = Dec_mech</description></item>
        /// </list>
        /// </para>
        /// <para><b>Beyond the pole</b>
        /// <list type="bullet">
        /// <item><description>HA_sky  = HA_mech + 12h, expressed in range ± 12h</description></item>
        /// <item><description>Dec_sky = 180d - Dec_mech, expressed in range ± 90d</description></item>
        /// </list>
        /// </para>
        /// <para>Astronomy software often needs to know which which pointing state the mount is in. Examples include setting guiding polarities 
        /// and calculating dome opening azimuth/altitude. The meaning of the SideOfPier property, then is:
        /// <list type="table">
        /// <item><term><b>pierEast</b></term><description>Normal pointing state</description></item>
        /// <item><term><b>pierWest</b></term><description>Beyond the pole pointing state</description></item>
        /// </list>
        /// </para>
        /// <para>If the mount hardware reports neither the true pointing state (or equivalent) nor the mechanical declination axis position 
        /// (which varies from -180 to +180), a driver cannot calculate the pointing state, and *must not* implement SideOfPier.
        /// If the mount hardware reports only the mechanical declination axis position (-180 to +180) then a driver can calculate SideOfPier as follows:
        /// <list type="bullet">
        /// <item><description>pierEast = abs(mechanical dec) &lt;= 90 deg</description></item>
        /// <item><description>pierWest = abs(mechanical Dec) &gt; 90 deg</description></item>
        /// </list>
        /// </para>
        /// <para>It is allowed (though not required) that this property may be written to force the mount to flip. Doing so, however, may change 
        /// the right ascension of the telescope. During flipping, Telescope.Slewing must return True.</para>
        /// <para>This property is only available in telescope InterfaceVersions 2 and 3.</para>
        /// <para><b>Pointing State and Side of Pier - Help for Driver Developers</b></para>
        /// <para>A further document, "Pointing State and Side of Pier", is installed in the Developer Documentation folder by the ASCOM Developer 
        /// Components installer. This further explains the pointing state concept and includes diagrams illustrating how it relates 
        /// to physical side of pier for German equatorial telescopes. It also includes details of the tests performed by Conform to determine whether 
        /// the driver correctly reports the pointing state as defined above.</para>
        /// </remarks>
        public PierSide SideOfPier
        {
            get { return (PierSide)memberFactory.CallMember(1, "SideOfPier", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "SideOfPier", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// The local apparent sidereal time from the telescope's internal clock (hours, sidereal)
        /// </summary>
        /// <remarks>
        /// It is required for a driver to calculate this from the system clock if the telescope 
        /// has no accessible source of sidereal time. Local Apparent Sidereal Time is the sidereal 
        /// time used for pointing telescopes, and thus must be calculated from the Greenwich Mean
        /// Sidereal time, longitude, nutation in longitude and true ecliptic obliquity. 
        /// </remarks>
        public double SiderealTime
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "SiderealTime", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// The elevation above mean sea level (meters) of the site at which the telescope is located
        /// </summary>
        /// <remarks>
        /// Setting this property will raise an error if the given value is outside the range -300 through +10000 metres.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public double SiteElevation
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "SiteElevation", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "SiteElevation", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// The geodetic(map) latitude (degrees, positive North, WGS84) of the site at which the telescope is located.
        /// </summary>
        /// <remarks>
        /// Setting this property will raise an error if the given value is outside the range -90 to +90 degrees.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable. 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public double SiteLatitude
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "SiteLatitude", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "SiteLatitude", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// The longitude (degrees, positive East, WGS84) of the site at which the telescope is located.
        /// </summary>
        /// <remarks>
        /// Setting this property will raise an error if the given value is outside the range -180 to +180 degrees.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable.
        /// Note that West is negative! 
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public double SiteLongitude
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "SiteLongitude", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "SiteLongitude", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Specifies a post-slew settling time (sec.).
        /// </summary>
        /// <remarks>
        /// Adds additional time to slew operations. Slewing methods will not return, 
        /// and the <see cref="Slewing" /> property will not become False, until the slew completes and the SlewSettleTime has elapsed.
        /// This feature (if supported) may be used with mounts that require extra settling time after a slew. 
        /// </remarks>
        public short SlewSettleTime
        {
            get { return Convert.ToInt16(memberFactory.CallMember(1, "SlewSettleTime", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "SlewSettleTime", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Move the telescope to the given local horizontal coordinates, return when slew is complete
        /// </summary>
        /// <remarks>
        /// This Method must be implemented if <see cref="CanSlewAltAz" /> returns True.
        /// Raises an error if the slew fails. 
        /// The slew may fail if the target coordinates are beyond limits imposed within the driver component.
        /// Such limits include mechanical constraints imposed by the mount or attached instruments,
        /// building or dome enclosure restrictions, etc.
        /// <para>
        /// The <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> properties are not changed by this method. 
        /// Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is True. 
        /// This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        /// <param name="Azimuth">Target azimuth (degrees, North-referenced, positive East/clockwise).</param>
        /// <param name="Altitude">Target altitude (degrees, positive up)</param>
        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            memberFactory.CallMember(3, "SlewToAltAz", new Type[] { typeof(double), typeof(double) }, new object[] { Azimuth, Altitude });
        }

        /// <summary>
        /// This Method must be implemented if <see cref="CanSlewAltAzAsync" /> returns True.
        /// </summary>
        /// <remarks>
        /// This method should only be implemented if the properties <see cref="Altitude" />, <see cref="Azimuth" />,
        /// <see cref="RightAscension" />, <see cref="Declination" /> and <see cref="Slewing" /> can be read while the scope is slewing.
        /// Raises an error if starting the slew fails. Returns immediately after starting the slew.
        /// The client may monitor the progress of the slew by reading the <see cref="Azimuth" />, <see cref="Altitude" />,
        /// and <see cref="Slewing" /> properties during the slew. When the slew completes, Slewing becomes False. 
        /// The slew may fail if the target coordinates are beyond limits imposed within the driver component.
        /// Such limits include mechanical constraints imposed by the mount or attached instruments,
        /// building or dome enclosure restrictions, etc. 
        /// The <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> properties are not changed by this method. 
        /// <para>
        /// Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is True.</para>
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        /// <param name="Azimuth">Azimuth to which to move</param>
        /// <param name="Altitude">Altitude to which to move to</param>
        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            memberFactory.CallMember(3, "SlewToAltAzAsync", new Type[] { typeof(double), typeof(double) }, new object[] { Azimuth, Altitude });
        }

        /// <summary>
        /// Move the telescope to the given equatorial coordinates, return when slew is complete
        /// </summary>
        /// <remarks>
        /// This Method must be implemented if <see cref="CanSlew" /> returns True. Raises an error if the slew fails. 
        /// The slew may fail if the target coordinates are beyond limits imposed within the driver component.
        /// Such limits include mechanical constraints imposed by the mount or attached instruments,
        /// building or dome enclosure restrictions, etc. The target coordinates are copied to
        /// <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> whether or not the slew succeeds. 
        /// <para>Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is False.</para>
        /// </remarks>
        /// <param name="RightAscension">The destination right ascension (hours). Copied to <see cref="TargetRightAscension" />.</param>
        /// <param name="Declination">The destination declination (degrees, positive North). Copied to <see cref="TargetDeclination" />.</param>
        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            memberFactory.CallMember(3, "SlewToCoordinates", new Type[] { typeof(double), typeof(double) }, new object[] { RightAscension, Declination });
        }

        /// <summary>
        /// Move the telescope to the given equatorial coordinates, return immediately after starting the slew.
        /// </summary>
        /// <remarks>
        /// This method must be implemented if <see cref="CanSlewAsync" /> returns True. Raises an error if starting the slew failed. 
        /// Returns immediately after starting the slew. The client may monitor the progress of the slew by reading
        /// the <see cref="RightAscension" />, <see cref="Declination" />, and <see cref="Slewing" /> properties during the slew. When the slew completes,
        /// <see cref="Slewing" /> becomes False. The slew may fail to start if the target coordinates are beyond limits
        /// imposed within the driver component. Such limits include mechanical constraints imposed
        /// by the mount or attached instruments, building or dome enclosure restrictions, etc. 
        /// <para>The target coordinates are copied to <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" />
        /// whether or not the slew succeeds. 
        /// Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is False.</para>
        /// </remarks>
        /// <param name="RightAscension">The destination right ascension (hours). Copied to <see cref="TargetRightAscension" />.</param>
        /// <param name="Declination">The destination declination (degrees, positive North). Copied to <see cref="TargetDeclination" />.</param>
        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            memberFactory.CallMember(3, "SlewToCoordinatesAsync", new Type[] { typeof(double), typeof(double) }, new object[] { RightAscension, Declination });
        }

        /// <summary>
        /// Move the telescope to the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> coordinates, return when slew complete.
        /// </summary>
        /// <remarks>
        /// This Method must be implemented if <see cref="CanSlew" /> returns True. Raises an error if the slew fails. 
        /// The slew may fail if the target coordinates are beyond limits imposed within the driver component.
        /// Such limits include mechanical constraints imposed by the mount or attached
        /// instruments, building or dome enclosure restrictions, etc. 
        /// Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is False. 
        /// </remarks>
        public void SlewToTarget()
        {
            memberFactory.CallMember(3, "SlewToTarget", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Move the telescope to the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" />  coordinates,
        /// returns immediately after starting the slew.
        /// </summary>
        /// <remarks>
        /// This Method must be implemented if  <see cref="CanSlewAsync" /> returns True.
        /// Raises an error if starting the slew failed. 
        /// Returns immediately after starting the slew. The client may
        /// monitor the progress of the slew by reading the RightAscension, Declination,
        /// and Slewing properties during the slew. When the slew completes,  <see cref="Slewing" /> becomes False. 
        /// The slew may fail to start if the target coordinates are beyond limits imposed within 
        /// the driver component. Such limits include mechanical constraints imposed by the mount
        /// or attached instruments, building or dome enclosure restrictions, etc. 
        /// Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is False. 
        /// </remarks>
        public void SlewToTargetAsync()
        {
            memberFactory.CallMember(3, "SlewToTargetAsync", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// True if telescope is currently moving in response to one of the
        /// Slew methods or the <see cref="MoveAxis" /> method, False at all other times.
        /// </summary>
        /// <remarks>
        /// Reading the property will raise an error if the value is unavailable.
        /// If the telescope is not capable of asynchronous slewing,
        /// this property will always be False. 
        /// The definition of "slewing" excludes motion caused by sidereal tracking,
        /// <see cref="PulseGuide">PulseGuide</see>, <see cref="RightAscensionRate" />, and <see cref="DeclinationRate" />.
        /// It reflects only motion caused by one of the Slew commands, 
        /// flipping caused by changing the <see cref="SideOfPier" /> property, or <see cref="MoveAxis" />. 
        /// </remarks>
        public bool Slewing
        {
            get { return (bool)memberFactory.CallMember(1, "Slewing", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Matches the scope's local horizontal coordinates to the given local horizontal coordinates.
        /// </summary>
        /// <remarks>
        /// This must be implemented if the <see cref="CanSyncAltAz" /> property is True. Raises an error if matching fails. 
        /// <para>Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is True.</para>
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        /// <param name="Azimuth">Target azimuth (degrees, North-referenced, positive East/clockwise)</param>
        /// <param name="Altitude">Target altitude (degrees, positive up)</param>
        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            memberFactory.CallMember(3, "SyncToAltAz", new Type[] { typeof(double), typeof(double) }, new object[] { Azimuth, Altitude });
        }

        /// <summary>
        /// Matches the scope's equatorial coordinates to the given equatorial coordinates.
        /// </summary>
        /// <param name="RightAscension">The corrected right ascension (hours). Copied to the TargetRightAscension property.</param>
        /// <param name="Declination">The corrected declination (degrees, positive North). Copied to the TargetDeclination property.</param>
        /// <remarks>
        /// This must be implemented if the <see cref="CanSync" /> property is True. Raises an error if matching fails. 
        /// Raises an error if <see cref="AtPark" /> AtPark is True, or if <see cref="Tracking" /> is False. 
        /// The way that Sync is implemented is mount dependent and it should only be relied on to improve pointing for positions close to
        /// the position at which the sync is done.
        /// </remarks>
        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            memberFactory.CallMember(3, "SyncToCoordinates", new Type[] { typeof(double), typeof(double) }, new object[] { RightAscension, Declination });
        }

        /// <summary>
        /// Matches the scope's equatorial coordinates to the given equatorial coordinates.
        /// </summary>
        /// <remarks>
        /// This must be implemented if the <see cref="CanSync" /> property is True. Raises an error if matching fails. 
        /// Raises an error if <see cref="AtPark" /> AtPark is True, or if <see cref="Tracking" /> is False. 
        /// The way that Sync is implemented is mount dependent and it should only be relied on to improve pointing for positions close to
        /// the position at which the sync is done.
        /// </remarks>
        public void SyncToTarget()
        {
            memberFactory.CallMember(3, "SyncToTarget", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// The declination (degrees, positive North) for the target of an equatorial slew or sync operation
        /// </summary>
        /// <remarks>
        /// Setting this property will raise an error if the given value is outside the range -90 to +90 degrees.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable. 
        /// </remarks>
        public double TargetDeclination
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "TargetDeclination", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "TargetDeclination", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// The right ascension (hours) for the target of an equatorial slew or sync operation
        /// </summary>
        /// <remarks>
        /// Setting this property will raise an error if the given value is outside the range 0 to 24 hours.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable. 
        /// </remarks>
        public double TargetRightAscension
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "TargetRightAscension", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "TargetRightAscension", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// The state of the telescope's sidereal tracking drive.
        /// </summary>
        /// <remarks>
        /// Changing the value of this property will turn the sidereal drive on and off.
        /// However, some telescopes may not support changing the value of this property
        /// and thus may not support turning tracking on and off.
        /// See the <see cref="CanSetTracking" /> property. 
        /// </remarks>
        public bool Tracking
        {
            get { return (bool)memberFactory.CallMember(1, "Tracking", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "Tracking", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// The current tracking rate of the telescope's sidereal drive
        /// </summary>
        /// <remarks>
        /// Supported rates (one of the <see cref="DriveRates" />  values) are contained within the <see cref="TrackingRates" /> collection.
        /// Values assigned to TrackingRate must be one of these supported rates. 
        /// If an unsupported value is assigned to this property, it will raise an error. 
        /// The currently selected tracking rate be further adjusted via the <see cref="RightAscensionRate" /> 
        /// and <see cref="DeclinationRate" /> properties. These rate offsets are applied to the currently 
        /// selected tracking rate. Mounts must start up with a known or default tracking rate,
        /// and this property must return that known/default tracking rate until changed.
        /// <para>If the mount's current tracking rate cannot be determined (for example, 
        /// it is a write-only property of the mount's protocol), 
        /// it is permitted for the driver to force and report a default rate on connect.
        /// In this case, the preferred default is Sidereal rate.</para>
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public DriveRates TrackingRate
        {
            get { return (DriveRates)memberFactory.CallMember(1, "TrackingRate", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "TrackingRate", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Returns a collection of supported <see cref="DriveRates" /> values that describe the permissible
        /// values of the <see cref="TrackingRate" /> property for this telescope type.
        /// </summary>
        /// <remarks>
        /// At a minimum, this must contain an item for <see cref="DriveRates.driveSidereal" />.
        /// <para>This is only available for telescope InterfaceVersions 2 and 3</para>
        /// </remarks>
        public ITrackingRates TrackingRates
        {
            get
            {
                /* if (!memberFactory.IsCOMObject)
                     return memberFactory.TrackingRates;
                 else*/
                TL.LogMessage("TrackingRates", "");
                TL.LogMessage("TrackingRates", "Creating TrackingRates object");
                TrackingRates retval = new TrackingRates(memberFactory.GetObjType, memberFactory.GetLateBoundObject, TL);
                TL.LogMessage("TrackingRates", "Returning TrackingRates object");
                return retval;
            }
        }

        /// <summary>
        /// The UTC date/time of the telescope's internal clock
        /// </summary>
        /// <remarks>
        /// The driver must calculate this from the system clock if the telescope has no accessible
        /// source of UTC time. In this case, the property must not be writeable 
        /// (this would change the system clock!) and will instead raise an error.
        /// However, it is permitted to change the telescope's internal UTC clock 
        /// if it is being used for this property. This allows clients to adjust 
        /// the telescope's UTC clock as needed for accuracy. Reading the property
        /// will raise an error if the value has never been set or is otherwise unavailable. 
        /// </remarks>
        public DateTime UTCDate
        {
            get { return (DateTime)memberFactory.CallMember(1, "UTCDate", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "UTCDate", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Takes telescope out of the Parked state.
        /// </summary>
        /// <remarks>
        /// The state of <see cref="Tracking" /> after unparking is undetermined. 
        /// Valid only after <see cref="Park" />.
        /// Applications must check and change Tracking as needed after unparking. 
        /// Raises an error if unparking fails. Calling this with <see cref="AtPark" /> = False does nothing (harmless) 
        /// </remarks>
        public void Unpark()
        {
            memberFactory.CallMember(3, "Unpark", new Type[] { }, new object[] { });
        }

        #endregion

    }
    #endregion

    #region Rate wrapper
    //
    // Late bound Rate implementation
    //
    /// <summary>
    /// Describes a range of rates supported by the MoveAxis() method (degrees/per second)
    /// These are contained within the AxisRates collection. They serve to describe one or more supported ranges of rates of motion about a mechanical axis. 
    /// It is possible that the Rate.Maximum and Rate.Minimum properties will be equal. In this case, the Rate object expresses a single discrete rate. 
    /// Both the Rate.Maximum and Rate.Minimum properties are always expressed in units of degrees per second. 
    /// </summary>
    class _Rate : IRate
    {
        Type objTypeRate;
        object objRateLateBound;

        /*        public IEnumerator GetEnumerator() Commented out to remove style warning
                {
                    return null;
                } */

        public _Rate(int index, Type objTypeAxisRates, object objAxisRatesLateBound)
        {
            objRateLateBound = objTypeAxisRates.InvokeMember("Item",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objAxisRatesLateBound, new object[] { index });
            if (objRateLateBound == null) throw new NullReferenceException("Driver returned a null reference instead of a Rate object");
            objTypeRate = objRateLateBound.GetType();
        }

        public _Rate(object objRateLateBound)
        {
            this.objRateLateBound = objRateLateBound;
            objTypeRate = objRateLateBound.GetType();
        }

        /// <summary>
        /// The maximum rate (degrees per second)
        /// This must always be a positive number. It indicates the maximum rate in either direction about the axis. 
        /// </summary>
        public double Maximum
        {
            get
            {
                return (double)objTypeRate.InvokeMember("Maximum",
                            BindingFlags.Default | BindingFlags.GetProperty,
                            null, objRateLateBound, new object[] { });
            }
            set
            {
                objTypeRate.InvokeMember("Maximum",
                            BindingFlags.Default | BindingFlags.SetProperty,
                            null, objRateLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// The minimum rate (degrees per second)
        /// This must always be a positive number. It indicates the maximum rate in either direction about the axis. 
        /// </summary>
        public double Minimum
        {
            get
            {
                return (double)objTypeRate.InvokeMember("Minimum",
                            BindingFlags.Default | BindingFlags.GetProperty,
                            null, objRateLateBound, new object[] { });
            }
            set
            {
                objTypeRate.InvokeMember("Minimum",
                            BindingFlags.Default | BindingFlags.SetProperty,
                            null, objRateLateBound, new object[] { value });
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        public void Dispose()
        {
            if (this.objRateLateBound != null)
            {
                objRateLateBound = null;
            }
        }

        #endregion
    }
    #endregion

    #region Internal strongly typed collection wrappers
    //<summary>
    // Strongly typed enumerator for late bound Rate
    // objects being enumarated
    //</summary>
    class _RateEnumerator : IEnumerator, IDisposable
    {
        IEnumerator objEnumerator;
        //Type objTypeAxisRates; Commented out to remove style checker warnings
        //object objAxisRatesLateBound;

        public _RateEnumerator(Type objTypeAxisRates, object objAxisRatesLateBound)
        {
            //this.objTypeAxisRates = objTypeAxisRates;  Commented out to remove style checker warnings
            //this.objAxisRatesLateBound = objAxisRatesLateBound;
            objEnumerator = (IEnumerator)objTypeAxisRates.InvokeMember("GetEnumerator",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null, objAxisRatesLateBound, new object[] { });
        }

        public void Reset()
        {
            objEnumerator.Reset();
        }

        public bool MoveNext()
        {
            return objEnumerator.MoveNext();
        }

        public Object Current
        {
            get
            {
                return new _Rate(objEnumerator.Current);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this.objEnumerator != null)
            {
                objEnumerator = null;
            }
        }

        #endregion
    }

    //<summary>
    // Late bound Axis Rates implementation.
    //</summary>
    class _AxisRates : IAxisRates
    {
        Type objTypeAxisRates;
        object objAxisRatesLateBound;
        TraceLogger TL;
        TelescopeAxes CurrentAxis;

        public _AxisRates(TelescopeAxes Axis, Type objTypeScope, object objScopeLateBound, TraceLogger TraceLog)
        {
            objAxisRatesLateBound = objTypeScope.InvokeMember("AxisRates",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null, objScopeLateBound, new object[] { (int)Axis });
            if (objAxisRatesLateBound == null) throw new NullReferenceException("Driver returned a null reference instead of an AxisRates object for axis " + Axis.ToString());
            objTypeAxisRates = objAxisRatesLateBound.GetType();
            CurrentAxis = Axis;
            TL = TraceLog;
            TL.LogMessage("AxisRates Class", "Created object: " + objTypeAxisRates.FullName + " for axis: " + CurrentAxis.ToString());
        }

        public IRate this[int index]
        {
            get
            {
                _Rate retval = new _Rate(index, objTypeAxisRates, objAxisRatesLateBound);
                TL.LogMessage("AxisRates Class", "Axis: " + CurrentAxis.ToString() + "- returned rate " + index.ToString() + " = Minimum: " + retval.Minimum.ToString() + ", Maximum: " + retval.Maximum.ToString());
                return retval;
            }
        }

        public IEnumerator GetEnumerator()
        {
            TL.LogMessage("AxisRates Class", "GetEnumerator(" + CurrentAxis.ToString() + "): Returning rate enumerator");
            return new _RateEnumerator(objTypeAxisRates, objAxisRatesLateBound);
        }

        public int Count
        {
            get
            {
                int retval = (int)objTypeAxisRates.InvokeMember("Count",
                            BindingFlags.Default | BindingFlags.GetProperty,
                            null, objAxisRatesLateBound, new object[] { });
                TL.LogMessage("AxisRates Class", "Count(" + CurrentAxis.ToString() + ") = " + retval);
                return retval;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this.objAxisRatesLateBound != null)
            {
                objAxisRatesLateBound = null;
            }
        }

        #endregion
    }

    ///<summary>
    /// Late bound TrackingRates implementation
    ///</summary>
    public class TrackingRates : ITrackingRates, IEnumerable //, IEnumerator
    {
        Type objTypeTrackingRates;
        object objTrackingRatesLateBound;
        TraceLogger TL;

        // private static int pos = 0;

        /// <summary>
        /// TrackingRates constructor
        /// </summary>
        /// <param name="objTypeScope">The type of the supplied object</param>
        /// <param name="objScopeLateBound">The object representing the telescope device</param>
        /// <param name="TraceLog">A pointer to a trace loger in which to record trace information</param>
        public TrackingRates(Type objTypeScope, object objScopeLateBound, TraceLogger TraceLog)
        {
            objTrackingRatesLateBound = objTypeScope.InvokeMember("TrackingRates",
                                                                  BindingFlags.Default | BindingFlags.GetProperty,
                                                                  null,
                                                                  objScopeLateBound,
                                                                  new object[] { });
            if (objTrackingRatesLateBound == null) throw new NullReferenceException("Driver returned a null reference instead of an TrackingRates object");
            objTypeTrackingRates = objTrackingRatesLateBound.GetType();
            TL = TraceLog; // Save the trace logger reference
            TL.LogMessage("TrackingRates Class", "Created object: " + objTypeTrackingRates.FullName);
        }

        /// <summary>
        /// Return a drive rate given its index
        /// </summary>
        /// <param name="index">Index position of the item</param>
        /// <returns>Integer DriveRate enum value</returns>
        public DriveRates this[int index]
        {
            get
            {
                DriveRates retval = (DriveRates)objTypeTrackingRates.InvokeMember("Item",
                                                                     BindingFlags.Default | BindingFlags.GetProperty,
                                                                     null,
                                                                     objTrackingRatesLateBound,
                                                                     new object[] { index });
                TL.LogMessage("TrackingRates Class", "DriveRates[" + index + "] " + retval.ToString());
                return retval;
            }
        }

        /// <summary>
        /// Returns an enumerator for the driverates object
        /// </summary>
        /// <returns>IEnumerator object</returns>
        public IEnumerator GetEnumerator()
        {
            //global::System.Windows.Forms.MessageBox.Show("DriverAccess.GetEnumerator - before calling Object.GetEnumerator");

            object enumeratorobj = (IEnumerator)objTypeTrackingRates.InvokeMember("GetEnumerator",
                                                                                  BindingFlags.Default | BindingFlags.InvokeMethod,
                                                                                  null,
                                                                                  objTrackingRatesLateBound,
                                                                                  new object[] { });
            IEnumerator enumerator = (IEnumerator)enumeratorobj;
            TL.LogMessage("TrackingRates Class", "Enumerator: " + enumerator.ToString());
            return enumerator;
        }

        /// <summary>
        /// Returns the number of driverates supported by the telescope 
        /// </summary>
        public int Count
        {
            get
            {
                int retval = (int)objTypeTrackingRates.InvokeMember("Count",
                                                               BindingFlags.Default | BindingFlags.GetProperty,
                                                               null,
                                                               objTrackingRatesLateBound,
                                                               new object[] { });
                TL.LogMessage("TrackingRates Class", "Count: " + retval);
                return retval;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of this object
        /// </summary>
        public void Dispose()
        {
            if (this.objTrackingRatesLateBound != null)
            {
                objTrackingRatesLateBound = null;
            }
        }

        #endregion
    }

    /// <summary>
    /// A collection of rates at which the telescope may be moved about the specified axis by the <see cref="ITelescopeV3.MoveAxis" /> method.
    /// This is only used if the telescope interface version is 2 or 3
    /// </summary>
    /// <remarks><para>See the description of the <see cref="ITelescopeV3.MoveAxis" /> method for more information.</para>
    /// <para>This method must return an empty collection if <see cref="ITelescopeV3.MoveAxis" /> is not supported.</para>
    /// <para>The values used in <see cref="IRate" /> members must be non-negative; forward and backward motion is achieved by the application
    /// applying an appropriate sign to the returned <see cref="IRate" /> values in the <see cref="ITelescopeV3.MoveAxis" /> command.</para>
    /// </remarks>
    public class AxisRates : ASCOM.DeviceInterface.IAxisRates, IEnumerator
    {
        //List<Rate> m_Rates = new List<Rate>();        //' Empty array, but an array nonetheless
        TraceLogger TL;
        int CurrentPosition;

        ASCOM.Interface.IAxisRates AxisRatesP5;

        /// <summary>
        /// Creates an empty AxisRates object
        /// </summary>
        public AxisRates() // Default constructor that returns an empty object
        {
            this.Reset();
            TL = null;
        }

        internal AxisRates(ASCOM.Interface.IAxisRates AxisRates, TraceLogger traceLogger)
        {
            TL = traceLogger;
            AxisRatesP5 = AxisRates;
            this.Reset();
            foreach (ASCOM.Interface.IRate Rate in AxisRates)
            {
                if (!(TL == null)) TL.LogMessage("AxisRates Class P5 New", "Adding rate: - Minimum: " + Rate.Minimum + ", Maximum: " + Rate.Maximum);
                //m_Rates.Add(new Rate(Rate.Minimum, Rate.Maximum));
            }

        }

        #region IEnumerable Members
        /// <summary>
        /// Adds a new rate to the collection
        /// </summary>
        /// <param name="Minimum">The minimum value of this rate range</param>
        /// <param name="Maximum">The maximum value of this rate range</param>
        public void Add(double Minimum, double Maximum)
        {
            //m_Rates.Add(new Rate(Minimum, Maximum));
        }

        /// <summary>
        /// Returns the current value of the collection
        /// </summary>
        public object Current
        {
            get
            {
                return this[CurrentPosition];
            }
        }

        /// <summary>
        /// Moves the pointer to the next element
        /// </summary>
        /// <returns>True if the Current will return a valid value</returns>
        public bool MoveNext()
        {
            bool ReturnValue;
            if (CurrentPosition <= this.Count) CurrentPosition += 1;

            if (CurrentPosition > this.Count) ReturnValue = false;
            else ReturnValue = true;
            if (!(TL == null)) TL.LogMessage("AxisRates Class P5", "MoveNext - Position: " + CurrentPosition + ", Return value: " + ReturnValue);

            return ReturnValue;
        }

        /// <summary>
        /// Resets the enumerator to its initial posiiton before the first element
        /// </summary>
        public void Reset()
        {
            if (!(TL == null)) TL.LogMessage("AxisRates Class P5", "Reset enumerator position to 0");
            CurrentPosition = 0;
        }

        #endregion

        #region IAxisRates Members

        /// <summary>
        /// Returns the number of rate objects in the collection
        /// </summary>
        public int Count
        {
            get
            {
                if (!(TL == null)) TL.LogMessage("AxisRates Class P5", "Count: " + AxisRatesP5.Count);
                return AxisRatesP5.Count;
            }
        }

        /// <summary>
        /// Disposes of any external resources acquired by the object
        /// </summary>
        public void Dispose()
        {
            // throw new System.NotImplementedException(); Nothing to dispose in this class
        }

        /// <summary>
        /// Returns an enumerator to provide access to the collection members
        /// </summary>
        /// <returns>IEnumerator object</returns>
        public IEnumerator GetEnumerator()
        {
            if (!(TL == null)) TL.LogMessage("AxisRates Class P5", "Returning Enumerator - New object");
            return this;
        }

        /// <summary>
        /// Return information about the rates at which the telescope may be moved about the specified axis by the <see cref="Telescope.MoveAxis" /> method.
        /// </summary>
        /// <param name="index">The axis about which rate information is desired</param>
        /// <value>Collection of Rate objects describing the supported rates of motion that can be supplied to the <see cref="Telescope.MoveAxis" /> method for the specified axis.</value>
        /// <returns>Collection of Rate objects </returns>
        /// <remarks><para>The (symbolic) values for Index (<see cref="TelescopeAxes" />) are:</para>
        /// <bl>
        /// <li><see cref="TelescopeAxes.axisPrimary"/> 0 Primary axis (e.g., Hour Angle or Azimuth)</li>
        /// <li><see cref="TelescopeAxes.axisSecondary"/> 1 Secondary axis (e.g., Declination or Altitude)</li>
        /// <li><see cref="TelescopeAxes.axisTertiary"/> 2 Tertiary axis (e.g. imager rotator/de-rotator)</li> 
        /// </bl>
        /// </remarks>
        public IRate this[int index]
        {
            get
            {
                if (!(TL == null)) TL.LogMessage("AxisRates Class P5", "Get IRate - Index: " + index);

                try
                {
                    if (!(TL == null)) TL.LogMessage("AxisRates Class P5", "Get IRate - Minimum: " + AxisRatesP5[index].Minimum + ", Maximum: " + AxisRatesP5[index].Maximum);
                }
                catch (Exception ex)
                {
                    if (!(TL == null)) TL.LogMessage("AxisRates Class P5", "Exception: " + ex.ToString());
                }

                return new Rate(AxisRatesP5[index].Minimum, AxisRatesP5[index].Maximum);
            }
        }

        #endregion

    }

    /// <summary>
    /// Describes a range of rates supported by the <see cref="Telescope.MoveAxis" /> method (degrees/per second)
    /// These are contained within an <see cref="AxisRates" /> collection and serve to describe one or more supported ranges of rates of motion about a mechanical axis. 
    /// It is possible that the <see cref="Rate.Maximum" /> and <see cref="Rate.Minimum" /> properties will be equal. In this case, the <see cref="Rate" /> object expresses a single discrete rate. 
    /// Both the <see cref="Rate.Minimum" />  and <see cref="Rate.Maximum" />  properties are always expressed in units of degrees per second.
    /// This is only using for Telescope InterfaceVersions 2 and 3
    /// </summary>
    /// <remarks>Values used must be non-negative and are scalar values. You do not need to supply complementary negative rates for each positive 
    /// rate that you specify. Movement in both directions is achieved by the application applying an appropriate positive or negative sign to the 
    /// rate when it is used in the <see cref="Telescope.MoveAxis" /> command.</remarks>
    public class Rate : IRate
    {

        double m_dMaximumR = 0;
        double m_dMinimumR = 0;

        //'
        //' Default constructor - Internal prevents public creation
        //' of instances. These are values for AxisRates.

        internal Rate(double Minimum, double Maximum)
        {
            m_dMaximumR = Maximum;
            m_dMinimumR = Minimum;
        }

        #region IRate Members

        /// <summary>
        /// The maximum rate (degrees per second)
        /// This must always be a positive number. It indicates the maximum rate in either direction about the axis. 
        /// </summary>
        public double Maximum
        {
            get { return m_dMaximumR; }
            set { m_dMaximumR = value; }
        }

        /// <summary>
        /// The minimum rate (degrees per second)
        /// This must always be a positive number. It indicates the maximum rate in either direction about the axis. 
        /// </summary>
        public double Minimum
        {
            get { return m_dMinimumR; }
            set { m_dMinimumR = value; }
        }

        /// <summary>
        /// Disposes of any external resources acquired by the rate object
        /// </summary>
        public void Dispose()
        {
        }

        #endregion
    }
    #endregion

}
