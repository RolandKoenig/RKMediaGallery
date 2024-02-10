using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RKMediaGallery.Controls;
using RKMediaGallery.Messages;
using RKMediaGallery.Util;

namespace RKMediaGallery;

public partial class MainWindowViewModel : OwnViewModelBase
{
    public static readonly MainWindowViewModel EmptyViewModel = new();

    [ObservableProperty]
    private bool _canNavigateBack = false;

    [ObservableProperty]
    private bool _canExit = true;

    [RelayCommand]
    private void NavigateBack()
    {
        var srvNavigation = base.GetViewService<INavigationViewService>();
        srvNavigation.NavigateBack();
    }

    [RelayCommand]
    private void Exit()
    {
        base.CloseHostWindow();
    }

    private void OnMessageReceived(NavigationControlNavigatedMessage message)
    {
        var srvNavigation = base.GetViewService<INavigationViewService>();
        var navStackSize = srvNavigation.NavigationStackSize;
        this.CanNavigateBack = navStackSize > 1;
        this.CanExit = navStackSize <= 1;
    }
}