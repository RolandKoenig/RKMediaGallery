using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using RKMediaGallery.Messages;
using RKMediaGallery.ViewServices;
using RolandK.AvaloniaExtensions.Mvvm;
using RolandK.AvaloniaExtensions.ViewServices.Base;
using RolandK.InProcessMessaging;

namespace RKMediaGallery.Util;

public class OwnViewModelBase : ObservableObject, IAttachableViewModel
{
    private object? _associatedView;
    private IEnumerable<MessageSubscription>? _messageSubscriptions;
    
    /// <inheritdoc />
    public event EventHandler<CloseWindowRequestEventArgs>? CloseWindowRequest;
    
    /// <inheritdoc />
    public event EventHandler<ViewServiceRequestEventArgs>? ViewServiceRequest;

    /// <inheritdoc />
    public object? AssociatedView
    {
        get => _associatedView;
        set
        {
            _associatedView = value;
            this.OnAssociatedViewChanged(_associatedView);
        }
    }

    protected T? TryGetViewService<T>()
        where T : class
    {
        var requestViewServiceArgs = new ViewServiceRequestEventArgs(typeof(T));
        this.ViewServiceRequest?.Invoke(this, requestViewServiceArgs);
        return requestViewServiceArgs.ViewService as T;
    }
    
    protected T GetViewService<T>()
        where T : class
    {
        var viewService = this.TryGetViewService<T>();
        if (viewService == null)
        {
            throw new InvalidOperationException($"ViewService {typeof(T).FullName} not found!");
        }

        return viewService;
    }

    protected void CloseHostWindow(object? dialogResult = null)
    {
        if (this.CloseWindowRequest == null)
        {
            throw new InvalidOperationException("Unable to call Close on host window!");
        }
        
        this.CloseWindowRequest.Invoke(
            this, 
            new CloseWindowRequestEventArgs(dialogResult));
    }

    protected void GetService<TService>(out TService service)
        where TService : notnull
    {
        var srvServiceProvider = this.GetViewService<IServiceProviderViewService>();
        srvServiceProvider.GetService(out service);
    }

    protected IDisposable GetScopedService<TService>(out TService service) 
        where TService : notnull
    {
        var srvServiceProvider = this.GetViewService<IServiceProviderViewService>();
        return srvServiceProvider.GetScopedUseCase(out service);
    }
    
    protected IDisposable GetScopedService<TService1, TService2>(out TService1 service1, out TService2 service2) 
        where TService1 : notnull
        where TService2 : notnull
    {
        var srvServiceProvider = this.GetViewService<IServiceProviderViewService>();
        return srvServiceProvider.GetScopedUseCase(out service1, out service2);
    }

    private void UpdateViewHeightInternal(double hostHeight)
    {
        if (double.IsNaN(hostHeight))
        {
            hostHeight = MediaGalleryConstants.SCREEN_REFERENCE_HEIGHT;
        }
        
        var heightFactor = hostHeight / MediaGalleryConstants.SCREEN_REFERENCE_HEIGHT;
        this.UpdateViewHeight(heightFactor);
    }

    protected virtual void UpdateViewHeight(double heightFactor)
    {

    }

    protected virtual void OnAssociatedViewChanged(object? associatedView)
    {
        if (_messageSubscriptions != null)
        {
            _messageSubscriptions.UnsubscribeAll();
            _messageSubscriptions = null;
        }

        if (associatedView != null)
        {
            var srvServiceProvider = GetViewService<IServiceProviderViewService>();
            srvServiceProvider.GetService(out IInProcessMessageSubscriber srvMessageSubscriber);
            
            _messageSubscriptions = srvMessageSubscriber.SubscribeAllWeak(this);
            _messageSubscriptions = _messageSubscriptions.Concat(
                [srvMessageSubscriber.Subscribe<MainWindowSizeChangedMessage>(this.OnMessageReceived)]);
            
            var srvMainWindowHeightProvider = this.GetViewService<IMainWindowHeightProviderViewService>();
            this.UpdateViewHeightInternal(srvMainWindowHeightProvider.Height);
        }
    }
    
    private void OnMessageReceived(MainWindowSizeChangedMessage message)
    {
        this.UpdateViewHeightInternal(message.Height);
    }
}