//tabs=4
// --------------------------------------------------------------------------------
// <summary>
// ASCOM.Interface.ITelescope Telescope Interface V2
// </summary>
//
// <copyright company="TiGra Astronomy" author="Timothy P. Long">
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
// 10-Feb-2010	TPL	6.0.*	Initial edit. Mirrors platform 5.0 PIAs.
// 21-Feb-2010  cdr 6.0.*   Remove properties and methods already in IAscomDriver
// --------------------------------------------------------------------------------
//
using System;
namespace ASCOM.Interface
	{
	public interface ITelescope : IAscomDriver
		{
		void AbortSlew();
		AlignmentModes AlignmentMode { get; }
		double Altitude { get; }
		double ApertureArea { get; }
		double ApertureDiameter { get; }
		bool AtHome { get; }
		bool AtPark { get; }
		IAxisRates AxisRates(TelescopeAxes Axis);
		double Azimuth { get; }
		bool CanFindHome { get; }
		bool CanMoveAxis(TelescopeAxes Axis);
		bool CanPark { get; }
		bool CanPulseGuide { get; }
		bool CanSetDeclinationRate { get; }
		bool CanSetGuideRates { get; }
		bool CanSetPark { get; }
		bool CanSetPierSide { get; }
		bool CanSetRightAscensionRate { get; }
		bool CanSetTracking { get; }
		bool CanSlew { get; }
		bool CanSlewAltAz { get; }
		bool CanSlewAltAzAsync { get; }
		bool CanSlewAsync { get; }
		bool CanSync { get; }
		bool CanSyncAltAz { get; }
		bool CanUnpark { get; }
		void CommandBlind(string Command, bool Raw);
		bool CommandBool(string Command, bool Raw);
		string CommandString(string Command, bool Raw);
		double Declination { get; }
		double DeclinationRate { get; set; }
		PierSide DestinationSideOfPier(double RightAscension, double Declination);
		bool DoesRefraction { get; set; }
		EquatorialCoordinateType EquatorialSystem { get; }
		void FindHome();
		double FocalLength { get; }
		double GuideRateDeclination { get; set; }
		double GuideRateRightAscension { get; set; }
		bool IsPulseGuiding { get; }
		void MoveAxis(TelescopeAxes Axis, double Rate);
		void Park();
		void PulseGuide(GuideDirections Direction, int Duration);
		double RightAscension { get; }
		double RightAscensionRate { get; set; }
		void SetPark();
		PierSide SideOfPier { get; set; }
		double SiderealTime { get; }
		double SiteElevation { get; set; }
		double SiteLatitude { get; set; }
		double SiteLongitude { get; set; }
		bool Slewing { get; }
		short SlewSettleTime { get; set; }
		void SlewToAltAz(double Azimuth, double Altitude);
		void SlewToAltAzAsync(double Azimuth, double Altitude);
		void SlewToCoordinates(double RightAscension, double Declination);
		void SlewToCoordinatesAsync(double RightAscension, double Declination);
		void SlewToTarget();
		void SlewToTargetAsync();
		void SyncToAltAz(double Azimuth, double Altitude);
		void SyncToCoordinates(double RightAscension, double Declination);
		void SyncToTarget();
		double TargetDeclination { get; set; }
		double TargetRightAscension { get; set; }
		bool Tracking { get; set; }
		DriveRates TrackingRate { get; set; }
		ITrackingRates TrackingRates { get; }
		void Unpark();
		DateTime UTCDate { get; set; }
		}
	}
