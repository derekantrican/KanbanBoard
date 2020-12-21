using System;

namespace KanbanBoard.ViewModels
{
    public class AddBoardViewModel : BaseViewModel
    {
        private BoardViewModel board;
        private string title;
        private string saveButtonText;

        public AddBoardViewModel()
        {
            this.board = new BoardViewModel();
            Title = "Add Board";
            SaveButtonText = "Add";
        }

        public AddBoardViewModel(BoardViewModel board)
        {
            this.board = new BoardViewModel(board.Model.Clone());
            Title = "Edit Board";
            SaveButtonText = "Save";
        }

        public Action<BoardViewModel> CloseDialog;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                FirePropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return board.Name;
            }
            set
            {
                board.Name = value;
                FirePropertyChanged();
            }
        }

        public string SaveButtonText
        {
            get
            {
                return saveButtonText;
            }
            set
            {
                saveButtonText = value;
                FirePropertyChanged();
            }
        }

        public void Save()
        {
            CloseDialog?.Invoke(board);
        }

        public void Cancel()
        {
            CloseDialog?.Invoke(null);
        }
    }
}
