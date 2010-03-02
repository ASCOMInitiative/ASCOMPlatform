'This script retrieves and displays the platform version number

'Version History
'Original version           - Distributed in Platform 5
'Peter Simpson - 06/02/2010 - Revised to use the new 32/64bit ASCOM.Utilities.Util component in place of the 
'                             32bit only DriverHelper2.Util component

Dim H2
Set H2 = CreateObject("ASCOM.Utilities.Util")
WScript.Echo "You have ASCOM Platform " & H2.PlatformVersion & " installed."
Set H2 = Nothing