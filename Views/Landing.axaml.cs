// AvaloniaPlayground https://github.com/LFebruary/Avalonia-playground 
// (c) 2024 Lyle February 
// Released under the MIT License

using Avalonia;
using Avalonia.Markup.Xaml;
using Playground.ViewModels;
using System;

namespace Playground.Views
{
    public partial class LandingWindow : BaseWindow<LandingVM>
    {
        public LandingWindow()
        {
            _InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new LandingVM(this);
        }

        private void _InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    public class LandingVM(LandingWindow view) : BaseViewModel(view)
    {
        private object? _content;
        public object? Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public static void OpenWindowCommand(object? windowType)
        {
            if (windowType is not null and Type castedType)
            {
                OpenWindow(castedType);
            }
        }
    }
}
