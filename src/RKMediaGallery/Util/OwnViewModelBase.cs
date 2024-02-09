using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
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

    protected IDisposable GetScopedService<TUseCase>(out TUseCase service) 
        where TUseCase : notnull
    {
        var srvUseCases = this.GetViewService<IServiceProviderViewService>();
        return srvUseCases.GetScopedUseCase(out service);
    }
    
    protected IDisposable GetScopedService<TUseCase1, TUseCase2>(out TUseCase1 service1, out TUseCase2 service2) 
        where TUseCase1 : notnull
        where TUseCase2 : notnull
    {
        var srvUseCases = this.GetViewService<IServiceProviderViewService>();
        return srvUseCases.GetScopedUseCase(out service1, out service2);
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
        }
    }
}