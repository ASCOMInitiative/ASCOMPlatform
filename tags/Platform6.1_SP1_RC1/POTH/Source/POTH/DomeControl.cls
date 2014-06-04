VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "DomeControl"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
'---------------------------------------------------------------------
' Copyright © 2012 Chris Rowland
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". I MAKE NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'
'   ==============
'   DomeControl.cls
'   ==============
'
' Written:  2012/05/06   Chris Rowland <chris.rowland@dsl.pipex.com>
'
' This class gets the dome pointing direction using Paul Bourke's
' algorithim for determining the intersections of a sphere and a line.
'
' The line starts at the scope OTA position and goes in the direction
' the scope is looking.
' The sphere represents the dome, there should be one intersection which
' is where the scope pointing direction intercepts the sphere.
'
' Edits:
'
' When       Who     What
' ---------  ---     --------------------------------------------------
' 2012/05/06 cdr     Initial edit
' -----------------------------------------------------------------------------
Option Explicit

' initialisation variables
Private m_mountPosn As XYZ
Private m_domeRadius As Double
Private m_otaOffset As Double
Private m_latitude As Double
Private m_isGem As Boolean

Private Const PI As Double = 3.1415926535

Private Type XY
    X As Double
    Y As Double
End Type

Private Type XYZ
    X As Double
    Y As Double
    z As Double
End Type

Private m_domeAltitude As Double

