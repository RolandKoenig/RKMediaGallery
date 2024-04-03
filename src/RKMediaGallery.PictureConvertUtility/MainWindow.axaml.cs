using RKMediaGallery.PictureConvertUtility.ViewService;
using RolandK.AvaloniaExtensions.Mvvm.Controls;

namespace RKMediaGallery.PictureConvertUtility;

public partial class MainWindow : MvvmWindow
{
    public MainWindow()
    {
        InitializeComponent();
        
        this.ViewServices.Add(new ProgressViewService(
            this.CtrlMainContent,
            this.CtrlProgressText,
            this.CtrlProgressBar));
    }
}