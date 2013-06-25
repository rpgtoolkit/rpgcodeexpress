/*
 ********************************************************************
 * RPGCode Express Version 1.0
 * This file copyright (C) 2012-2013 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

using System.ComponentModel;

namespace RpgCodeExpress.Files
{
    public class ProjectFile
    {
        private string fileName;
        private string fileLocation;

        #region Public Properties

        /// <summary>
        /// Gets or sets the files path.
        /// </summary>
        [CategoryAttribute("Information")]
        [DisplayNameAttribute("Full File")]
        [ReadOnlyAttribute(true)]
        [DescriptionAttribute("Location of the file or folder.")]
        public string FileLocation
        {
            get
            {
                return fileLocation;
            }
            set
            {
                fileLocation = value;
            }
        }

        /// <summary>
        /// Gets or sets the files name.
        /// </summary>
        [CategoryAttribute("Information")]
        [DisplayNameAttribute("File Name")]
        [ReadOnlyAttribute(true)]
        [Description("Name of the file or folder.")]
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

        #endregion

        #region Public Methods 

        
        /// <summary>
        /// Creates a blank ProjectFile. 
        /// </summary>
        public ProjectFile()
        {

        }

        /// <summary>
        /// Creates a ProjectFile with a path and name for use in a property grid.
        /// </summary>
        /// <param name="file">Name of the file.</param>
        /// <param name="path">File of the file.</param>
        public ProjectFile(string file, string path)
        {
            this.fileName = file;
            this.fileLocation = path;
        }

        #endregion
    }
}
