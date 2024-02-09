using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace RKMediaGallery.ViewServices;

internal class MainWindowHeightProviderViewService(MainWindow mainWindow) : ViewServiceBase, IMainWindowHeightProviderViewService
{
    public double Height => mainWindow.Height;
}