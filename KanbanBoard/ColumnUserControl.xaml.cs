using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using KanbanBoard.Models;
using LiteDB;
using System;
using System.Linq;

namespace KanbanBoard
{
    public class ColumnUserControl : UserControl
    {
        LiteDatabase db;
        ColumnModel column;
        Action<ColumnUserControl> delete;
        ILiteCollection<TaskModel> taskCollection;
        ILiteCollection<ColumnModel> columnCollection;
        TextBlock textBlockTitle;
        TextBlock textBlockWorkCount;
        StackPanel stackPanelTask;

        public ColumnUserControl()
        {
            this.InitializeComponent();
        }

        public ColumnUserControl(ref LiteDatabase db, ref ColumnModel column, Action<ColumnUserControl> delete) : this()
        {
            this.db = db;
            this.column = column;
            this.delete = delete;

            taskCollection = db.GetCollection<TaskModel>();
            columnCollection = db.GetCollection<ColumnModel>();
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
                    columnCollection.Update(result);
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
                    columnCollection.Delete(column.Id);
                    delete(this);
                }
            };

            this.FindControl<Button>("buttonAddTask").Click += async (sender, e) =>
            {
                var addTask = new AddTaskWindow();
                var result = await addTask.ShowDialog<TaskModel>(Application.Current.ApplicationLifetime.GetMainWindow());
                if (result != null)
                {
                    result.ColumnModelId = column.Id;

                    taskCollection.Insert(result);
                    RefreshTask();
                    RefreshWorkCount();
                }
            };
        }

        private void RefreshWorkCount()
        {
            var taskCount = taskCollection.Count(x => x.ColumnModelId == column.Id);
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

        private void RefreshTask()
        {
            stackPanelTask.Children.Clear();
            taskCollection.EnsureIndex(x => x.ColumnModelId);
            stackPanelTask.Children.AddRange(taskCollection
                .Find(x => x.ColumnModelId == column.Id)
                .OrderBy(x => x.Id)
                .Select(x => new TaskUserControl(ref db, ref x, (y) => stackPanelTask.Children.Remove(y))));
        }
    }
}
