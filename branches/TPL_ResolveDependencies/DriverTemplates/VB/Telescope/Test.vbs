Dim H, T, drvrID, axRates, trRates
Set H = CreateObject("DriverHelper.Chooser")
WScript.Echo "Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog"
H.DeviceType = "Telescope"							' Make chooser for Telescopes
drvrID = H.Choose("")
if drvrID = "" Then WScript.Quit() 					' Cancelled chooser
Set T = CreateObject(drvrID)						' Create instance of selected Driver
Set axRates = T.AxisRates(0)                        ' Primary axis rates collection
WScript.Echo axRates.Count & " rates" 
If axRates.Count = 0 Then
    WScript.Echo "Empty AxisRates!"
Else
    For Each rate In axRates
	    WScript.Echo "Max=" & rate.Maximum & " Min=" & rate.Minimum
	Next
End If
Set trRates  = T.TrackingRates                      ' Tracking rates collection
if trRates.Count = 0 Then
    WScript.Echo "Empty TrackingRates!"
Else
    For Each rate In trRates
	    WScript.Echo "DriveRate=" & rate
	Next
End If
Set T = Nothing
Set H = Nothing

