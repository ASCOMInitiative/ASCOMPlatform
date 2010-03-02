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
Attribute VB_Description = "ASCOM Scope Driver Helper Serial Port Object"
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
' 08-Mar-09 pwgs    5.1.0 - Refactored to use HelperNET serial port
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

Private m_fSafeForScripting As Boolean
Private m_COM As ASCOM_Utilities.Serial

Private Sub Class_Initialize()
'MsgBox "DRIVERHELPER.SERIAL Class init"
    m_fSafeForScripting = True
  'Set m_COM = CreateObject("DriverHelper.SerialSupport")
  
  Set m_COM = New ASCOM_Utilities.Serial
End Sub

Private Sub Class_Terminate()
    On Error Resume Next
    Call m_COM.Dispose
    Set m_COM = Nothing
End Sub

Public Property Get Port() As Integer
Attribute Port.VB_Description = "The COM port number to use for serial communication"
'MsgBox "DRIVERHELPER.SERIAL Get port"
    Port = m_COM.Port
End Property

Public Property Let Port(ByVal iNewValue As Integer)
'MsgBox "DRIVERHELPER.SERIAL Let port"
    m_COM.Port = iNewValue
    
End Property


Public Property Get ReceiveTimeout() As Integer
Attribute ReceiveTimeout.VB_Description = "The maximum time that the object will wait for incoming receive data (seconds, default = 5)"
'MsgBox "DRIVERHELPER.SERIAL Get Receivetimeout"
    ReceiveTimeout = m_COM.ReceiveTimeout
    
End Property

Public Property Let ReceiveTimeout(ByVal iNewValue As Integer)
'MsgBox "DRIVERHELPER.SERIAL Let Receivetimeout"
    If iNewValue <= 0 Or iNewValue > 120 Then
        Err.Raise SCODE_INVALID_TIMEOUT, _
                    ERR_SOURCE_SERIAL, _
                    MSG_INVALID_TIMEOUT
    End If
    
    m_COM.ReceiveTimeout = iNewValue
    
End Property

Public Property Get ReceiveTimeoutMs() As Long
'MsgBox "DRIVERHELPER.SERIAL Get Receivetimeoutms"
    ReceiveTimeoutMs = m_COM.ReceiveTimeoutMs
    
End Property

Public Property Let ReceiveTimeoutMs(ByVal lNewValue As Long)
'MsgBox "DRIVERHELPER.SERIAL let Receivetimeoutms"
    If lNewValue <= 0 Or lNewValue > 120000 Then
        Err.Raise SCODE_INVALID_TIMEOUT, _
                    ERR_SOURCE_SERIAL, _
                    MSG_INVALID_TIMEOUT
    End If
    
    m_COM.ReceiveTimeoutMs = lNewValue
    
End Property

Public Property Get Connected() As Boolean
Attribute Connected.VB_Description = "True means the object is connected to the physical serial (COM) port. "
'MsgBox "DRIVERHELPER.SERIAL Get connected"
        Connected = m_COM.Connected
End Property

Public Property Let Connected(ByVal bNewValue As Boolean)
 '   MsgBox "DRIVERHELPER.SERIAL Let connected"
    m_COM.Connected = bNewValue
    
End Property

Public Property Get Speed() As PortSpeed
Attribute Speed.VB_Description = "The data speed at which the port runs (baud, PortSpeed enum, default 9600)"
'MsgBox "DRIVERHELPER.SERIAL Get Speed"
    Dim s As Long
    
    s = m_COM.PortSpeed
    Select Case s                    ' Baud rate
        Case 300: Speed = ps300
        Case 1200: Speed = ps1200
        Case 2400: Speed = ps2400
        Case 4800: Speed = ps4800
        Case 9600: Speed = ps9600
        Case 14400: Speed = ps14400
        Case 19200: Speed = ps19200
        Case 28800: Speed = ps28800
        Case 38400: Speed = ps38400
        Case 57600: Speed = ps57600
        Case 115200: Speed = ps115200
        Case Else: Err.Raise SCODE_UNSUP_SPEED, _
                            ERR_SOURCE_SERIAL, _
                            MSG_UNSUP_SPEED  ' Should never happen
    End Select
            
