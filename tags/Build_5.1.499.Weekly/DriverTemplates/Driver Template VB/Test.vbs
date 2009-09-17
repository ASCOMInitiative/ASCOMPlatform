Dim H, F
Set H = CreateObject("DriverHelper.Chooser")
WScript.Echo "Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog"
H.DeviceType = "TEMPLATEDEVICECLASS"					' Make chooser for TEMPLATEDEVICECLASSs
Set F = CreateObject(H.Choose(""))					' Create instance of selected Driver
Set F = Nothing
Set H = Nothing

