'Implements the Serial component

Option Strict On
Option Explicit On

Imports System.IO.Ports
Imports ASCOM.Utilities.Interfaces
Imports ASCOM.Utilities.Exceptions
Imports System.Runtime.InteropServices
Imports System.Threading.Thread
Imports System.Runtime.Remoting.Contexts

#Region "Enums"
'PortSpeed enum
''' <summary>
''' Enumeration of serial port speeds for use with the Serial port
''' </summary>
''' <remarks>This contains an additional speed 230,400 baud that the COM component doesn't support.</remarks>
<Guid("06BB25BA-6E75-4d1b-8BB4-BA629454AE38"), _
ComVisible(True)> _
Public Enum SerialSpeed As Integer 'Defined at ACOM.Utilities level
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
    ps230400 = 230400
End Enum

''' <summary>
''' Number of stop bits appended to a serial character
''' </summary>
''' <remarks>
''' This enumeration specifies the number of stop bits to use. Stop bits separate each unit of data 
''' on an asynchronous serial connection. 
''' The None option is not supported. Setting the StopBits property to None raises an 
''' ArgumentOutOfRangeException. 
''' </remarks>
<Guid("17D30DAA-C767-43fd-8AF4-5E149D5C771F"), _
ComVisible(True)> _
Public Enum SerialStopBits
    ''' <summary>
    ''' No stop bits are used. This value is not supported. Setting the StopBits property to None raises an ArgumentOutOfRangeException. 
    ''' </summary>
    ''' <remarks></remarks>
    None = 0
    ''' <summary>
    ''' One stop bit is used. 
    ''' </summary>
    ''' <remarks></remarks>
    One = 1
    ''' <summary>
    ''' 1.5 stop bits are used. 
    ''' </summary>
    ''' <remarks></remarks>
    OnePointFive = 3
    ''' <summary>
    ''' Two stop bits are used. 
    ''' </summary>
    ''' <remarks></remarks>
    Two = 2
End Enum

''' <summary>
''' The type of parity used on the serial port
''' </summary>
''' <remarks>
''' Parity is an error-checking procedure in which the number of 1s must always be the 
''' same � either even or odd � for each group of bits that is transmitted without error. 
''' Parity is one of the parameters that must be 
''' agreed upon by both sending and receiving parties before transmission can take place. 
''' </remarks>
<Guid("92B19711-B44F-4642-9F96-5A20397B8FD1"), _
ComVisible(True)> _
Public Enum SerialParity
    ''' <summary>
    ''' Sets the parity bit so that the count of bits set is an even number. 
    ''' </summary>
    ''' <remarks></remarks>
    Even = 2
    ''' <summary>
    ''' Leaves the parity bit set to 1. 
    ''' </summary>
    ''' <remarks></remarks>
    Mark = 3
    ''' <summary>
    ''' No parity check occurs. 
    ''' </summary>
    ''' <remarks></remarks>
    None = 0
    ''' <summary>
    ''' Sets the parity bit so that the count of bits set is an odd number. 
    ''' </summary>
    ''' <remarks></remarks>
    Odd = 1
    ''' <summary>
    ''' Leaves the parity bit set to 0. 
    ''' </summary>
    ''' <remarks></remarks>
    Space = 4
End Enum

