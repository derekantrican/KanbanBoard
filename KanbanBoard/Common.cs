using KanbanBoard.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace KanbanBoard
{
    public class Common
    {
        public static List<BoardModel> Boards { get; set; }
        public static void DeserializeBoards()
        {
            if (File.Exists("Boards.json"))
            {
                Boards = JsonConvert.DeserializeObject<List<BoardModel>>(File.ReadAllText("Boards.json"));
            }
            else
            {
                Boards = new List<BoardModel>();
            }
        }

        public static void SerializeBoards()
        {
            File.WriteAllText("Boards.json", JsonConvert.SerializeObject(Boards));
        }
    }
}
