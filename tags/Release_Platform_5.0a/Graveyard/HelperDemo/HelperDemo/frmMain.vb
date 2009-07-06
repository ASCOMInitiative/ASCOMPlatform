Public Class frmMain

	Private Sub cmdChoose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChoose.Click
		Dim C As DriverHelper.Chooser
		Dim ChosenID As String

		C = New DriverHelper.Chooser
		C.DeviceType = "Telescope"			' This is a Telescope chooser
		ChosenID = C.Choose("")				' No scope type to pre-select
		If ChosenID = "" Then
			MsgBox("Cancel", MsgBoxStyle.OkOnly, Application.ProductName)
		Else
			MsgBox("Chosen ID = " & ChosenID, MsgBoxStyle.OkOnly, Application.ProductName)
		End If

	End Sub
End Class
