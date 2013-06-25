/*
 ********************************************************************
 * RPGCode Express Version 1.0
 * This file copyright (C) 2012-2013 Joshua Michael Daly
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

        public event EventHandler<NodeClickEventArgs> NodeClick;
        public event EventHandler<NodeClickEventArgs> NodeDoubleClick;
        public event EventHandler<NodeLabelRenameEventArgs> NodeRename;

        #region Public Properties

        /// <summary>
        /// Gets or sets the title of the project for the Project Explorers treeview parent parentNode.
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
        /// Populates the treeview with the parent parentNode and any sub nodes.
        /// </summary>
        public void PopulateTreeView()
        {
            treFileBrowser.Nodes.Clear();

            ProjectNode projectNode = new ProjectNode(null, this.Title, this.ProjectPath);
            treFileBrowser.Nodes.Add(projectNode);
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
            {
                return string.Concat("Filename contains invalid characters!");
            }
            else if (filename.Length > 255)
            {
                return "Filename to long.";
            }
            else if (editedNode is FileNode & Path.GetExtension(filename) != ".prg")
            {
                return "Invaild RPGCode Program file extension!";
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a blank .prg file.
        /// </summary>
        private void CreateNewFile()
        {
            int fileNumber = 1;
            bool fileCreated = false;

            ExplorerNode parentNode = (ExplorerNode)treFileBrowser.SelectedNode;
            string filePath = parentNode.AbsolutePath;

            while (!fileCreated)
            {
                if (File.Exists(filePath + @"\program" + fileNumber + ".prg"))
                {
                    fileNumber++;
                }
                else
                {
                    try
                    {
                        TextWriter textWriter = new StreamWriter(filePath + @"\program" + fileNumber + ".prg");
                        textWriter.Close();

                        FileNode fileNode = new FileNode(parentNode, @"program" + fileNumber + ".prg");

                        // Repeated code, exactly the same for a folder...
                        treFileBrowser.SelectedNode.Nodes.Add(fileNode);
                        treFileBrowser.SelectedNode = fileNode;
                        treFileBrowser.LabelEdit = true;
                        fileNode.BeginEdit();
                        fileCreated = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the file or folder the parentNode represents.
        /// </summary>
        private void DeleteFile()
        {
            if (MessageBox.Show(treFileBrowser.SelectedNode.Text + " will be deleted permanently.", "RPGCode Express", 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    ExplorerNode deleteNode = (ExplorerNode)treFileBrowser.SelectedNode;

                    if (deleteNode is FolderNode)
                    {
                        Directory.Delete(deleteNode.AbsolutePath, true);
                    }
                    else
                    {
                        File.Delete(deleteNode.AbsolutePath);
                    }

                    treFileBrowser.SelectedNode.Remove();

                    if (deleteNode.Children.Count > 0)
                    {
                        deleteNode.Children.Clear();
                    }

                    deleteNode.ParentNode.Children.Remove(deleteNode);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Toogles whether or not node labels can be edited based on the current selected parent node.
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
        /// <param name="editedNode">The parentNode that was edited.</param>
        private void RenameFile(NodeLabelEditEventArgs e, ExplorerNode editedNode)
        {
            try
            {
                string oldFile = editedNode.AbsolutePath;
                string newFile = System.IO.Path.Combine(editedNode.ParentNode.AbsolutePath, e.Label);

                if (editedNode is FileNode)
                {
                    File.Move(oldFile, newFile);
                }
                else
                {
                    foreach (ExplorerNode node in editedNode.Children)
                    {
                        NodeLabelRenameEventArgs args = new NodeLabelRenameEventArgs(oldFile + @"\" + node.File,
                            newFile + @"\" + node.Text);

                        this.OnNodeRename(args);
                    }

                    Directory.Move(oldFile, newFile);
                }

                editedNode.File = e.Label;

                NodeLabelRenameEventArgs args2 = new NodeLabelRenameEventArgs(oldFile, newFile);
                this.OnNodeRename(args2);
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
                    MessageBox.Show(CheckFileName(e.Label, editedNode), "File rename error!", MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
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
            ExplorerNode node = (ExplorerNode)e.Node;

            DirectoryInfo directoryInfo = new DirectoryInfo(node.AbsolutePath);

            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                FolderNode directoryNode = new FolderNode(node, directory.Name);
                directoryNode.SelectedImageIndex = directoryNode.ImageIndex;

                if (directory.GetFiles().Length != 0 | directory.GetDirectories().Length != 0)
                {
                    directoryNode.Nodes.Add("*DUMMY*");
                }

                e.Node.Nodes.Add(directoryNode);
                node.Children.Add(directoryNode);
            }

            foreach(FileInfo file in directoryInfo.GetFiles())
            {
                if(file.Extension == ".prg")
                {
                    FileNode fileNode = new FileNode(node, file.Name);

                    e.Node.Nodes.Add(fileNode);
                    node.Children.Add(fileNode);
                }
            }
        }

        private void treFileBrowser_MouseDown(object sender, MouseEventArgs e)
        {
            if (treFileBrowser.GetNodeAt(e.X, e.Y) != null)
            {
                EnableNodeEdit();
            }
        }

        private void treFileBrowser_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ExplorerNode explorerNode = (ExplorerNode)e.Node;

            if (explorerNode is FileNode)
            {
                ProjectFile selectedFile = new ProjectFile(e.Node.Text, explorerNode.AbsolutePath);
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
            bool isFile = e.Data.GetDataPresent(typeof(FileNode));
            bool isFolder = e.Data.GetDataPresent(typeof(FolderNode));

            if (isFile || isFolder)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void treFileBrowser_DragOver(object sender, DragEventArgs e)
        {
            bool isFile = e.Data.GetDataPresent(typeof(FileNode));
            bool isFolder = e.Data.GetDataPresent(typeof(FolderNode));
            bool isProject = e.Data.GetDataPresent(typeof(ProjectNode));

            if (!isFile && !isFolder && !isProject)
            {
                return;
            }

            TreeView selectedTreeview = (TreeView)sender;
            Point point = selectedTreeview.PointToClient(new Point(e.X, e.Y));
            ExplorerNode targetNode = (ExplorerNode)selectedTreeview.GetNodeAt(point);

            if (!object.ReferenceEquals(selectedTreeview, targetNode))
            {
                selectedTreeview.SelectedNode = targetNode;
                TreeNode dropNode;

                if (isFile)
                {
                    dropNode = (TreeNode)e.Data.GetData(typeof(FileNode));
                }
                else if (isFolder)
                {
                    dropNode = (TreeNode)e.Data.GetData(typeof(FolderNode));
                }
                else
                {
                    dropNode = (TreeNode)e.Data.GetData(typeof(ProjectNode));
                }

                while (targetNode != null)
                {
                    if (object.ReferenceEquals(targetNode, dropNode))
                    {
                        e.Effect = DragDropEffects.None;

                        return;
                    }
                    else if (targetNode is FileNode)
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
            bool isFile = e.Data.GetDataPresent(typeof(FileNode));
            bool isFolder = e.Data.GetDataPresent(typeof(FolderNode));

            if (!isFile && !isFolder)
            {
                return;
            }

            TreeView treeView = (TreeView)sender;
            ExplorerNode targetNode = (ExplorerNode)treeView.SelectedNode;
            ExplorerNode dropNode;

            if (isFile)
            {
                dropNode = (FileNode)e.Data.GetData(typeof(FileNode));
            }
            else
            {
                dropNode = (FolderNode)e.Data.GetData(typeof(FolderNode));
            }

            if (targetNode == null)
            {
                return;
            }
            else if (targetNode is FileNode)
            {
                return;
            }
            else
                try
                {
                    if (dropNode is FolderNode)
                    {
                        if (dropNode.Children.Count > 0)
                        {
                            foreach (ExplorerNode node in dropNode.Children)
                            {
                                NodeLabelRenameEventArgs args = new NodeLabelRenameEventArgs(node.AbsolutePath,
                                    targetNode.AbsolutePath + @"\" + dropNode.Text + @"\" + node.Text);

                                this.OnNodeRename(args);
                            }
                        }

                        Directory.Move(dropNode.AbsolutePath, targetNode.AbsolutePath + @"\" + dropNode.Text);
                    }
                    else
                    {
                        NodeLabelRenameEventArgs args2 = new NodeLabelRenameEventArgs(dropNode.AbsolutePath,
                        targetNode.AbsolutePath + @"\" + dropNode.Text);
                        this.OnNodeRename(args2);

                        File.Move(dropNode.AbsolutePath, targetNode.AbsolutePath + @"\" + dropNode.Text);
                    }

                    dropNode.ParentNode.Children.Remove(dropNode);
                    dropNode.ParentNode = targetNode;

                    targetNode.Children.Add(dropNode);

                    dropNode.Remove();
                    targetNode.Nodes.Add(dropNode);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }

            dropNode.EnsureVisible();
            treeView.SelectedNode = dropNode;
        }

        private void cmsFileBrowser_Opening(object sender, CancelEventArgs e)
        {
            if (treFileBrowser.SelectedNode == null)
                return;

            ExplorerNode selectedNode = (ExplorerNode)treFileBrowser.SelectedNode;

            if (selectedNode is ProjectNode)
            {
                for (int x = 0; x < 4; x++)
                    cmsFileBrowser.Items[x].Visible = true;

                for (int x = 4; x < 9; x++)
                    cmsFileBrowser.Items[x].Visible = false;
            }
            else if (selectedNode is FolderNode)
            {
                for (int x = 0; x < 5; x++)
                    cmsFileBrowser.Items[x].Visible = true;

                for (int x = 5; x < 7; x++)
                    cmsFileBrowser.Items[x].Visible = false;

                for (int x = 7; x < 9; x++)
                    cmsFileBrowser.Items[x].Visible = true;
            }
            else if (selectedNode is FileNode)
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
            {
                return;
            }
            else
            {
                CreateNewFile();
            }
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExplorerNode explorerNode = (ExplorerNode)treFileBrowser.SelectedNode;

            int folderNumber = 1;
            bool folderCreated = false;
            string folderPath = explorerNode.AbsolutePath;

            while (!folderCreated)
            {
                if (Directory.Exists(folderPath + @"\NewFolder" + folderNumber))
                {
                    folderNumber++;
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath + @"\NewFolder" + folderNumber);

                        FolderNode folderNode = new FolderNode(explorerNode, "NewFolder" + folderNumber);
                    
                        treFileBrowser.SelectedNode.Nodes.Add(folderNode);
                        treFileBrowser.SelectedNode = folderNode;
                        treFileBrowser.LabelEdit = true;
                        folderNode.BeginEdit();
                        folderCreated = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void mnuItemOpenWindowsExplorer_Click(object sender, EventArgs e)
        {
            ExplorerNode explorerNode = (ExplorerNode)treFileBrowser.SelectedNode;
            Process.Start("explorer.exe", explorerNode.AbsolutePath);
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
                ExplorerNode explorerNode = (ExplorerNode)treFileBrowser.SelectedNode;

                if (explorerNode is FileNode)
                {
                    ProjectFile selectedFile = new ProjectFile(explorerNode.Text, explorerNode.AbsolutePath);
                    NodeClickEventArgs args = new NodeClickEventArgs(selectedFile);
                    this.OnNodeDoubleClick(args);
                }
                else
                {
                    Process.Start("explorer.exe", explorerNode.AbsolutePath);
                }
            }   
        }

        private void treFileBrowser_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ExplorerNode explorerNode = (ExplorerNode)e.Node;

            ProjectFile selectedFile = new ProjectFile();
            selectedFile.FileName = explorerNode.Text;
            selectedFile.FileLocation = explorerNode.AbsolutePath;

            NodeClickEventArgs args = new NodeClickEventArgs(selectedFile);
            this.OnNodeClick(args);
        }

        #endregion
    }
}