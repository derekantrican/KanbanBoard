using System;
using System.Collections.Generic;

namespace KanbanBoard.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
