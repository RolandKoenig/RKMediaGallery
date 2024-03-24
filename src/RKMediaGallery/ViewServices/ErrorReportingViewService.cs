using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using RolandK.AvaloniaExtensions.ExceptionHandling;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace RKMediaGallery.ViewServices;

public class ErrorReportingViewService(Window hostWindow) : ViewServiceBase, IErrorReportingViewService
{
    /// <inheritdoc />
    public async Task ShowErrorDialogAsync(Exception exception)
    {
        await GlobalErrorReporting.ShowGlobalExceptionDialogAsync(exception, hostWindow);
    }
}