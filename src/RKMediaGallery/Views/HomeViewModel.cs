using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RKMediaGallery.Controls;
using RKMediaGallery.Services;
using RKMediaGallery.Util;
using RolandK.AvaloniaExtensions.ViewServices;

namespace RKMediaGallery.Views;

public partial class HomeViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly HomeViewModel EmptyViewModel = new();
    
    [ObservableProperty]
    private string _mediaDirectory;

    private bool _isFirstNavigation = true;
    
    public string Title { get; } = "Home";
    
    public Control CreateViewInstance()
    {
        return new HomeView();
    }
    
    [RelayCommand]
    private async Task SelectMediaDirectoryAndStartAsync()
    {
        var srvOpenDirectoryDialog = this.GetViewService<IOpenDirectoryViewService>();

        var directory = await srvOpenDirectoryDialog.ShowOpenDirectoryDialogAsync(
            "Select Media Directory");
        if (string.IsNullOrEmpty(directory))
        {
            return;
        }

        await NavigateToDirectoryAsync(directory);
    }

    private async Task NavigateToDirectoryAsync(string directory)
    {
        var srvNavigation = this.GetViewService<INavigationViewService>();
        base.GetService(out IRecentlyOpenedFilesService srvRecentlyOpened);

        await srvRecentlyOpened.AddOpenedFileAsync(directory);
        this.MediaDirectory = directory;
        
        var viewModel = ViewController.GetViewModelVorDirectory(directory);
        srvNavigation.NavigateTo(viewModel);
    }

    protected override async void OnAssociatedViewChanged(object? associatedView)
    {
        base.OnAssociatedViewChanged(associatedView);
        
        if ((associatedView != null) &&
            (_isFirstNavigation == true))
        {
            _isFirstNavigation = false;
            base.GetService(out IRecentlyOpenedFilesService srvRecentlyOpened);

            var recentlyOpenedFiles = await srvRecentlyOpened.GetAllRecentlyOpenedFilesAsync();
            if (recentlyOpenedFiles.Count > 0)
            {
                await NavigateToDirectoryAsync(recentlyOpenedFiles[0]);
            }
        }
    }
}