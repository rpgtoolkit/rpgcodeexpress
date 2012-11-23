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
using RpgCodeExpress.Files;

namespace RpgCodeExpress.Events
{
    public class NodeClickEventArgs : EventArgs
    {
        private string name;
        private string path;
        private ProjectFile item;

        #region Public Properties

        /// <summary>
        /// Gets the ProjectFile.
        /// </summary>
        public ProjectFile File
        {
            get
            {
                return item;
            }
        }

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        public string FilePath
        {
            get
            {
                return path;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the data when the node is clicked.
        /// </summary>
        /// <param name="projectFile">The ProjectFile clicked.</param>
        public NodeClickEventArgs(ProjectFile projectFile)
        {
            item = projectFile;
            this.name = projectFile.FileName;
            this.path = projectFile.FileLocation;
        }

        #endregion
    }
}
