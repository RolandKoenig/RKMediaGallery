using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;

// ReSharper disable HeapView.BoxingAllocation

namespace RKMediaGallery.Views.Navigation;

public class ThumbnailSwitchTransition : IPageTransition
{
    /// <summary>
    /// Gets the duration of the animation.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Gets or sets element entrance easing.
    /// </summary>
    public Easing? FadeInEasing { get; set; }

    /// <summary>
    /// Gets or sets element exit easing.
    /// </summary>
    public Easing? FadeOutEasing { get; set; }

    public double TransitionDistance { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CrossFade"/> class.
    /// </summary>
    public ThumbnailSwitchTransition()
    {
    }

    private void BuildAnimations(
        Control fromControl,
        out Animation fadeOutAnimation, 
        out Animation fadeInOpacityAnimation,
        out Animation fadeInTranslationAnimation)
    {
        var transitionDistance = this.TransitionDistance;
        if ((transitionDistance <= 0.0001) &&
            (!double.IsNaN(fromControl.Bounds.Height)))
        {
            transitionDistance = fromControl.Bounds.Height;
        }

        fadeOutAnimation = new Animation
        {
            Duration = this.Duration,
            Children =
            {
                new KeyFrame()
                {
                    Cue = new Cue(0d),
                    Setters =
                    {
                        new Setter
                        {
                            Property = Visual.OpacityProperty,
                            Value = 1d
                        },
                    }
                },
                new KeyFrame()
                {
                    Cue = new Cue(1d),
                    Setters =
                    {
                        new Setter
                        {
                            Property = Visual.OpacityProperty,
                            Value = 0d
                        },
                    }
                }

            }
        };
        
        fadeInOpacityAnimation = new Animation
        {
            Duration = this.Duration,
            Children =
            {
                new KeyFrame()
                {
                    Cue = new Cue(0d),
                    Setters =
                    {
                        new Setter()
                        {
                            Property = TranslateTransform.YProperty,
                            Value = transitionDistance
                        }
                    }
                },
                new KeyFrame()
                {
                    Cue = new Cue(1d),
                    Setters =
                    {
                        new Setter()
                        {
                            Property = TranslateTransform.YProperty,
                            Value = 0d
                        }
                    }
                }
            }
        };
        
        fadeInTranslationAnimation = new Animation
        {
            Duration = this.Duration,
            Children =
            {
                new KeyFrame()
                {
                    Cue = new Cue(0d),
                    Setters =
                    {
                        new Setter
                        {
                            Property = Visual.OpacityProperty,
                            Value = 0d
                        }
                    }
                },
                new KeyFrame()
                {
                    Cue = new Cue(1d),
                    Setters =
                    {
                        new Setter
                        {
                            Property = Visual.OpacityProperty,
                            Value = 1d
                        }
                    }
                }
            }
        };

        if (this.FadeOutEasing != null)
        {
            fadeOutAnimation.Easing = this.FadeOutEasing;
        }

        if (this.FadeInEasing != null)
        {
            fadeInOpacityAnimation.Easing = this.FadeInEasing;
            fadeInTranslationAnimation.Easing = this.FadeInEasing;
        }
    }

    public async Task Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) { return; }
        if (from is not Control fromControl) { return; }
        if (to is not Control toControl) { return; }
        if (!forward) { return; }
        
        this.BuildAnimations(
            fromControl,
            out var fadeOutAnimation,
            out var fadeInOpacityAnimation,
            out var fadeInTranslationAnimation);

        toControl.IsVisible = true;

        await Task.WhenAll(
            fadeOutAnimation.RunAsync(fromControl, cancellationToken),
            fadeInOpacityAnimation.RunAsync(toControl, cancellationToken),
            fadeInTranslationAnimation.RunAsync(toControl, cancellationToken));

        if (!cancellationToken.IsCancellationRequested)
        {
            from.IsVisible = false;
        }
    }
}