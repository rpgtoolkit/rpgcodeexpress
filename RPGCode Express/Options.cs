/*
 ********************************************************************
 * RPGCode Express Version 1.0
 * This file copyright (C) 2012-2013 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

using System;
using System.Windows.Forms;

namespace RpgCodeExpress
{
    public partial class Options : Form
    {
        private MainMdi parent;

        #region Public Properties

        public MainMdi ParentMdi
        {
            get
            {
                return parent;
            }
        }

        #endregion

        #region Public Constructors

        public Options(MainMdi parentForm)
        {
            InitializeComponent();

            parent = parentForm;

            txtToolkitPath.Text = parent.ToolkitPath;
        }

        #endregion

        #region Private Methods

        private void ShowToolkitPathDialog()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.Description = "Please select a folder containing a valid Toolkit installation.";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtToolkitPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        #endregion

        #region Private Events

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            ShowToolkitPathDialog();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtToolkitPath.Text != null)
            {
                if (txtToolkitPath.Text != parent.ToolkitPath)
                {
                    parent.UpdateToolkitPath(txtToolkitPath.Text + @"\");
                }
            }

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
