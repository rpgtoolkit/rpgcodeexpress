/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using RpgCodeExpress.Events;
using RpgCodeExpress.Files;
using RpgCodeExpress.Items;

namespace RpgCodeExpress
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProjectExplorer : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private string projectName;
        private string projectPath;

        private FileSystemWatcher watcher = new FileSystemWatcher();

        public event EventHandler<NodeClickEventArgs> NodeClick;
        public event EventHandler<NodeClickEventArgs> NodeDoubleClick;
        public event EventHandler<NodeLabelRenameEventArgs> NodeRename;

        #region Public Properties

        /// <summary>
        /// Gets or sets the title of the project for the Project Explorers treeview parent node.
        /// </summary>
        public string Title
        {
            get
            {
                return projectName;
            }
            set
            {
                projectName = value;
            }
        }

        /// <summary>
        /// Gets or sets the path to the projects folder.
        /// </summary>
        public string ProjectPath
        {
            get
            {
                return projectPath;
            }
            set
            {
                projectPath = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a project explorer.
        /// </summary>
        public ProjectExplorer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Populates the treeview with the parent node and any sub nodes.
        /// </summary>
        public void PopulateTreeView()
        {
            treFileBrowser.Nodes.Clear();

            ExplorerNode rootNode = new ExplorerNode();
            rootNode.Type = ExplorerItemType.Project;
            rootNode.ImageIndex = 0;
            rootNode.Text = Title;
            rootNode.Tag = ProjectPath;
            rootNode.NodeFont = new Font(this.Font, FontStyle.Bold);
            rootNode.Nodes.Add("*DUMMY*");
            rootNode.Expand();

            treFileBrowser.Nodes.Add(rootNode);
        }

        /// <summary>
        /// Starts the file watcher.
        /// </summary>
        public void StartWatcher()
        {
            watcher.Filter = "*.*";
            watcher.Path = ProjectPath;
            watcher.IncludeSubdirectories = true;

            watcher.Created += new FileSystemEventHandler(watcher_Created);
            watcher.Deleted += new FileSystemEventHandler(watcher_Deleted);
            watcher.Renamed += new RenamedEventHandler(watcher_Renamed);
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);

            watcher.EnableRaisingEvents = true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks the file name to see if it is valid.
        /// </summary>
        /// <param name="filename">File name to check.</param>
        /// <returns>The error, if any.</returns>
        private string CheckFileName(string filename, ExplorerNode editedNode)
        {
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();

            if (filename.IndexOfAny(invalidChars) > -1)
                return string.Concat("Filename contains invalid characters!");
            else if (filename.Length > 255)
                return "Filename to long.";
            else if (editedNode.Type == ExplorerItemType.Program & Path.GetExtension(filename) != ".prg")
                return "Invaild RPGCode Program file extension!";
            else
                return null;
        }

        /// <summary>
        /// Creates a blank .prg file.
        /// </summary>
        private void CreateNewFile()
        {
            int fileNumber = 1;
            bool fileCreated = false;
            string filePath = treFileBrowser.SelectedNode.Tag.ToString();

            while (!fileCreated)
            {
                if (File.Exists(filePath + @"\program" + fileNumber + ".prg"))
                    fileNumber++;
                else
                {
                    try
                    {
                        TextWriter textWriter = new StreamWriter(filePath + @"\program" + fileNumber + ".prg");
                        textWriter.Close();

                        ExplorerNode fileNode = new ExplorerNode();
                        fileNode.Type = ExplorerItemType.Program;
                        fileNode.Text = @"program" + fileNumber + ".prg";
                        fileNode.Tag = filePath + @"\program" + fileNumber + ".prg";
                        fileNode.ImageIndex = 3;
                        fileNode.SelectedImageIndex = fileNode.ImageIndex;

                        treFileBrowser.SelectedNode.Nodes.Add(fileNode);
                        treFileBrowser.SelectedNode = fileNode;
                        treFileBrowser.LabelEdit = true;
                        fileNode.BeginEdit();
                        fileCreated = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the file or folder the node represents.
        /// </summary>
        private void DeleteFile()
        {
            if (MessageBox.Show(treFileBrowser.SelectedNode.Text + " will be deleted permanently.", "RPGCode Express", MessageBoxButtons.OKCancel,
                          MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    ExplorerNode deleteNode = (ExplorerNode)treFileBrowser.SelectedNode;

                    if (deleteNode.Type == ExplorerItemType.Folder)
                        Directory.Delete(treFileBrowser.SelectedNode.Tag.ToString(), true);
                    else
                        File.Delete(treFileBrowser.SelectedNode.Tag.ToString());

                    treFileBrowser.SelectedNode.Remove();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Toogles whether or not nodes labels can be edited based on the current selected node.
        /// </summary>
        private void EnableNodeEdit()
        {
            if (treFileBrowser.SelectedNode != null)
            {
                if (treFileBrowser.SelectedNode.Parent != null)
                {
                    treFileBrowser.LabelEdit = true;
                    return;
                }
            }

            treFileBrowser.LabelEdit = false;
        }

        /// <summary>
        /// Renames a file.
        /// </summary>
        /// <param name="e">Event information.</param>
        /// <param name="editedNode">The node that was edited.</param>
        private void RenameFile(NodeLabelEditEventArgs e, ExplorerNode editedNode)
        {
            try
            {
                string oldFile = editedNode.Tag.ToString();
                string newFile = System.IO.Path.Combine(editedNode.Parent.Tag.ToString(), e.Label);

                if (editedNode.Type == ExplorerItemType.Program)
                    File.Move(oldFile, newFile);
                else
                    Directory.Move(oldFile, newFile);

                string filename = editedNode.Tag.ToString();
                filename = filename.Remove(filename.Length - editedNode.Text.Length, editedNode.Text.Length);
                editedNode.Tag = filename + e.Label;

                NodeLabelRenameEventArgs args = new NodeLabelRenameEventArgs(oldFile, newFile);
                this.OnNodeRename(args);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.CancelEdit = true;
            }
        }

        #endregion 

        #region Custom Events

        protected virtual void OnNodeClick(NodeClickEventArgs e)
        {
            if (NodeClick != null)
            {
                NodeClick(this, e);
            }
        }

        protected virtual void OnNodeDoubleClick(NodeClickEventArgs e)
        {
            if (NodeDoubleClick != null)
            {
                NodeDoubleClick(this, e);
            }
        }

        protected virtual void OnNodeRename(NodeLabelRenameEventArgs e)
        {
            if (NodeRename != null)
            {
                NodeRename(this, e);
            }
        }

        #endregion

        #region Events

        private void ProjectExplorer_Load(object sender, EventArgs e)
        {
            PopulateTreeView();
        }

        private void treFileBrowser_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            ExplorerNode editedNode = (ExplorerNode)treFileBrowser.SelectedNode;

            if (e.Label != null)
            {
                if (CheckFileName(e.Label, editedNode) != null)
                {
                    MessageBox.Show(CheckFileName(e.Label, editedNode), "File rename error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.CancelEdit = true;
                }
                else
                {
                    RenameFile(e, editedNode);
                }
            }
        }

        private void treFileBrowser_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            e.Node.Nodes.Add("*DUMMY*");
        }

        private void treFileBrowser_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();

            DirectoryInfo directoryInfo = new DirectoryInfo(e.Node.Tag.ToString());

            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                ExplorerNode directoryNode = new ExplorerNode();
                directoryNode.Type = ExplorerItemType.Folder;
                directoryNode.Tag = directory.FullName;
                directoryNode.Text = directory.Name;
                directoryNode.ImageIndex = 1;
                directoryNode.SelectedImageIndex = directoryNode.ImageIndex;

                if (directory.GetFiles().Length != 0 | directory.GetDirectories().Length != 0)
                    directoryNode.Nodes.Add("*DUMMY*");

                e.Node.Nodes.Add(directoryNode);
            }

            foreach(FileInfo file in directoryInfo.GetFiles())
            {
                if(file.Extension == ".prg")
                {
                    ExplorerNode fileNode = new ExplorerNode();
                    fileNode.Type = ExplorerItemType.Program;
                    fileNode.Tag = file.FullName;
                    fileNode.Text = file.Name;
                    fileNode.ImageIndex = 3;
                    fileNode.SelectedImageIndex = fileNode.ImageIndex;

                    e.Node.Nodes.Add(fileNode);
                }
            }
        }

        private void treFileBrowser_MouseDown(object sender, MouseEventArgs e)
        {
            if (treFileBrowser.GetNodeAt(e.X, e.Y) != null)
                EnableNodeEdit();
        }

        private void treFileBrowser_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ExplorerNode explorerItem = (ExplorerNode)e.Node;

            if (explorerItem.Type == ExplorerItemType.Program)
            {
                ProjectFile selectedFile = new ProjectFile(e.Node.Text, e.Node.Tag.ToString());
                NodeClickEventArgs args = new NodeClickEventArgs(selectedFile);
                this.OnNodeDoubleClick(args);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateTreeView();
        }

        private void btnCollapseAll_Click(object sender, EventArgs e)
        {
            treFileBrowser.CollapseAll();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            treFileBrowser_KeyPress(sender, new KeyPressEventArgs((char)13));
        }

        private void treFileBrowser_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treFileBrowser_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("RpgCodeExpress.Items.ExplorerNode", true))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void treFileBrowser_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("RpgCodeExpress.Items.ExplorerNode", true) == false)
                return;

            TreeView selectedTreeview = (TreeView)sender;

            Point point = selectedTreeview.PointToClient(new Point(e.X, e.Y));
            ExplorerNode targetNode = (ExplorerNode)selectedTreeview.GetNodeAt(point);

            if ((!object.ReferenceEquals(selectedTreeview, targetNode)))
            {
                selectedTreeview.SelectedNode = targetNode;

                TreeNode dropNode = (TreeNode)e.Data.GetData("RpgCodeExpress.Items.ExplorerNode");

                while (!(targetNode == null))
                {
                    if (object.ReferenceEquals(targetNode, dropNode))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                    else if (targetNode.Type != ExplorerItemType.Folder & targetNode.Type != ExplorerItemType.Project)
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }

                    targetNode = (ExplorerNode)targetNode.Parent;
                }
            }

            e.Effect = DragDropEffects.Move;
        }

        private void treFileBrowser_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("RpgCodeExpress.Items.ExplorerNode", true) == false)
                return;

            TreeView treeView = (TreeView)sender;
            ExplorerNode dropNode = (ExplorerNode)e.Data.GetData("RpgCodeExpress.Items.ExplorerNode");
            ExplorerNode targetNode = (ExplorerNode)treeView.SelectedNode;

            if (targetNode == null)
                return;
            else if(targetNode.Type == ExplorerItemType.File | targetNode.Type == ExplorerItemType.Program)
                return;
            else
                try
                {
                    if(dropNode.Type == ExplorerItemType.Folder)
                        Directory.Move(dropNode.Tag.ToString(), targetNode.Tag.ToString()+ @"\" + dropNode.Text);
                    else
                        File.Move(dropNode.Tag.ToString(), targetNode.Tag.ToString()+ @"\" + dropNode.Text);

                    dropNode.Tag = targetNode.Tag.ToString() + @"\" + dropNode.Text;

                    dropNode.Remove();
                    targetNode.Nodes.Add(dropNode);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            dropNode.EnsureVisible();
            treeView.SelectedNode = dropNode;
        }

        private void cmsFileBrowser_Opening(object sender, CancelEventArgs e)
        {
            if (treFileBrowser.SelectedNode == null)
                return;

            ExplorerNode selectedNode = (ExplorerNode)treFileBrowser.SelectedNode;

            if (selectedNode.Type == ExplorerItemType.Project)
            {
                for (int x = 0; x < 4; x++)
                    cmsFileBrowser.Items[x].Visible = true;

                for (int x = 4; x < 9; x++)
                    cmsFileBrowser.Items[x].Visible = false;
            }
            else if (selectedNode.Type == ExplorerItemType.Folder)
            {
                for (int x = 0; x < 5; x++)
                    cmsFileBrowser.Items[x].Visible = true;

                for (int x = 5; x < 7; x++)
                    cmsFileBrowser.Items[x].Visible = false;

                for (int x = 7; x < 9; x++)
                    cmsFileBrowser.Items[x].Visible = true;
            }
            else
            {
                for (int x = 0; x < 5; x++)
                    cmsFileBrowser.Items[x].Visible = false;

                for (int x = 5; x < 9; x++)
                    cmsFileBrowser.Items[x].Visible = true;
            }
        }

        private void mnuItemNew_Click(object sender, EventArgs e)
        {
            if (treFileBrowser.SelectedNode == null)
                return;
            else
                CreateNewFile();
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int folderNumber = 1;
            bool folderCreated = false;
            string folderPath = treFileBrowser.SelectedNode.Tag.ToString();

            while (!folderCreated)
            {
                if (Directory.Exists(folderPath + @"\NewFolder" + folderNumber))
                    folderNumber++;
                else
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath + @"\NewFolder" + folderNumber);

                        ExplorerNode directoryNode = new ExplorerNode();
                        directoryNode.Type = ExplorerItemType.Folder;
                        directoryNode.Text = "NewFolder" + folderNumber; ;
                        directoryNode.Tag = folderPath + @"\NewFolder" + folderNumber;
                        directoryNode.ImageIndex = 1;
                        directoryNode.SelectedImageIndex = directoryNode.ImageIndex;

                        treFileBrowser.SelectedNode.Nodes.Add(directoryNode);
                        treFileBrowser.SelectedNode = directoryNode;
                        treFileBrowser.LabelEdit = true;
                        directoryNode.BeginEdit();
                        folderCreated = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void mnuItemOpenWindowsExplorer_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", treFileBrowser.SelectedNode.Tag.ToString());
        }

        private void mnuItemOpen_Click(object sender, EventArgs e)
        {
            treFileBrowser_KeyPress(sender, new KeyPressEventArgs((char)13));
        }

        private void mnuItemDelete_Click(object sender, EventArgs e)
        {
            DeleteFile();
        }

        private void mnuItemRename_Click(object sender, EventArgs e)
        {
            EnableNodeEdit();
            treFileBrowser.SelectedNode.BeginEdit();
        }

        private void treFileBrowser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 & treFileBrowser.SelectedNode != null)
            {
                ExplorerNode explorerItem = (ExplorerNode)treFileBrowser.SelectedNode;

                if (explorerItem.Type == ExplorerItemType.Program)
                {
                    ProjectFile selectedFile = new ProjectFile(explorerItem.Text, explorerItem.Tag.ToString());
                    NodeClickEventArgs args = new NodeClickEventArgs(selectedFile);
                    this.OnNodeDoubleClick(args);
                }
                else
                    Process.Start("explorer.exe", explorerItem.Tag.ToString());
            }   
        }

        private void treFileBrowser_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ProjectFile selectedFile = new ProjectFile();
            selectedFile.FileName = e.Node.Text;
            selectedFile.FileLocation = e.Node.Tag.ToString();

            NodeClickEventArgs args = new NodeClickEventArgs(selectedFile);
            this.OnNodeClick(args);
        }

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show("File changed - {0}, change type - {1} " +  e.Name + " " + e.ChangeType);
        }

        private void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            MessageBox.Show("File renamed - old name - {0}, new name - {1} " + e.OldFullPath + " " + e.FullPath);
        }

        private void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show("File deleted - {0} ", e.Name);
        }

        private void watcher_Created(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show("File created - {0}, path - {1} " + e.Name + " " + e.FullPath);
        }

        #endregion
    }
}