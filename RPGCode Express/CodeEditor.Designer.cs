namespace RPGCode_Express
{
    partial class CodeEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeEditor));
            this.tableCodeEditor = new System.Windows.Forms.TableLayoutPanel();
            this.txtCodeEditor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.cmMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuItemCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuItemUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuItemFind = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuItemCommentSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemUncommandSlected = new System.Windows.Forms.ToolStripMenuItem();
            this.tableObjectExplorer = new System.Windows.Forms.TableLayoutPanel();
            this.cboObjectExplorer = new System.Windows.Forms.ComboBox();
            this.cboClassExplorer = new System.Windows.Forms.ComboBox();
            this.tmrCommandTooltip = new System.Windows.Forms.Timer(this.components);
            this.Tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.imageListPopup = new System.Windows.Forms.ImageList(this.components);
            this.tableCodeEditor.SuspendLayout();
            this.cmMain.SuspendLayout();
            this.tableObjectExplorer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableCodeEditor
            // 
            this.tableCodeEditor.ColumnCount = 1;
            this.tableCodeEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableCodeEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableCodeEditor.Controls.Add(this.txtCodeEditor, 0, 1);
            this.tableCodeEditor.Controls.Add(this.tableObjectExplorer, 0, 0);
            this.tableCodeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCodeEditor.Location = new System.Drawing.Point(0, 0);
            this.tableCodeEditor.Margin = new System.Windows.Forms.Padding(0);
            this.tableCodeEditor.Name = "tableCodeEditor";
            this.tableCodeEditor.RowCount = 2;
            this.tableCodeEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableCodeEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableCodeEditor.Size = new System.Drawing.Size(624, 408);
            this.tableCodeEditor.TabIndex = 0;
            // 
            // txtCodeEditor
            // 
            this.txtCodeEditor.AllowDrop = true;
            this.txtCodeEditor.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.txtCodeEditor.BackBrush = null;
            this.txtCodeEditor.ContextMenuStrip = this.cmMain;
            this.txtCodeEditor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCodeEditor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtCodeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCodeEditor.Language = FastColoredTextBoxNS.Language.RpgCode;
            this.txtCodeEditor.LeftBracket = '(';
            this.txtCodeEditor.Location = new System.Drawing.Point(3, 27);
            this.txtCodeEditor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.txtCodeEditor.Name = "txtCodeEditor";
            this.txtCodeEditor.Paddings = new System.Windows.Forms.Padding(0);
            this.txtCodeEditor.RightBracket = ')';
            this.txtCodeEditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCodeEditor.ShowFoldingLines = true;
            this.txtCodeEditor.Size = new System.Drawing.Size(618, 378);
            this.txtCodeEditor.TabIndex = 1;
            this.txtCodeEditor.Tag = "";
            this.txtCodeEditor.SelectionChanged += new System.EventHandler(this.txtCodeEditor_SelectionChanged);
            this.txtCodeEditor.TextChangedDelayed += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.txtCodeEditor_TextChangedDelayed);
            this.txtCodeEditor.SelectionChangedDelayed += new System.EventHandler(this.txtCodeEditor_SelectionChangedDelayed);
            this.txtCodeEditor.UndoRedoStateChanged += new System.EventHandler<System.EventArgs>(this.txtCodeEditor_UndoRedoStateChanged);
            this.txtCodeEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCodeEditor_KeyDown);
            this.txtCodeEditor.MouseLeave += new System.EventHandler(this.txtCodeEditor_MouseLeave);
            this.txtCodeEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtCodeEditor_MouseMove);
            // 
            // cmMain
            // 
            this.cmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuItemCut,
            this.mnuItemCopy,
            this.mnuItemPaste,
            this.mnuItemSelectAll,
            this.ToolStripSeparator1,
            this.mnuItemUndo,
            this.mnuItemRedo,
            this.ToolStripSeparator2,
            this.mnuItemFind,
            this.mnuItemReplace,
            this.ToolStripSeparator3,
            this.mnuItemCommentSelected,
            this.mnuItemUncommandSlected});
            this.cmMain.Name = "cmMain";
            this.cmMain.Size = new System.Drawing.Size(213, 242);
            // 
            // mnuItemCut
            // 
            this.mnuItemCut.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_CutIcon;
            this.mnuItemCut.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mnuItemCut.Name = "mnuItemCut";
            this.mnuItemCut.Size = new System.Drawing.Size(212, 22);
            this.mnuItemCut.Text = "Cut";
            this.mnuItemCut.Click += new System.EventHandler(this.mnuItemCut_Click);
            // 
            // mnuItemCopy
            // 
            this.mnuItemCopy.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_CopyIcon;
            this.mnuItemCopy.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mnuItemCopy.Name = "mnuItemCopy";
            this.mnuItemCopy.Size = new System.Drawing.Size(212, 22);
            this.mnuItemCopy.Text = "Copy";
            this.mnuItemCopy.Click += new System.EventHandler(this.mnuItemCopy_Click);
            // 
            // mnuItemPaste
            // 
            this.mnuItemPaste.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_PasteIcon;
            this.mnuItemPaste.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mnuItemPaste.Name = "mnuItemPaste";
            this.mnuItemPaste.Size = new System.Drawing.Size(212, 22);
            this.mnuItemPaste.Text = "Paste";
            this.mnuItemPaste.Click += new System.EventHandler(this.mnuItemPaste_Click);
            // 
            // mnuItemSelectAll
            // 
            this.mnuItemSelectAll.Name = "mnuItemSelectAll";
            this.mnuItemSelectAll.Size = new System.Drawing.Size(212, 22);
            this.mnuItemSelectAll.Text = "Select All";
            this.mnuItemSelectAll.Click += new System.EventHandler(this.mnuItemSelectAll_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(209, 6);
            // 
            // mnuItemUndo
            // 
            this.mnuItemUndo.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_UndoIcon;
            this.mnuItemUndo.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mnuItemUndo.Name = "mnuItemUndo";
            this.mnuItemUndo.Size = new System.Drawing.Size(212, 22);
            this.mnuItemUndo.Text = "Undo";
            this.mnuItemUndo.Click += new System.EventHandler(this.mnuItemUndo_Click);
            // 
            // mnuItemRedo
            // 
            this.mnuItemRedo.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_RedoIcon;
            this.mnuItemRedo.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mnuItemRedo.Name = "mnuItemRedo";
            this.mnuItemRedo.Size = new System.Drawing.Size(212, 22);
            this.mnuItemRedo.Text = "Redo";
            this.mnuItemRedo.Click += new System.EventHandler(this.mnuItemRedo_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(209, 6);
            // 
            // mnuItemFind
            // 
            this.mnuItemFind.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_FindIcon;
            this.mnuItemFind.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mnuItemFind.Name = "mnuItemFind";
            this.mnuItemFind.Size = new System.Drawing.Size(212, 22);
            this.mnuItemFind.Text = "Find";
            this.mnuItemFind.Click += new System.EventHandler(this.mnuItemFind_Click);
            // 
            // mnuItemReplace
            // 
            this.mnuItemReplace.Image = global::RPGCode_Express.Properties.Resources.Icons_16x16_ReplaceIcon;
            this.mnuItemReplace.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mnuItemReplace.Name = "mnuItemReplace";
            this.mnuItemReplace.Size = new System.Drawing.Size(212, 22);
            this.mnuItemReplace.Text = "Replace";
            this.mnuItemReplace.Click += new System.EventHandler(this.mnuItemReplace_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(209, 6);
            // 
            // mnuItemCommentSelected
            // 
            this.mnuItemCommentSelected.Name = "mnuItemCommentSelected";
            this.mnuItemCommentSelected.Size = new System.Drawing.Size(212, 22);
            this.mnuItemCommentSelected.Text = "Comment Selected Lines";
            this.mnuItemCommentSelected.Click += new System.EventHandler(this.mnuItemCommentSelected_Click);
            // 
            // mnuItemUncommandSlected
            // 
            this.mnuItemUncommandSlected.Name = "mnuItemUncommandSlected";
            this.mnuItemUncommandSlected.Size = new System.Drawing.Size(212, 22);
            this.mnuItemUncommandSlected.Text = "Uncomment Selected Lines";
            this.mnuItemUncommandSlected.Click += new System.EventHandler(this.mnuItemUncommandSlected_Click);
            // 
            // tableObjectExplorer
            // 
            this.tableObjectExplorer.ColumnCount = 2;
            this.tableObjectExplorer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableObjectExplorer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableObjectExplorer.Controls.Add(this.cboObjectExplorer, 0, 0);
            this.tableObjectExplorer.Controls.Add(this.cboClassExplorer, 0, 0);
            this.tableObjectExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableObjectExplorer.Location = new System.Drawing.Point(0, 0);
            this.tableObjectExplorer.Margin = new System.Windows.Forms.Padding(0);
            this.tableObjectExplorer.Name = "tableObjectExplorer";
            this.tableObjectExplorer.RowCount = 1;
            this.tableObjectExplorer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableObjectExplorer.Size = new System.Drawing.Size(624, 25);
            this.tableObjectExplorer.TabIndex = 1;
            // 
            // cboObjectExplorer
            // 
            this.cboObjectExplorer.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboObjectExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboObjectExplorer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboObjectExplorer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboObjectExplorer.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboObjectExplorer.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboObjectExplorer.FormattingEnabled = true;
            this.cboObjectExplorer.ItemHeight = 18;
            this.cboObjectExplorer.Location = new System.Drawing.Point(313, 1);
            this.cboObjectExplorer.Margin = new System.Windows.Forms.Padding(1, 1, 3, 3);
            this.cboObjectExplorer.Name = "cboObjectExplorer";
            this.cboObjectExplorer.Size = new System.Drawing.Size(308, 24);
            this.cboObjectExplorer.Sorted = true;
            this.cboObjectExplorer.TabIndex = 5;
            this.cboObjectExplorer.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboObjectExplorer_DrawItem);
            this.cboObjectExplorer.SelectedIndexChanged += new System.EventHandler(this.cboObjectExplorer_SelectedIndexChanged);
            this.cboObjectExplorer.Enter += new System.EventHandler(this.cboObjectExplorer_Enter);
            // 
            // cboClassExplorer
            // 
            this.cboClassExplorer.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboClassExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboClassExplorer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboClassExplorer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClassExplorer.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboClassExplorer.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboClassExplorer.ForeColor = System.Drawing.SystemColors.Control;
            this.cboClassExplorer.FormattingEnabled = true;
            this.cboClassExplorer.ItemHeight = 18;
            this.cboClassExplorer.Location = new System.Drawing.Point(3, 1);
            this.cboClassExplorer.Margin = new System.Windows.Forms.Padding(3, 1, 1, 3);
            this.cboClassExplorer.Name = "cboClassExplorer";
            this.cboClassExplorer.Size = new System.Drawing.Size(308, 24);
            this.cboClassExplorer.Sorted = true;
            this.cboClassExplorer.TabIndex = 4;
            this.cboClassExplorer.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboClassExplorer_DrawItem);
            this.cboClassExplorer.SelectedIndexChanged += new System.EventHandler(this.cboClassExplorer_SelectedIndexChanged);
            this.cboClassExplorer.Enter += new System.EventHandler(this.cboClassExplorer_Enter);
            // 
            // tmrCommandTooltip
            // 
            this.tmrCommandTooltip.Interval = 500;
            this.tmrCommandTooltip.Tick += new System.EventHandler(this.tmrCommandTooltip_Tick);
            // 
            // imageListPopup
            // 
            this.imageListPopup.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListPopup.ImageStream")));
            this.imageListPopup.TransparentColor = System.Drawing.Color.Magenta;
            this.imageListPopup.Images.SetKeyName(0, "Icons.16x16.Method.png");
            this.imageListPopup.Images.SetKeyName(1, "Icons.16x16.Literal.png");
            this.imageListPopup.Images.SetKeyName(2, "Icons.16x16.PropertiesIcon.png");
            this.imageListPopup.Images.SetKeyName(3, "Icons.16x16.Enum.png");
            this.imageListPopup.Images.SetKeyName(4, "Icons.16x16.Field.png");
            this.imageListPopup.Images.SetKeyName(5, "Icons.16x16.Class.png");
            // 
            // CodeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 408);
            this.Controls.Add(this.tableCodeEditor);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CodeEditor";
            this.Tag = "Code Editor";
            this.Text = "CodeEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CodeEditor_FormClosing);
            this.Load += new System.EventHandler(this.CodeEditor_Load);
            this.tableCodeEditor.ResumeLayout(false);
            this.cmMain.ResumeLayout(false);
            this.tableObjectExplorer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableCodeEditor;
        private System.Windows.Forms.TableLayoutPanel tableObjectExplorer;
        internal FastColoredTextBoxNS.FastColoredTextBox txtCodeEditor;
        internal System.Windows.Forms.ComboBox cboObjectExplorer;
        internal System.Windows.Forms.ComboBox cboClassExplorer;
        private System.Windows.Forms.Timer tmrCommandTooltip;
        internal System.Windows.Forms.ContextMenuStrip cmMain;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemCut;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemCopy;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemPaste;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemSelectAll;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemUndo;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemRedo;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemFind;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemReplace;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemCommentSelected;
        internal System.Windows.Forms.ToolStripMenuItem mnuItemUncommandSlected;
        internal System.Windows.Forms.ToolTip Tooltip;
        internal System.Windows.Forms.ImageList imageListPopup;
    }
}