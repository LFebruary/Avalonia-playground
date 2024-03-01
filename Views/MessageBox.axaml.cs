// AvaloniaPlayground https://github.com/LFebruary/Avalonia-playground 
// (c) 2024 Lyle February 
// Released under the MIT License

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using System;
using System.IO;
using System.Threading.Tasks;
using static Playground.ViewModels.BaseViewModel;

namespace Playground.Views
{
    internal partial class MessageBox : Window
    {
        public MessageBox()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public bool? FinalResult { get; set; } = false;

        /// <summary>
        /// This is a MessageBox.Show invocation that displays a QR-code
        /// </summary>
        /// <param name="parent">Parent of the dialog</param>
        /// <param name="bitmap">QRCode bitmap to display</param>
        internal static Task Show(Window? parent, QRCoder.BitmapByteQRCode bitmap)
        {
            MessageBox? messageBox = new()
            {
                Title = "Scan code"
            };

            TextBlock? textBlock = messageBox.FindControl<TextBlock>("Text") ?? throw new NullReferenceException($"Text can not be null!!!");
            textBlock.IsVisible = false;

            Image? image = messageBox.FindControl<Image>("DialogImage") ?? throw new NullReferenceException($"DialogImage can not be null!!!");

            MemoryStream memoryStream = new(bitmap.GetGraphic(20));

            Avalonia.Media.Imaging.Bitmap AvaloniaBitmap = new(memoryStream);
            image.IsVisible = true;
            image.Width = 400;
            image.Height = 400;
            image.Source = AvaloniaBitmap;


            StackPanel? buttonPanel = messageBox.FindControl<StackPanel>("Buttons") ?? throw new NullReferenceException($"Button panel can not be null!!!");

            void AddButton(string caption)
            {
                Button? btn = new()
                {
                    Content = caption,
                    MinWidth = 75
                };

                btn.Click += (_, __) => messageBox.Close();
                buttonPanel.Children.Add(btn);
            }

            AddButton("Close");

            TaskCompletionSource<bool?>? tcs = new();
            messageBox.Closed += delegate { _ = tcs.TrySetResult(true); };
            if (parent is not null)
            {
                _ = messageBox.ShowDialog(parent);
            }
            else
            {
                messageBox.Show();
            }

            return tcs.Task;
        }

        /// <summary>
        /// This is a MessageBox.Show invocation that displays a dialog with positive, negative and neutral buttons
        /// </summary>
        /// <param name="parent">Parent of the dialog</param>
        /// <param name="message">Text to show in dialog</param>
        /// <param name="title">Title of dialog</param>
        /// <param name="positive">Text on positive button of dialog</param>
        /// <param name="negative">Text on negative button of dialog</param>
        /// <param name="neutral">Text on neutral button of dialog</param>
        /// <returns>Returns true when positive button is clicked, false when negative button is clicked and null when neutral button is clicked</returns>
        internal static Task<bool?> Show(Window? parent, string title, string message, string positive, string negative, string neutral)
        {
            (TaskCompletionSource<bool?> taskCompletionSource, MessageBox messageBox) = _CreateNullableBoolMessageBox(parent, title, message);

            messageBox.AddButton(positive, true);
            messageBox.AddButton(negative, false);
            messageBox.AddButton(neutral, null, true);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// This is a MessageBox.Show invocation that displays a dialog with positive and negative buttons
        /// </summary>
        /// <param name="parent">Parent of the dialog</param>
        /// <param name="message">Text to show in dialog</param>
        /// <param name="title">Title of dialog</param>
        /// <param name="positive">Text on positive button of dialog</param>
        /// <param name="negative">Text on negative button of dialog</param>
        /// <returns>Returns true when positive button is clicked and false when negative button is clicked</returns>
        internal static Task<bool> Show(Window? parent, string title, string message, string positive, string negative)
        {
            (TaskCompletionSource<bool> taskCompletionSource, MessageBox messageBox) = _CreateBoolMessageBox(parent, title, message);

            messageBox.AddButton(positive, true);
            messageBox.AddButton(negative, false, true);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// This is a MessageBox.Show invocation that displays a dialog with only one button
        /// </summary>
        /// <param name="parent">Parent of the dialog</param>
        /// <param name="title">Title of dialog</param>
        /// <param name="message">Text to show in dialog</param>
        /// <param name="button">Text on button of dialog</param>
        /// <returns>True when button is clicked</returns>
        internal static Task<bool> Show(Window? parent, string title, string message, string button = "Cancel")
        {
            (TaskCompletionSource<bool> taskCompletionSource, MessageBox messageBox) = _CreateBoolMessageBox(parent, title, message);

            messageBox.AddButton(button, true, true);

            return taskCompletionSource.Task;
        }

        internal static async Task<bool?> Show(Window? parent, string title, string message, string positive, string negative, string neutral, Action onDismiss)
        {
            bool? result = await Show(parent, title, message, positive, negative, neutral);
            onDismiss.Invoke();
            return result;
        }

        private static void _ConfigureIconAndSound(string title, MessageBox messageBox)
        {
            if (title.ToUpper().Trim().Contains("SUCCESS"))
            {
                _SetIcon(Dialogtype.Success, messageBox);
            }
            else if (title.ToUpper().Trim().Contains("ERROR"))
            {
                _SetIcon(Dialogtype.Error, messageBox);

                if (OperatingSystem.IsWindows())
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    //SystemSounds.Hand();
#pragma warning restore CA1416 // Validate platform compatibility
                }
            }
            else if (title.ToUpper().Trim().Contains("WARNING"))
            {
                _SetIcon(Dialogtype.Warning, messageBox);
                if (OperatingSystem.IsWindows())
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    //SystemSounds.Exclamation.Play();
#pragma warning restore CA1416 // Validate platform compatibility
                }
            }
        }

