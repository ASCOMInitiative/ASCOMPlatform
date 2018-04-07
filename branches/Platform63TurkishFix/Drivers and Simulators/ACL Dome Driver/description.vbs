
Set dome = WScript.CreateObject("ACLDome.Dome")
dome.Connected = true

Wscript.echo dome.Name
Wscript.echo dome.Description
Wscript.echo dome.DriverInfo
