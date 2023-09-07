using System;
// Exceptions for the Utilities namesapce
using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Exceptions
{
    /// <summary>
    /// Base exception for the Utilities components
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    [Guid("A29FB43E-28C5-4ed0-8C8A-889DC7170A82")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    // Exception for Utilities component exceptions
    public class HelperException : Exception
    {

        /// <summary>
        /// Create a new exception with message
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <remarks></remarks>
        public HelperException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create a new exception with message and inner exception
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <param name="inner">Exception to be reported as the inner exception</param>
        /// <remarks></remarks>
        public HelperException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Serialise the exception
        /// </summary>
        /// <param name="info">Serialisation information</param>
        /// <param name="context">Serialisation context</param>
        /// <remarks></remarks>
        public HelperException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)

        {
        }

    }

    /// <summary>
    /// Exception thrown when the profile is not found. This is internally trapped and should not appear externally to an application.
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    [Guid("21AEDC6B-CC7F-4101-BC33-532DFEDEB7B5")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    // Exception for Utilities component exceptions
    public class ProfileNotFoundException : HelperException
    {

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <remarks></remarks>
        public ProfileNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <param name="inner">Exception to be reported as the inner exception</param>
        /// <remarks></remarks>
        public ProfileNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Serialise the exception
        /// </summary>
        /// <param name="info">Serialisation information</param>
        /// <param name="context">Serialisation context</param>
        /// <remarks></remarks>
        public ProfileNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)

        {
        }
    }

    /// <summary>
    /// Exception thrown when an invalid value is passed to a Utilities component
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    [Guid("A9C2CF73-C139-4fae-B47B-36F18C49B527")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    // Exception for Utilities component exceptions
    public class InvalidValueException : HelperException
    {

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <remarks></remarks>
        public InvalidValueException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <param name="inner">Exception to be reported as the inner exception</param>
        /// <remarks></remarks>
        public InvalidValueException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Serialise the exception
        /// </summary>
        /// <param name="info">Serialisation information</param>
        /// <param name="context">Serialisation context</param>
        /// <remarks></remarks>
        public InvalidValueException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)

        {
        }
    }

    /// <summary>
    /// Exception thrown when a serial port method is already in progress and a second attempt is made to use the serial port.
    /// </summary>
    /// <remarks>This exception is only thrown after 5 attempts, each with a 1 second timeout, have been made to 
    /// acquire the serial port. It may indicate that you have more than one thread attempting to access the serial 
    /// port and that you have not synchronised these within your application. The serial port can only handle 
    /// one transaction at a time e.g. Serial.Receive or Serial.Transmit etc.</remarks>
    [Serializable()]
    [Guid("7A3CFD64-D7E3-48b0-BEB6-5696CF7599B3")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    // Exception for Utilities component exceptions
    public class SerialPortInUseException : HelperException
    {

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <remarks></remarks>
        public SerialPortInUseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <param name="inner">Exception to be reported as the inner exception</param>
        /// <remarks></remarks>
        public SerialPortInUseException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Serialise the exception
        /// </summary>
        /// <param name="info">Serialisation information</param>
        /// <param name="context">Serialisation context</param>
        /// <remarks></remarks>
        public SerialPortInUseException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)

        {
        }
    }

    /// <summary>
    /// Exception thrown if there is any problem in reading or writing the profile from or to the file system
    /// </summary>
    /// <remarks>This is an ifrastructural exception indicatig that there is a problem at the file access layer
    /// in the profile code. Possible underlying reasons are security access permissions, file system full and hardware failure.
    /// </remarks>
    [Serializable()]
    [Guid("A38ABA4D-F872-4c2a-A19D-62DBBC761DD5")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    // Exception for Utilities component exceptions
    public class ProfilePersistenceException : HelperException
    {

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <remarks></remarks>
        public ProfilePersistenceException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <param name="inner">Exception to be reported as the inner exception</param>
        /// <remarks></remarks>
        public ProfilePersistenceException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Serialise the exception
        /// </summary>
        /// <param name="info">Serialisation information</param>
        /// <param name="context">Serialisation context</param>
        /// <remarks></remarks>
        public ProfilePersistenceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)

        {
        }
    }

    /// <summary>
    /// Exception thrown when a profile request is made for a driver that is not registered
    /// </summary>
    /// <remarks>Drivers must be registered before other profile commands are used to manipulate their 
    /// values.</remarks>
    [Serializable()]
    [Guid("0D2B7199-622D-4244-88C3-2577308F82E2")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    // Exception for Utilities component exceptions
    public class DriverNotRegisteredException : HelperException
    {

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <remarks></remarks>
        public DriverNotRegisteredException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <param name="inner">Exception to be reported as the inner exception</param>
        /// <remarks></remarks>
        public DriverNotRegisteredException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Serialise the exception
        /// </summary>
        /// <param name="info">Serialisation information</param>
        /// <param name="context">Serialisation context</param>
        /// <remarks></remarks>
        public DriverNotRegisteredException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)

        {
        }
    }

    /// <summary>
    /// Exception thrown when an attempt is made to write to a protected part of the the Profile store that is 
    /// reserved for Platform use. An example is attempting to write to the the default value of a device driver 
    /// profile. This value is reserved for use by the Chooser to display the device description and is set by the 
    /// Profile.Register method.
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    [Guid("00BC6F08-4277-47c3-9DBA-F80E02C5A448")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    // Exception for Utilities component exceptions
    public class RestrictedAccessException : HelperException
    {

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <remarks></remarks>
        public RestrictedAccessException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <param name="inner">Exception to be reported as the inner exception</param>
        /// <remarks></remarks>
        public RestrictedAccessException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Serialise the exception
        /// </summary>
        /// <param name="info">Serialisation information</param>
        /// <param name="context">Serialisation context</param>
        /// <remarks></remarks>
        public RestrictedAccessException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)

        {
        }
    }

    /// <summary>
    /// Exception thrown when an attempt is made to read a value that has not yet been set.
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    [Guid("C893C94C-3D48-4068-8BCE-6CED6AEF2512")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    // Exception for Utilities component exceptions
    public class ValueNotSetException : HelperException
    {

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <remarks></remarks>
        public ValueNotSetException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create a new exception with message 
        /// </summary>
        /// <param name="message">Message to be reported by the exception</param>
        /// <param name="inner">Exception to be reported as the inner exception</param>
        /// <remarks></remarks>
        public ValueNotSetException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Serialise the exception
        /// </summary>
        /// <param name="info">Serialisation information</param>
        /// <param name="context">Serialisation context</param>
        /// <remarks></remarks>
        public ValueNotSetException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)

        {
        }
    }

}