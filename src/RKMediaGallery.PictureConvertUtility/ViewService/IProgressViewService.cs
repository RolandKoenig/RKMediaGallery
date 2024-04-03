namespace RKMediaGallery.PictureConvertUtility.ViewService;

public interface IProgressViewService
{
    void NotifyActionStarted(string displayText);

    void NotifyActionProgress(int percent);

    void NotifyActionFinished();
}