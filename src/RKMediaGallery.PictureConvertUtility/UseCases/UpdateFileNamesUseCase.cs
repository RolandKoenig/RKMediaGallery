using System;
using System.IO;
using System.Threading.Tasks;
using RKMediaGallery.PictureConvertUtility.Util;
using RolandK.AvaloniaExtensions.ViewServices;

namespace RKMediaGallery.PictureConvertUtility.UseCases;

public class UpdateFileNamesUseCase
{
    public async Task UpdateFileNamesAsync(IOpenDirectoryViewService srvDirectoryDialog)
    {
        var directory = await srvDirectoryDialog.ShowOpenDirectoryDialogAsync("Select directory");
        if (string.IsNullOrEmpty(directory)) { return; }

        foreach (var actFile in FileUtility.LoopOverFilesRecursively(directory))
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
}