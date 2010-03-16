
Imports System

Namespace TEMPLATENAMESPACE
	Partial Class frmMain
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overloads Overrides Sub Dispose(disposing As Boolean)
            If disposing AndAlso components Is Nothing Then
                components.Dispose()
            End If
			MyBase.Dispose(disposing)
		End Sub

#Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.label1 = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            ' 
            ' label1
            ' 
            Me.label1.Location = New System.Drawing.Point(12, 10)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(199, 33)
            Me.label1.TabIndex = 0
            Me.label1.Text = "This is an ASCOM driver, not a program for you to use."
            ' 
            ' frmMain
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(233, 52)
            Me.Controls.Add(Me.label1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
            Me.Name = "frmMain"
            Me.Text = "TEMPLATEDEVICENAME Driver Server"
            Me.ResumeLayout(False)

        End Sub
#End Region

		Private label1 As System.Windows.Forms.Label

	End Class
End Namespace


'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Built and maintained by Todd Anglin and Telerik
'=======================================================
