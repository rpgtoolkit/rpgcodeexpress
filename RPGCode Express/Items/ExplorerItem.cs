﻿/*
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

using System.Windows.Forms;
using RpgCodeExpress.Files;

namespace RpgCodeExpress.Items
{

    public class ExplorerItem : TreeNode
    {
        private ExplorerItemType fileType;
        private ProjectFile fileInformation = new ProjectFile();

        #region Public Properties

        /// <summary>
        /// Gets or sets the ProjectFile.
        /// </summary>
        public ProjectFile Information
        {
            get
            {
                fileInformation.FileName = this.Text;
                fileInformation.FileLocation = this.Tag.ToString();

                return fileInformation;
            }
            set
            {
                fileInformation = value;
            }
        }

        /// <summary>
        /// Gets or sets the ExplorerItems type.
        /// </summary>
        public ExplorerItemType Type
        {
            get
            {
                return fileType;
            }
            set
            {
                fileType = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ExplorerItemType
    {
        Project, Program, File, Folder
    }
}
