using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RpgCodeExpress.Items
{
    class FolderNode : ExplorerNode
    {
        #region Public Constructors

        public FolderNode()
        {

        }

        public FolderNode(ExplorerNode parent, string fileName) : base(parent, fileName)
        {
            this.Text = fileName;
            this.ImageIndex = 1;
            this.SelectedImageIndex = this.ImageIndex;
            this.NodeFont = new Font(ProjectExplorer.DefaultFont, FontStyle.Regular);
        }

        #endregion
    }
}
