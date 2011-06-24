VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "MNCP"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
'---------------------------------------------------------------------
'   ========
'   MNCP.CLS
'   ========
'
' Implementation of Merlin Controls "Merlin Network Communications
' Protocol" - MNCP. This is used to encapsulate data flow between
' a telescope controller and the TCS program that speaks the Merlin
' Astronomy Command Language (ACL).
'
' By passing in a serial object, it is possible to create multiple
' instances of this object which share the same COM port, and thus
' implement the bus-addressing. This eliminates the need to pass
' the address, timeout, etc. as parameters in every method call.
' Instead, these are properties of this class.
'
' NOTE: You set the baud, timeout, etc. on the serial object itself.
'       This class does not implement or wrap serial properties.
'
' Written:  15-Oct-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 15-Oct-00 rbd     Initial edit. It works!
' 30-Apr-01 rbd     Fix FromSlave() - Infinite loop if TransmitBinary
'                   fails, and there was a DoEvents() in it! BAD!
'                   We cannot allow re-entry during MNCP state
'                   switches. Add error trapping for serial send
'                   failures.
' 05-Apr-02 rbd     Don't execute Resume in FromSlave unless really
'                   handling a raised error.
'---------------------------------------------------------------------
Option Explicit

' ----------------
' MNCP Definitions
' ----------------
Private Const MNCP_DATA_MAX As Integer = 250
Private Const MNCP_PACKET_MIN As Integer = 8
Private Const MNCP_IN_BUFFER_MAX As Integer = _
                (MNCP_DATA_MAX + MNCP_PACKET_MIN + 10)
Private Const MNCP_OUT_BUFFER_MAX As Integer = _
                ((MNCP_DATA_MAX + MNCP_PACKET_MIN) * 2 + 10)

Private Const MNCP_RESET As Byte = &H0
Private Const MNCP_DATA As Byte = &H1
Private Const MNCP_ACK As Byte = &H2
Private Const MNCP_ACKDATA As Byte = &H3

Private Const MNCP_STAT_NO_PACKET As Integer = 0
Private Const MNCP_STAT_NEW_PACKET As Integer = 1
Private Const MNCP_STAT_NEW_GLOBAL As Integer = 2
Private Const MNCP_STAT_FAILURE As Integer = -1

Private Const DLE As Byte = &H10
Private Const STX As Byte = &H2
Private Const ETX As Byte = &H3

'
' Packet state variables
'
Private m_bDLE As Boolean                       ' Seen DLE escape (receive)
Private m_bInPacket As Boolean                  ' In 'read packet' state (receive)
Private m_yRBuffer(MNCP_IN_BUFFER_MAX) As Byte  ' Input buffer
Private m_lRLength As Long                      ' Index of 1st unused byte in input buffer (0-based)
Private m_ySBuffer() As Byte                    ' Output buffer (dynamic)
Private m_lSLength As Long                      ' Index of 1st unused byte in output buffer (0-based)
Private m_lS1 As Long                           ' Send checksum accumulators
Private m_lS2 As Long
Private m_ySequence(256) As Byte                ' Sequence numbers for each bus address
Private m_lTries As Long                        ' Retry counter (counts down)
'
' MNCP communication parameters
'
Private m_yBusAddress As Byte                   ' Bus address (usually 1)
Private m_lRetries As Integer                   ' # of retries before failure
Private m_oSerial As DriverHelper.Serial        ' Serial port object (copy of client's)
'
' ----------------------
' Constructor/Destructor
' ----------------------
'
Private Sub Class_Initialize()
    Dim i As Integer
    
    Set m_oSerial = Nothing
    For i = 0 To 255
        m_ySequence(i) = 0
    Next i
    '
    ' Cannot call ResetINput as m_oSerial not yet set
    '
    m_bDLE = False
    m_bInPacket = False
    m_lRLength = 0
    
End Sub

Private Sub Class_Terminate()

    Set m_oSerial = Nothing
    
End Sub
'
' -----------------
' Public properties
' -----------------
'
Public Property Set SerialObject(SerObj As DriverHelper.Serial)

    Set m_oSerial = SerObj
    
End Property

Public Property Let Address(newVal As Byte)

    m_yBusAddress = newVal
    
End Property

Public Property Get Address() As Byte

    Address = m_yBusAddress
    
End Property

Public Property Let Retries(newVal As Integer)

    m_lRetries = newVal
    
End Property

Public Property Get Retries() As Integer

    Retries = m_lRetries
    
