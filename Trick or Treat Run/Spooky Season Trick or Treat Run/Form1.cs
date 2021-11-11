using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spooky_Season_Trick_or_Treat_Run
{
    public partial class Form1 : Form
    {

        bool goLeft, goRight, jumping, hasScythe;

        int jumpSpeed = 6;
        int force = 8;
        int score = 0;

        int playerSpeed = 10;
        int backgroundSpeed = 8;

        public Form1()
        {
            InitializeComponent();

        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "SCORE: " + score;
            txtScore.Left = 20;

            player.Top += jumpSpeed;

            if (goLeft == true && player.Left > 1)
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true && player.Left + (player.Width + 1) < this.ClientSize.Width)
            {
                player.Left += playerSpeed;
            }


            if (goLeft == true && background.Left < 0)
            {
                background.Left += backgroundSpeed;
                MoveGameElements("forward");
            }
            if (goRight == true && background.Left > -1000)
            {
                background.Left -= backgroundSpeed;
                MoveGameElements("back");

            }
            if (jumping == true)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "platform")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && jumping == false)
                    {
                        force = 8;
                        player.Top = x.Top - player.Height;
                        jumpSpeed = 0;
                    }
                    x.BringToFront();
                }

                if (x is PictureBox && (string)x.Tag == "potion")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        x.Visible = false;
                        score += 1;
                    }

                }
            }

            //collecting(interacting) with the spoon(key)
            if (player.Bounds.IntersectsWith(scythe.Bounds))
            {
                scythe.Visible = false;
                hasScythe = true;
            }

            //interacting with the cauldron/"door"
            if (player.Bounds.IntersectsWith(couldron.Bounds) && hasScythe == true)
            {

                GameTimer.Stop();
                MessageBox.Show("Well done! Your Trick or Treat Run is complete! " + Environment.NewLine + "Click OK if you would like to play again!");
                RestartGame();
            }

            //falling off the platforms
            if (player.Top + player.Height > this.ClientSize.Height)
            {
                GameTimer.Stop();
                MessageBox.Show("WOMP! You Died!" + Environment.NewLine + "Click OK if you would like to play again!");
                RestartGame();
            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (jumping == true)
            {
                jumping = false;
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }


        //Close all the instances of all the layer of windows that are open(closes all background windows that were initiated).
        private void CloseGame(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void RestartGame()
        {
            Form1 newWindow = new Form1(); //Creating a new instance of the current form
            newWindow.Show(); //Show the new instance of window created (form being used for the new game)
            this.Hide(); //Hide the current window (form being used for the current game)
        }

        private void MoveGameElements(string direction)
        {
            foreach (Control x in this.Controls)
                if (x is PictureBox && (string)x.Tag == "platform" || x is PictureBox && (string)x.Tag == "potion" || x is PictureBox && (string)x.Tag == "couldron" || x is PictureBox && (string)x.Tag == "scythe" || x is PictureBox && (string)x.Tag == "spike")
                {
                    if (direction == "back")
                    {
                        x.Left -= backgroundSpeed;
                    }
                    if (direction == "forward")
                    {
                        x.Left += backgroundSpeed;
                    }

                }
        }

    }
}