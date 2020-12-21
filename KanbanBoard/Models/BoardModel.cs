using System;
using System.Collections.Generic;
using System.Linq;

namespace KanbanBoard.Models
{
    public class BoardModel : BaseModel
    {
        public List<ColumnModel> Columns { get; set; } = new List<ColumnModel>();

        public BoardModel Clone()
        {
            return new BoardModel
            {
                Id = this.Id,
                Name = this.Name,
                Columns = this.Columns.Select(c => c.Clone()).ToList(),
            };
        }
    }
}
