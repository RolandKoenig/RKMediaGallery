using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RKMediaGallery.Controls;
using RKMediaGallery.Messages;
using RKMediaGallery.Util;

namespace RKMediaGallery;

public partial class MainWindowViewModel : OwnViewModelBase
{
    private const double DEFAULT_BUTTON_IMAGE_WIDTH = 180.0;
    private const double DEFAULT_TITLE_TEXT_MARGIN = 80.0;
    private const double DEFAULT_TITLE_FONT_SIZE = 72.0;
    
    public static readonly MainWindowViewModel EmptyViewModel = new();

    [ObservableProperty]
    private bool _canNavigateBack = false;

    [ObservableProperty]
    private bool _canExit = true;

    [ObservableProperty]
    private string _currentViewTitle = string.Empty;

    [ObservableProperty]
    private double _buttonImageWidth = DEFAULT_BUTTON_IMAGE_WIDTH;

    [ObservableProperty]
    private double _titleTextMargin = DEFAULT_TITLE_TEXT_MARGIN;

    [ObservableProperty]
    private double _titleFontSize = DEFAULT_TITLE_FONT_SIZE;
    
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

    protected override void UpdateViewHeight(double heightFactor)
    {
        base.UpdateViewHeight(heightFactor);

        this.ButtonImageWidth = DEFAULT_BUTTON_IMAGE_WIDTH * heightFactor;
        this.TitleTextMargin = DEFAULT_TITLE_TEXT_MARGIN * heightFactor;
        this.TitleFontSize = DEFAULT_TITLE_FONT_SIZE * heightFactor;
    }

    private void UpdateNavigationProperties()
    {
        if (this.AssociatedView == null)
        {
            return;
        }
        
        var srvNavigation = base.GetViewService<INavigationViewService>();
        var navStackSize = srvNavigation.NavigationStackSize;
        this.CanNavigateBack = navStackSize > 1;
        this.CanExit = navStackSize <= 1;
        this.CurrentViewTitle = srvNavigation.CurrentViewTitle;
    }

    protected override void OnAssociatedViewChanged(object? associatedView)
    {
        base.OnAssociatedViewChanged(associatedView);
        
        this.UpdateNavigationProperties();
    }

    private void OnMessageReceived(NavigationControlNavigatedMessage message)
    {
        this.UpdateNavigationProperties();
    }
}