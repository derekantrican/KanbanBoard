using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using KanbanBoard.ViewModels;
using System;

namespace KanbanBoard
{
    public class ColumnUserControl : UserControl
    {
        public ColumnUserControl()
        {
            this.InitializeComponent();

            this.DataContextChanged += ColumnUserControl_DataContextChanged;

            AddHandler(DragDrop.DropEvent, OnDrop);
        }

        private void ColumnUserControl_DataContextChanged(object sender, EventArgs e)
        {
            ColumnViewModel viewModel = DataContext as ColumnViewModel;

            if (viewModel.OpenEditColumnDialog == null)
            {
                viewModel.OpenEditColumnDialog += (column) =>
                {
                    AddColumnWindow editColumn = new AddColumnWindow(new AddColumnViewModel(column));
                    return editColumn.ShowDialogSync<ColumnViewModel>(Application.Current.ApplicationLifetime.GetMainWindow());
                };
            }

            if (viewModel.ShowMessageDialog == null)
            {
                viewModel.ShowMessageDialog += (title, message) =>
                {
                    MessageBoxWindow messageBox = new MessageBoxWindow(new MessageBoxViewModel(title, message));
                    return messageBox.ShowDialogSync<string>(Application.Current.ApplicationLifetime.GetMainWindow());
                };
            }

            if (viewModel.OpenAddTaskDialog == null)
            {
                viewModel.OpenAddTaskDialog += () =>
                {
                    AddTaskWindow addTask = new AddTaskWindow(new AddTaskViewModel() { Columns = viewModel.Parent.Columns, Parent = viewModel });
                    return addTask.ShowDialogSync<TaskViewModel>(Application.Current.ApplicationLifetime.GetMainWindow());
                };
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            TaskViewModel task = e.Data.Get("task") as TaskViewModel;
            task.Parent.DeleteTask(task);
            (this.DataContext as ColumnViewModel).AddTask(task);
        }
    }
}
