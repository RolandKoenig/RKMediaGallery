using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;
using RKMediaGallery.Views.Navigation;
using RKMediaGallery.ViewServices;

namespace RKMediaGallery.Views;

public partial class NavigationViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly NavigationViewModel EmptyViewModel = new("", Array.Empty<string>());

    private Vector? _scrollOffsetOnLastClose;
    
    public string Title { get; }
    
    [ObservableProperty]
    private double _viewMaxHeight = MediaGalleryConstants.SCREEN_CONTENT_MAX_HEIGHT;

    [ObservableProperty]
    private Vector _scrollOffset = Vector.Zero;
    
    public ObservableCollection<ThumbnailButtonViewModel> Subdirectories { get; } = new();
    
    public NavigationViewModel(
        string title,
        IReadOnlyList<string> subDirectories)
    {
        this.Title = title;

        var totalSubdirectoryCount = subDirectories.Count;
        var orderedSubdirectories = subDirectories
            .OrderBy(Path.GetFileName);
        foreach (var actSubDirectory in orderedSubdirectories)
        {
            var thumbnails = Directory.GetFiles(
                actSubDirectory,
                MediaGalleryConstants.BROWSING_SEARCH_PATTERN_THUMBNAIL);
            
            this.Subdirectories.Add(new ThumbnailButtonViewModel(
                actSubDirectory,
                totalSubdirectoryCount < 5,
                thumbnails));
        }
    }
    
    public Control CreateViewInstance()
    {
        return new NavigationView();
    }
    
    protected override void UpdateViewHeight(double heightFactor)
    {
        base.UpdateViewHeight(heightFactor);
        
        this.ViewMaxHeight = MediaGalleryConstants.SCREEN_CONTENT_MAX_HEIGHT * heightFactor;
    }

    protected override void OnAssociatedViewChanged(object? associatedView)
    {
        base.OnAssociatedViewChanged(associatedView);
        
        if ((associatedView == null) &&
            (this.ScrollOffset != Vector.Zero))
        {
            _scrollOffsetOnLastClose = this.ScrollOffset;
        }

        if ((associatedView != null) &&
            (_scrollOffsetOnLastClose.HasValue))
        {
            var srvDispatcher = this.GetViewService<IDispatcherViewService>();

            var newScrollOffset = _scrollOffsetOnLastClose.Value;
            srvDispatcher.DispatchInNewCycle(
                () => this.ScrollOffset = newScrollOffset,
                TimeSpan.FromMilliseconds(200));
            
            _scrollOffsetOnLastClose = null;
        }
    }
}