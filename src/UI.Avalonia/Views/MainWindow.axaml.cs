using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using UI.Avalonia.ViewModels;

namespace UI.Avalonia.Views;

public partial class MainWindow : Window
{
    private const string ServicesKey = "services";

    public MainWindow()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            Resources.Add(ServicesKey, App.AppHost!.Services);
        }

        WeakReferenceMessenger.Default.Register<MainWindow, DoExitMessage>(this,
            (r, _) => { r.Close(); });
    }
}