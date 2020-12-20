using KanbanBoard.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KanbanBoard
{
    public class Common
    {
        public static List<BoardModel> Boards { get; set; } = new List<BoardModel>();
        public static BoardModel CurrentBoard;
        public static void DeserializeBoards()
        {
            if (File.Exists("Boards.json"))
            {
                Boards = JsonConvert.DeserializeObject<List<BoardModel>>(File.ReadAllText("Boards.json"));
            }

            CurrentBoard = Boards.FirstOrDefault();
        }

        public static void SerializeBoards()
        {
            File.WriteAllText("Boards.json", JsonConvert.SerializeObject(Boards));
        }
    }
}
