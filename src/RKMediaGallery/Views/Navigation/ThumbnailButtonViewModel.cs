using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;

namespace RKMediaGallery.Views.Navigation;

public partial class ThumbnailButtonViewModel(
    string subdirectory,
    bool isBigSize,
    string[] thumbnails) : OwnViewModelBase
{
    public static ThumbnailButtonViewModel EmptyViewModel = new ThumbnailButtonViewModel(
        string.Empty, false, Array.Empty<string>());

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

    protected override void UpdateViewHeight(double heightFactor)
    {
        base.UpdateViewHeight(heightFactor);

        if (isBigSize)
        {
            heightFactor *= 2;
        }
        
        this.ImageHeight = MediaGalleryConstants.THUMBNAIL_REFERENCE_HEIGHT * heightFactor;
        this.ImageWidth = MediaGalleryConstants.THUMBNAIL_REFERENCE_WIDTH * heightFactor;
        this.ButtonMargin = MediaGalleryConstants.THUMBNAIL_BUTTON_REFERENCE_MARGIN * heightFactor;
        this.TextBoxHeight = MediaGalleryConstants.THUMBNAIL_BUTTON_TEXTBOX_HEIGHT * heightFactor;
        this.TextBoxBackgroundHeight = MediaGalleryConstants.THUMBNAIL_BUTTON_TEXTBOX_HEIGHT * heightFactor * 1.2; 
        this.FontSize = MediaGalleryConstants.THUMBNAIL_BUTTON_FONT_SIZE * heightFactor;
    }
}