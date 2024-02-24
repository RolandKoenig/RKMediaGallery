using System;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace RKMediaGallery.ViewServices;

public interface IDispatcherViewService : IViewService
{
    void DispatchInNewCycle(Action action);

    void DispatchInNewCycle(Action action, TimeSpan waitTime);
}