using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using UI.ViewModels;

namespace UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            Resources.Add("services", App.AppHost!.Services);
        }

        // see: https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/messenger
        WeakReferenceMessenger.Default.Register<MainWindow, DoExitMessage>(this,
            (r, m) =>
            {
                // Handle the message here, with r being the recipient and m being the
                // input message. Using the recipient passed as input makes it so that
                // the lambda expression doesn't capture "this", improving performance.
                r.Close();
            });
    }
}