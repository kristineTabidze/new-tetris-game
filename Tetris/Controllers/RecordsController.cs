using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris.Controllers
{
    public static class RecordsController
    {
        public static string recordPath = @"..\..\..\recordsFile.txt";
        public static void SaveRecords(string playerName)
        {
            List<RecordItem> recordsArray = LoadRecords();
            bool isNameExistsInRecords = false;
            for (int i = 0; i < recordsArray.Count; i++)
            {
                if (playerName == recordsArray[i].UserName)
                {
                    isNameExistsInRecords = true;
                    if (MapController.score > recordsArray[i].Score)
                    {
                        recordsArray.RemoveAt(i);
                        recordsArray.Add(RecordItem.StringToRecordItem(playerName + "|" + MapController.score));
                    }
                }
            }
            if (!isNameExistsInRecords)
            recordsArray.Add(RecordItem.StringToRecordItem(playerName + "|" + MapController.score));
            List<string> recordsStringArray = new List<string>();
            foreach(var rec in recordsArray)
            {
                recordsStringArray.Add(rec.ToString());
            }
            File.WriteAllLines(recordPath, recordsStringArray);
        }

        public static List<RecordItem> LoadRecords()
        {
            string[] recordsStringArray = File.ReadAllLines(recordPath);
            List<RecordItem> recordsArray = new List<RecordItem>();
            foreach(var rec in recordsStringArray)
            {
                recordsArray.Add(RecordItem.StringToRecordItem(rec));
            }
            return recordsArray;
        }

        public static void ShowRecords(Label label3)
        {
            List<RecordItem> recordsArray = LoadRecords();
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

    public class RecordItem : IComparable<RecordItem>
    {
        public string UserName{ get; set; }
        public int Score { get; set; }

        public RecordItem()
        {
            UserName = "Anonymous";
            Score = 0;
        }
        public RecordItem(string userName, int score)
        {
            UserName = userName;
            Score = score;
        }
        public static RecordItem StringToRecordItem(string str)
        {
            if (str.Contains("|"))
            {
                string[] args = str.Split('|');
                return new RecordItem(args[0], int.Parse(args[1]));
            }
            else return new RecordItem();
        }

        public override string ToString()
        {
            return UserName+"|"+Score;
        }
        public int CompareTo(RecordItem other)
        {
            if (Score > other.Score) return 1;
            else if (Score < other.Score) return -1;
            else return 0;
        }

        public static bool operator >(RecordItem rec1, RecordItem rec2)
        {
            if (rec1.CompareTo(rec2) > 0) return true;
            else return false;
        }
        public static bool operator <(RecordItem rec1, RecordItem rec2)
        {
            if (rec2.CompareTo(rec1) > 0) return true;
            else return false;
        }
    }
}
