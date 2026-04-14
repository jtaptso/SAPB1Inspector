namespace FormInspector.Domain.Common;

/// <summary>
/// Base class for domain entities providing common identity.
/// </summary>
public abstract class EntityBase
{
    /// <summary>Unique identifier for the entity.</summary>
    public string Id { get; protected set; } = Guid.NewGuid().ToString();
}
