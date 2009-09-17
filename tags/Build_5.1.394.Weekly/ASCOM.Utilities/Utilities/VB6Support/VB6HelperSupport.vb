'The purpose of these classes is to match the standards used in the .Utilities components to the
'standards expected by the VB6 COM  Helper componentsthus allowing allowing the two worlds to be different.

Option Explicit On
Option Strict On

Imports System.Runtime.InteropServices
Imports System.Collections
Imports ASCOM.Utilities
Imports ASCOM.Utilities.Interfaces

Namespace VB6HelperSupport 'Tuck this out of the way of the main ASCOM.Utilities namespace

#Region "ProfileAccess"
    <ProgId("DriverHelper.ProfileAccess"), _
    ComVisible(True), _
    ComClass(ProfileAccess.ClassId, ProfileAccess.InterfaceId, ProfileAccess.EventsId), _
    System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
    Public Class ProfileAccess
        Implements IDisposable

#Region "COM GUIDs"
        ' These  GUIDs provide the COM identity for this class 
        ' and its COM interfaces. If you change them, existing 
        ' clients will no longer be able to access the class.
        Public Const ClassId As String = "f0acf8ea-ddeb-4869-ae33-b25d4d6195b6"
        Public Const InterfaceId As String = "e8b28e9a-cecf-436d-982e-0bf8c5216e4e"
        Public Const EventsId As String = "eab943e2-0b66-45ac-9120-c6387eef1a94"
