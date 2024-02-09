using System;
using System.IO;
using CommunityToolkit.Mvvm.Input;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;

namespace RKMediaGallery.Views.Navigation;

public partial class ThumbnailButtonViewModel(
    string subdirectory,
    string[] thumbnails) : OwnViewModelBase
{
    public static ThumbnailButtonViewModel EmptyViewModel = new ThumbnailButtonViewModel(
        string.Empty, Array.Empty<string>());
    
    public string Name { get; } = Path.GetFileName(subdirectory);

    public string[] Thumbnails { get; } = thumbnails;

    [RelayCommand]
    private void NavigateToSubdirectory()
    {
        var srvNavigation = base.GetViewService<INavigationViewService>();

        var nextView = ViewController.GetViewModelVorDirectory(subdirectory);
        srvNavigation.NavigateTo(nextView);
    }
}