' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM
Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities

Class DeviceSwitch
    Implements ISwitchV2

    Private TL As New TraceLogger()

#Region "ISwitchV2 Implementation"

    Dim numSwitches As Short = 0

	''' <summary>
	''' The number of switches managed by this driver
	''' </summary>
	''' <returns>The number of devices managed by this driver.</returns>
	Public ReadOnly Property MaxSwitch As Short Implements ISwitchV2.MaxSwitch
		Get
			TL.LogMessage("MaxSwitch Get", numSwitches.ToString())
			Return numSwitches
		End Get
	End Property

	''' <summary>
	''' Return the name of switch device n.
	''' </summary>
	''' <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
	''' <returns>The name of the device</returns>
	Public Function GetSwitchName(id As Short) As String Implements ISwitchV2.GetSwitchName
		Validate("GetSwitchName", id)
		TL.LogMessage("GetSwitchName", "Not Implemented")
		Throw New MethodNotImplementedException("GetSwitchName")
	End Function

	''' <summary>
	''' Set a switch device name to a specified value.
	''' </summary>
	''' <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
	''' <param name="name">The name of the device</param>
	Sub SetSwitchName(id As Short, name As String) Implements ISwitchV2.SetSwitchName
		Validate("SetSwitchName", id)
		TL.LogMessage("SetSwitchName", "Not Implemented")
		Throw New MethodNotImplementedException("SetSwitchName")
	End Sub

	''' <summary>
	''' Gets the description of the specified switch device. This is to allow a fuller description of
	''' the device to be returned, for example for a tool tip.
	''' </summary>
	''' <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
	''' <returns>
	''' String giving the device description.
	''' </returns>
	Public Function GetSwitchDescription(id As Short) As String Implements ISwitchV2.GetSwitchDescription
		Validate("GetSwitchDescription", id)
		TL.LogMessage("GetSwitchDescription", "Not Implemented")
		Throw New MethodNotImplementedException("GetSwitchDescription")
	End Function

	''' <summary>
	''' Reports if the specified switch device can be written to, default true.
	''' This is false if the device cannot be written to, for example a limit switch or a sensor.
	''' </summary>
	''' <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
	''' <returns>
	''' <c>true</c> if the device can be written to, otherwise <c>false</c>.
	''' </returns>
	Public Function CanWrite(id As Short) As Boolean Implements ISwitchV2.CanWrite
		Validate("CanWrite", id)
		TL.LogMessage("CanWrite", "Default true")
		Return True
	End Function

#Region "boolean members"
	''' <summary>
	''' Return the state of switch device id as a boolean
	''' </summary>
	''' <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
	''' <returns>True or false</returns>
	Function GetSwitch(id As Short) As Boolean Implements ISwitchV2.GetSwitch
		Validate("GetSwitch", id, True)
		TL.LogMessage("GetSwitch", "Not Implemented")
		Throw New ASCOM.MethodNotImplementedException("GetSwitch")
	End Function

	''' <summary>
	''' Sets a switch controller device to the specified state, true or false.
	''' </summary>
	''' <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
	''' <param name="state">The required control state</param>
	Sub SetSwitch(id As Short, state As Boolean) Implements ISwitchV2.SetSwitch
		Validate("SetSwitch", id, True)
		TL.LogMessage("SetSwitch", "Not Implemented")
		Throw New ASCOM.MethodNotImplementedException("SetSwitch")
	End Sub

#End Region

