﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public static class BoardController
    {
        public static Figure currentFigure;
        public static int size;
        public static readonly int rows = 18;
        public static readonly int columns = 10;
        public static int[,] board = new int[rows, columns];
        public static int linesRemoved;
        public static int score;
        public static int Interval;
        public static int level;

        public static void ShowNextFigure(Graphics e)
        {
            for (int i = 0; i < currentFigure.sizeNextMatrix; i++)
            {
                for (int j = 0; j < currentFigure.sizeNextMatrix; j++)
                {
                    if (currentFigure.nextMatrix[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentFigure.nextMatrix[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Yellow, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentFigure.nextMatrix[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentFigure.nextMatrix[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Blue, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentFigure.nextMatrix[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentFigure.nextMatrix[i, j] == 6)
                    {
                        e.FillRectangle(Brushes.Aquamarine, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentFigure.nextMatrix[i, j] == 7)
                    {
                        e.FillRectangle(Brushes.LimeGreen, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                }
            }
        }

        public static void ClearBoard()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    board[i, j] = 0;
                }
            }
        }

        public static void DrawBoard(Graphics e)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (board[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (board[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Yellow, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (board[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (board[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Blue, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (board[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (board[i, j] == 6)
                    {
                        e.FillRectangle(Brushes.Aquamarine, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (board[i, j] == 7)
                    {
                        e.FillRectangle(Brushes.LimeGreen, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (board[i, j] == 8) //if pressed c keyboard
                    {
                        e.FillRectangle(Brushes.Pink, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                }
            }
        }

        public static void DrawGrid(Graphics g)
        {
            for (int i = 0; i <= rows; i++)
            {
                g.DrawLine(Pens.Black, new Point(50, 50 + i * size), new Point(50 + columns * size, 50 + i * size));
            }
            for (int i = 0; i <= columns; i++)
            {
                g.DrawLine(Pens.Black, new Point(50 + i * size, 50), new Point(50 + i * size, 50 + rows * size));
            }
        }

        public static void SliceBoard(Label label1, Label label2, Label label4)
        {
            int count;
            int curRemovedLines = 0;
            for (int i = 0; i < rows; i++)
            {
                count = 0;

                for (int j = 0; j < columns; j++)
                {
                    if (board[i, j] != 0)
                        count++;
                }
                if (count == columns)
                {
                    curRemovedLines++;  
                    for (int k = i; k >= 1; k--)
                    {
                        for (int o = 0; o < columns; o++)
                        {
                            board[k, o] = board[k - 1, o];
                        }
                    }
                }
            }

            if (curRemovedLines > 0)
            {
                linesRemoved += curRemovedLines;
                score += level * (curRemovedLines * 100 + (curRemovedLines - 1) * 50); //change score
                if (linesRemoved / 10 + 1 > level)
                {
                    level = linesRemoved/10+1;
                    Interval -= Interval * 25 / 100; //fall 25% faster than the previous level
                }
            }
            
            label1.Text = "Score: " + score;
            label2.Text = "Lines: " + linesRemoved;
            label4.Text = "Level: " + level;
        }

        public static bool IsIntersects()
        {
            for (int i = currentFigure.y; i < currentFigure.y + currentFigure.sizeMatrix; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.sizeMatrix; j++)
                {
                    if (j >= 0 && j < columns)
                    {
                        if (board[i, j] != 0 && currentFigure.matrix[i - currentFigure.y, j - currentFigure.x] == 0)
                            return true;
                    }
                }
            }
            return false;
        }

        public static void Merge()
        {
            for (int i = currentFigure.y; i < currentFigure.y + currentFigure.sizeMatrix; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.sizeMatrix; j++)
                {
                    if (currentFigure.matrix[i - currentFigure.y, j - currentFigure.x] != 0)
                        board[i, j] = currentFigure.matrix[i - currentFigure.y, j - currentFigure.x];
                }
            }
        }

        public static bool Collide()
        {
            for (int i = currentFigure.y + currentFigure.sizeMatrix - 1; i >= currentFigure.y; i--)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.sizeMatrix; j++)
                {
                    if (currentFigure.matrix[i - currentFigure.y, j - currentFigure.x] != 0)
                    {
                        if (i + 1 == rows)
                            return true;
                        if (board[i + 1, j] != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool CollideHor(int dir)
        {
            for (int i = currentFigure.y; i < currentFigure.y + currentFigure.sizeMatrix; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.sizeMatrix; j++)
                {
                    if (currentFigure.matrix[i - currentFigure.y, j - currentFigure.x] != 0)
                    {
                        if (j + 1 * dir > columns - 1 || j + 1 * dir < 0)
                            return true;

                        if (board[i, j + 1 * dir] != 0)
                        {
                            if (j - currentFigure.x + 1 * dir >= currentFigure.sizeMatrix || j - currentFigure.x + 1 * dir < 0)
                            {
                                return true;
                            }
                            if (currentFigure.matrix[i - currentFigure.y, j - currentFigure.x + 1 * dir] == 0)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public static void ResetArea()
        {
            for (int i = currentFigure.y; i < currentFigure.y + currentFigure.sizeMatrix; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.sizeMatrix; j++)
                {
                    if (i >= 0 && j >= 0 && i < rows && j < columns)
                    {
                        if (currentFigure.matrix[i - currentFigure.y, j - currentFigure.x] != 0)
                        {
                            board[i, j] = 0;
                        }
                    }
                }
            }
        }

        public static void FillArea(Graphics e)
        {
           
            for (int i = 4; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (board[i,j] == 0)
                    {
                        board[i, j] = 8;
                        e.FillRectangle(Brushes.Pink, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                }
            }
        }

    }
}
