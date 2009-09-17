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
' -----------------------------------------------------------------------------'

'local variables to hold property values
Private m_Maximum As Double
Private m_Minimum As Double

Public Property Let Maximum(ByVal max As Double)

    m_Maximum = max
    
End Property

Public Property Get Maximum() As Double
    
    Maximum = m_Maximum

End Property

Public Property Let Minimum(ByVal min As Double)
    
    m_Minimum = min

End Property

Public Property Get Minimum() As Double

    Minimum = m_Minimum

End Property
