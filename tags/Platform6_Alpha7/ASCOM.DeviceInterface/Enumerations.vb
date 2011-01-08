Imports System.Runtime.InteropServices

'tabs=4
' --------------------------------------------------------------------------------
' <summary>
' ASCOM.Interface Telescope Enumerations
' </summary>
'
' <copyright company="The ASCOM Initiative" author="Timothy P. Long">
'	Copyright © 2010 The ASCOM Initiative
' </copyright>
'
' <license>
' Permission is hereby granted, free of charge, to any person obtaining
' a copy of this software and associated documentation files (the
' "Software"), to deal in the Software without restriction, including
' without limitation the rights to use, copy, modify, merge, publish,
' distribute, sublicense, and/or sell copies of the Software, and to
' permit persons to whom the Software is furnished to do so, subject to
' the following conditions:
' 
' The above copyright notice and this permission notice shall be
' included in all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
' EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
' MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
' NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
' LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
' OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
' WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
' </license>
'
'
' Defines:	ITelescope interfaces
' Author:		(TPL) Timothy P. Long <Tim@tigranetworks.co.uk>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' 10-Feb-2010	TPL	6.0.*	Initial edit.
' --------------------------------------------------------------------------------
'

''' <summary>
''' The alignment mode of the mount.
''' </summary>
<Guid("B0C32247-9CF4-47bd-A5E4-FD430065CB4A"), ComVisible(True)> _
Public Enum AlignmentModes '30D18B61-AECC-4c03-8759-E3EDD246F062
    ''' <summary>
    ''' Altitude-Azimuth alignment.
    ''' </summary>
    algAltAz

    ''' <summary>
    ''' Polar (equatorial) mount other than German equatorial.
    ''' </summary>
    algPolar

    ''' <summary>
    ''' German equatorial mount.
    ''' </summary>
    algGermanPolar
End Enum

''' <summary>
''' Well-known telescope tracking rates.
''' </summary>
<Guid("3FA761EC-12F4-4757-8637-F0ABE5ECB9F7"), ComVisible(True)> _
Public Enum DriveRates ' D9998808-2DF0-4ca1-ADD6-CE592026C663
    ''' <summary>
    ''' Sidereal tracking rate (15.0 arcseconds per second).
    ''' </summary>
    driveSidereal

    ''' <summary>
    ''' Lunar tracking rate (14.685 arcseconds per second).
    ''' </summary>
    driveLunar

    ''' <summary>
    ''' Solar tracking rate (15.0 arcseconds per second).
    ''' </summary>
    driveSolar

    ''' <summary>
    ''' King tracking rate (15.0369 arcseconds per second).
    ''' </summary>
    driveKing
End Enum

''' <summary>
''' Equatorial coordinate systems used by telescopes.
''' </summary>
<Guid("46AB7149-B2AF-4160-A2FD-17B8923CBADE"), ComVisible(True)> _
Public Enum EquatorialCoordinateType ' 135265BA-25AC-4f43-95E5-80D0171E48FA
    ''' <summary>
    ''' Custom or unknown equinox and/or reference frame.
    ''' </summary>
    equOther

    ''' <summary>
    ''' Local topocentric; this is the most common for amateur telescopes.
    ''' </summary>
    equLocalTopocentric

    ''' <summary>
    ''' J2000 equator/equinox, ICRS reference frame.
    ''' </summary>
    equJ2000

    ''' <summary>
    ''' J2050 equator/equinox, ICRS reference frame.
    ''' </summary>
    equJ2050

    ''' <summary>
    ''' B1950 equinox, FK4 reference frame.
    ''' </summary>
    equB1950
End Enum

''' <summary>
''' The direction in which the guide-rate motion is to be made.
''' </summary>
<Guid("BF98641E-FC63-451d-9310-63EFEFA1B28B"), ComVisible(True)> _
Public Enum GuideDirections ' 3613EEEB-5563-47d8-B512-1D36D64CEEBB
    ''' <summary>
    ''' North (+ declination/altitude).
    ''' </summary>
    guideNorth

    ''' <summary>
    ''' South (- declination/altitude).
    ''' </summary>
    guideSouth

    ''' <summary>
    ''' East (+ right ascension/azimuth).
    ''' </summary>
    guideEast

    ''' <summary>
    ''' West (- right ascension/azimuth)
    ''' </summary>
    guideWest
End Enum

