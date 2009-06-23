//
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
//
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Interface;
using ASCOM.HelperNET;

namespace ASCOM.DriverAccess
{

	#region Telescope Wrapper
	/// <summary>
    /// Implements a telescope class to access any registered ASCOM telescope
    /// </summary>
    public class Telescope : ASCOM.Interface.ITelescope, IDisposable 
    {
        object objScopeLateBound;
		ASCOM.Interface.ITelescope ITelescope;
		Type objTypeScope;

        /// <summary>
        /// Creates an instance of the telescope class.
        /// </summary>
        /// <param name="telescopeID">The ProgID for the telescope</param>
        public Telescope(string telescopeID)
		{
			// Get Type Information 
            objTypeScope = Type.GetTypeFromProgID(telescopeID);
			
			// Create an instance of the telescope object
            objScopeLateBound = Activator.CreateInstance(objTypeScope);

			// Try to see if this driver has an ASCOM.Telescope interface
			try
			{
				ITelescope = (ASCOM.Interface.ITelescope)objScopeLateBound;
			}
			catch (Exception)
			{
				ITelescope = null;
			}
		}

        /// <summary>
        /// The Choose() method returns the DriverID of the selected driver.
        /// Choose() allows you to optionally pass the DriverID of a "current" driver (you probably save this in the registry),
        /// and the corresponding telescope type is pre-selected in the Chooser's list.
        /// In this case, the OK button starts out enabled (lit-up); the assumption is that the pre-selected driver has already been configured. 
        /// </summary>
        /// <param name="telescopeID">Optional DriverID of the previously selected telescope that is to be the pre-selected telescope in the list. </param>
        /// <returns>The DriverID of the user selected telescope. Null if the dialog is canceled.</returns>
        public static string Choose(string telescopeID)
        {
			Chooser oChooser = new Chooser();
			oChooser.DeviceType = "Telescope";			// Requires Helper 5.0.3 (May '07)
			return oChooser.Choose(telescopeID);
        }
        
        #region ITelescope Members
        /// <summary>
        /// Stops a slew in progress.
        /// Effective only after a call to SlewToTargetAsync(), SlewToCoordinatesAsync(), SlewToAltAzAsync(), or MoveAxis().
        /// Does nothing if no slew/motion is in progress. 
        /// Tracking is returned to its pre-slew state.
        /// Raises an error if AtPark is true. 
        /// </summary>
        public void AbortSlew()
        {
			if (ITelescope != null)
				ITelescope.AbortSlew();
			else
				objTypeScope.InvokeMember("AbortSlew",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { });
        }
        /// <summary>
        /// The alignment mode of the mount.
        /// </summary>
        public AlignmentModes AlignmentMode
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.AlignmentMode;
				else
					return (AlignmentModes)objTypeScope.InvokeMember("AlignmentMode",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// The Altitude above the local horizon of the telescope's current position (degrees, positive up)
        /// </summary>
        public double Altitude
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.Altitude;
				else
					return (double)objTypeScope.InvokeMember("Altitude", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            } 
        }

        /// <summary>
        /// The area of the telescope's aperture, taking into account any obstructions (square meters)
        /// </summary>
        public double ApertureArea
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.ApertureArea;
				else
					return (double)objTypeScope.InvokeMember("ApertureArea", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// The telescope's effective aperture diameter (meters)
        /// </summary>
        public double ApertureDiameter
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.ApertureDiameter;
				else
					return (double)objTypeScope.InvokeMember("ApertureDiameter",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }   
        }

