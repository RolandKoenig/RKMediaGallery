using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using RKMediaGallery.Controls;
using RKMediaGallery.Messages;
using RKMediaGallery.Util;
using RKMediaGallery.Views.Navigation;
using RKMediaGallery.ViewServices;

namespace RKMediaGallery.Views;

public partial class NavigationViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly NavigationViewModel EmptyViewModel = new("", Array.Empty<string>());
    
    public string Title { get; }
    
    [ObservableProperty]
    private double _viewMaxHeight = 500.0;

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
    
    private void UpdateViewMaxHeight(double mainWindowHeight)
    {
        if (double.IsNaN(mainWindowHeight))
        {
            mainWindowHeight = 800.0;
        }
        
        var newMaxHeight = mainWindowHeight - MediaGalleryConstants.HEIGHT_MARGIN;
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