End Property
'
' ==============
' Public Methods
' ==============
'

' -----------
' FromSlave() - Receive a message from the slave
' -----------
'
' Returns only when data is received, raises an error if fails.
'
Public Function FromSlave() As String
    Dim i As Integer
    Dim b() As Byte
    Dim seq As Byte, typ As Byte
    
    If m_yBusAddress = 0 Then                       ' ????
        m_lTries = 1
    Else
        m_lTries = m_lRetries + 1
    End If
    
    Do While m_lTries > 0                           ' Loop till valid data packet or retries exhausted
        On Error GoTo RP_TMO
        ReadPacket
        If ChecksumOK() Then
            If m_yRBuffer(0) = m_yBusAddress Then           ' If it's addressed to us
                seq = (m_yRBuffer(1) / 16) And &HF
                If seq = m_ySequence(m_yBusAddress) Then    ' If it has the correct sequence #
                    typ = m_yRBuffer(1) And &HF
                    If (typ = MNCP_ACK) Or (typ = MNCP_ACKDATA) Then    ' If it's a data packet
                        ReDim b(m_lRLength - 4)             ' Room only for payload
                        For i = 0 To (m_lRLength - 5)       ' Copy payload into b()
                            b(i) = m_yRBuffer(i + 2)
                        Next i
                        FromSlave = StrConv(b, vbUnicode)   ' Return payload as string
                        Exit Function
                    End If
                End If
            End If
        End If
        GoTo RP_RSUM
RP_TMO:
        Resume RP_RSUM
RP_RSUM:
        m_lTries = m_lTries - 1
        On Error GoTo RP_XFAIL                              ' This is really bad!
        m_oSerial.TransmitBinary m_ySBuffer                 ' Transmit byte array
    Loop
    On Error GoTo 0                                         ' Signal out of here
    FromSlave = ""
    Err.Raise SCODE_MNCP_RECVFAIL, ERR_SOURCE, MSG_MNCP_RECVFAIL
    Exit Function
    
RP_XFAIL:
    Resume RP_XFAIL_FIN
RP_XFAIL_FIN:
    On Error GoTo 0
    Err.Raise SCODE_MNCP_SENDFAIL, ERR_SOURCE, MSG_MNCP_SENDFAIL
End Function

' ---------
' ToSlave() - Send data message to the slave, wait for ACK
' ---------
'
Public Sub ToSlave(data As String)

    If m_yBusAddress = 0 Then                       ' ????
        m_lTries = 1
    Else
        m_lTries = m_lRetries + 1
    End If
    
    m_ySequence(m_yBusAddress) = (m_ySequence(m_yBusAddress) + 1) And &HF

    ResetInput
    SendPacket MNCP_DATA, data
    
End Sub

' ------------
' ResetSlave() - Reset comms from slave, sequence and retries
' ------------
'
Public Sub ResetSlave()

    If m_yBusAddress = 0 Then                       ' ????
        m_lTries = 1
    Else
        m_lTries = m_lRetries + 1
    End If
    
    m_ySequence(m_yBusAddress) = 0                  ' Reset sequence #

    SendPacket MNCP_RESET, ""                       ' Twig slave
    FromSlave                                       ' Must respond
    
End Sub

'
' ===============
' Private Methods
' ===============

'
' Receiving
'

' ------------
' ReadPacket() - Read an MNCP packet into the buffer
' ------------
'
' Returns with <addr><ctrl><data><chksum> in m_yRBuffer()
'
Private Sub ReadPacket()
    Dim b As Byte
    
    Do While True                                   ' (see Exit Do below)
        b = m_oSerial.ReceiveByte()
        If m_bDLE Then                              ' Seen DLE 'escape'
            Select Case b
                Case STX:                           ' Start a packet
                    m_lRLength = 0
                    m_bInPacket = True
                Case ETX:                           ' End if m_bInPacket = True
                    If m_bInPacket Then
                        m_bInPacket = False
                        Exit Do                     ' Packet received, exit loop
                    End If
                Case DLE:
                    If m_bInPacket Then             ' "stuffed" DLE
                        m_yRBuffer(m_lRLength) = DLE
                        m_lRLength = m_lRLength + 1
                    End If
            End Select                              ' Anything else is ignored in DLE state
            m_bDLE = False
        Else
            If b = DLE Then                         ' DLE escape?
                m_bDLE = True                       ' Enter DLE state
            ElseIf m_bInPacket Then                 ' In packet state?
                m_yRBuffer(m_lRLength) = b            ' Append byte to buffer
                m_lRLength = m_lRLength + 1
            End If
        End If
        If m_lRLength >= MNCP_IN_BUFFER_MAX Then    ' Corrupted, missed <DLE><ETX>
            m_lRLength = 0
            m_bInPacket = False
        End If
    Loop                                            ' Can fail via a COM timeout
    
