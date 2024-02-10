using RolandK.InProcessMessaging;

namespace RKMediaGallery.Messages;

[InProcessMessage]
public record MainWindowSizeChangedMessage(double Width, double Height);