' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports ASCOM
Imports ASCOM.Utilities

Class DeviceDome
    Implements IDomeV2
    Private m_util As New Util()
    Private TL As New TraceLogger()

#Region "IDome Implementation"

    Private domeShutterState As Boolean = False ' Variable to hold the open/closed status of the shutter, true = Open

	''' <summary>
	''' Immediately stops any and all movement of the dome.
	''' </summary>
	Public Sub AbortSlew() Implements IDomeV2.AbortSlew
		' This is a mandatory parameter but we have no action to take in this simple driver
		TL.LogMessage("AbortSlew", "Completed")
	End Sub

	''' <summary>
	''' The altitude (degrees, horizon zero and increasing positive to 90 zenith) of the part of the sky that the observer wishes to observe.
	''' </summary>
	Public ReadOnly Property Altitude() As Double Implements IDomeV2.Altitude
		Get
			TL.LogMessage("Altitude Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Altitude", False)
		End Get
	End Property

	''' <summary>
	''' <para><see langword="True" /> when the dome is in the home position. Raises an error if not supported.</para>
	''' <para>
	''' This is normally used following a <see cref="FindHome" /> operation. The value is reset
	''' with any azimuth slew operation that moves the dome away from the home position.
	''' </para>
	''' <para>
	''' <see cref="AtHome" /> may optionally also become True during normal slew operations, if the
	''' dome passes through the home position and the dome controller hardware is capable of
	''' detecting that; or at the end of a slew operation if the dome comes to rest at the home
	''' position.
	''' </para>
	''' </summary>
	Public ReadOnly Property AtHome() As Boolean Implements IDomeV2.AtHome
		Get
			TL.LogMessage("AtHome", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("AtHome", False)
		End Get
	End Property

	''' <summary>
	''' <see langword="True" /> if the dome is in the programmed park position.
	''' </summary>
	Public ReadOnly Property AtPark() As Boolean Implements IDomeV2.AtPark
		Get
			TL.LogMessage("AtPark", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("AtPark", False)
		End Get
	End Property

	''' <summary>
	''' The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West). North is true north and not magnetic north.
	''' </summary>
	Public ReadOnly Property Azimuth() As Double Implements IDomeV2.Azimuth
		Get
			TL.LogMessage("Azimuth", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Azimuth", False)
		End Get
	End Property

	''' <summary>
	''' <see langword="True" /> if the driver can perform a search for home position.
	''' </summary>
	Public ReadOnly Property CanFindHome() As Boolean Implements IDomeV2.CanFindHome
		Get
			TL.LogMessage("CanFindHome Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' <see langword="True" /> if the driver is capable of parking the dome.
	''' </summary>
	Public ReadOnly Property CanPark() As Boolean Implements IDomeV2.CanPark
		Get
			TL.LogMessage("CanPark Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' <see langword="True" /> if driver is capable of setting dome altitude.
	''' </summary>
	Public ReadOnly Property CanSetAltitude() As Boolean Implements IDomeV2.CanSetAltitude
		Get
			TL.LogMessage("CanSetAltitude Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' <see langword="True" /> if driver is capable of rotating the dome. Muste be <see "langword="False" /> for a 
	''' roll-off roof or clamshell.
	''' </summary>
	Public ReadOnly Property CanSetAzimuth() As Boolean Implements IDomeV2.CanSetAzimuth
		Get
			TL.LogMessage("CanSetAzimuth Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' <see langword="True" /> if the driver can set the dome park position.
	''' </summary>
	Public ReadOnly Property CanSetPark() As Boolean Implements IDomeV2.CanSetPark
		Get
			TL.LogMessage("CanSetPark Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' <see langword="True" /> if the driver is capable of opening and closing the shutter or roof
	''' mechanism.
	''' </summary>
	Public ReadOnly Property CanSetShutter() As Boolean Implements IDomeV2.CanSetShutter
		Get
			TL.LogMessage("CanSetShutter Get", True.ToString())
			Return True
		End Get
	End Property

	''' <summary>
	''' <see langword="true" /> if the dome hardware supports slaving to a telescope.
	''' </summary>
	Public ReadOnly Property CanSlave() As Boolean Implements IDomeV2.CanSlave
		Get
			TL.LogMessage("CanSlave Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' <see langword="true" /> if the driver is capable of synchronizing the dome azimuth position
	''' using the <see cref="SyncToAzimuth" /> method.
	''' </summary>
	Public ReadOnly Property CanSyncAzimuth() As Boolean Implements IDomeV2.CanSyncAzimuth
		Get
			TL.LogMessage("CanSyncAzimuth Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' Close the shutter or otherwise shield the telescope from the sky.
	''' </summary>
	Public Sub CloseShutter() Implements IDomeV2.CloseShutter
		TL.LogMessage("CloseShutter", "Shutter has been closed")
		domeShutterState = False
	End Sub

	''' <summary>
	''' Start operation to search for the dome home position.
	''' </summary>
	Public Sub FindHome() Implements IDomeV2.FindHome
		TL.LogMessage("FindHome", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("FindHome")
	End Sub

	''' <summary>
	''' Open shutter or otherwise expose telescope to the sky.
	''' </summary>
	Public Sub OpenShutter() Implements IDomeV2.OpenShutter
		TL.LogMessage("OpenShutter", "Shutter has been opened")
		domeShutterState = True
	End Sub

	''' <summary>
	''' Rotate dome in azimuth to park position.
	''' </summary>
	Public Sub Park() Implements IDomeV2.Park
		TL.LogMessage("Park", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("Park")
	End Sub

	''' <summary>
	''' Set the current azimuth position of dome to the park position.
	''' </summary>
	Public Sub SetPark() Implements IDomeV2.SetPark
		TL.LogMessage("SetPark", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SetPark")
	End Sub

	''' <summary>
	''' Gets the status of the dome shutter or roof structure.
	''' </summary>
	Public ReadOnly Property ShutterStatus() As ShutterState Implements IDomeV2.ShutterStatus
		Get
			TL.LogMessage("CanSyncAzimuth Get", False.ToString())
			If (domeShutterState) Then
				TL.LogMessage("ShutterStatus", ShutterState.shutterOpen.ToString())
				Return ShutterState.shutterOpen
			Else
				TL.LogMessage("ShutterStatus", ShutterState.shutterClosed.ToString())
				Return ShutterState.shutterClosed
			End If
		End Get
	End Property

	''' <summary>
	''' <see langword="True"/> if the dome is slaved to the telescope in its hardware, else <see langword="False"/>.
	''' </summary>
	Public Property Slaved() As Boolean Implements IDomeV2.Slaved
		Get
			TL.LogMessage("Slaved Get", False.ToString())
			Return False
		End Get
		Set(value As Boolean)
			TL.LogMessage("Slaved Set", "not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Slaved", True)
		End Set
	End Property

	''' <summary>
	''' Ensure that the requested viewing altitude is available for observing.
	''' </summary>
	''' <param name="Altitude">
	''' The desired viewing altitude (degrees, horizon zero and increasing positive to 90 degrees at the zenith)
	''' </param>
	Public Sub SlewToAltitude(Altitude As Double) Implements IDomeV2.SlewToAltitude
		TL.LogMessage("SlewToAltitude", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SlewToAltitude")
	End Sub

	''' <summary>
	''' Ensure that the requested viewing azimuth is available for observing.
	''' The method should not block and the slew operation should complete asynchronously.
	''' </summary>
	''' <param name="Azimuth">
	''' Desired viewing azimuth (degrees, North zero and increasing clockwise. i.e., 90 East,
	''' 180 South, 270 West)
	''' </param>
	Public Sub SlewToAzimuth(Azimuth As Double) Implements IDomeV2.SlewToAzimuth
		TL.LogMessage("SlewToAzimuth", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SlewToAzimuth")
	End Sub

	''' <summary>
	''' <see langword="True" /> if any part of the dome is currently moving or a move command has been issued, 
	''' but the dome has not yet started to move. <see langword="False" /> if all dome components are stationary
	''' and no move command has been issued. /> 
	''' </summary>
	Public ReadOnly Property Slewing() As Boolean Implements IDomeV2.Slewing
		Get
			TL.LogMessage("Slewing Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' Synchronize the current position of the dome to the given azimuth.
	''' </summary>
	''' <param name="Azimuth">
	''' Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East,
	''' 180 South, 270 West)
	''' </param>
	Public Sub SyncToAzimuth(Azimuth As Double) Implements IDomeV2.SyncToAzimuth
		TL.LogMessage("SyncToAzimuth", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SyncToAzimuth")
	End Sub

#End Region

	'//ENDOFINSERTEDFILE
End Class