#End Region

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

        Public Function GetProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal CName As String) As String
            'Get a single profile value
            Dim Ret As String
            Ret = Profile.GetProfile(p_SubKeyName, p_ValueName)
            TL.LogMessage("GetProfile", "SubKey: """ & p_SubKeyName & """ Value: """ & p_ValueName & """ Data: """ & Ret & """")
            Return Ret
        End Function

        Public Sub WriteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal p_ValueData As String, ByRef CName As String)
            'Write a single profile value
            TL.LogMessage("WriteProfile", "SubKey: """ & p_SubKeyName & """ Value: """ & p_ValueName & """ Data: """ & p_ValueData & """")
            If p_ValueData Is Nothing Then TL.LogMessage("WriteProfile", "WARNING - Supplied data value is Nothing, not empty string")
            Profile.WriteProfile(p_SubKeyName, p_ValueName, p_ValueData)
        End Sub

        Public Function EnumProfile(ByVal p_SubKeyName As String, ByVal CName As String) As ArrayList 'Scripting.Dictionary 'Hashtable
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

        Public Sub DeleteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal CName As String)
            'Delete a profile key
            TL.LogMessage("DeleteProfile", "SubKey: """ & p_SubKeyName & """ Value: """ & p_ValueName & """")

            Profile.DeleteProfile(p_SubKeyName, p_ValueName)
        End Sub

        Public Sub CreateKey(ByVal p_SubKeyName As String, ByVal CName As String)
            'Create a new profile key
            TL.LogMessage("CreateKey", "SubKey: """ & p_SubKeyName & """")

            Profile.CreateKey(p_SubKeyName)
        End Sub

        Public Function EnumKeys(ByVal p_SubKeyName As String, ByVal CName As String) As ArrayList 'Scripting.Dictionary 'Hashtable
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

        Public Sub DeleteKey(ByVal p_SubKeyName As String, ByVal CName As String)
            'Delete a key and all its contents
            TL.LogMessage("DeleteKey", "SubKey: """ & p_SubKeyName & """")

            Profile.DeleteKey(p_SubKeyName)
        End Sub
#End Region

    End Class

#Region "Interface IKeyValuePair"
    Public Interface IKeyValuePair
        Property Key() As String
        Property Value() As String
    End Interface
#End Region

    <ProgId("DriverHelper.KeyValuePair"), _
    ComVisible(True), _
    ComClass(KeyValuePair.ClassId, KeyValuePair.InterfaceId, KeyValuePair.EventsId), _
    System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
    Public Class KeyValuePair
        Implements IKeyValuePair
#Region "COM GUIDs"
        ' These  GUIDs provide the COM identity for this class 
        ' and its COM interfaces. If you change them, existing 
        ' clients will no longer be able to access the class.
        Public Const ClassId As String = "06F3E6FF-5355-4463-B31A-1FB03773AB71"
        Public Const InterfaceId As String = "DA4B972F-0C46-4ec1-8DE0-C769BFA72775"
        Public Const EventsId As String = "ED1CE85D-7A6B-4864-A6AA-416758EA7B9E"
#End Region

        Private m_Key As String
        Private m_Value As String

#Region "New"
        Public Sub New()
            m_Key = ""
            m_Value = ""
        End Sub
        Public Sub New(ByVal Key As String, ByVal Value As String)
            m_Key = Key
            m_Value = Value
        End Sub
#End Region

        Public Property Key() As String Implements IKeyValuePair.Key
            Get
                Return m_Key
            End Get
            Set(ByVal value As String)
                m_Key = value
            End Set
        End Property

        Public Property Value() As String Implements IKeyValuePair.Value
            Get
                Return m_Value
            End Get
            Set(ByVal value As String)
                m_Value = value
            End Set
        End Property
    End Class

#End Region

#Region "SerialSupport"
    <ProgId("DriverHelper.SerialSupport"), _
    ComVisible(True), _
    ComClass(SerialSupport.ClassId, SerialSupport.InterfaceId, SerialSupport.EventsId), _
    System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
    Public Class SerialSupport
        Implements IDisposable

#Region "COM GUIDs"
        ' These  GUIDs provide the COM identity for this class 
        ' and its COM interfaces. If you change them, existing 
        ' clients will no longer be able to access the class.
        Public Const ClassId As String = "114EBEC4-7887-4ab9-B750-98BB5F1C8A8F"
        Public Const InterfaceId As String = "FC9D9FFD-512A-4eaf-883B-38D564DF1948"
        Public Const EventsId As String = "B960ED6D-9FFD-4842-9813-E86673AAE9BB"
#End Region

        'Class to support the VB6 Helper serial component

        'This exposes a new .Utilities serial port so that it can be accessed through the VB6 helper component
        Private SerPort As ISerial

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

#Region "SerialAccess Implementation"

        Public Sub ClearBuffers()
            SerPort.ClearBuffers()
        End Sub

        Public Property Connected() As Boolean
            Get
                Return SerPort.Connected
            End Get
            Set(ByVal value As Boolean)
                SerPort.Connected = value
            End Set
        End Property

        Public Property Port() As Short
            Get
                Return CShort(SerPort.Port)
            End Get
            Set(ByVal value As Short)
                SerPort.Port = CInt(value)
            End Set
        End Property

        Public Property PortSpeed() As Integer
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

        Public Function Receive() As String
            Return SerPort.Receive
        End Function

        Public Function ReceiveByte() As Byte
            Return SerPort.ReceiveByte
        End Function

        Public Function ReceiveCounted(ByVal p_Count As Short) As String
            Return SerPort.ReceiveCounted(CInt(p_Count))
        End Function

        Public Function ReceiveCountedBinary(ByVal p_Count As Short) As Byte()
            Return SerPort.ReceiveCountedBinary(CInt(p_Count))
        End Function

        Public Function ReceiveTerminated(ByVal p_Terminator As String) As String
            Return SerPort.ReceiveTerminated(p_Terminator)
        End Function

        Public Function ReceiveTerminatedBinary(ByRef p_Terminator() As Byte) As Byte()
            Return SerPort.ReceiveTerminatedBinary(p_Terminator)
        End Function

        Public Property ReceiveTimeout() As Short
            Get
                Return CShort(SerPort.ReceiveTimeout)
            End Get
            Set(ByVal value As Short)
                SerPort.ReceiveTimeout = CInt(value)
            End Set
        End Property

        Public Property ReceiveTimeoutMs() As Integer
            Get
                Return SerPort.ReceiveTimeoutMs
            End Get
            Set(ByVal value As Integer)
                SerPort.ReceiveTimeoutMs = value
            End Set
        End Property

        Public Sub Transmit(ByVal p_Data As String)
            SerPort.Transmit(p_Data)
        End Sub

        Public Sub TransmitBinary(ByVal p_Data() As Byte)
            SerPort.TransmitBinary(p_Data)
        End Sub
#End Region

    End Class
#End Region

#Region "ChooserSupport"
    <ProgId("DriverHelper.ChooserSupport"), _
    ComVisible(True), _
    ComClass(ChooserSupport.ClassId, ChooserSupport.InterfaceId, ChooserSupport.EventsId), _
    System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
    Public Class ChooserSupport
        Implements IDisposable


#Region "COM GUIDs"
        ' These  GUIDs provide the COM identity for this class 
        ' and its COM interfaces. If you change them, existing 
        ' clients will no longer be able to access the class.
        Public Const ClassId As String = "9289B6A5-CAF1-4da1-8A36-999BEBCDD5E9"
        Public Const InterfaceId As String = "F8405951-6E1D-459d-842D-0CCC4634EC8C"
        Public Const EventsId As String = "40565D32-33C1-4ff9-9B88-973398120335"
#End Region

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
        Public Property DeviceType() As String
            Get
                Return myChooser.DeviceType
            End Get
            Set(ByVal Value As String)
                If Value = "" Then Err.Raise(SCODE_ILLEGAL_DEVTYPE, ERR_SOURCE_PROFILE, MSG_ILLEGAL_DEVTYPE)
                myChooser.DeviceType = Value
            End Set
        End Property

        Public Function [Choose](Optional ByVal CurrentDriverID As String = "") As String
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