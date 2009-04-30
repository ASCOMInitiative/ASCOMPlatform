VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Serial"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM Driver Helper Serial Port Object"
'---------------------------------------------------------------------
' Copyright © 2000-2002 SPACE.com Inc., New York, NY
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". SPACE.COM, INC. MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'   ==========
'   SERIAL.CLS
'   ==========
'
' Implementation of the ASCOM DriverHelper Serial class. Uses the
' MSCOMM control embedded in a hidden form. Simplifies the interface
' for vanilla scope comm uses. Usable from Windows Script Components.
'
' Written:  21-Aug-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 21-Aug-00 rbd     Initial edit
' 02-Nov-00 rbd     Binary I/O, reduce looping overhead.
' 13-Nov-00 rbd     Set return values BEFORE raising errors, so clients
'                   with On Error Resume Next receive correct values.
'                   Some parameters were missing ByVal :-(((
' 12-Dec-00 rbd     Assume port closed in class terminate(). Allow
'                   setting Connected to True when port is already
'                   open. Fix overflow on ReceiveTimeout > 32 sec.
'                   Never call DoEvents between sending and receiving
'                   the response, protecting the integrity of each
'                   exchange with the scope, and improving the
'                   responsivenes of the single character I/O :-((
'                   Explicitly test for timeout in ReceiveTerminated()
'                   do not rely on empty string... we could actually
'                   receive an empty string with the terminator, and
'                   that should be perfectly OK. Drop our thread
'                   priority a smidge while spin-looping for incoming
'                   data.
' 14-Jan-01 rbd     Fix ReceiveTerminated() to return a String
' 22-Jan-01 rbd     Implement IObjectSafety
' 27-Jan-01 rbd     Fix Connected = False (was no-op)
' 30-Apr-01 rbd     Add tracing (use registry for name)
' 22-Oct-01 rbd     1.1.4 - Make spin loops a bit more friendly by
'                   testing InBufferCount before appending to the
'                   buffer
' 24-Oct-01 rbd     1.1.5 - ClearBuffers() now attempts to eat
'                   characters coming into the port until it's been
'                   a "long time" since a character was seen.
' 12-Dec-01 rbd     1.1.7 - Remove priority drops in spin loops.
' 13-Dec-01 rbd     1.1.8 - Restore priority control.
' 27-Dec-01 rbd     1.1.9 - Undo "friendly' changes in 1.1.4 -- they
'                   turned out to be unfriendly.
' 08-Jan-02 rbd     1.1.10 - Undo changes in 1.1.5.
' 10-May-02 rbd     1.1.11 - Enhance binary tracing
' 29-Jun-02 rbd     1.1.12 - For EXE based drivers, remove all priority
'                   control.
' 27-Dec-02 cdr     1.1.13 - ensure that the serial output shows
'                   unprinted chars as hex values
' 04-Feb-03 rbd     2.1.1 - Assert RTS
' 04-Feb-03 rbd     2.1.2 - Merge Chris Rowland (cdr) tracing hex
'                   marked 1.1.13 above.
' 18-Mar-03 rbd     2.2.1 - SleepEx 0&, True in spin loops to lower CPU usage
' 24-Jun-03 rbd     2.3.1 - Version change for Platform 2.3
' 06-Feb-04 rbd     3.0.3 - Allow GetTickCount() to start out negative
'                   for Richard Berg's goofy notebook
' 23-Nov-04 rbd     4.0.1 - Version change for Platform 4.0
' 24-Nov-04 rbd     4.0.2 - Fix ReceiveXxx() to properly raise errors
'                   on timeouts after the Berg hack.
' 26-Jul-05 cdr     4.0.3 - Extend the interface to add ReceiveCountedBinary and
'                   ReceiveCountedTerminated methods to handle receive with a DBCS
'                   operating system.
' 01-Feb-06 rbd     4.1.4 - Got this from Chris R with 4.1.4 as project
'                   version. Cannot back it up!
' 01-Jan-07 rbd     5.0.1 - Add new property TimeoutMs. See comments
'                   there/below. Version change for Platform 5.0
' 26-Mar-07 rbd     5.0.2 - More informative error on failure to create
'                   the serial trace file (caused by multiple Serials
'                   being created in production and failure to turn off
'                   tracing). Had to move this into the Connected
'                   property in order for it to be visible in scripts
'                   since automation errors raised in object constructors
'                   do not honor the Source and Description in the Err,
'                   instead having a null source and a description from
'                   a hardwired table from VB.
'---------------------------------------------------------------------
Option Explicit

