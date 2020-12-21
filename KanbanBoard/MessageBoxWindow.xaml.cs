using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KanbanBoard.ViewModels;

namespace KanbanBoard
{
    public class MessageBoxWindow : Window
    {
        public MessageBoxWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public MessageBoxWindow(MessageBoxViewModel viewModel) : this()
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
