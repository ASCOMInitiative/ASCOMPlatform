'
' check-star.vbs
'
' Check many parts of NOVAS-COM for accuracy by getting the 
' TopocentricPosition of some stars using the internal
' ephemeris generator. These are the same values used 
' in the NOVAS-C checkout routine for stars. The USNO
' checkout.c specifies the times, site data and delta-t.
'
'
Dim star, util, TJD, Transform, site, pos

Set util = CreateObject("ASCOM.Utilities.Util")
Set astroutil = CreateObject("ASCOM.Astrometry.AstroUtils.AstroUtils")
Set star = CreateObject("ASCOM.Astrometry.NOVASCOM.Star")
Set site = CreateObject("ASCOM.Astrometry.NOVASCOM.Site")
Set Transform = CreateObject("ASCOM.Astrometry.Transform.Transform")

'TJD = 2456054.83235915
'TJD = 2456054.83501792
'TJD = 2456054.83693353
'TJD = 2456054.83790922
'TJD = 2456054.84588125
'TJD = 2456054.85009156
'TJD = 2456054.85748483
'TJD = 2456054.86128954
'TJD = 2456054.86180013
'TJD = 2456054.86206739
'TJD = 2456054.86339435
'TJD = 2456054.87351955
TJD = astroutil.JulianDateTT(0.0)

site.Latitude = util.DMSToDegrees("51:04:43")
site.Longitude = -util.DMSToDegrees("00:17:40")
site.Height = 80.0
site.Temperature = 10.0
site.Pressure = 1000.0

Transform.SiteElevation = site.Height
Transform.SiteLatitude = site.Latitude
Transform.SiteLongitude = site.Longitude
Transform.SiteTemperature = site.Temperature

star.DeltaT = astroutil.DeltaT
star.Catalog = "HIP"
star.Name = "Deneb"
star.Number = 102098
star.RightAscension = util.HMSToHours("20:41:25.916")
star.Declination = util.DMSToDegrees("45:16:49.236")
star.ProperMotionRA = 0.201 ' arcsec/Century
star.ProperMotionDec = 0.185 ' arcsec/Century
star.Parallax = 0.00231 ' arcsec
star.RadialVelocity = -4.5 'km/s
Transform.Refraction = False
Transform.SetJ2000 star.RightAscension, star.Declination
Transform.JulianDateTT = TJD
PrintStar star, Transform

star.DeltaT = astroutil.DeltaT
star.Catalog = "HIP"
star.Name = "Capella - No proper motion"
star.Number = 24608
star.RightAscension = util.HMSToHours("05:16:41.359")
star.Declination = util.DMSToDegrees("45:59:52.769")
star.ProperMotionRA = 0.0 ' arcsec/Century
star.ProperMotionDec = 0.0 ' arcsec/Century
star.Parallax = 0.0 ' arcsec
star.RadialVelocity = 0.0 ' km/s
Transform.Refraction = False
Transform.SetJ2000 star.RightAscension, star.Declination
Transform.JulianDateTT = TJD
PrintStar star, Transform

star.DeltaT = astroutil.DeltaT
star.Catalog = "HIP"
star.Name = "Capella - W ith proper motion"
star.Number = 24608
star.RightAscension = util.HMSToHours("05:16:41.359")
star.Declination = util.DMSToDegrees("45:59:52.769")
star.ProperMotionRA = 7.525 ' arcsec/Century
star.ProperMotionDec = -42.689 ' arcsec/Century
star.Parallax = 0.0762 ' arcsec
star.RadialVelocity = 30.2 ' km/s
Transform.Refraction = False
Transform.SetJ2000 star.RightAscension, star.Declination
Transform.JulianDateTT = TJD
PrintStar star, Transform

WScript.Echo("Finished")
Stop
 
