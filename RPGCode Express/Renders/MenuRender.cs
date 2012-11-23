/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012 Joshua Michael Daly
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
    /// <summary>
    /// 
    /// </summary>
    class MenuRender : System.Windows.Forms.ToolStripRenderer
    {
        /* Generate public property */
        public Colours Colours = new Colours();

        #region Protected Methods

        protected override void Initialize(System.Windows.Forms.ToolStrip toolStrip)
        {
            base.Initialize(toolStrip);
            toolStrip.ForeColor = Color.Black;
        }

        //Make sure the textcolor is black
        protected override void InitializeItem(System.Windows.Forms.ToolStripItem item)
        {
            base.InitializeItem(item);
            item.ForeColor = Color.Black;
        }

        //Render arrow
        protected override void OnRenderArrow(System.Windows.Forms.ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = Color.Black;
            base.OnRenderArrow(e);
        }

        //Render Image Margin and gray item background
        protected override void OnRenderImageMargin(System.Windows.Forms.ToolStripRenderEventArgs e)
        {
            base.OnRenderImageMargin(e);

            //Draw ImageMargin background gradient
            LinearGradientBrush brush = new LinearGradientBrush(e.AffectedBounds, Colours.ImageMarginWhite, Colours.ImageMarginBlue,
                LinearGradientMode.Horizontal);

            //Shadow at the right of the image margin
            SolidBrush darkLine = new SolidBrush(Colours.ImageMarginLine);
            SolidBrush whiteLine = new SolidBrush(Color.White);
            Rectangle rectangle = new Rectangle(e.AffectedBounds.Width, 2, 1, e.AffectedBounds.Height);
            Rectangle rectangle2 = new Rectangle(e.AffectedBounds.Width + 1, 2, 1, e.AffectedBounds.Height);

            //Gray background
            SolidBrush submenuBackgroundBrush = new SolidBrush(Colours.Submenu);
            Rectangle rectangle3 = new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height);

            //Border
            Pen borderPen = new Pen(Colours.MenuBorder);
            Rectangle rectangle4 = new Rectangle(0, 1, e.ToolStrip.Width - 1, e.ToolStrip.Height - 2);

            e.Graphics.FillRectangle(submenuBackgroundBrush, rectangle3);
            e.Graphics.FillRectangle(brush, e.AffectedBounds);
            e.Graphics.FillRectangle(darkLine, rectangle);
            e.Graphics.FillRectangle(whiteLine, rectangle2);
            e.Graphics.DrawRectangle(borderPen, rectangle4);
        }

        protected override void OnRenderItemCheck(System.Windows.Forms.ToolStripItemImageRenderEventArgs e)
        {
            base.OnRenderItemCheck(e);

            if (e.Item.Selected)
            {
                Rectangle rectangle = new Rectangle(3, 1, 20, 20);
                Rectangle rectangle2 = new Rectangle(4, 2, 18, 18);
                SolidBrush brush = new SolidBrush(Colours.ToolstripButtonBorder);
                SolidBrush brush2 = new SolidBrush(Colours.CheckBackground);

                e.Graphics.FillRectangle(brush, rectangle);
                e.Graphics.FillRectangle(brush2, rectangle2);
                e.Graphics.DrawImage(e.Image, new Point(5, 3));
            }
            else
            {
                Rectangle rectangle = new Rectangle(3, 1, 20, 20);
                Rectangle rectangle2 = new Rectangle(4, 2, 18, 18);
                SolidBrush brush = new SolidBrush(Colours.SelectedDropBorder);
                SolidBrush brush2 = new SolidBrush(Colours.CheckBackground);

                e.Graphics.FillRectangle(brush, rectangle);
                e.Graphics.FillRectangle(brush2, rectangle2);
                e.Graphics.DrawImage(e.Image, new Point(5, 3));
            }
        }

        //Render Menuitem background: lightblue if selected, darkblue if dropped down
        protected override void OnRenderMenuItemBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            base.OnRenderMenuItemBackground(e);

            if (e.Item.Enabled)
            {
                if (e.Item.IsOnDropDown == false && e.Item.Selected)
                {
                    //If the item is a MenuHeader and selected: draw darkblue border
                    Rectangle rectangle = new Rectangle(3, 2, e.Item.Width - 6, e.Item.Height - 4);
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(rectangle, Colours.SelectedWhite, Colours.SelectedHeaderBlue, LinearGradientMode.Vertical);

                    e.Graphics.FillRectangle(gradientBrush, rectangle);
                    Colours.DrawRoundedRectangle(e.Graphics, rectangle.Left - 1, rectangle.Top - 1, rectangle.Width, rectangle.Height + 1, 4, Colours.ToolstripButtonBorder);
                    Colours.DrawRoundedRectangle(e.Graphics, rectangle.Left - 2, rectangle.Top - 2, rectangle.Width + 2, rectangle.Height + 3, 4, Color.White);
                    e.Item.ForeColor = Color.Black;
                }
                else if (e.Item.IsOnDropDown && e.Item.Selected)
                {
                    //If the item is NOT a MenuHeader (but a subitem) and selected: draw lightblue border
                    Rectangle rectangle = new Rectangle(4, 2, e.Item.Width - 6, e.Item.Height - 4);
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(rectangle, Colours.SelectedWhite, Colours.SelectedBlue, LinearGradientMode.Vertical);

                    e.Graphics.FillRectangle(gradientBrush, rectangle);
                    Colours.DrawRoundedRectangle(e.Graphics, rectangle.Left - 1, rectangle.Top - 1, rectangle.Width, rectangle.Height + 1, 6, Colours.SelectedBorder);
                    e.Item.ForeColor = Color.Black;
                }

                ToolStripMenuItem menuItem = (ToolStripMenuItem)e.Item;

                if (menuItem.DropDown.Visible & menuItem.IsOnDropDown == false)
                {
                    Rectangle rectangle = new Rectangle(3, 2, e.Item.Width - 6, e.Item.Height - 4);
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(rectangle, Color.White, Colours.SelectedDropBlue, LinearGradientMode.Vertical);

                    e.Graphics.FillRectangle(gradientBrush, rectangle);
                    Colours.DrawRoundedRectangle(e.Graphics, rectangle.Left - 1, rectangle.Top - 1, rectangle.Width, rectangle.Height + 1, 4, Colours.SelectedDropBorder);
                    Colours.DrawRoundedRectangle(e.Graphics, rectangle.Left - 2, rectangle.Top - 2, rectangle.Width + 2, rectangle.Height + 3, 4, Color.White);
                    e.Item.ForeColor = Color.Black;
                }
            }
        }

        //Render separator
        protected override void OnRenderSeparator(System.Windows.Forms.ToolStripSeparatorRenderEventArgs e)
        {
            base.OnRenderSeparator(e);

            SolidBrush darkLine = new SolidBrush(Colours.ImageMarginLine);
            SolidBrush whiteLine = new SolidBrush(Color.White);
            Rectangle rectangle = new Rectangle(32, 3, e.Item.Width - 32, 1);
            Rectangle rectangle2 = new Rectangle(32, 4, e.Item.Width - 32, 1);

            e.Graphics.FillRectangle(darkLine, rectangle);
            e.Graphics.FillRectangle(whiteLine, rectangle2);
        }

        //Render horizontal background gradient
        protected override void OnRenderToolStripBackground(System.Windows.Forms.ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBackground(e);

            LinearGradientBrush brush = new LinearGradientBrush(e.AffectedBounds, Colours.HorizontalGrayBlue, Colours.HorizontalWhite,
                LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, e.AffectedBounds);
        }

        #endregion

    }
}