End Property

Public Property Let Speed(ByVal eNewValue As PortSpeed)
'MsgBox "DRIVERHELPER.SERIAL Let Speed"
    Select Case eNewValue                    ' Baud rate part
        Case ps300: m_COM.PortSpeed = 300
        Case ps1200: m_COM.PortSpeed = 1200
        Case ps2400: m_COM.PortSpeed = 2400
        Case ps4800: m_COM.PortSpeed = 4800
        Case ps9600: m_COM.PortSpeed = 9600
        Case ps14400: m_COM.PortSpeed = 14400
        Case ps19200: m_COM.PortSpeed = 19200
        Case ps28800: m_COM.PortSpeed = 28800
        Case ps38400: m_COM.PortSpeed = 38400
        Case ps57600: m_COM.PortSpeed = 57600
        Case ps115200: m_COM.PortSpeed = 115200
        Case Else: Err.Raise SCODE_UNSUP_SPEED, _
                            ERR_SOURCE_SERIAL, _
                            MSG_UNSUP_SPEED  ' Should never happen
    End Select
End Property

Public Sub Transmit(ByVal Data As String)
Attribute Transmit.VB_Description = "Send the string of text data to the serial port"
'MsgBox "DRIVERHELPER.SERIAL Transmit"
    Call m_COM.Transmit(Data)
End Sub

Public Sub TransmitBinary(Data() As Byte)
Attribute TransmitBinary.VB_Description = "Send the binary data array to the serial port"
 '   MsgBox "DRIVERHELPER.SERIAL TransmitBinary"
    Call m_COM.TransmitBinary(Data)
        
End Sub
'
' This receives at least one character from the serial line
'
Public Function Receive() As String
Attribute Receive.VB_Description = "Receive at least one character from the serial port"
  '  MsgBox "DRIVERHELPER.SERIAL Receive"
    Receive = m_COM.Receive
    
End Function
'
' This receives one binary Byte from the serial line
'
Public Function ReceiveByte() As Byte
Attribute ReceiveByte.VB_Description = "Receive one byte from the serial port"
'MsgBox "DRIVERHELPER.SERIAL Receivebyte"
    ReceiveByte = m_COM.ReceiveByte

End Function

'
' This receives exactly the given number of characters from the serial line
'
Public Function ReceiveCounted(ByVal Count As Integer) As String
Attribute ReceiveCounted.VB_Description = "Receive exactly the given number of text characters"
'MsgBox "DRIVERHELPER.SERIAL Receivecounted"
     ReceiveCounted = m_COM.ReceiveCounted(Count)
    
End Function
'
' This receives until a given terminator string is seen
'
Public Function ReceiveTerminated(ByVal Terminator As String) As String
Attribute ReceiveTerminated.VB_Description = "Receive text until the given terminator string is seen"
'MsgBox "DRIVERHELPER.SERIAL Receive terminated"
     If Terminator = "" Then Err.Raise SCODE_EMPTY_TERM, ERR_SOURCE_SERIAL, MSG_EMPTY_TERM
     
     ReceiveTerminated = m_COM.ReceiveTerminated(Terminator)

End Function
'
' This receives exactly the given number of bytes from the serial line
'
Public Function ReceiveCountedBinary(ByVal Count As Integer) As Byte()
'MsgBox "DRIVERHELPER.SERIAL ReceiveCountedBinary"
     ReceiveCountedBinary = m_COM.ReceiveCountedBinary(Count)
    
End Function
'
' This receives until a given terminator string is seen
'
Public Function ReceiveTerminatedBinary(Terminator() As Byte) As Byte()
'MsgBox "DRIVERHELPER.SERIAL ReceiveTerminatedBinary"
   ReceiveTerminatedBinary = m_COM.ReceiveTerminatedBinary(Terminator)
End Function
'
' Clear the input and output buffers.
'
Public Sub ClearBuffers()
Attribute ClearBuffers.VB_Description = "Empties both the incoming and outgoing buffers for the serial port."
'        MsgBox "DRIVERHELPER.SERIAL Clearbuffers"
    m_COM.ClearBuffers
    
End Sub

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

