using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KanbanBoard
{
    public class MessageBoxWindow : Window
    {
        public MessageBoxWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public MessageBoxWindow(string title, string body) : this()
        {
            this.FindControl<TextBlock>("textBlockTitle").Text = title;
            this.FindControl<TextBlock>("textBlockBody").Text = body;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.FindControl<Button>("buttonYes").Click += (sender, e) =>
            {
                Close("Yes");
            };

            this.FindControl<Button>("buttonNo").Click += (sender, e) =>
            {
                Close("No");
            };
        }
    }
}
