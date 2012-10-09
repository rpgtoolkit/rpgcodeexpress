namespace RPGCode_Express
{
    partial class ProjectExplorer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectExplorer));
            this.pnlProjectExplorer = new System.Windows.Forms.Panel();
            this.treFileBrowser = new System.Windows.Forms.TreeView();
            this.cmsFileBrowser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuItemNew = new System.Windows.Forms.ToolStripMenuItem();
            this.newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuItemOpenWindowsExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemRename = new System.Windows.Forms.ToolStripMenuItem();
            this.imgIcons = new System.Windows.Forms.ImageList(this.components);
            this.tspProjectExplorer = new System.Windows.Forms.ToolStrip();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.pnlProjectExplorer.SuspendLayout();
            this.cmsFileBrowser.SuspendLayout();
            this.tspProjectExplorer.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlProjectExplorer
            // 
            this.pnlProjectExplorer.Controls.Add(this.treFileBrowser);
            this.pnlProjectExplorer.Controls.Add(this.tspProjectExplorer);
            this.pnlProjectExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProjectExplorer.Location = new System.Drawing.Point(0, 0);
            this.pnlProjectExplorer.Name = "pnlProjectExplorer";
            this.pnlProjectExplorer.Size = new System.Drawing.Size(203, 376);
            this.pnlProjectExplorer.TabIndex = 0;
            // 
            // treFileBrowser
            // 
            this.treFileBrowser.AllowDrop = true;
            this.treFileBrowser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treFileBrowser.ContextMenuStrip = this.cmsFileBrowser;
            this.treFileBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treFileBrowser.ImageIndex = 0;
            this.treFileBrowser.ImageList = this.imgIcons;
            this.treFileBrowser.LabelEdit = true;
            this.treFileBrowser.Location = new System.Drawing.Point(0, 25);
            this.treFileBrowser.Name = "treFileBrowser";
            this.treFileBrowser.SelectedImageIndex = 0;
            this.treFileBrowser.Size = new System.Drawing.Size(203, 351);
            this.treFileBrowser.TabIndex = 1;
            this.treFileBrowser.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treFileBrowser_AfterLabelEdit);
            this.treFileBrowser.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treFileBrowser_BeforeCollapse);
            this.treFileBrowser.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treFileBrowser_BeforeExpand);
            this.treFileBrowser.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treFileBrowser_ItemDrag);
            this.treFileBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treFileBrowser_AfterSelect);
            this.treFileBrowser.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treFileBrowser_NodeMouseDoubleClick);
            this.treFileBrowser.DragDrop += new System.Windows.Forms.DragEventHandler(this.treFileBrowser_DragDrop);
            this.treFileBrowser.DragEnter += new System.Windows.Forms.DragEventHandler(this.treFileBrowser_DragEnter);
            this.treFileBrowser.DragOver += new System.Windows.Forms.DragEventHandler(this.treFileBrowser_DragOver);
            this.treFileBrowser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.treFileBrowser_KeyPress);
            this.treFileBrowser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treFileBrowser_MouseDown);
            // 
            // cmsFileBrowser
            // 
            this.cmsFileBrowser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuItemNew,
            this.newFolderToolStripMenuItem,
            this.ToolStripSeparator1,
            this.mnuItemOpenWindowsExplorer,
            this.ToolStripSeparator2,
            this.mnuItemOpen,
            this.ToolStripSeparator3,
            this.mnuItemDelete,
            this.mnuItemRename});
            this.cmsFileBrowser.Name = "cmsFileBrowser";
            this.cmsFileBrowser.Size = new System.Drawing.Size(212, 154);
            this.cmsFileBrowser.Opening += new System.ComponentModel.CancelEventHandler(this.cmsFileBrowser_Opening);
            // 
            // mnuItemNew
            // 
            this.mnuItemNew.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_NewDocumentIcon;
            this.mnuItemNew.Name = "mnuItemNew";
            this.mnuItemNew.Size = new System.Drawing.Size(211, 22);
            this.mnuItemNew.Text = "New";
            this.mnuItemNew.Click += new System.EventHandler(this.mnuItemNew_Click);
            // 
            // newFolderToolStripMenuItem
            // 
            this.newFolderToolStripMenuItem.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_NewFolderIcon;
            this.newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            this.newFolderToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.newFolderToolStripMenuItem.Text = "New Folder";
            this.newFolderToolStripMenuItem.Click += new System.EventHandler(this.newFolderToolStripMenuItem_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(208, 6);
            // 
            // mnuItemOpenWindowsExplorer
            // 
            this.mnuItemOpenWindowsExplorer.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_OpenFileIcon;
            this.mnuItemOpenWindowsExplorer.Name = "mnuItemOpenWindowsExplorer";
            this.mnuItemOpenWindowsExplorer.Size = new System.Drawing.Size(211, 22);
            this.mnuItemOpenWindowsExplorer.Text = "Open in Windows Explorer";
            this.mnuItemOpenWindowsExplorer.Click += new System.EventHandler(this.mnuItemOpenWindowsExplorer_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(208, 6);
            // 
            // mnuItemOpen
            // 
            this.mnuItemOpen.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_OpenFileIcon;
            this.mnuItemOpen.Name = "mnuItemOpen";
            this.mnuItemOpen.Size = new System.Drawing.Size(211, 22);
            this.mnuItemOpen.Text = "Open";
            this.mnuItemOpen.Click += new System.EventHandler(this.mnuItemOpen_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(208, 6);
            // 
            // mnuItemDelete
            // 
            this.mnuItemDelete.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_DeleteIcon;
            this.mnuItemDelete.Name = "mnuItemDelete";
            this.mnuItemDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.mnuItemDelete.Size = new System.Drawing.Size(211, 22);
            this.mnuItemDelete.Text = "Delete";
            this.mnuItemDelete.Click += new System.EventHandler(this.mnuItemDelete_Click);
            // 
            // mnuItemRename
            // 
            this.mnuItemRename.Name = "mnuItemRename";
            this.mnuItemRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.mnuItemRename.Size = new System.Drawing.Size(211, 22);
            this.mnuItemRename.Text = "Rename";
            this.mnuItemRename.Click += new System.EventHandler(this.mnuItemRename_Click);
            // 
            // imgIcons
            // 
            this.imgIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIcons.ImageStream")));
            this.imgIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgIcons.Images.SetKeyName(0, "toolkit3_16x16.png");
            this.imgIcons.Images.SetKeyName(1, "Folder.ico");
            this.imgIcons.Images.SetKeyName(2, "VSFolder_open.bmp");
            this.imgIcons.Images.SetKeyName(3, "EntityDataModel_ComplexTypeProperty_16x16.png");
            // 
            // tspProjectExplorer
            // 
            this.tspProjectExplorer.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tspProjectExplorer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefresh,
            this.toolStripSeparator5,
            this.btnCollapseAll,
            this.toolStripSeparator6,
            this.btnOpen});
            this.tspProjectExplorer.Location = new System.Drawing.Point(0, 0);
            this.tspProjectExplorer.Name = "tspProjectExplorer";
            this.tspProjectExplorer.Size = new System.Drawing.Size(203, 25);
            this.tspProjectExplorer.TabIndex = 0;
            this.tspProjectExplorer.Text = "toolStrip1";
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = global::RPGCode_Express.Properties.Resources.Toolbar_Refresh;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(23, 22);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCollapseAll
            // 
            this.btnCollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCollapseAll.Image = global::RPGCode_Express.Properties.Resources.HtmlHelp2_16x16_DynamicHelp;
            this.btnCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCollapseAll.Name = "btnCollapseAll";
            this.btnCollapseAll.Size = new System.Drawing.Size(23, 22);
            this.btnCollapseAll.Text = "Collapse All";
            this.btnCollapseAll.Click += new System.EventHandler(this.btnCollapseAll_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_OpenFileIcon;
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Black;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 22);
            this.btnOpen.Text = "Open";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // ProjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 376);
            this.Controls.Add(this.pnlProjectExplorer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ProjectExplorer";
            this.Tag = "Project Explorer";
            this.Text = "Project Explorer";
            this.Load += new System.EventHandler(this.ProjectExplorer_Load);
            this.pnlProjectExplorer.ResumeLayout(false);
            this.pnlProjectExplorer.PerformLayout();
            this.cmsFileBrowser.ResumeLayout(false);
            this.tspProjectExplorer.ResumeLayout(false);
            this.tspProjectExplorer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlProjectExplorer;
        private System.Windows.Forms.ToolStrip tspProjectExplorer;
        private System.Windows.Forms.TreeView treFileBrowser;
        internal System.Windows.Forms.ImageList imgIcons;
        internal System.Windows.Forms.ContextMenuStrip cmsFileBrowser;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemNew;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemOpenWindowsExplorer;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemOpen;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemDelete;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemRename;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnCollapseAll;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
    }
}