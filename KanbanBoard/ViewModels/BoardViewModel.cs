using KanbanBoard.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace KanbanBoard.ViewModels
{
    public class BoardViewModel : BaseViewModel
    {
        private ObservableCollection<ColumnViewModel> columns;

        public BoardViewModel()
        {
            this.Model = new BoardModel();
        }

        public BoardViewModel(BoardModel board)
        {
            this.Model = board;
        }

        public BoardModel Model { get; set; }

        public ObservableCollection<ColumnViewModel> Columns
        {
            get
            {
                if (columns == null)
                {
                    columns = new ObservableCollection<ColumnViewModel>(Model.Columns.Select(c => new ColumnViewModel(c) { Parent = this }));
                }

                return columns;
            }
            set
            {
                columns = value;
                FirePropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return Model.Name;
            }
            set
            {
                Model.Name = value;
                FirePropertyChanged();
            }
        }

        public Guid Id
        {
            get { return Model.Id; }
        }

        public void ReplaceColumn(ColumnViewModel oldColumn, ColumnViewModel newColumn)
        {
            newColumn.Parent = this;
            Model.Columns.Replace(oldColumn.Model, newColumn.Model); //Replace Model first
            Columns.Replace(oldColumn, newColumn);
            Common.SerializeBoards();
        }

        public void DeleteColumn(ColumnViewModel column)
        {
            Model.Columns.Remove(column.Model); //Delete Model first
            Columns.Remove(column);
            Common.SerializeBoards();
        }
    }
}
