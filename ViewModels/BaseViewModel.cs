using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static Playground.Views.MessageBox;

namespace Playground.ViewModels
{
    public class BaseViewModel : ReactiveObject
    {
        #region Constructor
        public BaseViewModel(Window parentView)
        {
            _parentView = parentView;
        }
        #endregion

        #region Fields
        internal Window _parentView;
        #endregion

        #region Property changed methods
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => Dispatcher.UIThread.Post(() => this.RaisePropertyChanged(propertyName), DispatcherPriority.DataBind);

        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            Dispatcher.UIThread.Post(() => this.RaisePropertyChanged(propertyName), DispatcherPriority.DataBind);
        }

        protected void SetProperty<T>(ref T storage, T value, Action onChange, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            Dispatcher.UIThread.Post(() => this.RaisePropertyChanged(propertyName), DispatcherPriority.DataBind);
            onChange?.Invoke();
        }

        protected void SetProperty<T>(ref T storage, T value, Action<T> onChange, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            Dispatcher.UIThread.Post(() => this.RaisePropertyChanged(propertyName), DispatcherPriority.DataBind);
            onChange?.Invoke(value);
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
                negative: negative);
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
                onDismiss: onDismiss);
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
                neutral: neutral);
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
                onDismiss: onDismiss);
        }

        internal async Task ShowDialog(QRCoder.BitmapByteQRCode bitmap)
        {
            await Show(_parentView, bitmap);
        }
        #endregion
    }
}
