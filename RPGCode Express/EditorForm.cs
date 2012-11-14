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
