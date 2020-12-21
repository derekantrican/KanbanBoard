using System;

namespace KanbanBoard.ViewModels
{
    public class AddColumnViewModel : BaseViewModel
    {
        private ColumnViewModel column;
        private string title;
        private string saveButtonText;

        public AddColumnViewModel()
        {
            this.column = new ColumnViewModel();
            Title = "Add Column";
            SaveButtonText = "Add";
        }

        public AddColumnViewModel(ColumnViewModel column)
        {
            this.column = new ColumnViewModel(column.Model.Clone());
            Title = "Edit Column";
            SaveButtonText = "Save";
        }

        public Action<ColumnViewModel> CloseDialog;

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
                return column.Name;
            }
            set
            {
                column.Name = value;
                FirePropertyChanged();
            }
        }

        public int MaxTask
        {
            get
            {
                return column.MaxTask;
            }
            set
            {
                column.MaxTask = value;
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
            CloseDialog?.Invoke(column);
        }

        public void Cancel()
        {
            CloseDialog?.Invoke(null);
        }
    }
}
