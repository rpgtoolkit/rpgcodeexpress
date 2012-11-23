/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

namespace RpgCodeExpress.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    interface ISaveable
    {
        bool Save();
        bool SaveAs();
    }
}