Function PrintStar(star, Transform)
Set pos = star.GetTopocentricPosition(TJD, site, False)
WScript.Echo("Terrestrial Time = " & astroutil.FormatJD(TJD, "yyyy MMM dd HH:mm:ss.fff") & "  Star = " & star.Name & vbcrlf)
WScript.Echo("NOVASCOM, Transform, Difference - Refraction off" & vbCrLf & _
"RA = " & util.HoursToHMS(pos.RightAscension,":",":","",3) & "  Dec = " & util.DegreesToDMS(pos.Declination,":",":","",3) & _
"  Az = " & util.DegreesToDMS(pos.Azimuth,":",":","",3) & "  El = " & util.DegreesToDMS(pos.Elevation,":",":","",3) & vbCrLf & _
"RA = " & util.HoursToHMS(Transform.RATopocentric,":",":","",3) & "  Dec = " & util.DegreesToDMS(Transform.DecTopocentric,":",":","",3) & _
"  Az = " & util.DegreesToDMS(Transform.AzimuthTopocentric,":",":","",3) & "  El = " & util.DegreesToDMS(Transform.ElevationTopocentric,":",":","",3) & vbCrLf &_
"RA = " & util.HoursToHMS(abs(pos.RightAscension - Transform.RATopocentric),":",":","",3) & "  Dec = " & util.DegreesToDMS(abs(pos.Declination - Transform.DecTopocentric),":",":","",3) & _
"  Az = " & util.DegreesToDMS(abs(pos.Azimuth - Transform.AzimuthTopocentric),":",":","",3) & "  El = " & util.DegreesToDMS(abs(pos.Elevation - Transform.ElevationTopocentric),":",":","",3) & vbCrLf  )

Transform.Refraction = True
Set pos = star.GetTopocentricPosition(TJD, site, True)
WScript.Echo("NOVASCOM, Transform, Difference - Refraction on" & vbCrLf & _
"RA = " & util.HoursToHMS(pos.RightAscension,":",":","",3) & "  Dec = " & util.DegreesToDMS(pos.Declination,":",":","",3) & _
"  Az = " & util.DegreesToDMS(pos.Azimuth,":",":","",3) & "  El = " & util.DegreesToDMS(pos.Elevation,":",":","",3) & vbCrLf & _
"RA = " & util.HoursToHMS(Transform.RATopocentric,":",":","",3) & "  Dec = " & util.DegreesToDMS(Transform.DecTopocentric,":",":","",3) &  _
"  Az = " & util.DegreesToDMS(Transform.AzimuthTopocentric,":",":","",3) & "  El = " & util.DegreesToDMS(Transform.ElevationTopocentric,":",":","",3) & vbCrLf  & _
"RA = " & util.HoursToHMS(abs(pos.RightAscension - Transform.RATopocentric),":",":","",3) & "  Dec = " & util.DegreesToDMS(abs(pos.Declination - Transform.DecTopocentric),":",":","",3) & _
"  Az = " & util.DegreesToDMS(abs(pos.Azimuth - Transform.AzimuthTopocentric),":",":","",3) & "  El = " & util.DegreesToDMS(abs(pos.Elevation - Transform.ElevationTopocentric),":",":","",3) & vbCrLf  )

Dim RATopo, DecTopo, AzTopo, ElTopo
RATopo = Transform.RATopoCentric
DecTopo = Transform.DecTopocentric
AzTopo = Transform.AzimuthTopocentric
ElTopo = Transform.ElevationTopocentric

Transform.SetTopocentric RATopo,DecTopo
WScript.Echo("Transform Topocentric to J2000, Difference - Refraction on" & vbCrLf & _
"RA = " & util.HoursToHMS(Transform.RAJ2000,":",":","",3) & "  Dec = " & util.DegreesToDMS(Transform.DecJ2000,":",":","",3) &  vbCrLf & _
"RA = " & util.HoursToHMS(abs(star.RightAscension - Transform.RAJ2000),":",":","",3) & "  Dec = " & util.DegreesToDMS(abs(star.Declination - Transform.DecJ2000),":",":","",3) & vbCrLf )

Transform.SetAzimuthElevation AzTopo,ElTopo
WScript.Echo("Transform Az/El to J2000, Difference - Refraction on" & vbCrLf & _
"RA = " & util.HoursToHMS(Transform.RAJ2000,":",":","",3) & "  Dec = " & util.DegreesToDMS(Transform.DecJ2000,":",":","",3) &  vbCrLf & _
"RA = " & util.HoursToHMS(abs(star.RightAscension - Transform.RAJ2000),":",":","",3) & "  Dec = " & util.DegreesToDMS(abs(star.Declination - Transform.DecJ2000),":",":","",3) &vbCrLf & vbcrlf )



End function
