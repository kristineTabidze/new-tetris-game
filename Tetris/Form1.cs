using System;
using System.IO;
using System.Windows.Forms;
using Tetris;

namespace Tetris
{
    public partial class Form1 : Form
    {
        readonly Random rand = new Random();
        readonly string playerName;
        bool isClickedOnFillBoard = false;
        bool handled = false;

        public Form1()
        {
            InitializeComponent();
            if (!File.Exists(UserDataController.scoresRecordsPath))
                File.Create(UserDataController.scoresRecordsPath);
            playerName = Microsoft.VisualBasic.Interaction.InputBox("Please, enter your name","User settings","New user ");
            if(playerName == "")
            {
                playerName = "Anonymous";
            }
            this.KeyUp += new KeyEventHandler(KeyFunc);

            Init();
        }

        public void Init()
        {
            UserDataController.ShowScoreRecords(label3);
            this.Text = "Tetris: current user - " + playerName;
            BoardController.size = 25;
            BoardController.score = 0;
            BoardController.linesRemoved = 0;
            BoardController.level = 1;
            BoardController.currentFigure = new Figure(rand.Next(0,7), 0);
            BoardController.Interval = 300;
            label1.Text = "Score: " + BoardController.score;
            label2.Text = "Lines: " + BoardController.linesRemoved;
            label4.Text = "Level: " + BoardController.level;

            timer1.Interval = BoardController.Interval;
            timer1.Tick += new EventHandler(Update);
            timer1.Start();
            
            Invalidate();

        }

        private void KeyFunc(object sender, KeyEventArgs e)
        {

            bool handled = true;

            if (e.Control && e.KeyCode == Keys.P)
            {
                // pause the game
                timer1.Stop();
                pauseToolStripMenuItem.Text = "Continue";
                saveToolStripMenuItem.Enabled = true;
                loadToolStripMenuItem.Enabled = true;
            }

            if (e.Control && e.KeyCode == Keys.G)
            {
                // resume the game
                timer1.Start();
                pauseToolStripMenuItem.Text = "Pause";
                saveToolStripMenuItem.Enabled = false;
                loadToolStripMenuItem.Enabled = false;
            }

            if (e.Control && e.KeyCode == Keys.N)
            {
                // start new game
                timer1.Tick -= new EventHandler(Update);
                timer1.Stop();
                BoardController.ClearBoard();
                Init();
            }

            switch (e.KeyCode)
            {
                case Keys.Home:
                    isClickedOnFillBoard = true;
                    break;
                case Keys.Down: //keyboard down event

                    if (!BoardController.IsIntersects())
                    {
                        BoardController.ResetArea();
                        BoardController.currentFigure.RotateFigureCw();
                        BoardController.Merge();
                        Invalidate();
                    }
                    break;
                case Keys.Up: //keyboard up event

                    if (!BoardController.IsIntersects())
                    {
                        BoardController.ResetArea();
                        BoardController.currentFigure.RotateFigureCcw();
                        BoardController.Merge();
                        Invalidate();
                    }
                    break;
                case Keys.Space:
                    timer1.Interval = 10;
                    break;
                case Keys.E:
                    Application.Exit();
                    break;
                case Keys.Right:
                    if (!BoardController.CollideHor(1))
                    {
                        BoardController.ResetArea();
                        BoardController.currentFigure.MoveRight();
                        BoardController.Merge();
                        Invalidate();
                    }
                    break;
                case Keys.Left:
                    if (!BoardController.CollideHor(-1))
                    {
                        BoardController.ResetArea();
                        BoardController.currentFigure.MoveLeft();
                        BoardController.Merge();
                        Invalidate();
                    }
                    break;
            }
        }
        
        private void Update(object sender, EventArgs e)
        {
            BoardController.ResetArea();
            if (!BoardController.Collide())
            {
                BoardController.currentFigure.MoveDown();
            }
            else
            {
                BoardController.Merge();
                BoardController.SliceBoard(label1,label2,label4);
                timer1.Interval = BoardController.Interval;
                BoardController.currentFigure.ResetFigure(rand.Next(0,7),0);
                if (BoardController.Collide())
                {
                    UserDataController.SaveRecords(playerName);
                    BoardController.ClearBoard();
                    timer1.Tick -= new EventHandler(Update);
                    timer1.Stop();
                    MessageBox.Show("Your score: " + BoardController.score);
                    Init();
                }
            }
            BoardController.Merge();
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            BoardController.DrawGrid(e.Graphics);
            BoardController.DrawBoard(e.Graphics);
            BoardController.ShowNextFigure(e.Graphics);
            if (isClickedOnFillBoard)
            {
                
                BoardController.FillArea(e.Graphics);
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
                saveToolStripMenuItem.Enabled = true;
                loadToolStripMenuItem.Enabled = true;
            }
            else
            {
                pressedButton.Text = "Pause";
                timer1.Start();
                saveToolStripMenuItem.Enabled = false;
                loadToolStripMenuItem.Enabled = false;
            }
        }

        private void OnStripRestartClick(object sender, EventArgs e)
        {
            timer1.Tick -= new EventHandler(Update);
            timer1.Stop();
            BoardController.ClearBoard();
            Init();
            
        }

        private void OnStripReferenceClick(object sender, EventArgs e)
        {
            string infoString = "To move the block use left/right arrow keys\n";
            infoString += "To rotate the block use up/down arrow keys\n";
            infoString += "To accelerate the block use spacebar\n";
            infoString += "To restart the game use ctrl + N \n";
            infoString += "To pause game use ctrl + P\n";
            infoString += "To resume game use ctrl + G\n";
            infoString += "To exit game use E \n";
            infoString += "\n\n\nIf you're looking for the cheat, it's 'home' key.";
            MessageBox.Show(infoString,"How to play");
        }

        private void OnClickAboutGame(object sender, EventArgs e)
        {
            string infoString = "This is a TETRIS game by:\n";
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

            timer1.Tick -= new EventHandler(Update);
                timer1.Stop();
                BoardController.ClearBoard();
                Init();
            
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void OnSaveGameClick(object sender, EventArgs e)
        {
            UserDataController.SaveGame(playerName);
            MessageBox.Show("Saved!", "Save Game");
        }

        private void OnLoadGameClick(object sender, EventArgs e)
        {
            BoardController.currentFigure.ResetFigure(rand.Next(0, 7), 0);
            BoardController.board = UserDataController.LoadGame(playerName);
        }

        private void OnExitButtonClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
