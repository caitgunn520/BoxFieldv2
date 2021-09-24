using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoxField
{
    public partial class GameScreen : UserControl
    {
        //player1 button control keys
        Boolean leftArrowDown, rightArrowDown;

        //create a list to hold a column of boxes   
        List<Box> boxes = new List<Box>();

        //starting x positions for boxes
        int xLeft = 250;
        int gap = 300;

        //pattern values
        bool moveRight = true;
        int patternLength = 10;
        int xChange = 20;

        int newBoxCounter = 0;

        //hero values
        Box hero;

        Random randGen = new Random();

        public GameScreen()
        {
            InitializeComponent();
            OnStart();
        }

        /// <summary>
        /// Set initial game values here
        /// </summary>
        public void OnStart()
        {
            CreateBox(xLeft);
            CreateBox(xLeft + gap);

            hero = new Box(this.Width/2, this.Height - 100, 30, 4, new SolidBrush(Color.Goldenrod));
        }

        public void CreateBox(int x)
        {
            SolidBrush boxBrush = new SolidBrush(Color.White);
            int colourValue = randGen.Next(1, 4);

            if (colourValue == 1)
            {
                boxBrush = new SolidBrush(Color.Red);
            }
            else if (colourValue == 2)
            {
                boxBrush = new SolidBrush(Color.Yellow);
            }
            else
            {
                boxBrush = new SolidBrush(Color.Orange);
            }

            //Box(x, y, size, speed, brush)
            Box b = new Box(x, 0, 30, 10, boxBrush);
            boxes.Add(b);
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
            }
        }
        
        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            newBoxCounter++;

            //move hero
            if(leftArrowDown)
            {
                hero.Move("left");
            }
            if(rightArrowDown)
            {
                hero.Move("right");
            }

            //update location of all boxes (drop down screen)
            foreach (Box b in boxes)
            {
                b.Move();
            }

            //remove box if it has gone of screen
            if (boxes[0].y > this.Height)
            {
                boxes.RemoveAt(0);   
            }

            //add new box if it is time
            if (newBoxCounter == 6)
            {
                if(moveRight == true)
                {
                    xLeft += xChange;
                }
                else
                {
                    xLeft -= xChange;
                }

                CreateBox(xLeft);
                CreateBox(xLeft + gap);

                patternLength--;

                if(patternLength == 0)
                {
                    moveRight = !moveRight;
                    patternLength = randGen.Next(5, 20);
                    xChange = randGen.Next(5, 20);
                }

                newBoxCounter = 0;
            }

            //check for collisions between hero and boxes
            foreach (Box b in boxes)
            {
                if (hero.Collision(b))
                {
                    gameLoop.Enabled = false;
                }
            }
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw boxes to screen
            foreach(Box b in boxes)
            {
                e.Graphics.FillRectangle(b.brushColour, b.x, b.y, b.size, b.size);          
            }

            e.Graphics.FillRectangle(hero.brushColour, hero.x, hero.y, hero.size, hero.size);
        }
    }
}
