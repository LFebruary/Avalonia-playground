using Avalonia.ReactiveUI;
using Playground.ViewModels;
using System;

namespace Playground
{
    public abstract class BaseWindow<TViewModel> : ReactiveWindow<TViewModel>, IDisposable where TViewModel : BaseViewModel
    {
        private bool disposedValue;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            App.Windows.Add(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                if (DataContext is BaseViewModel vm)
                {
                    vm.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}