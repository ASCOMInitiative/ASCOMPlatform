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
Dim time(4)
time(0) = 2450203.5
time(1) = 2450203.5
time(2) = 2450417.5
time(3) = 2450300.5

Dim star(3)
Set star(0) = CreateObject("NOVAS.Star")
star(0).DeltaT = 60.0
star(0).Catalog = "FK5"
star(0).Name = "Polaris"
star(0).Number = 0
star(0).RightAscension = 2.5301955556
star(0).Declination = 89.2640888889
star(0).ProperMotionRA = 19.8770
star(0).ProperMotionDec = -1.520
star(0).Parallax = 0.0070
star(0).RadialVelocity = -17.0
Set star(1) = CreateObject("NOVAS.Star")
star(1).DeltaT = 60.0
star(1).Catalog = "FK5"
star(1).Name = "Delta ORI"
star(1).Number = 1
star(1).RightAscension = 5.5334438889
star(1).Declination = -0.2991333333
star(1).ProperMotionRA = 0.0100
star(1).ProperMotionDec = -0.220
star(1).Parallax = 0.0140
star(1).RadialVelocity = 16.0
Set star(2) = CreateObject("NOVAS.Star")
star(2).DeltaT = 60.0
star(2).Catalog = "FK5"
star(2).Name = "Theta CAR"
star(2).Number = 2
star(2).RightAscension = 10.7159355556
star(2).Declination = -64.3944666667
star(2).ProperMotionRA = -0.3480
star(2).ProperMotionDec = 1.000
star(2).Parallax = 0.0000
star(2).RadialVelocity = 24.0

Dim site
Set site = CreateObject("NOVAS.Site")
site.Set 45.0, -75.0, 0.0
site.Temperature = 10.0
site.Pressure = 1010.0

For i = 0 to 3
	For j = 0 to 2
		Dim pos
		Set pos = star(j).GetTopocentricPosition(time(i), site, True)
		WScript.Echo("JD = " & time(i) & "  Star = " & star(j).Name) 
		WScript.Echo("RA = " & pos.RightAscension & "  Dec = " & pos.Declination & vbCrLf)
	Next
	WScript.Echo vbCrLf
Next

