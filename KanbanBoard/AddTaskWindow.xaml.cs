using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KanbanBoard.Models;
using System.Collections.Generic;
using System.Linq;

namespace KanbanBoard
{
    public class AddTaskWindow : Window
    {
        TaskModel task;
        TextBlock textBlockTitle;
        TextBox textBoxName;
        TextBox textBoxDescription;
        TextBox textBoxTags;
        Button buttonAdd;

        public AddTaskWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public AddTaskWindow(ref TaskModel task) : this()
        {
            this.task = task;
            textBlockTitle.Text = "Edit Task";
            textBoxName.Text = task.Name;
            textBoxDescription.Text = task.Description;
            textBoxTags.Text = string.Join(",", task.Tags);
            buttonAdd.Content = "Save";
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            textBlockTitle = this.FindControl<TextBlock>("textBlockTitle");
            textBoxName = this.FindControl<TextBox>("textBoxName");
            textBoxDescription = this.FindControl<TextBox>("textBoxDescription");
            textBoxTags = this.FindControl<TextBox>("textBoxTags");
            buttonAdd = this.FindControl<Button>("buttonAdd");

            buttonAdd.Click += (sender, e) =>
            {
                if (task == null)
                    task = new TaskModel();

                task.Name = textBoxName.Text;
                task.Description = textBoxDescription.Text;
                task.Tags = textBoxTags.Text
                    ?.Split(',')
                    ?.ToList() ?? new List<string>();

                Close(task);
            };

            this.FindControl<Button>("buttonCancel").Click += (sender, e) =>
            {
                Close(null);
            };
        }
    }
}
