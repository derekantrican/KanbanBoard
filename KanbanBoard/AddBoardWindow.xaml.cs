using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KanbanBoard.Models;

namespace KanbanBoard
{
    public class AddBoardWindow : Window
    {
        BoardModel board;
        TextBlock textBlockTitle;
        TextBox textBoxName;
        Button buttonAdd;

        public AddBoardWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public AddBoardWindow(ref BoardModel board) : this()
        {
            this.board = board;
            textBlockTitle.Text = "Edit Board";
            textBoxName.Text = board.Name;
            buttonAdd.Content = "Save";
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            textBlockTitle = this.FindControl<TextBlock>("textBlockTitle");
            textBoxName = this.FindControl<TextBox>("textBoxName");
            buttonAdd = this.FindControl<Button>("buttonAdd");

            buttonAdd.Click += (sender, e) =>
            {
                if (board == null)
                    board = new BoardModel();

                board.Name = textBoxName.Text;
                Close(board);
            };

            this.FindControl<Button>("buttonCancel").Click += (sender, e) =>
            {
                Close(null);
            };
        }
    }
}
