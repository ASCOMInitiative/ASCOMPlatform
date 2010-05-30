Set myRF = CreateObject("RoboFocus.FocusControl")
 myRF.actOpenComm()
' msgbox "Comm port status: " & myRF.getCommStatus()
' msgBox "Firmware version: " & myRF.getFirmwareVersion()

' msgbox "opening file"
 myrf.actLoadTempCompFile("c:\My Documents\a test 11 a.tcd")

 myRF.getTempCompAbsRel()
 myRF.setTempCompAbsRel(1)
 myRF.getTempCompAbsRel()

' msgbox myrf.getAutoRate()
' myrf.setautorate(5.6)
' msgbox myrf.getAutoRate()

' myrf.setBacklashDirection(2)
' msgbox myrf.getdeadZone()
' msgbox myrf.setDeadZone(50)
' msgbox myrf.getDeadZone()

' myrf.actTempCompAuto()

' msgbox "closing comm"
' msgbox myRF.actCloseComm()

set myRF = Nothing