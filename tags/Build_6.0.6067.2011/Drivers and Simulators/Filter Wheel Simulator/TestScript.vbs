' Test script for the FilterWeel Simulator
' Mark Crossley Nov 2008
'
' Note that Windows Scripting Host (WSH) does not support
' any array type other than Variant. Therefore you cannot
' read the filter name or focus offset values from ASCOM
' filter wheel drivers using VBScript or JScript.
'

Set x = createObject("FilterWheelSim.FilterWheel")

x.connected = true

' show the setup dialog
x.setupdialog

' show the curent position
wscript.echo x.position

' move to the first slot
x.position = 0

' should still be moving, returns -1?
wscript.echo x.position

' wait for move to complete
wscript.echo "wait"

' did we get there?
wscript.echo x.position
