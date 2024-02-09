using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using RKMediaGallery.Controls;
using RKMediaGallery.Messages;
using RKMediaGallery.Views;
using RKMediaGallery.ViewServices;
using RolandK.AvaloniaExtensions.DependencyInjection;
using RolandK.AvaloniaExtensions.Mvvm.Controls;
using RolandK.InProcessMessaging;

namespace RKMediaGallery;

public partial class MainWindow : MvvmWindow
{
    public MainWindow()
    {
        InitializeComponent();
        
        this.ViewServices.Add(new NavigationViewService(this.CtrlNavigation));
        this.ViewServices.Add(new MainWindowHeightProviderViewService(this));
        this.ViewServices.Add(new ServiceProviderViewService(this));
        
        this.CtrlNavigation.NavigateTo<HomeViewModel>();
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        var serviceProvider = this.GetServiceProvider();
        var srvMessagePublisher = serviceProvider.GetRequiredService<IInProcessMessagePublisher>();
        
        srvMessagePublisher.Publish(new MainWindowSizeChanged(
            e.NewSize.Width, e.NewSize.Height));
    }
}