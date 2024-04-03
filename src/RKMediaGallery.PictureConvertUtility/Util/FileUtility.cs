using System.Collections.Generic;
using System.IO;

namespace RKMediaGallery.PictureConvertUtility.Util;

public static class FileUtility
{
    public static IEnumerable<string> LoopOverFilesRecursively(string directory)
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