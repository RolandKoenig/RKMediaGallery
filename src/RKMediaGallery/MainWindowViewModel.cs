using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RKMediaGallery.Controls;
using RKMediaGallery.Util;
using RKMediaGallery.Views;
using RolandK.AvaloniaExtensions.ViewServices;

namespace RKMediaGallery;

public partial class MainWindowViewModel : OwnViewModelBase
{
    public static readonly MainWindowViewModel EmptyViewModel = new();

    [ObservableProperty]
    private string _mediaDirectory;

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