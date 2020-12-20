using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using KanbanBoard.Models;
using System;
using System.Linq;

namespace KanbanBoard
{
    public class ColumnUserControl : UserControl
    {
        ColumnModel column;
        Action<ColumnUserControl> delete;
        TextBlock textBlockTitle;
        TextBlock textBlockWorkCount;
        StackPanel stackPanelTask;

        public ColumnUserControl()
        {
            this.InitializeComponent();

            AddHandler(DragDrop.DropEvent, OnDrop);
        }

        public ColumnUserControl(ColumnModel column, Action<ColumnUserControl> delete) : this()
        {
            this.column = column;
            this.delete = delete;

            textBlockTitle.Text = column.Name;

            RefreshWorkCount();
            RefreshTask();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            textBlockTitle = this.FindControl<TextBlock>("textBlockTitle");
            textBlockWorkCount = this.FindControl<TextBlock>("textBlockWorkCount");
            stackPanelTask = this.FindControl<StackPanel>("stackPanelTask");

            this.FindControl<Button>("buttonEdit").Click += async (sender, e) =>
            {
                var addColumn = new AddColumnWindow(ref column);
                var result = await addColumn.ShowDialog<ColumnModel>(Application.Current.ApplicationLifetime.GetMainWindow());
                if (result != null)
                {
                    Common.CurrentBoard.Columns.Insert(Common.CurrentBoard.Columns.IndexOf(column), result);
                    Common.SerializeBoards();

                    textBlockTitle.Text = result.Name;
                    RefreshWorkCount();
                }
            };

            this.FindControl<Button>("buttonDelete").Click += async (sender, e) =>
            {
                var messageBox = new MessageBoxWindow("Confirmation", "Are you sure you want to delete this task?");
                var result = await messageBox.ShowDialog<string>(Application.Current.ApplicationLifetime.GetMainWindow());
                if (result == "Yes")
                {
                    Common.CurrentBoard.Columns.Remove(column);
                    Common.SerializeBoards();

                    delete(this);
                }
            };

            this.FindControl<Button>("buttonAddTask").Click += async (sender, e) =>
            {
                var addTask = new AddTaskWindow();
                var result = await addTask.ShowDialog<TaskModel>(Application.Current.ApplicationLifetime.GetMainWindow());
                if (result != null)
                {
                    column.Tasks.Add(result);
                    Common.SerializeBoards();

                    RefreshTask();
                    RefreshWorkCount();
                }
            };
        }

        private void RefreshWorkCount()
        {
            var taskCount = column.Tasks.Count;
            textBlockWorkCount.Text = $"{taskCount} / {column.MaxTask}";
            if (taskCount > column.MaxTask)
            {
                textBlockWorkCount.Foreground = Brush.Parse("Red");
                textBlockTitle.Foreground = Brush.Parse("Red");
            }
            else
            {
                textBlockWorkCount.Foreground = Brush.Parse("White");
                textBlockTitle.Foreground = Brush.Parse("White");
            }
        }

        public void RefreshTask()
        {
            stackPanelTask.Children.Clear();
            stackPanelTask.Children.AddRange(column.Tasks
                .Select(x => new TaskUserControl(x, column, (y) => stackPanelTask.Children.Remove(y))));
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            TaskUserControl task = e.Data.Get("task") as TaskUserControl;
            task.column.Tasks.Remove(task.task);

            IControl parent = task.Parent;
            while (parent != null && !(parent is ColumnUserControl))
            {
                parent = parent.Parent;
            }

            (parent as ColumnUserControl).RefreshTask();

            this.column.Tasks.Add(task.task);

            Common.SerializeBoards();

            RefreshTask();
            RefreshWorkCount();
        }
    }
}