''' <summary>
''' The telescope axes
''' </summary>
<Guid("26F6BD6C-5289-4aa1-B270-F3EA0EBEAFD7"), ComVisible(True)> _
Public Enum TelescopeAxes ' BCB5C21D-B0EA-40d1-B36C-272456F44D01
    ''' <summary>
    ''' Primary axis (e.g., Right Ascension or Azimuth).
    ''' </summary>
    axisPrimary

    ''' <summary>
    ''' Secondary axis (e.g., Declination or Altitude).
    ''' </summary>
    axisSecondary

    ''' <summary>
    ''' Tertiary axis (e.g. imager rotator/de-rotator).
    ''' </summary>
    axisTertiary
End Enum

''' <summary>
''' The side of the pier on which the optical tube assembly is located.
''' </summary>
''' <remarks>
'''		<alert class="caution">
'''			<para>
'''			<c>Pier side</c> is a GEM-specific term that has historically
'''			caused much confusion. Do not confuse <c>Pier Side</c>
'''			with <c>Pointing State</c>.
'''			</para>
'''		</alert>
''' </remarks>
<Guid("6F0E1F45-129A-4c3a-A3B0-3611AEDB33FB"), ComVisible(True)> _
Public Enum PierSide ' ECD99531-A2CF-4b9f-91A0-35FE5D12B043
    ''' <summary>
    ''' Mount on East side of pier (looking West)
    ''' </summary>
    pierEast = 0

    ''' <summary>
    ''' Unknown or indeterminate.
    ''' </summary>
    pierUnknown = -1

    ''' <summary>
    ''' Mount on West side of pier (looking East)
    ''' </summary>
    pierWest = 1
End Enum

''' <summary>
''' ASCOM Camera ShutterState status values.
''' </summary>
<Guid("DA182F18-4133-4d6f-A533-67306F48AC5C"), ComVisible(True)> _
Public Enum ShutterState ' 8915DF3D-B055-4195-8D23-AAD7F58FDF3B
    ''' <summary>
    ''' Camera shutter status open
    ''' </summary>
    shutterOpen = 0
    ''' <summary>
    ''' Camera shutter status closed
    ''' </summary>
    shutterClosed = 1
    ''' <summary>
    ''' Camera shutter status opening
    ''' </summary>
    shutterOpening = 2
    ''' <summary>
    ''' Camera shutter status closing
    ''' </summary>
    shutterClosing = 3
    ''' <summary>
    ''' Camera shutter status error
    ''' </summary>
    shutterError = 4
End Enum

''' <summary>
''' ASCOM Camera status values.
''' </summary>
<Guid("BBD1CD3C-5983-4584-96F9-E22AB0F8BB31"), ComVisible(True)> _
Public Enum CameraStates ' D40EB54D-0F0F-406D-B68F-C2A7984235BC
    ''' <summary>
    ''' Camera status idle
    ''' </summary>
    cameraIdle = 0
    ''' <summary>
    ''' Camera status waiting
    ''' </summary>
    cameraWaiting = 1
    ''' <summary>
    ''' Camera status exposing
    ''' </summary>
    cameraExposing = 2
    ''' <summary>
    ''' Camera status reading
    ''' </summary>
    cameraReading = 3
    ''' <summary>
    ''' Camera status download
    ''' </summary>
    cameraDownload = 4
    ''' <summary>
    ''' Camera status error
    ''' </summary>
    cameraError = 5
End Enum

''' <summary>
''' Sensor type, identifies the type of colour sensor
''' V2 cameras only
''' </summary>]
<Guid("B1F24499-879F-4fa1-8FE1-C491741EBBF4"), ComVisible(True)> _
Public Enum SensorType
    ''' <summary>
    ''' Camera produces monochrome array with no Bayer encoding
    ''' </summary>
    Monochrome = 0
    ''' <summary>
    ''' Camera produces color image directly, requiring not Bayer decoding
    ''' </summary>
    Color = 1
    ''' <summary>
    ''' Camera produces RGGB encoded Bayer array images
    ''' </summary>
    RGGB = 2
    ''' <summary>
    ''' Camera produces CMYG encoded Bayer array images
    ''' </summary>
    CMYG = 3
    ''' <summary>
    ''' Camera produces CMYG2 encoded Bayer array images
    ''' </summary>
    CMYG2 = 4
    ''' <summary>
    ''' Camera produces Kodak TRUESENSE Bayer LRGB array images
    ''' </summary>
    LRGB = 5
End Enum