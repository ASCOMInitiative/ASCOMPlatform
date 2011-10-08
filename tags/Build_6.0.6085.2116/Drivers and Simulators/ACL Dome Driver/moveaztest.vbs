
Dim az

Set dome = WScript.CreateObject("ACLDome.Dome")
dome.Connected = true

az = dome.azimuth + 10
dome.SlewToAzimuth(az)
