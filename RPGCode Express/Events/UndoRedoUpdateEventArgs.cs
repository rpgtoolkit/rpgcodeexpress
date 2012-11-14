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
