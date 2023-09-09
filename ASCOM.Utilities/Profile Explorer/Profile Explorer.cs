using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace ASCOM.Utilities
{

    public partial class frmProfileExplorer
    {
        private int RecursionLevel;
        private SortedList<string, string> Values; // Variable to hold current key values
        private string KeyPath; // Profile path to the current key
        private RegistryAccess Prof;
        private UtilitiesSettings Settings;
        private string CurrentSubKey;
        private string SelectedFullPath;
        private ActionType Action;

        private enum ActionType : int
        {
            None,
            Add,
            Delete,
            Rename
        }

        private const string ROOT_NAME = "Profile Root";
        private const string TOOLTIP_ROOT_RO = "The Profile root is read-only for safety, use Options to enable write access";
        private const string TOOLTIP_ROOT_RW = "The Profile root is writeable, please be very careful!";
        private const string REGISTRY_DEFAULT = "(Default)";

        private const string NEW_KEY_NAME = "New Key 1";

        public frmProfileExplorer()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Version MyAssVer;
            object snd;
            TreeNodeMouseClickEventArgs args;

            try
            {
                MyAssVer = Assembly.GetExecutingAssembly().GetName().Version;
                Text = "Profile Explorer " + MyAssVer.ToString();

                Prof = new RegistryAccess();
                Settings = new UtilitiesSettings(); // Read the state of the edit flag
                mnuRootEdit.Checked = Settings.ProfileRootEdit;
                Settings.Dispose();
                Settings = (UtilitiesSettings)null;

                RecursionLevel = 0;
                ProcessTree("", null, -1); // Process the directory tree starting at the base point = root node

                KeyTree.ShowNodeToolTips = true;

                snd = null;
                args = new TreeNodeMouseClickEventArgs(KeyTree.Nodes[0], MouseButtons.Left, 1, 0, 0);
                KeyTree_NodeMouseClick(snd, args);
                KeyTree.TopNode.Expand();
                SelectedFullPath = ROOT_NAME;
                KeyValues.Columns[0].Width = 200;
            }
            catch (Exception ex)
            {
                LogException("Form1_Load", ex);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            KeyValues.Width = Width - 350;
            KeyValues.Height = Height - 65;
            KeyTree.Height = Height - 65;
            KeyValues_ColumnWidthChanged(null, null);
        }

        private void KeyValues_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            int ColumnWidth;

            ColumnWidth = (KeyValues.Width - 60) / 2;
            KeyValues.Columns[1].Width = KeyValues.Width - KeyValues.Columns[0].Width - 60;
        }

        private void ExpandedNodesGet(TreeNode ThisNode, ref List<string> ExpandedNodes)
        {
            foreach (TreeNode Nod in ThisNode.Nodes)
            {
                if (Nod.IsSelected)
                    SelectedFullPath = Nod.FullPath;
                if (Nod.IsExpanded)
                {
                    ExpandedNodes.Add(Nod.FullPath);
                }
                ExpandedNodesGet(Nod, ref ExpandedNodes); // Recursively process the node tree
            }
        }

        private void ExpandedNodesSet(TreeNode ThisNode, List<string> ExpandedNodes)
        {
            foreach (TreeNode Nod in ThisNode.Nodes)
            {
                if ((Nod.FullPath ?? "") == (SelectedFullPath ?? ""))
                    KeyTree.SelectedNode = Nod;
                if (ExpandedNodes.Contains(Nod.FullPath))
                {
                    Nod.Expand();
                }
                ExpandedNodesSet(Nod, ExpandedNodes); // Recursively process the node tree
            }
        }

        private void KeyTree_KeyUp(object sender, KeyEventArgs e)
        {
            MouseEventArgs e1;
            List<string> ExpandedNodes;
            bool RootIsSelected;

            try
            {
                e1 = new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0);
                ExpandedNodes = new List<string>();
                RootIsSelected = false;

                switch (e.KeyData)
                {
                    case Keys.F2:
                        {
                            mnuRenameKey_MouseUp(sender, e1);
                            e.Handled = true;
                            break;
                        }
                    case Keys.F5:
                        {

                            if (KeyTree.TopNode.IsSelected)
                                RootIsSelected = true;
                            ExpandedNodesGet(KeyTree.Nodes[0], ref ExpandedNodes);

                            KeyTree.Nodes.Clear();
                            ProcessTree("", null, -1); // Process the directory tree starting at the base point = root node
                            object snd;
                            var args = new TreeNodeMouseClickEventArgs(KeyTree.Nodes[0], MouseButtons.Left, 1, 0, 0);
                            snd = null;
                            KeyTree.ShowNodeToolTips = true;
                            KeyTree_NodeMouseClick(snd, args);
                            KeyTree.TopNode.Expand();
                            ExpandedNodesSet(KeyTree.Nodes[0], ExpandedNodes);
                            if (RootIsSelected)
                            {
                                KeyTree.SelectedNode = KeyTree.TopNode;
                                SelectedFullPath = ROOT_NAME;
                            }
                            RefreshKeyValues(SelectedFullPath);
                            break;
                        }
                    case Keys.Delete:
                        {
                            mnuDeleteKey_MouseUp(sender, e1);
                            break;
                        }
                    case Keys.Insert:
                        {
                            mnuNewKey_MouseUp(sender, e1);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                LogException("KeyTree_KeyUp", ex);
            }
        }

        private void LogException(string Caller, Exception ex)
        {
            Utilities.Global.LogEvent("Profile Explorer", Caller + " exception", EventLogEntryType.Error, Global.EventLogErrors.ProfileExplorerException, ex.ToString());
            MessageBox.Show(Caller + " Exception, please run ASCOM Diagnostics and report this on ASCOM Talk: " + ex.ToString());
        }

        private void KeyTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                RefreshKeyValues(e.Node.FullPath);
            }
            catch (Exception ex)
            {
                LogException("KeyTree_NodeMouseClick", ex);
            }
        }

        private void KeyValues_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (KeyValues.IsCurrentRowDirty) // Commit value back to the profile store
                {
                    switch (e.ColumnIndex)
                    {
                        case 0: // Value name has changed
                            {
                                string oldValueName = Values.Keys[e.RowIndex];

                                try
                                {
                                    if (string.IsNullOrEmpty(KeyValues.CurrentRow.Cells[1].Value.ToString()))
                                    {
                                    } // Test whether the value is null and condition to empty string
                                }
                                catch (Exception ex)
                                {
                                    KeyValues.CurrentRow.Cells[1].Value = "";
                                }

                                // Turn the (Default) key name into empty string
                                if ((KeyValues.CurrentRow.Cells[0].Value.ToString() ?? "") == REGISTRY_DEFAULT)
                                {
                                    KeyValues.CurrentRow.Cells[0].Value = "";
                                }

                                try
                                {
                                    Prof.WriteProfile(KeyPath, KeyValues.CurrentRow.Cells[0].Value.ToString(), KeyValues.CurrentRow.Cells[1].Value.ToString());
                                }
                                catch
                                {
                                }

                                // Create new value

                                MessageBox.Show($"row index: {e.RowIndex}, Values.count: {Values.Count}, old value name: {oldValueName}");

                                if (e.RowIndex <= (Values.Count - 1))
                                {
                                    MessageBox.Show($"Deleting KeyPath: {KeyPath}, value being deleted: {oldValueName}");
                                    Prof.DeleteProfile(KeyPath, oldValueName); // Delete old value if not a new row
                                }
                                // Make the last row value read only if its data name is null or empty, i.e. hasn't been filled out yet

                                KeyValues.CurrentRow.Cells[1].ReadOnly = false;
                                KeyValues.CurrentRow.Cells[1].Style.BackColor = Color.White;
                                RefreshKeyValues();
                                break;
                            }
                        case 1: // Value data has changed
                            {
                                // Write new value back to the profile

                                try
                                {
                                    if ((KeyValues.CurrentRow.Cells[0].Value.ToString() ?? "") == REGISTRY_DEFAULT)
                                    {
                                        KeyValues.CurrentRow.Cells[0].Value = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    KeyValues.CurrentRow.Cells[0].Value = "";
                                }
                                // Turn the (Default) key name into empty string

                                if (KeyValues.CurrentCell.Value is null) // Guard against value deleted, in which case create an empty string
                                {
                                    KeyValues.CurrentCell.Value = "";
                                }
                                Prof.WriteProfile(KeyPath, KeyValues.CurrentRow.Cells[0].Value.ToString(), KeyValues.CurrentRow.Cells[1].Value.ToString());
                                RefreshKeyValues();
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                LogException("KeyValues_CellValueChanged", ex);
            }
        }

        private void KeyValues_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult Res;

            Res = MessageBox.Show("Are you sure you want to delete this row?", "Confirm delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (Res == DialogResult.OK)
            {
                try
                {
                    if ((KeyValues.Rows[e.Row.Index].Cells[0].Value.ToString() ?? "") == REGISTRY_DEFAULT)
                    {
                        Prof.DeleteProfile(KeyPath, "");
                    }
                    else
                    {
                        Prof.DeleteProfile(KeyPath, KeyValues.Rows[e.Row.Index].Cells[0].Value.ToString());
                    }

                    Values = Prof.EnumProfile(KeyPath); // Refresh the values from the profile store
                    RefreshKeyValues();
                }

                catch (Exception ex)
                {
                    LogException("KeyValues_UserDeletingRow", ex);
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void ProcessTree(string KeyName, TreeNode CurrentNode, int NodeNumber)
        {
            const int MAX_PARTS = 100; // Maximum number of string
            int DirNum, MyNodeNumber;
            string[] KeyNameParts;
            var SubKeys = new SortedList<string, string>();
            var NewNode = new TreeNode();

            MyNodeNumber = NodeNumber + 1;
            RecursionLevel += 1;
            KeyTree.BeginUpdate();

            if (string.IsNullOrEmpty(KeyName))
            {
                NewNode.Name = ROOT_NAME;
                NewNode.Text = ROOT_NAME;
                NewNode.ToolTipText = "Root node can not be deleted or renamed";
                KeyTree.Nodes.Add(NewNode);
            }
            else
            {
                KeyNameParts = KeyName.Split(@"\".ToCharArray(), MAX_PARTS);
                NewNode.Name = KeyNameParts[KeyNameParts.GetUpperBound(0)];
                NewNode.Text = KeyNameParts[KeyNameParts.GetUpperBound(0)];
                CurrentNode.Nodes.Add(NewNode);
            }

            KeyTree.EndUpdate();
            KeyTree.Refresh();

            try
            {
                SubKeys = Prof.EnumKeys(KeyName);
            }
            catch (Exception ex)
            {
                LogException("ProcessTree SubKeys", ex);
            }

            DirNum = -1;
            foreach (KeyValuePair<string, string> SubKey in SubKeys)
            {
                try
                {
                    if (CurrentNode is null)
                    {
                        ProcessTree(SubKey.Key, KeyTree.Nodes[0], DirNum);
                    }
                    else
                    {
                        ProcessTree(KeyName + @"\" + SubKey.Key, CurrentNode.Nodes[MyNodeNumber], DirNum);
                    }
                    DirNum += 1;
                }
                catch (Exception ex)
                {
                    LogException("ProcessTree Error accessing: \"" + SubKey.Key + "\"", ex);
                }
            }
            RecursionLevel -= 1;
        }

        private void RefreshKeyValues()
        {
            RefreshKeyValues(CurrentSubKey);
        }

        private void RefreshKeyValues(string SubKey)
        {
            int CurrentRow = 0;
            int CurrentCell = 0;
            bool DoRefresh;

            try
            {
                CurrentRow = KeyValues.CurrentRow.Index;
                CurrentCell = KeyValues.CurrentCell.ColumnIndex;
            }
            catch (NullReferenceException ex)
            {
                CurrentRow = 0;
                CurrentCell = 0;
            }
            DoRefresh = true;
            try
            {
                if (CurrentRow >= Values.Count & CurrentCell == 0)
                {
                    if (KeyValues.IsCurrentCellDirty)
                        DoRefresh = false;
                }
            }
            catch { }

            if (DoRefresh)
            {
                try
                {
                    CurrentSubKey = SubKey; // Save current value so that the display can be refreshed whenever required
                    KeyValues.Rows.Clear();

                    KeyPath = SubKey.Substring(ROOT_NAME.Length);
                    if (KeyPath.Length > 0)
                    {
                        if (KeyPath.Substring(0, 1) == @"\")
                            KeyPath = KeyPath.Substring(1);
                    }
                    try
                    {
                        Values = Prof.EnumProfile(KeyPath);
                    }
                    catch (NullReferenceException) // Ignore keys that no longer exist - they must have been modified outside this application so we just have to live with it!
                    {
                        Values = new SortedList<string, string>();
                    }
                    if (!Values.ContainsKey(""))
                    {
                        Values.Add("", "");
                    }

                    foreach (var kvp in Values)
                    {
                        if (string.IsNullOrEmpty(kvp.Key))
                        {
                            KeyValues.Rows.Add(REGISTRY_DEFAULT, kvp.Value);
                        }
                        else
                        {
                            KeyValues.Rows.Add(kvp.Key, kvp.Value);
                        }

                        if ((SubKey ?? "") == ROOT_NAME & mnuRootEdit.Checked == false)
                        {
                            KeyValues.Rows[KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 2].Cells[0].ToolTipText = TOOLTIP_ROOT_RO;
                            KeyValues.Rows[KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 2].Cells[1].ToolTipText = TOOLTIP_ROOT_RO;
                            KeyValues.ShowCellToolTips = true;
                        }
                        else if ((SubKey ?? "") == ROOT_NAME)
                        {
                            KeyValues.Rows[KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 2].Cells[0].ToolTipText = TOOLTIP_ROOT_RW;
                            KeyValues.Rows[KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 2].Cells[1].ToolTipText = TOOLTIP_ROOT_RW;
                        }
                    }
                    KeyValues.Rows[0].Cells[0].ReadOnly = true;
                    if ((SubKey ?? "") == ROOT_NAME & mnuRootEdit.Checked == false)
                    {
                        KeyValues.Rows[0].Cells[0].ToolTipText = TOOLTIP_ROOT_RO;
                        KeyValues.Rows[0].Cells[1].ToolTipText = TOOLTIP_ROOT_RO;
                        KeyValues.ShowCellToolTips = true;
                    }
                    else if ((SubKey ?? "") == ROOT_NAME)
                    {
                        KeyValues.Rows[0].Cells[0].ToolTipText = TOOLTIP_ROOT_RW;
                        KeyValues.Rows[0].Cells[1].ToolTipText = TOOLTIP_ROOT_RW;
                    }


                    if ((SubKey ?? "") == ROOT_NAME & mnuRootEdit.Checked == false)
                    {
                        KeyValues.Rows[KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 1].Cells[0].ToolTipText = TOOLTIP_ROOT_RO;
                        KeyValues.Rows[KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 1].Cells[1].ToolTipText = TOOLTIP_ROOT_RO;
                        KeyValues.ReadOnly = true;
                        KeyValues.BackgroundColor = Color.Crimson;
                        KeyValues.ShowCellToolTips = true;
                    }
                    else
                    {
                        if ((SubKey ?? "") == ROOT_NAME)
                        {
                            KeyValues.Rows[KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 1].Cells[0].ToolTipText = TOOLTIP_ROOT_RW;
                            KeyValues.Rows[KeyValues.Rows.GetRowCount(DataGridViewElementStates.Visible) - 1].Cells[1].ToolTipText = TOOLTIP_ROOT_RW;
                            KeyValues.BackgroundColor = Color.Chartreuse;
                            KeyValues.ShowCellToolTips = true;
                        }
                        else
                        {
                            KeyValues.BackgroundColor = Color.White;
                            KeyValues.ShowCellToolTips = false;
                        }
                        KeyValues.ReadOnly = false;
                    }
                    if (CurrentRow > KeyValues.RowCount - 2)
                    {
                        CurrentRow = KeyValues.RowCount - 2;
                    }
                    KeyValues.CurrentCell = KeyValues.Rows[CurrentRow].Cells[CurrentCell];


                    KeyValues.Rows[0].Cells[0].ReadOnly = true;
                    KeyValues.Rows[0].Cells[0].Style.BackColor = Color.Crimson;

                    // Make the last row value read only if its data name is null or empty, i.e. hasn't been filled out yet
                    try
                    {
                        if (string.IsNullOrEmpty(KeyValues.Rows[KeyValues.RowCount - 1].Cells[0].Value.ToString()))
                        {
                            KeyValues.Rows[KeyValues.RowCount - 1].Cells[1].ReadOnly = true;
                            KeyValues.Rows[KeyValues.RowCount - 1].Cells[1].Style.BackColor = Color.Crimson;
                        }
                        else
                        {
                            KeyValues.Rows[KeyValues.RowCount - 1].Cells[1].ReadOnly = false;
                            KeyValues.Rows[KeyValues.RowCount - 1].Cells[1].Style.BackColor = Color.White;
                        }
                    }
                    catch (Exception ex)
                    {
                        KeyValues.Rows[KeyValues.RowCount - 1].Cells[1].ReadOnly = true;
                        KeyValues.Rows[KeyValues.RowCount - 1].Cells[1].Style.BackColor = Color.Crimson;
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                }
                // Ignore these re-entrant error messages
                catch (Exception ex)
                {
                    LogException("RefreshKeyValues \"" + SubKey + "\"", ex);
                }
            }
        }

        private void KeyValues_KeyUp(object Sender, KeyEventArgs KeyPress)
        {
            int CurrentRow, CurrentCell;

            try
            {
                switch (KeyPress.KeyCode)
                {
                    case Keys.F5:
                        {
                            CurrentRow = KeyValues.CurrentRow.Index;
                            CurrentCell = KeyValues.CurrentCell.ColumnIndex;
                            Values = Prof.EnumProfile(KeyPath); // Refresh the values from the profile store
                            RefreshKeyValues();
                            if (CurrentRow > KeyValues.RowCount - 2)
                            {
                                CurrentRow = KeyValues.RowCount - 2;
                            }
                            KeyValues.CurrentCell = KeyValues.Rows[CurrentRow].Cells[CurrentCell];
                            break;
                        }

                    default:
                        {
                            break;
                        }
                        // Do nothing
                }
            }
            catch (Exception ex)
            {
                LogException("KeyValues_KeyUp", ex);
            }

        }

        #region Values Right Click

        private void KeyValues_MouseUp(object sender, MouseEventArgs e)
        {
            Point pt;
            int xpos = default, ypos = default;

            try
            {
                switch (e.Button)
                {
                    case MouseButtons.Right:
                        {
                            try
                            {
                                pt = new Point(e.X, e.Y);
                                KeyValues.PointToClient(pt);

                                ypos = (pt.Y + KeyValues.VerticalScrollingOffset - 20) / KeyValues.Rows[0].Height;
                                xpos = (pt.X - 40) / KeyValues.Columns[0].Width;
                                KeyValues.CurrentCell = KeyValues[xpos, ypos];
                            }

                            catch (Exception ex)
                            {
                            }
                            mnuRenameValue.Text = "Edit Value";
                            mnuEditData.Text = "Edit Data";
                            mnuEditData.Enabled = true;
                            mnuClearData.Text = "Clear Data";
                            switch (xpos)
                            {
                                case 0:
                                    {
                                        mnuNewValue.Visible = true;
                                        mnuRenameValue.Visible = true;
                                        mnuDeleteValue.Visible = true;
                                        mnuValueSeparator.Visible = true;
                                        mnuEditData.Visible = false;
                                        mnuClearData.Visible = false;
                                        break;
                                    }

                                default:
                                    {
                                        mnuNewValue.Visible = true;
                                        mnuRenameValue.Visible = false;
                                        mnuDeleteValue.Visible = true;
                                        mnuValueSeparator.Visible = true;
                                        mnuEditData.Visible = true;
                                        mnuClearData.Visible = true;
                                        break;
                                    }
                            }
                            if (ypos >= KeyValues.RowCount - 1)
                            {
                                mnuNewValue.Visible = false;
                                mnuDeleteValue.Visible = false;
                                mnuClearData.Visible = false;
                                mnuValueSeparator.Visible = false;
                                mnuRenameValue.Text = "Enter New Value";
                                try
                                {
                                    if (string.IsNullOrEmpty(KeyValues.CurrentRow.Cells[0].Value.ToString()))
                                    {
                                        mnuEditData.Text = "Please enter Value first";
                                        mnuEditData.Enabled = false;
                                    }
                                    else
                                    {
                                        mnuEditData.Text = "Enter New Data";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    mnuEditData.Text = "Please enter Value first";
                                    mnuEditData.Enabled = false;
                                }
                            }
                            mnuCtxValues.Show(KeyValues, new Point(e.X, e.Y)); // Display the context menu at the mouse position
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                LogException("KeyValues_MouseUp", ex);
            }
        }

        private void mnuNewValue_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                KeyValues.CurrentCell = KeyValues.Rows[KeyValues.RowCount - 1].Cells[0];
                KeyValues.BeginEdit(true);
            }
            catch (Exception ex)
            {
                LogException("mnuNewValue_MouseUp", ex);
            }
        }

        private void mnuEditData_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                KeyValues.BeginEdit(true);
            }
            catch (Exception ex)
            {
                LogException("mnuEditData_MouseUp", ex);
            }

        }

        private void mnuRenameValue_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                KeyValues.BeginEdit(true);
            }
            catch (Exception ex)
            {
                LogException("mnuRenameValue_MouseUp", ex);
            }

        }

        private void mnuClearData_MouseUp(object sender, MouseEventArgs e)
        {
            DialogResult Res;
            DataGridViewCellEventArgs EventArgs;

            try
            {
                Res = MessageBox.Show("Are you sure you want to clear this data?", "Confirm delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (Res == DialogResult.OK)
                {
                    KeyValues.CurrentCell.Value = "";
                    EventArgs = new DataGridViewCellEventArgs(KeyValues.CurrentCell.ColumnIndex, KeyValues.CurrentRow.Index);
                    KeyValues_CellValueChanged(new object(), EventArgs);
                }
                else
                {
                    // Do nothing
                }
            }
            catch (Exception ex)
            {
                LogException("mnuClearData_MouseUp", ex);
            }

        }

        private void mnuDeleteValue_MouseUp(object sender, MouseEventArgs e)
        {
            DialogResult Res;

            try
            {
                Res = MessageBox.Show("Are you sure you want to delete this row?", "Confirm delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (Res == DialogResult.OK)
                {
                    try
                    {
                        if ((KeyValues.CurrentRow.Cells[0].Value.ToString() ?? "") == REGISTRY_DEFAULT)
                        {
                            Prof.DeleteProfile(KeyPath, "");
                        }
                        else
                        {
                            Prof.DeleteProfile(KeyPath, KeyValues.CurrentRow.Cells[0].Value.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        LogException("mnuDeleteValue_MouseUp KeyValues", ex);
                    }
                    Values = Prof.EnumProfile(KeyPath); // Refresh the values from the profile store
                    RefreshKeyValues();
                }
                else
                {
                    // Do nothing
                }
            }
            catch (Exception ex)
            {
                LogException("mnuDeleteValue_MouseUp Overall", ex);
            }
        }

        #endregion

        #region Keys TreeView Right Click
        // Examples from: http://www.eggheadcafe.com/tutorials/aspnet/847ac120-3cdc-4249-8029-26c15de209d1/net-treeview-faq--drag.aspx 'Author: Robbie Morris Article title: .NET TreeView FAQ - Drag and Drop Right Click Menu

        private void KeyTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            const int NOT_FOUND = -1;
            int ct;

            try
            {
                if (!e.CancelEdit) // Check if user has cancelled edit
                {
                    if (e.Label is not null)
                    {
                        if (e.Label.Length > 0) // Check for label being empty string
                        {
                            if (e.Label.IndexOfAny(new char[] { '@', ',', '!' }) == NOT_FOUND) // Check for invalid characters
                            {
                                e.Node.EndEdit(false); // Stop editing without cancelling the label change.
                                                       // MsgBox("Action" & Action.ToString)
                                                       // Check whether the new key is duplicate of one already existing
                                switch (Action)
                                {
                                    case ActionType.Add:
                                        {
                                            // Add options:
                                            // 1) Edited name, but now edited back to same as original
                                            // Test: name and label are the same then confirm name only occurs once in list
                                            // 2) Edited name to different than original
                                            // Test: Name and label different so test if the name already exists in the collection

                                            if ((e.Node.Name ?? "") == (e.Label ?? "")) // It has been edited back to its original form
                                            {
                                                ct = 0;
                                                foreach (TreeNode TN in e.Node.Parent.Nodes)
                                                {
                                                    if ((TN.Name ?? "") == (e.Node.Name ?? ""))
                                                        ct += 1;
                                                }
                                                if (ct > 1)
                                                {
                                                    e.CancelEdit = true;
                                                    MessageBox.Show("Key " + e.Label + " already exists, cannot create a duplicate");
                                                    e.Node.BeginEdit();
                                                }
                                                else
                                                {
                                                    e.Node.Name = e.Label;
                                                    e.Node.Text = e.Label;
                                                    KeyCreate(e.Node);
                                                    Action = ActionType.None;
                                                    KeyTree.LabelEdit = false;
                                                }
                                            }
                                            else if (e.Node.Parent.Nodes.ContainsKey(e.Label)) // Edited to different than its original form
                                                                                               // It is a duplicate so show message
                                            {
                                                e.CancelEdit = true;
                                                MessageBox.Show("Key " + e.Label + " already exists, cannot create a duplicate");
                                                e.Node.BeginEdit();
                                            }
                                            else // Not a duplicate so make the change
                                            {
                                                e.Node.Name = e.Label;
                                                e.Node.Text = e.Label;
                                                KeyCreate(e.Node);
                                                Action = ActionType.None;
                                                KeyTree.LabelEdit = false;
                                            }

                                            break;
                                        }
                                    case ActionType.Rename:
                                        {
                                            if (e.Node.Parent.Nodes.ContainsKey(e.Label) & (e.Label ?? "") != (e.Node.Name ?? "")) // Key exists and the new name is not the same as the original so it is a duplicate
                                            {
                                                e.CancelEdit = true;
                                                MessageBox.Show("Key " + e.Label + " already exists, cannot create a duplicate");
                                                e.Node.BeginEdit();
                                            }
                                            else // Not a duplicate
                                            {
                                                if ((e.Label ?? "") != (e.Node.Name ?? ""))
                                                    KeyRename(e.Node, e.Label); // Has been changed to a different value from original so rename the key
                                                Action = ActionType.None;
                                                KeyTree.LabelEdit = false;
                                                // Now enqueue an F5 Refresh so that the edited node appears in the correct position in the tree based on its new name
                                                SendKeys.SendWait("{F5}");
                                            }

                                            break;
                                        }

                                    default:
                                        {
                                            MessageBox.Show("Unexpected Action type: " + Action.ToString());
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                // Cancel the label edit action, inform the user, and place the node in edit mode again. 
                                e.CancelEdit = true;
                                MessageBox.Show("Invalid tree node label.\r\nThe invalid characters are: '@','.', ',', '!'", "Node Label Edit");
                                e.Node.BeginEdit();
                            }
                        }
                        else
                        {
                            // Cancel the label edit action, inform the user and place the node in edit mode again. 
                            e.CancelEdit = true;
                            MessageBox.Show("Invalid tree node label.\r\nThe label cannot be blank", "Node Label Edit");
                            e.Node.BeginEdit();
                        }
                    }
                    else
                    {
                        // MsgBox("Text not changed")
                        switch (Action)
                        {
                            case ActionType.Add:
                                {
                                    ct = 0;
                                    foreach (TreeNode TN in e.Node.Parent.Nodes)
                                    {
                                        if ((TN.Name ?? "") == (e.Node.Name ?? ""))
                                            ct += 1;
                                    }
                                    if (ct > 1)
                                    {
                                        e.CancelEdit = true;
                                        MessageBox.Show("Key " + e.Label + " already exists, cannot create a duplicate");
                                        e.Node.BeginEdit();
                                    }
                                    else
                                    {
                                        KeyCreate(e.Node);
                                        Action = ActionType.None;
                                        KeyTree.LabelEdit = false;
                                    }

                                    break;
                                }
                            // No action required as name has not changed
                            case ActionType.Rename:
                                {
                                    break;
                                }

                            default:
                                {
                                    MessageBox.Show("Unexpected Action type: " + Action.ToString());
                                    break;
                                }
                        }
                    }
                }
                else // User has cancelled edit
                {
                    e.Node.EndEdit(true);
                    KeyTree.LabelEdit = false;
                }
            }
            catch (Exception ex)
            {
                LogException("KeyTree_AfterLabelEdit", ex);
            }
        }

        private void KeyTree_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                switch (e.Button)
                {
                    case MouseButtons.Right:
                        {
                            SetSelectedNodeByPosition(KeyTree, e.X, e.Y);
                            if (KeyTree.SelectedNode is not null)
                            {
                                if ((KeyTree.SelectedNode.FullPath ?? "") == ROOT_NAME) // Prevent delete or rename of root node
                                {
                                    mnuDeleteKey.Enabled = false;
                                    mnuRenameKey.Enabled = false;
                                    mnuNewKey.Enabled = true;
                                }
                                else
                                {
                                    mnuDeleteKey.Enabled = true;
                                    mnuRenameKey.Enabled = true;
                                    mnuNewKey.Enabled = true;
                                }
                            }
                            else
                            {
                                mnuDeleteKey.Enabled = false;
                                mnuRenameKey.Enabled = false;
                                mnuNewKey.Enabled = false;
                            }
                            mnuCtxKeys.Show(KeyTree, new Point(e.X, e.Y));
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                LogException("KeyTree_MouseUp", ex);
            }

        }

        private void KeyCreate(TreeNode Node)
        {
            string SubKeyPath;

            Node.Name = Node.Text; // Set the node name to be the same as the label
            SubKeyPath = Node.FullPath.Substring(Node.FullPath.IndexOf('\\'));
            Prof.CreateKey(SubKeyPath);
            RefreshKeyValues(Node.FullPath);
        }

        private void KeyRename(TreeNode Node, string NewName)
        {
            Prof.RenameKey(Node.FullPath.Substring(ROOT_NAME.Length) + 1, (Node.Parent.FullPath.Substring(ROOT_NAME.Length) + @"\" + NewName));
            Node.Name = NewName;
            Node.Text = NewName;
            RefreshKeyValues(Node.FullPath);
        }

        private void SetSelectedNodeByPosition(TreeView tv, int mouseX, int mouseY)
        {
            TreeNode Node;
            Point pt;

            try
            {
                pt = new Point(mouseX, mouseY);
                tv.PointToClient(pt);
                Node = tv.GetNodeAt(pt);
                tv.SelectedNode = Node;
                if (Node is null)
                    return;
                if (!Node.Bounds.Contains(pt))
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RightClickAdd(object sender, EventArgs e)
        {
            var NewNode = new TreeNode();

            try
            {
                NewNode.Name = "New Node";
                NewNode.Text = "New Node";

                KeyTree.SelectedNode.Nodes.Add(NewNode);
                KeyTree.SelectedNode.ExpandAll();
                NewNode.BeginEdit();
                Action = ActionType.Add;
            }

            catch (Exception ex)
            {
                LogException("RightClickAdd", ex);
            }
        }

        private void mnuDeleteKey_MouseUp(object sender, MouseEventArgs e)
        {
            DialogResult Res;
            string SubKeyPath;

            try
            {
                Res = MessageBox.Show("Are you sure you want to delete this key?", "Confirm delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (Res == DialogResult.OK)
                {
                    Action = ActionType.Delete;
                    SubKeyPath = KeyTree.SelectedNode.FullPath.Substring(KeyTree.SelectedNode.FullPath.IndexOf('\\'));
                    Prof.DeleteKey(SubKeyPath);
                    KeyTree.SelectedNode.Remove();
                    RefreshKeyValues(KeyTree.SelectedNode.FullPath);
                }
            }
            catch (Exception ex)
            {
                LogException("mnuDeleteKey_MouseUp", ex);
            }
        }

        private void mnuRenameKey_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Action = ActionType.Rename;
                KeyTree.LabelEdit = true;
                KeyTree.SelectedNode.BeginEdit();
            }
            catch (Exception ex)
            {
                LogException("mnuRenameKey_MouseUp", ex);
            }
        }

        private void mnuNewKey_MouseUp(object sender, MouseEventArgs e)
        {
            var NewNode = new TreeNode(NEW_KEY_NAME);

            try
            {
                NewNode.Name = NEW_KEY_NAME;
                NewNode.Text = NEW_KEY_NAME;
                Action = ActionType.Add;
                KeyTree.SelectedNode.Nodes.Add(NewNode);
                KeyTree.SelectedNode = NewNode;
                KeyTree.LabelEdit = true;
                NewNode.BeginEdit();
            }
            catch (Exception ex)
            {
                LogException("mnuNewKey_MouseUp", ex);
            }
        }

        #endregion

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void mnuRootEdit_Click(object sender, EventArgs e)
        {
            UtilitiesSettings Settings;

            try
            {
                Settings = new UtilitiesSettings();
                mnuRootEdit.Checked = !mnuRootEdit.Checked; // Reverse the checked state
                RefreshKeyValues(); // Refresh current key values
                Settings.ProfileRootEdit = mnuRootEdit.Checked;
                Settings.Dispose();
                Settings = (UtilitiesSettings)null;
            }
            catch (Exception ex)
            {
                LogException("mnuRootEdit_Click", ex);
            }
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            AboutBox About;

            try
            {
                About = new AboutBox();
                About.ShowDialog();
                About.Close();
                About.Dispose();
                About = null;
            }
            catch (Exception ex)
            {
                LogException("mnuAbout_Click", ex);
            }

        }
    }
}