/*
 ********************************************************************
 * RPGCode Express Version 1.0
 * This file copyright (C) 2012-2013 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

using RpgCodeExpress.Utilities;

namespace RpgCodeExpress.Files
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationFile : SerializableData
    {
        private string toolkitPath;
        private string projectTitle;
        private string projectPath;

        #region Public Properties

        public string ToolkitPath
        {
            get
            {
                return toolkitPath;
            }
            set
            {
                toolkitPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the path of the project.
        /// </summary>
        public string ProjectFolder
        {
            get
            {
                return projectPath;
            }
            set
            {
                projectPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the projects name.
        /// </summary>
        public string ProjectName
        {
            get
            {
                return projectTitle;
            }
            set
            {
                projectTitle = value;
            }
        }

        #endregion

    }
}
