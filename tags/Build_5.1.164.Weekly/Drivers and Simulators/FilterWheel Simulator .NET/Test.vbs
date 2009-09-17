Dim H, F, D
Set H = CreateObject("DriverHelper.Chooser")
WScript.Echo "Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog"
H.DeviceType = "FilterWheel"							' Make chooser for FilterWheels
D=H.Choose("")
if D<>"" Then
  Set F = CreateObject(D)					' Create instance of selected Driver
  F.Connected = True
  WScript.Echo "Connected, move to position 2"
  F.Position=2
  WScript.Echo "Done"
end if
Set F = Nothing
Set H = Nothing

