'Implements the Serial component

Option Strict On
Option Explicit On
Imports ASCOM
Imports System.IO.Ports
Imports ASCOM.Utilities.Interfaces
Imports ASCOM.Utilities.Exceptions
Imports System.Runtime.InteropServices
Imports System.Threading
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
''' same — either even or odd — for each group of bits that is transmitted without error. 
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

''' <summary>
''' Creates a .NET serial port and provides a simple set of commands to use it.
''' </summary>
''' <remarks>This object provides an easy to use interface to a serial (COM) port. 
''' It provides ASCII and binary I/O with controllable timeout.
''' The interface is callable from any .NET client.
''' <para>The platform allows you to control use of the DTR and RTS/CTS lines for a particular 
''' COM port and to remove or force listing of individual COM ports in the AvailableComPorts 
''' list through configuration in the ASCOM Profile.
''' Please see the Tools and Features section of this help file for further details.</para> 
''' </remarks>
''' <example>
''' Example of how to create and use an ASCOM serial port.
''' <code lang="vbnet" title="ASCOM Serial Port Example" 
''' source="..\ASCOM Platform Examples\Examples\SerialExamples.vb"
''' />
'''</example>
<ClassInterface(ClassInterfaceType.None), _
Guid("0B8E6DC4-7451-4484-BE15-0D0727569FB8"), _
ComVisible(True)> _
Public Class Serial
    Implements ISerial, IDisposable

#Region "Variables"
    'State variables holding com port configuration
    Private m_Port As SerialPort
    Private m_PortName As String
    Private m_ReceiveTimeout As Integer
    Private m_Speed As SerialSpeed
    Private m_Connected As Boolean
    Private m_DataBits As Integer
    Private m_DTREnable As Boolean
    Private m_RTSEnable As Boolean
    Private m_Handshake As SerialHandshake
    Private m_Parity As SerialParity
    Private m_StopBits As SerialStopBits

    Private TraceFile As String 'Current trace file name
    Private disposed As Boolean = False 'IDisposable variable to detect redundant calls
    Private DebugTrace As Boolean = False 'Flag specifying type of information to record in log file
    Private ts As New Stopwatch

    Private Logger As TraceLogger
    Private TextEncoding As System.Text.Encoding
    Private m_SerTraceFile As String = SERIAL_DEFAULT_FILENAME 'Set the default trace file name

    Private SerialProfile As RegistryAccess = Nothing
    Private ForcedCOMPorts As Generic.SortedList(Of String, String)
    Private IgnoredCOMPorts As Generic.SortedList(Of String, String)

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
    Private Const SERIALPORT_DEFAULT_RTSENABLE As Boolean = False
    Private Const SERIALPORT_DEFAULT_HANDSHAKE As SerialHandshake = SerialHandshake.None
    Private Const SERIALPORT_DEFAULT_PARITY As SerialParity = SerialParity.None
    Private Const SERIALPORT_DEFAULT_STOPBITS As SerialStopBits = SerialStopBits.One
    Private Const AVAILABLE_PORTS_SERIAL_TIMEOUT As Integer = 500 'Number of milliseconds to allow for a port to respond to the open command during available port scanning
    Private Const AVAILABLE_PORTS_SERIAL_TIMEOUT_REPORT_THRESHOLD As Integer = 1000 'Number of milliseconds above which to report a long open time during available port scanning

    '18/3/12 Peter - Experimental change in behaviour of read routines that may address serial issues with some drivers, added at Chris Rowland's suggestion
    'This new switch is initialised in the state that the Platform has been using since Platform 5.5
    'The new behaviour is enabled by creating a ReadPolling value in the COMPortSettings key with the contents True, anything else results in normal behaviour
    Private Const SERIALPORT_DEFAULT_POLLING As Boolean = False
    Private Const SERIAL_READ_POLLING As String = "ReadPolling"
    Private UseReadPolling As Boolean = SERIALPORT_DEFAULT_POLLING

    Private TypeOfWait As WaitType = Serial.WaitType.ManualResetEvent

#End Region

#Region "Enums"
    Private Enum SerialCommandType
        AvailableCOMPorts
        ClearBuffers
        Connected
        Receive
        Receivebyte
        ReceiveCounted
        ReceiveCountedBinary
        ReceiveTerminated
        ReceiveTerminatedBinary
        ReceiveTimeout
        ReceiveTimeoutMs
        Transmit
        TransmitBinary
    End Enum

    Friend Enum WaitType
        ManualResetEvent
        Sleep
        WaitForSingleObject
    End Enum
#End Region

