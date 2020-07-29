using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM.Utilities.CS.Exceptions
{
    [Guid("A29FB43E-28C5-4ed0-8C8A-889DC7170A82")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Serializable]
    public class HelperException : Exception
    {
        public HelperException(string message) : base(message) { }
        public HelperException(string message, Exception inner) : base(message, inner) { }
        public HelperException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }

    [Guid("21AEDC6B-CC7F-4101-BC33-532DFEDEB7B5")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Serializable]
    public class ProfileNotFoundException : HelperException
    {
        public ProfileNotFoundException(string message) : base(message) { }
        public ProfileNotFoundException(string message, Exception inner) : base(message, inner) { }
        public ProfileNotFoundException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }

    [Guid("A9C2CF73-C139-4fae-B47B-36F18C49B527")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Serializable]
    public class InvalidValueException : HelperException
    {
        public InvalidValueException(string message) : base(message) { }
        public InvalidValueException(string message, Exception inner) : base(message, inner) { }
        public InvalidValueException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }

    [Guid("7A3CFD64-D7E3-48b0-BEB6-5696CF7599B3")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Serializable]
    public class SerialPortInUseException : HelperException
    {
        public SerialPortInUseException(string message) : base(message) { }
        public SerialPortInUseException(string message, Exception inner) : base(message, inner) { }
        public SerialPortInUseException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }

    [Guid("A38ABA4D-F872-4c2a-A19D-62DBBC761DD5")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Serializable]
    public class ProfilePersistenceException : HelperException
    {
        public ProfilePersistenceException(string message) : base(message) { }
        public ProfilePersistenceException(string message, Exception inner) : base(message, inner) { }
        public ProfilePersistenceException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }

    [Guid("0D2B7199-622D-4244-88C3-2577308F82E2")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Serializable]
    public class DriverNotRegisteredException : HelperException
    {
        public DriverNotRegisteredException(string message) : base(message) { }
        public DriverNotRegisteredException(string message, Exception inner) : base(message, inner) { }
        public DriverNotRegisteredException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }

    [Guid("00BC6F08-4277-47c3-9DBA-F80E02C5A448")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Serializable]
    public class RestrictedAccessException : HelperException
    {
        public RestrictedAccessException(string message) : base(message) { }
        public RestrictedAccessException(string message, Exception inner) : base(message, inner) { }
        public RestrictedAccessException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }

    [Guid("C893C94C-3D48-4068-8BCE-6CED6AEF2512")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Serializable]
    public class ValueNotSetException : HelperException
    {
        public ValueNotSetException(string message) : base(message) { }
        public ValueNotSetException(string message, Exception inner) : base(message, inner) { }
        public ValueNotSetException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }
}
