using Avalonia;
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

        public TagUserControl(string name) : this()
        {
            this.FindControl<TextBlock>("textBlockTitle").Text = name;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
