using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace RKMediaGallery.ViewServices;

public class DispatcherViewService : ViewServiceBase, IDispatcherViewService
{
    public void DispatchInNewCycle(Action action)
    {
        Dispatcher.UIThread.Post(action);
    }

    public async void DispatchInNewCycle(Action action, TimeSpan waitTime)
    {
        await Task.Delay(waitTime);

        Dispatcher.UIThread.Post(action);
    }
}