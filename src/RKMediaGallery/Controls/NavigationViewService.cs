using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace RKMediaGallery.Controls;

internal class NavigationViewService(NavigationControl navigationControl) : ViewServiceBase, INavigationViewService
{
    public string CurrentViewTitle => navigationControl.CurrentViewTitle;

    public string CurrentHistoryDisplayText => navigationControl.CurrentHistoryDisplayText;

    public int NavigationStackSize => navigationControl.NavigationStackSize;

    /// <inheritdoc />
    public void NavigateTo<TViewModel, TNavigationArgument>(TNavigationArgument argument) 
        where TViewModel : INavigationTarget, INavigationDataReceiver<TNavigationArgument>
    {
        navigationControl.NavigateTo<TViewModel, TNavigationArgument>(argument);
    }

    public void NavigateTo(INavigationTarget targetViewModel)
    {
        navigationControl.NavigateTo(targetViewModel);
    }

    /// <inheritdoc />
    public void NavigateTo<TViewModel>() where TViewModel : INavigationTarget
    {
        navigationControl.NavigateTo<TViewModel>();
    }
    
    /// <inheritdoc />
    public bool IsCurrentlyOn<TViewModel>()
    {
        return navigationControl.IsCurrentlyOn<TViewModel>();
    }

    public bool IsCurrentlyOnAnyView()
    {
        return navigationControl.IsCurrentlyOnAnyView();
    }
    
    public bool CanNavigationBack()
    {
        return navigationControl.CanNavigateBack();
    }

    public void NavigateBack()
    {
        navigationControl.NavigateBack();
    }
}