''' <summary>
''' The control protocol used by the serial port
''' </summary>
''' <remarks></remarks>
<Guid("55A24A18-02C8-47df-A048-2E0982E1E4FE"), _
ComVisible(True)> _
Public Enum SerialHandshake
    ''' <summary>
    ''' No control is used for the handshake. 
    ''' </summary>
    ''' <remarks></remarks>
    None = 0
    ''' <summary>
    ''' Request-to-Send (RTS) hardware flow control is used. RTS signals that data is available 
    ''' for transmission. If the input buffer becomes full, the RTS line will be set to false. 
    ''' The RTS line will be set to true when more room becomes available in the input buffer. 
    ''' </summary>
    ''' <remarks></remarks>
    RequestToSend = 2
    ''' <summary>
    ''' Both the Request-to-Send (RTS) hardware control and the XON/XOFF software controls are used. 
    ''' </summary>
    ''' <remarks></remarks>
    RequestToSendXonXoff = 3
    ''' <summary>
    ''' The XON/XOFF software control protocol is used. The XOFF control is sent to stop 
    ''' the transmission of data. The XON control is sent to resume the transmission. 
    ''' These software controls are used instead of Request to Send (RTS) and Clear to Send (CTS) 
    ''' hardware controls. 
    ''' </summary>
    ''' <remarks></remarks>
    XonXoff = 1
End Enum
#End Region

'<SynchronizationAttribute(SynchronizationAttribute.REQUIRES_NEW, False)> _

''' <summary>
''' Creates a .NET serial port and provides a simple set of commands to use it.
''' </summary>
''' <remarks>This object provides an easy to use interface to a serial (COM) port. 
''' It provides ASCII and binary I/O with controllable timeout.
''' The interface is callable from any .NET client.</remarks>
''' <example>
''' Example of how to create and use an ASCOM serial port.
''' <code lang="vbnet" title="ASCOM Serial Port Example" 
''' source="..\..\ASCOM Platform Examples\Examples\SerialExamples.vb"
''' />
'''</example>
<ClassInterface(ClassInterfaceType.None), _
Guid("0B8E6DC4-7451-4484-BE15-0D0727569FB8"), _
ComVisible(True)> _
Public Class Serial
    Implements ISerial, IDisposable

    'State variables holding com port configuration
    Private m_Port As SerialPort
    Private m_PortName As String
    Private m_ReceiveTimeout As Integer
    Private m_Speed As SerialSpeed
    Private m_Connected As Boolean
    Private m_DataBits As Integer
    Private m_DTREnable As Boolean
    Private m_Handshake As SerialHandshake
    Private m_Parity As SerialParity
    Private m_StopBits As SerialStopBits

    Private TraceFile As String 'Current trace file name
    Private disposed As Boolean = False 'IDisposable variable to detect redundant calls
    Private DebugTrace As Boolean = True 'Flag specifying type of information to record in log file

    Private Logger As TraceLogger
    Private TextEncoding As System.Text.Encoding
    Private m_SerTraceFile As String = SERIAL_DEFAULT_FILENAME 'Set the default trace file name

    Private SerSemaphore As System.Threading.Semaphore
    Private SerPortInUse As Boolean = False

    Private CallCountSemaphore As New System.Threading.Semaphore(1, 1)
    Private CallCount As Long ' Counter for calls to this component

    Private Const TIMEOUT_NUMBER As Integer = vbObjectError + &H402
    Private Const TIMEOUT_MESSAGE As String = "Timed out waiting for received data"
    Private Const SEMAPHORE_TIMEOUT As Integer = 1000

    'Serial port parameters
    Private Const SERIALPORT_ENCODING As Integer = 1252
    Private Const SERIALPORT_DEFAULT_NAME As String = "COM1"
    Private Const SERIALPORT_DEFAULT_TIMEOUT As Integer = 5000
    Private Const SERIALPORT_DEFAULT_SPEED As SerialSpeed = SerialSpeed.ps9600
    Private Const SERIALPORT_DEFAULT_DATABITS As Integer = 8
    Private Const SERIALPORT_DEFAULT_DTRENABLE As Boolean = True
    Private Const SERIALPORT_DEFAULT_HANDSHAKE As SerialHandshake = SerialHandshake.None
    Private Const SERIALPORT_DEFAULT_PARITY As SerialParity = SerialParity.None
    Private Const SERIALPORT_DEFAULT_STOPBITS As SerialStopBits = SerialStopBits.One


#Region "New and IDisposable Support"
    Sub New()
        Dim SerialProfile As XMLAccess = Nothing
        Dim TraceFileName As String = ""

        SerSemaphore = New System.Threading.Semaphore(1, 1) 'Create a new semaphore to control access to the serial port

        m_Connected = False 'Set inital values
        m_PortName = SERIALPORT_DEFAULT_NAME
        m_ReceiveTimeout = SERIALPORT_DEFAULT_TIMEOUT
        m_Speed = SERIALPORT_DEFAULT_SPEED
        m_DataBits = SERIALPORT_DEFAULT_DATABITS
        m_DTREnable = SERIALPORT_DEFAULT_DTRENABLE
        m_Handshake = SERIALPORT_DEFAULT_HANDSHAKE
        m_Parity = SERIALPORT_DEFAULT_PARITY
        m_StopBits = SERIALPORT_DEFAULT_STOPBITS

        TextEncoding = System.Text.Encoding.GetEncoding(SERIALPORT_ENCODING) 'Initialise text encoding for use by transmitbinary
        Try
            SerialProfile = New XMLAccess 'Profile class that can retrieve the value of tracefile
            TraceFileName = SerialProfile.GetProfile("", SERIAL_FILE_NAME_VARNAME)
            Logger = New TraceLogger(TraceFileName, "Serial")
            If TraceFileName <> "" Then Logger.Enabled = True
        Catch ex As Exception
            MsgBox("Serial:New exception " & ex.ToString)
        End Try

        'Clean up
        Try : SerialProfile.Dispose() : Catch : End Try
        SerialProfile = Nothing
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    ''' <summary>
    ''' Disposes of resources used by the profile object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ''' <summary>
    ''' Disposes of resources used by the profile object - called by IDisposable interface
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposed Then
            If Not m_Port Is Nothing Then 'Clean up the port
                Try : m_Port.Dispose() : Catch : End Try
                m_Port = Nothing
            End If
            If Not Logger Is Nothing Then 'Clean up the logger
                Try : Logger.Dispose() : Catch : End Try
                Logger = Nothing
            End If
            If Not SerSemaphore Is Nothing Then 'Clean up the semaphore
                Try : SerSemaphore.Release() : Catch : End Try
                Try : SerSemaphore.Close() : Catch : End Try
                SerSemaphore = Nothing
            End If
        End If

        Me.disposed = True
    End Sub
#End Region

#Region "ISerial Implementation"
    ''' <summary>
    ''' Gets or sets the number of data bits in each byte
    ''' </summary>
    ''' <value>The number of data bits in each byte, default is 8 data bits</value>
    ''' <returns>Integer number of data bits in each byte</returns>
    ''' <exception cref="ArgumentOutOfRangeException">The data bits value is less than 5 or more than 8</exception>
    ''' <remarks></remarks>
    Public Property DataBits() As Integer Implements ISerial.DataBits
        Get
            Return m_DataBits
        End Get
        Set(ByVal NumDataBits As Integer)
            m_DataBits = NumDataBits
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the state of the DTR line
    ''' </summary>
    ''' <value>The state of the DTR line, default is enabled</value>
    ''' <returns>Boolean true/false indicating enabled/disabled</returns>
    ''' <remarks></remarks>
    Public Property DTREnable() As Boolean Implements ISerial.DTREnable
        Get
            Return m_DTREnable
        End Get
        Set(ByVal Enabled As Boolean)
            m_DTREnable = Enabled
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the type of serial handshake used on the serial link
    ''' </summary>
    ''' <value>The type of flow control handshake used on the serial line, default is none</value>
    ''' <returns>One of the SerialHandshake enumeration values</returns>
    ''' <remarks></remarks>
    Public Property Handshake() As SerialHandshake Implements ISerial.Handshake
        Get
            Return m_Handshake
        End Get
        Set(ByVal HandshakeType As SerialHandshake)
            m_Handshake = HandshakeType
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the type of parity check used over the serial link
    ''' </summary>
    ''' <value>The type of parity check used over the serial link, default none</value>
    ''' <returns>One of the SerialParity enumeration values</returns>
    ''' <remarks></remarks>
    Public Property Parity() As SerialParity Implements ISerial.Parity
        Get
            Return m_Parity
        End Get
        Set(ByVal ParityType As SerialParity)
            m_Parity = ParityType
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the number of stop bits used on the serial link
    ''' </summary>
    ''' <value>the number of stop bits used on the serial link, default 1</value>
    ''' <returns>One of the SerialStopBits enumeration values</returns>
    ''' <remarks></remarks>
    Public Property StopBits() As SerialStopBits Implements ISerial.StopBits
        Get
            Return m_StopBits
        End Get
        Set(ByVal NumStopBits As SerialStopBits)
            m_StopBits = NumStopBits
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the connected state of the ASCOM serial port.
    ''' </summary>
    ''' <value>Connected state of the serial port.</value>
    ''' <returns><c>True</c> if the serial port is connected.</returns>
    ''' <remarks>Set this property to True to connect to the serial (COM) port. You can read the property to determine if the object is connected. </remarks>
    ''' <exception cref="Exceptions.InvalidValueException">Throws this exception if the requested 
    ''' COM port does not exist.</exception>
    Public Property Connected() As Boolean Implements ISerial.Connected
        Get
            Return m_Connected
        End Get
        Set(ByVal Connecting As Boolean)
            Dim SerPorts As String()
            Try
                '5.0.2 added port enumeration to log
                SerPorts = AvailableCOMPorts ' This causes log entries tobe written
                Logger.LogMessage("Set Connected", "Using COM port: " & m_PortName & _
                                  " Baud rate: " & m_Speed.ToString & _
                                  " Timeout: " & m_ReceiveTimeout.ToString & _
                                  " DTR: " & m_DTREnable.ToString & _
                                  " Handshake: " & m_Handshake.ToString & _
                                  " Encoding: " & SERIALPORT_ENCODING.ToString)
                Logger.LogMessage("Set Connected", "Transmission format - Bits: " & m_DataBits & _
                                  " Parity: " & [Enum].GetName(m_Parity.GetType, m_Parity) & _
                                  " Stop bits: " & [Enum].GetName(m_StopBits.GetType, m_StopBits))

                Logger.LogStart("Set Connected", Connecting.ToString & " ")
                If Connecting Then ' Trying to connect
                    If Not m_Connected Then
                        If Not My.Computer.Ports.SerialPortNames.Contains(m_PortName) Then Throw New Exceptions.InvalidValueException("Requested COM Port does not exist: " & m_PortName)
                        If m_Port Is Nothing Then m_Port = New System.IO.Ports.SerialPort(m_PortName)

                        'Set up serial port
                        m_Port.BaudRate = m_Speed
                        m_Port.ReadTimeout = m_ReceiveTimeout
                        m_Port.WriteTimeout = m_ReceiveTimeout

                        'Ensure we can get all 256 possible values, default encoding is ASCII that only gives first 127 values
                        m_Port.Encoding = System.Text.Encoding.GetEncoding(SERIALPORT_ENCODING)

                        'Set handshaking and control signals
                        m_Port.DtrEnable = m_DTREnable
                        m_Port.Handshake = CType(m_Handshake, System.IO.Ports.Handshake)

                        'Set transmission format
                        m_Port.DataBits = m_DataBits
                        m_Port.Parity = CType(m_Parity, System.IO.Ports.Parity)
                        m_Port.StopBits = CType(m_StopBits, System.IO.Ports.StopBits)

                        'Open port for communication
                        m_Port.Open()
                        m_Connected = True
                        Logger.LogFinish("OK")
                    Else
                        Logger.LogFinish("already connected")
                    End If
                Else ' Trying to disconnect
                    If m_Connected Then
                        m_Connected = False
                        m_Port.DiscardOutBuffer()
                        m_Port.DiscardInBuffer()
                        Logger.LogContinue("cleared buffers, ")
                        m_Port.Close()
                        Logger.LogContinue("closed, ")
                        m_Port.Dispose()
                        Logger.LogFinish("disposed OK")
                    Else
                        Logger.LogFinish("already disconnected")
                    End If
                End If
            Catch ex As Exception
                Logger.LogFinish("EXCEPTION: " & ex.Message & ex.ToString)
                Throw
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the number of the ASCOM serial port (Default is 1, giving COM1 as the serial port name).
    ''' </summary>
    ''' <value>COM port number of the ASCOM serial port.</value>
    ''' <returns>Integer, number of the ASCOM serial port</returns>
    ''' <remarks>This works for serial port names of the form COMnnn. Use PortName if your COM port name does not fit the form COMnnn.</remarks>
    Public Property Port() As Integer Implements ISerial.Port
        'Set and get port names of the form COMnn. This only works with port names of that form
        'See PortName for a more flexible version that can deal with any valid port name.
        Get
            Return CInt(Mid(m_PortName, 4))
        End Get
        Set(ByVal value As Integer)
            m_PortName = "COM" & value.ToString
            Logger.LogMessage("Port", "Set to: " & value.ToString)
        End Set
    End Property

    ''' <summary>
    ''' The maximum time that the ASCOM serial port will wait for incoming receive data (seconds, default = 5)
    ''' </summary>
    ''' <value>Integer, serial port timeout in seconds</value>
    ''' <returns>Integer, serial port timeout in seconds.</returns>
    ''' <remarks>The minimum delay timout that can be set through this command is 1 seconds. Use ReceiveTimeoutMs to set a timeout less than 1 second.</remarks>
    ''' <exception cref="InvalidValueException">Thrown when <paramref name="value"><c>=0</c></paramref> is invalid (outside the range 1 to 120 seconds.</exception>
    Public Property ReceiveTimeout() As Integer Implements ISerial.ReceiveTimeout
        'Get and set the receive timeout
        Get
            Return CInt(m_ReceiveTimeout / 1000)
        End Get
        Set(ByVal value As Integer)
            Dim MyCount As Long
            MyCount = GetNextCount("ReceiveTimeout")
            If DebugTrace Then Logger.LogMessage("ReceiveTimeout", MyCount.ToString & " Set Start - TimeOut: " & value & " seconds")

            value = value * 1000 ' Timeout is measured in milliseconds
            If (value <= 0) Or (value > 120000) Then Throw New InvalidValueException("ReceiveTimeout", Format(CDbl(value / 1000), "0.0"), "0 to 120 seconds")
            m_ReceiveTimeout = value
            If m_Connected Then
                If DebugTrace Then Logger.LogMessage("ReceiveTimeout", MyCount.ToString & " Connected so writing to serial port")
                If GetSemaphore("ReceiveTimeout", MyCount) Then
                    Try
                        m_Port.WriteTimeout = value
                        m_Port.ReadTimeout = value
                        Logger.LogMessage("ReceiveTimeout", MyCount.ToString & " Written to serial port OK")
                    Catch ex As Exception
                        Logger.LogMessage("ReceiveTimeout", MyCount.ToString & " EXCEPTION: " & ex.ToString)
                        Throw
                    Finally
                        ReleaseSemaphore("ReceiveTimeout", MyCount)
                    End Try
                Else
                    Logger.LogMessage("ReceiveTimeout", MyCount.ToString & " Throwing SerialPortInUse exception")
                    Throw New SerialPortInUseException("Serial:ReceiveTimeout - unable to get serial port semaphore before timeout.")
                End If
            End If
            Logger.LogMessage("ReceiveTimeout", MyCount.ToString & " Set to: " & value / 1000 & " seconds")
        End Set
    End Property

    ''' <summary>
    ''' The maximum time that the ASCOM serial port will wait for incoming receive data (milliseconds, default = 5000)
    ''' </summary>
    ''' <value>Integer, serial port timeout in milli-seconds</value>
    ''' <returns>Integer, serial port timeout in milli-seconds</returns>
    ''' <remarks>If a timeout occurs, an IO timeout error is raised. See ReceiveTimeout for an alternate form 
    ''' using the timeout in seconds. </remarks>
    ''' <exception cref="InvalidValueException">Thrown when <paramref name="value"><c>=0</c></paramref> is invalid.</exception>
    Public Property ReceiveTimeoutMs() As Integer Implements ISerial.ReceiveTimeoutMs
        'Get and set the receive timeout in ms
        Get
            Return m_ReceiveTimeout
        End Get
        Set(ByVal value As Integer)
            Dim MyCount As Long
            MyCount = GetNextCount("ReceiveTimeoutMs")
            If DebugTrace Then Logger.LogMessage("ReceiveTimeoutMs", MyCount.ToString & " Set Start - TimeOut: " & value.ToString & " mS")

            If (value <= 0) Or (value > 120000) Then Throw New InvalidValueException("ReceiveTimeoutMs", value.ToString, "1 to 120000 milliseconds")
            m_ReceiveTimeout = value
            If m_Connected Then
                If DebugTrace Then Logger.LogMessage("ReceiveTimeoutMs", MyCount.ToString & " Connected so writing to serial port")
                If GetSemaphore("ReceiveTimeoutMs", MyCount) Then
                    Try
                        m_Port.WriteTimeout = value
                        m_Port.ReadTimeout = value
                        Logger.LogMessage("ReceiveTimeoutMs", MyCount.ToString & " Written to serial port OK")
                    Catch ex As Exception
                        Logger.LogMessage("ReceiveTimeoutMs", MyCount.ToString & " EXCEPTION: " & ex.ToString)
                        Throw
                    Finally
                        ReleaseSemaphore("ReceiveTimeoutMs", MyCount)
                    End Try
                Else
                    Logger.LogMessage("ReceiveTimeoutMs", MyCount.ToString & " Throwing SerialPortInUse exception")
                    Throw New SerialPortInUseException("Serial:ReceiveTimeoutMs - unable to get serial port semaphore before timeout.")
                End If
            End If
            Logger.LogMessage("ReceiveTimeoutMs", MyCount.ToString & " Set to: " & value.ToString & " mS")
        End Set
    End Property

    ''' <summary>
    ''' Gets and sets the baud rate of the ASCOM serial port
    ''' </summary>
    ''' <value>Port speed using the PortSpeed enum</value>
    ''' <returns>Port speed as a SerialSpeed enum value</returns>
    ''' <remarks>This is modelled on the COM component with possible values enumerated in the PortSpeed enum.</remarks>
    Public Property Speed() As SerialSpeed Implements ISerial.Speed
        'Get and set the port speed using the portspeed enum
        Get
            Return m_Speed
        End Get
        Set(ByVal value As SerialSpeed)
            m_Speed = value
            Logger.LogMessage("Speed", "Set to: " & value.ToString)
        End Set
    End Property

    ''' <summary>
    ''' Clears the ASCOM serial port receive and transmit buffers
    ''' </summary>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <remarks></remarks>
    Public Sub ClearBuffers() Implements ISerial.ClearBuffers
        'Clear both comm buffers
        Dim MyCount As Long
        MyCount = GetNextCount("ClearBuffers")
        If DebugTrace Then Logger.LogMessage("ClearBuffers", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Start")
        If GetSemaphore("ClearBuffers", MyCount) Then
            Try
                m_Port.DiscardInBuffer()
                m_Port.DiscardOutBuffer()
                Logger.LogMessage("ClearBuffers", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Completed")
            Catch ex As Exception
                Logger.LogMessage("ClearBuffers", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " EXCEPTION: " & ex.ToString)
                Throw
            Finally
                ReleaseSemaphore("ClearBuffers ", MyCount)
            End Try
        Else
            Logger.LogMessage("ClearBuffers", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Throwing SerialPortInUse exception")
            'Throw New SerialPortInUseException("Serial:Clearbuffers - unable to get serial port semaphore before timeout.")
            Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)

        End If
    End Sub

    ''' <summary>
    ''' Receive at least one text character from the ASCOM serial port
    ''' </summary>
    ''' <returns>The characters received</returns>
    ''' <exception cref="System.TimeoutException">Thrown when a receive timeout occurs.</exception>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <remarks>This method reads all of the characters currently in the serial receive buffer. It will not return 
    ''' unless it reads at least one character. A timeout will cause a TimeoutException to be raised. Use this for 
    ''' text data, as it returns a String. </remarks>
    Public Function Receive() As String Implements ISerial.Receive
        'Return all characters in the receive buffer
        Dim Received As String = ""
        Dim MyCount As Long
        MyCount = GetNextCount("Receive")
        If DebugTrace Then Logger.LogMessage("Receive", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Start")
        If GetSemaphore("Receive", MyCount) Then
            Try
                If Not DebugTrace Then Logger.LogStart("Receive", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " < ")
                Received = ReadChar("Receive", MyCount)
                Received = Received & m_Port.ReadExisting
                If DebugTrace Then
                    Logger.LogMessage("Receive", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " < " & Received)
                Else
                    Logger.LogFinish(Received)
                End If
            Catch ex As TimeoutException
                Logger.LogMessage("Receive", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.ToString)
                Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
            Catch ex As Exception
                Logger.LogMessage("Receive", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.ToString)
                Throw
            Finally 'Ensure we release the semaphore whatever happens
                ReleaseSemaphore("Receive", MyCount)
            End Try
            Return Received
        Else
            Logger.LogMessage("Receive", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Throwing SerialPortInUse exception")
            Throw New SerialPortInUseException("Serial:Receive - unable to get serial port semaphore before timeout.")
        End If
    End Function

    ''' <summary>
    ''' Receive one binary byte from the ASCOM serial port
    ''' </summary>
    ''' <returns>The received byte</returns>
    ''' <exception cref="System.TimeoutException">Thrown when a receive timeout occurs.</exception>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <remarks>Use this for 8-bit (binary data). If a timeout occurs, a TimeoutException is raised. </remarks>
    Public Function ReceiveByte() As Byte Implements ISerial.ReceiveByte
        'Return a single byte of data
        Dim RetVal As Byte
        Dim MyCount As Long
        MyCount = GetNextCount("ReceiveByte")
        If DebugTrace Then Logger.LogMessage("ReceiveByte", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Start")
        If GetSemaphore("ReceiveByte", MyCount) Then
            Try
                If Not DebugTrace Then Logger.LogStart("ReceiveByte", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " ")
                RetVal = ReadByte("ReceiveByte", MyCount)
                If DebugTrace Then
                    Logger.LogMessage("ReceiveByte", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " " & Chr(RetVal), True)
                Else
                    Logger.LogFinish(Chr(RetVal), True)
                End If
            Catch ex As TimeoutException
                Logger.LogMessage("ReceiveByte", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " EXCEPTION: " & ex.ToString)
                Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
            Catch ex As Exception
                Logger.LogMessage("ReceiveByte", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " EXCEPTION: " & ex.ToString)
                Throw
            Finally 'Ensure we release the semaphore whatever happens
                ReleaseSemaphore("ReceiveByte", MyCount)
            End Try
            Return RetVal
        Else
            Logger.LogMessage("ReceiveByte", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Throwing SerialPortInUse exception")
            Throw New SerialPortInUseException("Serial:ReceiveByte - unable to get serial port semaphore before timeout.")
        End If
    End Function

    ''' <summary>
    ''' Receive exactly the given number of characters from the ASCOM serial port and return as a string
    ''' </summary>
    ''' <param name="Count">The number of characters to return</param>
    ''' <returns>String of length "Count" characters</returns>
    ''' <exception cref="System.TimeoutException">Thrown when a receive timeout occurs.</exception>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <remarks>If a timeout occurs a TimeoutException is raised.</remarks>
    Public Function ReceiveCounted(ByVal Count As Integer) As String Implements ISerial.ReceiveCounted
        'Return a fixed number of characters
        Dim i As Integer, Received As String = ""
        Dim MyCount As Long
        MyCount = GetNextCount("ReceiveCounted")
        If DebugTrace Then Logger.LogMessage("ReceiveCounted", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Start - count: " & Count.ToString)
        If GetSemaphore("ReceiveCounted", MyCount) Then
            Try
                If Not DebugTrace Then Logger.LogStart("ReceiveCounted " & Count.ToString, MyCount.ToString & " " & CurrentThread.ManagedThreadId & " < ")
                For i = 1 To Count
                    Received = Received & ReadChar("ReceiveCounted", MyCount)
                Next
                If DebugTrace Then
                    Logger.LogMessage("ReceiveCounted", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " < " & Received)
                Else
                    Logger.LogFinish(Received)
                End If
            Catch ex As TimeoutException
                Logger.LogMessage("ReceiveCounted", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.Message)
                Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
            Catch ex As Exception
                Logger.LogMessage("ReceiveCounted", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.Message)
                Throw
            Finally 'Ensure we release the semaphore whatever happens
                ReleaseSemaphore("ReceiveCounted", MyCount)
            End Try
            Return Received
        Else
            Logger.LogMessage("ReceiveCounted", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Throwing SerialPortInUse exception")
            Throw New SerialPortInUseException("ReceiveCounted - unable to get serial port semaphore before timeout.")
        End If
    End Function

    ''' <summary>
    ''' Receive exactly the given number of characters from the ASCOM serial port and return as a byte array
    ''' </summary>
    ''' <param name="Count">The number of characters to return</param>
    ''' <returns>Byte array of size "Count" elements</returns>
    ''' <exception cref="System.TimeoutException">Thrown when a receive timeout occurs.</exception>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <remarks>
    ''' <para>If a timeout occurs, a TimeoutException is raised. </para>
    ''' <para>This function exists in the COM component but is not documented in the help file.</para>
    ''' </remarks>
    Public Function ReceiveCountedBinary(ByVal Count As Integer) As Byte() Implements ISerial.ReceiveCountedBinary
        Dim i As Integer, Received(0) As Byte
        Dim TextEncoding As System.Text.Encoding
        Dim MyCount As Long
        TextEncoding = System.Text.Encoding.GetEncoding(1252)

        MyCount = GetNextCount("ReceiveCountedBinary ")
        If DebugTrace Then Logger.LogMessage("ReceiveCountedBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Start - count: " & Count.ToString)

        If GetSemaphore("ReceiveCountedBinary ", MyCount) Then
            Try
                If Not DebugTrace Then Logger.LogStart("ReceiveCountedBinary " & Count.ToString, MyCount.ToString & " " & CurrentThread.ManagedThreadId & " < ")
                For i = 0 To Count - 1
                    ReDim Preserve Received(i)
                    Received(i) = ReadByte("ReceiveCountedBinary ", MyCount)
                Next
                If DebugTrace Then
                    Logger.LogMessage("ReceiveCountedBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " < " & TextEncoding.GetString(Received), True)
                Else
                    Logger.LogFinish(TextEncoding.GetString(Received), True)
                End If
            Catch ex As TimeoutException
                Logger.LogMessage("ReceiveCountedBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " " & FormatRXSoFar(TextEncoding.GetString(Received)) & "EXCEPTION: " & ex.Message)
                Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
            Catch ex As Exception
                Logger.LogMessage("ReceiveCountedBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " " & FormatRXSoFar(TextEncoding.GetString(Received)) & "EXCEPTION: " & ex.Message)
                Throw
            Finally
                ReleaseSemaphore("ReceiveCountedBinary ", MyCount)
            End Try
            Return Received
        Else
            Logger.LogMessage("ReceiveCountedBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Throwing SerialPortInUse exception")
            'Throw New SerialPortInUseException("Serial:ReceiveCountedBinary - unable to get serial port semaphore before timeout.")
            Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)

        End If
    End Function

    ''' <summary>
    ''' Receive characters from the ASCOM serial port until the given terminator string is seen
    ''' </summary>
    ''' <param name="Terminator">The character string that indicates end of message</param>
    ''' <returns>Received characters including the terminator string</returns>
    ''' <exception cref="System.TimeoutException">Thrown when a receive timeout occurs.</exception>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <remarks>If a timeout occurs, a TimeoutException is raised.</remarks>
    Public Function ReceiveTerminated(ByVal Terminator As String) As String Implements ISerial.ReceiveTerminated
        'Return all characters up to and including a specified terminator string
        Dim Terminated As Boolean, tLen As Integer, Received As String = ""
        Dim MyCount As Long
        MyCount = GetNextCount("ReceiveTerminated ")
        If DebugTrace Then Logger.LogMessage("ReceiveTerminated ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Start - terminator: """ & Terminator.ToString & """")
        'Check for bad terminator string
        If Terminator = "" Then Throw New InvalidValueException("ReceiveTerminated Terminator", "Null or empty string", "Character or character string")

        If GetSemaphore("ReceiveTerminated ", MyCount) Then
            Try
                If Not DebugTrace Then Logger.LogStart("ReceiveTerminated " & Terminator.ToString, MyCount.ToString & " " & CurrentThread.ManagedThreadId & " < ")
                tLen = Len(Terminator)
                Do
                    Received = Received & ReadChar("ReceiveTerminated ", MyCount) ' Build up the string one char at a time
                    If Len(Received) >= tLen Then ' Check terminator when string is long enough
                        If Right(Received, tLen) = Terminator Then Terminated = True
                    End If
                Loop Until Terminated
                If DebugTrace Then
                    Logger.LogMessage("ReceiveTerminated ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " < " & Received)
                Else
                    Logger.LogFinish(Received)
                End If
            Catch ex As InvalidValueException
                Logger.LogMessage("ReceiveTerminated ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " EXCEPTION - Terminator cannot be a null string")
                Throw
            Catch ex As TimeoutException
                Logger.LogMessage("ReceiveTerminated ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.ToString)
                Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
            Catch ex As Exception
                Logger.LogMessage("ReceiveTerminated ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.ToString)
                Throw
            Finally 'Ensure we release the semaphore whatever happens
                ReleaseSemaphore("ReceiveTerminated ", MyCount)
            End Try
            Return Received
        Else
            Logger.LogMessage("ReceiveTerminated", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Throwing SerialPortInUse exception")
            Throw New SerialPortInUseException("Serial:ReceiveTerminated - unable to get serial port semaphore before timeout.")
        End If
    End Function

    ''' <summary>
    ''' Receive characters from the ASCOM serial port until the given terminator bytes are seen, return as a byte array
    ''' </summary>
    ''' <param name="TerminatorBytes">Array of bytes that indicates end of message</param>
    ''' <returns>Byte array of received characters</returns>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <remarks>
    ''' <para>If a timeout occurs, a TimeoutException is raised.</para>
    ''' <para>This function exists in the COM component but is not documented in the help file.</para>
    ''' </remarks>
    Public Function ReceiveTerminatedBinary(ByVal TerminatorBytes() As Byte) As Byte() Implements ISerial.ReceiveTerminatedBinary
        'Return all characters up to and including a specified terminator string
        Dim Terminated As Boolean, tLen As Integer, Terminator As String, Received As String = ""
        Dim TextEncoding As System.Text.Encoding
        Dim MyCount As Long
        TextEncoding = System.Text.Encoding.GetEncoding(1252)
        Terminator = TextEncoding.GetString(TerminatorBytes)

        MyCount = GetNextCount("ReceiveTerminatedBinary ")
        If DebugTrace Then Logger.LogMessage("ReceiveTerminatedBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Start - terminator: """ & Terminator.ToString & """")
        'Check for bad terminator string
        If Terminator = "" Then Throw New InvalidValueException("ReceiveTerminatedBinary Terminator", "Null or empty string", "Character or character string")

        If GetSemaphore("ReceiveTerminatedBinary ", MyCount) Then
            Try
                If Not DebugTrace Then Logger.LogStart("ReceiveTerminatedBinary " & Terminator.ToString, MyCount.ToString & " " & CurrentThread.ManagedThreadId & " < ")

                tLen = Len(Terminator)
                Do
                    Received = Received & ReadChar("ReceiveTerminatedBinary ", MyCount) ' Build up the string one char at a time
                    If Len(Received) >= tLen Then ' Check terminator when string is long enough
                        If Right(Received, tLen) = Terminator Then Terminated = True
                    End If
                Loop Until Terminated
                If DebugTrace Then
                    Logger.LogMessage("ReceiveTerminatedBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " < " & Received, True)
                Else
                    Logger.LogFinish(Received, True)
                End If
            Catch ex As InvalidValueException
                Logger.LogMessage("ReceiveTerminatedBinary " & MyCount.ToString, " " & CurrentThread.ManagedThreadId & "EXCEPTION - Terminator cannot be a null string")
                Throw
            Catch ex As TimeoutException
                Logger.LogMessage("ReceiveTerminatedBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.ToString)
                Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
            Catch ex As Exception
                Logger.LogMessage("ReceiveTerminatedBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.ToString)
                Throw
            Finally
                ReleaseSemaphore("ReceiveTerminatedBinary ", MyCount)
            End Try
            Return TextEncoding.GetBytes(Received)
        Else
            Logger.LogMessage("ReceiveTerminatedBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Throwing SerialPortInUse exception")
            'Throw New SerialPortInUseException("Serial:ReceiveTerminatedBinary - unable to get serial port semaphore before timeout.")
            Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)

        End If

    End Function

    ''' <summary>
    ''' Transmits a string through the ASCOM serial port
    ''' </summary>
    ''' <param name="Data">String to transmit</param>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <remarks></remarks>
    Public Sub Transmit(ByVal Data As String) Implements ISerial.Transmit
        Dim MyCount As Long
        'Send provided string to the serial port
        MyCount = GetNextCount("Transmit")
        If DebugTrace Then Logger.LogMessage("Transmit", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " > " & Data.ToString & " ")
        If GetSemaphore("Transmit", MyCount) Then
            Try
                If Not DebugTrace Then Logger.LogMessage("Transmit", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " > " & Data.ToString & " ")
                m_Port.Write(Data)
                If DebugTrace Then Logger.LogMessage("Transmit", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Completed")
            Catch ex As Exception
                Logger.LogMessage("Transmit", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Exception: " & ex.ToString)
                Throw
            Finally 'Ensure we release the semaphore whatever happens
                ReleaseSemaphore("Transmit", MyCount)
            End Try
        Else
            Logger.LogMessage("Transmit", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Throwing SerialPortInUse exception")
            Throw New SerialPortInUseException("Serial:Transmit - unable to get serial port semaphore before timeout.")
        End If
    End Sub

    Public SL As String = "MySyncLock"

    ''' <summary>
    ''' Transmit an array of binary bytes through the ASCOM serial port 
    ''' </summary>
    ''' <param name="Data">Byte array to transmit</param>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <remarks></remarks>
    Public Sub TransmitBinary(ByVal Data() As Byte) Implements ISerial.TransmitBinary
        'Send provided binary array to the serial port
        Dim TxString As String, MyCount As Long, MouseDown As String
        MyCount = GetNextCount("TransmitBinary ")
        TxString = TextEncoding.GetString(Data)
        If DebugTrace Then Logger.LogMessage("TransmitBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " > " & TxString & "  (HEX" & MakeHex(TxString) & ") ")
        MouseDown = Chr(80) & Chr(2) & Chr(16) & Chr(37) & Chr(8) & Chr(0) & Chr(0) & Chr(0)

        If TxString = MouseDown Then
            SyncLock SL
                Logger.LogMessage("TransmitBinary", "FOUND Mousedown event, starting wait")

                For i As Integer = 1 To 10
                    Threading.Thread.Sleep(1000)
                    Logger.LogMessage("TransmitBinary", "Wait loop " & i)
                Next
                'Dim sem As New System.Threading.Semaphore(0, 1)
                'sem.WaitOne(10000, True)



                Logger.LogMessage("TransmitBinary", "FOUND Mousedown event, ending wait")
            End SyncLock
        End If


        If GetSemaphore("TransmitBinary ", MyCount) Then
            Try
                If Not DebugTrace Then Logger.LogMessage("TransmitBinary", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " > " & TxString & "  (HEX" & MakeHex(TxString) & ") ")
                m_Port.Write(Data, 0, Data.Length)
                If DebugTrace Then Logger.LogMessage("TransmitBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Completed")
            Catch ex As Exception
                Logger.LogMessage("TransmitBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Exception: " & ex.ToString)
                Throw
            Finally 'Ensure we release the semaphore whatever happens
                ReleaseSemaphore("TransmitBinary ", MyCount)
            End Try
        Else
            Logger.LogMessage("TransmitBinary ", MyCount.ToString & " " & CurrentThread.ManagedThreadId & " Throwing SerialPortInUse exception")
            Throw New SerialPortInUseException("TransmitBinary - unable to get serial port semaphore before timeout.")
        End If
    End Sub
#End Region

#Region "ISerialExtensions Implementation"
    'These are additional funcitons not provided in the original Helper and Helper2 components
    ''' <summary>
    ''' Sets the ASCOM serial port name as a string
    ''' </summary>
    ''' <value>Serial port name to be used</value>
    ''' <returns>Current serial port name</returns>
    ''' <remarks>This property allows any serial port name to be used, even if it doesn't conform to a format of COMnnn
    ''' If the required port name is of the form COMnnn then Serial.Port=nnn and Serial.PortName="COMnnn" are 
    ''' equivalent.</remarks>
    Public Property PortName() As String Implements ISerial.PortName
        'Allows the full COM port name to be read and set. This works for COM ports of any name
        Get
            Return m_PortName
        End Get
        Set(ByVal value As String)
            m_PortName = value
            Logger.LogMessage("PortName", "Set to: " & value)
        End Set
    End Property

    ''' <summary>
    ''' Returns a list of all available ASCOM serial ports with COMnnn ports sorted into ascending port number order
    ''' </summary>
    ''' <value>String array of available serial ports</value>
    ''' <returns>A string array of available serial ports</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property AvailableCOMPorts() As String() Implements ISerial.AvailableComPorts
        'Returns a list of all available COM ports sorted into ascending COM port number order
        Get
            Dim RetVal() As String
            Dim myPortNameComparer As PortNameComparer = New PortNameComparer
            RetVal = SerialPort.GetPortNames
            Array.Sort(RetVal, myPortNameComparer) 'Use specialised comparer to get the sort order right
            For Each Port As String In RetVal
                Logger.LogMessage("AvailableCOMPorts", Port)
            Next
            Return RetVal
        End Get
    End Property

    ''' <summary>
    ''' Adds a message to the ASCOM serial trace file
    ''' </summary>
    ''' <param name="Caller">String identifying the module logging the message</param>
    ''' <param name="Message">Message text to be logged.</param>
    ''' <remarks>
    ''' <para>This can be called regardless of whether logging is enabled</para>
    ''' </remarks>
    Public Sub LogMessage(ByVal Caller As String, ByVal Message As String) Implements ISerial.LogMessage
        'Allows a driver to include its own comments in the serial log
        Logger.LogMessage(Caller, Message)
    End Sub
#End Region

#Region "Support Code"
    Private Function FormatRXSoFar(ByVal p_Chars As String) As String
        If p_Chars <> "" Then Return p_Chars & " "
        Return ""
    End Function

    ''' <summary>
    ''' Translates a supplied string into hex characters
    ''' </summary>
    ''' <param name="p_Msg">The string to convert</param>
    ''' <returns>A string with each character represented as [xx]</returns>
    ''' <remarks></remarks>
    Private Function MakeHex(ByVal p_Msg As String) As String
        Dim l_Msg As String = ""
        Dim i, CharNo As Integer
        'Present all characters in [0xHH] format
        For i = 1 To Len(p_Msg)
            CharNo = Asc(Mid(p_Msg, i, 1))
            l_Msg = l_Msg & "[" & Right("00" & Hex(CharNo), 2) & "]"
        Next
        Return l_Msg
    End Function

    Private Function GetSemaphore2(ByVal p_Caller As String, ByVal MyCallNumber As Long) As Boolean 'Gets the serial control semaphore and returns success or failure
        If SerPortInUse Then
            Sleep(10)
            Application.DoEvents()
        End If
        SerPortInUse = True
        Return True
    End Function

    Private Sub ReleaseSemaphore2(ByVal p_Caller As String, ByVal MyCallNumber As Long) 'Release the semaphore if we have it but don't error if there is a problem
        SerPortInUse = False
    End Sub

    Private Function GetSemaphore(ByVal p_Caller As String, ByVal MyCallNumber As Long) As Boolean 'Gets the serial control semaphore and returns success or failure
        Dim GotSemaphore As Boolean, StartTime As Date
        StartTime = Now 'Save the current start time so we will wait longer than a port timeout before giving up on getting the semaphore
        Try
            If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " Entered GetSemaphore " & CurrentThread.ManagedThreadId)
            GotSemaphore = SerSemaphore.WaitOne(m_ReceiveTimeout + 2000)
            If DebugTrace Then
                If GotSemaphore Then
                    Logger.LogMessage(p_Caller, MyCallNumber.ToString & " Got Semaphore OK")
                Else
                    Logger.LogMessage(p_Caller, MyCallNumber.ToString & " Failed to get Semaphore, returning: False")
                End If
            End If
        Catch ex As Exception 'Log abandoned mutex exception
            Logger.LogMessage("GetSemaphore", MyCallNumber.ToString & CurrentThread.ManagedThreadId & " " & " Exception: " & ex.ToString & " " & ex.StackTrace)
        End Try
        'Return False 'Didn't get the semaphore so return false
        Return GotSemaphore
    End Function

    Private Sub ReleaseSemaphore(ByVal p_Caller As String, ByVal MyCallNumber As Long) 'Release the semaphore if we have it but don't error if there is a problem
        Try
            If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " Entered ReleaseSemaphore " & CurrentThread.ManagedThreadId)
            SerSemaphore.Release()
            If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " Semaphore released OK")
        Catch ex As System.Threading.SemaphoreFullException
            Logger.LogMessage("ReleaseSemaphore", MyCallNumber.ToString & " " & CurrentThread.ManagedThreadId & " " & " SemaphoreFullException: " & ex.ToString & " " & ex.StackTrace)
        Catch ex As Exception
            Logger.LogMessage("ReleaseSemaphore", MyCallNumber.ToString & " " & CurrentThread.ManagedThreadId & " " & " Exception: " & ex.ToString & " " & ex.StackTrace)
        End Try
    End Sub

    Private Function GetNextCount(ByVal p_Caller As String) As Long
        Dim ReturnValue As Long
        Try
            If DebugTrace Then Logger.LogMessage(p_Caller, "Entered GetNextCount " & System.Threading.Thread.CurrentThread.ManagedThreadId)
            CallCountSemaphore.WaitOne()
            If DebugTrace Then Logger.LogMessage(p_Caller, "Got CallCountMutex")
            CallCount += 1
            ReturnValue = CallCount
            CallCountSemaphore.Release()
            If DebugTrace Then Logger.LogMessage(p_Caller, ReturnValue.ToString & " Released CallCountMutex")
        Catch ex As Exception
            Logger.LogMessage(p_Caller, "EXCEPTION: " & ex.ToString)
            Throw
        End Try
        Return ReturnValue
    End Function

    Private Function ReadByte(ByVal p_Caller As String, ByVal MyCallNumber As Long) As Byte
        Dim StartTime As Date, RxByte As Byte, RxBytes(10) As Byte, BytesRead As Integer
        StartTime = Now
        If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " Entered ReadByte " & CurrentThread.ManagedThreadId)
        While (m_Port.BytesToRead = 0) And (Now.Subtract(StartTime).TotalMilliseconds < m_ReceiveTimeout)
            Sleep(10)
            If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " ReadByte WaitForBytes loop... ")
        End While
        If m_Port.BytesToRead = 0 Then
            If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " ReadByte timed out so throwing a TimeoutException" & CurrentThread.ManagedThreadId)
            Throw New TimeoutException("Serial:ReadByte - Timed out waiting for byte to arrive")
        Else
            If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " ReadByte ready to read byte " & CurrentThread.ManagedThreadId & " NumBytes: " & m_Port.BytesToRead)
            BytesRead = m_Port.Read(RxBytes, 0, 1)
            'RxByte = CByte(m_Port.ReadByte)
            If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " ReadByte returning result " & CurrentThread.ManagedThreadId & " NumBytes: " & m_Port.BytesToRead & " Bytes Read: " & BytesRead)
            Return RxBytes(0)
        End If
    End Function

    Private Function ReadChar(ByVal p_Caller As String, ByVal MyCallNumber As Long) As Char
        Dim StartTime As Date, RxChar As Char, RxChars(10) As Char, CharsRead As Integer
        StartTime = Now
        If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " Entered ReadChar " & CurrentThread.ManagedThreadId)
        While (m_Port.BytesToRead = 0) And (Now.Subtract(StartTime).TotalMilliseconds < m_ReceiveTimeout)
            Sleep(10)
            If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " ReadChar WaitForChars loop... ")
        End While
        If m_Port.BytesToRead = 0 Then
            If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " ReadChar timed out so throwing a TimeoutException" & CurrentThread.ManagedThreadId)
            Throw New TimeoutException("Serial:ReadChar - Timed out waiting for character to arrive")
        Else
            If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " ReadChar ready to read char " & CurrentThread.ManagedThreadId & " NumBytes: " & m_Port.BytesToRead)
            CharsRead = m_Port.Read(RxChars, 0, 1)
            'RxChar = Chr(m_Port.ReadByte)
            If DebugTrace Then Logger.LogMessage(p_Caller, MyCallNumber.ToString & " ReadChar returning result " & CurrentThread.ManagedThreadId & " NumBytes: " & m_Port.BytesToRead & " Chars Read: " & CharsRead)
            Return RxChars(0)
        End If
    End Function

    Friend Class PortNameComparer
        Implements IComparer
        'IComparer implementation that compares COM port names based on the numeric port number
        'rather than the whole port name string

        Friend Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            Dim xs, ys As String
            xs = x.ToString 'Make sure we are working with strings
            ys = y.ToString
            If (Len(xs) >= 4) And (Len(ys) >= 4) Then 'Ensure that we have at least enough characters for COMn format
                If (Left(xs, 3).ToUpper = "COM") And (Left(ys, 3).ToUpper = "COM") Then 'Test whether we have COMnnn format
                    If IsNumeric(Mid(xs, 4)) And IsNumeric(Mid(ys, 4)) Then ' We have numeric values so compare based on these
                        Return Comparer.Default.Compare(CInt(Mid(xs, 4)), CInt(Mid(ys, 4))) 'Compare based on port numbers
                    End If
                End If
            End If
            Return Comparer.Default.Compare(x, y) 'Compare based on port names
        End Function
    End Class
#End Region

#Region "Threading Support"
    Private Enum SerialCommandType
        ClearBuffers
        Receive
        Receivebyte
        ReceiveCounted
        ReceiveCountedBinary
        ReceiveTerminated
        ReceiveTerminatedBinary
        Transmit
        TransmitBinary
    End Enum

    Private Class ThreadData
        Sub New()
            'Initialise values to required state
            Me.Completed = False
            Me.LastException = Nothing
        End Sub

        'Transmit Input values
        Public TransmitString As String
        Public TransMitBytes() As Byte

        'Receive Input values
        Public Terminator As String
        Public terminatorBytes() As Byte
        Public Count As Integer

        'Output values
        Public LastException As Exception
        Public ResultByte As Byte
        Public ResultByteArray() As Byte
        Public ResultString As String
        Public ResultChar As Char

        'Control values
        Public SerialCommand As SerialCommandType
        Public Completed As Boolean
    End Class
#End Region

End Class