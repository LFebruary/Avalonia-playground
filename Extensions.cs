using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Playground.Views;
using Avalonia.Controls;
using Avalonia.Threading;

namespace Playground
{
    internal static partial class Extensions
    {
        internal static void RunOnMainThread(Action action)    => Dispatcher.UIThread.Post(action);
        internal static void RunOnMainThread(Func<Task> task)  => Dispatcher.UIThread.Post(async () => await task());

        //These methods were for use with Console-based farFulcrum
        

        /// <summary>
        /// A custom FirstOrDefault implementation that provides an out parameter for the matching predicate
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="source">An IEnumerable to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <param name="result">First element that matches predicate on source</param>
        /// <returns>True if matching element found, false if no matching element found</returns>
        internal static bool FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out TSource? result)
        {
            if (source.Any(predicate))
            {
                result = source.First(predicate);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// This method runs a delay and task alongside each other and checks which task finishes first as a custom timeout
        /// </summary>
        /// <typeparam name="T">The type of task</typeparam>
        /// <param name="task">The task which should throw a timeout after specified timespan</param>
        /// <param name="timeoutAction">Action to be invoked if task times out</param>
        /// <param name="timeSpan">Timespan in which task should finish</param>
        /// <returns>True when task times out and false if task finishes in time</returns>
        internal static async Task<bool> OnTimeout<T>(this T task, Action<T> timeoutAction, TimeSpan timeSpan) where T: Task
        {
            Task? timeout = Task.Delay(timeSpan);

            if (await Task.WhenAny(task, timeout) == timeout)
            {
                timeoutAction(task);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This method runs a delay and task alongside each other and checks which task finishes first as a custom timeout <br/>
        /// This method runs the task without returning a result
        /// </summary>
        /// <typeparam name="T">The type of task</typeparam>
        /// <param name="task">The task which should throw a timeout after specified timespan</param>
        /// <param name="timeoutAction">Action to be invoked if task times out</param>
        /// <param name="timeSpan">Timespan in which task should finish</param>
        internal static async Task OnTimeoutWithoutResult<T>(this T task, Action<T> timeoutAction, TimeSpan timeSpan) where T : Task
        {
            Task? timeout = Task.Delay(timeSpan);

            if (await Task.WhenAny(task, timeout) == timeout)
            {
                timeoutAction(task);
            }
        }

        /// <summary>
        /// This method runs a delay and task alongside each other and checks which task finishes first as a custom timeout
        /// </summary>
        /// <typeparam name="T">The type of task</typeparam>
        /// <param name="task">The task which should throw a timeout after specified timespan</param>
        /// <param name="timeSpan">Timespan in which task should finish</param>
        /// <exception cref="TimeoutException">If task does not finish in time, a timeout exception is thrown</exception>
        /// <returns>True when task times out and false if task finishes in time</returns>
        internal static async Task<bool> OnTimeout<T>(this T task, TimeSpan timeSpan) where T: Task
        {
            return await OnTimeout(task, _ => throw new TimeoutException(), timeSpan);
        }

        /// <summary>
        /// This method is used to add button on MessageBox and result associated with button on MessageBox
        /// </summary>
        /// <param name="messageBox">MessageBox to add button to</param>
        /// <param name="buttonText">Text to appear on button</param>
        /// <param name="buttonResult">Result returned by clicking button</param>
        /// <param name="isDefault">Whether this button's result should return by default</param>
        internal static void AddButton(this MessageBox messageBox, string buttonText, bool? buttonResult, bool isDefault = false)
        {
            Button? button = new() { Content = buttonText, MinWidth = 75 };
            StackPanel? buttonPanel = messageBox.FindControl<StackPanel>("Buttons");

            button.Click += (_, __) => {
                messageBox.FinalResult = buttonResult;
                messageBox.Close();
            };

            buttonPanel.Children.Add(button);

            if (isDefault)
            {
                messageBox.FinalResult = buttonResult;
            }
        }
    }
}
