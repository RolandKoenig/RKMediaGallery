using Avalonia.Media;
using RKMediaGallery.Controls;
using RKMediaGallery.Views;
using RolandK.AvaloniaExtensions.Mvvm.Controls;

namespace RKMediaGallery;

public partial class MainWindow : MvvmWindow
{
    public MainWindow()
    {
        InitializeComponent();
        
        this.ViewServices.Add(new NavigationViewService(this.CtrlNavigation));
        this.CtrlNavigation.NavigateTo<HomeViewModel>();
    }
}