using Avalonia.Controls;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;

namespace RKMediaGallery.Views;

public class HomeViewModel : OwnViewModelBase, INavigationTarget
{
    public string Title => "Home";
    
    public Control CreateViewInstance()
    {
        return new HomeView();
    }
}