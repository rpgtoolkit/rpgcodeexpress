/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012  Joshua Michael Daly
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

using System.ComponentModel;

namespace RpgCodeExpress.Files
{
    public class ProjectFile
    {
        private string fileName;
        private string fileLocation;

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Information")]
        [DisplayNameAttribute("Full Path")]
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
        /// 
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

        #region Methods 

        public ProjectFile(string file, string path)
        {
            this.fileName = file;
            this.fileLocation = path;
        }

        public ProjectFile()
        {

        }

        #endregion
    }
}
