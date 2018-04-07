VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "TrackingRates"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"No"
Attribute VB_Ext_KEY = "Collection" ,"Rate"
Attribute VB_Ext_KEY = "Member0" ,"Rate"
' -----------------------------------------------------------------------------'
' =====================
'   TrackingRates.cls
' =====================
'
' Implementation of the ASCOM TrackingRates Class
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' ??-??-??  cr      Initial edit (Chris Rowland created an early version)
' 10-Sep-03 jab     cut out "keys", changed varient to long, minor cleanup
' 28-Feb-04 jab     converted into AxisRates.cls from some pre-V2 experiments
' 21-May-07 rbd     New ASCOM Master interfaces, early bindeable.
' -----------------------------------------------------------------------------

Option Explicit

'========================
Implements ITrackingRates
'========================

' local variable to hold collection
Private mCol As Collection

' Creates the collection when this class is created

Private Sub Class_Initialize()

    Set mCol = New Collection
    
End Sub

' Destroys collection when this class is terminated

Private Sub Class_Terminate()

    Set mCol = Nothing
    
End Sub


' Add an element to the collection

Friend Function Add(newRate As DriveRates) As DriveRates

    mCol.Add newRate
    
    Add = newRate
    
End Function

' Remove an element from the collection
' Example: x.Remove(5)

Friend Sub Remove(Index As Long)

    mCol.Remove Index
    
End Sub

' ================================
' IMPLEMENTATION OF ITrackingRates
' ================================

' Get the number of elements in the collection.
' Example: Debug.Print x.Count

Private Property Get ITrackingRates_Count() As Long
    ITrackingRates_Count = mCol.Count
End Property

' Get an element in the collection
' Example: Set foo = x.Item(5)

Private Property Get ITrackingRates_Item(ByVal Index As Long) As DriveRates
    ITrackingRates_Item = mCol(Index)
End Property

' this property allows you to enumerate
' this collection with the For...Each syntax

Private Property Get ITrackingRates_NewEnum() As stdole.IEnumVARIANT
    Set ITrackingRates_NewEnum = mCol.[_NewEnum]
End Property

' ==============
' PUBLIC MEMBERS
' ==============

Public Property Get Count() As Long
    Count = mCol.Count
End Property

Public Property Get Item(ByVal Index As Long) As DriveRates
    Item = mCol(Index)
End Property

Public Property Get NewEnum() As stdole.IEnumVARIANT
    Set NewEnum = mCol.[_NewEnum]
End Property

