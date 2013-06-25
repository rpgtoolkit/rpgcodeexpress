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
using System.Drawing;
using System.Collections.Generic;

namespace RpgCodeExpress.Items
{
    public class ExplorerNode : TreeNode
    {
        private ExplorerNode parent;
        private string fileName;

        private ExplorerItemType fileType;
        private ProjectFile fileInformation = new ProjectFile();

        private List<ExplorerNode> childNodes = new List<ExplorerNode>();

        #region Public Properties

        /// <summary>
        /// Gets or sets the parent ExplorerNode.
        /// </summary>
        public ExplorerNode ParentNode
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        public string File
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AbsolutePath
        {
            get
            {
                string absolutePath = null;
                List<string> path = new List<string>();

                this.BuildAbsolutePath(path);

                for (int i = path.Count - 1; i >= 0; i--)
                {
                    absolutePath += path[i];

                    if (i > 0)
                    {
                        absolutePath += @"\";
                    }
                }

                return absolutePath;
            }
        }

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

        public List<ExplorerNode> Children
        {
            get
            {
                return childNodes;
            }
        }

        #endregion

        #region Public Constructors

        public ExplorerNode()
        {
            
        }

        public ExplorerNode(ExplorerNode parent, string fileName)
        {
            this.parent = parent;
            this.fileName = fileName;
        }

        #endregion

        #region Public Methods

      

        #endregion

        #region Private Methods

        private void BuildAbsolutePath(List<string> path)
        {
            path.Add(this.fileName);

            if (this.parent != null)
            {
                this.ParentNode.BuildAbsolutePath(path);
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