#Region "Analogue members"
	''' <summary>
	''' Returns the maximum value for this switch device, this must be greater than <see cref="MinSwitchValue"/>.
	''' </summary>
	''' <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
	''' <returns>The maximum value to which this device can be set or which a read only sensor will return.</returns>
	Function MaxSwitchValue(id As Short) As Double Implements ISwitchV2.MaxSwitchValue
		Validate("MaxSwitchValue", id)
		TL.LogMessage("MaxSwitchValue", "Not Implemented")
		Throw New MethodNotImplementedException("MaxSwitchValue")
	End Function

	''' <summary>
	''' Returns the minimum value for this switch device, this must be less than <see cref="MaxSwitchValue"/>
	''' </summary>
	''' <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
	''' <returns>The minimum value to which this device can be set or which a read only sensor will return.</returns>
	Function MinSwitchValue(id As Short) As Double Implements ISwitchV2.MinSwitchValue
		Validate("MinSwitchValue", id)
		TL.LogMessage("MinSwitchValue", "Not Implemented")
		Throw New MethodNotImplementedException("MinSwitchValue")
	End Function

	''' <summary>
	''' Returns the step size that this device supports (the difference between successive values of the device).
	''' </summary>
	''' <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
	''' <returns>The step size for this device.</returns>
	Function SwitchStep(id As Short) As Double Implements ISwitchV2.SwitchStep
		Validate("SwitchStep", id)
		TL.LogMessage("SwitchStep", "Not Implemented")
		Throw New MethodNotImplementedException("SwitchStep")
	End Function

	''' <summary>
	''' Returns the value for switch device id as a double
	''' </summary>
	''' <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
	''' <returns>The value for this switch, this is expected to be between <see cref="MinSwitchValue"/> and
	''' <see cref="MaxSwitchValue"/>.</returns>
	Function GetSwitchValue(id As Short) As Double Implements ISwitchV2.GetSwitchValue
		Validate("GetSwitchValue", id, False)
		TL.LogMessage("GetSwitchValue", "Not Implemented")
		Throw New MethodNotImplementedException("GetSwitchValue")
	End Function

	''' <summary>
	''' Set the value for this device as a double.
	''' </summary>
	''' <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
	''' <param name="value">The value to be set, between <see cref="MinSwitchValue"/> and <see cref="MaxSwitchValue"/></param>
	Sub SetSwitchValue(id As Short, value As Double) Implements ISwitchV2.SetSwitchValue
		Validate("SetSwitchValue", id, value)
		If value < MinSwitchValue(id) Or value > MaxSwitchValue(id) Then
			Throw New InvalidValueException("", value.ToString(), String.Format("{0} to {1}", MinSwitchValue(id), MaxSwitchValue(id)))
		End If
		TL.LogMessage("SetSwitchValue", "Not Implemented")
		Throw New MethodNotImplementedException("SetSwitchValue")
	End Sub

#End Region

#End Region

#Region "Private methods"

	''' <summary>
	''' Checks that the switch id is in range and throws an InvalidValueException if it isn't
	''' </summary>
	''' <param name="message">The message.</param>
	''' <param name="id">The id.</param>
	Private Sub Validate(message As String, id As Short)
        If (id < 0 Or id >= numSwitches) Then
            Throw New InvalidValueException(message, id.ToString(), String.Format("0 to {0}", numSwitches - 1))
        End If
    End Sub

    ''' <summary>
    ''' Checks that the number of states for the switch is correct and throws a methodNotImplemented exception if not.
    ''' Boolean switches must have 2 states and multi-value switches more than 2.
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="id"></param>
    ''' <param name="expectBoolean"></param>
    Private Sub Validate(message As String, id As Short, expectBoolean As Boolean)
        Validate(message, id)
        Dim ns As Integer = (((MaxSwitchValue(id) - MinSwitchValue(id)) / SwitchStep(id)) + 1)
        If (expectBoolean And ns <> 2) Or (Not expectBoolean And ns <= 2) Then
            TL.LogMessage(message, String.Format("Switch {0} has the wrong number of states", id, ns))
            Throw New MethodNotImplementedException(String.Format("{0}({1})", message, id))
        End If
    End Sub

    ''' <summary>
    ''' Checks that the switch id and value are in range and throws an
    ''' InvalidValueException if they are not.
    ''' </summary>
    ''' <param name="message">The message.</param>
    ''' <param name="id">The id.</param>
    ''' <param name="value">The value.</param>
    Private Sub Validate(message As String, id As Short, value As Double)
        Validate(message, id, False)
        Dim min = MinSwitchValue(id)
        Dim max = MaxSwitchValue(id)
        If (value < min Or value > max) Then
            TL.LogMessage(message, String.Format("Value {1} for Switch {0} is out of the allowed range {2} to {3}", id, value, min, max))
            Throw New InvalidValueException(message, value.ToString(), String.Format("Switch({0}) range {1} to {2}", id, min, max))
        End If
    End Sub
#End Region

    '//ENDOFINSERTEDFILE
End Class