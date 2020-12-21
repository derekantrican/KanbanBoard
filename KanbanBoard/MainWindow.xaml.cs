using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KanbanBoard.ViewModels;

namespace KanbanBoard
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.WindowState = WindowState.Normal;
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            MainWindowViewModel viewModel = new MainWindowViewModel();
            
            this.DataContext = viewModel;

            viewModel.OpenAddBoardDialog += () =>
            {
                AddBoardWindow addBoard = new AddBoardWindow(new AddBoardViewModel());
                BoardViewModel result = addBoard.ShowDialogSync<BoardViewModel>(this);
                return result;
            };

            viewModel.OpenEditBoardDialog += (board) =>
            {
                AddBoardWindow addBoard = new AddBoardWindow(new AddBoardViewModel(board));
                BoardViewModel result = addBoard.ShowDialogSync<BoardViewModel>(this);
                return result;
            };

            viewModel.ShowMessageDialog += (title, message) =>
            {
                MessageBoxWindow messageBox = new MessageBoxWindow(new MessageBoxViewModel(title, message));
                return messageBox.ShowDialogSync<string>(this);
            };

            viewModel.OpenAddColumnDialog += () =>
            {
                AddColumnWindow addColumn = new AddColumnWindow();
                ColumnViewModel result = addColumn.ShowDialogSync<ColumnViewModel>(this);
                return result;
            };
        }
    }
}
