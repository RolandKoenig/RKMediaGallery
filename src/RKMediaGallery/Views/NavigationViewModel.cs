using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;
using RKMediaGallery.Views.Navigation;

namespace RKMediaGallery.Views;

public class NavigationViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly NavigationViewModel EmptyViewModel = new("", Array.Empty<string>());
    
    public string Title { get; }

    public ObservableCollection<ThumbnailButtonViewModel> Subdirectories { get; } = new();
    
    public NavigationViewModel(
        string title,
        IReadOnlyList<string> subDirectories)
    {
        this.Title = title;

        var orderedSubdirectories = subDirectories
            .OrderBy(Path.GetFileName);
        foreach (var actSubDirectory in orderedSubdirectories)
        {
            var thumbnails = Directory.GetFiles(
                actSubDirectory,
                MediaGalleryConstants.BROWSING_SEARCH_PATTERN_THUMBNAIL);
            
            this.Subdirectories.Add(new ThumbnailButtonViewModel(
                actSubDirectory,
                thumbnails));
        }
    }
    
    public Control CreateViewInstance()
    {
        return new NavigationView();
    }
}