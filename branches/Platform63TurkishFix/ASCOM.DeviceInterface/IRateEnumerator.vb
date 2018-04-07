Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the IRateEnumerator Interface</summary>
'-----------------------------------------------------------------------
'''<summary>
''' Strongly typed enumerator for late bound Rate
''' objects being enumarated
'''</summary>
<Guid("DCFDCA25-F7DB-4b62-B8FF-50ABB4844E4E"), ComVisible(True)> _
Public Interface IRateEnumerator
    ReadOnly Property Current() As Object
    Sub Dispose()
    Function MoveNext() As Boolean
    Sub Reset()
End Interface