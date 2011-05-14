Dim U

Set U = CreateObject("DriverHelper2.Util")
WScript.Echo "Platform version " & U.PlatformVersion
WScript.Echo U.TimeZoneName & " offset " & U.TimeZoneOffset & " hours"
WScript.Echo U.UTCDate
WScript.Echo U.JulianDate
WScript.Echo U.DateUTCToJulian(Now + (U.TimeZoneOffset / 24.0))
WScript.Echo U.DateJulianToUTC(U.JulianDate)
WScript.Echo U.SerialTraceFile
WScript.Echo "Tracing " & CStr(U.SerialTrace)
U.SerialTrace = True
WScript.Echo "Tracing " & CStr(U.SerialTrace)
U.SerialTrace = False
WScript.Echo "Tracing " & CStr(U.SerialTrace)
