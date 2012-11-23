/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

using WeifenLuo.WinFormsUI.Docking;

namespace RpgCodeExpress
{
    /// <summary>
    /// Redudant for the moment, more use in Toolkit Editor. Just a test really for now.
    /// </summary>
    public class EditorForm : DockContent
    {
        private bool updateNeeded;
        private string filePath;
        private string projectDirectory;

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the current file needs saving.
        /// </summary>
        public bool IsUpdated
        {
            get
            {
                return updateNeeded;
            }
            set
            {
                updateNeeded = value;
            }
        }

        /// <summary>
        /// Gets or sets the code editors current file.
        /// </summary>
        public string EditorFile
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }

        /// <summary>
        /// Gets or sets the current projects directory path.
        /// </summary>
        public string ProjectPath
        {
            get
            {
                return projectDirectory;
            }
            set
            {
                projectDirectory = value;
            }
        }

        #endregion
    }
}
