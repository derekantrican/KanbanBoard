
using Avalonia.Media;
using KanbanBoard.Converters;
using Newtonsoft.Json;

namespace KanbanBoard.Models
{
    public class TagModel : BaseModel
    {
        [JsonConverter(typeof(ColorHexConverter))]
        public Color Color { get; set; } = System.Drawing.Color.LightGray.Convert();

        public TagModel Clone()
        {
            return new TagModel
            {
                Id = this.Id,
                Name = this.Name,
                Color = this.Color
            };
        }
    }
}
