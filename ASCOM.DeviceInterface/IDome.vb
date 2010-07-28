Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the IDome Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Defines the IDome Interface
''' </summary>
<Guid("88CFA00C-DDD3-4b42-A1F0-9387E6823832"), ComVisible(True)> _
Public Interface IDome 'CCDA0D85-474A-4775-8105-1D513ADC3896
    Inherits IAscomDriver
    Inherits IDeviceControl
    ''' <summary>
    ''' Immediately cancel current dome operation.
    ''' Calling this method will immediately disable hardware slewing (Dome.Slaved will become False).
    ''' Raises an error if a communications failure occurs, or if the command is known to have failed. 
    ''' </summary>
    Sub AbortSlew()

    ''' <summary>
    ''' The dome altitude (degrees, horizon zero and increasing positive to 90 zenith).
    ''' Raises an error only if no altitude control. If actual dome altitude can not be read,
    ''' then reports back the last slew position. 
    ''' </summary>
    ReadOnly Property Altitude() As Double

    ''' <summary>
    ''' True if the dome is in the Home position.
    ''' Set only following a Dome.FindHome operation and reset with any azimuth slew operation.
    ''' Raises an error if not supported. 
    ''' </summary>
    ReadOnly Property AtHome() As Boolean

    ''' <summary>
    ''' True if the dome is in the programmed park position.
    ''' Set only following a Dome.Park operation and reset with any slew operation.
    ''' Raises an error if not supported. 
    ''' </summary>
    ReadOnly Property AtPark() As Boolean

    ''' <summary>
    ''' The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West)
    ''' </summary>
    ReadOnly Property Azimuth() As Double

    ''' <summary>
    ''' True if driver can do a search for home position.
    ''' </summary>
    ReadOnly Property CanFindHome() As Boolean

    ''' <summary>
    ''' True if driver is capable of setting dome altitude.
    ''' </summary>
    ReadOnly Property CanPark() As Boolean

    ''' <summary>
    ''' True if driver is capable of setting dome altitude.
    ''' </summary>
    ReadOnly Property CanSetAltitude() As Boolean

    ''' <summary>
    ''' True if driver is capable of setting dome azimuth.
    ''' </summary>
    ReadOnly Property CanSetAzimuth() As Boolean

    ''' <summary>
    ''' True if driver can set the dome park position.
    ''' </summary>
    ReadOnly Property CanSetPark() As Boolean

    ''' <summary>
    ''' True if driver is capable of automatically operating shutter.
    ''' </summary>
    ReadOnly Property CanSetShutter() As Boolean

    ''' <summary>
    ''' True if the dome hardware supports slaving to a telescope.
    ''' </summary>
    ReadOnly Property CanSlave() As Boolean

    ''' <summary>
    ''' True if driver is capable of synchronizing the dome azimuth position using the Dome.SyncToAzimuth method.
    ''' </summary>
    ReadOnly Property CanSyncAzimuth() As Boolean

    ''' <summary>
    ''' Close shutter or otherwise shield telescope from the sky.
    ''' </summary>
    Sub CloseShutter()

    ''' <summary>
    ''' Dispose the late-bound interface, if needed. Will release it via COM
    ''' if it is a COM object, else if native .NET will just dereference it
    ''' for GC.
    ''' </summary>
    Sub Dispose()

    ''' <summary>
    ''' Description and version information about this ASCOM dome driver.
    ''' </summary>
    Sub FindHome()

    ''' <summary>
    ''' Open shutter or otherwise expose telescope to the sky.
    ''' Raises an error if not supported or if a communications failure occurs. 
    ''' </summary>
    Sub OpenShutter()

    ''' <summary>
    ''' Rotate dome in azimuth to park position.
    ''' After assuming programmed park position, sets Dome.AtPark flag. Raises an error if Dome.Slaved is True,
    ''' or if not supported, or if a communications failure has occurred. 
    ''' </summary>
    Sub Park()

    ''' <summary>
    ''' Set the current azimuth, altitude position of dome to be the park position.
    ''' Raises an error if not supported or if a communications failure occurs. 
    ''' </summary>
    Sub SetPark()

    ''' <summary>
    ''' Status of the dome shutter or roll-off roof.
    ''' Raises an error only if no shutter control.
    ''' If actual shutter status can not be read, 
    ''' then reports back the last shutter state. 
    ''' </summary>
    ReadOnly Property ShutterStatus() As ShutterState

    ''' <summary>
    ''' True if the dome is slaved to the telescope in its hardware, else False.
    ''' Set this property to True to enable dome-telescope hardware slaving,
    ''' if supported (see Dome.CanSlave). Raises an exception on any attempt to set 
    ''' this property if hardware slaving is not supported).
    ''' Always returns False if hardware slaving is not supported. 
    ''' </summary>
    Property Slaved() As Boolean

    ''' <summary>
    ''' True if any part of the dome is currently moving, False if all dome components are steady.
    ''' Raises an error if Dome.Slaved is True, if not supported, if a communications failure occurs,
    ''' or if the dome can not reach indicated azimuth. 
    ''' </summary>
    ReadOnly Property Slewing() As Boolean

    ''' <summary>
    ''' Slew the dome to the given altitude position.
    ''' Raises an error if Dome.Slaved is True, if not supported, if a communications failure occurs,
    ''' or if the dome can not reach indicated altitude. 
    ''' </summary>
    ''' <param name="Altitude">Target dome altitude (degrees, horizon zero and increasing positive to 90 zenith)</param>
    Sub SlewToAltitude(ByVal Altitude As Double)

    ''' <summary>
    ''' Slew the dome to the given azimuth position.
    ''' Raises an error if Dome.Slaved is True, if not supported, if a communications failure occurs,
    ''' or if the dome can not reach indicated azimuth. 
    ''' </summary>
    ''' <param name="Azimuth">Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East, 180 South, 270 West)</param>
    Sub SlewToAzimuth(ByVal Azimuth As Double)

    ''' <summary>
    ''' Synchronize the current position of the dome to the given azimuth.
    ''' Raises an error if not supported or if a communications failure occurs. 
    ''' </summary>
    ''' <param name="Azimuth">Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East, 180 South, 270 West)</param>
    Sub SyncToAzimuth(ByVal Azimuth As Double)

End Interface