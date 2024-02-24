using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;

namespace RKMediaGallery.Views;

public partial class SingleImageViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly SingleImageViewModel EmptyViewModel = new(null!);

    public string Title { get; } = string.Empty;
    
    [ObservableProperty]
    private Bitmap _bitmap;

    [ObservableProperty]
    private double _screenWidth = MediaGalleryConstants.SCREEN_REFERENCE_WIDTH;

    [ObservableProperty]
    private double _screenHeight = MediaGalleryConstants.SCREEN_REFERENCE_HEIGHT;
    
    public SingleImageViewModel(Bitmap bitmap)
    {
        this.Bitmap = bitmap;
    }
    
    public Control CreateViewInstance()
    {
        return new SingleImageView();
    }

    protected override void UpdateViewHeight(double heightFactor)
    {
        base.UpdateViewHeight(heightFactor);

        this.ScreenWidth = MediaGalleryConstants.SCREEN_REFERENCE_WIDTH * heightFactor;
        this.ScreenHeight = MediaGalleryConstants.SCREEN_REFERENCE_HEIGHT * heightFactor;
    }
}