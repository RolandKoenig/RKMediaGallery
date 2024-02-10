using System.Collections.Generic;
using System.Threading.Tasks;

namespace RKMediaGallery.Services;

public interface IRecentlyOpenedFilesService
{
    Task AddOpenedFileAsync(string filePath);

    Task<string> TryGetLastOpenedFileAsync();

    Task<IReadOnlyList<string>> GetAllRecentlyOpenedFilesAsync();
}