// AvaloniaPlayground https://github.com/LFebruary/Avalonia-playground 
// (c) 2024 Lyle February 
// Released under the MIT License

using Avalonia.Controls;
using Playground.Views;
using ReactiveUI;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static Playground.Extensions;
using static Playground.Views.MessageBox;
using static Playground.Views.ViewTools;

namespace Playground.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject, IDisposable
    {
        #region Constructor
        public BaseViewModel(Window parentView)
        {
            _parentView = parentView;
            _isOpen = true;

            _parentView.Closing -= OnClosing;
            _parentView.Closing += OnClosing;

            _parentView.Closed -= OnClosed;
            _parentView.Closed += OnClosed;

            _parentView.Opened -= OnOpened;
            _parentView.Opened += OnOpened;
        }

        protected static void OpenWindow<T>() where T : Window, new() => RunOnMainThread(() => OpenNewOrRestoreWindow<T>());

        protected static void OpenWindow(Type windowType)
        {
            if (_windows.Select(i => i.type).Contains(windowType))
            {
                RunOnMainThread(() => _windows.First(i => i.type == windowType).openWindow());
            }
        }

        protected virtual void OnOpened(object? sender, EventArgs e)
        {
        }

        protected virtual void OnClosed(object? sender, EventArgs e)
        {
            _isOpen = false;
            Dispose();
        }

        protected virtual void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
        }


        #endregion

        #region Fields
        internal Window _parentView;
        internal bool _isOpen;

        internal MessageBox? CurrentMessageBox { get; set; } = null;
        #endregion

        #region Property changed methods
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            new Thread(() => this.RaisePropertyChanged(propertyName)).Start();
        }

        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        protected void SetProperty<T>(ref T storage, T value, Action onChange, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            new Thread(() =>
            {
                this.RaisePropertyChanged(propertyName);
                onChange?.Invoke();
            }).Start();
        }

        protected void SetProperty<T>(ref T storage, T value, Action<T> onChange, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;

            new Thread(() =>
            {
                this.RaisePropertyChanged(propertyName);
                onChange?.Invoke(value);
            }).Start();
        }
        #endregion

        #region Dialogs
        internal enum Dialogtype
        {
            Success,
            Warning,
            Error
        }

        internal async Task<bool> ShowDialog(Dialogtype messageBoxType, string message, string positive = "Ok", string negative = "Cancel", string overrideTitle = "")
        {
            string title = string.IsNullOrWhiteSpace(overrideTitle)
                ? messageBoxType.ToString()
                : overrideTitle;

            return await Show(
                parent: _parentView,
                title: title,
                message: message,
                positive: positive,
                negative: negative,
                onDismiss: () => CurrentMessageBox = null);
        }

        internal async Task<bool> ShowDialog(Dialogtype messageBoxType, string message, string positive, string negative, Action onDismiss, string overrideTitle = "")
        {
            string title = string.IsNullOrWhiteSpace(overrideTitle)
                ? messageBoxType.ToString()
                : overrideTitle;

            return await Show(
                parent: _parentView,
                title: title,
                message: message,
                positive: positive,
                negative: negative,
                onDismiss: () =>
                {
                    onDismiss();
                    CurrentMessageBox = null;
                });
        }

        internal async Task<bool?> ShowDialog(Dialogtype messageBoxType, string message, string positive, string negative, string neutral, string overrideTitle = "")
        {
            string title = string.IsNullOrWhiteSpace(overrideTitle)
                ? messageBoxType.ToString()
                : overrideTitle;

            return await Show(
                parent: _parentView,
                message: message,
                title: title,
                positive: positive,
                negative: negative,
                neutral: neutral,
                onDismiss: () => CurrentMessageBox = null);
        }

        internal async Task<bool?> ShowDialog(Dialogtype messageBoxType, string message, string positive, string negative, Action onDismiss, string neutral, string overrideTitle = "")
        {
            string title = string.IsNullOrWhiteSpace(overrideTitle)
                ? messageBoxType.ToString()
                : overrideTitle;

            return await Show(
                parent: _parentView,
                title: title,
                message: message,
                positive: positive,
                negative: negative,
                neutral: neutral,
                onDismiss: () =>
                {
                    onDismiss();
                    CurrentMessageBox = null;
                });
        }

        internal async Task ShowDialog(QRCoder.BitmapByteQRCode bitmap)
        {
            await Show(_parentView, bitmap);
        }

        public void Dispose()
        {
            CurrentMessageBox?.Close();
            CurrentMessageBox = null;

            _parentView.Closing -= OnClosing;
            _parentView.Closed -= OnClosed;
            _parentView.Opened -= OnOpened;

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