Implements IObjectSafety

Public Enum PortSpeed
    ps300 = 300
    ps1200 = 1200
    ps2400 = 2400
    ps4800 = 4800
    ps9600 = 9600
    ps14400 = 14400
    ps19200 = 19200
    ps28800 = 28800
    ps38400 = 38400
    ps57600 = 57600
    ps115200 = 115200
End Enum

Private m_frmCOM As CommForm
Private m_COM As MSComm
Private m_lTmoMS As Long
Private m_fSafeForScripting As Boolean
Private m_fTracing As Boolean
Private m_TraceFile As Scripting.TextStream
Private FSO As Scripting.FileSystemObject

Private Const THREAD_PRIORITY_NORMAL As Long = 0
Private Const THREAD_PRIORITY_BELOW_NORMAL As Long = -1
Private Const THREAD_PRIORITY_ABOVE_NORMAL As Long = 1


Private Declare Sub SleepEx Lib "kernel32" (ByVal dwMilliseconds As Long, _
                        ByVal bAlertable As Boolean)
Private Declare Function GetTickCount Lib "kernel32" () As Long
Private Declare Function GetCurrentThread Lib "kernel32" () As Long
Private Declare Function SetThreadPriority Lib "kernel32" _
                        (hThread As Long, nPriority As Long) As Long
                        
Private Declare Function URLEscape Lib "shlwapi.dll" Alias "UrlEscapeA" _
                (ByVal pszURL As String, ByVal pszEscaped As String, _
                ByRef pcchEscaped As Long, ByVal dwFlags As Long) As Long

Private g_HackOffset As Long

Private Function GetTickCountHack()
    GetTickCountHack = GetTickCount() + g_HackOffset
End Function

Private Sub Class_Initialize()
    Dim z As Long
    
    z = GetTickCount()                              ' For Richard Berg's goofy notebook
    If z < 0 Then
        g_HackOffset = -z + 1
    Else
        g_HackOffset = 0
    End If
    
    m_fSafeForScripting = True
    m_fTracing = False
    Set m_frmCOM = New CommForm
    Set m_COM = m_frmCOM.com
    m_COM.CommPort = 1
    m_COM.Settings = "9600,N,8,1"
    m_COM.InputMode = comInputModeText
    m_COM.Handshaking = comNone
    m_COM.InBufferSize = 4096
    m_COM.OutBufferSize = 1024
    m_COM.RTSEnable = True
    m_lTmoMS = 5000
    On Error Resume Next
    m_COM.PortOpen = False
    On Error GoTo 0
    
End Sub

Private Sub Class_Terminate()

    On Error Resume Next
    m_COM.PortOpen = False
    If m_fTracing Then m_TraceFile.Close
    
End Sub

Public Property Get Port() As Integer
Attribute Port.VB_Description = "The COM port number to use for serial communication"

    Port = m_COM.CommPort
    
End Property

Public Property Let Port(ByVal iNewValue As Integer)

    m_COM.CommPort = iNewValue
    
End Property


Public Property Get ReceiveTimeout() As Integer
Attribute ReceiveTimeout.VB_Description = "The maximum time that the object will wait for incoming receive data (seconds, default = 5)"

    ReceiveTimeout = m_lTmoMS / 1000
    
End Property

Public Property Let ReceiveTimeout(ByVal iNewValue As Integer)

    If iNewValue <= 0 Or iNewValue > 120 Then
        Err.Raise SCODE_INVALID_TIMEOUT, _
                    ERR_SOURCE_SERIAL, _
                    MSG_INVALID_TIMEOUT
    End If
    
    m_lTmoMS = iNewValue * 1000&    ' 1000& required to prevent overflow???
    
