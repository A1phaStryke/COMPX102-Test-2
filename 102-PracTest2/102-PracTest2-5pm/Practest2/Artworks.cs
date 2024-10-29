using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practest2
{
    /// <summary>
    /// Abstract base class for all artwork objects in the gallery
    /// </summary>
    abstract class Artworks
    {
        /// <summary>
        /// Represents the location and size of the artwork
        /// </summary>
        public Rectangle Frame;

        /// <summary>
        /// Constructor for the Artworks class
        /// </summary>
        /// <param name="x">X-coordinate of the artwork</param>
        /// <param name="y">Y-coordinate of the artwork</param>
        /// <param name="w">Width of the artwork</param>
        /// <param name="h">Height of the artwork</param>
        public Artworks(int x, int y, int w, int h) 
        {
            Frame = new Rectangle(x, y, w, h);
        }

        /// <summary>
        /// Specifies a new location and size for the artwork on a gallery wall
        /// </summary>
        /// <param name="x">Left position of artwork on wall</param>
        /// <param name="y">Bottom position of artwork on wall from ground</param>
        /// <param name="w">Width of artwork on wall</param>
        /// <param name="h">Height of artwork on wall</param>
        public virtual void Update(int x, int y, int w, int h)
        {
            Frame = new Rectangle(x, y, w, h);
        }

        /// <summary>
        /// Checks if a (mouse) position is 'on' this artwork.
        /// </summary>
        /// <param name="x">X position to check</param>
        /// <param name="y">Y position to check</param>
        /// <returns>True if x,y is inside of an artwork</returns>
        public bool IsMouseOn(int x, int y)
        {
            Rectangle newFrame = new Rectangle(Frame.X, Form1.WALL_HEIGHT - Frame.Y - Frame.Height, Frame.Width, Frame.Height);
            Point mousePos = new Point(x, y);
            return newFrame.Contains(mousePos);
        }

        /// <summary>
        /// Abstract method to display the artwork
        /// </summary>
        /// <param name="wall">Graphics object representing the wall</param>
        public abstract void Display(Graphics wall);

        /// <summary>
        /// Returns a string representation of the artwork's position and size
        /// </summary>
        /// <returns>A string with X, Y, Width, and Height values separated by commas</returns>
        public override string ToString()
        {
            return Frame.X.ToString() + "," +
                Frame.Y.ToString() + "," +
                Frame.Width.ToString() + "," +
                Frame.Height.ToString();
        }

        /// <summary>
        /// Finds all artworks that match a given predicate
        /// </summary>
        /// <param name="artworks">List of artworks to search</param>
        /// <param name="predicate">Predicate to match artworks against</param>
        /// <returns>A list of artworks that match the predicate</returns>
        public static List<Artworks> FindAll(List<Artworks> artworks, Predicate<Artworks> predicate)
        {
            // Find all artworks that match the predicate
            List<Artworks> matchingArtworks = artworks.FindAll(predicate);

            // Print matching artworks to console
            Console.WriteLine("Matching Artworks:");
            foreach (Artworks artwork in matchingArtworks)
            {
                Console.WriteLine(artwork.ToString());
            }

            return matchingArtworks;
        }
    }
}
