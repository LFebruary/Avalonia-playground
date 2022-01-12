using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Playground.Logging;
using Playground.ViewModels;
using Playground.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Playground
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new Landing();
            }

            base.OnFrameworkInitializationCompleted();
        }
        
        public static List<Window> Windows { get; set; } = new();
    }
}
