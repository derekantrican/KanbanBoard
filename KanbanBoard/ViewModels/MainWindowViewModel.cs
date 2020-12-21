using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace KanbanBoard.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ObservableCollection<BoardViewModel> boards;
        private BoardViewModel selectedBoard;

        public ObservableCollection<BoardViewModel> Boards
        {
            get
            {
                if (boards == null)
                {
                    Common.DeserializeBoards();
                    boards = new ObservableCollection<BoardViewModel>(Common.Boards.Select(b => new BoardViewModel(b)));
                    SelectedBoard = boards.FirstOrDefault();
                }

                return boards;
            }
            set
            {
                boards = value;
                FirePropertyChanged();
            }
        }

        public BoardViewModel SelectedBoard
        {
            get
            {
                return selectedBoard;
            }
            set
            {
                selectedBoard = value;
                FirePropertyChanged();
            }
        }

        public Func<BoardViewModel> OpenAddBoardDialog { get; set; }
        public Func<BoardViewModel, BoardViewModel> OpenEditBoardDialog { get; set; }
        public Func<string, string, string> ShowMessageDialog { get; set; }
        public Func<ColumnViewModel> OpenAddColumnDialog { get; set; }

        public void Add()
        {
            if (OpenAddBoardDialog != null)
            {
                BoardViewModel result = OpenAddBoardDialog();
                if (result != null)
                {
                    Boards.Add(result);
                    Common.Boards.Add(result.Model);
                    SelectedBoard = result;
                    Common.SerializeBoards();
                }
            }
        }

        public void Edit()
        {
            if (OpenEditBoardDialog != null)
            {
                BoardViewModel result = OpenEditBoardDialog(SelectedBoard);
                if (result != null)
                {
                    Boards.Replace(SelectedBoard, result);
                    SelectedBoard = result;
                    Common.SerializeBoards();
                }
            }
        }

        public void Delete()
        {
            if (ShowMessageDialog != null)
            {
                string result = ShowMessageDialog("Confirmation", "Are you sure you want to delete this board?");
                if (result == "Yes")
                {
                    Common.Boards.Remove(SelectedBoard.Model); //Delete Model first
                    Boards.Remove(SelectedBoard);
                    SelectedBoard = Boards.LastOrDefault();
                    Common.SerializeBoards();
                }
            }
        }

        public void AddColumn()
        {
            if (OpenAddColumnDialog != null)
            {
                ColumnViewModel column = OpenAddColumnDialog();
                if (column != null)
                {
                    SelectedBoard.Columns.Add(column);
                    SelectedBoard.Model.Columns.Add(column.Model);
                    column.Parent = SelectedBoard;
                    Common.SerializeBoards();
                }
            }
        }
    }
}
