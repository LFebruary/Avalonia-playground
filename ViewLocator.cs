// AvaloniaPlayground https://github.com/LFebruary/Avalonia-playground 
// (c) 2024 Lyle February 
// Released under the MIT License

using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Playground.ViewModels;
using System;

namespace Playground
{
    public class ViewLocator : IDataTemplate
    {
        public bool Match(object? data)
        {
            return data is BaseViewModel;
        }

        Control? ITemplate<object?, Control?>.Build(object? param)
        {
            string? name = param?.GetType().FullName!.Replace("ViewModel", "View");
            if (name is null)
                return null;

            Type? type = Type.GetType(name);

            return type is not null
                ? (Control)Activator.CreateInstance(type)!
                : new TextBlock { Text = "Not Found: " + name };
        }
    }
}
