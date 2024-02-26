using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using RKMediaGallery.Messages;
using RolandK.AvaloniaExtensions.DependencyInjection;
using RolandK.AvaloniaExtensions.Mvvm.Controls;
using RolandK.InProcessMessaging;

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

    public string CurrentHistoryDisplayText
    {
        get
        {
            var historyItemCount = _historyItems.Count;
            if (historyItemCount <= 1)
            {
                return string.Empty;
            }
            
            var strBuilder = new StringBuilder(256);
            foreach (var actHistoryItem in _historyItems.Reverse().Take(historyItemCount - 1))
            {
                if (strBuilder.Length > 0)
                {
                    strBuilder.Append(" / ");
                }
                strBuilder.Append(actHistoryItem.ViewModel.Title);
            }
            
            return strBuilder.ToString();
        }
    }

    public int NavigationStackSize => _historyItems.Count;
    
    public NavigationControl()
    {
        this.InitializeComponent();
    }

    public void NavigateTo(INavigationTarget viewModel)
    {
        var viewObject = viewModel.CreateViewInstance();
        viewObject.DataContext = viewModel;
        
        this.CtrlTransition.Content = viewObject;
        _historyItems.Push(new NavigationHistoryItem(viewModel));
        
        this.PublishNavigatedEvent();
    }

    private void PublishNavigatedEvent()
    {
        var serviceProvider = this.GetServiceProvider();
        var messagePublisher = serviceProvider.GetRequiredService<IInProcessMessagePublisher>();
        
        messagePublisher.Publish<NavigationControlNavigatedMessage>();
    }

    public void NavigateTo<TViewModel, TNavigationArgument>(TNavigationArgument argument)
        where TViewModel : INavigationTarget, INavigationDataReceiver<TNavigationArgument>
    {
        var serviceProvider = this.GetServiceProvider();
        
        var viewModel = serviceProvider.GetRequiredService<TViewModel>();
        viewModel.OnReceiveParameterFromNavigation(argument);
        
        this.NavigateTo(viewModel);
    }
    
    public void NavigateTo<TViewModel>()
        where TViewModel : INavigationTarget
    {
        var serviceProvider = this.GetServiceProvider();
        
        var viewModel = serviceProvider.GetRequiredService<TViewModel>();

        this.NavigateTo(viewModel);
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
        if (!_historyItems.TryPeek(out var historyItem))
        {
            return;
        }
        
        var serviceProvider = this.GetServiceProvider();
        
        var viewObject = historyItem.ViewModel.CreateViewInstance();
        viewObject.DataContext = historyItem.ViewModel;
        
        this.CtrlTransition.Content = viewObject;
        
        this.PublishNavigatedEvent();
    }

    public bool CanNavigateBack()
    {
        return _historyItems.Count > 1;
    }
}