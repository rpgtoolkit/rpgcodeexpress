using System;

namespace RpgCodeExpress.Events
{
    public class NodeLabelRenameEventArgs : EventArgs
    {
        private string oldFilePath;
        private string newFilePath;

        #region Properties

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

        #region Methods

        public NodeLabelRenameEventArgs(string oldFile, string newFile)
        {
            this.oldFilePath = oldFile;
            this.newFilePath = newFile;
        }

        #endregion
    }
}
