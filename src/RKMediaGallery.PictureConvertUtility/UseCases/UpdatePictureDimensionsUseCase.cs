using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageMagick;
using RKMediaGallery.PictureConvertUtility.Util;
using RolandK.AvaloniaExtensions.ViewServices;

namespace RKMediaGallery.PictureConvertUtility.UseCases;

public class UpdatePictureDimensionsUseCase
{
    public async Task UpdatePictureDimensionsAsync(IOpenDirectoryViewService srvDirectoryDialog)
    {
        var directory = await srvDirectoryDialog.ShowOpenDirectoryDialogAsync("Select directory");
        if (string.IsNullOrEmpty(directory)) { return; }

        foreach (var actFilePath in FileUtility.LoopOverFilesRecursively(directory))
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
}