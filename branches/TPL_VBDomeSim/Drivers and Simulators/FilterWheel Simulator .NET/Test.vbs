Dim H, F
Set H = CreateObject("DriverHelper.Chooser")
WScript.Echo "Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog"
H.DeviceType = "FilterWheel"							' Make chooser for FilterWheels
Set F = CreateObject(H.Choose(""))					' Create instance of selected Driver
F.Connected = True
Set F = Nothing
Set H = Nothing

