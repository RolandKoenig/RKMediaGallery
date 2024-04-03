using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using ImageMagick;
using RKMediaGallery.PictureConvertUtility.Util;
using RolandK.AvaloniaExtensions.ViewServices;
using Path = System.IO.Path;

namespace RKMediaGallery.PictureConvertUtility;

public partial class MainWindowViewModel : OwnViewModelBase
{
    public static readonly MainWindowViewModel DesignViewModel = new MainWindowViewModel();

    [RelayCommand]
    private void Close()
    {
        this.CloseHostWindow();
    }

    [RelayCommand]
    private async Task UpdateFileNamesAsync()
    {
        var srvDirectoryDialog = this.GetViewService<IOpenDirectoryViewService>();
        var directory = await srvDirectoryDialog.ShowOpenDirectoryDialogAsync("Select directory");
        if (string.IsNullOrEmpty(directory)) { return; }

        foreach (var actFile in LoopOverFilesRecursively(directory))
        {
            var actDirectory = Path.GetDirectoryName(actFile);
            if(string.IsNullOrEmpty(actDirectory)){ continue; }
            
            var actFileCreationTime = File.GetCreationTime(actFile);
            var actFileDate = DateOnly.FromDateTime(actFileCreationTime);
            
            var actFileStringStr = actFileCreationTime.ToString("yyyy-MM-dd_HH-mm-ss");
            var actExt = Path.GetExtension(actFile);
            
            File.Move(
                actFile,
                Path.Combine(
                    actDirectory,
                    $"{actFileStringStr}{actExt}"));
        }
    }

    [RelayCommand]
    private async Task UpdatePictureDimensionsAsync()
    {
        var srvDirectoryDialog = this.GetViewService<IOpenDirectoryViewService>();
        var directory = await srvDirectoryDialog.ShowOpenDirectoryDialogAsync("Select directory");
        if (string.IsNullOrEmpty(directory)) { return; }

        foreach (var actFilePath in LoopOverFilesRecursively(directory))
        {
            var actFileExtension = Path.GetExtension(actFilePath);
            if (!MediaGalleryConstants.SUPPORTED_IMAGE_FORMATS.Contains(actFileExtension, StringComparer.OrdinalIgnoreCase)){ continue; }

            var desiredLongestDimension = 3427.0;
            if (Path.GetFileName(actFilePath)
                .StartsWith(MediaGalleryConstants.THUMBNAIL_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                desiredLongestDimension = 1280.0;
            }
            
            using var image = new MagickImage(actFilePath);

            var longestDimension = image.Width > image.Height ? image.Width : image.Height;
            var resizePercentage = desiredLongestDimension / longestDimension;
            image.Resize(
                (int)(image.Width * resizePercentage),
                (int)(image.Height * resizePercentage));
            
            await image.WriteAsync(actFilePath);
        }
    }

    private IEnumerable<string> LoopOverFilesRecursively(string directory)
    {
        foreach (var actFile in Directory.GetFiles(directory))
        {
            yield return actFile;
        }
        
        foreach (var actSubDirectory in Directory.GetDirectories(directory))
        {
            foreach (var actSubPath in LoopOverFilesRecursively(actSubDirectory))
            {
                yield return actSubPath;
            }
        }
    }
}