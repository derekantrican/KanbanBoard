using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static Avalonia.Media.Color Convert(this System.Drawing.Color color)
        {
            return Avalonia.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Drawing.Color Convert(this Avalonia.Media.Color color)
        {
            System.Drawing.Color result = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

            var colorLookup = Enum.GetValues(typeof(System.Drawing.KnownColor))
               .Cast<System.Drawing.KnownColor>()
               .Select(System.Drawing.Color.FromKnownColor)
               .ToLookup(c => c.ToArgb());

            IEnumerable<System.Drawing.Color> matchingKnownColors = colorLookup[result.ToArgb()];
            if (matchingKnownColors.Any())
                return matchingKnownColors.First();
            else
                return result;
        }

        public static string GetHexCode(this System.Drawing.Color color, bool includeTransparency = false)
        {
            if (color.IsEmpty)
                return "";

            if (includeTransparency)
                return $"#{color.ToArgb():X8}";
            else
                return $"#{(color.ToArgb() & 0x00FFFFFF):X8}";
        }
    }
}