End Property

Public Property Get ReceiveTimeoutMs() As Long

    ReceiveTimeout = m_lTmoMS
    
End Property

Public Property Let ReceiveTimeoutMs(ByVal lNewValue As Long)

    If lNewValue <= 0 Or lNewValue > 120000 Then
        Err.Raise SCODE_INVALID_TIMEOUT, _
                    ERR_SOURCE_SERIAL, _
                    MSG_INVALID_TIMEOUT
    End If
    
    m_lTmoMS = lNewValue
    
End Property

Public Property Get Connected() As Boolean
Attribute Connected.VB_Description = "True means the object is connected to the physical serial (COM) port. "
        Connected = m_COM.PortOpen
End Property

Public Property Let Connected(ByVal bNewValue As Boolean)
    Dim buf As String
    Dim ed As String
    
    m_COM.PortOpen = bNewValue
    
    If m_COM.PortOpen Then
        Me.ClearBuffers
        If Not m_fTracing Then
            buf = GetProfile("", "SerTraceFile", ERR_SOURCE_SERIAL)
            If buf <> "" Then                       ' Set up tracing
                Set FSO = New Scripting.FileSystemObject
                On Error Resume Next                ' Catch "in use" and other errors here!
                Set m_TraceFile = FSO.CreateTextFile(buf, True)
                If Err.Number <> 0 Then
                    ed = Err.Description
                    On Error GoTo 0
                    Err.Raise SCODE_TRACE_ERR, _
                            ERR_SOURCE_SERIAL, _
                            MSG_TRACE_ERR & ed
                End If
                On Error GoTo 0
                m_fTracing = True
            End If
            Trace ("Serial trace file:")
        End If
    End If
    
End Property

Public Property Get Speed() As PortSpeed
Attribute Speed.VB_Description = "The data speed at which the port runs (baud, PortSpeed enum, default 9600)"
    Dim s() As String
    
    s = Split(m_COM.Settings, ",")
    Select Case s(0)                    ' Baud rate part
        Case "300": Speed = ps300
        Case "1200": Speed = ps1200
        Case "2400": Speed = ps2400
        Case "4800": Speed = ps4800
        Case "9600": Speed = ps9600
        Case "14400": Speed = ps14400
        Case "19200": Speed = ps19200
        Case "28800": Speed = ps28800
        Case "38400": Speed = ps38400
        Case "57600": Speed = ps57600
        Case "115200": Speed = ps115200
        Case Else: Err.Raise SCODE_UNSUP_SPEED, _
                            ERR_SOURCE_SERIAL, _
                            MSG_UNSUP_SPEED  ' Should never happen
    End Select
            
End Property

Public Property Let Speed(ByVal eNewValue As PortSpeed)

    Select Case eNewValue                    ' Baud rate part
        Case ps300: m_COM.Settings = "300,N,8,1"
        Case ps1200: m_COM.Settings = "1200,N,8,1"
        Case ps2400: m_COM.Settings = "2400,N,8,1"
        Case ps4800: m_COM.Settings = "4800,N,8,1"
        Case ps9600: m_COM.Settings = "9600,N,8,1"
        Case ps14400: m_COM.Settings = "14400,N,8,1"
        Case ps19200: m_COM.Settings = "19200,N,8,1"
        Case ps28800: m_COM.Settings = "28800,N,8,1"
        Case ps38400: m_COM.Settings = "38400,N,8,1"
        Case ps57600: m_COM.Settings = "57600,N,8,1"
        Case ps115200: m_COM.Settings = "115200,N,8,1"
        Case Else: Err.Raise SCODE_UNSUP_SPEED, _
                            ERR_SOURCE_SERIAL, _
                            MSG_UNSUP_SPEED  ' Should never happen
    End Select
End Property

