VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "VB6Sample"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
#Const ForV2 = True

Option Explicit

Implements IAscomSample

'
' COM Error Constants
'
Private Const SCODE_X_UNSET As Long = &H80040001
Private Const MSG_X_UNSET As String = "The value of X has not been set"
Private Const SCODE_Y_UNSET As Long = &H80040002
Private Const MSG_Y_UNSET As String = "The value of Y has not been set"

'
' Class private vars
'
Private m_X As Double
Private m_Y As Double
Private m_enumVal As IAscomSampleLib.SampleEnumType
'
' Class private functions
'
Private Sub Class_Initialize()
    m_X = -12345678.9012345             ' Sleazy sentinel (typ.)
    m_Y = -12345678.9012345
    m_enumVal = sampleType1
End Sub

Private Sub checkX()
    If m_X = -12345678.9012345 Then Err.Raise SCODE_X_UNSET, , MSG_X_UNSET
End Sub

Private Sub checkY()
    If m_Y = -12345678.9012345 Then Err.Raise SCODE_Y_UNSET, , MSG_Y_UNSET
End Sub

Private Function calcDiag() As Double
    Call checkX
    Call checkY
    calcDiag = Sqr(m_X * m_X + m_Y * m_Y)
End Function

#If ForV2 Then
Private Function calcArea() As Double
    Call checkX
    Call checkY
    calcArea = m_X * m_Y
End Function
#End If
'
' Implementation of IAscomSample
'
Public Function CalculateDiagonal(ByVal X As Double, ByVal Y As Double) As Double
    m_X = X
    m_Y = Y
    CalculateDiagonal = calcDiag()
End Function

Public Property Get Diagonal() As Double
    Diagonal = calcDiag()
End Property

Public Property Let EnumTest(ByVal RHS As SampleEnumType)
    m_enumVal = RHS
End Property

Public Property Get EnumTest() As SampleEnumType
    EnumTest = m_enumVal
End Property

Public Property Let X(ByVal RHS As Double)
    m_X = RHS
End Property

Public Property Get X() As Double
    Call checkX
    X = m_X
End Property

Public Property Let Y(ByVal RHS As Double)
    m_Y = RHS
End Property

Public Property Get Y() As Double
    Call checkY
    Y = m_Y
End Property

#If ForV2 Then
Public Function CalculateArea(ByVal X As Double, ByVal Y As Double) As Double
    m_X = X
    m_Y = Y
    CalculateArea = calcArea()
End Function

Public Property Get Area() As Double
    Area = calcArea()
End Property
#End If

'
' Public interface just calls the above implementations
'
Private Function IAscomSample_CalculateDiagonal(ByVal X As Double, ByVal Y As Double) As Double
    IAscomSample_CalculateDiagonal = CalculateDiagonal(X, Y)
End Function

Private Property Get IAscomSample_Diagonal() As Double
    IAscomSample_Diagonal = Diagonal
End Property

Private Property Let IAscomSample_EnumTest(ByVal RHS As SampleEnumType)
    EnumTest = RHS
End Property

Private Property Get IAscomSample_EnumTest() As SampleEnumType
    IAscomSample_EnumTest = EnumTest
End Property

Private Property Let IAscomSample_X(ByVal RHS As Double)
    X = RHS
End Property

Private Property Get IAscomSample_X() As Double
    IAscomSample_X = X
End Property

Private Property Let IAscomSample_Y(ByVal RHS As Double)
    Y = RHS
End Property

Private Property Get IAscomSample_Y() As Double
    IAscomSample_Y = Y
End Property

#If ForV2 Then
Private Function IAscomSample_CalculateArea(ByVal X As Double, ByVal Y As Double) As Double
    IAscomSample_CalculateArea = CalculateArea(X, Y)
End Function

Private Property Get IAscomSample_Area() As Double
    IAscomSample_Area = Area
End Property
#End If
