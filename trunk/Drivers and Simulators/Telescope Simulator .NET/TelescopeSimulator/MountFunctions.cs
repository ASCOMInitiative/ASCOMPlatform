using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ASCOM.Simulator
{
    /// <summary>
    /// These functions convert between the axis positions and Ra/dec aand Alt Azm
    /// axis.X is primary - Ra or Azimuth, axis.Y is dec or altitude
    ///Alt Az scopes have the axis and AltAz coordinates parallel
    ///Polar mounts have the Hour angle and Dec axes paarallel with axis.X and axis.Y
    ///The secondary (dec) axis increases to the local zenith so for the Southern hemisphere dec = -axis.Y
    /// </summary>
    internal static class MountFunctions
    {

        internal static Vector ConvertRaDecToAxes(Vector raDec)
        {
            Vector axes = new Vector();
            switch (TelescopeHardware.AlignmentMode)
	        {
		        case ASCOM.DeviceInterface.AlignmentModes.algAltAz:
                    axes = AstronomyFunctions.CalculateAltAzm(raDec.X, raDec.Y, TelescopeHardware.Latitude);
                    break;
                case ASCOM.DeviceInterface.AlignmentModes.algGermanPolar:
                    axes.X = (TelescopeHardware.SiderealTime - raDec.X) * 15.0;
                    axes.Y = (TelescopeHardware.Latitude >= 0) ? raDec.Y : -raDec.Y;
                    axes.X = RangeAzm(axes.X);
                    if (axes.X > 180.0 || axes.X < 0)
                    {
                        // adjust the targets to be through the pole
                        axes.X += 180;
                        axes.Y = 180 - axes.Y;
                    }
                    break;
                case ASCOM.DeviceInterface.AlignmentModes.algPolar:
                    axes.X = (TelescopeHardware.SiderealTime - raDec.X) * 15.0;
                    axes.Y = (TelescopeHardware.Latitude >= 0) ? raDec.Y : -raDec.Y;
                    break;
	        }
            return RangeAxes(axes);
        }

        internal static Vector ConvertAltAzmToAxes(Vector altAz)
        {
            Vector axes = altAz;
            switch (TelescopeHardware.AlignmentMode)
	        {
		        case ASCOM.DeviceInterface.AlignmentModes.algAltAz:
                    break;
                case ASCOM.DeviceInterface.AlignmentModes.algGermanPolar:
                    axes = AstronomyFunctions.CalculateRaDec(altAz, TelescopeHardware.Latitude, TelescopeHardware.Longitude);
                    if (TelescopeHardware.Latitude < 0)
                    {
                        axes.Y = -axes.Y;
                    }
                    axes = RangeAltAzm(axes);
                    if (axes.X > 180.0 || axes.X < 0)
                    {
                        // adjust the targets to be through the pole
                        axes.X += 180;
                        axes.Y = 180 - axes.Y;
                    }
                    break;
                case ASCOM.DeviceInterface.AlignmentModes.algPolar:
                    axes = AstronomyFunctions.CalculateRaDec(altAz, TelescopeHardware.Latitude, TelescopeHardware.Longitude);
                    if (TelescopeHardware.Latitude < 0)
                    {
                        axes.Y = -axes.Y;
                    }
                    axes = RangeAltAzm(axes);
                    break;
	        }
            return RangeAxes(axes);
        }

        internal static Vector ConvertAxesToRaDec(Vector axes)
        {
            Vector raDec = new Vector();
            switch (TelescopeHardware.AlignmentMode)
	        {
		        case ASCOM.DeviceInterface.AlignmentModes.algAltAz:
                    raDec = AstronomyFunctions.CalculateRaDec(axes, TelescopeHardware.Latitude, TelescopeHardware.Longitude);
                    break;
                case ASCOM.DeviceInterface.AlignmentModes.algGermanPolar:
                case ASCOM.DeviceInterface.AlignmentModes.algPolar:
                    // undo through the pole
                    if (axes.Y > 90)
                    {
                        axes.X += 180.0;
                        axes.Y = 180 - axes.Y;
                        axes = RangeAltAzm(axes);
                    }
                    raDec.X = TelescopeHardware.SiderealTime - axes.X / 15.0;
                    raDec.Y = (TelescopeHardware.Latitude >= 0) ? axes.Y : -axes.Y;
                    break;
	        }
            return RangeRaDec(raDec);
        }

        internal static Vector ConvertAxesToAltAzm(Vector axes)
        {
            Vector altAzm = axes;
            switch (TelescopeHardware.AlignmentMode)
	        {
		        case ASCOM.DeviceInterface.AlignmentModes.algAltAz:
                    break;
                case ASCOM.DeviceInterface.AlignmentModes.algGermanPolar:
                    if (axes.Y > 90)
                    {
                        axes.X += 180;
                        axes.Y = 180 - axes.Y;
                    }
                    if (TelescopeHardware.Latitude < 0)
                    {
                        axes.Y = -axes.Y;
                    }
                    var ra = TelescopeHardware.SiderealTime - axes.X / 15.0;
                    altAzm = AstronomyFunctions.CalculateAltAzm(ra, axes.Y, TelescopeHardware.Latitude);
                    break;
                case ASCOM.DeviceInterface.AlignmentModes.algPolar:
                    ra = TelescopeHardware.SiderealTime - axes.X / 15.0;
                    if (TelescopeHardware.Latitude < 0)
                    {
                        axes.Y = -axes.Y;
                    }
                    altAzm = AstronomyFunctions.CalculateAltAzm(ra, axes.Y, TelescopeHardware.Latitude);
                    break;
	        }
            return RangeAltAzm(altAzm);
        }

        /// <summary>
        /// forces a ra dec value to the range 0 to 24.0 and -90 to 90
        /// </summary>
        /// <param name="raDec">The ra dec.</param>
        private static Vector RangeRaDec(Vector raDec)
        {
            return new Vector(RangeHa(raDec.X), RangeDec(raDec.Y));
        }

        /// <summary>
        /// forces an altz value the the range 0 to 360 for azimuth and -90 to 90 for altitude
        /// </summary>
        /// <param name="altAzm"></param>
        private static Vector RangeAltAzm(Vector altAzm)
        {
            return new Vector(RangeAzm(altAzm.X), RangeDec(altAzm.Y));
        }

        /// <summary>
        /// forces axis values to the range 0 to 360 and -90 to 270
        /// </summary>
        /// <param name="axes"></param>
        private static Vector RangeAxes(Vector axes)
        {
            return new Vector(RangeAzm(axes.X), RangeDecx(axes.Y));
        }

        /// <summary>
        ///forces the azm to be in the range 0 to 360
        /// </summary>
        /// <param name="azm">The azm.</param>
        /// <returns></returns>
        private static double RangeAzm(double azm)
        {
            while ((azm >= 360.0) || (azm < 0.0))
            {
                if (azm < 0.0) azm += 360.0;
                if (azm >= 360.0) azm -= 360.0;
            }
            return azm;
        }

        /// <summary>
        /// Forces the dec to be in the range -90 to 0 to 90 to 180 to 270.
        /// </summary>
        /// <param name="dec">The dec in degrees.</param>
        /// <returns></returns>
        internal static double RangeDecx(double dec)
        {
            while ((dec >= 270) || (dec < -90))
            {
                if (dec < -90) dec += 360.0;
                if (dec >= 270) dec -= 360.0;
            }
            return dec;
        }

        /// <summary>
        /// forces the Dec to the range -90 to 0 to +90
        /// </summary>
        /// <param name="dec">The dec.</param>
        /// <returns></returns>
        private static double RangeDec(double dec)
        {
            while ((dec > 90.0) || (dec < -90.0))
            {
                if (dec < -90.0) dec += 180.0;
                if (dec > 90.0) dec = 180.0 - dec;
            }
            return dec;
        }

        /// <summary>
        /// forces the ha/ra value to the range 0 to 24
        /// </summary>
        /// <param name="ha">The ha.</param>
        /// <returns></returns>
        private static double RangeHa(double ha)
        {
            while ((ha >= 24.0) || (ha < 0.0))
            {
                if (ha < 0.0) ha += 24.0;
                if (ha >= 24.0) ha -= 24.0;
            }
            return ha;
        }
    }
}
