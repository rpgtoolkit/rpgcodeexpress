/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012  Joshua Michael Daly
 ********************************************************************
 * This file is part of RPGCode Express Version 1.
 *
 * RPGCode Express is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * RPGCode Express is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RPGCode Express.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace RpgCodeExpress
{
    public partial class PropertiesWindow : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region methods

        public PropertiesWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set's the forms current PropertyGrid's item, to display.
        /// </summary>
        /// <param name="item">The object to display in the PropertyGrid.</param>
        public void SetGridItem(object item)
        {
            propertyGrid1.SelectedObject = item;
        }

        #endregion
    }
}
