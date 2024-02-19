using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;

namespace RKMediaGallery.Views;

public partial class ImageCollectionViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly ImageCollectionViewModel EmptyViewModel = new("", Array.Empty<string>());

    [ObservableProperty]
    private double _viewMaxHeight = 500.0;
    
    public string Title { get; }

    public ObservableCollection<Bitmap> LoadedBitmaps { get; } = new();
    
    public ImageCollectionViewModel(
        string directory,
        IReadOnlyList<string> imageFiles)
    {
        this.Title = Path.GetFileName(directory);

        this.LoadBitmaps(imageFiles);
    }
    
    public Control CreateViewInstance()
    {
        return new ImageCollectionView();
    }

    private async void LoadBitmaps(IReadOnlyList<string> imageFiles)
    {
        foreach (var actImage in imageFiles)
        {
            var actBitmap = await Task.Run(() => new Bitmap(actImage));
            this.LoadedBitmaps.Add(actBitmap);

            await Task.Delay(100);
        }
    }

    protected override void UpdateViewHeight(double heightFactor)
    {
        base.UpdateViewHeight(heightFactor);
        
        this.ViewMaxHeight = MediaGalleryConstants.SCREEN_CONTENT_MAX_HEIGHT * heightFactor;
    }
}