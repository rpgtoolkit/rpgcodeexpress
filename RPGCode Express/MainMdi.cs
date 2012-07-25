/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012 Joshua Michael Daly
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
using System.Windows.Forms;
using System.IO;
using System.Collections;
using Microsoft.VisualBasic;
using WeifenLuo.WinFormsUI.Docking;
using RPGCode_Express.Classes;
using RPGCode_Express.Classes.Renders;
using RPGCode_Express.Classes.RPGCode;
using RPGCode_Express.Classes.Utilities;

namespace RPGCode_Express
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainMdi : Form
    {
        public const string ProgramVersion = "RPGCode Express 1.4a";

        public RPGcode RpgCodeReference = new RPGcode();
        public ConfigurationFile Configuration = new ConfigurationFile();

        public string MainFolder;
        public string GameFolder;
        public string Toolkit3 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Toolkit3\";

        private bool projectLoaded;
        private string projectPath;
        private string projectTitle;

        //Docks to keep track of.
        private ProjectExplorer projectExplorer;
        private PropertiesWindow propertiesWindow;

        #region Properties

        /// <summary>
        /// Gets the full path of the editors Xml configuration file.
        /// </summary>
        public string ConfigurationFilePath
        {
            get
            {
                return Application.StartupPath + @"\config.xml";
            }
        }

        /// <summary>
        /// Gets the full path of the RPGCode Xml reference file.
        /// </summary>
        public string RPGCodeReferencePath
        {
            get
            {
                return Application.StartupPath + @"\RPGcode.xml";
            }
        }

        /// <summary>
        /// Gets the current active DockContent in the dockpanel.
        /// </summary>
        public DockContent ActiveContent
        {
            get
            {
                return (DockContent)dockPanel.ActiveContent;
            }
        }

        /// <summary>
        /// Gets the current active DockConent in the dockpanel and casts it to a CodeEditor.
        /// </summary>
        public CodeEditor CurrentCodeEditor
        {
            get
            {
                return (CodeEditor)ActiveContent;
            }
        }

        /// <summary>
        /// Gets or sets the current projects title.
        /// </summary>
        public string Title
        {
            get
            {
                return projectTitle;
            }
            set
            {
                projectTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets the current projects path.
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

        /// <summary>
        /// Gets or sets a value which indicates whether or not a project has been opened.
        /// </summary>
        public bool IsProjectOpen
        {
            get
            {
                return projectLoaded;
            }
            set
            {
                projectLoaded = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public MainMdi()
        {
            InitializeComponent();

            //Set Menu Renders
            menuStrip.Renderer = new MenuRender();
            ToolStripManager.Renderer = new ToolstripRender();
            ((ToolstripRender)toolStrip.Renderer).RoundedEdges = false;

            //Set path variables
            MainFolder = Toolkit3 + @"main\";
            GameFolder = Toolkit3 + @"game\";
            projectPath = GameFolder;

            //Load RPGCode Reference
            SerializableData serializer = new SerializableData();
            RpgCodeReference = (RPGcode)serializer.Load(RPGCodeReferencePath, typeof(RPGcode));

            //Check for a configuration file
            if(File.Exists(ConfigurationFilePath))
                LoadProject();

            CreateBasicLayout();
        }

        /// <summary>
        /// Opens a new code editor.
        /// </summary>
        /// <param name="file">The path of the program file to be opened.</param>
        public void OpenCodeEditor(string file)
        {
            try
            {
                CodeEditor newCodeEditor = new CodeEditor(RpgCodeReference);
                newCodeEditor.CaretUpdated += new CodeEditor.CaretPositionUpdateHandler(CodeEditor_CaretMove);
                newCodeEditor.ProjectPath = ProjectPath;
                newCodeEditor.ProgramFile = file;
                newCodeEditor.MdiParent = this;
                newCodeEditor.Show(dockPanel);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Displays a OpenFileDialog and prompts the user to open a RPGCode program file.
        /// </summary>
        public void OpenProgram()
        {
            OpenFileDialog openProgramDialog = new OpenFileDialog();
            openProgramDialog.Filter = "RPGCode Programs (*.prg)|*.prg";
            openProgramDialog.Title = "Open RPGCode Program";
            openProgramDialog.InitialDirectory = ProjectPath;
            openProgramDialog.FilterIndex = 1;

            if (openProgramDialog.ShowDialog() == DialogResult.OK)
                OpenCodeEditor(openProgramDialog.FileName);
        }

        /// <summary>
        /// Saves a program file.
        /// </summary>
        /// <param name="saveAs">Write straight to the file or save it as something else.</param>
        private void SaveFile(bool saveAs)
        {
            CodeEditor newCodeEditor = CurrentCodeEditor;

            if (saveAs == true | newCodeEditor.ProgramFile == "Untitled")
            {
                newCodeEditor.SaveAs(); //Problem here

                if (projectExplorer != null)
                    projectExplorer.PopulateTreeView();
            }
            else
                newCodeEditor.Save();
        }

        /// <summary>
        /// Displays an OpenFileDialog and prompts the user to open a Toolkit project, writes the
        /// changes to the Xml Configuration file, and loads the project into the editor.
        /// </summary>
        private void OpenProject()
        {
            OpenFileDialog openProjectDialog = new OpenFileDialog();
            openProjectDialog.Filter = "Projects (*.gam)|*.gam";
            openProjectDialog.Title = "Open Toolkit Project";
            openProjectDialog.InitialDirectory = MainFolder;
            openProjectDialog.FilterIndex = 1;

            if (openProjectDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Configuration.Title = Path.GetFileNameWithoutExtension(openProjectDialog.SafeFileName);
                    Configuration.Path = GameFolder + Configuration.Title + @"\";
                    this.Text = Configuration.Title + " - " + ProgramVersion;
                    Configuration.Save(ConfigurationFilePath);

                    LoadProject();
                    CloseAllDocks();
                    CreateBasicLayout();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Loads the information stored in the editors configuration file.
        /// </summary>
        private void LoadProject()
        {
            try
            {
                SerializableData serializer = new SerializableData();
                Configuration = (ConfigurationFile)serializer.Load(ConfigurationFilePath, typeof(ConfigurationFile));

                projectLoaded = true;
                projectTitle = Configuration.Title;
                projectPath = Configuration.Path + @"prg\";
                this.Text = Configuration.Title + " - " + ProgramVersion;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Sets up the editors basic layout including the Project Explorer and Properties windows.
        /// </summary>
        private void CreateBasicLayout()
        {
            ShowProjectExplorer();
            ShowPropertiesWindow();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowProjectExplorer()
        {
            if (projectExplorer != null)
            {
                projectExplorer.Show();
                return;
            }

            projectExplorer = new ProjectExplorer();
            projectExplorer.NodeClick += new ProjectExplorer.NodeClickHandler(ProjectExplorer_NodeClick);
            projectExplorer.NodeDoubleClick += new ProjectExplorer.NodeDoubleClickHandler(ProjectExplorer_NodeDoubleClick);

            if (IsProjectOpen)
            {
                projectExplorer.Title = projectTitle;
                projectExplorer.ProjectPath = projectPath;
            }
            else
            {
                projectExplorer.Title = "Toolkit 3";
                projectExplorer.ProjectPath = GameFolder;
            }

            if (propertiesWindow != null)
            {
                if (propertiesWindow.DockState == DockState.DockRight)
                {
                    projectExplorer.Show(propertiesWindow.Pane, DockAlignment.Top, 0.5);
                    return;
                }
            }

            projectExplorer.Show(dockPanel, DockState.DockRight);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowPropertiesWindow()
        {
            if (propertiesWindow != null)
            {
                propertiesWindow.Show();
                return;
            }

            propertiesWindow = new PropertiesWindow();

            if (projectExplorer != null)
            {
                if (projectExplorer.DockState == DockState.DockRight)
                {
                    propertiesWindow.Show(projectExplorer.Pane, DockAlignment.Bottom, 0.5);
                    return;
                }
            }
            
            propertiesWindow.Show(dockPanel, DockState.DockRight);
        }

        /// <summary>
        /// 
        /// </summary>
        private void CloseAllDocks()
        {
            ArrayList docks = new ArrayList(dockPanel.Contents);

            foreach (DockContent childForm in docks)
                childForm.Close();
        }

        /// <summary>
        /// Enables or disables the menustrip and toolstrip buttons based upon the state of the current
        /// active document.
        /// </summary>
        private void SetToolStripButtons()
        {
            bool state = true;
            DockContent activeDock = ActiveContent;

            if (activeDock == null)
                state = false;
            else if (activeDock.DockState == DockState.Float)
                state = false;
            else if (activeDock.Tag.ToString() != "Code Editor")
                state = false;

            mnuItemSave.Enabled = state;
            mnuItemSaveAs.Enabled = state;
            mnuItemSaveAll.Enabled = state;
            tspButtonSave.Enabled = state;
            tspButtonSaveAll.Enabled = state;

            tspButtonCut.Enabled = state;
            tspButtonCopy.Enabled = state;
            tspButtonPaste.Enabled = state;
            tspButtonFind.Enabled = state;
            tspButtonCommentSelected.Enabled = state;
            tspButtonUndo.Enabled = state;
            tspButtonRedo.Enabled = state;
            tspButtonRunProgram.Enabled = state;

            mnuItemUndo.Enabled = state;
            mnuItemRedo.Enabled = state;
            mnuItemCut.Enabled = state;
            mnuItemCopy.Enabled = state;
            mnuItemPaste.Enabled = state;
            mnuItemSelectAll.Enabled = state;
            mnuItemQuickFind.Enabled = state;
            mnuItemQuickReplace.Enabled = state;
        }

        #endregion

        #region Custom Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeEditor_CaretMove(object sender, CaretPositionUpdateEventArgs e)
        {
            lblLineNumber.Text = "Ln " + e.CurrentLine.ToString();
            lblColumnNumber.Text = "Col " + e.CurrentColumn.ToString();
            lblCharacterNumber.Text = "Ch " + e.CurrentCharacter.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectExplorer_NodeDoubleClick(object sender, NodeClickEventArgs e)
        {
            OpenCodeEditor(e.FilePath);
        }

        private void ProjectExplorer_NodeClick(object sender, NodeClickEventArgs e)
        {
            if (propertiesWindow != null)
                propertiesWindow.SetGridItem(e.File);
        }

        #endregion

        #region Events

        private void mnuItemNew_Click(object sender, EventArgs e)
        {
            OpenCodeEditor("Untitled");
        }

        private void mnuItemOpen_Click(object sender, EventArgs e)
        {
            OpenProgram();
        }

        private void mnuItemOpenProject_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private void mnuItemSave_Click(object sender, EventArgs e)
        {
            SaveFile(false);
        }

        private void mnuItemSaveAs_Click(object sender, EventArgs e)
        {
            SaveFile(true);
        }

        private void mnuItemSaveAll_Click(object sender, EventArgs e)
        {
            ArrayList docks = new ArrayList(dockPanel.Contents);

            foreach (DockContent childForm in docks)
            {
                if (childForm.GetType() == typeof(CodeEditor))
                {
                    CodeEditor codeEditor = (CodeEditor)childForm;
                    codeEditor.Save();
                }
            }
        }

        private void mnuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuItemUndo_Click(object sender, EventArgs e)
        {
            CurrentCodeEditor.txtCodeEditor.Undo();
        }

        private void mnuItemRedo_Click(object sender, EventArgs e)
        {
            CurrentCodeEditor.txtCodeEditor.Redo();
        }

        private void mnuItemCut_Click(object sender, EventArgs e)
        {
            CurrentCodeEditor.txtCodeEditor.Cut();
        }

        private void mnuItemCopy_Click(object sender, EventArgs e)
        {
            CurrentCodeEditor.txtCodeEditor.Copy();
        }

        private void mnuItemPaste_Click(object sender, EventArgs e)
        {
            CurrentCodeEditor.txtCodeEditor.Paste();
        }

        private void mnuItemSelectAll_Click(object sender, EventArgs e)
        {
            CurrentCodeEditor.txtCodeEditor.SelectAll();
        }

        private void mnuItemQuickFind_Click(object sender, EventArgs e)
        {
            CurrentCodeEditor.txtCodeEditor.ShowFindDialog();
        }

        private void mnuItemQuickReplace_Click(object sender, EventArgs e)
        {
            CurrentCodeEditor.txtCodeEditor.ShowReplaceDialog();
        }

        private void tspButtonCommentSelected_Click(object sender, EventArgs e)
        {
            CurrentCodeEditor.txtCodeEditor.CommentSelected();
        }

        private void mnuItemProjectExplorer_Click(object sender, EventArgs e)
        {
            ShowProjectExplorer();
        }

        private void mnuItemPropertiesWindow_Click(object sender, EventArgs e)
        {
            ShowPropertiesWindow();
        }

        private void mnuItemNewWindow_Click(object sender, EventArgs e)
        {
            OpenCodeEditor("Untitled");
        }

        private void tspButtonRunProgram_Click(object sender, EventArgs e)
        {
            string program = CurrentCodeEditor.txtCodeEditor.Text;

            StreamWriter textWriter = new StreamWriter(ProjectPath + @"sys_test.prg");
            textWriter.Write(program);
            textWriter.Close();

            string oldDirectory = Directory.GetCurrentDirectory();

            Directory.SetCurrentDirectory(@"C:\Program Files\Toolkit3\");
            Interaction.Shell("trans3 demo.gam sys_test.prg", AppWinStyle.NormalFocus, false, -1);

            Directory.SetCurrentDirectory(oldDirectory);
        }

        private void mnuItemCloseAll_Click(object sender, EventArgs e)
        {
            CloseAllDocks(); //Stop this from closing project explorer and properties
        }

        private void dockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            SetToolStripButtons(); //This fires constantly, find a better event
        }

        private void dockPanel_ContentRemoved(object sender, DockContentEventArgs e)
        {
            if (e.Content.GetType() == typeof(ProjectExplorer))
                projectExplorer = null;
            else if (e.Content.GetType() == typeof(PropertiesWindow))
                propertiesWindow = null;
        }

        #endregion
    }
}
