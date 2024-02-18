using System;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using RKMediaGallery.Controls;

namespace RKMediaGallery.Views;

public static class ViewController
{
    public static INavigationTarget GetViewModelVorDirectory(string directoryPath)
    {
        var viewTitle = Path.GetFileName(directoryPath);
        
        if (!Directory.Exists(directoryPath))
        {
            return new EmptyViewModel(viewTitle);
        }
        
        var subDirectories = Directory.GetDirectories(directoryPath);
        if (subDirectories.Length > 0)
        {
            return new NavigationViewModel(
                viewTitle,
                subDirectories);
        }

        var imageFiles = Directory.GetFiles(directoryPath)
            .Where(x =>
            {
                var actFileExtension = Path.GetExtension(x);

                if (FileSystemName.MatchesSimpleExpression(
                        MediaGalleryConstants.BROWSING_SEARCH_PATTERN_THUMBNAIL,
                        Path.GetFileName(x),
                        true))
                {
                    return false;
                }

                return MediaGalleryConstants.SUPPORTED_IMAGE_FORMATS.Contains(actFileExtension,
                    StringComparer.OrdinalIgnoreCase);
            })
            .Order()
            .ToArray();
        if (imageFiles.Length > 0)
        {
            return new ImageCollectionViewModel(
                directoryPath,
                imageFiles);
        }

        return new EmptyViewModel(viewTitle);
    }
}