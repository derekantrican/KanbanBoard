using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KanbanBoard.Models;

namespace KanbanBoard
{
    public class AddColumnWindow : Window
    {
        ColumnModel column;
        TextBlock textBlockTitle;
        TextBox textBoxName;
        NumericUpDown numericUpDownMaximumTask;
        Button buttonAdd;

        public AddColumnWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public AddColumnWindow(ref ColumnModel column) : this()
        {
            this.column = column;
            textBlockTitle.Text = "Edit Column";
            textBoxName.Text = column.Name;
            numericUpDownMaximumTask.Value = column.MaxTask;
            buttonAdd.Content = "Save";
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            textBlockTitle = this.FindControl<TextBlock>("textBlockTitle");
            textBoxName = this.FindControl<TextBox>("textBoxName");
            numericUpDownMaximumTask = this.FindControl<NumericUpDown>("numericUpDownMaximumTask");
            buttonAdd = this.FindControl<Button>("buttonAdd");

            buttonAdd.Click += (sender, e) =>
            {
                if (column == null)
                    column = new ColumnModel();

                column.Name = textBoxName.Text;
                column.MaxTask = (int)numericUpDownMaximumTask.Value;
                Close(column);
            };

            this.FindControl<Button>("buttonCancel").Click += (sender, e) =>
            {
                Close(null);
            };
        }
    }
}
