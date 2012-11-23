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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using RpgCodeExpress.Events;
using RpgCodeExpress.Items;
using RpgCodeExpress.RpgCode;
using RpgCodeExpress.Utilities;

namespace RpgCodeExpress
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CodeEditor : EditorForm, ISaveable
    {
        private Point lastMouseCoordinate;
        private AutocompleteMenu popupMenu; //Intellisense menu
        private RPGcode rpgCodeReference = new RPGcode(); //List of RPGCode Functions
        private Autocomplete autocompleteItems = new Autocomplete(); //Intellisense Items

        private List<DropDownItem> classes = new List<DropDownItem>();
        private List<DropDownItem> declarations = new List<DropDownItem>();
        private Dictionary<string, string> fileIncludes = new Dictionary<string, string>();

        private TextStyle greenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);

        public event EventHandler<CaretPositionUpdateEventArgs> CaretUpdated;
        public event EventHandler<UndoRedoUpdateEventArgs> UndoRedoUpdated;

        #region Public Methods

        /// <summary>
        /// Creates a blank code editor.
        /// </summary>
        public CodeEditor()
        {

        }

        /// <summary>
        /// Creates a code editor and opens a file.
        /// </summary>
        /// <param name="file">File path.</param>
        /// <param name="currentRpgCode">RPGCode autocomplete items.</param>
        public CodeEditor(string file, RPGcode currentRpgCode)
        {
            InitializeComponent();

            rpgCodeReference = currentRpgCode;
            EditorFile = file;

            this.TabText = Path.GetFileNameWithoutExtension(EditorFile);
            txtCodeEditor.AddStyle(new MarkerStyle(new SolidBrush(Color.FromArgb(50, Color.Gray))));

            if (EditorFile != "Untitled")
                ReadFile();

            popupMenu = new AutocompleteMenu(txtCodeEditor); //Set autocompletemenu's text source
            popupMenu.Opening += this.popupMenu_Opening; //Override the menu's Opening event.
            popupMenu.Items.ImageList = imageListPopup;
            popupMenu.MinFragmentLength = 1;
            popupMenu.AppearInterval = 400;
            popupMenu.AllowTabKey = true;

            BuildAutocompleteMenu();
        }

        /// <summary>
        /// Saves a file that already exists, otherwise it will call SaveAs().
        /// </summary>
        public bool Save()
        {
            if (EditorFile == "Untitled") //The file has no path
            {
                SaveAs();
            }

            WriteToFile();
            ClearFileIncludes();
            this.TabText = Path.GetFileName(EditorFile);

            return true;
        }

        /// <summary>
        /// Displays a SaveFileDialog and allow the user to save the file.
        /// Possibly a redundant routine at the moment.
        /// </summary>
        public bool SaveAs()
        {
            SaveFileDialog saveProgramFile = new SaveFileDialog();

            saveProgramFile.FilterIndex = 1;
            saveProgramFile.DefaultExt = "prg";
            saveProgramFile.OverwritePrompt = true;
            saveProgramFile.InitialDirectory = ProjectPath;
            saveProgramFile.Title = "Save RPGCode Program File";
            saveProgramFile.Filter = "RPGCode Programs (*.prg)|*.prg";
            saveProgramFile.FileName = Path.GetFileName(EditorFile);

            if (saveProgramFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.TabText = Path.GetFileName(saveProgramFile.FileName);
                    EditorFile = saveProgramFile.FileName;
                    ClearFileIncludes();
                    WriteToFile();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds all the imported autocomplete items to popup menu.
        /// </summary>
        private void AddFileDeclarations()
        {
            AddFileIncludes();

            //Add all the classes to autocompletemenu
            autocompleteItems.UserDefinedClasses.Clear();
            autocompleteItems.UserDefinedClasses = FindClasses(txtCodeEditor.Text);
            autocompleteItems.UserDefinedClasses.AddRange(autocompleteItems.IncludedClasses);

            //Add all the methods to autocompletemenu
            autocompleteItems.UserDefinedMethods.Clear();
            autocompleteItems.UserDefinedMethods = FindMethods(txtCodeEditor.Text);
            autocompleteItems.UserDefinedMethods.AddRange(autocompleteItems.IncludedMethods);

            //Add all the variables to autocompletemenu
            autocompleteItems.UserDefinedVariables.Clear();
            autocompleteItems.UserDefinedVariables = AddLocalVariables();
            autocompleteItems.UserDefinedVariables.AddRange(FindVariables(txtCodeEditor.Text));
            autocompleteItems.UserDefinedVariables.AddRange(autocompleteItems.IncludedVariables);
        }

        /// <summary>
        /// Looks for included files, and adds a reference to them.
        /// </summary>
        private void AddFileIncludes()
        {
            Regex expression = new Regex(@"^(?<range>[\w\s]*#?\b(include)\s*\(?"".+""\)?)", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (Match match in expression.Matches(txtCodeEditor.Text))
            {
                string fileInclude = match.Value.Trim();
                int position = fileInclude.IndexOf('"') + 1;

                //Get the files name from in between the quotes "...".
                fileInclude = fileInclude.Substring(position, fileInclude.LastIndexOf('"') - position);
                fileInclude = ProjectPath + fileInclude;

                if (!File.Exists(fileInclude))
                    continue; //File doesn't exist.
                else if (fileIncludes.ContainsKey(fileInclude))
                    continue; //We already have it indexed.
                else
                    ReadIncludedFile(fileInclude);
            }
        }

        /// <summary>
        /// Adds the locally defined variables.
        /// </summary>
        private ArrayList AddLocalVariables()
        {
            ArrayList locals = new ArrayList();
            Regex expression = new Regex(@"^(?<range>[\w\s]*\b(local)\(+\w+\)+)", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (Match match in expression.Matches(txtCodeEditor.Text))
            {
                string definedVariable = match.Value.Trim();
                int position = definedVariable.IndexOf('(') + 1;

                //Get the variable name between the brackets.
                definedVariable = definedVariable.Substring(position, definedVariable.IndexOf(')') - position);

                if (!locals.Contains(definedVariable))
                    locals.Add(definedVariable);
            }

            return locals;
        }

        /// <summary>
        /// Populates the code editors auto complete menu, it runs through a series of loops
        /// assigning data to the individual AutocompleteItems.
        /// </summary>
        private void BuildAutocompleteMenu()
        {
            dynamic items = new List<AutocompleteItem>();

            foreach (string item in autocompleteItems.StatementSnippets)
                items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 1 });

            foreach (string item in autocompleteItems.DeclartionSnippets)
                items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 1 });

            foreach (string item in autocompleteItems.Keywords)
                items.Add(new AutocompleteItem(item) { ImageIndex = 1 });

            foreach (string item in autocompleteItems.Constants)
                items.Add(new AutocompleteItem(item) { ImageIndex = 3 });

            foreach (Command command in rpgCodeReference.Items)
            {
                SnippetAutocompleteItem menuItem = new SnippetAutocompleteItem(command.Code);
                menuItem.ToolTipText = command.Description;
                menuItem.ToolTipTitle = command.Tooltip;
                menuItem.Text = command.Code;
                menuItem.ImageIndex = 0;
                items.Add(menuItem);
            }

            foreach (string definedClass in autocompleteItems.UserDefinedClasses)
                items.Add(new AutocompleteItem(definedClass) { ImageIndex = 5 });

            foreach (string method in autocompleteItems.UserDefinedMethods)
                items.Add(new SnippetAutocompleteItem(method) { ImageIndex = 0 });

            foreach (string variable in autocompleteItems.UserDefinedVariables)
                items.Add(new AutocompleteItem(variable) { ImageIndex = 4 });

            popupMenu.Items.SetAutocompleteItems(items);
        }

        /// <summary>
        /// Clears all of the "Included" auto complete items.
        /// </summary>
        private void ClearFileIncludes()
        {
            fileIncludes.Clear();
            autocompleteItems.IncludedClasses.Clear();
            autocompleteItems.IncludedMethods.Clear();
            autocompleteItems.IncludedVariables.Clear();
        }

        /// <summary>
        /// Stops the tooltip timer and tooltips for hoverwords.
        /// </summary>
        private void CancelTooltip()
        {
            tmrCommandTooltip.Stop();
            Tooltip.Hide(this);
        }

        /// <summary>
        /// Finds all of the classes/structs in a string of text, and return any matches.
        /// </summary>
        /// <param name="text">Text to scan.</param>
        /// <returns>A list of matches.</returns>
        private ArrayList FindClasses(string text)
        {
            ArrayList classes = new ArrayList();
            Regex expression = new Regex(@"\b(class|struct)[^\S\n]+(?<range>\w+)\b", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (Match match in expression.Matches(text))
            {
                string definedClass = match.Value.Trim();
                definedClass = definedClass.Substring(definedClass.LastIndexOf(" ")).Trim(); //Get the classes name

                if (!classes.Contains(definedClass))
                    classes.Add(definedClass);
            }

            return classes;
        }

        /// <summary>
        /// Finds all of the methods/functions in a string of text, and return any matches.
        /// </summary>
        /// <param name="text">Text to scan.</param>
        /// <returns>A list of matches.</returns>
        private ArrayList FindMethods(string text)
        {
            ArrayList methods = new ArrayList();
            Regex expression = new Regex(@"\b(method|function)[^\S\n]+(\w+::)?(?<range>\w+?)\b", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (Match match in expression.Matches(text))
            {
                string definedMethod = match.Value.Trim();

                if (definedMethod.Contains("::"))
                    definedMethod = definedMethod.Substring(definedMethod.LastIndexOf("::") + 2).Trim();
                else
                    definedMethod = definedMethod.Substring(definedMethod.LastIndexOf(" ")).Trim();

                definedMethod += "(^)";

                if (!methods.Contains(definedMethod))
                    methods.Add(definedMethod);
            }

            return methods;
        }

        /// <summary>
        /// Finds all the global/var variables in a string of text, and return any matches.
        /// </summary>
        /// <param name="text">Text to scan.</param>
        /// <returns>A list of matches</returns>
        private ArrayList FindVariables(string text)
        {
            ArrayList variables = new ArrayList();
            Regex expression = new Regex(@"^(?<range>[\w\s]*\b(global)\(+\w+\)+)", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (Match match in expression.Matches(text)) //Find the global variables i.e. "global(example)"
            {
                string definedVariable = match.Value.Trim();
                int position = definedVariable.IndexOf('(') + 1;

                definedVariable = definedVariable.Substring(position, definedVariable.IndexOf(')') - position);


                if (!variables.Contains(definedVariable))
                    variables.Add(definedVariable);
            }

            expression = new Regex(@"^(?<range>[\w\s]*\b(var)\s+\w+)", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (Match match in expression.Matches(text)) //Find class memebers i.e. "var example"
            {
                string definedVariable = match.Value.Trim();

                definedVariable = definedVariable.Substring(definedVariable.LastIndexOf(" ")).Trim();

                if (!variables.Contains(definedVariable))
                    variables.Add(definedVariable);
            }

            return variables;
        }

        /// <summary>
        /// Populates the Class Explorer combobox, taking all of the defined class declarations from the current
        /// document using a regular expression.
        /// </summary>
        private void PopulateClassExplorer()
        {
            Regex expression = new Regex(@"^(?<range>[\w\s]*\b(class|struct)[^\S\n]+[\w<>,\s]+)", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            classes.Clear();
            cboClassExplorer.Items.Clear();

            foreach (Match match in expression.Matches(txtCodeEditor.Text))
            {
                string definedClass = match.Value.Trim();

                DropDownItem item = new DropDownItem();
                item.Title = definedClass.Substring(definedClass.LastIndexOf(" ")).Trim();
                item.Position = match.Index; //Store the classes position in the code editor

                classes.Add(item);
                cboClassExplorer.Items.Add(item.Title);
            }

            //Sort them descending, alphabetically.
            classes.Sort(delegate(DropDownItem item1, DropDownItem item2) { return item1.Title.CompareTo(item2.Title); });
        }

        /// <summary>
        /// Populates the Object Explorer combobox, taking all of the defined methods and functions from the current
        /// document using regular expressions.
        /// </summary>
        private void PopulateObjectExplorer()
        {
            Regex expression = new Regex(@"^(?<range>[\w\s]*\b(method|function)[^\S\n]+(\w+::)?(\w+\(.*\)\s+))",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            declarations.Clear();
            cboObjectExplorer.Items.Clear();

            foreach (Match match in expression.Matches(txtCodeEditor.Text))
            {
                string method = match.Value.Trim();

                DropDownItem item = new DropDownItem();
                item.Type = DropDownType.Method;
                item.Title = method.Substring(method.IndexOf(" ")).Trim(); //remove the word method
                item.Position = match.Index;

                declarations.Add(item);
                cboObjectExplorer.Items.Add(item.Title);
            }

            expression = new Regex(@"^(?<range>[\w\s]*\b(global)\(+\w+\)+)", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (Match match in expression.Matches(txtCodeEditor.Text))
            {
                DropDownItem item = new DropDownItem();
                string variable = match.Value.Trim();
                int position = variable.IndexOf('(') + 1;

                item.Type = DropDownType.Global;
                item.Position = match.Index;
                item.Title = variable.Substring(variable.IndexOf('(') + 1, variable.IndexOf(')') - position);

                declarations.Add(item);
                cboObjectExplorer.Items.Add(item.Title);
            }

            expression = new Regex(@"^(?<range>[\w\s]*\b(var)\s+\w+)", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (Match match in expression.Matches(txtCodeEditor.Text))
            {
                string variable = match.Value.Trim();

                DropDownItem item = new DropDownItem();
                item.Type = DropDownType.Var;
                item.Position = match.Index;
                item.Title = variable.Substring(variable.LastIndexOf(" ")).Trim();

                declarations.Add(item);
                cboObjectExplorer.Items.Add(item.Title);
            }

            //Sort them descending, alphabetically. 
            declarations.Sort(delegate(DropDownItem item1, DropDownItem item2) { return item1.Title.CompareTo(item2.Title); });
        }

        /// <summary>
        /// Reads the contents of a file into the code editor.
        /// </summary>
        private void ReadFile()
        {
            try
            {
                TextReader textReader = new StreamReader(EditorFile);
                txtCodeEditor.Text = textReader.ReadToEnd();
                textReader.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Reads the text from the included file.
        /// </summary>
        /// <param name="file">File path.</param>
        private void ReadIncludedFile(string file)
        {
            try
            {
                TextReader textReader = new StreamReader(file);
                string text = textReader.ReadToEnd();
                textReader.Close();

                fileIncludes.Add(file, text);
                autocompleteItems.IncludedClasses.AddRange(FindClasses(text));
                autocompleteItems.IncludedMethods.AddRange(FindMethods(text));
                autocompleteItems.IncludedVariables.AddRange(FindVariables(text));
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Prompts the user to save a file that needs updating, they can choose to cancel their action.
        /// </summary>
        /// <returns>Returns either Yes, No, or Cancel, which can be used by the caller
        /// to determine whether or not they should save the file.</returns>
        private DialogResult UpdateFile()
        {
            System.Windows.Forms.DialogResult result = MessageBox.Show("Do you want to save changes to " + EditorFile,
                "RPGCode Express", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                Save();

            return result;
        }

        /// <summary>
        /// Writes the text in the code editor to a file.
        /// </summary>
        private void WriteToFile()
        {
            try
            {
                TextWriter textWriter = new StreamWriter(EditorFile);
                textWriter.Write(txtCodeEditor.Text);
                textWriter.Close();
                IsUpdated = false;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, Application.ExecutablePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Custom Events

        private void txtCodeEditor_SelectionChanged(object sender, EventArgs e)
        {
            int currentLine = txtCodeEditor.Selection.Start.iLine;
            int currentCharacter = txtCodeEditor.Selection.Start.iChar;

            CaretPositionUpdateEventArgs args = new CaretPositionUpdateEventArgs(currentLine, currentCharacter, currentCharacter);
            this.OnCaretUpdated(args);
        }

        protected virtual void OnCaretUpdated(CaretPositionUpdateEventArgs e)
        {
            if (CaretUpdated != null)
                CaretUpdated(this, e);
        }

        private void txtCodeEditor_UndoRedoStateChanged(object sender, EventArgs e)
        {
            UndoRedoUpdateEventArgs args = new UndoRedoUpdateEventArgs(txtCodeEditor.UndoEnabled, txtCodeEditor.RedoEnabled);
            this.OnUndoRedoUpdated(args);
        }

        protected virtual void OnUndoRedoUpdated(UndoRedoUpdateEventArgs e)
        {
            if (UndoRedoUpdated != null)
                UndoRedoUpdated(this, e);
        }

        #endregion

        #region Events

        private void CodeEditor_Load(object sender, EventArgs e)
        {
            IsUpdated = false;
            txtCodeEditor.ClearUndo();
            this.TabText = Path.GetFileName(EditorFile);
        }

        private void CodeEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsUpdated)
                if (UpdateFile() == DialogResult.Cancel)
                    e.Cancel = true;
        }

        private void popupMenu_Opening(object sender, CancelEventArgs e)
        {
            dynamic iGreenStyle = txtCodeEditor.GetStyleIndex(txtCodeEditor.SyntaxHighlighter.GreenStyle);
            if (iGreenStyle >= 0)
            {
                if (txtCodeEditor.Selection.Start.iChar > 0)
                {
                    dynamic characterBeforeCaret = txtCodeEditor[txtCodeEditor.Selection.Start.iLine][txtCodeEditor.Selection.Start.iChar - 1];
                    dynamic greenStyleIndex = Range.ToStyleIndex(iGreenStyle);
                    //if char contains green style then block popup menu
                    if ((characterBeforeCaret.style & greenStyleIndex) != 0)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void tmrCommandTooltip_Tick(object sender, EventArgs e)
        {
            tmrCommandTooltip.Stop();

            Place place = txtCodeEditor.PointToPlace(lastMouseCoordinate);
            Point distance = txtCodeEditor.PlaceToPoint(place);

            if (Math.Abs(distance.X - lastMouseCoordinate.X) > txtCodeEditor.CharWidth * 2 |
                Math.Abs(distance.Y - lastMouseCoordinate.Y) > txtCodeEditor.CharHeight * 2)
                return;

            FastColoredTextBoxNS.Range range = new FastColoredTextBoxNS.Range(txtCodeEditor, place, place);

            string hoverWord = range.GetFragment(@"[a-zA-Z]").Text;

            if (hoverWord == "")
                return;

            Command foundCommand = rpgCodeReference.FindCommand(hoverWord);

            if (foundCommand != null)
            {
                Tooltip.ToolTipTitle = foundCommand.Tooltip;
                Tooltip.SetToolTip(txtCodeEditor, foundCommand.Description);
                Tooltip.Show(foundCommand.Description, txtCodeEditor, new Point(lastMouseCoordinate.X,
                        lastMouseCoordinate.Y + txtCodeEditor.CharHeight));
            }
        }

        private void txtCodeEditor_SelectionChangedDelayed(object sender, EventArgs e)
        {
            txtCodeEditor.VisibleRange.ClearStyle(txtCodeEditor.Styles[0]);

            if (!txtCodeEditor.Selection.IsEmpty)
                return;

            Range fragment = txtCodeEditor.Selection.GetFragment(@"\w");
            string text = fragment.Text;

            if (text.Length == 0)
                return;

            Array ranges = txtCodeEditor.VisibleRange.GetRanges(@"\b" + text + @"\b").ToArray();

            if (ranges.Length > 1)
            {
                foreach (Range r in ranges)
                    r.SetStyle(txtCodeEditor.Styles[0]);
            }
        }

        private void txtCodeEditor_TextChangedDelayed(object sender, TextChangedEventArgs e)
        {
            CancelTooltip();
            IsUpdated = true;
            AddFileDeclarations();
            BuildAutocompleteMenu();
            this.TabText = Path.GetFileName(EditorFile) + '*';

            //Covers Multi-line C-style comments.
            FastColoredTextBoxNS.Range range;
            FastColoredTextBox textBox = (FastColoredTextBox)sender;

            range = textBox.VisibleRange;
            range.ClearStyle();
            range.SetStyle(greenStyle, "//.*$", RegexOptions.Multiline);
            range.SetStyle(greenStyle, @"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline);
            range.SetStyle(greenStyle, @"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline | RegexOptions.RightToLeft);
        }

        private void txtCodeEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyValue == 75)
                popupMenu.Show(true);
        }

        private void txtCodeEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if (lastMouseCoordinate != e.Location)
            {
                CancelTooltip();
                tmrCommandTooltip.Start();
            }

            lastMouseCoordinate = e.Location;
        }

        private void txtCodeEditor_MouseLeave(object sender, EventArgs e)
        {
            CancelTooltip();
        }

        private void cboClassExplorer_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox combobox = (ComboBox)sender;

            if (combobox.Items.Count == 0)
                return;

            if (e.Index != -1)
            {
                Image classImage = Properties.Resources.Icons_16x16_Class;
                Font itemFont = new System.Drawing.Font("Verdana", 8, FontStyle.Regular);

                e.Graphics.FillRectangle(Brushes.Bisque, e.Bounds);
                e.Graphics.DrawString(Convert.ToString(classes[e.Index].Title), itemFont, Brushes.Black, new Point(classImage.Width * 2, e.Bounds.Y));
                e.Graphics.DrawImage(classImage, new Point(e.Bounds.X, e.Bounds.Y));

                if ((e.State & DrawItemState.Focus) == 0)
                {
                    e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                    e.Graphics.DrawString(Convert.ToString(classes[e.Index].Title), itemFont, Brushes.Black, new Point(classImage.Width * 2, e.Bounds.Y));
                    e.Graphics.DrawImage(classImage, new Point(e.Bounds.X, e.Bounds.Y));
                }
            }
        }

        private void cboClassExplorer_Enter(object sender, EventArgs e)
        {
            PopulateClassExplorer();
        }

        private void cboClassExplorer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboClassExplorer.SelectedIndex != -1)
            {
                DropDownItem item = classes[cboClassExplorer.SelectedIndex];
                txtCodeEditor.GoEnd();
                txtCodeEditor.SelectionStart = item.Position;
                txtCodeEditor.DoSelectionVisible();
                txtCodeEditor.Focus();
            }
        }

        private void cboObjectExplorer_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox combobox = (ComboBox)sender;

            if (combobox.Items.Count == 0)
                return;

            if (e.Index != -1)
            {
                Image objectImage = Properties.Resources.Icons_16x16_Method;
                Font itemFont = new System.Drawing.Font("Verdana", 8, FontStyle.Regular);

                switch (declarations[e.Index].Type)
                {
                    case DropDownType.Method:
                        objectImage = Properties.Resources.Icons_16x16_Method;
                        break;
                    case DropDownType.Global:
                        objectImage = Properties.Resources.Icons_16x16_Field;
                        break;
                    case DropDownType.Var:
                        objectImage = Properties.Resources.Icons_16x16_InternalField;
                        break;
                }

                e.Graphics.FillRectangle(Brushes.Bisque, e.Bounds);
                e.Graphics.DrawString(Convert.ToString(declarations[e.Index].Title), itemFont, Brushes.Black, new Point(objectImage.Width * 2, e.Bounds.Y));
                e.Graphics.DrawImage(objectImage, new Point(e.Bounds.X, e.Bounds.Y));

                if ((e.State & DrawItemState.Focus) == 0)
                {
                    e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                    e.Graphics.DrawString(Convert.ToString(declarations[e.Index].Title), itemFont, Brushes.Black, new Point(objectImage.Width * 2, e.Bounds.Y));
                    e.Graphics.DrawImage(objectImage, new Point(e.Bounds.X, e.Bounds.Y));
                }
            }
        }

        private void cboObjectExplorer_Enter(object sender, EventArgs e)
        {
            PopulateObjectExplorer();
        }

        private void cboObjectExplorer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboObjectExplorer.SelectedIndex != -1)
            {
                DropDownItem item = declarations[cboObjectExplorer.SelectedIndex];
                txtCodeEditor.GoEnd();
                txtCodeEditor.SelectionStart = item.Position;
                txtCodeEditor.DoSelectionVisible();
                txtCodeEditor.Focus();
            }
        }

        private void mnuItemCut_Click(object sender, EventArgs e)
        {
            txtCodeEditor.Cut();
        }

        private void mnuItemCopy_Click(object sender, EventArgs e)
        {
            txtCodeEditor.Copy();
        }

        private void mnuItemPaste_Click(object sender, EventArgs e)
        {
            txtCodeEditor.Paste();
        }

        private void mnuItemSelectAll_Click(object sender, EventArgs e)
        {
            txtCodeEditor.SelectAll();
        }

        private void mnuItemUndo_Click(object sender, EventArgs e)
        {
            txtCodeEditor.Undo();
        }

        private void mnuItemRedo_Click(object sender, EventArgs e)
        {
            txtCodeEditor.Redo();
        }

        private void mnuItemFind_Click(object sender, EventArgs e)
        {
            txtCodeEditor.ShowFindDialog();
        }

        private void mnuItemReplace_Click(object sender, EventArgs e)
        {
            txtCodeEditor.ShowReplaceDialog();
        }

        private void mnuItemCommentSelected_Click(object sender, EventArgs e)
        {
            txtCodeEditor.CommentSelected();
        }

        private void mnuItemUncommandSlected_Click(object sender, EventArgs e)
        {
            txtCodeEditor.CommentSelected();
        }

        #endregion
    }
}

