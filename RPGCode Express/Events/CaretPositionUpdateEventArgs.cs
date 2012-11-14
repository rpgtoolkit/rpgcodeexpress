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

using System;

namespace RpgCodeExpress.Events
{
    /// <summary>
    /// Provides data for the FastColoredTextBoxNS.FastColoredTextBox.CaretUpdated event.
    /// </summary>
    public class CaretPositionUpdateEventArgs : EventArgs
    {
        private int lineNumber;
        private int columnNumber;
        private int characterNumber;

        #region Public Properties

        /// <summary>
        /// Gets the current character number.
        /// </summary>
        public int CurrentCharacter
        {
            get
            {
                return characterNumber + 1;
            }
        }

        /// <summary>
        /// Gets the current column number.
        /// </summary>
        public int CurrentColumn
        {
            get
            {
                return columnNumber + 1;
            }
        }

        /// <summary>
        /// Get the current line number.
        /// </summary>
        public int CurrentLine
        {
            get
            {
                return lineNumber + 1;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the data when the Caret is moved.
        /// </summary>
        /// <param name="line">Line number.</param>
        /// <param name="column">Column number.</param>
        /// <param name="character">Character number.</param>
        public CaretPositionUpdateEventArgs(int line, int column, int character)
        {
            this.lineNumber = line;
            this.columnNumber = column;
            this.characterNumber = character;
        }

        #endregion
    }
}
