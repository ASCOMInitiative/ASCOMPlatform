//tabs=4
// --------------------------------------------------------------------------------
// <summary>
// ASCOM.Interface Telescope Enumerations
// </summary>
//
// <copyright company="The ASCOM Initiative" author="Timothy P. Long">
//	Copyright © 2010 The ASCOM Initiative
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
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.Interface
	{
	//[Guid("30D18B61-AECC-4C03-8759-E3EDD246F062")]
	/// <summary>
	/// The alignment mode of the mount.
	/// </summary>
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

	//[Guid("D9998808-2DF0-4CA1-ADD6-CE592026C663")]
	/// <summary>
	/// Well-known telescope tracking rates.
	/// </summary>
	public enum DriveRates
		{
		/// <summary>
		/// Sidereal tracking rate (15.0 arcseconds per second).
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

	//[Guid("135265BA-25AC-4F43-95E5-80D0171E48FA")]
	/// <summary>
	/// Equatorial coordinate systems used by telescopes.
	/// </summary>
	public enum EquatorialCoordinateType
		{
		/// <summary>
		/// Custom or unknown equinox and/or reference frame.
		/// </summary>
		equOther,

		/// <summary>
		/// Local topocentric; this is the most common for amateur telescopes.
		/// </summary>
		equLocalTopocentric,

		/// <summary>
		/// J2000 equator/equinox, ICRS reference frame.
		/// </summary>
		equJ2000,

		/// <summary>
		/// J2050 equator/equinox, ICRS reference frame.
		/// </summary>
		equJ2050,

		/// <summary>
		/// B1950 equinox, FK4 reference frame.
		/// </summary>
		equB1950
		}

	//[Guid("3613EEEB-5563-47D8-B512-1D36D64CEEBB")]
	/// <summary>
	/// The direction in which the guide-rate motion is to be made.
	/// </summary>
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

	//[Guid("BCB5C21D-B0EA-40D1-B36C-272456F44D01")]
	/// <summary>
	/// The telescope axes
	/// </summary>
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

	//[Guid("ECD99531-A2CF-4B9F-91A0-35FE5D12B043")]
	/// <summary>
	/// The side of the pier on which the optical tube assembly is located.
	/// </summary>
	/// <remarks>
	///		<alert class="caution">
	///			<para>
	///			<c>Pier side</c> is a GEM-specific term that has historically
	///			caused much confusion. Do not confuse <c>Pier Side</c>
	///			with <c>Pointing State</c>.
	///			</para>
	///		</alert>
	/// </remarks>
	public enum PierSide
		{
		/// <summary>
		/// Mount on East side of pier (looking West)
		/// </summary>
		pierEast = 0,

		/// <summary>
		/// Unknown or indeterminate.
		/// </summary>
		pierUnknown = -1,

		/// <summary>
		/// Mount on West side of pier (looking East)
		/// </summary>
		pierWest = 1
		}

 

	}
