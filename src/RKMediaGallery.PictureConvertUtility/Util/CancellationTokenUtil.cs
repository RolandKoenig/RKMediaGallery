using System;
using System.Threading;

namespace RKMediaGallery.PictureConvertUtility.Util;

public static class CancellationTokenUtil
{
    public static CancellationToken CancelAfterViewDisconnected(OwnViewModelBase viewModel)
    {
        var tokenSource = new CancellationTokenSource();

        EventHandler<EventArgs>? eventHandler = null;
        eventHandler = (_, _) =>
        {
            tokenSource.Cancel();
            viewModel.ViewDisconnected -= eventHandler;
        };

        viewModel.ViewDisconnected += eventHandler;
        
        return tokenSource.Token;
    }
}