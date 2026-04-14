using FormInspector.Application.Interfaces;

namespace FormInspector.Infrastructure.Time;

/// <summary>
/// System implementation of ITimeProvider.
/// Returns the real UTC time. For testing, use a mock implementation.
/// </summary>
public class SystemTimeProvider : ITimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
