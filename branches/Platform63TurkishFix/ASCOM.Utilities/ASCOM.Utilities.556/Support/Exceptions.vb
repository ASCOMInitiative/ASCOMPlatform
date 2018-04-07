'Exceptions for the Utilities namesapce

Namespace Exceptions
    ''' <summary>
    ''' Base exception for the Utilities components
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
        Public Class HelperException
        'Exception for Helper.NET component exceptions
        Inherits System.Exception

        ''' <summary>
        ''' Create a new exception with message
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Create a new exception with message and inner exception
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
    ''' Exception thrown when the profile is not found. This is internally trapped and should not appear externally to an application.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
        Public Class ProfileNotFoundException
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
    ''' Exception thrown when an invalid value is passed to a Utilities component
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
        Public Class InvalidValueException
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
    ''' Exception thrown when a serial port method is already in progress and a second attempt is made to use the serial port.
    ''' </summary>
    ''' <remarks>This exception is only thrown after 5 attempts, each with a 1 second timeout, have been made to 
    ''' acquire the serial port. It may indicate that you have more than one thread attempting to access the serial 
    ''' port and that you have not synchronised these within your application. The serial port can only handle 
    ''' one transaction at a time e.g. Serial.Receive or Serial.Transmit etc.</remarks>
    <Serializable()> _
        Public Class SerialPortInUseException
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
    ''' Exception thrown if there is any problem in reading or writing the profile from or to the file system
    ''' </summary>
    ''' <remarks>This is an ifrastructural exception indicatig that there is a problem at the file access layer
    ''' in the profile code. Possible underlying reasons are security access permissions, file system full and hardware failure.
    ''' </remarks>
    <Serializable()> _
        Public Class ProfilePersistenceException
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
    ''' Exception thrown when a profile request is made for a driver that is not registered
    ''' </summary>
    ''' <remarks>Drivers must be registered before other profile commands are used to manipulate their 
    ''' values.</remarks>
    <Serializable()> _
        Public Class DriverNotRegisteredException
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
    ''' Exception thrown when an attempt is made to write to a protected part of the the Profile store that is 
    ''' reserved for Platform use. An example is attempting to write to the the default value of a device driver 
    ''' profile. This value is reserved for use by the Chooser to display the device description and is set by the 
    ''' Profile.Register method.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
        Public Class RestrictedAccessException
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

End Namespace