using System.Collections.Generic;
using Avalonia.Controls;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;

namespace RKMediaGallery.Views;

public class NavigationViewModel : OwnViewModelBase, INavigationTarget
{
    public string Title { get; }
    
    public NavigationViewModel(
        string title,
        IReadOnlyList<string> subDirectories)
    {
        this.Title = title;
    }
    
    public Control CreateViewInstance()
    {
        return new NavigationView();
    }
}