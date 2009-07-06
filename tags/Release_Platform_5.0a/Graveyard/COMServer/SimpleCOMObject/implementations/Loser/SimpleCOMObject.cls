VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "SimpleCOMObject"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Implements ISimpleCOMObject

Private m_iLongProperty As Long

Public Property Let ISimpleCOMObject_LongProperty(ByVal RHS As Long)
    m_iLongProperty = RHS
End Property

Public Property Get ISimpleCOMObject_LongProperty() As Long
    ISimpleCOMObject_LongProperty = m_iLongProperty
End Property

Public Sub ISimpleCOMObject_Method01(ByVal strMessage As String)
    MsgBox strMessage & CStr(m_iLongProperty), "SimpleCOMObject_VB6Impl.SimpleCOMObject"
End Sub
