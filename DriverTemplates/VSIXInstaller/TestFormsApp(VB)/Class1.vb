Namespace Global.ASCOM.TEMPLATEDEVICENAME.My
    Public NotInheritable Class MySettings
        Public Shared Property [Default]() As MySettings
            Get
                Return New MySettings()
            End Get
            Set(value As MySettings)

            End Set
        End Property
        Public Shared Property DriverID As String
            Get
                Return ""
            End Get
            Set(value As String)

            End Set
        End Property
    End Class
End Namespace

Namespace Global.ASCOM.TEMPLATEDEVICENAME.My.Resources
    Public Class Resources
        Public Shared Property ASCOM As Drawing.Image
            Get
                Return Drawing.Image.FromFile("")
            End Get
            Set(value As Drawing.Image)

            End Set
        End Property
    End Class
End Namespace
