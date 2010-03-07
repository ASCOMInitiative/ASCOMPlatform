using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace ASCOM.Interface
	{
	
	public interface ITelescope
	{
		
		AlignmentModes AlignmentMode {  get; }
		
		double Altitude {  get; }
		
		double ApertureArea {  get; }
		
		double ApertureDiameter {  get; }
		
		bool AtHome {  get; }
		
		bool AtPark {  get; }
		
		double Azimuth {  get; }
		
		bool CanFindHome {  get; }
		
		bool CanPark {  get; }
		
		bool CanPulseGuide {  get; }
		
		bool CanSetDeclinationRate {  get; }
		
		bool CanSetGuideRates {  get; }
		
		bool CanSetPark {  get; }
		
		bool CanSetRightAscensionRate {  get; }
		
		bool CanSetPierSide {  get; }
		
		bool CanSetTracking {  get; }
		
		bool CanSlew {  get; }
		
		bool CanSlewAltAz {  get; }
		
		bool CanSlewAltAzAsync {  get; }
		
		bool CanSlewAsync {  get; }
		
		bool CanSync {  get; }
		
		bool CanSyncAltAz {  get; }
		
		bool CanUnpark {  get; }
		
		bool Connected {  get;   set; }
		
		double Declination {  get; }
		
		double DeclinationRate {  get;   set; }
		
		string Description {   get; }
		
		bool DoesRefraction {  get;   set; }
		
		string DriverInfo {   get; }
		
		string DriverVersion {   get; }
		
		EquatorialCoordinateType EquatorialSystem {  get; }
		
		double FocalLength {  get; }
		
		double GuideRateDeclination {  get;   set; }
		
		double GuideRateRightAscension {  get;   set; }
		
		short InterfaceVersion {  get; }
		
		bool IsPulseGuiding {  get; }
		
		string Name {   get; }
		
		double RightAscension {  get; }
		
		double RightAscensionRate {  get;   set; }
		
		PierSide SideOfPier {  get;   set; }
		
		double SiderealTime {  get; }
		
		double SiteElevation {  get;   set; }
		
		double SiteLatitude {  get;   set; }
		
		double SiteLongitude {  get;   set; }
		
		bool Slewing {  get; }
		
		short SlewSettleTime {  get;   set; }
		
		double TargetDeclination {  get;   set; }
		
		double TargetRightAscension {  get;   set; }
		
		bool Tracking {  get;   set; }
		
		DriveRates TrackingRate {  get;   set; }
		
		ITrackingRates TrackingRates {   get; }
		
		DateTime UTCDate {  get;   set; }
		
		void AbortSlew();
		
		
		IAxisRates AxisRates( TelescopeAxes Axis);
		
		bool CanMoveAxis( TelescopeAxes Axis);
		
		PierSide DestinationSideOfPier( double RightAscension,  double Declination);
		
		void FindHome();
		
		void MoveAxis( TelescopeAxes Axis,  double Rate);
		
		void Park();
		
		void PulseGuide( GuideDirections Direction,  int Duration);
		
		void SetPark();
		
		void SetupDialog();
		
		void SlewToAltAz( double Azimuth,  double Altitude);
		
		void SlewToAltAzAsync( double Azimuth,  double Altitude);
		
		void SlewToCoordinates( double RightAscension,  double Declination);
		
		void SlewToCoordinatesAsync( double RightAscension,  double Declination);
		
		void SlewToTarget();
		
		void SlewToTargetAsync();
		
		void SyncToAltAz( double Azimuth,  double Altitude);
		
		void SyncToCoordinates( double RightAscension,  double Declination);
		
		void SyncToTarget();
		
		void Unpark();
		
		void CommandBlind( string Command,  bool Raw);
		
		bool CommandBool( string Command,  bool Raw);
		
		
		string CommandString( string Command,  bool Raw);
	}

 

 

	}
