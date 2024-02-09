using RolandK.InProcessMessaging;

namespace RKMediaGallery.Messages;

[InProcessMessage]
public record MainWindowSizeChanged(double Width, double Height);