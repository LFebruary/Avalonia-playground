// AvaloniaPlayground https://github.com/LFebruary/Avalonia-playground 
// (c) 2024 Lyle February 
// Released under the MIT License

using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Playground.Logging;
using Playground.ViewModels;
using System;
using System.Diagnostics.Contracts;

namespace Playground
{
    public abstract class BaseWindow<TViewModel> : ReactiveWindow<TViewModel>, IDisposable where TViewModel : BaseViewModel
    {
        internal T FindControlNullSafe<T>(string name) where T : Control, new()
        {
            Contract.Requires<ArgumentNullException>(this is not null);
            Contract.Requires<ArgumentNullException>(name is not null);

            T? foundControl = this.FindControl<T>(name!);

            CustomDebug.WriteLine(name);
            CustomDebug.WriteLine($"Matching control: {foundControl}");

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