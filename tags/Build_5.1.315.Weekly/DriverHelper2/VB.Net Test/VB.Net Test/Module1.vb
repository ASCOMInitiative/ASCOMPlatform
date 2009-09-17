Module Module1

    Sub Main()
		Dim U As ASCOM.Helper2.Util

		U = New ASCOM.Helper2.Util
		Debug.Print("Platform version " & U.PlatformVersion)
		Debug.Print(U.TimeZoneName & " offset " & U.TimeZoneOffset & " hours")
		Debug.Print(CStr(U.UTCDate))
		Debug.Print(CStr(U.JulianDate))
		Debug.Print(CStr(U.DateUTCToJulian(Date.FromOADate(CDbl(Now.ToOADate) + (U.TimeZoneOffset / 24.0)))))
		Debug.Print(CStr(U.DateJulianToUTC(U.JulianDate)))
		Debug.Print(U.SerialTraceFile)
		Debug.Print("Tracing " & CStr(U.SerialTrace))
		U.SerialTrace = True
		Debug.Print("Tracing " & CStr(U.SerialTrace))
		U.SerialTrace = False
		Debug.Print("Tracing " & CStr(U.SerialTrace))
	End Sub

End Module
