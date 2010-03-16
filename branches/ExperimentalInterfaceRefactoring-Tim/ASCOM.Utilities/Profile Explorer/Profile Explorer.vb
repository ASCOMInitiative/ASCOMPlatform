Imports System.IO
Imports System.Reflection
Public Class frmProfileExplorer
    Private RecursionLevel As Integer
    Private Values As Generic.SortedList(Of String, String) 'Variable to hold current key values
    Private KeyPath As String ' Profile path to the current key
    Private Prof As XMLAccess
    Private Settings As UtilitiesSettings
    Private CurrentSubKey As String

    Private LastNode As TreeNode, Action As ActionType

    Enum ActionType As Integer
        None
        Add
        Delete
        Rename
    End Enum

    Const ROOT_NAME As String = "Profile Root"
    Const TOOLTIP_ROOT_RO As String = "The Profile root is read-only for safety, use Options to enable write access"
    Const TOOLTIP_ROOT_RW As String = "The Profile root is writable, please be very careful!"


    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MyAssVer As Version = Assembly.GetExecutingAssembly.GetName.Version
        Me.Text = "Profile Explorer " & MyAssVer.ToString
        Prof = New XMLAccess()
        Settings = New UtilitiesSettings 'Read the state of the edit flag
        mnuRootEdit.Checked = Settings.ProfileRootEdit
        Settings.Dispose()
        Settings = Nothing
        RecursionLevel = 0
        Dim ProfileProvider As New AllUsersFileSystemProvider 'Get a file system provider so it can tell s what the base path is on this system
        ProcessTree(ProfileProvider.BasePath, ROOT_NAME, Nothing, -1) 'Process the directory tree starting at the base point = root node
        Dim snd As Object, args As New TreeNodeMouseClickEventArgs(KeyTree.Nodes(0), Windows.Forms.MouseButtons.Left, 1, 0, 0)
        snd = Nothing
        KeyTree.ShowNodeToolTips = True
        ReadProfile(snd, args)
        LastNode = KeyTree.TopNode 'Initialise LastNode
        KeyTree.TopNode.Expand()
    End Sub

    Sub ProcessTree(ByVal Dir As String, ByVal Name As String, ByVal CurrentNode As TreeNode, ByVal NodeNumber As Integer)
        Dim DirNum, MyNodeNumber As Integer
        Dim DirObj As DirectoryInfo
        Dim Dirs As DirectoryInfo()
        Dim DirectoryName As DirectoryInfo
        Dim NewNode As New TreeNode
        MyNodeNumber = NodeNumber + 1
        RecursionLevel += 1
        KeyTree.BeginUpdate()
        If CurrentNode Is Nothing Then
            NewNode.Name = Name
            NewNode.Text = Name
            NewNode.ToolTipText = "Root node can not be deleted or renamed"
            KeyTree.Nodes.Add(NewNode)
            'KeyTree.Nodes(0).ForeColor = Color.Gray
        Else
            NewNode.Name = Name
            NewNode.Text = Name
            CurrentNode.Nodes.Add(NewNode)
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

    Private Sub KeyTree_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles KeyTree.KeyUp
        Dim e1 As New Windows.Forms.MouseEventArgs(Windows.Forms.MouseButtons.Left, 0, 0, 0, 0)
        Select Case e.KeyData
            Case System.Windows.Forms.Keys.F2
                mnuRenameClick(sender, e1)
                e.Handled = True
            Case Keys.Delete
                mnuDeleteClick(sender, e1)
            Case Keys.Insert
                mnuNewKeyClick(sender, e1)
        End Select
    End Sub

    Sub ReadProfile(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles KeyTree.NodeMouseClick
        Try
            RefreshKeyValues(e.Node.FullPath)
        Catch ex As Exception
            MsgBox("ReadProfile Exception: " & ex.ToString)
        End Try
    End Sub

    Overloads Sub RefreshKeyValues()
        RefreshKeyValues(CurrentSubKey)
    End Sub

    Overloads Sub RefreshKeyValues(ByVal SubKey As String)
        CurrentSubKey = SubKey 'Safe current value so that the display can be refreshed whenever required
        KeyValues.Rows.Clear()
        KeyPath = Mid(SubKey, Len(ROOT_NAME) + 1)
        If Microsoft.VisualBasic.Left(KeyPath, 1) <> "\" Then KeyPath = "\" & KeyPath
        Values = Prof.EnumProfile(KeyPath)
        For Each kvp In Values
            KeyValues.Rows.Add(kvp.Key, kvp.Value)
            If (SubKey = ROOT_NAME) And (mnuRootEdit.Checked = False) Then
                KeyValues.Rows(KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 2).Cells(0).ToolTipText = TOOLTIP_ROOT_RO
                KeyValues.Rows(KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 2).Cells(1).ToolTipText = TOOLTIP_ROOT_RO
                KeyValues.ShowCellToolTips = True
            Else
                If SubKey = ROOT_NAME Then
                    KeyValues.Rows(KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 2).Cells(0).ToolTipText = TOOLTIP_ROOT_RW
                    KeyValues.Rows(KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 2).Cells(1).ToolTipText = TOOLTIP_ROOT_RW
                End If
            End If
        Next
        If (SubKey = ROOT_NAME) And (mnuRootEdit.Checked = False) Then
            KeyValues.Rows(KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 1).Cells(0).ToolTipText = TOOLTIP_ROOT_RO
            KeyValues.Rows(KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 1).Cells(1).ToolTipText = TOOLTIP_ROOT_RO
            KeyValues.ReadOnly = True
            KeyValues.BackgroundColor = Color.Crimson
            KeyValues.ShowCellToolTips = True
        Else
            If SubKey = ROOT_NAME Then
                KeyValues.Rows(KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 1).Cells(0).ToolTipText = TOOLTIP_ROOT_RW
                KeyValues.Rows(KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 1).Cells(1).ToolTipText = TOOLTIP_ROOT_RW
                KeyValues.BackgroundColor = Color.Chartreuse
                KeyValues.ShowCellToolTips = True
            Else
                KeyValues.BackgroundColor = Color.White
                KeyValues.ShowCellToolTips = False
            End If
            KeyValues.ReadOnly = False
        End If

    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        KeyValues.Width = Me.Width - 350
        KeyValues.Height = Me.Height - 65
        KeyTree.Height = Me.Height - 65
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
                Try : Prof.WriteProfile(KeyPath, KeyValues.CurrentCell.Value.ToString, KeyValues.CurrentRow.Cells(1).Value.ToString) : Catch : End Try   'Create new value
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

                If KeyValues.CurrentCell.Value Is Nothing Then 'Guard against value deleted, in which case create an empty string
                    KeyValues.CurrentCell.Value = ""
                End If
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
            Try : Prof.DeleteProfile(KeyPath, KeyValues.Rows(e.Row.Index).Cells(0).Value.ToString) : Catch : End Try
            Values = Prof.EnumProfile(KeyPath) 'Refresh the values from the profile store
        Else
            e.Cancel = True
        End If
    End Sub

#Region "Tree View Right Click"
    'Examples from: http://www.eggheadcafe.com/tutorials/aspnet/847ac120-3cdc-4249-8029-26c15de209d1/net-treeview-faq--drag.aspx
    'Author: Robbie Morris 
    'Article title: .NET TreeView FAQ - Drag and Drop Right Click Menu

    Private Sub KeyTree_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles KeyTree.AfterLabelEdit
        Const NOT_FOUND As Integer = -1
        Dim ct As Integer
        'MsgBox("AfterLabelEdit " & e.Node.FullPath & " " & e.CancelEdit.ToString)
        If Not e.CancelEdit Then 'Check if user has cancelled edit
            If Not (e.Label Is Nothing) Then
                If e.Label.Length > 0 Then 'Check for label being empty string
                    If e.Label.IndexOfAny(New Char() {"@"c, ","c, "!"c}) = NOT_FOUND Then 'Check for invalid characters
                        e.Node.EndEdit(False) ' Stop editing without canceling the label change.
                        'MsgBox("Action" & Action.ToString)
                        'Check whether the new key is duplicate of one already existing
                        Select Case Action
                            Case ActionType.Add
                                'Add options:
                                '1) Edited name, but now edited back to same as original
                                'Test: name and label are the same then confirm name only occurs once in list
                                '2) Edited name to different than original
                                'Test: Name and lable different so test if the name already eists in the collection

                                If e.Node.Name = e.Label Then 'It has been edited back to its original form
                                    ct = 0
                                    For Each TN As TreeNode In e.Node.Parent.Nodes
                                        If TN.Name = e.Node.Name Then ct += 1
                                    Next
                                    If ct > 1 Then
                                        e.CancelEdit = True
                                        MessageBox.Show("Key " & e.Label & " already exists, cannot create a duplicate")
                                        e.Node.BeginEdit()
                                    Else
                                        e.Node.Name = e.Label
                                        e.Node.Text = e.Label
                                        Call KeyCreate(e.Node)
                                        Action = ActionType.None
                                        KeyTree.LabelEdit = False
                                    End If
                                Else 'Edited to different than its original form
                                    If e.Node.Parent.Nodes.ContainsKey(e.Label) Then 'It is a duplicate so show message
                                        e.CancelEdit = True
                                        MessageBox.Show("Key " & e.Label & " already exists, cannot create a duplicate")
                                        e.Node.BeginEdit()
                                    Else 'Not a duplicate so make the change
                                        e.Node.Name = e.Label
                                        e.Node.Text = e.Label
                                        Call KeyCreate(e.Node)
                                        Action = ActionType.None
                                        KeyTree.LabelEdit = False
                                    End If
                                End If
                            Case ActionType.Rename
                                If e.Node.Parent.Nodes.ContainsKey(e.Label) And (e.Label <> e.Node.Name) Then 'Key exists and the new name is not the same as the original so it is a duplicate
                                    e.CancelEdit = True
                                    MessageBox.Show("Key " & e.Label & " already exists, cannot create a duplicate")
                                    e.Node.BeginEdit()
                                Else 'Not a duplicate
                                    If e.Label <> e.Node.Name Then Call KeyRename(e.Node, e.Label) 'Has been changed to a different value from original so rename the key
                                    Action = ActionType.None
                                    KeyTree.LabelEdit = False
                                End If
                            Case Else
                                MsgBox("Unexpected Action type: " & Action.ToString)
                        End Select
                    Else
                        ' Cancel the label edit action, inform the user, and place the node in edit mode again. 
                        e.CancelEdit = True
                        MessageBox.Show("Invalid tree node label." & Microsoft.VisualBasic.ControlChars.Cr & _
                                        "The invalid characters are: '@','.', ',', '!'", "Node Label Edit")
                        e.Node.BeginEdit()
                    End If
                Else
                    ' Cancel the label edit action, inform the user and place the node in edit mode again. 
                    e.CancelEdit = True
                    MessageBox.Show("Invalid tree node label." & Microsoft.VisualBasic.ControlChars.Cr & _
                                    "The label cannot be blank", "Node Label Edit")
                    e.Node.BeginEdit()
                End If
            Else
                'MsgBox("Text not changed")
                Select Case Action
                    Case ActionType.Add
                        'MsgBox(e.Node.FullPath & " " & e.Node.Name & " " & e.Node.Parent.FullPath)
                        ct = 0
                        For Each TN As TreeNode In e.Node.Parent.Nodes
                            If TN.Name = e.Node.Name Then ct += 1
                        Next
                        If ct > 1 Then
                            e.CancelEdit = True
                            MessageBox.Show("Key " & e.Label & " already exists, cannot create a duplicate")
                            e.Node.BeginEdit()
                        Else
                            Call KeyCreate(e.Node)
                            Action = ActionType.None
                            KeyTree.LabelEdit = False
                        End If
                    Case ActionType.Rename
                        'No action required as name has not changed
                    Case Else
                        MsgBox("Unexpected Action type: " & Action.ToString)
                End Select
            End If
        Else 'User has cancelled edit
            e.Node.EndEdit(True)
            KeyTree.LabelEdit = False
        End If

    End Sub

    Sub KeyCreate(ByVal Node As TreeNode)
        Dim SubKeyPath As String
        'MsgBox(Node.FullPath)
        Node.Name = Node.Text 'Set the node name to be the same as the label
        SubKeyPath = Mid(Node.FullPath, InStr(Node.FullPath, "\"))
        Prof.CreateKey(SubKeyPath)
        RefreshKeyValues(Node.FullPath)
    End Sub

    Sub KeyRename(ByVal Node As TreeNode, ByVal NewName As String)
        'MsgBox("Rename: " & Node.FullPath & " " & NewName)
        Prof.RenameKey(Mid(Node.FullPath, Len(ROOT_NAME) + 1), Mid(Node.Parent.FullPath, Len(ROOT_NAME) + 1) & "\" & NewName)
        Node.Name = NewName
        Node.Text = NewName
        RefreshKeyValues(Node.FullPath)
    End Sub

    Private Sub KeyTree_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles KeyTree.MouseUp
        Select Case e.Button
            Case MouseButtons.Right
                'CtxMenu.Show(KeyTree, New Point(e.X, e.Y))
                SetSelectedNodeByPosition(KeyTree, e.X, e.Y)
                If KeyTree.SelectedNode.FullPath = ROOT_NAME Then 'Prevent delete or rename of reoot node
                    mnuDelete.Enabled = False
                    mnuRename.Enabled = False
                Else
                    mnuDelete.Enabled = True
                    mnuRename.Enabled = True
                End If

                mnuCtx.Show(KeyTree, New Point(e.X, e.Y))
            Case Else
        End Select
    End Sub
    Public Sub SetSelectedNodeByPosition(ByVal tv As TreeView, ByVal mouseX As Integer, ByVal mouseY As Integer)
        Dim Node As TreeNode
        Dim pt As Point
        Node = Nothing
        Try
            pt = New Point(mouseX, mouseY)
            tv.PointToClient(pt)
            Node = tv.GetNodeAt(pt)
            tv.SelectedNode = Node
            If (Node Is Nothing) Then Return
            If Not (Node.Bounds.Contains(pt)) Then Return
        Catch ex As Exception
        End Try
    End Sub
    Sub RightClickAdd(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim NewNode As TreeNode = New TreeNode
        '  UI.Hourglass(true);
        Try
            'TreeNode node = tvSample.SelectedNode;
            NewNode.Name = "New Node"
            NewNode.Text = "New Node"

            KeyTree.SelectedNode.Nodes.Add(NewNode)
            KeyTree.SelectedNode.ExpandAll()
            NewNode.BeginEdit()
            Action = ActionType.Add

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Sub mnuDeleteClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mnuDelete.MouseUp
        Dim Res As MsgBoxResult, SubKeyPath As String
        Try
            Res = MsgBox("Are you sure you want to delete this key?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Exclamation, "Confirm delete")
            If Res = MsgBoxResult.Ok Then
                'MsgBox("Deleting: " & KeyPath & "#" & KeyValues.Rows(e.Row.Index).Cells(0).Value.ToString & "#")
                'Prof.DeleteProfile(KeyPath, KeyValues.Rows(e.Row.Index).Cells(0).Value.ToString)
                'Values = Prof.EnumProfile(KeyPath) 'Refresh the values from the profile store
                Action = ActionType.Delete
                SubKeyPath = Mid(KeyTree.SelectedNode.FullPath, InStr(KeyTree.SelectedNode.FullPath, "\"))
                Prof.DeleteKey(SubKeyPath)
                KeyTree.SelectedNode.Remove()
                RefreshKeyValues(KeyTree.SelectedNode.FullPath)

            Else
                'e.Cancel = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Sub mnuRenameClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mnuRename.MouseUp
        Try
            Action = ActionType.Rename
            KeyTree.LabelEdit = True
            KeyTree.SelectedNode.BeginEdit()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Sub mnuNewKeyClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mnuNewKey.MouseUp
        Const NEW_KEY_NAME As String = "New Key 1"
        Dim NewNode As New TreeNode(NEW_KEY_NAME)
        Try
            NewNode.Name = NEW_KEY_NAME
            NewNode.Text = NEW_KEY_NAME
            Action = ActionType.Add
            KeyTree.SelectedNode.Nodes.Add(NewNode)
            KeyTree.SelectedNode = NewNode
            KeyTree.LabelEdit = True
            NewNode.BeginEdit()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

#End Region

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        End
    End Sub

    Private Sub mnuRootEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRootEdit.Click
        Dim Settings As New UtilitiesSettings
        mnuRootEdit.Checked = Not mnuRootEdit.Checked 'Reverse the checked state
        RefreshKeyValues() 'Refresh current key values
        Settings.ProfileRootEdit = mnuRootEdit.Checked
        Settings.Dispose()
        Settings = Nothing
    End Sub
End Class
