Imports System

Friend MustInherit Class RestResponseBase
    Private exception As Exception
    Public Property ClientTransactionID As UInteger
    Public Property ServerTransactionID As UInteger
    Public Property ErrorNumber As Integer = 0
    Public Property ErrorMessage As String = ""

    Public Property DriverException As Exception
        Get
            Return exception
        End Get
        Set(ByVal value As Exception)
            exception = value

            If exception IsNot Nothing Then
                ' Set the error number and message fields from the exception
                'ErrorNumber = exception.HResult;
                ErrorMessage = exception.Message


                ' Convert ASCOM exception error numbers (0x80040400 - 0x80040FFF) to equivalent Alpaca error numbers (0x400 to 0xFFF) so that they will be interpreted correctly by native Alpaca clients
                If ErrorNumber >= Constants.ASCOM_ERROR_NUMBER_BASE AndAlso ErrorNumber <= Constants.ASCOM_ERROR_NUMBER_MAX Then
                    ErrorNumber -= Constants.ASCOM_ERROR_NUMBER_OFFSET
                End If
            End If
        End Set
    End Property


    ''' <summary>
    ''' Method used by NewtonSoft JSON to determine whether the DriverException field should be included in the serialise JSON response.
    ''' </summary>
    ''' <returns></returns>
    Public Function ShouldSerializeDriverException() As Boolean
        Return SerializeDriverException
    End Function


    ''' <summary>
    ''' Control variable that determines whether the DriverException field will be included in serialised JSON responses
    ''' </summary>
    Friend Property SerializeDriverException As Boolean = True
End Class
