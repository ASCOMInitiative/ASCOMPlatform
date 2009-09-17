'1/4/09 Converted to new traceLogger hex string call
Option Strict On
Option Explicit On

Imports System.IO.Ports
Imports ASCOM.HelperNET.Interfaces
Imports ASCOM.HelperNET.Exceptions

''' <summary>
''' Creates a .NET serial port and provides a simple set of commands to use it.
''' </summary>
''' <remarks>This object provides an easy to use interface to a serial (COM) port. 
''' It provides ASCII and binary I/O with controllable timeout. It is limited to 8 bit, no parity, 
''' one stop-bit modes. The interface is callable from any .NET client.</remarks>
''' <example>
''' Example of how to create and use an ASCOM serial port.
''' <code lang="vbnet" title="ASCOM Serial Port Example" 
''' source="..\..\HelperNET Samples\HelperNET Samples\SerialExamples.vb"
''' />
'''</example>
Public Class Serial
    Implements ISerial, IDisposable

    'State variables holding com port configuration
    Private m_Port As SerialPort
    Private m_PortName As String
    Private m_ReceiveTimeout As Integer
    Private m_Speed As PortSpeed
    Private m_Connected As Boolean
    Private m_DataBits As Integer
    Private m_DTREnable As Boolean
    Private m_Handshake As Handshake
    Private m_Parity As Parity
    Private m_StopBits As StopBits

    Private TraceFile As String 'Current trace file name
    Private disposed As Boolean = False 'IDisposable variable to detect redundant calls

    Private Logger As TraceLogger
    Private TextEncoding As System.Text.Encoding
    Private m_SerTraceFile As String = SERIAL_DEFAULT_FILENAME 'Set the default trace file name

    Private mut As System.Threading.Mutex

    Private Const TIMEOUT_NUMBER As Integer = vbObjectError + &H402
    Private Const TIMEOUT_MESSAGE As String = "Timed out waiting for received data"
    Private Const MUTEX_TIMEOUT As Integer = 1000

    'Serial port parameters
    Private Const SERIALPORT_ENCODING As Integer = 1252
    Private Const SERIALPORT_DEFAULT_NAME As String = "COM1"
    Private Const SERIALPORT_DEFAULT_TIMEOUT As Integer = 5000
    Private Const SERIALPORT_DEFAULT_SPEED As PortSpeed = PortSpeed.ps9600
    Private Const SERIALPORT_DEFAULT_DATABITS As Integer = 8
    Private Const SERIALPORT_DEFAULT_DTRENABLE As Boolean = True
    Private Const SERIALPORT_DEFAULT_HANDSHAKE As Handshake = IO.Ports.Handshake.None
    Private Const SERIALPORT_DEFAULT_PARITY As Parity = IO.Ports.Parity.None
    Private Const SERIALPORT_DEFAULT_STOPBITS As StopBits = IO.Ports.StopBits.One

#Region "Enums"
    'PortSpeed enum
    ''' <summary>
    ''' Enumeration of serial port speeds for use with the Serial port
    ''' </summary>
    ''' <remarks>This contains an additional speed 230,400 baud that the COM component doesn't support.</remarks>
    Public Enum PortSpeed As Integer 'Defined at ACOM.HelperNET level
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
#End Region

