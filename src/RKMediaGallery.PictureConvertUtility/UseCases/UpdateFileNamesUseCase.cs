using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RKMediaGallery.PictureConvertUtility.Util;
using RKMediaGallery.PictureConvertUtility.ViewService;
using RolandK.AvaloniaExtensions.ViewServices;

namespace RKMediaGallery.PictureConvertUtility.UseCases;

public class UpdateFileNamesUseCase
{
    public async Task UpdateFileNamesAsync(
        IOpenDirectoryViewService srvDirectoryDialog,
        IProgressViewService srvProgress,
        CancellationToken cancellationToken)
    {
        srvProgress.NotifyActionStarted("Updating file names...");
        try
        {
            var directory = await srvDirectoryDialog.ShowOpenDirectoryDialogAsync("Select directory");
            if (string.IsNullOrEmpty(directory))
            {
                return;
            }

            var allFiles = FileUtility.LoopOverFilesRecursively(directory)
                .Where(x => MediaGalleryConstants.SUPPORTED_IMAGE_FORMATS.Contains(
                    Path.GetExtension(x),
                    StringComparer.OrdinalIgnoreCase))
                .ToList();
            for (var loop = 0; loop < allFiles.Count; loop++)
            {
                if (cancellationToken.IsCancellationRequested) { break; }
                
                var actFilePath = allFiles[loop];

                var actDirectory = Path.GetDirectoryName(actFilePath);
                if (string.IsNullOrEmpty(actDirectory))
                {
                    continue;
                }

                var actFileCreationTime = File.GetCreationTime(actFilePath);

                var actFileStringStr = actFileCreationTime.ToString("yyyy-MM-dd_HH-mm-ss");
                var actExt = Path.GetExtension(actFilePath);
                
                await Task.Run(() =>
                {
                    File.Move(
                        actFilePath,
                        Path.Combine(
                            actDirectory,
                            $"{actFileStringStr}{actExt}"));
                });

                srvProgress.NotifyActionProgress((int)(((double)loop / (double)allFiles.Count) * 100));
            }
        }
        finally
        {
            srvProgress.NotifyActionFinished();
        }
    }
}