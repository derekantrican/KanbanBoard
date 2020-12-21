
namespace KanbanBoard.Models
{
    public class TagModel : BaseModel
    {
        public TagModel Clone()
        {
            return new TagModel
            {
                Id = this.Id,
                Name = this.Name
            };
        }
    }
}
