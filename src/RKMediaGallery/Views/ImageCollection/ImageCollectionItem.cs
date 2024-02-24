using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;

namespace RKMediaGallery.Views.ImageCollection;

public record ImageCollectionItem(
    string ImagePath, 
    Bitmap Bitmap,
    IRelayCommand<ImageCollectionItem> NavigateToImageCommand);