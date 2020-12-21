using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KanbanBoard.ViewModels;

namespace KanbanBoard
{
    public class AddColumnWindow : Window
    {
        public AddColumnWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public AddColumnWindow(AddColumnViewModel viewModel) : this()
        {
            this.DataContext = viewModel;
            viewModel.CloseDialog += (result) => Close(result);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
