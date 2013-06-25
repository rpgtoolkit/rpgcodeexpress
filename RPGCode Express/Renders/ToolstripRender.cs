/*
 ********************************************************************
 * RPGCode Express Version 1.0
 * This file copyright (C) 2012-2013 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RpgCodeExpress.Renders
{
    class ToolstripRender : System.Windows.Forms.ToolStripProfessionalRenderer
    {
        /* Generate public property */
        public Colours Colours = new Colours();

        #region Protected Methods

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
