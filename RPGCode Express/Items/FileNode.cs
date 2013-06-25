/*
 ********************************************************************
 * RPGCode Express Version 1.0
 * This file copyright (C) 2012-2013 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */


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
