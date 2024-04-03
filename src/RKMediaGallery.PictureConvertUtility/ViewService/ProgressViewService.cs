using Avalonia.Controls;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace RKMediaGallery.PictureConvertUtility.ViewService;

public class ProgressViewService(
    Control mainContent,
    TextBlock progressText,
    ProgressBar progressBar)
    : ViewServiceBase, IProgressViewService
{
    public void NotifyActionStarted(string displayText)
    {
        mainContent.IsEnabled = false;
        
        progressText.Text = displayText;
        progressBar.Value = 0;
        
        progressText.IsVisible = true;
        progressBar.IsVisible = true;
    }

    public void NotifyActionProgress(int percent)
    {
        progressBar.Value = percent;
    }

    public void NotifyActionFinished()
    {
        mainContent.IsEnabled = true;

        progressText.IsVisible = false;
        progressBar.IsVisible = false;
    }
}