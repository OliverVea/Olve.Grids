using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        if (!Avalonia.Controls.Design.IsDesignMode)
        {
        }
    }

    [RelayCommand]
    private void DoExit()
    {
        // the viewmodel should never talk to the view (MainWindow) directly, so we can't close the program here.
        // the solution I use here is to send a (self defined) DoExitMessage which will be handled by the view.
        WeakReferenceMessenger.Default.Send<DoExitMessage>();
    }
}