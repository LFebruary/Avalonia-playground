using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Playground.Logging;
using Playground.ViewModels;
using System;

namespace Playground
{
    public abstract class BaseWindow<TViewModel> : ReactiveWindow<TViewModel>, IDisposable where TViewModel : BaseViewModel
    {
        internal T FindControlNullSafe<T>(string name) where T : class, IControl, new()
        {
            Contract.Requires<ArgumentNullException>(this != null);
            Contract.Requires<ArgumentNullException>(name != null);

            T? foundControl = this.FindControl<T>(name);

            Debug.WriteLine(name);
            Debug.WriteLine($"Matching control: {foundControl}");

            return foundControl ?? new T();
        }

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