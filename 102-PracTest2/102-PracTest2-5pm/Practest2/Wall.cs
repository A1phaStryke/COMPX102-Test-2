using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Practest2
{
    /// <summary>
    /// Stores a design for a wall of a gallery.
    /// It contains a list of artworks and their positions, and a background colour
    /// </summary>
    class Wall
    {
        public Color BackgroundColour;

        public List<Artworks> artworks;

        /// <summary>
        /// Creates a wall of a gallery.
        /// It contains a list of artworks and their positions, and a background colour
        /// </summary>
        public Wall()
        {
            BackgroundColour = Color.White;
            artworks = new List<Artworks>();
        }

        /// <summary>
        /// Adds an Artwork to the gallery wall
        /// </summary>
        /// <param name="painting">painting to add to the wall</param>
        public void AddArtwork(Artworks artwork)
        {
            artworks.Add(artwork);
        }

        /// <summary>
        /// Checks if a (mouse) position is 'on' any artworks, and returns the artwork if it does.
        /// </summary>
        /// <param name="x">x position to check</param>
        /// <param name="y">y position to check</param>
        /// <returns>painting that x,y is 'on'</returns>
        public Artworks SelectArtwork(int x, int y)
        {
            foreach (Artworks a in artworks)
            {
                if (a.IsMouseOn(x, y))
                {
                    return a;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Displays the gallery wall with any artworks on it.
        /// </summary>
        /// <param name="wall">Graphics object specifying where to draw image</param>
        public void Display(Graphics wall)
        {
            wall.Clear(BackgroundColour);
            foreach (Artworks a in artworks)
            {
                a.Display(wall);
            }
        }

        public List<Artworks> FindAllArtworks(Predicate<Artworks> predicate)
        {
            return Artworks.FindAll(this.artworks, predicate);
        }

        /// <summary>
        /// Returns a string summarising this wall
        /// </summary>
        /// <returns>String describing this wall</returns>
        public override string ToString()
        {
            return "Wall has " + artworks.Count.ToString() + " Artworks.";
        }
    }
}
