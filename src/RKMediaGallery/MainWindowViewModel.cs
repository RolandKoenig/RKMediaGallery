using CommunityToolkit.Mvvm.Input;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;

namespace RKMediaGallery;

public partial class MainWindowViewModel : OwnViewModelBase
{
    public static readonly MainWindowViewModel EmptyViewModel = new();

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
}