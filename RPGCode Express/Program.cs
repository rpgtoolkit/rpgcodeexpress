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
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMdi());
        }
    }
}
