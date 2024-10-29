using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Practest2
{
    public partial class Form1 : Form
    {
        // Constants
        const string CSVFILTER = "CSV files|*.csv|All files|*.*";
        public const int WALL_HEIGHT = 400;

        // Fields
        Wall wall; //the current gallery wall being designed
        Artworks currentSelect; //the currently selected artwork in the wall

        // Predicates for artwork filtering
        Predicate<Artworks> isPainting = artwork => artwork is Painting; // Predicate to check if the artwork is a painting
        Predicate<Artworks> isLargeHeight = artwork => artwork.Frame.Height > 100; // Predicate to check if the artwork's height is greater than 100

        /// <summary>
        /// Loads the form
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            pictureBoxWall.Height = WALL_HEIGHT;
        }

        /// <summary>
        /// Draws a new wall when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wall = new Wall();
            pictureBoxWall.Invalidate();  //draw empty wall

        }

        /// <summary>
        /// Loads artworks from a csv file when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wall = new Wall();
            try
            {
                openFileDialog1.Filter = CSVFILTER;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamReader infile = new StreamReader(openFileDialog1.FileName);

                    // read list of artworks
                    while (!infile.EndOfStream)
                    {
                        //data is "x,y,w,h"
                        string line = infile.ReadLine();
                        string[] data = line.Split(',');

                        if (data.Length == 4)
                        {
                            // Parse painting data and add to wall
                            int x = int.Parse(data[0]); //first data is x value
                            int y = int.Parse(data[1]); //second data is y
                            int w = int.Parse(data[2]); //third data is width
                            int h = int.Parse(data[3]); //fourth data is height
                            Artworks artwork = new Painting(x, y, w, h);
                            wall.AddArtwork(artwork);
                        }
                        else if (data.Length == 5)
                        {
                            // Parse pedestal data and add to wall
                            int x = int.Parse(data[0]); //first data is x value
                            int y = int.Parse(data[1]); //second data is y
                            int w = int.Parse(data[2]); //third data is width
                            int h = int.Parse(data[3]); //fourth data is height
                            Color color;

                            if (comboBox1.Text == "White")
                            {
                                color = Color.White;
                            }
                            else
                            {
                                color = Color.Black;
                            }

                            Artworks artwork = new Pedestal(x, w, h, color);
                            wall.AddArtwork(artwork);
                        }
                        else //Invalid data at line
                        {
                            MessageBox.Show("Invalid data on line:" + line);
                        }

                    }
                    infile.Close(); //be a tidy kiwi
                    pictureBoxWall.Invalidate();  //cause picturebox to be painted with gallery wall
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        /// <summary>
        /// Saves the current wall contents as a csv file when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = CSVFILTER;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter outfile = new StreamWriter(saveFileDialog1.FileName);

                    // write list of paintings
                    foreach (Artworks p in wall.artworks)
                    {
                        outfile.WriteLine(p.ToString());  //ToString method gives CSV format
                    }
                    outfile.Close(); //be a tidy kiwi
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Changes the background colour of the wall if the button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                InitialiseWall(); //in case they did not create a new wall first
                wall.BackgroundColour = colorDialog1.Color;
                pictureBoxWall.Invalidate();
            }
        }

        /// <summary>
        /// Initialises the wall variable if it has not already been initialised
        /// </summary>
        private void InitialiseWall()
        {
            if (wall == null)
            {
                wall = new Wall();
            }
        }

        /// <summary>
        /// Resets all input controls that control adding new paintings
        /// </summary>
        private void ResetInputParams()
        {
            txtXPos.Clear();
            txtYPos.Clear();
            txtWidth.Clear();
            txtHeight.Clear();
        }

        /// <summary>
        /// Populate input controls from selected painting
        /// </summary>
        private void SetInputParams()
        {
            txtXPos.Text = currentSelect.Frame.X.ToString();
            txtYPos.Text = currentSelect.Frame.Y.ToString();
            txtWidth.Text = currentSelect.Frame.Width.ToString();
            txtHeight.Text = currentSelect.Frame.Height.ToString();
        }

        /// <summary>
        /// validates if appropriate input is in the textboxes
        /// </summary>
        /// <returns>True if input is valid, false otherwise</returns>
        private bool ValidateInput()
        {
            try
            {
                // Check if all input values are within valid ranges
                return int.Parse(txtXPos.Text) >= 0 &&
                       int.Parse(txtXPos.Text) <= pictureBoxWall.Width - int.Parse(txtWidth.Text) &&
                       int.Parse(txtYPos.Text) >= 0 &&
                       int.Parse(txtYPos.Text) <= pictureBoxWall.Height - int.Parse(txtHeight.Text) &&
                       int.Parse(txtWidth.Text) > 0 &&
                       int.Parse(txtHeight.Text) > 0;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            return false; //try-catch has caught 'not a number' error
        }

        /// <summary>
        /// Draws / Paints all objects onto the picturebox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxWall_Paint(object sender, PaintEventArgs e)
        {
            if (wall != null)
                wall.Display(e.Graphics);
        }

        /// <summary>
        /// Reads when the mouse click is released
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxWall_MouseUp(object sender, MouseEventArgs e)
        {
            InitialiseWall(); //in case they did not create a new wall first
            currentSelect = wall.SelectArtwork(e.X, e.Y);
            if (currentSelect != null) //checks to see if the mouse is over a painting, and selects it if it is
                SetInputParams();
            else //otherwise clear the selected painting data
                ResetInputParams();
        }

        /// <summary>
        /// Handles the click event for the "Add painting" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPainting_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                int x = int.Parse(txtXPos.Text);
                int y = int.Parse(txtYPos.Text);
                int w = int.Parse(txtWidth.Text);
                int h = int.Parse(txtHeight.Text);
                if (currentSelect == null)
                {
                    // Add new painting
                    Painting newPainting = new Painting(x, y, w, h);
                    InitialiseWall(); //in case they did not create a new wall first
                    wall.AddArtwork(newPainting);
                    ResetInputParams();
                }
                else if (currentSelect is Painting painting) //updating an existing painting
                {
                    painting.Update(x, y, w, h);
                 }
                pictureBoxWall.Invalidate(); //use paint method to draw gallery wall
            }
            else
            {
                MessageBox.Show("Invalid input parameters");
            }
        }

        /// <summary>
        /// Handles the click event for the "Add Pedestal" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPedestal_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                int x = int.Parse(txtXPos.Text);
                int w = int.Parse(txtWidth.Text);
                int h = int.Parse(txtHeight.Text);
                Color color;

                if (comboBox1.SelectedIndex == 1)
                {
                    color = Color.White;
                }
                else
                {
                    color = Color.Black;
                }

                if (currentSelect == null)
                {
                    // Add new pedestal
                    Pedestal newPedestal = new Pedestal(x, w, h, color);
                    InitialiseWall(); //in case they did not create a new wall first
                    wall.AddArtwork(newPedestal);
                    ResetInputParams();
                }
                else if (currentSelect is Pedestal pedestal)//updating an existing painting
                {
                    pedestal.Update(x, w, h);
                    pedestal.colour = color;
                }
                pictureBoxWall.Invalidate(); //use paint method to draw gallery wall
            }
            else
            {
                MessageBox.Show("Invalid input parameters");
            }
        }
        
        /// <summary>
        /// Test button for finding all the artworks matching specific requirements (predicates) that are on the wall
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestingFindAllArtworks_Click(object sender, EventArgs e)
        {
            if (wall != null)
            {
                wall.FindAllArtworks(isPainting);
            }
            else
            {
                MessageBox.Show("No wall has been created yet.");
            }
        }
    }
}
