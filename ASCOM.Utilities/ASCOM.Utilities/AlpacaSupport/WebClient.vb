Imports System
Imports System.Net

Friend Class WebClientWithTimeOut
    Inherits Net.WebClient

    Public Property Timeout As Integer

    Protected Overrides Function GetWebRequest(ByVal uri As Uri) As WebRequest
        Dim webRequest As WebRequest = MyBase.GetWebRequest(uri)
        webRequest.Timeout = Timeout
        CType(webRequest, HttpWebRequest).ReadWriteTimeout = Timeout
        Return webRequest
    End Function
End Class
