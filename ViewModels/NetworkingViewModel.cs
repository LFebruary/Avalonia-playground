// AvaloniaPlayground https://github.com/LFebruary/Avalonia-playground 
// (c) 2024 Lyle February 
// Released under the MIT License

using DynamicData;
using Playground.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Playground.ViewModels
{
    public class NetworkingViewModel : BaseViewModel
    {
        public NetworkingViewModel(NetworkingWindow networkingWindow) : base(networkingWindow)
        {
            _ResetHeaders();
        }

        public enum WindowMode
        {
            XML, JSON
        }

        private static List<Header> DefaultHeaders =>
        [
            new Header("Accept", "*/*", 0, true),
            new Header("Accept-Encoding", "gzip, deflate, br", 1, true),
            new Header("Connection", "keep-alive", 2, true),
        ];

        private WindowMode _selectedWindowMode;
        public WindowMode SelectedWindowMode
        {
            get => _selectedWindowMode;
            set => SetProperty(ref _selectedWindowMode, value, () =>
            {
                _ResetHeaders();
                Headers.Add(new Header("Content-Type", SelectedWindowMode == WindowMode.XML ? "application/xml" : "application/xml", 3, true));
                Headers.AddRange(CredentialHeaders);
            });
        }

        private void _ResetHeaders()
        {
            Headers.Clear();
            Headers.AddRange(DefaultHeaders);
            Headers.AddRange(CredentialHeaders);
        }

        private static IEnumerable<Header> CredentialHeaders =>
        [
            new Header("username", "L", 4, true),
            new Header("password", "L", 5, true)
        ];

        private ObservableCollection<Header> _headers = [];

        public ObservableCollection<Header> Headers
        {
            get => _headers;
            set => SetProperty(ref _headers, value);
        }

        private Header? _selectedHeader = null;
        public Header? SelectedHeader
        {
            get => _selectedHeader;
            set => SetProperty(ref _selectedHeader, value);
        }

        public async void AddHeaderCommand()
        {
            if (Headers.Any() && string.IsNullOrWhiteSpace(Headers.Last().Key) && string.IsNullOrWhiteSpace(Headers.Last().Value))
            {
                _ = await ShowDialog(Dialogtype.Error, "Can not create new header when previous header is still blank");
                _FocusLastItem();
            }
            else
            {
                Headers.Add(new Header(string.Empty, string.Empty, Headers.Count));
                _FocusLastItem();
            }
        }

        private void _FocusLastItem()
        {
            if (Headers.Any() && string.IsNullOrWhiteSpace(Headers.Last().Key))
            {
                ((NetworkingWindow)_parentView).FocusItem(Headers.Last());
            }
            else if (Headers.Any() && string.IsNullOrWhiteSpace(Headers.Last().Value))
            {

            }
            else
            {

            }
        }
    }
}