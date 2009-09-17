Attribute VB_Name = "CalcDome"
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

Option Explicit

Const pi As Double = 3.1415926535

' all angles are in radians
Public Sub CalcDomeAzAlt(HA As Double, Dec As Double, _
        phi As Double, _
        Xdome0 As Double, Ydome0 As Double, Zdome0 As Double, _
        rDecAxis As Double, Rdome As Double, _
        dome_A As Double, dome_h As Double)

    Dim A As Double, B As Double, C As Double
    Dim D As Double, E As Double, F As Double
    Dim knum As Double, k As Double
    Dim Xdome As Double, Ydome As Double, Zdome As Double
    Dim sin_h As Double
    
    A = Xdome0 + rDecAxis * Cos(phi - pi / 2#) * Sin(HA - pi)
    B = Ydome0 + rDecAxis * Cos(HA - pi)
    C = Zdome0 - rDecAxis * Sin(phi - pi / 2#) * Sin(HA - pi)
    D = Cos(phi - pi / 2#) * Cos(Dec) * Cos(-HA) + Sin(phi - pi / 2#) * Sin(Dec)
    E = Cos(Dec) * Sin(-HA)
    F = -Sin(phi - pi / 2#) * Cos(Dec) * Cos(-HA) + Cos(phi - pi / 2#) * Sin(Dec)
    knum = -(A * D + B * E + C * F) + _
            Sqr((A * D + B * E + C * F) ^ 2 + _
            (D ^ 2 + E ^ 2 + F ^ 2) * (Rdome ^ 2 - A ^ 2 - B ^ 2 - C ^ 2))
    k = knum / (D ^ 2 + E ^ 2 + F ^ 2)
    Xdome = A + D * k
    Xdome = -Xdome      ' test reversed X
    Ydome = B + E * k
    Zdome = C + F * k
    
    dome_A = -Atn2(Ydome, Xdome)
    dome_A = -dome_A    ' reversed Az sign
    If dome_A < 0 Then _
        dome_A = 2 * pi + dome_A
        
    sin_h = Zdome / Rdome
    dome_h = Asin(sin_h)
    
End Sub

Private Function Asin(ang As Double) As Double

    If ang = 1 Or ang = -1 Then
        Asin = pi / 2# * Sgn(ang)
        Exit Function
    End If
    
    Asin = Atn(ang / Sqr(-ang * ang + 1))
    
End Function

Private Function Atn2(num As Double, denom As Double) As Double

    If denom = 0 Then
        Atn2 = pi / 2# + Sgn(num)
        Exit Function
    End If
    
    If denom > 0# Then
        Atn2 = Atn(num / denom)
    ElseIf denom < 0 Then
        Atn2 = Atn(num / denom) + pi
    End If
  
End Function
