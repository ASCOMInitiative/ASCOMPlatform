On Error Resume Next
Dim H, F, D
Set H = CreateObject("DriverHelper.Chooser")
WScript.Echo "Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog"
H.DeviceType = "FilterWheel"
D=H.Choose("")
if D<>"" Then
  Set F = CreateObject(D)
  WScript.Echo "Created Driver, Click OK to Connect"
  F.Connected = True
  WScript.Echo "Connected, click OK to move to position 2"
  F.Position=2
  WScript.Echo "Click OK to move to position 6"
  F.Position=6
  WScript.Echo "Click OK to move to position -1"
  F.Position=-1
  WScript.Echo "Click OK to Disconnect"
  F.Connected = False
  WScript.Echo "Done"
end if
Set F = Nothing
Set H = Nothing

