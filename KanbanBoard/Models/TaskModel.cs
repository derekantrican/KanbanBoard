using LiteDB;
using System.Collections.Generic;

namespace KanbanBoard.Models
{
    public class TaskModel
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ObjectId ColumnModelId { get; set; }
        public List<string> Tags { get; set; }
    }
}