Public Sub Transmit(ByVal Data As String)
Attribute Transmit.VB_Description = "Send the string of text data to the serial port"
    Trace "Transmit(""" & strASCII(Data) & """)..."
    m_COM.Output = Data
    Trace "...done"
End Sub

Public Sub TransmitBinary(Data() As Byte)
Attribute TransmitBinary.VB_Description = "Send the binary data array to the serial port"
    Dim buf As String, abuf As String, hbuf As String
    Dim i As Integer
    
    Trace "TransmitBinary(hex)..."
    buf = ""
    abuf = ""
    For i = LBound(Data) To UBound(Data)
        hbuf = Hex$(Data(i))
        If Len(hbuf) < 2 Then hbuf = "0" & hbuf
        buf = buf & " " & hbuf
        abuf = abuf & ToASCII(Data(i))
        If (i > 0) And ((i Mod 16) = 0) Then
            Trace buf & " - " & abuf
'            Trace abuf
            buf = ""
            abuf = ""
        End If
    Next
    If Len(buf) > 0 Then
        Trace buf & " - " & abuf
'        Trace abuf
    End If
    m_COM.Output = Data
    Trace "...done"
    
End Sub
'
' This receives at least one character from the serial line
'
Public Function Receive() As String
Attribute Receive.VB_Description = "Receive at least one character from the serial port"
    Dim tmo As Long
    Dim chars As String, buf As String
    
    Trace "Receive()..."
    tmo = GetTickCountHack() + m_lTmoMS             ' Timeout tix
    m_COM.InputMode = comInputModeText          ' Text
    m_COM.InputLen = 0                          ' Read all available
    buf = ""                                    ' Assure one loop
    Do While (buf = "") And (GetTickCountHack() < tmo) ''(t < tmo)
        chars = m_COM.Input
        If chars = "" Then
            SleepEx 10&, True
        Else
            buf = buf & chars
        End If
    Loop
    
    Receive = buf                               ' Pass back what we got
    
    If GetTickCountHack() >= tmo Then
        Err.Raise SCODE_RECEIVE_TIMEOUT, _
                ERR_SOURCE_SERIAL, _
                MSG_RECEIVE_TIMEOUT
    End If
    Trace "...Received """ & strASCII(buf) & """"
End Function
'
' This receives one binary Byte from the serial line
'
Public Function ReceiveByte() As Byte
Attribute ReceiveByte.VB_Description = "Receive one byte from the serial port"
    Dim tmo As Long
    Dim buf() As Byte
    Dim ok As Boolean
    
    Trace "ReceiveByte()..."
    ok = False                                  ' Assume failure
    tmo = GetTickCountHack() + m_lTmoMS             ' Timeout tix
    m_COM.InputMode = comInputModeBinary        ' Binary
    m_COM.InputLen = 1                          ' Read one byte
    Do While GetTickCountHack() < tmo
        If m_COM.InBufferCount > 0 Then
            buf = m_COM.Input
            ok = True                           ' Got a byte
            Exit Do
        End If
        SleepEx 10&, True
    Loop
        
    If Not ok Then
        Trace "Timeout"
        ReceiveByte = 0
        Err.Raise SCODE_RECEIVE_TIMEOUT, _
                ERR_SOURCE_SERIAL, _
                MSG_RECEIVE_TIMEOUT
    End If
    
    ReceiveByte = buf(0)
    Trace "...received 0x" & Hex$(ReceiveByte)
    
End Function

'
' This receives exactly the given number of characters from the serial line
'
Public Function ReceiveCounted(ByVal Count As Integer) As String
Attribute ReceiveCounted.VB_Description = "Receive exactly the given number of text characters"
    Dim tmo As Long
    Dim ch As String, buf As String
    
    If Count <= 0 Then _
        Err.Raise SCODE_ILLEGAL_COUNT, _
                ERR_SOURCE_SERIAL, _
                MSG_ILLEGAL_COUNT
    Trace "ReceiveCounted(" & Count & ")..."
    '
    ' We read one character at a time so that any data following
    ' the "Count" bytes will be left in the COM port input buf.
    ' Yes, this is inefficient.
    '
    tmo = GetTickCountHack() + m_lTmoMS             ' Timeout tix
    m_COM.InputMode = comInputModeText          ' Text
    m_COM.InputLen = 1                          ' Read 1 char at a time
    buf = ""                                    ' Assure one loop
    Do While (Len(buf) < Count) And (GetTickCountHack() < tmo) '''(t < tmo)
        ch = m_COM.Input
        If ch = "" Then
            SleepEx 10&, True
        Else
            buf = buf & ch
        End If
    Loop
    
    ReceiveCounted = buf                        ' Pass back what we got so far
    
    If GetTickCountHack() >= tmo Then
        Trace "Timeout"
        Err.Raise SCODE_RECEIVE_TIMEOUT, _
                ERR_SOURCE_SERIAL, _
                MSG_RECEIVE_TIMEOUT
    End If
    Trace "...received """ & strASCII(buf) & """"
    
End Function
'
' This receives until a given terminator string is seen
'
Public Function ReceiveTerminated(ByVal Terminator As String) As String
Attribute ReceiveTerminated.VB_Description = "Receive text until the given terminator string is seen"
    Dim tmo As Long
    Dim ch As String, buf As String
    Dim tlen As Integer
    
    Trace "ReceiveTerminated(""" & Terminator & """)..."
    If Terminator = "" Then _
        Err.Raise SCODE_EMPTY_TERM, _
                ERR_SOURCE_SERIAL, _
                MSG_EMPTY_TERM
    
    '
    ' We read one character at a time so that any data following
    ' the terminator string will be left in the COM port input buf.
    ' Yes, this is inefficient.
    '
    tmo = GetTickCountHack() + m_lTmoMS             ' Timeout tix
    tlen = Len(Terminator)
    m_COM.InputMode = comInputModeText          ' Text
    m_COM.InputLen = 1                          ' Read 1 char at a time
    buf = ""                                    ' Assure one loop
    Do While (Right$(buf, tlen) <> Terminator) And (GetTickCountHack() < tmo) ''(t < tmo)
        ch = m_COM.Input
        If ch = "" Then
            SleepEx 10&, True
        Else
            buf = buf & ch
        End If
    Loop
    
    ReceiveTerminated = buf                     ' Pass back what we got so far
    
    If GetTickCountHack() >= tmo Then
        Trace "Timeout"
        Err.Raise SCODE_RECEIVE_TIMEOUT, _
                ERR_SOURCE_SERIAL, _
                MSG_RECEIVE_TIMEOUT
    End If
    Trace "...received """ & strASCII(ReceiveTerminated) & """"
End Function
'
' This receives exactly the given number of bytes from the serial line
'
Public Function ReceiveCountedBinary(ByVal Count As Integer) As Byte()
    Dim tmo As Long
    
    If Count <= 0 Then _
        Err.Raise SCODE_ILLEGAL_COUNT, _
                ERR_SOURCE_SERIAL, _
                MSG_ILLEGAL_COUNT
    Trace "ReceiveCounted(" & Count & ")..."
    
    tmo = GetTickCountHack() + m_lTmoMS             ' Timeout tix
    m_COM.InputMode = comInputModeBinary
    ' poll until the correct number of bytes have been received or a timeout
    Do While (m_COM.InBufferCount < Count) And (GetTickCountHack() < tmo) '''(t < tmo)
            SleepEx 10&, True
    Loop
    ' read what we have, leave any more data in the input buffer
    m_COM.InputLen = Count
    ReceiveCountedBinary = m_COM.Input       ' read Count bytes
    
    If GetTickCountHack() >= tmo Then
        Trace "Timeout"
        Err.Raise SCODE_RECEIVE_TIMEOUT, _
                ERR_SOURCE_SERIAL, _
                MSG_RECEIVE_TIMEOUT
    End If
    Trace "...received """ & bytASCII(ReceiveCountedBinary) & """"
    
End Function
'
' This receives until a given terminator string is seen
'
Public Function ReceiveTerminatedBinary(Terminator() As Byte) As Byte()
    Dim tmo As Long
    Dim byt() As Byte, buf() As Byte
    Dim tlen As Integer, nMatch As Integer, nData As Integer
    
    Trace "ReceiveTerminatedBinary(""" & bytASCII(Terminator) & """)..."
    tlen = UBound(Terminator) - LBound(Terminator) + 1
    If tlen = 0 Then _
        Err.Raise SCODE_EMPTY_TERM, _
                ERR_SOURCE_SERIAL, _
                MSG_EMPTY_TERM
    
    '
    ' We read one character at a time so that any data following
    ' the terminator string will be left in the COM port input buf.
    ' Yes, this is inefficient.
    '
    tmo = GetTickCountHack() + m_lTmoMS             ' Timeout tix
    m_COM.InputMode = comInputModeBinary
    m_COM.InputLen = 1                          ' Read 1 char at a time
    buf = ""                                    ' Assure one loop
    Do While ((nMatch <> tlen) And (GetTickCountHack() < tmo)) ''(t < tmo)
        If m_COM.InBufferCount = 0 Then
            SleepEx 10&, True
        Else    ' 1 byte received
            ReDim Preserve buf(0 To nData)
            byt = m_COM.Input
            buf(nData) = byt(0)
            nData = nData + 1
            If byt(0) = Terminator(nMatch) Then
                nMatch = nMatch + 1
            Else
                nMatch = 0
            End If
        End If
    Loop
    
    ReceiveTerminatedBinary = buf                     ' Pass back what we got so far
    
    If GetTickCountHack() >= tmo Then
        Trace "Timeout"
        Err.Raise SCODE_RECEIVE_TIMEOUT, _
                ERR_SOURCE_SERIAL, _
                MSG_RECEIVE_TIMEOUT
    End If
    Trace "...received """ & bytASCII(ReceiveTerminatedBinary) & """"
End Function
'
' Clear the input and output buffers.
'
Public Sub ClearBuffers()
Attribute ClearBuffers.VB_Description = "Empties both the incoming and outgoing buffers for the serial port."
    Dim buf() As Byte
    
    m_COM.InBufferCount = 0
    m_COM.OutBufferCount = 0
    Trace "Buffers cleared"
    
End Sub

Private Sub Trace(msg As String)
    If m_fTracing Then
        m_TraceFile.WriteLine "[" & Format$(Now(), "Hh:Nn:Ss") & "] " & msg
    End If
End Sub

'
' Convert 8 bit binary to something printable.
'
Private Function ToASCII(ByVal b As Byte) As String
    ' cdr non printing characters are shown as "<hh>"
    If b < &H20 Or b > &H7F Then
        ToASCII = "<" & Hex(b) & ">"
    Else
        ToASCII = Chr$(b)
    End If

End Function

' cdr
' convert a string to something entirely printable
'
Private Function strASCII(ByVal b As String) As String
    Dim buf As String, i As Integer
    For i = 1 To Len(b)
        buf = buf & ToASCII(Asc(Mid$(b, i, 1)))
    Next
    strASCII = buf
End Function

' convert a byte array to something entirely printable
'
Private Function bytASCII(b() As Byte) As String
    Dim buf As String, i As Integer
    For i = LBound(b) To UBound(b)
        buf = buf & ToASCII(b(i))
    Next
    bytASCII = buf
End Function

Private Sub IObjectSafety_GetInterfaceSafetyOptions(ByVal riid As Long, _
                                                    ByRef pdwSupportedOptions As Long, _
                                                    ByRef pdwEnabledOptions As Long)

    Call GetInterfaceSafetyOptions(riid, _
                                pdwSupportedOptions, _
                                pdwEnabledOptions, _
                                m_fSafeForScripting)
                            
End Sub

Private Sub IObjectSafety_SetInterfaceSafetyOptions(ByVal riid As Long, _
                                                    ByVal dwOptionsSetMask As Long, _
                                                    ByVal dwEnabledOptions As Long)

    Call SetInterfaceSafetyOptions(riid, _
                                dwOptionsSetMask, _
                                dwEnabledOptions, _
                                m_fSafeForScripting)

End Sub

