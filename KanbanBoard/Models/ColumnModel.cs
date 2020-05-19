using LiteDB;

namespace KanbanBoard.Models
{
    public class ColumnModel
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public int MaxTask { get; set; }
        public ObjectId BoardModelId { get; set; }
    }
}
