using System;
using System.IO;
using System.Windows.Forms;
using Tetris;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Random rand = new Random();
        string playerName;
        bool isClickedOnFillBoard = false;
        bool handled = false;

        public Form1()
        {
            InitializeComponent();
            if (!File.Exists(RecordsController.scoresRecordsPath))
                File.Create(RecordsController.scoresRecordsPath);
            playerName = Microsoft.VisualBasic.Interaction.InputBox("Enter your name","User settings","New user ");
            if(playerName == "")
            {
                playerName = "Anonymous";
            }
            this.KeyUp += new KeyEventHandler(keyFunc);
            this.MouseClick += new MouseEventHandler(mouseFunction);

            Init();
        }

        public void Init()
        {
            RecordsController.ShowScoreRecords(label3);
            this.Text = "Tetris: current user - " + playerName;
            MapController.size = 25;
            MapController.score = 0;
            MapController.linesRemoved = 0;
            MapController.level = 1;
            MapController.currentShape = new Shape(rand.Next(0,7), 0);
            MapController.Interval = 300;
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

            bool handled = true;

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
        }
        
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
                MapController.SliceMap(label1,label2,label4);
                timer1.Interval = MapController.Interval;
                MapController.currentShape.ResetShape(rand.Next(0,7),0);
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

        private void OnStripPauseClick(object sender, EventArgs e)
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

        private void OnStripRestartClick(object sender, EventArgs e)
        {
            timer1.Tick -= new EventHandler(update);
            timer1.Stop();
            MapController.ClearMap();
            Init();
        }


        private void OnStripReferenceClick(object sender, EventArgs e)
        {
            string infoString = "";
            infoString = "To move the block use left/right arrow keys\n";
            infoString += "To rotate the block use up/down arrow keys\n";
            infoString += "To accelerate the block use spacebar\n";
            MessageBox.Show(infoString,"How to play");
        }

        private void onClickAboutGame(object sender, EventArgs e)
        {
            string infoString = "";
            infoString = "This is a TETRIS game by:\n";
            infoString += "Ioseb Gejadze / 823459813 \n";
            infoString += "Kristine Tabidze / 823377042 \n";
            MessageBox.Show(infoString, "About");
        }


        private void OnPauseButtonClick(object sender, EventArgs e)
        {
            if (handled)
            {
                handled = false;
                return;
            }
            var pressedButton = sender as Button;
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

        private void OnRestartButtonClick(object sender, EventArgs e)
        {
            if (handled)
            {
                handled = false;
                return;
            }

            timer1.Tick -= new EventHandler(update);
                timer1.Stop();
                MapController.ClearMap();
                Init();
            
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void OnSaveGameClick(object sender, EventArgs e)
        {
            RecordsController.SaveGame(playerName);
            MessageBox.Show("Saved!", "Save Game");
        }

        private void OnLoadGameClick(object sender, EventArgs e)
        {
            MapController.currentShape.ResetShape(rand.Next(0, 7), 0);
            MapController.map = RecordsController.LoadGame(playerName);
        }
    }
}
