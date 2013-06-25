using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgCodeExpress.Items
{
    class FileNode : ExplorerNode
    {
        public FileNode(ExplorerNode parent, string fileName)
            : base(parent, fileName)
        {
            this.Text = fileName;
            this.ImageIndex = 3;
            this.SelectedImageIndex = this.ImageIndex;
        }
    }
}
