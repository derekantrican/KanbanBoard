using KanbanBoard.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace KanbanBoard.ViewModels
{
    public class ColumnViewModel : BaseViewModel
    {
        private ObservableCollection<TaskViewModel> tasks;

        public ColumnViewModel()
        {
            Model = new ColumnModel();
        }

        public ColumnViewModel(ColumnModel column)
        {
            Model = column;
        }

        public ColumnModel Model { get; set; }
        public BoardViewModel Parent { get; set; }

        public ObservableCollection<TaskViewModel> Tasks
        {
            get
            {
                if (tasks == null)
                {
                    tasks = new ObservableCollection<TaskViewModel>(Model.Tasks.Select(t => new TaskViewModel(t) { Parent = this }));
                }

                return tasks;
            }
            set
            {
                tasks = value;
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

        public string WorkItemCount
        {
            get { return $"{Tasks.Count} / {MaxTask}"; }
        }

        public int MaxTask
        {
            get
            {
                return Model.MaxTask;
            }
            set
            {
                Model.MaxTask = value;
                FirePropertyChanged();
                FirePropertyChanged(nameof(WorkItemCount));
            }
        }

        public Guid Id
        {
            get { return Model.Id; }
        }

        public Func<ColumnViewModel, ColumnViewModel> OpenEditColumnDialog { get; set; }
        public Func<string, string, string> ShowMessageDialog { get; set; }
        public Func<TaskViewModel> OpenAddTaskDialog { get; set; }

        public void EditColumn()
        {
            if (OpenEditColumnDialog != null)
            {
                ColumnViewModel result = OpenEditColumnDialog(this);
                if (result != null)
                {
                    Parent.ReplaceColumn(this, result);
                }
            }
        }

        public void DeleteColumn()
        {
            if (ShowMessageDialog != null)
            {
                string result = ShowMessageDialog("Confirmation", "Are you sure you want to delete this column?");
                if (result == "Yes")
                {
                    Parent.DeleteColumn(this);
                }
            }
        }

        public void AddTask()
        {
            if (OpenAddTaskDialog != null)
            {
                TaskViewModel result = OpenAddTaskDialog();
                if (result != null)
                {
                    AddTask(result);
                }
            }
        }

        public void AddTask(TaskViewModel task)
        {
            task.Parent = this;
            Tasks.Add(task);
            Model.Tasks.Add(task.Model);
            FirePropertyChanged(nameof(WorkItemCount));
            Common.SerializeBoards();
        }

        public void ReplaceTask(TaskViewModel oldTask, TaskViewModel newTask)
        {
            newTask.Parent = this;
            Model.Tasks.Replace(oldTask.Model, newTask.Model); //Replace Model first
            Tasks.Replace(oldTask, newTask);
            FirePropertyChanged(nameof(WorkItemCount));
            Common.SerializeBoards();
        }

        public void DeleteTask(TaskViewModel task)
        {
            Model.Tasks.Remove(task.Model); //Delete Model first
            Tasks.Remove(task);
            FirePropertyChanged(nameof(WorkItemCount));
            Common.SerializeBoards();
        }
    }
}
