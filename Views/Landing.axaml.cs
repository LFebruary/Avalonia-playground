using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Playground.SerialReadSocketSend;
using Playground.ViewModels;
using System;

namespace Playground.Views
{
    public partial class Landing : BaseWindow<LandingVM>
    {
        public Landing()
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
        public LandingVM(Landing view) : base(view)
        {
            SelectedTabIndex = 0;
        }

        protected override void OnOpened(object? sender, EventArgs e)
        {
            base.OnOpened(sender, e);

            OpenWindow<SerialWindow>();
        }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        private object? _content;
        public object Content
        {
            get => _content ?? "";
            set => SetProperty(ref _content, value);
        }

        public static void CreateSerialWindow()
        {
            OpenWindow<SerialWindow>();
        }
    }
}
