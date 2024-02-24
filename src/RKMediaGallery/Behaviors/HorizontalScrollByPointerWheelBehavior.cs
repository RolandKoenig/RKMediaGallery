using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace RKMediaGallery.Behaviors;

// ReSharper disable once ClassNeverInstantiated.Global
public class HorizontalScrollByPointerWheelBehavior : AvaloniaObject
{
    public static readonly AttachedProperty<bool> IsHorizontalScrollByPointerWheelEnabledProperty =
        AvaloniaProperty.RegisterAttached<HorizontalScrollByPointerWheelBehavior, ScrollViewer, bool>(
            "IsHorizontalScrollByPointerWheelEnabled");

    public static void SetIsHorizontalScrollByPointerWheelEnabled(ScrollViewer element, bool value)
    {
        var prefValue = element.GetValue(IsHorizontalScrollByPointerWheelEnabledProperty);

        if (prefValue)
        {
            element.PointerWheelChanged -= OnTargetControl_PointerWheelChanged;
        }
        
        if (value)
        {
            element.PointerWheelChanged += OnTargetControl_PointerWheelChanged;
        }
    }

    public static bool GetIsHorizontalScrollByPointerWheelEnabled(ScrollViewer element)
    {
        return element.GetValue(IsHorizontalScrollByPointerWheelEnabledProperty);
    }

    private static void OnTargetControl_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (sender is not ScrollViewer scrollViewer)
        {
            return;
        }
        
        scrollViewer.SetCurrentValue(
            ScrollViewer.OffsetProperty, 
            scrollViewer.Offset - new Vector(500 * e.Delta.Y, 0));
    }
}