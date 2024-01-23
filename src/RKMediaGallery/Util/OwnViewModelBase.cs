using System;
using CommunityToolkit.Mvvm.ComponentModel;
using RKMediaGallery.ViewServices;
using RolandK.AvaloniaExtensions.Mvvm;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace RKMediaGallery.Util;

public class OwnViewModelBase : ObservableObject, IAttachableViewModel
{
    private object? _associatedView;
    
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

    protected IDisposable GetScopedUseCase<TUseCase>(out TUseCase useCase) 
        where TUseCase : notnull
    {
        var srvUseCases = this.GetViewService<IUseCaseViewService>();
        return srvUseCases.GetScopedUseCase(out useCase);
    }
    
    protected IDisposable GetScopedUseCase<TUseCase1, TUseCase2>(out TUseCase1 useCase1, out TUseCase2 useCase2) 
        where TUseCase1 : notnull
        where TUseCase2 : notnull
    {
        var srvUseCases = this.GetViewService<IUseCaseViewService>();
        return srvUseCases.GetScopedUseCase(out useCase1, out useCase2);
    }
    
    protected virtual void OnAssociatedViewChanged(object? associatedView)
    {
        
    }
}