#Region "New and IDisposable Support"
    Sub New()
        Dim TraceFileName As String = ""
        Dim WorkerThreads, CompletionThreads As Integer

        SerSemaphore = New System.Threading.Semaphore(1, 1) 'Create a new semaphore to control access to the serial port

        m_Connected = False 'Set inital values
        m_PortName = SERIALPORT_DEFAULT_NAME
        m_ReceiveTimeout = SERIALPORT_DEFAULT_TIMEOUT
        m_Speed = SERIALPORT_DEFAULT_SPEED
        m_DataBits = SERIALPORT_DEFAULT_DATABITS
        m_DTREnable = SERIALPORT_DEFAULT_DTRENABLE
        m_RTSEnable = SERIALPORT_DEFAULT_RTSENABLE
        m_Handshake = SERIALPORT_DEFAULT_HANDSHAKE
        m_Parity = SERIALPORT_DEFAULT_PARITY
        m_StopBits = SERIALPORT_DEFAULT_STOPBITS

        TextEncoding = System.Text.Encoding.GetEncoding(SERIALPORT_ENCODING) 'Initialise text encoding for use by transmitbinary
        Try
            SerialProfile = New RegistryAccess 'Profile class that can retrieve the value of tracefile
            TraceFileName = SerialProfile.GetProfile("", SERIAL_FILE_NAME_VARNAME)
            Logger = New TraceLogger(TraceFileName, "Serial")
            If TraceFileName <> "" Then Logger.Enabled = True

            'Get debug trace level on / off
            DebugTrace = GetBool(SERIAL_TRACE_DEBUG, SERIAL_TRACE_DEBUG_DEFAULT)

            'Get the type of wait to use
            TypeOfWait = GetWaitType(SERIAL_WAIT_TYPE, SERIAL_WAIT_TYPE_DEFAULT)
            LogMessage("New", "Worker thread synchronisation by: " & TypeOfWait.ToString)

            ThreadPool.GetMinThreads(WorkerThreads, CompletionThreads)
            If DebugTrace Then Logger.LogMessage("New", "Minimum Threads: " & WorkerThreads & " " & CompletionThreads)
            ThreadPool.GetMaxThreads(WorkerThreads, CompletionThreads)
            If DebugTrace Then Logger.LogMessage("New", "Maximum Threads: " & WorkerThreads & " " & CompletionThreads)
            ThreadPool.GetAvailableThreads(WorkerThreads, CompletionThreads)
            If DebugTrace Then Logger.LogMessage("New", "Available Threads: " & WorkerThreads & " " & CompletionThreads)

        Catch ex As Exception
            MsgBox("Serial:New exception " & ex.ToString)
        End Try

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
            If Not SerialProfile Is Nothing Then 'Clean up the profile accessor
                Try : SerialProfile.Dispose() : Catch : End Try
                SerialProfile = Nothing
            End If
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
            Logger.LogMessage("DataBits", "Set to: " & NumDataBits.ToString)
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
            Logger.LogMessage("DTREnable", "Set to: " & Enabled.ToString)
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets use of the RTS handshake control line
    ''' </summary>
    ''' <value>The state of RTS line use, default is disabled (false)</value>
    ''' <returns>Boolean true/false indicating RTS line use enabled/disabled</returns>
    ''' <remarks>By default the serial component will not drive the RTS line. If RTSEnable is true, the RTS line will be raised before
    ''' characters are sent. Please also see the associated <see cref="Handshake"/> property.</remarks>
    Public Property RTSEnable() As Boolean Implements ISerial.RTSEnable
        Get
            Return m_RTSEnable
        End Get
        Set(ByVal Enabled As Boolean)
            m_RTSEnable = Enabled
            Logger.LogMessage("RTSEnable", "Set to: " & Enabled.ToString)
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the type of serial handshake used on the serial link
    ''' </summary>
    ''' <value>The type of flow control handshake used on the serial line, default is none</value>
    ''' <returns>One of the SerialHandshake enumeration values</returns>
    ''' <remarks>Use of the RTS line can additionally be controlled by the <see cref="RTSEnable"/> property.</remarks>
    Public Property Handshake() As SerialHandshake Implements ISerial.Handshake
        Get
            Return m_Handshake
        End Get
        Set(ByVal HandshakeType As SerialHandshake)
            m_Handshake = HandshakeType
            Logger.LogMessage("HandshakeType", "Set to: " & [Enum].GetName(GetType(SerialHandshake), HandshakeType))
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
            Logger.LogMessage("Parity", "Set to: " & [Enum].GetName(GetType(SerialParity), ParityType))
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
            Logger.LogMessage("NumStopBits", "Set to: " & [Enum].GetName(GetType(SerialStopBits), NumStopBits))
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
            Dim TData As New ThreadData
            Dim TS As New Stopwatch

            ' set the RTSEnable and DTREnable states from the registry,
            ' NOTE this overrides any other settings, but only if it's set.
            Dim buf As String
            Dim b, ForcedRTS, ForcedDTR As Boolean

            TS = Stopwatch.StartNew ' Initialise stopwatch

            'Foorce RTS if required
            buf = SerialProfile.GetProfile(SERIALPORT_COM_PORT_SETTINGS & "\" & m_PortName, "RTSEnable")
            If Boolean.TryParse(buf, b) Then
                m_RTSEnable = b
                ForcedRTS = True
            End If

            'Force DTR if required
            buf = SerialProfile.GetProfile(SERIALPORT_COM_PORT_SETTINGS & "\" & m_PortName, "DTREnable")
            If Boolean.TryParse(buf, b) Then
                m_DTREnable = b
                ForcedDTR = True
            End If

            'Force Read Polling if required
            buf = SerialProfile.GetProfile(SERIALPORT_COM_PORT_SETTINGS, SERIAL_READ_POLLING)
            If Boolean.TryParse(buf, b) Then UseReadPolling = b

            Try
                Logger.LogMessage("Set Connected", Connecting.ToString)
                If Connecting Then 'Log port parameters only if we are connecting
                    Logger.LogMessage("Set Connected", "Using COM port: " & m_PortName & _
                                      " Baud rate: " & m_Speed.ToString & _
                                      " Timeout: " & m_ReceiveTimeout.ToString & _
                                      " DTR: " & m_DTREnable.ToString & _
                                      " ForcedDTR: " & ForcedDTR.ToString & _
                                      " RTS: " & m_RTSEnable.ToString & _
                                      " ForcedRTS: " & ForcedRTS.ToString & _
                                      " Handshake: " & m_Handshake.ToString & _
                                      " Encoding: " & SERIALPORT_ENCODING.ToString)
                    Logger.LogMessage("Set Connected", "Transmission format - Bits: " & m_DataBits & _
                                      " Parity: " & [Enum].GetName(m_Parity.GetType, m_Parity) & _
                                      " Stop bits: " & [Enum].GetName(m_StopBits.GetType, m_StopBits))
                    If UseReadPolling Then
                        Logger.LogMessage("Set Connected", "Reading COM Port through Read Polling")
                    Else
                        Logger.LogMessage("Set Connected", "Reading COM port through Interrupt Handling")
                    End If
                End If

                TData.SerialCommand = SerialCommandType.Connected
                TData.Connecting = Connecting
                TData.TransactionID = GetTransactionID("Set Connected")
                ThreadPool.QueueUserWorkItem(AddressOf ConnectedWorker, TData)
                WaitForThread(TData) ' Sleep this thread until serial operation is complete

                If DebugTrace Then Logger.LogMessage("Set Connected", "Completed: " & TData.Completed & ", Duration: " & TS.Elapsed.TotalMilliseconds.ToString("0.0"))
                If Not TData.LastException Is Nothing Then Throw TData.LastException
                TData = Nothing

            Catch ex As Exception
                Logger.LogMessage("Set Connected", ex.ToString)
                Throw
            End Try
        End Set
    End Property

    Private Sub ConnectedWorker(ByVal TDataObject As Object)
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData)

        Try
            If DebugTrace Then Logger.LogMessage("ConnectedWorker", FormatIDs(TData.TransactionID) & "Connected: " & TData.Connecting)

            If TData.Connecting Then ' Trying to connect
                If Not m_Connected Then
                    If m_Port Is Nothing Then
                        m_Port = New System.IO.Ports.SerialPort(m_PortName)
                    Else
                        m_Port.PortName = m_PortName
                    End If

                    'Set up serial port
                    m_Port.BaudRate = m_Speed
                    m_Port.ReadTimeout = m_ReceiveTimeout
                    m_Port.WriteTimeout = m_ReceiveTimeout

                    'Ensure we can get all 256 possible values, default encoding is ASCII that only gives first 127 values
                    m_Port.Encoding = System.Text.Encoding.GetEncoding(SERIALPORT_ENCODING)

                    'Set handshaking and control signals
                    m_Port.DtrEnable = m_DTREnable
                    m_Port.RtsEnable = m_RTSEnable
                    m_Port.Handshake = CType(m_Handshake, System.IO.Ports.Handshake)

                    'Set transmission format
                    m_Port.DataBits = m_DataBits
                    m_Port.Parity = CType(m_Parity, System.IO.Ports.Parity)
                    m_Port.StopBits = CType(m_StopBits, System.IO.Ports.StopBits)

                    'Open port for communication
                    m_Port.Open()
                    m_Connected = True
                    Logger.LogMessage("ConnectedWorker", FormatIDs(TData.TransactionID) & "Port connected OK")
                Else
                    Logger.LogMessage("ConnectedWorker", FormatIDs(TData.TransactionID) & "Port already connected")
                End If
            Else ' Trying to disconnect
                If m_Connected Then
                    m_Connected = False
                    m_Port.DiscardOutBuffer()
                    m_Port.DiscardInBuffer()
                    Logger.LogMessage("ConnectedWorker", FormatIDs(TData.TransactionID) & "Cleared buffers")
                    m_Port.Close()
                    Logger.LogMessage("ConnectedWorker", FormatIDs(TData.TransactionID) & "Closed port")
                    m_Port.Dispose()
                    Logger.LogMessage("ConnectedWorker", FormatIDs(TData.TransactionID) & "Port disposed OK")
                Else
                    Logger.LogMessage("ConnectedWorker", FormatIDs(TData.TransactionID) & "Port already disconnected")
                End If
            End If
        Catch ex As Exception
            Try : Logger.LogMessage("ConnectedWorker", FormatIDs(TData.TransactionID) & "EXCEPTION: ConnectedWorker - " & ex.Message & " " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            TData.ThreadCompleted()
        End Try

    End Sub

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
    ''' <exception cref="InvalidValueException">Thrown when <i>value</i> is invalid (outside the range 1 to 120 seconds.)</exception>
    Public Property ReceiveTimeout() As Integer Implements ISerial.ReceiveTimeout
        'Get and set the receive timeout
        Get
            Return CInt(m_ReceiveTimeout / 1000)
        End Get
        Set(ByVal value As Integer)
            Dim TData As New ThreadData
            Try
                m_ReceiveTimeout = value * 1000 'Save the requested value
                If m_Connected Then ' Try and set the timeout in flight
                    If DebugTrace Then Logger.LogMessage("ReceiveTimeout", "Start")
                    TData.SerialCommand = SerialCommandType.ReceiveTimeout
                    TData.TimeoutValue = value
                    TData.TransactionID = GetTransactionID("ReceiveTimeout")
                    ThreadPool.QueueUserWorkItem(AddressOf ReceiveTimeoutWorker, TData)
                    WaitForThread(TData) ' Sleep this thread until serial operation is complete

                    If DebugTrace Then Logger.LogMessage("ReceiveTimeout", "Completed: " & TData.Completed)
                    If Not (TData.LastException Is Nothing) Then Throw TData.LastException
                    TData = Nothing
                Else 'Just report that the value has been set
                    Logger.LogMessage("ReceiveTimeout", "Set to: " & value & " seconds")
                End If
            Catch ex As Exception
                Logger.LogMessage("ReceiveTimeout", ex.ToString)
                Throw
            End Try
        End Set
    End Property

    Private Sub ReceiveTimeoutWorker(ByVal TDataObject As Object)
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData)
        Dim Value As Integer
        Try
            Value = TData.TimeoutValue
            If DebugTrace Then Logger.LogMessage("ReceiveTimeout", FormatIDs(TData.TransactionID) & "Set Start - TimeOut: " & Value & "seconds")

            Value = Value * 1000 ' Timeout is measured in milliseconds
            If (Value <= 0) Or (Value > 120000) Then Throw New InvalidValueException("ReceiveTimeout", Format(CDbl(Value / 1000), "0.0"), "1 to 120 seconds")
            m_ReceiveTimeout = Value
            If m_Connected Then
                If DebugTrace Then Logger.LogMessage("ReceiveTimeout", FormatIDs(TData.TransactionID) & "Connected so writing to serial port")
                If GetSemaphore("ReceiveTimeout", TData.TransactionID) Then
                    Try
                        m_Port.WriteTimeout = Value
                        m_Port.ReadTimeout = Value
                        Logger.LogMessage("ReceiveTimeout", FormatIDs(TData.TransactionID) & "Written to serial port OK")
                    Catch ex As Exception
                        Logger.LogMessage("ReceiveTimeout", FormatIDs(TData.TransactionID) & "EXCEPTION: " & ex.ToString)
                        Throw
                    Finally
                        ReleaseSemaphore("ReceiveTimeout", TData.TransactionID)
                    End Try
                Else
                    Logger.LogMessage("ReceiveTimeout", FormatIDs(TData.TransactionID) & "Throwing SerialPortInUse exception")
                    Throw New SerialPortInUseException("Serial:ReceiveTimeout - unable to get serial port semaphore before timeout.")
                End If
            End If
            Logger.LogMessage("ReceiveTimeout", FormatIDs(TData.TransactionID) & "Set to: " & Value / 1000 & " seconds")
        Catch ex As Exception
            Try : Logger.LogMessage("ReceiveTimeout", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            TData.ThreadCompleted()
        End Try

    End Sub

    ''' <summary>
    ''' The maximum time that the ASCOM serial port will wait for incoming receive data (milliseconds, default = 5000)
    ''' </summary>
    ''' <value>Integer, serial port timeout in milli-seconds</value>
    ''' <returns>Integer, serial port timeout in milli-seconds</returns>
    ''' <remarks>If a timeout occurs, an IO timeout error is raised. See ReceiveTimeout for an alternate form 
    ''' using the timeout in seconds. </remarks>
    ''' <exception cref="InvalidValueException">Thrown when <i>value</i> is invalid.</exception>
    Public Property ReceiveTimeoutMs() As Integer Implements ISerial.ReceiveTimeoutMs
        'Get and set the receive timeout in ms
        Get
            Return m_ReceiveTimeout
        End Get
        Set(ByVal value As Integer)
            Dim TData As New ThreadData
            Try
                m_ReceiveTimeout = value 'Save the requested value
                If m_Connected Then ' Try and set the timeout in flight
                    If DebugTrace Then Logger.LogMessage("ReceiveTimeoutMs", "Start")
                    TData.SerialCommand = SerialCommandType.ReceiveTimeoutMs
                    TData.TimeoutValueMs = value
                    TData.TransactionID = GetTransactionID("ReceiveTimeoutMs")
                    ThreadPool.QueueUserWorkItem(AddressOf ReceiveTimeoutMsWorker, TData)
                    WaitForThread(TData) ' Sleep this thread until serial operation is complete

                    If DebugTrace Then Logger.LogMessage("ReceiveTimeoutMs", "Completed: " & TData.Completed)
                    If Not TData.LastException Is Nothing Then Throw TData.LastException
                    TData = Nothing
                Else 'Just report that the value has been set
                    Logger.LogMessage("ReceiveTimeoutMs", "Set to: " & value & " milli-seconds")
                End If

            Catch ex As Exception
                Logger.LogMessage("ReceiveTimeoutMs", ex.ToString)
                Throw
            End Try
        End Set
    End Property

    Private Sub ReceiveTimeoutMsWorker(ByVal TDataObject As Object)
        Dim Value As Integer
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData)
        Try
            Value = TData.TimeoutValueMs

            If DebugTrace Then Logger.LogMessage("ReceiveTimeoutMs", FormatIDs(TData.TransactionID) & "Set Start - TimeOut: " & Value.ToString & "mS")

            If (Value <= 0) Or (Value > 120000) Then Throw New InvalidValueException("ReceiveTimeoutMs", Value.ToString, "1 to 120000 milliseconds")
            m_ReceiveTimeout = Value
            If m_Connected Then
                If DebugTrace Then Logger.LogMessage("ReceiveTimeoutMs", FormatIDs(TData.TransactionID) & "Connected so writing to serial port")
                If GetSemaphore("ReceiveTimeoutMs", TData.TransactionID) Then
                    Try
                        m_Port.WriteTimeout = Value
                        m_Port.ReadTimeout = Value
                        Logger.LogMessage("ReceiveTimeoutMs", FormatIDs(TData.TransactionID) & "Written to serial port OK")
                    Catch ex As Exception
                        Logger.LogMessage("ReceiveTimeoutMs", FormatIDs(TData.TransactionID) & "EXCEPTION: " & ex.ToString)
                        Throw
                    Finally
                        ReleaseSemaphore("ReceiveTimeoutMs", TData.TransactionID)
                    End Try
                Else
                    Logger.LogMessage("ReceiveTimeoutMs", FormatIDs(TData.TransactionID) & "Throwing SerialPortInUse exception")
                    Throw New SerialPortInUseException("Serial:ReceiveTimeoutMs - unable to get serial port semaphore before timeout.")
                End If
            End If
            Logger.LogMessage("ReceiveTimeoutMs", FormatIDs(TData.TransactionID) & "Set to: " & Value.ToString & "ms")
        Catch ex As Exception
            Try : Logger.LogMessage("ReceiveTimeoutMs", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            TData.ThreadCompleted()
        End Try
    End Sub

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
            Logger.LogMessage("Speed", "Set to: " & [Enum].GetName(GetType(SerialSpeed), value))
        End Set
    End Property

    ''' <summary>
    ''' Clears the ASCOM serial port receive and transmit buffers
    ''' </summary>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <remarks></remarks>
    Public Sub ClearBuffers() Implements ISerial.ClearBuffers
        Dim TData As New ThreadData
        Dim TS As New Stopwatch

        TS = Stopwatch.StartNew ' Initialise stopwatch

        Try
            If m_Connected Then 'Clear buffers as we are connected
                If DebugTrace Then Logger.LogMessage("ClearBuffers", "Start")
                TData.SerialCommand = SerialCommandType.ClearBuffers
                TData.TransactionID = GetTransactionID("ClearBuffers")
                ThreadPool.QueueUserWorkItem(AddressOf ClearBuffersWorker, TData)
                WaitForThread(TData) ' Sleep this thread until serial operation is complete

                If DebugTrace Then Logger.LogMessage("ClearBuffers", "Completed: " & TData.Completed & ", Duration: " & TS.Elapsed.TotalMilliseconds.ToString("0.0"))
                If Not TData.LastException Is Nothing Then Throw TData.LastException
                TData = Nothing
            Else ' Not connected so ignore
                Logger.LogMessage("ClearBuffers", "***** ClearBuffers ignored because the port is not connected!")
            End If
        Catch ex As Exception
            Logger.LogMessage("ClearBuffers", ex.ToString)
            Throw
        End Try

    End Sub

    Private Sub ClearBuffersWorker(ByVal TDataObject As Object)
        'Clear both comm buffers
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData)

        Try
            If DebugTrace Then Logger.LogMessage("ClearBuffersWorker", FormatIDs(TData.TransactionID) & " " & CurrentThread.ManagedThreadId & "Start")
            If GetSemaphore("ClearBuffersWorker", TData.TransactionID) Then
                Try
                    If Not (m_Port Is Nothing) Then 'Ensure that ClearBuffers always succeeds for compatibility with MSCOMM32
                        If m_Port.IsOpen Then 'This test is required to maintain compatibility with the original MSCOMM32 control
                            m_Port.DiscardInBuffer()
                            m_Port.DiscardOutBuffer()
                            Logger.LogMessage("ClearBuffersWorker", FormatIDs(TData.TransactionID) & "Completed")
                        Else
                            Logger.LogMessage("ClearBuffersWorker", FormatIDs(TData.TransactionID) & "Command ignored as port is not open")
                        End If
                    End If
                Catch ex As Exception
                    Logger.LogMessage("ClearBuffersWorker", FormatIDs(TData.TransactionID) & "EXCEPTION: " & ex.ToString)
                    Throw
                Finally
                    ReleaseSemaphore("ClearBuffersWorker ", TData.TransactionID)
                End Try
            Else
                Logger.LogMessage("ClearBuffersWorker", FormatIDs(TData.TransactionID) & "Throwing SerialPortInUse exception")
                Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)

            End If
        Catch ex As Exception
            Try : Logger.LogMessage("ClearBuffersWorker", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            TData.ThreadCompleted()
        End Try
    End Sub

    ''' <summary>
    ''' Receive at least one text character from the ASCOM serial port
    ''' </summary>
    ''' <returns>The characters received</returns>
    ''' <exception cref="System.TimeoutException">Thrown when a receive timeout occurs.</exception>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <exception cref="NotConnectedException">Thrown when this command is used before setting Connect = True</exception>
    ''' <remarks>This method reads all of the characters currently in the serial receive buffer. It will not return 
    ''' unless it reads at least one character. A timeout will cause a TimeoutException to be raised. Use this for 
    ''' text data, as it returns a String. </remarks>
    Public Function Receive() As String Implements ISerial.Receive
        'Return all characters in the receive buffer
        Dim Result As String
        Dim TData As New ThreadData
        Dim TS As New Stopwatch

        TS = Stopwatch.StartNew ' Initialise stopwatch

        If m_Connected Then
            Try
                If DebugTrace Then Logger.LogMessage("Receive", "Start")
                TData.SerialCommand = SerialCommandType.Receive
                TData.TransactionID = GetTransactionID("Receive")
                ThreadPool.QueueUserWorkItem(AddressOf ReceiveWorker, TData)
                WaitForThread(TData) ' Sleep this thread until serial operation is complete

                If DebugTrace Then Logger.LogMessage("Receive", "Completed: " & TData.Completed & ", Duration: " & TS.Elapsed.TotalMilliseconds.ToString("0.0"))
                If Not (TData.LastException Is Nothing) Then Throw TData.LastException
                Result = TData.ResultString
                TData = Nothing
            Catch ex As TimeoutException
                Logger.LogMessage("Receive", ex.Message)
                Throw
            Catch ex As Exception
                Logger.LogMessage("Receive", ex.ToString)
                Throw
            End Try
        Else 'Not connected so throw an exception
            Throw New ASCOM.NotConnectedException("Serial port is not connected - you cannot use the Serial.Receive command")
        End If
        Return Result
    End Function

    Private Sub ReceiveWorker(ByVal TDataObject As Object)
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData), Received As String = ""

        Try

            If DebugTrace Then Logger.LogMessage("ReceiveWorker", FormatIDs(TData.TransactionID) & "Start")
            If GetSemaphore("ReceiveWorker", TData.TransactionID) Then
                Try
                    If Not DebugTrace Then Logger.LogStart("ReceiveWorker", FormatIDs(TData.TransactionID) & "< ")
                    Received = ReadChar("ReceiveWorker", TData.TransactionID)
                    Received = Received & m_Port.ReadExisting
                    If DebugTrace Then
                        Logger.LogMessage("ReceiveWorker", FormatIDs(TData.TransactionID) & "< " & Received)
                    Else
                        Logger.LogFinish(Received)
                    End If
                Catch ex As TimeoutException
                    Logger.LogMessage("ReceiveWorker", FormatIDs(TData.TransactionID) & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.ToString)
                    Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
                Catch ex As Exception
                    Logger.LogMessage("ReceiveWorker", FormatIDs(TData.TransactionID) & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.ToString)
                    Throw
                Finally 'Ensure we release the semaphore whatever happens
                    ReleaseSemaphore("ReceiveWorker", TData.TransactionID)
                End Try
                TData.ResultString = Received
            Else
                Logger.LogMessage("ReceiveWorker", FormatIDs(TData.TransactionID) & "Throwing SerialPortInUse exception")
                Throw New SerialPortInUseException("Serial:Receive - unable to get serial port semaphore before timeout.")
            End If
        Catch ex As Exception
            Try : Logger.LogMessage("ReceiveWorker", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            TData.ThreadCompleted()
        End Try
    End Sub

    ''' <summary>
    ''' Receive one binary byte from the ASCOM serial port
    ''' </summary>
    ''' <returns>The received byte</returns>
    ''' <exception cref="System.TimeoutException">Thrown when a receive timeout occurs.</exception>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <exception cref="NotConnectedException">Thrown when this command is used before setting Connect = True</exception>
    ''' <remarks>Use this for 8-bit (binary data). If a timeout occurs, a TimeoutException is raised. </remarks>
    Public Function ReceiveByte() As Byte Implements ISerial.ReceiveByte
        Dim TData As New ThreadData, RetVal As Byte
        Dim TS As New Stopwatch

        TS = Stopwatch.StartNew ' Initialise stopwatch

        If m_Connected Then 'Process command
            Try
                If DebugTrace Then Logger.LogMessage("ReceiveByte", "Start")
                TData.SerialCommand = SerialCommandType.Receivebyte
                TData.TransactionID = GetTransactionID("ReceiveByte")
                ThreadPool.QueueUserWorkItem(AddressOf ReceiveByteWorker, TData)
                WaitForThread(TData) ' Sleep this thread until serial operation is complete

                If DebugTrace Then Logger.LogMessage("ReceiveByte", "Completed: " & TData.Completed & ", Duration: " & TS.Elapsed.TotalMilliseconds.ToString("0.0"))
                If Not TData.LastException Is Nothing Then Throw TData.LastException
                RetVal = TData.ResultByte
                TData = Nothing
                Return RetVal
            Catch ex As TimeoutException
                Logger.LogMessage("ReceiveByte", ex.Message)
                Throw
            Catch ex As Exception
                Logger.LogMessage("ReceiveByte", ex.ToString)
                Throw
            End Try
        Else 'Not connected so throw an exception
            Throw New ASCOM.NotConnectedException("Serial port is not connected - you cannot use the Serial.ReceiveByte command")
        End If
    End Function

    Private Sub ReceiveByteWorker(ByVal TDataObject As Object)
        'Return a single byte of data
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData)
        Dim RetVal As Byte

        Try
            If DebugTrace Then Logger.LogMessage("ReceiveByteWorker", FormatIDs(TData.TransactionID) & "Start")
            If GetSemaphore("ReceiveByteWorker", TData.TransactionID) Then
                Try
                    If Not DebugTrace Then Logger.LogStart("ReceiveByteWorker", FormatIDs(TData.TransactionID) & "< ")
                    RetVal = ReadByte("ReceiveByteWorker", TData.TransactionID)
                    If DebugTrace Then
                        Logger.LogMessage("ReceiveByteWorker", FormatIDs(TData.TransactionID) & " " & Chr(RetVal), True)
                    Else
                        Logger.LogFinish(Chr(RetVal), True)
                    End If
                Catch ex As TimeoutException
                    Logger.LogMessage("ReceiveByteWorker", FormatIDs(TData.TransactionID) & "EXCEPTION: " & ex.ToString)
                    Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
                Catch ex As Exception
                    Logger.LogMessage("ReceiveByteWorker", FormatIDs(TData.TransactionID) & "EXCEPTION: " & ex.ToString)
                    Throw
                Finally 'Ensure we release the semaphore whatever happens
                    ReleaseSemaphore("ReceiveByteWorker", TData.TransactionID)
                End Try
                TData.ResultByte = RetVal
            Else
                Logger.LogMessage("ReceiveByteWorker", FormatIDs(TData.TransactionID) & "Throwing SerialPortInUse exception")
                Throw New SerialPortInUseException("Serial:ReceiveByte - unable to get serial port semaphore before timeout.")
            End If
        Catch ex As Exception
            Try : Logger.LogMessage("ReceiveByteWorker", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            TData.ThreadCompleted()
        End Try
    End Sub

    ''' <summary>
    ''' Receive exactly the given number of characters from the ASCOM serial port and return as a string
    ''' </summary>
    ''' <param name="Count">The number of characters to return</param>
    ''' <returns>String of length "Count" characters</returns>
    ''' <exception cref="System.TimeoutException">Thrown when a receive timeout occurs.</exception>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <exception cref="NotConnectedException">Thrown when this command is used before setting Connect = True</exception>
    ''' <remarks>If a timeout occurs a TimeoutException is raised.</remarks>
    Public Function ReceiveCounted(ByVal Count As Integer) As String Implements ISerial.ReceiveCounted
        Dim TData As New ThreadData, RetVal As String
        Dim TS As New Stopwatch

        TS = Stopwatch.StartNew ' Initialise stopwatch

        If m_Connected Then 'Process command
            Try
                If DebugTrace Then Logger.LogMessage("ReceiveCounted", "Start")
                TData.SerialCommand = SerialCommandType.ReceiveCounted
                TData.Count = Count
                TData.TransactionID = GetTransactionID("ReceiveCounted")
                ThreadPool.QueueUserWorkItem(AddressOf ReceiveCountedWorker, TData)
                WaitForThread(TData) ' Sleep this thread until serial operation is complete

                If DebugTrace Then Logger.LogMessage("ReceiveCounted", "Completed: " & TData.Completed & ", Duration: " & TS.Elapsed.TotalMilliseconds.ToString("0.0"))
                If Not TData.LastException Is Nothing Then Throw TData.LastException
                RetVal = TData.ResultString
                TData = Nothing
                Return RetVal
            Catch ex As TimeoutException
                Logger.LogMessage("ReceiveCounted", ex.Message)
                Throw
            Catch ex As Exception
                Logger.LogMessage("ReceiveCounted", ex.ToString)
                Throw
            End Try
        Else 'Not connected so throw an exception
            Throw New ASCOM.NotConnectedException("Serial port is not connected - you cannot use the Serial.ReceiveCounted command")
        End If
    End Function

    Private Sub ReceiveCountedWorker(ByVal TDataObject As Object)
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData)
        Dim i As Integer, Received As String = ""

        Try
            'Return a fixed number of characters
            If DebugTrace Then Logger.LogMessage("ReceiveCountedWorker", FormatIDs(TData.TransactionID) & "Start - count: " & TData.Count.ToString)
            If GetSemaphore("ReceiveCountedWorker", TData.TransactionID) Then
                Try
                    If Not DebugTrace Then Logger.LogStart("ReceiveCountedWorker " & TData.Count.ToString, FormatIDs(TData.TransactionID) & "< ")
                    For i = 1 To TData.Count
                        Received = Received & ReadChar("ReceiveCountedWorker", TData.TransactionID)
                    Next
                    If DebugTrace Then
                        Logger.LogMessage("ReceiveCountedWorker", FormatIDs(TData.TransactionID) & "< " & Received)
                    Else
                        Logger.LogFinish(Received)
                    End If
                Catch ex As TimeoutException
                    Logger.LogMessage("ReceiveCountedWorker", FormatIDs(TData.TransactionID) & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.Message)
                    Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
                Catch ex As Exception
                    Logger.LogMessage("ReceiveCountedWorker", FormatIDs(TData.TransactionID) & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.Message)
                    Throw
                Finally 'Ensure we release the semaphore whatever happens
                    ReleaseSemaphore("ReceiveCountedWorker", TData.TransactionID)
                End Try
                TData.ResultString = Received
            Else
                Logger.LogMessage("ReceiveCountedWorker", FormatIDs(TData.TransactionID) & "Throwing SerialPortInUse exception")
                Throw New SerialPortInUseException("ReceiveCounted - unable to get serial port semaphore before timeout.")
            End If
        Catch ex As Exception
            Try : Logger.LogMessage("ReceiveCountedWorker", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            TData.ThreadCompleted()
        End Try
    End Sub

    ''' <summary>
    ''' Receive exactly the given number of characters from the ASCOM serial port and return as a byte array
    ''' </summary>
    ''' <param name="Count">The number of characters to return</param>
    ''' <returns>Byte array of size "Count" elements</returns>
    ''' <exception cref="System.TimeoutException">Thrown when a receive timeout occurs.</exception>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <exception cref="NotConnectedException">Thrown when this command is used before setting Connect = True</exception>
    ''' <remarks>
    ''' <para>If a timeout occurs, a TimeoutException is raised. </para>
    ''' <para>This function exists in the COM component but is not documented in the help file.</para>
    ''' </remarks>
    Public Function ReceiveCountedBinary(ByVal Count As Integer) As Byte() Implements ISerial.ReceiveCountedBinary
        Dim Result As Byte()
        Dim TData As New ThreadData
        Dim TS As New Stopwatch

        TS = Stopwatch.StartNew ' Initialise stopwatch

        If m_Connected Then 'Process command
            Try
                If DebugTrace Then Logger.LogMessage("ReceiveCountedBinary", "Start")
                TData.SerialCommand = SerialCommandType.ReceiveCountedBinary
                TData.Count = Count
                TData.TransactionID = GetTransactionID("ReceiveCountedBinary")
                ThreadPool.QueueUserWorkItem(AddressOf ReceiveCountedBinaryWorker, TData)
                WaitForThread(TData) ' Sleep this thread until serial operation is complete

                If DebugTrace Then Logger.LogMessage("ReceiveCountedBinary", "Completed: " & TData.Completed & ", Duration: " & TS.Elapsed.TotalMilliseconds.ToString("0.0"))
                If Not (TData.LastException Is Nothing) Then Throw TData.LastException
                Result = TData.ResultByteArray
                TData = Nothing
                Return Result

            Catch ex As TimeoutException
                Logger.LogMessage("ReceiveCountedBinary", ex.Message)
                Throw
            Catch ex As Exception
                Logger.LogMessage("ReceiveCountedBinary", ex.ToString)
                Throw
            End Try
        Else 'Not connected so throw an exception
            Throw New ASCOM.NotConnectedException("Serial port is not connected - you cannot use the Serial.ReceiveCountedBinary command")
        End If
    End Function

    Private Sub ReceiveCountedBinaryWorker(ByVal TDataObject As Object)
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData)
        Dim i As Integer, Received(0) As Byte
        Dim TextEncoding As System.Text.Encoding

        Try
            TextEncoding = System.Text.Encoding.GetEncoding(1252)

            If DebugTrace Then Logger.LogMessage("ReceiveCountedBinaryWorker ", FormatIDs(TData.TransactionID) & "Start - count: " & TData.Count.ToString)

            If GetSemaphore("ReceiveCountedBinaryWorker ", TData.TransactionID) Then
                Try
                    If Not DebugTrace Then Logger.LogStart("ReceiveCountedBinaryWorker " & TData.Count.ToString, FormatIDs(TData.TransactionID) & "< ")
                    For i = 0 To TData.Count - 1
                        ReDim Preserve Received(i)
                        Received(i) = ReadByte("ReceiveCountedBinaryWorker ", TData.TransactionID)
                    Next
                    If DebugTrace Then
                        Logger.LogMessage("ReceiveCountedBinaryWorker ", FormatIDs(TData.TransactionID) & "< " & TextEncoding.GetString(Received), True)
                    Else
                        Logger.LogFinish(TextEncoding.GetString(Received), True)
                    End If
                Catch ex As TimeoutException
                    Logger.LogMessage("ReceiveCountedBinaryWorker ", FormatIDs(TData.TransactionID) & " " & FormatRXSoFar(TextEncoding.GetString(Received)) & "EXCEPTION: " & ex.Message)
                    Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
                Catch ex As Exception
                    Logger.LogMessage("ReceiveCountedBinaryWorker ", FormatIDs(TData.TransactionID) & " " & FormatRXSoFar(TextEncoding.GetString(Received)) & "EXCEPTION: " & ex.Message)
                    Throw
                Finally
                    ReleaseSemaphore("ReceiveCountedBinaryWorker ", TData.TransactionID)
                End Try
                TData.ResultByteArray = Received
            Else
                Logger.LogMessage("ReceiveCountedBinaryWorker ", FormatIDs(TData.TransactionID) & "Throwing SerialPortInUse exception")
                Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
            End If
        Catch ex As Exception
            Try : Logger.LogMessage("ReceiveCountedBinaryWorker", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            TData.ThreadCompleted()
        End Try
    End Sub

    ''' <summary>
    ''' Receive characters from the ASCOM serial port until the given terminator string is seen
    ''' </summary>
    ''' <param name="Terminator">The character string that indicates end of message</param>
    ''' <returns>Received characters including the terminator string</returns>
    ''' <exception cref="System.TimeoutException">Thrown when a receive timeout occurs.</exception>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <exception cref="NotConnectedException">Thrown when this command is used before setting Connect = True</exception>
    ''' <remarks>If a timeout occurs, a TimeoutException is raised.</remarks>
    Public Function ReceiveTerminated(ByVal Terminator As String) As String Implements ISerial.ReceiveTerminated
        Dim TData As New ThreadData, RetVal As String

        ts = Stopwatch.StartNew ' Initialise stopwatch

        If m_Connected Then 'Process command
            Try
                If DebugTrace Then Logger.LogMessage("ReceiveTerminated", "Start")
                TData.SerialCommand = SerialCommandType.ReceiveTerminated
                TData.Terminator = Terminator
                TData.TransactionID = GetTransactionID("ReceiveTerminated")
                ThreadPool.QueueUserWorkItem(AddressOf ReceiveTerminatedWorker, TData)
                WaitForThread(TData) ' Sleep this thread until serial operation is complete

                If DebugTrace Then Logger.LogMessage("ReceiveTerminated", "Completed: " & TData.Completed & ", Duration: " & ts.Elapsed.TotalMilliseconds.ToString("0.0"))
                If Not TData.LastException Is Nothing Then Throw TData.LastException
                RetVal = TData.ResultString
                TData = Nothing
                Return RetVal

            Catch ex As TimeoutException
                Logger.LogMessage("ReceiveTerminated", ex.Message)
                Throw
            Catch ex As Exception
                Logger.LogMessage("ReceiveTerminated", ex.ToString)
                Throw
            End Try
        Else 'Not connected so throw an exception
            Throw New ASCOM.NotConnectedException("Serial port is not connected - you cannot use the Serial.ReceiveTerminated command")
        End If
    End Function

    Private Sub ReceiveTerminatedWorker(ByVal TDataObject As Object)
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData)
        Dim Terminated As Boolean, tLen As Integer, Received As String = ""

        Try
            'Return all characters up to and including a specified terminator string
            If DebugTrace Then Logger.LogMessage("ReceiveTerminatedWorker ", FormatIDs(TData.TransactionID) & "Start - terminator: """ & TData.Terminator.ToString & """")
            'Check for bad terminator string
            If TData.Terminator = "" Then Throw New InvalidValueException("ReceiveTerminated Terminator", "Null or empty string", "Character or character string")

            If GetSemaphore("ReceiveTerminatedWorker ", TData.TransactionID) Then
                Try
                    If Not DebugTrace Then Logger.LogStart("ReceiveTerminatedWorker " & TData.Terminator.ToString, FormatIDs(TData.TransactionID) & "< ")
                    tLen = Len(TData.Terminator)
                    Do
                        Received = Received & ReadChar("ReceiveTerminatedWorker ", TData.TransactionID) ' Build up the string one char at a time
                        If Len(Received) >= tLen Then ' Check terminator when string is long enough
                            If Right(Received, tLen) = TData.Terminator Then Terminated = True
                        End If
                    Loop Until Terminated
                    If DebugTrace Then
                        Logger.LogMessage("ReceiveTerminatedWorker ", FormatIDs(TData.TransactionID) & "< " & Received)
                    Else
                        Logger.LogFinish(Received)
                    End If
                Catch ex As InvalidValueException
                    Logger.LogMessage("ReceiveTerminatedWorker ", FormatIDs(TData.TransactionID) & "EXCEPTION - Terminator cannot be a null string")
                    Throw
                Catch ex As TimeoutException
                    Logger.LogMessage("ReceiveTerminatedWorker ", FormatIDs(TData.TransactionID) & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.ToString)
                    Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
                Catch ex As Exception
                    Logger.LogMessage("ReceiveTerminatedWorker ", FormatIDs(TData.TransactionID) & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.ToString)
                    Throw
                Finally 'Ensure we release the semaphore whatever happens
                    ReleaseSemaphore("ReceiveTerminatedWorker ", TData.TransactionID)
                End Try
                TData.ResultString = Received
            Else
                Logger.LogMessage("ReceiveTerminatedWorker", FormatIDs(TData.TransactionID) & "Throwing SerialPortInUse exception")
                Throw New SerialPortInUseException("Serial:ReceiveTerminated - unable to get serial port semaphore before timeout.")
            End If

        Catch ex As Exception
            Try : Logger.LogMessage("ReceiveTerminatedWorker", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            TData.ThreadCompleted()
        End Try
    End Sub

    ''' <summary>
    ''' Receive characters from the ASCOM serial port until the given terminator bytes are seen, return as a byte array
    ''' </summary>
    ''' <param name="TerminatorBytes">Array of bytes that indicates end of message</param>
    ''' <returns>Byte array of received characters</returns>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <exception cref="NotConnectedException">Thrown when this command is used before setting Connect = True</exception>
    ''' <remarks>
    ''' <para>If a timeout occurs, a TimeoutException is raised.</para>
    ''' <para>This function exists in the COM component but is not documented in the help file.</para>
    ''' </remarks>
    Public Function ReceiveTerminatedBinary(ByVal TerminatorBytes() As Byte) As Byte() Implements ISerial.ReceiveTerminatedBinary
        Dim Result() As Byte, TData As New ThreadData

        ts = Stopwatch.StartNew ' Initialise stopwatch

        If m_Connected Then 'Process command
            Try
                If DebugTrace Then Logger.LogMessage("ReceiveTerminatedBinary", "Start")
                TData.SerialCommand = SerialCommandType.ReceiveCounted
                TData.TerminatorBytes = TerminatorBytes
                TData.TransactionID = GetTransactionID("ReceiveTerminatedBinary")
                ThreadPool.QueueUserWorkItem(AddressOf ReceiveTerminatedBinaryWorker, TData)
                WaitForThread(TData) ' Sleep this thread until serial operation is complete

                If DebugTrace Then Logger.LogMessage("ReceiveTerminatedBinary", "Completed: " & TData.Completed & ", Duration: " & ts.Elapsed.TotalMilliseconds.ToString("0.0"))
                If Not (TData.LastException Is Nothing) Then Throw TData.LastException
                Result = TData.ResultByteArray
                TData = Nothing
                Return Result

            Catch ex As TimeoutException
                Logger.LogMessage("ReceiveTerminatedBinary", ex.Message)
                Throw
            Catch ex As Exception
                Logger.LogMessage("ReceiveTerminatedBinary", ex.ToString)
                Throw
            End Try
        Else 'Not connected so throw an exception
            Throw New ASCOM.NotConnectedException("Serial port is not connected - you cannot use the Serial.ReceiveTerminatedBinary command")
        End If

    End Function

    Private Sub ReceiveTerminatedBinaryWorker(ByVal TDataObject As Object)
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData)
        Dim Terminated As Boolean, tLen As Integer, Terminator As String, Received As String = ""
        Dim TextEncoding As System.Text.Encoding

        Try
            TextEncoding = System.Text.Encoding.GetEncoding(1252)
            Terminator = TextEncoding.GetString(TData.TerminatorBytes)

            If DebugTrace Then Logger.LogMessage("ReceiveTerminatedBinaryWorker ", FormatIDs(TData.TransactionID) & "Start - terminator: """ & Terminator.ToString & """")
            'Check for bad terminator string
            If Terminator = "" Then Throw New InvalidValueException("ReceiveTerminatedBinary Terminator", "Null or empty string", "Character or character string")

            If GetSemaphore("ReceiveTerminatedBinaryWorker ", TData.TransactionID) Then
                Try
                    If Not DebugTrace Then Logger.LogStart("ReceiveTerminatedBinaryWorker " & Terminator.ToString, FormatIDs(TData.TransactionID) & "< ")

                    tLen = Len(Terminator)
                    Do
                        Received = Received & ReadChar("ReceiveTerminatedBinaryWorker ", TData.TransactionID) ' Build up the string one char at a time
                        If Len(Received) >= tLen Then ' Check terminator when string is long enough
                            If Right(Received, tLen) = Terminator Then Terminated = True
                        End If
                    Loop Until Terminated
                    If DebugTrace Then
                        Logger.LogMessage("ReceiveTerminatedBinaryWorker ", FormatIDs(TData.TransactionID) & "< " & Received, True)
                    Else
                        Logger.LogFinish(Received, True)
                    End If
                Catch ex As InvalidValueException
                    Logger.LogMessage("ReceiveTerminatedBinaryWorker ", FormatIDs(TData.TransactionID) & "EXCEPTION - Terminator cannot be a null string")
                    Throw
                Catch ex As TimeoutException
                    Logger.LogMessage("ReceiveTerminatedBinaryWorker ", FormatIDs(TData.TransactionID) & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.ToString)
                    Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)
                Catch ex As Exception
                    Logger.LogMessage("ReceiveTerminatedBinaryWorker ", FormatIDs(TData.TransactionID) & " " & FormatRXSoFar(Received) & "EXCEPTION: " & ex.ToString)
                    Throw
                Finally
                    ReleaseSemaphore("ReceiveTerminatedBinaryWorker ", TData.TransactionID)
                End Try
                TData.ResultByteArray = TextEncoding.GetBytes(Received)
            Else
                Logger.LogMessage("ReceiveTerminatedBinaryWorker ", FormatIDs(TData.TransactionID) & "Throwing SerialPortInUse exception")
                'Throw New SerialPortInUseException("Serial:ReceiveTerminatedBinary - unable to get serial port semaphore before timeout.")
                Throw New System.Runtime.InteropServices.COMException(TIMEOUT_MESSAGE, TIMEOUT_NUMBER)

            End If
        Catch ex As Exception
            Try : Logger.LogMessage("ReceiveTerminatedBinaryWorker", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            TData.ThreadCompleted()
        End Try
    End Sub

    ''' <summary>
    ''' Transmits a string through the ASCOM serial port
    ''' </summary>
    ''' <param name="Data">String to transmit</param>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <exception cref="NotConnectedException">Thrown when this command is used before setting Connect = True</exception>
    ''' <remarks></remarks>
    Public Sub Transmit(ByVal Data As String) Implements ISerial.Transmit
        Dim TData As New ThreadData

        ts = Stopwatch.StartNew ' Initialise stopwatch

        If m_Connected Then 'Process command
            Try
                If DebugTrace Then Logger.LogMessage("Transmit", "Start")
                TData.SerialCommand = SerialCommandType.Transmit
                TData.TransmitString = Data
                TData.TransactionID = GetTransactionID("Transmit")
                ThreadPool.QueueUserWorkItem(AddressOf TransmitWorker, TData)
                WaitForThread(TData) ' Sleep this thread until serial operation is complete

                If DebugTrace Then Logger.LogMessage("Transmit", "Completed: " & TData.Completed & ", Duration: " & ts.Elapsed.TotalMilliseconds.ToString("0.0"))
                If Not TData.LastException Is Nothing Then Throw TData.LastException
                TData = Nothing

            Catch ex As TimeoutException
                Logger.LogMessage("Transmit", ex.Message)
                Throw
            Catch ex As Exception
                Logger.LogMessage("Transmit", ex.ToString)
                Throw
            End Try
        Else 'Not connected so throw an exception
            Throw New ASCOM.NotConnectedException("Serial port is not connected - you cannot use the Serial.Transmit command")
        End If
    End Sub

    Private Sub TransmitWorker(ByVal TDataObject As Object)
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData)

        Try
            'Send provided string to the serial port
            If DebugTrace Then Logger.LogMessage("TransmitWorker", FormatIDs(TData.TransactionID) & "> " & TData.TransmitString)
            If GetSemaphore("TransmitWorker", TData.TransactionID) Then
                Try
                    If Not DebugTrace Then Logger.LogMessage("TransmitWorker", FormatIDs(TData.TransactionID) & "> " & TData.TransmitString)
                    m_Port.Write(TData.TransmitString)
                    If DebugTrace Then Logger.LogMessage("TransmitWorker", FormatIDs(TData.TransactionID) & "Completed")
                Catch ex As Exception
                    Logger.LogMessage("TransmitWorker", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString)
                    Throw
                Finally 'Ensure we release the semaphore whatever happens
                    ReleaseSemaphore("TransmitWorker", TData.TransactionID)
                End Try
            Else
                Logger.LogMessage("TransmitWorker", FormatIDs(TData.TransactionID) & "Throwing SerialPortInUse exception")
                Throw New SerialPortInUseException("Serial:Transmit - unable to get serial port semaphore before timeout.")
            End If
        Catch ex As Exception
            Try : Logger.LogMessage("TransmitWorker", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            TData.ThreadCompleted()
        End Try
    End Sub

    ''' <summary>
    ''' Transmit an array of binary bytes through the ASCOM serial port 
    ''' </summary>
    ''' <param name="Data">Byte array to transmit</param>
    ''' <exception cref="SerialPortInUseException">Thrown when unable to acquire the serial port</exception>
    ''' <exception cref="NotConnectedException">Thrown when this command is used before setting Connect = True</exception>
    ''' <remarks></remarks>
    Public Sub TransmitBinary(ByVal Data() As Byte) Implements ISerial.TransmitBinary
        Dim Result As String, TData As New ThreadData

        ts = Stopwatch.StartNew ' Initialise stopwatch

        If m_Connected Then 'Process command
            Try
                If DebugTrace Then Logger.LogMessage("TransmitBinary", "Start")
                TData.SerialCommand = SerialCommandType.ReceiveCounted
                TData.TransmitBytes = Data
                TData.TransactionID = GetTransactionID("TransmitBinary")
                ThreadPool.QueueUserWorkItem(AddressOf TransmitBinaryWorker, TData)
                WaitForThread(TData) ' Sleep this thread until serial operation is complete

                If DebugTrace Then Logger.LogMessage("TransmitBinary", "Completed: " & TData.Completed & ", Duration: " & ts.Elapsed.TotalMilliseconds.ToString("0.0"))
                If Not (TData.LastException Is Nothing) Then Throw TData.LastException
                Result = TData.ResultString
                TData = Nothing

            Catch ex As TimeoutException
                Logger.LogMessage("TransmitBinary", ex.Message)
                Throw
            Catch ex As Exception
                Logger.LogMessage("TransmitBinary", ex.ToString)
                'Throw New ASCOM.Utilities.Exceptions.SerialPortInUseException("Serial Port Issue", ex)
                Throw
            End Try
        Else 'Not connected so throw an exception
            Throw New ASCOM.NotConnectedException("Serial port is not connected - you cannot use the Serial.TransmitBinary command")
        End If
    End Sub

    Private Sub TransmitBinaryWorker(ByVal TDataObject As Object)
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData)
        Dim TxString As String

        Try
            TxString = TextEncoding.GetString(TData.TransmitBytes)
            If DebugTrace Then Logger.LogMessage("TransmitBinaryWorker ", FormatIDs(TData.TransactionID) & "> " & TxString & " (HEX" & MakeHex(TxString) & ") ")

            If GetSemaphore("TransmitBinaryWorker ", TData.TransactionID) Then
                Try
                    If Not DebugTrace Then Logger.LogMessage("TransmitBinaryWorker", FormatIDs(TData.TransactionID) & "> " & TxString & " (HEX" & MakeHex(TxString) & ") ")
                    m_Port.Write(TData.TransmitBytes, 0, TData.TransmitBytes.Length)
                    If DebugTrace Then Logger.LogMessage("TransmitBinaryWorker ", FormatIDs(TData.TransactionID) & "Completed")
                Catch ex As Exception
                    Logger.LogMessage("TransmitBinaryWorker ", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString)
                    Throw
                Finally 'Ensure we release the semaphore whatever happens
                    ReleaseSemaphore("TransmitBinaryWorker ", TData.TransactionID)
                End Try
            Else
                Logger.LogMessage("TransmitBinaryWorker ", FormatIDs(TData.TransactionID) & "Throwing SerialPortInUse exception")
                Throw New SerialPortInUseException("TransmitBinary - unable to get serial port semaphore before timeout.")
            End If
        Catch ex As Exception
            Try : Logger.LogMessage("TransmitBinaryWorker", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            TData.ThreadCompleted()
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
    ''' <remarks><b>Update in platform 6.0.0.0</b> This call uses the .NET Framework to retrieve available 
    ''' COM ports and this has been found not to return names of some USB serial adapters. Additional 
    ''' code has been added to attempt to open all COM ports up to COM32. Any ports that can be 
    ''' successfully opened are now returned alongside the ports returned by the .NET call.
    ''' <para>If this new approach still does not detect a COM port it can be forced to appear in the list by adding its name
    ''' as a string entry in the ForceCOMPorts key of the ASCOM Profile. In the event that this scanning causes issues, a COM port can be 
    ''' omitted from the scan by adding its name as a string entry in the IgnoreCOMPorts key of the ASCOM Profile.</para></remarks>
    Public ReadOnly Property AvailableCOMPorts() As String() Implements ISerial.AvailableComPorts
        'Returns a list of all available COM ports sorted into ascending COM port number order
        Get
            Dim PortNames As Generic.List(Of String)
            Dim myPortNameComparer As New PortNameComparer
            Dim TData As New ThreadData

            Try
                'Get the current lists of forced and ignored COM ports
                ForcedCOMPorts = SerialProfile.EnumProfile(SERIAL_FORCED_COMPORTS_VARNAME)
                Try : ForcedCOMPorts.Remove("") : Catch : End Try 'Remove the default value
                For Each kvp As Generic.KeyValuePair(Of String, String) In ForcedCOMPorts
                    Logger.LogMessage("AvailableCOMPorts", "Forcing COM port " & kvp.Key & " " & kvp.Value & " to appear")
                Next

                IgnoredCOMPorts = SerialProfile.EnumProfile(SERIAL_IGNORE_COMPORTS_VARNAME)
                Try : IgnoredCOMPorts.Remove("") : Catch : End Try 'Remove the default value
                For Each kvp As Generic.KeyValuePair(Of String, String) In IgnoredCOMPorts
                    Logger.LogMessage("AvailableCOMPorts", "Ignoring COM port " & kvp.Key & " " & kvp.Value)
                Next
            Catch ex As Exception
                Logger.LogMessage("AvailableCOMPorts Profile", ex.ToString)
            End Try

            'Find COM port names from the framework
            If DebugTrace Then Logger.LogMessage("AvailableCOMPorts", "Entered AvailableCOMPorts")
            PortNames = New Generic.List(Of String)(SerialPort.GetPortNames)
            If DebugTrace Then Logger.LogMessage("AvailableCOMPorts", "Retrieved port names using SerialPort.GetPortNames")

            'Add any forced ports that aren't already in the list
            For Each PortName As Generic.KeyValuePair(Of String, String) In ForcedCOMPorts
                If Not PortNames.Contains(Trim(PortName.Key)) Then
                    PortNames.Add(Trim(PortName.Key))
                End If
            Next

            'Add any ignored ports that aren't already in the list so that these are not scanned
            For Each PortName As Generic.KeyValuePair(Of String, String) In IgnoredCOMPorts
                If Not PortNames.Contains(Trim(PortName.Key)) Then
                    PortNames.Add(Trim(PortName.Key))
                End If
            Next

            'Some ports aren't returned by the framework method so probe for them
            Try
                If DebugTrace Then Logger.LogMessage("AvailableCOMPorts", "Start")
                TData.SerialCommand = SerialCommandType.AvailableCOMPorts
                TData.AvailableCOMPorts = PortNames
                TData.TransactionID = GetTransactionID("AvailableCOMPorts")
                ThreadPool.QueueUserWorkItem(AddressOf AvailableCOMPortsWorker, TData)
                WaitForThread(TData) ' Sleep this thread until serial operation is complete

                If DebugTrace Then Logger.LogMessage("AvailableCOMPorts", "Completed: " & TData.Completed)
                If Not (TData.LastException Is Nothing) Then Throw TData.LastException
                PortNames = TData.AvailableCOMPorts
                TData = Nothing
            Catch ex As Exception
                Logger.LogMessage("AvailableCOMPorts", ex.ToString)
                Throw
            End Try

            'Now remove the ports that are to be ignored and log the rest
            For Each PortName As Generic.KeyValuePair(Of String, String) In IgnoredCOMPorts
                If PortNames.Contains(Trim(PortName.Key)) Then PortNames.Remove(Trim(PortName.Key))
            Next

            PortNames.Sort(myPortNameComparer) 'Use specialised comparer to get the sort order right

            For Each PortName As String In PortNames
                Logger.LogMessage("AvailableCOMPorts", PortName)
            Next
            If DebugTrace Then Logger.LogMessage("AvailableCOMPorts", "Finished")
            Return PortNames.ToArray
        End Get
    End Property

    Private Sub AvailableCOMPortsWorker(ByVal TDataObject As Object)
        'Test for available COM ports
        Dim TData As ThreadData = DirectCast(TDataObject, ThreadData)
        Dim PortNumber As Integer, PortName As String = "", RetVal As Generic.List(Of String)
        Dim SWatch As New Stopwatch, SerPort As New SerialPort

        Try
            If DebugTrace Then Logger.LogMessage("AvailableCOMPortsWorker", FormatIDs(TData.TransactionID) & "Started")

            Try
                If DebugTrace Then Logger.LogMessage("AvailableCOMPortsWorker", FormatIDs(TData.TransactionID) & "Port probe started")
                RetVal = TData.AvailableCOMPorts
                SerPort.ReadTimeout = AVAILABLE_PORTS_SERIAL_TIMEOUT 'Set low timeouts so the process completes quickly
                SerPort.WriteTimeout = AVAILABLE_PORTS_SERIAL_TIMEOUT

                For PortNumber = 0 To 32
                    Try
                        PortName = "COM" & PortNumber.ToString
                        If Not RetVal.Contains(PortName) Then ' Only test ports we don't yet know about
                            If DebugTrace Then Logger.LogMessage("AvailableCOMPortsWorker", FormatIDs(TData.TransactionID) & "Starting to probe port " & PortNumber)

                            SWatch.Reset() 'Reset and start the timer stopwatch
                            SWatch.Start()
                            SerPort.PortName = PortName 'Set the port name and attempt to open it
                            SerPort.Open()
                            SerPort.Close()
                            SWatch.Stop() 'Stop the timer

                            'If we get here without an exception, the port must exist, so check whether it took a long time and report if it did
                            If SWatch.ElapsedMilliseconds >= AVAILABLE_PORTS_SERIAL_TIMEOUT_REPORT_THRESHOLD Then Logger.LogMessage("AvailableCOMPortsWorker", FormatIDs(TData.TransactionID) & "Probing port " & PortName & " took  a long time: " & SWatch.ElapsedMilliseconds & "ms")

                            'Its real so add it to the list, i.e. no exception was generated!
                            RetVal.Add(PortName)
                            If DebugTrace Then Logger.LogMessage("AvailableCOMPortsWorker", FormatIDs(TData.TransactionID) & "Port " & PortNumber & " exists, elapsed time: " & SWatch.ElapsedMilliseconds & "ms")
                        Else
                            If DebugTrace Then Logger.LogMessage("AvailableCOMPortsWorker", FormatIDs(TData.TransactionID) & "Skiping probe as port  " & PortName & " is already known to exist")
                        End If
                    Catch ex As System.UnauthorizedAccessException
                        'Port exists but is in use so add it to the list
                        RetVal.Add(PortName)
                        If DebugTrace Then Logger.LogMessage("AvailableCOMPortsWorker", FormatIDs(TData.TransactionID) & "Port " & PortNumber & " UnauthorisedAccessException, elapsed time: " & SWatch.ElapsedMilliseconds & "ms")
                    Catch ex As Exception 'Ignore other exceptions as these indicate port not present or not openable
                        If DebugTrace Then Logger.LogMessage("AvailableCOMPortsWorker", FormatIDs(TData.TransactionID) & "Port " & PortNumber & " Exception, found is, elapsed time: " & SWatch.ElapsedMilliseconds & "ms " & ex.Message)
                    End Try
                Next
                TData.AvailableCOMPorts = RetVal 'Save updated array for return to the calling thread

                If DebugTrace Then Logger.LogMessage("AvailableCOMPortsWorker ", FormatIDs(TData.TransactionID) & "Completed")
            Catch ex As Exception
                Logger.LogMessage("AvailableCOMPortsWorker ", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString)
                Throw
            End Try

        Catch ex As Exception
            Try : Logger.LogMessage("AvailableCOMPortsWorker", FormatIDs(TData.TransactionID) & "Exception: " & ex.ToString) : Catch : End Try
            Try : TData.LastException = ex : Catch : End Try
        Finally
            Try : SerPort.Dispose() : Catch : End Try 'Dispose of the COM port
            Try : SerPort = Nothing : Catch : End Try
            TData.ThreadCompleted()
        End Try
        'This thread ends here so the calling WaitForThread releases the main thread to continue execution
    End Sub

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

    Private Function GetSemaphore(ByVal p_Caller As String, ByVal MyCallNumber As Long) As Boolean 'Gets the serial control semaphore and returns success or failure
        Dim GotSemaphore As Boolean, StartTime As Date
        StartTime = Now 'Save the current start time so we will wait longer than a port timeout before giving up on getting the semaphore
        Try
            If DebugTrace Then Logger.LogMessage(p_Caller, FormatIDs(MyCallNumber) & "Entered GetSemaphore ")
            GotSemaphore = SerSemaphore.WaitOne(m_ReceiveTimeout + 2000, False) 'Peter 17/11/09 Converted to use the framework 2.0 rather than 3.5 call
            If DebugTrace Then
                If GotSemaphore Then
                    Logger.LogMessage(p_Caller, FormatIDs(MyCallNumber) & "Got Semaphore OK")
                Else
                    Logger.LogMessage(p_Caller, FormatIDs(MyCallNumber) & "Failed to get Semaphore, returning: False")
                End If
            End If
        Catch ex As Exception 'Log abandoned mutex exception
            Logger.LogMessage("GetSemaphore", MyCallNumber.ToString & CurrentThread.ManagedThreadId & " " & "Exception: " & ex.ToString & " " & ex.StackTrace)
        End Try
        'Return False 'Didn't get the semaphore so return false
        Return GotSemaphore
    End Function

    Private Sub ReleaseSemaphore(ByVal p_Caller As String, ByVal MyCallNumber As Long) 'Release the semaphore if we have it but don't error if there is a problem
        Try
            If DebugTrace Then Logger.LogMessage(p_Caller, FormatIDs(MyCallNumber) & "Entered ReleaseSemaphore " & CurrentThread.ManagedThreadId)
            SerSemaphore.Release()
            If DebugTrace Then Logger.LogMessage(p_Caller, FormatIDs(MyCallNumber) & "Semaphore released OK")
        Catch ex As System.Threading.SemaphoreFullException
            Logger.LogMessage("ReleaseSemaphore", FormatIDs(MyCallNumber) & "SemaphoreFullException: " & ex.ToString & " " & ex.StackTrace)
        Catch ex As Exception
            Logger.LogMessage("ReleaseSemaphore", FormatIDs(MyCallNumber) & "Exception: " & ex.ToString & " " & ex.StackTrace)
        End Try
    End Sub

    Private Function GetTransactionID(ByVal p_Caller As String) As Long
        Dim ReturnValue As Long
        Try
            If DebugTrace Then Logger.LogMessage(p_Caller, FormatIDs(0) & "Entered GetNextCount ")
            CallCountSemaphore.WaitOne()
            If DebugTrace Then Logger.LogMessage(p_Caller, FormatIDs(0) & "Got CallCountMutex")

            If CallCount <> Long.MaxValue Then ' Normal case so just increment the counter
                CallCount += 1
            Else ' Somehow we have got to the maximum value of a Long variable so reset to zero and start the cycle again!
                CallCount = 0
            End If

            ReturnValue = CallCount
            CallCountSemaphore.Release()
            If DebugTrace Then Logger.LogMessage(p_Caller, FormatIDs(ReturnValue) & "Released CallCountMutex")
        Catch ex As Exception
            Logger.LogMessage(p_Caller, "EXCEPTION: " & ex.ToString)
            Throw
        End Try
        Return ReturnValue
    End Function

    Private Function ReadByte(ByVal p_Caller As String, ByVal MyCallNumber As Long) As Byte
        Dim StartTime As Date, RxByte As Byte, RxBytes(10) As Byte
        StartTime = Now
        If DebugTrace Then Logger.LogMessage(p_Caller, FormatIDs(MyCallNumber) & "Entered ReadByte: " & UseReadPolling)
        If UseReadPolling Then
            While (m_Port.BytesToRead = 0)
                If (Now - StartTime).TotalMilliseconds > m_ReceiveTimeout Then
                    Logger.LogMessage(p_Caller, FormatIDs(MyCallNumber) & "ReadByte timed out waitng for a byte to read, throwing TimeoutException")
                    Throw New TimeoutException("Serial port timed out waiting to read a byte")
                End If
                Thread.Sleep(1)
            End While
        End If
        RxByte = CByte(m_Port.ReadByte)
        If DebugTrace Then Logger.LogMessage(p_Caller, FormatIDs(MyCallNumber) & "ReadByte returning result - """ & RxByte.ToString & """")
        Return RxByte
    End Function

    Private Function ReadChar(ByVal p_Caller As String, ByVal TransactionID As Long) As Char
        Dim StartTime As Date, RxChar As Char, RxChars(10) As Char
        StartTime = Now
        If DebugTrace Then Logger.LogMessage(p_Caller, FormatIDs(TransactionID) & "Entered ReadChar: " & UseReadPolling)
        If UseReadPolling Then
            While (m_Port.BytesToRead = 0)
                If (Now - StartTime).TotalMilliseconds > m_ReceiveTimeout Then
                    Logger.LogMessage(p_Caller, FormatIDs(TransactionID) & "ReadByte timed out waitng for a character to read, throwing TimeoutException")
                    Throw New TimeoutException("Serial port timed out waiting to read a character")
                End If
                Thread.Sleep(1)
            End While
        End If
        RxChar = Chr(m_Port.ReadByte)
        If DebugTrace Then Logger.LogMessage(p_Caller, FormatIDs(TransactionID) & "ReadChar returning result - """ & RxChar & """")
        Return RxChar
    End Function

    Friend Function FormatIDs(ByVal TransactionID As Long) As String
        Static LastTransactionID As Long, TransactionIDString As String
        If DebugTrace Then
            If TransactionID <> 0 Then
                LastTransactionID = TransactionID
                TransactionIDString = TransactionID.ToString
            Else
                TransactionIDString = Left(Space(8), Len(LastTransactionID.ToString))
            End If

            Return TransactionIDString & " " & CurrentThread.ManagedThreadId & " "
        Else
            Return ""
        End If
    End Function

    Friend Class PortNameComparer
        Implements Collections.Generic.IComparer(Of String)
        'IComparer implementation that compares COM port names based on the numeric port number
        'rather than the whole port name string

        Friend Function Compare(ByVal x As String, ByVal y As String) As Integer Implements Collections.Generic.IComparer(Of String).Compare
            Dim xs, ys As String
            xs = x.ToString 'Make sure we are working with strings
            ys = y.ToString
            If (Len(xs) >= 4) And (Len(ys) >= 4) Then 'Ensure that we have at least enough characters for COMn format
                If (Left(xs, 3).ToUpperInvariant = "COM") And (Left(ys, 3).ToUpperInvariant = "COM") Then 'Test whether we have COMnnn format
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

    Const INFINITE As UInt32 = &HFFFFFFFFUI
    Const WAIT_ABANDONED As UInt32 = &H80UI
    Const WAIT_OBJECT_0 As UInt32 = &H0UI
    Const WAIT_TIMEOUT As UInt32 = &H102UI

    ''' <summary>
    ''' OS level blocking wait for an event 
    ''' </summary>
    ''' <param name="handle">The triggering even't handle</param>
    ''' <param name="milliseconds">Length of time to wait before timing out</param>
    ''' <returns>Status, 0 = success</returns>
    ''' <remarks></remarks>
    <DllImport("kernel32", SetLastError:=True)>
    Private Shared Function WaitForSingleObject(ByVal handle As IntPtr, ByVal milliseconds As UInt32) As UInt32
    End Function

    ''' <summary>
    ''' Sleep the calling thread until the worker thread has completed
    ''' </summary>
    ''' <param name="TData">ThreadData class holding required inputs, outputs and thread management information</param>
    ''' <remarks></remarks>
    Private Sub WaitForThread(ByVal TData As ThreadData)
        Dim RetCode As UInteger

        ' WARNING WARNING WARNING - Never put a logging statement here, before the sync command!

        ' It will release the primary thread in VB6 drivers, which results in the next serial command being scheduled before
        ' this one is completed and you get out of order serial command execution.

        ' It is vital that the primary thread is fully blocked in order for VB6 drivers to work as expected. This means that some .NET sync primitives
        ' such as ManualResetEvent CANNOT be used because they have been designed by MS to pump messages in the background while waiting
        ' and this results in behaviur described above!

        ' Execute the correct wait according to the set configuration
        Select Case TypeOfWait
            Case WaitType.ManualResetEvent
                If DebugTrace Then
                    Dim ts As New Stopwatch
                    ts.Start()
                    TData.ManualResetEvent.WaitOne(Timeout.Infinite)
                    LogMessage("WaitForThread", FormatIDs(TData.TransactionID) & "Completed ManualResetEvent OK, Command: " & TData.SerialCommand.ToString & " Elapsed: " & ts.Elapsed.TotalMilliseconds)
                Else
                    TData.ManualResetEvent.WaitOne(Timeout.Infinite)
                End If

            Case WaitType.Sleep
                If DebugTrace Then
                    Dim ts As New Stopwatch
                    ts.Start()
                    Do
                        Thread.Sleep(1)
                        LogMessage("WaitForThread", FormatIDs(TData.TransactionID) & "Command: " & TData.SerialCommand.ToString & " Elapsed: " & ts.Elapsed.TotalMilliseconds)
                    Loop Until TData.Completed
                    LogMessage("WaitForThread", FormatIDs(TData.TransactionID) & "Completed Sleep OK, Command: " & TData.SerialCommand.ToString & " Elapsed: " & ts.Elapsed.TotalMilliseconds)
                Else
                    Do
                        Thread.Sleep(1)
                    Loop Until TData.Completed
                End If

            Case WaitType.WaitForSingleObject
                If DebugTrace Then
                    Dim ts As New Stopwatch
                    ts.Start()
                    RetCode = WaitForSingleObject(TData.ManualResetEvent.SafeWaitHandle.DangerousGetHandle, INFINITE)
                    Select Case RetCode
                        Case WAIT_OBJECT_0
                            LogMessage("WaitForThread", FormatIDs(TData.TransactionID) & "Completed WaitForSingleObject OK, " & "Command: " & TData.SerialCommand.ToString & " Elapsed: " & ts.Elapsed.TotalMilliseconds)
                        Case WAIT_ABANDONED
                            LogMessage("***WaitForThread***", FormatIDs(TData.TransactionID) & "Completed WaitForSingleObject - ABANDONED, Return code: 0x" & RetCode.ToString("X8") & ", Command: " & TData.SerialCommand.ToString & " Elapsed: " & ts.Elapsed.TotalMilliseconds)
                        Case WAIT_TIMEOUT
                            LogMessage("***WaitForThread***", FormatIDs(TData.TransactionID) & "Completed WaitForSingleObject - TIMEOUT, Return code: 0x" & RetCode.ToString("X8") & ", Command: " & TData.SerialCommand.ToString & " Elapsed: " & ts.Elapsed.TotalMilliseconds)
                    End Select
                Else
                    RetCode = WaitForSingleObject(TData.ManualResetEvent.SafeWaitHandle.DangerousGetHandle, INFINITE)
                    Select Case RetCode
                        Case WAIT_OBJECT_0
                            ' No action in production mode
                        Case WAIT_ABANDONED
                            LogMessage("***WaitForThread***", FormatIDs(TData.TransactionID) & "Completed WaitForSingleObject - ABANDONED, Return code: 0x" & RetCode.ToString("X8") & ", Command: " & TData.SerialCommand.ToString)
                        Case WAIT_TIMEOUT
                            LogMessage("***WaitForThread***", FormatIDs(TData.TransactionID) & "Completed WaitForSingleObject - TIMEOUT, Return code: 0x" & RetCode.ToString("X8") & ", Command: " & TData.SerialCommand.ToString)
                    End Select
                End If

        End Select

        'Check whether we are propcessing out of order, which is a bad thing!
        If DebugTrace And (TData.TransactionID <> CallCount) Then LogMessage("***WaitForThread***", "Transactions out of order! TransactionID CurrentCallCount: " & TData.TransactionID & " " & CallCount)

    End Sub

    ''' <summary>
    ''' Worker thread data
    ''' </summary>
    ''' <remarks></remarks>
    Private Class ThreadData
        'Transmit Input values
        Public TransmitString As String
        Public TransmitBytes() As Byte

        'Receive Input values
        Public Terminator As String
        Public TerminatorBytes() As Byte
        Public Count As Integer

        'Output values
        Public LastException As Exception
        Public ResultByte As Byte
        Public ResultByteArray() As Byte
        Public ResultString As String
        Public ResultChar As Char

        'AvailableCOMPorts value
        Public AvailableCOMPorts As Generic.List(Of String)

        'Control values
        Public SerialCommand As SerialCommandType
        Public Completed As Boolean
        Public ManualResetEvent As ManualResetEvent
        Public TransactionID As Long

        'Management
        Public Connecting As Boolean
        Public TimeoutValueMs As Integer
        Public TimeoutValue As Integer

        ''' <summary>
        ''' Initialises a new ThreadData synchronisation object
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            'Initialise values to required state
            Me.Completed = False
            Me.LastException = Nothing
            Me.ManualResetEvent = New ManualResetEvent(False)
        End Sub

        ''' <summary>
        ''' Signals that this thread has completed its work
        ''' </summary>
        ''' <remarks></remarks>
        Sub ThreadCompleted()
            Try : Completed = True : Catch : End Try
            Try : ManualResetEvent.Set() : Catch : End Try
        End Sub

    End Class
#End Region

End Class