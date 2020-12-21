using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KanbanBoard
{
    public class TagUserControl : UserControl
    {
        public TagUserControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
