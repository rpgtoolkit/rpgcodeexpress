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
