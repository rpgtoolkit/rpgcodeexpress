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
        private bool initialLoad;

        private Point lastMouseCoordinate;
        private AutocompleteMenu popupMenu; // Intellisense menu
        private RPGcode rpgCodeReference = new RPGcode(); // List of RPGCode Functions, centralise this.
        private Autocomplete autocompleteItems = new Autocomplete(); // Intellisense Items

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

            initialLoad = true;

            txtCodeEditor.DescriptionFile = Application.StartupPath + @"\Resources\RPGCodeHighlighter.xml";
            txtCodeEditor.Language = Language.Custom;

            rpgCodeReference = currentRpgCode;
            EditorFile = file;

            this.TabText = Path.GetFileNameWithoutExtension(EditorFile);
            txtCodeEditor.AddStyle(new MarkerStyle(new SolidBrush(Color.FromArgb(50, Color.Gray))));

            if (EditorFile != "Untitled")
            {
                ReadFile();
            }

            popupMenu = new AutocompleteMenu(txtCodeEditor); // Set autocompletemenu's text source.
            popupMenu.Opening += this.popupMenu_Opening; // Override the menu's Opening event.
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
            if (EditorFile == "Untitled") // The file has no path.
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
                    MessageBox.Show(ex.Message, Application.ExecutablePath, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Add all the classes to autocompletemenu
            autocompleteItems.UserDefinedClasses.Clear();
            autocompleteItems.UserDefinedClasses.AddRange(GetDefinedClasses(txtCodeEditor.Text));
            autocompleteItems.UserDefinedClasses.AddRange(autocompleteItems.IncludedClasses);

            // Add all the methods to autocompletemenu
            autocompleteItems.UserDefinedMethods.Clear();
            autocompleteItems.UserDefinedMethods.AddRange(GetDefinedMethods(txtCodeEditor.Text));
            autocompleteItems.UserDefinedMethods.AddRange(autocompleteItems.IncludedMethods);

            autocompleteItems.UserDefinedGlobals.Clear();
            autocompleteItems.UserDefinedGlobals.AddRange(GetDefinedGlobalVariables(txtCodeEditor.Text));
            autocompleteItems.UserDefinedGlobals.AddRange(autocompleteItems.IncludedGlobals);

            autocompleteItems.UserDefinedLocals.Clear();
            autocompleteItems.UserDefinedLocals.AddRange(GetDefinedLocalVariables(txtCodeEditor.Text));

            autocompleteItems.UserDefinedMembers.Clear();
            autocompleteItems.UserDefinedMembers.AddRange(GetDefinedClassMembers(txtCodeEditor.Text));
            autocompleteItems.UserDefinedMembers.AddRange(autocompleteItems.IncludedMembers);
        }

        /// <summary>
        /// Looks for included files, and adds a reference to them.
        /// </summary>
        private void AddFileIncludes()
        {
            Regex expression = new Regex(@"^(?<range>[\w\s]*#?\b(include)\s*\(?"".+""\)?)", 
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (Match match in expression.Matches(txtCodeEditor.Text))
            {
                string fileInclude = match.Value.Trim();
                int position = fileInclude.IndexOf('"') + 1;

                // Get the files name from in between the quotes "...".
                fileInclude = fileInclude.Substring(position, fileInclude.LastIndexOf('"') - position);
                fileInclude = ProjectPath + fileInclude;

                if (!File.Exists(fileInclude))
                {
                    continue; // File doesn't exist.
                }
                else if (fileIncludes.ContainsKey(fileInclude))
                {
                    continue; // We already have it indexed.
                }
                else
                {
                    ReadIncludedFile(fileInclude);
                }
            }
        }

        /// <summary>
        /// Populates the code editor's auto complete menu, it runs through a series of loops
        /// assigning data to the individual AutocompleteItems.
        /// </summary>
        private void BuildAutocompleteMenu()
        {
            List<AutocompleteItem> items = new List<AutocompleteItem>();

            foreach (string item in autocompleteItems.StatementSnippets)
                items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 1 });

            foreach (string item in autocompleteItems.DeclartionSnippets)
                items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 1 });

            foreach (string item in autocompleteItems.Keywords)
                items.Add(new AutocompleteItem(item) { ImageIndex = 1 });

            foreach (string item in autocompleteItems.Constants)
                items.Add(new AutocompleteItem(item) { ImageIndex = 3 });

            foreach (RpgCodeExpress.RpgCode.Command command in rpgCodeReference.Items)
            {
                SnippetAutocompleteItem menuItem = new SnippetAutocompleteItem(command.Code);
                menuItem.ToolTipText = command.Description;
                menuItem.ToolTipTitle = command.Tooltip;
                menuItem.MenuText = command.Code.Substring(0, command.Code.Length - 3); // Add a "Name" to xml file.
                menuItem.ImageIndex = 0;
                items.Add(menuItem);
            }

            foreach (string definedClass in autocompleteItems.UserDefinedClasses)
                items.Add(new AutocompleteItem(definedClass) { ImageIndex = 5 });

            foreach (string method in autocompleteItems.UserDefinedMethods)
                items.Add(new SnippetAutocompleteItem(method) { ImageIndex = 0 });

            foreach (string global in autocompleteItems.UserDefinedGlobals)
                items.Add(new AutocompleteItem(global) { ImageIndex = 4 });

            foreach (string local in autocompleteItems.UserDefinedLocals)
                items.Add(new AutocompleteItem(local) { ImageIndex = 7 });

            foreach (string member in autocompleteItems.UserDefinedMembers)
                items.Add(new AutocompleteItem(member) { ImageIndex = 6 });

            // Sort them descending, alphabetically.
            items.Sort(delegate(AutocompleteItem item1, AutocompleteItem item2)
            {
                return item1.Text.CompareTo(item2.Text);
            });

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
            autocompleteItems.IncludedGlobals.Clear();
            autocompleteItems.IncludedMembers.Clear();
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
        /// Get all of defined classes in this RPGCode program for use in the autocomplete menu.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        /// <returns>A List of all the defined classes.</returns>
        private List<string> GetDefinedClasses(string text)
        {
            List<string> classes = new List<string>();
            MatchCollection matches = FindClasses(text);

            foreach (Match match in matches)
            {
                string definedClass = match.Value.Trim();
                
                // Get the classes name.
                definedClass = definedClass.Substring(definedClass.LastIndexOf(" ")).Trim(); 

                if (!classes.Contains(definedClass))
                {
                    classes.Add(definedClass);
                }
            }

            return classes;
        }

        /// <summary>
        /// Finds all of the classes in a string of RPGCode, and returns any matches.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        /// <returns>A collection of matches.</returns>
        private MatchCollection FindClasses(string text)
        {
            Regex expression = new Regex(@"^[\s]*\b(class|struct)[^\S\n]+(?<range>\w+)\b", 
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            return expression.Matches(text);
        }

        /// <summary>
        /// Get all of defined methods in this RPGCode program for use in the autocomplete menu.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        /// <returns>A List of all the defined methods.</returns>
        private List<string> GetDefinedMethods(string text)
        {
            List<string> methods = new List<string>();
            MatchCollection matches = FindMethods(text);

            foreach (Match match in matches)
            {
                string definedMethod = match.Value.Trim();

                if (definedMethod.Contains("::"))
                {
                    definedMethod = definedMethod.Substring(definedMethod.LastIndexOf("::") + 2).Trim();
                }
                else
                {
                    definedMethod = definedMethod.Substring(definedMethod.LastIndexOf(" ")).Trim();
                }

                definedMethod += "(^)";

                if (!methods.Contains(definedMethod))
                {
                    methods.Add(definedMethod);
                }
            }

            return methods;
        }

        /// <summary>
        /// Finds all of the methods in a string of RPGCode, and returns any matches.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        /// <returns>A collection of matches.</returns>
        private MatchCollection FindMethods(string text)
        {
            List<string> methods = new List<string>();

            Regex expression = new Regex(@"^[\s]*\b(method|function)[^\S\n]+(\w+::)?(?<range>[~]?\w+?)\b", 
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            return expression.Matches(text);
        }

        /// <summary>
        /// Get all of defined globals in this RPGCode program for use in the autocomplete menu.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        /// <returns>A List of all the defined globals.</returns>
        private List<string> GetDefinedGlobalVariables(string text)
        {
            List<string> globalVariables = new List<string>();
            MatchCollection matches = FindGlobalVariables(text);

            foreach (Match match in matches) //Find the global variables i.e. "global(example)"
            {
                // Figure out how to access groups based on name.
                string definedVariable = match.Groups[3].Value;

                if (!globalVariables.Contains(definedVariable))
                {
                    globalVariables.Add(definedVariable);
                }
            }

            return globalVariables;
        }

        /// <summary>
        /// Finds all of the global variables in a string of RPGCode, and returns any matches.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        /// <returns>A collection of matches.</returns>
        private MatchCollection FindGlobalVariables(string text)
        {
            List<string>  globalVariables = new List<string>();

            Regex expression = new Regex(@"^([\s]*\b(global)[^\S\n]*\(+\s*(?<range>\w+)\s*\)+)", 
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            return expression.Matches(text);
        }

        /// <summary>
        /// Get all of defined locals in this RPGCode program for use in the autocomplete menu.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        /// <returns>A List of all the defined locals.</returns>
        private List<string> GetDefinedLocalVariables(string text)
        {
            List<string> localVariables = new List<string>();
            MatchCollection matches = FindLocalVariables(text);

            foreach (Match match in matches)
            {
                // Figure out how to access groups based on name.
                string definedVariable = match.Groups[3].Value;

                if (!localVariables.Contains(definedVariable))
                {
                    localVariables.Add(definedVariable);
                }
            }

            return localVariables;
        }

        /// <summary>
        /// Finds all of the local variables in a string of RPGCode, and returns any matches.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        /// <returns>A collection of matches.</returns>
        private MatchCollection FindLocalVariables(string text)
        {
            List<string> localVariables = new List<string>();

            Regex expression = new Regex(@"^([\s]*\b(local)[^\S\n]*\(+\s*(?<range>\w+)\s*\)+)",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            return expression.Matches(text);
        }

        /// <summary>
        /// Get all of defined class members in this RPGCode program for use in the autocomplete menu.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        /// <returns>A List of all the defined class members.</returns>
        private List<string> GetDefinedClassMembers(string text)
        {
            List<string> classMembers = new List<string>();
            MatchCollection matches = FindClassMembers(text);

            foreach (Match match in matches) //Find class memebers i.e. "var example"
            {
                string definedVariable = match.Value.Trim();

                definedVariable = definedVariable.Substring(definedVariable.LastIndexOf(" ")).Trim();

                if (!classMembers.Contains(definedVariable))
                {
                    classMembers.Add(definedVariable);
                }
            }

            return classMembers;
        }

        /// <summary>
        /// Finds all of the member variables in a string of RPGCode, and returns any matches.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        /// <returns>A collection of matches.</returns>
        private MatchCollection FindClassMembers(string text)
        {
            Regex expression = new Regex(@"^[\s]*\b(var)[^\S\n]+(?<range>\w+)\b", 
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            return expression.Matches(text);
        }

        /// <summary>
        /// Populates the Class Explorer combobox, taking all of the defined class declarations from the current
        /// document using a regular expression.
        /// </summary>
        private void PopulateClassExplorer()
        {
            classes.Clear();
            cboClassExplorer.Items.Clear();

            MatchCollection matches = FindClasses(txtCodeEditor.Text);

            foreach (Match match in matches)
            {
                string definedClass = match.Value.Trim();

                DropDownItem item = new DropDownItem();
                item.Title = definedClass.Substring(definedClass.LastIndexOf(" ")).Trim();
                item.Position = match.Index; //Store the classes position in the code editor

                classes.Add(item);
                cboClassExplorer.Items.Add(item.Title);
            }

            // Sort them descending, alphabetically.
            classes.Sort(delegate(DropDownItem item1, DropDownItem item2) 
            { 
                return item1.Title.CompareTo(item2.Title); 
            });
        }

        /// <summary>
        /// Populates the Object Explorer by calling a number of sub-methods, it then sorts the results
        /// from these methods alphabetically.
        /// </summary>
        private void PopulateObjectExplorer()
        {
            declarations.Clear();
            cboObjectExplorer.Items.Clear();

            AddMethodsToObjectExplorer();
            AddGlobalsToObjectExplorer();
            AddMembersToObjectExplorer();

            // Sort them descending, alphabetically. 
            declarations.Sort(delegate(DropDownItem item1, DropDownItem item2) 
            { 
                return item1.Title.CompareTo(item2.Title); 
            });
        }

        /// <summary>
        /// Adds all of the methods declared in this RPGCode program to the Object Explorer.
        /// </summary>
        private void AddMethodsToObjectExplorer()
        {
            // We need to use a different Regex when matching methods for the Object Explorer,
            // because we want to get the parameter lists.
            Regex expression = new Regex(@"^(?<range>[\w\s]*\b(method|function)[^\S\n]+(\w+::)?([~]?\w+\(.*\)\s+))",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

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
        }

        /// <summary>
        /// Adds all of the globals declared in this RPGCode program to the Object Explorer.
        /// </summary>
        private void AddGlobalsToObjectExplorer()
        {
            MatchCollection matches = FindGlobalVariables(txtCodeEditor.Text);

            foreach (Match match in matches)
            {
                DropDownItem item = new DropDownItem();
                // Figure out how to access groups based on name.
                string variable = match.Groups[3].Value;

                item.Type = DropDownType.Global;
                item.Position = match.Index;
                item.Title = variable;

                declarations.Add(item);
                cboObjectExplorer.Items.Add(item.Title);
            }
        }

        /// <summary>
        /// Adds all of the members declared in this RPGCode program to the Object Explorer.
        /// </summary>
        private void AddMembersToObjectExplorer()
        {
            MatchCollection matches = FindClassMembers(txtCodeEditor.Text);

            foreach (Match match in matches)
            {
                string variable = match.Value.Trim();

                DropDownItem item = new DropDownItem();
                item.Type = DropDownType.Var;
                item.Position = match.Index;
                item.Title = variable.Substring(variable.LastIndexOf(" ")).Trim();

                declarations.Add(item);
                cboObjectExplorer.Items.Add(item.Title);
            }
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
                IsUpdated = true;
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
                autocompleteItems.IncludedClasses.AddRange(GetDefinedClasses(text));
                autocompleteItems.IncludedMethods.AddRange(GetDefinedMethods(text));
                autocompleteItems.IncludedGlobals.AddRange(GetDefinedGlobalVariables(text));
                autocompleteItems.IncludedMembers.AddRange(GetDefinedClassMembers(text));
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
            {
                Save();
            }

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
                IsUpdated = true;
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
            IsUpdated = true;
            txtCodeEditor.ClearUndo();
            this.TabText = Path.GetFileName(EditorFile);
        }

        private void CodeEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsUpdated)
            {
                if (UpdateFile() == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
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

            RpgCodeExpress.RpgCode.Command foundCommand = rpgCodeReference.FindCommand(hoverWord);

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
            if (!initialLoad)
            {
                CancelTooltip();
                IsUpdated = false;
                AddFileDeclarations();
                BuildAutocompleteMenu();
                this.TabText = Path.GetFileName(EditorFile) + '*';

                // Covers Multi-line C-style comments.
                FastColoredTextBoxNS.Range range;
                FastColoredTextBox textBox = (FastColoredTextBox)sender;

                range = textBox.VisibleRange;
                range.ClearStyle();
                range.SetStyle(greenStyle, "//.*$", RegexOptions.Multiline);
                range.SetStyle(greenStyle, @"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline);
                range.SetStyle(greenStyle, @"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline | RegexOptions.RightToLeft);
            }
            else
            {
                initialLoad = false;
            }
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
                        objectImage = Properties.Resources.Icons_16x16_PrivateField;
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

