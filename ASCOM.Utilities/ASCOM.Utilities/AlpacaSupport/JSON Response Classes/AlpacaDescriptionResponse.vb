Public Class AlpacaDescriptionResponse
    Inherits RestResponseBase

    Public Sub New()
        Value = New AlpacaDeviceDescription()
    End Sub

    Public Sub New(ByVal clientTransactionID As UInteger, ByVal transactionID As UInteger, ByVal value As AlpacaDeviceDescription)
        ServerTransactionID = transactionID
        MyBase.ClientTransactionID = clientTransactionID
        Me.Value = value
    End Sub

    Public Property Value As AlpacaDeviceDescription
End Class
