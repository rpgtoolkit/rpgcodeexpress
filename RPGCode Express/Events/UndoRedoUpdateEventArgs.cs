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
    /// Provides data for the FastColoredTextBoxNS.FastColoredTextBox.UndoRedoUpdated event.
    /// </summary>
    public class UndoRedoUpdateEventArgs : EventArgs
    {
        private bool undo;
        private bool redo;

        #region Public Properties

        /// <summary>
        /// Gets or sets the redo state (true or false).
        /// </summary>
        public bool RedoState
        {
            get
            {
                return redo;
            }
            set
            {
                redo = value;
            }
        }

        /// <summary>
        /// Gets or sets the undo state (true or false).
        /// </summary>
        public bool UndoState
        {
            get
            {
                return undo;
            }
            set
            {
                undo = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the data when the undo/redo state is changed.
        /// </summary>
        /// <param name="undoValue"></param>
        /// <param name="redoValue"></param>
        public UndoRedoUpdateEventArgs(bool undoValue, bool redoValue)
        {
            this.undo = undoValue;
            this.redo = redoValue;
        }

        #endregion
    }
}
