using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;
using RolandK.AvaloniaExtensions.ViewServices;

namespace RKMediaGallery.Views;

public partial class HomeViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly HomeViewModel EmptyViewModel = new();
    
    [ObservableProperty]
    private string _mediaDirectory;
    
    public string Title { get; } = "Home";
    
    public Control CreateViewInstance()
    {
        return new HomeView();
    }
    
    [RelayCommand]
    private async void SelectMediaDirectoryAndStart()
    {
        var srvOpenDirectoryDialog = this.GetViewService<IOpenDirectoryViewService>();
        var srvNavigation = this.GetViewService<INavigationViewService>();

        var directory = await srvOpenDirectoryDialog.ShowOpenDirectoryDialogAsync(
            "Select Media Directory");
        if (string.IsNullOrEmpty(directory))
        {
            return;
        }

        this.MediaDirectory = directory;

        var viewModel = ViewController.GetViewModelVorDirectory(directory);
        srvNavigation.NavigateTo(viewModel);
    }
}