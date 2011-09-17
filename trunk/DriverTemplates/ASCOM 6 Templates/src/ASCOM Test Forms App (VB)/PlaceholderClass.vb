Namespace ASCOM.DeviceInterface

    ''' <summary>
    ''' Placeholder interface for the device class,
    ''' enough to allow the application to run without errors
    ''' </summary>
    Class TEMPLATEDEVICECLASS

        Sub New(ByVal p1 As String)
        End Sub

        Property Connected As Boolean

        Shared Function Choose(ByVal id As String) As String
            Return "template chooser"
        End Function

    End Class
End Namespace
