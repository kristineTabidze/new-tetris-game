using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public static class UserDataController
    {
        public static string scoresRecordsPath = @"..\..\Resources\recordsFile.txt";
        public static string gameRecordsDirectory = @"..\..\Resources\Saved-Games\";
        public static void SaveRecords(string playerName)
        {
            List<ScoreRecordItem> recordsArray = LoadScoreRecords();
            bool isNameExistsInRecords = false;
            for (int i = 0; i < recordsArray.Count; i++)
            {
                if (playerName == recordsArray[i].UserName)
                {
                    isNameExistsInRecords = true;
                    if (BoardController.score > recordsArray[i].Score)
                    {
                        recordsArray.RemoveAt(i);
                        recordsArray.Add(ScoreRecordItem.StringToRecordItem(playerName + "|" + BoardController.score));
                    }
                }
            }
            if (!isNameExistsInRecords)
            recordsArray.Add(ScoreRecordItem.StringToRecordItem(playerName + "|" + BoardController.score));
            List<string> recordsStringArray = new List<string>();
            foreach(var rec in recordsArray)
            {
                recordsStringArray.Add(rec.ToString());
            }
            File.WriteAllLines(scoresRecordsPath, recordsStringArray);
        }

        public static void SaveGame(string playerName)
        {
            string path = gameRecordsDirectory + playerName + ".txt";
            FileStream fs = File.Create(path);

            int[,] currBoard = BoardController.board.Clone() as int[,];
            int x = BoardController.currentFigure.x;
            int y = BoardController.currentFigure.y;
            int size = BoardController.currentFigure.sizeMatrix;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (y + i >= BoardController.rows || x + j >= BoardController.columns) break;
                    if (currBoard[y + i, x + j] == BoardController.currentFigure.GetFigureType())
                    {
                        currBoard[y + i, x + j] = 0;
                    }
                }
            }

            string data = "";
            for(int i=0; i<BoardController.rows; i++)
            {
                for(int j=0; j<BoardController.columns; j++)
                {
                    data += currBoard[i, j];
                }
                data += "\n";
            }

            byte[] byteData = new UTF8Encoding(true).GetBytes(data);
            fs.Write(byteData, 0, byteData.Length);
            fs.Close();
        }

        public static List<ScoreRecordItem> LoadScoreRecords()
        {
            string[] recordsStringArray = File.ReadAllLines(scoresRecordsPath);
            List<ScoreRecordItem> recordsArray = new List<ScoreRecordItem>();
            foreach(var rec in recordsStringArray)
            {
                recordsArray.Add(ScoreRecordItem.StringToRecordItem(rec));
            }
            return recordsArray;
        }

        public static int[,] LoadGame(string playerName)
        {
            string path = gameRecordsDirectory + playerName + ".txt";
            if (!File.Exists(path)) return new int[BoardController.rows, BoardController.columns];
            string[] recordsStringArray = File.ReadAllLines(path);
            int[,] loadedBoard = new int[BoardController.rows, BoardController.columns];
            for(int i=0; i<BoardController.rows; i++)
            {
                string currLine = recordsStringArray[i];
                for(int j=0; j<BoardController.columns; j++)
                {
                    loadedBoard[i, j] = currLine[j] - '0';
                }
            }
            return loadedBoard;
        }

        public static void ShowScoreRecords(Label label3)
        {
            List<ScoreRecordItem> recordsArray = LoadScoreRecords();
            recordsArray.Sort();
            label3.Text = "Top scorers";
            int bound = recordsArray.Count > 8 ? recordsArray.Count-8 : 0;
            int rank = 1;
            for (int i = recordsArray.Count-1; i >= bound; i--)
            {
                label3.Text += "\n";
                label3.Text += rank+". " + recordsArray[i].UserName + " : " + recordsArray[i].Score;
                rank++;
            }
        }
    }

    //public class ScoreRecordItem

    public class ScoreRecordItem : IComparable<ScoreRecordItem>
    {
        public string UserName{ get; set; }
        public int Score { get; set; }

        public ScoreRecordItem()
        {
            UserName = "Anonymous";
            Score = 0;
        }
        public ScoreRecordItem(string userName, int score)
        {
            UserName = userName;
            Score = score;
        }
        public static ScoreRecordItem StringToRecordItem(string str)
        {
            if (str.Contains("|"))
            {
                string[] args = str.Split('|');
                return new ScoreRecordItem(args[0], int.Parse(args[1]));
            }
            else return new ScoreRecordItem();
        }

        public override string ToString()
        {
            return UserName+"|"+Score;
        }
        public int CompareTo(ScoreRecordItem other)
        {
            if (Score > other.Score) return 1;
            else if (Score < other.Score) return -1;
            else return 0;
        }

        public static bool operator >(ScoreRecordItem rec1, ScoreRecordItem rec2)
        {
            if (rec1.CompareTo(rec2) > 0) return true;
            else return false;
        }
        public static bool operator <(ScoreRecordItem rec1, ScoreRecordItem rec2)
        {
            if (rec2.CompareTo(rec1) > 0) return true;
            else return false;
        }
    }
}
