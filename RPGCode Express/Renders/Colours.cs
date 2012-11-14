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
using System.Drawing;

namespace RpgCodeExpress.Renders
{
    public class Colours
    {
        public Color HorizontalGrayBlue = Color.FromArgb(255, 233, 236, 250);
        public Color HorizontalWhite = Color.FromArgb(255, 244, 247, 252);
        public Color Submenu = Color.FromArgb(255, 240, 240, 240);
        public Color ImageMarginBlue = Color.FromArgb(255, 212, 216, 230);
        public Color ImageMarginWhite = Color.FromArgb(255, 244, 247, 252);
        public Color ImageMarginLine = Color.FromArgb(255, 160, 160, 180);
        public Color SelectedBlue = Color.FromArgb(255, 186, 228, 246);
        public Color SelectedHeaderBlue = Color.FromArgb(255, 146, 202, 230);
        public Color SelectedWhite = Color.FromArgb(255, 241, 248, 251);
        public Color SelectedBorder = Color.FromArgb(255, 150, 217, 249);
        public Color SelectedDropBlue = Color.FromArgb(255, 139, 195, 225);
        public Color SelectedDropBorder = Color.FromArgb(255, 48, 127, 177);
        public Color MenuBorder = Color.FromArgb(255, 160, 160, 160);
        public Color CheckBackground = Color.FromArgb(255, 206, 237, 250);

        public Color VerticalGrayBlue = Color.FromArgb(255, 196, 203, 219);
        public Color VerticalWhite = Color.FromArgb(255, 250, 250, 253);
        public Color VerticalShadow = Color.FromArgb(255, 181, 190, 206);

        public Color BlueToolstripButtonGradient = Color.FromArgb(255, 129, 192, 224);
        public Color WhiteToolstripButtonGradient = Color.FromArgb(255, 237, 248, 253);
        public Color ToolstripButtonBorder = Color.FromArgb(255, 41, 153, 255);
        public Color BlueToolstripButtonGradientPressed = Color.FromArgb(255, 124, 177, 204);
        public Color WhiteToolstripButtonGradientPressed = Color.FromArgb(255, 228, 245, 252);

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="diameter"></param>
        /// <param name="color"></param>
        public void DrawRoundedRectangle(Graphics graphics, int xAxis, int yAxis, int width, int height, int diameter, Color color)
        {
            Pen pen = new Pen(color);

            RectangleF baseRectangle = new RectangleF(xAxis, yAxis, width, height);
            RectangleF arcRectangle = new RectangleF(baseRectangle.Location, new SizeF(diameter, diameter));

            //Top left arc
            graphics.DrawArc(pen, arcRectangle, 180, 90);
            graphics.DrawLine(pen, xAxis + Convert.ToInt32(diameter / 2), yAxis, xAxis + width - Convert.ToInt32(diameter / 2), yAxis);

            //Top right arc
            arcRectangle.X = baseRectangle.Right - diameter;
            graphics.DrawArc(pen, arcRectangle, 270, 90);
            graphics.DrawLine(pen, xAxis + width, yAxis + Convert.ToInt32(diameter / 2), xAxis + width, yAxis + height - Convert.ToInt32(diameter / 2));

            //Bottom right arc
            arcRectangle.Y = baseRectangle.Bottom - diameter;
            graphics.DrawArc(pen, arcRectangle, 0, 90);
            graphics.DrawLine(pen, xAxis + Convert.ToInt32(diameter / 2), yAxis + height, xAxis + width - Convert.ToInt32(diameter / 2), yAxis + height);

            //Bottom left arc
            arcRectangle.X = baseRectangle.Left;
            graphics.DrawArc(pen, arcRectangle, 90, 90);
            graphics.DrawLine(pen, xAxis, yAxis + Convert.ToInt32(diameter / 2), xAxis, yAxis + height - Convert.ToInt32(diameter / 2));
        }

        #endregion
    }
}
