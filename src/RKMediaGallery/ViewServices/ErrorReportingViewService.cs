using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using RKMediaGallery.ExceptionViewer;
using RKMediaGallery.ExceptionViewer.Data;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace RKMediaGallery.ViewServices;

public class ErrorReportingViewService(Window hostWindow) : ViewServiceBase, IErrorReportingViewService
{
    /// <inheritdoc />
    public async Task ShowErrorDialogAsync(Exception exception)
    {
        var exceptionInfo = new ExceptionInfo(exception);
        
        var dialog = new UnexpectedErrorDialog();
        dialog.DataContext = exceptionInfo;
        await dialog.ShowDialog(hostWindow);
    }
}