VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "AxisRates"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"No"
Attribute VB_Ext_KEY = "Collection" ,"Rate"
Attribute VB_Ext_KEY = "Member0" ,"Rate"
' -----------------------------------------------------------------------------'
' ==================
'   AxisRates.cls
' ==================
'
' Implementation of the ASCOM AxisRates Class
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

Implements IAxisRates


' local variable to hold collection
Private mCol As Collection

'
' Creates the collection when this class is created
'
Private Sub Class_Initialize()

    Set mCol = New Collection
    
End Sub

'
' Destroys collection when this class is terminated
'
Private Sub Class_Terminate()

    Set mCol = Nothing
    
End Sub


' Add an element to the collection
' Example: set foo = x.Add(3.0, 1.0), or just x.Add(3.0, 1.0)

Friend Function Add(Maximum As Double, Minimum As Double) As Rate

    'create a new object
    Dim objNewMember As Rate
    Set objNewMember = New Rate
    
    'set the properties passed into the method
    objNewMember.Maximum = Maximum
    objNewMember.Minimum = Minimum
    mCol.Add objNewMember
    
    'return the object created
    Set Add = objNewMember
    Set objNewMember = Nothing
    
End Function

' Remove an element from the collection
' Example: x.Remove(5)

Friend Sub Remove(Index As Long)

    mCol.Remove Index
    
End Sub

' ============================
' IMPLEMENTATION OF IAxisRates
' ============================

' Get the number of elements in the collection.
' Example: Debug.Print x.Count

Private Property Get IAxisRates_Count() As Long
    IAxisRates_Count = mCol.Count
End Property

' Get an element in the collection
' Example: Set foo = x.Item(5)

Private Property Get IAxisRates_Item(ByVal Index As Long) As IRate
    Set IAxisRates_Item = mCol(Index)
End Property

' this property allows you to enumerate
' this collection with the For...Each syntax

Private Property Get IAxisRates_NewEnum() As stdole.IEnumVARIANT
    Set IAxisRates_NewEnum = mCol.[_NewEnum]
End Property

' ==============
' PUBLIC MEMBERS
' ==============

Public Property Get Count() As Long
    Count = mCol.Count
End Property

Public Property Get Item(ByVal Index As Long) As Rate
    Set Item = mCol(Index)
End Property

Public Property Get NewEnum() As stdole.IEnumVARIANT
Attribute NewEnum.VB_UserMemId = -4
Attribute NewEnum.VB_MemberFlags = "40"
    Set NewEnum = mCol.[_NewEnum]
End Property
