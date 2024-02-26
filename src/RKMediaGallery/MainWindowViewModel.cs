using Avalonia;
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
    private const double DEFAULT_SUBTITLE_FONT_SIZE = 32.0;
    private const double DEFAULT_BUTTON_IMAGE_MARGIN = 15.0;
    private const double DEFAULT_BUTTON_BORDER_THICKNESS = 10.0;
    
    public static readonly MainWindowViewModel EmptyViewModel = new();

    [ObservableProperty]
    private bool _canNavigateBack = false;

    [ObservableProperty]
    private bool _canExit = true;

    [ObservableProperty]
    private string _currentViewTitle = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(this.IsCurrentHistoryVisible))]
    private string _currentHistoryDisplayText = string.Empty;

    public bool IsCurrentHistoryVisible => !string.IsNullOrEmpty(this.CurrentHistoryDisplayText);
    
    [ObservableProperty]
    private double _buttonImageSideWidth = DEFAULT_BUTTON_IMAGE_WIDTH;

    [ObservableProperty] 
    private CornerRadius _buttonCornerRadius = new(
        DEFAULT_BUTTON_IMAGE_WIDTH / 2,
        DEFAULT_BUTTON_IMAGE_WIDTH / 2);

    [ObservableProperty]
    private Thickness _buttonImageMargin = new Thickness(
        DEFAULT_TITLE_TEXT_MARGIN);

    [ObservableProperty]
    private Thickness _buttonBorderThickness = new Thickness(
        DEFAULT_BUTTON_BORDER_THICKNESS);
    
    [ObservableProperty]
    private double _titleTextMargin = DEFAULT_TITLE_TEXT_MARGIN;

    [ObservableProperty]
    private double _titleFontSize = DEFAULT_TITLE_FONT_SIZE;

    [ObservableProperty]
    private double _subtitleFontSize = DEFAULT_SUBTITLE_FONT_SIZE;
    
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
        
        this.ButtonImageSideWidth = DEFAULT_BUTTON_IMAGE_WIDTH * heightFactor;
        this.ButtonCornerRadius = new CornerRadius(
            DEFAULT_BUTTON_IMAGE_WIDTH * heightFactor * 0.5,
            DEFAULT_BUTTON_IMAGE_WIDTH * heightFactor * 0.5);
        this.ButtonImageMargin = new Thickness(
            DEFAULT_BUTTON_IMAGE_MARGIN * heightFactor);
        this.ButtonBorderThickness = new Thickness(
            DEFAULT_BUTTON_BORDER_THICKNESS * heightFactor);
        this.TitleTextMargin = DEFAULT_TITLE_TEXT_MARGIN * heightFactor;
        this.TitleFontSize = DEFAULT_TITLE_FONT_SIZE * heightFactor;
        this.SubtitleFontSize = DEFAULT_SUBTITLE_FONT_SIZE * heightFactor;
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
        this.CurrentHistoryDisplayText = !string.IsNullOrEmpty(this.CurrentViewTitle) 
            ? srvNavigation.CurrentHistoryDisplayText
            : string.Empty;
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