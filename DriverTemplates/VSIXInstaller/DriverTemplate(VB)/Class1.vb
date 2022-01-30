Namespace Global.ASCOM.TEMPLATEDEVICENAME.My
    Public Class MySettings
        Public Shared Property [Default]() As MySettings
            Get
                Return New MySettings()
            End Get
            Set(value As MySettings)

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
