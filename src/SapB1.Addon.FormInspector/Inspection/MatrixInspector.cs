#if SAP_UI_SDK
using SAPbouiCOM;
#endif
using System;
using System.Collections.Generic;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;
using SapB1.Addon.FormInspector.Utilities;

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
    public MatrixDto InspectMatrix(string formUid, string matrixUid)
    {
#if SAP_UI_SDK
        var form = SapContext.TryGetForm(formUid);
        if (form != null)
        {
            try
            {
                var item = form.Items.Item(matrixUid);
                var specific = item.Specific;

                if (specific is SAPbouiCOM.Matrix matrix)
                {
                    var dto = new MatrixDto
                    {
                        MatrixUid = matrixUid,
                        RowCount = matrix.RowCount,
                        Editable = matrix.Editable,
                        Columns = ExtractColumns(matrix)
                    };

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(matrix);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(item);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(form);
                    return dto;
                }

                if (specific is SAPbouiCOM.Grid grid)
                {
                    var dto = new MatrixDto
                    {
                        MatrixUid = matrixUid,
                        RowCount = grid.Rows?.Count ?? 0,
                        Editable = true, // Grid doesn't have a direct Editable property
                        Columns = ExtractGridColumns(grid)
                    };

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(grid);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(item);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(form);
                    return dto;
                }

                // Item is not a matrix or grid — release specific and item, fall through
                if (specific != null)
                    try { System.Runtime.InteropServices.Marshal.ReleaseComObject(specific); } catch { }
                try { System.Runtime.InteropServices.Marshal.ReleaseComObject(item); } catch { }
            }
            catch (Exception)
            {
                // Matrix access failed — fall through to default
            }
            finally
            {
                try { System.Runtime.InteropServices.Marshal.ReleaseComObject(form); } catch { }
            }
        }
#endif
        return new MatrixDto
        {
            MatrixUid = matrixUid,
            RowCount = 0,
            Editable = true,
            Columns = new List<ColumnDto>()
        };
    }

    /// <summary>
    /// Inspects all matrix/grid controls on a given form.
    /// </summary>
    public List<MatrixDto> InspectAllMatrices(string formUid)
    {
#if SAP_UI_SDK
        var form = SapContext.TryGetForm(formUid);
        if (form != null)
        {
            var matrices = new List<MatrixDto>();
            try
            {
                for (int i = 0; i < form.Items.Count; i++)
                {
                    try
                    {
                        var item = form.Items.Item(i);
                        if (item.Type == BoFormItemTypes.it_MATRIX ||
                            item.Type == BoFormItemTypes.it_GRID)
                        {
                            var matrixDto = InspectMatrix(formUid, item.ItemUID);
                            matrices.Add(matrixDto);
                        }
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(item);
                    }
                    catch (Exception)
                    {
                        // Individual item failure — continue enumeration
                    }
                }
            }
            catch (Exception)
            {
                // Form.Items enumeration failed — return what we have
            }
            finally
            {
                try { System.Runtime.InteropServices.Marshal.ReleaseComObject(form); } catch { }
            }
            return matrices;
        }
#endif
        return new List<MatrixDto>();
    }

#if SAP_UI_SDK
    /// <summary>
    /// Extracts column metadata from a SAPbouiCOM.Matrix control.
    /// </summary>
    private static List<ColumnDto> ExtractColumns(SAPbouiCOM.Matrix matrix)
    {
        var columns = new List<ColumnDto>();

        try
        {
            for (int i = 0; i < matrix.Columns.Count; i++)
            {
                try
                {
                    var col = matrix.Columns.Item(i);
                    var colDto = new ColumnDto
                    {
                        ColumnUid = col.UniqueID ?? string.Empty,
                        ColumnType = col.Type.ToString(),
                        Editable = col.Editable,
                        Width = col.Width,
                        Caption = TryGetColumnCaption(col),
                        DataBinding = TryGetColumnDataBinding(col)
                    };
                    columns.Add(colDto);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(col);
                }
                catch (Exception)
                {
                    // Individual column failure — continue
                }
            }
        }
        catch (Exception)
        {
            // Columns enumeration failed — return what we have
        }

        return columns;
    }

    /// <summary>
    /// Extracts column metadata from a SAPbouiCOM.Grid control.
    /// </summary>
    private static List<ColumnDto> ExtractGridColumns(SAPbouiCOM.Grid grid)
    {
        var columns = new List<ColumnDto>();

        try
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                try
                {
                    var col = grid.Columns.Item(i);
                    var colDto = new ColumnDto
                    {
                        ColumnUid = col.UniqueID ?? string.Empty,
                        ColumnType = col.Type.ToString(),
                        Editable = col.Editable,
                        Width = col.Width,
                        Caption = col.TitleObject?.Caption,
                        DataBinding = null // Grid columns don't use DataBind the same way
                    };
                    columns.Add(colDto);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(col);
                }
                catch (Exception)
                {
                    // Individual column failure — continue
                }
            }
        }
        catch (Exception)
        {
            // Columns enumeration failed — return what we have
        }

        return columns;
    }

    /// <summary>
    /// Safely attempts to get the caption of a matrix column.
    /// </summary>
    private static string? TryGetColumnCaption(SAPbouiCOM.Column col)
    {
        try
        {
            return col.TitleObject?.Caption;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Safely attempts to get the data binding of a matrix column.
    /// </summary>
    private static DataBindingDto? TryGetColumnDataBinding(SAPbouiCOM.Column col)
    {
        try
        {
            if (col.DataBind != null)
            {
                var binding = new DataBindingDto
                {
                    TableName = col.DataBind.TableName ?? string.Empty,
                    ColumnName = col.DataBind.Alias ?? string.Empty
                };
                if (!string.IsNullOrEmpty(binding.TableName) || !string.IsNullOrEmpty(binding.ColumnName))
                    return binding;
            }
        }
        catch
        {
            // DataBind may not be available for this column type
        }
        return null;
    }
#endif
}
