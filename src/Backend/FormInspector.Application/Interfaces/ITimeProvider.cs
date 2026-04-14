namespace FormInspector.Application.Interfaces;

/// <summary>
/// Time provider interface (port) for testable time access.
/// Implemented by Infrastructure layer; enables deterministic testing.
/// </summary>
public interface ITimeProvider
{
    /// <summary>Gets the current UTC time.</summary>
    DateTime UtcNow { get; }
}
