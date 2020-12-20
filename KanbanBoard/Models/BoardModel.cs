using System;
using System.Collections.Generic;
using System.Text;

namespace KanbanBoard.Models
{
    public class BoardModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<ColumnModel> Columns { get; set; } = new List<ColumnModel>();
    }
}
