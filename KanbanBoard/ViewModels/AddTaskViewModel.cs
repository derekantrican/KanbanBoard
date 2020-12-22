using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace KanbanBoard.ViewModels
{
    public class AddTaskViewModel : BaseViewModel
    {
        private TaskViewModel task;
        private string title;
        private string saveButtonText;
        private ObservableCollection<ColumnViewModel> columns = new ObservableCollection<ColumnViewModel>();

        public AddTaskViewModel()
        {
            this.task = new TaskViewModel();
            Title = "Add Task";
            SaveButtonText = "Add";
        }

        public AddTaskViewModel(TaskViewModel task)
        {
            this.task = new TaskViewModel(task.Model.Clone());
            Title = "Edit Task";
            SaveButtonText = "Save";
        }

        public Action<TaskViewModel> CloseDialog;

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
                return task.Name;
            }
            set
            {
                task.Name = value;
                FirePropertyChanged();
            }
        }

        public string Description
        {
            get
            {
                return task.Description;
            }
            set
            {
                task.Description = value;
                FirePropertyChanged();
            }
        }

        public string Tags
        {
            get
            {
                return string.Join(",", task.Tags.Select(t => 
                {
                    if (t.Color.Convert().IsEmpty || t.Color.Convert().Equals(System.Drawing.Color.LightGray)) //LightGray is default
                        return t.Name;
                    else
                    {
                        return $"{t.Name} ({t.Color.Convert().GetHexCode(true)})";
                    }
                }));
            }
            set
            {
                task.SetTags(new ObservableCollection<TagViewModel>(value?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)?.Select(t =>
                {
                    Match color = Regex.Match(t, @"\((?<color>#(?:[0-9a-fA-F]{3}|[0-9a-fA-F]{8}){1,2})\)");
                    if (color.Success)
                        return new TagViewModel { Name = t.Replace(color.Value, "").Trim(), Color = System.Drawing.ColorTranslator.FromHtml(color.Groups["color"].Value).Convert() };
                    else
                        return new TagViewModel { Name = t };
                }) ?? new List<TagViewModel>()));
                FirePropertyChanged();
            }
        }

        public ObservableCollection<ColumnViewModel> Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
                FirePropertyChanged();
                FirePropertyChanged(nameof(Parent));
            }
        }

        public ColumnViewModel Parent
        {
            get
            {
                return task.Parent;
            }
            set
            {
                task.Parent = value;
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
            CloseDialog?.Invoke(task);
        }

        public void Cancel()
        {
            CloseDialog?.Invoke(null);
        }
    }
}
