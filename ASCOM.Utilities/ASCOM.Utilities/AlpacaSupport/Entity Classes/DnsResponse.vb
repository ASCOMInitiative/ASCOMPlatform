Imports System.Net
Imports System.Threading

''' <summary>
''' Define the state object for the callback. 
''' Use hostName to correlate calls with the proper result.
''' </summary>
Friend Class DnsResponse
    Private f_IpHostEntry As IPHostEntry

    Public Sub New()
        CallComplete = New ManualResetEvent(False)
    End Sub

    Public Property IpHostEntry As IPHostEntry
        Get
            Return f_IpHostEntry
        End Get
        Set(ByVal value As IPHostEntry)
            ' Save the value and populate the other DnsResponse fields
            f_IpHostEntry = value
            HostName = value.HostName
            Aliases = value.Aliases
            AddressList = value.AddressList
        End Set
    End Property

    Public Property CallComplete As ManualResetEvent
    Public Property HostName As String
    Public Property Aliases As String()
    Public Property AddressList As IPAddress()
End Class
