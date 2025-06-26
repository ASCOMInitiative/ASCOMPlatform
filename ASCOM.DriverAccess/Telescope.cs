//-----------------------------------------------------------------------
// <summary>Defines the Telescope class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Collections;
using System.Reflection;
using System.Globalization;

namespace ASCOM.DriverAccess
{

    #region Telescope Wrapper
    /// <summary>
    /// Implements a telescope class to access any registered ASCOM telescope
    /// </summary>
    public class Telescope : AscomDriver, ITelescopeV4, ITelescopeV3
    {
        internal MemberFactory memberFactory;
        internal bool isPlatform7Telescope = false;
        internal bool isPlatform6Telescope = false;
        internal bool isPlatform5Telescope = false;

        #region Telescope constructors

        /// <summary>
        /// Static initialiser called once per AppDomain to log the component name.
        /// </summary>
        static Telescope()
        {
            Log.Component(Assembly.GetExecutingAssembly().FullName, "DriverAccess.Telescope");
        }

        /// <summary>
        /// Creates an instance of the telescope class.
        /// </summary>
        /// <param name="telescopeId">The ProgID for the telescope</param>
        public Telescope(string telescopeId) : base(telescopeId)
        {
            memberFactory = base.MemberFactory;

            // Set capabilities depending on returned interface version
            switch (DriverInterfaceVersion)
            {
                // Unknown or Platform 5
                case 0:
                case 1:
                    TL.LogMessage("Telescope", $"Reported interface version: {DriverInterfaceVersion}, setting Platform 5 compatibility.");
                    isPlatform5Telescope = true;
                    break;

                // Platform 6
                case 2:
                case 3:
                    TL.LogMessage("Telescope", $"Reported interface version: {DriverInterfaceVersion}, setting Platform 6 compatibility.");
                    isPlatform6Telescope = true;
                    break;

                // Platform 7 onward
                case 4:
                default:
                    TL.LogMessage("Telescope", $"Reported interface version: {DriverInterfaceVersion}, setting Platform 7 compatibility.");
                    isPlatform6Telescope = true;
                    isPlatform7Telescope = true;
                    break;
            }

            TL.LogMessage("Telescope", $"Platform 5 Telescope: {isPlatform5Telescope},  Platform 6 Telescope: {isPlatform6Telescope},  Platform 7 Telescope: {isPlatform7Telescope}");
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
        /// <returns>The DriverID of the user selected telescope. Null if the dialogue is cancelled.</returns>
        public static string Choose(string telescopeId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Telescope";
                return chooser.Choose(telescopeId);
            }
        }

        /// <summary>
		/// State response from the device
		/// </summary>
        /// <remarks>
        /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on using the class returned by this property.</para>
        /// </remarks>
		public TelescopeState TelescopeState
        {
            get
            {
                // Create a state object to return.
                TelescopeState state = new TelescopeState(DeviceState, TL);
                TL.LogMessage(nameof(TelescopeState), $"Returning: '{state.Altitude}' '{state.AtHome}' '{state.AtPark}' '{state.Azimuth}' '{state.Declination}' '{state.IsPulseGuiding}' " +
                    $"'{state.RightAscension}' '{state.SideOfPier}' '{state.SiderealTime}' '{state.Slewing}' '{state.Tracking}' '{state.UTCDate}' '{state.TimeStamp}'");

                // Return the device specific state class
                return state;
            }
        }

        #endregion

        #region ITelescope Members

        /// <inheritdoc/>
        public void AbortSlew()
        {
            TL.LogMessage("AbortSlew", "Calling method");
            memberFactory.CallMember(3, "AbortSlew", new Type[] { }, new object[] { });
            TL.LogMessage("AbortSlew", "Finished");
        }

