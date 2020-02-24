Imports System.Collections.Generic

Friend Class AlpacaConfiguredDevicesResponse
    Inherits RestResponseBase

    Public Sub New()
        Value = New ConfiguredDevice() {}
    End Sub

    Public Sub New(ByVal clientTransactionID As UInteger, ByVal transactionID As UInteger, ByVal value As ConfiguredDevice())
        ServerTransactionID = transactionID
        MyBase.ClientTransactionID = clientTransactionID
        Me.Value = value
    End Sub

    Public Property Value As ConfiguredDevice()
End Class