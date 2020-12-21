using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KanbanBoard.ViewModels;

namespace KanbanBoard
{
    public class AddTaskWindow : Window
    {
        public AddTaskWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public AddTaskWindow(AddTaskViewModel viewModel) : this()
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
