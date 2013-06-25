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
