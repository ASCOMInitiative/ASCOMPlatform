Namespace Conform

    Public Interface IConform
        ReadOnly Property ConformErrors() As ConformErrorNumbers
        ReadOnly Property ConformCommands() As ConformCommandStrings
        ReadOnly Property ConformCommandsRaw() As ConformCommandStrings
    End Interface

    Public Class ConformErrorNumbers
        Dim errNotImplemented(), errInvalidValue(), errValueNotSet() As Integer

        Sub New(ByVal NotImplemented() As Integer, ByVal InvalidValue() As Integer, ByVal ValueNotSet() As Integer)
            Me.NotImplemented = NotImplemented
            Me.InvalidValue = InvalidValue
            Me.ValueNotSet = ValueNotSet
        End Sub

        Property NotImplemented() As Integer()
            Get
                NotImplemented = errNotImplemented
            End Get
            Set(ByVal value As Integer())
                errNotImplemented = value
            End Set
        End Property

        Property InvalidValue() As Integer()
            Get
                InvalidValue = errInvalidValue
            End Get
            Set(ByVal value As Integer())
                errInvalidValue = value
            End Set
        End Property

        Property ValueNotSet() As Integer()
            Get
                ValueNotSet = errValueNotSet
            End Get
            Set(ByVal value As Integer())
                errValueNotSet = value
            End Set
        End Property
    End Class

    Public Class ConformCommandStrings
        Dim cmdBlind, cmdBool, cmdString As String
        Dim rtnString As String, rtnBool As Boolean

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

        Property CommandString() As String
            Get
                CommandString = cmdString
            End Get
            Set(ByVal value As String)
                cmdString = value
            End Set
        End Property

        Property ReturnString() As String
            Get
                ReturnString = rtnString
            End Get
            Set(ByVal value As String)
                rtnString = value
            End Set
        End Property

        Property CommandBlind() As String
            Get
                CommandBlind = cmdBlind
            End Get
            Set(ByVal value As String)
                cmdBlind = value
            End Set
        End Property

        Property CommandBool() As String
            Get
                CommandBool = cmdBool
            End Get
            Set(ByVal value As String)
                cmdBool = value
            End Set
        End Property

        Property ReturnBool() As Boolean
            Get
                ReturnBool = rtnBool
            End Get
            Set(ByVal value As Boolean)
                rtnBool = value
            End Set
        End Property
    End Class

End Namespace
