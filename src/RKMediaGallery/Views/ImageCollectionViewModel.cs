using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;
using RKMediaGallery.Views.ImageCollection;
using RKMediaGallery.ViewServices;

namespace RKMediaGallery.Views;

public partial class ImageCollectionViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly ImageCollectionViewModel EmptyViewModel = new("", Array.Empty<string>());

    [ObservableProperty]
    private double _viewMaxHeight = 500.0;
    
    public string Title { get; }

    public ObservableCollection<ImageCollectionItem> LoadedBitmaps { get; } = new();
    
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

    [RelayCommand]
    private void NavigateToImage(ImageCollectionItem item)
    {
        var srvNavigation = this.GetViewService<INavigationViewService>();
        var singleImageViewModel = new SingleImageViewModel(item.Bitmap);
        srvNavigation.NavigateTo(singleImageViewModel);
    }

    private async void LoadBitmaps(IReadOnlyList<string> imageFiles)
    {
        try
        {
            foreach (var actImageFilePath in imageFiles)
            {
                var actBitmap = await Task.Run(() => new Bitmap(actImageFilePath));
                this.LoadedBitmaps.Add(new ImageCollectionItem(
                    actImageFilePath, actBitmap, this.NavigateToImageCommand));

                await Task.Delay(100);
            }
        }
        catch (Exception e)
        {
            var srvErrorReporting = this.GetViewService<IErrorReportingViewService>();
            await srvErrorReporting.ShowErrorDialogAsync(e);
        }
    }

    protected override void UpdateViewHeight(double heightFactor)
    {
        base.UpdateViewHeight(heightFactor);
        
        this.ViewMaxHeight = MediaGalleryConstants.SCREEN_CONTENT_MAX_HEIGHT * heightFactor;
    }
}