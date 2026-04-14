namespace FormInspector.Domain.ValueObjects;

/// <summary>
/// Represents the data binding of an SAP UI item to a database table and column.
/// </summary>
public record DataBinding
{
    /// <summary>The database table name the item is bound to.</summary>
    public string TableName { get; init; }

    /// <summary>The database column name the item is bound to.</summary>
    public string ColumnName { get; init; }

    public DataBinding(string tableName, string columnName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException("TableName cannot be null or empty.", nameof(tableName));
        if (string.IsNullOrWhiteSpace(columnName))
            throw new ArgumentException("ColumnName cannot be null or empty.", nameof(columnName));

        TableName = tableName;
        ColumnName = columnName;
    }

    public override string ToString() => $"{TableName}.{ColumnName}";
}
