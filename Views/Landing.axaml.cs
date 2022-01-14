using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Playground.SerialReadSocketSend;
using Playground.ViewModels;
using System;

namespace Playground.Views
{
    public partial class LandingWindow : BaseWindow<LandingVM>
    {
        public LandingWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new LandingVM(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    public class LandingVM : BaseViewModel
    {
        public LandingVM(LandingWindow view) : base(view)
        {

        }

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
