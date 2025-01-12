using Avalonia.Controls;
using Avalonia.Controls.Templates;
using UI.Avalonia.ViewModels;

namespace UI.Avalonia;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
        {
            return null;
        }


        // Todo: This is kind of disgusting.
        var viewModelType = data.GetType();
        var viewModelName = viewModelType.FullName;
        var viewName = viewModelName?.Replace("ViewModel", "View", StringComparison.Ordinal);

#pragma warning disable IL2057
        var viewType = viewName is null ? null : Type.GetType(viewName);
#pragma warning restore IL2057

        if (viewType != null)
        {
            var control = (Control)Activator.CreateInstance(viewType)!;
            control.DataContext = data;
            return control;
        }

        return new TextBlock
        {
            Text = "Not Found: " + viewName,
        };
    }

    public bool Match(object? data) => data is ViewModelBase;
}