Dim util, TL, returndouble
Set TL= CreateObject("ASCOM.Utilities.TraceLogger")
TL.SetLogFile "", "ScriptTest"
TL.Enabled = True
Set  util =  CreateObject("ASCOM.Utilities.Util")
REM WScript.Echo "Version = " & util.PlatformVersion
TL.LogMessage "Script", "PlatformVersion " & util.PlatformVersion, False

Dim cache
TL.LogMessage "Script", "Creating cache", False

Set cache = CreateObject("ASCOM.Utilities.Cache")
TL.LogMessage "Script", "Cache created OK.", False

cache.SetDouble "Script1", 123.456, 1.0
TL.LogMessage "Script", "Set cache value OK", False

Dim retval
TL.LogMessage "Script", "Getting value from cache", False
retval= cache.GetDouble("Script1",0.0)
TL.LogMessage "Script", "Returned value from cache: " & retval, False

TL.LogMessage "Script", "Getting missing value from cache", False
On Error Resume Next
Err.Clear      ' Clear any possible Error that previous code raised

retval= cache.GetDouble("ScriptXXX", 0.0)

If Err.Number <> 0 Then
TL.LogMessage "Script", "Exception generated!", False
    TL.LogMessage "ErrorNumber: ", Err.Number & " " & Hex(Err.Number), False
    TL.LogMessage "ErrSource: ", Err.Source, False
    TL.LogMessage "ErrDescription: ", Err.Description, False
    Err.Clear             ' Clear the Error
End If

on Error GoTo 0

TL.LogMessage "Script", "Returned missing value from cache: " & retval, False

TL.LogMessage "Script", "Setting double object in cache", False
cache.Set "ObjectDouble", 123.456, 1.0

TL.LogMessage "Script", "Retrieving double object from cache", False
returndouble = cache.Get("ObjectDouble", 0.0)
TL.LogMessage "Script", "Returned double value from cache: " & returndouble, False


TL.LogMessage "Script", "Finished script", False
TL.Enabled = False