''' <summary>
''' Set the dome and mount properties, these only need to be done once.
''' The units for the distances do not matter but must be the same for all items.
''' </summary>
''' <param name="DomeRadius">The dome radius.</param>
''' <param name="OtaOffset">The ota offset.</param>
''' <param name="mountOffsetN">The mount offset N.</param>
''' <param name="mountoffsetE">The mountoffset E.</param>
''' <param name="mountOffsetUp">The mount offset up.</param>
''' <param name="latitude">The latitude in degrees</param>
''' <param name="isGem">if set to <c>true</c> [is gem].</param>
Public Sub InitDome(DomeRadius As Double, _
                    OtaOffset As Double, _
                    mountOffsetN As Double, _
                    mountoffsetE As Double, _
                    mountOffsetUp As Double, _
                    Latitude As Double, _
                    isGem As Boolean)
    m_domeRadius = DomeRadius
    m_otaOffset = OtaOffset
    m_mountPosn = SetXYZ(mountOffsetN, mountoffsetE, mountOffsetUp)
    m_latitude = Latitude
    m_isGem = isGem
End Sub

''' <summary>
''' Gets the dome azimuth that is required so the scope can see out using the
''' scope pointing direction, the mount parameters and the dome radius
''' </summary>
''' <param name="scopeAzimuth">The scope azimuth in degrees</param>
''' <param name="scopeAltitude">The scope altitude in degrees</param>
''' <param name="hourAngle">The hour angle in degrees</param>
''' <param name="isPierWest">if set to <c>true</c> [is pier west].</param>
''' <returns>the dome azimuth in degrees</returns>
Public Function DomeAzimuth(scopeAzimuth As Double, scopeAltitude As Double, hourAngle As Double, isPierWest As Boolean) As Double
    Dim scopePos As XY
    Dim domePos As XY
    Dim otaPosn As XYZ

    ' get the ota position WRT the centre of the dome, allowing for the offsets
    ' the scope direction and the latitude
    otaPosn = OtaPosition(hourAngle, isPierWest)

    ' set the scope alt azm in degrees
    scopePos = SetXY(scopeAzimuth, scopeAltitude)

    domePos = DomeAltAzm(scopePos, otaPosn)
    ' set the altitude to the range -180 to +180
    Do While domePos.Y >= 180#
        domePos.Y = domePos.Y - 360#
    Loop
    Do While domePos.Y < -180#
        domePos.Y = domePos.Y + 360#
    Loop
    ' save this so it can be read by the DomeAltitude Function
    m_domeAltitude = domePos.Y

    ' set the azimuth to the range 0 to 360 degrees
    Do While (domePos.X >= 360#)
        domePos.X = domePos.X - 360#
    Loop
    Do While (domePos.X < 0)
        domePos.X = domePos.X + 360#
    Loop
    DomeAzimuth = domePos.X
    Debug.Print "DomeAzimuth Azm:" & scopeAzimuth & ", Alt:" & scopeAltitude & ", Ha:" & hourAngle & ", isPierWest:" & isPierWest; " = " & DomeAzimuth
End Function

''' <summary>
''' Get the dome altitude in degrees.
''' The altitude is set by the DomeAzimuth function
''' so this must only be called immediately after this.
''' </summary>
''' <returns></returns>
Public Function DomeAltitude() As Double
    DomeAltitude = m_domeAltitude
End Function

''' <summary>
''' Return the OTA position wrt the dome centre
''' X is to the N,
''' Y is to the W,
''' Z is up
''' Currently assumes the Nothern hemisphere
''' </summary>
''' <param name="ha>hour angle in degrees</param>
''' <param name="isPierWest"></param>
''' <returns></returns>
Private Function OtaPosition(ha As Double, isPierWest As Boolean) As XYZ
    ' get the position of the mount
    Dim p1 As XYZ
    p1 = m_mountPosn
    p1.Y = -p1.Y        ' change the sign of the y direction to match the WPF coordinate system

    Dim os As XYZ
    os = SetXYZ(0, m_otaOffset, 0)      ' ota offset is along the dec axis

    If (m_otaOffset > 0.00001) Then
        If (m_isGem And isPierWest) Then
            ' reverse the dec ota offset
            os.Y = -os.Y
        End If
        'os.Y = -os.Y
        ' the order of the rotations matters
        ' rotate by dec
'        os = Rotate(os, SetXYZ(0, 1, 0), haDec.Y)
        ' rotate by Ha
        os = Rotate(os, SetXYZ(-1, 0, 0), ha)
        ' rotate by latitude
        os = Rotate(os, SetXYZ(0, -1, 0), Abs(m_latitude))
    End If
    OtaPosition = SetXYZ(p1.X + os.X, p1.Y + os.Y, p1.z + os.z)
    Debug.Print "OtaPosition X:" & OtaPosition.X & ", Y:" & OtaPosition.Y & ", Z:" & OtaPosition.z
End Function

''' <summary>
''' Determine the position of the dome opening using Paul Bourke's intersection of
''' a ray and a sphere code.  The ray represents the scope pointing direction and the
''' sphere the dome.  The OTA offset is determined from the mount offset and the
''' scope pointing direction specifies the direction of the ray.
''' </summary>
''' <param name="scopeAltAz">scope pointing direction in Alt(y) Azm(x), as radians</param>
''' <param name="otaPosn">OTA position wrt the centre of the dome, allowing for the Ha and Dec and the pier side</param>
''' <returns>
''' Dome Altitude(y), Azimuth(x), in degrees
''' </returns>
Private Function DomeAltAzm(scopeAltAz As XY, otaPosn As XYZ) As XY
    ' get the position of the OTA
    Dim p1 As XYZ
    p1 = otaPosn

    ' get the offset from the OTA position towards the scope pointing direction,
    Dim dp As XYZ
    dp = PolarToCartesian(scopeAltAz, m_domeRadius * 4)

    ' the conversion is simplified because we are using the centre of the dome as the origin
    Dim a As Double
    a = dp.X * dp.X + dp.Y * dp.Y + dp.z * dp.z
    Dim b As Double
    b = 2 * (dp.X * p1.X + dp.Y * p1.Y + dp.z * p1.z)
    Dim c As Double
    c = p1.X * p1.X + p1.Y * p1.Y + p1.z * p1.z
    c = c - m_domeRadius * m_domeRadius
    Dim bb4ac As Double
    bb4ac = b * b - 4 * a * c
    If Abs(a) < 0.000001 Or bb4ac < 0 Then
        Err.Raise 123, "DomeControl", "scope is not pointing out of the dome"
    End If
    Dim mu As Double
    mu = (-b + Sqr(bb4ac)) / (2 * a)
    If (mu <= 0 Or mu > 1#) Then
        mu = (-b - Sqr(bb4ac)) / (2 * a)
    End If
    ' There are potentially two points of intersection given by
    ' p = p1 + mu1 (p2 - p1)
    ' p = p1 + mu2 (p2 - p1)
    Dim pos As XYZ
    pos = SetXYZ(p1.X + mu * dp.X, p1.Y + mu * dp.Y, p1.z + mu * dp.z)
    DomeAltAzm = CartesianToPolar(pos)
    Debug.Print "DomeAltAzm Azm:" & DomeAltAzm.X & ", Alt:" & DomeAltAzm.Y
End Function

''' <summary>
''' Rotates the specified val.
''' </summary>
''' <param name="vector">The vector that's rotated</param>
''' <param name="axis">A unit vector defining the axis direction, must be in the X or Y direction</param>
''' <param name="angle">The rotation angle in degrees</param>
''' <returns></returns>
Private Function Rotate(vector As XYZ, axis As XYZ, angle As Double) As XYZ
    Dim ar As Double
    ar = D2R(angle)     ' convert the angle to radians
    ' choose the axis to rotate about
    If Math.Abs(axis.X) > Abs(axis.Y) Then
        If axis.X > 0 Then ar = -ar
        Rotate.X = vector.X
        Rotate.z = Cos(ar) * vector.z - Sin(ar) * vector.Y
        Rotate.Y = Sin(ar) * vector.z + Cos(ar) * vector.Y
    Else
        If axis.Y < 0 Then ar = -ar
        Rotate.Y = vector.Y
        Rotate.X = Cos(ar) * vector.X - Sin(ar) * vector.z
        Rotate.z = Sin(ar) * vector.X + Cos(ar) * vector.z
    End If
End Function

''' <summary>
''' converts angle in degres to radians.
''' </summary>
''' <param name="angle">The angle in degrees</param>
''' <returns>the angle in radians</returns>
Private Function D2R(ByVal angle As Double) As Double
    D2R = angle / 180# * PI
End Function

''' <summary>
''' converts angle in radians to degrees
''' </summary>
''' <param name="rad">The angle in radians</param
''' ><returns>the angle in degrees</returns>
Private Function R2D(ByVal rad As Double) As Double
    R2D = rad * 180# / PI
End Function

Private Function SetXY(X As Double, Y As Double) As XY
    Dim ret As XY
    ret.X = X
    ret.Y = Y
    SetXY = ret
End Function

Private Function SetXYZ(X As Double, Y As Double, z As Double) As XYZ
    Dim ret As XYZ
    ret.X = X
    ret.Y = Y
    ret.z = z
    SetXYZ = ret
End Function

''' <summary>
''' Converts an alt azm vector in radians and a distance in radius to an xyz coordinate
''' </summary>
''' <param name="azmAlt">The azm(X) and alt(Y) angles in degrees</param>
''' <param name="radius">The radius.</param>
''' <returns>XYZ vector, length is radius</returns>
Private Function PolarToCartesian(azmAlt As XY, radius As Double) As XYZ
    Dim azm As Double
    Dim alt As Double
    azm = D2R(azmAlt.X)
    alt = D2R(azmAlt.Y)
    PolarToCartesian = SetXYZ(radius * Cos(alt) * Cos(azm), _
                              radius * Cos(alt) * Sin(azm), _
                              radius * Sin(alt))
End Function

''' <summary>
''' Converts a vector to azimuth and altitude in degrees
''' </summary>
''' <param name="vector">The vector.</param>
''' <returns>azimuth and altitude in degrees</returns>
Private Function CartesianToPolar(vector As XYZ) As XY
    CartesianToPolar = SetXY(R2D(Atn2(vector.Y, vector.X)), _
                             90 - R2D(Atn2(Sqr(vector.X * vector.X + vector.Y * vector.Y), vector.z)))
End Function

''' <summary>
''' Atn2 function, returns angle in radians
''' </summary>
''' <param name="Y">The Y.</param>
''' <param name="X">The X.</param>
''' <returns>angle in radians</returns>
Private Function Atn2(Y As Double, X As Double) As Double
    Select Case X
        Case Is > 0#
            Atn2 = Atn(Y / X)
        Case Is < 0#
            Atn2 = Atn(Y / X) + PI * IIf(Y > 0, 1, -1)
        Case Else
            Atn2 = (PI / 2#) * Sgn(Y)
    End Select
End Function

