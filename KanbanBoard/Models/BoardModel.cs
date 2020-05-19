using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace KanbanBoard.Models
{
    public class BoardModel
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}
