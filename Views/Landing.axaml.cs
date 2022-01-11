using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Playground.ViewModels;

namespace Playground.Views
{
    public partial class Landing : Window
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

    internal class LandingVM : BaseViewModel
    {
        public LandingVM(Landing view) : base(view)
        {

        }
    }
}
