namespace FormInspector.Domain.Snapshots;

/// <summary>
/// Represents metadata for a matrix or grid on an SAP Business One form.
/// </summary>
public record MatrixMetadata
{
    /// <summary>Unique identifier of the matrix within the form.</summary>
    public string MatrixUid { get; init; }

    /// <summary>Number of rows currently visible in the matrix.</summary>
    public int RowCount { get; init; }

    /// <summary>Whether the matrix is editable.</summary>
    public bool Editable { get; init; }

    /// <summary>Columns within this matrix.</summary>
    public IReadOnlyList<ColumnMetadata> Columns { get; init; }

    public MatrixMetadata(
        string matrixUid,
        int rowCount,
        bool editable = true,
        IReadOnlyList<ColumnMetadata>? columns = null)
    {
        if (string.IsNullOrWhiteSpace(matrixUid))
            throw new ArgumentException("MatrixUid cannot be null or empty.", nameof(matrixUid));
        if (rowCount < 0)
            throw new ArgumentOutOfRangeException(nameof(rowCount), "RowCount cannot be negative.");

        MatrixUid = matrixUid;
        RowCount = rowCount;
        Editable = editable;
        Columns = columns ?? [];
    }
}
