'Exceptions for the Utilities namesapce
Imports System.Runtime.InteropServices

Namespace Exceptions
    ''' <summary>
    ''' Base exception for the Utilities components
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable(), _
    Guid("A29FB43E-28C5-4ed0-8C8A-889DC7170A82"), _
    ComVisible(True), _
    ClassInterface(ClassInterfaceType.None)> _
    Public Class HelperException
        'Exception for Utilities component exceptions
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
    <Serializable(), _
        Guid("21AEDC6B-CC7F-4101-BC33-532DFEDEB7B5"), _
        ComVisible(True), _
        ClassInterface(ClassInterfaceType.None)> _
        Public Class ProfileNotFoundException
        'Exception for Utilities component exceptions
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
    <Serializable(), _
       Guid("A9C2CF73-C139-4fae-B47B-36F18C49B527"), _
       ComVisible(True), _
       ClassInterface(ClassInterfaceType.None)> _
           Public Class InvalidValueException
        'Exception for Utilities component exceptions
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
    <Serializable(), _
       Guid("7A3CFD64-D7E3-48b0-BEB6-5696CF7599B3"), _
       ComVisible(True), _
       ClassInterface(ClassInterfaceType.None)> _
           Public Class SerialPortInUseException
        'Exception for Utilities component exceptions
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
    <Serializable(), _
        Guid("A38ABA4D-F872-4c2a-A19D-62DBBC761DD5"), _
        ComVisible(True), _
        ClassInterface(ClassInterfaceType.None)> _
        Public Class ProfilePersistenceException
        'Exception for Utilities component exceptions
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
    <Serializable(), _
        Guid("0D2B7199-622D-4244-88C3-2577308F82E2"), _
        ComVisible(True), _
        ClassInterface(ClassInterfaceType.None)> _
        Public Class DriverNotRegisteredException
        'Exception for Utilities component exceptions
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
    <Serializable(), _
        Guid("00BC6F08-4277-47c3-9DBA-F80E02C5A448"), _
        ComVisible(True), _
        ClassInterface(ClassInterfaceType.None)> _
        Public Class RestrictedAccessException
        'Exception for Utilities component exceptions
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
    <Serializable(), _
        Guid("C893C94C-3D48-4068-8BCE-6CED6AEF2512"), _
        ComVisible(True), _
        ClassInterface(ClassInterfaceType.None)> _
        Public Class ValueNotSetException
        'Exception for Utilities component exceptions
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