        /// <summary>
        /// True if the telescope is stopped in the Home position. Set only following a FindHome() operation, and reset with any slew operation. This property must be False if the telescope does not support homing. 
        /// </summary>
        public bool AtHome
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.AtHome;
				else
					return (bool)objTypeScope.InvokeMember("AtHome", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if the telescope has been put into the parked state by the Park() method. Set False by calling the Unpark() method.
        /// AtPark is True when the telescope is in the parked state. This is achieved by calling the Park method. When AtPark is true, the telescope movement is stopped (or restricted to a small safe range of movement) and all calls that would cause telescope movement (e.g. slewing, changing Tracking state) must not do so, and must raise an error. The telescope is taken out of parked state by calling the Unpark() method. If the telescope cannot be parked, then AtPark must always return False. 
        /// </summary>
        public bool AtPark
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.AtPark;
				else
					return (bool)objTypeScope.InvokeMember("AtPark", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Determine the rates at which the telescope may be moved about the specified axis by the MoveAxis() method.
        /// See the description of MoveAxis() for more information. This method must return an empty collection if MoveAxis is not supported. 
        /// </summary>
		/// <param name="Axis">The axis about which rate information is desired (TelescopeAxes value)</param>
        /// <returns>Collection of Axis Rates</returns>
        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
			if (ITelescope != null)
				return ITelescope.AxisRates(Axis);
			else
				return new _AxisRates(Axis, objTypeScope, objScopeLateBound);
        }
        /// <summary>
        /// The azimuth at the local horizon of the telescope's current position (degrees, North-referenced, positive East/clockwise).
        /// </summary>
        public double Azimuth
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.Azimuth;
				else
					return (double)objTypeScope.InvokeMember("Azimuth", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }
        /// <summary>
        /// True if this telescope is capable of programmed finding its home position (FindHome() method).
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanFindHome
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanFindHome;
				else
					return (bool)objTypeScope.InvokeMember("CanFindHome", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if this telescope is capable of programmed parking (Park() method)
        /// </summary>
        /// <param name="Axis"></param>
        /// <returns></returns>
        public bool CanMoveAxis(TelescopeAxes Axis)
        {

			if (ITelescope != null)
				return ITelescope.CanMoveAxis(Axis);
			else
				return (bool)objTypeScope.InvokeMember("CanMoveAxis", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objScopeLateBound, new object[] {(int)Axis });
        }
        /// <summary>
        /// True if this telescope is capable of programmed parking (Park() method)
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanPark
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanPark;
				else
					return (bool)objTypeScope.InvokeMember("CanPark", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }
        /// <summary>
        /// True if this telescope is capable of software-pulsed guiding (via the PulseGuide() method)
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanPulseGuide
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanPulseGuide;
				else
					return (bool)objTypeScope.InvokeMember("CanPulseGuide", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if the DeclinationRate property can be changed to provide offset tracking in the declination axis.
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanSetDeclinationRate
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanSetDeclinationRate;
				else
					return (bool)objTypeScope.InvokeMember("CanSetDeclinationRate", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if the guide rate properties used for PulseGuide() can ba adjusted.
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanSetGuideRates
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanSetGuideRates;
				else
					return (bool)objTypeScope.InvokeMember("CanSetGuideRates", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if this telescope is capable of programmed setting of its park position (SetPark() method)
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanSetPark
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanSetPark;
				else
					return (bool)objTypeScope.InvokeMember("CanSetPark", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if the SideOfPier property can be set, meaning that the mount can be forced to flip.
        /// This will always return False for mounts (non-German-equatorial) that do not have to be flipped. 
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanSetPierSide
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanSetPierSide;
				else
					return (bool)objTypeScope.InvokeMember("CanSetPierSide", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if the RightAscensionRateproperty can be changed to provide offset tracking in the right ascension axis.
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanSetRightAscensionRate
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanSetRightAscensionRate;
				else
					return (bool)objTypeScope.InvokeMember("CanSetRightAscensionRate", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if the Tracking property can be changed, turning telescope sidereal tracking on and off.
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanSetTracking
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanSetTracking;
				else
					return (bool)objTypeScope.InvokeMember("CanSetTracking", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if this telescope is capable of programmed slewing (synchronous or asynchronous) to equatorial coordinates
        /// If this is true, then only the synchronous equatorial slewing methods are guaranteed to be supported.
        /// See the CanSlewAsync property for the asynchronous slewing capability flag. 
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanSlew
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanSlew;
				else
					return (bool)objTypeScope.InvokeMember("CanSlew", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if this telescope is capable of programmed slewing (synchronous or asynchronous) to local horizontal coordinates
        /// If this is true, then only the synchronous local horizontal slewing methods are guaranteed to be supported.
        /// See the CanSlewAltAzAsync property for the asynchronous slewing capability flag. 
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanSlewAltAz
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanSlewAltAz;
				else
					return (bool)objTypeScope.InvokeMember("CanSlewAltAz", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if this telescope is capable of programmed asynchronous slewing to local horizontal coordinates
        /// This indicates the the asynchronous local horizontal slewing methods are supported.
        /// If this is True, then CanSlewAltAz will also be true. 
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanSlewAltAzAsync
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanSlewAltAzAsync;
				else
					return (bool)objTypeScope.InvokeMember("CanSlewAltAzAsync", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if this telescope is capable of programmed asynchronous slewing to equatorial coordinates.
        /// This indicates the the asynchronous equatorial slewing methods are supported.
        /// If this is True, then CanSlew will also be true.
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanSlewAsync
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanSlewAsync;
				else
					return (bool)objTypeScope.InvokeMember("CanSlewAsync", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if this telescope is capable of programmed synching to equatorial coordinates.
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanSync
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanSync;
				else
					return (bool)objTypeScope.InvokeMember("CanSync", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if this telescope is capable of programmed synching to local horizontal coordinates
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanSyncAltAz
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanSyncAltAz;
				else
					return (bool)objTypeScope.InvokeMember("CanSyncAltAz", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if this telescope is capable of programmed unparking (Unpark() method).
        /// If this is true, then CanPark will also be true. May raise an error if the telescope is not connected.
        /// May raise an error if the telescope is not connected. 
        /// </summary>
        public bool CanUnpark
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.CanUnpark;
				else
					return (bool)objTypeScope.InvokeMember("CanUnpark",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Send a string comand directly to the telescope without expecting response data.
        /// If the optional Raw parameter is set True, the driver must not insert or append any delimiters;
        /// this must send the unmodified raw string directly to the device. 
        /// If the driver cannot support Raw=True, it must raise an error if Raw is set to True. 
        /// If you use this feature of the Telescope driver interface,
        /// your application will be dependent on the low-level protocol used by the particular scope you are connected to.
        /// Thus your application will not work with any arbitrary type of telescope. 
        /// Raises an error if there is a problem communicating with the telescope. 
        /// </summary>
        /// <param name="Command">The command string to be sent to the telescope.</param>
        /// <param name="Raw">Bypass any delimiters or framing around the command (optional, default = False)</param>
        public void CommandBlind(string Command, bool Raw)
        {
			if (ITelescope != null)
				ITelescope.CommandBlind(Command, Raw);
			else
				objTypeScope.InvokeMember("CommandBlind", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objScopeLateBound, new object[] { Command, Raw });
        }

        /// <summary>
        /// Send a string comand to the telescope, returning a true/false response
        /// If the optional Raw parameter is set True, the driver must not insert or append any delimiters;
        /// this must send the unmodified raw string directly to the device.
        /// If the driver cannot support Raw=True, it must raise an error if Raw is set to True. 
        /// If you use this feature of the Telescope driver interface,
        /// your application will be dependent on the low-level protocol
        /// used by the particular scope you are connected to.
        /// Thus your application will not work with any arbitrary type of telescope. 
        /// Raises an error if there is a problem communicating with the telescope. 
        /// It is the responsibility of the driver implementing this interface to translate raw response data to True/False values for return.
        /// If you want to see the raw response string, see CommandString(). 
        /// </summary>
        /// <param name="Command">The command string to be sent to the telescope</param>
        /// <param name="Raw">Bypass any delimiters or framing around the command (optional, default = False)</param>
        /// <returns>True if the response indicated true or success, else False.</returns>
        public bool CommandBool(string Command, bool Raw)
        {
			if (ITelescope != null)
				return ITelescope.CommandBool(Command, Raw);
			else
				return (bool)objTypeScope.InvokeMember("CommandBool", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objScopeLateBound, new object[] { Command, Raw });
        }

       /// <summary>
        /// Send a string comand to the telescope, returning a string response
        /// If the optional Raw parameter is set True, the driver must not insert or append any delimiters;
        /// this must send the unmodified raw string directly to the device.
        /// If the driver cannot support Raw=True, it must raise an error if Raw is set to True. 
        /// If you use this feature of the Telescope driver interface,
        /// your application will be dependent on the low-level protocol
        /// used by the particular scope you are connected to.
        /// Thus your application will not work with any arbitrary type of telescope. 
        /// Raises an error if there is a problem communicating with the telescope. 
        /// It is the responsibility of the driver implementing this interface to translate raw response data to True/False values for return.
        /// </summary>
        /// <param name="Command">The command string to be sent to the telescope</param>
        /// <param name="Raw">Bypass any delimiters or framing around the command (optional, default = False)</param>
        /// <returns>The response data from the telescope resulting from the sent command.</returns>
        public string CommandString(string Command, bool Raw)
        {
			if (ITelescope != null)
				return ITelescope.CommandString(Command, Raw);
			else
				return (string)objTypeScope.InvokeMember("CommandString", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objScopeLateBound, new object[] { Command, Raw });
        }

        /// <summary>
        /// True if telescope connected, False otherwise
        /// Set this property to True to connect to the telescope.
        /// Raises an error if there is a problem connecting. 
        /// Some Telescope properties and methods will raise errors if the scope is not connected. 
        /// In V2, setting the Connected property to True does not automatically unpark the telescope,
        /// nor does it explicitly turn on tracking.
        /// This may affect clients that use the V1 interface.
        /// </summary>
        public bool Connected
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.Connected;
				else
					return (bool)objTypeScope.InvokeMember("Connected", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.Connected = value;
				else
					objTypeScope.InvokeMember("Connected", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });

            }
        }

        /// <summary>
        /// The declination (degrees) of the telescope's current equatorial coordinates, in the coordinate system given by the EquatorialSystem property.
        /// Reading the property will raise an error if the value is unavailable. 
        /// </summary>
        public double Declination
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.Declination;
				else
					return (double)objTypeScope.InvokeMember("Declination", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// The declination tracking rate (arcseconds per second, default = 0.0)
        /// This property, together with RightAscensionRate, provides support for "offset tracking".
        /// Offset tracking is used primarily for tracking objects that move relatively slowly against the equatorial coordinate system.
        /// It also may be used by a software guiding system that controls rates instead of using the PulseGuide() method. 
        /// NOTES:
        /// The property value represents an offset from zero motion. 
        /// If CanSetDeclinationRate is False, this property will always return 0. 
        /// To discover whether this feature is supported, test the CanSetDeclinationRate property. 
        /// The supported range of this property is telescope specific, however, if this feature is supported,
        /// it can be expected that the range is sufficient to allow correction of guiding errors caused by moderate misalignment and periodic error. 
        /// If this property is non-zero when an equatorial slew is initiated, the telescope should continue to update the slew destination coordinates at the given offset rate.
        /// This will allow precise slews to a fast-moving target with a slow-slewing telescope.
        /// When the slew completes, the TargetRightAscension and TargetDeclination properties should reflect the final (adjusted) destination.
        /// This is not a required feature of this specification, however it is desirable. 
        /// </summary>
        public double DeclinationRate
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.DeclinationRate;
				else
					return (double)objTypeScope.InvokeMember("DeclinationRate", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.DeclinationRate = value;
				else
					objTypeScope.InvokeMember("DeclinationRate", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// The long description of the telescope.
        /// This string may contain line endings and may be hundreds to thousands of characters long.
        /// It is intended to display detailed information on the telescope itself.
        /// See the DriverInfo property for descriptive info on the driver itself.
        /// NOTE: this string should not be over 1000 characters in length,
        /// as applications may use popup boxes to display Description.
        /// Older versions of Windows have string length limitations in (e.g.) MessageBox(),
        /// which will cause an application failure if the string is too long. 
        /// </summary>
        public string Description
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.Description;
				else
					return (string)objTypeScope.InvokeMember("Description", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Predict side of pier for German equatorial mounts
        /// </summary>
        /// <param name="RightAscension">The destination right ascension (hours).</param>
        /// <param name="Declination">The destination declination (degrees, positive North).</param>
        /// <returns>The side of the pier on which the telescope would be on if a slew to the given equatorial coordinates is performed at the current instant of time.</returns>
        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
			if (ITelescope != null)
				return ITelescope.DestinationSideOfPier(RightAscension, Declination);
			else
				return (PierSide)objTypeScope.InvokeMember("DestinationSideOfPier", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objScopeLateBound, new object[] {RightAscension, Declination});
        }

        /// <summary>
        /// True if the telescope or driver applies atmospheric refraction to coordinates.
        /// If this property is True, the coordinates sent to, and retrieved from, the telescope are unrefracted. 
        /// NOTES:
        /// If the driver does not know whether the attached telescope does its own refraction,
        /// and if the driver does not itself calculate refraction, this property (if implemented) must raise an error when read. 
        /// Writing to this property is optional. Often, a telescope (or its driver) calculates refraction using standard atmospheric parameters.
        /// If the client wishes to calculate a more accurate refraction,
        /// then this property could be set to False and these client-refracted coordinates used.
        /// If disabling the telescope or driver's refraction is not supported,
        /// the driver must raise an error when an attempt to set this property to False is made. 
        /// Setting this property to True for a telescope or driver that does refraction,
        /// or to False for a telescope or driver that does not do refraction,
        /// shall not raise an error. It shall have no effect. 
        /// </summary>
        public bool DoesRefraction
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.DoesRefraction;
				else
					return (bool)objTypeScope.InvokeMember("DoesRefraction", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.DoesRefraction = value;
				else
					objTypeScope.InvokeMember("DoesRefraction", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM Telescope driver.
        /// This string may contain line endings and may be hundreds to thousands of characters long.
        /// It is intended to display detailed information on the ASCOM driver, including version and copyright data.
        /// See the Description property for descriptive info on the telescope itself.
        /// To get the driver version in a parseable string, use the DriverVersion property.
        /// </summary>
        public string DriverInfo
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.DriverInfo;
				else
					return (string)objTypeScope.InvokeMember("DriverInfo", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// This must be in the form "n.n".
        /// Not to be confused with the InterfaceVersion property, which is the version of this specification supported by the driver (currently 2). 
        /// </summary>
        public string DriverVersion
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.DriverVersion;
				else
					return (string)objTypeScope.InvokeMember("DriverVersion", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Equatorial coordinate system used by this telescope.
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
        /// </summary>
        public EquatorialCoordinateType EquatorialSystem
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.EquatorialSystem;
				else
					return (EquatorialCoordinateType)objTypeScope.InvokeMember("EquatorialSystem",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }
        /// <summary>
        /// Locates the telescope's "home" position (synchronous)
        /// Returns only after the home position has been found.
        /// At this point the AtHome property will be True.
        /// Raises an error if there is a problem. 
        /// Raises an error if AtPark is true. 
        /// </summary>
        public void FindHome()
        {
			if (ITelescope != null)
				ITelescope.FindHome();
			else
				objTypeScope.InvokeMember("FindHome", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objScopeLateBound, new object[] {  });   
        }

        /// <summary>
		/// The telescope's focal length, meters
		/// This property may be used by clients to calculate telescope field of view and plate scale when combined with detector pixel size and geometry. 
        /// </summary>
        public double FocalLength
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.FocalLength;
				else
					return (double)objTypeScope.InvokeMember("FocalLength", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// The current Declination movement rate offset for telescope guiding (degrees/sec)
        /// This is the rate for both hardware/relay guiding and the PulseGuide() method. 
        /// NOTES: 
        /// To discover whether this feature is supported, test the CanSetGuideRates property. 
        /// The supported range of this property is telescope specific, however,
        /// if this feature is supported, it can be expected that the range is sufficient to
        /// allow correction of guiding errors caused by moderate misalignment and periodic error. 
        /// If a telescope does not support separate guiding rates in Right Ascension and Declination,
        /// then it is permissible for GuideRateRightAscension and GuideRateDeclination to be tied together.
        /// In this case, changing one of the two properties will cause a change in the other. 
        /// Mounts must start up with a known or default declination guide rate,
        /// and this property must return that known/default guide rate until changed. 
        /// </summary>
        public double GuideRateDeclination
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.GuideRateDeclination;
				else
					return (double)objTypeScope.InvokeMember("GuideRateDeclination", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.GuideRateDeclination = value;
				else
					objTypeScope.InvokeMember("GuideRateDeclination", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// The current Right Ascension movement rate offset for telescope guiding (degrees/sec)
        /// This is the rate for both hardware/relay guiding and the PulseGuide() method. 
        /// NOTES:
        /// To discover whether this feature is supported, test the CanSetGuideRates property. 
        /// The supported range of this property is telescope specific, however, if this feature is supported, 
        /// it can be expected that the range is sufficient to allow correction of guiding errors caused by moderate
        /// misalignment and periodic error. 
        /// If a telescope does not support separate guiding rates in Right Ascension and Declination,
        /// then it is permissible for GuideRateRightAscension and GuideRateDeclination to be tied together. 
        /// In this case, changing one of the two properties will cause a change in the other. 
        /// Mounts must start up with a known or default right ascension guide rate,
        /// and this property must return that known/default guide rate until changed. 
        /// </summary>
        public double GuideRateRightAscension
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.GuideRateRightAscension;
				else
					return (double)objTypeScope.InvokeMember("GuideRateRightAscension", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.GuideRateRightAscension = value;
				else
					objTypeScope.InvokeMember("GuideRateRightAscension", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// The version of this interface. Will return 2 for this version.
        /// Clients can detect legacy V1 drivers by trying to read ths property.
        /// If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1. 
        /// In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver. 
        /// </summary>
        public short InterfaceVersion
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.InterfaceVersion;
				else
					return (short)objTypeScope.InvokeMember("InterfaceVersion", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }
        /// <summary>
        /// True if a PulseGuide() command is in progress, False otherwise
        /// Raises an error if the value of the CanPulseGuide property is false
        /// (the driver does not support the PulseGuide() method). 
        /// </summary>
        public bool IsPulseGuiding
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.IsPulseGuiding;
				else
					return (bool)objTypeScope.InvokeMember("IsPulseGuiding", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Move the telescope in one axis at the given rate.
        /// This method supports control of the mount about its mechanical axes.
        /// The telescope will start moving at the specified rate about the specified axis and continue indefinitely.
        /// This methos can be called for each axis separately, and have them all operate concurretly at separate rates of motion. 
        /// Set the rate for an axis to zero to stop the motionabout that axis.
        /// Tracking motion (if enabled, see note below) is suspended during this mode of operation. 
        /// Raises an error if AtPark is true. 
        /// This must be implemented for the if the CanMoveAxis property returns True for the given axis. 
        /// Notes: 
        /// The movement rate must be within the value(s) obtained from a Rate object in the the AxisRates collection.
        /// An out of range exception is raised the rate is out of range. 
        /// The value of the Slewing property must be True if the telescope is moving 
        /// about any of its axes as a result of this method being called. 
        /// This can be used to simulate a handbox by initiating motion with the
        /// MouseDown event and stopping the motion with the MouseUp event. 
        /// When the motion is stopped the scope will be set to the previous 
        /// TrackingRate or to no movement, depending on the state of the Tracking property. 
        /// It may be possible to implement satellite tracking by using the MoveAxis()
        /// method to move the scope in the required manner to track a satellite. 
        /// </summary>
        /// <param name="Axis">The physical axis about which movement is desired</param>
        /// <param name="Rate">The rate of motion (deg/sec) about the specified axis</param>
        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
			if (ITelescope != null)
				ITelescope.MoveAxis(Axis, Rate);
			else
				objTypeScope.InvokeMember("MoveAxis",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { Axis, Rate });
		}

        /// <summary>
        /// The short name of the telescope, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.Name;
				else
					return (string)objTypeScope.InvokeMember("Name", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Move the telescope to its park position, stop all motion (or restrict to a small safe range), and set AtPark to True.
        /// Raises an error if there is a problem communicating with the telescope or if parking fails. 
        /// Parking should put the telescope into a state where its pointing accuracy 
        /// will not be lost if it is power-cycled (without moving it).
        /// Some telescopes must be power-cycled before unparking.
        /// Others may be unparked by simply calling the Unpark() method.
        /// Calling this with AtPark = True does nothing (harmless) 
        /// </summary>
        public void Park()
        {
			if (ITelescope != null)
				ITelescope.Park();
			else
				objTypeScope.InvokeMember("Park", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objScopeLateBound, new object[] {  });   
        }

        /// <summary>
        /// Moves the scope in the given direction for the given interval or time at 
        /// the rate given by the corresponding guide rate property 
        /// This method returns immediately if the hardware is capable of back-to-back moves,
        /// i.e. dual-axis moves. For hardware not having the dual-axis capability,
        /// the method returns only after the move has completed. 
        /// Raises an error if AtPark is true. 
        /// The IsPulseGuiding property must be be True during pulse-guiding. 
        /// The rate of motion for movements about the right ascension axis is 
        /// specified by the GuideRateRightAscension property. The rate of motion
        /// for movements about the declination axis is specified by the 
        /// GuideRateDeclination property. These two rates may be tied together
        /// into a single rate, depending on the driver's implementation
        /// and the capabilities of the telescope. 
        /// </summary>
        /// <param name="Direction">The direction in which the guide-rate motion is to be made</param>
        /// <param name="Duration">The duration of the guide-rate motion (milliseconds)</param>
        public void PulseGuide(GuideDirections Direction, int Duration)
        {
			if (ITelescope != null)
				ITelescope.PulseGuide(Direction, Duration);
			else
				objTypeScope.InvokeMember("PulseGuide",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { (int)Direction, Duration });  
        }

        /// <summary>
        /// The right ascension (hours) of the telescope's current equatorial coordinates,
        /// in the coordinate system given by the EquatorialSystem property
        /// Reading the property will raise an error if the value is unavailable. 
        /// </summary>
        public double RightAscension
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.RightAscension;
				else
					return (double)objTypeScope.InvokeMember("RightAscension", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// The right ascension tracking rate offset from sidereal (seconds per sidereal second, default = 0.0)
        /// This property, together with DeclinationRate, provides support for "offset tracking".
        /// Offset tracking is used primarily for tracking objects that move relatively slowly
        /// against the equatorial coordinate system. It also may be used by a software guiding
        /// system that controls rates instead of using the PulseGuide() method.
        /// NOTES:
        /// The property value represents an offset from the current selected TrackingRate. 
        /// If this property is zero, tracking will be at the selected TrackingRate. 
        /// If CanSetRightAscensionRate is False, this property must always return 0. 
        /// To discover whether this feature is supported, test the CanSetRightAscensionRate property. 
        /// The property value is in in seconds of right ascension per sidereal second. 
        /// To convert a given rate in (the more common) units of sidereal seconds
        /// per UTC (clock) second, multiply the value by 0.9972695677 
        /// (the number of UTC seconds in a sidereal second) then set the property.
        /// Please note that these units were chosen for the Telescope V1 standard,
        /// and in retrospect, this was an unfortunate choice.
        /// However, to maintain backwards compatibility, the units cannot be changed.
        /// A simple multiplication is all that's needed, as noted. 
        /// The supported range of this property is telescope specific, however,
        /// if this feature is supported, it can be expected that the range
        /// is sufficient to allow correction of guiding errors
        /// caused by moderate misalignment and periodic error. 
        /// If this property is non-zero when an equatorial slew is initiated,
        /// the telescope should continue to update the slew destination coordinates 
        /// at the given offset rate. This will allow precise slews to a fast-moving 
        /// target with a slow-slewing telescope. When the slew completes, 
        /// the TargetRightAscension and TargetDeclination properties should
        /// reflect the final (adjusted) destination. This is not a required
        /// feature of this specification, however it is desirable. 
        /// 
        /// Use the Tracking property to enable and disable sidereal tracking (if supported). 
        /// </summary>
        public double RightAscensionRate
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.RightAscensionRate;
				else
					return (double)objTypeScope.InvokeMember("RightAscensionRate", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.RightAscensionRate = value;
				else
					objTypeScope.InvokeMember("RightAscensionRate", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetPark()
        {
			if (ITelescope != null)
				ITelescope.SetPark();
			else
				objTypeScope.InvokeMember("SetPark",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { });
		}

        /// <summary>
        /// Sets the telescope's park position to be its current position
        /// Raises an error if there is a problem. 
        /// 
        /// </summary>
        public void SetupDialog()
        {
			if (ITelescope != null)
				ITelescope.SetupDialog();
			else
				objTypeScope.InvokeMember("SetupDialog",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { }); 
        }


        /// <summary>
        /// Indicates which side of the pier a German equatorial mount is currently on
        /// It is allowed (though not required) that this property may be written to
        /// force the mount to flip. Doing so, however, may change the right 
        /// ascension of the telescope. During flipping,
        /// Telescope.Slewing must return True. 
        /// If the telescope is not a German equatorial mount
        /// (Telescope.AlignmentMode is not algGermanPolar), this method will raise an error. 
        /// </summary>
        public PierSide SideOfPier
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.SideOfPier;
				else
					return (PierSide)objTypeScope.InvokeMember("SideOfPier", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.SideOfPier = value;
				else
					objTypeScope.InvokeMember("SideOfPier", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { (int)value });
            }
        }

        /// <summary>
        /// The local apparent sidereal time from the telescope's internal clock (hours, sidereal)
        /// It is required for a driver to calculate this from the system clock if the telescope 
        /// has no accessible source of sidereal time. Local Apparent Sidereal Time is the sidereal 
        /// time used for pointing telescopes, and thus must be calculated from the Greenwich Mean
        /// Sidereal time, longitude, nutation in longitude and true ecliptic obliquity. 
        /// </summary>
        public double SiderealTime
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.SiderealTime;
				else
					return (double)objTypeScope.InvokeMember("SiderealTime", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }
        
        /// <summary>
        /// The elevation above mean sea level (meters) of the site at which the telescope is located
        /// Setting this property will raise an error if the given value is outside the range -300 through +10000 metres.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable. 
        /// </summary>
        public double SiteElevation
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.SiteElevation;
				else
					return (double)objTypeScope.InvokeMember("SiteElevation", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.SiteElevation = value;
				else
					objTypeScope.InvokeMember("SiteElevation", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// The geodetic(map) latitude (degrees, positive North, WGS84) of the site at which the telescope is located.
        /// Setting this property will raise an error if the given value is outside the range -90 to +90 degrees.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable. 
        /// </summary>
        public double SiteLatitude
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.SiteLatitude;
				else
					return (double)objTypeScope.InvokeMember("SiteLatitude", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.SiteLatitude = value;
				else
					objTypeScope.InvokeMember("SiteLatitude", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// The longitude (degrees, positive East, WGS84) of the site at which the telescope is located.
        /// Setting this property will raise an error if the given value is outside the range -180 to +180 degrees.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable.
        /// Note that West is negative! 
        /// </summary>
        public double SiteLongitude
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.SiteLongitude;
				else
					return (double)objTypeScope.InvokeMember("SiteLongitude", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.SiteLongitude = value;
				else
					objTypeScope.InvokeMember("SiteLongitude", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Specifies a post-slew settling time (sec.).
        /// Adds additional time to slew operations. Slewing methods will not return, 
        /// and the Slewing property will not become False, until the slew completes and the SlewSettleTime has elapsed.
        /// This feature (if supported) may be used with mounts that require extra settling time after a slew. 
        /// </summary>
        public short SlewSettleTime
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.SlewSettleTime;
				else
					return (short)objTypeScope.InvokeMember("SlewSettleTime", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.SlewSettleTime = value;
				else
					objTypeScope.InvokeMember("SlewSettleTime", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Move the telescope to the given local horizontal coordinates, return when slew is complete
        /// This Method must be implemented if CanSlewAltAz returns True.
        /// Raises an error if the slew fails. 
        /// The slew may fail if the target coordinates are beyond limits imposed within the driver component.
        /// Such limits include mechanical constraints imposed by the mount or attached instruments,
        /// building or dome enclosure restrictions, etc. 
        /// The TargetRightAscension and TargetDeclination properties are not changed by this method. 
        /// Raises an error if AtPark is True, or if Tracking is True. 
        /// </summary>
        /// <param name="Azimuth">Target azimuth (degrees, North-referenced, positive East/clockwise).</param>
        /// <param name="Altitude">Target altitude (degrees, positive up)</param>
        public void SlewToAltAz(double Azimuth, double Altitude)
        {
			if (ITelescope != null)
				ITelescope.SlewToAltAz(Azimuth, Altitude);
			else
				objTypeScope.InvokeMember("SlewToAltAz",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { Azimuth, Altitude });
        }

        /// <summary>
        /// This Method must be implemented if CanSlewAltAzAsync returns True.
        /// This method should only be implemented if the properties Altitude, Azimuth,
        /// Right Ascension, Declination and Slewing can be read while the scope is slewing.
        /// Raises an error if starting the slew fails. Returns immediately after starting the slew.
        /// The client may monitor the progress of the slew by reading the Azimuth, Altitude,
        /// and Slewing properties during the slew. When the slew completes, Slewing becomes False. 
        /// The slew may fail if the target coordinates are beyond limits imposed within the driver component.
        /// Such limits include mechanical constraints imposed by the mount or attached instruments,
        /// building or dome enclosure restrictions, etc. 
        /// The TargetRightAscension and TargetDeclination properties are not changed by this method. 
        /// Raises an error if AtPark is True, or if Tracking is True. 
        /// </summary>
        /// <param name="Azimuth"></param>
        /// <param name="Altitude"></param>
        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
			if (ITelescope != null)
				ITelescope.SlewToAltAzAsync(Azimuth, Altitude);
			else
				objTypeScope.InvokeMember("SlewToAltAzAsync",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { Azimuth, Altitude });
        }

        /// <summary>
        /// Move the telescope to the given equatorial coordinates, return when slew is complete
        /// This Method must be implemented if CanSlew returns True. Raises an error if the slew fails. 
        /// The slew may fail if the target coordinates are beyond limits imposed within the driver component.
        /// Such limits include mechanical constraints imposed by the mount or attached instruments,
        /// building or dome enclosure restrictions, etc. The target coordinates are copied to
        /// Telescope.TargetRightAscension and Telescope.TargetDeclination whether or not the slew succeeds. 
        /// Raises an error if AtPark is True, or if Tracking is False. 
        /// </summary>
        /// <param name="RightAscension">The destination right ascension (hours). Copied to Telescope.TargetRightAscension.</param>
        /// <param name="Declination">The destination declination (degrees, positive North). Copied to Telescope.TargetDeclination.</param>
        public void SlewToCoordinates(double RightAscension, double Declination)
        {
			if (ITelescope != null)
				ITelescope.SlewToCoordinates(RightAscension, Declination);
			else
				objTypeScope.InvokeMember("SlewToCoordinates",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { RightAscension, Declination });
        }

        /// <summary>
        /// Move the telescope to the given equatorial coordinates, return immediately after starting the slew.
        /// This Method must be implemented if CanSlewAsync returns True. Raises an error if starting the slew failed. 
        /// Returns immediately after starting the slew. The client may monitor the progress of the slew by reading
        /// the RightAscension, Declination, and Slewing properties during the slew. When the slew completes,
        /// Slewing becomes False. The slew may fail to start if the target coordinates are beyond limits
        /// imposed within the driver component. Such limits include mechanical constraints imposed
        /// by the mount or attached instruments, building or dome enclosure restrictions, etc. 
        /// The target coordinates are copied to TargetRightAscension and TargetDeclination
        /// whether or not the slew succeeds. 
        /// Raises an error if AtPark is True, or if Tracking is False. 
        /// </summary>
        /// <param name="RightAscension">The destination right ascension (hours). Copied to Telescope.TargetRightAscension.</param>
        /// <param name="Declination">The destination declination (degrees, positive North). Copied to Telescope.TargetDeclination.</param>
        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
			if (ITelescope != null)
				ITelescope.SlewToCoordinatesAsync(RightAscension, Declination);
			else
				objTypeScope.InvokeMember("SlewToCoordinatesAsync",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { RightAscension, Declination });
        }

        /// <summary>
        /// Move the telescope to the TargetRightAscension and TargetDeclination coordinates, return when slew complete.
        /// This Method must be implemented if CanSlew returns True. Raises an error if the slew fails. 
        /// The slew may fail if the target coordinates are beyond limits imposed within the driver component.
        /// Such limits include mechanical constraints imposed by the mount or attached
        /// instruments, building or dome enclosure restrictions, etc. 
        /// Raises an error if AtPark is True, or if Tracking is False. 
        /// </summary>
        public void SlewToTarget()
        {
			if (ITelescope != null)
				ITelescope.SlewToTarget();
			else
				objTypeScope.InvokeMember("SlewToTarget",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { });
        }

        /// <summary>
        /// Move the telescope to the TargetRightAscension and TargetDeclination coordinates,
        /// returns immediately after starting the slew.
        /// This Method must be implemented if CanSlewAsync returns True.
        /// Raises an error if starting the slew failed. 
        /// Returns immediately after starting the slew. The client may
        /// monitor the progress of the slew by reading the RightAscension, Declination,
        /// and Slewing properties during the slew. When the slew completes, Slewing becomes False. 
        /// The slew may fail to start if the target coordinates are beyond limits imposed within 
        /// the driver component. Such limits include mechanical constraints imposed by the mount
        /// or attached instruments, building or dome enclosure restrictions, etc. 
        /// Raises an error if AtPark is True, or if Tracking is False. 
        /// </summary>
        public void SlewToTargetAsync()
        {
			if (ITelescope != null)
				ITelescope.SlewToTargetAsync();
			else
				objTypeScope.InvokeMember("SlewToTargetAsync",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { });
        }

        /// <summary>
        /// True if telescope is currently moving in response to one of the
        /// Slew methods or the MoveAxis() method, False at all other times.
        /// Reading the property will raise an error if the value is unavailable.
        /// If the telescope is not capable of asynchronous slewing,
        /// this property will always be False. 
        /// The definition of "slewing" excludes motion caused by sidereal tracking,
        /// PulseGuide(), RightAscensionRate, and DeclinationRate.
        /// It reflects only motion caused by one of the Slew commands, 
        /// flipping caused by changing the SideOfPier property, or MoveAxis(). 
        /// </summary>
        public bool Slewing
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.Slewing;
				else
					return (bool)objTypeScope.InvokeMember("Slewing", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Matches the scope's local horizontal coordinates to the given local horizontal coordinates.
        /// This must be implemented if the CanSyncAltAz property is True. Raises an error if matching fails. 
        /// Raises an error if AtPark is True, or if Tracking is True. 
        /// </summary>
        /// <param name="Azimuth">Target azimuth (degrees, North-referenced, positive East/clockwise)</param>
        /// <param name="Altitude">Target altitude (degrees, positive up)</param>
        public void SyncToAltAz(double Azimuth, double Altitude)
        {
			if (ITelescope != null)
				ITelescope.SyncToAltAz(Azimuth, Altitude);
			else
				objTypeScope.InvokeMember("SyncToAltAz",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { Azimuth, Altitude });
        }

        /// <summary>
        /// Matches the scope's equatorial coordinates to the given equatorial coordinates.
        /// </summary>
        /// <param name="RightAscension">The corrected right ascension (hours). Copied to the TargetRightAscension property.</param>
        /// <param name="Declination">The corrected declination (degrees, positive North). Copied to the TargetDeclination property.</param>
        public void SyncToCoordinates(double RightAscension, double Declination)
        {
			if (ITelescope != null)
				ITelescope.SyncToCoordinates(RightAscension, Declination);
			else
				objTypeScope.InvokeMember("SyncToCoordinates",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { RightAscension, Declination });
        }

        /// <summary>
        /// Matches the scope's equatorial coordinates to the given equatorial coordinates.
        /// This must be implemented if the CanSync property is True. Raises an error if matching fails. 
        /// Raises an error if AtPark is True, or if Tracking is False. 
        /// </summary>
        public void SyncToTarget()
        {
			if (ITelescope != null)
				ITelescope.SyncToTarget();
			else
				objTypeScope.InvokeMember("SyncToTarget",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { });
        }

        /// <summary>
        /// The declination (degrees, positive North) for the target of an equatorial slew or sync operation
        /// Setting this property will raise an error if the given value is outside the range -90 to +90 degrees.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable. 
        /// </summary>
        public double TargetDeclination
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.TargetDeclination;
				else
					return (double)objTypeScope.InvokeMember("TargetDeclination", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.TargetDeclination = value;
				else
					objTypeScope.InvokeMember("TargetDeclination", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// The right ascension (hours) for the target of an equatorial slew or sync operation
        /// Setting this property will raise an error if the given value is outside the range 0 to 24 hours.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable. 
        /// </summary>
        public double TargetRightAscension
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.TargetRightAscension;
				else
					return (double)objTypeScope.InvokeMember("TargetRightAscension", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.TargetRightAscension = value;
				else
					objTypeScope.InvokeMember("TargetRightAscension", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// The state of the telescope's sidereal tracking drive.
        /// Changing the value of this property will turn the sidereal drive on and off.
        /// However, some telescopes may not support changing the value of this property
        /// and thus may not support turning tracking on and off.
        /// See the CanSetTracking property. 
        /// </summary>
        public bool Tracking
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.Tracking;
				else
					return (bool)objTypeScope.InvokeMember("Tracking", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.Tracking = value;
				else
					objTypeScope.InvokeMember("Tracking", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// The current tracking rate of the telescope's sidereal drive
        /// Supported rates (one of the DriveRates values) are contained within the TrackingRates collection.
        /// Values assigned to TrackingRate must be one of these supported rates. 
        /// If an unsupported value is assigned to this property, it will raise an error. 
        /// The currently selected tracking rate be further adjusted via the RightAscensionRate 
        /// and DeclinationRate properties. These rate offsets are applied to the currently 
        /// selected TrackingRate. Mounts must start up with a known or default tracking rate,
        /// and this property must return that known/default tracking rate until changed.
        /// If the mount's current tracking rate cannot be determined (for example, 
        /// it is a write-only property of the mount's protocol), 
        /// it is permitted for the driver to force and report a default rate on connect.
        /// In this case, the preferred default is Sidereal rate. 
        /// </summary>
        public DriveRates TrackingRate
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.TrackingRate;
				else
					return (DriveRates)objTypeScope.InvokeMember("TrackingRate", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.TrackingRate = value;
				else
					objTypeScope.InvokeMember("TrackingRate", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Returns a collection of supported DriveRate values that describe the permissible
        /// values of the TrackingRate property for this telescope type.
        /// At a minimum, this must contain an item for driveSidereal. 
        /// </summary>
        public ITrackingRates TrackingRates
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.TrackingRates;
				else
					return new _TrackingRates(objTypeScope, objScopeLateBound);
            }

        }

        /// <summary>
        /// The UTC date/time of the telescope's internal clock
        /// The driver must calculate this from the system clock if the telescope has no accessible
        /// source of UTC time. In this case, the property must not be writeable 
        /// (this would change the system clock!) and will instead raise an error.
        /// However, it is permitted to change the telescope's internal UTC clock 
        /// if it is being used for this property. This allows clients to adjust 
        /// the telescope's UTC clock as needed for accuracy. Reading the property
        /// will raise an error if the value has never been set or is otherwise unavailable. 
        /// </summary>
        public DateTime UTCDate
        {
            get
            {
				if (ITelescope != null)
					return ITelescope.UTCDate;
				else
					return (DateTime)objTypeScope.InvokeMember("UTCDate", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
            }
            set
            {
				if (ITelescope != null)
					ITelescope.UTCDate = value;
				else
					objTypeScope.InvokeMember("UTCDate", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objScopeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Takes telescope out of the Parked state.
        /// The state of Tracking after unparking is undetermined. 
        /// Valid only after Park().
        /// Applications must check and change Tracking as needed after unparking. 
        /// Raises an error if unparking fails. Calling this with AtPark = False does nothing (harmless) 
        /// </summary>
        public void Unpark()
        {
			if (ITelescope != null)
				ITelescope.Unpark();
			else
				objTypeScope.InvokeMember("Unpark",
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objScopeLateBound, new object[] { });
        }

        #endregion

        #region IDisposable Members
		/// <summary>
		/// Dispose the late-bound interface, if needed. Will release it via COM
		/// if it is a COM object, else if native .NET will just dereference it
		/// for GC.
		/// </summary>
		public void Dispose()
        {
            if (this.objScopeLateBound != null)
            {
				try { Marshal.ReleaseComObject(objScopeLateBound); }
				catch (Exception) { }
				objScopeLateBound = null;
            }
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

		public _Rate(int index, Type objTypeAxisRates, object objAxisRatesLateBound)
		{
			objRateLateBound = objTypeAxisRates.InvokeMember("Item",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objAxisRatesLateBound, new object[] { index });
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
	//
	// Strongly typed enumerator for late bound Rate
	// objects being enumarated
	//
	class _RateEnumerator : IEnumerator, IDisposable
	{
		IEnumerator objEnumerator;
		Type objTypeAxisRates;
		object objAxisRatesLateBound;

		public _RateEnumerator(Type objTypeAxisRates, object objAxisRatesLateBound)
		{
			this.objTypeAxisRates = objTypeAxisRates;
			this.objAxisRatesLateBound = objAxisRatesLateBound;
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

	//
	// Late bound Axis Rates implementation.
	//
	class _AxisRates : IAxisRates
	{
		Type objTypeAxisRates;
		object objAxisRatesLateBound;

		public _AxisRates(TelescopeAxes Axis, Type objTypeScope, object objScopeLateBound)
		{
			objAxisRatesLateBound = objTypeScope.InvokeMember("AxisRates",
						BindingFlags.Default | BindingFlags.InvokeMethod,
						null, objScopeLateBound, new object[] { (int)Axis });
			objTypeAxisRates = objAxisRatesLateBound.GetType();
		}

		public IRate this[int index]
		{
			get 
			{
				return new _Rate(index, objTypeAxisRates, objAxisRatesLateBound);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return new _RateEnumerator(objTypeAxisRates, objAxisRatesLateBound);
		}

		public int Count
		{
			get 
			{
				return (int)objTypeAxisRates.InvokeMember("Count",
							BindingFlags.Default | BindingFlags.GetProperty,
							null, objAxisRatesLateBound, new object[] { });
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

	//
	// Late bound TrackingRates implementation
	//
	class _TrackingRates : ITrackingRates
	{
		Type objTypeTrackingRates;
		object objTrackingRatesLateBound;

		public _TrackingRates(Type objTypeScope, object objScopeLateBound)
		{
			objTrackingRatesLateBound = objTypeScope.InvokeMember("TrackingRates",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objScopeLateBound, new object[] { });
			objTypeTrackingRates = objTrackingRatesLateBound.GetType();
		}

		public DriveRates this[int index]
		{
			get 
			{
				return (DriveRates)objTypeTrackingRates.InvokeMember("Item",
							BindingFlags.Default | BindingFlags.GetProperty,
							null, objTrackingRatesLateBound, new object[] { index });
			}
		}

		public IEnumerator GetEnumerator()
		{
			return (IEnumerator)objTypeTrackingRates.InvokeMember("GetEnumerator",
						BindingFlags.Default | BindingFlags.InvokeMethod,
						null, objTrackingRatesLateBound, new object[] { });
		}

		public int Count
		{
			get 
			{
				return (int)objTypeTrackingRates.InvokeMember("Count",
							BindingFlags.Default | BindingFlags.GetProperty,
							null, objTrackingRatesLateBound, new object[] { });
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (this.objTrackingRatesLateBound != null)
			{
				objTrackingRatesLateBound = null;
			}
		}

		#endregion
	}
	#endregion
}
