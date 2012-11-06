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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualBasic;
using RpgCodeExpress.Events;
using RpgCodeExpress.Files;
using RpgCodeExpress.Renders;
using RpgCodeExpress.RpgCode;
using RpgCodeExpress.Utilities;
using WeifenLuo.WinFormsUI.Docking;

namespace RpgCodeExpress
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainMdi : Form
    {
        private const string programVersion = "RPGCode Express Beta 1";

        private RPGcode rpgCodeReference = new RPGcode();
        private ConfigurationFile configurationFile = new ConfigurationFile();

        private string mainFolder;
        private string gameFolder;
        private string toolkitPath;
        private bool engineExists;
        private bool projectLoaded;
        private string projectPath;
        private string projectTitle;

        ////Using courier new or lucida console throws an untraceable exception here.
        ////But verdana doesn't?
        //private Font codeEditorFont = new Font("Verdana", 10);

        //Docks to keep track of.
        private ProjectExplorer projectExplorer;
        private PropertiesWindow propertiesWindow;
        private Dictionary<string, EditorForm> editorDictionary = new Dictionary<string, EditorForm>();

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

        public MainMdi()
        {
            InitializeComponent();
 
            //Set Menu Renders
            menuStrip.Renderer = new MenuRender();
            ToolStripManager.Renderer = new ToolstripRender();
            ((ToolstripRender)toolStrip.Renderer).RoundedEdges = false; //Get rid of toolstrip rounded edges.

            //Load RPGCode Reference
            SerializableData serializer = new SerializableData();
            rpgCodeReference = (RPGcode)serializer.Load(RPGCodeReferencePath, typeof(RPGcode));

            if (CheckToolkitInstall())
            {
                if (File.Exists(ConfigurationFilePath))
                    LoadConfiguration();
            }

            CreateBasicLayout();
        }

        /// <summary>
        /// Check the current Toolkit install.
        /// </summary>
        private bool CheckToolkitInstall()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Toolkit3\";

            if (Directory.Exists(path))
            {
                toolkitPath = path;

                if (File.Exists(toolkitPath + "trans3.exe"))
                    engineExists = true;
                else
                    engineExists = false;

                if (Directory.Exists(toolkitPath + @"main\"))
                    mainFolder = toolkitPath + @"main\";
                else
                    mainFolder = toolkitPath;

                if (Directory.Exists(toolkitPath + @"game\"))
                    gameFolder = toolkitPath + @"game\";
                else
                    gameFolder = toolkitPath;

                projectPath = gameFolder;

                return true;
            }
            else
            {
                projectTitle = "My Documents";
                gameFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                return false;
            }
        }

        /// <summary>
        /// Closes all the open docks.
        /// </summary>
        private void CloseAllDocks()
        {
            ArrayList docks = new ArrayList(dockPanel.Contents);

            foreach (DockContent childForm in docks)
                childForm.Close();
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
        private void  FocusCodeEditor(string file)
        {
            EditorForm editorForm = new EditorForm();
            editorDictionary.TryGetValue(file, out editorForm);
            editorForm.Focus();
        }

        /// <summary>
        /// Loads the information stored in the editors configuration file.
        /// </summary>
        private void LoadConfiguration()
        {
            try
            {
                SerializableData serializer = new SerializableData();
                configurationFile = (ConfigurationFile)serializer.Load(ConfigurationFilePath, typeof(ConfigurationFile));

                if (Directory.Exists(configurationFile.ProjectFolder))
                {
                    if (Directory.Exists(configurationFile.ProjectFolder + @"prg\"))
                        projectPath = configurationFile.ProjectFolder + @"prg\";
                    else
                        projectPath = configurationFile.ProjectFolder;

                    projectTitle = configurationFile.ProjectName;
                }
                else
                {
                    throw new DirectoryNotFoundException();
                }

                projectLoaded = true;
                this.Text = configurationFile.ProjectName + " - " + programVersion;
            }
            catch (DirectoryNotFoundException)
            {
                string error = "The project folder " + configurationFile.ProjectFolder + " could not be found.";
                MessageBox.Show(error, "Project Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.Delete(ConfigurationFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens a new code editor.
        /// </summary>
        /// <param name="file">The path of the program file to be opened.</param>
        private void OpenCodeEditor(string file)
        {
            try
            {
                //Lowercase analysis only works for case insensitive Operating System
                //Basically OK on Windows but UNIX like could cause very slight issues.
                //The source of this issue is probably somewhere in the ProjectExplorer.
                if (editorDictionary.ContainsKey(file.ToLower()))
                {
                    FocusCodeEditor(file.ToLower());
                    return;
                }

                CodeEditor newCodeEditor = new CodeEditor(file, rpgCodeReference);
                newCodeEditor.CaretUpdated += new System.EventHandler<CaretPositionUpdateEventArgs>(CodeEditor_CaretMove);
                newCodeEditor.UndoRedoUpdated += new System.EventHandler<UndoRedoUpdateEventArgs>(CodeEditor_UndoRedoUpdated);
                //newCodeEditor.txtCodeEditor.Font = codeEditorFont;
                newCodeEditor.ProjectPath = ProjectPath;
                newCodeEditor.MdiParent = this;
                newCodeEditor.Show(dockPanel);

                if (newCodeEditor.EditorFile != "Untitled" & editorDictionary.ContainsKey(file) == false)
                    editorDictionary.Add(file.ToLower(), newCodeEditor);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Displays a OpenFileDialog and prompts the user to open a RPGCode program file.
        /// </summary>
        private void ShowOpenProgramDialog()
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
        /// Displays an OpenFileDialog and prompts the user to open a Toolkit project, writes the
        /// changes to the Xml Configuration file, and loads the project into the editor.
        /// </summary>
        private void OpenProject()
        {
            OpenFileDialog openProjectDialog = new OpenFileDialog();
            openProjectDialog.Filter = "Projects (*.gam)|*.gam";
            openProjectDialog.Title = "Open Toolkit Project";
            openProjectDialog.InitialDirectory = mainFolder;
            openProjectDialog.FilterIndex = 1;

            if (openProjectDialog.ShowDialog() == DialogResult.OK)
            {
                SaveConfiguration(openProjectDialog.SafeFileName);
                LoadConfiguration();
                CloseAllDocks();
                CreateBasicLayout();
            }
        }

        /// <summary>
        /// Run the current program in trans3.
        /// </summary>
        private void Run(object sender)
        {
            try
            {
                string shellCommand = "trans3 " + projectTitle + ".gam";

                if (!sender.Equals(mnuItemRunProject))
                {
                    string program = CurrentCodeEditor.txtCodeEditor.Text;

                    StreamWriter textWriter = new StreamWriter(ProjectPath + @"sys_test.prg");
                    textWriter.Write(program);
                    textWriter.Close();

                    shellCommand += " sys_test.prg";
                }

                string oldDirectory = Directory.GetCurrentDirectory();

                Directory.SetCurrentDirectory(@"C:\Program Files\Toolkit3\");
                Interaction.Shell(shellCommand, AppWinStyle.NormalFocus, false, -1);

                Directory.SetCurrentDirectory(oldDirectory);
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Saves the current projects configuration.
        /// </summary>
        private void SaveConfiguration(string title)
        {
            try
            {
                configurationFile.ProjectName = Path.GetFileNameWithoutExtension(title);
                configurationFile.ProjectFolder = gameFolder + configurationFile.ProjectName + @"\";

                configurationFile.Save(ConfigurationFilePath);
                this.Text = configurationFile.ProjectName + " - " + programVersion;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Saves a program file.
        /// </summary>
        /// <param name="saveAs">Write straight to the file or save it as something else.</param>
        private void SaveFile(bool saveAs)
        {
            CodeEditor newCodeEditor = CurrentCodeEditor;

            if (saveAs == true | newCodeEditor.EditorFile == "Untitled")
            {

                if (newCodeEditor.SaveAs())
                {
                    editorDictionary.Add(newCodeEditor.EditorFile.ToLower(), newCodeEditor);

                    if (projectExplorer != null)
                        projectExplorer.PopulateTreeView();
                }

            }
            else
                newCodeEditor.Save();
        }

        /// <summary>
        /// Shows or sets the focus to the Project Explorer dock, depending on its current state.
        /// </summary>
        private void ShowProjectExplorer()
        {
            if (projectExplorer != null)
            {
                projectExplorer.Show();
                return;
            }

            projectExplorer = new ProjectExplorer();
            projectExplorer.NodeClick += new EventHandler<NodeClickEventArgs>(ProjectExplorer_NodeClick);
            projectExplorer.NodeDoubleClick += new EventHandler<NodeClickEventArgs>(ProjectExplorer_NodeDoubleClick);
            projectExplorer.NodeRename += new EventHandler<NodeLabelRenameEventArgs>(ProjectExplorer_NodeRename);

            if (IsProjectOpen)
            {
                projectExplorer.Title = projectTitle;
                projectExplorer.ProjectPath = projectPath;
            }
            else
            {
                projectExplorer.Title = projectTitle;
                projectExplorer.ProjectPath = gameFolder;
            }

            projectExplorer.StartWatcher();

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
        /// Shows or sets the focus to the Properties Window dock, depending on its current state.
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
        /// Enables or disables the menustrip and toolstrip buttons based upon the state of the current
        /// active document.
        /// </summary>
        private void ToogleButtonStates()
        {
            bool isEnabled = true;

            if (ActiveContent == null)
            {
                isEnabled = false;
                DisableUndoRedo();
            }
            else if (ActiveContent.DockState == DockState.Float)
            {
                isEnabled = false;
                DisableUndoRedo();
            }
            else if (ActiveContent.GetType() != typeof(CodeEditor))
            {
                isEnabled = false;
                DisableUndoRedo();
            }
            else
            {
                CodeEditor editor = CurrentCodeEditor;
                tspButtonUndo.Enabled = editor.txtCodeEditor.UndoEnabled;
                tspButtonRedo.Enabled = editor.txtCodeEditor.RedoEnabled;
                mnuItemUndo.Enabled = editor.txtCodeEditor.UndoEnabled;
                mnuItemRedo.Enabled = editor.txtCodeEditor.RedoEnabled;
            }

            ToogleMenuButtons(isEnabled);
            ToogleToolstripButtons(isEnabled);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DisableUndoRedo()
        {
            mnuItemUndo.Enabled = false;
            mnuItemRedo.Enabled = false;
            tspButtonUndo.Enabled = false;
            tspButtonRedo.Enabled = false;
        }

        /// <summary>
        /// Enables or disables certain menu buttons.
        /// </summary>
        /// <param name="isEnabled">Enable or disable.</param>
        private void ToogleMenuButtons(bool isEnabled)
        {
            mnuItemSave.Enabled = isEnabled;
            mnuItemSaveAs.Enabled = isEnabled;
            mnuItemSaveAll.Enabled = isEnabled;
            mnuItemCut.Enabled = isEnabled;
            mnuItemCopy.Enabled = isEnabled;
            mnuItemPaste.Enabled = isEnabled;
            mnuItemSelectAll.Enabled = isEnabled;
            mnuItemCommentSelected.Enabled = isEnabled;
            mnuItemQuickFind.Enabled = isEnabled;
            mnuItemQuickReplace.Enabled = isEnabled;

            if (isEnabled)
                mnuItemDebugProgram.Enabled = engineExists & projectLoaded;
            else
                mnuItemDebugProgram.Enabled = false;
        }

        /// <summary>
        /// Enables or disables certain toolstrip buttons.
        /// </summary>
        /// <param name="isEnabled">Enable or disable.</param>
        private void ToogleToolstripButtons(bool isEnabled)
        {
            tspButtonSave.Enabled = isEnabled;
            tspButtonSaveAll.Enabled = isEnabled;
            tspButtonCut.Enabled = isEnabled;
            tspButtonCopy.Enabled = isEnabled;
            tspButtonPaste.Enabled = isEnabled;
            tspButtonFind.Enabled = isEnabled;
            tspButtonCommentSelected.Enabled = isEnabled;

            if (isEnabled)
                tspButtonRunProgram.Enabled = engineExists & projectLoaded;
            else
                tspButtonRunProgram.Enabled = false;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //private void UpdateCodeEditorFonts()
        //{
        //    ArrayList docks = new ArrayList(dockPanel.Contents);

        //    foreach (DockContent dock in docks)
        //    {
        //        if (dock.GetType() == typeof(CodeEditor))
        //        {
        //            CodeEditor codeEditor = (CodeEditor)dock;
        //            codeEditor.txtCodeEditor.Font = codeEditorFont;
        //        }
        //    }
        //}

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
        private void CodeEditor_UndoRedoUpdated(object sender, UndoRedoUpdateEventArgs e)
        {
            tspButtonUndo.Enabled = e.UndoState;
            tspButtonRedo.Enabled = e.RedoState;
            mnuItemUndo.Enabled = e.UndoState;
            mnuItemRedo.Enabled = e.RedoState;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectExplorer_NodeClick(object sender, NodeClickEventArgs e)
        {
            if (propertiesWindow != null)
                propertiesWindow.SetGridItem(e.File);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectExplorer_NodeRename(object sender, NodeLabelRenameEventArgs e)
        {
            if (editorDictionary.ContainsKey(e.OldFile.ToLower()))
            {
                EditorForm editorForm = new EditorForm();
                editorDictionary.TryGetValue(e.OldFile.ToLower(), out editorForm);

                CodeEditor codeEditor = (CodeEditor)editorForm;
                codeEditor.EditorFile = e.NewFile;
                codeEditor.TabText = Path.GetFileName(e.NewFile);

                editorDictionary.Remove(e.OldFile.ToLower());
                editorDictionary.Add(e.NewFile.ToLower(), editorForm);
            }
        }

        #endregion

        #region Events

        private void mnuItemNew_Click(object sender, EventArgs e)
        {
            OpenCodeEditor("Untitled");
        }

        private void mnuItemOpen_Click(object sender, EventArgs e)
        {
            ShowOpenProgramDialog();
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

        private void mnuItemDebugProgram_Click(object sender, EventArgs e)
        {
            Run(sender);
        }

        private void runProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run(sender);
        }

        private void fontSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.AllowScriptChange = false;
            fontDialog.ShowColor = false;
            fontDialog.ShowEffects = false;
            fontDialog.FixedPitchOnly = true;
            fontDialog.ShowDialog();

            //codeEditorFont = fontDialog.Font;
            //UpdateCodeEditorFonts();
        }

        private void mnuItemNewWindow_Click(object sender, EventArgs e)
        {
            OpenCodeEditor("Untitled");
        }

        private void mnuItemCloseAll_Click(object sender, EventArgs e)
        {
            CloseAllDocks();
        }

        private void mnuItemAbout_Click(object sender, EventArgs e)
        {
            About aboutBox = new About();
            aboutBox.ShowDialog();
        }

        private void dockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            ToogleButtonStates(); 
        }

        private void dockPanel_ContentRemoved(object sender, DockContentEventArgs e)
        {
            if (e.Content.GetType() == typeof(ProjectExplorer))
                projectExplorer = null;
            else if (e.Content.GetType() == typeof(PropertiesWindow))
                propertiesWindow = null;
            else if (e.Content.GetType() == typeof(CodeEditor))
            {
                CodeEditor codeEditor = (CodeEditor)e.Content;

                if (codeEditor.EditorFile != "Untitled")
                    editorDictionary.Remove(codeEditor.EditorFile.ToLower());
            }
        }

        #endregion
    }
}
