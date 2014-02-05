Imports System.Runtime.InteropServices
Imports ASCOM.Utilities.Interfaces

''' <summary>
''' Class that returns a key and associated value
''' </summary>
''' <remarks>This class is used by some Profile properties and methods and
''' compensates for the inability of .NET to return Generic classes to COM clients.
''' <para>The properties and methods are: 
''' <see cref="Profile.RegisteredDevices">Profile.RegisteredDevices</see>, 
''' <see cref="Profile.SubKeys">Profile.SubKeys</see> and 
''' <see cref="Profile.Values">Profile.Values</see>.</para></remarks>
<ClassInterface(ClassInterfaceType.None), _
Guid("69CFE7E6-E64F-4045-8D0D-C61F50F31CAC"), _
ComVisible(True)> _
Public Class KeyValuePair
    Implements IKeyValuePair

    Private m_Key As String
    Private m_Value As String

#Region "New"
    ''' <summary>
    ''' COM visible default constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        m_Key = ""
        m_Value = ""
    End Sub
    ''' <summary>
    ''' Constructor that can set the key and value simultaneously.
    ''' </summary>
    ''' <param name="Key">The Key element of a key value pair</param>
    ''' <param name="Value">The Value element of a key value pair</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal Key As String, ByVal Value As String)
        m_Key = Key
        m_Value = Value
    End Sub
#End Region

#Region "IKeyValuePair Implementation"
    ''' <summary>
    ''' The Key element of a key value pair
    ''' </summary>
    ''' <value>Key</value>
    ''' <returns>Key as a string</returns>
    ''' <remarks></remarks>
    Public Property Key() As String Implements IKeyValuePair.Key
        Get
            Return m_Key
        End Get
        Set(ByVal value As String)
            m_Key = value
        End Set
    End Property

    ''' <summary>
    ''' The Value element of a key value pair.
    ''' </summary>
    ''' <value>Value</value>
    ''' <returns>Value as a string</returns>
    ''' <remarks></remarks>
    Public Property Value() As String Implements IKeyValuePair.Value
        Get
            Return m_Value
        End Get
        Set(ByVal value As String)
            m_Value = value
        End Set
    End Property
#End Region

End Class