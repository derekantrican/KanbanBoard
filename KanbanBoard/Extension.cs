using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace KanbanBoard
{
    public static class Extension
    {
        public static Window GetMainWindow(this IApplicationLifetime lifetime)
        {
            return ((IClassicDesktopStyleApplicationLifetime)lifetime).MainWindow;
        }
    }
}
