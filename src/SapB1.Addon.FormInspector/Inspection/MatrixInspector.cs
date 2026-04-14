using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

namespace SapB1.Addon.FormInspector.Inspection;

/// <summary>
/// Inspects matrix and grid controls on SAP Business One forms.
/// Extracts column metadata: UID, type, data bindings, editable flags.
/// Read-only — does not modify the SAP UI.
/// </summary>
public class MatrixInspector
{
    /// <summary>
    /// Inspects a matrix control by its UID on a given form.
    /// </summary>
    public MatrixDto InspectMatrix(int formId, string matrixUid)
    {
        // TODO: Use SAPbouiCOM.Matrix to extract column metadata
        // var matrix = (SAPbouiCOM.Matrix)form.Items.Item(matrixUid).Specific;
        // for (int i = 0; i < matrix.Columns.Count; i++) { ... }

        return new MatrixDto
        {
            MatrixUid = matrixUid,
            RowCount = 0,
            Editable = true,
            Columns = []
        };
    }

    /// <summary>
    /// Inspects all matrix/grid controls on a given form.
    /// </summary>
    public List<MatrixDto> InspectAllMatrices(int formId)
    {
        // TODO: Find all matrix-type items and inspect each
        return [];
    }
}