End Sub

' ------------
' ChecksumOK() - Check packet's checksum
' ------------
'
Private Function ChecksumOK() As Boolean
    Dim i As Integer
    Dim z As Integer
    Dim s As Long
    
    z = m_lRLength - 1
    s = 0
    For i = 0 To z
        s = s + m_yRBuffer(i)
    Next i
    ChecksumOK = ((s Mod 255) = 0)

End Function

' ------------
' ResetInput() - Reset input buffer state
' ------------
'
Private Sub ResetInput()

    m_bDLE = False
    m_bInPacket = False
    m_lRLength = 0
    m_oSerial.ClearBuffers

End Sub

'
' Sending
'
' ---------
' AddByte() - Add a byte to the output buffer with DLE-stuffing
' ---------
'
Private Sub AddByte(b As Byte)

    If m_lSLength >= MNCP_OUT_BUFFER_MAX Then _
        Err.Raise SCODE_MNCP_SBUFOVFLW, ERR_SOURCE, MSG_MNCP_SBUFOVFLW
        
    m_ySBuffer(m_lSLength) = b                      ' Add this byte to the buffer
    m_lSLength = m_lSLength + 1
    If b = DLE Then
        m_ySBuffer(m_lSLength) = b                  ' DLE stuffing
        m_lSLength = m_lSLength + 1
    End If
    
End Sub

' --------------
' ChecksumByte() - Accumulate a byte into the checksum
' --------------
'
Private Sub ChecksumByte(ByVal b As Byte)

    m_lS1 = m_lS1 + b
    If m_lS1 >= 255 Then m_lS1 = m_lS1 - 255
    m_lS2 = m_lS2 + m_lS1
    If m_lS2 >= 255 Then m_lS2 = m_lS2 - 255

End Sub

' ------------
' SendPacket() - Frame and transmit a packet
' ------------
'
' Uses dynamic array sizing to finish with m_ySBuffer dimensioned
' to the size of the packet. This permits us to call Serial.TransmitBinary
' without copying (though the ReDim Preserve copies!!)
'
Private Function SendPacket(typ As Byte, data As String)
    Dim temp As Byte
    Dim csum As Integer
    Dim i As Integer
    Dim l As Integer
    
    ReDim m_ySBuffer(MNCP_OUT_BUFFER_MAX)   ' Expand buffer to max size
    m_lS1 = 0
    m_lS2 = 0
    m_lSLength = 2
    m_ySBuffer(0) = DLE
    m_ySBuffer(1) = STX
    
    temp = (m_ySequence(m_yBusAddress) * 16) + (typ And &HF) ' Form control byte
    
    AddByte m_yBusAddress                   ' Address byte
    ChecksumByte m_yBusAddress
    AddByte temp                            ' Control byte
    ChecksumByte temp
    
    l = Len(data)                           ' Length of payload string
    For i = 1 To l
        temp = CByte(Asc(Mid(data, i, 1)))  ' Byte value of string character
        AddByte temp
        ChecksumByte temp
    Next i
    
    csum = m_lS1 + m_lS2
    If csum >= 255 Then csum = csum - 255
    temp = 255 - csum
    AddByte temp
    csum = m_lS1 + temp
    If csum >= 255 Then csum = csum - 255
    temp = 255 - csum
    AddByte temp
    
    m_ySBuffer(m_lSLength) = DLE            ' Packet trailer
    m_lSLength = m_lSLength + 1             ' Don't stuff on this!
    m_ySBuffer(m_lSLength) = ETX
    m_lSLength = m_lSLength + 1
    
    ReDim Preserve m_ySBuffer(m_lSLength)    ' Shrink to actual length
    On Error GoTo SP_XFAIL
    m_oSerial.TransmitBinary m_ySBuffer     ' Transmit byte array
    Exit Function
    
SP_XFAIL:
    Resume SP_XFAIL_FIN
SP_XFAIL_FIN:
    On Error GoTo 0
    Err.Raise SCODE_MNCP_SENDFAIL, ERR_SOURCE, MSG_MNCP_SENDFAIL

End Function

