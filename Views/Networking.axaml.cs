using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Playground.Logging;
using Playground.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Playground.Views
{
    public partial class NetworkingWindow : BaseWindow<NetworkingViewModel>
    {
        public NetworkingWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new NetworkingViewModel(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        internal void FocusItem(Header header)
        {
            ListBox? headersListBox = FindControlNullSafe<ListBox>("headersListBox");
                
            headersListBox.BeginBatchUpdate();

            ListBoxItem? listBoxItem = (ListBoxItem)headersListBox
                .ItemContainerGenerator
                .ContainerFromIndex(header.Index);

            listBoxItem.Focus();
        }
    }

    public abstract class PropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChangedHandler;

        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                PropertyChangedHandler += value;
            }

            remove
            {
                PropertyChangedHandler -= value;
            }
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName]string? propertyName = "")
        {
            PropertyChangedHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected bool SetProperty<T>(ref T field, T newValue, Action onChanged, [CallerMemberName] string? propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                OnPropertyChanged(propertyName);
                onChanged();
                return true;
            }

            return false;
        }
    }

    public class Header: PropertyChanged
    {
        private int _index = -1;
        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }


        private string _key = string.Empty;
        internal string Key
        {
            get => _key;
            set => SetProperty(ref _key, value, () => OnPropertyChanged(nameof(Label)));
        }

        private string _value = string.Empty;

        public string Label => string.IsNullOrWhiteSpace(Key) ? string.Empty : Key.FirstCharToUpper();

        public Header(string label, string value, int index, bool isDefaultField = false)
        {
            Key             = label;
            Value           = value;
            IsDefaultField  = isDefaultField;
            Index           = index;
        }

        private bool _isDefaultField = false;
        public bool IsDefaultField
        {
            get => _isDefaultField;
            set => SetProperty(ref _isDefaultField, value, () => OnPropertyChanged(nameof(IsNotDefaultField)));
        }

        public bool IsNotDefaultField => !IsDefaultField;

        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}
