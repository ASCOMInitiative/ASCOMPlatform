' This file is a placeholder used during template development,
' It plays no part in the final ASCOM driver and can safely be deleted.
Imports ASCOM.DeviceInterface

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
    ReadOnly Property Name As String
    ReadOnly Property SupportedActions() As ArrayList
    Sub Dispose()

End Interface

' The following classes are just dummies to stop compilation errors in this template solution. They are not used in drivers or anywhere else
Partial Public Class DeviceTelescope
    Public Sub Dispose() Implements ITelescopeV3.Dispose
    End Sub

    Public Sub SetupDialog() Implements ITelescopeV3.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements ITelescopeV3.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ITelescopeV3.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ITelescopeV3.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements ITelescopeV3.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements ITelescopeV3.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements ITelescopeV3.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements ITelescopeV3.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements ITelescopeV3.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ITelescopeV3.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ITelescopeV3.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ITelescopeV3.Name
        Get
            Return ""
        End Get
    End Property

End Class

Partial Public Class DeviceCamera
    Public Sub Dispose() Implements ICameraV2.Dispose
    End Sub

    Public Sub SetupDialog() Implements ICameraV2.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements ICameraV2.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ICameraV2.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ICameraV2.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements ICameraV2.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements ICameraV2.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements ICameraV2.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements ICameraV2.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements ICameraV2.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ICameraV2.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ICameraV2.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ICameraV2.Name
        Get
            Return ""
        End Get
    End Property

End Class

Partial Public Class DeviceDome
    Public Sub Dispose() Implements IDomeV2.Dispose
    End Sub

    Public Sub SetupDialog() Implements IDomeV2.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements IDomeV2.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IDomeV2.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements IDomeV2.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements IDomeV2.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements IDomeV2.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements IDomeV2.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements IDomeV2.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements IDomeV2.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements IDomeV2.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements IDomeV2.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IDomeV2.Name
        Get
            Return ""
        End Get
    End Property

End Class

Partial Public Class DeviceFilerWheel
    Public Sub Dispose() Implements IFilterWheelV2.Dispose
    End Sub

    Public Sub SetupDialog() Implements IFilterWheelV2.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements IFilterWheelV2.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IFilterWheelV2.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements IFilterWheelV2.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements IFilterWheelV2.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements IFilterWheelV2.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements IFilterWheelV2.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements IFilterWheelV2.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements IFilterWheelV2.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements IFilterWheelV2.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements IFilterWheelV2.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IFilterWheelV2.Name
        Get
            Return ""
        End Get
    End Property

End Class

Partial Public Class DeviceFocuser
    Public Sub Dispose() Implements IFocuserV2.Dispose
    End Sub

    Public Sub SetupDialog() Implements IFocuserV2.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements IFocuserV2.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IFocuserV2.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements IFocuserV2.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements IFocuserV2.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements IFocuserV2.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements IFocuserV2.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements IFocuserV2.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements IFocuserV2.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements IFocuserV2.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements IFocuserV2.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IFocuserV2.Name
        Get
            Return ""
        End Get
    End Property

End Class

Partial Public Class DeviceRotator
    Public Sub Dispose() Implements IRotatorV2.Dispose
    End Sub

    Public Sub SetupDialog() Implements IRotatorV2.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements IRotatorV2.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IRotatorV2.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements IRotatorV2.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements IRotatorV2.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements IRotatorV2.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements IRotatorV2.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements IRotatorV2.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements IRotatorV2.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements IRotatorV2.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements IRotatorV2.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IRotatorV2.Name
        Get
            Return ""
        End Get
    End Property

End Class


Partial Public Class DeviceSwitch
    Public Sub Dispose() Implements ISwitchV2.Dispose
    End Sub

    Public Sub SetupDialog() Implements ISwitchV2.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements ISwitchV2.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ISwitchV2.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ISwitchV2.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements ISwitchV2.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements ISwitchV2.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements ISwitchV2.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements ISwitchV2.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements ISwitchV2.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ISwitchV2.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ISwitchV2.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ISwitchV2.Name
        Get
            Return ""
        End Get
    End Property

End Class

Partial Public Class DeviceSafetyMonitor
    Public Sub Dispose() Implements ISafetyMonitor.Dispose
    End Sub

    Public Sub SetupDialog() Implements ISafetyMonitor.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements ISafetyMonitor.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ISafetyMonitor.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ISafetyMonitor.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements ISafetyMonitor.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements ISafetyMonitor.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements ISafetyMonitor.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements ISafetyMonitor.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements ISafetyMonitor.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ISafetyMonitor.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ISafetyMonitor.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ISafetyMonitor.Name
        Get
            Return ""
        End Get
    End Property

End Class
