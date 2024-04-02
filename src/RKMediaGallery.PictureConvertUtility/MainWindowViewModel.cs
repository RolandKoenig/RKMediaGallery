using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
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
            
            var actFileStringStr = actFileCreationTime.ToString("yyyy-MM-dd_hh-mm-ss");
            var actExt = Path.GetExtension(actFile);
            
            File.Move(
                actFile,
                Path.Combine(
                    actDirectory,
                    $"{actFileStringStr}{actExt}"));
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