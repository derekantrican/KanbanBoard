using Avalonia.Controls;
using Avalonia.Threading;
using System.Collections.Generic;
using System.Threading;

namespace KanbanBoard
{
    public static class ExtensionMethods
    {
        public static T ShowDialogSync<T>(this Window window, Window parentWindow)
        {
            T result = default;

            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                if (Dispatcher.UIThread.CheckAccess())
                {
                    window.ShowDialog<T>(parentWindow).ContinueWith(t =>
                    {
                        result = t.Result;
                        source.Cancel();
                    });

                    Dispatcher.UIThread.MainLoop(source.Token);
                }
                else
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        window.ShowDialog<T>(parentWindow).ContinueWith(t =>
                        {
                            result = t.Result;
                            source.Cancel();
                        });
                    });

                    while (!source.IsCancellationRequested) { } //Loop until dialog is closed
                }
            }

            return result;
        }

        public static void Replace<T>(this IList<T> collection, T oldItem, T newItem)
        {
            collection.Insert(collection.IndexOf(oldItem), newItem);
            collection.Remove(oldItem);
        }
    }
}
