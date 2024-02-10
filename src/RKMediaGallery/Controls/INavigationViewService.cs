namespace RKMediaGallery.Controls;

public interface INavigationViewService
{
    string CurrentViewTitle { get; }
    
    int NavigationStackSize { get; }

    /// <summary>
    /// Navigates to the given ViewModel.
    /// </summary>
    void NavigateTo(INavigationTarget targetViewModel);
    
    /// <summary>
    /// Navigates to the given viewmodel. 
    /// </summary>
    void NavigateTo<TViewModel>()
        where TViewModel : INavigationTarget;

    /// <summary>
    /// Navigates to the given viewmodel and passes the given argument.
    /// </summary>
    void NavigateTo<TViewModel, TNavigationArgument>(TNavigationArgument argument)
        where TViewModel : INavigationTarget, INavigationDataReceiver<TNavigationArgument>;

    bool IsCurrentlyOn<TViewModel>();

    bool IsCurrentlyOnAnyView();

    void NavigateBack();

    bool CanNavigationBack();
}
