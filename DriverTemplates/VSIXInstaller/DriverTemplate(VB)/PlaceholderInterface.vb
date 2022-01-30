' This file is a placeholder used during template development. It holds the common method definitions for all the VB interfaces to remove compilation errors in the VB template
' that arise from including the "Implements" statement that is in each device type file.
' It plays no part in the final ASCOM driver.
Imports ASCOM
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
    Public Sub Dispose() Implements ICameraV3.Dispose
    End Sub

    Public Sub SetupDialog() Implements ICameraV3.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements ICameraV3.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ICameraV3.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ICameraV3.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements ICameraV3.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements ICameraV3.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements ICameraV3.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements ICameraV3.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements ICameraV3.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ICameraV3.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ICameraV3.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ICameraV3.Name
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
    Public Sub Dispose() Implements IFocuserV3.Dispose
    End Sub

    Public Sub SetupDialog() Implements IFocuserV3.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements IFocuserV3.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IFocuserV3.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements IFocuserV3.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements IFocuserV3.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements IFocuserV3.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements IFocuserV3.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements IFocuserV3.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements IFocuserV3.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements IFocuserV3.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements IFocuserV3.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IFocuserV3.Name
        Get
            Return ""
        End Get
    End Property

End Class

Partial Public Class DeviceRotator
    Public Sub Dispose() Implements IRotatorV3.Dispose
    End Sub

    Public Sub SetupDialog() Implements IRotatorV3.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements IRotatorV3.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IRotatorV3.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements IRotatorV3.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements IRotatorV3.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements IRotatorV3.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements IRotatorV3.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements IRotatorV3.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements IRotatorV3.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements IRotatorV3.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements IRotatorV3.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IRotatorV3.Name
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

Partial Public Class DeviceObservingConditions
    Public Sub Dispose() Implements IObservingConditions.Dispose
    End Sub

    Public Sub SetupDialog() Implements IObservingConditions.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements IObservingConditions.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IObservingConditions.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements IObservingConditions.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements IObservingConditions.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements IObservingConditions.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements IObservingConditions.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements IObservingConditions.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements IObservingConditions.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements IObservingConditions.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements IObservingConditions.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IObservingConditions.Name
        Get
            Return ""
        End Get
    End Property

End Class

Partial Public Class DeviceCoverCalibrator
    Public Sub Dispose() Implements ICoverCalibratorV1.Dispose
    End Sub

    Public Sub SetupDialog() Implements ICoverCalibratorV1.SetupDialog
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements ICoverCalibratorV1.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ICoverCalibratorV1.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ICoverCalibratorV1.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements ICoverCalibratorV1.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements ICoverCalibratorV1.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements ICoverCalibratorV1.Connected
        Get
            Return False
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Description As String Implements ICoverCalibratorV1.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements ICoverCalibratorV1.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ICoverCalibratorV1.DriverVersion
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ICoverCalibratorV1.InterfaceVersion
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ICoverCalibratorV1.Name
        Get
            Return ""
        End Get
    End Property

End Class