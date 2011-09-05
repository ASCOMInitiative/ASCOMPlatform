'Create a variable to hold the test output data
Dim msg
msg = "Starting Test..." & vbNewLine
'Create an instance of the FilterWheels Class
'this provides you with a means of accessing all of the FilterWheels attached to the PC
Set FiltWheelsObj= WScript.CreateObject ("OptecHID_FilterWheelAPI.FilterWheels" )
'Get an ArrayList of FilterWheel Objects representing the attached devices
Set ListOfDevices=FiltWheelsObj.FilterWheelList
'Create a Reference to the first item in the list.
Set FW = ListOfDevices(0)
'Home the device
FW.HomeDevice
'Change to various positions
FW.CurrentPosition = 4
FW.CurrentPosition = 1
FW.CurrentPosition = 2
FW.CurrentPosition = 3
FW.CurrentPosition = 5
'Get the Number of Filters from the device
msg = msg & "This filter wheel has " & FW.NumberOfFilters & " filters" & vbNewLine 
'Get the WheelID from the device (Make sure to cast it using chr())
msg = msg & "This filter wheel ID is " & chr(FW.WheelID) & vbNewLine
'Get the Firmware version from the device
msg = msg & "Firmware Version = " & FW.FirmwareVersion & vbNewLine
msg = msg & "Test Complete"
'Print the output data
WScript.Echo(msg)