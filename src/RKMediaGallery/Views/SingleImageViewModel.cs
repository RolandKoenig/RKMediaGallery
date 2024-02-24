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
    
    public SingleImageViewModel(Bitmap bitmap)
    {
        this.Bitmap = bitmap;
    }
    
    public Control CreateViewInstance()
    {
        return new SingleImageView();
    }
}