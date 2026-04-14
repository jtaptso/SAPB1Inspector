namespace FormInspector.Domain.ValueObjects;

/// <summary>
/// Represents the type identifier of an SAP Business One form (e.g., "139" for Sales Order).
/// </summary>
public record FormType
{
    public string Value { get; }

    public FormType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("FormType value cannot be null or empty.", nameof(value));
        Value = value;
    }

    public static implicit operator string(FormType formType) => formType.Value;
    public static explicit operator FormType(string value) => new(value);

    public override string ToString() => Value;
}
