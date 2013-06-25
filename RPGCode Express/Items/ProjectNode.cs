using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RpgCodeExpress.Items
{
    class ProjectNode : ExplorerNode
    {
        #region Public Constructors

        public ProjectNode()
        {

        }

        public ProjectNode(ExplorerNode parent, string title, string fileName) : base(parent, fileName)
        {
            this.Text = title;
            this.ImageIndex = 0;
            this.NodeFont = new Font(ProjectExplorer.DefaultFont, FontStyle.Bold);
            
            this.Nodes.Add("*DUMMY*");
            this.Expand();
        }

        #endregion
    }
}
