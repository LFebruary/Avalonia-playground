using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Playground.ViewModels;
using System;

namespace Playground
{
    public class ViewLocator : IDataTemplate
    {
        public IControl Build(object data)
        {
            string? name = data.GetType().FullName!.Replace("ViewModel", "View");
            Type? type = Type.GetType(name);

            return type is not null 
                ? (Control)Activator.CreateInstance(type)! 
                : new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object data)
        {
            return data is BaseViewModel;
        }
    }
}
