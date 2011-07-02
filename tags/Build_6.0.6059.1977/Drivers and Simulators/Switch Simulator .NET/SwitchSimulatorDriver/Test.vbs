Dim H, F
Set H = CreateObject("DriverHelper.Chooser")
WScript.Echo "Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog"
H.DeviceType = "Switch"							' Make chooser for Switchs
Set F = CreateObject(H.Choose("")) ' Create instance of selected Driver
msgbox(cstr(F.connected))
F.connected = true
msgbox(cstr(F.connected))
					


