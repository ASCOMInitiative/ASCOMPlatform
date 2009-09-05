'The purpose of these classes is to match the standards used in the .Utilities components to the
'standards expected by the VB6 COM  Helper componentsthus allowing allowing the two worlds to be different.

Option Explicit On
Option Strict On

Imports System.Runtime.InteropServices
Imports System.Collections
Imports ASCOM.Utilities
Imports ASCOM.Utilities.Interfaces

Namespace VB6HelperSupport 'Tuck this out of the way of the main ASCOM.Utilities namespace

#Region "VB6HelperInterfaces"
    <Guid("87D14110-BEB7-43ff-991E-AAA11C44E5AF"), _
    System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), _
    ComVisible(True)> _
    Public Interface IProfileAccess
        <DispId(1)> Function GetProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal CName As String) As String
        <DispId(2)> Sub WriteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal p_ValueData As String, ByRef CName As String)
        <DispId(3)> Function EnumProfile(ByVal p_SubKeyName As String, ByVal CName As String) As ArrayList 'Scripting.Dictionary 'Hashtable
        <DispId(4)> Sub DeleteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal CName As String)
        <DispId(5)> Sub CreateKey(ByVal p_SubKeyName As String, ByVal CName As String)
        <DispId(6)> Function EnumKeys(ByVal p_SubKeyName As String, ByVal CName As String) As ArrayList 'Scripting.Dictionary 'Hashtable
        <DispId(7)> Sub DeleteKey(ByVal p_SubKeyName As String, ByVal CName As String)
    End Interface

    <Guid("ABE720E6-9C2C-47e9-8476-6CE5A3F994E2"), _
    System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), _
    ComVisible(True)> _
    Public Interface ISerialSupport
        <DispId(1)> Sub ClearBuffers()
        <DispId(2)> Property Connected() As Boolean
        <DispId(3)> Property Port() As Short
        <DispId(4)> Property PortSpeed() As Integer
        <DispId(5)> Function Receive() As String
        <DispId(6)> Function ReceiveByte() As Byte
        <DispId(7)> Function ReceiveCounted(ByVal p_Count As Short) As String
        <DispId(8)> Function ReceiveCountedBinary(ByVal p_Count As Short) As Byte()
        <DispId(9)> Function ReceiveTerminated(ByVal p_Terminator As String) As String
        <DispId(10)> Function ReceiveTerminatedBinary(ByRef p_Terminator() As Byte) As Byte()
        <DispId(11)> Property ReceiveTimeout() As Short
        <DispId(12)> Property ReceiveTimeoutMs() As Integer
        <DispId(13)> Sub Transmit(ByVal p_Data As String)
        <DispId(14)> Sub TransmitBinary(ByVal p_Data() As Byte)
    End Interface

    <Guid("5E3A9439-A1A4-4d8d-8658-53E2470C69F6"), _
    System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), _
    ComVisible(True)> _
    Public Interface IChooserSupport
        <DispId(1)> Property DeviceType() As String
        <DispId(2)> Function [Choose](Optional ByVal CurrentDriverID As String = "") As String
    End Interface
#End Region

#Region "ProfileAccess"
    <ProgId("DriverHelper.ProfileAccess"), _
    ComVisible(True), _
    Guid("f0acf8ea-ddeb-4869-ae33-b25d4d6195b6"), _
    ClassInterface(ClassInterfaceType.None), _
    System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
    Public Class ProfileAccess
        Implements IProfileAccess, IDisposable

        'Class to support the VB6 Helper components

        'This exposes the new profile access mechanic, implemented through XMLAccess, as a COM component
        'that can be accessed by the VB6 helpers instead of using their built in registry access tools to
        'get profile values from the registry

        'The function and sub signatures below exactly match those provided by the registry toolkit 
        'originally used by the helpers.

        Private Profile As IAccess
        Private TL As TraceLogger

