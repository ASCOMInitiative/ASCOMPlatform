'---------------------------------------------------------------------
' Copyright © 2000-2002 SPACE.com Inc., New York, NY
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". SPACE.COM, INC. MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'   ============
'   SetupDialogForm.vb
'   ============
'
' ASCOM Dome Simulator setup form
'
' Written:  20-Jun-03   Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 20-Jun-03 jab     Initial edit
' 27-Jun-03 jab     Initial release
' 03-Dec-04 rbd     Add "Start up with shutter error" mode
' 06-Dec-04 rbd     More non-standard behavior controls, ugh.
' 23-Jun-09 rbt     Port to Visual Basic .NET
' -----------------------------------------------------------------------------
Imports System.Windows.Forms

<ComVisible(False)> _
Public Class SetupDialogForm
    Private m_bResult As Boolean

    Private Sub SetupDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

#Region "Properties"
    Public Property Home() As Double
        Get
           
            Double.TryParse(txtHome.Text, Home)


            Home = AzScale(Home)
        End Get
        Set(ByVal value As Double)
            If value < -360 Or value > 360 Then
                txtHome.Text = "000.0"
            Else
                value = AzScale(value)
                txtHome.Text = Format$(value, "000.0")
            End If
        End Set
    End Property

    Public Property OCDelay() As Double
        Get
            
            Double.TryParse(txtOCDelay.Text, OCDelay)


            If OCDelay > 30 Then _
                OCDelay = 30
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                txtOCDelay.Text = "0"
            Else
                txtOCDelay.Text = Format$(value, "0")
            End If
        End Set
    End Property
    Public Property Park() As Double
        Get
            Park = 180
            Try
                Park = Double.Parse(txtPark.Text)
            Catch ex As Exception

            End Try




            Park = AzScale(Park)
        End Get
        Set(ByVal value As Double)
            If value < -360 Or value > 360 Then
                txtPark.Text = "000.0"
            Else
                value = AzScale(value)
                txtPark.Text = Format$(value, "000.0")
            End If
        End Set
    End Property


    Public Property AltRate() As Double
        Get
            AltRate = 10
            Try
                AltRate = Double.Parse(txtAltRate.Text)
            Catch ex As Exception

            End Try



            If AltRate < 1 Then _
                AltRate = 1
            If AltRate > 90 Then _
                AltRate = 90
        End Get
        Set(ByVal value As Double)
            txtAltRate.Text = Format$(value, "0.0")
        End Set
    End Property

    Public Property AzRate() As Double
        Get
            AzRate = 10
            Try
                AzRate = Double.Parse(txtAzRate.Text)

            Catch ex As Exception

            End Try



            If AzRate < 1 Then _
                AzRate = 1
            If AzRate > 90 Then _
                AzRate = 90
        End Get
        Set(ByVal value As Double)
            txtAzRate.Text = Format$(value, "0.0")
        End Set
    End Property

    Public Property MaxAlt() As Double
        Get
            MaxAlt = 90
            Try
                MaxAlt = Double.Parse(txtMaxAlt.Text)
            Catch ex As Exception

            End Try



            If MaxAlt < 0 Then _
                MaxAlt = 0
            If MaxAlt > 90 Then _
                MaxAlt = 90
        End Get
        Set(ByVal value As Double)
            txtMaxAlt.Text = Format$(value, "0.0")
        End Set
    End Property
    Public Property MinAlt() As Double
        Get
            MinAlt = 0


            Try
                MinAlt = Double.Parse(txtMinAlt.Text)
            Catch ex As Exception

            End Try

            If MinAlt < 0 Then _
                MinAlt = 0
            If MinAlt > 90 Then _
                MinAlt = 90
        End Get
        Set(ByVal value As Double)
            txtMinAlt.Text = Format$(value, "0.0")
        End Set
    End Property
    Public Property StepSize() As Double
        Get
            StepSize = 1
            Try
                StepSize = Double.Parse(txtStepSize.Text)
            Catch ex As Exception

            End Try



            If StepSize < 1 Then _
                StepSize = 1
            If StepSize > 90 Then _
                StepSize = 90
        End Get
        Set(ByVal value As Double)
            txtStepSize.Text = Format$(value, "0.0")
        End Set
    End Property
#End Region

End Class