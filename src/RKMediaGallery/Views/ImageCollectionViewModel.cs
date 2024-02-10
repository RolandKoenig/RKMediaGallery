using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using RKMediaGallery.Controls;
using RKMediaGallery.Messages;
using RKMediaGallery.Util;
using RKMediaGallery.ViewServices;

namespace RKMediaGallery.Views;

public partial class ImageCollectionViewModel : OwnViewModelBase, INavigationTarget
{
    private const double HEIGHT_MARGIN = 180;
    
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

    private void UpdateViewMaxHeight(double mainWindowHeight)
    {
        if (double.IsNaN(mainWindowHeight))
        {
            mainWindowHeight = 800.0;
        }
        
        var newMaxHeight = mainWindowHeight - HEIGHT_MARGIN;
        if (newMaxHeight < 0)
        {
            return;
        }

        this.ViewMaxHeight = newMaxHeight;
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