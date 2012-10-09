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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using RPGCode_Express.Classes.Renders;
using System.Windows.Forms;

namespace RPGCode_Express.Classes.Renders
{
    class ToolstripRender : System.Windows.Forms.ToolStripProfessionalRenderer
    {
        Colours Colours = new Colours();

        #region Methods

        //Render button selected and pressed state
        protected override void OnRenderButtonBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            base.OnRenderButtonBackground(e);
            ToolStripButton button = (ToolStripButton)e.Item;

            if (button.Selected | button.Checked)
            {
                Rectangle rectangleBorder = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
                Rectangle rectangle = new Rectangle(1, 1, e.Item.Width - 2, e.Item.Height - 2);
                LinearGradientBrush gradientBrush = new LinearGradientBrush(rectangle, Colours.WhiteToolstripButtonGradient,
                    Colours.BlueToolstripButtonGradient, LinearGradientMode.Vertical);
                SolidBrush solidBrush = new SolidBrush(Colours.ToolstripButtonBorder);

                e.Graphics.FillRectangle(solidBrush, rectangleBorder);
                e.Graphics.FillRectangle(gradientBrush, rectangle);
            }

            if (button.Pressed)
            {
                Rectangle rectangleBorder = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
                Rectangle rectangle = new Rectangle(1, 1, e.Item.Width - 2, e.Item.Height - 2);
                LinearGradientBrush gradientBrush = new LinearGradientBrush(rectangle, Colours.WhiteToolstripButtonGradientPressed,
                    Colours.BlueToolstripButtonGradientPressed, LinearGradientMode.Vertical);
                SolidBrush solidBrush = new SolidBrush(Colours.ToolstripButtonBorder);

                e.Graphics.FillRectangle(solidBrush, rectangleBorder);
                e.Graphics.FillRectangle(gradientBrush, rectangle);
            }
        }

        //Render container background gradient
        protected override void OnRenderToolStripBackground(System.Windows.Forms.ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBackground(e);

            LinearGradientBrush gradientBrush = new LinearGradientBrush(e.AffectedBounds, Colours.VerticalWhite, Colours.VerticalGrayBlue,
                LinearGradientMode.Vertical);
            SolidBrush shadow = new SolidBrush(Colours.VerticalShadow);
            Rectangle rectangle = new Rectangle(0, e.ToolStrip.Height - 2, e.ToolStrip.Width, 1);

            e.Graphics.FillRectangle(gradientBrush, e.AffectedBounds);
            e.Graphics.FillRectangle(shadow, rectangle);
        }

        #endregion

    }
}
