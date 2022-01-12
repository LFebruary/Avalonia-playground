using System.Threading;

namespace Playground.ViewModels
{
    internal interface INetworkHandler
    {
        internal void RefreshData();
        internal void CancelActiveRequests();
        internal bool AnyWebRequestsRunning();

        internal CancellationTokenSource    CancellationTokenSource { get; set; }
    }
}