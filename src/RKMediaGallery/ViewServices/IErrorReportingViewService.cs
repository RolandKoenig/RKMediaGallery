using System;
using System.Threading.Tasks;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace RKMediaGallery.ViewServices;

public interface IErrorReportingViewService : IViewService
{
    Task ShowErrorDialogAsync(Exception exception);
}