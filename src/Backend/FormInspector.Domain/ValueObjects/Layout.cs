namespace FormInspector.Domain.ValueObjects;

/// <summary>
/// Represents the position and size of an SAP UI item on a form.
/// </summary>
public record Layout
{
    /// <summary>Top position in pixels.</summary>
    public int Top { get; init; }

    /// <summary>Left position in pixels.</summary>
    public int Left { get; init; }

    /// <summary>Width in pixels.</summary>
    public int Width { get; init; }

    /// <summary>Height in pixels.</summary>
    public int Height { get; init; }

    public Layout(int top, int left, int width, int height)
    {
        if (top < 0) throw new ArgumentOutOfRangeException(nameof(top), "Top cannot be negative.");
        if (left < 0) throw new ArgumentOutOfRangeException(nameof(left), "Left cannot be negative.");
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width), "Width cannot be negative.");
        if (height < 0) throw new ArgumentOutOfRangeException(nameof(height), "Height cannot be negative.");

        Top = top;
        Left = left;
        Width = width;
        Height = height;
    }
}
