Imports System.Runtime.InteropServices
''' <summary>
''' Driver interface to inform Conform of valid driver commands and returned error codes.
''' </summary>
''' <remarks></remarks>
<Guid("903D0C2C-CE3D-4e27-BD50-3B2C76B7EEE1"), _
ComVisible(True)> _
Public Interface IConform
    ''' <summary>
    ''' Error numbers returned for "Not Implemented", "Invalid Value" and "Not Set" error states.
    ''' </summary>
    ''' <value>Expected driver error numbers</value>
    ''' <returns>Expected driver error numbers</returns>
    ''' <remarks></remarks>
    <DispId(1)> ReadOnly Property ConformErrors() As ConformErrorNumbers
    ''' <summary>
    ''' Commands to be sent with Raw parameter false
    ''' </summary>
    ''' <value>Set of commands to be sent</value>
    ''' <returns>Set of commands to be sent</returns>
    ''' <remarks></remarks>
    <DispId(2)> ReadOnly Property ConformCommands() As ConformCommandStrings
    ''' <summary>
    ''' Commands to be sent with Raw parameter true
    ''' </summary>
    ''' <value>Set of commands to be sent</value>
    ''' <returns>Set of commands to be sent</returns>
    ''' <remarks></remarks>
    <DispId(3)> ReadOnly Property ConformCommandsRaw() As ConformCommandStrings
End Interface

''' <summary>
''' Interface for Conform error numbers class
''' </summary>
''' <remarks></remarks>
<Guid("676E4D8C-2CD1-4e63-B55A-B0A1C338C6CE"), _
ComVisible(True)> _
Public Interface IConformErrorNumbers
    ''' <summary>
    ''' Error number the driver will return for a not implemented error
    ''' </summary>
    ''' <value>NotImplemented error number(s)</value>
    ''' <returns>Array of integer error numbers</returns>
    ''' <remarks>If you only use one error number, set it as element 0 of the array.</remarks>
    <DispId(1)> Property NotImplemented() As Integer()
    ''' <summary>
    ''' Error number the driver will return for an invalid value error
    ''' </summary>
    ''' <value>InvalidValue error number(s)</value>
    ''' <returns>Array of integer error numbers</returns>
    ''' <remarks>If you only use one error number, set it as element 0 of the array.</remarks>
    <DispId(2)> Property InvalidValue() As Integer()
    ''' <summary>
    ''' Error number the driver will return for a value not set error
    ''' </summary>
    ''' <value>NotSetError error number(s)</value>
    ''' <returns>Array of integer error number(s)</returns>
    ''' <remarks>If you only use one error number, set it as element 0 of the array.</remarks>
    <DispId(3)> Property ValueNotSet() As Integer()
End Interface

''' <summary>
''' Interface for Conform CommandStrings class
''' </summary>
''' <remarks></remarks>
<Guid("4876295B-6FCC-47c8-AEC9-CF39F1244AEB"), _
InterfaceType(ComInterfaceType.InterfaceIsIDispatch), _
ComVisible(True)> _
Public Interface IConformCommandStrings
    ''' <summary>
    ''' Command to be sent for CommandString test
    ''' </summary>
    ''' <value>String command to be sent</value>
    ''' <returns>String command that will be sent</returns>
    ''' <remarks></remarks>
    <DispId(1)> Property CommandString() As String
    ''' <summary>
    ''' Expected respons to CommandString command
    ''' </summary>
    ''' <value>String response expected</value>
    ''' <returns>String response expected</returns>
    ''' <remarks></remarks>
    <DispId(2)> Property ReturnString() As String
    ''' <summary>
    ''' Command to be sent for CommandBlind test
    ''' </summary>
    ''' <value>String command to be sent</value>
    ''' <returns>String command that will be sent</returns>
    ''' <remarks></remarks>
    <DispId(3)> Property CommandBlind() As String
    ''' <summary>
    ''' Command to be sent for CommandBlind test
    ''' </summary>
    ''' <value>String command to be sent</value>
    ''' <returns>String command that will be sent</returns>
    ''' <remarks></remarks>
    <DispId(4)> Property CommandBool() As String
    ''' <summary>
    ''' Command to be sent for CommandBool test
    ''' </summary>
    ''' <value>String command to be sent</value>
    ''' <returns>String command that will be sent</returns>
    ''' <remarks></remarks>
    <DispId(5)> Property ReturnBool() As Boolean
