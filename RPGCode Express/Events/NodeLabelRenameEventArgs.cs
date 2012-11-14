﻿using System;

namespace RpgCodeExpress.Events
{
    public class NodeLabelRenameEventArgs : EventArgs
    {
        private string oldFilePath;
        private string newFilePath;

        #region Public Properties

        /// <summary>
        /// Gets or Sets the nodes old file path.
        /// </summary>
        public string OldFile
        {
            get
            {
                return oldFilePath;
            }
            set
            {
                oldFilePath = value;
            }
        }

        /// <summary>
        /// Gets or sets the nodes new file path.
        /// </summary>
        public string NewFile
        {
            get
            {
                return newFilePath;
            }
            set
            {
                newFilePath = value;
            }
        }
       
        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the data when the Node is renamed.
        /// </summary>
        /// <param name="oldFile">Old path.</param>
        /// <param name="newFile">New path.</param>
        public NodeLabelRenameEventArgs(string oldFile, string newFile)
        {
            this.oldFilePath = oldFile;
            this.newFilePath = newFile;
        }

        #endregion
    }
}
