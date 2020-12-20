using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KanbanBoard.Models;
using System;
using System.Linq;

namespace KanbanBoard
{
    public class TaskUserControl : UserControl
    {
        TaskModel task;
        ColumnModel column;
        Action<TaskUserControl> delete;
        TextBlock textBlockTitle;
        Button buttonEdit;
        Button buttonDelete;
        TextBlock textBlockDescription;
        StackPanel stackPanelTag;

        public TaskUserControl()
        {
            this.InitializeComponent();
        }

        public TaskUserControl(TaskModel task, ColumnModel column, Action<TaskUserControl> delete) : this()
        {
            this.task = task;
            this.column = column;
            this.delete = delete;

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
                    column.Tasks.Insert(column.Tasks.IndexOf(task), result);
                    Common.SerializeBoards();

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
                    column.Tasks.Remove(task);
                    Common.SerializeBoards();

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
