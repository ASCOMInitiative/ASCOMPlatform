' This file is a placeholder used during template development,
' It plays no part in the final ASCOM driver and can safely be deleted.

Partial Public Class TEMPLATEDEVICECLASS
    Friend Shared TEMPLATEINTERFACEVERSION As Short = 0
End Class

Interface ITEMPLATEDEVICEINTERFACE
    Sub SetupDialog()

    Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String

    Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False)

    Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean

    Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String

    Property Connected() As Boolean

    ReadOnly Property DriverVersion() As String
    ReadOnly Property Description() As String
    ReadOnly Property DriverInfo() As String

    ReadOnly Property InterfaceVersion() As Short

    ReadOnly Property SupportedActions() As ArrayList

End Interface