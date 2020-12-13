using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetris.Controllers;

namespace Tetris
{
    public partial class Form1 : Form
    {
        
        
        string playerName;
        bool isClickedOnFillBoard = false;


        public Form1()
        {
            InitializeComponent();
            if (!File.Exists(RecordsController.recordPath))
                File.Create(RecordsController.recordPath);
            playerName = Microsoft.VisualBasic.Interaction.InputBox("Enter your name","User settings","New user ");
            if(playerName == "")
            {
                playerName = "New user";
            }
            this.KeyUp += new KeyEventHandler(keyFunc);
            this.MouseClick += new MouseEventHandler(mouseFunction);

            Init();
        }

        public void Init()
        {
            RecordsController.ShowRecords(label3);
            this.Text = "Tetris: current user - " + playerName;
            MapController.size = 25;
            MapController.score = 0;
            MapController.linesRemoved = 0;
            MapController.currentShape = new Shape(3, 0);
            MapController.Interval = 500;
            label1.Text = "Score: " + MapController.score;
            label2.Text = "Lines: " + MapController.linesRemoved;
            label4.Text = "Level: " + MapController.level;

            timer1.Interval = MapController.Interval;
            timer1.Tick += new EventHandler(update);
            timer1.Start();
            

            Invalidate();
        }

        private void keyFunc(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.P)
            {
                // pause the game
                timer1.Stop();
            }

            if (e.Control && e.KeyCode == Keys.G)
            {
                // resume the game
                timer1.Start();
            }

            switch (e.KeyCode)
            {
                case Keys.Home:
                    isClickedOnFillBoard = true;
                    break;
                case Keys.Down: //keyboard down event

                    if (!MapController.IsIntersects())
                    {
                        MapController.ResetArea();
                        MapController.currentShape.RotateShapeCw();
                        MapController.Merge();
                        Invalidate();
                    }
                    break;
                case Keys.Up: //keyboard up event

                    if (!MapController.IsIntersects())
                    {
                        MapController.ResetArea();
                        MapController.currentShape.RotateShapeCcw();
                        MapController.Merge();
                        Invalidate();
                    }
                    break;
                case Keys.Space:
                    timer1.Interval = 10;
                    break;
                case Keys.Right:
                    if (!MapController.CollideHor(1))
                    {
                        MapController.ResetArea();
                        MapController.currentShape.MoveRight();
                        MapController.Merge();
                        Invalidate();
                    }
                    break;
                case Keys.Left:
                    if (!MapController.CollideHor(-1))
                    {
                        MapController.ResetArea();
                        MapController.currentShape.MoveLeft();
                        MapController.Merge();
                        Invalidate();
                    }
                    break;

                   
            }

        }

        // add events after click mouse right/left button

        private void mouseFunction(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                
                case MouseButtons.Right:
                    if (!MapController.CollideHor(1))
                    {
                        MapController.ResetArea();
                        MapController.currentShape.MoveRight();
                        MapController.Merge();
                        Invalidate();
                    }
                    break;
                case MouseButtons.Left:
                    if (!MapController.CollideHor(-1))
                    {
                        MapController.ResetArea();
                        MapController.currentShape.MoveLeft();
                        MapController.Merge();
                        Invalidate();
                    }
                    break;
            }

            if (e.Delta > 0 || e.Delta < 0) //mouse wheel up  /down
            {
                if (!MapController.IsIntersects())
                {
                    MapController.ResetArea();
                    MapController.currentShape.RotateShapeCcw();
                    MapController.Merge();
                    Invalidate();
                }
            }

        }

        //add event on mouse wheel up/down
        
        private void update(object sender, EventArgs e)
        {
            MapController.ResetArea();
            if (!MapController.Collide())
            {
                MapController.currentShape.MoveDown();
            }
            else
            {
                MapController.Merge();
                MapController.SliceMap(label1,label2);
                timer1.Interval = MapController.Interval;
                MapController.currentShape.ResetShape(3,0);
                if (MapController.Collide())
                {
                    RecordsController.SaveRecords(playerName);
                    MapController.ClearMap();
                    timer1.Tick -= new EventHandler(update);
                    timer1.Stop();
                    MessageBox.Show("Your score: " + MapController.score);
                    Init();
                }
            }
            MapController.Merge();
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            MapController.DrawGrid(e.Graphics);
            MapController.DrawMap(e.Graphics);
            MapController.ShowNextShape(e.Graphics);
            if (isClickedOnFillBoard)
            {
                
                MapController.FillArea(e.Graphics);
                isClickedOnFillBoard = false;
            }
        }

        private void OnPauseButtonClick(object sender, EventArgs e)
        {
            var pressedButton = sender as ToolStripMenuItem;
            if (timer1.Enabled)
            {
                pressedButton.Text = "Continue";
                timer1.Stop();
            }
            else
            {
                pressedButton.Text = "Pause";
                timer1.Start();
            }
        }

        private void OnAgainButtonClick(object sender, EventArgs e)
        {
            timer1.Tick -= new EventHandler(update);
            timer1.Stop();
            MapController.ClearMap();
            Init();
        }


        private void OnInfoPressed(object sender, EventArgs e)
        {
            string infoString = "";
            infoString = "To move the block use left/right arrow keys .\n";
            infoString += "To rotate the block use up/down arrow keys .\n";
            infoString += "To accelerate use space bar .\n";
            MessageBox.Show(infoString,"Reference");
        }

       
    }
}
