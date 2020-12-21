using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    // tabs=4
    // --------------------------------------------------------------------------------
    // <summary>
    // ASCOM.Interface Telescope Enumerations
    // </summary>
    // 
    // <copyright company="The ASCOM Initiative" author="Timothy P. Long">
    // Copyright © ASCOM Initiative 2021
    // </copyright>
    // 
    // <license>
    // Permission is hereby granted, free of charge, to any person obtaining
    // a copy of this software and associated documentation files (the
    // "Software"), to deal in the Software without restriction, including
    // without limitation the rights to use, copy, modify, merge, publish,
    // distribute, sublicense, and/or sell copies of the Software, and to
    // permit persons to whom the Software is furnished to do so, subject to
    // the following conditions:
    // 
    // The above copyright notice and this permission notice shall be
    // included in all copies or substantial portions of the Software.
    // 
    // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    // EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    // MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    // NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    // LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    // OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    // WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    // </license>
    // 
    // 
    // Defines:	ITelescope interfaces
    // Author:		(TPL) Timothy P. Long <Tim@tigranetworks.co.uk>
    // 
    // Edit Log:
    // 
    // Date			Who	Vers	Description
    // -----------	---	-----	-------------------------------------------------------
    // 10-Feb-2010	TPL	6.0.*	Initial edit.
    // --------------------------------------------------------------------------------

    /// <summary>
    /// Describes the state of a telescope cover
    /// </summary>
    /// <remarks>This has an "Unknown" state because the device may not be able to determine the state of the hardware at power up if it doesn't provide this feedback and needs 
    /// to be commanded into a known state before use.</remarks>
    [Guid("99A6EBE8-52E5-4E83-BB53-070E4431F4E5")]
    [ComVisible(true)]
    public enum CoverStatus
    {
        /// <summary>
        /// This device does not have a cover that can be closed independently 
        /// </summary>
        NotPresent = 0,

        /// <summary>
        /// The cover is closed
        /// </summary>
        Closed = 1,

        /// <summary>
        /// The cover is moving to a new position
        /// </summary>
        Moving = 2,

        /// <summary>
        /// The cover is open
        /// </summary>
        Open = 3,

        /// <summary>
        /// The state of the cover is unknown
        /// </summary>
        Unknown = 4,

        /// <summary>
        /// The device encountered an error when changing state
        /// </summary>
        Error = 5
    }

    /// <summary>
    /// Describes the state of a calibration device
    /// </summary>
    /// <remarks>This has an "Unknown" state because the device may not be able to determine the state of the hardware at power up if it doesn't provide this feedback and needs 
    /// to be commanded into a known state before use.</remarks>
    [Guid("76B3BCA9-F8EE-4839-A53F-E56CC360BCD7")]
    [ComVisible(true)]
    public enum CalibratorStatus
    {
        /// <summary>
        /// This device does not have a calibration capability
        /// </summary>
        NotPresent = 0,

        /// <summary>
        /// The calibrator is off
        /// </summary>
        Off = 1,

        /// <summary>
        /// The calibrator is stabilising or is not yet in the commanded state
        /// </summary>
        NotReady = 2,

        /// <summary>
        /// The calibrator is ready for use
        /// </summary>
        Ready = 3,

        /// <summary>
        /// The calibrator state is unknown
        /// </summary>
        Unknown = 4,

        /// <summary>
        /// The calibrator encountered an error when changing state
        /// </summary>
        Error = 5
    }

    /// <summary>
    /// The alignment mode of the mount.
    /// </summary>
    [Guid("B0C32247-9CF4-47bd-A5E4-FD430065CB4A")]
    [ComVisible(true)]
    public enum AlignmentModes
    {
        /// <summary>
        /// Altitude-Azimuth alignment.
        /// </summary>
        algAltAz,

        /// <summary>
        /// Polar (equatorial) mount other than German equatorial.
        /// </summary>
        algPolar,

        /// <summary>
        /// German equatorial mount.
        /// </summary>
        algGermanPolar
    }

    /// <summary>
    /// Well-known telescope tracking rates.
    /// </summary>
    [Guid("3FA761EC-12F4-4757-8637-F0ABE5ECB9F7")]
    [ComVisible(true)]
    public enum DriveRates
    {
        /// <summary>
        /// Sidereal tracking rate (15.041 arcseconds per second).
        /// </summary>
        driveSidereal,

        /// <summary>
        /// Lunar tracking rate (14.685 arcseconds per second).
        /// </summary>
        driveLunar,

        /// <summary>
        /// Solar tracking rate (15.0 arcseconds per second).
        /// </summary>
        driveSolar,

        /// <summary>
        /// King tracking rate (15.0369 arcseconds per second).
        /// </summary>
        driveKing
    }

    /// <summary>
    /// Equatorial coordinate systems used by telescopes.
    /// Only used with telescope interface versions 2 and 3
    /// </summary>
    /// <remarks>
    /// In June 2018 the name equLocalTopocentric was deprecated in favour of equTopocentric, both names return the same value (1).
    /// The rationale for this change is set out in the <conceptualLink target="72A95B28-BBE2-4C7D-BC03-2D6AB324B6F7">Astronomical Coordinates</conceptualLink> section.
    /// </remarks>
    [Guid("46AB7149-B2AF-4160-A2FD-17B8923CBADE")]
    [ComVisible(true)]
    public enum EquatorialCoordinateType
    {
        /// <summary>
        /// Custom or unknown equinox and/or reference frame.
        /// </summary>
        equOther = 0,

        /// <summary>
        /// Topocentric coordinates. Coordinates of the object at the current date having allowed for annual aberration, precession and nutation. This is the most common coordinate type for amateur telescopes.
        /// </summary>
        equTopocentric = 1,

        /// <summary>
        /// J2000 equator/equinox. Coordinates of the object at mid-day on 1st January 2000, ICRS reference frame.
        /// </summary>
        equJ2000 = 2,

        /// <summary>
        /// J2050 equator/equinox, ICRS reference frame.
        /// </summary>
        equJ2050 = 3,

        /// <summary>
        /// B1950 equinox, FK4 reference frame.
        /// </summary>
        equB1950 = 4,

        // This entry has been moved to the end of the enum rather than leaving it in the middle so that anyone who is extractiung values from the enum,
        // based on position within the list, will continue to receive the values they are expecting.
        /// <summary>
        /// Please use equTopocentric instead - see <conceptualLink target="72A95B28-BBE2-4C7D-BC03-2D6AB324B6F7">Astronomical Coordinates</conceptualLink> for an explanation.
        /// </summary>
        [Obsolete("Please use equTopocentric instead. (See Astronomical Coordinates page in Developer's Help for further information)")]
        equLocalTopocentric = 1
    }

    /// <summary>
    /// The direction in which the guide-rate motion is to be made.
    /// </summary>
    [Guid("BF98641E-FC63-451d-9310-63EFEFA1B28B")]
    [ComVisible(true)]
    public enum GuideDirections
    {
        /// <summary>
        /// North (+ declination/altitude).
        /// </summary>
        guideNorth,

        /// <summary>
        /// South (- declination/altitude).
        /// </summary>
        guideSouth,

        /// <summary>
        /// East (+ right ascension/azimuth).
        /// </summary>
        guideEast,

        /// <summary>
        /// West (- right ascension/azimuth)
        /// </summary>
        guideWest
    }

    /// <summary>
    /// The telescope axes
    /// Only used with if the telescope interface version is 2 or 3
    /// </summary>
    [Guid("26F6BD6C-5289-4aa1-B270-F3EA0EBEAFD7")]
    [ComVisible(true)]
    public enum TelescopeAxes
    {
        /// <summary>
        /// Primary axis (e.g., Right Ascension or Azimuth).
        /// </summary>
        axisPrimary,

        /// <summary>
        /// Secondary axis (e.g., Declination or Altitude).
        /// </summary>
        axisSecondary,

        /// <summary>
        /// Tertiary axis (e.g. imager rotator/de-rotator).
        /// </summary>
        axisTertiary
    }

    /// <summary>
    /// The pointing state of the mount
    /// </summary>
    /// <remarks>
    /// <para><c>Pier side</c> is a GEM-specific term that has historically caused much confusion.
    /// As of Platform 6, the PierSide property is defined to refer to the telescope pointing state. Please see <see cref="ITelescopeV3.SideOfPier" /> for
    /// much more information on this topic.</para>
    /// <para>In order to support Dome slaving, where it is important to know on which side of the pier the mount is actually located, ASCOM has adopted the
    /// convention that the Normal pointing state will be the state where the mount is on the East side of the pier, looking West with the counterweights below
    /// the optical assembly.</para>
    /// <para>Only used with telescope interface versions 2 and later.</para>
    /// </remarks>
    [Guid("6F0E1F45-129A-4c3a-A3B0-3611AEDB33FB")]
    [ComVisible(true)]
    public enum PierSide
    {
        /// <summary>
        /// Normal pointing state - Mount on the East side of pier (looking West)
        /// </summary>
        pierEast = 0,

        /// <summary>
        /// Unknown or indeterminate.
        /// </summary>
        pierUnknown = -1,

        /// <summary>
        /// Through the pole pointing state - Mount on the West side of pier (looking East)
        /// </summary>
        pierWest = 1
    }

    /// <summary>
    /// ASCOM Dome ShutterState status values.
    /// </summary>
    [Guid("DA182F18-4133-4d6f-A533-67306F48AC5C")]
    [ComVisible(true)]
    public enum ShutterState
    {
        /// <summary>
        /// Dome shutter status open
        /// </summary>
        shutterOpen = 0,
        /// <summary>
        /// Dome shutter status closed
        /// </summary>
        shutterClosed = 1,
        /// <summary>
        /// Dome shutter status opening
        /// </summary>
        shutterOpening = 2,
        /// <summary>
        /// Dome shutter status closing
        /// </summary>
        shutterClosing = 3,
        /// <summary>
        /// Dome shutter status error
        /// </summary>
        shutterError = 4
    }

    /// <summary>
    /// ASCOM Camera status values.
    /// </summary>
    [Guid("BBD1CD3C-5983-4584-96F9-E22AB0F8BB31")]
    [ComVisible(true)]
    public enum CameraStates
    {
        /// <summary>
        /// Camera status idle
        /// </summary>
        cameraIdle = 0,
        /// <summary>
        /// Camera status waiting
        /// </summary>
        cameraWaiting = 1,
        /// <summary>
        /// Camera status exposing
        /// </summary>
        cameraExposing = 2,
        /// <summary>
        /// Camera status reading
        /// </summary>
        cameraReading = 3,
        /// <summary>
        /// Camera status download
        /// </summary>
        cameraDownload = 4,
        /// <summary>
        /// Camera status error
        /// </summary>
        cameraError = 5
    }

    /// <summary>
    /// Sensor type, identifies the type of colour sensor
    /// V2 cameras only
    /// </summary>]
    [Guid("B1F24499-879F-4fa1-8FE1-C491741EBBF4")]
    [ComVisible(true)]
    public enum SensorType
    {
        /// <summary>
        /// Camera produces monochrome array with no Bayer encoding
        /// </summary>
        Monochrome = 0,
        /// <summary>
        /// Camera produces color image directly, requiring not Bayer decoding
        /// </summary>
        Color = 1,
        /// <summary>
        /// Camera produces RGGB encoded Bayer array images
        /// </summary>
        RGGB = 2,
        /// <summary>
        /// Camera produces CMYG encoded Bayer array images
        /// </summary>
        CMYG = 3,
        /// <summary>
        /// Camera produces CMYG2 encoded Bayer array images
        /// </summary>
        CMYG2 = 4,
        /// <summary>
        /// Camera produces Kodak TRUESENSE Bayer LRGB array images
        /// </summary>
        LRGB = 5
    }

}
