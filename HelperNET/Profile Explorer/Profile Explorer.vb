Imports System.IO
Public Class frmProfileExplorer
    Private RecursionLevel As Integer
    Private Values As Generic.SortedList(Of String, String) 'Variable to hold current key values
    Private KeyPath As String ' Profile path to the current key
    Private Prof As XMLAccess

    Const ROOT_NAME As String = "Profile Root"

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Prof = New XMLAccess()

        RecursionLevel = 0
        ProcessTree("C:\Documents and Settings\All Users\Application Data\ASCOM\Profile", "Profile Root", Nothing, -1) 'Keys.Nodes(0)
        Dim snd As Object, args As New TreeNodeMouseClickEventArgs(KeyTree.Nodes(0), Windows.Forms.MouseButtons.Left, 1, 0, 0)
        snd = Nothing
        ReadProfile(snd, args)

    End Sub

    Sub ProcessTree(ByVal Dir As String, ByVal Name As String, ByVal CurrentNode As TreeNode, ByVal NodeNumber As Integer)
        Dim DirNum, MyNodeNumber As Integer
        Dim DirObj As DirectoryInfo
        Dim Dirs As DirectoryInfo()
        Dim DirectoryName As DirectoryInfo

        MyNodeNumber = NodeNumber + 1
        RecursionLevel += 1
        KeyTree.BeginUpdate()
        If CurrentNode Is Nothing Then
            KeyTree.Nodes.Add(Name)
            KeyTree.Nodes(0).ForeColor = Color.Gray
        Else
            CurrentNode.Nodes.Add(New TreeNode(Name))
        End If

        KeyTree.EndUpdate()
        KeyTree.Refresh()

        DirObj = New DirectoryInfo(Dir)
        Dirs = DirObj.GetDirectories("*.*")
        DirNum = -1
        For Each DirectoryName In Dirs
            Try
                If CurrentNode Is Nothing Then
                    ProcessTree(DirectoryName.FullName, DirectoryName.Name, KeyTree.Nodes(0), DirNum)
                Else
                    ProcessTree(DirectoryName.FullName, DirectoryName.Name, CurrentNode.Nodes(MyNodeNumber), DirNum)
                End If
                DirNum += 1
            Catch E As Exception
                MessageBox.Show("Error accessing " & DirectoryName.FullName)
            End Try
        Next
        RecursionLevel -= 1
    End Sub
    Sub ReadProfile(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles KeyTree.NodeMouseClick
        Dim Parms() As Object
        'MsgBox(e.Node.FullPath.ToString)
        Try
            KeyValues.Rows.Clear()
            ReDim Parms(1)
            Parms(0) = e.Node.FullPath.ToString
            Parms(1) = e.Button.ToString
            'KeyValues.Rows.Add(Parms)

            KeyPath = e.Node.FullPath.ToString
            KeyPath = Mid(KeyPath, Len(ROOT_NAME) + 1)
            If Microsoft.VisualBasic.Left(KeyPath, 1) <> "\" Then KeyPath = "\" & KeyPath
            Values = Prof.EnumProfile(KeyPath)
            For Each kvp In Values
                Parms(0) = kvp.Key
                Parms(1) = kvp.Value
                KeyValues.Rows.Add(Parms)
            Next
        Catch ex As Exception
            MsgBox("ReadProfile Exception: " & ex.ToString)
        End Try
    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        KeyValues.Width = Me.Width - 350
        KeyValues.Height = Me.Height - 50
        KeyTree.Height = Me.Height - 50
        KeyValues.Columns.Item(0).Width = CInt((KeyValues.Width - 43.0) / 2.0)
        KeyValues.Columns.Item(1).Width = CInt((KeyValues.Width - 43.0) / 2.0)

    End Sub

    Private Sub Commit(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles KeyValues.CellEndEdit
        'If KeyValues.IsCurrentCellDirty Then 'Commit value back to the profile store
        Select Case e.ColumnIndex
            Case 0 'Value name has changed

                Try
                    If KeyValues.CurrentRow.Cells(1).Value.ToString = "" Then : End If
                Catch ex As Exception
                    'MsgBox(ex.Message)
                    KeyValues.CurrentRow.Cells(1).Value = ""
                End Try

                'MsgBox("Value name changed " & e.ColumnIndex.ToString & " " & e.RowIndex.ToString & " " & KeyPath & "#" & KeyValues.CurrentCell.Value.ToString & "#" & KeyValues.CurrentRow.Cells(1).Value.ToString & "#")
                Prof.WriteProfile(KeyPath, KeyValues.CurrentCell.Value.ToString, KeyValues.CurrentRow.Cells(1).Value.ToString) 'Values.Values(e.RowIndex)) 'Create new value
                If e.RowIndex <= (Values.Count - 1) Then Prof.DeleteProfile(KeyPath, Values.Keys(e.RowIndex)) 'Delete old value if not a new row

                Values = Prof.EnumProfile(KeyPath) 'Refresh the values from the profile store

            Case 1 'Value data has changed
                'Write new value back to the profile

                Try
                    If KeyValues.CurrentRow.Cells(0).Value.ToString = "" Then : End If
                Catch ex As Exception
                    'MsgBox(ex.Message)
                    KeyValues.CurrentRow.Cells(0).Value = ""
                End Try


                'Prof.WriteProfile(KeyPath, Values.Keys(e.RowIndex), KeyValues.CurrentCell.Value.ToString)
                Prof.WriteProfile(KeyPath, KeyValues.CurrentRow.Cells(0).Value.ToString, KeyValues.CurrentCell.Value.ToString)
                'MsgBox("Value data changed " & e.ColumnIndex.ToString & " " & e.RowIndex.ToString & " " & KeyPath & " " & Values.Keys(e.RowIndex) & " " & KeyValues.CurrentCell.Value.ToString)
                Values = Prof.EnumProfile(KeyPath) 'Refresh the values from the profile store
        End Select

        'End If
    End Sub

    Sub ValuesDeleting(ByVal sender As Object, ByVal e As DataGridViewRowCancelEventArgs) Handles KeyValues.UserDeletingRow
        Dim Res As MsgBoxResult
        Res = MsgBox("Are you sure you want to delete this row?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Exclamation, "Confirm delete")
        If Res = MsgBoxResult.Ok Then
            'MsgBox("Deleting: " & KeyPath & "#" & KeyValues.Rows(e.Row.Index).Cells(0).Value.ToString & "#")
            Prof.DeleteProfile(KeyPath, KeyValues.Rows(e.Row.Index).Cells(0).Value.ToString)
            Values = Prof.EnumProfile(KeyPath) 'Refresh the values from the profile store
        Else
            e.Cancel = True
        End If
    End Sub


End Class
