using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using RKMediaGallery.PictureConvertUtility.UseCases;
using RKMediaGallery.PictureConvertUtility.Util;
using RKMediaGallery.PictureConvertUtility.ViewService;
using RolandK.AvaloniaExtensions.ViewServices;

namespace RKMediaGallery.PictureConvertUtility;

public partial class MainWindowViewModel : OwnViewModelBase
{
    public static readonly MainWindowViewModel DesignViewModel = new MainWindowViewModel();

    [RelayCommand]
    private void Close()
    {
        this.CloseHostWindow();
    }

    [RelayCommand]
    private async Task UpdateFileNamesAsync()
    {
        var useCase = new UpdateFileNamesUseCase();
        
        await useCase.UpdateFileNamesAsync(
            this.GetViewService<IOpenDirectoryViewService>(),
            this.GetViewService<IProgressViewService>(),
            CancellationTokenUtil.CancelAfterViewDisconnected(this));
    }

    [RelayCommand]
    private async Task UpdatePictureDimensionsAsync()
    {
        var useCase = new UpdatePictureDimensionsUseCase();

        await useCase.UpdatePictureDimensionsAsync(
            this.GetViewService<IOpenDirectoryViewService>(),
            this.GetViewService<IProgressViewService>(),
            CancellationTokenUtil.CancelAfterViewDisconnected(this));
    }
}