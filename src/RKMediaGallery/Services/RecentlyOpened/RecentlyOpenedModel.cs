using System.Collections.Generic;

namespace RKMediaGallery.Services.RecentlyOpened;

internal class RecentlyOpenedModel
{
    public List<RecentlyOpenedFileModel> Files { get; set; } = new();
}