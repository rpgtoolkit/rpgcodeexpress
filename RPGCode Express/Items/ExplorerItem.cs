/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
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
