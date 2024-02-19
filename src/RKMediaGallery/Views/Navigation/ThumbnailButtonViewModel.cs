using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RKMediaGallery.Controls;
using RKMediaGallery.Messages;
using RKMediaGallery.Util;
using RKMediaGallery.ViewServices;

namespace RKMediaGallery.Views.Navigation;

public partial class ThumbnailButtonViewModel(
    string subdirectory,
    string[] thumbnails) : OwnViewModelBase
{
    public static ThumbnailButtonViewModel EmptyViewModel = new ThumbnailButtonViewModel(
        string.Empty, Array.Empty<string>());

    [ObservableProperty]
    private double _imageWidth = MediaGalleryConstants.THUMBNAIL_REFERENCE_WIDTH;
    
    [ObservableProperty]
    private double _imageHeight = MediaGalleryConstants.THUMBNAIL_REFERENCE_HEIGHT;

    [ObservableProperty]
    private double _buttonMargin = MediaGalleryConstants.THUMBNAIL_BUTTON_REFERENCE_MARGIN;

    [ObservableProperty]
    private double _fontSize = MediaGalleryConstants.THUMBNAIL_BUTTON_FONT_SIZE;

    [ObservableProperty]
    private double _textBoxHeight = MediaGalleryConstants.THUMBNAIL_BUTTON_TEXTBOX_HEIGHT;
    
    [ObservableProperty]
    private double _textBoxBackgroundHeight = MediaGalleryConstants.THUMBNAIL_BUTTON_TEXTBOX_HEIGHT * 1.2;

    public string Name { get; } = Path.GetFileName(subdirectory);

    public string[] Thumbnails { get; } = thumbnails;

    [RelayCommand]
    private void NavigateToSubdirectory()
    {
        var srvNavigation = base.GetViewService<INavigationViewService>();
        
        var nextView = ViewController.GetViewModelVorDirectory(subdirectory);
        srvNavigation.NavigateTo(nextView);
    }
    
    private void UpdateViewMaxHeight(double mainWindowHeight)
    {
        if (double.IsNaN(mainWindowHeight))
        {
            mainWindowHeight = MediaGalleryConstants.SCREEN_REFERENCE_HEIGHT;
        }

        var heightFactor = mainWindowHeight / MediaGalleryConstants.SCREEN_REFERENCE_HEIGHT;
        this.ImageHeight = MediaGalleryConstants.THUMBNAIL_REFERENCE_HEIGHT * heightFactor;
        this.ImageWidth = MediaGalleryConstants.THUMBNAIL_REFERENCE_WIDTH * heightFactor;
        this.ButtonMargin = MediaGalleryConstants.THUMBNAIL_BUTTON_REFERENCE_MARGIN * heightFactor;
        this.TextBoxHeight = MediaGalleryConstants.THUMBNAIL_BUTTON_TEXTBOX_HEIGHT * heightFactor;
        this.TextBoxBackgroundHeight = MediaGalleryConstants.THUMBNAIL_BUTTON_TEXTBOX_HEIGHT * heightFactor * 1.2; 
        this.FontSize = MediaGalleryConstants.THUMBNAIL_BUTTON_FONT_SIZE * heightFactor;
    }
    
    protected override void OnAssociatedViewChanged(object? associatedView)
    {
        base.OnAssociatedViewChanged(associatedView);

        if (associatedView != null)
        {
            var srvMainWindowHeightProvider = this.GetViewService<IMainWindowHeightProviderViewService>();
            this.UpdateViewMaxHeight(srvMainWindowHeightProvider.Height);
        }
    }
    
    private void OnMessageReceived(MainWindowSizeChangedMessage message)
    {
        this.UpdateViewMaxHeight(message.Height);
    }
}