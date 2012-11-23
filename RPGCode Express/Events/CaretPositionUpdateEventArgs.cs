/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
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
