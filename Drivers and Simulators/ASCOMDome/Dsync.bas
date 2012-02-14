Attribute VB_Name = "modDsync"
' This file is part of DomeSync.
'
' DomeSync is free software; you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation; either version 2 of the License, or
' (at your option) any later version.
'
' DomeSync is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with DomeSync; if not, write to the Free Software
' Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

' The following equations are based on the material presented at
' http://www.brayebrookobservatory.org/BrayObsWebSite/HOMEPAGE/BrayObs.html
' by Mr. Chris Lord
'
' Coded by John Oliver
'
Option Explicit

Private Xdome0 As Double, Ydome0 As Double, Zdome0 As Double
Private Rdome As Double, rDecAxis As Double
Private phi As Double
Private HArad As Double, DECrad As Double

Private Const PI As Double = 3.1415926535

Private Function Atn2(num As Double, denom As Double) As Double
  If denom = 0 Then
    Atn2 = PI / 2# + Sgn(num)
    Exit Function
  End If
  If denom > 0# Then
   Atn2 = Atn(num / denom)
  ElseIf denom < 0 Then
   Atn2 = Atn(num / denom) + PI
  End If
End Function

Public Sub CalcDomeAzAlt(ha As Double, dec As Double, _
   phi As Double, _
   Xdome0 As Double, Ydome0 As Double, Zdome0 As Double, _
   rDecAxis As Double, Rdome As Double, _
   dome_A As Double, dome_h As Double)
' all angles are in radians
Dim A As Double, B As Double, C As Double
Dim d As Double, E As Double, F As Double
Dim knum As Double, k As Double
Dim Xdome As Double, Ydome As Double, Zdome As Double
Dim sin_h As Double
    
    A = Xdome0 + rDecAxis * Cos(phi - PI / 2#) * Sin(ha - PI)
    B = Ydome0 + rDecAxis * Cos(ha - PI)
    C = Zdome0 - rDecAxis * Sin(phi - PI / 2#) * Sin(ha - PI)
    d = Cos(phi - PI / 2#) * Cos(dec) * Cos(-ha) + Sin(phi - PI / 2#) * Sin(dec)
    E = Cos(dec) * Sin(-ha)
    F = -Sin(phi - PI / 2#) * Cos(dec) * Cos(-ha) + Cos(phi - PI / 2#) * Sin(dec)
    knum = -(A * d + B * E + C * F) + _
      Sqr((A * d + B * E + C * F) ^ 2 + (d ^ 2 + E ^ 2 + F ^ 2) * (Rdome ^ 2 - A ^ 2 - B ^ 2 - C ^ 2))
    k = knum / (d ^ 2 + E ^ 2 + F ^ 2)
    Xdome = A + d * k
    Xdome = -Xdome ' test reversed X
    Ydome = B + E * k
    Zdome = C + F * k
    dome_A = -Atn2(Ydome, Xdome)
    dome_A = -dome_A  ' reversed Az sign
    If dome_A < 0 Then dome_A = 2 * PI + dome_A
    sin_h = Zdome / Rdome
    dome_h = Asin(sin_h)
End Sub

Private Function Asin(ang As Double) As Double
    If ang = 1 Or ang = -1 Then
        Asin = PI / 2# * Sgn(ang)
        Exit Function
    End If
    Asin = Atn(ang / Sqr(-ang * ang + 1))
End Function

Public Function rad2deg(rad As Double) As Double
    rad2deg = rad * 180# / PI
End Function

Public Function deg2rad(deg As Double) As Double
    deg2rad = deg / 180# * PI
End Function
