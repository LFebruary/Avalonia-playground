using Avalonia;
using Avalonia.Controls;

namespace Playground.Views
{
    internal static partial class ViewTools
    {
        internal static void OpenNewOrRestoreWindow<T>() where T : Window, new()
        {
            bool isWindowOpen = false;

            foreach (Window w in App.Windows)
            {
                if (w is T)
                {
                    isWindowOpen = true;
                    w.Activate();
                    break;
                }
            }

            if (!isWindowOpen)
            {
                T newwindow = new();
                newwindow.Show();
            }
        }
    }
}
