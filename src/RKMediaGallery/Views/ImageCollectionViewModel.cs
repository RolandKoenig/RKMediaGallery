using System.Collections.Generic;
using Avalonia.Controls;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;

namespace RKMediaGallery.Views;

public partial class ImageCollectionViewModel : OwnViewModelBase, INavigationTarget
{
    public string Title { get; }
    
    public ImageCollectionViewModel(
        string title,
        IReadOnlyList<string> imageFiles)
    {
        this.Title = title;
    }
    
    public Control CreateViewInstance()
    {
        return new ImageCollectionView();
    }
}