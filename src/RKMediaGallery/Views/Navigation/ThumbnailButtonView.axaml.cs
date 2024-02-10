using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using RolandK.AvaloniaExtensions.Mvvm.Controls;

namespace RKMediaGallery.Views.Navigation;

public partial class ThumbnailButtonView : MvvmUserControl
{
    public static readonly DirectProperty<ThumbnailButtonView, string[]> ThumbnailsProperty =
        AvaloniaProperty.RegisterDirect<ThumbnailButtonView, string[]>(
            nameof(Thumbnails),
            o => o.Thumbnails,
            (o, v) => o.Thumbnails = v,
            defaultBindingMode: BindingMode.OneWay);

    public static readonly DirectProperty<ThumbnailButtonView, string?> TitleTextProperty =
        AvaloniaProperty.RegisterDirect<ThumbnailButtonView, string?>(
            nameof(TitleText),
            o => o.TitleText,
            (o, v) => o.TitleText = v,
            defaultBindingMode: BindingMode.OneTime);

    private Random? _random;
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
    
    public ThumbnailButtonView()
    {
        InitializeComponent();
    }

    private int GetNextRandomInt(int min, int max)
    {
        if (_random == null)
        {
            var seed = Environment.TickCount;
            if (_thumbnails.Length > 0)
            {
                seed = _thumbnails[0].GetHashCode();
            }
            _random = new Random(seed);
        }

        return _random.Next(min, max);
    }

    private async void UpdateCurrentImage()
    {
        if (_thumbnails.Length <= 0) { return; }

        var nextThumbnail = _thumbnails[GetNextRandomInt(0, _thumbnails.Length)];
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
            TimeSpan.FromSeconds(GetNextRandomInt(3, 6)),
            DispatcherPriority.Default,
            (_, _) =>
            {
                refreshTimer!.Interval = TimeSpan.FromSeconds(GetNextRandomInt(3, 6));
                refreshTimer!.Interval = TimeSpan.FromSeconds(GetNextRandomInt(3, 6));
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