VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Rate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Member0" ,"Collection1"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
' -----------------------------------------------------------------------------'
' ==================
'   Rate.cls
' ==================
'
' Implementation of the ASCOM Rate Class
'
' Written: Chris Rowland
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' ??-??-??  cr      Initial edit
' 21-May-07 rbd     Add new ASCOM master interface, for early binding
' -----------------------------------------------------------------------------'

Option Explicit

'===============
Implements IRate
'===============

'local variables to hold property values
Private m_Maximum As Double
Private m_Minimum As Double

'========================
' IMPLEMEMTATION OF IRate
' =======================

Private Property Let IRate_Maximum(ByVal RHS As Double)
    m_Maximum = RHS
End Property

Private Property Get IRate_Maximum() As Double
    IRate_Maximum = m_Maximum
End Property

Private Property Let IRate_Minimum(ByVal RHS As Double)
    m_Minimum = RHS
End Property

Private Property Get IRate_Minimum() As Double
    IRate_Minimum = m_Minimum
End Property

' ================
' PUBLIC INTERFACE
' ================

Public Property Get Maximum() As Double
    Maximum = m_Maximum
End Property

Public Property Let Maximum(ByVal RHS As Double)
    m_Maximum = RHS
End Property

Public Property Get Minimum() As Double
    Minimum = m_Minimum
End Property

Public Property Let Minimum(ByVal RHS As Double)
    m_Minimum = RHS
End Property
