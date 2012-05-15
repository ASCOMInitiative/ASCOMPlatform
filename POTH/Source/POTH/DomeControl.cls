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

Private m_mountPosn As XYZ
Private m_domeRadius As Double
Private m_otaOffset As Double

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

' set the dome and mount properties, these only need to be
' done once.
' The units for the distances do not matter but must be the same for
' all items.
Public Sub InitDome(DomeRadius As Double, _
                    OtaOffset As Double, _
                    mountOffsetN As Double, _
                    mountoffsetE As Double, _
                    mountOffsetUp As Double)
    m_domeRadius = DomeRadius
    m_otaOffset = OtaOffset
    m_mountPosn = SetXYZ(mountOffsetN, mountoffsetE, mountOffsetUp)
End Sub

' Gets the dome azimuth that is required so the scope can see out using the
' scope pointing direction, the mount parameters and the dome radius
'
' scopeAzimuth  azimuth of scope in degrees
' scopeAltitude altitude of scope in degrees
' hourAngle     hour angle in degrees
' isPierWest    true is the pier is west of the meridian, don't care for fork mounts
'
' returns the dome azimuth in degrees
Public Function DomeAzimuth(scopeAzimuth As Double, scopeAltitude As Double, hourAngle As Double, isPierWest As Boolean) As Double
    Dim scopePos As XY
    Dim domePos As XY
    Dim angle As Double
    
    ' convert scope alt azm to radians
    scopePos = SetXY(scopeAzimuth * PI / 180#, scopeAltitude * PI / 180#)
    
    domePos = DomeAltAzm(scopePos, hourAngle * PI / 180#, isPierWest)
    ' get the altitude and convert to degrees
    angle = domePos.Y * 180# / PI
    Do While angle >= 180#
        angle = angle - 360#
    Loop
    Do While angle < -180#
        angle = angle + 360#
    Loop
    ' save this so it can be read by the DomeAltitude Function
    m_domeAltitude = angle
    ' get the azimuth and convert to degrees
    angle = domePos.X * 180# / PI
    ' force it to the range 0 to 360 degrees
    Do While (angle >= 360#)
        angle = angle - 360#
    Loop
    Do While (angle < 0)
        angle = angle + 360#
    Loop
    DomeAzimuth = angle
End Function

' Get the dome altitude in degrees.
' The altitude is set by the DomeAzimuth function
' so this must only be called immediately after this.
Public Function DomeAltitude() As Double
    DomeAltitude = m_domeAltitude
End Function

''' <summary>
''' Determine the position of the dome opening using Paul Bourke's intersection of
''' a ray and a sphere code.  The ray represents the scope pointing direction and the
''' sphere the dome.  The OTA offset is determined from the mount offset and the
''' scope pointing direction specifies the direction of the ray.
''' </summary>
''' <param name="scopeAltAz">scope pointing direction in Alt(y) Azm(x), as radians</param>
''' <param name="scopeHaAxis">The scope hour angle axis in radians</param>
''' <param name="isPierWest">if set to <c>true</c> if the scope is on the west side of the pier.</param>
''' <returns>
''' Dome Altitude(y), Azimuth(x), in radians
''' </returns>
Private Function DomeAltAzm(scopeAltAz As XY, scopeHaAxis As Double, isPierWest As Boolean) As XY
    ' get the position of the OTA
    Dim p1 As XYZ
    p1 = SetXYZ(m_mountPosn.X, m_mountPosn.Y, m_mountPosn.z)
    
    ' apply the ota offset in the direction of the hour angle
    If Abs(m_otaOffset) > 0.000001 Then
        Dim os As XYZ
        os = PolarToCartesian(SetXY(scopeHaAxis, 0), IIf(isPierWest, m_otaOffset, -m_otaOffset))
        p1.X = p1.X + os.Y
        p1.Y = p1.Y - os.X
        p1.z = p1.z + os.z
    End If
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

Private Function PolarToCartesian(polar As XY, radius As Double) As XYZ
    PolarToCartesian = SetXYZ(radius * Cos(polar.Y) * Cos(polar.X), radius * Cos(polar.Y) * Sin(polar.X), radius * Sin(polar.Y))
End Function

Private Function CartesianToPolar(cartes As XYZ) As XY
    CartesianToPolar = SetXY(Atn2(cartes.Y, cartes.X), Atn2(Sqr(cartes.X * cartes.X + cartes.Y * cartes.Y), cartes.z))
End Function

Private Function Atn2(Y As Double, X As Double) As Double
    Select Case X
    Case 0
        Atn2 = PI / 2# + Sgn(Y)
    Case Is > 0#
        Atn2 = Atn(Y / X)
    Case Is < 0
        Atn2 = Atn(Y / X) + PI
    End Select
End Function

