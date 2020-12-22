using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using KanbanBoard.ViewModels;
using System;

namespace KanbanBoard
{
    public class TaskUserControl : UserControl
    {
        public TaskUserControl()
        {
            this.InitializeComponent();

            this.DataContextChanged += TaskUserControl_DataContextChanged;
        }

        private void TaskUserControl_DataContextChanged(object sender, EventArgs e)
        {
            TaskViewModel viewModel = DataContext as TaskViewModel;

            this.DoubleTapped += (sender, args) => viewModel.EditTask();

            if (viewModel.OpenEditTaskDialog == null)
            {
                viewModel.OpenEditTaskDialog += (task) =>
                {
                    AddTaskWindow editTask = new AddTaskWindow(new AddTaskViewModel(task) { Columns = task.Parent.Parent.Columns, Parent = task.Parent });
                    return editTask.ShowDialogSync<TaskViewModel>(Application.Current.ApplicationLifetime.GetMainWindow());
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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        PointerPoint downPoint;
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            downPoint = e.GetCurrentPoint(this);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed && MovedMinimumDistance(e))
            {
                downPoint = e.GetCurrentPoint(this);
                DataObject data = new DataObject();
                data.Set("task", this.DataContext as TaskViewModel);

                DragDrop.DoDragDrop(e, data, DragDropEffects.Move);
            }
        }

        private bool MovedMinimumDistance(PointerEventArgs e)
        {
            return downPoint != null && Math.Abs(e.GetCurrentPoint(this).Position.X - downPoint.Position.X) >= 2 &&
                Math.Abs(e.GetCurrentPoint(this).Position.Y - downPoint.Position.Y) >= 2;
        }
    }
}
