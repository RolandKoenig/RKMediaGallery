using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using RKMediaGallery.PictureConvertUtility.Util;
using RKMediaGallery.PictureConvertUtility.ViewService;
using RolandK.AvaloniaExtensions.ViewServices;

namespace RKMediaGallery.PictureConvertUtility.UseCases;

public class UpdatePictureDimensionsUseCase
{
    public async Task UpdatePictureDimensionsAsync(
        IOpenDirectoryViewService srvDirectoryDialog,
        IProgressViewService srvProgress,
        CancellationToken cancellationToken)
    {
        var directory = await srvDirectoryDialog.ShowOpenDirectoryDialogAsync("Select directory");
        if (string.IsNullOrEmpty(directory)) { return; }

        srvProgress.NotifyActionStarted("Updating dimensions...");
        try
        {
            var allFiles = FileUtility.LoopOverFilesRecursively(directory)
                .Where(x => MediaGalleryConstants.SUPPORTED_IMAGE_FORMATS_FOR_RESIZE.Contains(
                    Path.GetExtension(x),
                    StringComparer.OrdinalIgnoreCase))
                .ToList();
            for(var loop=0; loop<allFiles.Count; loop++)
            {
                if (cancellationToken.IsCancellationRequested) { break; }
                srvProgress.NotifyActionProgress((int)(((double)loop / (double)allFiles.Count) * 100));
                
                var actFilePath = allFiles[loop];
                
                var actFileExtension = Path.GetExtension(actFilePath);
                if (!MediaGalleryConstants.SUPPORTED_IMAGE_FORMATS_FOR_RESIZE.Contains(actFileExtension,
                        StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                var desiredLongestDimension = 3427.0;
                if (Path.GetFileName(actFilePath)
                    .StartsWith(MediaGalleryConstants.THUMBNAIL_PREFIX, StringComparison.OrdinalIgnoreCase))
                {
                    desiredLongestDimension = 1280.0;
                }
                
                using var image = await Task.Run(() => new MagickImage(actFilePath), cancellationToken);

                var longestDimension = image.Width > image.Height ? image.Width : image.Height;
                var resizePercentage = desiredLongestDimension / longestDimension;
                if(Math.Abs(resizePercentage - 1.0) < 0.001){ continue; }

                await Task.Run(() =>
                {
                    image.Resize(
                        (uint)(image.Width * resizePercentage),
                        (uint)(image.Height * resizePercentage));
                }, cancellationToken);

                await image.WriteAsync(actFilePath, cancellationToken);
            }
        }
        finally
        {
            srvProgress.NotifyActionFinished();   
        }
    }
}