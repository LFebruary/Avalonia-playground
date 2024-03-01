using Avalonia.Controls;
using System;
using System.Collections.Generic;

namespace Playground.Views
{
    internal static partial class ViewTools
    {
        internal static readonly List<(Type type, Action openWindow)> _windows =
        [
            (typeof(LandingWindow),     () => OpenNewOrRestoreWindow<LandingWindow>()), 
            (typeof(SerialWindow),      () => OpenNewOrRestoreWindow<SerialWindow>()),
            (typeof(NetworkingWindow),  () => OpenNewOrRestoreWindow<NetworkingWindow>()),
        ];

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
