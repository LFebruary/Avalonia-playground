// AvaloniaPlayground https://github.com/LFebruary/Avalonia-playground 
// (c) 2024 Lyle February 
// Released under the MIT License

using System.Threading;

namespace Playground.ViewModels
{
    internal interface INetworkHandler
    {
        internal void RefreshData();
        internal void CancelActiveRequests();
        internal bool AnyWebRequestsRunning();
        internal CancellationTokenSource CancellationTokenSource { get; set; }
    }
}