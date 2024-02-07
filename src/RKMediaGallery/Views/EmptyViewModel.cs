using Avalonia.Controls;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;

namespace RKMediaGallery.Views;

public class EmptyViewModel(string title) : OwnViewModelBase, INavigationTarget
{
    public string Title { get; } = title;

    public Control CreateViewInstance()
    {
        return new EmptyView();
    }
}