#Region "New and IDisposable Support"
    Sub New()
        Dim SerialProfile As IAccess = Nothing
        Dim TraceFileName As String = ""

        mut = New System.Threading.Mutex 'Create a new mutex to control access to the serial port

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
            If Not mut Is Nothing Then 'Clean up the mutex
                Try : mut.ReleaseMutex() : Catch : End Try
                Try : mut.Close() : Catch : End Try
                mut = Nothing
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
    ''' <value>The type of handshake used on the serial line, default is none</value>
    ''' <returns>One of the Ports.Handshake enumeration values</returns>
    ''' <remarks></remarks>
    Public Property Handshake() As Handshake Implements ISerial.Handshake
        Get
            Return m_Handshake
        End Get
        Set(ByVal HandshakeType As Handshake)
            m_Handshake = HandshakeType
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the type of parity check used over the serial link
    ''' </summary>
    ''' <value>The type of parity check used over the serial link, default none</value>
    ''' <returns>One of the Ports.Parity enumeration values</returns>
    ''' <remarks></remarks>
    Public Property Parity() As Parity Implements ISerial.Parity
        Get
            Return m_Parity
        End Get
        Set(ByVal ParityType As Parity)
            m_Parity = ParityType
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the number of stop bits used on the serial link
    ''' </summary>
    ''' <value>the number of stop bits used on the serial link, default 1</value>
    ''' <returns>One of the Ports.StopBits enumeration values</returns>
    ''' <remarks></remarks>
    Public Property StopBits() As StopBits Implements ISerial.StopBits
        Get
            Return m_StopBits
        End Get
        Set(ByVal NumStopBits As StopBits)
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
                        m_Port.Handshake = m_Handshake

                        'Set transmission format
                        m_Port.DataBits = m_DataBits
                        m_Port.Parity = m_Parity
                        m_Port.StopBits = m_StopBits

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
    ''' <exception cref="InvalidValueException">Thrown when <paramref name="value"><c>=0</c></paramref> is invalid.</exception>
    Public Property ReceiveTimeout() As Integer Implements ISerial.ReceiveTimeout
        'Get and set the receive timeout
        Get
            Return CInt(m_ReceiveTimeout / 1000)
        End Get
        Set(ByVal value As Integer)
            value = value * 1000 ' Timeout is measured in milliseconds
            If (value <= 0) Or (value > 120000) Then Throw New InvalidValueException("ReceiveTimeout Invalid timeout value: " & Format(CDbl(value / 1000), "0.0") & " seconds")
            m_ReceiveTimeout = value
            If m_Connected Then
                m_Port.WriteTimeout = value
                m_Port.ReadTimeout = value
            End If
            Logger.LogMessage("ReceiveTimeout", "Set to: " & value / 1000 & " seconds")
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
            If (value <= 0) Or (value > 120000) Then Throw New InvalidValueException("ReceiveTimeoutMs Invalid timeout value: " & value.ToString & " milliseconds")
            m_ReceiveTimeout = value
            If m_Connected Then
                m_Port.WriteTimeout = value
                m_Port.ReadTimeout = value
            End If
            Logger.LogMessage("ReceiveTimeoutMs", "Set to: " & value.ToString & " mS")
        End Set
    End Property

    ''' <summary>
    ''' Gets and sets the baud rate of the ASCOM serial port
    ''' </summary>
    ''' <value>Port speed using the PortSpeed enum</value>
    ''' <returns>Port speed using the PortSpeed enum</returns>
    ''' <remarks>This is modelled on the COM component with possible values enumerated in the PortSpeed enum.</remarks>
    Public Property Speed() As PortSpeed Implements ISerial.Speed
        'Get and set the port speed using the portspeed enum
        Get
            Return m_Speed
        End Get
        Set(ByVal value As PortSpeed)
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
        Try
            If GetMutex() Then
                m_Port.DiscardInBuffer()
                m_Port.DiscardOutBuffer()
                Logger.LogMessage("ClearBuffers", "")
            Else
                Throw New SerialPortInUseException("Serial:Clearbuffers - unable to get serial port mutex before timeout.")
            End If
        Finally 'Ensure we release the mutex whatever happens
            Try : mut.ReleaseMutex() : Catch : End Try
        End Try

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
        Try
            If GetMutex() Then
                Logger.LogStart("Receive", "< ")
                Received = Chr(m_Port.ReadChar)
                Received = Received & m_Port.ReadExisting
                Logger.LogFinish(Received)
                Return Received
            Else
                Throw New SerialPortInUseException("Serial:Clearbuffers - unable to get serial port mutex before timeout.")
            End If
        Catch ex As TimeoutException
            Logger.LogFinish(FormatRXSoFar(Received) & "EXCEPTION: " & ex.Message)
            Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
        Catch ex As Exception
            Logger.LogFinish(FormatRXSoFar(Received) & "EXCEPTION: " & ex.Message)
            Throw
        Finally 'Ensure we release the mutex whatever happens
            Try : mut.ReleaseMutex() : Catch : End Try
        End Try
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
        Try
            Dim RetVal As Byte
            If GetMutex() Then
                Logger.LogStart("ReceiveByte", "< ")
                RetVal = CByte(m_Port.ReadByte)
                Logger.LogFinish(Chr(RetVal), True)
                Return RetVal
            Else
                Throw New SerialPortInUseException("Serial:Clearbuffers - unable to get serial port mutex before timeout.")
            End If
        Catch ex As TimeoutException
            Logger.LogFinish("EXCEPTION: " & ex.Message)
            Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
        Catch ex As Exception
            Logger.LogFinish("EXCEPTION: " & ex.Message)
            Throw
        Finally 'Ensure we release the mutex whatever happens
            Try : mut.ReleaseMutex() : Catch : End Try
        End Try
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
        Try
            If GetMutex() Then
                Logger.LogStart("ReceiveCounted " & Count.ToString, "< ")
                For i = 1 To Count
                    Received = Received & Chr(m_Port.ReadByte)
                Next
                Logger.LogFinish(Received)
                Return Received
            Else
                Throw New SerialPortInUseException("Serial:Clearbuffers - unable to get serial port mutex before timeout.")
            End If
        Catch ex As TimeoutException
            Logger.LogFinish(FormatRXSoFar(Received) & "EXCEPTION: " & ex.Message)
            Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
        Catch ex As Exception
            Logger.LogFinish(FormatRXSoFar(Received) & "EXCEPTION: " & ex.Message)
            Throw
        Finally 'Ensure we release the mutex whatever happens
            Try : mut.ReleaseMutex() : Catch : End Try
        End Try
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
        TextEncoding = System.Text.Encoding.GetEncoding(1252)

        Try
            If GetMutex() Then
                Logger.LogStart("ReceiveCountedBinary " & Count.ToString, "< ")
                For i = 0 To Count - 1
                    ReDim Preserve Received(i)
                    Received(i) = CByte(m_Port.ReadByte)
                Next
                Logger.LogFinish(TextEncoding.GetString(Received), True)
                Return Received
            Else
                Throw New SerialPortInUseException("Serial:Clearbuffers - unable to get serial port mutex before timeout.")
            End If
        Catch ex As TimeoutException
            Logger.LogFinish(FormatRXSoFar(TextEncoding.GetString(Received)) & "EXCEPTION: " & ex.Message)
            Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
        Catch ex As Exception
            Logger.LogFinish(FormatRXSoFar(TextEncoding.GetString(Received)) & "EXCEPTION: " & ex.Message)
            Throw
        Finally 'Ensure we release the mutex whatever happens
            Try : mut.ReleaseMutex() : Catch : End Try
        End Try
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
        Try
            If GetMutex() Then
                'Check for bad terminator string
                If Terminator = "" Then Throw New InvalidValueException("ReceiveTerminated - Terminator cannot be a null string")

                Logger.LogStart("ReceiveTerminated " & Terminator.ToString, "< ")

                tLen = Len(Terminator)
                Do
                    Received = Received & Chr(m_Port.ReadByte) ' Build up the string one char at a time
                    If Len(Received) >= tLen Then ' Check terminator when string is long enough
                        If Right(Received, tLen) = Terminator Then Terminated = True
                    End If
                Loop Until Terminated
                Logger.LogFinish(Received)
                Return Received
            Else
                Throw New SerialPortInUseException("Serial:Clearbuffers - unable to get serial port mutex before timeout.")
            End If
        Catch ex As InvalidValueException
            Logger.LogMessage("ReceiveTerminated", "EXCEPTION - Terminator cannot be a null string")
            Throw
        Catch ex As TimeoutException
            Logger.LogFinish(FormatRXSoFar(Received) & "EXCEPTION: " & ex.Message)
            Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
        Catch ex As Exception
            Logger.LogFinish(FormatRXSoFar(Received) & "EXCEPTION: " & ex.Message)
            Throw
        Finally 'Ensure we release the mutex whatever happens
            Try : mut.ReleaseMutex() : Catch : End Try
        End Try
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
        TextEncoding = System.Text.Encoding.GetEncoding(1252)
        Terminator = TextEncoding.GetString(TerminatorBytes)

        Try
            If GetMutex() Then
                'Check for bad terminator string
                If Terminator = "" Then Throw New InvalidValueException("ReceiveTerminatedBinary - Terminator cannot be a null string")

                Logger.LogStart("ReceiveTerminatedBinary " & Terminator.ToString, "< ")

                tLen = Len(Terminator)
                Do
                    Received = Received & Chr(m_Port.ReadByte) ' Build up the string one char at a time
                    If Len(Received) >= tLen Then ' Check terminator when string is long enough
                        If Right(Received, tLen) = Terminator Then Terminated = True
                    End If
                Loop Until Terminated
                Logger.LogFinish(Received, True)
                Return TextEncoding.GetBytes(Received)
            Else
                Throw New SerialPortInUseException("Serial:Clearbuffers - unable to get serial port mutex before timeout.")
            End If
        Catch ex As InvalidValueException
            Logger.LogMessage("ReceiveTerminatedBinary", "EXCEPTION - Terminator cannot be a null string")
            Throw
        Catch ex As TimeoutException
            Logger.LogFinish(FormatRXSoFar(Received) & "EXCEPTION: " & ex.Message)
            Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
        Catch ex As Exception
            Logger.LogFinish(FormatRXSoFar(Received) & "EXCEPTION: " & ex.Message)
            Throw
        Finally 'Ensure we release the mutex whatever happens
            Try : mut.ReleaseMutex() : Catch : End Try
        End Try

    End Function

    ''' <summary>
    ''' Transmits a string through the ASCOM serial port
    ''' </summary>
    ''' <param name="Data">String to transmit</param>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <remarks></remarks>
    Public Sub Transmit(ByVal Data As String) Implements ISerial.Transmit
        'Send provided string to the serial port
        Try
            If GetMutex() Then
                Logger.LogStart("Transmit", "> " & Data.ToString & " ")
                m_Port.Write(Data)
                Logger.LogFinish("")
            Else
                Throw New SerialPortInUseException("Serial:Clearbuffers - unable to get serial port mutex before timeout.")
            End If
        Catch ex As Exception
            Logger.LogFinish("Exception: " & ex.Message)
            Throw
        Finally 'Ensure we release the mutex whatever happens
            Try : mut.ReleaseMutex() : Catch : End Try
        End Try
    End Sub

    ''' <summary>
    ''' Transmit an array of binary bytes through the ASCOM serial port 
    ''' </summary>
    ''' <param name="Data">Byte array to transmit</param>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <remarks></remarks>
    Public Sub TransmitBinary(ByVal Data() As Byte) Implements ISerial.TransmitBinary
        'Send provided binary array to the serial port
        Dim TxString As String
        Try
            TxString = TextEncoding.GetString(Data)
            Logger.LogStart("TransmitBinary", "> " & TxString & "  (HEX" & MakeHex(TxString) & ") ")
            m_Port.Write(Data, 0, Data.Length)
            Logger.LogFinish("") 'Just append the hex interpretation 
        Catch ex As Exception
            Logger.LogFinish("Exception: " & ex.Message)
            Throw
        Finally 'Ensure we release the mutex whatever happens
            Try : mut.ReleaseMutex() : Catch : End Try
        End Try
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

    Private Function GetMutex() As Boolean 'Gets the serial control mutex and returns success or failure
        Dim LoopCount As Integer, GotMutex As Boolean
        Try
            For LoopCount = 1 To 5 'Try up to 5 times to get the mutex
                GotMutex = mut.WaitOne(0) 'Test whether the mutex is already free
                If GotMutex Then Return True 'It is free, we have it so return true

                'Mutex is in use by something else - this should not happen, so log an error message
                Logger.LogIssue("Serial:GetMutex", "Mutex not available, please see help file. Re-trying...")
                GotMutex = mut.WaitOne(MUTEX_TIMEOUT) 'Wait for longer this time
                If GotMutex Then Return True 'It is now free, we have it so return true

            Next 'Try again for up to 5 attempts
        Catch ex As System.Threading.AbandonedMutexException 'Log abandoned mutex exception
            LogMessage("Serial:GetMutex", "Received an abandoned mutex exception, please see help file.")
        End Try

        Return False 'Didn't get the mutex so return false
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

End Class

