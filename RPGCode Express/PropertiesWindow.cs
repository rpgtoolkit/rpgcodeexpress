/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

namespace RpgCodeExpress
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PropertiesWindow : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Public Methods

        /// <summary>
        /// Creates a new Properties Window.
        /// </summary>
        public PropertiesWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the forms current PropertyGrid's item, to display.
        /// </summary>
        /// <param name="item">The object to display in the PropertyGrid.</param>
        public void SetGridItem(object item)
        {
            propertyGrid1.SelectedObject = item;
        }

        #endregion

    }
}
