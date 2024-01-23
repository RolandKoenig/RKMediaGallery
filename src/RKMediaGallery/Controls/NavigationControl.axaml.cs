using System.Collections.Generic;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using RolandK.AvaloniaExtensions.DependencyInjection;
using RolandK.AvaloniaExtensions.Mvvm.Controls;

namespace RKMediaGallery.Controls;

public partial class NavigationControl : ViewServiceHostUserControl
{
    private Stack<NavigationHistoryItem> _historyItems = new();
    
    public string CurrentViewTitle
    {
        get
        {
            if (this.CtrlTransition.Content is not StyledElement control) { return string.Empty; }
            if (control.DataContext is not INavigationTarget navTarget) { return string.Empty; }
            return navTarget.Title;
        }
    }
    
    public NavigationControl()
    {
        this.InitializeComponent();
    }

    public void NavigateTo<TViewModel, TNavigationArgument>(TNavigationArgument argument)
        where TViewModel : INavigationTarget, INavigationDataReceiver<TNavigationArgument>
    {
        var serviceProvider = this.GetServiceProvider();
        
        var viewModel = serviceProvider.GetRequiredService<TViewModel>();
        viewModel.OnReceiveParameterFromNavigation(argument);
        
        var viewObject = viewModel.CreateViewInstance();
        viewObject.DataContext = viewModel;
        
        this.CtrlTransition.Content = viewObject;
        _historyItems.Push(new NavigationHistoryItem(viewModel));
    }
    
    public void NavigateTo<TViewModel>()
        where TViewModel : INavigationTarget
    {
        var serviceProvider = this.GetServiceProvider();
        
        var viewModel = serviceProvider.GetRequiredService<TViewModel>();
        
        var viewObject = viewModel.CreateViewInstance();
        viewObject.DataContext = viewModel;
        
        this.CtrlTransition.Content = viewObject;
        _historyItems.Push(new NavigationHistoryItem(viewModel));
    }

    public bool IsCurrentlyOn<TViewModel>()
    {
        if (this.CtrlTransition.Content == null) { return false; }

        return
            (this.CtrlTransition.Content is StyledElement currentView) &&
            (currentView.DataContext is TViewModel);
    }

    public bool IsCurrentlyOnAnyView()
    {
        return this.CtrlTransition.Content != null;
    }

    public void NavigateBack()
    {
        if (!this.CanNavigateBack()) { return; }

        _historyItems.Pop();
        if (!_historyItems.TryPop(out var viewModel))
        {
            return;
        }

        if (viewModel is not INavigationTarget viewModelNavTarget)
        {
            return;
        }
        
        var serviceProvider = this.GetServiceProvider();
        
        var viewObject = viewModelNavTarget.CreateViewInstance();
        viewObject.DataContext = viewModel;
        
        this.CtrlTransition.Content = viewObject;
    }

    public bool CanNavigateBack()
    {
        return _historyItems.Count > 1;
    }
}