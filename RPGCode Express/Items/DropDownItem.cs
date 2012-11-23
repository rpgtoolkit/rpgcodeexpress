/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

namespace RpgCodeExpress.Items
{
    /// <summary>
    /// 
    /// </summary>
    public class DropDownItem
    {
        public DropDownType Type;
        public string Title;
        public int Position;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DropDownType
    {
        Method, Global, Var
    }
}
