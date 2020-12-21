using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KanbanBoard.ViewModels;

namespace KanbanBoard
{
    public class AddBoardWindow : Window
    {
        public AddBoardWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public AddBoardWindow(AddBoardViewModel viewModel) : this()
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
