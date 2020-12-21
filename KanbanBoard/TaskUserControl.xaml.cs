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

            if (viewModel.OpenEditTaskDialog == null)
            {
                viewModel.OpenEditTaskDialog += (task) =>
                {
                    AddTaskWindow editTask = new AddTaskWindow(new AddTaskViewModel(task));
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

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                DataObject data = new DataObject();
                data.Set("task", this.DataContext as TaskViewModel);

                DragDrop.DoDragDrop(e, data, DragDropEffects.Move);
            }
        }
    }
}
