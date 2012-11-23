/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using RpgCodeExpress.Utilities;

namespace RpgCodeExpress.RpgCode
{
    /// <summary>
    /// 
    /// </summary>
    public class RPGcode : SerializableData
    {
        private Dictionary<string, Command> dictionary = new Dictionary<string, Command>();
        [XmlIgnore()]public ArrayList Items = new ArrayList();

        #region Public Properties

        /// <summary>
        /// Gets or sets the RPGCode commands.
        /// </summary>
        public Command[] Commands
        {
            get
            {
                Command[] commands = new Command[Items.Count - 1];
                Items.CopyTo(commands);
                return commands;
            }
            set
            {
                Items.Clear();
                if(value != null)
                {
                    foreach (Command command in value)
                    {
                        dictionary.Add(command.Name.ToLower(), command);
                        Items.Add(command);
                    }
                }
            }
        }

        #endregion

        #region Pubic Methods

        /// <summary>
        /// Finds a command.
        /// </summary>
        /// <param name="code">Name of the command.</param>
        /// <returns>Commands data.</returns>
        public Command FindCommand(string code)
        {
            if (dictionary.ContainsKey(code.ToLower()))
                return dictionary[code.ToLower()];
            else
                return null;
        }

        #endregion

    }
}
