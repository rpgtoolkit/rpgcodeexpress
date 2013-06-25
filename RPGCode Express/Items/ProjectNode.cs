/*
 ********************************************************************
 * RPGCode Express Version 1.0
 * This file copyright (C) 2012-2013 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

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
