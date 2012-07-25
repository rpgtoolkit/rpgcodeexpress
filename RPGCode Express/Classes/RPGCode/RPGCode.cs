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

using System.Collections.Generic;
using System.Xml.Serialization;
using RPGCode_Express.Classes.Utilities;
using System.Collections;

namespace RPGCode_Express.Classes.RPGCode
{
    /// <summary>
    /// 
    /// </summary>
    public class RPGcode : SerializableData
    {
        private Dictionary<string, Command> dictionary = new Dictionary<string, Command>();

        [XmlIgnore()]public ArrayList Items = new ArrayList();

        /// <summary>
        /// 
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Command FindCommand(string code)
        {
            if (dictionary.ContainsKey(code.ToLower()))
                return dictionary[code.ToLower()];
            else
                return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Command : SerializableData
    {
        public string Name;
        public string Code;
        public string Tooltip;
        public string Description;
    }
}
