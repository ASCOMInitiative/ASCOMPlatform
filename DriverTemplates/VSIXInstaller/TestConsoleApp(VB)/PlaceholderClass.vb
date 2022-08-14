' satisfies the template module so it will run without errors using the template substitution parameters.
' this file is removed by the template wizard.
Imports System.Text

Namespace ASCOM.DriverAccess

    Public Class TEMPLATEDEVICECLASS
        Dim connectedState As Boolean = False

        Sub New(ByVal id As String)
        End Sub

        ReadOnly Property Description As String
            Get
                Return "Template description"
            End Get
        End Property

        ReadOnly Property DriverInfo As String
            Get
                Return "Template driverinfo"
            End Get
        End Property

        ReadOnly Property DriverVersion As String
            Get
                Return "Template driver version"
            End Get
        End Property

        ReadOnly Property Name As String
            Get
                Return "Template name"
            End Get
        End Property

        Shared Function Choose(ByVal p1 As String) As String
            Return "Placeholder.device"
        End Function

        Property Connected As Boolean
            Get
                Return connectedState
            End Get
            Set
                connectedState = Value
            End Set
        End Property

        ReadOnly Property InterfaceVersion As Short
            Get
                Return 1
            End Get
        End Property

    End Class

End Namespace
