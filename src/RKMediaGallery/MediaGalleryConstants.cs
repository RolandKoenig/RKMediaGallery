namespace RKMediaGallery;

public static class MediaGalleryConstants
{
    public const int THUMBNAIL_REFERENCE_WIDTH = 700;
    public const int THUMBNAIL_REFERENCE_HEIGHT = 500;
    public const int THUMBNAIL_BUTTON_REFERENCE_MARGIN = 40;

    public const int THUMBNAIL_BUTTON_TEXTBOX_HEIGHT = 40;
    public const int THUMBNAIL_BUTTON_FONT_SIZE = 26;

    public const int SCREEN_REFERENCE_HEIGHT = 1500;
    
    public const double HEIGHT_MARGIN = 200;

    public static readonly string[] SUPPORTED_IMAGE_FORMATS = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".dat", ".JPG", ".JPEG", ".PNG", ".BMP" };
    // public static readonly string[] SUPPORTED_VIDEO_FORMATS = new string[] { ".mpg", ".mpeg", ".avi", ".wmv", ".mp4" };
    public const string BROWSING_SEARCH_PATTERN_THUMBNAIL = "Thumbnail*.*";
}