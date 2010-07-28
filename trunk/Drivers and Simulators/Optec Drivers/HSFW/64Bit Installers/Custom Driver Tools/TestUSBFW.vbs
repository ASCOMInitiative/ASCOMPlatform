Dim msg
msg = "Starting Test..." & vbNewLine
Set FiltWheelsObj= WScript.CreateObject ("OptecHID_FilterWheelAPI.FilterWheels" )
'Set FiltWheelsObj= WScript.CreateObject ("OptecHID_RotatorAPI.Rotators" )
WScript.Echo("Hello")
Set al = WScript.CreateObject("System.Collections.ArrayList")

Set al=FiltWheelsObj.FilterWheelList

Set FW = al(0)
FW.HomeDevice
FW.CurrentPosition = 4
FW.CurrentPosition = 1
FW.CurrentPosition = 2
FW.CurrentPosition = 3
FW.CurrentPosition = 4
FW.CurrentPosition = 5
FW.CurrentPosition = 4
FW.CurrentPosition = 3
FW.CurrentPosition = 2
FW.CurrentPosition = 4

msg = msg & "This filter wheel has " & FW.NumberOfFilters & " filters" & vbNewLine 
'msg = msg & "This filter wheel ID is " & FW.WheelID &vbNewLine
msg = msg & "Firmware Version = " & FW.FirmwareVersion & vbNewLine

msg = msg & "Test Complete"

WScript.Echo(msg)