        internal static async Task<bool> Show(Exception error) => await Show(null, "Error", error.Message, "Exit");

        internal static async Task<bool> Show(Window owner, Exception error) => await Show(owner, "Error", error.Message, "Exit");

        internal void SetAlertIconSource(Uri uri)
        {
            var alertIcon = this.FindControl<Image>("AlertIcon") ?? throw new NullReferenceException($"AlertIconcan not be null!!!");

            alertIcon.Source = new Avalonia.Media.Imaging.Bitmap(AssetLoader.Open(uri));
            alertIcon.IsVisible = true;
        }

        private static void _SetIcon(Dialogtype type, MessageBox messageBox)
        {
            switch (type)
            {
                case Dialogtype.Success:
                    messageBox.SetAlertIconSource(new Uri("resm:Playground.Assets.check-circle-outline.png"));
                    break;
                case Dialogtype.Warning:
                    messageBox.SetAlertIconSource(new Uri("resm:Playground.Assets.alert-outline.png"));
                    break;
                case Dialogtype.Error:
                    messageBox.SetAlertIconSource(new Uri("resm:Playground.Assets.alert-circle-outline.png"));
                    break;
            }
        }

        public static async Task<bool> Show(Window? parent, string title, string message, string positive, string negative, Action onDismiss)
        {
            bool result = await Show(parent, title, message, positive, negative);
            onDismiss.Invoke();
            return result;
        }

        private static (TaskCompletionSource<bool?> taskCompletionSource, MessageBox messageBox) _CreateNullableBoolMessageBox(Window? parent, string title, string message)
        {
            MessageBox messageBox = new()
            {
                Title = title
            };


            var messageBoxTextBlock = messageBox.FindControl<TextBlock>("Text") ?? throw new NullReferenceException("Message box's text block can not be null.");
            messageBoxTextBlock.Text = message;
            messageBox.FinalResult = false;

            TaskCompletionSource<bool?>? taskCompletionSource = new();

            messageBox.Closed += delegate
            {
                _ = taskCompletionSource.TrySetResult(messageBox.FinalResult);
            };

            _ConfigureIconAndSound(title, messageBox);

            if (parent is not null)
            {
                _ = messageBox.ShowDialog(parent);
            }
            else
            {
                messageBox.Show();
            }

            return (taskCompletionSource, messageBox);
        }

        private static (TaskCompletionSource<bool> taskCompletionSource, MessageBox messageBox) _CreateBoolMessageBox(Window? parent, string title, string message)
        {
            MessageBox messageBox = new()
            {
                Title = title
            };

            var messageBoxTextBlock = messageBox.FindControl<TextBlock>("Text") ?? throw new NullReferenceException("Message box's text block can not be null.");
            messageBoxTextBlock.Text = message;
            messageBox.FinalResult = false;

            TaskCompletionSource<bool>? taskCompletionSource = new();

            messageBox.Closed += delegate
            {
                _ = taskCompletionSource.TrySetResult(messageBox.FinalResult ?? false);
            };

            _ConfigureIconAndSound(title, messageBox);

            if (parent is not null)
            {
                _ = messageBox.ShowDialog(parent);
            }
            else
            {
                messageBox.Show();
            }

            return (taskCompletionSource, messageBox);
        }
    }
}
