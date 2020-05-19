using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KanbanBoard.Models;
using LiteDB;
using System;
using System.Linq;

namespace KanbanBoard
{
    public class TaskUserControl : UserControl
    {
        LiteDatabase db;
        TaskModel task;
        Action<TaskUserControl> delete;
        ILiteCollection<TaskModel> taskCollection;
        TextBlock textBlockTitle;
        Button buttonEdit;
        Button buttonDelete;
        TextBlock textBlockDescription;
        StackPanel stackPanelTag;

        public TaskUserControl()
        {
            this.InitializeComponent();
        }

        public TaskUserControl(ref LiteDatabase db, ref TaskModel task, Action<TaskUserControl> delete) : this()
        {
            this.db = db;
            this.task = task;
            this.delete = delete;
            taskCollection = db.GetCollection<TaskModel>();

            textBlockTitle.Text = task.Name;
            textBlockDescription.Text = task.Description;

            RefreshTag();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            textBlockTitle = this.FindControl<TextBlock>("textBlockTitle");
            buttonEdit = this.FindControl<Button>("buttonEdit");
            buttonDelete = this.FindControl<Button>("buttonDelete");
            textBlockDescription = this.FindControl<TextBlock>("textBlockDescription");
            stackPanelTag = this.FindControl<StackPanel>("stackPanelTag");

            buttonEdit.Click += async (sender, e) =>
            {
                var addTask = new AddTaskWindow(ref task);
                var result = await addTask.ShowDialog<TaskModel>(Application.Current.ApplicationLifetime.GetMainWindow());
                if (result != null)
                {
                    taskCollection.Update(result);
                    textBlockTitle.Text = result.Name;
                    textBlockDescription.Text = result.Description;
                    RefreshTag();
                }
            };

            buttonDelete.Click += async (sender, e) =>
            {
                var messageBox = new MessageBoxWindow("Confirmation", "Are you sure you want to delete this task?");
                var result = await messageBox.ShowDialog<string>(Application.Current.ApplicationLifetime.GetMainWindow());
                if (result == "Yes")
                {
                    taskCollection.Delete(task.Id);
                    delete(this);
                }
            };
        }

        private void RefreshTag()
        {
            stackPanelTag.Children.Clear();
            stackPanelTag.Children.AddRange(task.Tags.Select(x => new TagUserControl(x)));
        }
    }
}
