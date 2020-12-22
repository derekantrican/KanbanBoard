using KanbanBoard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KanbanBoard.ViewModels
{
    public class TaskViewModel : BaseViewModel
    {
        private ObservableCollection<TagViewModel> tags;

        public TaskViewModel()
        {
            this.Model = new TaskModel();
        }

        public TaskViewModel(TaskModel task)
        {
            this.Model = task;
        }

        public ObservableCollection<TagViewModel> Tags
        {
            get
            {
                if (tags == null)
                {
                    tags = new ObservableCollection<TagViewModel>(Model.Tags.Select(t => new TagViewModel(t) { Parent = this }));
                }

                return tags;
            }
            set
            {
                tags = value;
                FirePropertyChanged();
            }
        }

        public TaskModel Model { get; set; }
        public ColumnViewModel Parent { get; set; }

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

        public string Description
        {
            get
            {
                return Model.Description;
            }
            set
            {
                Model.Description = value;
                FirePropertyChanged();
            }
        }

        public Guid Id
        {
            get { return Model.Id; }
        }

        public Func<TaskViewModel, TaskViewModel> OpenEditTaskDialog { get; set; }
        public Func<string, string, string> ShowMessageDialog { get; set; }

        public void EditTask()
        {
            if (OpenEditTaskDialog != null)
            {
                TaskViewModel result = OpenEditTaskDialog(this);
                if (result != null)
                {
                    if (result.Parent == this.Parent)
                        this.Parent.ReplaceTask(this, result);
                    else
                    {
                        result.Parent.AddTask(result);
                        this.Parent.DeleteTask(this);
                    }
                }
            }
        }

        public void DeleteTask()
        {
            if (ShowMessageDialog != null)
            {
                string result = ShowMessageDialog("Confirmation", "Are you sure you want to delete this task?");
                if (result == "Yes")
                {
                    Parent.DeleteTask(this);
                }
            }
        }

        public void SetTags(ObservableCollection<TagViewModel> tags)
        {
            Model.Tags = tags.Select(t => t.Model).ToList();
            Tags = tags;
        }
    }
}
