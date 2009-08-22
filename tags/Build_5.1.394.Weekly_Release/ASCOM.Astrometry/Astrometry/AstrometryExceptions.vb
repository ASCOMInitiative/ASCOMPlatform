Imports ASCOM.Utilities.Exceptions

Namespace Exceptions
    'ASCOM.Astrometry exceptions

    ''' <summary>
    ''' Exception thrown when an attempt is made to read from the transform component before it has had co-ordinates
    ''' set once by SetJ2000 or SetJNow.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
        Public Class TransformUninitialisedException
        'Exception for Helper.NET component exceptions
        Inherits HelperException

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <param name="inner">Exception to be reported as the inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

        ''' <summary>
        ''' Serialise the exception
        ''' </summary>
        ''' <param name="info">Serialisation information</param>
        ''' <param name="context">Serialisation context</param>
        ''' <remarks></remarks>
        Public Sub New( _
                    ByVal info As System.Runtime.Serialization.SerializationInfo, _
                    ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

    ''' <summary>
    ''' Exception thrown when an incompatible component is encountered that prevents Utilities from funcitoning
    ''' correctly.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
        Public Class CompatibilityException
        'Exception for Helper.NET component exceptions
        Inherits HelperException

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <param name="inner">Exception to be reported as the inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

        ''' <summary>
        ''' Serialise the exception
        ''' </summary>
        ''' <param name="info">Serialisation information</param>
        ''' <param name="context">Serialisation context</param>
        ''' <remarks></remarks>
        Public Sub New( _
                    ByVal info As System.Runtime.Serialization.SerializationInfo, _
                    ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

    ''' <summary>
    ''' Exception thrown when an attempt is made to read a value that has not yet been set.
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    <Serializable()> _
        Public Class ValueNotSetException
        'Exception for Helper.NET component exceptions
        Inherits HelperException

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <param name="inner">Exception to be reported as the inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

        ''' <summary>
        ''' Serialise the exception
        ''' </summary>
        ''' <param name="info">Serialisation information</param>
        ''' <param name="context">Serialisation context</param>
        ''' <remarks></remarks>
        Public Sub New( _
                    ByVal info As System.Runtime.Serialization.SerializationInfo, _
                    ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

    ''' <summary>
    ''' Exception thrown when an attempt is made to read a value that has not yet been calculated.
    ''' </summary>
    ''' <remarks>This probably occurs because another variable has not been set or a required method has not been called.</remarks>
    <Serializable()> _
        Public Class ValueNotAvailableException
        'Exception for Helper.NET component exceptions
        Inherits HelperException

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <param name="inner">Exception to be reported as the inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

        ''' <summary>
        ''' Serialise the exception
        ''' </summary>
        ''' <param name="info">Serialisation information</param>
        ''' <param name="context">Serialisation context</param>
        ''' <remarks></remarks>
        Public Sub New( _
                    ByVal info As System.Runtime.Serialization.SerializationInfo, _
                    ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

    ''' <summary>
    ''' Exception thrown when a NOVAS function returns a non-zero, error completion code.
    ''' </summary>
    ''' <remarks>This probably occurs because another variable has not been set or a required method has not been called.</remarks>
    <Serializable()> _
        Public Class NOVASFunctionException
        'Exception for Helper.NET component exceptions
        Inherits HelperException

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal FuncName As String, ByVal ErrCode As Short)
            MyBase.New(message & " Error returned from function " & FuncName & " - error code: " & ErrCode.ToString)
        End Sub

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <param name="inner">Exception to be reported as the inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

        ''' <summary>
        ''' Serialise the exception
        ''' </summary>
        ''' <param name="info">Serialisation information</param>
        ''' <param name="context">Serialisation context</param>
        ''' <remarks></remarks>
        Public Sub New( _
                    ByVal info As System.Runtime.Serialization.SerializationInfo, _
                    ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

End Namespace