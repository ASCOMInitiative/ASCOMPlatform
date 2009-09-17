Imports System.Runtime.InteropServices
Namespace Conform

    Public Interface IConform
        ReadOnly Property ConformErrors() As ConformErrorNumbers
        ReadOnly Property ConformCommands() As ConformCommandStrings
        ReadOnly Property ConformCommandsRaw() As ConformCommandStrings
    End Interface

    Public Interface IConformErrorNumbers
        Property NotImplemented() As Integer()
        Property InvalidValue() As Integer()
        Property ValueNotSet() As Integer()
    End Interface

    Public Interface IConformCommandStrings
        Property CommandString() As String
        Property ReturnString() As String
        Property CommandBlind() As String
        Property CommandBool() As String
        Property ReturnBool() As Boolean
    End Interface

    Public Class ConformErrorNumbers
        Implements IConformErrorNumbers
        Private errNotImplemented, errInvalidValue, errValueNotSet As Integer()

        Sub New() 'COM visible setup

        End Sub

        Sub New(ByVal NotImplemented As Integer, ByVal InvalidValue1 As Integer, ByVal InvalidValue2 As Integer, ByVal ValueNotSet As Integer)
            Me.NotImplemented(0) = NotImplemented
            Me.InvalidValue(0) = InvalidValue1
            Me.InvalidValue(1) = InvalidValue2
            Me.ValueNotSet(0) = ValueNotSet
        End Sub


        Sub New(ByVal NotImplemented() As Integer, ByVal InvalidValue() As Integer, ByVal ValueNotSet() As Integer)
            Me.NotImplemented = NotImplemented
            Me.InvalidValue = InvalidValue
            Me.ValueNotSet = ValueNotSet
        End Sub

        Property NotImplemented() As Integer() Implements IConformErrorNumbers.NotImplemented
            Get
                NotImplemented = errNotImplemented
            End Get
            Set(ByVal value As Integer())
                errNotImplemented = value
            End Set
        End Property

        Property InvalidValue() As Integer() Implements IConformErrorNumbers.InvalidValue
            Get
                InvalidValue = errInvalidValue
            End Get
            Set(ByVal value As Integer())
                errInvalidValue = value
            End Set
        End Property

        Property ValueNotSet() As Integer() Implements IConformErrorNumbers.ValueNotSet
            Get
                ValueNotSet = errValueNotSet
            End Get
            Set(ByVal value As Integer())
                errValueNotSet = value
            End Set
        End Property
    End Class

    Public Class ConformCommandStrings
        Implements IConformCommandStrings
        Private cmdBlind, cmdBool, cmdString As String
        Private rtnString As String, rtnBool As Boolean

        Sub New(ByVal CommandString As String, ByVal ReturnString As String, ByVal CommandBlind As String, ByVal CommandBool As String, ByVal ReturnBool As Boolean)
            Me.CommandString = CommandString
            Me.CommandBlind = CommandBlind
            Me.CommandBool = CommandBool
            Me.ReturnString = ReturnString
            Me.ReturnBool = ReturnBool
        End Sub

        Sub New()
            Me.CommandString = Nothing
            Me.CommandBlind = Nothing
            Me.CommandBool = Nothing
            Me.ReturnString = Nothing
            Me.ReturnBool = False
        End Sub

        Property CommandString() As String Implements IConformCommandStrings.CommandString
            Get
                CommandString = cmdString
            End Get
            Set(ByVal value As String)
                cmdString = value
            End Set
        End Property

        Property ReturnString() As String Implements IConformCommandStrings.ReturnString
            Get
                ReturnString = rtnString
            End Get
            Set(ByVal value As String)
                rtnString = value
            End Set
        End Property

        Property CommandBlind() As String Implements IConformCommandStrings.CommandBlind
            Get
                CommandBlind = cmdBlind
            End Get
            Set(ByVal value As String)
                cmdBlind = value
            End Set
        End Property

        Property CommandBool() As String Implements IConformCommandStrings.CommandBool
            Get
                CommandBool = cmdBool
            End Get
            Set(ByVal value As String)
                cmdBool = value
            End Set
        End Property

        Property ReturnBool() As Boolean Implements IConformCommandStrings.ReturnBool
            Get
                ReturnBool = rtnBool
            End Get
            Set(ByVal value As Boolean)
                rtnBool = value
            End Set
        End Property
    End Class

End Namespace
