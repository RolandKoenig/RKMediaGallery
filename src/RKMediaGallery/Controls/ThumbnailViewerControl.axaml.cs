using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace RKMediaGallery.Controls;

public partial class ThumbnailViewerControl : UserControl
{
    public static readonly DirectProperty<ThumbnailViewerControl, string[]> ThumbnailsProperty =
        AvaloniaProperty.RegisterDirect<ThumbnailViewerControl, string[]>(
            nameof(Thumbnails),
            o => o.Thumbnails,
            (o, v) => o.Thumbnails = v,
            defaultBindingMode: BindingMode.OneWay);

    public static readonly DirectProperty<ThumbnailViewerControl, string?> TitleTextProperty =
        AvaloniaProperty.RegisterDirect<ThumbnailViewerControl, string?>(
            nameof(TitleText),
            o => o.TitleText,
            (o, v) => o.TitleText = v,
            defaultBindingMode: BindingMode.OneTime);

    private readonly Random _random = new Random(Environment.TickCount);
    private string[] _thumbnails = Array.Empty<string>();
    private DispatcherTimer? _refreshTimer;
    private string? _currentThumbnail;

    public string[] Thumbnails
    {
        get => _thumbnails;
        set
        {
            var givenValue = value ?? Array.Empty<string>();
            this.SetAndRaise(ThumbnailsProperty, ref _thumbnails, givenValue);
            
            UpdateCurrentImage();
        }
    }

    public string? TitleText
    {
        get => this.CtrlTitleText.Text;
        set => this.CtrlTitleText.Text = value;
    }
    
    public ThumbnailViewerControl()
    {
        InitializeComponent();
    }

    private async void UpdateCurrentImage()
    {
        if (_thumbnails.Length <= 0) { return; }

        var nextThumbnail = _thumbnails[_random.Next(0, _thumbnails.Length)];
        if (_currentThumbnail == nextThumbnail)
        {
            return;
        }
        _currentThumbnail = nextThumbnail;
        
        var nextThumbnailBitmap = await Task.Run(() => new Bitmap(nextThumbnail));

        var newImageControl = new Image();
        newImageControl.Source = nextThumbnailBitmap;
        newImageControl.Width = 504;
        newImageControl.Height = 360;
        newImageControl.Margin = new Thickness(20);
        newImageControl.Stretch = Stretch.UniformToFill;
        this.CtrlTransition.Content = newImageControl;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (_refreshTimer != null)
        {
            return;
        }

        this.UpdateCurrentImage();

        DispatcherTimer refreshTimer = null;
        refreshTimer = new DispatcherTimer(
            TimeSpan.FromSeconds(_random.Next(3, 6)),
            DispatcherPriority.Default,
            (_, _) =>
            {
                refreshTimer!.Interval = TimeSpan.FromSeconds(_random.Next(3, 6));
                UpdateCurrentImage();
            });
        _refreshTimer = refreshTimer;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        if (_refreshTimer == null)
        {
            return;
        }
        
        _refreshTimer.Stop();
        _refreshTimer = null;
    }
}