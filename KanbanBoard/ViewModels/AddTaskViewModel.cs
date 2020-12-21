﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KanbanBoard.ViewModels
{
    public class AddTaskViewModel : BaseViewModel
    {
        private TaskViewModel task;
        private string title;
        private string saveButtonText;

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
                return string.Join(",", task.Tags.Select(t => t.Name));
            }
            set
            {
                task.Tags = new ObservableCollection<TagViewModel>(value?.Split(',')?.Select(t => new TagViewModel { Name = t }) ?? new List<TagViewModel>());
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