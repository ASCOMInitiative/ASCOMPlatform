Imports System.IO
Imports System.Reflection
Public Class frmProfileExplorer
    Private RecursionLevel As Integer
    Private Values As Generic.SortedList(Of String, String) 'Variable to hold current key values
    Private KeyPath As String ' Profile path to the current key
    Private Prof As RegistryAccess
    Private Settings As UtilitiesSettings
    Private CurrentSubKey As String
    Private SelectedFullPath As String
    Private Action As ActionType

    Private Enum ActionType As Integer
        None
        Add
        Delete
        Rename
    End Enum

    Private Const ROOT_NAME As String = "Profile Root"
    Private Const TOOLTIP_ROOT_RO As String = "The Profile root is read-only for safety, use Options to enable write access"
    Private Const TOOLTIP_ROOT_RW As String = "The Profile root is writable, please be very careful!"
    Private Const REGISTRY_DEFAULT As String = "(Default)"

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MyAssVer As Version, snd As Object, args As TreeNodeMouseClickEventArgs

        Try
            MyAssVer = Assembly.GetExecutingAssembly.GetName.Version
            Me.Text = "Profile Explorer " & MyAssVer.ToString

            Prof = New RegistryAccess()
            Settings = New UtilitiesSettings 'Read the state of the edit flag
            mnuRootEdit.Checked = Settings.ProfileRootEdit
            Settings.Dispose()
            Settings = Nothing

            RecursionLevel = 0
            ProcessTree("", Nothing, -1) 'Process the directory tree starting at the base point = root node

            KeyTree.ShowNodeToolTips = True

            snd = Nothing
            args = New TreeNodeMouseClickEventArgs(KeyTree.Nodes(0), Windows.Forms.MouseButtons.Left, 1, 0, 0)
            KeyTree_NodeMouseClick(snd, args)
            KeyTree.TopNode.Expand()
            SelectedFullPath = ROOT_NAME
            KeyValues.Columns(0).Width = 200
        Catch ex As Exception
            LogException("Form1_Load", ex)
        End Try
    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        KeyValues.Width = Me.Width - 350
        KeyValues.Height = Me.Height - 65
        KeyTree.Height = Me.Height - 65
        KeyValues_ColumnWidthChanged(Nothing, Nothing)
    End Sub

    Private Sub KeyValues_ColumnWidthChanged(ByVal sender As Object, ByVal e As DataGridViewColumnEventArgs) Handles KeyValues.ColumnWidthChanged
        Dim ColumnWidth As Integer
        'ColumnWidth = CInt(KeyValues.Width - 60) \ 2
        'KeyValues.Columns(0).Width = ColumnWidth
        'KeyValues.Columns(1).Width = KeyValues.Width - ColumnWidth - 60

        ColumnWidth = CInt(KeyValues.Width - 60) \ 2
        'KeyValues.Columns(0).Width = ColumnWidth
        KeyValues.Columns(1).Width = KeyValues.Width - KeyValues.Columns(0).Width - 60


        'MsgBox("Cwidth")
    End Sub


    Private Sub ExpandedNodesGet(ByVal ThisNode As TreeNode, ByRef ExpandedNodes As Generic.List(Of String))
        For Each Nod As TreeNode In ThisNode.Nodes
            'MsgBox(Nod.FullPath)
            If Nod.IsSelected Then SelectedFullPath = Nod.FullPath
            If Nod.IsExpanded Then
                ExpandedNodes.Add(Nod.FullPath)
            End If
            ExpandedNodesGet(Nod, ExpandedNodes) 'Recursively process the node tree
        Next
    End Sub

    Private Sub ExpandedNodesSet(ByVal ThisNode As TreeNode, ByVal ExpandedNodes As Generic.List(Of String))
        For Each Nod As TreeNode In ThisNode.Nodes
            'MsgBox(Nod.FullPath)
            If Nod.FullPath = SelectedFullPath Then KeyTree.SelectedNode = Nod
            If ExpandedNodes.Contains(Nod.FullPath) Then
                Nod.Expand()
            End If
            ExpandedNodesSet(Nod, ExpandedNodes) 'Recursively process the node tree
        Next
    End Sub

    Private Sub KeyTree_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles KeyTree.KeyUp
        Dim e1 As Windows.Forms.MouseEventArgs
        Dim ExpandedNodes As Generic.List(Of String)
        Dim RootIsSelected As Boolean

        Try
            e1 = New Windows.Forms.MouseEventArgs(Windows.Forms.MouseButtons.Left, 0, 0, 0, 0)
            ExpandedNodes = New Generic.List(Of String)
            RootIsSelected = False

            Select Case e.KeyData
                Case Keys.F2
                    mnuRenameKey_MouseUp(sender, e1)
                    e.Handled = True
                Case Keys.F5

                    If KeyTree.TopNode.IsSelected Then RootIsSelected = True
                    ExpandedNodesGet(KeyTree.Nodes(0), ExpandedNodes)
                    For Each Path As String In ExpandedNodes
                        'MsgBox(Path)
                    Next

                    KeyTree.Nodes.Clear()
                    ProcessTree("", Nothing, -1) 'Process the directory tree starting at the base point = root node
                    Dim snd As Object, args As New TreeNodeMouseClickEventArgs(KeyTree.Nodes(0), Windows.Forms.MouseButtons.Left, 1, 0, 0)
                    snd = Nothing
                    KeyTree.ShowNodeToolTips = True
                    KeyTree_NodeMouseClick(snd, args)
                    KeyTree.TopNode.Expand()
                    ExpandedNodesSet(KeyTree.Nodes(0), ExpandedNodes)
                    If RootIsSelected Then
                        KeyTree.SelectedNode = KeyTree.TopNode
                        SelectedFullPath = ROOT_NAME
                    End If
                    RefreshKeyValues(SelectedFullPath)
                Case Keys.Delete
                    mnuDeleteKey_MouseUp(sender, e1)
                Case Keys.Insert
                    mnuNewKey_MouseUp(sender, e1)
            End Select
        Catch ex As Exception
            LogException("KeyTree_KeyUp", ex)
        End Try
    End Sub

    Private Sub LogException(ByVal Caller As String, ByVal ex As Exception)
        EventLogCode.LogEvent("Profile Explorer", Caller & " exception", EventLogEntryType.Error, EventLogErrors.ProfileExplorerException, ex.ToString)
        MsgBox(Caller & " Exception, please run ASCOM Diagnostics and report this on ASCOM Talk: " & ex.ToString)
    End Sub

    Private Sub KeyTree_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles KeyTree.NodeMouseClick
        Try
            RefreshKeyValues(e.Node.FullPath)
        Catch ex As Exception
            LogException("KeyTree_NodeMouseClick", ex)
        End Try
    End Sub

    Private Sub KeyValues_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles KeyValues.CellValueChanged
        Try
            If KeyValues.IsCurrentRowDirty Then 'Commit value back to the profile store
                Select Case e.ColumnIndex
                    Case 0 'Value name has changed

                        Try
                            If KeyValues.CurrentRow.Cells(1).Value.ToString = "" Then : End If ' Test whether the value is null and condition to empty string
                        Catch ex As Exception
                            KeyValues.CurrentRow.Cells(1).Value = ""
                        End Try

                        ' Turn the (Default) key name into empty string
                        If KeyValues.CurrentRow.Cells(0).Value.ToString = REGISTRY_DEFAULT Then
                            KeyValues.CurrentRow.Cells(0).Value = ""
                        End If

                        'MsgBox("Value name changed " & _
                        '       e.RowIndex.ToString & " " & _
                        '       KeyPath & "#" & _
                        '       KeyValues.CurrentCell.Value.ToString & "#" & _
                        '       Values.Keys(e.RowIndex).ToString & "#")
                        Try : Prof.WriteProfile(KeyPath, KeyValues.CurrentRow.Cells(0).Value.ToString, KeyValues.CurrentRow.Cells(1).Value.ToString) : Catch : End Try   'Create new value
                        If e.RowIndex <= (Values.Count - 1) Then Prof.DeleteProfile(KeyPath, Values.Keys(e.RowIndex)) 'Delete old value if not a new row

                        'Make the last row value readonly if its data name is null or empty, i.e. hasn't been filled out yet

                        KeyValues.CurrentRow.Cells(1).ReadOnly = False
                        KeyValues.CurrentRow.Cells(1).Style.BackColor = Color.White
                        'RefreshKeyValues()
                    Case 1 'Value data has changed
                        'Write new value back to the profile

                        Try
                            If KeyValues.CurrentRow.Cells(0).Value.ToString = REGISTRY_DEFAULT Then
                                KeyValues.CurrentRow.Cells(0).Value = ""
                            End If
                        Catch ex As Exception
                            'MsgBox(ex.Message)
                            KeyValues.CurrentRow.Cells(0).Value = ""
                        End Try
                        ' Turn the (Default) key name into empty string

                        If KeyValues.CurrentCell.Value Is Nothing Then 'Guard against value deleted, in which case create an empty string
                            KeyValues.CurrentCell.Value = ""
                        End If
                        'Prof.WriteProfile(KeyPath, Values.Keys(e.RowIndex), KeyValues.CurrentCell.Value.ToString)
                        Prof.WriteProfile(KeyPath, KeyValues.CurrentRow.Cells(0).Value.ToString, KeyValues.CurrentRow.Cells(1).Value.ToString)
                        'MsgBox("Value data changed " & e.ColumnIndex.ToString & " " & e.RowIndex.ToString & " " & KeyPath & " " & Values.Keys(e.RowIndex) & " " & KeyValues.CurrentCell.Value.ToString)
                        'Values = Prof.EnumProfile(KeyPath) 'Refresh the values from the profile store
                        RefreshKeyValues()
                End Select
            End If
        Catch ex As Exception
            LogException("KeyValues_CellValueChanged", ex)
        End Try
    End Sub

    Private Sub KeyValues_UserDeletingRow(ByVal sender As Object, ByVal e As DataGridViewRowCancelEventArgs) Handles KeyValues.UserDeletingRow
        Dim Res As MsgBoxResult
        Res = MsgBox("Are you sure you want to delete this row?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Exclamation, "Confirm delete")
        If Res = MsgBoxResult.Ok Then
            Try
                MsgBox("Deleting: " & KeyPath & "#" & KeyValues.Rows(e.Row.Index).Cells(0).Value.ToString & "#")
                If KeyValues.Rows(e.Row.Index).Cells(0).Value.ToString = REGISTRY_DEFAULT Then
                    Prof.DeleteProfile(KeyPath, "")
                Else
                    Prof.DeleteProfile(KeyPath, KeyValues.Rows(e.Row.Index).Cells(0).Value.ToString)
                End If

                Values = Prof.EnumProfile(KeyPath) 'Refresh the values from the profile store
                RefreshKeyValues()

            Catch ex As Exception
                LogException("KeyValues_UserDeletingRow", ex)
            End Try
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub ProcessTree(ByVal KeyName As String, ByVal CurrentNode As TreeNode, ByVal NodeNumber As Integer)
        Const MAX_PARTS As Integer = 100 ' Maximum number of string
        Dim DirNum, MyNodeNumber As Integer
        Dim KeyNameParts() As String
        Dim SubKeys As New Generic.SortedList(Of String, String)

        Dim NewNode As New TreeNode
        MyNodeNumber = NodeNumber + 1
        RecursionLevel += 1
        KeyTree.BeginUpdate()

        If KeyName = "" Then
            NewNode.Name = ROOT_NAME
            NewNode.Text = ROOT_NAME
            NewNode.ToolTipText = "Root node can not be deleted or renamed"
            KeyTree.Nodes.Add(NewNode)
        Else
            KeyNameParts = KeyName.Split("\".ToCharArray, MAX_PARTS)
            NewNode.Name = KeyNameParts(KeyNameParts.GetUpperBound(0))
            NewNode.Text = KeyNameParts(KeyNameParts.GetUpperBound(0))
            CurrentNode.Nodes.Add(NewNode)
        End If

        KeyTree.EndUpdate()
        KeyTree.Refresh()

        Try
            SubKeys = Prof.EnumKeys(KeyName)
        Catch ex As Exception
            LogException("ProcessTree SubKeys", ex)
        End Try

        DirNum = -1
        For Each SubKey As Generic.KeyValuePair(Of String, String) In SubKeys
            Try
                If CurrentNode Is Nothing Then
                    ProcessTree(SubKey.Key, KeyTree.Nodes(0), DirNum)
                Else
                    ProcessTree(KeyName & "\" & SubKey.Key, CurrentNode.Nodes(MyNodeNumber), DirNum)
                End If
                DirNum += 1
            Catch ex As Exception
                LogException("ProcessTree Error accessing: """ & SubKey.Key & """", ex)
            End Try
        Next
        RecursionLevel -= 1
    End Sub

    Private Overloads Sub RefreshKeyValues()
        RefreshKeyValues(CurrentSubKey)
    End Sub

    Private Overloads Sub RefreshKeyValues(ByVal SubKey As String)
        Dim CurrentRow As Integer = 0, CurrentCell As Integer = 0, DoRefresh As Boolean

        Try
            CurrentRow = KeyValues.CurrentRow.Index
            CurrentCell = KeyValues.CurrentCell.ColumnIndex
        Catch ex As NullReferenceException
            CurrentRow = 0
            CurrentCell = 0
        End Try
        DoRefresh = True
        Try
            If (CurrentRow >= Values.Count) And (CurrentCell = 0) Then
                If KeyValues.IsCurrentCellDirty Then DoRefresh = False
            End If
        Catch ex As Exception

        End Try

        If DoRefresh Then
            Try
                CurrentSubKey = SubKey 'Save current value so that the display can be refreshed whenever required
                KeyValues.Rows.Clear()
                KeyPath = Mid(SubKey, Len(ROOT_NAME) + 1)
                'If Microsoft.VisualBasic.Left(KeyPath, 1) <> "\" Then KeyPath = "\" & KeyPath
                If Microsoft.VisualBasic.Left(KeyPath, 1) = "\" Then KeyPath = Mid(KeyPath, 2)
                Try
                    Values = Prof.EnumProfile(KeyPath)
                Catch ex As NullReferenceException ' Ignore keys that no longer exist - they must have been modified outside this application so we just have to live with it!
                    Values = New Generic.SortedList(Of String, String)
                End Try
                If Not Values.ContainsKey("") Then
                    Values.Add("", "")
                End If

                For Each kvp In Values
                    If kvp.Key = "" Then
                        KeyValues.Rows.Add(REGISTRY_DEFAULT, kvp.Value)
                    Else
                        KeyValues.Rows.Add(kvp.Key, kvp.Value)
                    End If

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
                KeyValues.Rows(0).Cells(0).ReadOnly = True
                If (SubKey = ROOT_NAME) And (mnuRootEdit.Checked = False) Then
                    KeyValues.Rows(0).Cells(0).ToolTipText = TOOLTIP_ROOT_RO
                    KeyValues.Rows(0).Cells(1).ToolTipText = TOOLTIP_ROOT_RO
                    KeyValues.ShowCellToolTips = True
                Else
                    If SubKey = ROOT_NAME Then
                        KeyValues.Rows(0).Cells(0).ToolTipText = TOOLTIP_ROOT_RW
                        KeyValues.Rows(0).Cells(1).ToolTipText = TOOLTIP_ROOT_RW
                    End If
                End If


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
                If CurrentRow > (KeyValues.RowCount - 2) Then
                    CurrentRow = KeyValues.RowCount - 2
                End If
                KeyValues.CurrentCell = KeyValues.Rows(CurrentRow).Cells(CurrentCell)


                KeyValues.Rows(0).Cells(0).ReadOnly = True
                KeyValues.Rows(0).Cells(0).Style.BackColor = Color.Crimson

                'Make the last row value readonly if its data name is null or empty, i.e. hasn't been filled out yet
                Try
                    If String.IsNullOrEmpty(KeyValues.Rows(KeyValues.RowCount - 1).Cells(0).Value.ToString) Then
                        KeyValues.Rows(KeyValues.RowCount - 1).Cells(1).ReadOnly = True
                        KeyValues.Rows(KeyValues.RowCount - 1).Cells(1).Style.BackColor = Color.Crimson
                    Else
                        KeyValues.Rows(KeyValues.RowCount - 1).Cells(1).ReadOnly = False
                        KeyValues.Rows(KeyValues.RowCount - 1).Cells(1).Style.BackColor = Color.White
                    End If
                Catch ex As Exception
                    KeyValues.Rows(KeyValues.RowCount - 1).Cells(1).ReadOnly = True
                    KeyValues.Rows(KeyValues.RowCount - 1).Cells(1).Style.BackColor = Color.Crimson
                End Try
            Catch ex As System.InvalidOperationException
                'Ignore these re-entrant error messages
            Catch ex As Exception
                LogException("RefreshKeyValues """ & SubKey & """", ex)
            End Try
        End If
    End Sub

    Private Sub KeyValues_KeyUp(ByVal Sender As Object, ByVal KeyPress As KeyEventArgs) Handles KeyValues.KeyUp
        Dim CurrentRow, CurrentCell As Integer

        Try
            Select Case KeyPress.KeyCode
                Case Keys.F5
                    CurrentRow = KeyValues.CurrentRow.Index
                    CurrentCell = KeyValues.CurrentCell.ColumnIndex
                    Values = Prof.EnumProfile(KeyPath) 'Refresh the values from the profile store
                    RefreshKeyValues()
                    If CurrentRow > (KeyValues.RowCount - 2) Then
                        CurrentRow = KeyValues.RowCount - 2
                    End If
                    KeyValues.CurrentCell = KeyValues.Rows(CurrentRow).Cells(CurrentCell)
                Case Else
                    'Do nothing
            End Select
        Catch ex As Exception
            LogException("KeyValues_KeyUp", ex)
        End Try

    End Sub

#Region "Values Right Click"

    Private Sub KeyValues_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles KeyValues.MouseUp
        Dim pt As Point
        Dim xpos, ypos As Integer
        Try
            Select Case e.Button
                Case MouseButtons.Right
                    Try
                        pt = New Point(e.X, e.Y)
                        KeyValues.PointToClient(pt)

                        ypos = CInt((pt.Y + KeyValues.VerticalScrollingOffset - 20) \ KeyValues.Rows(0).Height)
                        xpos = CInt((pt.X - 40) \ KeyValues.Columns(0).Width)
                        'MsgBox(pt.Y & " " & dgv.Rows(0).Height & " " & ypos)
                        'MsgBox(xpos & " " & ypos)
                        KeyValues.CurrentCell = KeyValues(xpos, ypos)

                    Catch ex As Exception
                    End Try
                    mnuRenameValue.Text = "Edit Value"
                    mnuEditData.Text = "Edit Data"
                    mnuEditData.Enabled = True
                    mnuClearData.Text = "Clear Data"
                    Select Case xpos
                        Case 0
                            mnuNewValue.Visible = True
                            mnuRenameValue.Visible = True
                            mnuDeleteValue.Visible = True
                            mnuValueSeparator.Visible = True
                            mnuEditData.Visible = False
                            mnuClearData.Visible = False
                        Case Else
                            mnuNewValue.Visible = True
                            mnuRenameValue.Visible = False
                            mnuDeleteValue.Visible = True
                            mnuValueSeparator.Visible = True
                            mnuEditData.Visible = True
                            mnuClearData.Visible = True
                    End Select
                    If ypos >= KeyValues.RowCount - 1 Then
                        mnuNewValue.Visible = False
                        mnuDeleteValue.Visible = False
                        mnuClearData.Visible = False
                        mnuValueSeparator.Visible = False
                        mnuRenameValue.Text = "Enter New Value"
                        Try
                            If String.IsNullOrEmpty(KeyValues.CurrentRow.Cells(0).Value.ToString) Then
                                mnuEditData.Text = "Please enter Value first"
                                mnuEditData.Enabled = False
                            Else
                                mnuEditData.Text = "Enter New Data"
                            End If
                        Catch ex As Exception
                            mnuEditData.Text = "Please enter Value first"
                            mnuEditData.Enabled = False
                        End Try
                    End If
                    mnuCtxValues.Show(KeyValues, New Point(e.X, e.Y)) ' Display the context menu at the mouse position
                Case Else
            End Select
        Catch ex As Exception
            LogException("KeyValues_MouseUp", ex)
        End Try
    End Sub

    Private Sub mnuNewValue_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mnuNewValue.MouseUp
        Try
            KeyValues.CurrentCell = KeyValues.Rows(KeyValues.RowCount - 1).Cells(0)
            KeyValues.BeginEdit(True)
        Catch ex As Exception
            LogException("mnuNewValue_MouseUp", ex)
        End Try
    End Sub

    Private Sub mnuEditData_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mnuEditData.MouseUp
        Try
            KeyValues.BeginEdit(True)
        Catch ex As Exception
            LogException("mnuEditData_MouseUp", ex)
        End Try

    End Sub

    Private Sub mnuRenameValue_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mnuRenameValue.MouseUp
        Try
            KeyValues.BeginEdit(True)
        Catch ex As Exception
            LogException("mnuRenameValue_MouseUp", ex)
        End Try

    End Sub

    Private Sub mnuClearData_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mnuClearData.MouseUp
        Dim Res As MsgBoxResult, EventArgs As DataGridViewCellEventArgs
        Try
            Res = MsgBox("Are you sure you want to clear this data?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Exclamation, "Confirm delete")
            If Res = MsgBoxResult.Ok Then
                ' MsgBox(KeyValues.CurrentRow.Index & " " & 0 & " " & KeyValues.CurrentRow.Cells(0).Value.ToString)
                KeyValues.CurrentCell.Value = ""
                EventArgs = New DataGridViewCellEventArgs(KeyValues.CurrentCell.ColumnIndex, KeyValues.CurrentRow.Index)
                Me.KeyValues_CellValueChanged(New Object, EventArgs)
            Else
                'Do nothing
            End If
        Catch ex As Exception
            LogException("mnuClearData_MouseUp", ex)
        End Try

    End Sub

    Private Sub mnuDeleteValue_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mnuDeleteValue.MouseUp
        Dim Res As MsgBoxResult

        Try
            Res = MsgBox("Are you sure you want to delete this row?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Exclamation, "Confirm delete")
            If Res = MsgBoxResult.Ok Then
                ' MsgBox(KeyValues.CurrentRow.Index & " " & 0 & " " & KeyValues.CurrentRow.Cells(0).Value.ToString)
                Try
                    If KeyValues.CurrentRow.Cells(0).Value.ToString = REGISTRY_DEFAULT Then
                        Prof.DeleteProfile(KeyPath, "")
                    Else
                        Prof.DeleteProfile(KeyPath, KeyValues.CurrentRow.Cells(0).Value.ToString)
                    End If
                Catch ex As Exception
                    LogException("mnuDeleteValue_MouseUp KeyValues", ex)
                End Try
                Values = Prof.EnumProfile(KeyPath) 'Refresh the values from the profile store
                RefreshKeyValues()
            Else
                'Do nothing
            End If
        Catch ex As Exception
            LogException("mnuDeleteValue_MouseUp Overall", ex)
        End Try
    End Sub

#End Region

#Region "Keys TreeView Right Click"
    'Examples from: http://www.eggheadcafe.com/tutorials/aspnet/847ac120-3cdc-4249-8029-26c15de209d1/net-treeview-faq--drag.aspx 'Author: Robbie Morris Article title: .NET TreeView FAQ - Drag and Drop Right Click Menu

    Private Sub KeyTree_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles KeyTree.AfterLabelEdit
        Const NOT_FOUND As Integer = -1
        Dim ct As Integer

        Try
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
        Catch ex As Exception
            LogException("KeyTree_AfterLabelEdit", ex)
        End Try
    End Sub

    Private Sub KeyTree_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles KeyTree.MouseUp
        Try
            Select Case e.Button
                Case MouseButtons.Right
                    'CtxMenu.Show(KeyTree, New Point(e.X, e.Y))
                    SetSelectedNodeByPosition(KeyTree, e.X, e.Y)
                    If Not (KeyTree.SelectedNode Is Nothing) Then
                        If KeyTree.SelectedNode.FullPath = ROOT_NAME Then 'Prevent delete or rename of reoot node
                            mnuDeleteKey.Enabled = False
                            mnuRenameKey.Enabled = False
                            mnuNewKey.Enabled = True
                        Else
                            mnuDeleteKey.Enabled = True
                            mnuRenameKey.Enabled = True
                            mnuNewKey.Enabled = True
                        End If
                    Else
                        mnuDeleteKey.Enabled = False
                        mnuRenameKey.Enabled = False
                        mnuNewKey.Enabled = False
                    End If
                    mnuCtxKeys.Show(KeyTree, New Point(e.X, e.Y))
                Case Else
            End Select
        Catch ex As Exception
            LogException("KeyTree_MouseUp", ex)
        End Try

    End Sub

    Private Sub KeyCreate(ByVal Node As TreeNode)
        Dim SubKeyPath As String
        'MsgBox(Node.FullPath)
        Node.Name = Node.Text 'Set the node name to be the same as the label
        SubKeyPath = Mid(Node.FullPath, InStr(Node.FullPath, "\"))
        Prof.CreateKey(SubKeyPath)
        RefreshKeyValues(Node.FullPath)
    End Sub

    Private Sub KeyRename(ByVal Node As TreeNode, ByVal NewName As String)
        'MsgBox("Rename: " & Node.FullPath & " " & NewName)
        Prof.RenameKey(Mid(Node.FullPath, Len(ROOT_NAME) + 1), Mid(Node.Parent.FullPath, Len(ROOT_NAME) + 1) & "\" & NewName)
        Node.Name = NewName
        Node.Text = NewName
        RefreshKeyValues(Node.FullPath)
    End Sub

    Private Sub SetSelectedNodeByPosition(ByVal tv As TreeView, ByVal mouseX As Integer, ByVal mouseY As Integer)
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
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub RightClickAdd(ByVal sender As Object, ByVal e As System.EventArgs)
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
            LogException("RightClickAdd", ex)
        End Try
    End Sub

    Private Sub mnuDeleteKey_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mnuDeleteKey.MouseUp
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
            LogException("mnuDeleteKey_MouseUp", ex)
        End Try
    End Sub

    Private Sub mnuRenameKey_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mnuRenameKey.MouseUp
        Try
            Action = ActionType.Rename
            KeyTree.LabelEdit = True
            KeyTree.SelectedNode.BeginEdit()
        Catch ex As Exception
            LogException("mnuRenameKey_MouseUp", ex)
        End Try
    End Sub

    Private Sub mnuNewKey_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mnuNewKey.MouseUp
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
            LogException("mnuNewKey_MouseUp", ex)
        End Try
    End Sub

#End Region

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        End
    End Sub

    Private Sub mnuRootEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRootEdit.Click
        Dim Settings As UtilitiesSettings

        Try
            Settings = New UtilitiesSettings
            mnuRootEdit.Checked = Not mnuRootEdit.Checked 'Reverse the checked state
            RefreshKeyValues() 'Refresh current key values
            Settings.ProfileRootEdit = mnuRootEdit.Checked
            Settings.Dispose()
            Settings = Nothing
        Catch ex As Exception
            LogException("mnuRootEdit_Click", ex)
        End Try
    End Sub

    Private Sub mnuAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAbout.Click
        Dim About As AboutBox

        Try
            About = New AboutBox
            About.ShowDialog()
            About.Close()
            About.Dispose()
            About = Nothing
        Catch ex As Exception
            LogException("mnuAbout_Click", ex)
        End Try

    End Sub
End Class
