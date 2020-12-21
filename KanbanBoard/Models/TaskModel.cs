using System;
using System.Collections.Generic;
using System.Linq;

namespace KanbanBoard.Models
{
    public class TaskModel : BaseModel
    {
        public string Description { get; set; }
        public List<TagModel> Tags { get; set; } = new List<TagModel>();

        public TaskModel Clone()
        {
            return new TaskModel
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Tags = this.Tags.Select(t => t.Clone()).ToList(),
            };
        }
    }
}