        /// <inheritdoc/>
        public AlignmentModes AlignmentMode
        {
            get { return (AlignmentModes)memberFactory.CallMember(1, "AlignmentMode", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double Altitude
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Altitude", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double ApertureArea
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "ApertureArea", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double ApertureDiameter
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "ApertureDiameter", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool AtHome
        {
            get { return (bool)memberFactory.CallMember(1, "AtHome", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool AtPark
        {
            get { return (bool)memberFactory.CallMember(1, "AtPark", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
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
                                TL.LogMessage("AxisRates", "Found Minimum: " + AxisRatesP6[i].Minimum + ", Maximum: " + AxisRatesP6[i].Maximum);
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
                                TL.LogMessage("AxisRates", "Found Minimum: " + AxisRatesP5[i].Minimum + ", Maximum: " + AxisRatesP5[i].Maximum);
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
                            TL.LogMessage("AxisRates", "Found Minimum: " + ReturnValue[i].Minimum + ", Maximum: " + ReturnValue[i].Maximum);
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

        /// <inheritdoc/>
        public double Azimuth
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Azimuth", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool CanFindHome
        {
            get { return (bool)memberFactory.CallMember(1, "CanFindHome", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            return (bool)memberFactory.CallMember(3, "CanMoveAxis", new Type[] { typeof(TelescopeAxes) }, new object[] { Axis });
        }

        /// <inheritdoc/>
        public bool CanPark
        {
            get { return (bool)memberFactory.CallMember(1, "CanPark", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanPulseGuide
        {
            get { return (bool)memberFactory.CallMember(1, "CanPulseGuide", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanSetDeclinationRate
        {
            get { return (bool)memberFactory.CallMember(1, "CanSetDeclinationRate", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanSetGuideRates
        {
            get { return (bool)memberFactory.CallMember(1, "CanSetGuideRates", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanSetPark
        {
            get { return (bool)memberFactory.CallMember(1, "CanSetPark", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanSetPierSide
        {
            get { return (bool)memberFactory.CallMember(1, "CanSetPierSide", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanSetRightAscensionRate
        {
            get { return (bool)memberFactory.CallMember(1, "CanSetRightAscensionRate", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanSetTracking
        {
            get { return (bool)memberFactory.CallMember(1, "CanSetTracking", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanSlew
        {
            get { return (bool)memberFactory.CallMember(1, "CanSlew", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanSlewAltAz
        {
            get { return (bool)memberFactory.CallMember(1, "CanSlewAltAz", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanSlewAltAzAsync
        {
            get { return (bool)memberFactory.CallMember(1, "CanSlewAltAzAsync", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanSlewAsync
        {
            get { return (bool)memberFactory.CallMember(1, "CanSlewAsync", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanSync
        {
            get { return (bool)memberFactory.CallMember(1, "CanSync", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanSyncAltAz
        {
            get { return (bool)memberFactory.CallMember(1, "CanSyncAltAz", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool CanUnpark
        {
            get { return (bool)memberFactory.CallMember(1, "CanUnpark", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double Declination
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Declination", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double DeclinationRate
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "DeclinationRate", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "DeclinationRate", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            return (PierSide)memberFactory.CallMember(3, "DestinationSideOfPier", new Type[] { typeof(double), typeof(double) }, new object[] { RightAscension, Declination });
        }

        /// <inheritdoc/>
        public bool DoesRefraction
        {
            get { return (bool)memberFactory.CallMember(1, "DoesRefraction", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "DoesRefraction", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public EquatorialCoordinateType EquatorialSystem
        {
            get { return (EquatorialCoordinateType)memberFactory.CallMember(1, "EquatorialSystem", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public void FindHome()
        {
            memberFactory.CallMember(3, "FindHome", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
        public double FocalLength
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "FocalLength", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double GuideRateDeclination
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "GuideRateDeclination", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "GuideRateDeclination", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public double GuideRateRightAscension
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "GuideRateRightAscension", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "GuideRateRightAscension", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public bool IsPulseGuiding
        {
            get { return (bool)memberFactory.CallMember(1, "IsPulseGuiding", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            memberFactory.CallMember(3, "MoveAxis", new Type[] { typeof(TelescopeAxes), typeof(double) }, new object[] { Axis, Rate });
        }

        /// <inheritdoc/>
        public void Park()
        {
            memberFactory.CallMember(3, "Park", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            memberFactory.CallMember(3, "PulseGuide", new Type[] { typeof(GuideDirections), typeof(int) }, new object[] { (int)Direction, Duration });
        }

        /// <inheritdoc/>
        public double RightAscension
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "RightAscension", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double RightAscensionRate
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "RightAscensionRate", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "RightAscensionRate", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public void SetPark()
        {
            memberFactory.CallMember(3, "SetPark", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
        public PierSide SideOfPier
        {
            get { return (PierSide)memberFactory.CallMember(1, "SideOfPier", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "SideOfPier", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public double SiderealTime
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "SiderealTime", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double SiteElevation
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "SiteElevation", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "SiteElevation", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public double SiteLatitude
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "SiteLatitude", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "SiteLatitude", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public double SiteLongitude
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "SiteLongitude", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "SiteLongitude", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Specifies a post-slew settling time (sec.).
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid settle time is set.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
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

        /// <inheritdoc/>
        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            memberFactory.CallMember(3, "SlewToAltAz", new Type[] { typeof(double), typeof(double) }, new object[] { Azimuth, Altitude });
        }

        /// <inheritdoc/>
        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            memberFactory.CallMember(3, "SlewToAltAzAsync", new Type[] { typeof(double), typeof(double) }, new object[] { Azimuth, Altitude });
        }

        /// <inheritdoc/>
        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            memberFactory.CallMember(3, "SlewToCoordinates", new Type[] { typeof(double), typeof(double) }, new object[] { RightAscension, Declination });
        }

        /// <inheritdoc/>
        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            memberFactory.CallMember(3, "SlewToCoordinatesAsync", new Type[] { typeof(double), typeof(double) }, new object[] { RightAscension, Declination });
        }

        /// <inheritdoc/>
        public void SlewToTarget()
        {
            memberFactory.CallMember(3, "SlewToTarget", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
        public void SlewToTargetAsync()
        {
            memberFactory.CallMember(3, "SlewToTargetAsync", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
        public bool Slewing
        {
            get { return (bool)memberFactory.CallMember(1, "Slewing", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            memberFactory.CallMember(3, "SyncToAltAz", new Type[] { typeof(double), typeof(double) }, new object[] { Azimuth, Altitude });
        }

        /// <inheritdoc/>
        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            memberFactory.CallMember(3, "SyncToCoordinates", new Type[] { typeof(double), typeof(double) }, new object[] { RightAscension, Declination });
        }

        /// <inheritdoc/>
        public void SyncToTarget()
        {
            memberFactory.CallMember(3, "SyncToTarget", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
        public double TargetDeclination
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "TargetDeclination", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "TargetDeclination", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public double TargetRightAscension
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "TargetRightAscension", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "TargetRightAscension", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public bool Tracking
        {
            get { return (bool)memberFactory.CallMember(1, "Tracking", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "Tracking", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public DriveRates TrackingRate
        {
            get { return (DriveRates)memberFactory.CallMember(1, "TrackingRate", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "TrackingRate", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public DateTime UTCDate
        {
            get { return (DateTime)memberFactory.CallMember(1, "UTCDate", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "UTCDate", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
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
                        null, objAxisRatesLateBound, new object[] { index },
                        CultureInfo.InvariantCulture);
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
                            null, objRateLateBound, new object[] { },
                            CultureInfo.InvariantCulture);
            }
            set
            {
                objTypeRate.InvokeMember("Maximum",
                            BindingFlags.Default | BindingFlags.SetProperty,
                            null, objRateLateBound, new object[] { value },
                            CultureInfo.InvariantCulture);
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
                            null, objRateLateBound, new object[] { },
                            CultureInfo.InvariantCulture);
            }
            set
            {
                objTypeRate.InvokeMember("Minimum",
                            BindingFlags.Default | BindingFlags.SetProperty,
                            null, objRateLateBound, new object[] { value },
                            CultureInfo.InvariantCulture);
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
    // objects being enumerated
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
                        null, objAxisRatesLateBound, new object[] { },
                        CultureInfo.InvariantCulture);
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
                        null, objScopeLateBound, new object[] { (int)Axis },
                        CultureInfo.InvariantCulture);
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
                            null, objAxisRatesLateBound, new object[] { },
                            CultureInfo.InvariantCulture);
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
        /// <param name="TraceLog">A pointer to a trace logger in which to record trace information</param>
        public TrackingRates(Type objTypeScope, object objScopeLateBound, TraceLogger TraceLog)
        {
            objTrackingRatesLateBound = objTypeScope.InvokeMember("TrackingRates",
                                                                  BindingFlags.Default | BindingFlags.GetProperty,
                                                                  null,
                                                                  objScopeLateBound,
                                                                  new object[] { },
                                                                  CultureInfo.InvariantCulture);
            if (objTrackingRatesLateBound == null) throw new NullReferenceException("Driver returned a null reference instead of a TrackingRates object");
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
                                                                     new object[] { index },
                                                                     CultureInfo.InvariantCulture);
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
                                                                                  new object[] { },
                                                                                  CultureInfo.InvariantCulture);
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
                                                               new object[] { },
                                                               CultureInfo.InvariantCulture);
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
    /// A collection of rates at which the telescope may be moved about the specified axis by the <see cref="ITelescopeV4.MoveAxis" /> method.
    /// This is only used if the telescope interface version is 2 or 3
    /// </summary>
    /// <remarks><para>See the description of the <see cref="ITelescopeV4.MoveAxis" /> method for more information.</para>
    /// <para>This method must return an empty collection if <see cref="ITelescopeV4.MoveAxis" /> is not supported.</para>
    /// <para>The values used in <see cref="IRate" /> members must be non-negative; forward and backward motion is achieved by the application
    /// applying an appropriate sign to the returned <see cref="IRate" /> values in the <see cref="ITelescopeV4.MoveAxis" /> command.</para>
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
        /// Resets the enumerator to its initial position before the first element
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
    /// This is only using for Telescope InterfaceVersions 2 and later.
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
