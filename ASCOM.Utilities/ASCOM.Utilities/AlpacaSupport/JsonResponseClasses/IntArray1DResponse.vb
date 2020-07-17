Namespace ASCOM.Utilities.Support
    Public Class IntArray1DResponse
        Inherits RestResponseBase

        Private intArray1D As Integer()

        Public Sub New()
        End Sub

        Public Sub New(ByVal clientTransactionID As UInteger, ByVal transactionID As UInteger, ByVal value As Integer())
            ServerTransactionID = transactionID
            MyBase.ClientTransactionID = clientTransactionID
            intArray1D = value
        End Sub

        Public Property Value As Integer()
            Get
                Return intArray1D
            End Get
            Set(ByVal value As Integer())
                intArray1D = value
            End Set
        End Property
    End Class
End Namespace
