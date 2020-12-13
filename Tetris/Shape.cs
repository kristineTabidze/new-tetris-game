using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Shape
    {

     

        public int x;
        public int y;
        public int[,] matrix;
        public int[,] nextMatrix;
        public int sizeMatrix;
        public int sizeNextMatrix;

        public int[,] IBlock = new int[4, 4]{
            {1,1,1,1  },
            {0,0,0,0  },
            {0,0,0,0  },
            {0,0,0,0  },
        };

        public int[,] SBlock = new int[3, 3]{
            {0,0,0 },
            {0,2,2 },
            {2,2,0 },
        };

        public int[,] ZBlock = new int[3, 3]{
            {0,0,0 },
            {4,4,0 },
            {0,4,4 },
        };

        public int[,] TBlock = new int[3, 3]{
            {0,0,0 },
            {3,3,3 },
            {0,3,0 },
        };

       
        public int[,] OBlock = new int[2, 2]{
            {5,5 },
            {5,5 },
        };

        public int[,] JBlock = new int[3, 3]{
            {0,0,0 },
            {6,0,0 },
            {6,6,6},
        };

        public int[,] LBlock = new int[3, 3]{
            {0,0,0 },
            {0,0,7 },
            {7,7,7 },
        };


        public Shape(int _x,int _y)
        {
            x = _x;
            y = _y;
            matrix = GenerateMatrix();
            sizeMatrix = (int)Math.Sqrt(matrix.Length);
            nextMatrix = GenerateMatrix();
            sizeNextMatrix = (int)Math.Sqrt(nextMatrix.Length);
        }

        public void ResetShape(int _x, int _y)
        {
            x = _x;
            y = _y;
            matrix = nextMatrix;
            sizeMatrix = (int)Math.Sqrt(matrix.Length);
            nextMatrix = GenerateMatrix();
            sizeNextMatrix = (int)Math.Sqrt(nextMatrix.Length);
        }

        public int[,] GenerateMatrix()
        {
            int[,] _matrix = IBlock;
            Random r = new Random();
            switch (r.Next(1, 8))
            {
                case 1:
                    _matrix = IBlock;
                    break;
                case 2:
                    _matrix = SBlock;
                    break;
                case 3:
                    _matrix = TBlock;
                    break;
                case 4:
                    _matrix = ZBlock;
                    break;
                case 5:
                    _matrix = OBlock;
                    break;
                case 6:
                    _matrix = JBlock;
                    break;
                case 7:
                    _matrix = LBlock;
                    break;
            }
            return _matrix;
        }

        public void RotateShape()
        {
            int[,] tempMatrix = new int[sizeMatrix,sizeMatrix];
            for(int i = 0; i < sizeMatrix; i++)
            {
                for (int j = 0; j < sizeMatrix; j++)
                {
                    tempMatrix[i, j] = matrix[j, (sizeMatrix - 1) - i];
                }
            }
            matrix = tempMatrix;
            int offset1 = (8 - (x + sizeMatrix));
            if (offset1 < 0)
            {
                for (int i = 0; i < Math.Abs(offset1); i++)
                    MoveLeft();
            }
            
            if (x < 0)
            {
                for (int i = 0; i < Math.Abs(x)+1; i++)
                    MoveRight();
            }

        }

        public void MoveDown()
        {
            y++;
        }
        public void MoveRight()
        {
            x++;
        }
        public void MoveLeft()
        {
            x--;
        }
    }
}
