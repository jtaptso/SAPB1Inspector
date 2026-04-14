namespace FormInspector.Domain.ValueObjects;

/// <summary>
/// Represents the type of an SAP Business One UI item (e.g., "EditText", "Button", "Matrix", "CheckBox").
/// </summary>
public record ItemType
{
    public string Value { get; }

    public ItemType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ItemType value cannot be null or empty.", nameof(value));
        Value = value;
    }

    public static implicit operator string(ItemType itemType) => itemType.Value;
    public static explicit operator ItemType(string value) => new(value);

    public override string ToString() => Value;
}
