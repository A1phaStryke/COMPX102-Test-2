using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Practest2
{
    /// <summary>
    /// Stores a Pedestals location and size on the gallery wall
    /// </summary>
    class Pedestal : Artworks
    {
        public Color colour = new Color();

        /// <summary>
        /// Specifies the location and size of a Pedestal on a gallery wall
        /// </summary>
        /// <param name="x">left position of painting on wall</param>
        /// <param name="y">bottom position of painting on wall from ground</param>
        /// <param name="w">width of painting on wall</param>
        /// <param name="h">height of painting on wall</param>
        public Pedestal(int x, int w, int h, Color color, int y = 0) : base(x, y = 0, w, h)
        {
            colour = color;
        }

        /// <summary>
        /// Specifies a new location and size for the Pedestal on a gallery wall
        /// </summary>
        /// <param name="x">left position of Pedestal on wall</param>
        /// <param name="y">bottom position of Pedestal on wall from ground</param>
        /// <param name="w">width of Pedestal on wall</param>
        /// <param name="h">height of Pedestal on wall</param>
        public override void Update(int x, int w, int h, int y = 0)
        {
            base.Update(x, y = 0, w, h);
        }

        /// <summary>
        /// Displays the Pedestal on the gallery wall using size of frame.
        /// a Pedestal is represented by a rectangle with a circle on top.
        /// </summary>
        /// <param name="wall">Graphics object specifying where to draw image</param>
        public override void Display(Graphics wall)
        {
            Brush brush = new SolidBrush(colour);

            Rectangle newFrame = new Rectangle(Frame.X, Form1.WALL_HEIGHT - Frame.Y - Frame.Height, Frame.Width, Frame.Height);

            wall.FillRectangle(brush, newFrame);

            wall.DrawRectangle(Pens.Black, newFrame);

            // Create a new rectangle for the ellipse, positioned above the main rectangle
            Rectangle ellipseFrame = new Rectangle(newFrame.X, newFrame.Y - Frame.Width, newFrame.Width, Frame.Width);
            wall.DrawEllipse(Pens.Black, ellipseFrame);
        }

        /// <summary>
        /// Returns a string summarising this Pedestal
        /// format is "x,y,w,h" i.e. CSV                      
        /// </summary>
        /// <returns>String describing this Pedestal</returns>
        public override string ToString()
        {
            return Frame.X.ToString() + "," +
                "0," +
                Frame.Width.ToString() + "," +
                Frame.Height.ToString() + "," +
                colour.ToString();
        }
    }
}
