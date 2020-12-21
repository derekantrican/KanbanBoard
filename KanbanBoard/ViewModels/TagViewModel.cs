using Avalonia.Media;
using KanbanBoard.Models;

namespace KanbanBoard.ViewModels
{
    public class TagViewModel : BaseViewModel
    {
        public TagViewModel()
        {
            Model = new TagModel();
        }

        public TagViewModel(TagModel column)
        {
            Model = column;
        }

        public TagModel Model { get; set; }
        public TaskViewModel Parent { get; set; }

        public string Name
        {
            get
            {
                return Model.Name;
            }
            set
            {
                Model.Name = value;
                FirePropertyChanged();
            }
        }

        public Color Color
        {
            get
            {
                return Model.Color;
            }
            set
            {
                Model.Color = value;
                FirePropertyChanged();
            }
        }
    }
}
