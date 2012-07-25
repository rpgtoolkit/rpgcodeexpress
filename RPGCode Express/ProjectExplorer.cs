/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012  Joshua Michael Daly
 ********************************************************************
 * This file is part of RPGCode Express Version 1.
 *
 * RPGCode Express is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * RPGCode Express is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RPGCode Express.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using RPGCode_Express.Classes;

namespace RPGCode_Express
{
    public partial class ProjectExplorer : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private string projectName;
        private string projectPath;

        public event NodeClickHandler NodeClick;
        public delegate void NodeClickHandler(object sender, NodeClickEventArgs e);

        public event NodeDoubleClickHandler NodeDoubleClick;
        public delegate void NodeDoubleClickHandler(object sender, NodeClickEventArgs e);

        #region Properties

        /// <summary>
        /// Get or set the title of the project for the Project Explorers treeview parent node.
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
        /// Get or set the path to the projects folder.
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

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public ProjectExplorer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnNodeClick(NodeClickEventArgs e)
        {
            if (NodeClick != null)
            {
                NodeClick(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnNodeDoubleClick(NodeClickEventArgs e)
        {
            if (NodeDoubleClick != null)
            {
                NodeDoubleClick(this, e);
            }
        }

        /// <summary>
        /// Populate the treeview with the parent node and any sub nodes.
        /// </summary>
        public void PopulateTreeView()
        {
            treFileBrowser.Nodes.Clear();

            ExplorerItem rootNode = new ExplorerItem();
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
        /// Toogle whether or not nodes labels can be edited based on the current selected node.
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
        /// Deletes the file or folder the node represents.
        /// </summary>
        private void DeleteFile()
        {
            if (MessageBox.Show(treFileBrowser.SelectedNode.Text + " will be deleted permanently.", "RPGCode Express", MessageBoxButtons.OKCancel,
                          MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    ExplorerItem deleteNode = (ExplorerItem)treFileBrowser.SelectedNode;

                    if (deleteNode.Type == ExplorerItemType.Folder)
                        Directory.Delete(treFileBrowser.SelectedNode.Tag.ToString(), true);
                    else
                        File.Delete(treFileBrowser.SelectedNode.Tag.ToString());

                    treFileBrowser.SelectedNode.Remove();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string CheckFileName(string filename, ExplorerItem editedNode)
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
        /// 
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

                        ExplorerItem fileNode = new ExplorerItem();
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
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="editedNode"></param>
        private void RenameFile(NodeLabelEditEventArgs e, ExplorerItem editedNode)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.CancelEdit = true;
            }
        }

        #endregion 

        #region Generated Events

        private void ProjectExplorer_Load(object sender, EventArgs e)
        {
            PopulateTreeView();
        }

        private void treFileBrowser_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            ExplorerItem editedNode = (ExplorerItem)treFileBrowser.SelectedNode;

            if (e.Label != null)
            {
                if (CheckFileName(e.Label, editedNode) != null)
                {
                    MessageBox.Show(CheckFileName(e.Label, editedNode), "File rename error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.CancelEdit = true;
                }
                else
                    RenameFile(e, editedNode);
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
                ExplorerItem directoryNode = new ExplorerItem();
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
                    ExplorerItem fileNode = new ExplorerItem();
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
            ExplorerItem explorerItem = (ExplorerItem)e.Node;

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
            if (e.Data.GetDataPresent("RPGCode_Express.Classes.ExplorerItem", true))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void treFileBrowser_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("RPGCode_Express.Classes.ExplorerItem", true) == false)
                return;

            TreeView selectedTreeview = (TreeView)sender;

            Point point = selectedTreeview.PointToClient(new Point(e.X, e.Y));
            ExplorerItem targetNode = (ExplorerItem)selectedTreeview.GetNodeAt(point);

            if ((!object.ReferenceEquals(selectedTreeview, targetNode)))
            {
                selectedTreeview.SelectedNode = targetNode;

                TreeNode dropNode = (TreeNode)e.Data.GetData("RPGCode_Express.Classes.ExplorerItem");

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

                    targetNode = (ExplorerItem)targetNode.Parent;
                }
            }

            e.Effect = DragDropEffects.Move;
        }

        private void treFileBrowser_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("RPGCode_Express.Classes.ExplorerItem", true) == false)
                return;

            TreeView treeView = (TreeView)sender;
            ExplorerItem dropNode = (ExplorerItem)e.Data.GetData("RPGCode_Express.Classes.ExplorerItem");
            ExplorerItem targetNode = (ExplorerItem)treeView.SelectedNode;

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

            ExplorerItem selectedNode = (ExplorerItem)treFileBrowser.SelectedNode;

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

                        ExplorerItem directoryNode = new ExplorerItem();
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
                ExplorerItem explorerItem = (ExplorerItem)treFileBrowser.SelectedNode;

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

        #endregion
    }
}