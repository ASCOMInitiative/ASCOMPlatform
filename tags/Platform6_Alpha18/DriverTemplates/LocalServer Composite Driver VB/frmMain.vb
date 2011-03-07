
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Namespace TEMPLATENAMESPACE
	Public Partial Class frmMain
		Inherits Form
		Delegate Sub SetTextCallback(text As String)

		Public Sub New()
			InitializeComponent()
		End Sub

	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Built and maintained by Todd Anglin and Telerik
'=======================================================
