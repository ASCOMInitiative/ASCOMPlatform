'
' This was created by adding a "COM Class" item to the project
'
<ComClass(Test.ClassId, Test.InterfaceId, Test.EventsId)> _
Public Class Test

	' =============================================================
	' WARNING! THESE GUIDS MUST BE UNIQUE FOR EACH COM OBJECT!!!!!!
	' =============================================================
#Region "COM GUIDs"
	' These  GUIDs provide the COM identity for this class 
	' and its COM interfaces. If you change them, existing 
	' clients will no longer be able to access the class.
	Public Const ClassId As String = "0704811b-7db3-477b-a618-097993e4b5c6"
	Public Const InterfaceId As String = "eda140f6-b9f2-43ab-935d-6e718ecca154"
	Public Const EventsId As String = "e17499c4-33ee-4e26-b34b-2eae79b63a6f"
#End Region

	' A creatable COM class must have a Public Sub New() 
	' with no parameters, otherwise, the class will not be 
	' registered in the COM registry and cannot be created 
	' via CreateObject.
	Public Sub New()
		MyBase.New()
	End Sub

	'
	' PRIVATE FIELDS (Property COM visible = False generates these attributes)
	'
	<System.Runtime.InteropServices.ComVisible(False)> Private c_Age As Integer = 0
	<System.Runtime.InteropServices.ComVisible(False)> Private c_Name As String = ""

	'
	' PUBLIC INTERFACE
	'
	Public Property Name() As String
		Get
			Name = c_Name
		End Get
		Set(ByVal value As String)
			c_Name = value
		End Set
	End Property

	Public Property Age() As Integer
		Get
			Age = c_Age
		End Get
		Set(ByVal value As Integer)
			c_Age = value
		End Set
	End Property

	Public Function PopUp(ByVal message As String) As Boolean
		PopUp = MsgBox("Hello " + c_Name + ". " + message + " Are you " + CStr(c_Age) + _
		  " years old?", MsgBoxStyle.YesNo, "BaseCom.Test") = MsgBoxResult.Yes
	End Function

End Class
