using System.Collections.Generic;
using System.Linq;

namespace KanbanBoard.Models
{
    public class ColumnModel : BaseModel
    {
        public int MaxTask { get; set; }
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();

        public ColumnModel Clone()
        {
            return new ColumnModel
            {
                Id = this.Id,
                Name = this.Name,
                MaxTask = this.MaxTask,
                Tasks = this.Tasks.Select(t => t.Clone()).ToList(),
            };
        }
    }
}
