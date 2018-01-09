
Set dome = WScript.CreateObject("ACLDome.Dome")
dome.Connected = true

Wscript.echo dome.Connected
Wscript.echo dome.Azimuth
Wscript.echo dome.DriverInfo