#Region "New and IDisposable Support"
        Public Sub New()
            'Create a new instance and instanciate the XMLAccess object to do all the hard work
            'MyBase.New()
            'Try
            'Profile = New ASCOM.Utilities.XMLAccess("Unspecified Component")
            'Catch ex As Exception
            ' MsgBox("HelperProfile " & ex.ToString)
            ' End Try
            Me.New("Unspecified Component")
        End Sub

        Public Sub New(ByVal ComponentName As String)
            ' As New() excpet that it allows the calling component to identify itself, this name is used in error messages
            MyBase.New()
            Try
                Profile = New XMLAccess(ComponentName)
                TL = New TraceLogger("", "VB6ProfileSupport")
                TL.Enabled = True
                Call RunningVersions(TL)
            Catch ex As Exception
                MsgBox("HelperProfile " & ex.ToString)
            End Try
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    Try
                        Profile.Dispose()
                        Profile = Nothing
                    Catch ex As Exception
                        MsgBox("VB6HelperSupport ProfileAccess Exception " & ex.ToString)
                    End Try
                    If Not TL Is Nothing Then
                        TL.Enabled = False
                        TL.Dispose()
                        TL = Nothing
                    End If
                End If

            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
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

#End Region

#Region "ProfileAccess Implementation"

        Public Function GetProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal CName As String) As String Implements IProfileAccess.GetProfile
            'Get a single profile value
            Dim Ret As String
            Ret = Profile.GetProfile(p_SubKeyName, p_ValueName)
            TL.LogMessage("GetProfile", "SubKey: """ & p_SubKeyName & """ Value: """ & p_ValueName & """ Data: """ & Ret & """")
            Return Ret
        End Function

        Public Sub WriteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal p_ValueData As String, ByRef CName As String) Implements IProfileAccess.WriteProfile
            'Write a single profile value
            TL.LogMessage("WriteProfile", "SubKey: """ & p_SubKeyName & """ Value: """ & p_ValueName & """ Data: """ & p_ValueData & """")
            If p_ValueData Is Nothing Then TL.LogMessage("WriteProfile", "WARNING - Supplied data value is Nothing, not empty string")
            Profile.WriteProfile(p_SubKeyName, p_ValueName, p_ValueData)
        End Sub

        Public Function EnumProfile(ByVal p_SubKeyName As String, ByVal CName As String) As ArrayList Implements IProfileAccess.EnumProfile
            'Enumerate values within a given profile key
            'Return these as a Scripting.Dictionary object

            Dim RetVal As ArrayList = New ArrayList
            Dim ReturnedProfile As Generic.SortedList(Of String, String)
            ReturnedProfile = Profile.EnumProfile(p_SubKeyName) 'Get the requested values as a hashtable
            TL.LogMessage("EnumProfile", "SubKey: """ & p_SubKeyName & """ found " & ReturnedProfile.Count & " values")
            For Each de As Generic.KeyValuePair(Of String, String) In ReturnedProfile 'Copy the hashtable entries to the scripting.dictionary
                Dim kvp As New KeyValuePair(de.Key, de.Value)
                RetVal.Add(kvp)
                TL.LogMessage("  EnumProfile", "  Key: """ & de.Key & """ Value: """ & de.Value & """")
            Next
            Return RetVal
        End Function

        Public Sub DeleteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal CName As String) Implements IProfileAccess.DeleteProfile
            'Delete a profile key
            TL.LogMessage("DeleteProfile", "SubKey: """ & p_SubKeyName & """ Value: """ & p_ValueName & """")

            Profile.DeleteProfile(p_SubKeyName, p_ValueName)
        End Sub

        Public Sub CreateKey(ByVal p_SubKeyName As String, ByVal CName As String) Implements IProfileAccess.CreateKey
            'Create a new profile key
            TL.LogMessage("CreateKey", "SubKey: """ & p_SubKeyName & """")

            Profile.CreateKey(p_SubKeyName)
        End Sub

        Public Function EnumKeys(ByVal p_SubKeyName As String, ByVal CName As String) As ArrayList Implements IProfileAccess.EnumKeys
            'Enuerate the subkeys in a specified key
            'Return these as a Scripting.Dictionary object

            Dim RetVal As ArrayList = New ArrayList
            Dim Keys As Generic.SortedList(Of String, String)
            Keys = Profile.EnumKeys(p_SubKeyName) 'Get the list of subkeys
            TL.LogMessage("EnumKeys", "SubKey: """ & p_SubKeyName & """ found " & Keys.Count & " values")

            For Each de As Generic.KeyValuePair(Of String, String) In Keys 'Copy into the scripting.dictionary
                Dim kvp As New KeyValuePair(de.Key, de.Value)
                RetVal.Add(kvp)
                TL.LogMessage("  EnumKeys", "  Key: """ & de.Key & """ Value: """ & de.Value & """")
            Next
            Return RetVal
        End Function

        Public Sub DeleteKey(ByVal p_SubKeyName As String, ByVal CName As String) Implements IProfileAccess.DeleteKey
            'Delete a key and all its contents
            TL.LogMessage("DeleteKey", "SubKey: """ & p_SubKeyName & """")

            Profile.DeleteKey(p_SubKeyName)
        End Sub
#End Region

    End Class


#End Region

#Region "SerialSupport"
    <ProgId("DriverHelper.SerialSupport"), _
    ComVisible(True), _
    Guid("114EBEC4-7887-4ab9-B750-98BB5F1C8A8F"), _
    ClassInterface(ClassInterfaceType.None), _
    System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
    Public Class SerialSupport
        Implements ISerialSupport, IDisposable

        'Class to support the VB6 Helper serial component

        'This exposes a new .Utilities serial port so that it can be accessed through the VB6 helper component
        Private SerPort As Serial

#Region "New and IDisposable Support"
        Public Sub New()
            'Create a new instance and instanciate the XMLAccess object to do all the hard work
            MyBase.New()
            Try
                SerPort = New Serial
            Catch ex As Exception
                MsgBox("SERIALSUPPORT: " & ex.ToString)
            End Try
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                Try : SerPort.Connected = False : Catch : End Try
                Try : SerPort.Dispose() : Catch : End Try
                SerPort = Nothing
                MyBase.Finalize()
            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
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

#End Region

#Region "SerialSupport Implementation"

        Public Sub ClearBuffers() Implements ISerialSupport.ClearBuffers
            SerPort.ClearBuffers()
        End Sub

        Public Property Connected() As Boolean Implements ISerialSupport.Connected
            Get
                Return SerPort.Connected
            End Get
            Set(ByVal value As Boolean)
                SerPort.Connected = value
            End Set
        End Property

        Public Property Port() As Short Implements ISerialSupport.Port
            Get
                Return CShort(SerPort.Port)
            End Get
            Set(ByVal value As Short)
                SerPort.Port = CInt(value)
            End Set
        End Property

        Public Property PortSpeed() As Integer Implements ISerialSupport.PortSpeed
            Get
                Select Case SerPort.Speed
                    Case Utilities.SerialSpeed.ps300 : Return 300
                    Case Utilities.SerialSpeed.ps1200 : Return 1200
                    Case Utilities.SerialSpeed.ps2400 : Return 2400
                    Case Utilities.SerialSpeed.ps4800 : Return 4800
                    Case Utilities.SerialSpeed.ps9600 : Return 9600
                    Case Utilities.SerialSpeed.ps14400 : Return 14400
                    Case Utilities.SerialSpeed.ps19200 : Return 19200
                    Case Utilities.SerialSpeed.ps28800 : Return 28800
                    Case Utilities.SerialSpeed.ps38400 : Return 38400
                    Case Utilities.SerialSpeed.ps57600 : Return 57600
                    Case Utilities.SerialSpeed.ps115200 : Return 115200
                End Select
            End Get
            Set(ByVal value As Integer)
                Select Case value
                    Case 300 : SerPort.Speed = Utilities.SerialSpeed.ps300
                    Case 1200 : SerPort.Speed = Utilities.SerialSpeed.ps1200
                    Case 2400 : SerPort.Speed = Utilities.SerialSpeed.ps2400
                    Case 4800 : SerPort.Speed = Utilities.SerialSpeed.ps4800
                    Case 9600 : SerPort.Speed = Utilities.SerialSpeed.ps9600
                    Case 14400 : SerPort.Speed = Utilities.SerialSpeed.ps14400
                    Case 19200 : SerPort.Speed = Utilities.SerialSpeed.ps19200
                    Case 28800 : SerPort.Speed = Utilities.SerialSpeed.ps28800
                    Case 34000 : SerPort.Speed = Utilities.SerialSpeed.ps38400
                    Case 57600 : SerPort.Speed = Utilities.SerialSpeed.ps57600
                    Case 115200 : SerPort.Speed = Utilities.SerialSpeed.ps115200
                End Select
            End Set
        End Property

        Public Function Receive() As String Implements ISerialSupport.Receive
            Return SerPort.Receive
        End Function

        Public Function ReceiveByte() As Byte Implements ISerialSupport.ReceiveByte
            Return SerPort.ReceiveByte
        End Function

        Public Function ReceiveCounted(ByVal p_Count As Short) As String Implements ISerialSupport.ReceiveCounted
            Return SerPort.ReceiveCounted(CInt(p_Count))
        End Function

        Public Function ReceiveCountedBinary(ByVal p_Count As Short) As Byte() Implements ISerialSupport.ReceiveCountedBinary
            Return SerPort.ReceiveCountedBinary(CInt(p_Count))
        End Function

        Public Function ReceiveTerminated(ByVal p_Terminator As String) As String Implements ISerialSupport.ReceiveTerminated
            Return SerPort.ReceiveTerminated(p_Terminator)
        End Function

        Public Function ReceiveTerminatedBinary(ByRef p_Terminator() As Byte) As Byte() Implements ISerialSupport.ReceiveTerminatedBinary
            Return SerPort.ReceiveTerminatedBinary(p_Terminator)
        End Function

        Public Property ReceiveTimeout() As Short Implements ISerialSupport.ReceiveTimeout
            Get
                Return CShort(SerPort.ReceiveTimeout)
            End Get
            Set(ByVal value As Short)
                SerPort.ReceiveTimeout = CInt(value)
            End Set
        End Property

        Public Property ReceiveTimeoutMs() As Integer Implements ISerialSupport.ReceiveTimeoutMs
            Get
                Return SerPort.ReceiveTimeoutMs
            End Get
            Set(ByVal value As Integer)
                SerPort.ReceiveTimeoutMs = value
            End Set
        End Property

        Public Sub Transmit(ByVal p_Data As String) Implements ISerialSupport.Transmit
            SerPort.Transmit(p_Data)
        End Sub

        Public Sub TransmitBinary(ByVal p_Data() As Byte) Implements ISerialSupport.TransmitBinary
            SerPort.TransmitBinary(p_Data)
        End Sub
#End Region

    End Class
#End Region

#Region "ChooserSupport"
    <ProgId("DriverHelper.ChooserSupport"), _
    ComVisible(True), _
    Guid("9289B6A5-CAF1-4da1-8A36-999BEBCDD5E9"), _
    ClassInterface(ClassInterfaceType.None), _
    System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
    Public Class ChooserSupport
        Implements IChooserSupport, IDisposable

        'Class to support the VB6 Helper chooser component

        'This exposes a new .Utilities chooser so that it can be accessed through the VB6 helper component
        Private myChooser As Utilities.Chooser
        Private disposedValue As Boolean = False        ' To detect redundant calls

#Region "New, IDisposable and Finalize"
        Public Sub New()
            'Create a new instance and instanciate the XMLAccess object to do all the hard work
            MyBase.New()
            Try
                'MsgBox("VB6Helper Before New Utilities.Chooser")
                myChooser = New Utilities.Chooser
                'MsgBox("VB6Helper After New Utilities.Chooser")
            Catch ex As Exception
                MsgBox("CHOOSERSUPPORT: " & ex.ToString)
            End Try
        End Sub

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                End If

                If Not myChooser Is Nothing Then
                    myChooser.Dispose()
                    myChooser = Nothing
                End If
            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
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
#End Region

#Region "ChooserSupport Implementation"
        Public Property DeviceType() As String Implements IChooserSupport.DeviceType
            Get
                Return myChooser.DeviceType
            End Get
            Set(ByVal Value As String)
                If Value = "" Then Err.Raise(SCODE_ILLEGAL_DEVTYPE, ERR_SOURCE_PROFILE, MSG_ILLEGAL_DEVTYPE)
                myChooser.DeviceType = Value
            End Set
        End Property

        Public Function [Choose](Optional ByVal CurrentDriverID As String = "") As String Implements IChooserSupport.Choose
            Try
                Return myChooser.Choose(CurrentDriverID)
            Catch ex As Exception
                MsgBox("VB6HELPERSUPPORT.CHOOSE Exception " & ex.ToString)
                Return "EXCEPTION VALUE"
            End Try
        End Function
#End Region

    End Class
#End Region

End Namespace