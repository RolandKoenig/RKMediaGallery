using RKMediaGallery.Controls;
using RolandK.AvaloniaExtensions.Mvvm.Controls;

namespace RKMediaGallery;

public partial class MainWindow : MvvmWindow
{
    public MainWindow()
    {
        InitializeComponent();
        
        this.ViewServices.Add(new NavigationViewService(this.CtrlNavigation));
    }
}