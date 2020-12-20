using System;
using System.Collections.Generic;

namespace KanbanBoard.Models
{
    public class ColumnModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int MaxTask { get; set; }
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}
