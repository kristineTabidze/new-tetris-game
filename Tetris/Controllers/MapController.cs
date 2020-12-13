using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris.Controllers
{
    public static class MapController
    {
        public static Shape currentShape;
        public static int size;
        private static int rows = 18;
        private static int columns = 10;
        public static int[,] map = new int[rows, columns];
        public static int linesRemoved;
        public static int score;
        public static int Interval;
        public static int level;

        public static void ShowNextShape(Graphics e)
        {
            for (int i = 0; i < currentShape.sizeNextMatrix; i++)
            {
                for (int j = 0; j < currentShape.sizeNextMatrix; j++)
                {
                    if (currentShape.nextMatrix[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentShape.nextMatrix[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Yellow, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentShape.nextMatrix[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentShape.nextMatrix[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Blue, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentShape.nextMatrix[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentShape.nextMatrix[i, j] == 6)
                    {
                        e.FillRectangle(Brushes.Aquamarine, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentShape.nextMatrix[i, j] == 7)
                    {
                        e.FillRectangle(Brushes.LimeGreen, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                }
            }
        }

        public static void ClearMap()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    map[i, j] = 0;
                }
            }
        }

        public static void DrawMap(Graphics e)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (map[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Yellow, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Blue, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 6)
                    {
                        e.FillRectangle(Brushes.Aquamarine, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 7)
                    {
                        e.FillRectangle(Brushes.LimeGreen, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
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

        public static void SliceMap(Label label1,Label label2)
        {
            int count = 0;
            int curRemovedLines = 0;
            for (int i = 0; i < rows; i++)
            {
                count = 0;
                for (int j = 0; j < columns; j++)
                {
                    if (map[i, j] != 0)
                        count++;
                }
                if (count == columns)
                {
                    curRemovedLines++;
                   
                    for (int k = i; k >= 1; k--)
                    {
                        for (int o = 0; o < columns; o++)
                        {
                            map[k, o] = map[k - 1, o];
                        }
                    }
                }
            }

            if (curRemovedLines > 0)
            {
                linesRemoved += curRemovedLines;
                if (linesRemoved / 10 + 1 > level) level++;
                score += level * (curRemovedLines * 100 + (curRemovedLines - 1) * 50); //change score
            }

            if (linesRemoved % 10 == 0 && linesRemoved > 0)
            {
                if (Interval > 30)
                    Interval -= Interval * 25 / 100; //fall 25% faster than the previous level
            }

            
            label1.Text = "Score: " + score;
            label2.Text = "Lines: " + linesRemoved;
        }

        public static bool IsIntersects()
        {
            for (int i = currentShape.y; i < currentShape.y + currentShape.sizeMatrix; i++)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (j >= 0 && j < columns)
                    {
                        if (map[i, j] != 0 && currentShape.matrix[i - currentShape.y, j - currentShape.x] == 0)
                            return true;
                    }
                }
            }
            return false;
        }

        public static void Merge()
        {
            for (int i = currentShape.y; i < currentShape.y + currentShape.sizeMatrix; i++)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (currentShape.matrix[i - currentShape.y, j - currentShape.x] != 0)
                        map[i, j] = currentShape.matrix[i - currentShape.y, j - currentShape.x];
                }
            }
        }

        public static bool Collide()
        {
            for (int i = currentShape.y + currentShape.sizeMatrix - 1; i >= currentShape.y; i--)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (currentShape.matrix[i - currentShape.y, j - currentShape.x] != 0)
                    {
                        if (i + 1 == rows)
                            return true;
                        if (map[i + 1, j] != 0)
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
            for (int i = currentShape.y; i < currentShape.y + currentShape.sizeMatrix; i++)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (currentShape.matrix[i - currentShape.y, j - currentShape.x] != 0)
                    {
                        if (j + 1 * dir > columns - 1 || j + 1 * dir < 0)
                            return true;

                        if (map[i, j + 1 * dir] != 0)
                        {
                            if (j - currentShape.x + 1 * dir >= currentShape.sizeMatrix || j - currentShape.x + 1 * dir < 0)
                            {
                                return true;
                            }
                            if (currentShape.matrix[i - currentShape.y, j - currentShape.x + 1 * dir] == 0)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public static void ResetArea()
        {
            for (int i = currentShape.y; i < currentShape.y + currentShape.sizeMatrix; i++)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (i >= 0 && j >= 0 && i < rows && j < columns)
                    {
                        if (currentShape.matrix[i - currentShape.y, j - currentShape.x] != 0)
                        {
                            map[i, j] = 0;
                        }
                    }
                }
            }
        }

    }
}
