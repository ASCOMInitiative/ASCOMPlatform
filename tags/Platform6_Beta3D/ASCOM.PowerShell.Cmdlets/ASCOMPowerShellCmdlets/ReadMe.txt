Install the snapin using the installer.

Start PowerShell.

Check the snapin has installed correctly...

    PS C:\> Get-PSSnapin -Registered ASCOM*

    Name        : ASCOM.PowerShell.Cmdlets
    PSVersion   : 2.0
    Description : Cmdlets to interface with the ASCOM platform
    

Add the snapin to your session...
    PS C:\> Add-PSsnapin ASCOM*
    PS C:\>
    
Show the cmdlets available...

    PS C:\> Get-Command -PSsnapin ASCOM*

    CommandType     Name                                                Definition
    -----------     ----                                                ----------
    Cmdlet          Get-AstrometryEnum                                  Get-AstrometryEnum [-type] <String> [-Verbose] [...
    Cmdlet          Get-AstrometryStruct                                Get-AstrometryStruct [-type] <String> [-Verbose]...
    Cmdlet          New-Camera                                          New-Camera [-DriverId] <String> [-Verbose] [-Deb...
    Cmdlet          New-Dome                                            New-Dome [-DeviceId] <String> [-Verbose] [-Debug...
    Cmdlet          New-Earth                                           New-Earth [[-tjd] <Double>] [-Verbose] [-Debug] ...
    Cmdlet          New-FilterWheel                                     New-FilterWheel [-DriverId] <String> [-Verbose] ...
    Cmdlet          New-Focuser                                         New-Focuser [-DriverId] <String> [-Verbose] [-De...
    Cmdlet          New-Kepler                                          New-Kepler [-Verbose] [-Debug] [-ErrorAction <Ac...
    Cmdlet          New-NOVAS2                                          New-NOVAS2 [-Verbose] [-Debug] [-ErrorAction <Ac...
    Cmdlet          New-Planet                                          New-Planet [-Verbose] [-Debug] [-ErrorAction <Ac...
    Cmdlet          New-PositionVector                                  New-PositionVector [[-init] <Double[]>] [-Verbos...
    Cmdlet          New-Rotator                                         New-Rotator [-DriverId] <String> [-Verbose] [-De...
    Cmdlet          New-Site                                            New-Site [[-latitude] <Double>] [[-longitude] <D...
    Cmdlet          New-Star                                            New-Star [[-init] <Double[]>] [-Verbose] [-Debug...
    Cmdlet          New-Switch                                          New-Switch [-DriverId] <String> [-Verbose] [-Deb...
    Cmdlet          New-Telescope                                       New-Telescope [-DriverId] <String> [-Verbose] [-...
    Cmdlet          New-Transform                                       New-Transform [-Verbose] [-Debug] [-ErrorAction ...
    Cmdlet          New-Util                                            New-Util [-Verbose] [-Debug] [-ErrorAction <Acti...
    Cmdlet          New-VelocityVector                                  New-VelocityVector [-Verbose] [-Debug] [-ErrorAc...
    Cmdlet          Show-Chooser                                        Show-Chooser [[-DeviceType] <String>] [-DriverId...


Display the help on a cmdlet...

    PS C:\> help Show-Chooser

    NAME
        Show-Chooser

    SYNOPSIS
        Display the ASCOM Chooser dialog.


    SYNTAX
        Show-Chooser [[-DeviceType] <String>] [-DriverId <String>] [-WarningAction <ActionPreference>] [-WarningVariable <S
        tring>] [<CommonParameters>]


    DESCRIPTION
        Display the ASCOM Chooser dialog.


    RELATED LINKS

    REMARKS
        To see the examples, type: "get-help Show-Chooser -examples".
        For more information, type: "get-help Show-Chooser -detailed".
        For technical information, type: "get-help Show-Chooser -full".
        
You can display more detailed help by using the -examples, -detailed, or -full arguments.
Note that the first parameter is optional, as is the parameter name (it defaults to "Telescope"). So all these commands are equivalent..

    PS C:\> $id = Show-Chooser
    PS C:\> $id = Show-Chooser -DeviceType "Telescope"
    PS C:\> $id = Show-Chooser "Telescope"

Lets invoke the Telescope Simulator, and connect it...

    PS C:\> $id = Show-Chooser
    PS C:\> $scope = New-Telescope $id
    PS C:\> $scope.connected
    False
    PS C:\> $scope.Connected = $true


Display what methods and properties are available with the Telescope object...

    PS C:\> $scope | Get-Member


       TypeName: ASCOM.DriverAccess.Telescope

    Name                     MemberType Definition
    ----                     ---------- ----------
    AbortSlew                Method     System.Void AbortSlew()
    AxisRates                Method     ASCOM.Interface.IAxisRates AxisRates(ASCOM.Interface.TelescopeAxes Axis)
    CanMoveAxis              Method     bool CanMoveAxis(ASCOM.Interface.TelescopeAxes Axis)
    CommandBlind             Method     System.Void CommandBlind(string Command, bool Raw)
    CommandBool              Method     bool CommandBool(string Command, bool Raw)
    CommandString            Method     string CommandString(string Command, bool Raw)
    DestinationSideOfPier    Method     ASCOM.Interface.PierSide DestinationSideOfPier(double RightAscension, double Dec...
    Dispose                  Method     System.Void Dispose()
    Equals                   Method     bool Equals(System.Object obj)
    FindHome                 Method     System.Void FindHome()
    GetHashCode              Method     int GetHashCode()
    GetType                  Method     type GetType()
    MoveAxis                 Method     System.Void MoveAxis(ASCOM.Interface.TelescopeAxes Axis, double Rate)
    Park                     Method     System.Void Park()
    PulseGuide               Method     System.Void PulseGuide(ASCOM.Interface.GuideDirections Direction, int Duration)
    SetPark                  Method     System.Void SetPark()
    SetupDialog              Method     System.Void SetupDialog()
    SlewToAltAz              Method     System.Void SlewToAltAz(double Azimuth, double Altitude)
    SlewToAltAzAsync         Method     System.Void SlewToAltAzAsync(double Azimuth, double Altitude)
    SlewToCoordinates        Method     System.Void SlewToCoordinates(double RightAscension, double Declination)
    SlewToCoordinatesAsync   Method     System.Void SlewToCoordinatesAsync(double RightAscension, double Declination)
    SlewToTarget             Method     System.Void SlewToTarget()
    SlewToTargetAsync        Method     System.Void SlewToTargetAsync()
    SyncToAltAz              Method     System.Void SyncToAltAz(double Azimuth, double Altitude)
    SyncToCoordinates        Method     System.Void SyncToCoordinates(double RightAscension, double Declination)
    SyncToTarget             Method     System.Void SyncToTarget()
    ToString                 Method     string ToString()
    Unpark                   Method     System.Void Unpark()
    AlignmentMode            Property   ASCOM.Interface.AlignmentModes AlignmentMode {get;}
    Altitude                 Property   System.Double Altitude {get;}
    ApertureArea             Property   System.Double ApertureArea {get;}
    ApertureDiameter         Property   System.Double ApertureDiameter {get;}
    AtHome                   Property   System.Boolean AtHome {get;}
    AtPark                   Property   System.Boolean AtPark {get;}
    Azimuth                  Property   System.Double Azimuth {get;}
    CanFindHome              Property   System.Boolean CanFindHome {get;}
    CanPark                  Property   System.Boolean CanPark {get;}
    CanPulseGuide            Property   System.Boolean CanPulseGuide {get;}
    CanSetDeclinationRate    Property   System.Boolean CanSetDeclinationRate {get;}
    CanSetGuideRates         Property   System.Boolean CanSetGuideRates {get;}
    CanSetPark               Property   System.Boolean CanSetPark {get;}
    CanSetPierSide           Property   System.Boolean CanSetPierSide {get;}
    CanSetRightAscensionRate Property   System.Boolean CanSetRightAscensionRate {get;}
    CanSetTracking           Property   System.Boolean CanSetTracking {get;}
    CanSlew                  Property   System.Boolean CanSlew {get;}
    CanSlewAltAz             Property   System.Boolean CanSlewAltAz {get;}
    CanSlewAltAzAsync        Property   System.Boolean CanSlewAltAzAsync {get;}
    CanSlewAsync             Property   System.Boolean CanSlewAsync {get;}
    CanSync                  Property   System.Boolean CanSync {get;}
    CanSyncAltAz             Property   System.Boolean CanSyncAltAz {get;}
    CanUnpark                Property   System.Boolean CanUnpark {get;}
    Connected                Property   System.Boolean Connected {get;set;}
    Declination              Property   System.Double Declination {get;}
    DeclinationRate          Property   System.Double DeclinationRate {get;set;}
    Description              Property   System.String Description {get;}
    DoesRefraction           Property   System.Boolean DoesRefraction {get;set;}
    DriverInfo               Property   System.String DriverInfo {get;}
    DriverVersion            Property   System.String DriverVersion {get;}
    EquatorialSystem         Property   ASCOM.Interface.EquatorialCoordinateType EquatorialSystem {get;}
    FocalLength              Property   System.Double FocalLength {get;}
    GuideRateDeclination     Property   System.Double GuideRateDeclination {get;set;}
    GuideRateRightAscension  Property   System.Double GuideRateRightAscension {get;set;}
    InterfaceVersion         Property   System.Int16 InterfaceVersion {get;}
    IsPulseGuiding           Property   System.Boolean IsPulseGuiding {get;}
    Name                     Property   System.String Name {get;}
    RightAscension           Property   System.Double RightAscension {get;}
    RightAscensionRate       Property   System.Double RightAscensionRate {get;set;}
    SideOfPier               Property   ASCOM.Interface.PierSide SideOfPier {get;set;}
    SiderealTime             Property   System.Double SiderealTime {get;}
    SiteElevation            Property   System.Double SiteElevation {get;set;}
    SiteLatitude             Property   System.Double SiteLatitude {get;set;}
    SiteLongitude            Property   System.Double SiteLongitude {get;set;}
    Slewing                  Property   System.Boolean Slewing {get;}
    SlewSettleTime           Property   System.Int16 SlewSettleTime {get;set;}
    TargetDeclination        Property   System.Double TargetDeclination {get;set;}
    TargetRightAscension     Property   System.Double TargetRightAscension {get;set;}
    Tracking                 Property   System.Boolean Tracking {get;set;}
    TrackingRate             Property   ASCOM.Interface.DriveRates TrackingRate {get;set;}
    TrackingRates            Property   ASCOM.Interface.ITrackingRates TrackingRates {get;}
    UTCDate                  Property   System.DateTime UTCDate {get;set;}


Close the Telescope object...

    PS C:\> $scope.Dispose()
    PS C:\> $scope = $null
    PS C:\>




====================================================
ASTROMETRY
====================================================

Lets convert the NOVAS2 example in the ASCOM help file...

        Public Class NOVAS2Examples
            Private Utl As ASCOM.Utilities.Util

            Private StarStruct As ASCOM.Astrometry.CatEntry 'Define structures
            Private Body As ASCOM.Astrometry.BodyDescription
            Private Earth As ASCOM.Astrometry.BodyDescription
            Private LocationStruct As ASCOM.Astrometry.SiteInfo

            Private POS(2), VEL(2), POSNow(2), RANow, DECNow, rc, Distance, JD As Double

            Private Const J2000 As Double = 2451545.0 'Julian day for J2000 epoch


            Sub Example()
                Utl = New ASCOM.Utilities.Util 'Create structures and get Julian date
                JD = Utl.JulianDate

                StarStruct = New ASCOM.Astrometry.CatEntry
                Body = New ASCOM.Astrometry.BodyDescription
                Earth = New ASCOM.Astrometry.BodyDescription
                LocationStruct = New ASCOM.Astrometry.SiteInfo

                StarStruct.RA = 12.0 'Initialise star structure with arbitary data
                StarStruct.Dec = 30.0
                StarStruct.Parallax = 2.0
                StarStruct.ProMoDec = 1.5
                StarStruct.ProMoRA = 2.5
                StarStruct.RadialVelocity = 3

                ASCOM.Astrometry.NOVAS.NOVAS2.StarVectors(StarStruct, POS, VEL) 'Convert the star data to position and velocity vectors
                ASCOM.Astrometry.NOVAS.NOVAS2.Precession(J2000, POS, JD, POSNow) 'Precess position vector to current date
                ASCOM.Astrometry.NOVAS.NOVAS2.Vector2RADec(POSNow, RANow, DECNow) 'Convert vector back into precessed RA and Dec
                MsgBox("POS StarVectors RA: " & Utl.HoursToHMS(RANow, ":", ":", "", 3) & "  DEC: " & Utl.DegreesToDMS(DECNow, ":", ":", "", 3))

                Body.Name = "Mars" 'Initialise planet object
                Body.Type = ASCOM.Astrometry.BodyType.MajorPlanet
                Body.Number = ASCOM.Astrometry.Body.Mars

                Earth.Name = "Earth" 'Initialise Earth object
                Earth.Type = ASCOM.Astrometry.BodyType.MajorPlanet
                Earth.Number = ASCOM.Astrometry.Body.Earth

                LocationStruct.Height = 80 'Initialise observing location structure
                LocationStruct.Latitude = 51
                LocationStruct.Longitude = 0.0
                LocationStruct.Pressure = 1000.0
                LocationStruct.Temperature = 10.0

                'Find Mars astrometric position at Julian date JD
                rc = ASCOM.Astrometry.NOVAS.NOVAS2.AstroPlanet(JD, Body, Earth, RANow, DECNow, Distance)
                MsgBox("Mars astrometric RC: " & rc & " RA: " & Utl.HoursToHMS(RANow) & "  DEC: " & Utl.DegreesToDMS(DECNow, ":", ":"))

                'Find Mars topocentric position at Julian date JD
                rc = ASCOM.Astrometry.NOVAS.NOVAS2.TopoPlanet(JD, Body, Earth, 0.0, LocationStruct, RANow, DECNow, Distance)
                MsgBox("Mars topocentric RC: " & rc & " RA: " & Utl.HoursToHMS(RANow) & "  DEC: " & Utl.DegreesToDMS(DECNow, ":", ":"))
            End Sub
        End Class


Converted to PowerShell (just the start of Sub Example, to give a flavour)...
[Note that the NOVAS2 library now uses structures rather than arrays of doubles in the sample code above]


    > $Utl = New-Util
    > $JD = $Utl.JulianDate
    > $StarStruct = Get-AstrometryStruct CatEntry
    > $Body = Get-AstrometryStruct BodyDescription
    > $Earth = Get-AstrometryStruct BodyDescription
    > $LocationStruct = Get-AstrometryStruct SiteInfo
    > $StarStruct.RA = 12.0
    > $StarStruct.Dec = 30.0
    > $StarStruct.Parallax = 2.0
    > $StarStruct.ProMoDec = 1.5
    > $StarStruct.ProMoRA = 2.5
    > $StarStruct.RadialVelocity = 3
    > $POS = Get-AstrometryStruct PosVector
    > $VEL = Get-AstrometryStruct VelVector
    > $NOVAS2 = New-NOVAS2
    > $NOVAS2.StarVectors($StarStruct, $POS, $VEL)
    ...
    > $Body.Name = "Mars"
    > $Body.Type = Get-AstrometryEnum BodyType MajorPlanet
    > $Body.Number = Get-AstrometryEnum Body Mars
    >...
    etc
    
Note that you could use the ASCOM.Astrometry enumerations directly...

    > [void] [System.Reflection.Assembly]::LoadWithPartialName("ASCOM.Astrometry")
    > $Body.Type = [ASCOM.Astrometry.BodyType]::MajorPlanet.value__

But the whole point of these cmdlets is to remove all that guff!    