End Interface

''' <summary>
''' Contains error numbers that the driver will return when expected invalid conditions occur. 
''' </summary>
''' <remarks></remarks>
<Guid("A8CFEEE1-F27B-4a68-AF05-5F6CCE3F1257"), _
ComVisible(True), _
ClassInterface(ClassInterfaceType.None)> _
Public Class ConformErrorNumbers
    Implements IConformErrorNumbers
    Private errNotImplemented(), errInvalidValue(), errValueNotSet() As Integer

    ''' <summary>
    ''' Creates a new instance of the error numbers class.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New() 'COM visible setup

    End Sub

    ''' <summary>
    ''' Set not implemented, invalid value and value not set error codes in one call
    ''' </summary>
    ''' <param name="NotImplemented">Error number for not implemented exceptions</param>
    ''' <param name="InvalidValue1">Error number for first invalid value exception</param>
    ''' <param name="InvalidValue2">Error number for second invalid value exception</param>
    ''' <param name="ValueNotSet">Error number for value not set exception</param>
    ''' <remarks>Use this call if you have one "not implemented", up to two "invalid value" and one
    ''' "value not set" error codes. If you have more than this number of distinct error numbers,
    ''' use the other nethods in the class to set them.</remarks>
    Sub New(ByVal NotImplemented As Integer, ByVal InvalidValue1 As Integer, ByVal InvalidValue2 As Integer, ByVal ValueNotSet As Integer)
        ReDim errNotImplemented(0), errInvalidValue(1), errValueNotSet(0)
        errNotImplemented(0) = NotImplemented
        errInvalidValue(0) = InvalidValue1
        errInvalidValue(1) = InvalidValue2
        errValueNotSet(0) = ValueNotSet
    End Sub

    ''' <summary>
    ''' Set not implemented, invalid value and value not set error codes in one call
    ''' </summary>
    ''' <param name="NotImplemented">Array of "not implemented" error numbers</param>
    ''' <param name="InvalidValue">Array of "invalid value" error numbers</param>
    ''' <param name="ValueNotSet">Array of "value not set" error numbers</param>
    ''' <remarks>Use this call to set any number of error codes in each category.</remarks>
    Sub New(ByVal NotImplemented() As Integer, ByVal InvalidValue() As Integer, ByVal ValueNotSet() As Integer)
        Me.NotImplemented = NotImplemented
        Me.InvalidValue = InvalidValue
        Me.ValueNotSet = ValueNotSet
    End Sub

    ''' <summary>
    ''' Error code(s) returned for "Not Implemented" errors.
    ''' </summary>
    ''' <value>"Not implemented" error codes</value>
    ''' <returns>Array of integer error numbers</returns>
    ''' <remarks></remarks>
    Property NotImplemented() As Integer() Implements IConformErrorNumbers.NotImplemented
        Get
            NotImplemented = errNotImplemented
        End Get
        Set(ByVal value As Integer())
            errNotImplemented = value
        End Set
    End Property

    ''' <summary>
    ''' Error code(s) returned for "Invalid Value" errors.
    ''' </summary>
    ''' <value>"Invalid value" error codes</value>
    ''' <returns>Array of integer error numbers</returns>
    ''' <remarks></remarks>
    Property InvalidValue() As Integer() Implements IConformErrorNumbers.InvalidValue
        Get
            InvalidValue = errInvalidValue
        End Get
        Set(ByVal value As Integer())
            errInvalidValue = value
        End Set
    End Property

    ''' <summary>
    ''' Error code(s) returned for "Value Not Set" errors.
    ''' </summary>
    ''' <value>"Value not set" error codes</value>
    ''' <returns>Array of integer error numbers</returns>
    ''' <remarks></remarks>
    Property ValueNotSet() As Integer() Implements IConformErrorNumbers.ValueNotSet
        Get
            ValueNotSet = errValueNotSet
        End Get
        Set(ByVal value As Integer())
            errValueNotSet = value
        End Set
    End Property
End Class

''' <summary>
''' The device specific commands and expected responses to be used by Conform when testing the 
''' CommandXXX commands.
''' </summary>
''' <remarks></remarks>
<Guid("F6774C71-BA75-4400-B8DB-20960D373170"), _
ComVisible(True), _
ClassInterface(ClassInterfaceType.None)> _
Public Class ConformCommandStrings
    Implements IConformCommandStrings
    Private cmdBlind, cmdBool, cmdString As String
    Private rtnString As String, rtnBool As Boolean

    ''' <summary>
    ''' Set all Conform CommandXXX commands and expected responses in one call
    ''' </summary>
    ''' <param name="CommandString">String to be sent through CommandString method</param>
    ''' <param name="ReturnString">Expected return value from CommandString command</param>
    ''' <param name="CommandBlind">String to be sent through CommandBling method</param>
    ''' <param name="CommandBool">Expected return value from CommandBlind command</param>
    ''' <param name="ReturnBool">Expected boolean response from CommandBool command</param>
    ''' <remarks>To suppress a Command XXX test, set the command and return values to Nothing (VB) 
    ''' or null (C#). To accept any response to a command just set the return value to Nothing or null
    ''' as appropriate.</remarks>
    Sub New(ByVal CommandString As String, ByVal ReturnString As String, ByVal CommandBlind As String, ByVal CommandBool As String, ByVal ReturnBool As Boolean)
        Me.CommandString = CommandString
        Me.CommandBlind = CommandBlind
        Me.CommandBool = CommandBool
        Me.ReturnString = ReturnString
        Me.ReturnBool = ReturnBool
    End Sub

    ''' <summary>
    ''' Create a new CommandStrings object with all fields set to Nothing (VB) / null (C#), use 
    ''' other properties to set commands and expected return values.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        Me.CommandString = Nothing
        Me.CommandBlind = Nothing
        Me.CommandBool = Nothing
        Me.ReturnString = Nothing
        Me.ReturnBool = False
    End Sub

    ''' <summary>
    ''' Set the command to be sent by the Conform CommandString test
    ''' </summary>
    ''' <value>Device command</value>
    ''' <returns>String device command</returns>
    ''' <remarks></remarks>
    Property CommandString() As String Implements IConformCommandStrings.CommandString
        Get
            CommandString = cmdString
        End Get
        Set(ByVal value As String)
            cmdString = value
        End Set
    End Property

    ''' <summary>
    ''' Set the expected return value from the CommandString command
    ''' </summary>
    ''' <value>Device response</value>
    ''' <returns>String device response</returns>
    ''' <remarks></remarks>
    Property ReturnString() As String Implements IConformCommandStrings.ReturnString
        Get
            ReturnString = rtnString
        End Get
        Set(ByVal value As String)
            rtnString = value
        End Set
    End Property

    ''' <summary>
    ''' Set the command to be sent by the Conform CommandBlind test
    ''' </summary>
    ''' <value>Device command</value>
    ''' <returns>String device command</returns>
    ''' <remarks></remarks>
    Property CommandBlind() As String Implements IConformCommandStrings.CommandBlind
        Get
            CommandBlind = cmdBlind
        End Get
        Set(ByVal value As String)
            cmdBlind = value
        End Set
    End Property

    ''' <summary>
    ''' Set the command to be sent by the Conform CommandBool test
    ''' </summary>
    ''' <value>Device command</value>
    ''' <returns>String device command</returns>
    ''' <remarks></remarks>
    Property CommandBool() As String Implements IConformCommandStrings.CommandBool
        Get
            CommandBool = cmdBool
        End Get
        Set(ByVal value As String)
            cmdBool = value
        End Set
    End Property

    ''' <summary>
    ''' Set the expected return value from the CommandBool command
    ''' </summary>
    ''' <value>Device response</value>
    ''' <returns>Boolean device response</returns>
    ''' <remarks></remarks>
    Property ReturnBool() As Boolean Implements IConformCommandStrings.ReturnBool
        Get
            ReturnBool = rtnBool
        End Get
        Set(ByVal value As Boolean)
            rtnBool = value
        End Set
    End Property
End Class